using AtlasToolbox.Services;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using System;
using BcdSharp;

namespace AtlasToolbox.HostBuilder
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.AddConfigurationServices();
            return host;
        }

        /// <summary>
        /// Register IConfigurationServices
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationServices(this IHostBuilder host)
        {
            host.ConfigureServices((_, services) =>
            {
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Animation");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("AppStoreArchiving");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Bluetooth");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("FSOGameBar");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("WindowsFirewall");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("GameMode");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Hags");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("LanmanWorkstation");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("MicrosoftStore");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("NetworkDiscovery");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Notifications");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Printing");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("SearchIndexing");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Troubleshooting");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Uwp");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Vpn");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("ModernAltTab");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("CpuIdleContextMenu");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("DarkTitlebars");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("LockScreen");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("ModernVolumeFlyout");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("RunWithPriority");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("ShortcutText");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("BootLogo");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("BootMessages");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("NewBootMenu");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("SpinningAnimation");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("AdvancedBootOptions");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("AutomaticRepair");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("KernelParameters");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("HighestMode");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("CompactView");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("QuickAccess");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("RemovableDrivesInSidebar");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("AutomaticUpdates");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("BackgroundApps");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("DeliveryOptimisation");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Hibernation");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Location");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("PhoneLink");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("PowerSaving");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Sleep");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("SystemRestore");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("UpdateNotifications");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("WebSearch");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Widgets");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("WindowsSpotlight");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("ExtractContextMenu");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("TakeOwnership");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("CpuIdle");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("OldContextMenu");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("EdgeSwipe");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("AppIconsThumbnail");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("AutomaticFolderDiscovery");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Gallery");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("SnapLayouts");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("RecentItems");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("VerboseMessages");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("NVidiaDisplayContainer");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("NVidiaDisplayContainerContextMenu");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("SuperFetch");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("HideAppBrowserControl");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("SecurityHealthTray");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("FaultTolerantHeap");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Copilot");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("Recall");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("ProcessExplorer");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("VbsState");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("GiveAccessToMenu");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("NetworkNavigationPane");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("FileSharing");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("WindowsHello");
                services.AddKeyedSingleton<IRoutable, ToggleServiceRegister>("ToggleWindowsUpdates");
                services.AddKeyedSingleton<IRoutable, MultiServiceRegister>("ContextMenuTerminals");
                services.AddKeyedSingleton<IRoutable, MultiServiceRegister>("ShortcutIcon");
                services.AddKeyedSingleton<IRoutable, MultiServiceRegister>("Mitigations");
                services.AddKeyedSingleton<IRoutable, MultiServiceRegister>("SafeMode");
            });
            App.logger.Info($"[SERVICES] Added services to host");
            return host;
        }
    }
}
