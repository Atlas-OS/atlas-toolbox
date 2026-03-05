using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class HagsConfigurationService : IRoutable
    {
        private const string GRAPHIC_DRIVERS_KEY_NAME = @"HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";

        private const string HW_SCH_MODE_VALUE_NAME = "HwSchMode";

        private readonly ConfigurationStore _hagsConfigurationStore;

        public HagsConfigurationService(
            [FromKeyedServices("Hags")] ConfigurationStore hagsConfigurationStore)
        {
            _hagsConfigurationStore = hagsConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.DeleteValue(GRAPHIC_DRIVERS_KEY_NAME, HW_SCH_MODE_VALUE_NAME);

            _hagsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(GRAPHIC_DRIVERS_KEY_NAME, HW_SCH_MODE_VALUE_NAME, 2);

            _hagsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(GRAPHIC_DRIVERS_KEY_NAME, HW_SCH_MODE_VALUE_NAME, 2);
        }
    }
}
