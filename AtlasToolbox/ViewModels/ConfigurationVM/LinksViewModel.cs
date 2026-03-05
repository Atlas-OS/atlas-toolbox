using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Models;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public class LinksViewModel : IConfigurationItem
    {
        private Links link { get; set; }
        public string Name => link.name ?? "N/A";
        public string Link => link.link;
        public string FontIcon => link.Icon;
        public string Key => link.name.ToLower().Replace(" ", "") ?? "N/A";

        public LinksViewModel(Links link)
        {
            this.link = link;
        }
    }
}
