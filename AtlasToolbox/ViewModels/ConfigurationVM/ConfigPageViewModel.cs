using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    class ConfigPageViewModel : ObservableObject
    {
        public ObservableCollection<IConfigurationItem> ConfigurationItems { get; set; }

        public ConfigPageViewModel(
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<LinksViewModel> linksViewModel,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModel)
        {
            ConfigurationItems = new ObservableCollection<IConfigurationItem>();
            multiOptionConfigurationItemViewModels.ToList().ForEach(item => ConfigurationItems.Add(item));
            configurationItemViewModels.ToList().ForEach(item => ConfigurationItems.Add(item));
            configurationButtonViewModel.ToList().ForEach(item => ConfigurationItems.Add(item));
            linksViewModel.ToList().ForEach(item => ConfigurationItems.Add(item));

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
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModels)
        {
            ConfigPageViewModel viewModel = new(configurationItemViewModels, multiOptionConfigurationItemViewModels, linksViewModels, configurationButtonViewModels);

            return viewModel;
        }
    }
}
