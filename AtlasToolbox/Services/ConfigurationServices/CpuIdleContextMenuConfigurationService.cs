using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class CpuIdleContextMenuConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\CPUIdleContextMenu";
        private const string STATE_VALUE_NAME = "state";

        private const string IDLE_KEY_NAME = @"HKCR\DesktopBackground\Shell\Idle";

        private readonly ConfigurationStore _cpuIdleContextMenuConfigurationStore;

        public CpuIdleContextMenuConfigurationService(
            [FromKeyedServices("CpuIdleContextMenu")] ConfigurationStore cpuIdleContextMenuConfigurationStore)
        {
            _cpuIdleContextMenuConfigurationStore = cpuIdleContextMenuConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.DeleteKey(IDLE_KEY_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\CPU Idle\Desktop Context Menu\Add Idle Toggle in Desktop Context Menu.cmd");

            _cpuIdleContextMenuConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(IDLE_KEY_NAME, "Icon", "powercpl.dll");
            RegistryHelper.SetValue(IDLE_KEY_NAME, "MUIVerb", "CPU Idle");
            RegistryHelper.SetValue(IDLE_KEY_NAME, "Position", "Bottom");
            RegistryHelper.SetValue(IDLE_KEY_NAME, "SubCommands", string.Empty);

            string disableIdleKeyName = Path.Combine(IDLE_KEY_NAME, "Shell", "Disable Idle");
            RegistryHelper.SetValue(disableIdleKeyName, "MUIVerb", "Disable Idle");
            RegistryHelper.SetValue(disableIdleKeyName, "Icon", "powercpl.dll");

            string disableIdleCommandKeyName = Path.Combine(disableIdleKeyName, "Command");
            RegistryHelper.SetValue(disableIdleCommandKeyName, null,
                "powercfg /setacvalueindex scheme_current sub_processor 5d76a2ca-e8c0-402f-a133-2158492d58ad 1");

            string enableIdleKeyName = Path.Combine(IDLE_KEY_NAME, "Shell", "Enable Idle");
            RegistryHelper.SetValue(enableIdleKeyName, "MUIVerb", "Enable Idle");
            RegistryHelper.SetValue(enableIdleKeyName, "Icon", "powercpl.dll");

            string enableIdleCommandKeyName = Path.Combine(enableIdleKeyName, "Command");
            RegistryHelper.SetValue(enableIdleCommandKeyName, null,
                "powercfg /setacvalueindex scheme_current sub_processor 5d76a2ca-e8c0-402f-a133-2158492d58ad 0");

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\CPU Idle\Desktop Context Menu\Remove Idle Toggle in Desktop Context Menu (default).cmd");

            _cpuIdleContextMenuConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.KeyExists(IDLE_KEY_NAME);
        }
    }
}
