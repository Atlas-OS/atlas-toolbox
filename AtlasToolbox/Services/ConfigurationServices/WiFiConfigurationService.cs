using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class WiFiConfigurationService : IRoutable
    {
        private const string EVENTLOG_SERVICE_NAME = "eventlog";
        private const string VWIFIFLT_SERVICE_NAME = "vwififlt";
        private const string WLANSVC_SERVICE_NAME = "WlanSvc";

        private readonly ConfigurationStore _wiFiConfigurationStore;

        public WiFiConfigurationService(
            [FromKeyedServices("WiFi")] ConfigurationStore wiFiConfigurationStore)
        {
            _wiFiConfigurationStore = wiFiConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(VWIFIFLT_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(WLANSVC_SERVICE_NAME, ServiceStartMode.Disabled);

            _wiFiConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(EVENTLOG_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(VWIFIFLT_SERVICE_NAME, ServiceStartMode.System);
            ServiceHelper.SetStartupType(WLANSVC_SERVICE_NAME, ServiceStartMode.Automatic);

            _wiFiConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                ServiceHelper.IsStartupTypeMatch(EVENTLOG_SERVICE_NAME, ServiceStartMode.Automatic),
                ServiceHelper.IsStartupTypeMatch(VWIFIFLT_SERVICE_NAME, ServiceStartMode.System),
                ServiceHelper.IsStartupTypeMatch(WLANSVC_SERVICE_NAME, ServiceStartMode.Automatic)
            };

            return checks.All(x => x);
        }
    }
}
