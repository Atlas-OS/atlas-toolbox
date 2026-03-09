using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.Models;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public partial class RouteViewModel : IConfigurationItem
    {
        private readonly ConfigurationStore _configurationStore;
        private readonly IRoutable _configurationService;

        public Route RouteModel { get; set; }

        public string Key => RouteModel.Key;
        public string Name => RouteModel.Name;
        public string Description => RouteModel.Description;
        public string RouteItem => RouteModel.RouteItem;
        public FontIcon Icon => RouteModel.Icon;
        public RouteViewModel(
            Route route,
            ConfigurationStore configurationStore,
            IRoutable configurationService)
        {
            _configurationStore = configurationStore;
            _configurationService = configurationService;
            RouteModel = route;

        }
    }
}
