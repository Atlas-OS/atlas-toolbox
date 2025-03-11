using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.Services.ConfigurationServices;

namespace AtlasToolbox.Models
{
    public class Profiles
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public List<string> ConfigurationServices { get; set; }
        public List<KeyValuePair<string, string>> MultiOptionConfigServices { get; set; }

        public Profiles(
            string name,
            string key,
            List<string> configurationServices,
            List<KeyValuePair<string, string>> multiOptionConfigServices) 
        {
            Name = name;
            Key = key;
            ConfigurationServices = configurationServices;
            MultiOptionConfigServices = multiOptionConfigServices;
        }

    }
}
