
using AtlasToolbox.Models;
using AtlasToolbox.Stores;
using System.Windows.Input;
using AtlasToolbox.Commands;

using Windows.UI;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using AtlasToolbox.ViewModels.ConfigurationVM;
using AtlasToolbox.Services;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public class MultiConfigViewModel : IConfigurationItem
    {
        public MultiServiceRegister Configuration { get; set; }
        public string Name => Configuration.Name;
        public FontIcon Icon => Configuration.Icon;

        public List<string> Options => Configuration.Options; 
        public string Key => Configuration.Key;

        private string _currentSetting;

        public string CurrentSetting
        {
            get => _currentSetting;
            set
            {
                _currentSetting = value;
                Configuration.CurrentOption = CurrentSetting;
            }
        }

        public string RouteItem => Configuration.Route;

        public MultiConfigViewModel(
            MultiServiceRegister configuration)
        {
            Configuration = configuration;
        }
    }
}
