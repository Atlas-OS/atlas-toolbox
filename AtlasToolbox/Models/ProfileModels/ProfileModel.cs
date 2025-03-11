using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Models.ProfileModels
{
    class ProfileModel
    {
        public string Name { get; set; }
        public List<string> Config { get; set; }
        public List<KeyValuePair<string, string>> MultiConfig { get; set; }
    }
}
