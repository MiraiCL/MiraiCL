from hashlib import md5,sha1,sha256,sha512,sha3_512,_Hash
from pathlib import Path


def basehash(c_obj:_Hash,path:Path):
    if not path.exists(follow_symlinks=True):
        return c_obj
    with path.open("rb") as f:
        while True:
            data = f.read(1024)
            if not data:
                break
            c_obj.update(data)
    return c_obj

def cmd5(path:Path):
    return basehash(md5(),path).hexdigest()

def csha1(path:Path):
    return basehash(sha1(),path).hexdigest()

def csha256(path:Path):
    return basehash(sha256(),path).hexdigest()

def csha512(path:Path):
    return basehash(sha512(),path).hexdigest()

def csha3_512(path:Path):
    return basehash(sha3_512(),path)