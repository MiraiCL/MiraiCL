import platform

from miraicl.core.errors import SystemException

if platform.system().lower() != "windows":
    raise SystemException("This library only allow load on windows")