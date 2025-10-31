using System.Diagnostics;

namespace AtlasToolbox.Utils
{
    public class CommandPromptHelper
    {
        /// <summary>
        /// Runs any given command in parameters
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="noWindow">True by default</param>
        public static string RunCommand(string command, bool noWindow = true, bool waitForExit = true)
        {
            using (Process commandPrompt = new Process())
            {
                commandPrompt.StartInfo.FileName = "cmd.exe";
                commandPrompt.StartInfo.Arguments = $"/c {command}";
                commandPrompt.StartInfo.CreateNoWindow = noWindow;
                commandPrompt.StartInfo.UseShellExecute = false;

                // 👇 redirect both output and error streams
                commandPrompt.StartInfo.RedirectStandardOutput = true;
                commandPrompt.StartInfo.RedirectStandardError = true;

                commandPrompt.Start();

                // Read output (non-blocking async way)
                string output = commandPrompt.StandardOutput.ReadToEnd();
                string error = commandPrompt.StandardError.ReadToEnd();

                if (waitForExit)
                    commandPrompt.WaitForExit();

                // Combine both streams if you want full context
                string result = output + (string.IsNullOrWhiteSpace(error) ? "" : "\n[Error]\n" + error);

                return result;
            }
        }
        /// <summary>
        /// Restarts explorer.exe
        /// if there's a better way to do this, please make a PR
        /// </summary>
        public static void RestartExplorer()
        {
            Process stopExplorer = new Process();
            stopExplorer.StartInfo.FileName = "cmd.exe";
            stopExplorer.StartInfo.Arguments = $"/c taskkill /f /im explorer.exe";
            stopExplorer.StartInfo.CreateNoWindow = true;
            stopExplorer.StartInfo.UseShellExecute = false;

            stopExplorer.Start();
            stopExplorer.WaitForExit();

            Process startExplorer = new Process();
            startExplorer.StartInfo.FileName = "cmd.exe";
            startExplorer.StartInfo.Arguments = $"/c explorer.exe";
            startExplorer.StartInfo.CreateNoWindow = true;
            startExplorer.StartInfo.UseShellExecute = false;

            startExplorer.Start();
            startExplorer.WaitForExit();
        }

        public static string ReturnRunCommand(string command)
        {
            Process commandPrompt = new Process();
            commandPrompt.StartInfo.FileName = "cmd.exe";
            commandPrompt.StartInfo.Arguments = $"/c {command}";
            commandPrompt.StartInfo.CreateNoWindow = true;
            commandPrompt.StartInfo.UseShellExecute = false;
            commandPrompt.StartInfo.RedirectStandardOutput = true;

            commandPrompt.Start();
            string output = commandPrompt.StandardOutput.ReadToEnd();
            commandPrompt.WaitForExit();
            return output;
        }
    }
}
