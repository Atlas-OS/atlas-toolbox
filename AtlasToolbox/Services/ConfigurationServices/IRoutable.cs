namespace AtlasToolbox.Services.ConfigurationServices
{
    public interface IRoutable
    {
        bool IsEnabled();
        void Enable();
        void Disable();
    }
}
