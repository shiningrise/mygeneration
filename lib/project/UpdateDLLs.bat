if "%PROGRAMFILES(X86)%"=="" goto :x86
goto :x64
:x86
	if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"  /out ".\plugins_build.log" /rebuild release
	if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES%\NSIS\makensis.exe
	goto done
:x64
	if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES(X86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"  /out ".\plugins_build.log" /rebuild release
	if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES(X86)%\NSIS\makensis.exe
:done

%DEVENV% "..\..\plugins\MyMetaPlugins.sln"
%DEVENV% "..\..\plugins\ZeusPlugins.sln"
set DEVENV=

COPY "..\..\plugins\Dnp.Utils\bin\release\Dnp.Utils.dll" .
COPY "..\..\plugins\MyMetaSqlCePlugin\bin\release\MyMeta.Plugins.SqlCe.dll" .
COPY "..\..\plugins\MyMetaTextFilePlugin\bin\release\MyMeta.Plugins.DelimitedText.dll" .
COPY "..\..\plugins\MyMetaXsd3bPlugin\bin\release\MyMeta.Plugins.Xsd3b.dll" .
COPY "..\..\plugins\MyMetaSybaseASEPlugin\bin\release\MyMeta.Plugins.SybaseASE.dll" .
COPY "..\..\plugins\MyMetaSybaseASAPlugin\bin\release\MyMeta.Plugins.SybaseASA.dll" .
COPY "..\..\plugins\MyMetaIngres2006Plugin\bin\release\MyMeta.Plugins.Ingres2006.dll" .
COPY "..\..\plugins\MyMetaEffiprozPlugin\bin\release\MyMeta.Plugins.EffiProz.dll" .
COPY "..\..\plugins\MyMetaFoxProPlugin\bin\release\MyMeta.Plugins.VisualFoxPro.dll" .
COPY "..\..\plugins\SampleUIPlugin\bin\release\MyGeneration.UI.Plugins.Sample.dll" .
COPY "..\..\plugins\MyGeneration.UI.Plugins.CodeSmith2MyGen\bin\release\MyGeneration.UI.Plugins.CodeSmith2MyGen.dll" .
COPY "..\..\plugins\MyGeneration.UI.Plugins.SqlTool\bin\release\MyGeneration.UI.Plugins.SqlTool.dll" .
rem COPY "..\..\plugins\MyMetaEffiprozPlugin\bin\release\MyMeta.dll" .
rem COPY "..\..\plugins\TypeSerializer\bin\release\*.dll" .
rem COPY "..\..\plugins\ContextProcessor\bin\release\ContextProcessor.dll" .
rem COPY "..\..\..\plugins\MyMetaVistaDB3xPlugin\bin\release\*.dll" .
