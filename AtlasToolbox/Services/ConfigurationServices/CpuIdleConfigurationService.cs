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
    class CpuIdleConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\CpuIdle";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _cpuIdle;
        public CpuIdleConfigurationService(
            [FromKeyedServices("CpuIdle")] ConfigurationStore cpuIdle) 
        {
            _cpuIdle = cpuIdle;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            CommandPromptHelper.RunCommand("powercfg /setacvalueindex scheme_current sub_processor 5d76a2ca-e8c0-402f-a133-2158492d58ad 1");
            CommandPromptHelper.RunCommand("powercfg /setactive scheme_current");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\CPU Idle\Disable Idle.cmd");

            _cpuIdle.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            CommandPromptHelper.RunCommand("powercfg /setacvalueindex scheme_current sub_processor 5d76a2ca-e8c0-402f-a133-2158492d58ad 0");
            CommandPromptHelper.RunCommand("powercfg /setactive scheme_current");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\CPU Idle\Enable Idle (default).cmd");

            _cpuIdle.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
           return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
