from loguru import logger
import sys
from miraicl.core.secret import Channel,UpdateChannel,enable_debug_mode
from enum import Enum

logger.remove()

if Channel == UpdateChannel.Native:
    logger.add(
        sink=sys.stdout, 
        level="TRACE",    
        colorize=True,
    )


logger.add(
    sink="logs/app_{time:YYYY-MM-DD}.log",
    rotation="25 MB",      
    retention="30 days",    
    compression="zip",      
    format="{time:YYYY-MM-DD HH:mm:ss} | {level: <8} | {name}:{function}:{line} - {message}", 
    level=("DEBUG" if enable_debug_mode else "INFO") if Channel != UpdateChannel.Native else "TRACE",     
    encoding="utf-8",       
    backtrace=True,         
    diagnose=True,          
    enqueue=True,           
)


def trace(message:str,record_exc:bool = False):
    logger.log("TRACE",message,exc_info=record_exc)

def debug(message:str,record_exc:bool = False):
    logger.log("DEBUG",message,exc_info=record_exc)

def info(message:str,record_exc:bool = False):
    logger.log("INFO",message,exc_info=record_exc)

def warning(message:str,record_exc:bool = False):
    logger.log("WARNING",message,exc_info=record_exc)

def error(message:str,record_exc:bool = True):
    logger.log("ERROR",message,exc_info=record_exc)


def fatal(message:str):
    logger.log("CRITICAL",message,exc_info=True)