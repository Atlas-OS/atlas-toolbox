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
        public string Name { get; set; }
        public string Description { get; set; }
        public ConfigurationCategory Type { get; set; }
        public FontIcon Icon { get; set; }

        public ConfigurationSubMenu(string name, string description, ConfigurationCategory type)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }
}
