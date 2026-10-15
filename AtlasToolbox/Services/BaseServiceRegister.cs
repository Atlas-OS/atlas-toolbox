namespace AtlasToolbox.Services
{
    public class BaseServiceRegister : IRoutable
    {
        public string Key { get; set; } // this is also the name of the toggle in Atlas's catalog
        /// <summary>The language-file key for the name and description; the key unless set.</summary>
        public string TextKey { get; set; }
        public string Name { get => App.GetValueFromItemList(TextKey ?? Key); }
        public string Description { get => App.GetValueFromItemList(TextKey ?? Key, true); }
        public string Route { get; set; }
        public string Icon { get; set; }
        public BaseServiceRegister() { }
        /// <summary>
        /// Contstructor for the base registry service.
        /// </summary>
        /// <param name="key">Name of the toggle in Atlas's catalog</param>
        /// <param name="route">Where the service should appear in the toolbox</param>
        /// <param name="icon">Icon shown of the item</param>
        public BaseServiceRegister(string key, string route, string icon = "\uE897")
        {
            Key = key;
            Route = route;
            Icon = icon;
        }
    }
}
