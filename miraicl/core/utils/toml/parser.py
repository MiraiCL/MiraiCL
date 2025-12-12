# Copyright (c) Ruollin 2025. All rights reserved.
#
# This file license under MIT License. 

from typing import Any
from io import TextIOBase
import tomllib

class TomlProperty[T]:
    def __init__(self,value:T,alias:str = "",exclude_none:bool = False,dyn_type:bool = False):
        self.__alias__ = alias
        self.__exclude_none__ = exclude_none
        self.__dyn_type__ = dyn_type
        self.value = value

def load(file):
    return tomllib.load(file)

def loads(content:str):
    return tomllib.loads(content)

def dumps(content:dict[str,Any]):
    import datetime
    def fmt_key(key: str) -> str:
        # Bare key if possible, else quoted
        if key == "" or not key.isidentifier() or any(c in key for c in " .-\"'"):
            return '"' + key.replace('"', '\"') + '"'
        return key

    def fmt_value(val: Any) -> str:
        if isinstance(val, str):
            # Use basic string, escape quotes and backslashes
            if '\n' in val or '\r' in val:
                return '"""' + val.replace('"""', '\"\"\"') + '"""'
            return '"' + val.replace('"', '\"').replace('\\', '\\') + '"'
        if isinstance(val, bool):
            return "true" if val else "false"
        if isinstance(val, int):
            return str(val)
        if isinstance(val, float):
            if val == float('inf'):
                return 'inf'
            if val == float('-inf'):
                return '-inf'
            if str(val).lower() == 'nan':
                return 'nan'
            return repr(val)
        if isinstance(val, datetime.datetime):
            # RFC: offset or local
            if val.tzinfo:
                return val.isoformat().replace('+00:00', 'Z')
            return val.isoformat()
        if isinstance(val, datetime.date):
            return val.isoformat()
        if isinstance(val, datetime.time):
            return val.isoformat()
        if isinstance(val, dict):
            # Inline table
            items = []
            for k, v in val.items():
                items.append(f"{fmt_key(k)} = {fmt_value(v)}")
            return '{' + ', '.join(items) + '}'
        if isinstance(val, list):
            # Array
            return '[' + ', '.join(fmt_value(v) for v in val) + ']'
        raise TypeError(f"Unsupported TOML type: {type(val)}")

    def write_table(prefix: list[str], table: dict[str, Any], lines: list[str]):
        # Simple keys first
        for k, v in table.items():
            if isinstance(v, dict) or (isinstance(v, list) and v and all(isinstance(i, dict) for i in v)):
                continue
            lines.append(f"{fmt_key(k)} = {fmt_value(v)}")
        # Nested tables
        for k, v in table.items():
            if isinstance(v, dict):
                path = prefix + [k]
                lines.append("")
                lines.append("[" + ".".join(fmt_key(p) for p in path) + "]")
                write_table(path, v, lines)
            elif isinstance(v, list) and v and all(isinstance(i, dict) for i in v):
                path = prefix + [k]
                for item in v:
                    lines.append("")
                    lines.append("[[" + ".".join(fmt_key(p) for p in path) + "]]")
                    write_table(path, item, lines)

    lines = []
    write_table([], content, lines)
    return "\n".join(lines).lstrip('\n') + "\n"


def dump(content:dict[str,Any],f:TextIOBase):
    f.write(dumps(content))