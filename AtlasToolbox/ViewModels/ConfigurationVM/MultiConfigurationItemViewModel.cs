using AtlasToolbox.Atlas;
using AtlasToolbox.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AtlasToolbox.ViewModels.ConfigurationVM
{
    /// <summary>One state of a choice, with the text the list shows for it.</summary>
    public sealed record MultiOption(string Name, string Label);

    public partial class MultiConfigViewModel : ObservableObject, IConfigurationItem
    {
        public MultiServiceRegister Configuration { get; set; }
        public string Name => Configuration.Name;
        public string Icon => Configuration.Icon;
        public string Key => Configuration.Key;
        public string RouteItem => Configuration.Route;

        public List<MultiOption> Options { get; }

        /// <summary>
        /// The state Atlas has applied, by name; null for an action that records nothing.
        /// Choosing another one in the list applies it.
        /// </summary>
        [ObservableProperty]
        private string _currentSetting;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsIdle))]
        private bool _isBusy;

        public bool IsIdle => !IsBusy;

        private bool _showingRecorded;

        public MultiConfigViewModel(
            MultiServiceRegister configuration)
        {
            Configuration = configuration;
            Options = configuration.Options.Select(option => new MultiOption(option, Label(configuration.Key, option))).ToList();
            ShowRecorded(AtlasContract.State);
            AtlasContract.StateChanged += (_, state) => App.UiQueue?.TryEnqueue(() => { if (!IsBusy) ShowRecorded(state); });
        }

        partial void OnCurrentSettingChanged(string value)
        {
            if (_showingRecorded || value is null) return;
            _ = ApplyAsync(value);
        }

        public async Task ApplyAsync(string option)
        {
            if (IsBusy || option == Configuration.ReadCurrent(AtlasContract.State)) return;
            IsBusy = true;
            try
            {
                await Configuration.SetAsync(option);
            }
            finally
            {
                IsBusy = false;
                ShowRecorded(AtlasContract.State);
            }
        }

        private void ShowRecorded(AtlasState state)
        {
            _showingRecorded = true;
            try { CurrentSetting = Configuration.ReadCurrent(state); }
            finally { _showingRecorded = false; }
        }

        /// <summary>The language file's text for a state, or the state's name in words.</summary>
        private static string Label(string key, string option)
        {
            string text = App.GetValueFromItemList(key + "_" + option);
            return text != App.GetValueFromItemList("ToBeTranslated") ? text : Regex.Replace(option, "(?<=[a-z])(?=[A-Z])", " ");
        }
    }
}
