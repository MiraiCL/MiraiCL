import ctypes
import ctypes.wintypes as wintypes

# load advapi32.dll
advapi32 = ctypes.WinDLL('advapi32.dll')

# Key
HKEY_CLASSES_ROOT     = 0x80000000
HKEY_CURRENT_USER     = 0x80000001
HKEY_LOCAL_MACHINE    = 0x80000002
HKEY_USERS            = 0x80000003
HKEY_PERFORMANCE_DATA = 0x80000004
HKEY_CURRENT_CONFIG   = 0x80000005
HKEY_DYN_DATA         = 0x80000006

# Access Permission
KEY_QUERY_VALUE       = 0x0001
KEY_SET_VALUE         = 0x0002
KEY_CREATE_SUB_KEY    = 0x0004
KEY_ENUMERATE_SUB_KEYS= 0x0008
KEY_READ              = 0x20019
KEY_WRITE             = 0x20006
KEY_ALL_ACCESS        = 0xF003F
KEY_WOW64_32KEY       = 0x0200
KEY_WOW64_64KEY       = 0x0100

# Type
REG_NONE                       = 0
REG_SZ                         = 1
REG_EXPAND_SZ                  = 2
REG_BINARY                     = 3
REG_DWORD                      = 4
REG_DWORD_LITTLE_ENDIAN        = 4
REG_DWORD_BIG_ENDIAN           = 5
REG_LINK                       = 6
REG_MULTI_SZ                   = 7
REG_RESOURCE_LIST              = 8
REG_QWORD                      = 11
REG_QWORD_LITTLE_ENDIAN        = 11

# Error
ERROR_SUCCESS                  = 0
ERROR_FILE_NOT_FOUND           = 2
ERROR_ACCESS_DENIED            = 5
ERROR_MORE_DATA                = 234
ERROR_NO_MORE_ITEMS            = 259

# RegOpenKeyExW
advapi32.RegOpenKeyExW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.LPCWSTR,   # lpSubKey
    wintypes.DWORD,     # ulOptions
    wintypes.DWORD,     # samDesired
    ctypes.POINTER(wintypes.HKEY) # phkResult
]
advapi32.RegOpenKeyExW.restype = wintypes.LONG

# RegCreateKeyExW
class SECURITY_ATTRIBUTES(ctypes.Structure):
    _fields_ = [
        ("nLength", wintypes.DWORD),
        ("lpSecurityDescriptor", wintypes.LPVOID),
        ("bInheritHandle", wintypes.BOOL)
    ]

advapi32.RegCreateKeyExW.argtypes = [
    wintypes.HKEY,                # hKey
    wintypes.LPCWSTR,             # lpSubKey
    wintypes.DWORD,               # Reserved
    wintypes.LPWSTR,              # lpClass
    wintypes.DWORD,               # dwOptions
    wintypes.DWORD,               # samDesired
    ctypes.POINTER(SECURITY_ATTRIBUTES), # lpSecurityAttributes
    ctypes.POINTER(wintypes.HKEY),        # phkResult
    ctypes.POINTER(wintypes.DWORD)        # lpdwDisposition
]
advapi32.RegCreateKeyExW.restype = wintypes.LONG

# RegQueryValueExW
advapi32.RegQueryValueExW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.LPCWSTR,   # lpValueName
    wintypes.LPDWORD,   # lpReserved
    ctypes.POINTER(wintypes.DWORD), # lpType
    wintypes.LPBYTE,    # lpData
    ctypes.POINTER(wintypes.DWORD)  # lpcbData
]
advapi32.RegQueryValueExW.restype = wintypes.LONG

# RegSetValueExW
advapi32.RegSetValueExW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.LPCWSTR,   # lpValueName
    wintypes.DWORD,     # Reserved
    wintypes.DWORD,     # dwType
    wintypes.LPCVOID,   # lpData
    wintypes.DWORD      # cbData
]
advapi32.RegSetValueExW.restype = wintypes.LONG

# RegDeleteKeyExW
advapi32.RegDeleteKeyExW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.LPCWSTR,   # lpSubKey
    wintypes.DWORD,     # samDesired
    wintypes.DWORD      # Reserved
]
advapi32.RegDeleteKeyExW.restype = wintypes.LONG

# RegDeleteValueW
advapi32.RegDeleteValueW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.LPCWSTR    # lpValueName
]
advapi32.RegDeleteValueW.restype = wintypes.LONG

# RegEnumKeyExW
advapi32.RegEnumKeyExW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.DWORD,     # dwIndex
    wintypes.LPWSTR,    # lpName
    ctypes.POINTER(wintypes.DWORD), # lpcName
    wintypes.LPDWORD,   # lpReserved
    wintypes.LPWSTR,    # lpClass
    ctypes.POINTER(wintypes.DWORD), # lpcClass
    ctypes.POINTER(wintypes.FILETIME) # lpftLastWriteTime
]
advapi32.RegEnumKeyExW.restype = wintypes.LONG

# RegEnumValueW
advapi32.RegEnumValueW.argtypes = [
    wintypes.HKEY,      # hKey
    wintypes.DWORD,     # dwIndex
    wintypes.LPWSTR,    # lpValueName
    ctypes.POINTER(wintypes.DWORD), # lpcValueName
    wintypes.LPDWORD,   # lpReserved
    ctypes.POINTER(wintypes.DWORD), # lpType
    wintypes.LPBYTE,    # lpData
    ctypes.POINTER(wintypes.DWORD)  # lpcbData
]
advapi32.RegEnumValueW.restype = wintypes.LONG

# RegCloseKey
advapi32.RegCloseKey.argtypes = [wintypes.HKEY]
advapi32.RegCloseKey.restype = wintypes.LONG

# ==================== 工具函数 ====================
def _check_result(result, func_name):
    """检查API调用结果"""
    if result != ERROR_SUCCESS:
        error_msg = ctypes.FormatError(result)
        raise WindowsError(f"{func_name} 失败，错误代码: {result} - {error_msg}")
    return result

# ==================== 主类 ====================
class WindowsRegistry:
    """Windows注册表操作类"""
    
    def __init__(self, wow64_32bit=False):
        """
        初始化注册表操作对象
        
        Args:
            wow64_32bit: 在64位系统上访问32位注册表视图
        """
        self.wow64_flag = KEY_WOW64_32KEY if wow64_32bit else KEY_WOW64_64KEY
    
    def open_key(self, hkey, subkey, access=KEY_READ):
        """
        打开注册表键
        
        Args:
            hkey: 根键句柄 (如 HKEY_CURRENT_USER)
            subkey: 子键路径
            access: 访问权限
            
        Returns:
            打开的键句柄
        """
        handle = wintypes.HKEY()
        result = advapi32.RegOpenKeyExW(
            hkey,
            subkey,
            0,
            access | self.wow64_flag,
            ctypes.byref(handle)
        )
        _check_result(result, "RegOpenKeyExW")
        return handle
    
    def create_key(self, hkey, subkey, access=KEY_ALL_ACCESS):
        """
        创建或打开注册表键
        
        Args:
            hkey: 根键句柄
            subkey: 子键路径
            access: 访问权限
            
        Returns:
            (键句柄, 创建标志)
        """
        handle = wintypes.HKEY()
        disposition = wintypes.DWORD()
        
        result = advapi32.RegCreateKeyExW(
            hkey,
            subkey,
            0,
            None,
            REG_OPTION_NON_VOLATILE,
            access | self.wow64_flag,
            None,
            ctypes.byref(handle),
            ctypes.byref(disposition)
        )
        _check_result(result, "RegCreateKeyExW")
        
        created_new = disposition.value == 1  # REG_CREATED_NEW_KEY
        return handle, created_new
    
    def read_value(self, hkey, value_name):
        """
        读取注册表值
        
        Args:
            hkey: 键句柄
            value_name: 值名称 (None或空字符串表示默认值)
            
        Returns:
            (值数据, 值类型)
        """
        if value_name is None:
            value_name = ""
        
        # 第一次调用获取数据大小
        data_type = wintypes.DWORD()
        data_size = wintypes.DWORD()
        
        result = advapi32.RegQueryValueExW(
            hkey,
            value_name,
            None,
            ctypes.byref(data_type),
            None,
            ctypes.byref(data_size)
        )
        
        if result == ERROR_FILE_NOT_FOUND:
            raise WindowsError(f"注册表值不存在: {value_name}")
        
        # 分配缓冲区并读取数据
        buffer = ctypes.create_string_buffer(data_size.value)
        result = advapi32.RegQueryValueExW(
            hkey,
            value_name,
            None,
            None,
            buffer,
            ctypes.byref(data_size)
        )
        _check_result(result, "RegQueryValueExW")
        
        # 根据类型转换数据
        raw_data = buffer.raw[:data_size.value]
        reg_type = data_type.value
        
        if reg_type == REG_SZ or reg_type == REG_EXPAND_SZ:
            # 去掉结尾的\0\0 (REG_MULTI_SZ) 或单个\0 (REG_SZ)
            data = raw_data.decode('utf-16le').rstrip('\x00')
        elif reg_type == REG_DWORD:
            data = int.from_bytes(raw_data[:4], 'little')
        elif reg_type == REG_QWORD:
            data = int.from_bytes(raw_data[:8], 'little')
        elif reg_type == REG_MULTI_SZ:
            # 多字符串以\0\0结尾
            data = raw_data.decode('utf-16le').rstrip('\x00').split('\x00')
        elif reg_type == REG_BINARY:
            data = bytes(raw_data)
        else:
            data = raw_data
        
        return data, reg_type
    
    def set_value(self, hkey, value_name, data, reg_type=None):
        """
        设置注册表值
        
        Args:
            hkey: 键句柄
            value_name: 值名称
            data: 要设置的数据
            reg_type: 值类型 (自动推断如果为None)
        """
        if value_name is None:
            value_name = ""
        
        # 自动推断类型
        if reg_type is None:
            if isinstance(data, str):
                reg_type = REG_SZ
            elif isinstance(data, int):
                if -2147483648 <= data <= 4294967295:
                    reg_type = REG_DWORD
                else:
                    reg_type = REG_QWORD
            elif isinstance(data, bytes):
                reg_type = REG_BINARY
            elif isinstance(data, list) and all(isinstance(x, str) for x in data):
                reg_type = REG_MULTI_SZ
            else:
                raise TypeError(f"不支持的数据类型: {type(data)}")
        
        # 准备数据
        if reg_type == REG_SZ or reg_type == REG_EXPAND_SZ:
            data_bytes = (data + '\x00').encode('utf-16le')
        elif reg_type == REG_DWORD:
            data_bytes = data.to_bytes(4, 'little')
        elif reg_type == REG_QWORD:
            data_bytes = data.to_bytes(8, 'little')
        elif reg_type == REG_MULTI_SZ:
            # 连接字符串并用\0分隔，最后加\0\0
            joined = '\x00'.join(data) + '\x00\x00'
            data_bytes = joined.encode('utf-16le')
        elif reg_type == REG_BINARY:
            data_bytes = data
        else:
            data_bytes = data
        
        result = advapi32.RegSetValueExW(
            hkey,
            value_name,
            0,
            reg_type,
            data_bytes,
            len(data_bytes)
        )
        _check_result(result, "RegSetValueExW")
    
    def delete_value(self, hkey, value_name):
        """删除注册表值"""
        result = advapi32.RegDeleteValueW(hkey, value_name)
        if result != ERROR_SUCCESS and result != ERROR_FILE_NOT_FOUND:
            _check_result(result, "RegDeleteValueW")
    
    def delete_key(self, hkey, subkey):
        """删除注册表键"""
        result = advapi32.RegDeleteKeyExW(
            hkey,
            subkey,
            self.wow64_flag,
            0
        )
        _check_result(result, "RegDeleteKeyExW")
    
    def enum_keys(self, hkey):
        """枚举子键"""
        keys = []
        index = 0
        
        while True:
            # 准备缓冲区
            name_len = wintypes.DWORD(256)
            name_buffer = ctypes.create_unicode_buffer(name_len.value)
            
            result = advapi32.RegEnumKeyExW(
                hkey,
                index,
                name_buffer,
                ctypes.byref(name_len),
                None,
                None,
                None,
                None
            )
            
            if result == ERROR_NO_MORE_ITEMS:
                break
            _check_result(result, "RegEnumKeyExW")
            
            keys.append(name_buffer.value)
            index += 1
        
        return keys
    
    def enum_values(self, hkey):
        """枚举值项"""
        values = {}
        index = 0
        
        while True:
            # 准备缓冲区
            name_len = wintypes.DWORD(256)
            name_buffer = ctypes.create_unicode_buffer(name_len.value)
            
            result = advapi32.RegEnumValueW(
                hkey,
                index,
                name_buffer,
                ctypes.byref(name_len),
                None,
                None,
                None,
                None
            )
            
            if result == ERROR_NO_MORE_ITEMS:
                break
            _check_result(result, "RegEnumValueW")
            
            # 读取值
            try:
                value_data, value_type = self.read_value(hkey, name_buffer.value)
                values[name_buffer.value] = (value_data, value_type)
            except Exception:
                values[name_buffer.value] = (None, None)
            
            index += 1
        
        return values
    
    @staticmethod
    def close_key(hkey):
        """关闭键句柄"""
        if hkey:
            advapi32.RegCloseKey(hkey)
