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
                services.AddKeyedTransient<IConfigurationService, AnimationsConfigurationService>("Animations");
                services.AddKeyedTransient<IConfigurationService, AppStoreArchivingConfigurationService>("AppStoreArchiving");
                services.AddKeyedTransient<IConfigurationService, BluetoothConfigurationService>("Bluetooth");
                services.AddKeyedTransient<IConfigurationService, FsoAndGameBarConfigurationService>("FsoAndGameBar");
                services.AddKeyedTransient<IConfigurationService, WindowsFirewallConfigurationService>("WindowsFirewall");
                services.AddKeyedTransient<IConfigurationService, GameModeConfigurationService>("GameMode");
                services.AddKeyedTransient<IConfigurationService, HagsConfigurationService>("Hags");
                services.AddKeyedTransient<IConfigurationService, LanmanWorkstationConfigurationService>("LanmanWorkstation");
                services.AddKeyedTransient<IConfigurationService, MicrosoftStoreConfigurationService>("MicrosoftStore");
                services.AddKeyedTransient<IConfigurationService, NetworkDiscoveryConfigurationService>("NetworkDiscovery");
                services.AddKeyedTransient<IConfigurationService, NotificationsConfigurationService>("Notifications");
                services.AddKeyedTransient<IConfigurationService, PrintingConfigurationService>("Printing");
                services.AddKeyedTransient<IConfigurationService, SearchIndexingConfigurationService>("SearchIndexing");
                services.AddKeyedTransient<IConfigurationService, TroubleshootingConfigurationService>("Troubleshooting");
                services.AddKeyedTransient<IConfigurationService, UwpConfigurationService>("Uwp");
                services.AddKeyedTransient<IConfigurationService, VpnConfigurationService>("Vpn");
                services.AddKeyedTransient<IConfigurationService, ModernAltTabConfigurationService>("ModernAltTab");
                services.AddKeyedTransient<IConfigurationService, CpuIdleContextMenuConfigurationService>("CpuIdleContextMenu");
                services.AddKeyedTransient<IConfigurationService, DarkTitlebarsConfigurationService>("DarkTitlebars");
                services.AddKeyedTransient<IConfigurationService, LockScreenConfigurationService>("LockScreen");
                services.AddKeyedTransient<IConfigurationService, ModernVolumeFlyoutConfigurationService>("ModernVolumeFlyout");
                services.AddKeyedTransient<IConfigurationService, RunWithPriorityConfigurationService>("RunWithPriority");
                services.AddKeyedTransient<IConfigurationService, ShortcutTextConfigurationService>("ShortcutText");
                services.AddKeyedTransient<IConfigurationService, BootLogoConfigurationService>("BootLogo");
                services.AddKeyedTransient<IConfigurationService, BootMessagesConfigurationService>("BootMessages");
                services.AddKeyedTransient<IConfigurationService, NewBootMenuConfigurationService>("NewBootMenu");
                services.AddKeyedTransient<IConfigurationService, SpinningAnimationConfigurationService>("SpinningAnimation");
                services.AddKeyedTransient<IConfigurationService, AdvancedBootOptionsConfigurationService>("AdvancedBootOptions");
                services.AddKeyedTransient<IConfigurationService, AutomaticRepairConfigurationService>("AutomaticRepair");
                services.AddKeyedTransient<IConfigurationService, KernelParametersConfigurationService>("KernelParameters");
                services.AddKeyedTransient<IConfigurationService, HighestModeConfigurationService>("HighestMode");
                services.AddKeyedTransient<IConfigurationService, CompactViewConfigurationService>("CompactView");
                services.AddKeyedTransient<IConfigurationService, QuickAccessConfigurationService>("QuickAccess");
                services.AddKeyedTransient<IConfigurationService, RemovableDrivesInSidebarConfigurationService>("RemovableDrivesInSidebar");
                services.AddKeyedTransient<IConfigurationService, AutomaticUpdatesConfigurationService>("AutomaticUpdates");
                services.AddKeyedTransient<IConfigurationService, BackgroundAppsConfigurationService>("BackgroundApps");
                services.AddKeyedTransient<IConfigurationService, DeliveryOptimisationConfigurationService>("DeliveryOptimisation");
                services.AddKeyedTransient<IConfigurationService, HibernationConfigurationService>("Hibernation");
                services.AddKeyedTransient<IConfigurationService, LocationConfigurationService>("Location");
                services.AddKeyedTransient<IConfigurationService, PhoneLinkConfigurationService>("PhoneLink");
                services.AddKeyedTransient<IConfigurationService, PowerSavingConfigurationService>("PowerSaving");
                services.AddKeyedTransient<IConfigurationService, SleepConfigurationService>("Sleep");
                services.AddKeyedTransient<IConfigurationService, AppStoreArchivingConfigurationService>("AppStoreArchiving");
                services.AddKeyedTransient<IConfigurationService, SystemRestoreConfigurationService>("SystemRestore");
                services.AddKeyedTransient<IConfigurationService, UpdateNotificationsConfigurationService>("UpdateNotifications");
                services.AddKeyedTransient<IConfigurationService, WebSearchConfigurationService>("WebSearch");
                services.AddKeyedTransient<IConfigurationService, WidgetsConfigurationService>("Widgets");
                services.AddKeyedTransient<IConfigurationService, WindowsSpotlightConfigurationService>("WindowsSpotlight");
                services.AddKeyedTransient<IConfigurationService, ExtractContextMenuConfigurationService>("ExtractContextMenu");
                services.AddKeyedTransient<IConfigurationService, TakeOwnershipConfigurationService>("TakeOwnership");
                services.AddKeyedTransient<IConfigurationService, CpuIdleConfigurationService>("CpuIdle");
                services.AddKeyedTransient<IConfigurationService, OldContextMenuConfigurationService>("OldContextMenu");
                services.AddKeyedTransient<IConfigurationService, EdgeSwipeConfigurationService>("EdgeSwipe");
                services.AddKeyedTransient<IConfigurationService, AppIconsThumbnailConfigurationService>("AppIconsThumbnail");
                services.AddKeyedTransient<IConfigurationService, AutomaticFolderDiscoveryConfigurationService>("AutomaticFolderDiscovery");
                services.AddKeyedTransient<IConfigurationService, GalleryConfigurationService>("Gallery");
                services.AddKeyedTransient<IConfigurationService, SnapLayoutsConfigurationService>("SnapLayout");
                services.AddKeyedTransient<IConfigurationService, RecentItemsConfigurationService>("RecentItems");
                services.AddKeyedTransient<IConfigurationService, VerboseStatusMessageConfiguarationServices>("VerboseStatusMessage");
                services.AddKeyedTransient<IConfigurationService, NvidiaDispayContainerConfigurationService>("NvidiaDispayContainer");
                services.AddKeyedTransient<IConfigurationService, AddNvidiaDisplayContainerContextMenuConfigurationService>("AddNvidiaDisplayContainerContextMenu");
                services.AddKeyedTransient<IConfigurationService, SuperFetchConfigurationService>("SuperFetch");
                services.AddKeyedTransient<IConfigurationService, HideAppBrowserControlConfigurationService>("HideAppBrowserControl");
                services.AddKeyedTransient<IConfigurationService, SecurityHealthTrayConfigurationService>("SecurityHealthTray");
                services.AddKeyedTransient<IConfigurationService, FaultTolerantHeapConfigurationService>("FaultTolerantHeap");
                services.AddKeyedTransient<IConfigurationService, CopilotConfigurationService>("Copilot");
                services.AddKeyedTransient<IConfigurationService, RecallSupportConfigurationService>("Recall");
                services.AddKeyedTransient<IConfigurationService, ProcessExplorerConfigurationService>("ProcessExplorer");
                services.AddKeyedTransient<IConfigurationService, VbsConfigurationService>("VbsState");
                services.AddKeyedTransient<IConfigurationService, GiveAccessToMenuConfigurationService>("GiveAccessToMenu");
                services.AddKeyedTransient<IConfigurationService, NetworkNavigationPaneConfigurationService>("NetworkNavigationPane");
                services.AddKeyedTransient<IConfigurationService, FileSharingConfigurationService>("FileSharing");
                services.AddKeyedTransient<IConfigurationService, WindowsHelloConfigurationServices>("WindowsHello");
                services.AddKeyedTransient<IConfigurationService, ToggleWindowsUpdateConfigurationService>("ToggleWindowsUpdates");
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
