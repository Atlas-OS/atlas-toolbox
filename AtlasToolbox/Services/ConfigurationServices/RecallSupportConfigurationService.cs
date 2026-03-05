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
    class RecallSupportConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\recall";
        private const string STATE_VALUE_NAME = "state";

        private const string WINDOWS_AI_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

        private readonly ConfigurationStore _recallConfigurationStore;

        public RecallSupportConfigurationService([FromKeyedServices("Recall")] ConfigurationStore RecallConfigurationStore)
        {
            _recallConfigurationStore = RecallConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\AI Features\Microsoft Copilot\Disable Recall Support (default).cmd");

            RegistryHelper.SetValue(WINDOWS_AI_KEY_NAME, "DisableAIDataAnalysis", 01, Microsoft.Win32.RegistryValueKind.DWord);
            _recallConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\AI Features\Microsoft Copilot\Enable Recall Support.cmd");

            RegistryHelper.DeleteValue(WINDOWS_AI_KEY_NAME, "DisableAIDataAnalysis");

            _recallConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
