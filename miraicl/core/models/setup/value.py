from miraicl.core.models.jtoken import JToken

class SetupValue[T]:
    value:T
    undefined:bool

    def __init__(self,value:T):
        self.value = value
        
    def get(self) -> T:
        return self.value
    def set(self,value:T):
        self.value = value
    def reset(self):
        self.undefined = True