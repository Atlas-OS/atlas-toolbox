using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AtlasToolbox.Models;
using AtlasToolbox.Services;
using AtlasToolbox.ViewModels.ConfigurationVM;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public partial class ConfigurationButtonViewModel : IConfigurationItem
    {
        private ButtonServiceRegister ConfigButton { get; set; }
        /// <summary>The button's action; an asynchronous one disables the button while it runs.</summary>
        public ICommand Command => ConfigButton.Command;
        public string Name => ConfigButton.Name;
        public string Description => ConfigButton.Description;
        public string Key => ConfigButton.Key;
        public string Icon => ConfigButton.Icon;

        public string RouteItem => ConfigButton.Route;

        public ConfigurationButtonViewModel(ButtonServiceRegister configurationButton)
        {
            ConfigButton = configurationButton;
        }
    }
}
