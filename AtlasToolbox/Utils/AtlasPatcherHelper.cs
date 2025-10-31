using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AtlasToolbox.Utils
{
    public class AtlasPatcherHelper
    {
        public static bool CheckForPatches()
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
            }
            catch (Exception ex)
            {
                App.logger.Error("[UPDATEHELPER] An error occurred while checking for updates.", ex.Message + ex.InnerException);
                return false;
            }
            return false;
        }
    }
}
