; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Atlas Toolbox"
#define MyAppVersion "0.1.2"
#define MyAppPublisher "AtlasOS"
#define MyAppURL "https://www.atlasos.net/"
#define MyAppExeName "AtlasToolbox.exe"
#define MyAppAssocName MyAppName + ""
#define MyAppAssocExt ".exe"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{98A41650-0BAA-476E-A372-01B5FB0A76FA}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
; "ArchitecturesAllowed=x64compatible" specifies that Setup cannot run
; on anything but x64 and Windows 11 on Arm.
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
ChangesAssociations=yes
DisableProgramGroupPage=yes
LicenseFile=D:\a\atlas-toolbox\atlas-toolbox\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only).
;PrivilegesRequired=lowest
OutputBaseFilename=mysetup
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\a\atlas-toolbox\atlas-toolbox\Deploy\src\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Assets\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\runtimes\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\AtlasToolbox.deps.json"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\AtlasToolbox.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\AtlasToolbox.dll.config"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\AtlasToolbox.pdb"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\AtlasToolbox.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\BcdSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.Mvvm.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.WinUI.Animations.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.WinUI.Controls.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.WinUI.Controls.SettingsControls.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.WinUI.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.WinUI.Helpers.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\CommunityToolkit.WinUI.Triggers.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\dism.log"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\ICSharpCode.Decompiler.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Dism.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.Binder.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.CommandLine.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.EnvironmentVariables.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.FileExtensions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Configuration.UserSecrets.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.DependencyInjection.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.DependencyInjection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Diagnostics.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Diagnostics.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.FileProviders.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.FileProviders.Physical.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.FileSystemGlobbing.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Hosting.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Hosting.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.Abstractions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.Configuration.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.Console.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.Debug.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.EventLog.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Logging.EventSource.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Options.ConfigurationExtensions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Options.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Extensions.Primitives.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Graphics.Canvas.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Graphics.Canvas.Interop.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.InteractiveExperiences.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Web.WebView2.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Web.WebView2.Core.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.ApplicationModel.DynamicDependency.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.ApplicationModel.Resources.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.ApplicationModel.WindowsAppRuntime.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.AppLifecycle.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.AppNotifications.Builder.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.AppNotifications.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.Management.Deployment.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.PushNotifications.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.SDK.NET.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.Security.AccessControl.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.Storage.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.System.Power.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.System.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Windows.Widgets.Projection.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.WindowsAppRuntime.Bootstrap.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.WindowsAppRuntime.Bootstrap.Net.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.WinUI.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Xaml.Interactions.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Microsoft.Xaml.Interactivity.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\MVVMEssentials.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\NLog.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\NLog.Extensions.Logging.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\resources.pri"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.CodeDom.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.Configuration.ConfigurationManager.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.Diagnostics.EventLog.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.Diagnostics.EventLog.Messages.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.Management.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.Runtime.InteropServices.WindowsRuntime.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.Security.Cryptography.ProtectedData.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\System.ServiceProcess.ServiceController.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\WebView2Loader.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\WindowsDisplayAPI.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\WinRT.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "C:\Users\TheyCreeper\Desktop\Toolbox\WinUIEx.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

