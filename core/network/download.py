from request import client,sign_headers
from typing import Optional
from pathlib import Path
import aiofiles
import asyncio
import aiorwlock
import clr



lock = aiorwlock.RWLock()

tasks = {}

allow_max = asyncio.Semaphore(64)

class NetFile:
    def __init__(self,urls:list[str],hash:str,algo:str,path:Path,size:Optional[int] = None,headers:Optional[dict[str,str]] = None):
        self.urls = urls
        self.algo = algo
        self.hash = hash
        self.path = path
        self.speed = 0
        self.progress = -1
        self.size = size
        if not headers:
            self.headers = dict[str,str]()
        else:
            self.headers = headers
        self.errors = dict[str,Exception]()
    def begin_start(self):
        return False
    async def start(self):
        async with allow_max:
            async with lock.reader:
                if self.begin_start():
                    return
                for url in self.urls:
                    sign_headers(url,self.headers)
                    try:
                        downloaded = 0
                        async with client.stream("GET",url,headers=self.headers) as response:
                            response.raise_for_status()
                            async with aiofiles.open(self.path,"wb") as f:
                                async for b in response.aiter_bytes(16384):
                                    await f.write(b)
                                    if self.size:
                                        downloaded += 16384
                                        self.progress = f"{downloaded/self.size:.2f}"
                    except Exception as e:
                        self.errors[url] = e
                self.end_start(self)
    def end_start(self):
        if tasks.get(self.algo):
            del tasks[self.algo]
        else:
            del tasks[self.path]


def submit_task(tasks:list[NetFile]):
    with lock.writer:
        for f in tasks:
            if not tasks.get(f.hash if f.hash else f.path):
                tasks[f.hash if f.hash else f.path] = f

async def start_dispatcher():
    pass