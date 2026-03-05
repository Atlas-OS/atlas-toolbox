using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AtlasToolbox.Models;
using AtlasToolbox.ViewModels.ConfigurationVM;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public partial class ConfigurationButtonViewModel : IConfigurationItem
    {
        private ConfigurationButton ConfigButton { get; set; }
        public ICommand Command => ConfigButton.Command;
        public string Name => ConfigButton.Name;
        public string Description => ConfigButton.Description;
        public string Key => ConfigButton.Name.Replace(" ", "");
        public FontIcon Icon => ConfigButton.Icon;

        public ConfigurationButtonViewModel(ConfigurationButton configurationButton)
        {
            ConfigButton = configurationButton;
        }

        [RelayCommand]
        public void ExecuteCommand()
        {
            Command.Execute(this);
        }
    }
}
