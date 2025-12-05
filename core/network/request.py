import aiorwlock
from hishel.httpx import AsyncCacheClient
from aiofiles.threadpool.binary import AsyncBufferedReader
from httpx import URL
from core import secret
from pathlib import Path

lock = aiorwlock.RWLock()

client:AsyncCacheClient = None

def init_client(proxies:dict[str,str] = None):
    global client
    with lock.writer:
        client = AsyncCacheClient(proxies)

async def make_request_retry(url:str | URL,method:str,headers:dict[str,str],data:HttpContent):
    sign_headers(url,headers)
    with lock.reader:
        pass

def sign_headers(url:str | URL,headers:dict[str,str]):
    if not headers.get("User-Agent"):
        if "minecraftforge.net" in str(url):
            headers["User-Agent"] = secret.BrowserUserAgent
        else:
            headers["User-Agent"] = secret.UserAgent
    if "api.curseforge.com" in url and not headers.get("x-api-key"):
        headers["x-api-key"] = secret.CurseForgeApiKey

class HttpContent:
    def __init__(self,content_type:str,content:object):
        if not (isinstance(content,AsyncBufferedReader) and isinstance(content,str) and isinstance(content,Path)):
            raise TypeError(f"{type(content)} can not cast to HttpContent")
        self.content = content
        self.content_type = content_type

