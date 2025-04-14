using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Models;
using AtlasToolbox.Stores;
using System.Windows.Input;
using AtlasToolbox.Commands;
using AtlasToolbox.Enums;
using Windows.UI;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.ViewModels
{
    public class MultiOptionConfigurationItemViewModel
    {
        private readonly MultiOptionConfigurationStore _configurationStore;
        private readonly IMultiOptionConfigurationServices _configurationService;

        public MultiOptionConfiguration Configuration { get; set; }
        public string Name => Configuration.Name;
        public ConfigurationCategory Type => Configuration.Type;
        public FontIcon Icon => Configuration.Icon;

        public List<string> Options => _configurationStore.Options; 
        public string Key => Configuration.Key;

        public Color Color { get; set; }


        private string _currentSetting;

        public string CurrentSetting
        {
            get => _currentSetting;
            set
            {
                _currentSetting = value;
                _configurationStore.CurrentSetting = CurrentSetting;
                this.MultiOptionSaveConfigurationCommand.Execute(this);
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

        public ICommand MultiOptionSaveConfigurationCommand { get; }

        // Temporarily commented until a more consistent defenition for what a risk is is found
        //public Color SetColor(RiskRating riskRating)
        //{
        //    switch (riskRating)
        //    {
        //        case RiskRating.HighRisk:
        //            return Color.FromArgb(255, 255, 0, 0);
        //        case RiskRating.MediumRisk:
        //            return Color.FromArgb(255, 255, 255, 0);
        //        case RiskRating.LowRisk:
        //            return Color.FromArgb(255, 0, 176, 80);
        //    }
        //    return Color.FromArgb(255, 0, 176, 80);
        //}

        public MultiOptionConfigurationItemViewModel(
            MultiOptionConfiguration configuration,
            MultiOptionConfigurationStore configurationStore,
            IMultiOptionConfigurationServices configurationService)
        {
            Configuration = configuration;

            _configurationStore = configurationStore;
            _configurationService = configurationService;

            _currentSetting = FetchCurrentSetting();
            //Color = SetColor(Configuration.RiskRating);
            //RiskRatingIcon = RiskRatingFormatter(Configuration.RiskRating);

            MultiOptionSaveConfigurationCommand = new MultiOptionSaveConfigurationCommand(this, configurationStore, configurationService);
        }

        public string FetchCurrentSetting()
        {
            IsBusy = true;

            try
            {
                string currentSetting = _configurationService.Status();
                _configurationStore.CurrentSetting = currentSetting;
                return currentSetting;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
