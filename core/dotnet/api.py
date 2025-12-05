# .NET CLR
import subprocess

use_core = True

try:
    subprocess.run("dotnet")
except FileNotFoundError:
    use_core = False


import pythonnet

if use_core:
    pythonnet.load("coreclr")
else:
    pythonnet.load()

import clr

