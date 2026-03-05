using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class SearchIndexingConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\SearchIndexing";
        private const string STATE_VALUE_NAME = "state";

        private const string WSEARCH_SERVICE_NAME = "WSearch";


        private readonly ConfigurationStore _searchIndexingConfigurationStore;

        public SearchIndexingConfigurationService(
            [FromKeyedServices("SearchIndexing")] ConfigurationStore searchIndexingConfigurationStore)
        {
            _searchIndexingConfigurationStore = searchIndexingConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(WSEARCH_SERVICE_NAME, ServiceStartMode.Disabled);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _searchIndexingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(WSEARCH_SERVICE_NAME, ServiceStartMode.Automatic);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _searchIndexingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
