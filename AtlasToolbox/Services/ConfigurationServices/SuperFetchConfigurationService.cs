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
    public class SuperFetchConfigurationService : IConfigurationService
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\SuperFetch";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _superFetchConfigurationService;

        public SuperFetchConfigurationService(
            [FromKeyedServices("SuperFetch")] ConfigurationStore superFetchConfigurationService)
        {
            _superFetchConfigurationService = superFetchConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\SuperFetch\DisableSuperFetch.cmd");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Superfetch\Disable SuperFetch.cmd");

            _superFetchConfigurationService.CurrentSetting = IsEnabled();
            App.ContentDialogCaller("restart");
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\SuperFetch\EnableSuperFetch.cmd");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Superfetch\Enable SuperFetch (default).cmd");

            _superFetchConfigurationService.CurrentSetting = IsEnabled();
            App.ContentDialogCaller("restart");
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
