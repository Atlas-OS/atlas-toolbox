using AtlasToolbox.Models;
using AtlasToolbox.Services;
using AtlasToolbox.ViewModels.ConfigurationVM;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    class ConfigPageViewModel : ObservableObject
    {
        public List<IConfigurationItem> ConfigurationItems { get; set; }

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
            IEnumerable<MultiConfigViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<LinksViewModel> linksViewModel,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModel,
            IEnumerable<RouteViewModel> routes)
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
            IEnumerable<MultiConfigViewModel> multiOptionConfigurationItemViewModels,
            IEnumerable<ConfigurationButtonViewModel> configurationButtonViewModels,
            IEnumerable<RouteViewModel> routes)
        {
            ConfigPageViewModel viewModel = new(configuration, multiOptionConfigurationItemViewModels, linksViewModels, configurationButtonViewModels, routes);

            return viewModel;
        }
    }
}
