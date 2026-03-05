using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class WindowsSpotlightConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\WindowsSpotlight";
        private const string STATE_VALUE_NAME = "state";

        private const string CLOUD_CONTENT_LOCAL_MACHINE_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
        private const string CLOUD_CONTENT_CURRENT_USER_KEY_NAME = @"HKCU\SOFTWARE\Policies\Microsoft\Windows\CloudContent";
        private const string CONTENT_DELIVERY_MANGER_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager";
        private const string NEW_START_PANEL_KEY_NAME = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\HideDesktopIcons\NewStartPanel";
        private const string BACKGROUND_ACCESS_APPLICATIONS_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications";
        private const string APP_PRIVACY_KEY_NAME = @"HKLM\Software\Policies\Microsoft\Windows\AppPrivacy";
        private const string SEARCH_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search";

        private const string DISABLE_CLOUD_OPTIMIZED_CONTENT_VALUE_NAME = "DisableCloudOptimizedContent";
        private const string DISABLE_WINDOWS_SPOTLIGHT_FEATURES_VALUE_NAME = "DisableWindowsSpotlightFeatures";
        private const string DISABLE_WINDOWS_SPOTLIGHT_WINDOWS_WELCOME_EXPERIENCE_VALUE_NAME = "DisableWindowsSpotlightWindowsWelcomeExperience";
        private const string DISABLE_WINDOWS_SPOTLIGHT_ON_ACTION_CENTER_VALUE_NAME = "DisableWindowsSpotlightOnActionCenter";
        private const string DISABLE_WINDOWS_SPOTLIGHT_ON_SETTINGS_VALUE_NAME = "DisableWindowsSpotlightOnSettings";
        private const string DISABLE_THIRD_PARTY_SUGGESTIONS_VALUE_NAME = "DisableThirdPartySuggestions";
        private const string CONTENT_DELIVERY_ALLOWED_VALUE_NAME = "ContentDeliveryAllowed";
        private const string FEATURE_MANAGEMENT_ENABLED_VALUE_NAME = "FeatureManagementEnabled";
        private const string SUBSCRIBED_CONTENT_ENABLED_VALUE_NAME = "SubscribedContentEnabled";
        private const string SUBSCRIBED_CONTENT_NUMBERS_ENABLED_VALUE_NAME = "SubscribedContent-338387Enabled";
        private const string ROTATING_LOCK_SCREEN_OVERLAY_ENABLED_VALUE_NAME = "RotatingLockScreenOverlayEnabled";
        private const string NEW_START_PANEL_NUMBERS_VALUE_NAME = "{2cc5ca98-6485-489a-920e-b3e88a6ccce3}";
        private const string DISABLE_SOFT_LANDING_VALUE_NAME = "DisableSoftLanding";
        private const string BACKGROUND_APP_GLOBAL_TOGGLE_VALUE_NAME = "BackgroundAppGlobalToggle";
        private const string LET_APPS_RUN_IN_BACKGROUND_VALUE_NAME = "LetAppsRunInBackground";
        private const string GLOBAL_USER_DISABLED_VALUE_NAME = "GlobalUserDisabled";

        private readonly ConfigurationStore _windowsSpotlightConfigurationService;

        public WindowsSpotlightConfigurationService(
            [FromKeyedServices("WindowsSpotlight")] ConfigurationStore WindowsSpotlightConfigurationService)
        {
            _windowsSpotlightConfigurationService = WindowsSpotlightConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(CLOUD_CONTENT_LOCAL_MACHINE_KEY_NAME, DISABLE_CLOUD_OPTIMIZED_CONTENT_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_FEATURES_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_WINDOWS_WELCOME_EXPERIENCE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_ON_ACTION_CENTER_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_ON_SETTINGS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_THIRD_PARTY_SUGGESTIONS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, CONTENT_DELIVERY_ALLOWED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, FEATURE_MANAGEMENT_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, SUBSCRIBED_CONTENT_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, SUBSCRIBED_CONTENT_NUMBERS_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, ROTATING_LOCK_SCREEN_OVERLAY_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(NEW_START_PANEL_KEY_NAME, NEW_START_PANEL_NUMBERS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Windows Spotlight\Disable Windows Spotlight (default).cmd");

            _windowsSpotlightConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_FEATURES_VALUE_NAME);
            RegistryHelper.DeleteValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_WINDOWS_WELCOME_EXPERIENCE_VALUE_NAME);
            RegistryHelper.DeleteValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_ON_ACTION_CENTER_VALUE_NAME);
            RegistryHelper.DeleteValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_WINDOWS_SPOTLIGHT_ON_SETTINGS_VALUE_NAME);
            RegistryHelper.DeleteValue(CLOUD_CONTENT_CURRENT_USER_KEY_NAME, DISABLE_THIRD_PARTY_SUGGESTIONS_VALUE_NAME);

            RegistryHelper.DeleteValue(CLOUD_CONTENT_LOCAL_MACHINE_KEY_NAME, DISABLE_SOFT_LANDING_VALUE_NAME);

            RegistryHelper.DeleteValue(SEARCH_KEY_NAME, BACKGROUND_APP_GLOBAL_TOGGLE_VALUE_NAME);

            RegistryHelper.DeleteValue(BACKGROUND_ACCESS_APPLICATIONS_KEY_NAME, GLOBAL_USER_DISABLED_VALUE_NAME);

            RegistryHelper.DeleteValue(APP_PRIVACY_KEY_NAME, LET_APPS_RUN_IN_BACKGROUND_VALUE_NAME);

            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, CONTENT_DELIVERY_ALLOWED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, FEATURE_MANAGEMENT_ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, SUBSCRIBED_CONTENT_ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, SUBSCRIBED_CONTENT_NUMBERS_ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(CONTENT_DELIVERY_MANGER_KEY_NAME, ROTATING_LOCK_SCREEN_OVERLAY_ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            RegistryHelper.SetValue(NEW_START_PANEL_KEY_NAME, NEW_START_PANEL_NUMBERS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Windows Spotlight\Enable Windows Spotlight.cmd");

            _windowsSpotlightConfigurationService.CurrentSetting = IsEnabled();

        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
