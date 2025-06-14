﻿using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Models;
using AtlasToolbox.Stores;
using System.Windows.Input;
using AtlasToolbox.Commands;
using AtlasToolbox.Enums;
using Windows.UI;
using Microsoft.UI.Xaml.Media;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
//using System.Drawing;

namespace AtlasToolbox.ViewModels
{
    public class ConfigurationItemViewModel : IConfigurationItem
    {
        private readonly ConfigurationStore _configurationStore;
        private readonly IConfigurationService _configurationService;

        public Configuration Configuration { get; set; }
        public string Name => Configuration.Name;
        public string Key => Configuration.Key;
        public ConfigurationType Type => Configuration.Type;
        public FontIcon Icon => Configuration.Icon;
        public Color Color { get; set; }

        private bool _currentSetting;

        public bool CurrentSetting
        {
            get => _currentSetting;
            set
            {
                _currentSetting = value;
                _configurationStore.CurrentSetting = CurrentSetting;
                this.SaveConfigurationCommand.Execute(this);
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


        public ICommand SaveConfigurationCommand { get; }

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
        //        default:
        //            return Color.FromArgb(255, 0, 176, 80);
        //    }
        //}

        public ConfigurationItemViewModel(
            Configuration configuration,
            ConfigurationStore configurationStore,
            IConfigurationService configurationService)
        {
            _configurationStore = configurationStore;
            _configurationService = configurationService;
            Configuration = configuration;

            _currentSetting = FetchCurrentSetting();
            //Task.Run(() =>
            //{
            //    Color = SetColor(Configuration.RiskRating);
            //    RiskRatingIcon = RiskRatingFormatter(Configuration.RiskRating);
            //});
            SaveConfigurationCommand = new SaveConfigurationCommand(this, configurationStore, configurationService);
            
        }

        public bool FetchCurrentSetting()
        {
            IsBusy = true;

            try
            {
                bool currentSetting = _configurationService.IsEnabled();
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
