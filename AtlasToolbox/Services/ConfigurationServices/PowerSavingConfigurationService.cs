using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class PowerSavingConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\PowerSaving";
        private const string STATE_VALUE_NAME = "state";

        private const string DEFAULT_POWER_SAVING_SCRIPT_PATH_NAME = @"%windir%\AtlasModules\Scripts\ScriptWrappers\DefaultPowerSaving.ps1";
        private const string DISABLE_POWER_SAVING_SCRIPT_PATH_NAME = @"%windir%\AtlasModules\Scripts\ScriptWrappers\DisablePowerSaving.ps1";

        private readonly ConfigurationStore _powerSavingConfigurationStore;

        public PowerSavingConfigurationService(
            [FromKeyedServices("PowerSaving")] ConfigurationStore powerSavingConfigurationService) 
        {
            _powerSavingConfigurationStore = powerSavingConfigurationService;
        }
        public void Disable()
        {
            CommandPromptHelper.RunCommand($@"powershell -EP Bypass -NoP ^& """"""$env:{DISABLE_POWER_SAVING_SCRIPT_PATH_NAME}"""""" %*");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Power-saving\Default Power-saving (default).cmd");

            _powerSavingConfigurationStore.CurrentSetting = IsEnabled();

        }

        public void Enable()
        {
            CommandPromptHelper.RunCommand($@"powershell -EP Bypass -NoP ^& """"""$env:{DEFAULT_POWER_SAVING_SCRIPT_PATH_NAME}"""""" %*");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Power-saving\Disable Power-saving.cmd");

            _powerSavingConfigurationStore.CurrentSetting = IsEnabled();

        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
