from typing import get_type_hints,Any,Self
from .parser import TomlProperty

class BaseTomlModel:
    def __init__(self):
        pass
    @classmethod
    def serialize(cls,data:dict[str,Any]) -> Self:
        obj = cls()

        alias = {}

        for key,property in obj.__dict__:
            if isinstance(property,TomlProperty):
                if property.__alias__:
                    alias[property.__alias__] = {
                        "alias": key,
                        "dyn_type": property.__dyn_type__,
                        "exclude_none": property.__exclude_none__
                    }
                    

        types = get_type_hints(cls)

        for key,value in data.items():
            alias_name = alias.get(key)
            if alias_name:
                setattr(obj,alias_name,TomlProperty(value,alias=alias_name.get("alias"),dyn_type=alias_name.get("dyn_type"),exclude_none=alias_name.get("exclude_none")))
            attr_t = types.get(key)
            if not attr_t:
                continue
            if not isinstance(value,attr_t):
                if not hasattr(obj,key) or not getattr(obj,key).__dyn_type__:
                    raise TypeError(f"DataType mistched: except {attr_t.__name__} ,actual is {type(value).__name__}")
            setattr(obj,key,value)
        return obj
    
    def deserialize(self):
        obj = {}
        for key,value in self.__dict__.items():
            if key.startswith("_"):
                continue
            obj[key] = value
        return obj
    
    