using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Enums;
using AtlasToolbox.Models;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels
{
    public class LinksViewModel
    {
        private Links link { get; set; }

        public string Name => link.name;
        public string Link => link.link;
        public ConfigurationCategory ConfigurationType => link.configurationType;
        public string FontIcon => link.Icon;

        public LinksViewModel(Links link)
        {
            this.link = link;
        }
    }
}
