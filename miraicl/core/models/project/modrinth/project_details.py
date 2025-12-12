from ...jtoken import JToken
from typing import Optional
from pydantic import Field

class ProjectDetails(JToken):
    project_sulg:str = Field(alias="slug")
    title:str
    description:str
    categories:list[str]
    client_side:str
    server_side:str
    project_type:str
    downloads:int
    icon_url:Optional[str]
    color:int
    thread_id:int
    monetization_status:str
    project_id:str
    author:str
    display_categories:list[str]
    versions:list[str]
    follows:int
    date_created:str
    date_modified:str
    latest_version:str
    license:str
    gallery:str
    featured_gallery:Optional[str]