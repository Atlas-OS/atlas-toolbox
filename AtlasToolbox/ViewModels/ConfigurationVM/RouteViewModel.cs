
using AtlasToolbox.Stores;
using AtlasToolbox.Models;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using AtlasToolbox.ViewModels.ConfigurationVM;
using AtlasToolbox.Services;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public partial class RouteViewModel : IConfigurationItem
    {
        public RouteService RouteModel { get; set; }

        public string Key => RouteModel.Key;
        public string Name => RouteModel.Name;
        public string Description => RouteModel.Description;
        public string RouteItem => RouteModel.Route;
        public FontIcon Icon => RouteModel.Icon;
        public RouteViewModel(
            RouteService route)
        {
            RouteModel = route;
        }
    }
}
