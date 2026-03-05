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
    public class GalleryConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Gallery";
        private const string STATE_VALUE_NAME = "state";

        private const string LONG_STRING_KEY_NAME = @"HKCU\Software\Classes\CLSID\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}";

        private const string IS_PINNED_TO_NAME_SPACE_TREE_VALUE_NAME = "System.IsPinnedToNameSpaceTree";

        private readonly ConfigurationStore _galleryConfigurationService;

        public GalleryConfigurationService(
            [FromKeyedServices("Gallery")] ConfigurationStore galleryConfigurationService)
        {
            _galleryConfigurationService = galleryConfigurationService;
        }


        public void Disable()
        {
            RegistryHelper.SetValue(LONG_STRING_KEY_NAME, IS_PINNED_TO_NAME_SPACE_TREE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\Gallery\Disable Gallery (default).cmd");

            _galleryConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(LONG_STRING_KEY_NAME, IS_PINNED_TO_NAME_SPACE_TREE_VALUE_NAME, 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\File Explorer Customization\Gallery\Enable Gallery.cmd");

            _galleryConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
