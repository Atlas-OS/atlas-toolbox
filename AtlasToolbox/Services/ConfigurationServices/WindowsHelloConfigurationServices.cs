using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class WindowsHelloConfigurationServices : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\WindowsHello";
        private const string STATE_VALUE_NAME = "state";

        private const string PASSPORT_FOR_WORK_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\PassportForWork";
        private const string SYSTEM_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string BIOMETRICS_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Biometrics";
        private const string FACIAL_FEATURES_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Biometrics\FacialFeatures";
        private const string FINGERPRINT_FEATURES_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Biometrics\FingerprintFeatures";
        private const string ALLOW_SIGN_IN_OPTIONS_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\PolicyManager\default\Settings\AllowSignInOptions";

        private const string ENABLED_VALUE_NAME = @"Enabled";
        private const string ALLOW_DOMAIN_PIN_LOGON_VALUE_NAME = @"AllowDomainPINLogon";
        private const string VALUE_VALUE_NAME = @"value";

        private readonly ConfigurationStore configurationStore;

        public WindowsHelloConfigurationServices(
            [FromKeyedServices("WindowsHello")] ConfigurationStore ConfigurationStore)
        {
            configurationStore = ConfigurationStore;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(PASSPORT_FOR_WORK_KEY_NAME, ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SYSTEM_KEY_NAME, ALLOW_DOMAIN_PIN_LOGON_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(BIOMETRICS_KEY_NAME, ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FACIAL_FEATURES_KEY_NAME, ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FINGERPRINT_FEATURES_KEY_NAME, ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ALLOW_SIGN_IN_OPTIONS_KEY_NAME, VALUE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);

            configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(PASSPORT_FOR_WORK_KEY_NAME, ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(SYSTEM_KEY_NAME, ALLOW_DOMAIN_PIN_LOGON_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(BIOMETRICS_KEY_NAME, ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FACIAL_FEATURES_KEY_NAME, ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FINGERPRINT_FEATURES_KEY_NAME, ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ALLOW_SIGN_IN_OPTIONS_KEY_NAME, VALUE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);

            configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
