﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class OldContextMenuConfigurationService : IConfigurationService
    {
        
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\ServicesOldContextMenu";
        private const string STATE_VALUE_NAME = "state";

        private const string INCROP_SERVER_32 = @"HKCU\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32";


        private readonly ConfigurationStore _oldContextMenuConfigurationService;

        public OldContextMenuConfigurationService(
            [FromKeyedServices("OldContextMenu")] ConfigurationStore oldContextMenuConfigurationService)
        {
            _oldContextMenuConfigurationService = oldContextMenuConfigurationService;
        }

        public void Disable()
        {
            RegistryHelper.DeleteKey(INCROP_SERVER_32);

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);

            _oldContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            RegistryHelper.SetValue(INCROP_SERVER_32, "", "");

            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);

            _oldContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
