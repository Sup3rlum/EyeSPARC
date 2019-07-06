import glob


ipycpath = 'C:\Program Files\IronPython 2.7\ipyc.exe'
cmd = ''

for name in glob.glob('sapphire/*.py'):
	cmd += name + ' '
	
cmd = ipycpath + ''

print cmd