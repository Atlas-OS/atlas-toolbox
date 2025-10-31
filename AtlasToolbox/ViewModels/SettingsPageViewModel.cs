using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using AtlasToolbox.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace AtlasToolbox.ViewModels
{
    public partial class SettingsPageViewModel : INotifyPropertyChanged
    {
        public Language _currentLanguage { get; set; }
        public Language CurrentLanguage 
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                OnPropertyChanged(); // Notifies UI
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            RegistryHelper.SetValue(@"HKLM\SOFTWARE\AtlasOS\Services\Toolbox", "lang", this.CurrentLanguage.Key);
            App.LoadLangString();
            MainWindow mWindows = App.m_window as MainWindow;
        }

        public ObservableCollection<Language> Languages { get; set; }

        public SettingsPageViewModel()
        {
            Languages = new();
            Dictionary<string, string> langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@$"lang\index.json"));
            foreach (KeyValuePair<string, string> language in langs)
            {
                Languages.Add(new (language.Value, language.Key));
            }
            string lang = (string)RegistryHelper.GetValue(@"HKLM\SOFTWARE\AtlasOS\Services\Toolbox", "lang");
            CurrentLanguage = Languages.Where(item => item.Key == lang).FirstOrDefault();
        }

        public bool CheckUpdates()
        {
            if (ToolboxUpdateHelper.CheckUpdates())
            {
                App.ContentDialogCaller("newUpdate");
                return false;
            }else
            {
                return true;
            }
        }

        public bool CheckAtlasOSUpdates()
        {
            if (AtlasPatcherHelper.CheckForPatches())
            {
                App.ContentDialogCaller("newUpdate");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
