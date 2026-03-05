using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class SecurityHealthTrayConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\SecurityHealthTray";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _securityHealthTrayConfigurationService;

        public SecurityHealthTrayConfigurationService(
            [FromKeyedServices("SecurityHealthTray")] ConfigurationStore securityHealthTrayConfigurationService)
        {
            _securityHealthTrayConfigurationService = securityHealthTrayConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.MergeRegFile(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\SecurityHealthTray\RemoveTray.reg");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\7. Security\Defender\Security Health Tray\Remove Security Tray from Startup (default).cmd");

            _securityHealthTrayConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.MergeRegFile(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\SecurityHealthTray\AddTray.reg");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\7. Security\Defender\Security Health Tray\Add Security Tray from Startup.cmd");

            _securityHealthTrayConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
