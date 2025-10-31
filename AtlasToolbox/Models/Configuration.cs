using AtlasToolbox.Enums;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.Models
{
    public class Configuration
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public ConfigurationType Type { get; set; }
        public FontIcon Icon { get; set; }
        public string Description { get; set; }

        public Configuration(string name, string key, ConfigurationType type, string icon = "\uE897")
        {
            Name = name;
            Key = key;
            Type = type;
            Icon = new FontIcon();
            Icon.Glyph = icon;
            Description = App.GetValueFromItemList(key, true);
        }
    }
}
