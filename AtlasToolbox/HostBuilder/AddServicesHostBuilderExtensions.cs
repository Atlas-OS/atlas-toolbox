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
                services.AddTransient(CreateBcdStore);
                services.AddTransient<IDismService, DismService>();
                services.AddTransient<IBcdService, BcdService>();
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
                services.AddKeyedTransient<IRoutable, AnimationsConfigurationService>("Animations");
                services.AddKeyedTransient<IRoutable, AppStoreArchivingConfigurationService>("AppStoreArchiving");
                services.AddKeyedTransient<IRoutable, BluetoothConfigurationService>("Bluetooth");
                services.AddKeyedTransient<IRoutable, FsoAndGameBarConfigurationService>("FsoAndGameBar");
                services.AddKeyedTransient<IRoutable, WindowsFirewallConfigurationService>("WindowsFirewall");
                services.AddKeyedTransient<IRoutable, GameModeConfigurationService>("GameMode");
                services.AddKeyedTransient<IRoutable, HagsConfigurationService>("Hags");
                services.AddKeyedTransient<IRoutable, LanmanWorkstationConfigurationService>("LanmanWorkstation");
                services.AddKeyedTransient<IRoutable, MicrosoftStoreConfigurationService>("MicrosoftStore");
                services.AddKeyedTransient<IRoutable, NetworkDiscoveryConfigurationService>("NetworkDiscovery");
                services.AddKeyedTransient<IRoutable, NotificationsConfigurationService>("Notifications");
                services.AddKeyedTransient<IRoutable, PrintingConfigurationService>("Printing");
                services.AddKeyedTransient<IRoutable, SearchIndexingConfigurationService>("SearchIndexing");
                services.AddKeyedTransient<IRoutable, TroubleshootingConfigurationService>("Troubleshooting");
                services.AddKeyedTransient<IRoutable, UwpConfigurationService>("Uwp");
                services.AddKeyedTransient<IRoutable, VpnConfigurationService>("Vpn");
                services.AddKeyedTransient<IRoutable, ModernAltTabConfigurationService>("ModernAltTab");
                services.AddKeyedTransient<IRoutable, CpuIdleContextMenuConfigurationService>("CpuIdleContextMenu");
                services.AddKeyedTransient<IRoutable, DarkTitlebarsConfigurationService>("DarkTitlebars");
                services.AddKeyedTransient<IRoutable, LockScreenConfigurationService>("LockScreen");
                services.AddKeyedTransient<IRoutable, ModernVolumeFlyoutConfigurationService>("ModernVolumeFlyout");
                services.AddKeyedTransient<IRoutable, RunWithPriorityConfigurationService>("RunWithPriority");
                services.AddKeyedTransient<IRoutable, ShortcutTextConfigurationService>("ShortcutText");
                services.AddKeyedTransient<IRoutable, BootLogoConfigurationService>("BootLogo");
                services.AddKeyedTransient<IRoutable, BootMessagesConfigurationService>("BootMessages");
                services.AddKeyedTransient<IRoutable, NewBootMenuConfigurationService>("NewBootMenu");
                services.AddKeyedTransient<IRoutable, SpinningAnimationConfigurationService>("SpinningAnimation");
                services.AddKeyedTransient<IRoutable, AdvancedBootOptionsConfigurationService>("AdvancedBootOptions");
                services.AddKeyedTransient<IRoutable, AutomaticRepairConfigurationService>("AutomaticRepair");
                services.AddKeyedTransient<IRoutable, KernelParametersConfigurationService>("KernelParameters");
                services.AddKeyedTransient<IRoutable, HighestModeConfigurationService>("HighestMode");
                services.AddKeyedTransient<IRoutable, CompactViewConfigurationService>("CompactView");
                services.AddKeyedTransient<IRoutable, QuickAccessConfigurationService>("QuickAccess");
                services.AddKeyedTransient<IRoutable, RemovableDrivesInSidebarConfigurationService>("RemovableDrivesInSidebar");
                services.AddKeyedTransient<IRoutable, AutomaticUpdatesConfigurationService>("AutomaticUpdates");
                services.AddKeyedTransient<IRoutable, BackgroundAppsConfigurationService>("BackgroundApps");
                services.AddKeyedTransient<IRoutable, DeliveryOptimisationConfigurationService>("DeliveryOptimisation");
                services.AddKeyedTransient<IRoutable, HibernationConfigurationService>("Hibernation");
                services.AddKeyedTransient<IRoutable, LocationConfigurationService>("Location");
                services.AddKeyedTransient<IRoutable, PhoneLinkConfigurationService>("PhoneLink");
                services.AddKeyedTransient<IRoutable, PowerSavingConfigurationService>("PowerSaving");
                services.AddKeyedTransient<IRoutable, SleepConfigurationService>("Sleep");
                services.AddKeyedTransient<IRoutable, AppStoreArchivingConfigurationService>("AppStoreArchiving");
                services.AddKeyedTransient<IRoutable, SystemRestoreConfigurationService>("SystemRestore");
                services.AddKeyedTransient<IRoutable, UpdateNotificationsConfigurationService>("UpdateNotifications");
                services.AddKeyedTransient<IRoutable, WebSearchConfigurationService>("WebSearch");
                services.AddKeyedTransient<IRoutable, WidgetsConfigurationService>("Widgets");
                services.AddKeyedTransient<IRoutable, WindowsSpotlightConfigurationService>("WindowsSpotlight");
                services.AddKeyedTransient<IRoutable, ExtractContextMenuConfigurationService>("ExtractContextMenu");
                services.AddKeyedTransient<IRoutable, TakeOwnershipConfigurationService>("TakeOwnership");
                services.AddKeyedTransient<IRoutable, CpuIdleConfigurationService>("CpuIdle");
                services.AddKeyedTransient<IRoutable, OldContextMenuConfigurationService>("OldContextMenu");
                services.AddKeyedTransient<IRoutable, EdgeSwipeConfigurationService>("EdgeSwipe");
                services.AddKeyedTransient<IRoutable, AppIconsThumbnailConfigurationService>("AppIconsThumbnail");
                services.AddKeyedTransient<IRoutable, AutomaticFolderDiscoveryConfigurationService>("AutomaticFolderDiscovery");
                services.AddKeyedTransient<IRoutable, GalleryConfigurationService>("Gallery");
                services.AddKeyedTransient<IRoutable, SnapLayoutsConfigurationService>("SnapLayout");
                services.AddKeyedTransient<IRoutable, RecentItemsConfigurationService>("RecentItems");
                services.AddKeyedTransient<IRoutable, VerboseStatusMessageConfiguarationServices>("VerboseStatusMessage");
                services.AddKeyedTransient<IRoutable, NvidiaDispayContainerConfigurationService>("NvidiaDispayContainer");
                services.AddKeyedTransient<IRoutable, AddNvidiaDisplayContainerContextMenuConfigurationService>("AddNvidiaDisplayContainerContextMenu");
                services.AddKeyedTransient<IRoutable, SuperFetchConfigurationService>("SuperFetch");
                services.AddKeyedTransient<IRoutable, HideAppBrowserControlConfigurationService>("HideAppBrowserControl");
                services.AddKeyedTransient<IRoutable, SecurityHealthTrayConfigurationService>("SecurityHealthTray");
                services.AddKeyedTransient<IRoutable, FaultTolerantHeapConfigurationService>("FaultTolerantHeap");
                services.AddKeyedTransient<IRoutable, CopilotConfigurationService>("Copilot");
                services.AddKeyedTransient<IRoutable, RecallSupportConfigurationService>("Recall");
                services.AddKeyedTransient<IRoutable, ProcessExplorerConfigurationService>("ProcessExplorer");
                services.AddKeyedTransient<IRoutable, VbsConfigurationService>("VbsState");
                services.AddKeyedTransient<IRoutable, GiveAccessToMenuConfigurationService>("GiveAccessToMenu");
                services.AddKeyedTransient<IRoutable, NetworkNavigationPaneConfigurationService>("NetworkNavigationPane");
                services.AddKeyedTransient<IRoutable, FileSharingConfigurationService>("FileSharing");
                services.AddKeyedTransient<IRoutable, WindowsHelloConfigurationServices>("WindowsHello");
                services.AddKeyedTransient<IRoutable, ToggleWindowsUpdateConfigurationService>("ToggleWindowsUpdates");
                services.AddKeyedTransient<IMultiOptionConfigurationServices, ContextMenuTeminalsConfigurationService>("ContextMenuTerminals");
                services.AddKeyedTransient<IMultiOptionConfigurationServices, ShortcutIconConfigurationService>("ShortcutIcon");
                services.AddKeyedTransient<IMultiOptionConfigurationServices, MitigationsConfigurationService>("Mitigations");
                services.AddKeyedTransient<IMultiOptionConfigurationServices, SafeModeConfigurationService>("SafeMode");
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
                services.AddKeyedTransient<IConfigurationSubMenu, ContextMenuSubMenu>("ContextMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, AiSubMenu>("AiSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, ServicesSubMenu>("ServicesSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, BootConfigurationSubMenu>("BootConfigurationSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, FileExplorerSubMenu>("FileExplorerSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, StartMenuSubMenu>("StartMenuSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, BootMenuAppearance>("BootConfigAppearance");
                services.AddKeyedTransient<IConfigurationSubMenu, BootConfigBehavior>("BootConfigBehavior");
                services.AddKeyedTransient<IConfigurationSubMenu, DriverConfigurationSubMenu>("DriverConfigurationSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, NvidiaDisplayContainerSubMenu>("NvidiaDisplayContainerSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, CoreIsolationSubMenu>("CoreIsolationSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, DefenderSubMenu>("DefenderSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, MitigationsSubMenu>("MitigationsSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, TroubleshootingNetworkSubMenu>("TroubleshootingNetwork");
                services.AddKeyedTransient<IConfigurationSubMenu, FileSharingSubMenu>("FileSharingSubMenu");
                services.AddKeyedTransient<IConfigurationSubMenu, WindowsUpdateSubMenu>("WindowsUpdate");
            });
            App.logger.Info($"[SERVICES] Added submenu services to host");
            return host;
        }
    }
}
