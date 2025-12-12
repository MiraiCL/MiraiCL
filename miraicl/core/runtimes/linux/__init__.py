import platform
from miraicl.core.errors import SystemException

if platform.system().lower() != "linux":
    raise SystemException("This library are only allow load on linux")