using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    internal class LocationConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Location";
        private const string STATE_VALUE_NAME = "state";

        private const string LFSCV_SERVICE_NAME = "lfscv";
        private const string MAPS_BROKER_SERVICE_NAME = "MapsBroker";

        private const string FIND_MY_DEVICE_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\FindMyDevice";
        private const string LOCATION_KEY_NAME = @"HKCU\Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location";

        private const string ALLOW_FIND_MY_DEVICE_VALUE_NAME = "AllowFindMyDevice";
        private const string LOCATION_SYNC_ENABLED_VALUE_NAME = "LocationSyncEnabled";
        private const string SHOW_GLOBAL_PROMPTS_VALUE_NAME = "ShowGlobalPrompts";

        private readonly ConfigurationStore _locationConfigurationStore;

        public LocationConfigurationService(
            [FromKeyedServices("Location")] ConfigurationStore locationConfigurationStore)
        {
            _locationConfigurationStore = locationConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(LFSCV_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(MAPS_BROKER_SERVICE_NAME, ServiceStartMode.Disabled);

            RegistryHelper.SetValue(FIND_MY_DEVICE_KEY_NAME, ALLOW_FIND_MY_DEVICE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(FIND_MY_DEVICE_KEY_NAME, LOCATION_SYNC_ENABLED_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(LOCATION_KEY_NAME, SHOW_GLOBAL_PROMPTS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Location\Disable Location (default).cmd");

            CommandPromptHelper.RunCommand("sc stop lfscv");
            CommandPromptHelper.RunCommand("sc stop MapsBroker");
            CommandPromptHelper.RunCommand("call \"%windir%\\AtlasModules\\Scripts\\settingsPages.cmd\" /hide privacy-location");
            CommandPromptHelper.RunCommand("call \"%windir%\\AtlasModules\\Scripts\\settingsPages.cmd\" /hide findmydevice");

            _locationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(LFSCV_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(MAPS_BROKER_SERVICE_NAME, ServiceStartMode.Automatic);

            RegistryHelper.DeleteKey(FIND_MY_DEVICE_KEY_NAME);
            RegistryHelper.SetValue(LOCATION_KEY_NAME, SHOW_GLOBAL_PROMPTS_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Location\Enable Location.cmd");

            CommandPromptHelper.RunCommand("sc start lfscv");
            CommandPromptHelper.RunCommand("sc start MapsBroker");
            CommandPromptHelper.RunCommand("call \"%windir%\\AtlasModules\\Scripts\\settingsPages.cmd\" /unhide privacy-location");
            CommandPromptHelper.RunCommand("call \"%windir%\\AtlasModules\\Scripts\\settingsPages.cmd\" /unhide findmydevice");

            _locationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
