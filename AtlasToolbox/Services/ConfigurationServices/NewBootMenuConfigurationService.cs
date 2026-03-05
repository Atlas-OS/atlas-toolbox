using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class NewBootMenuConfigurationService : IRoutable
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\NewBootMenu";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _newBootMenuConfigurationStore;
        private readonly IBcdService _bcdService;

        public NewBootMenuConfigurationService(
            [FromKeyedServices("NewBootMenu")] ConfigurationStore newBootMenuConfigurationStore,
            IBcdService bcdService)
        {
            _newBootMenuConfigurationStore = newBootMenuConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Default, WellKnownElementTypes.BootMenuPolicyWinload, 0UL);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _newBootMenuConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Default, WellKnownElementTypes.BootMenuPolicyWinload, 1UL);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _newBootMenuConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
