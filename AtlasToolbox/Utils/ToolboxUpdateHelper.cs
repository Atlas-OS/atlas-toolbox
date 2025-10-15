﻿using System;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace AtlasToolbox.Utils
{
    public class ToolboxUpdateHelper
    {
        const string RELEASE_URL = "https://api.github.com/repos/atlas-os/atlas-toolbox/releases/latest";
        public static string commandUpdate;
        public static bool CheckUpdates()
        {
            try
            {
                // get the api result
                string htmlContent = CommandPromptHelper.ReturnRunCommand("curl " + RELEASE_URL);
                var result = JsonDocument.Parse(htmlContent);
                string tagName = result.RootElement.GetProperty("tag_name").GetString();

                // Format everything to compare 
                int version = int.Parse(RegistryHelper.GetValue($@"HKLM\SOFTWARE\AtlasOS\Toolbox", "Version").ToString().Replace(".", ""));

                if (int.Parse(tagName.Replace(".", "").Replace("v", "")) > version)
                {
                    // get the download link and create a temporary directory
                    string downloadUrl = result.RootElement.GetProperty("assets")[0].GetProperty("browser_download_url").GetString();
                    string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(tempDirectory);

                    CommandPromptHelper.RunCommand($"cd {tempDirectory} && curl -LSs {downloadUrl} -O \"setup.exe\"");
                    commandUpdate = $"{tempDirectory}\\{downloadUrl.Split('/').Last()} /silent /install";
                    return true;
                }
            }catch
            {
                return false;
            }
            return false;
        }

        public static void InstallUpdate()
        {
            // Call the installer and close Toolbox
            CommandPromptHelper.RunCommand(commandUpdate, true, false);
            Environment.Exit(0);
        }
    }
}
