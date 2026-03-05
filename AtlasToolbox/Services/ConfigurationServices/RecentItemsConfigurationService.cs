using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class RecentItemsConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\RecentItems";
        private const string STATE_VALUE_NAME = "state";

        private const string EXPLORER_POLICIES_HKCU_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string EXPLORER_POLICIES_HKLM_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";
        private const string EXPLORER_WINDOWS_HKCU_KEY_NAME = @"HKCU\SOFTWARE\Policies\Microsoft\Windows\Explorer";
        private const string EXPLORER_WINDOWS_HKLM_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Explorer";
        private const string ADVANCED_EXPLORER_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

        private const string NO_INSTRUMENTATION_VALUE_NAME = "NoInstrumentation";
        private const string CLEAR_RECENT_DOCS_ON_EXIT_VALUE_NAME = "ClearRecentDocOnExit";
        private const string NO_RECENT_DOCS_HISTORY_VALUE_NAME = "NoRecentDocsHistory";
        private const string NO_REMOTE_DESTINATIONS_VALUE_NAME = "NoRemoteDestinations";
        private const string NO_START_MENU_MFU_PROGRAMS_LIST_VALUE_NAME = "NoStartMenuMFUprogramsList";
        private const string SHOW_OR_HIDE_MOST_USED_APPS_VALUE_NAME = "ShowOrHideMostUsedApps";
        private const string HIDE_RECENTLY_ADDED_APPS_VALUE_NAME = "HideRecentlyAddedApps";
        private const string START_TRACK_PROGS_VALUE_NAME = "Start_TrackProgs";
        private const string START_TRACK_DOCS_VALUE_NAME = "Start_TrackDocs";

        private readonly ConfigurationStore _recentItemsConfigurationService;

        public RecentItemsConfigurationService(
            [FromKeyedServices("RecentItems")] ConfigurationStore recentItemsConfigurationItems)
        {
            _recentItemsConfigurationService = recentItemsConfigurationItems;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(EXPLORER_POLICIES_HKCU_KEY_NAME, NO_INSTRUMENTATION_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_POLICIES_HKCU_KEY_NAME, CLEAR_RECENT_DOCS_ON_EXIT_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_POLICIES_HKCU_KEY_NAME, NO_RECENT_DOCS_HISTORY_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_WINDOWS_HKCU_KEY_NAME, NO_REMOTE_DESTINATIONS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_POLICIES_HKLM_KEY_NAME, NO_START_MENU_MFU_PROGRAMS_LIST_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_POLICIES_HKLM_KEY_NAME, NO_RECENT_DOCS_HISTORY_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_WINDOWS_HKLM_KEY_NAME, SHOW_OR_HIDE_MOST_USED_APPS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(EXPLORER_WINDOWS_HKLM_KEY_NAME, HIDE_RECENTLY_ADDED_APPS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ADVANCED_EXPLORER_KEY_NAME, START_TRACK_PROGS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_EXPLORER_KEY_NAME, START_TRACK_DOCS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Unlock Recent Items\Disable Recent Items (default).cmd");

            CommandPromptHelper.RunCommand(@"call ""%windir%\AtlasModules\Scripts\settingsPages.cmd"" /hide privacy-general");
            CommandPromptHelper.RestartExplorer();


            _recentItemsConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(EXPLORER_POLICIES_HKCU_KEY_NAME, NO_INSTRUMENTATION_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_POLICIES_HKCU_KEY_NAME, CLEAR_RECENT_DOCS_ON_EXIT_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_POLICIES_HKCU_KEY_NAME, NO_RECENT_DOCS_HISTORY_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_WINDOWS_HKCU_KEY_NAME, NO_REMOTE_DESTINATIONS_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_POLICIES_HKLM_KEY_NAME, NO_START_MENU_MFU_PROGRAMS_LIST_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_POLICIES_HKLM_KEY_NAME, NO_RECENT_DOCS_HISTORY_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_WINDOWS_HKLM_KEY_NAME, SHOW_OR_HIDE_MOST_USED_APPS_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_WINDOWS_HKLM_KEY_NAME, HIDE_RECENTLY_ADDED_APPS_VALUE_NAME);

            RegistryHelper.SetValue(ADVANCED_EXPLORER_KEY_NAME, START_TRACK_PROGS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_EXPLORER_KEY_NAME, START_TRACK_DOCS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Unlock Recent Items\Enable Recent Items.cmd");

            CommandPromptHelper.RunCommand(@"call ""%windir%\AtlasModules\Scripts\settingsPages.cmd"" /unhide privacy-general");
            CommandPromptHelper.RestartExplorer();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
