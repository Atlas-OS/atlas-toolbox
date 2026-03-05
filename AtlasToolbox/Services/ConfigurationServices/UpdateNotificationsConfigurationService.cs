using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class UpdateNotificationsConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\UpdateNotifications";
        private const string STATE_VALUE_NAME = "state";

        private const string WINDOWS_UPADTE_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string UX_SETTINGS_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings";

        private const string SET_AUTO_RESTART_NOTIFICATION_DISABLE_VALUE_NAME = "SetAutoRestartNotificationDisable";
        private const string RESTART_NOTIFICATIONS_ALLOWED2 = "RestartNotificationsAllowed2";
        private const string SET_UPDATE_NOTIFICATION_LEVEL = "SetUpdateNotificationLevel";

        private readonly ConfigurationStore _updateNotificationsConfigurationService;

        public UpdateNotificationsConfigurationService(
            [FromKeyedServices("UpdateNotifications")] ConfigurationStore UpdateNotificationsConfigurationService)
        {
            _updateNotificationsConfigurationService = UpdateNotificationsConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(WINDOWS_UPADTE_KEY_NAME, SET_AUTO_RESTART_NOTIFICATION_DISABLE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(WINDOWS_UPADTE_KEY_NAME, SET_UPDATE_NOTIFICATION_LEVEL, 2, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(UX_SETTINGS_KEY_NAME, RESTART_NOTIFICATIONS_ALLOWED2, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Update Notifications\Disable Update Notifications.cmd");

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _updateNotificationsConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(WINDOWS_UPADTE_KEY_NAME, SET_AUTO_RESTART_NOTIFICATION_DISABLE_VALUE_NAME);
            RegistryHelper.DeleteValue(WINDOWS_UPADTE_KEY_NAME, SET_UPDATE_NOTIFICATION_LEVEL);
            RegistryHelper.DeleteValue(UX_SETTINGS_KEY_NAME, RESTART_NOTIFICATIONS_ALLOWED2);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Update Notifications\Enable Update Notifications (default).cmd");

            _updateNotificationsConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
