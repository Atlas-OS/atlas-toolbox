using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services.ConfigurationServices
{
    class WebSearchConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\WebSearch";
        private const string STATE_VALUE_NAME = "state";

        private const string WINDOWS_SEARCH_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search";
        private const string SEARCH_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";
        private const string SEARCH_SETTINGS_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings";
        private const string EXPLORER_KEY_NAME = @"HKCU\SOFTWARE\Policies\Microsoft\Windows\Explorer";

        private const string ALLOW_SEARCH_TO_USE_LOCATION_VALUE_NAME = "AllowSearchToUseLocation";
        private const string BING_SEARCH_ENABLED_VALUE_NAME = "BingSearchEnabled";
        private const string IS_AAD_CLOUD_SEARCH_ENABLED_VALUE_NAME = "IsAADCloudSearchEnabled";
        private const string IS_DEVICE_SEARCH_HISTORY_ENABLED_VALUE_NAME = "IsDeviceSearchHistoryEnabled";
        private const string IS_MSA_CLOUD_SEARCH_ENABLED_VALUE_NAME = "IsMSACloudSearchEnabled";
        private const string SAFE_SEARCH_MODE_VALUE_NAME = "SafeSearchMode";
        private const string CONNECTED_SEARCH_USE_WEB_VALUE_NAME = "ConnectedSearchUseWeb";
        private const string DISABLE_WEB_SEARCH = "DisableWebSearch";
        private const string DISABLE_SEARCH_BOX_SUGGESTIONS_VALUE_NAME = "DisableSearchBoxSuggestions";
        private const string SEARCHBOX_TASKBAR_MODE_VALUE_NAME = "SearchboxTaskbarMode";
        private const string ENABLE_DYNAMIC_CONTENT_IN_WSB_VALUE_NAME = "EnableDynamicContentInWSB";
        private const string IS_DYNAMIC_SEARCH_BOX_ENABLED_VALUE_NAME = "IsDynamicSearchBoxEnabled";

        private const string TASKKILL_SEARCHHOST_COMMAND_NAME = "taskkill /f /im SearchHost.exe";
        private const string REMOVE_PACKAGE_BING_COMMAND_NAME = "powershell -NoP -NonI \"Get-AppxPackage -AllUsers Microsoft.BingSearch* | Remove-AppxPackage -AllUsers\"";
        private const string INSTALL_BING_COMMAND_NAME = "winget install -e --id 9NZBF4GT040C --uninstall-previous -h --accept-source-agreements --accept-package-agreements --force --disable-interactivity > nul";

        private readonly ConfigurationStore _webSearchConfigurationService;

        public WebSearchConfigurationService(
            [FromKeyedServices("WebSearch")] ConfigurationStore WebSearchConfigurationService)
        {
            _webSearchConfigurationService = WebSearchConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(WINDOWS_SEARCH_KEY_NAME, ALLOW_SEARCH_TO_USE_LOCATION_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_KEY_NAME, BING_SEARCH_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_SETTINGS_KEY_NAME, IS_AAD_CLOUD_SEARCH_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_SETTINGS_KEY_NAME, IS_DEVICE_SEARCH_HISTORY_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_SETTINGS_KEY_NAME, IS_MSA_CLOUD_SEARCH_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_SETTINGS_KEY_NAME, SAFE_SEARCH_MODE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_SEARCH_KEY_NAME, CONNECTED_SEARCH_USE_WEB_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_SEARCH_KEY_NAME, DISABLE_WEB_SEARCH, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_KEY_NAME, DISABLE_SEARCH_BOX_SUGGESTIONS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_KEY_NAME, SEARCHBOX_TASKBAR_MODE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_SEARCH_KEY_NAME, ENABLE_DYNAMIC_CONTENT_IN_WSB_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_SETTINGS_KEY_NAME, IS_DYNAMIC_SEARCH_BOX_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            CommandPromptHelper.RunCommand(TASKKILL_SEARCHHOST_COMMAND_NAME);
            CommandPromptHelper.RestartExplorer();
            CommandPromptHelper.RunCommand(REMOVE_PACKAGE_BING_COMMAND_NAME);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Web Search (includes Search Highlights)\Disable Web Search (default).cmd");

            _webSearchConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(WINDOWS_SEARCH_KEY_NAME, ALLOW_SEARCH_TO_USE_LOCATION_VALUE_NAME);
            RegistryHelper.DeleteValue(SEARCH_KEY_NAME, BING_SEARCH_ENABLED_VALUE_NAME);
            RegistryHelper.DeleteValue(SEARCH_SETTINGS_KEY_NAME, IS_AAD_CLOUD_SEARCH_ENABLED_VALUE_NAME);
            RegistryHelper.DeleteValue(SEARCH_SETTINGS_KEY_NAME, IS_DEVICE_SEARCH_HISTORY_ENABLED_VALUE_NAME);
            RegistryHelper.DeleteValue(SEARCH_SETTINGS_KEY_NAME, IS_MSA_CLOUD_SEARCH_ENABLED_VALUE_NAME);
            RegistryHelper.DeleteValue(SEARCH_SETTINGS_KEY_NAME, SAFE_SEARCH_MODE_VALUE_NAME);
            RegistryHelper.DeleteValue(WINDOWS_SEARCH_KEY_NAME, CONNECTED_SEARCH_USE_WEB_VALUE_NAME);
            RegistryHelper.DeleteValue(WINDOWS_SEARCH_KEY_NAME, DISABLE_WEB_SEARCH);
            RegistryHelper.DeleteValue(EXPLORER_KEY_NAME, DISABLE_SEARCH_BOX_SUGGESTIONS_VALUE_NAME);
            RegistryHelper.DeleteValue(WINDOWS_SEARCH_KEY_NAME, ENABLE_DYNAMIC_CONTENT_IN_WSB_VALUE_NAME);
            RegistryHelper.SetValue(SEARCH_KEY_NAME, SEARCHBOX_TASKBAR_MODE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SEARCH_SETTINGS_KEY_NAME, IS_DYNAMIC_SEARCH_BOX_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Web Search (includes Search Highlights)\Enable Web Search.cmd");
            CommandPromptHelper.RunCommand(INSTALL_BING_COMMAND_NAME);

            CommandPromptHelper.RunCommand(INSTALL_BING_COMMAND_NAME);
            CommandPromptHelper.RunCommand(TASKKILL_SEARCHHOST_COMMAND_NAME);
            CommandPromptHelper.RestartExplorer();

            _webSearchConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
