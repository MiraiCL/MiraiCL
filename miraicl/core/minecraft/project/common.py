from enum import Enum
from aiorwlock import RWLock
class Project:
    def __init__():
        pass

class ProjectType(Enum):
    Mod = 6
    Modpack = 4471
    Datapack = 6945
    ResourcePack = 12

database_rw_lock = RWLock()

database = {}