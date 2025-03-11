using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Models.ProfileModels
{
    class ConfigModel
    {
        public string Name { get; set; }
        public string Key {  get; set; }
        public bool Value { get; set; }

        public ConfigModel(string name, string key, bool value)
        {
            Name = name;
            Key = key;
            Value = value;
        }
    }
}
