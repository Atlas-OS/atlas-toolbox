using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Services;
using AtlasToolbox.Services.ConfigurationSubMenu;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using System;
using AtlasOSToolbox.Services.ConfigurationServices;
using BcdSharp;

namespace AtlasToolbox.HostBuilder
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices((_,services) =>
            {
                services.AddSingleton(CreateBcdStore);
                services.AddSingleton<IDismService, DismService>();
                services.AddSingleton<IBcdService, BcdService>();
            });

            host.AddConfigurationServices();
            host.AddConfigurationMenus();

            return host;
        }

        private static BcdStore CreateBcdStore(IServiceProvider _)
        {
            return BcdStore.OpenStore();
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
                services.AddKeyedSingleton<IRoutable, AnimationsConfigurationService>("Animations");
                services.AddKeyedSingleton<IRoutable, AppStoreArchivingConfigurationService>("AppStoreArchiving");
                services.AddKeyedSingleton<IRoutable, BluetoothConfigurationService>("Bluetooth");
                services.AddKeyedSingleton<IRoutable, FsoAndGameBarConfigurationService>("FsoAndGameBar");
                services.AddKeyedSingleton<IRoutable, WindowsFirewallConfigurationService>("WindowsFirewall");
                services.AddKeyedSingleton<IRoutable, GameModeConfigurationService>("GameMode");
                services.AddKeyedSingleton<IRoutable, HagsConfigurationService>("Hags");
                services.AddKeyedSingleton<IRoutable, LanmanWorkstationConfigurationService>("LanmanWorkstation");
                services.AddKeyedSingleton<IRoutable, MicrosoftStoreConfigurationService>("MicrosoftStore");
                services.AddKeyedSingleton<IRoutable, NetworkDiscoveryConfigurationService>("NetworkDiscovery");
                services.AddKeyedSingleton<IRoutable, NotificationsConfigurationService>("Notifications");
                services.AddKeyedSingleton<IRoutable, PrintingConfigurationService>("Printing");
                services.AddKeyedSingleton<IRoutable, SearchIndexingConfigurationService>("SearchIndexing");
                services.AddKeyedSingleton<IRoutable, TroubleshootingConfigurationService>("Troubleshooting");
                services.AddKeyedSingleton<IRoutable, UwpConfigurationService>("Uwp");
                services.AddKeyedSingleton<IRoutable, VpnConfigurationService>("Vpn");
                services.AddKeyedSingleton<IRoutable, ModernAltTabConfigurationService>("ModernAltTab");
                services.AddKeyedSingleton<IRoutable, CpuIdleContextMenuConfigurationService>("CpuIdleContextMenu");
                services.AddKeyedSingleton<IRoutable, DarkTitlebarsConfigurationService>("DarkTitlebars");
                services.AddKeyedSingleton<IRoutable, LockScreenConfigurationService>("LockScreen");
                services.AddKeyedSingleton<IRoutable, ModernVolumeFlyoutConfigurationService>("ModernVolumeFlyout");
                services.AddKeyedSingleton<IRoutable, RunWithPriorityConfigurationService>("RunWithPriority");
                services.AddKeyedSingleton<IRoutable, ShortcutTextConfigurationService>("ShortcutText");
                services.AddKeyedSingleton<IRoutable, BootLogoConfigurationService>("BootLogo");
                services.AddKeyedSingleton<IRoutable, BootMessagesConfigurationService>("BootMessages");
                services.AddKeyedSingleton<IRoutable, NewBootMenuConfigurationService>("NewBootMenu");
                services.AddKeyedSingleton<IRoutable, SpinningAnimationConfigurationService>("SpinningAnimation");
                services.AddKeyedSingleton<IRoutable, AdvancedBootOptionsConfigurationService>("AdvancedBootOptions");
                services.AddKeyedSingleton<IRoutable, AutomaticRepairConfigurationService>("AutomaticRepair");
                services.AddKeyedSingleton<IRoutable, KernelParametersConfigurationService>("KernelParameters");
                services.AddKeyedSingleton<IRoutable, HighestModeConfigurationService>("HighestMode");
                services.AddKeyedSingleton<IRoutable, CompactViewConfigurationService>("CompactView");
                services.AddKeyedSingleton<IRoutable, QuickAccessConfigurationService>("QuickAccess");
                services.AddKeyedSingleton<IRoutable, RemovableDrivesInSidebarConfigurationService>("RemovableDrivesInSidebar");
                services.AddKeyedSingleton<IRoutable, AutomaticUpdatesConfigurationService>("AutomaticUpdates");
                services.AddKeyedSingleton<IRoutable, BackgroundAppsConfigurationService>("BackgroundApps");
                services.AddKeyedSingleton<IRoutable, DeliveryOptimisationConfigurationService>("DeliveryOptimisation");
                services.AddKeyedSingleton<IRoutable, HibernationConfigurationService>("Hibernation");
                services.AddKeyedSingleton<IRoutable, LocationConfigurationService>("Location");
                services.AddKeyedSingleton<IRoutable, PhoneLinkConfigurationService>("PhoneLink");
                services.AddKeyedSingleton<IRoutable, PowerSavingConfigurationService>("PowerSaving");
                services.AddKeyedSingleton<IRoutable, SleepConfigurationService>("Sleep");
                services.AddKeyedSingleton<IRoutable, SystemRestoreConfigurationService>("SystemRestore");
                services.AddKeyedSingleton<IRoutable, UpdateNotificationsConfigurationService>("UpdateNotifications");
                services.AddKeyedSingleton<IRoutable, WebSearchConfigurationService>("WebSearch");
                services.AddKeyedSingleton<IRoutable, WidgetsConfigurationService>("Widgets");
                services.AddKeyedSingleton<IRoutable, WindowsSpotlightConfigurationService>("WindowsSpotlight");
                services.AddKeyedSingleton<IRoutable, ExtractContextMenuConfigurationService>("ExtractContextMenu");
                services.AddKeyedSingleton<IRoutable, TakeOwnershipConfigurationService>("TakeOwnership");
                services.AddKeyedSingleton<IRoutable, CpuIdleConfigurationService>("CpuIdle");
                services.AddKeyedSingleton<IRoutable, OldContextMenuConfigurationService>("OldContextMenu");
                services.AddKeyedSingleton<IRoutable, EdgeSwipeConfigurationService>("EdgeSwipe");
                services.AddKeyedSingleton<IRoutable, AppIconsThumbnailConfigurationService>("AppIconsThumbnail");
                services.AddKeyedSingleton<IRoutable, AutomaticFolderDiscoveryConfigurationService>("AutomaticFolderDiscovery");
                services.AddKeyedSingleton<IRoutable, GalleryConfigurationService>("Gallery");
                services.AddKeyedSingleton<IRoutable, SnapLayoutsConfigurationService>("SnapLayout");
                services.AddKeyedSingleton<IRoutable, RecentItemsConfigurationService>("RecentItems");
                services.AddKeyedSingleton<IRoutable, VerboseStatusMessageConfiguarationServices>("VerboseStatusMessage");
                services.AddKeyedSingleton<IRoutable, NvidiaDispayContainerConfigurationService>("NvidiaDispayContainer");
                services.AddKeyedSingleton<IRoutable, AddNvidiaDisplayContainerContextMenuConfigurationService>("AddNvidiaDisplayContainerContextMenu");
                services.AddKeyedSingleton<IRoutable, SuperFetchConfigurationService>("SuperFetch");
                services.AddKeyedSingleton<IRoutable, HideAppBrowserControlConfigurationService>("HideAppBrowserControl");
                services.AddKeyedSingleton<IRoutable, SecurityHealthTrayConfigurationService>("SecurityHealthTray");
                services.AddKeyedSingleton<IRoutable, FaultTolerantHeapConfigurationService>("FaultTolerantHeap");
                services.AddKeyedSingleton<IRoutable, CopilotConfigurationService>("Copilot");
                services.AddKeyedSingleton<IRoutable, RecallSupportConfigurationService>("Recall");
                services.AddKeyedSingleton<IRoutable, ProcessExplorerConfigurationService>("ProcessExplorer");
                services.AddKeyedSingleton<IRoutable, VbsConfigurationService>("VbsState");
                services.AddKeyedSingleton<IRoutable, GiveAccessToMenuConfigurationService>("GiveAccessToMenu");
                services.AddKeyedSingleton<IRoutable, NetworkNavigationPaneConfigurationService>("NetworkNavigationPane");
                services.AddKeyedSingleton<IRoutable, FileSharingConfigurationService>("FileSharing");
                services.AddKeyedSingleton<IRoutable, WindowsHelloConfigurationServices>("WindowsHello");
                services.AddKeyedSingleton<IRoutable, ToggleWindowsUpdateConfigurationService>("ToggleWindowsUpdates");
                services.AddKeyedSingleton<IMultiOptionConfigurationServices, ContextMenuTeminalsConfigurationService>("ContextMenuTerminals");
                services.AddKeyedSingleton<IMultiOptionConfigurationServices, ShortcutIconConfigurationService>("ShortcutIcon");
                services.AddKeyedSingleton<IMultiOptionConfigurationServices, MitigationsConfigurationService>("Mitigations");
                services.AddKeyedSingleton<IMultiOptionConfigurationServices, SafeModeConfigurationService>("SafeMode");
            });
            App.logger.Info($"[SERVICES] Added services to host");
            return host;
        }

        /// <summary>
        /// Registers Configuration sub menus
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationMenus(this IHostBuilder host)
        {
            host.ConfigureServices((_,services) =>
            {
                services.AddKeyedSingleton<IConfigurationSubMenu, ContextMenuSubMenu>("ContextMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, AiSubMenu>("AiSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, ServicesSubMenu>("ServicesSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, BootConfigurationSubMenu>("BootConfigurationSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, FileExplorerSubMenu>("FileExplorerSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, StartMenuSubMenu>("StartMenuSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, BootMenuAppearance>("BootConfigAppearance");
                services.AddKeyedSingleton<IConfigurationSubMenu, BootConfigBehavior>("BootConfigBehavior");
                services.AddKeyedSingleton<IConfigurationSubMenu, DriverConfigurationSubMenu>("DriverConfigurationSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, NvidiaDisplayContainerSubMenu>("NvidiaDisplayContainerSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, CoreIsolationSubMenu>("CoreIsolationSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, DefenderSubMenu>("DefenderSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, MitigationsSubMenu>("MitigationsSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, TroubleshootingNetworkSubMenu>("TroubleshootingNetwork");
                services.AddKeyedSingleton<IConfigurationSubMenu, FileSharingSubMenu>("FileSharingSubMenu");
                services.AddKeyedSingleton<IConfigurationSubMenu, WindowsUpdateSubMenu>("WindowsUpdate");
            });
            App.logger.Info($"[SERVICES] Added submenu services to host");
            return host;
        }
    }
}
