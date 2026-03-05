using AtlasToolbox.Services;
using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class BootLogoConfigurationService : IRoutable
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\BootLogo";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _configurationStore;
        private readonly IBcdService _bcdService;

        public BootLogoConfigurationService(
            [FromKeyedServices("BootLogo")] ConfigurationStore configurationStore,
            IBcdService bcdService)
        {
            _configurationStore = configurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxLogo, true);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxLogo);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
