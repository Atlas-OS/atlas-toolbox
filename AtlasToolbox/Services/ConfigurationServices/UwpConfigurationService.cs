using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class UwpConfigurationService : IRoutable
    {
        private const string BFE_SERVICE_NAME = "BFE";
        private const string MPS_SVC_SERVICE_NAME = "mpssvc";
        private const string TABLET_INPUT_SERVICE_SERVICE_NAME = "TabletInputService";
        private const string TEXT_INPUT_MANAGEMENT_SERVICE_SERVICE_NAME = "TextInputManagementService";

        private const string START_MENU_EXPERIENCE_HOST_PROCESS_NAME = "StartMenuExperienceHost%";
        private const string SEARCH_APP_PROCESS_NAME = "SearchApp%";
        private const string RUNTIME_BROKER_PROCESS_NAME = "RuntimeBroker%";
        private const string EXPLORER_PROCESS_NAME = "explorer.exe";

        private const string SEARCH_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";

        private const string SEARCHBOX_TASKBAR_MODE_VALUE_NAME = "SearchboxTaskbarMode";

        private readonly ConfigurationStore _uwpConfigurationStore;
        private readonly ConfigurationStore _microsoftStoreConfigurationStore;
        private readonly IRoutable _microsoftStoreConfigurationService;
        private readonly string _systemDirectory;
        private readonly string _systemAppsDirectory;

        public UwpConfigurationService(
            [FromKeyedServices("Uwp")] ConfigurationStore uwpConfigurationStore,
            [FromKeyedServices("MicrosoftStore")] ConfigurationStore microsoftStoreConfigurationStore,
            [FromKeyedServices("MicrosoftStore")] IRoutable microsoftStoreConfigurationService)
        {
            _uwpConfigurationStore = uwpConfigurationStore;
            _microsoftStoreConfigurationStore = microsoftStoreConfigurationStore;
            _microsoftStoreConfigurationService = microsoftStoreConfigurationService;

            _microsoftStoreConfigurationStore.CurrentSettingChanged += MicrosoftStoreConfigurationStore_CurrentSettingChanged;

            _systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
            _systemAppsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SystemApps");
        }

        public void Disable()
        {
            if (_microsoftStoreConfigurationStore.CurrentSetting)
            {
                _microsoftStoreConfigurationService.Disable();
            }

            ServiceHelper.SetStartupType(BFE_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(MPS_SVC_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(TABLET_INPUT_SERVICE_SERVICE_NAME, ServiceStartMode.Disabled);

            ProcessHelper.KillProcessByName(START_MENU_EXPERIENCE_HOST_PROCESS_NAME);

            FileSystemHelper.AppendLastCharacterToDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy"));

            ProcessHelper.KillProcessByName(SEARCH_APP_PROCESS_NAME);

            FileSystemHelper.AppendLastCharacterToDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.Windows.Search_cw5n1h2txyewy"));
            FileSystemHelper.AppendLastCharacterToDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.XboxGameCallableUI_cw5n1h2txyewy"));
            FileSystemHelper.AppendLastCharacterToDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.XboxApp_48.49.31001.0_x64__8wekyb3d8bbwe"));

            ProcessHelper.KillProcessByName(RUNTIME_BROKER_PROCESS_NAME);

            FileSystemHelper.AppendLastCharacterToFileName(
                Path.Combine(_systemDirectory, "RuntimeBroker.exe"));

            RegistryHelper.SetValue(SEARCH_KEY_NAME, SEARCHBOX_TASKBAR_MODE_VALUE_NAME, 0);

            ProcessHelper.KillProcessByName(EXPLORER_PROCESS_NAME);

            _uwpConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            if (!_microsoftStoreConfigurationStore.CurrentSetting)
            {
                _microsoftStoreConfigurationService.Enable();
            }

            ServiceHelper.SetStartupType(BFE_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(MPS_SVC_SERVICE_NAME, ServiceStartMode.Automatic);
            ServiceHelper.SetStartupType(TABLET_INPUT_SERVICE_SERVICE_NAME, ServiceStartMode.Manual);

            ProcessHelper.KillProcessByName(START_MENU_EXPERIENCE_HOST_PROCESS_NAME);

            FileSystemHelper.TrimLastCharacterFromDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewyy"));

            ProcessHelper.KillProcessByName(SEARCH_APP_PROCESS_NAME);

            FileSystemHelper.TrimLastCharacterFromDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.Windows.Search_cw5n1h2txyewyy"));
            FileSystemHelper.TrimLastCharacterFromDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.XboxGameCallableUI_cw5n1h2txyewyy"));
            FileSystemHelper.TrimLastCharacterFromDirectoryName(
                Path.Combine(_systemAppsDirectory, "Microsoft.XboxApp_48.49.31001.0_x64__8wekyb3d8bbwee"));

            ProcessHelper.KillProcessByName(RUNTIME_BROKER_PROCESS_NAME);

            FileSystemHelper.TrimLastCharacterFromFileName(
                Path.Combine(_systemDirectory, "RuntimeBroker.exee"));

            RegistryHelper.DeleteValue(SEARCH_KEY_NAME, SEARCHBOX_TASKBAR_MODE_VALUE_NAME);

            ProcessHelper.KillProcessByName(EXPLORER_PROCESS_NAME);

            _uwpConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            OperatingSystem os = Environment.OSVersion;
            if (os.Version.ToString() == "10.0.19045.0")
            {
                bool[] checks =
                {
                    _microsoftStoreConfigurationService.IsEnabled(),
                    ServiceHelper.IsStartupTypeMatch(BFE_SERVICE_NAME, ServiceStartMode.Automatic),
                    ServiceHelper.IsStartupTypeMatch(MPS_SVC_SERVICE_NAME, ServiceStartMode.Automatic),
                    ServiceHelper.IsStartupTypeMatch(TABLET_INPUT_SERVICE_SERVICE_NAME, ServiceStartMode.Manual),
                    RegistryHelper.IsMatch(SEARCH_KEY_NAME, SEARCHBOX_TASKBAR_MODE_VALUE_NAME, null)
                };
                return checks.All(x => x);
            }
            else
            {
                bool[] checks =
                {
                    _microsoftStoreConfigurationService.IsEnabled(),
                    ServiceHelper.IsStartupTypeMatch(BFE_SERVICE_NAME, ServiceStartMode.Automatic),
                    ServiceHelper.IsStartupTypeMatch(MPS_SVC_SERVICE_NAME, ServiceStartMode.Automatic),
                    ServiceHelper.IsStartupTypeMatch(TEXT_INPUT_MANAGEMENT_SERVICE_SERVICE_NAME, ServiceStartMode.Manual),
                    RegistryHelper.IsMatch(SEARCH_KEY_NAME, SEARCHBOX_TASKBAR_MODE_VALUE_NAME, null)
                };
                return checks.All(x => x);
            }
        }

        private void MicrosoftStoreConfigurationStore_CurrentSettingChanged()
        {
            _uwpConfigurationStore.CurrentSetting = IsEnabled();
        }
    }
}
