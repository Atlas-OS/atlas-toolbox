using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services
{
    public class MultiServiceRegister : BaseServiceRegister
    {
        public List<string> Options;
        public string DefaultOption;
        public string CurrentOption;

        /// <summary>
        /// Initialize the multi service register
        /// </summary>
        /// <param name="key"></param>
        /// <param name="route"></param>
        /// <param name="defaultOption"></param>
        /// <param name="icon"></param>
        public MultiServiceRegister(string key, string route, string[] options, string defaultOption, string icon = "")
        {
            Key = key;
            Route = route;
            Options = new(options);
            Icon.Glyph = icon;
            DefaultOption = defaultOption;
            //try
            //{
            //    CurrentState = GetCurrentState();

            //}
            //catch
            //{
            //    App.logger.Error($"[INITIALIZATION] {Key} was not 1 or 0 in the registry. Reverting to default value...");
            //    ToDefault();
            //}
        }

    }
}
