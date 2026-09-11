using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Models;
using AtlasToolbox.Services;
using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public class LinksViewModel : IConfigurationItem
    {
        private LinkRegister link { get; set; }
        public string Name => link.Name ?? "N/A";
        public string Link => link.Link;
        public FontIcon FontIcon => link.Icon;
        public string Key => link.Name.ToLower().Replace(" ", "") ?? "N/A";

        public string RouteItem => link.Route;
        public LinksViewModel(LinkRegister link)
        {
            this.link = link;
        }
    }
}
