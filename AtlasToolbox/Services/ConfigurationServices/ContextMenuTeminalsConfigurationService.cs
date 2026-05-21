using System.Collections.Generic;
using System;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class ContextMenuTeminalsConfigurationService : IMultiOptionConfigurationServices
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\ContextMenuTerminals";
        private const string STATE_VALUE_NAME = "state";

        private readonly MultiOptionConfigurationStore _contextMenuTeminalsConfigurationService;

        private static readonly string CONTEXT_MENU_REG_FILE_PATH = @$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\ConfigurationServices\ContextMenuTerminals\ContextMenuTerminals_";

        private List<string> options = new List<string>()
        {
            "Remove terminals from the context menu",
            "Add terminals",
            "Add terminals (no Windows Terminal)",
        };

        public ContextMenuTeminalsConfigurationService(
            [FromKeyedServices("ContextMenuTerminals")] MultiOptionConfigurationStore contextMenuTeminalsConfigurationService)
        {
            _contextMenuTeminalsConfigurationService = contextMenuTeminalsConfigurationService;
            _contextMenuTeminalsConfigurationService.Options = options;
        }

        public void ChangeStatus(int status)
        {
            RegistryHelper.MergeRegFile(CONTEXT_MENU_REG_FILE_PATH + status.ToString() + ".reg");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, status);

            _contextMenuTeminalsConfigurationService.CurrentSetting = Status();
        }

        public string Status()
        {
            try
            {
                return options[((int)RegistryHelper.GetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME))];
            }
            catch
            {
                ChangeStatus(0);
                return options[((int)RegistryHelper.GetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME))];

            }
        }
    }
}
