from miraicl.core.runtimes.universal.information import sys_type,SystemType
from miraicl.core.errors import SystemException
if sys_type != SystemType.Windows:
    raise SystemException("This library are only allow load on windows")

import ctypes

ole32 = ctypes.WinDLL("ole32.dll")

ole32.CoInitialize(None)

