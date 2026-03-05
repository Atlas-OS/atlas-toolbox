using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class EventLogConfigurationService : IRoutable
    {
        private const string EVENTLOG_SERVICE_NAME = "EventLog";

        private readonly ConfigurationStore _eventLogConfigurationStore;

        public EventLogConfigurationService(
            [FromKeyedServices("EventLog")] ConfigurationStore eventLogConfigurationStore)
        {
            _eventLogConfigurationStore = eventLogConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(EVENTLOG_SERVICE_NAME, ServiceStartMode.Disabled);

            _eventLogConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(EVENTLOG_SERVICE_NAME, ServiceStartMode.Automatic);

            _eventLogConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return ServiceHelper.IsStartupTypeMatch(EVENTLOG_SERVICE_NAME, ServiceStartMode.Automatic);
        }
    }
}
