using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class PrintingConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\Printing";
        private const string STATE_VALUE_NAME = "state";

        private const string PROGRAMMATIC_ACCESS_ONLY_VALUE_NAME = "ProgrammaticAccessOnly";

        private const string SPOOLER_SERVICE_NAME = "Spooler";

        private readonly ConfigurationStore _printingConfigurationStore;
        private readonly IEnumerable<string> _keyNames;

        public PrintingConfigurationService(
            [FromKeyedServices("Printing")] ConfigurationStore printingConfigurationStore)
        {
            _printingConfigurationStore = printingConfigurationStore;

            _keyNames = new[]
            {
                @"HKCR\SystemFileAssociations\image\shell\print",
                @"HKCR\batfile\shell\print",
                @"HKCR\cmdfile\shell\print",
                @"HKCR\docxfile\shell\print",
                @"HKCR\fonfile\shell\print",
                @"HKCR\htmlfile\shell\print",
                @"HKCR\inffile\shell\print",
                @"HKCR\inifile\shell\print",
                @"HKCR\JSEFile\shell\print",
                @"HKCR\otffile\shell\print",
                @"HKCR\pfmfile\shell\print",
                @"HKCR\regfile\shell\print",
                @"HKCR\rtffile\shell\print",
                @"HKCR\ttcfile\shell\print",
                @"HKCR\ttffile\shell\print",
                @"HKCR\txtfile\shell\print",
                @"HKCR\VBEFile\shell\print",
                @"HKCR\VBSFile\shell\print",
                @"HKCR\WSFFile\shell\print",
            };
        }

        public void Disable()
        {
            foreach (string keyName in _keyNames)
            {
                RegistryHelper.DeleteValue(keyName, PROGRAMMATIC_ACCESS_ONLY_VALUE_NAME);
            }

            ServiceHelper.SetStartupType(SPOOLER_SERVICE_NAME, ServiceStartMode.Disabled);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Printing\Disable Printing.cmd");

            _printingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            foreach (string keyName in _keyNames)
            {
                RegistryHelper.SetValue(keyName, PROGRAMMATIC_ACCESS_ONLY_VALUE_NAME, string.Empty);
            }

            ServiceHelper.SetStartupType(SPOOLER_SERVICE_NAME, ServiceStartMode.Automatic);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\6. Advanced Configuration\Services\Printing\Enabble Printing (default).cmd");

            _printingConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
