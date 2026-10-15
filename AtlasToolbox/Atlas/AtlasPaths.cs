using System;
using System.IO;

namespace AtlasToolbox.Atlas
{
    /// <summary>
    /// Where Atlas keeps the files the Toolbox reads and runs. Everything is under the
    /// Windows directory, which only administrators can change.
    /// </summary>
    public static class AtlasPaths
    {
        public static string Windows { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public static string System32 { get; } = Environment.GetFolderPath(Environment.SpecialFolder.System);
        public static string AtlasModules { get; } = Path.Combine(Windows, "AtlasModules");
        public static string AtlasDesktop { get; } = Path.Combine(Windows, "AtlasDesktop");

        /// <summary>The toggle catalog Atlas generates from its toggle definitions.</summary>
        public static string Catalog { get; } = Path.Combine(AtlasModules, "Toggles", "catalog.json");

        /// <summary>Atlas's machine state document: the installed version and recorded choices.</summary>
        public static string StateDocument { get; } = Path.Combine(Windows, "AtlasOS", "state.json");

        /// <summary>The shared body every Atlas toggle launcher calls.</summary>
        public static string ToggleLauncher { get; } = Path.Combine(AtlasModules, "Scripts", "Entry", "Invoke-AtlasToggleLauncher.cmd");

        /// <summary>Atlas's own Toolbox installer, which checks the release before running it.</summary>
        public static string ToolboxInstaller { get; } = Path.Combine(AtlasModules, "Scripts", "Entry", "Install-Toolbox.ps1");

        public static string CommandProcessor { get; } = Path.Combine(System32, "cmd.exe");
        public static string WindowsPowerShell { get; } = Path.Combine(System32, "WindowsPowerShell", "v1.0", "powershell.exe");

        /// <summary>
        /// The Toolbox's own data: profiles and logs. Not part of Atlas's files; created
        /// through <see cref="Utils.ProtectedFolder"/> from <see cref="DataRoot"/> down.
        /// </summary>
        public static string DataRoot { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "AtlasOS");
        public static string ToolboxData { get; } = Path.Combine(DataRoot, "Toolbox");
        public static string Profiles { get; } = Path.Combine(ToolboxData, "Profiles");

        /// <summary>Where Toolbox releases before this one kept profiles, inside Atlas's files.</summary>
        public static string LegacyProfiles { get; } = Path.Combine(AtlasModules, "Toolbox", "Profiles");

        public static string Logs { get; } = Path.Combine(ToolboxData, "Logs");
    }
}
