import sys

from anyio.from_thread import start_blocking_portal
from pydantic import BaseModel
#from pytauri import (
#    Commands,
#    builder_factory,
#    context_factory,
#)

#commands: Commands = Commands()
from miraicl.core.secret import CoreVersion,CommitHash
from miraicl.core.runtimes.universal.information import sys_type,SystemType
from miraicl.core import logger
import sys

def update():
    pass

def startup():
    return_code = main()
    # 0 -> Program Exit
    # 1 -> Exception (Require feedback)
    # 2 -> Update
    if return_code == 1:
        logger.info("请在 https://github.com/MiraiCL/MiraiCL/issues 提交反馈以帮助我们解决此问题")
    elif return_code == 2:
        logger.info("准备安装更新")
        update()
    elif return_code == 0:
        logger.info("")
    logger.info("---- 程序日志结束 ----")
    return return_code

def shutdown(force_shutdown:bool = False,is_exception:bool = False):
    pass

def restart():
    pass


def main() -> int:
    logger.info(f"程序启动，核心版本：{CoreVersion}，构建号：{CommitHash}")
    logger.info(f"操作系统类型：{sys_type.name}，版本号：-")
    logger.info(f"程序位置：{sys.executable}")
    if sys_type == SystemType.Unknown:
        logger.warning("检测到可能不支持的操作系统，部分功能将不可用")
    logger.info("尝试加载配置文件系统")
    #with start_blocking_portal("asyncio") as portal:  # or `trio`
    #    app = builder_factory().build(
    #        context=context_factory(),
    #        invoke_handler=commands.generate_handler(portal),
    #    )
    #    exit_code = app.run_return()
    #    return exit_code
    return 1