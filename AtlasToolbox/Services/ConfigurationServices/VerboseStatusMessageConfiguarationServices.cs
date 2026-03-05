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
    public class VerboseStatusMessageConfiguarationServices : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\VerboseStatusMessage";
        private const string STATE_VALUE_NAME = "state";

        private const string SYSTEM_POLICIES_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
        private const string VERBOSE_STATUS = "verbosestatus";

        private readonly ConfigurationStore _verboseStatusMessageConfigurationService;

        public VerboseStatusMessageConfiguarationServices(
            [FromKeyedServices("VerboseStatusMessage")] ConfigurationStore verboseStatusMessageConfigurationService)
        {
            _verboseStatusMessageConfigurationService = verboseStatusMessageConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.DeleteValue(SYSTEM_POLICIES_KEY_NAME, VERBOSE_STATUS);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Verbose Status Messages\Disable Verbose Messages (default).cmd");

            _verboseStatusMessageConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(SYSTEM_POLICIES_KEY_NAME, VERBOSE_STATUS, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Verbose Status Messages\Enable Verbose Messages.cmd");

            _verboseStatusMessageConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
