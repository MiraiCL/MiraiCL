from concurrent.futures import ThreadPoolExecutor
from .. import logger
import psutil



default = ThreadPoolExecutor()

io_thread = ThreadPoolExecutor()

def submit_task():
    pass