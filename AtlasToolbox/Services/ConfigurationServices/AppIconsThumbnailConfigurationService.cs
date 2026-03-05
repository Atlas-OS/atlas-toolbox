using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class AppIconsThumbnailConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\AppIconsThumbnail";
        private const string STATE_VALUE_NAME = "state";

        private const string ADVANCED_KEY_NAME = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";

        private const string SHOW_TYPE_OVERLAY_KEY_NAME = "ShowTypeOverlay";

        private readonly ConfigurationStore _appIconsThumbnailConfigurationService;

        public AppIconsThumbnailConfigurationService(
            [FromKeyedServices("AppIconsThumbnail")] ConfigurationStore appIconsThumbnailConfigurationService)
        {
            _appIconsThumbnailConfigurationService = appIconsThumbnailConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, SHOW_TYPE_OVERLAY_KEY_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\App Icons on Thumbnails\Disable App Icons on Thumbnails.cmd");

            _appIconsThumbnailConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ADVANCED_KEY_NAME, SHOW_TYPE_OVERLAY_KEY_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\App Icons on Thumbnails\Enable App Icons on Thumbnails (default).cmd");

            _appIconsThumbnailConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
