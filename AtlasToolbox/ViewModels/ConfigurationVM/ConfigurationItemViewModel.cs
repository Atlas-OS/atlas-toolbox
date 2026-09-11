
using AtlasToolbox.Models;
using AtlasToolbox.Stores;
using System.Windows.Input;
using AtlasToolbox.Commands;
using Windows.UI;
using Microsoft.UI.Xaml.Media;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using AtlasToolbox.ViewModels.ConfigurationVM;
using AtlasToolbox.Services;
//using System.Drawing;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public class ConfigurationItemViewModel : IConfigurationItem
    {
        public ToggleServiceRegister ToggleService { get; set; }
        public string Name => ToggleService.Name;
        public string Key => ToggleService.Key;
        public string Description => ToggleService.Description;
        public string RouteItem => ToggleService.Route;
        public FontIcon Icon => ToggleService.Icon;

        private bool _currentSetting => ToggleService.CurrentState;

        public bool CurrentSetting
        {
            get => _currentSetting;
            set
            {
                ToggleService.CurrentState = CurrentSetting;
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
            }
        }



        public ConfigurationItemViewModel(
            ToggleServiceRegister toggleService)
        {
            ToggleService = toggleService;
        }
    }
}
