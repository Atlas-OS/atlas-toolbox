using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Windows.Devices.PointOfService;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class FileSharingConfigurationService : IConfigurationService
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\FileSharing";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _configurationStore;
        public FileSharingConfigurationService(
            [FromKeyedServices("FileSharing")] ConfigurationStore configurationStore)
        {
            _configurationStore = configurationStore;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\ConfigurationServices\FIleSharing\disable.cmd", false);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\File Sharing\Disable File Sharing (default).cmd");

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\ConfigurationServices\FIleSharing\enable.cmd", false);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\File Sharing\Enable File Sharing.cmd");

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
