using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AtlasToolbox.Models
{
    public class ConfigurationButton : IConfigurationItem
    {
        public string Key { get; set; }
        public string Route { get; set; }
        public ICommand Command { get; set; }
        public string Name { get => App.GetValueFromItemList(Key); }
        public string Description { get => App.GetValueFromItemList(Key, true); }
        public FontIcon Icon { get; set; } = new FontIcon();

        public ConfigurationButton(string key, ICommand command, string route, string icon = "\uE897") 
        {
            Key = key;
            Route = route;
            Command = command;
            Icon.Glyph = icon;
        }
    }
}