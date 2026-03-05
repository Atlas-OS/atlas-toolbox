using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class ModernVolumeFlyoutConfigurationService : IRoutable
    {
        private const string MTCUVC_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\MTCUVC";

        private const string ENABLE_MTC_UVC_VALUE_NAME = "EnableMtcUvc";

        private readonly ConfigurationStore _modernVolumeFlyoutConfigurationStore;

        public ModernVolumeFlyoutConfigurationService(
            [FromKeyedServices("ModernVolumeFlyout")] ConfigurationStore modernVolumeFlyoutConfigurationStore)
        {
            _modernVolumeFlyoutConfigurationStore = modernVolumeFlyoutConfigurationStore;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(MTCUVC_KEY_NAME, ENABLE_MTC_UVC_VALUE_NAME, 0);

            _modernVolumeFlyoutConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(MTCUVC_KEY_NAME, ENABLE_MTC_UVC_VALUE_NAME);

            _modernVolumeFlyoutConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(MTCUVC_KEY_NAME, ENABLE_MTC_UVC_VALUE_NAME, null);
        }
    }
}
