from enum import Enum
import platform


class SystemType(Enum):
    Windows = 0
    Linux = 1
    MacOS = 2
    Unknown = 3

sys_type:SystemType

match platform.system().lower():
    case "windows":
        sys_type = SystemType.Windows
    case "linux":
        sys_type = SystemType.Linux
    case "macos" | "osx":
        sys_type = SystemType.MacOS
    case _:
        sys_type = SystemType.Unknown