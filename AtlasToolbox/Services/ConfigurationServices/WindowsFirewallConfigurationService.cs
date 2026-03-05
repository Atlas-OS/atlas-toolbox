using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class WindowsFirewallConfigurationService : IRoutable
    {
        private const string BFE_SERVICE_NAME = "BFE";
        private const string MPSSVC_SERVICE_NAME = "mpssvc";

        private readonly ConfigurationStore _windowsFirewallConfigurationStore;

        public WindowsFirewallConfigurationService(
            [FromKeyedServices("WindowsFirewall")] ConfigurationStore windowsFirewallConfigurationStore)
        {
            _windowsFirewallConfigurationStore = windowsFirewallConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(BFE_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(MPSSVC_SERVICE_NAME, ServiceStartMode.Disabled);

            _windowsFirewallConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(BFE_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(MPSSVC_SERVICE_NAME, ServiceStartMode.Automatic);

            _windowsFirewallConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                ServiceHelper.IsStartupTypeMatch(BFE_SERVICE_NAME, ServiceStartMode.Automatic),
                ServiceHelper.IsStartupTypeMatch(MPSSVC_SERVICE_NAME, ServiceStartMode.Automatic)
            };

            return checks.All(x => x);
        }
    }
}
