using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Models.ProfileModels
{
    class MultiConfigModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public MultiConfigModel(string name, string key, string value)
        {
            Name = name;
            Key = key;
            Value = value;
        }
    }
}
