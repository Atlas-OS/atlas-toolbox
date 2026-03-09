using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.ViewModels.ConfigurationVM;
using MVVMEssentials.Commands;
using System.Threading.Tasks;

namespace AtlasToolbox.Commands
{
    public class SaveConfigurationCommand : AsyncCommandBase
    {
        private readonly ConfigurationItemViewModel _configuration;
        private readonly ConfigurationStore _configurationStore;
        private readonly IRoutable _configurationService;

        public SaveConfigurationCommand(
            ConfigurationItemViewModel configuration,
            ConfigurationStore configurationStore,
            IRoutable configurationService)
        {
            _configuration = configuration;
            _configurationStore = configurationStore;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Saves the current state of a ConfigurationService
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(object parameter)
        {
            bool currentSetting = _configurationStore.CurrentSetting;

            App.logger.Info($"Toggled {_configuration.Key} to {currentSetting}");
            _configuration.IsBusy = true;

            try
            {
                await Task.Run(currentSetting
                    ? _configurationService.Enable
                    : _configurationService.Disable);
            }
            finally
            {
                _configuration.IsBusy = false;
            }
        }
    }
}
