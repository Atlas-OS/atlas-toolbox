using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Management.Deployment;


namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class CopilotConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\MicrosoftCopilot";
        private const string STATE_VALUE_NAME = "state";


        private readonly ConfigurationStore _copilotConfigurationStore;

        public CopilotConfigurationService([FromKeyedServices("Copilot")] ConfigurationStore copilotConfigurationStore)
        {
            _copilotConfigurationStore = copilotConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\AI Features\Microsoft Copilot\Disable Microsoft Copilot (default).cmd");
            ProcessHelper.StartShellExecute(@$"{Environment.GetEnvironmentVariable("windir")}AtlasModules\Toolbox\Scripts\Copilot\DisableMicrosoftCopilot.cmd");

            _copilotConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\AI Features\Microsoft Copilot\Enable Microsoft Copilot.cmd");

            ProcessHelper.StartShellExecute(@$"{Environment.GetEnvironmentVariable("windir")}AtlasModules\Toolbox\Scripts\Copilot\EnableMicrosoftCopilot.cmd");

            _copilotConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
