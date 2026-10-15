using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtlasToolbox.Atlas;
using AtlasToolbox.Models;
using AtlasToolbox.Models.ProfileModels;
using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AtlasToolbox.Utils
{
    public static class ProfileSerializing
    {
        /// <summary>
        /// Creates a .json profile with what Atlas has applied for every item
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static Profiles CreateProfile(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName) || profileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("A profile name can't be empty or contain characters Windows doesn't allow in file names.", nameof(profileName));
            }
            List<ConfigurationItemViewModel> items = App._host.Services.GetRequiredService<IEnumerable<ConfigurationItemViewModel>>().ToList();
            ProfileModel profileModel = new()
            {
                Name = profileName,
                Config = items.Where(item => item.CurrentSetting).Select(item => item.Key).ToList(),
                Disabled = items.Where(item => !item.CurrentSetting).Select(item => item.Key).ToList(),
                // An action such as Safe Mode has no current choice and isn't kept.
                MultiConfig = App._host.Services.GetRequiredService<IEnumerable<MultiConfigViewModel>>()
                    .Where(item => item.CurrentSetting is not null)
                    .Select(item => new KeyValuePair<string, string>(item.Key, item.CurrentSetting))
                    .ToList(),
            };

            string path = Path.Combine(ProtectedFolder.Ensure(AtlasPaths.DataRoot, AtlasPaths.Profiles), profileName + ".json");
            File.WriteAllText(path, JsonConvert.SerializeObject(profileModel));
            return DeserializeProfile(path);
        }

        public static Profiles DeserializeProfile(string file)
        {
            ProfileModel profileModel = JsonConvert.DeserializeObject<ProfileModel>(File.ReadAllText(file))
                ?? throw new InvalidDataException("The profile is empty.");
            App.logger.Info($"[PROFILES] Loaded profile: \"{profileModel.Name}\"");

            return new Profiles(profileModel.Name, Path.GetFileNameWithoutExtension(file), profileModel.Config, profileModel.Disabled, profileModel.MultiConfig);
        }

        /// <summary>
        /// Moves profiles that earlier releases kept inside Atlas's files to the Toolbox's
        /// own folder, which updating Atlas leaves alone.
        /// </summary>
        public static void MoveLegacyProfiles()
        {
            try
            {
                if (!Directory.Exists(AtlasPaths.LegacyProfiles)) return;
                ProtectedFolder.Ensure(AtlasPaths.DataRoot, AtlasPaths.Profiles);
                foreach (string file in Directory.GetFiles(AtlasPaths.LegacyProfiles, "*.json"))
                {
                    string destination = Path.Combine(AtlasPaths.Profiles, Path.GetFileName(file));
                    if (!File.Exists(destination)) File.Copy(file, destination);
                }
                Directory.Delete(AtlasPaths.LegacyProfiles, true);
            }
            catch (Exception ex)
            {
                App.logger.Warn(ex, "[PROFILES] Couldn't move the profiles earlier releases kept with Atlas's files");
            }
        }
    }
}
