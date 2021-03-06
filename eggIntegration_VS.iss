; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "eggIntegration for Visual Studio"
#define MyAppVersion "0.7"
#define MyAppPublisher "TestPlant Europe Ltd"
#define MyAppURL "http://www.testplant.com/"
#define eggPlantVSPackage "eggPlantVSPackage.vsix"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D86EBE25-CBC3-4A72-B69F-4C2F06F33E8A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=License Agreement.rtf
OutputBaseFilename=eggIntegrationVS_setup
SetupIconFile=VSPackage\Resources\WAsPackage.ico
Compression=lzma
SolidCompression=yes
MinVersion=0,6.1
SourceDir=C:\Dev\eggIntegration_VS

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
;Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "VSPackage\bin\Release\eggPlantVSPackage.vsix"; DestDir: "{app}"; Flags: ignoreversion
Source: "SenseTalkLanguage\bin\Release\SenseTalkLanguage.vsix"; DestDir: "{app}"; Flags: ignoreversion
Source: "RunEPFScriptVS\bin\Release\CommandLine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "RunEPFScriptVS\bin\Release\RunEPFScriptVS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "RunEPFScriptVS\bin\Release\CookComputing.XmlRpcV2.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "VSPackage\License Agreement.txt"; DestDir: "{app}"
Source: "RunEPFScriptVS\ThirdParty.txt"; DestDir: "{app}"


[Icons]
Name: "{group}\{#eggPlantVSPackage}"; Filename: "{app}\{#eggPlantVSPackage}"
Name: "{group}\SenseTalkLanguage.vsix"; Filename: "{app}\SenseTalkLanguage.vsix"
;Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#eggPlantVSPackage}"; Flags: shellexec postinstall skipifsilent; Description: "eggPlant script execution for Visual Studio"
Filename: "{app}\SenseTalkLanguage.vsix"; Flags: shellexec postinstall runhidden; Description: "SenseTalk Language extension for Visual Studio"

[ThirdParty]
UseRelativePaths=True

[Registry]
Root: "HKLM"; Subkey: "Software\TestPlant\eggIntegrationVS"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Flags: createvalueifdoesntexist
Root: "HKLM"; Subkey: "Software\TestPlant\eggIntegrationVS"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: createvalueifdoesntexist

[Messages]
FinishedLabel=After the Visual Studio packages have installed,there will be a new Visual Studio Tools Menu item:%neggPlant Functional Tests...%nThis tool enables you to add eggPlant Functional scripts to a Visual Studio Test project.
ClickFinish=Click Finish to install the Visual Studio Packages.
UninstalledAll=%1 was successfully removed from your computer.%nTo remove the eggIntegration Visual Studio extensions select TOOLS, Extensions and Updates from Visual Studio and uninstall the eggPlant extensions.
