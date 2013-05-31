; CodeSmith2MyGeneration 0.1 Beta Installation Script
;-----------------------------------------

; Include common functions for checking softwrae versions, etc
!include ".\common_functions.nsh"

; Set the compressions to lzma, which is always the best compression!
SetCompressor lzma 

; The name of the installer
Name "CodeSmith2MyGeneration Convertion Tool Plugin 0.1 Beta"

; The file to write
OutFile "mygen_plugin_cst2mygen010b.exe"

; Icon doesn't work for some reason
Icon ".\modern-install.ico"

XPStyle on

ShowInstDetails show

LicenseText "Liscence Agreement"
LicenseData "BSDLicense.rtf"


; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM SOFTWARE\CodeSmith2MyGeneration "Install_Dir"

; The text to prompt the user to enter a directory
ComponentText "This will install the CodeSmith2MyGeneration on your computer. Select which optional things you want installed."

; The text to prompt the user to enter a directory
;DirText "Choose an install directory for CodeSmith2MyGeneration."

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

; The stuff to install
Section "Install Files, Reg Entries, Uninstaller, & Shortcuts"

  ReadRegStr $0 HKLM Software\MyGeneration13 "Install_Dir"
  DetailPrint "MyGeneration is installed at: $0"
  StrCpy $INSTDIR $0

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  CreateDirectory "$INSTDIR"

  File /oname=MyGeneration.UI.Plugins.CodeSmith2MyGen.dll ..\plugins\MyGeneration.UI.Plugins.CodeSmith2MyGen\bin\Release\MyGeneration.UI.Plugins.CodeSmith2MyGen.dll

  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\CodeSmith2MyGeneration "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CodeSmith2MyGeneration" "DisplayName" "CodeSmith2MyGeneration Plugin (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CodeSmith2MyGeneration" "UninstallString" '"$INSTDIR\CodeSmith2MyGenUninstall.exe"'

  WriteUninstaller "CodeSmith2MyGenUninstall.exe"
  
  CreateDirectory "$SMPROGRAMS\MyGeneration 1.3"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\Uninstall CodeSmith2MyGen.lnk" "$INSTDIR\CodeSmith2MyGenUninstall.exe" "" "$INSTDIR\CodeSmith2MyGenUninstall.exe" 0

SectionEnd ; end the section

; uninstall stuff
UninstallText "This will uninstall the CodeSmith2MyGeneration conversion tool plugin. Hit next to continue."
UninstallIcon ".\modern-uninstall.ico"

; special uninstall section.
Section "Uninstall"
    
  ; remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\CodeSmith2MyGeneration"
  DeleteRegKey HKLM SOFTWARE\CodeSmith2MyGeneration

  ; MUST REMOVE UNINSTALLER, too
  Delete $INSTDIR\CodeSmith2MyGenUninstall.exe
  Delete $INSTDIR\MyGeneration.UI.Plugins.CodeSmith2MyGen.dll
   
  Delete "$SMPROGRAMS\MyGeneration 1.3\Uninstall CodeSmith2MyGen.lnk"
SectionEnd

