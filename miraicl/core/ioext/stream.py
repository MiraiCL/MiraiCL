from io import BufferedIOBase,RawIOBase
from typing import Optional,Union
from miraicl.core.errors import InvalidOperationException,ObjectAccessException
import asyncio

class ThrottleStream:
    pass