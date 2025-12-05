from common import Project,ProjectType
from ...network.request import make_request_retry

from urllib.parse import urlencode

from httpx import URL

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
    
    url = URL(
        f"{API_ADDRESS}/search",
        query = urlencode(query_param)
    )

    result = make_request_retry(url,"GET")