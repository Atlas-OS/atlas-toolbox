using AtlasToolbox.Atlas;
using System.Threading.Tasks;

namespace AtlasToolbox.Services
{
    /// <summary>
    /// An on/off item: an Atlas toggle whose two states are Disable and Enable.
    /// </summary>
    public class ToggleServiceRegister : BaseServiceRegister
    {
        public CatalogToggle Toggle { get; }

        private readonly bool? _unrecorded;

        /// <summary>
        /// Contstructor for the toggle service.
        /// </summary>
        /// <param name="key">Name of the toggle in Atlas's catalog</param>
        /// <param name="route">Where the service should appear in the toolbox</param>
        /// <param name="icon">Icon shown of the item</param>
        /// <param name="unrecorded">What to show while Atlas has recorded no choice and its catalog names no default</param>
        public ToggleServiceRegister(string key, string route, string icon = "\uE897", string textKey = null, bool? unrecorded = null)
            : base(key, route, icon)
        {
            TextKey = textKey;
            _unrecorded = unrecorded;
            Toggle = AtlasContract.Catalog.Find(key);
        }

        /// <summary>Whether this Atlas has the toggle as an on/off choice.</summary>
        public bool IsAvailable => Toggle is { IsSwitch: true };

        /// <summary>
        /// Whether the toggle is on: the recorded choice, or, when Atlas recorded none, the
        /// state Atlas applies by default.
        /// </summary>
        public bool ReadIsOn(AtlasState state)
        {
            if (state?.Recorded(Key) is int recorded) return recorded == 1;
            if (Toggle.DefaultState is CatalogState fallback) return fallback.Name == "Enable";
            return _unrecorded ?? false;
        }

        public CatalogState StateFor(bool on) => Toggle.State(on ? "Enable" : "Disable");

        public Task<bool> SetAsync(bool on) => ToggleActions.ApplyAsync(Toggle, StateFor(on));
    }
}
