using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class VpnConfigurationService : IRoutable
    {
        private const string BFE_SERVICE_NAME = "BFE";
        private const string EAPHOST_SERVICE_NAME = "Eaphost";
        private const string IKEEXT_SERVICE_NAME = "IKEEXT";
        private const string IPHLPSVC_SERVICE_NAME = "iphlpsvc";
        private const string NDIS_VIRTUAL_BUS_SERVICE_NAME = "NdisVirtualBus";
        private const string RAS_MAN_SERVICE_NAME = "RasMan";
        private const string SSTP_SVC_SERVICE_NAME = "SstpSvc";
        private const string WIN_HTTP_AUTO_PROXY_SVC_SERVICE_NAME = "WinHttpAutoProxySvc";

        private readonly ConfigurationStore _vpnConfigurationStore;
        private readonly IEnumerable<string> _deviceNames;

        public VpnConfigurationService(
            [FromKeyedServices("Vpn")] ConfigurationStore vpnConfigurationStore)
        {
            _vpnConfigurationStore = vpnConfigurationStore;

            _deviceNames = new[]
            {
                "NDIS Virtual Network Adapter Enumerator",
                "Microsoft RRAS Root Enumerator",
                "WAN Miniport%"
            };
        }

        public void Disable()
        {
            foreach (string deviceName in _deviceNames)
            {
                DeviceHelper.DisableDevice(deviceName);
            }

            ServiceHelper.SetStartupType(EAPHOST_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(IKEEXT_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(IPHLPSVC_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(NDIS_VIRTUAL_BUS_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(RAS_MAN_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(SSTP_SVC_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(WIN_HTTP_AUTO_PROXY_SVC_SERVICE_NAME, ServiceStartMode.Disabled);

            _vpnConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            foreach (string deviceName in _deviceNames)
            {
                DeviceHelper.EnableDevice(deviceName);
            }

            ServiceHelper.SetStartupType(BFE_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(EAPHOST_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(IKEEXT_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(IPHLPSVC_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(NDIS_VIRTUAL_BUS_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(RAS_MAN_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(SSTP_SVC_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(WIN_HTTP_AUTO_PROXY_SVC_SERVICE_NAME, ServiceStartMode.Manual);

            _vpnConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                _deviceNames.All(x => DeviceHelper.GetDeviceStatus(x) is true),
                ServiceHelper.IsStartupTypeMatch(BFE_SERVICE_NAME, ServiceStartMode.Automatic),
                ServiceHelper.IsStartupTypeMatch(EAPHOST_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(IKEEXT_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(IPHLPSVC_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(NDIS_VIRTUAL_BUS_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(RAS_MAN_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(SSTP_SVC_SERVICE_NAME, ServiceStartMode.Manual),
                ServiceHelper.IsStartupTypeMatch(WIN_HTTP_AUTO_PROXY_SVC_SERVICE_NAME, ServiceStartMode.Manual)
            };

            return checks.All(x => x);
        }
    }
}
