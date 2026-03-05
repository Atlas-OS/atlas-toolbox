using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.Activation;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class GiveAccessToMenuConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\GiveAccessToMenu";
        private const string STATE_VALUE_NAME = "state";

        private const string SHARING_1_KEY_NAME = @"HKCR\*\shellex\ContextMenuHandlers\Sharing";
        private const string SHARING_2_KEY_NAME = @"HKCR\Directory\Background\shellex\ContextMenuHandlers\Sharing";
        private const string SHARING_3_KEY_NAME = @"HKCR\Directory\shellex\ContextMenuHandlers\Sharing";
        private const string SHARING_4_KEY_NAME = @"HKCR\Directory\shellex\ContextMenuHandlers\Sharing";
        private const string SHARING_5_KEY_NAME = @"HKCR\LibraryFolder\background\shellex\ContextMenuHandlers\Sharing";
        private const string SHARING_6_KEY_NAME = @"HKCR\UserLibraryFolder\shellex\ContextMenuHandlers\Sharing";

        private const string VALUE_VALUE_NAME = "{f81e9010-6ea4-11ce-a7ff-00aa003ca9f6}";

        private readonly ConfigurationStore _configurationStore;

        public GiveAccessToMenuConfigurationService(
            [FromKeyedServices("GiveAccessToMenu")] ConfigurationStore configurationStore)
        {
            _configurationStore = configurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(SHARING_1_KEY_NAME, string.Empty, VALUE_VALUE_NAME);
            RegistryHelper.SetValue(SHARING_2_KEY_NAME, string.Empty, VALUE_VALUE_NAME);
            RegistryHelper.SetValue(SHARING_3_KEY_NAME, string.Empty, VALUE_VALUE_NAME);
            RegistryHelper.SetValue(SHARING_4_KEY_NAME, string.Empty, VALUE_VALUE_NAME);
            RegistryHelper.SetValue(SHARING_5_KEY_NAME, string.Empty, VALUE_VALUE_NAME);
            RegistryHelper.SetValue(SHARING_6_KEY_NAME, string.Empty, VALUE_VALUE_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\File Sharing\Give Access To Menu\Disable Give Access To Menu (default).cmd");

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.DeleteKey(SHARING_1_KEY_NAME);
            RegistryHelper.DeleteKey(SHARING_2_KEY_NAME);
            RegistryHelper.DeleteKey(SHARING_3_KEY_NAME);
            RegistryHelper.DeleteKey(SHARING_4_KEY_NAME);
            RegistryHelper.DeleteKey(SHARING_5_KEY_NAME);
            RegistryHelper.DeleteKey(SHARING_6_KEY_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\File Sharing\Give Access To Menu\Enable Give Access To Menu.cmd");

            _configurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
