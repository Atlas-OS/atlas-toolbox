using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels;
using MVVMEssentials.Commands;

namespace AtlasToolbox.Commands
{
    public class MultiOptionSaveConfigurationCommand : AsyncCommandBase
    {
        private readonly MultiOptionConfigurationItemViewModel _configurationItemViewModel;
        private readonly MultiOptionConfigurationStore _configurationStore;
        private readonly IMultiOptionConfigurationServices _configurationService;

        public MultiOptionSaveConfigurationCommand(
            MultiOptionConfigurationItemViewModel configurationItemViewModel,
            MultiOptionConfigurationStore configurationStore,
            IMultiOptionConfigurationServices configurationService)
        {
            _configurationItemViewModel = configurationItemViewModel;
            _configurationStore = configurationStore;
            _configurationService = configurationService;
        }
        /// <summary>
        /// Saves the state of a MultiOptionConfigurationService
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(object parameter)
        {
            int currentSetting = _configurationItemViewModel.Options.IndexOf(_configurationStore.CurrentSetting);

            App.logger.Info($"Changed {_configurationItemViewModel.Key} to option index {currentSetting}");
            RegistryHelper.SetServiceState(_configurationItemViewModel.Key, currentSetting);
            _configurationItemViewModel.IsBusy = true;

            try
            {
                await Task.Run(() => _configurationService.ChangeStatus(currentSetting));
            }
            finally
            {
                _configurationItemViewModel.IsBusy = false;
            }
        }
    }
}
