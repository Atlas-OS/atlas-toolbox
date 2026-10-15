using AtlasToolbox.Atlas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtlasToolbox.Services
{
    /// <summary>
    /// A choice between more than two states of an Atlas toggle, offered in the order of
    /// Atlas's catalog.
    /// </summary>
    public class MultiServiceRegister : BaseServiceRegister
    {
        public CatalogToggle Toggle { get; }

        /// <summary>
        /// Initialize the multi service register
        /// </summary>
        /// <param name="key">Name of the toggle in Atlas's catalog</param>
        /// <param name="route">Where the service should appear in the toolbox</param>
        /// <param name="icon">Icon shown of the item</param>
        public MultiServiceRegister(string key, string route, string icon = "\uE897")
            : base(key, route, icon)
        {
            Toggle = AtlasContract.Catalog.Find(key);
        }

        public bool IsAvailable => Toggle is { States.Count: > 0 };

        public List<string> Options => Toggle.States.Select(state => state.Name).ToList();

        /// <summary>
        /// The recorded choice, or the default while none is recorded. An action that
        /// records nothing, such as starting in Safe Mode, has no current choice.
        /// </summary>
        public string ReadCurrent(AtlasState state)
        {
            if (Toggle.NoStateRecord) return null;
            CatalogState current = state?.Recorded(Key) is int recorded ? Toggle.StateWithValue(recorded) : Toggle.DefaultState;
            return current?.Name;
        }

        public Task<bool> SetAsync(string option) => ToggleActions.ApplyAsync(Toggle, Toggle.State(option));
    }
}
