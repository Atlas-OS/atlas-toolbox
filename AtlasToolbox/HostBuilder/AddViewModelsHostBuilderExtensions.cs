using AtlasToolbox.Models;
using AtlasToolbox.Stores;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Graphics.Canvas.Text;
using System.Linq;
using System.Threading.Tasks;
using AtlasToolbox.Commands;
using System.Windows.Input;
using Windows.Security.Cryptography.Core;
using Windows.Devices.WiFi;
using AtlasToolbox.Commands.ConfigurationButtonsCommand;
using AtlasToolbox.Utils;
using AtlasToolbox.Models.ProfileModels;
using Newtonsoft.Json;
using AtlasToolbox.ViewModels.ConfigurationVM;
using AtlasToolbox.Services;

namespace AtlasToolbox.HostBuilder
{
    public static class AddViewModelsHostBuilderExtensions
    {
        private static List<Object> subMenuOnlyItems = new List<Object>();
        private static Dictionary<string, string> list = new Dictionary<string, string>();
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton(CreateConfigPageViewModel);
                services.AddSingleton(CreateHomePageViewModel);
                services.AddSingleton(CreateSoftwarePageViewModel);
            });

            host.AddConfigurationButtonItemViewModels();
            host.AddLinksItemViewModels();
            host.AddSoftwareItemsViewModels();
            host.AddMultiOptionConfigurationViewModels();
            host.AddConfigurationItemViewModels();
            host.AddRoutes();
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
        /// Regsiters profiles from the profile folder
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddProfiles(this IHostBuilder host)
        {
            List<Profiles> configurationDictionary = new List<Profiles>();
            DirectoryInfo profilesDirectory = new DirectoryInfo($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles");
            try
            {
                FileInfo[] profileFile = profilesDirectory.GetFiles();
            }
            catch
            {
                Directory.CreateDirectory($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles");
            }
            finally
            {
                FileInfo[] profileFile = profilesDirectory.GetFiles();
                foreach (FileInfo file in profileFile)
                {
                    configurationDictionary.Add(ProfileSerializing.DeserializeProfile(file.FullName));
                }
                ;
                host.ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IEnumerable<Profiles>>(provider =>
                    {
                        return configurationDictionary;
                    });
                });
            }

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
                new("OpenShellAtlasPreset", @"http://github.com/Atlas-OS/Atlas/blob/main/src/playbook/Executables/AtlasDesktop/4.%20Interface%20Tweaks/Start%20Menu/Atlas%20Open-Shell%20Preset.xml", "Interface/StartMenuSubMenu"),
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
        /// Registers configuration buttons
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationButtonItemViewModels(this IHostBuilder host)
        {
            ICommand buttonCommand;
            List<ButtonServiceRegister> configurationDictionary = new()
            {
                new("RestartExplorerButton", buttonCommand = new RestartExplorerCommand(), "Interface"),
                new("ViewCurrentSettingsBootConfig", buttonCommand = new ViewCurrentValuesCommand(), "Advanced/BootConfigurationSubMenu"),
                new("VBSCurrentConfig", buttonCommand = new CurrentVBSConfigurationCommand(), "Security/CoreIsolationSubMenu"),
                new("ToggleDefender", buttonCommand = new ToggleDefenderCommand(), "Security/DefenderSubMenu"),
                new("ResetFTH", buttonCommand = new ResetFTHCommand(), "Security/MitigationsSubMenu"),
                new("InstallOpenShell", buttonCommand = new InstallOpenShellCommand(), "Interface/StartMenuSubMenu"),

                new("FixErrors", buttonCommand = new FixErrorsCommand(), "Troubleshooting"),
                new("RepairWinComponent", buttonCommand = new RepairWindowsComponentsCommand(), "Troubleshooting", "\uE90F"),
                new("TelemetryComponents", buttonCommand = new TelemetryComponentsCommand(), "Troubleshooting", "\uE90F"),
                new("AtlasDefault", buttonCommand = new NetworkAtlasDefaults(), "Troubleshooting/TroubleshootingNetwork", "\uE839"),
                new("WindowsDefault", buttonCommand = new NetworkWindowsDefaults(), "Troubleshooting/TroubleshootingNetwork", "\uE839"),
                new("SetUpdateDeferral", buttonCommand = new SetUpdateDeferralConfigurationButton(), "General/WindowsUpdate", "\uE916"),
                new("ResetUpdateDeferral", buttonCommand = new ResetWindowsUpdateDeferral(), "General/WindowsUpdate", "\uE81C"),
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
                services.AddSingleton<IEnumerable<RouteService>>(provider =>
                {
                    List<RouteViewModel> viewModels = new();
                    foreach (RouteService item in routes)
                    {
                        viewModels.Add(new RouteViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {routes.Count} route entries");
                    return routes.Where(r => !r.IsRootRoute);
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
                new("Indexing", "General", ["Enable", "Disable", "Minimal"], "Minimal"),

                // Interface
                new("ContextMenuTerminals", "Interface/ContextMenuSubMenu", ["AddNoWindowsTerminal", "Add", "Remove"], "Remove", "\uE756"),
                new("ShortcutIcon", "Interface", ["Classic", "Default", "None"], "Default","\uE8A7"),
                
                // Security
                new("Mitigations", "Security/MitigationsSubMenu",["Disable", "Enable", "WindowsDefault"], "WindowsDefault","\uF0EF"),

                // Troubleshooting
                new("SafeMode", "Troubleshooting", ["Exit", "CommandPrompt", "Networking", "Minimal"],"Exit","\uEA18"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<MultiConfigViewModel>>(provider =>
                {
                    List<MultiConfigViewModel> viewModels = new();

                    foreach (MultiServiceRegister item in configurations)
                    {
                        viewModels.Add(CreateMultiOptionConfigurationItemViewModel(item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} multi-configuration entries");
                    return viewModels;
                });
            });
            return host;
        }

        /// <summary>
        /// Registers configuration items
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationItemViewModels(this IHostBuilder host)
        {
            List<ToggleServiceRegister> configurationDictionary = new()
            {
                // General
                new("BackgroundApps", "General", false),
                new("FSOGameBar", "General", true),
                new("AutomaticUpdates", "General", false),
                new("DeliveryOptimisation", "General", false),
                new("Hibernation", "General", false),
                new("Location", "General", false),
                new("PhoneLink", "General", false),
                new("PowerSaving", "General", true),
                new("Sleep", "General", true),
                new("SystemRestore", "General", true),
                new("UpdateNotifications", "General", true),
                new("WebSearch", "General", false),
                new("Widgets", "General", false),
                new("WindowsSpotlight", "General", false),
                new("AppStoreArchiving", "General", false),
                new("Workplace", "General", false),

                // General - AI SubMenu
                new("Copilot", "General/AiSubMenu", false),
                new("Recall", "General/AiSubMenu", false),
                new("ClickToDo", "General/AiSubMenu", false),

                // General - File Sharing SubMenu
                new("GiveAccessToMenu", "General/FileSharingSubMenu", false),
                new("NetworkNavigationPane", "General/FileSharingSubMenu", false),
                new("FileSharing", "General/FileSharingSubMenu", false),

                // General - CPU Idle
                new("CpuIdle", "General/CpuIdleContextMenu", true),
                new("CpuIdleContextMenu", "General/CpuIdleContextMenu", false),

                // Interface
                new("Animation", "Interface", false),
                new("LockScreen", "Interface", true),
                new("ShortcutText", "Interface", false),
                new("EdgeSwipe", "Interface", true),
                new("SnapLayouts", "Interface", true),
                new("RecentItems", "Interface", false),
                new("VerboseMessages", "Interface", false),

                // Interface - Context Menu SubMenu
                new("ExtractContextMenu", "Interface/ContextMenuSubMenu", false),
                new("RunWithPriority", "Interface/ContextMenuSubMenu", false),
                new("TakeOwnership", "Interface/ContextMenuSubMenu", false),
                new("OldContextMenu", "Interface/ContextMenuSubMenu", true),

                // Interface - File Explorer SubMenu
                new("CompactView", "Interface/FileExplorerSubMenu", true),
                new("RemovableDrivesInSidebar", "Interface/FileExplorerSubMenu", false),
                new("AppIconsThumbnail", "Interface/FileExplorerSubMenu", true),
                new("AutomaticFolderDiscovery", "Interface/FileExplorerSubMenu", false),
                new("Gallery", "Interface/FileExplorerSubMenu", false),
                new("Home", "Interface/FileExplorerSubMenu", false),

                // Advanced
                new("ProcessExplorer", "Advanced", false),
                new("MicrosoftStore", "Advanced", true),

                // Advanced - Services SubMenu
                new("Bluetooth", "Advanced/ServicesSubMenu", true),
                new("LanmanWorkstation", "Advanced/ServicesSubMenu", true),
                new("NetworkDiscovery", "Advanced/ServicesSubMenu", true),
                new("Printing", "Advanced/ServicesSubMenu", true),
                new("SuperFetch", "Advanced/ServicesSubMenu", true),

                // Advanced - Services SubMenu - Nvidia Display Container SubMenu
                new("NVidiaDisplayContainer", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu", true),
                new("NVidiaDisplayContainerContextMenu", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu", false),

                // Advanced - Boot Configuration SubMenu - Appearance
                new("BootLogo", "Advanced/BootConfigurationSubMenu/BootConfigAppearance", true),
                new("BootMessages", "Advanced/BootConfigurationSubMenu/BootConfigAppearance", true),
                new("NewBootMenu", "Advanced/BootConfigurationSubMenu/BootConfigAppearance", true),
                new("SpinningAnimation", "Advanced/BootConfigurationSubMenu/BootConfigAppearance", true),

                // Advanced - Boot Configuration SubMenu - Behavior
                new("AdvancedBootOptions", "Advanced/BootConfigurationSubMenu/BootConfigBehavior", false),
                new("AutomaticRepair", "Advanced/BootConfigurationSubMenu/BootConfigBehavior", false),
                new("KernelParameters", "Advanced/BootConfigurationSubMenu/BootConfigBehavior", false),
                new("HighestMode", "Advanced/BootConfigurationSubMenu/BootConfigBehavior", false),

                // Security - Core Isolation SubMenu
                new("VbsState", "Security/CoreIsolationSubMenu", true),

                // Security - Defender SubMenu
                new("HideAppBrowserControl", "Security/DefenderSubMenu", true),
                new("SecurityHealthTray", "Security/DefenderSubMenu", false),

                // Security - Mitigations SubMenu
                new("FaultTolerantHeap", "Security/MitigationsSubMenu", false),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationItemViewModel>>(provider =>
                {
                    List<ConfigurationItemViewModel> viewModels = new();

                    foreach (ToggleServiceRegister item in configurationDictionary)
                    {
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
                serviceProvider.GetServices<Profiles>(),
                serviceProvider.GetServices<ConfigurationItemViewModel>());
        }
        private static SoftwarePageViewModel CreateSoftwarePageViewModel(IServiceProvider serviceProvider)
        {
            return SoftwarePageViewModel.LoadViewModel(
                serviceProvider.GetServices<SoftwareItemViewModel>());
        }
        #endregion Create ViewModels
    }
}
