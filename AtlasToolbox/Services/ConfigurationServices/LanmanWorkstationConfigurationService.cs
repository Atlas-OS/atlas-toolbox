using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Dism;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class LanmanWorkstationConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\LanmanWorkstation";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _lanmanWorkstationConfigurationStore;
        private readonly IDismService _dismService;

        private const string KSECPKG_SERVICE_NAME = "KSecPkg";
        private const string LANMANSERVER_SERVICE_NAME = "LanmanServer";
        private const string LANMANWORKSTATION_SERVICE_NAME = "LanmanWorkstation";
        private const string MRXSMB_SERVICE_NAME = "mrxsmb";
        private const string MRXSMB20_SERVICE_NAME = "mrxsmb20";
        private const string RDBSS_SERVICE_NAME = "rdbss";
        private const string SRV2_SERVICE_NAME = "srv2";

        private const string SMB_DIRECT_FEATURE_NAME = "SmbDirect";

        public LanmanWorkstationConfigurationService(
            [FromKeyedServices("LanmanWorkstation")] ConfigurationStore lanmanWorkstationConfigurationStore,
            IDismService dismService)
        {
            _lanmanWorkstationConfigurationStore = lanmanWorkstationConfigurationStore;
            _dismService = dismService;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(KSECPKG_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(LANMANSERVER_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(LANMANWORKSTATION_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(MRXSMB_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(MRXSMB20_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(RDBSS_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(SRV2_SERVICE_NAME, ServiceStartMode.Disabled);

            _dismService.DisableFeature(SMB_DIRECT_FEATURE_NAME);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Lanman Workstation (SMB)\Disable Lanman Workstation.cmd");


            _lanmanWorkstationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(KSECPKG_SERVICE_NAME, ServiceStartMode.Boot);
            ServiceHelper.SetStartupType(LANMANSERVER_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(LANMANWORKSTATION_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(MRXSMB_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(MRXSMB20_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(RDBSS_SERVICE_NAME, ServiceStartMode.System);
            ServiceHelper.SetStartupType(SRV2_SERVICE_NAME, ServiceStartMode.Manual);

            _dismService.EnableFeature(SMB_DIRECT_FEATURE_NAME);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Lanman Workstation (SMB)\Enable Lanman Workstation (default).cmd");

            _lanmanWorkstationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
