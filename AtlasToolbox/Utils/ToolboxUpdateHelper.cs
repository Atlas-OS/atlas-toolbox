using System;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace AtlasToolbox.Utils
{
    public class ToolboxUpdateHelper
    {
        const string RELEASE_URL = "https://api.github.com/repos/atlas-os/atlas-toolbox/releases/latest";
        public static string commandUpdate;
        public static JsonDocument result;
        public static bool CheckUpdates()
        {
            try
            {
                // get the api result
                string htmlContent = CommandPromptHelper.ReturnRunCommand("curl " + RELEASE_URL);
                result = JsonDocument.Parse(htmlContent);
                string tagName = result.RootElement.GetProperty("tag_name").GetString();

                // Format everything to compare 
                int version = int.Parse(RegistryHelper.GetValue($@"HKLM\SOFTWARE\AtlasOS\Toolbox", "Version").ToString().Replace(".", ""));

                if (int.Parse(tagName.Replace(".", "").Replace("v", "")) > version)
                {
                    return true;
                }
            }catch (Exception ex)
            {
                App.logger.Error("[UPDATEHELPER] An error occurred while checking for updates.", ex.Message + ex.InnerException);
                return false;
            }
            return false;
        }

        public static void InstallUpdate()
        {
            // Call the installer and close Toolbox
            // get the download link and create a temporary directory
            string downloadUrl = result.RootElement.GetProperty("assets")[0].GetProperty("browser_download_url").GetString();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);

            CommandPromptHelper.RunCommand($"cd {tempDirectory} && curl -LSs {downloadUrl} -O \"setup.exe\"");
            commandUpdate = $"{tempDirectory}\\{downloadUrl.Split('/').Last()} /silent /install";
            CommandPromptHelper.RunCommandToUpdate(commandUpdate);
        }
    }
}
