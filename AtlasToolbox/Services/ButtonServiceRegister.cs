using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AtlasToolbox.Services
{
    public class ButtonServiceRegister : BaseServiceRegister
    {
        public ICommand Command { get; set; }

        public ButtonServiceRegister(string key, ICommand command, string route, string icon = "\uE897") : base(key, route, icon)
        {
            Key = key;
            Route = route;
            Command = command;
            Icon.Glyph = icon;
        }
    }
}
