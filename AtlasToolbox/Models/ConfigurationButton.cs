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
    public class ConfigurationButton
    {
        public ICommand Command { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ConfigurationType Type { get; set; }
        public string Icon { get; set; }

        public ConfigurationButton(ICommand command, string name, string description, ConfigurationType type, string icon = "\uE897") 
        {
            Command = command;
            Name = name;
            Description = description;
            Type = type;
            
            Icon = icon;
        }
    }
}
