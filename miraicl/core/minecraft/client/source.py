from typing import Optional

class McVersionSource:
    version_index:str
    version_index_v2:Optional[str]
    maven_server:Optional[str]
    resource_server:Optional[str]
    java_index_url:Optional[str]
    description:str

mojang = McVersionSource()

mojang.version_index = "https://piston-meta.mojang.com/mc/game/version_manifest.json"

mojang.version_index_v2 = "https://piston-meta.mojang.com/mc/game/version_manifest_v2.json"

mojang.maven_server = "https://libraries.minecraft.net"

mojang.resource_server = "https://resources.download.minecraft.net"

mojang.java_index_url = "https://piston-meta.mojang.com/v1/products/java-runtime/2ec0cc96c44e5a76b9c8b7c39df7210883d12871/all.json"

mojang.description = "Official download source."

source = {
    "Official": mojang
}