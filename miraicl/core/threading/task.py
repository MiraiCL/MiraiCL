from enum import Enum
from .. import logger
from . import dispatcher
from threading import Event
from aiorwlock import RWLock
from concurrent.futures import Future
from typing import Optional,Any

import asyncio

class TaskIO[TInput,TOutput]:
    input:Optional[TInput]
    output:Optional[TOutput]

class TaskState(Enum):
    Pending = 0,
    Processing = 1
    Completed = 2
    Failed = 3
    Aborted = 4

class Task:
    "任务基类"
    use_cpu:bool = False
    delegate_method:callable[Task]
    progress:float
    state:TaskState = TaskState.Pending
    name:str = "未命名任务",
    cancel_event:Event = Event()
    thread:Optional[Future]
    io:Optional[TaskIO]
    exc:Optional[Exception]
    def start(self):
        "启动任务"
        if self.state == TaskState.Processing:
            return
        if not self.io:
            self.io = TaskIO[Any,Any]()
        if self.use_cpu:
            dispatcher.default.submit(self.delegate_method,self)
        else:
            dispatcher.io.submit(self.delegate_method,self)
        logger.info(f"{self.name} 任务状态改变：{self.state.name} -> {TaskState.Processing.name}")
        self.state = TaskState.Processing
    def abort(self):
        "取消当前任务（不保证线程完全停止）"
        self.cancel_event.set()
    def _report_cancelled(self):
        "由托管线程调用，指示当前任务已取消"
        logger.debug(f"{self.name} 任务状态改变: {self.state.name} -> {TaskState.Aborted.name}")
        self.state = TaskState.Aborted
    def _report_error(self):
        "由托管线程调用，指示执行过程中遇到错误"
        self.state = TaskState.Failed
    def _report_complete(self):
        "由托管线程调用，指示任务正常结束"
        logger.info(f"{self.name} 任务状态改变：{self.state.name} -> {TaskState.Completed.name}")
        self.state = TaskState.Completed
    def wait_for_complete(self):
        "等待任务完成，如果任务还没有启动，则启动任务"
        if self.state != TaskState.Processing:
            self.start()
        self.thread.result()
    async def await_for_complete(self):
        "异步等待任务完成，如果任务还没有启动，则启动任务"
        if self.state != TaskState.Processing:
            self.start()
        await asyncio.wrap_future(self.thread)

class MultipleTask[T](Task):
    "并行执行任务，同时只允许设置输入"
    input:Optional[T]
    _tasks = list[Task]()
    lock:RWLock = RWLock()
    def link(self,tasks:Task | list[Task]):
        "将一个或多个任务挂入托管执行"
        with self.lock.writer:
            if isinstance(tasks,Task):
                self._tasks.append(tasks)
            else:
                self._tasks.extend(tasks)
    def start(self):
        for t in self._tasks:
            if not t.io and self.input:
                t.io = TaskIO[T,Optional[Any]]()
                t.io.input = self.input
                t.io.output = None
            t.start()
    def wait_for_complete(self):
        for t in self._tasks:
            t.wait_for_complete()
    async def await_for_complete(self):
        ts = [t.await_for_complete() for t in self._tasks]
        await asyncio.gather(*ts)