from ..jtoken import JToken
from typing import Optional
from pydantic import Field

class McAssetsIndex(JToken):
    id:str
    sha1:str
    size:int
    total_size:int = Field(alias="totalSize")
    url:str

class McOs(JToken):
    name:Optional[str]
    arch:Optional[str]

class McFeatures(JToken):
    is_demo_user:Optional[bool]
    has_custom_resolution:Optional[bool]
    has_quick_plays_support:Optional[bool]
    is_quick_play_singleplayer:Optional[bool]
    is_quick_play_multiplayer:Optional[bool]
    is_quick_play_realms:Optional[bool]

class McAction(JToken):
    action:str
    features:Optional[McFeatures]
    os:Optional[McOs]

class McGameArguments(JToken):
    rules:list[McAction]
    value:str

class McArguments(JToken):
    game:list[str|McGameArguments]
    jvm:list[str|McGameArguments]

class McDownloads(JToken):
    path:Optional[str]
    size:int
    sha1:str
    url:str

class McClientServer(JToken):
    client:McDownloads
    client_mappings:Optional[McDownloads]
    server:Optional[McDownloads]
    server_mappings:Optional[McDownloads]

class McExtract(JToken):
    exclude:list[str]

class McLibraries(JToken):
    artifact:Optional[McDownloads]
    classifiers:Optional[dict[str,McDownloads]]
    extract:Optional[McExtract]
    name:Optional[str]
    natives:Optional[str]
    rules:Optional[list[McAction]]

class LoggingFile(JToken):
    id:str
    sha1:str
    size:int
    url:str

class LoggingClient(JToken):
    arguments:str
    file:LoggingFile
    type:str

class Mclogging(JToken):
    client:LoggingClient

class VersionJson(JToken):
    arguments:Optional[McArguments]
    assets_index:McAssetsIndex
    assets:str
    id:Optional[str]
    compliance_level:int = Field(alias="complianceLevel")
    inherits_from:Optional[str] = Field(alias="inheritsFrom")
    downloads:McClientServer
    libraries:list[McLibraries]
    logging:Optional[Mclogging]
    main_class:str = Field(alias="mainClass")
    # PCL 会在版本 Json 留下 clientVersion 用于标识原版版本
    client_version:Optional[str] = Field(alias="clientVersion")
    
class FullVersionJson(VersionJson):
    # HMCL/SJMCL/PCL CE 会留下 patches 字段
    patches:Optional[list[VersionJson]]

class McVersion:
    def __init__(self,version:str):
        splited_str = version.replace("-snapshot-","").split(".")
        if len(splited_str) > 2:
            self.major_version = splited_str[0]
            self.minor_version = splited_str[1]
            self.patch_version = splited_str[2]
        elif len(splited_str) > 1:
            self.major_version = splited_str[0]
            self.minor_version = splited_str[1]
        else:
            raise BadMcVersionException(f"Invalid version string:{version}")



class BadMcVersionException(Exception):
    pass

