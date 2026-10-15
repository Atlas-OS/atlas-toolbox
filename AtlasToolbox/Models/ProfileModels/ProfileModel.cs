using System.Collections.Generic;

namespace AtlasToolbox.Models.ProfileModels
{
    class ProfileModel
    {
        public string Name { get; set; }
        /// <summary>The on/off items that are on.</summary>
        public List<string> Config { get; set; }
        /// <summary>
        /// The on/off items that are off. Profiles from earlier releases have none, and
        /// leave every item they don't list as it is.
        /// </summary>
        public List<string> Disabled { get; set; }
        public List<KeyValuePair<string, string>> MultiConfig { get; set; }
    }
}
