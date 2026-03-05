using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class DeliveryOptimisationConfigurationService : IRoutable

    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\DeliveryOptimisation";
        private const string STATE_VALUE_NAME = "state";

        private const string DELIVERY_OPTIMIZATION_KEY_NAME = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";
        private const string DO_DOWNLOAD_MODE_VALUE_NAME = "DODownloadMode";

        private readonly ConfigurationStore _deliveryOptimisationConfigurationStore;
        public DeliveryOptimisationConfigurationService(
            [FromKeyedServices("DeliveryOptimisation")] ConfigurationStore deliveryOptimisationConfigurationStore)
        {
            _deliveryOptimisationConfigurationStore = deliveryOptimisationConfigurationStore;
        }
        public void Disable()
        {
            RegistryHelper.SetValue(DELIVERY_OPTIMIZATION_KEY_NAME, DO_DOWNLOAD_MODE_VALUE_NAME, 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Delivery Optimization\Disable Delivery Optimization (default).cmd");

            _deliveryOptimisationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.DeleteValue(DELIVERY_OPTIMIZATION_KEY_NAME, DO_DOWNLOAD_MODE_VALUE_NAME);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\3. General Configuration\Delivery Optimization\Enable Delivery Optimization.cmd");

            _deliveryOptimisationConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            bool[] checks =
            {
                RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1)
            };

            return checks.All(x => x);
        }
    }
}
