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
    public class FaultTolerantHeapConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\FaultTolerantHeap";
        private const string STATE_VALUE_NAME = "state";

        private const string FTH_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\FTH";


        private readonly ConfigurationStore _faultTolerantHeapConfigurationService;

        public FaultTolerantHeapConfigurationService(
            [FromKeyedServices("FaultTolerantHeap")] ConfigurationStore faultTolerantHeapConfigurationService)
        {
            _faultTolerantHeapConfigurationService = faultTolerantHeapConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(FTH_KEY_NAME, "Enabled", 0, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\7. Security\Mitigations\Fault Tolerant Heap\Disable FTH (default).cmd");

            _faultTolerantHeapConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(FTH_KEY_NAME, "Enabled", 1, Microsoft.Win32.RegistryValueKind.DWord);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\7. Security\Mitigations\Fault Tolerant Heap\Enable FTH.cmd");

            _faultTolerantHeapConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
