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
    public class VbsConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\VbsState";
        private const string STATE_VALUE_NAME = "state";

        private const string HYPERVISOR_ENFORCED_CODE_INTEGRITY = @"HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";
        private const string DEVICE_GUARD_KEY_NAME = @"HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard";

        private const string ENABLED_VALUE_NAME = "Enabled";
        private const string ENABLE_VIRTUALIZATION_BASED_SECURITY_VALUE_NAME = "EnableVirtualizationBasedSecurity";

        private readonly ConfigurationStore _configurationStore;

        public VbsConfigurationService(
            [FromKeyedServices("VbsState")] ConfigurationStore configurationStore)
        {
            _configurationStore = configurationStore;
        }


        public void Disable()
        {
            RegistryHelper.SetValue(HYPERVISOR_ENFORCED_CODE_INTEGRITY, ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DEVICE_GUARD_KEY_NAME, ENABLE_VIRTUALIZATION_BASED_SECURITY_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
        }

        public void Enable()
        {
            RegistryHelper.SetValue(HYPERVISOR_ENFORCED_CODE_INTEGRITY, ENABLED_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(DEVICE_GUARD_KEY_NAME, ENABLE_VIRTUALIZATION_BASED_SECURITY_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
