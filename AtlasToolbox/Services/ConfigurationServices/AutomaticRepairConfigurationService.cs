using AtlasToolbox.Services;
using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class AutomaticRepairConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\AutomaticRepair";
        private const string STATE_VALUE_NAME = "state";


        private readonly ConfigurationStore _automaticRepairConfigurationStore;
        private readonly IBcdService _bcdService;

        public AutomaticRepairConfigurationService(
            [FromKeyedServices("AutomaticRepair")] ConfigurationStore automaticRepairConfigurationStore,
            IBcdService bcdService)
        {
            _automaticRepairConfigurationStore = automaticRepairConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Current, WellKnownElementTypes.BootStatusPolicy, 1UL);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);


            _automaticRepairConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.SetIntegerElement(WellKnownObjectIdentifiers.Current, WellKnownElementTypes.BootStatusPolicy, 0UL);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _automaticRepairConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
