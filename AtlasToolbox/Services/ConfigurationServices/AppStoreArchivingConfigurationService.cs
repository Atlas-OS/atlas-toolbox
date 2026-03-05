using Microsoft.Extensions.DependencyInjection;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using System;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class AppStoreArchivingConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\AppStoreArchiving";
        private const string STATE_VALUE_NAME = "state";

        private const string APPX_KEY_NAME = @"HKLM\Software\Policies\Microsoft\Windows\Appx";

        private const string ALLOW_AUTOMATIC_APP_ARCHIVING_VALUE_NAME = "AllowAutomaticAppArchiving";

        private readonly ConfigurationStore _appStoreArchivingConfigurationService;

        public AppStoreArchivingConfigurationService(
            [FromKeyedServices("AppStoreArchiving")] ConfigurationStore appStoreArchivingConfigurationStore)
        {
            _appStoreArchivingConfigurationService = appStoreArchivingConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(APPX_KEY_NAME, ALLOW_AUTOMATIC_APP_ARCHIVING_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Store App Archiving\Disable Store App Archiving (default).cmd");

            _appStoreArchivingConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(APPX_KEY_NAME, ALLOW_AUTOMATIC_APP_ARCHIVING_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Store App Archiving\Enable Store App Archiving.cmd");

            _appStoreArchivingConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
