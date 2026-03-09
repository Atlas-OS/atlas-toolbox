using AtlasToolbox.Services.ConfigurationServices;
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
                services.AddTransient(CreateConfigPageViewModel);
                services.AddTransient(CreateHomePageViewModel);
                services.AddTransient(CreateSoftwarePageViewModel);
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


        //private static string App.App.GetValueFromItemList(string key, bool desc = false)
        //{
        //    if (!desc) return list.Where(item => item.Key == key).Select(item => item.Value).FirstOrDefault();
        //    else return list.Where(item => item.Key == key + "Description").Select(item => item.Value).FirstOrDefault();
        //}

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
            List<Links> configurationDictionary = new()
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

                    foreach (Links item in configurationDictionary)
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
            List<ConfigurationButton> configurationDictionary = new()
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

                    foreach (ConfigurationButton item in configurationDictionary)
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
            List<Route> routes = new()
            {
                // 'General' routes
                new Route("General", "\uE80F", true),
                new Route("General/AiSubMenu", "\uF4A5"),
                new Route("General/FileSharingSubMenu", "\uF193"),
                new Route("General/WindowsUpdate", "\uEDAB"),

                // 'Interface' routes
                new Route("Interface", "\uE713", true),
                new Route("Interface/StartMenuSubMenu", "\uE8FC"),
                new Route("Interface/ContextMenuSubMenu", null),
                new Route("Interface/FileExplorerSubMenu", "\uEC50"),

                // 'Windows' routes
                new Route("Windows", "\uE71D", true),

                // 'Advanced' routes
                new Route("Advanced", "\uE71D", true),
                new Route("Advanced/ServicesSubMenu", "\uE9F5"),
                new Route("Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu", null),
                new Route("Advanced/BootConfigurationSubMenu", "\uF259"),
                new Route("Advanced/BootConfigurationSubMenu/BootConfigAppearance", "\uE620"),
                new Route("Advanced/BootConfigurationSubMenu/BootConfigBehavior", "\uF259"),
                new Route("Advanced/DriverConfigurationSubMenu", "\uE772"),

                // 'Security' routes
                new Route("Security", "\uE71D", true),
                new Route("Security/CoreIsolationSubMenu", "\uEEA1"),
                new Route("Security/DefenderSubMenu", "\uE83D"),
                new Route("Security/MitigationsSubMenu", "\uE730"),

                // 'Troubleshooting' routes
                new Route("Troubleshooting", "\uE71D", true),
                new Route("Troubleshooting/TroubleshootingNetwork", "\uE90F"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<Route>>(provider =>
                {
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {routes.Count} route entries");
                    return routes.Where(r => !r.RootRoute);
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
            List<MultiOptionConfiguration> configurations = new()
            {
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
                services.AddSingleton<IEnumerable<MultiOptionConfigurationItemViewModel>>(provider =>
                {
                    List<MultiOptionConfigurationItemViewModel> viewModels = new();

                    foreach (MultiOptionConfiguration item in configurations)
                    {
                        viewModels.Add(CreateMultiOptionConfigurationItemViewModel(provider, item));
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
            List<Configuration> configurationDictionary = new()
            {
                // General
                new("BackgroundApps", "General"),
                new("SearchIndexing", "General"),
                new("FsoAndGameBar", "General"),
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
                new("CpuIdle", "General"),
                new("WindowsHello", "General"),

                // General - AI SubMenu
                new("Copilot", "General/AiSubMenu"),
                new("Recall", "General/AiSubMenu"),

                // General - File Sharing SubMenu
                new("GiveAccessToMenu", "General/FileSharingSubMenu"),
                new("NetworkNavigationPane", "General/FileSharingSubMenu"),
                new("FileSharing", "General/FileSharingSubMenu"),

                // General - Windows Update
                new("ToggleWindowsUpdates", "General/WindowsUpdate"),

                // Interface
                new("Animations", "Interface"),
                new("LockScreen", "Interface"),
                new("ShortcutText", "Interface"),
                new("EdgeSwipe", "Interface"),
                new("SnapLayout", "Interface"),
                new("RecentItems", "Interface"),
                new("VerboseStatusMessage", "Interface"),

                // Interface - Context Menu SubMenu
                new("ExtractContextMenu", "Interface/ContextMenuSubMenu"),
                new("RunWithPriority", "Interface/ContextMenuSubMenu"),
                new("CpuIdleContextMenu", "Interface/ContextMenuSubMenu"),
                new("TakeOwnership", "Interface/ContextMenuSubMenu"),
                new("OldContextMenu", "Interface/ContextMenuSubMenu"),

                // Interface - File Explorer SubMenu
                new("CompactView", "Interface/FileExplorerSubMenu"),
                new("RemovableDrivesInSidebar", "Interface/FileExplorerSubMenu"),
                new("AppIconsThumbnail", "Interface/FileExplorerSubMenu"),
                new("AutomaticFolderDiscovery", "Interface/FileExplorerSubMenu"),
                new("Gallery", "Interface/FileExplorerSubMenu"),

                // Advanced
                new("ProcessExplorer", "Advanced"),
                new("MicrosoftStore", "Advanced"),

                // Advanced - Services SubMenu
                new("Bluetooth", "Bluetooth", "Advanced/ServicesSubMenu"),
                new("LanmanWorkstation", "Advanced/ServicesSubMenu"),
                new("NetworkDiscovery", "Advanced/ServicesSubMenu"),
                new("Printing", "Advanced/ServicesSubMenu"),
                new("SuperFetch", "Advanced/ServicesSubMenu"),

                // Advanced - Services SubMenu - Nvidia Display Container SubMenu
                new("NvidiaDispayContainer", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu"),
                new("AddNvidiaDisplayContainerContextMenu", "Advanced/ServicesSubMenu/NvidiaDisplayContainerSubMenu"),

                // Advanced - Boot Configuration SubMenu - Appearance
                new("BootLogo", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),
                new("BootMessages", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),
                new("NewBootMenu", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),
                new("SpinningAnimation", "Advanced/BootConfigurationSubMenu/BootConfigAppearance"),

                // Advanced - Boot Configuration SubMenu - Behavior
                new("AdvancedBootOptions", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),
                new("AutomaticRepair", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),
                new("KernelParameters", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),
                new("HighestMode", "Advanced/BootConfigurationSubMenu/BootConfigBehavior"),

                // Security - Core Isolation SubMenu
                new("VbsState", "Security/CoreIsolationSubMenu"),

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

                    foreach (Configuration item in configurationDictionary)
                    {
                        viewModels.Add(CreateConfigurationItemViewModel(provider, item));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} configuration entries");
                    return viewModels;
                });
            });
            return host;
        }



        private static MultiOptionConfigurationItemViewModel CreateMultiOptionConfigurationItemViewModel(
            IServiceProvider serviceProvider, MultiOptionConfiguration configuration)
        {
            MultiOptionConfigurationItemViewModel viewModel = new(
                configuration, serviceProvider.GetRequiredKeyedService<MultiOptionConfigurationStore>(configuration.Key),
                serviceProvider.GetRequiredKeyedService<IMultiOptionConfigurationServices>(configuration.Key));

            return viewModel;
        }

        private static ConfigurationItemViewModel CreateConfigurationItemViewModel(
            IServiceProvider serviceProvider, Configuration configuration)
        {
            ConfigurationItemViewModel viewModel = new(
                configuration, serviceProvider.GetRequiredKeyedService<ConfigurationStore>(configuration.Key),
                serviceProvider.GetRequiredKeyedService<IRoutable>(configuration.Key));

            return viewModel;
        }

        #region Create ViewModels
        // Entire region is made to create view models
        private static SoftwareItemViewModel CreateSoftwareItemViewModel(SoftwareItem softwareItem)
        {
            SoftwareItemViewModel viewModel = new(softwareItem);

            return viewModel;
        }

        private static ConfigurationButtonViewModel CreateButtonViewModel(ConfigurationButton configurationButtonViewModel)
        {
            ConfigurationButtonViewModel viewModel = new(configurationButtonViewModel);

            return viewModel;
        }

        private static LinksViewModel CreateLinksViewModel(Links linksItem)
        {
            LinksViewModel viewModel = new(linksItem);

            return viewModel;
        }

        private static ConfigPageViewModel CreateConfigPageViewModel(IServiceProvider serviceProvider)
        {
            return ConfigPageViewModel.LoadViewModel(
                serviceProvider.GetServices<LinksViewModel>(),
                serviceProvider.GetServices<ConfigurationItemViewModel>(),
                serviceProvider.GetServices<MultiOptionConfigurationItemViewModel>(),
                serviceProvider.GetServices<ConfigurationButtonViewModel>(),
                serviceProvider.GetServices<Route>());
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
