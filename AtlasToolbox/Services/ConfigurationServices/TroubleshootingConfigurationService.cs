using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class TroubleshootingConfigurationService : IRoutable
    {
        private const string DPS_SERVICE_NAME = "DPS";
        private const string WDI_SERVICE_HOST_SERVICE_NAME = "WdiServiceHost";
        private const string WDI_SYSTEM_HOST_SERVICE_NAME = "WdiSystemHost";

        private const string DIAG_LOG_KEY_NAME = @"HKLM\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagLog";

        private const string START_VALUE_NAME = "Start";

        private readonly ConfigurationStore _troubleshootingConfigurationStore;

        public TroubleshootingConfigurationService(
            [FromKeyedServices("Troubleshooting")] ConfigurationStore troubleshootingConfigurationStore)
        {
            _troubleshootingConfigurationStore = troubleshootingConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(DPS_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(WDI_SERVICE_HOST_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(WDI_SYSTEM_HOST_SERVICE_NAME, ServiceStartMode.Disabled);

            RegistryHelper.SetValue(DIAG_LOG_KEY_NAME, START_VALUE_NAME, "0");

            _troubleshootingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(DPS_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(WDI_SERVICE_HOST_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(WDI_SYSTEM_HOST_SERVICE_NAME, ServiceStartMode.Manual);

            RegistryHelper.SetValue(DIAG_LOG_KEY_NAME, START_VALUE_NAME, "1");

            _troubleshootingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                ServiceHelper.IsStartupTypeMatch(DPS_SERVICE_NAME, ServiceStartMode.Automatic),
                ServiceHelper.IsStartupTypeMatch(WDI_SERVICE_HOST_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(WDI_SYSTEM_HOST_SERVICE_NAME, ServiceStartMode.Manual),
                RegistryHelper.IsMatch(DIAG_LOG_KEY_NAME, START_VALUE_NAME, "1")
            };

            return checks.All(x => x);
        }
    }
}
