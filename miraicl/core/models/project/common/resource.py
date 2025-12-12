from ....utils.toml.model import BaseTomlModel

class McResource(BaseTomlModel):
    name:str
    project_id:str
    project_name:str
    path:str
    sha1:str
    data:CommonProjectDetails
    murmur_hash2:int
    

class CommonProjectDetails(BaseTomlModel):
    pass