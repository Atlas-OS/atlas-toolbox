﻿using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class NetworkDiscoveryConfigurationService : IConfigurationService
    {
        private readonly ConfigurationStore _networkDiscoveryConfigurationStore;
        private readonly ConfigurationStore _lanmanWorkstationConfigurationStore;
        private readonly IConfigurationService _lanmanWorkstationConfigurationService;

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
            [FromKeyedServices("LanmanWorkstation")] IConfigurationService lanmanWorkstationConfigurationService)
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

            _networkDiscoveryConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                _lanmanWorkstationConfigurationStore.CurrentSetting is true,
                ServiceHelper.IsStartupTypeMatch(EVENTLOG_SERVICE_NAME, ServiceStartMode.Automatic),
                ServiceHelper.IsStartupTypeMatch(FDPHOST_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(FDRESPUB_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(LMHOSTS_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(NETMAN_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(NLASVC_SERVICE_NAME, ServiceStartMode.Automatic),
                ServiceHelper.IsStartupTypeMatch(SSDPSRV_SERVICE_NAME, ServiceStartMode.Manual)
            };

            return checks.All(x => x);
        }

        private void LanmanWorkstationConfigurationStore_CurrentSettingChanged()
        {
            _networkDiscoveryConfigurationStore.CurrentSetting = IsEnabled();
        }
    }
}
