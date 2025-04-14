using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AtlasToolbox.Enums;
using AtlasToolbox.Models;
using CommunityToolkit.Mvvm.Input;

namespace AtlasToolbox.ViewModels
{
    public partial class ConfigurationButtonViewModel
    {
        private ConfigurationButton ConfigButton { get; set; }
        public ICommand Command => ConfigButton.Command;
        public string Name => ConfigButton.Name;
        public string Description => ConfigButton.Description;
        public ConfigurationCategory Type => ConfigButton.Type;

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
