using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Provider;

namespace AtlasToolbox.Models
{
    public class Links : IConfigurationItem
    {
        public string Key { get; set; }
        public string Route { get; set; }   
        public string Link {  get; set; }
        public string Name { get => App.GetValueFromItemList(Key); }
        public string Description { get => App.GetValueFromItemList(Key, true); }
        public FontIcon Icon { get; set; } = new FontIcon();


        public Links(string key, string link, string route, string icon = "\uF6FA")
        {
            Key = key;
            Link = link;
            Route = route;
            Icon.Glyph = icon;
        }
    }
}
