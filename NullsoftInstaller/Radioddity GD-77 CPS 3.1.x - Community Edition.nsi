############################################################################################
#      NSIS Installation Script created by NSIS Quick Setup Script Generator v1.09.18
#               Entirely Edited with NullSoft Scriptable Installation System                
#              by Vlasis K. Barkas aka Red Wine red_wine@freemail.gr Sep 2006               
############################################################################################

!define APP_NAME "Radioddity GD-77 CPS 3.1.x - Community Edition"
!define COMP_NAME "Roger Clark VK3KYY"
!define WEB_SITE "https://www.rogerclark.net"
!define VERSION "00.00.00.1"
!define COPYRIGHT "Roger Clark � 2019"
!define DESCRIPTION "Application"
!define INSTALLER_NAME "GD77CPSCommunityEditionInstaller.exe"
!define MAIN_APP_EXE "GD77CPS.exe"
!define INSTALL_TYPE "SetShellVarContext current"
!define REG_ROOT "HKCU"
!define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

######################################################################

VIProductVersion  "${VERSION}"
VIAddVersionKey "ProductName"  "${APP_NAME}"
VIAddVersionKey "CompanyName"  "${COMP_NAME}"
VIAddVersionKey "LegalCopyright"  "${COPYRIGHT}"
VIAddVersionKey "FileDescription"  "${DESCRIPTION}"
VIAddVersionKey "FileVersion"  "${VERSION}"

######################################################################

SetCompressor ZLIB
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${INSTALLER_NAME}"
BrandingText "${APP_NAME}"
XPStyle on
InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir "$PROGRAMFILES\RadioddityGD77CPS31XCommunityEdition"

######################################################################

!include "MUI.nsh"

!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!insertmacro MUI_PAGE_WELCOME

!ifdef LICENSE_TXT
!insertmacro MUI_PAGE_LICENSE "${LICENSE_TXT}"
!endif

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "RadioddityGD77CPS31XCommunityEdition"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application $SM_Folder
!endif

!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM

!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

######################################################################

Section -MainProgram
${INSTALL_TYPE}
SetOverwrite ifnewer
SetOutPath "$INSTDIR"
File "..\bin\Release_3.1.x\Default306.dat"
File "..\bin\Release_3.1.x\Default31X.dat"
File "..\bin\Release_3.1.x\DockPanel.config"
File "..\bin\Release_3.1.x\GD77CPS.exe"
File "..\bin\Release_3.1.x\GD77CPS.exe.config"
File "..\bin\Release_3.1.x\help.xml"
File "..\bin\Release_3.1.x\Tone.txt"
File "..\bin\Release_3.1.x\WeifenLuo.WinFormsUI.Docking.dll"
SetOutPath "$INSTDIR\Language"
File "..\bin\Release_3.1.x\Language\English.chm"
File "..\bin\Release_3.1.x\Language\English.xml"
File "..\bin\Release_3.1.x\Language\French.xml"
File "..\bin\Release_3.1.x\Language\German.chm"
File "..\bin\Release_3.1.x\Language\German.xml"
File "..\bin\Release_3.1.x\Language\Polski.chm"
File "..\bin\Release_3.1.x\Language\Polski.xml"
File "..\bin\Release_3.1.x\Language\Portuguese.xml"
File "..\bin\Release_3.1.x\Language\Slovenian.chm"
File "..\bin\Release_3.1.x\Language\Slovenian.xml"
File "..\bin\Release_3.1.x\Language\Spanish.chm"
File "..\bin\Release_3.1.x\Language\Spanish.xml"
File "..\bin\Release_3.1.x\Language\SpanishCatalan.chm"
File "..\bin\Release_3.1.x\Language\SpanishCatalan.xml"
File "..\bin\Release_3.1.x\Language\Ukrainian.chm"
File "..\bin\Release_3.1.x\Language\Ukrainian.xml"
SectionEnd

######################################################################

Section -Icons_Reg
SetOutPath "$INSTDIR"
WriteUninstaller "$INSTDIR\uninstall.exe"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
CreateDirectory "$SMPROGRAMS\$SM_Folder"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!insertmacro MUI_STARTMENU_WRITE_END
!endif

!ifndef REG_START_MENU
CreateDirectory "$SMPROGRAMS\RadioddityGD77CPS31XCommunityEdition"
CreateShortCut "$SMPROGRAMS\RadioddityGD77CPS31XCommunityEdition\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\RadioddityGD77CPS31XCommunityEdition\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!endif

WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"

!ifdef WEB_SITE
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"
!endif
SectionEnd

######################################################################

Section Uninstall
${INSTALL_TYPE}
Delete "$INSTDIR\Default306.dat"
Delete "$INSTDIR\Default31X.dat"
Delete "$INSTDIR\DockPanel.config"
Delete "$INSTDIR\GD77CPS.exe"
Delete "$INSTDIR\GD77CPS.exe.config"
Delete "$INSTDIR\help.xml"
Delete "$INSTDIR\Tone.txt"
Delete "$INSTDIR\WeifenLuo.WinFormsUI.Docking.dll"
Delete "$INSTDIR\Language\English.chm"
Delete "$INSTDIR\Language\English.xml"
Delete "$INSTDIR\Language\French.xml"
Delete "$INSTDIR\Language\German.chm"
Delete "$INSTDIR\Language\German.xml"
Delete "$INSTDIR\Language\Polski.chm"
Delete "$INSTDIR\Language\Polski.xml"
Delete "$INSTDIR\Language\Portuguese.xml"
Delete "$INSTDIR\Language\Slovenian.chm"
Delete "$INSTDIR\Language\Slovenian.xml"
Delete "$INSTDIR\Language\Spanish.chm"
Delete "$INSTDIR\Language\Spanish.xml"
Delete "$INSTDIR\Language\Ukrainian.chm"
Delete "$INSTDIR\Language\Ukrainian.xml"
 
RmDir "$INSTDIR\Language"
 
Delete "$INSTDIR\uninstall.exe"
!ifdef WEB_SITE
Delete "$INSTDIR\${APP_NAME} website.url"
!endif

RmDir "$INSTDIR"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
!endif
RmDir "$SMPROGRAMS\$SM_Folder"
!endif

!ifndef REG_START_MENU
Delete "$SMPROGRAMS\RadioddityGD77CPS31XCommunityEdition\${APP_NAME}.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\RadioddityGD77CPS31XCommunityEdition\${APP_NAME} Website.lnk"
!endif
RmDir "$SMPROGRAMS\RadioddityGD77CPS31XCommunityEdition"
!endif

DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
SectionEnd

######################################################################

