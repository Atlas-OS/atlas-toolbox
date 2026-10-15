using System.Collections.Generic;

namespace AtlasToolbox.Models
{
    public class Profiles
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public List<string> ConfigurationServices { get; set; }
        /// <summary>The on/off items to turn off; null in a profile from an earlier release.</summary>
        public List<string> DisabledServices { get; set; }
        public List<KeyValuePair<string, string>> MultiOptionConfigServices { get; set; }

        // What screen readers announce for the item.
        public override string ToString() => Name;

        public Profiles(
            string name,
            string key,
            List<string> configurationServices,
            List<string> disabledServices,
            List<KeyValuePair<string, string>> multiOptionConfigServices)
        {
            Name = name;
            Key = key;
            ConfigurationServices = configurationServices ?? new();
            DisabledServices = disabledServices;
            MultiOptionConfigServices = multiOptionConfigServices ?? new();
        }

    }
}
