using AtlasToolbox.Atlas;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace AtlasToolbox.Utils
{
    public class ToolboxUpdateHelper
    {
        // The release Atlas installs: the latest stable release on GitHub.
        const string RELEASE_URL = "https://api.github.com/repos/Atlas-OS/atlas-toolbox/releases/latest";
        public static string version = "";

        /// <summary>
        /// Whether GitHub has a newer stable Toolbox release than the one installed.
        /// </summary>
        public static bool CheckUpdates()
        {
            try
            {
                using HttpClient client = new() { Timeout = TimeSpan.FromSeconds(20) };
                client.DefaultRequestHeaders.UserAgent.ParseAdd("AtlasToolbox");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
                using JsonDocument release = JsonDocument.Parse(client.GetStringAsync(RELEASE_URL).GetAwaiter().GetResult());
                version = release.RootElement.GetProperty("tag_name").GetString()?.TrimStart('v', 'V') ?? "";

                string installed = RegistryHelper.GetValue(@"HKLM\SOFTWARE\AtlasOS\Toolbox", "Version")?.ToString();
                return Version.TryParse(version, out Version latest) && Version.TryParse(installed, out Version current) && latest > current;
            }
            catch (Exception e)
            {
                App.logger.Error(e, "Failed to check for updates");
                return false;
            }
        }

        /// <summary>
        /// Installs the latest release through Atlas, which checks the release and its
        /// download before running it. The installer closes the Toolbox.
        /// </summary>
        public static void InstallUpdate()
        {
            if (!File.Exists(AtlasPaths.ToolboxInstaller))
            {
                App.logger.Error($"Atlas's Toolbox installer is missing at {AtlasPaths.ToolboxInstaller}");
                return;
            }
            // The Toolbox closes so its files can be replaced; the same PowerShell opens the
            // new release once Atlas has installed it.
            string toolbox = Environment.ProcessPath;
            if (toolbox is null || toolbox.Contains('\'') || AtlasPaths.ToolboxInstaller.Contains('\''))
            {
                App.logger.Error("The Toolbox or its installer has a path the update can't pass on");
                return;
            }
            string command = $"& '{AtlasPaths.ToolboxInstaller}' -Silent; if ($LASTEXITCODE -eq 0) {{ Start-Process -FilePath '{toolbox}' }}";
            Process.Start(new ProcessStartInfo(AtlasPaths.WindowsPowerShell,
                $"-NoProfile -NoLogo -NonInteractive -ExecutionPolicy Bypass -Command \"{command}\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = AtlasPaths.System32,
            })?.Dispose();
            App.logger.Info($"Installing Toolbox {version}");
            Environment.Exit(0);
        }
    }
}
