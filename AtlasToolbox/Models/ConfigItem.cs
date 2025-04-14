using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AtlasToolbox.Enums;
using Microsoft.UI.Xaml.Controls;

namespace AtlasToolbox.Models
{
    public class ConfigItem
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public ConfigurationCategory Category { get; set; }
        public ConfigurationType Type { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public ICommand Command { get; set; }

        /// <summary>
        /// Normal setting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="category"></param>
        public ConfigItem(string name, string key, ConfigurationCategory category)
        {
            Name = name;
            Key = key;
            Category = category;
            ConfigurationType type = ConfigurationType.Normal;
        }

        /// <summary>
        /// Link settings
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="category"></param>
        /// <param name="link"></param>
        public ConfigItem(string name, string key, ConfigurationCategory category, string link)
        {
            Name = name;
            Key = key;
            Category = category;
            Link = link;
            ConfigurationType type = ConfigurationType.Link;
        }

        /// <summary>
        /// Button Setting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="category"></param>
        /// <param name="command"></param>
        /// <param name="description"></param>
        public ConfigItem(string name, string key, ConfigurationCategory category, ICommand command, string description)
        {
            Name = name;
            Key = key;
            Category = category;
            Command = command;
            Description = description;
            ConfigurationType type = ConfigurationType.Button;
        }
        
        /// <summary>
        /// Submenu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="category"></param>
        public ConfigItem(string name, string key, string description, ConfigurationCategory category)
        {
            Name = name;
            Category = category;
            Description = description;
            ConfigurationType type = ConfigurationType.SubMenu;
        }
    }
}
