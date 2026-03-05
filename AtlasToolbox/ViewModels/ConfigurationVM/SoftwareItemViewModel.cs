using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Models;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public class SoftwareItemViewModel
    {
        public SoftwareItem SoftwareItem { get; set; }

        public string Key => SoftwareItem.Key;
        public string Name => SoftwareItem.Name;
        public string Icon { get; set; }
        public string BitMapIcon { get; set; }

        public SoftwareItemViewModel(SoftwareItem softwareItem)
        {
            SoftwareItem = softwareItem;
            BitMapIcon = $"https://api.winstall.app/icons/next/{Key}.webp";
        }
    }
}
