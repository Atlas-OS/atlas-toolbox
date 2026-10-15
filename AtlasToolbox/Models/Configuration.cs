using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.Models
{
    public class Configuration : IConfigurationItem
    {
        public string Key { get; set; }
        public string Name { get => App.GetValueFromItemList(Key); }
        public string Description { get => App.GetValueFromItemList(Key, true); }
        public string RouteItem { get; set; }
        public FontIcon Icon { get; set; } = new();

        public Configuration(string key, string route, string icon = "\uE897")
        {
            Key = key;
            RouteItem = route;
            Icon.Glyph = icon;
        }
    }
}
