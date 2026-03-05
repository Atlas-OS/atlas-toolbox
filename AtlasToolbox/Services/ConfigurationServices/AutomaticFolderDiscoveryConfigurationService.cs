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
    public class AutomaticFolderDiscoveryConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\AutomaticFolderDiscovery";
        private const string STATE_VALUE_NAME = "state";

        private const string SHELL_KEY_NAME = @"HKCU\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell";

        private const string FOLDER_TYPE_VALUE_NAME = "FolderType";

        private readonly ConfigurationStore _automaticFolderDiscoveryConfigurationService;

        public AutomaticFolderDiscoveryConfigurationService(
            [FromKeyedServices("AutomaticFolderDiscovery")] ConfigurationStore automaticFolderDiscoveryConfigurationService)
        {
            _automaticFolderDiscoveryConfigurationService = automaticFolderDiscoveryConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.DeleteValue(SHELL_KEY_NAME, FOLDER_TYPE_VALUE_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\Automatic Folder Discovery\Disable Automatic Folder Discovery (default).cmd");

            _automaticFolderDiscoveryConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(SHELL_KEY_NAME, FOLDER_TYPE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\Automatic Folder Discovery\Enable Automatic Folder Discovery.cmd");

            _automaticFolderDiscoveryConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
