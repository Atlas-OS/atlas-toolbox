using AtlasToolbox.Services;
using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.ServiceProcess;

namespace AtlasOSToolbox.Services.ConfigurationServices
{
    public class BluetoothConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Bluetooth";
        private const string STATE_VALUE_NAME = "state";

        private const string BLUETOOTH_USER_SERVICE_SERVICE_NAME = "BluetoothUserService";
        private const string BTAG_SERVICE_SERVICE_NAME = "BTAGService";
        private const string BTH_A2DP_SERVICE_NAME = "BthA2dp";
        private const string BTH_AVCTP_SVC_SERVICE_NAME = "BthAvctpSvc";
        private const string BTH_ENUM_SERVICE_NAME = "BthEnum";
        private const string BTH_HF_ENUM_SERVICE_NAME = "BthHFEnum";
        private const string BTH_LE_ENUM_SERVICE_NAME = "BthLEEnum";
        private const string BTH_MINI_SERVICE_NAME = "BthMini";
        private const string BTH_MODEM_SERVICE_NAME = "BTHMODEM";
        private const string BTH_PAN_SERVICE_NAME = "BthPan";
        private const string BTH_PORT_SERVICE_NAME = "BTHPORT";
        private const string BTH_SERV_SERVICE_NAME = "bthserv";
        private const string BTH_USB_SERVICE_NAME = "BTHUSB";
        private const string HIT_BTH_SERVICE_NAME = "HidBth";
        private const string MICROSOFT_BLUETOOTH_AVRCP_TRANSPORT_NAME = "Microsoft_Bluetooth_AvrcpTransport";
        private const string RFCOMM_BLUETOOTH_AVRCP_TRANSPORT_NAME = "RFCOMM";

        private readonly ConfigurationStore _bluetoothConfigurationStore;

        public BluetoothConfigurationService(
            [FromKeyedServices("Bluetooth")] ConfigurationStore bluetoothConfigurationStore)
        {
            _bluetoothConfigurationStore = bluetoothConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(BLUETOOTH_USER_SERVICE_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTAG_SERVICE_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_A2DP_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_AVCTP_SVC_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_ENUM_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_HF_ENUM_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_LE_ENUM_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_MINI_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_MODEM_SERVICE_NAME, ServiceStartMode.Disabled);
            //ServiceHelper.SetStartupType(BTH_PAN_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_PORT_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_SERV_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(BTH_USB_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(HIT_BTH_SERVICE_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(MICROSOFT_BLUETOOTH_AVRCP_TRANSPORT_NAME, ServiceStartMode.Disabled);
            ServiceHelper.SetStartupType(RFCOMM_BLUETOOTH_AVRCP_TRANSPORT_NAME, ServiceStartMode.Disabled);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Bluetooth\Disable Bluetooth.cmd");

            DeviceHelper.DisableDevice("%Bluetooth%");

            _bluetoothConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(BLUETOOTH_USER_SERVICE_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTAG_SERVICE_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_A2DP_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_AVCTP_SVC_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_ENUM_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_HF_ENUM_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_LE_ENUM_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_MINI_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_MODEM_SERVICE_NAME, ServiceStartMode.Manual);
            //ServiceHelper.SetStartupType(BTH_PAN_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_PORT_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_SERV_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(BTH_USB_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(HIT_BTH_SERVICE_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(MICROSOFT_BLUETOOTH_AVRCP_TRANSPORT_NAME, ServiceStartMode.Manual);
            ServiceHelper.SetStartupType(RFCOMM_BLUETOOTH_AVRCP_TRANSPORT_NAME, ServiceStartMode.Manual);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Bluetooth\Enable Bluetooth (default).cmd");

            DeviceHelper.EnableDevice("%Bluetooth%");

            _bluetoothConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
