using ICSharpCode.Decompiler.IL;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;

namespace AtlasToolbox.Utils
{
    public class RegistryHelper
    {
        public static object GetValue(string keyPath, string valueName)
        {   
            using RegistryKey key = OpenKey(keyPath);
            return key?.GetValue(valueName);
        }


        public static void SetValue(string keyPath, string valueName, object value)
        {
            using RegistryKey key = OpenKey(keyPath, true);
            key?.SetValue(valueName, value);
            App.logger.Info($"[REGHELPER] Set registry value: {keyPath}\\{valueName} to {value}");
        }

        public static void SetValue(string keyPath, string valueName, object value, RegistryValueKind valueKind)
        {
            using RegistryKey key = OpenKey(keyPath, true);

            value = value switch
            {
                uint => BitConverter.ToInt32(BitConverter.GetBytes((uint)value)),
                ulong => BitConverter.ToInt64(BitConverter.GetBytes((ulong)value)),
                _ => value
            };

            key?.SetValue(valueName, value, valueKind);
            App.logger.Info($"[REGHELPER] Set registry value: {keyPath}\\{valueName} to {value} ({valueKind})");
        }

        public static void DeleteValue(string keyPath, string valueName)
        {
            using RegistryKey key = OpenKey(keyPath, true);
            key?.DeleteValue(valueName, false);
            App.logger.Info($"[REGHELPER] Deleted registry value: {keyPath}\\{valueName}");
        }

        public static bool IsMatch(string keyPath, string valueName, object data)
        {
            if (keyPath is null)
            {
                return false;
            }

            data = data switch
            {
                uint => BitConverter.ToInt32(BitConverter.GetBytes((uint)data)),
                ulong => BitConverter.ToInt64(BitConverter.GetBytes((ulong)data)),
                _ => data
            };

            object registryData = GetValue(keyPath, valueName);

            return registryData is byte[] byteArrayData && data is byte[] byteArray
                ? byteArrayData.SequenceEqual(byteArray)
                : registryData?.Equals(data) ?? data is null;
        }

        private static RegistryKey OpenKey(string keyPath, bool writable = false)
        {
            string[] split = keyPath.Split('\\');

            RegistryHive hive = split[0] switch
            {
                "HKCR" => RegistryHive.ClassesRoot,
                "HKCU" => RegistryHive.CurrentUser,
                "HKLM" => RegistryHive.LocalMachine,
                _ => throw new Exception("Hive was not found")
            };

            string keyName = string.Join('\\', split[1..]);

            RegistryKey baseKey;
            if (Environment.Is64BitOperatingSystem)
                baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
            else
                baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry32);

            return writable
                ? baseKey.CreateSubKey(keyName)
                : baseKey.OpenSubKey(keyName, writable);
        }

        public static void DeleteKey(string keyPath)
        {
            string[] split = keyPath.Split('\\');

            string parentKeyPath = string.Join('\\', split[..^1]);
            string targetKeyName = split[^1];

            using RegistryKey key = OpenKey(parentKeyPath, true);
            key?.DeleteSubKeyTree(targetKeyName, false);
            App.logger.Info($"[REGHELPER] Deleted registry key: {keyPath}");
        }
        public static bool KeyExists(string keyPath)
        {
            using RegistryKey key = OpenKey(keyPath);
            return key is not null;
        }

        public static void MergeRegFile(string regFilePath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "regedit.exe",
                Arguments = $"/s \"{regFilePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
        
            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }
            App.logger.Info($"[REGHELPER] Merged registry file: \"{regFilePath}\"");
        }

        // Methods for automatic service state management
        public static void SetServiceState(string serviceKey, int state)
        {
            SetValue(@$"HKLM\SOFTWARE\AtlasOS\Services\{serviceKey}", "state", state);
        }

    }
}
