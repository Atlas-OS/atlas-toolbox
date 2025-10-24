using System;
using System.Collections.Generic;
using System.IO;
using AtlasToolbox.Models;
using AtlasToolbox.Models.ProfileModels;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AtlasToolbox.Utils
{
    public static class ProfileSerializing
    {
        /// <summary>
        /// Creates a .json profile with the enabled configuration services
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static Profiles CreateProfile(string profileName)
        {
            FileInfo[] profileFile = new DirectoryInfo($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles\\").GetFiles();
            List<string> configModelList = new ();
            List<KeyValuePair<string, string>> multiConfigModelList = new ();
            ProfileModel profileModel = new ();

            // Checks for enabled config services and adds them to the configModelList
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
            App.logger.Info($"[PROFILES] Loaded profile: \"{profileModel.Name}\"");
            List<Profiles> listProfiles = new();

            return new Profiles(profileModel.Name, profileModel.Name, profileModel.Config, profileModel.MultiConfig);
        }
    }
}
