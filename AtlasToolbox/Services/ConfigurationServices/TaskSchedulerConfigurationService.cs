using AtlasToolbox.Stores;
using AtlasToolbox.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceProcess;

namespace AtlasToolbox.Services.ConfigurationServices
{
    public class TaskSchedulerConfigurationService : IRoutable
    {
        private const string SCHEDULE_SERVICE_NAME = "Schedule";

        private readonly ConfigurationStore _taskSchedulerConfigurationStore;

        public TaskSchedulerConfigurationService(
            [FromKeyedServices("TaskScheduler")] ConfigurationStore taskSchedulerConfigurationStore)
        {
            _taskSchedulerConfigurationStore = taskSchedulerConfigurationStore;
        }

        public void Disable()
        {
            ServiceHelper.SetStartupType(SCHEDULE_SERVICE_NAME, ServiceStartMode.Disabled);

            _taskSchedulerConfigurationStore.CurrentSetting = IsEnabled();
        }

        public void Enable()
        {
            ServiceHelper.SetStartupType(SCHEDULE_SERVICE_NAME, ServiceStartMode.Automatic);

            _taskSchedulerConfigurationStore.CurrentSetting = IsEnabled();
        }

        public bool IsEnabled()
        {
            return ServiceHelper.IsStartupTypeMatch(SCHEDULE_SERVICE_NAME, ServiceStartMode.Automatic);
        }
    }
}
