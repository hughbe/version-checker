@echo off

set "msbuildpath=C:\Program Files (x86)\MSBuild\14.0\Bin"

set "projpath=\VersionChecker\VersionChecker.sln"

set "curpath=C:\Users\Hugh\Documents\GitHub\version-checker"
set "fullpath=%curpath%%projpath%"

cd %msbuildpath%

msbuild %fullpath% /nologo /maxcpucount /verbosity:minimal /nodeReuse:false

cd %curpath%

exit /b %BUILDERRORLEVEL%