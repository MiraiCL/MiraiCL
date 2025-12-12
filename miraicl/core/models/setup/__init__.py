from ...utils.toml.model import BaseTomlModel
from ..jtoken import JToken
from .value import SetupValue

class Setup(BaseTomlModel):
    system_proxy: SetupValue[str] = SetupValue("")
    profiles: SetupValue[str] = SetupValue("")