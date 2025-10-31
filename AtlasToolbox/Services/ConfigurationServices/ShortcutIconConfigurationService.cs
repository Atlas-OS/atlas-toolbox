using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class ShortcutIconConfigurationService : IMultiOptionConfigurationServices
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\ShortcutIcon";
        private const string STATE_VALUE_NAME = "state";


        private const string SHORTCUT_ICON_REG_FILE_PATH = "C:\\Windows\\AtlasModues\\Scripts\\ConfigurationServices\\ShortcutIcon\\ShortcutIcon_";

        private List<string> options = new List<string>()
        {
            "Default Windows",
            "Classic",
            "None (security risk)",
        };

        private readonly MultiOptionConfigurationStore _shortcutIconConfigurationService;

        public ShortcutIconConfigurationService(
            [FromKeyedServices("ShortcutIcon")] MultiOptionConfigurationStore shortcutIconConfigurationService)
        {
            _shortcutIconConfigurationService = shortcutIconConfigurationService;
            _shortcutIconConfigurationService.Options = options;
        }

        public void ChangeStatus(int status)
        {
            RegistryHelper.MergeRegFile(SHORTCUT_ICON_REG_FILE_PATH + status.ToString() + ".reg");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, status);

            _shortcutIconConfigurationService.CurrentSetting = Status();
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
