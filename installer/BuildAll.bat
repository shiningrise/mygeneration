del ".\application_build.log"
del ".\installbuild_mygen.log"
del ".\installbuild_mymeta.log"
del ".\installbuild_doodads.log"

if "%PROGRAMFILES(X86)%"=="" goto :x86
goto :x64
:x86
	if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"  /out ".\application_build.log" /rebuild release
	if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES%\NSIS\makensis.exe
	goto done
:x64
	if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES(X86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"  /out ".\application_build.log" /rebuild release
	if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES(X86)%\NSIS\makensis.exe
:done

%DEVENV% "..\plugins\MyMetaPlugins.sln"
%DEVENV% "..\plugins\ZeusPlugins.sln"
%DEVENV% "..\mygeneration\Zeus.sln"
set DEVENV=

"%MAKENSIS%" ".\mygeneration.nsi" > ".\installbuild_mygen.log"
"%MAKENSIS%"  ".\mymeta.nsi" > ".\installbuild_mymeta.log"
"%MAKENSIS%"  ".\doodads.nsi" > ".\installbuild_doodads.log"
"%MAKENSIS%"  ".\cst2mygen.nsi" > ".\installbuild_cst2mygen.log"
set MAKENSIS=

