using System;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class CompactViewConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\CompactView";
        private const string STATE_VALUE_NAME = "state";


        private const string ADVANCED_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string USE_COMPACT_MODE_VALUE_NAME = "UseCompactMode";

        private readonly ConfigurationStore _compactViewConfigurationStore;

        public CompactViewConfigurationService(
            [FromKeyedServices("CompactView")] ConfigurationStore compactViewConfigurationStore)
        {
            _compactViewConfigurationStore = compactViewConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, USE_COMPACT_MODE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\Automatic Folder Discovery\Disable Compact View.cmd");

            _compactViewConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, USE_COMPACT_MODE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\Compact View\Enable Compact View (default).cmd");

            _compactViewConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
