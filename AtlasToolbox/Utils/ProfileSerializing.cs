using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using AtlasToolbox.Models;
using AtlasToolbox.ViewModels;
using ICSharpCode.Decompiler.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace AtlasToolbox.Utils
{
    public static class ProfileSerializing
    {
        public static void CreateProfile(string profileName)
        {
            List<object> listConfigurationServices = new List<object>();

            listConfigurationServices.Add(App._host.Services.GetRequiredService<IEnumerable<ConfigurationItemViewModel>>());
            listConfigurationServices.Add(App._host.Services.GetRequiredService<IEnumerable<MultiOptionConfigurationItemViewModel>>());

            //todo: Change profiles to be Json files 
            DirectoryInfo profilesDirectory = new DirectoryInfo("..\\..\\..\\..\\Profiles\\");
            FileInfo[] profileFile = profilesDirectory.GetFiles();

            
            //outputFile.WriteLine(Name);

            IEnumerable<ConfigurationItemViewModel> configViewModels = App._host.Services.GetRequiredService<IEnumerable<ConfigurationItemViewModel>>();

            List<string> configItemKeys = new List<string>();

            foreach (ConfigurationItemViewModel configItemViewModel in configViewModels)
            {
                if (configItemViewModel.CurrentSetting)
                {
                    (configItemViewModel.Key);
                    configItemKeys.Add(configItemViewModel.Key);
                }
            }

            string jsonString = JsonSerializer.Serialize(listConfigurationServices);

            File.WriteAllText($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles\\{profileName}.json", jsonString);
        }

        public static Profiles DeserializeProfile(FileInfo file)
        {
            string jsonString = File.ReadAllText(file.FullName);
            
        }
    }
}
