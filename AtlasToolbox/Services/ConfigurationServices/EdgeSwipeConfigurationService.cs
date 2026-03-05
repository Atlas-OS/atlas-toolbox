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
    public class EdgeSwipeConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\EdgeSwipe";
        private const string STATE_VALUE_NAME = "state";

        private const string EDGE_UI_KEY_NAME = @"HKLM\Software\Policies\Microsoft\Windows\EdgeUI";

        private const string ALLOW_EDGE_SWIPE_VALUE_NAME = "AllowEdgeSwipe";


        private readonly ConfigurationStore _edgeSwipeConfigurationService;
        
        public EdgeSwipeConfigurationService(
            [FromKeyedServices("EdgeSwipe")] ConfigurationStore edgeSwipeConfigurationService)
        {
            _edgeSwipeConfigurationService = edgeSwipeConfigurationService;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(EDGE_UI_KEY_NAME, ALLOW_EDGE_SWIPE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Edge Swipe\Disallow Edge Swipe.cmd");

            _edgeSwipeConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(EDGE_UI_KEY_NAME, ALLOW_EDGE_SWIPE_VALUE_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Edge Swipe\Allow Edge Swipe (default).cmd");

            _edgeSwipeConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
