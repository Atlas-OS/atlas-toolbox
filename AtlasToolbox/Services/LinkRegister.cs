
using AtlasToolbox.Utils;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Provider;

namespace AtlasToolbox.Services
{
    public class LinkRegister : IRoutable
    {
        public string Key { get; set; } // this is also the name of the service in the atlas core system
        public string Name { get => App.GetValueFromItemList(Key); }
        public string Description { get => App.GetValueFromItemList(Key, true); }
        public string Route { get; set; }
        public FontIcon Icon { get; set; } = new();
        public string Link { get; set; }

        /// <summary>
        /// Contstructor for the base registry service.
        /// </summary>
        /// <param name="key">Name of the service in the AtlasToggleLauncher</param>
        /// <param name="route">Where the service should appear in the toolbox</param>
        /// <param name="defaultValue">Default base value for the setting</param>
        /// <param name="icon">Icon shown of the item</param>
        public LinkRegister(string key, string link, string route,string icon = "\uE897")
        {
            Key = key;
            Route = route;
            Link = link;
            Icon.Glyph = icon;
        }
    }
}

