from enum import Enum

class UpdateChannel(Enum):
    Native = 0
    Nightly = 1
    Preview = 2
    Release = 3

LauncherName = "MiraiCL"

CoreVersion = "0.0.0"

CommitHash = "native"

CurseForgeApiKey = ""

Branch = "Official"

Channel = UpdateChannel.Native

UserAgent = f"{LauncherName}/{CoreVersion}(CoreVersion: {CoreVersion}-{CommitHash[:6] if len(CommitHash) > 7 else CommitHash}, Channel:{Channel.name})"

BrowserUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/142.0.0.0 Safari/537.36"

enable_debug_mode = False