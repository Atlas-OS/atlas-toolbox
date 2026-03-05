using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using WinRT;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class AddNvidiaDisplayContainerContextMenuConfigurationService : IRoutable
    {

        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\NVidiaDisplayContainerContextMenu";
        private const string STATE_VALUE_NAME = "state";

        private const string NVIDIA_CONTAINER_KEY_NAME = @"HKCR\DesktopBackground\Shell\NVIDIAContainer";
        private const string NVIDIA_CONTAINER_001_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer001";
        private const string NVIDIA_CONTAINER_001_COMMAND_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer001\command";
        private const string NVIDIA_CONTAINER_002_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer002";
        private const string NVIDIA_CONTAINER_002_COMMAND_KEY_NAME = @"HKCR\DesktopBackground\shell\NVIDIAContainer\shell\NVIDIAContainer002\command";

        private readonly ConfigurationStore _addNvidiaDisplayContainerContextMenuConfigurationService;

        public AddNvidiaDisplayContainerContextMenuConfigurationService(
            [FromKeyedServices("AddNvidiaDisplayContainerContextMenu")]  ConfigurationStore addNvidiaDisplayContainerContextMenuConfigurationService)
        {
            _addNvidiaDisplayContainerContextMenuConfigurationService = addNvidiaDisplayContainerContextMenuConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.DeleteKey(NVIDIA_CONTAINER_KEY_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Context Menu\Remove Container Context Menu (default).cmd");

            _addNvidiaDisplayContainerContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "Icon", "NVIDIA.ico,0", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "MUIVerb", "NVIDIA Container", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "Position", "Bottom", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_KEY_NAME, "SubCommands", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_001_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_001_KEY_NAME, "MUIVerb", "Enable NVIDIA Display Container LS", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_001_COMMAND_KEY_NAME, "", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\NVidia\EnableNVIDIADisplayContainerLS.cmd", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_002_KEY_NAME, "HasLUAShield", "", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_002_KEY_NAME, "MUIVerb", "Disable NVIDIA Display Container LS", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(NVIDIA_CONTAINER_002_COMMAND_KEY_NAME, "", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasModules\Toolbox\Scripts\NVidia\DisableNVIDIADisplayContainerLS.cmd", Microsoft.Win32.RegistryValueKind.String);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\NVIDIA Display Container\Context Menu\Add Container Context Menu.cmd");

            CommandPromptHelper.RestartExplorer();

            _addNvidiaDisplayContainerContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
