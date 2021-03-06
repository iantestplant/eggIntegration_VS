; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "eggPlant Visual Studio Integration"
#define MyAppVersion "1.0"
#define MyAppPublisher "TestPlant Europe Ltd"
#define MyAppURL "http://www.testplant.com/"
#define MyAppExeName "eggPlantVSPackage.vsix"

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
Source: "RunEPFScriptVS\bin\Release\CommandLine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "RunEPFScriptVS\bin\Release\RunEPFScriptVS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "RunEPFScriptVS\bin\Release\CookComputing.XmlRpcV2.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "VSPackage\License Agreement.txt"; DestDir: "{app}"
Source: "RunEPFScriptVS\ThirdParty.txt"; DestDir: "{app}"

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
;Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: shellexec postinstall skipifsilent

[ThirdParty]
UseRelativePaths=True

[Registry]
Root: "HKLM"; Subkey: "Software\TestPlant\eggIntegrationVS"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Flags: createvalueifdoesntexist
Root: "HKLM"; Subkey: "Software\TestPlant\eggIntegrationVS"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: createvalueifdoesntexist
