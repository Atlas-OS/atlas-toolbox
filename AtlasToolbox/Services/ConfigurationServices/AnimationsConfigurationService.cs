using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace AtlasToolbox.Services.ConfigurationServices
{
    public class AnimationsConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Animation";
        private const string STATE_VALUE_NAME = "state";


        private const string DWM_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\DWM";
        private const string WINDOW_METRICS_KEY_NAME = @"HKCU\Control Panel\Desktop\WindowMetrics";
        private const string ADVANCED_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string VISUAL_EFFECTS_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects";
        private const string DESKTOP_KEY_NAME = @"HKCU\Control Panel\Desktop";

        private const string ENABLE_AERO_PEEK_VALUE_NAME = "EnableAeroPeek";
        private const string ALWAYS_HIBERNATE_THUMBNAILS_VALUE_NAME = "AlwaysHibernateThumbnails";
        private const string MIN_ANIMATE_VALUE_NAME = "MinAnimate";
        private const string TASKBAR_ANIMATIONS_VALUE_NAME = "TaskbarAnimations";
        private const string LIST_VIEW_ALPHA_SELECT_VALUE_NAME = "ListViewAlphaSelect";
        private const string ICONS_ONLY_VALUE_NAME = "IconsOnly";
        private const string LISTVIEW_SHADOW_VALUE_NAME = "ListviewShadow";
        private const string VISUAL_FX_SETTING_VALUE_NAME = "VisualFXSetting";
        private const string USER_PREFERENCES_MASK_VALUE_NAME = "UserPreferencesMask";
        private const string FONT_SMOOTHING_VALUE_NAME = "FontSmoothing";
        private const string DRAG_FULL_WINDOWS_VALUE_NAME = "DragFullWindows";



        private readonly ConfigurationStore _animationsConfigurationStore;

        public AnimationsConfigurationService(
            [FromKeyedServices("Animations")] ConfigurationStore animationsConfigurationStore)
        {
            _animationsConfigurationStore = animationsConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(DESKTOP_KEY_NAME, FONT_SMOOTHING_VALUE_NAME, 2, Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(DESKTOP_KEY_NAME, USER_PREFERENCES_MASK_VALUE_NAME,
                Convert.FromHexString("9012038010000000"), Microsoft.Win32.RegistryValueKind.Binary);
            RegistryHelper.SetValue(DESKTOP_KEY_NAME, DRAG_FULL_WINDOWS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(WINDOW_METRICS_KEY_NAME, MIN_ANIMATE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, LIST_VIEW_ALPHA_SELECT_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, ICONS_ONLY_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, TASKBAR_ANIMATIONS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, LISTVIEW_SHADOW_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(VISUAL_EFFECTS_KEY_NAME, VISUAL_FX_SETTING_VALUE_NAME, 3, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, ENABLE_AERO_PEEK_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, ALWAYS_HIBERNATE_THUMBNAILS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Visual Effects (Animations)\Atlas Visual Effects (default).cmd");

            App.ContentDialogCaller("logoff");

            _animationsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(DESKTOP_KEY_NAME, FONT_SMOOTHING_VALUE_NAME, 2, Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(DESKTOP_KEY_NAME, USER_PREFERENCES_MASK_VALUE_NAME,
                Convert.FromHexString("9E1E078012000000"), Microsoft.Win32.RegistryValueKind.Binary);
            RegistryHelper.SetValue(DESKTOP_KEY_NAME, DRAG_FULL_WINDOWS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(WINDOW_METRICS_KEY_NAME, MIN_ANIMATE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, LIST_VIEW_ALPHA_SELECT_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, ICONS_ONLY_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, TASKBAR_ANIMATIONS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, LISTVIEW_SHADOW_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(VISUAL_EFFECTS_KEY_NAME, VISUAL_FX_SETTING_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, ENABLE_AERO_PEEK_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, ALWAYS_HIBERNATE_THUMBNAILS_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Visual Effects (Animations)\Default Windows Visual Effects.cmd");

            App.ContentDialogCaller("logoff");

            _animationsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
