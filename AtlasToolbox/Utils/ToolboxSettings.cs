using Microsoft.Win32;
using System;

namespace AtlasToolbox.Utils
{
    /// <summary>
    /// The Toolbox's own settings, under HKLM\SOFTWARE\AtlasOS\Toolbox beside the record
    /// its installer writes. Earlier releases kept some of them under Atlas's toggle store
    /// (AtlasOS\Services\Toolbox), which belongs to Atlas; they are moved out once.
    /// </summary>
    public static class ToolboxSettings
    {
        public const string Key = @"HKLM\SOFTWARE\AtlasOS\Toolbox";
        private const string LegacySubKey = @"SOFTWARE\AtlasOS\Services\Toolbox";

        public static object Get(string name) => RegistryHelper.GetValue(Key, name);

        public static void Set(string name, object value) => RegistryHelper.SetValue(Key, name, value);

        public static void Set(string name, object value, RegistryValueKind kind) => RegistryHelper.SetValue(Key, name, value, kind);

        public static void MoveLegacy()
        {
            try
            {
                using RegistryKey machine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using (RegistryKey legacy = machine.OpenSubKey(LegacySubKey))
                {
                    if (legacy is null) return;
                    foreach (string name in new[] { "lang", "AppWidth", "AppHeight" })
                    {
                        if (legacy.GetValue(name) is object value && Get(name) is null)
                        {
                            Set(name, value, legacy.GetValueKind(name));
                        }
                    }
                }
                machine.DeleteSubKeyTree(LegacySubKey, false);
            }
            catch (Exception ex)
            {
                App.logger.Warn(ex, "[SETTINGS] Couldn't move the settings earlier Toolbox releases kept with Atlas's toggles");
            }
        }
    }
}
