using AtlasToolbox.Stores;
using BcdSharp.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class BootMessagesConfigurationService : IRoutable
    {
        private readonly ConfigurationStore _bootMessagesConfigurationStore;
        private readonly IBcdService _bcdService;

        public BootMessagesConfigurationService(
            [FromKeyedServices("BootMessages")] ConfigurationStore bootMessagesConfigurationStore,
            IBcdService bcdService)
        {
            _bootMessagesConfigurationStore = bootMessagesConfigurationStore;
            _bcdService = bcdService;
        }

        public void Disable()
        {
            _bcdService.SetBooleanElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxText, true);

            _bootMessagesConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            _bcdService.DeleteElement(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxText);

            _bootMessagesConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            object value = _bcdService.GetElementValue(WellKnownObjectIdentifiers.GlobalSettings, WellKnownElementTypes.NoBootUxText);

            return value is null or false;
        }
    }
}
