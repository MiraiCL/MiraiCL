from concurrent import interpreters

threading = {}

def create(name:str,pre_init:callable) -> interpreters.Interpreter:
    threads:list[interpreters.Interpreter] = threading.get(name)
    for t in threads:
        if t.is_running():
            return t