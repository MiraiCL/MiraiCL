from pydantic import BaseModel
from typing import Type, TypeVar

T = TypeVar("T", bound="JToken")

class JToken(BaseModel):
    def as_json(self, exclude_none: bool = True) -> str:
        return self.model_dump_json(exclude_none=exclude_none)
    
    @classmethod
    def parse(cls: Type[T], content: str) -> T:
        return cls.model_validate_json(content)