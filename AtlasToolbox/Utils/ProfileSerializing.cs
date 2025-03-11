using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using AtlasToolbox.Models;
using AtlasToolbox.Models.ProfileModels;
using AtlasToolbox.ViewModels;
using ICSharpCode.Decompiler.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AtlasToolbox.Utils
{
    public static class ProfileSerializing
    {
        public static Profiles CreateProfile(string profileName)
        {
            //List<object> listConfigurationServices = new List<object>();

            //listConfigurationServices.Add(App._host.Services.GetRequiredService<IEnumerable<ConfigurationItemViewModel>>());
            //listConfigurationServices.Add(App._host.Services.GetRequiredService<IEnumerable<MultiOptionConfigurationItemViewModel>>());

            //todo: Change profiles to be Json files 
            DirectoryInfo profilesDirectory = new DirectoryInfo("..\\..\\..\\..\\Profiles\\");
            FileInfo[] profileFile = profilesDirectory.GetFiles();


            //outputFile.WriteLine(Name);

            List<string> configModelList = new ();
            List<KeyValuePair<string, string>> multiConfigModelList = new ();
            ProfileModel profileModel = new ();

            foreach (ConfigurationItemViewModel configItemViewModel in App._host.Services.GetRequiredService<IEnumerable<ConfigurationItemViewModel>>())
            {
                if (configItemViewModel.CurrentSetting == true) configModelList.Add(configItemViewModel.Key.ToString());
            }
            foreach (MultiOptionConfigurationItemViewModel configItemViewModel in App._host.Services.GetRequiredService<IEnumerable<MultiOptionConfigurationItemViewModel>>())
            {
                multiConfigModelList.Add(new (configItemViewModel.Key, configItemViewModel.CurrentSetting.ToString()));
            }
            profileModel.Name = profileName;
            profileModel.Config = configModelList;
            profileModel.MultiConfig = multiConfigModelList;

            string jsonString = System.Text.Json.JsonSerializer.Serialize(profileModel);

            File.WriteAllText($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles\\{profileName}.json", jsonString);
            return DeserializeProfile($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles\\{profileName}.json");
        }

        public static Profiles DeserializeProfile(string file)
        {
            ProfileModel profileModel = JsonConvert.DeserializeObject<ProfileModel>(File.ReadAllText(file));
            List<Profiles> listProfiles = new();

            return new Profiles(profileModel.Name, profileModel.Name, profileModel.Config, profileModel.MultiConfig);
        }
    }
}
