import glob
import os
import subprocess

ipycpath = 'C:\Program Files\IronPython 2.7\ipyc.exe'
cmd = ' /main:'

for name in glob.glob('C:\Program Files\IronPython 2.7\sapphire\*.py'):
	cmd += '"' + name + '" '

for name in glob.glob('C:\Program Files\IronPython 2.7\sapphire\*\*.py'):
	cmd += '"' + name + '" '
	
cmd = ipycpath + cmd + '/target:dll'

print(cmd)

subprocess.call(cmd)