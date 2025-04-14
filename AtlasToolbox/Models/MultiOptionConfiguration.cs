using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Enums;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Provider;

namespace AtlasToolbox.Models
{
    public class MultiOptionConfiguration
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public ConfigurationCategory Type { get; set; }
        public RiskRating RiskRating { get; set; }
        public FontIcon Icon { get; set; }

        public MultiOptionConfiguration(string name, string key, ConfigurationCategory type, RiskRating riskRating, string icon = "\uE897")
        {
            Name = name;
            Key = key;
            Type = type;
            RiskRating = riskRating;
            Icon = new FontIcon();
            Icon.Glyph = icon;
        }
    }
}
