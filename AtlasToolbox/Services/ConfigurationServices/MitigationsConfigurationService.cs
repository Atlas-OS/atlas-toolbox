using System.Collections.Generic;
using System;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class MitigationsConfigurationService : IMultiOptionConfigurationServices
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Mitigations";
        private const string STATE_VALUE_NAME = "state";

        private readonly MultiOptionConfigurationStore _mitigationsConfigurationService;

        private static readonly string MITIGATIONS_SCRIPT_PATH = @$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\ConfigurationServices\Mitigations\Mitigations_";

        private List<string> options = new List<string>()
        {
            "Disable mitigations",
            "Default Windows mitigations",
            "Enable all mitigations",
        };

        public MitigationsConfigurationService(
            [FromKeyedServices("Mitigations")] MultiOptionConfigurationStore mitigationsConfigurationService)
        {
            _mitigationsConfigurationService = mitigationsConfigurationService;
            _mitigationsConfigurationService.Options = options;
        }

        public void ChangeStatus(int status)
        {
            ProcessHelper.StartShellExecute(MITIGATIONS_SCRIPT_PATH + status.ToString() + ".cmd");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, status);

            _mitigationsConfigurationService.CurrentSetting = Status();
        }

        public string Status()
        {
            try
            {
                return options[((int)RegistryHelper.GetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME))];
            }
            catch
            {
                ChangeStatus(1);
                return options[((int)RegistryHelper.GetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME))];
            }
        }
    }
}
