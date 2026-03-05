using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class PhoneLinkConfigurationService :IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\PhoneLink";
        private const string STATE_VALUE_NAME = "state";

        private const string SYSTEM_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string WINDOWS_UPDATE_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsStore\WindowsUpdate";
        private const string CLOUD_CONTENT_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        private const string DISABLE_WINDOWS_CONSUMER_FEATURES_VALUE_NAME = "DisableWindowsConsumerFeatures";
        private const string AUTO_DOWNLOAD_VALUE_NAME = "AutoDownload";
        private const string NO_CONNECTED_USER_VALUE_NAME = "NoConnectedUser";

        private readonly ConfigurationStore _phoneLinkConfigurationService;
        public PhoneLinkConfigurationService(
            [FromKeyedServices("PhoneLink")] ConfigurationStore phoneLinkConfigurationService) 
        {
            _phoneLinkConfigurationService = phoneLinkConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(SYSTEM_KEY_NAME, NO_CONNECTED_USER_VALUE_NAME, 1, RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_UPDATE_KEY_NAME, AUTO_DOWNLOAD_VALUE_NAME, 2, RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Mobile Devices (Phone Link)\Disable Mobile Device Settings (default).cmd");

            CommandPromptHelper.RunCommand(@"call %windir%\AtlasModules\Scripts\settingsPages.cmd /hide mobile-devices");
            CommandPromptHelper.RunCommand(@"powershell -NoP -NonI ""Get-AppxPackage -AllUsers Microsoft.YourPhone* | Remove-AppxPackage -AllUsers""");

            _phoneLinkConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(SYSTEM_KEY_NAME, NO_CONNECTED_USER_VALUE_NAME);
            RegistryHelper.DeleteValue(CLOUD_CONTENT_KEY_NAME, DISABLE_WINDOWS_CONSUMER_FEATURES_VALUE_NAME);
            RegistryHelper.SetValue(WINDOWS_UPDATE_KEY_NAME, AUTO_DOWNLOAD_VALUE_NAME, 4, RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Mobile Devices (Phone Link)\Enable Mobile Device Settings.cmd");

            CommandPromptHelper.RunCommand(@"call %windir%\AtlasModules\Scripts\settingsPages.cmd /unhide mobile-devices");

            _phoneLinkConfigurationService.CurrentSetting = IsEnabled();

        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
