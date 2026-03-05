using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class NetworkDiscoveryConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\NetworkDiscovery";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _networkDiscoveryConfigurationStore;
        private readonly ConfigurationStore _lanmanWorkstationConfigurationStore;
        private readonly IRoutable _lanmanWorkstationConfigurationService;

        private const string EVENTLOG_SERVICE_NAME = "eventlog";
        private const string FDPHOST_SERVICE_NAME = "fdPHost";
        private const string FDRESPUB_SERVICE_NAME = "FDResPub";
        private const string LMHOSTS_SERVICE_NAME = "lmhosts";
        private const string NETMAN_SERVICE_NAME = "netman";
        private const string NLASVC_SERVICE_NAME = "NlaSvc";
        private const string SSDPSRV_SERVICE_NAME = "SSDPSRV";

        public NetworkDiscoveryConfigurationService(
            [FromKeyedServices("NetworkDiscovery")] ConfigurationStore networkDiscoveryConfigurationStore,
            [FromKeyedServices("LanmanWorkstation")] ConfigurationStore lanmanWorkstationConfigurationStore,
            [FromKeyedServices("LanmanWorkstation")] IRoutable lanmanWorkstationConfigurationService)
        {
            _networkDiscoveryConfigurationStore = networkDiscoveryConfigurationStore;
            _lanmanWorkstationConfigurationStore = lanmanWorkstationConfigurationStore;
            _lanmanWorkstationConfigurationService = lanmanWorkstationConfigurationService;

            _lanmanWorkstationConfigurationStore.CurrentSettingChanged += LanmanWorkstationConfigurationStore_CurrentSettingChanged;
        }

        public void Disable()
        {
            if (_lanmanWorkstationConfigurationStore.CurrentSetting)
            {
                _lanmanWorkstationConfigurationService.Disable();
            }

            ServiceHelper.SetStartupType(FDPHOST_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(FDRESPUB_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(LMHOSTS_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(NETMAN_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(NLASVC_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(SSDPSRV_SERVICE_NAME, ServiceStartMode.Disabled);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Network Discovery\Disable Network Discovery Services.cmd");


            _networkDiscoveryConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            if (!_lanmanWorkstationConfigurationStore.CurrentSetting)
            {
                _lanmanWorkstationConfigurationService.Enable();
            }

            ServiceHelper.SetStartupType(EVENTLOG_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(FDPHOST_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(FDRESPUB_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(LMHOSTS_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(NETMAN_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(NLASVC_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(SSDPSRV_SERVICE_NAME, ServiceStartMode.Manual);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Network Discovery\Enable Network Discovery Services (default).cmd");

            _networkDiscoveryConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }

        private void LanmanWorkstationConfigurationStore_CurrentSettingChanged()
        {
            _networkDiscoveryConfigurationStore.CurrentSetting = IsEnabled();
        }
    }
}
