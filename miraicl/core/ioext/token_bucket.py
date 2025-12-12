import asyncio

class TokenBucket:
    def __init__(self,maxToken:int,remain_token:int):
        pass
    async def wait_token(self,acquire_token:int):
        while self