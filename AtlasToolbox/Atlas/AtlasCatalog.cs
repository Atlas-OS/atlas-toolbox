using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtlasToolbox.Atlas
{
    /// <summary>
    /// Atlas's toggle catalog (AtlasModules\Toggles\catalog.json): every toggle, its
    /// states and what each state needs. Atlas generates it from the same definitions its
    /// launchers run, so the Toolbox never keeps its own copy of what a toggle does.
    /// </summary>
    public sealed class AtlasCatalog
    {
        public const int SupportedSchema = 1;

        private readonly Dictionary<string, CatalogToggle> _toggles;

        private AtlasCatalog(IEnumerable<CatalogToggle> toggles)
        {
            _toggles = toggles.ToDictionary(toggle => toggle.Name, StringComparer.Ordinal);
        }

        public static AtlasCatalog Load(string path)
        {
            CatalogDocument document;
            using (FileStream stream = File.OpenRead(path))
            {
                document = JsonSerializer.Deserialize(stream, CatalogJsonContext.Default.CatalogDocument)
                    ?? throw new InvalidDataException("The Atlas toggle catalog is empty.");
            }
            if (document.SchemaVersion != SupportedSchema)
            {
                throw new InvalidDataException($"The Atlas toggle catalog has schema {document.SchemaVersion}, not {SupportedSchema}.");
            }
            return new AtlasCatalog(document.Toggles ?? []);
        }

        public CatalogToggle Find(string name) => _toggles.TryGetValue(name, out CatalogToggle toggle) ? toggle : null;

        public IEnumerable<CatalogToggle> Toggles => _toggles.Values;
    }

    public sealed class CatalogDocument
    {
        [JsonPropertyName("schemaVersion")] public int SchemaVersion { get; set; }
        [JsonPropertyName("toggles")] public List<CatalogToggle> Toggles { get; set; }
    }

    public sealed class CatalogToggle
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("group")] public string Group { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }

        /// <summary>None, Admin or TrustedInstaller.</summary>
        [JsonPropertyName("elevation")] public string Elevation { get; set; }

        /// <summary>The toggle is an action: running it records no choice.</summary>
        [JsonPropertyName("noStateRecord")] public bool NoStateRecord { get; set; }

        /// <summary>One launcher offers every state as a menu (<see cref="Launcher"/>).</summary>
        [JsonPropertyName("menu")] public bool Menu { get; set; }

        /// <summary>A menu toggle's launcher, relative to AtlasDesktop.</summary>
        [JsonPropertyName("launcher")] public string Launcher { get; set; }

        /// <summary>What Atlas asks the user to accept before applying a state.</summary>
        [JsonPropertyName("warning")] public string Warning { get; set; }

        [JsonPropertyName("states")] public List<CatalogState> States { get; set; } = [];

        public CatalogState State(string name) => States.FirstOrDefault(state => state.Name == name);

        public CatalogState StateWithValue(int value) => States.FirstOrDefault(state => state.StateValue == value);

        /// <summary>The state Atlas applies when nobody chose one: its launcher says "(default)".</summary>
        public CatalogState DefaultState =>
            States.FirstOrDefault(state => (state.Launcher ?? state.MenuLabel ?? "").Contains("(default)", StringComparison.Ordinal));

        /// <summary>
        /// A switch: exactly Disable and Enable, recorded as 0 and 1, as Atlas defines every
        /// two-state toggle.
        /// </summary>
        public bool IsSwitch =>
            !NoStateRecord && States.Count == 2 && State("Disable")?.StateValue == 0 && State("Enable")?.StateValue == 1;
    }

    public sealed class CatalogState
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        /// <summary>The value Atlas records for this state, or null when it records none.</summary>
        [JsonPropertyName("stateValue")] public int? StateValue { get; set; }

        /// <summary>This state's launcher, relative to AtlasDesktop; null in menu toggles.</summary>
        [JsonPropertyName("launcher")] public string Launcher { get; set; }

        [JsonPropertyName("menuLabel")] public string MenuLabel { get; set; }

        /// <summary>None, Recommend, Prompt or RestartExplorer.</summary>
        [JsonPropertyName("reboot")] public string Reboot { get; set; }

        [JsonPropertyName("work")] public CatalogWork Work { get; set; } = new();
    }

    public sealed class CatalogWork
    {
        /// <summary>
        /// The state changes the signed-in user's own settings. Atlas does that only in an
        /// unelevated process of that user, so it never runs from the elevated Toolbox.
        /// </summary>
        [JsonPropertyName("user")] public bool User { get; set; }
    }

    [JsonSerializable(typeof(CatalogDocument))]
    internal sealed partial class CatalogJsonContext : JsonSerializerContext
    {
    }
}
