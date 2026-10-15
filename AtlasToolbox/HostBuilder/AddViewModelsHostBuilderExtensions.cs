using AtlasToolbox.Models;
using AtlasToolbox.Stores;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Graphics.Canvas.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Cryptography.Core;
using Windows.Devices.WiFi;
using AtlasToolbox.Utils;
using AtlasToolbox.Models.ProfileModels;
using Newtonsoft.Json;
using AtlasToolbox.ViewModels.ConfigurationVM;
using AtlasToolbox.Services;
using AtlasToolbox.Atlas;
using CommunityToolkit.Mvvm.Input;

namespace AtlasToolbox.HostBuilder
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton(CreateConfigPageViewModel);
                services.AddSingleton(CreateHomePageViewModel);
                services.AddSingleton(CreateSoftwarePageViewModel);
            });

            host.AddRoutes();
            host.AddConfigurationButtonItemViewModels();
            host.AddLinksItemViewModels();
            host.AddSoftwareItemsViewModels();
            host.AddMultiOptionConfigurationViewModels();
            host.AddConfigurationItemViewModels();
            host.AddProfiles();

            App.logger.Info($"[VMHostBuilder] Successfully loaded host");
            return host;
        }

        /// <summary>
        /// Registers software items
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddSoftwareItemsViewModels(this IHostBuilder host)
        {
            List<SoftwareItem> configurationDictionary = new()
            {
                new("Ungoogled Chromium", "eloston.ungoogled-chromium"),
                new("Google Chrome", "Google.Chrome"),
                new("Mozilla Firefox", "Mozilla.Firefox"),
                new("Waterfox", "Waterfox.Waterfox"),
                new("Brave Browser", "Brave.Brave"),
                new("LibreWolf", "LibreWolf.LibreWolf"),
                new("Tor Browser", "TorProject.TorBrowser"),
                new("Discord", "Discord.Discord"),
                new("Discord Canary", "Discord.Discord.Canary"),
                new("Steam", "Valve.Steam"),
                new("Playnite", "Playnite.Playnite"),
                new("Heroic", "HeroicGamesLauncher.HeroicGamesLauncher"),
                new("Everything", "voidtools.Everything"),
                new("Mozilla Thunderbird", "Mozilla.Thunderbird"),
                new("IrfanView", "IrfanSkiljan.IrfanView"),
                new("Git", "Git.Git"),
                new("VLC", "VideoLAN.VLC"),
                new("PuTTY", "PuTTY.PuTTY"),
                new("Ditto", "Ditto.Ditto"),
                new("7-Zip", "7zip.7zip"),
                new("Teamspeak", "TeamSpeakSystems.TeamSpeakClient"),
                new("Spotify", "Spotify.Spotify"),
                new("OBS Studio", "OBSProject.OBSStudio"),
                new("MSI Afterburner", "Guru3D.Afterburner"),
                new("NVCleanstall", "TechPowerUp.NVCleanstall"),
                new("foobar2000", "PeterPawlowski.foobar2000"),
                new("CPU-Z", "CPUID.CPU-Z"),
                new("GPU-Z", "TechPowerUp.GPU-Z"),
                new("Notepad++", "Notepad++.Notepad++"),
                new("VSCode", "Microsoft.VisualStudioCode"),
                new("VSCodium", "VSCodium.VSCodium"),
                new("BCUninstaller", "Klocman.BulkCrapUninstaller"),
                new("HWiNFO", "REALiX.HWiNFO"),
                new("Lightshot", "Skillbrains.Lightshot"),
                new("ShareX", "ShareX.ShareX"),
                new("Snipping Tool", "9MZ95KL8MR0L"),
                new("ExplorerPatcher", "valinet.ExplorerPatcher"),
                new("Powershell 7", "Microsoft.PowerShell"),
                new("UniGetUI", "MartiCliment.UniGetUI"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<SoftwareItemViewModel>>(provider =>
                {
                    List<SoftwareItemViewModel> viewModels = new();

                    foreach (SoftwareItem item in configurationDictionary)
                    {
                        viewModels.Add(CreateSoftwareItemViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} software entries");
                    return viewModels;
                });
            });
            return host;
        }

        /// <summary>
        /// Registers profiles from the profile folder
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddProfiles(this IHostBuilder host)
        {
            List<Profiles> configurationDictionary = new List<Profiles>();
            ProfileSerializing.MoveLegacyProfiles();
            FileInfo[] files = [];
            try
            {
                files = new DirectoryInfo(ProtectedFolder.Ensure(AtlasPaths.DataRoot, AtlasPaths.Profiles)).GetFiles("*.json");
            }
            catch (Exception ex)
            {
                App.logger.Error(ex, "[PROFILES] The profiles folder can't be used");
            }
            foreach (FileInfo file in files)
            {
                try
                {
                    configurationDictionary.Add(ProfileSerializing.DeserializeProfile(file.FullName));
                }
                catch (Exception ex)
                {
                    App.logger.Error(ex, $"[PROFILES] Couldn't read profile \"{file.Name}\"");
                }
            }
            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<Profiles>>(provider =>
                {
                    return configurationDictionary;
                });
            });

            return host;
        }

        /// <summary>
        /// Registers links
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddLinksItemViewModels(this IHostBuilder host)
        {
            List<LinkRegister> configurationDictionary = new()
            {
                // Interface Tweaks links
                new("ExplorerPatcher", @"https://github.com/valinet/ExplorerPatcher", "ExplorerPatcher", "Interface/StartMenuSubMenu"),
                new("StartAllBack", @"https://www.startallback.com/", "StartAllBack", "Interface/StartMenuSubMenu"),
                new("OpenShellAtlasPreset", @"https://github.com/Atlas-OS/Atlas/blob/main/playbook/Executables/AtlasDesktop/4.%20Interface%20Tweaks/Start%20Menu/Atlas%20Open-Shell%20Preset.xml", "Interface/StartMenuSubMenu"),
                new("InterfaceTweaksDocumentation", @"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/interface-tweaks/", "Interface"),

                // Windows tweaks links
                new("ActivationPage", @"ms-settings:activation", "Windows", "\uE713"),
                new("ColorsPage", @"ms-settings:personalization-colors", "Windows", "\uE713"),
                new("DateAndTime", @"ms-settings:dateandtime", "Windows", "\uE713"),
                new("DefaultApps", @"ms-settings:defaultapps", "Windows", "\uE713"),
                new("DefaultGraphicsSettings", @"ms-settings:display-advancedgraphics-default", "Windows", "\uE713"),
                new("RegionLanguage", @"ms-settings:regionlanguage", "Windows", "\uE713"),
                new("Privacy", @"ms-settings:privacy", "Windows", "\uE713"),
                new("RegionProperties", @"ms-settings:regionProperties", "Windows", "\uE713"),
                new("Taskbar", @"ms-settings:taskbar", "Windows", "\uE713"),
                new("WindowsSettingsDocumentation", @"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/windows-settings/", "Windows"),

                // Advanced tweaks links
                new("AdvancedConfigMustRead", @"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/", "Advanced"),
                new("BootConfigExplanations", @"https://learn.microsoft.com/windows-hardware/drivers/devtest/bcdedit--set", "Advanced/BootConfigurationSubMenu"),
                new("AutoGpuAffinity", @"https://github.com/valleyofdoom/AutoGpuAffinity", "AutoGpuAffinity", "Advanced/DriverConfigurationSubMenu"),
                new("NvidiaDisplayContainerMustReadFirst", @"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/#nvidia-display-container", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu"),
                new("GoInterruptPolicy", @"https://github.com/spddl/GoInterruptPolicy", "GoInterruptPolicy", "Advanced/DriverConfigurationSubMenu"),
                new("InterrupAffinityTool", @"https://www.techpowerup.com/download/microsoft-interrupt-affinity-tool", "Advanced/DriverConfigurationSubMenu"),
                new("MSIUtilityV3", @"https://forums.guru3d.com/threads/windows-line-based-vs-message-signaled-based-interrupts-msi-tool.378044", "Advanced/DriverConfigurationSubMenu"),
                new("ProcessExplorerApp", @"https://learn.microsoft.com/en-us/sysinternals/downloads/process-explorer", "Advanced"),

                // Security links
                new("SecurityDocumentation", @"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/security/", "Security"),
                new("CoreIsolation", @"windowsdefender://coreisolation/", "Security/CoreIsolationSubMenu", "\uE83D"),

                // Troubleshooting links
                new("ResetPC", @"https://docs.atlasos.net/getting-started/reverting-atlas/", "Troubleshooting"),
                new("TroubleshootingDocumentation", @"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/troubleshooting/", "Troubleshooting"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<LinksViewModel>>(provider =>
                {
                    List<LinksViewModel> viewModels = new();

                    foreach (LinkRegister item in configurationDictionary)
                    {
                        viewModels.Add(CreateLinksViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} link entries");
                    return viewModels;
                });
            });
            return host;
        }

        /// <summary>
        /// Registers configuration buttons. Each runs an Atlas action, as its launcher in
        /// the Atlas folder does.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationButtonItemViewModels(this IHostBuilder host)
        {
            static ICommand RunAction(string toggle, string state, bool? interactive = null) =>
                new AsyncRelayCommand(() => ToggleActions.RunActionAsync(toggle, state, interactive));

            List<ButtonServiceRegister> configurationDictionary = new()
            {
                new("RestartExplorerButton", RunAction("RestartExplorer", "Run", interactive: false), "Interface"),
                new("ViewCurrentSettingsBootConfig", RunAction("ViewCurrentValues", "View"), "Advanced/BootConfigurationSubMenu"),
                new("VBSCurrentConfig", RunAction("ConfigVBS", "Run"), "Security/CoreIsolationSubMenu"),
                new("ToggleDefender", RunAction("ToggleDefender", "Run"), "Security/DefenderSubMenu"),
                new("ResetFTH", new AsyncRelayCommand(ToggleActions.ResetFaultTolerantHeapAsync), "Security/MitigationsSubMenu"),
                new("InstallOpenShell", new AsyncRelayCommand(() => ToggleActions.RunLauncherAsync(@"4. Interface Tweaks\Start Menu\Install Open-Shell.cmd")), "Interface/StartMenuSubMenu"),

                new("FixErrors", RunAction("FixErrors2502and2503", "Run"), "Troubleshooting"),
                new("RepairWinComponent", RunAction("RepairWindowsComponents", "Run"), "Troubleshooting", "\uE90F"),
                new("TelemetryComponents", RunAction("TelemetryComponents", "Run"), "Troubleshooting", "\uE90F"),
                new("AtlasDefault", RunAction("DefaultAtlasNetwork", "Enable"), "Troubleshooting/TroubleshootingNetwork", "\uE839"),
                new("WindowsDefault", RunAction("DefaultAtlasNetwork", "Disable"), "Troubleshooting/TroubleshootingNetwork", "\uE839"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationButtonViewModel>>(provider =>
                {
                    List<ConfigurationButtonViewModel> viewModels = new();

                    foreach (ButtonServiceRegister item in configurationDictionary)
                    {
                        viewModels.Add(CreateButtonViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} button entries");
                    return viewModels;
                });
            });
            return host;
        }

        private static IHostBuilder AddRoutes(this IHostBuilder host)
        {
            List<RouteService> routes = new()
            {
                // 'General' RouteServices
                new RouteService("General", "\uE80F", true),
                new RouteService("General/AiSubMenu", "\uF4A5"),
                new RouteService("General/FileSharingSubMenu", "\uF193"),
                new RouteService("General/WindowsUpdate", "\uEDAB"),
                new RouteService("General/CpuIdleContextMenu", "\uEDAB"),

                // 'Interface' RouteServices
                new RouteService("Interface", "\uE713", true),
                new RouteService("Interface/StartMenuSubMenu", "\uE8FC"),
                new RouteService("Interface/ContextMenuSubMenu", null),
                new RouteService("Interface/FileExplorerSubMenu", "\uEC50"),

                // 'Windows' RouteServices
                new RouteService("Windows", "\uE71D", true),

                // 'Advanced' RouteServices
                new RouteService("Advanced", "\uE71D", true),
                new RouteService("Advanced/ServicesSubMenu", "\uE9F5"),
                new RouteService("Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu", null),
                new RouteService("Advanced/BootConfigurationSubMenu", "\uF259"),
                new RouteService("Advanced/BootConfigurationSubMenu/BootConfigAppearance", "\uE620"),
                new RouteService("Advanced/BootConfigurationSubMenu/BootConfigBehavior", "\uF259"),
                new RouteService("Advanced/DriverConfigurationSubMenu", "\uE772"),

                // 'Security' RouteServices
                new RouteService("Security", "\uE71D", true),
                new RouteService("Security/CoreIsolationSubMenu", "\uEEA1"),
                new RouteService("Security/DefenderSubMenu", "\uE83D"),
                new RouteService("Security/MitigationsSubMenu", "\uE730"),

                // 'Troubleshooting' RouteServices
                new RouteService("Troubleshooting", "\uE71D", true),
                new RouteService("Troubleshooting/TroubleshootingNetwork", "\uE90F"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<RouteViewModel>>(provider =>
                {
                    List<RouteViewModel> viewModels = new();
                    foreach (RouteService item in routes.Where(r => !r.IsRootRoute))
                    {
                        viewModels.Add(new RouteViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} route entries");
                    return viewModels;
                });
            });

            return host;
        }


        /// <summary>
        /// Registers multioption configuration services
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddMultiOptionConfigurationViewModels(this IHostBuilder host)
        {
            List<MultiServiceRegister> configurations = new()
            {
                // General
                new("Indexing", "General"),

                // Interface
                new("ContextMenuTerminals", "Interface/ContextMenuSubMenu", "\uE756"),
                new("ShortcutIcon", "Interface", "\uE8A7"),

                // Security
                new("Mitigations", "Security/MitigationsSubMenu", "\uF0EF"),

                // Troubleshooting
                new("SafeMode", "Troubleshooting", "\uEA18"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<MultiConfigViewModel>>(provider =>
                {
                    List<MultiConfigViewModel> viewModels = new();

                    foreach (MultiServiceRegister item in configurations)
                    {
                        if (!item.IsAvailable)
                        {
                            App.logger.Warn($"[VMHostBuilder] This Atlas has no toggle {item.Key}; left out");
                            continue;
                        }
                        viewModels.Add(CreateMultiOptionConfigurationItemViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} multi-configuration entries");
                    return viewModels;
                });
            });
            return host;
        }

        /// <summary>
        /// Registers configuration items: Atlas toggles with an Enable and a Disable state.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationItemViewModels(this IHostBuilder host)
        {
            List<ToggleServiceRegister> configurationDictionary = new()
            {
                // General
                new("BackgroundApps", "General"),
                new("FSOGameBar", "General"),
                new("AutomaticUpdates", "General"),
                new("DeliveryOptimisation", "General"),
                new("Hibernation", "General"),
                new("Location", "General"),
                new("PhoneLink", "General"),
                new("PowerSaving", "General"),
                new("Sleep", "General"),
                new("SystemRestore", "General"),
                new("UpdateNotifications", "General"),
                new("WebSearch", "General"),
                new("Widgets", "General"),
                new("WindowsSpotlight", "General"),
                new("AppStoreArchiving", "General"),
                new("Workplace", "General", unrecorded: false),

                // General - Windows Update SubMenu
                new("ToggleWindowsUpdates", "General/WindowsUpdate", unrecorded: true),
                new("PauseUpdates", "General/WindowsUpdate", unrecorded: false),

                // General - AI SubMenu
                new("Copilot", "General/AiSubMenu"),
                new("Recall", "General/AiSubMenu"),
                new("ClickToDo", "General/AiSubMenu"),

                // General - File Sharing SubMenu
                new("GiveAccessToMenu", "General/FileSharingSubMenu"),
                new("NetworkNavigationPane", "General/FileSharingSubMenu"),
                new("FileSharing", "General/FileSharingSubMenu"),

                // General - CPU Idle
                new("CpuIdle", "General/CpuIdleContextMenu"),
                new("CpuIdleContextMenu", "General/CpuIdleContextMenu"),

                // Interface
                new("Animation", "Interface"),
                new("LockScreen", "Interface"),
                new("ShortcutText", "Interface"),
                new("EdgeSwipe", "Interface"),
                new("SnapLayouts", "Interface"),
                new("RecentItems", "Interface"),
                new("VerboseMessages", "Interface"),
                new("KeyboardShortcuts", "Interface", unrecorded: true),

                // Interface - Context Menu SubMenu
                new("ExtractContextMenu", "Interface/ContextMenuSubMenu"),
                new("RunWithPriority", "Interface/ContextMenuSubMenu"),
                new("TakeOwnership", "Interface/ContextMenuSubMenu"),
                new("OldContextMenu", "Interface/ContextMenuSubMenu"),

                // Interface - File Explorer SubMenu
                new("CompactView", "Interface/FileExplorerSubMenu"),
                new("RemovableDrivesInSidebar", "Interface/FileExplorerSubMenu"),
                new("AppIconThumbnail", "Interface/FileExplorerSubMenu"),
                new("AutomaticFolderDiscovery", "Interface/FileExplorerSubMenu"),
                new("Gallery", "Interface/FileExplorerSubMenu"),
                new("Home", "Interface/FileExplorerSubMenu", textKey: "ExplorerHome"),
                new("QuickAccess", "Interface/FileExplorerSubMenu"),

                // Advanced
                new("ProcessExplorer", "Advanced", unrecorded: false),
                new("MicrosoftStore", "Advanced"),

                // Advanced - Services SubMenu
                new("Bluetooth", "Advanced/ServicesSubMenu"),
                new("LanmanWorkstation", "Advanced/ServicesSubMenu"),
                new("NetworkDiscovery", "Advanced/ServicesSubMenu"),
                new("Printing", "Advanced/ServicesSubMenu"),
                new("SuperFetch", "Advanced/ServicesSubMenu"),

                // Advanced - Services SubMenu - Nvidia Display Container SubMenu
                new("NVidiaDisplayContainer", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu"),
                new("NVidiaDisplayContainerContextMenu", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu"),

                // Advanced - Boot Configuration SubMenu - Appearance
                new("BootLogo", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),
                new("NewBootMenu", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),
                new("SpinningAnimations", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),

                // Advanced - Boot Configuration SubMenu - Behavior
                new("AdvancedBootOptions", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),
                new("AutomaticRepair", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),
                new("KernelParameters", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),
                new("HighestMode", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),

                // Security - Core Isolation SubMenu
                new("VbsState", "Security/CoreIsolationSubMenu", unrecorded: true),

                // Security - Defender SubMenu
                new("HideAppBrowserControl", "Security/DefenderSubMenu"),
                new("SecurityHealthTray", "Security/DefenderSubMenu"),

                // Security - Mitigations SubMenu
                new("FaultTolerantHeap", "Security/MitigationsSubMenu"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationItemViewModel>>(provider =>
                {
                    List<ConfigurationItemViewModel> viewModels = new();

                    foreach (ToggleServiceRegister item in configurationDictionary)
                    {
                        if (!item.IsAvailable)
                        {
                            App.logger.Warn($"[VMHostBuilder] This Atlas has no on/off toggle {item.Key}; left out");
                            continue;
                        }
                        viewModels.Add(CreateConfigurationItemViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} configuration entries");
                    return viewModels;
                });
            });
            return host;
        }



        private static MultiConfigViewModel CreateMultiOptionConfigurationItemViewModel(MultiServiceRegister configuration)
        {
            MultiConfigViewModel viewModel = new(configuration);

            return viewModel;
        }

        private static ConfigurationItemViewModel CreateConfigurationItemViewModel(ToggleServiceRegister configuration)
        {
            ConfigurationItemViewModel viewModel = new(configuration);

            return viewModel;
        }

        #region Create ViewModels
        // Entire region is made to create view models
        private static SoftwareItemViewModel CreateSoftwareItemViewModel(SoftwareItem softwareItem)
        {
            SoftwareItemViewModel viewModel = new(softwareItem);

            return viewModel;
        }

        private static ConfigurationButtonViewModel CreateButtonViewModel(ButtonServiceRegister configurationButtonViewModel)
        {
            ConfigurationButtonViewModel viewModel = new(configurationButtonViewModel);

            return viewModel;
        }

        private static LinksViewModel CreateLinksViewModel(LinkRegister linksItem)
        {
            LinksViewModel viewModel = new(linksItem);

            return viewModel;
        }

        private static ConfigPageViewModel CreateConfigPageViewModel(IServiceProvider serviceProvider)
        {
            return ConfigPageViewModel.LoadViewModel(
                serviceProvider.GetServices<LinksViewModel>(),
                serviceProvider.GetServices<ConfigurationItemViewModel>(),
                serviceProvider.GetServices<MultiConfigViewModel>(),
                serviceProvider.GetServices<ConfigurationButtonViewModel>(),
                serviceProvider.GetServices<RouteViewModel>());
        }

        private static HomePageViewModel CreateHomePageViewModel(IServiceProvider serviceProvider)
        {
            return HomePageViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<IEnumerable<Profiles>>(),
                serviceProvider.GetRequiredService<IEnumerable<ConfigurationItemViewModel>>(),
                serviceProvider.GetRequiredService<IEnumerable<MultiConfigViewModel>>());
        }
        private static SoftwarePageViewModel CreateSoftwarePageViewModel(IServiceProvider serviceProvider)
        {
            return SoftwarePageViewModel.LoadViewModel(
                serviceProvider.GetServices<SoftwareItemViewModel>());
        }
        #endregion Create ViewModels
    }
}
