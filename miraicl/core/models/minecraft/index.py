from typing import Optional
from ..jtoken import JToken
from pydantic import Field

class VersionIndexLatest(JToken):
    release: str
    snapshot: str

class VersionManifest(JToken):
    id: str
    type: str
    url: str
    time: str
    release_time: str = Field(alias="releaseTime")
    sha1: Optional[str]
    compliance_level: Optional[int] = Field(alias="complianceLevel")

class VersionIndex(JToken):
    latest: VersionIndexLatest
    versions:list[VersionManifest]