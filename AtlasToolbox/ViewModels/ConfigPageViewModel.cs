using AtlasToolbox.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AtlasToolbox.ViewModels
{
    class ConfigPageViewModel : ObservableObject
    {
        private readonly List<IConfigurationItem> _allItems;
        private ObservableCollection<IConfigurationItem> _configurationItems;
        public ObservableCollection<IConfigurationItem> ConfigurationItems
        {
            get => _configurationItems;
            set => SetProperty(ref _configurationItems, value);
        }

        public ConfigPageViewModel(
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<ConfigurationSubMenuViewModel> configurationSubMenuViewModel,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<LinksViewModel> linksViewModel,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModel)
        {
            _allItems = new List<IConfigurationItem>();
            configurationSubMenuViewModel.ToList().ForEach(item => _allItems.Add(item));
            multiOptionConfigurationItemViewModels.ToList().ForEach(item => _allItems.Add(item));
            configurationItemViewModels.ToList().ForEach(item => _allItems.Add(item));
            configurationButtonViewModel.ToList().ForEach(item => _allItems.Add(item));
            linksViewModel.ToList().ForEach(item => _allItems.Add(item));
            _configurationItems = new ObservableCollection<IConfigurationItem>(_allItems);
        }

        /// <summary>
        /// Gets the configuration services
        /// </summary>
        /// <param name="configurationType">Type to get</param>
        public void ShowForType(ConfigurationType configurationType)
        {
            ConfigurationItems = new ObservableCollection<IConfigurationItem>(
                _allItems.Where(item => item.Type == configurationType));
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
