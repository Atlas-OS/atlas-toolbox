
using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Provider;

namespace AtlasToolbox.Models
{
    public class MultiOptionConfiguration : IConfigurationItem
	{
        public string Name { get => App.GetValueFromItemList(Key);}
        public string Description { get => App.GetValueFromItemList(Key, true);}
        public string Key { get; set; }
        public string Route { get; set; }
        public FontIcon Icon { get; set; } = new();

        public MultiOptionConfiguration(string key, string route, string icon = "\uE897")
        {
            Key = key;
            Route = route;
            Icon.Glyph = icon;
        }
    }
}
