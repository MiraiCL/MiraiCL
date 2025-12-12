from requests import Response
from typing import Optional

class InvalidOperationException(Exception):
    pass

class EndOfStreamException(InvalidOperationException):
    pass

class ObjectAccessException(InvalidOperationException):
    pass

class TimeoutException(Exception):
    pass

class WebException(Exception):
    def __init__(self,message:Optional[str] = None,response:Optional[Response] = None):
        super().__init__()
        self.message = message
        self.response = response
    def __str__(self):
        if self.message:
            return self.message
        elif self.response:
            return f"Remote server return an error: {self.response.status_code} ({self.response.reason})"
        return ""
    @property
    def status(self):
        return self.response.status_code
    @property
    def headers(self):
        return self.response.headers
    @property
    def content(self):
        return self.response.content
    


class SystemException(Exception):
    pass

class DebugException(Exception):
    pass

class LoginException(Exception):
    pass

class InvalidSignatureException(Exception):
    pass

raise WebException()