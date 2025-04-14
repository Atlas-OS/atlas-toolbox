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
    public class Links
    {
        public string link {  get; set; }
        public string name { get; set; }
        public ConfigurationCategory configurationType { get; set; }
        public string Icon { get; set; }

        public Links(string link, string name, ConfigurationCategory configurationType, string icon = "\uF6FA")
        {
            this.link = link;
            this.name = name;
            this.configurationType = configurationType;
            Icon = icon;
        }
    }
}
