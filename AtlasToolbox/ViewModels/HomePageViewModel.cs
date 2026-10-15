using AtlasToolbox.Atlas;
using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels.ConfigurationVM;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AtlasToolbox.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private IEnumerable<ConfigurationItemViewModel> ConfigurationItemViewModels { get; }
        private IEnumerable<MultiConfigViewModel> MultiOptionConfigurationItemViewModels { get; }


        [ObservableProperty]
        public ObservableCollection<Profiles> _profilesList;

        [ObservableProperty]
        public string _name;

        [ObservableProperty]
        public Profiles _profileSelected;

        public HomePageViewModel(
            IEnumerable<Profiles> profiles,
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<MultiConfigViewModel> multiOptionConfigurationItemViewModels)
        {
            _name = "";
            ConfigurationItemViewModels = configurationItemViewModels;
            MultiOptionConfigurationItemViewModels = multiOptionConfigurationItemViewModels;
            _profilesList = new();
            foreach (Profiles profile in profiles) { ProfilesList.Add(profile); }
        }

        public static HomePageViewModel LoadViewModel(
            IEnumerable<Profiles> profiles,
            IEnumerable<ConfigurationItemViewModel> configurationItemViewModels,
            IEnumerable<MultiConfigViewModel> multiOptionConfigurationItemViewModels)
        {
            HomePageViewModel viewModel = new(profiles, configurationItemViewModels, multiOptionConfigurationItemViewModels);

            return viewModel;
        }

        [RelayCommand]
        private void AddProfile()
        {
            try
            {
                Profiles profile = ProfileSerializing.CreateProfile(Name.Trim());
                Profiles existing = ProfilesList.FirstOrDefault(item => item.Key == profile.Key);
                if (existing is not null) ProfilesList.Remove(existing);
                ProfilesList.Add(profile);
            }
            catch (Exception ex)
            {
                App.logger.Error(ex, "[PROFILES] Couldn't save the profile");
            }
        }

        [RelayCommand]
        private void RemoveProfile()
        {
            if (ProfileSelected is null) return;
            string file = Path.Combine(AtlasPaths.Profiles, ProfileSelected.Key + ".json");
            if (File.Exists(file)) File.Delete(file);
            ProfilesList.Remove(ProfileSelected);
        }

        /// <summary>
        /// Applies the selected profile: only what differs from what Atlas has applied,
        /// one change after another.
        /// </summary>
        [RelayCommand]
        private async Task SetProfile()
        {
            Profiles profile = ProfileSelected;
            if (profile is null)
            {
                App.logger.Warn("Tried to set a profile whilst nothing was selected");
                return;
            }
            await ToggleActions.RunBatchAsync(async () =>
            {
                foreach (ConfigurationItemViewModel viewModel in ConfigurationItemViewModels)
                {
                    bool? wanted = profile.ConfigurationServices.Contains(viewModel.Key) ? true
                        : profile.DisabledServices?.Contains(viewModel.Key) == true ? false
                        : null;
                    if (wanted is bool on && on != viewModel.CurrentSetting) await viewModel.ApplyAsync(on);
                }
                foreach (KeyValuePair<string, string> keyPair in profile.MultiOptionConfigServices)
                {
                    // An action such as Safe Mode never runs from a profile.
                    MultiConfigViewModel viewModel = MultiOptionConfigurationItemViewModels.FirstOrDefault(vm => vm.Key == keyPair.Key);
                    if (viewModel is not null && !viewModel.Configuration.Toggle.NoStateRecord &&
                        keyPair.Value is not null && viewModel.Options.Any(option => option.Name == keyPair.Value))
                    {
                        await viewModel.ApplyAsync(keyPair.Value);
                    }
                }
            });
        }
    }
}
