using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Linq;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class DarkTitlebarsConfigurationService : IRoutable
    {
        private const string DWM_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\DWM";
        private const string ACCENT_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent";

        private const string COLORIZATION_COLOR_VALUE_NAME = "ColorizationColor";
        private const string COLORIZATION_AFTERGLOW_VALUE_NAME = "ColorizationAfterglow";
        private const string ACCENT_COLOR_VALUE_NAME = "AccentColor";
        private const string COLOR_PREVALENCE_VALUE_NAME = "ColorPrevalence";
        private const string ACCENT_COLOR_INACTIVE_VALUE_NAME = "AccentColorInactive";
        private const string ACCENT_COLOR_MENU_VALUE_NAME = "AccentColorMenu";

        private readonly ConfigurationStore _darkTitlebarsConfigurationStore;

        public DarkTitlebarsConfigurationService(
            [FromKeyedServices("DarkTitlebars")] ConfigurationStore darkTitlebarsConfigurationStore)
        {
            _darkTitlebarsConfigurationStore = darkTitlebarsConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(DWM_KEY_NAME, COLORIZATION_COLOR_VALUE_NAME, 0xc40078d7, RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, COLORIZATION_AFTERGLOW_VALUE_NAME, 0xc40078d7, RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, ACCENT_COLOR_VALUE_NAME, 0xffd77800, RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, COLOR_PREVALENCE_VALUE_NAME, 0x0, RegistryValueKind.DWord);
            RegistryHelper.DeleteValue(DWM_KEY_NAME, ACCENT_COLOR_INACTIVE_VALUE_NAME);
            RegistryHelper.SetValue(ACCENT_KEY_NAME, ACCENT_COLOR_MENU_VALUE_NAME, 0xffd77800, RegistryValueKind.DWord);

            _darkTitlebarsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(DWM_KEY_NAME, COLORIZATION_COLOR_VALUE_NAME, 0xc4000000, RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, COLORIZATION_AFTERGLOW_VALUE_NAME, 0xc4000000, RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, ACCENT_COLOR_VALUE_NAME, 0xff000000, RegistryValueKind.DWord);
            RegistryHelper.SetValue(DWM_KEY_NAME, COLOR_PREVALENCE_VALUE_NAME, 1);
            RegistryHelper.SetValue(DWM_KEY_NAME, ACCENT_COLOR_INACTIVE_VALUE_NAME, 0xff2b2b2b, RegistryValueKind.DWord);
            RegistryHelper.SetValue(ACCENT_KEY_NAME, ACCENT_COLOR_MENU_VALUE_NAME, 0xc4000000, RegistryValueKind.DWord);

            _darkTitlebarsConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                RegistryHelper.IsMatch(DWM_KEY_NAME, COLORIZATION_COLOR_VALUE_NAME, 0xc4000000),
                RegistryHelper.IsMatch(DWM_KEY_NAME, COLORIZATION_AFTERGLOW_VALUE_NAME, 0xc4000000),
                RegistryHelper.IsMatch(DWM_KEY_NAME, ACCENT_COLOR_VALUE_NAME, 0xff000000),
                RegistryHelper.IsMatch(DWM_KEY_NAME, COLOR_PREVALENCE_VALUE_NAME, 1),
                RegistryHelper.IsMatch(DWM_KEY_NAME, ACCENT_COLOR_INACTIVE_VALUE_NAME, 0xff2b2b2b),
                RegistryHelper.IsMatch(ACCENT_KEY_NAME, ACCENT_COLOR_MENU_VALUE_NAME, 0xc4000000)
            };

            return checks.All(x => x);
        }
    }
}
