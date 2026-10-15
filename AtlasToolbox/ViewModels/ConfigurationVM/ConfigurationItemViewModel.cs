using AtlasToolbox.Atlas;
using AtlasToolbox.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    public partial class ConfigurationItemViewModel : ObservableObject, IConfigurationItem
    {
        public ToggleServiceRegister ToggleService { get; set; }
        public string Name => ToggleService.Name;
        public string Key => ToggleService.Key;
        public string Description => ToggleService.Description;
        public string RouteItem => ToggleService.Route;
        public string Icon => ToggleService.Icon;

        /// <summary>What Atlas has applied: the recorded choice, or its default.</summary>
        public bool CurrentSetting { get; private set; }

        /// <summary>What the switch shows: the applied state, except while the user's change runs.</summary>
        [ObservableProperty]
        private bool _isOn;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsIdle))]
        private bool _isBusy;

        public bool IsIdle => !IsBusy;

        public ConfigurationItemViewModel(
            ToggleServiceRegister toggleService)
        {
            ToggleService = toggleService;
            ShowApplied(AtlasContract.State);
            AtlasContract.StateChanged += (_, state) => App.UiQueue?.TryEnqueue(() => { if (!IsBusy) ShowApplied(state); });
        }

        /// <summary>
        /// Applies the state the switch was moved to, then shows what Atlas applied, which
        /// is the old state again when the change didn't apply.
        /// </summary>
        public async Task ApplyAsync(bool on)
        {
            if (IsBusy) return;
            if (on == CurrentSetting)
            {
                IsOn = on;
                return;
            }
            // Busy first: showing the requested state raises Toggled, which must not
            // start a second run.
            IsBusy = true;
            IsOn = on;
            try
            {
                await ToggleService.SetAsync(on);
            }
            finally
            {
                IsBusy = false;
                ShowApplied(AtlasContract.State);
            }
        }

        private void ShowApplied(AtlasState state)
        {
            CurrentSetting = ToggleService.ReadIsOn(state);
            IsOn = CurrentSetting;
        }
    }
}
