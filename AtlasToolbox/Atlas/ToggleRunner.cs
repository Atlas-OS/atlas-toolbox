using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AtlasToolbox.Atlas
{
    /// <summary>What running a toggle did, and how it was started.</summary>
    public sealed record ToggleResult(bool Succeeded, int ExitCode, string Message, bool Interactive, bool AsUser);

    /// <summary>
    /// Runs Atlas toggles through Atlas's own launcher, by name, exactly as the Atlas
    /// folder's launchers do. One toggle runs at a time.
    /// </summary>
    public static partial class ToggleRunner
    {
        private static readonly SemaphoreSlim Gate = new(1, 1);

        static ToggleRunner()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Applies one state of a toggle.
        /// <list type="bullet">
        /// <item>A state that only changes the PC runs silently from the Toolbox, which is
        /// already elevated, so it needs no prompt.</item>
        /// <item>A state that changes the signed-in user's own settings runs in a window as
        /// that user without administrator rights, as Atlas requires; Atlas asks for
        /// administrator rights itself for the rest.</item>
        /// <item>An action (a toggle that records no choice) runs in a window, because most
        /// actions ask questions or show their results.</item>
        /// </list>
        /// </summary>
        public static async Task<ToggleResult> RunAsync(CatalogToggle toggle, CatalogState state, bool? interactive = null)
        {
            bool asUser = state.Work.User || toggle.Elevation == "None";
            bool inWindow = interactive ?? (toggle.NoStateRecord || state.Work.User);
            string launcherPath = Path.Combine(AtlasPaths.AtlasDesktop, state.Launcher ?? toggle.Launcher ?? toggle.Name + ".cmd");
            if (launcherPath.Contains('"') || toggle.Name.Contains('"') || state.Name.Contains('"'))
            {
                throw new ArgumentException("A toggle, state or launcher name contains a quotation mark.");
            }
            // cmd.exe /s /c strips exactly the outer quotation marks around the launcher call.
            // "call" keeps the launcher's exit code: run directly, a bare exit /b in it reports 0.
            string arguments = $"/d /s /c \"call \"{AtlasPaths.ToggleLauncher}\" {toggle.Name} {state.Name} \"{launcherPath}\"{(inWindow ? "" : " /silent")}\"";

            await Gate.WaitAsync().ConfigureAwait(false);
            try
            {
                App.logger.Info($"[TOGGLE] {toggle.Name} {state.Name}: {(asUser ? "as the signed-in user" : "elevated")}, {(inWindow ? "in a window" : "silently")}");
                ToggleResult result = asUser
                    ? await RunAsUserAsync(arguments, inWindow).ConfigureAwait(false)
                    : inWindow
                        ? await RunElevatedInWindowAsync(arguments).ConfigureAwait(false)
                        : await RunElevatedSilentlyAsync(arguments).ConfigureAwait(false);
                App.logger.Info($"[TOGGLE] {toggle.Name} {state.Name} exited {result.ExitCode}{(string.IsNullOrEmpty(result.Message) ? "" : ": " + result.Message)}");
                return result;
            }
            finally
            {
                Gate.Release();
            }
        }

        /// <summary>
        /// Runs a launcher from the Atlas folder in a window as the signed-in user, as
        /// opening it there does; it asks for administrator rights itself when it needs them.
        /// </summary>
        public static async Task<ToggleResult> RunLauncherAsUserAsync(string launcherPath)
        {
            if (launcherPath.Contains('"')) throw new ArgumentException("The launcher path contains a quotation mark.");
            if (!File.Exists(launcherPath)) throw new FileNotFoundException("The Atlas launcher is missing.", launcherPath);
            await Gate.WaitAsync().ConfigureAwait(false);
            try
            {
                App.logger.Info($"[TOGGLE] {launcherPath}: as the signed-in user, in a window");
                return await RunAsUserAsync($"/d /s /c \"call \"{launcherPath}\"\"", inWindow: true).ConfigureAwait(false);
            }
            finally
            {
                Gate.Release();
            }
        }

        private static async Task<ToggleResult> RunAsUserAsync(string arguments, bool inWindow)
        {
            string commandLine = $"\"{AtlasPaths.CommandProcessor}\" {arguments}";
            using SafeProcessHandle process = UnelevatedProcess.Start(AtlasPaths.CommandProcessor, commandLine, AtlasPaths.System32, hidden: !inWindow);
            int exitCode = await UnelevatedProcess.WaitForExitAsync(process).ConfigureAwait(false);
            return new ToggleResult(exitCode == 0, exitCode, Describe(exitCode, null), inWindow, AsUser: true);
        }

        private static async Task<ToggleResult> RunElevatedInWindowAsync(string arguments)
        {
            ProcessStartInfo start = NewStart(arguments);
            start.CreateNoWindow = false;
            using Process process = Process.Start(start) ?? throw new InvalidOperationException("The toggle launcher did not start.");
            await process.WaitForExitAsync().ConfigureAwait(false);
            return new ToggleResult(process.ExitCode == 0, process.ExitCode, Describe(process.ExitCode, null), true, AsUser: false);
        }

        private static async Task<ToggleResult> RunElevatedSilentlyAsync(string arguments)
        {
            ProcessStartInfo start = NewStart(arguments);
            start.CreateNoWindow = true;
            // Atlas writes its console output in the console code page. Nothing can answer
            // a question here, so input is closed rather than left to wait.
            Encoding console = Encoding.GetEncoding((int)GetOEMCP());
            start.RedirectStandardInput = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.StandardOutputEncoding = console;
            start.StandardErrorEncoding = console;
            using Process process = Process.Start(start) ?? throw new InvalidOperationException("The toggle launcher did not start.");
            process.StandardInput.Close();
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> error = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync().ConfigureAwait(false);
            string text = (await output.ConfigureAwait(false)).TrimEnd() + "\n" + (await error.ConfigureAwait(false)).TrimEnd();
            if (!string.IsNullOrWhiteSpace(text)) App.logger.Info($"[TOGGLE] Output:\n{text.Trim()}");
            return new ToggleResult(process.ExitCode == 0, process.ExitCode, Describe(process.ExitCode, text), false, AsUser: false);
        }

        private static ProcessStartInfo NewStart(string arguments)
        {
            ProcessStartInfo start = new(AtlasPaths.CommandProcessor, arguments)
            {
                UseShellExecute = false,
                WorkingDirectory = AtlasPaths.System32,
            };
            start.Environment.Remove("ATLAS_USER_CONTEXT");
            return start;
        }

        /// <summary>Why a run failed, from Atlas's own failure line when it printed one.</summary>
        private static string Describe(int exitCode, string output)
        {
            if (exitCode == 0) return null;
            if (exitCode == 87) return "The Atlas toggle launcher refused its arguments.";
            // A silent run logs "[time] [-] [ERROR] Applying toggle 'Name' failed: reason | At ...".
            Match failure = FailureLine().Matches(output ?? "").LastOrDefault();
            return failure?.Groups["reason"].Value.Trim();
        }

        [GeneratedRegex(@"Applying toggle '[^']+' failed: (?<reason>.*?)(?: \| At .*)?\r?$", RegexOptions.Multiline)]
        private static partial Regex FailureLine();

        [DllImport("kernel32.dll")]
        private static extern uint GetOEMCP();
    }
}
