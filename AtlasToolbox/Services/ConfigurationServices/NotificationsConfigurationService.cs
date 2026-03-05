using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class NotificationsConfigurationService : IRoutable
    {
        private const string WPN_SERVICE_SERVICE_NAME = "WpnService";

        private const string SETTINGS_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings";
        private const string PUSH_NOTIFICATIONS_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\PushNotifications";
        private const string POLICIES_PUSH_NOTIFICATIONS_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\PushNotifications";
        private const string EXPLORER_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Explorer";

        private const string NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND_VALUE_NAME = "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND";
        private const string TOAST_ENABLED_VALUE_NAME = "ToastEnabled";
        private const string NO_CLOUD_APPLICATION_NOTOFICATION_VALUE_NAME = "NoCloudApplicationNotification";
        private const string DISABLE_NOTIFICATION_CENTER_VALUE_NAME = "DisableNotificationCenter";

        private readonly ConfigurationStore _notificationsConfigurationStore;

        public NotificationsConfigurationService(
            [FromKeyedServices("Notifications")] ConfigurationStore notificationsConfigurationStore)
        {
            _notificationsConfigurationStore = notificationsConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(WPN_SERVICE_SERVICE_NAME, ServiceStartMode.Disabled);

            RegistryHelper.SetValue(SETTINGS_KEY_NAME, NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND_VALUE_NAME, 0);
            RegistryHelper.SetValue(PUSH_NOTIFICATIONS_KEY_NAME, TOAST_ENABLED_VALUE_NAME, 0);
            RegistryHelper.SetValue(POLICIES_PUSH_NOTIFICATIONS_KEY_NAME, NO_CLOUD_APPLICATION_NOTOFICATION_VALUE_NAME, 1);
            RegistryHelper.SetValue(EXPLORER_KEY_NAME, DISABLE_NOTIFICATION_CENTER_VALUE_NAME, 1);

            _notificationsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(WPN_SERVICE_SERVICE_NAME, ServiceStartMode.Automatic);

            RegistryHelper.DeleteValue(SETTINGS_KEY_NAME, NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND_VALUE_NAME);
            RegistryHelper.DeleteValue(PUSH_NOTIFICATIONS_KEY_NAME, TOAST_ENABLED_VALUE_NAME);
            RegistryHelper.DeleteValue(POLICIES_PUSH_NOTIFICATIONS_KEY_NAME, NO_CLOUD_APPLICATION_NOTOFICATION_VALUE_NAME);
            RegistryHelper.DeleteValue(EXPLORER_KEY_NAME, DISABLE_NOTIFICATION_CENTER_VALUE_NAME);

            _notificationsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                ServiceHelper.IsStartupTypeMatch(WPN_SERVICE_SERVICE_NAME, ServiceStartMode.Automatic),
                RegistryHelper.IsMatch(SETTINGS_KEY_NAME, NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND_VALUE_NAME, null),
                RegistryHelper.IsMatch(PUSH_NOTIFICATIONS_KEY_NAME, TOAST_ENABLED_VALUE_NAME, null),
                RegistryHelper.IsMatch(POLICIES_PUSH_NOTIFICATIONS_KEY_NAME, NO_CLOUD_APPLICATION_NOTOFICATION_VALUE_NAME, null),
                RegistryHelper.IsMatch(EXPLORER_KEY_NAME, DISABLE_NOTIFICATION_CENTER_VALUE_NAME, null)
            };

            return checks.All(x => x);
        }
    }
}
