using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using WinRT;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class ClickToDoConfigurationService : IConfigurationService
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\ClickToDo";
        private const string STATE_VALUE_NAME = "state";

        private const string WINDOWS_AI_KEY_NAME_USER = @"HKCU\Software\Policies\Microsoft\Windows\WindowsAI";
        private const string WINDOWS_AI_KEY_NAME_MACHINE = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

        private const string DISABLE_CLICK_TO_DO_VALUE_NAME = "DisableClickToDo";

        private readonly ConfigurationStore _ClickToDoConfigurationService;

        public ClickToDoConfigurationService(
            [FromKeyedServices("ClickToDo")]  ConfigurationStore ClickToDoConfigurationService)
        {
            _ClickToDoConfigurationService = ClickToDoConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            RegistryHelper.DeleteValue(WINDOWS_AI_KEY_NAME_USER, DISABLE_CLICK_TO_DO_VALUE_NAME);
            RegistryHelper.SetValue(WINDOWS_AI_KEY_NAME_MACHINE, DISABLE_CLICK_TO_DO_VALUE_NAME, 1);

            _ClickToDoConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            RegistryHelper.DeleteValue(WINDOWS_AI_KEY_NAME_USER, DISABLE_CLICK_TO_DO_VALUE_NAME);
            RegistryHelper.DeleteValue(WINDOWS_AI_KEY_NAME_MACHINE, DISABLE_CLICK_TO_DO_VALUE_NAME);

            _ClickToDoConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
