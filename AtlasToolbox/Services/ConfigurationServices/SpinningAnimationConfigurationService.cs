using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class SpinningAnimationConfigurationService : IRoutable
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\SpinningAnimations";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _spinningAnimationConfigurationStore;
        private readonly IBcdService _bcdService;

        public SpinningAnimationConfigurationService(
            [FromKeyedServices("SpinningAnimation")] ConfigurationStore spinningAnimationConfigurationStore,
            IBcdService bcdService)
        {
            _spinningAnimationConfigurationStore = spinningAnimationConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxProgress, true);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _spinningAnimationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxProgress);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _spinningAnimationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
