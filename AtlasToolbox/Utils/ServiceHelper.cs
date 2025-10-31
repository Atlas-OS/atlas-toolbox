using System.ServiceProcess;

namespace AtlasToolbox.Utils
{
    public class ServiceHelper
    {
        /// <summary>
        /// Returns the startup type of a service
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        /// <returns></returns>
        public static ServiceStartMode GetStartupType(string serviceName)
        {
            using ServiceController serviceController = new(serviceName);
            return serviceController.StartType;
        }

        /// <summary>
        /// Sets the startup type
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        /// <param name="startupType">Startup type of the service</param>
        public static void SetStartupType(string serviceName, ServiceStartMode startupType)
        {
            string keyName = $@"HKLM\SYSTEM\CurrentControlSet\Services\{serviceName}";
            RegistryHelper.SetValue(keyName, "Start", (int)startupType);
            App.logger.Info($"Set {serviceName} startup type to {startupType}");
        }

        /// <summary>
        /// Checks for a match
        /// </summary>
        /// <param name="serviceName">Name of the service to match</param>
        /// <param name="startupType">Startup type to match</param>
        /// <returns></returns>
        public static bool IsStartupTypeMatch(string serviceName, ServiceStartMode startupType)
        {
            return GetStartupType(serviceName) == startupType;
        }
    }
}
