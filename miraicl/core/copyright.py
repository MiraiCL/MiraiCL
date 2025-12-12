import psutil
from httpx import AsyncClient
import asyncio
import os
import pathlib
import locale

# 不添加国内的网站，因为有时候国内访问比国外还慢
# 例如 im.qq.com，国内普遍 50+ms，新加坡只需要 11ms...
# 不得不感慨国内运营商绕路由的能力
urls = [
    "https://www.google.com",
    "https://www.youtube.com",
    "https://www.x.com",
    "https://t.me",
    "https://mail.google.com",
    "https://dns.google",
    "https://pki.goog",
    "https://www.wikipedia.org",
    "https://chatgpt.com"
    "https://gemini.google.com"
]

# 提供地区检测服务，以决定是否应该提供离线登录许可
# 基于系统语言、进程列表、关键文件夹及网络信息进行检测，不保证结果绝对准确

async def start_check():
    # 加权平均数，取值范围 0 ~ 1，大于 0.85 则视为大陆地区
    weight = (await network_check()) * 0.025
    weight += 0.3 if await location_check() else 0
    weight += 0.1 if check_env() else 0
    weight += 0.2 if search_process() else 0
    weight += 0.15 if check_special_path() else 0
    return weight >= 0.85,weight

async def network_check():
    "进行网络检测，以确定计算机网络环境（可能不准）"
    return await netrequest_getweight(urls)

async def location_check():
    
    async with AsyncClient(proxy=None) as client:
        result = await client.get("https://www.cloudflare-cn.com/cdn-cgi/trace")
        return "loc=CN" in result.text

def check_env():
    return "Chinese" in locale.getlocale()[0]

async def netrequest_getweight(url:list[str]):
    "绕过系统代理并发向目标网站发送 HTTP 请求，并返回目标网站在当前网络的可用性"
    client = AsyncClient(proxy=None)

    task = []

    for url in urls:
        task.append(req(client,url))
    result = await asyncio.gather(*task)
    weight = 0
    for r in result:
        if not r:
            weight += 1
        else:
            print(r.url)
    await client.aclose()
    print(weight)
    return weight


async def req(client:AsyncClient,url:str):
    try:
        return await client.get(url)
    except:
        return None

def search_process():
    "进程检测"
    for p in psutil.process_iter(["name"]):
        name = p.info.get("name").lower()
        match name:
            # 360
            case "360se.exe" | "zudongfangyu.exe" | "360huabao.exe" | "360safe.exe":
                return True
            # Huorong
            case "hipsmain.exe":
                return True
            # Tencent
            case "qq.exe" | "wechat.exe" | "tim.exe" | "qqpcmgr.exe" | "qqpctray.exe" | "qqbrowser.exe":
                return True
            # SteamCommunity 302
            case "Steamcommunity_302.exe" | "steamcommunity_302.cli.exe" | "steamcommunity_302.caddy" | "steamcommunity_302.cli":
                return True
            # KingSoft
            case "wps.exe" | "kxetray.exe" | "knewvip.exe":
                return True
        # 3rd Launcher
        if "pcl" in name or "plain craft launcher" in name or "bakaxl" in name or "hmcl" in name or "sjmcl" in name or "launcherx" in name:
            return True
    return False

def check_special_path():
    pcl_ce_path = pathlib.Path(os.environ.get("APPDATA",""),".pclce")
    pcl_path = pathlib.Path(os.environ.get("APPDATA"),"PCL")
    bakaxl_path = pathlib.Path(os.environ.get("APPDATA"),"BakaXL")
    hmcl_path = pathlib.Path(os.environ.get("APPDATA"),".hmcl")
    netease_path = list[pathlib.Path]()
    for d in psutil.disk_partitions():
        netease_path.append(pathlib.Path(d.device,"MCLDownload"))
    if pcl_ce_path.exists() or pcl_path.exists() or bakaxl_path.exists() or hmcl_path.exists():
        return True
    else:
        for n in netease_path:
            if n.exists():
                return True
        return False
