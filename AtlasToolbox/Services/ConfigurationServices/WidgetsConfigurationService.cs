using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services.ConfigurationServices
{
    class WidgetsConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Widgets";
        private const string STATE_VALUE_NAME = "state";

        private const string WINDOWS_FEED_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Feeds";
        private const string DSH_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Dsh";

        private const string ENABLE_FIELDS_VALUE_NAME = "EnableFeeds";
        private const string ALLOW_NEWS_AND_INTERESTS_VALUE_NAME = "AllowNewsAndInterests";

        private readonly ConfigurationStore _widgetsConfigurationService;

        public WidgetsConfigurationService(
            [FromKeyedServices("Widgets")] ConfigurationStore WidgetsConfigurationService)
        {
            _widgetsConfigurationService = WidgetsConfigurationService;
        }


        public void Disable()
        {
            RegistryHelper.SetValue(WINDOWS_FEED_KEY_NAME, ENABLE_FIELDS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DSH_KEY_NAME, ALLOW_NEWS_AND_INTERESTS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Widgets (News and Interests)\Disable Widgets (default).cmd");

            CommandPromptHelper.RestartExplorer();

            _widgetsConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(WINDOWS_FEED_KEY_NAME, ENABLE_FIELDS_VALUE_NAME);
            RegistryHelper.DeleteValue(DSH_KEY_NAME, ALLOW_NEWS_AND_INTERESTS_VALUE_NAME);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Widgets (News and Interests)\Enable Widgets.cmd");

            CommandPromptHelper.RestartExplorer();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
