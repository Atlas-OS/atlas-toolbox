using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class ModernAltTabConfigurationService : IRoutable
    {
        private const string EXPLORER_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

        private const string ALT_TAB_VALUE_NAME = "AltTabSettings";

        private readonly ConfigurationStore _modernAltTabConfigurationStore;

        public ModernAltTabConfigurationService(
            [FromKeyedServices("ModernAltTab")] ConfigurationStore modernAltTabConfigurationStore)
        {
            _modernAltTabConfigurationStore = modernAltTabConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(EXPLORER_KEY_NAME, ALT_TAB_VALUE_NAME, 1);

            _modernAltTabConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(EXPLORER_KEY_NAME, ALT_TAB_VALUE_NAME);

            _modernAltTabConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(EXPLORER_KEY_NAME, ALT_TAB_VALUE_NAME, null);
        }
    }
}
