;--------------------------------------------------------
; Detects Microsoft .Net Framework 4
;--------------------------------------------------------
Function DotNet4Exists
	ClearErrors
	ReadRegStr $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
	IfErrors MDNFFullNotFound MDNFFound

	MDNFFullNotFound:
		ReadRegStr $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" "Version"
		IfErrors MDNFNotFound MDNFFound

	MDNFFound:
		Push 1
		Goto ExitFunction

	MDNFNotFound:
		Push 0
		Goto ExitFunction

	ExitFunction:
FunctionEnd

; detects MDAC 2.7
Function MDAC27Exists

	ClearErrors
	ReadRegStr $1 HKLM "SOFTWARE\Microsoft\DataAccess" "FullInstallVer"
	IfErrors MDACNotFound MDACFound

	MDACFound:
		StrCpy $2 $1 3

		StrCmp $2 "2.7" MDAC27Found
		StrCmp $2 "2.8" MDAC27Found
		StrCmp $2 "6.0" MDAC27Found
		StrCmp $2 "6.1" MDAC27Found
		StrCmp $2 "6.2" MDAC27Found
		Goto MDACNotFound
		
	MDAC27Found:
		Push 0
		Goto ExitFunction

	MDACNotFound:
		Push 1
		Goto ExitFunction
	ExitFunction:

FunctionEnd

; detects Microsoft Script Control
Function ScriptControlExists

	ClearErrors
	ReadRegStr $1 HKLM "SOFTWARE\Classes\CLSID\{0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC}" ""
	IfErrors MSCNotFound MSCFound

	MSCFound:
		Push 0
		Goto ExitFunction
		
	MSCNotFound:
		Push 1
		Goto ExitFunction

	ExitFunction:

FunctionEnd

; GetWindowsVersion
 ;
 ; Based on Yazno's function, http://yazno.tripod.com/powerpimpit/
 ; Updated by Joost Verburg
 ;
 ; Returns on top of stack
 ;
 ; Windows Version (95, 98, ME, NT x.x, 2000, XP, 2003, Vista)
 ; or
 ; '' (Unknown Windows Version)
 ;
 ; Usage:
 ;   Call GetWindowsVersion
 ;   Pop $R0
 ;   ; at this point $R0 is "NT 4.0" or whatnot
 
 Function GetWindowsVersion
 
   Push $R0
   Push $R1
 
   ClearErrors
 
   ReadRegStr $R0 HKLM \
   "SOFTWARE\Microsoft\Windows NT\CurrentVersion" CurrentVersion

   IfErrors 0 lbl_winnt
   
   ; we are not NT
   ReadRegStr $R0 HKLM \
   "SOFTWARE\Microsoft\Windows\CurrentVersion" VersionNumber
 
   StrCpy $R1 $R0 1
   StrCmp $R1 '4' 0 lbl_error
 
   StrCpy $R1 $R0 3
 
   StrCmp $R1 '4.0' lbl_win32_95
   StrCmp $R1 '4.9' lbl_win32_ME lbl_win32_98
 
   lbl_win32_95:
     StrCpy $R0 '95'
   Goto lbl_done
 
   lbl_win32_98:
     StrCpy $R0 '98'
   Goto lbl_done
 
   lbl_win32_ME:
     StrCpy $R0 'ME'
   Goto lbl_done
 
   lbl_winnt:
 
   StrCpy $R1 $R0 1
 
   StrCmp $R1 '3' lbl_winnt_x
   StrCmp $R1 '4' lbl_winnt_x
 
   StrCpy $R1 $R0 3
 
   StrCmp $R1 '5.0' lbl_winnt_2000
   StrCmp $R1 '5.1' lbl_winnt_XP
   StrCmp $R1 '5.2' lbl_winnt_2003
   StrCmp $R1 '6.0' lbl_winnt_vista lbl_error
 
   lbl_winnt_x:
     StrCpy $R0 "NT $R0" 6
   Goto lbl_done
 
   lbl_winnt_2000:
     Strcpy $R0 '2000'
   Goto lbl_done
 
   lbl_winnt_XP:
     Strcpy $R0 'XP'
   Goto lbl_done
 
   lbl_winnt_2003:
     Strcpy $R0 '2003'
   Goto lbl_done
 
   lbl_winnt_vista:
     Strcpy $R0 'Vista'
   Goto lbl_done
 
   lbl_error:
     Strcpy $R0 ''
   lbl_done:
 
   Pop $R1
   Exch $R0
 
 FunctionEnd
; eof
