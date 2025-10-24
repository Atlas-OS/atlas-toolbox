using System;
using System.Diagnostics;
using System.Management;

namespace AtlasToolbox.Utils
{
    public class ProcessHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void StartShellExecute(string fileName)
        {
            ProcessStartInfo startInfo = new(fileName)
            {
                UseShellExecute = true
            };
            using (Process.Start(startInfo))
            { }
        }

        public static void KillProcessByName(string processName)
        {
            using ManagementObjectSearcher searcher = new($"SELECT * FROM Win32_Process WHERE Name LIKE '{processName}'");
            foreach (ManagementObject process in searcher.Get())
            {
                process.InvokeMethod("Terminate", Array.Empty<object>());
            }
            App.logger.Info($"Killed all processes with name: {processName}");
        }
    }
}
