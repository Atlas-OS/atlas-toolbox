using AtlasToolbox.Enums;
using AtlasToolbox.Models;
using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Services.ConfigurationSubMenu;
using AtlasToolbox.Stores;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace AtlasToolbox.ViewModels
{
    public class ConfigurationSubMenuViewModel : IConfigurationItem
    {
        private readonly ConfigurationStoreSubMenu _configurationStoreSubMenu;

        public ObservableCollection<ConfigurationItemViewModel> ConfigurationItems { get; set; }
        public ObservableCollection<MultiOptionConfigurationItemViewModel> MultiOptionConfigurationItems { get; set; }
        public ObservableCollection<LinksViewModel> LinksViewModels { get; set; }
        public ObservableCollection<ConfigurationSubMenuViewModel> ConfigurationSubMenuViewModels { get; set; }
        public ObservableCollection<ConfigurationButtonViewModel> ConfigurationButtonViewModels { get; set; }

        public ConfigurationSubMenu _configurationSubMenu { get; set; }
        public string Name => _configurationSubMenu.Name;
        public string Description => _configurationSubMenu.Description;
        public ConfigurationType Type => _configurationSubMenu.Type;
        public FontIcon Icon => _configurationSubMenu.Icon;

        public string Key => _configurationSubMenu.Key;

        public ConfigurationSubMenuViewModel() { }

        public ConfigurationSubMenuViewModel(
            ConfigurationSubMenu configurationSubMenu,
            ConfigurationStoreSubMenu configurationStoreSubMenu,
            ObservableCollection<ConfigurationItemViewModel> configurationItems,
            ObservableCollection<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItems,
            ObservableCollection<LinksViewModel> linksViewModels,
            ObservableCollection<ConfigurationSubMenuViewModel> configurationSubMenuViewModels,
            ObservableCollection<ConfigurationButtonViewModel> configurationButtonViewModels)
        {
            _configurationSubMenu = configurationSubMenu;
            _configurationStoreSubMenu = configurationStoreSubMenu;
            ConfigurationItems = configurationItems;
            MultiOptionConfigurationItems = multiOptionConfigurationItems;
            LinksViewModels = linksViewModels;
            ConfigurationSubMenuViewModels = configurationSubMenuViewModels;
            ConfigurationButtonViewModels = configurationButtonViewModels;
        }
    }
}
