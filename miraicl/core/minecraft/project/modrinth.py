from .common import Project,ProjectType
from ...network.request import make_request_retry
from ...models.project.modrinth.search import ModrinthSearch
from ...models.project.modrinth.project_details import ProjectDetails
from ...models.project.common.resource import McResource
from urllib.parse import urlencode
from ... import logger

API_ADDRESS = "https://api.modrinth.com/v2"

async def search(keyword:str,type:ProjectType,limit:int,game_version:str,modloaders:list[str],offset:int):
    query_param = {
        "query": keyword,
        "offset":offset,
        "limit":limit
    }
    factets = "["
    for loader in modloaders:
        factets += f"[\"categories:{loader}\"]"
    factets += f"[\"versions:{game_version}\"]]"
    factets += f"[\"project_type:{type.name.lower()}\"]"
    query_param["factets"] = factets
    url = f"{API_ADDRESS}/search?{urlencode(query_param)}"

    result = await make_request_retry(url,"GET",check_status=True)
    if not result:
        raise ValueError("获取网络结果失败")
    return ModrinthSearch.parse(result.text)
    

async def match_project(mods:list[McResource]):
    logger.info("")
