from ...jtoken import JToken
from typing import Optional
from .project_details import ProjectDetails

class ModrinthSearch(JToken):
    hits: list[ProjectDetails]
    offest: int
    limit: int
    total_hits: int