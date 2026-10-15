using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace AtlasToolbox.Atlas
{
    /// <summary>
    /// Atlas's machine state document (%windir%\AtlasOS\state.json): the installed Atlas
    /// version and the choice recorded for each toggle. Atlas replaces the file whole when
    /// it records a choice, so it is read with sharing that never blocks that.
    /// </summary>
    public sealed class AtlasState
    {
        public string InstalledVersion { get; private init; }

        private readonly Dictionary<string, int> _toggles;

        private AtlasState(string installedVersion, Dictionary<string, int> toggles)
        {
            InstalledVersion = installedVersion;
            _toggles = toggles;
        }

        public static AtlasState Read(string path)
        {
            StateDocument document = null;
            for (int attempt = 1; ; attempt++)
            {
                try
                {
                    using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                    document = JsonSerializer.Deserialize(stream, StateJsonContext.Default.StateDocument);
                    break;
                }
                catch (IOException) when (attempt < 5 && File.Exists(path))
                {
                    // Atlas is replacing the document; it takes moments.
                    Thread.Sleep(100 * attempt);
                }
            }
            if (document is null) throw new InvalidDataException("The Atlas state document is empty.");

            Dictionary<string, int> toggles = new(StringComparer.Ordinal);
            foreach (KeyValuePair<string, ToggleRecord> record in document.Toggles ?? [])
            {
                if (record.Value?.State is int state) toggles[record.Key] = state;
            }
            return new AtlasState(document.InstalledVersion, toggles);
        }

        /// <summary>The value recorded for a toggle, or null when nothing was recorded.</summary>
        public int? Recorded(string toggleName) => _toggles.TryGetValue(toggleName, out int state) ? state : null;
    }

    public sealed class StateDocument
    {
        [JsonPropertyName("schemaVersion")] public int SchemaVersion { get; set; }
        [JsonPropertyName("installedVersion")] public string InstalledVersion { get; set; }
        [JsonPropertyName("toggles")] public Dictionary<string, ToggleRecord> Toggles { get; set; }
    }

    public sealed class ToggleRecord
    {
        [JsonPropertyName("state")] public int? State { get; set; }
    }

    [JsonSerializable(typeof(StateDocument))]
    internal sealed partial class StateJsonContext : JsonSerializerContext
    {
    }
}
