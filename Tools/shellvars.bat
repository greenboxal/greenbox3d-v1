@echo off

if "%GB3DVARS%" == "TRUE" goto :end

echo Preparing GreenBox3D shell environment...
echo.

set GB3DPATH=%1DebugBuild
set RUBYLIB=%1Lib;%GB3DPATH%;%RUBYLIB%

set PATH=%GB3DPATH%;%PATH%

set GB3DVARS=TRUE
:end
