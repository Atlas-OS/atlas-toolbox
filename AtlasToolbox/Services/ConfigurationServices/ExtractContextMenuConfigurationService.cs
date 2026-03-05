using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using AtlasToolbox.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;


namespace AtlasToolbox.Services.ConfigurationServices
{
    public class ExtractContextMenuConfigurationService : IRoutable
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services\ExtractContextMenu";
        private const string STATE_VALUE_NAME = "state";

        private const string BLOCKED_KEY_NAME = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked";

        private IEnumerable<string> _valueNames;

        private readonly ConfigurationStore _extractContextMenuConfigurationService;
        public ExtractContextMenuConfigurationService(
            [FromKeyedServices("ExtractContextMenu")] ConfigurationStore extractContextMenuConfigurationService)
        {
            _extractContextMenuConfigurationService = extractContextMenuConfigurationService;

            _valueNames = new[]
            {
                "{b8cdcb65-b1bf-4b42-9428-1dfdb7ee92af}",
                "{BD472F60-27FA-11cf-B8B4-444553540000}",
                "{EE07CEF5-3441-4CFB-870A-4002C724783A}",
                "{D12E3394-DE4B-4777-93E9-DF0AC88F8584}",
                "{D12E3394-DE4B-4777-93E9-DF0AC88F8584}"
            };
        }

        public void Disable()
        {
            foreach (string value in _valueNames)
            {
                RegistryHelper.SetValue(BLOCKED_KEY_NAME, value, "");
            }
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 0);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Context Menus\Extract\Remove Extract (default).cmd");

            _extractContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            foreach (string value in _valueNames)
            {
                RegistryHelper.DeleteValue(BLOCKED_KEY_NAME, value);
            }
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
            RegistryHelper.SetValue(ATLAS_STORE_KEY_NAME, "path", @$"{Environment.GetEnvironmentVariable("windir")}\AtlasDesktop\4. Interface Tweaks\Context Menus\Extract\Add Extract.cmd");

            _extractContextMenuConfigurationService.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME, STATE_VALUE_NAME, 1);
        }
    }
}
