using AtlasToolbox.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using ICSharpCode.Decompiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AtlasToolbox.ViewModels
{
    class ConfigPageViewModel : ObservableObject
    {
        // If there's a better solution for this, please do a PR, this isn't great imo
        private IEnumerable<ConfigurationItemViewModel> ConfigurationItemViewModels { get; }
        private IEnumerable<MultiOptionConfigurationItemViewModel> MultiOptionConfigurationItemViewModels { get; }
        private IEnumerable<ConfigurationSubMenuViewModel> ConfigurationSubMenuViewModels { get; }
        private IEnumerable<LinksViewModel> LinksViewModels { get; }
        private IEnumerable<ConfigurationButtonViewModel> ConfigurationButtonViewModels { get; }

        public ObservableCollection<ConfigurationItemViewModel> ConfigurationItem { get; set; }
        public ObservableCollection<MultiOptionConfigurationItemViewModel> MultiOptionConfigurationItem { get; set; }
        public ObservableCollection<ConfigurationSubMenuViewModel> ConfigurationItemSubMenu { get; set; }
        public ObservableCollection<LinksViewModel> LinksItemViewModel { get; set; }
        public ObservableCollection<ConfigurationButtonViewModel> ConfigurationButtonViewModel { get; set; }

        public ConfigPageViewModel(
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<ConfigurationSubMenuViewModel> configurationSubMenuViewModel,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<LinksViewModel> linksViewModel,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModel)
        {

            ConfigurationItemViewModels = configurationItemViewModels;
            MultiOptionConfigurationItemViewModels = multiOptionConfigurationItemViewModels;
            ConfigurationSubMenuViewModels = configurationSubMenuViewModel;
            LinksViewModels = linksViewModel;
            ConfigurationButtonViewModels = configurationButtonViewModel;
        }

        /// <summary>
        /// Gets the configuration services
        /// </summary>
        /// <param name="configurationType">Type to get</param>
        public void ShowForType(ConfigurationCategory configurationType)
        {
            ConfigurationItem = new ObservableCollection<ConfigurationItemViewModel>(ConfigurationItemViewModels.Where(item => item.Category == configurationType));
            MultiOptionConfigurationItem = new ObservableCollection<MultiOptionConfigurationItemViewModel>(MultiOptionConfigurationItemViewModels.Where(item => item.Type == configurationType));
            ConfigurationItemSubMenu = new ObservableCollection<ConfigurationSubMenuViewModel>(ConfigurationSubMenuViewModels.Where(item => item.Type == configurationType));
            LinksItemViewModel = new ObservableCollection<LinksViewModel>(LinksViewModels.Where(item => item.ConfigurationType == configurationType));
            ConfigurationButtonViewModel = new ObservableCollection<ConfigurationButtonViewModel>(ConfigurationButtonViewModels.Where(item => item.Type == configurationType));
        }

        /// <summary>
        /// Loads the view model
        /// </summary>
        /// <param name="linksViewModels"></param>
        /// <param name="configurationItemViewModels"></param>
        /// <param name="multiOptionConfigurationItemViewModels"></param>
        /// <param name="configurationSubMenuViewModels"></param>
        /// <param name="configurationButtonViewModels"></param>
        /// <returns></returns>
        public static ConfigPageViewModel LoadViewModel(
            IEnumerable<LinksViewModel> linksViewModels,
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<ConfigurationSubMenuViewModel> configurationSubMenuViewModels,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModels)
        {
            ConfigPageViewModel viewModel = new(configurationItemViewModels, configurationSubMenuViewModels, multiOptionConfigurationItemViewModels, linksViewModels, configurationButtonViewModels);

            return viewModel;
        }
    }
}
