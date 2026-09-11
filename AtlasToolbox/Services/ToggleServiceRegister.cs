
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
    public class ToggleServiceRegister : BaseServiceRegister
    {
        private const string ATLAS_STORE_KEY_NAME = @"HKLM\SOFTWARE\AtlasOS\Services";
        private static readonly string toggleLauncherPath = Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.Windows),
           @"AtlasModules\Scripts\Entry\Invoke-AtlasToggleLauncher.cmd"
       );
        private readonly string switchStateCommand = $"call {toggleLauncherPath}";
        public bool DefaultValue { get; set; }
        public bool CurrentState { get; set; }

        /// <summary>
        /// Contstructor for the base registry service.
        /// </summary>
        /// <param name="key">Name of the service in the AtlasToggleLauncher</param>
        /// <param name="route">Where the service should appear in the toolbox</param>
        /// <param name="defaultValue">False for default is disable, True for enable is default</param>
        /// <param name="icon">Icon shown of the item</param>
        public ToggleServiceRegister(string key, string route, bool defaultValue, string icon = "\uE897")
        {
            Key = key;
            Route = route;
            Icon.Glyph = icon;
            DefaultValue = defaultValue;
            try
            {
                CurrentState = GetCurrentState();
            
            }
            catch
            {
                App.logger.Error($"[INITIALIZATION] {Key} was not 1 or 0 in the registry. Reverting to default value...");
                ToDefault();
            }
        }

        private bool GetCurrentState()
        {
            return RegistryHelper.IsMatch(ATLAS_STORE_KEY_NAME + Key, "state", 1);
        }

        private string SwitchState()
        {
            string returnString = ServiceToggle(CurrentState ? "disable" : "enable");
            CurrentState = GetCurrentState();
            return returnString;
        }
        private string ToDefault()
        {
            string returnString = ServiceToggle(DefaultValue ? "enable" : "disable");
            CurrentState = GetCurrentState();
            return returnString;
        }

        private string ServiceToggle(string toggleValue)
        {
            using (Process commandPrompt = new Process())
            {
                commandPrompt.StartInfo.FileName = "cmd.exe";
                commandPrompt.StartInfo.Arguments = $"/c {switchStateCommand} {this.Key} {toggleValue} \"%~f0\" %*";
                commandPrompt.StartInfo.CreateNoWindow = true;
                commandPrompt.StartInfo.UseShellExecute = true;

                commandPrompt.StartInfo.RedirectStandardOutput = true;
                commandPrompt.StartInfo.RedirectStandardError = true;

                commandPrompt.Start();

                string output = commandPrompt.StandardOutput.ReadToEnd();
                string error = commandPrompt.StandardError.ReadToEnd();

                string result = output + (string.IsNullOrWhiteSpace(error) ? "" : "\n[Error]\n" + error);
                App.logger.Info($"[CMD] {switchStateCommand}:\n\t{result}");
                commandPrompt.WaitForExit();

                return result;
            }
        }
    }
}

