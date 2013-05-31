;-----------------------------------------
; MyMeta Installation Script
;-----------------------------------------

!define DNF4_URL "http://download.microsoft.com/download/1/B/E/1BE39E79-7E39-46A3-96FF-047F95396215/dotNetFx40_Full_setup.exe"

; Include common functions for checking softwrae versions, etc
!include ".\common_functions.nsh"

; Set the compressions to lzma, which is always the best compression!
SetCompressor lzma 

; The name of the installer
Name "MyMeta"

; The file to write
OutFile "mymeta_installer.exe"

; Icon doesn't work for some reason
Icon ".\modern-install.ico"

XPStyle on

ShowInstDetails show

LicenseText "Liscence Agreement"
LicenseData "BSDLicense.rtf"

; The default installation directory
InstallDir $PROGRAMFILES\MyGenerations

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM SOFTWARE\MyGeneration "Install_Dir"

; The text to prompt the user to enter a directory
ComponentText "This will install the MyMeta Meta Data API."

; The text to prompt belithe user to enter a directory
DirText "Choose an install directory for MyMeta."

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

; The stuff to install
Section "Install Files and Reg Entries"

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ;Create Settings Directory 
  ;ExecShell mkdir $INSTDIR\Settings


  
  File /oname=adodb.dll ..\lib\thirdparty\adodb.dll
  File /oname=System.Data.SQLite.DLL ..\lib\thirdparty\System.Data.SQLite.DLL
  File /oname=Npgsql.dll ..\lib\thirdparty\Npgsql.dll
  File /oname=Mono.Security.dll ..\lib\thirdparty\Mono.Security.dll
  File /oname=FirebirdSql.Data.FirebirdClient.dll ..\lib\thirdparty\FirebirdSql.Data.FirebirdClient.dll
  File /oname=MySql.Data.dll ..\lib\thirdparty\MySql.Data.dll
  File /oname=EffiProz.dll ..\lib\thirdparty\EffiProz.dll
    
  File /nonfatal /oname=MyMeta.Plugins.DelimitedText.dll ..\plugins\MyMetaTextFilePlugin\bin\Release\MyMeta.Plugins.DelimitedText.dll
  File /nonfatal /oname=MyMeta.Plugins.SqlCe.dll ..\plugins\MyMetaSqlCePlugin\bin\Release\MyMeta.Plugins.SqlCe.dll
  File /nonfatal /oname=MyMeta.Plugins.SybaseASE.dll ..\plugins\MyMetaSybaseASEPlugin\bin\Release\MyMeta.Plugins.SybaseASE.dll
  File /nonfatal /oname=MyMeta.Plugins.SybaseASA.dll ..\plugins\MyMetaSybaseASAPlugin\bin\Release\MyMeta.Plugins.SybaseASA.dll
  File /nonfatal /oname=MyMeta.Plugins.Ingres2006.dll ..\plugins\MyMetaIngres2006Plugin\bin\Release\MyMeta.Plugins.Ingres2006.dll
  File /nonfatal /oname=MyMeta.Plugins.EffiProz.dll ..\plugins\MyMetaEffiProzPlugin\bin\Release\MyMeta.Plugins.EffiProz.dll
  File /nonfatal /oname=MyMeta.Plugins.VisualFoxPro.dll ..\plugins\MyMetaFoxProPlugin\bin\Release\MyMeta.Plugins.VisualFoxPro.dll

  File /oname=Interop.ADOX.dll ..\mymeta\bin\Release\Interop.ADOX.dll
  File /oname=MyMeta.dll ..\mymeta\bin\Release\MyMeta.dll
  File /oname=MyMeta.tlb ..\mymeta\bin\Release\MyMeta.tlb
  File /oname=MyMeta.chm ..\mymeta\MyMeta.chm

  CreateDirectory "$INSTDIR\Settings"
  
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
  File /oname=Settings\DbTargets.xml ..\mygeneration\MyGeneration\Settings\DbTargets.xml
  File /oname=Settings\Languages.xml ..\mygeneration\MyGeneration\Settings\Languages.xml
 
SectionEnd ; end the section

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

  ;WriteUninstaller "uninstall.exe"
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
  CreateDirectory "$SMPROGRAMS\MyGeneration"
  CreateShortCut "$SMPROGRAMS\MyGeneration\MyGeneration Website.lnk" "http://www.mygenerationsoftware.com/" "" "$INSTDIR\MyGeneration.exe" 0
  CreateShortCut "$SMPROGRAMS\MyGeneration\MyGeneration SourceForge Page.lnk" "http://sourceforge.net/projects/mygeneration/" "" "$INSTDIR\MyGeneration.exe" 0
  CreateShortCut "$SMPROGRAMS\MyGeneration\MyMeta Reference.lnk" "$INSTDIR\MyMeta.chm"
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
