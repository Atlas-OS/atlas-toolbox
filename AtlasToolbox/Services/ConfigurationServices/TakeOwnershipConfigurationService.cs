using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace AtlasToolbox.Services.ConfigurationServices
{
    public class TakeOwnershipConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\TakeOwnership";
        private const string STATE_VALUE_NAME = "state";

        private const string RUNAS_DRIVE_KEY_NAME = @"HKCR\Drive\shell\runas";
        private const string TAKE_OWNERSHIP_KEY_NAME = @"HKCR\*\shell\TakeOwnership";
        private const string TAKE_OWNERSHIP_COMMAND_KEY_NAME = @"HKCR\*\shell\TakeOwnership\command";
        private const string TAKE_OWNERSHIP_DIRECTORY_KEY_NAME = @"HKCR\Directory\shell\TakeOwnership";
        private const string TAKE_OWNERSHIP_DIRECTORY_COMMAND_KEY_NAME = @"HKCR\Directory\shell\TakeOwnership\command";
        private const string RUNAS_KEY_NAME = @"HKCR\*\shell\runas";
        private const string RUNAS_COMMAND_KEY_NAME = @"HKCR\Drive\shell\runas\command";

        private readonly ConfigurationStore _takeOwnership;

        public TakeOwnershipConfigurationService(
                    [FromKeyedServices("TakeOwnership")] ConfigurationStore takeOwnership)
        {
            _takeOwnership = takeOwnership;
        }
        public void Disable()
        {
            RegistryHelper.DeleteKey(RUNAS_DRIVE_KEY_NAME);
            RegistryHelper.DeleteKey(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME);
            RegistryHelper.DeleteKey(TAKE_OWNERSHIP_KEY_NAME);
            RegistryHelper.DeleteKey(RUNAS_KEY_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Context Menus\Take Ownership\Remove Take Ownership to Context Menu (default).cmd");

            _takeOwnership.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(TAKE_OWNERSHIP_KEY_NAME, "", "Take Ownership", Microsoft.Win32.RegistryValueKind.String);    
            RegistryHelper.DeleteValue(TAKE_OWNERSHIP_KEY_NAME, "Extended");
            RegistryHelper.SetValue(TAKE_OWNERSHIP_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_KEY_NAME, "NoWorkingDirectory", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_KEY_NAME, "NeverDefault", "", Microsoft.Win32.RegistryValueKind.String);

            RegistryHelper.SetValue(TAKE_OWNERSHIP_COMMAND_KEY_NAME, "", "PowerShell -windowstyle hidden -command \"Start-Process cmd -ArgumentList '/c takeown /f \\\"%1\\\" && icacls \\\"%1\\\" /grant *S-1-3-4:F /t /c /l & pause' -Verb runAs\"", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_COMMAND_KEY_NAME, "IsolatedCommand", "PowerShell -windowstyle hidden -command \"Start-Process cmd -ArgumentList '/c takeown /f \\\"%1\\\" && icacls \\\"%1\\\" /grant *S-1-3-4:F /t /c /l & pause' -Verb runAs\"", Microsoft.Win32.RegistryValueKind.String);

            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "", "Take Ownership", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "AppliedTo", "NOT (System.ItemPathDisplay:=\"C:\\Users\" OR System.ItemPathDisplay:=\"C:\\ProgramData\" OR System.ItemPathDisplay:=\"C:\\Windows\" OR System.ItemPathDisplay:=\"C:\\Windows\\System32\" OR System.ItemPathDisplay:=\"C:\\Program Files\" OR System.ItemPathDisplay:=\"C:\\Program Files (x86)\")", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.DeleteValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "Extended");
            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "NoWorkingDirectory", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "NeverDefault", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_KEY_NAME, "Position", "Middle", Microsoft.Win32.RegistryValueKind.String);

            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_COMMAND_KEY_NAME, "", "PowerShell -windowstyle hidden -command \"$Y = ($null | choice).Substring(1,1); Start-Process cmd -ArgumentList ('/c takeown /f \\\"%1\\\" /r /d ' + $Y + ' && icacls \\\"%1\\\" /grant *S-1-3-4:F /t /c /l /q & pause') -Verb runAs\"", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(TAKE_OWNERSHIP_DIRECTORY_COMMAND_KEY_NAME, "PowerShell -windowstyle hidden -command \"$Y = ($null | choice).Substring(1,1); Start-Process cmd -ArgumentList ('/c takeown /f \\\"%1\\\" /r /d ' + $Y + ' && icacls \\\"%1\\\" /grant *S-1-3-4:F /t /c /l /q & pause') -Verb runAs\"", Microsoft.Win32.RegistryValueKind.String);

            RegistryHelper.SetValue(RUNAS_DRIVE_KEY_NAME, "", "Take Ownership", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(RUNAS_DRIVE_KEY_NAME, "AppliedTo", "NOT (System.ItemPathDisplay:=\"C:\\\")", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.DeleteValue(RUNAS_DRIVE_KEY_NAME, "Extended");
            RegistryHelper.SetValue(RUNAS_DRIVE_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(RUNAS_DRIVE_KEY_NAME, "NoWorkingDirectory", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(RUNAS_DRIVE_KEY_NAME, "NeverDefault", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(RUNAS_DRIVE_KEY_NAME, "Position", "Middle", Microsoft.Win32.RegistryValueKind.String);

            RegistryHelper.SetValue(RUNAS_COMMAND_KEY_NAME, "", "cmd.exe /c takeown /f \"%1\\\" /r /d y && icacls \"%1\\\" /grant *S-1-3-4:F /t /c & Pause", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(RUNAS_COMMAND_KEY_NAME, "cmd.exe /c takeown /f \"%1\\\" /r /d y && icacls \"%1\\\" /grant *S-1-3-4:F /t /c & Pause", Microsoft.Win32.RegistryValueKind.String);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Context Menus\Take Ownership\Add Take Ownership to Context Menu.cmd");

            _takeOwnership.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}

