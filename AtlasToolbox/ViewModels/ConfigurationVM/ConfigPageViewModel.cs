using AtlasToolbox.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    class ConfigPageViewModel : ObservableObject
    {
        public ObservableCollection<IConfigurationItem> ConfigurationItems { get; set; }

        private string _currentRoute;
        public string CurrentRoute
        {
            get => _currentRoute;
            set
            {
                SetProperty(ref _currentRoute, value);
                UpdateFilteredItems();
            }
        }

        public ObservableCollection<IConfigurationItem> FilteredItems { get; } = new();

        private void UpdateFilteredItems()
        {
            FilteredItems.Clear();
            foreach (var item in ConfigurationItems.Where(i => i.RouteItem == CurrentRoute))
                FilteredItems.Add(item);
        }

        public ConfigPageViewModel(
            IEnumerable<ConfigurationItemViewModel> configuration,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<LinksViewModel> linksViewModel,
            IEnumerable<Route> routes,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModel)
        {
            List<IConfigurationItem> configurationItems = new();

            configurationItems.AddRange(routes);
            configurationItems.AddRange(multiOptionConfigurationItemViewModels);
            configurationItems.AddRange(configuration);
            configurationItems.AddRange(configurationButtonViewModel);
            configurationItems.AddRange(linksViewModel);

            ConfigurationItems = new(configurationItems);
        }

        /// <summary>
        /// Loads the view model
        /// </summary>
        /// <param name="linksViewModels"></param>
        /// <param name="configuration"></param>
        /// <param name="multiOptionConfigurationItemViewModels"></param>
        /// <param name="configurationSubMenuViewModels"></param>
        /// <param name="configurationButtonViewModels"></param>
        /// <returns></returns>
        public static ConfigPageViewModel LoadViewModel(
            IEnumerable<LinksViewModel> linksViewModels,
            IEnumerable<ConfigurationItemViewModel> configuration,
            IEnumerable<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModels,
            IEnumerable<Route> routes)
        {
            ConfigPageViewModel viewModel = new(configuration, multiOptionConfigurationItemViewModels, linksViewModels, routes,configurationButtonViewModels);

            return viewModel;
        }
    }
}
