import glob
import os
import subprocess

ipycpath = 'C:\Program Files\IronPython 2.7\ipyc.exe'
cmd = ' '

_paths1 = ['"' + name + '" ' for name in glob.glob('C:\Program Files\IronPython 2.7\sapphire\*.py')]
_paths2 = ['"' + name + '" ' for name in glob.glob('C:\Program Files\IronPython 2.7\sapphire\*\*.py')]

cmd += ' /main:' + _paths1[0] + ' '
cmd += ''.join(_paths1)
cmd += ''.join(_paths2)
cmd = ipycpath + cmd + '/target:dll'


subprocess.call(cmd)
