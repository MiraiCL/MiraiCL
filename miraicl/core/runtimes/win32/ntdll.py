import ctypes
from ctypes import wintypes

ntdll = ctypes.WinDLL("ntdll")

ntdll.RtlGetNtVersionNumbers.argtypes = [
        ctypes.POINTER(wintypes.DWORD),  # pdwMajorVersion
        ctypes.POINTER(wintypes.DWORD),  # pdwMinorVersion  
        ctypes.POINTER(wintypes.DWORD)   # pdwBuildNumber
    ]

ntdll.RtlGetNtVersionNumbers.restype = None

def get_nt_version() -> tuple[int,int,int]:
    major = wintypes.DWORD()
    minor = wintypes.DWORD()
    build = wintypes.DWORD()

    ntdll.RtlGetNtVersionNumbers(
        ctypes.byref(major),
        ctypes.byref(minor),
        ctypes.byref(build)
    )
    return (major.value,minor.value,build.value & 0xffff)