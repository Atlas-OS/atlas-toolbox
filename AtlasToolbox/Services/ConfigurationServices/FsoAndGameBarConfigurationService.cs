using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System;

namespace AtlasToolbox.Services.ConfigurationServices
{
    // This one does not work, a 'System.UnauthorizedAccessException' is being thrown
    public class FsoAndGameBarConfigurationService : IConfigurationService
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\FSOGameBar";
        private const string STATE_VALUE_NAME = "state";
        //private const string GAME_BAR_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\GameBar";
        //private const string GAME_CONFIG_STORE_KEY_NAME = @"HKCU\System\GameConfigStore";
        //private const string HKCU_GAME_DVR_KEY_NAME = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR";
        //private const string ALLOW_GAME_DVR_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDVR";
        //private const string HKLM_GAME_DVR_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
        //private const string ENVIRONMENT_KEY_NAME = @"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
        //private const string PRESENCE_WRITER_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\WindowsRuntime\ActivatableClassId\Windows.Gaming.GameBar.PresenceServer.Internal.PresenceWriter";

        //private const string ALLOW_AUTO_GAME_MODE_VALUE_NAME = "AllowAutoGameMode";
        //private const string AUTO_GAME_MODE_ENABLED_VALUE_NAME = "AutoGameModeEnabled";
        //private const string GAME_PANEL_STARTUP_TIP_INDEX_VALUE_NAME = "GamePanelStartupTipIndex";
        //private const string SHOW_STARTUP_PANEL_VALUE_NAME = "ShowStartupPanel";
        //private const string USE_NEXUS_FOR_GAME_BAR_ENABLED_VALUE_NAME = "UseNexusForGameBarEnabled";
        //private const string GAME_DVR_ENABLED_VALUE_NAME = "GameDVR_Enabled";
        //private const string GAME_DVR_FSE_BEHAVIOR_MODE_VALUE_NAME = "GameDVR_FSEBehaviorMode";
        //private const string GAME_DVR_FSE_BEHAVIOR_VALUE_NAME = "GameDVR_FSEBehavior";
        //private const string GAME_DVR_HONOR_USER_FSE_BEHAVIOR_MODE_VALUE_NAME = "GameDVR_HonorUserFSEBehaviorMode";
        //private const string GAME_DVR_DXGI_HONOR_FSE_WINDOWS_COMPATIBLE_VALUE_NAME = "GameDVR_DXGIHonorFSEWindowsCompatible";
        //private const string GAME_DVR_EFSE_FEATURE_FLAGS_VALUE_NAME = "GameDVR_EFSEFeatureFlags";
        //private const string GAME_DVR_DSE_BEHAVIOR_VALUE_NAME = "GameDVR_DSEBehavior";
        //private const string APP_CAPTURE_ENABLED_VALUE_NAME = "AppCaptureEnabled";
        //private const string VALUE_VALUE_NAME = "value";
        //private const string ALLOW_GAME_DVR_VALUE_NAME = "AllowGameDVR";
        //private const string COMPAT_LAYER_VALUE_NAME = "__COMPAT_LAYER";
        //private const string ACTIVATION_TYPE_VALUE_NAME = "ActivationType";

        private readonly ConfigurationStore _fsoAndGameBarConfigurationStore;

        public FsoAndGameBarConfigurationService(
            [FromKeyedServices("FsoAndGameBar")] ConfigurationStore fsoAndGameBarConfigurationStore)
        {
            _fsoAndGameBarConfigurationStore = fsoAndGameBarConfigurationStore;
        }

        public void Disable()
        {
            // TODO change the directory of the ProcessStartInfo since this will probably not be in AtlasDesktop anymore.
            // This is also a placeholder since I am unsure of how to make it run with RunAsTI.cmd

            CommandPromptHelper.RunCommand(@"call \%windir%\AtlasDesktop\3. General Configuration\FSO and Game Bar\Disable FSO and Game Bar Support.cmd");
            // HKCU\System\GameConfigStore
            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_DSEBehavior", 2, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_DXGIHonorFSEWindowsCompatible", 1, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_EFSEFeatureFlags", 0, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_FSEBehavior", 2, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_HonorUserFSEBehaviorMode", 1, Microsoft.Win32.RegistryValueKind.DWord);

            //// HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment
            //RegistryHelper.SetValue(@"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "__COMPAT_LAYER", "~ DISABLEDXMAXIMIZEDWINDOWEDMODE", Microsoft.Win32.RegistryValueKind.String);

            //// HKCU\System\GameBar
            //RegistryHelper.SetValue(@"HKCU\System\GameBar", "GamePanelStartupTipIndex", 3, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameBar", "ShowStartupPanel", 0, Microsoft.Win32.RegistryValueKind.DWord);
            //RegistryHelper.SetValue(@"HKCU\System\GameBar", "UseNexusForGameBarEnabled", 0, Microsoft.Win32.RegistryValueKind.DWord);

            //RegistryHelper.SetValue(@"HKLM\SOFTWARE\Microsoft\WindowsRuntime\ActivatableClassId\Windows.Gaming.GameBar.PresenceServer.Internal.PresenceWriter", "ActivationType", 0, Microsoft.Win32.RegistryValueKind.DWord);

            //RegistryHelper.SetValue(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0, Microsoft.Win32.RegistryValueKind.DWord);

            //RegistryHelper.SetValue(@"HKLM\SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDVR", "value", 0, Microsoft.Win32.RegistryValueKind.DWord);

            //RegistryHelper.SetValue(@"HKCU\System\GameConfigStore", "GameDVR_Enabled", 0, Microsoft.Win32.RegistryValueKind.DWord);

            //RegistryHelper.SetValue(@"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0, Microsoft.Win32.RegistryValueKind.DWord);

            //CommandPromptHelper.RunCommand(@"Get-AppxPackage *xboxgamingoverlay* | Remove-AppxPackage -Confirm:$false");

            //RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\FSO and Game Bar\Disable FSO and Game Bar Support.cmd");

            _fsoAndGameBarConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            CommandPromptHelper.RunCommand(@"call \%windir%\AtlasDesktop\3. General Configuration\FSO and Game Bar\Enable FSO and Game Bar Support (default).cmd");

            //RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, ALLOW_AUTO_GAME_MODE_VALUE_NAME);
            //RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, AUTO_GAME_MODE_ENABLED_VALUE_NAME);
            //RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, GAME_PANEL_STARTUP_TIP_INDEX_VALUE_NAME);
            //RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, SHOW_STARTUP_PANEL_VALUE_NAME);
            //RegistryHelper.DeleteValue(GAME_BAR_KEY_NAME, USE_NEXUS_FOR_GAME_BAR_ENABLED_VALUE_NAME);
            //RegistryHelper.SetValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_ENABLED_VALUE_NAME, 0);
            //RegistryHelper.SetValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_FSE_BEHAVIOR_MODE_VALUE_NAME, 2);
            //RegistryHelper.DeleteValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_FSE_BEHAVIOR_VALUE_NAME);
            //RegistryHelper.SetValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_HONOR_USER_FSE_BEHAVIOR_MODE_VALUE_NAME, 0);
            //RegistryHelper.SetValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_DXGI_HONOR_FSE_WINDOWS_COMPATIBLE_VALUE_NAME, 0);
            //RegistryHelper.SetValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_EFSE_FEATURE_FLAGS_VALUE_NAME, 0);
            //RegistryHelper.DeleteValue(GAME_CONFIG_STORE_KEY_NAME, GAME_DVR_DSE_BEHAVIOR_VALUE_NAME);
            //RegistryHelper.DeleteValue(HKCU_GAME_DVR_KEY_NAME, APP_CAPTURE_ENABLED_VALUE_NAME);
            //RegistryHelper.SetValue(ALLOW_GAME_DVR_KEY_NAME, VALUE_VALUE_NAME, 1);
            //RegistryHelper.DeleteKey(HKLM_GAME_DVR_KEY_NAME);
            //RegistryHelper.DeleteValue(ENVIRONMENT_KEY_NAME, COMPAT_LAYER_VALUE_NAME);
            ////RegistryHelper.SetValue(PRESENCE_WRITER_KEY_NAME, ACTIVATION_TYPE_VALUE_NAME, 1);
            //RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            //RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\FSO and Game Bar\Enable FSO and Game Bar Support (default).cmd");

            _fsoAndGameBarConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
