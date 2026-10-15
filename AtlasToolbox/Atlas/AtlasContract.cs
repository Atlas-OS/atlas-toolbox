using System;
using System.IO;
using System.Threading;

namespace AtlasToolbox.Atlas
{
    /// <summary>
    /// The installed Atlas as the Toolbox sees it: its toggle catalog and the choices it
    /// recorded. Loaded once at startup; the recorded choices are read again whenever
    /// Atlas changes them, whether through the Toolbox or the Atlas folder.
    /// </summary>
    public static class AtlasContract
    {
        public static AtlasCatalog Catalog { get; private set; }
        public static AtlasState State { get; private set; }

        /// <summary>Why this Atlas can't be used, for the log, when <see cref="Load"/> fails.</summary>
        public static string IncompatibleReason { get; private set; }

        /// <summary>Raised, on a worker thread, after Atlas recorded a change.</summary>
        public static event EventHandler<AtlasState> StateChanged;

        private static FileSystemWatcher _watcher;
        private static Timer _debounce;

        /// <summary>
        /// Loads the installed Atlas when it is a release this Toolbox supports:
        /// <paramref name="series"/> is the major and minor version, such as 0.6.
        /// </summary>
        public static bool Load(string series)
        {
            try
            {
                if (!File.Exists(AtlasPaths.StateDocument)) throw new FileNotFoundException("Atlas has no state document; it is older than 0.6 or not installed.");
                AtlasState state = AtlasState.Read(AtlasPaths.StateDocument);
                if (!IsSeries(state.InstalledVersion, series)) throw new InvalidDataException($"Atlas {state.InstalledVersion} is installed; this Toolbox supports Atlas {series}.");
                if (!File.Exists(AtlasPaths.ToggleLauncher)) throw new FileNotFoundException("The Atlas toggle launcher is missing.", AtlasPaths.ToggleLauncher);
                Catalog = AtlasCatalog.Load(AtlasPaths.Catalog);
                State = state;
                return true;
            }
            catch (Exception ex)
            {
                IncompatibleReason = ex.Message;
                return false;
            }
        }

        /// <summary>Whether a version such as 0.6.0 or 0.6.1-rc.2 belongs to a series such as 0.6.</summary>
        public static bool IsSeries(string version, string series)
        {
            if (string.IsNullOrWhiteSpace(version)) return false;
            string release = version.Split('-', '+')[0];
            return Version.TryParse(release, out Version installed) && Version.TryParse(series, out Version supported) &&
                installed.Major == supported.Major && installed.Minor == supported.Minor;
        }

        /// <summary>Reads the recorded choices again and tells everyone who shows them.</summary>
        public static void Refresh()
        {
            try
            {
                State = AtlasState.Read(AtlasPaths.StateDocument);
                StateChanged?.Invoke(null, State);
            }
            catch (Exception ex)
            {
                App.logger.Warn(ex, "[ATLAS] Couldn't read the recorded choices");
            }
        }

        /// <summary>Follows changes to the state document, which Atlas replaces whole.</summary>
        public static void Watch()
        {
            if (_watcher is not null) return;
            _debounce = new Timer(_ => Refresh(), null, Timeout.Infinite, Timeout.Infinite);
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(AtlasPaths.StateDocument), Path.GetFileName(AtlasPaths.StateDocument))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
            };
            FileSystemEventHandler changed = (_, _) => _debounce.Change(300, Timeout.Infinite);
            _watcher.Changed += changed;
            _watcher.Created += changed;
            _watcher.Renamed += (_, _) => _debounce.Change(300, Timeout.Infinite);
            _watcher.EnableRaisingEvents = true;
        }
    }
}
