import aiorwlock
import asyncio

from aiofiles.threadpool.binary import AsyncBufferedReader
from requests import Session,Response
from miraicl.core import secret,logger
from typing import Optional
from miraicl.core.models.jtoken import JToken
from io import BytesIO,IOBase
from typing import Optional
from ..threading import dispatcher
from urllib.parse import urlencode
from miraicl.core.errors import WebException

lock = aiorwlock.RWLock()

session:Session = Session()

proxies = {}

async def make_request_retry(url:str,method:str,headers:Optional[dict[str,str]] = None,data:Optional[HttpContent] = None,timeout:int = 25000,retry:int = 3,delay:int = 300,check_status:bool = False,make_log:bool = True):
    if not headers:
        headers = {}
        if make_log:
            logger.info(f"发送网络请求：{method} {url}，最大超时：{timeout}ms")
    result:Optional[Response] = None
    sign_headers(url,headers)
    with lock.reader:
        loop = asyncio.get_event_loop()
        for i in range(retry):
            if data:
                result = await loop.run_in_executor(dispatcher.io_thread,lambda: session.request(method,url,data=data.make(),timeout=timeout,proxies=None))
            else:
                result = await loop.run_in_executor(dispatcher.io_thread,lambda: session.request(method,url,timeout=timeout,proxies=None))
            if not result:
                # :(
                raise WebException("获取网络结果失败：无效响应")
            if result.status_code == 429:
                await asyncio.sleep(delay * i ** i)
                continue
            if check_status and not result.ok:
                raise WebException(response=result)
            return result
    raise WebException(response=result)
        
def sign_headers(url:str,headers:dict[str,str]):
    if not headers.get("User-Agent"):
        if "minecraftforge.net" in url:
            headers["User-Agent"] = secret.BrowserUserAgent
        else:
            headers["User-Agent"] = secret.UserAgent
    if "api.curseforge.com" in url and not headers.get("x-api-key"):
        headers["x-api-key"] = secret.CurseForgeApiKey

class HttpContent:
    """
    HttpContent

    将给定请求载荷转换为 HTTP 正文形式

    HttpContent 不支持流式传输，如果需要加载较大载荷，请改用 BufferedIOBase
    """
    def __init__(self,content_type:str,content:str|bytearray|bytes|JToken,encoding:Optional[str] = None):
        self.content = BytesIO()
        if isinstance(content,str):
            self.content.write(content.encode(encoding if encoding else "utf-8"))
        elif isinstance(content,bytes) or isinstance(content,bytearray):
            self.content.write(content)
        elif isinstance(content,JToken):
            self.content.write(content.as_json().encode(encoding if encoding else "utf-8"))
        else:
            raise TypeError(f"{type(content)} can not cast to HttpContent.")

        self.content_type = f"{content_type}{f";charset={encoding}" if encoding else ""}"
    def make(self) -> BytesIO:
        self.content.seek(0)
        return self.content


class HttpJsonContent(HttpContent):
    """
    HttpJsonContent

    适用于 Json 载荷的内容类
    """
    def __init__(self,content:JToken,encoding:str = "utf-8"):
        super().__init__("application/json",content,encoding)

class HttpUrlEncodedContent(HttpContent):
    """
    HttpUrlEncodedContent

    适用于表单的内容类
    """

    def __init__(self,contnet:dict[str,object],encoding:str = "utf-8"):
        super().__init__("application/x-www-form-urlencoded",urlencode(contnet))