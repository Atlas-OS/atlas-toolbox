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
    public class NvidiaDispayContainerConfigurationService : IRoutable
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\NVidiaDisplayContainer";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _nvidiaDispayContainerConfigurationService;

        public NvidiaDispayContainerConfigurationService(
            [FromKeyedServices("NvidiaDispayContainer")]  ConfigurationStore nvidiaDispayContainerConfigurationService)
        {
            _nvidiaDispayContainerConfigurationService = nvidiaDispayContainerConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\NVidia\DisableNVIDIADisplayContainerLS.cmd", false);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Disable NVIDIA Display Container LS.cmd");

            _nvidiaDispayContainerConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            CommandPromptHelper.RunCommand(@$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\NVidia\DisableNVIDIADisplayContainerLS.cmd", false);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Enable NVIDIA Display Container LS (default).cmd");

            _nvidiaDispayContainerConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
