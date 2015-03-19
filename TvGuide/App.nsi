SetCompressor /SOLID lzma

!addincludedir "D:\Dati\Dropbox\DocAnd\Risorse\NSIS"

!include "MUI2.nsh"
!include "CompilationInfo.nsh"


!define APPNAME "GuidaTV"
!define APPDISPNAME "Guida TV"
!define MAINEXE "GuidaTV.exe"
!define ICONSRC "Images\tv.ico"
!define APPDESC "Guida TV"
!define WEBSITE "http://at-my-window.blogspot.com/?page=tvguide"
!define AUTHOR "Andrea Martinelli"
Icon ${ICONSRC}


!include "Common.nsh"



!include "DownloadRequirements.nsh"



!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_TEXT "Launch ${APPDISPNAME}"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchApp"

!ifdef FullExe
    !insertmacro MUI_PAGE_WELCOME
!endif


!insertmacro MUI_PAGE_INSTFILES
!ifndef Standalone
	!insertmacro MUI_PAGE_FINISH
	!insertmacro MUI_UNPAGE_CONFIRM
	!insertmacro MUI_UNPAGE_INSTFILES
	!insertmacro MUI_UNPAGE_FINISH
!endif

!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Italian"


!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
;;;!insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
!insertmacro MUI_FUNCTION_DESCRIPTION_END




!insertmacro MUI_RESERVEFILE_LANGDLL





Function LaunchApp
	!ifdef Standalone
		Call UserLaunchApp
	!else
		!insertmacro UAC_AsUser_Call Function UserLaunchApp ${UAC_SYNCREGISTERS}
	!endif
FunctionEnd


Function UserLaunchApp
	Exec '"$INSTDIR\${MAINEXE}"'
FunctionEnd


!define RES "\GuidaTV.resources.dll"


Section ""
	SetOutPath "$INSTDIR"
	SetShellVarContext all
	
	!ifndef Standalone
		Call MaybeInstallXPServicePack3
		
		!ifdef FullExe
			Call MaybeInstallDotNetFx35
		!endif
	!endif
	
	${nsProcess::KillProcess} "GuidaTV.exe" $R0
	Sleep 500

	
	File "Dotfuscated\${MAINEXE}"
	File "Include\GuidaTV.exe.config"
	File "Include\Movies.dat"


	File /r "Dotfuscated\it"

	
	WriteRegStr HKCU "Software\antiufo\TvGuide" "LastAutomaticUpdateCheck" "1"
	
	!ifndef Standalone
		WriteUninstaller "$INSTDIR\Uninstall.exe"
		!insertmacro WriteUninstallReg 0x16ce
		!insertmacro CreateShortcuts
	!endif
	

	


	!ifdef Standalone
		Call UserLaunchApp
		Quit
	!endif
	

	
SectionEnd



Section "Uninstall"

	!insertmacro UninstallFeedback

	${nsProcess::KillProcess} "GuidaTV.exe" $R0
	
	!insertmacro DeleteShortcuts

	RMDir /r /REBOOTOK $INSTDIR
	!insertmacro DeleteUninstallReg
SectionEnd





