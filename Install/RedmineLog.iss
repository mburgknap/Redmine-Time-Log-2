; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "RedmineLog"
#define MyAppVersion "12.16.0.0"
#define MyAppPublisher "Marcin Burgknap"
#define MyAppURL "https://liteapps-redmine.updog.co/"
#define MyAppExeName "RedmineLog.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{511B4CA6-52B4-4EF9-B9D5-083231293E70}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableProgramGroupPage=yes
OutputDir=E:\Dev\Praca\RedmineTimeLog\Install
OutputBaseFilename=RedmineLog
SetupIconFile=E:\Dev\Praca\RedmineTimeLog\RedmineLog\RedmineTimeLog.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\RedmineLog.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Appccelerate.EventBroker.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Appccelerate.Fundamentals.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\CommonServiceLocator.NinjectAdapter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\DBreeze.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\FluentDateTime.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Ninject.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Ninject.Extensions.AppccelerateEventBroker.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Ninject.Extensions.ContextPreservation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Ninject.Extensions.NamedScope.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\NLog.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\NLog.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\Redmine.Net.Api.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\RedmineLog.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\RedmineLog.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\RedmineLog.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\RedmineLog.Logic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\System.Reactive.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\System.Reactive.Interfaces.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\System.Reactive.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Praca\RedmineTimeLog\Bin\System.Reactive.Providers.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

