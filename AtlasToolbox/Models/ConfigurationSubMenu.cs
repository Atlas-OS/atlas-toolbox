using AtlasToolbox.Enums;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Models
{
    public class ConfigurationSubMenu
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ConfigurationType Type { get; set; }
        public FontIcon Icon { get; set; }

        public ConfigurationSubMenu(string key, string name, string description, ConfigurationType type, string icon = "\uE897")
        {
            Key = key;
            Name = name;
            Description = description;
            Type = type;
            Icon = new FontIcon();
            Icon.Glyph = icon;
        }
    }
}
