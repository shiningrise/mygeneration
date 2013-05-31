;-----------------------------------------
; MyGeneration Installation Script
; This version gets its dll-/exe-files from the 
;	local build paths below the src directories
;
; History
;	2007-08-12 installer will build with warning instead of fatalerror 
;				if plugins are not found
;			Fixed installer for xsd3b plugin
;-----------------------------------------

!define DNF4_URL "http://download.microsoft.com/download/1/B/E/1BE39E79-7E39-46A3-96FF-047F95396215/dotNetFx40_Full_setup.exe"

; Include common functions for checking softwrae versions, etc
!include ".\common_functions.nsh"

; Set the compressions to lzma, which is always the best compression!
SetCompressor lzma 

; The name of the installer
Name "MyGeneration 1.3"

; The file to write
OutFile "mygeneration_installer.exe"

; Icon doesn't work for some reason
Icon ".\modern-install.ico"

XPStyle on

ShowInstDetails show

LicenseText "Liscence Agreement"
LicenseData "BSDLicense.rtf"

; The default installation directory
InstallDir $PROGRAMFILES\MyGeneration13

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM SOFTWARE\MyGeneration13 "Install_Dir"

; The text to prompt the user to enter a directory
ComponentText "This will install the MyGeneration Code Generation Tool 1.3 on your computer. Select which optional things you want installed."

; The text to prompt belithe user to enter a directory
DirText "Choose an install directory for MyGeneration 1.3."

; Install .Net Framework 2.0
;Section "Detect .Net Framework 2.0"
;  Call DotNet20Exists
;  Pop $1
;  IntCmp $1 0 SkipFramework
;    MessageBox MB_OK|MB_ICONINFORMATION "You cannot run MyGeneration without having the .Net Framework 2.0 installed. It is not included $\r$\nin the installer because the file is huge and most people already have it installed." IDOK
;    ExecShell open http://www.microsoft.com/downloads/details.aspx?familyid=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5&displaylang=en
;    DetailPrint ".Net Framework 2.0 not installed... Aborting Installation."
;    Abort
;    Goto FrameworkDone
;	SkipFramework:
;		DetailPrint ".Net Framework 2.0 found... Continuing."
;	FrameworkDone:
;SectionEnd

;--------------------------------------------------------
; Download and install the .Net Framework 4
;--------------------------------------------------------
Section "-.Net Framework 4" net4_section_id
	Call DotNet4Exists
	Pop $1
	IntCmp $1 1 SkipDotNet4

	StrCpy $1 "dotNetFx40_Full_setup.exe"
	StrCpy $2 "$EXEDIR\$1"
	IfFileExists $2 FileExistsAlready FileMissing

	FileMissing:
		DetailPrint ".Net Framework 4 not installed... Downloading file."
		StrCpy $2 "$TEMP\$1"
		NSISdl::download "${DNF4_URL}" $2

	FileExistsAlready:
		DetailPrint "Installing the .Net Framework 4."
		;ExecWait '"$SYSDIR\msiexec.exe" "$2" /quiet'
		ExecWait '"$2" /quiet'

		Call DotNet4Exists
		Pop $1
		IntCmp $1 1 DotNet4Done DotNet4Failed

	DotNet4Failed:
		DetailPrint ".Net Framework 4 install failed... Aborting Install"
		MessageBox MB_OK ".Net Framework 4 install failed... Aborting Install"
		Abort

	SkipDotNet4:
		DetailPrint ".Net Framework 4 found... Continuing."

	DotNet4Done:
SectionEnd

; Install MDAC 2.7
Section "Detect MDAC 2.7+"

	Call GetWindowsVersion
	Pop $R0
	StrCmp $R0 "Vista" SkipVistaMDAC
	
	Call MDAC27Exists
	Pop $1
	IntCmp $1 0 SkipMDAC
		MessageBox MB_OK|MB_ICONINFORMATION "You cannot run MyGeneration without having MDAC 2.7+ installed. It is not included $\r$\nin the installer because the file is large and most people already have it installed." IDOK
		ExecShell open http://www.microsoft.com/downloads/details.aspx?FamilyID=6c050fe3-c795-4b7d-b037-185d0506396c&DisplayLang=en
		DetailPrint "MDAC 2.7+ not installed... Aborting Installation."
		Abort
		Goto MDACDone
	SkipVistaMDAC:
		DetailPrint "Vista doesn't need MDAC installed... Continuing."
		Goto MDACDone
	SkipMDAC:
		DetailPrint "MDAC 2.7+ found... Continuing."
	MDACDone:
SectionEnd

; Install Microsoft Script Control
Section "Detect/Install Microsoft Script Control"
	Call ScriptControlExists
	Pop $1
	IntCmp $1 0 SkipMSC
		GetTempFileName $R1
		File /oname=$R1 SCT10EN.EXE
		DetailPrint "Installing the Microsoft Script Control..."
		ExecWait "$R1 /Q"
		Delete $R1
		Goto MSCDone
	SkipMSC:
		DetailPrint "Microsoft Script Control found... Skipping install."
	MSCDone:
SectionEnd


; The stuff to install
Section "-Install Mygeneration and Register Shell Extensions"

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ;Create Settings Directory 
  ;ExecShell mkdir $INSTDIR\Settings

  ;unregister current MyMeta.dll if it exists
  IfFileExists "$INSTDIR\MyMeta.dll" 0 +2
  ExecWait `"$WINDIR\Microsoft.NET\Framework\v2.0.50727\regasm.exe" /u "$INSTDIR\MyMeta.dll" /tlb:MyMeta.tlb`
  
  ; delete some old assemblies
  IfFileExists "$INSTDIR\Settings\ZeusConfig.xml" 0 +3
    Rename $INSTDIR\Settings\ZeusConfig.xml $INSTDIR\Settings\ZeusConfig.xml.upgrade.backup
    Delete "$INSTDIR\Settings\ZeusConfig.xml"
  
  IfFileExists "$INSTDIR\DotNetScriptingEngine.dll" 0 +2
    Delete "$INSTDIR\DotNetScriptingEngine.dll"

  IfFileExists "$INSTDIR\MicrosoftScriptingEngine.dll" 0 +2
    Delete "$INSTDIR\MicrosoftScriptingEngine.dll"

  IfFileExists "$INSTDIR\ContextProcessor.dll" 0 +2
    Delete "$INSTDIR\ContextProcessor.dll"

  IfFileExists "$INSTDIR\MyWinformUI.dll" 0 +2
    Delete "$INSTDIR\MyWinformUI.dll"

  IfFileExists "$INSTDIR\TypeSerializer.dll" 0 +2
    Delete "$INSTDIR\TypeSerializer.dll"

  IfFileExists "$INSTDIR\Templates\Other\WinformDemo.vbgen" 0 +2
    Delete "$INSTDIR\Templates\Other\WinformDemo.vbgen"
    
  IfFileExists "$INSTDIR\FirebirdSql.Data.Firebird.dll" 0 +2
    Delete "$INSTDIR\FirebirdSql.Data.Firebird.dll"
  
  ; Get latest DLLs and EXE
  File /oname=ZeusCmd.exe ..\mygeneration\ZeusCmd\bin\Release\ZeusCmd.exe
  File /oname=MyGeneration.exe ..\mygeneration\MyGeneration\bin\Release\MyGeneration.exe
  File /oname=ZeusCmd.exe.config ..\mygeneration\ZeusCmd\bin\Release\ZeusCmd.exe.config
  File /oname=MyGeneration.exe.config ..\mygeneration\MyGeneration\bin\Release\MyGeneration.exe.config

  File /oname=Interop.ADOX.dll ..\mygeneration\MyGeneration\bin\Release\Interop.ADOX.dll
  File /oname=Interop.MSDASC.dll ..\mygeneration\MyGeneration\bin\Release\Interop.MSDASC.dll
  File /oname=Interop.MSScriptControl.dll ..\mygeneration\MyGeneration\bin\Release\Interop.MSScriptControl.dll
  File /oname=Interop.Scripting.dll ..\mygeneration\MyGeneration\bin\Release\Interop.Scripting.dll

  File /oname=adodb.dll ..\lib\thirdparty\adodb.dll
  File /oname=System.Data.SQLite.DLL ..\lib\thirdparty\System.Data.SQLite.DLL
  File /oname=Npgsql.dll ..\lib\thirdparty\Npgsql.dll
  File /oname=Mono.Security.dll ..\lib\thirdparty\Mono.Security.dll
  File /oname=FirebirdSql.Data.FirebirdClient.dll ..\lib\thirdparty\FirebirdSql.Data.FirebirdClient.dll
  File /oname=MySql.Data.dll ..\lib\thirdparty\MySql.Data.dll
  File /oname=EffiProz.dll ..\lib\thirdparty\EffiProz.dll
  File /oname=CollapsibleSplitter.dll ..\mygeneration\MyGeneration\bin\Release\CollapsibleSplitter.dll
  File /oname=ScintillaNET.dll ..\lib\thirdparty\ScintillaNET.dll
  File /oname=SciLexer.dll ..\lib\thirdparty\SciLexer.dll
  File /oname=WeifenLuo.WinFormsUI.Docking.dll ..\lib\thirdparty\WeifenLuo.WinFormsUI.Docking.dll

; Plugins nonfatal means create installer even if the filese do not exist
  File /nonfatal /oname=MyMeta.Plugins.DelimitedText.dll ..\plugins\MyMetaTextFilePlugin\bin\Release\MyMeta.Plugins.DelimitedText.dll
  File /nonfatal /oname=MyMeta.Plugins.SqlCe.dll ..\plugins\MyMetaSqlCePlugin\bin\Release\MyMeta.Plugins.SqlCe.dll
  File /nonfatal /oname=MyMeta.Plugins.SybaseASE.dll ..\plugins\MyMetaSybaseASEPlugin\bin\Release\MyMeta.Plugins.SybaseASE.dll
  File /nonfatal /oname=MyMeta.Plugins.SybaseASA.dll ..\plugins\MyMetaSybaseASAPlugin\bin\Release\MyMeta.Plugins.SybaseASA.dll
  File /nonfatal /oname=MyMeta.Plugins.Ingres2006.dll ..\plugins\MyMetaIngres2006Plugin\bin\Release\MyMeta.Plugins.Ingres2006.dll
  File /nonfatal /oname=MyMeta.Plugins.EffiProz.dll ..\plugins\MyMetaEffiProzPlugin\bin\Release\MyMeta.Plugins.EffiProz.dll
  File /nonfatal /oname=MyMeta.Plugins.VisualFoxPro.dll ..\plugins\MyMetaFoxProPlugin\bin\Release\MyMeta.Plugins.VisualFoxPro.dll
  File /nonfatal /oname=MyGeneration.UI.Plugins.SqlTool.dll ..\plugins\MyGeneration.UI.Plugins.SqlTool\bin\Release\MyGeneration.UI.Plugins.SqlTool.dll
  
  Delete $INSTDIR\WeifenLuo.WinFormsUI.dll
  Delete $INSTDIR\VistaDBHelper.dll
 
  File /oname=Zeus.dll ..\mygeneration\Zeus\bin\Release\Zeus.dll
  File /oname=PluginInterfaces.dll ..\mygeneration\EngineInterface\bin\Release\PluginInterfaces.dll
  File /oname=MyMeta.dll ..\mymeta\bin\Release\MyMeta.dll
  File /oname=MyMeta.tlb ..\mymeta\bin\Release\MyMeta.tlb
  File /oname=MyGenUtility.dll ..\mygeneration\MyGenUtility\bin\Release\MyGenUtility.dll
  
  File /oname=MyMeta.chm ..\mymeta\MyMeta.chm
  File /oname=dOOdads.chm ..\doodads\dOOdads.chm
  File /oname=Zeus.chm ..\mygeneration\Zeus\Zeus.chm
  File /oname=MyGeneration.chm .\MyGeneration.chm
  
  File /oname=todo.txt .\todo.txt
  File /oname=changelog.txt .\changelog.txt
  
  File /oname=UnregisterMyMeta12.reg .\UnregisterMyMeta12.reg
  File /oname=UnregisterMyMeta13.reg .\UnregisterMyMeta13.reg
  File /oname=RegisterMyMeta.bat .\RegisterMyMeta.bat

  File /oname=MyGeneration.ico ..\mygeneration\MyGeneration\Icons\MainWindow.ico
  File /oname=ZeusProject.ico ..\mygeneration\MyGeneration\Icons\NewZeus.ico
  
  ;Install DnpUtils
  File /oname=Dnp.Utils.chm ..\plugins\Dnp.Utils\Dnp.Utils.chm
  File /oname=Dnp.Utils.dll ..\plugins\Dnp.Utils\bin\Release\Dnp.Utils.dll
  ExecWait `"$INSTDIR\ZeusCmd.exe" -aio "%ZEUSHOME%\Dnp.Utils.dll" "Dnp.Utils.Utils" "DnpUtils"`
  
  ; Create Folders
  CreateDirectory "$INSTDIR\Templates"
  CreateDirectory "$INSTDIR\Settings"
  CreateDirectory "$INSTDIR\GeneratedCode"
  CreateDirectory "$INSTDIR\Architectures"
  
  ; Create Architecture Sub-Folders
  ;CreateDirectory "$INSTDIR\Architectures\MyGenPHP"
  CreateDirectory "$INSTDIR\Architectures\dOOdads"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\dOOdad_Demo"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\dOOdad_Demo\Generated"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\MyGeneration.dOOdads"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\"  
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\dOOdad_Demo"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\dOOdad_Demo\Generated"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\MyGeneration.dOOdads"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\"
  
  ; Create Template Sub-Folders
  CreateDirectory "$INSTDIR\Templates\Microsoft_Access"
  CreateDirectory "$INSTDIR\Templates\Microsoft_SQL_Server"
  CreateDirectory "$INSTDIR\Templates\VB.Net"
  CreateDirectory "$INSTDIR\Templates\C#"
  CreateDirectory "$INSTDIR\Templates\PHP"
  CreateDirectory "$INSTDIR\Templates\Other"
  CreateDirectory "$INSTDIR\Templates\IBM_DB2"
  CreateDirectory "$INSTDIR\Templates\MySQL"
  CreateDirectory "$INSTDIR\Templates\Oracle"
  CreateDirectory "$INSTDIR\Templates\Java"
  CreateDirectory "$INSTDIR\Templates\HTML"
  CreateDirectory "$INSTDIR\Templates\Firebird"
  CreateDirectory "$INSTDIR\Templates\GentleNET"
  CreateDirectory "$INSTDIR\Templates\VistaDB"
  CreateDirectory "$INSTDIR\Templates\Tutorials"
  CreateDirectory "$INSTDIR\Templates\Samples"
  CreateDirectory "$INSTDIR\Templates\PostgreSQL"
  CreateDirectory "$INSTDIR\Templates\MySQL"
  CreateDirectory "$INSTDIR\Templates\SQLite"
  CreateDirectory "$INSTDIR\Templates\IBM_ISeries"


  ; Copy the architecture files into the Architectures folder
  ; PHP
  ;File /oname=Architectures\MyGenPHP\mygen_mysql.php ..\..\..\mygenphp\mygen_mysql.php
  ;File /oname=Architectures\MyGenPHP\mygen_postgres7.php ..\..\..\mygenphp\mygen_postgres7.php
  ;File /oname=Architectures\MyGenPHP\mygen_msaccess.php ..\..\..\mygenphp\mygen_msaccess.php
  ;File /oname=Architectures\MyGenPHP\mygen_mssql-odbc.php ..\..\..\mygenphp\mygen_mssql-odbc.php
  ;File /oname=Architectures\MyGenPHP\mygen_framework.php ..\..\..\mygenphp\mygen_framework.php

  ; dOOdads
  ; VB.NET Demo
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\README.TXT ..\doodads\VB.NET\dOOdad_Demo\README.TXT
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\About.resx ..\doodads\VB.NET\dOOdad_Demo\About.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\About.vb ..\doodads\VB.NET\dOOdad_Demo\About.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\App.config ..\doodads\VB.NET\dOOdad_Demo\App.config
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\AssemblyInfo.vb ..\doodads\VB.NET\dOOdad_Demo\AssemblyInfo.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo.sln ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo.sln
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.sln ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.sln  
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo.vbproj ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo.vbproj
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.vbproj ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.vbproj  
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Employees.vb ..\doodads\VB.NET\dOOdad_Demo\Employees.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\EmployeesEdit.resx ..\doodads\VB.NET\dOOdad_Demo\EmployeesEdit.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\EmployeesEdit.vb ..\doodads\VB.NET\dOOdad_Demo\EmployeesEdit.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\FillComboBox.resx ..\doodads\VB.NET\dOOdad_Demo\FillComboBox.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\FillComboBox.vb ..\doodads\VB.NET\dOOdad_Demo\FillComboBox.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Form1.resx ..\doodads\VB.NET\dOOdad_Demo\Form1.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Form1.vb ..\doodads\VB.NET\dOOdad_Demo\Form1.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Invoices.vb ..\doodads\VB.NET\dOOdad_Demo\Invoices.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Products.vb ..\doodads\VB.NET\dOOdad_Demo\Products.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\TheMasterSample.vb ..\doodads\VB.NET\dOOdad_Demo\TheMasterSample.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Generated\_Employees.vb ..\doodads\VB.NET\dOOdad_Demo\Generated\_Employees.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Generated\_Products.vb ..\doodads\VB.NET\dOOdad_Demo\Generated\_Products.vb
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Application.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Application.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Application.myapp" "..\doodads\VB.NET\dOOdad_Demo\My Project\Application.myapp"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Application1.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Application1.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Resources.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Resources.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Resources.resx" "..\doodads\VB.NET\dOOdad_Demo\My Project\Resources.resx"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Settings.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Settings.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Settings.settings" "..\doodads\VB.NET\dOOdad_Demo\My Project\Settings.settings"

  ; VB.NET dOOdads
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\README.TXT ..\doodads\VB.NET\MyGeneration.dOOdads\README.TXT
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\AssemblyInfo.vb ..\doodads\VB.NET\MyGeneration.dOOdads\AssemblyInfo.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\BusinessEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\BusinessEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.vbproj ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.vbproj
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.vbproj ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.vbproj  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\TransactionMgr.vb ..\doodads\VB.NET\MyGeneration.dOOdads\TransactionMgr.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\WhereParameter.vb ..\doodads\VB.NET\MyGeneration.dOOdads\WhereParameter.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\AggregateParameter.vb ..\doodads\VB.NET\MyGeneration.dOOdads\AggregateParameter.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.sln ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.sln
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4Entity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4Entity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.vb
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Application.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Application.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Application.myapp" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Application.myapp"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Application1.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Application1.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Resources.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Resources.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Resources.resx" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Resources.resx"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Settings.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Settings.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Settings.settings" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Settings.settings"
									
  ; CSharp Demo
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\README.TXT ..\doodads\CSharp\dOOdad_Demo\README.TXT
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\App.ico ..\doodads\CSharp\dOOdad_Demo\App.ico
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\About.resx ..\doodads\CSharp\dOOdad_Demo\About.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\About.cs ..\doodads\CSharp\dOOdad_Demo\About.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\App.config ..\doodads\CSharp\dOOdad_Demo\App.config
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\AssemblyInfo.cs ..\doodads\CSharp\dOOdad_Demo\AssemblyInfo.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo.sln ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo.sln
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.sln ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.sln  
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo.csproj ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo.csproj
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.csproj ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.csproj  
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Employees.cs ..\doodads\CSharp\dOOdad_Demo\Employees.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\EmployeesEdit.resx ..\doodads\CSharp\dOOdad_Demo\EmployeesEdit.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\EmployeesEdit.cs ..\doodads\CSharp\dOOdad_Demo\EmployeesEdit.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\FillComboBox.resx ..\doodads\CSharp\dOOdad_Demo\FillComboBox.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\FillComboBox.cs ..\doodads\CSharp\dOOdad_Demo\FillComboBox.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Form1.resx ..\doodads\CSharp\dOOdad_Demo\Form1.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Form1.cs ..\doodads\CSharp\dOOdad_Demo\Form1.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Invoices.cs ..\doodads\CSharp\dOOdad_Demo\Invoices.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Products.cs ..\doodads\CSharp\dOOdad_Demo\Products.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\TheMasterSample.cs ..\doodads\CSharp\dOOdad_Demo\TheMasterSample.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Generated\_Employees.cs ..\doodads\CSharp\dOOdad_Demo\Generated\_Employees.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Generated\_Products.cs ..\doodads\CSharp\dOOdad_Demo\Generated\_Products.cs

    

  ; CSharp dOOdads
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\README.TXT ..\doodads\CSharp\MyGeneration.dOOdads\README.TXT
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\AssemblyInfo.cs ..\doodads\CSharp\MyGeneration.dOOdads\AssemblyInfo.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\BusinessEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\BusinessEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.csproj ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.csproj
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.csproj ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.csproj  
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx   
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\TransactionMgr.cs ..\doodads\CSharp\MyGeneration.dOOdads\TransactionMgr.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\WhereParameter.cs ..\doodads\CSharp\MyGeneration.dOOdads\WhereParameter.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\AggregateParameter.cs ..\doodads\CSharp\MyGeneration.dOOdads\AggregateParameter.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.sln ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.sln
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln  
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx   
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4Entity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4Entity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.cs


  ; Copy the template files into the Templates folder
  File /oname=Templates\Microsoft_Access\Access_StoredProcs.vbgen ..\templates\Microsoft_Access\Access_StoredProcs.vbgen
  File /oname=Templates\Microsoft_SQL_Server\SQL_DataInserts.jgen ..\templates\Microsoft_SQL_Server\SQL_DataInserts.jgen
  File /oname=Templates\Microsoft_SQL_Server\SQL_DataReplication.jgen ..\templates\Microsoft_SQL_Server\SQL_DataReplication.jgen
  File /oname=Templates\Microsoft_SQL_Server\SQL_StoredProcs.jgen ..\templates\Microsoft_SQL_Server\SQL_StoredProcs.jgen
  File /oname=Templates\Microsoft_SQL_Server\SQL_StoredProcs.vbgen ..\templates\Microsoft_SQL_Server\SQL_StoredProcs.vbgen
  File /oname=Templates\Microsoft_SQL_Server\SQL_DeleteAllData.jgen ..\templates\Microsoft_SQL_Server\SQL_DeleteAllData.jgen
  File /oname=Templates\Microsoft_SQL_Server\sql_library.js ..\templates\Microsoft_SQL_Server\sql_library.js
  File /oname=Templates\IBM_ISeries\iSeries_StoredProcs.vbgen ..\templates\IBM_ISeries\iSeries_StoredProcs.vbgen
  File /oname=Templates\Oracle\Oracle_StoredProcs.vbgen ..\templates\Oracle\Oracle_StoredProcs.vbgen
  File /oname=Templates\VB.Net\VbNet_SQL_BusinessObject.vbgen ..\templates\VB.Net\VbNet_SQL_BusinessObject.vbgen
  File /oname=Templates\VB.Net\VbNet_Access_BusinessObject.vbgen ..\templates\VB.Net\VbNet_Access_BusinessObject.vbgen
  File /oname=Templates\VB.Net\VbNet_SQL_dOOdads_BusinessEntity.vbgen ..\templates\VB.Net\VbNet_SQL_dOOdads_BusinessEntity.vbgen
  File /oname=Templates\VB.Net\VbNet_SQL_dOOdads_View.vbgen ..\templates\VB.Net\VbNet_SQL_dOOdads_View.vbgen
  File /oname=Templates\VB.Net\VBNet_SQL_dOOdads_ConcreteClass.vbgen ..\templates\VB.Net\VBNet_SQL_dOOdads_ConcreteClass.vbgen
  File "/oname=Templates\C#\CSharp_SQL_BusinessObject.vbgen" "..\templates\C#\CSharp_SQL_BusinessObject.vbgen"
  File "/oname=Templates\C#\CSharp_Access_BusinessObject.vbgen" "..\templates\C#\CSharp_Access_BusinessObject.vbgen"
  File "/oname=Templates\C#\CSharp_SQL_dOOdads_BusinessEntity.vbgen" "..\templates\C#\CSharp_SQL_dOOdads_BusinessEntity.vbgen"
  File "/oname=Templates\C#\CSharp_SQL_dOOdads_View.vbgen" "..\templates\C#\CSharp_SQL_dOOdads_View.vbgen"
  File "/oname=Templates\C#\CSharp_dOOdads_StoredProc.vbgen" "..\templates\C#\CSharp_dOOdads_StoredProc.vbgen"
  File "/oname=Templates\C#\CSharp_SQL_dOOdads_ConcreteClass.vbgen" "..\templates\C#\CSharp_SQL_dOOdads_ConcreteClass.vbgen"
  File /oname=Templates\PHP\PHP_BusinessObject.jgen ..\templates\PHP\PHP_BusinessObject.jgen
  File /oname=Templates\Other\TemplateGroupExample.jgen ..\templates\Other\TemplateGroupExample.jgen
  File /oname=Templates\Other\UserMetaData.vbgen ..\templates\Other\UserMetaData.vbgen
  File /oname=Templates\Other\UserMetaData.jgen ..\templates\Other\UserMetaData.jgen
  ;File /oname=Templates\Other\WinformDemo.vbgen ..\templates\Other\WinformDemo.vbgen
  File /oname=Templates\HTML\HTML_DatabaseReport.csgen ..\templates\HTML\HTML_DatabaseReport.csgen
  File /oname=Templates\HTML\HTML_TableDefinition.vbgen ..\templates\HTML\HTML_TableDefinition.vbgen
  File /oname=Templates\Firebird\FirebirdStoredProcs.vbgen ..\templates\Firebird\FirebirdStoredProcs.vbgen
  File /oname=Templates\GentleNET\BusinessEntity.csgen ..\templates\Gentle.NET\BusinessEntity.csgen
  File /oname=Templates\VistaDB\VistaDB_CSharp_BusinessEntity.vbgen ..\templates\VistaDB\VistaDB_CSharp_BusinessEntity.vbgen
  File /oname=Templates\VistaDB\VistaDB_VBNet_BusinessEntity.vbgen ..\templates\VistaDB\VistaDB_VBNet_BusinessEntity.vbgen
  File /oname=Templates\MySQL\MySQL4_CSharp_BusinessEntity.vbgen ..\templates\MySql\MySQL4_CSharp_BusinessEntity.vbgen
  File /oname=Templates\MySQL\MySQL4_VBNet_BusinessEntity.vbgen ..\templates\MySql\MySQL4_VBNet_BusinessEntity.vbgen
  File /oname=Templates\MySQL\MySQL4_CSharp_BusinessView.vbgen ..\templates\MySql\MySQL4_CSharp_BusinessView.vbgen
  File /oname=Templates\MySQL\MySQL4_VBNet_BusinessView.vbgen ..\templates\MySql\MySQL4_VBNet_BusinessView.vbgen  
  File /oname=Templates\SQLite\SQLite_CSharp_BusinessEntity.vbgen ..\templates\SQLite\SQLite_CSharp_BusinessEntity.vbgen
  File /oname=Templates\SQLite\SQLite_VBNet_BusinessEntity.vbgen ..\templates\SQLite\SQLite_VBNet_BusinessEntity.vbgen
  File /oname=Templates\SQLite\SQLite_CSharp_BusinessView.vbgen ..\templates\SQLite\SQLite_CSharp_BusinessView.vbgen
  File /oname=Templates\SQLite\SQLite_VBNet_BusinessView.vbgen ..\templates\SQLite\SQLite_VBNet_BusinessView.vbgen

  File /oname=Templates\Tutorials\Chapter1(VBScript).zeus ..\templates\Tutorials\Chapter1(VBScript).zeus
  File /oname=Templates\Tutorials\Chapter1(JScript).zeus ..\templates\Tutorials\Chapter1(JScript).zeus
  File /oname=Templates\Tutorials\Chapter1(VB.NET).zeus ..\templates\Tutorials\Chapter1(VB.NET).zeus
  File "/oname=Templates\Tutorials\Chapter1(C#).zeus" "..\templates\Tutorials\Chapter1(C#).zeus"
  File /oname=Templates\Tutorials\Chapter2(VBScript).zeus ..\templates\Tutorials\Chapter2(VBScript).zeus
  File /oname=Templates\Tutorials\Chapter2(JScript).zeus ..\templates\Tutorials\Chapter2(JScript).zeus
  File /oname=Templates\Tutorials\Chapter2(VB.NET).zeus ..\templates\Tutorials\Chapter2(VB.NET).zeus
  File "/oname=Templates\Tutorials\Chapter2(C#).zeus" "..\templates\Tutorials\Chapter2(C#).zeus"

  File /oname=Templates\PostgreSQL\PostgreSQL_StoredProcs.vbgen ..\templates\PostgreSQL\PostgreSQL_StoredProcs.vbgen
  File "/oname=Templates\Samples\GuiTest.zeus" "..\templates\Samples\GuiTest.zeus"

  Delete Templates\Firebird\FirebirdStoredProcs_Dialect1.vbgen
  Delete Templates\Firebird\FirebirdStoredProcs_Dialect3.vbgen

  ;Rename file if it already exists
  Delete Settings\Languages.xml.4.old
  Rename Settings\Languages.xml.3.old Settings\Languages.xml.4.old
  Rename Settings\Languages.xml.2.old Settings\Languages.xml.3.old
  Rename Settings\Languages.xml.1.old Settings\Languages.xml.2.old
  Rename Settings\Languages.xml Settings\Languages.xml.1.old

  ;Rename file if it already exists
  Delete Settings\DbTargets.xml.4.old
  Rename Settings\DbTargets.xml.3.old Settings\DbTargets.xml.xml.4.old
  Rename Settings\DbTargets.xml.2.old Settings\DbTargets.xml.3.old
  Rename Settings\DbTargets.xml.1.old Settings\DbTargets.xml.2.old
  Rename Settings\DbTargets.xml Settings\DbTargets.xml.1.old

  ; Copy the config files into the Settings folder
  ;Delete $INSTDIR\DockManager.config
  File /oname=Settings\DbTargets.xml ..\mygeneration\MyGeneration\Settings\DbTargets.xml
  File /oname=Settings\Languages.xml ..\mygeneration\MyGeneration\Settings\Languages.xml
  File /oname=Settings\MyGeneration.xml ..\mygeneration\MyGeneration\Settings\MyGeneration.xml
  File /oname=Settings\ScintillaNET.xml ..\mygeneration\MyGeneration\Settings\ScintillaNET.xml
 
  ; Delete the old config files
  Delete Settings\ZeusScriptingEngines.zcfg
  Delete Settings\ZeusScriptingObjects.zcfg

  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\MyGeneration13 "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MyGeneration13" "DisplayName" "MyGeneration 1.3 (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MyGeneration13" "UninstallString" '"$INSTDIR\uninstall.exe"'

  ; MyGeneration Development Shell Extensions - JGEN
  WriteRegStr HKCR ".jgen" "" "JGenMyGenFile"
  WriteRegStr HKCR "JGenMyGenFile" "" "JGen Template File"
  WriteRegStr HKCR "JGenMyGenFile\shell" "" "open"
  WriteRegStr HKCR "JGenMyGenFile\DefaultIcon" "" $INSTDIR\MyGeneration.ico
  WriteRegStr HKCR "JGenMyGenFile\shell\open\command" "" '"$INSTDIR\MyGeneration.exe" "%1"'

  ; MyGeneration Development Shell Extensions - VBGEN
  WriteRegStr HKCR ".vbgen" "" "VBGenMyGenFile"
  WriteRegStr HKCR "VBGenMyGenFile" "" "VBGen Template File"
  WriteRegStr HKCR "VBGenMyGenFile\shell" "" "open"
  WriteRegStr HKCR "VBGenMyGenFile\DefaultIcon" "" $INSTDIR\MyGeneration.ico
  WriteRegStr HKCR "VBGenMyGenFile\shell\open\command" "" '"$INSTDIR\MyGeneration.exe" "%1"'

  ; MyGeneration Development Shell Extensions - CSGEN
  WriteRegStr HKCR ".csgen" "" "CSGenMyGenFile"
  WriteRegStr HKCR "CSGenMyGenFile" "" "CSGen Template File"
  WriteRegStr HKCR "CSGenMyGenFile\shell" "" "open"
  WriteRegStr HKCR "CSGenMyGenFile\DefaultIcon" "" $INSTDIR\MyGeneration.ico
  WriteRegStr HKCR "CSGenMyGenFile\shell\open\command" "" '"$INSTDIR\MyGeneration.exe" "%1"'

  ; MyGeneration Development Shell Extensions - ZEUS
  WriteRegStr HKCR ".zeus" "" "ZeusMyGenFile"
  WriteRegStr HKCR "ZeusMyGenFile" "" "Zeus Template File"
  WriteRegStr HKCR "ZeusMyGenFile\shell" "" "open"
  WriteRegStr HKCR "ZeusMyGenFile\DefaultIcon" "" $INSTDIR\MyGeneration.ico
  WriteRegStr HKCR "ZeusMyGenFile\shell\open\command" "" '"$INSTDIR\MyGeneration.exe" "%1"'

  ; MyGeneration Development Shell Extensions - ZINP
  WriteRegStr HKCR ".zinp" "" "ZeusInputMyGenFile"
  WriteRegStr HKCR "ZeusInputMyGenFile" "" "MyGeneration Input File"
  WriteRegStr HKCR "ZeusInputMyGenFile\DefaultIcon" "" $INSTDIR\MyGeneration.ico

  ; MyGeneration Development Shell Extensions - ZPRJ
  WriteRegStr HKCR ".zprj" "" "ProjectMyGenFile"
  WriteRegStr HKCR "ProjectMyGenFile" "" "MyGeneration Project File"
  WriteRegStr HKCR "ProjectMyGenFile\shell" "" "open"
  WriteRegStr HKCR "ProjectMyGenFile\DefaultIcon" "" $INSTDIR\ZeusProject.ico
  WriteRegStr HKCR "ProjectMyGenFile\shell\open\command" "" '"$INSTDIR\MyGeneration.exe" "%1"'

  ; MyGeneration Development Shell Extensions - ZPRJ
  WriteRegStr HKCR ".zprjusr" "" "ProjectMyGenFile"
  WriteRegStr HKCR "ProjectMyGenFile" "" "MyGeneration Project (User) File"
  ;WriteRegStr HKCR "ProjectMyGenFile\shell" "" "open"
  WriteRegStr HKCR "ProjectMyGenFile\DefaultIcon" "" $INSTDIR\ZeusProject.ico
  ;WriteRegStr HKCR "ProjectMyGenFile\shell\open\command" "" '"$INSTDIR\MyGeneration.exe" "%1"'
  

  WriteUninstaller "uninstall.exe"

SectionEnd ; end the section

; *** We will just have to add this in later, there are too many bugs ***
;Section /o "Visual Studio 2005 Add-In"
  ; Set output path to the installation directory.
  ;SetOutPath $INSTDIR
    
  ;File /nonfatal ..\ideplugins\visualstudio2005\MyGenVS2005\MyGenVS2005.AddIn
  ;File /nonfatal ..\ideplugins\visualstudio2005\MyGenVS2005\bin\MyGenVS2005.dll

  ;ExecWait `"$INSTDIR\ZeusCmd.exe" -installvs2005`
  
;SectionEnd ; end the section


Section "Install Xsd3b Provider for xml (xsd, uml, entityrelationship)"
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
    
  File /nonfatal /oname=MyMeta.Plugins.Xsd3b.dll ..\plugins\MyMetaXsd3bPlugin\bin\Release\MyMeta.Plugins.Xsd3b.dll
  File /nonfatal ..\lib\thirdparty\Dl3bak.*.dll
  File /nonfatal .\*xsd3b*.chm

  SetOutPath "$INSTDIR\Templates\Xsd3b"
  ; CreateDirectory "$INSTDIR\Templates\Xsd3b"
  
  ; File /oname=Templates\Xsd3b\ToXsd3b.csgen ..\Templates\Xsd3b\ToXsd3b.csgen
  ; File /oname=Templates\Xsd3b\ToXsd.csgen ..\Templates\Xsd3b\ToXsd.csgen
  ; File ..\Templates\Xsd3b\*.*
  File ..\plugins\MyMetaXsd3bPlugin\templates\xsd3b\*.*
  
  SetOutPath $INSTDIR

  WriteUninstaller "uninstall.exe"
SectionEnd ; end the section

Section "MSDTC Reset Log (sometimes needed)"

    DetailPrint "Resetting the MSDTC Log"
    ExecWait `"$WINDIR\system32\msdtc.exe" -resetlog`
SectionEnd

; Register  MyMeta DLL
Section "Register MyMeta Assembly"

    DetailPrint "Register the MyMeta DLL into the Global Assembly Cache"
    ;ExecWait `"$WINDIR\Microsoft.Net\Framework\v1.1.4322\regasm.exe" "$INSTDIR\MyMeta.dll" /codebase`
    ExecWait `"$WINDIR\Microsoft.NET\Framework\v2.0.50727\regasm.exe" "$INSTDIR\MyMeta.dll" /tlb:MyMeta.tlb`
SectionEnd

; optional section
Section "Start Menu Shortcuts"
  CreateDirectory "$SMPROGRAMS\MyGeneration 1.3"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\MyGeneration.lnk" "$INSTDIR\MyGeneration.exe" "" "$INSTDIR\MyGeneration.exe" 0
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\MyGeneration Website.lnk" "http://www.mygenerationsoftware.com/" "" "$INSTDIR\MyGeneration.exe" 0
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\MyGeneration SourceForge Page.lnk" "http://sourceforge.net/projects/mygeneration/" "" "$INSTDIR\MyGeneration.exe" 0
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\MyGeneration Help.lnk" "$INSTDIR\MyGeneration.chm"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\MyMeta Reference.lnk" "$INSTDIR\MyMeta.chm"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\dOOdads Reference.lnk" "$INSTDIR\dOOdads.chm"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\Zeus Reference.lnk" "$INSTDIR\Zeus.chm"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\DNP Utils Reference.lnk" "$INSTDIR\Dnp.Utils.chm"
SectionEnd

; Launch Program After Install on section select
;Section /o "Launch Program After Install"
;  ExecShell open "$INSTDIR\MyGeneration.exe" SW_SHOWNORMAL
;SectionEnd

; Launch Program After Install with messagebox
Section
  MessageBox MB_YESNO|MB_ICONQUESTION "Launch MyGeneration?" IDNO DontLaunchThingy
  	ExecShell open "$INSTDIR\MyGeneration.exe" SW_SHOWNORMAL
  	Quit
  DontLaunchThingy:
SectionEnd


; uninstall stuff
UninstallText "This will uninstall the MyGeneration Code Generator. Hit next to continue."
UninstallIcon ".\modern-uninstall.ico"

; special uninstall section.
Section "Uninstall"
    
  IfFileExists "$INSTDIR\MyMeta.dll" 0 +2
	ExecWait `"$WINDIR\Microsoft.NET\Framework\v2.0.50727\regasm.exe" /u "$INSTDIR\MyMeta.dll" /tlb:MyMeta.tlb`
  
  ; remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MyGeneration13"
  DeleteRegKey HKLM SOFTWARE\MyGeneration13
  DeleteRegKey HKCR ".vbgen"
  DeleteRegKey HKCR ".jgen"
  DeleteRegKey HKCR ".zeus"
  DeleteRegKey HKCR ".csgen"
  DeleteRegKey HKCR "JGenMyGenFile"
  DeleteRegKey HKCR "CSGenMyGenFile"
  DeleteRegKey HKCR "VBGenMyGenFile"
  DeleteRegKey HKCR "ZeusMyGenFile"

  ; MUST REMOVE UNINSTALLER, too
  Delete $INSTDIR\uninstall.exe
 
  ; remove shortcuts, if any.
  Delete "$SMPROGRAMS\MyGeneration 1.3\*.*"
  
  ; remove directories used.
  RMDir "$SMPROGRAMS\MyGeneration 1.3"
  
  ;RMDir /r "$INSTDIR"
  Delete $INSTDIR\*.exe
  Delete $INSTDIR\*.dll
  
  ;get rid of new config files, but back them up first.
  Rename $INSTDIR\Settings\Languages.xml $INSTDIR\Settings\Languages.xml.downgrade.backup
  Delete $INSTDIR\Settings\Languages.xml
  
  Rename $INSTDIR\Settings\DbTargets.xml $INSTDIR\Settings\DbTargets.xml.downgrade.backup
  Delete $INSTDIR\Settings\DbTargets.xml
  
  Rename $INSTDIR\Settings\MyGeneration.xml $INSTDIR\Settings\MyGeneration.xml.downgrade.backup
  Delete $INSTDIR\Settings\MyGeneration.xml
  
  Rename $INSTDIR\Settings\ZeusConfig.xml $INSTDIR\Settings\ZeusConfig.xml.downgrade.backup
  Delete $INSTDIR\Settings\ZeusConfig.xml
  
  Rename $INSTDIR\Settings\DefaultSettings.xml $INSTDIR\Settings\DefaultSettings.xml.downgrade.backup
  Delete $INSTDIR\Settings\DefaultSettings.xml
  
  Rename $INSTDIR\Settings\ScintillaNET.xml $INSTDIR\Settings\ScintillaNET.xml.downgrade.backup
  Delete $INSTDIR\Settings\ScintillaNET.xml
  
SectionEnd

; functions defined here:
Function .onInit

    SetOutPath $TEMP
    File /oname=spltmp.bmp "logo.bmp"
    File /oname=spltmp.wav "start.wav"

    advsplash::show 1600 600 600 -1 $TEMP\spltmp

    Pop $0 ; $0 has '1' if the user closed the splash screen early,
           ; '0' if everything closed normal, and '-1' if some error occured.

    Delete $TEMP\spltmp.bmp
    Delete $TEMP\spltmp.wav

    Return
FunctionEnd
