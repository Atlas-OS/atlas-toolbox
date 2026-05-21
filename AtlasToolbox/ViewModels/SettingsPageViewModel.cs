using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace AtlasToolbox.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private Language _currentLanguage;

        public Language CurrentLanguage 
        {
            get => _currentLanguage;
            set
            {
                if (SetProperty(ref _currentLanguage, value))
                {
                    SaveLanguage();
                }
            }
        }

        private void SaveLanguage()
        {
            if (CurrentLanguage == null) return;

            RegistryHelper.SetValue(@"HKLM\SOFTWARE\AtlasOS\Services\Toolbox", "lang", this.CurrentLanguage.Key);
            App.LoadLangString();
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
    }
}
