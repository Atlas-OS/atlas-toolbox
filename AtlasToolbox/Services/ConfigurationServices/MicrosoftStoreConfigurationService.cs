using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class MicrosoftStoreConfigurationService : IConfigurationService
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\MicrosoftStore";
        private const string STATE_VALUE_NAME = "state";

        private readonly ConfigurationStore _microsoftStoreConfigurationStore;

        public MicrosoftStoreConfigurationService(
            [FromKeyedServices("MicrosoftStore")] ConfigurationStore microsoftStoreConfigurationStore)
        {
            _microsoftStoreConfigurationStore = microsoftStoreConfigurationStore;
        }

        public void Disable()
        {
            CommandPromptHelper.RunCommand("powershell.exe \"Get-AppxPackage -AllUsers Microsoft.WindowsStore | Remove-AppxPackage\"");
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}AtlasDesktop\6. Advanced Configuration\Microsoft Store\Disable Microsoft Store.cmd");

            _microsoftStoreConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            CommandPromptHelper.RunCommand("powershell.exe \"Get-AppxPackage -AllUsers Microsoft.WindowsStore | ForEach-Object {Add-AppxPackage -DisableDevelopmentMode -Register (Join-Path $_.InstallLocation 'AppXManifest.xml')}\"");
            
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}AtlasDesktop\6. Advanced Configuration\Microsoft Store\Enable Microsoft Store (default).cmd");
            CommandPromptHelper.RunCommand("powershell.exe \"start ms-windows-store:\"");

            _microsoftStoreConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
