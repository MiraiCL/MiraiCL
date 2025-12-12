from ...models.minecraft.version import McVersion,FullVersionJson
from pathlib import Path


def install(version:McVersion,name:str,base_folder: Path,path: Path):
    if not base_folder.exists():
        base_folder.mkdir(parents=True)
    if not path.exists():
        path.mkdir(parents=True)
    
    

def repair_libraries(json:FullVersionJson,base_foler:Path):
    libraries_path = base_foler.joinpath("libraries")
    if not libraries_path.exists():
        libraries_path.mkdir(parents=True)
    pass

def repair_asssets(json:FullVersionJson,base_foler:Path):
    pass