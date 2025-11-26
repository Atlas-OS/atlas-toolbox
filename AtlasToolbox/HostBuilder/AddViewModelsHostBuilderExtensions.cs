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
using AtlasToolbox.Enums;
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
            host.AddConfigurationSubMenu();
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
            Dictionary<string, SoftwareItem> configurationDictionary = new()
            {
                ["Ungoogled Chromium"] = new("Ungoogled Chromium", "eloston.ungoogled-chromium"),
                ["Google Chrome"] = new("Google Chrome", "Google.Chrome"),
                ["Mozilla Firefox"] = new("Mozilla Firefox", "Mozilla.Firefox"),
                ["Waterfox"] = new("Waterfox", "Waterfox.Waterfox"),
                ["Brave Browser"] = new("Brave Browser", "Brave.Brave"),
                ["LibreWolf"] = new("LibreWolf", "LibreWolf.LibreWolf"),
                ["Tor Browser"] = new("Tor Browser", "TorProject.TorBrowser"),
                ["Discord"] = new("Discord", "Discord.Discord"),
                ["Discord Canary"] = new("Discord Canary", "Discord.Discord.Canary"),
                ["Steam"] = new("Steam", "Valve.Steam"),
                ["Playnite"] = new("Playnite", "Playnite.Playnite"),
                ["Heroic"] = new("Heroic", "HeroicGamesLauncher.HeroicGamesLauncher"),
                ["Everything"] = new("Everything", "voidtools.Everything"),
                ["Mozilla Thunderbird"] = new("Mozilla Thunderbird", "Mozilla.Thunderbird"),
                ["IrfanView"] = new("IrfanView", "IrfanSkiljan.IrfanView"),
                ["Git"] = new("Git", "Git.Git"),
                ["VLC"] = new("VLC", "VideoLAN.VLC"),
                ["PuTTY"] = new("PuTTY", "PuTTY.PuTTY"),
                ["Ditto"] = new("Ditto", "Ditto.Ditto"),
                ["7-Zip"] = new("7-Zip", "7zip.7zip"),
                ["Teamspeak"] = new("Teamspeak", "TeamSpeakSystems.TeamSpeakClient"),
                ["Spotify"] = new("Spotify", "Spotify.Spotify"),
                ["OBS Studio"] = new("OBS Studio", "OBSProject.OBSStudio"),
                ["MSI Afterburner"] = new("MSI Afterburner", "Guru3D.Afterburner"),
                ["NVCleanstall"] = new("NVCleanstall", "TechPowerUp.NVCleanstall"),
                ["foobar2000"] = new("foobar2000", "PeterPawlowski.foobar2000"),
                ["CPU-Z"] = new("CPU-Z", "CPUID.CPU-Z"),
                ["GPU-Z"] = new("GPU-Z", "TechPowerUp.GPU-Z"),
                ["Notepad++"] = new("Notepad++", "Notepad++.Notepad++"),
                ["VSCode"] = new("VSCode", "Microsoft.VisualStudioCode"),
                ["VSCodium"] = new("VSCodium", "VSCodium.VSCodium"),
                ["BCUninstaller"] = new("BCUninstaller", "Klocman.BulkCrapUninstaller"),
                ["HWiNFO"] = new("HWiNFO", "REALiX.HWiNFO"),
                ["Lightshot"] = new("Lightshot", "Skillbrains.Lightshot"),
                ["ShareX"] = new("ShareX", "ShareX.ShareX"),
                ["Snipping Tool"] = new("Snipping Tool", "9MZ95KL8MR0L"),
                ["ExplorerPatcher"] = new("ExplorerPatcher", "valinet.ExplorerPatcher"),
                ["Powershell 7"] = new("Powershell 7", "Microsoft.PowerShell"),
                ["UniGetUI"] = new("UniGetUI", "MartiCliment.UniGetUI"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<SoftwareItemViewModel>>(provider =>
                {
                    List<SoftwareItemViewModel> viewModels = new();

                    foreach (KeyValuePair<string, SoftwareItem> item in configurationDictionary)
                    {
                        viewModels.Add(CreateSoftwareItemViewModel(item.Value));
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
            Dictionary<string, Links> configurationDictionary = new()
            {
                ["ExplorerPatcher"] = new("https://github.com/valinet/ExplorerPatcher", "ExplorerPatcher", ConfigurationType.StartMenuSubMenu),
                ["StartAllBack"] = new("https://www.startallback.com/", "StartAllBack", ConfigurationType.StartMenuSubMenu),
                ["OpenShellAtlasPreset"] = new(@"http://github.com/Atlas-OS/Atlas/blob/main/src/playbook/Executables/AtlasDesktop/4.%20Interface%20Tweaks/Start%20Menu/Atlas%20Open-Shell%20Preset.xml", App.GetValueFromItemList("OpenShellAtlasPreset"), ConfigurationType.StartMenuSubMenu),
                ["InterfaceTweaksDocumentation"] = new(@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/interface-tweaks/", App.GetValueFromItemList("InterfaceTweaksDocumentation"), ConfigurationType.Interface),

                ["ActivationPage"] = new(@"ms-settings:activation", App.GetValueFromItemList("ActivationPage"), ConfigurationType.Windows, "\uE713"),
                ["ColorsPage"] = new(@"ms-settings:personalization-colors", App.GetValueFromItemList("ColorsPage"), ConfigurationType.Windows, "\uE713"),
                ["DateAndTime"] = new(@"ms-settings:dateandtime", App.GetValueFromItemList("DateAndTime"), ConfigurationType.Windows, "\uE713"),
                ["DefaultApps"] = new(@"ms-settings:defaultapps", App.GetValueFromItemList("DefaultApps"), ConfigurationType.Windows, "\uE713"),
                ["DefaultGraphicsSettings"] = new(@"ms-settings:display-advancedgraphics-default", App.GetValueFromItemList("DefaultGraphicsSettings"), ConfigurationType.Windows, "\uE713"),
                ["RegionLanguage"] = new(@"ms-settings:regionlanguage", App.GetValueFromItemList("RegionLanguage"), ConfigurationType.Windows, "\uE713"),
                ["Privacy"] = new(@"ms-settings:privacy", App.GetValueFromItemList("Privacy"), ConfigurationType.Windows, "\uE713"),
                ["RegionProperties"] = new(@"ms-settings:regionProperties", App.GetValueFromItemList("RegionProperties"), ConfigurationType.Windows, "\uE713"),
                ["Taskbar"] = new(@"ms-settings:taskbar", App.GetValueFromItemList("Taskbar"), ConfigurationType.Windows, "\uE713"),
                ["CoreIsolation"] = new(@"windowsdefender://coreisolation/", App.GetValueFromItemList("CoreIsolation"), ConfigurationType.CoreIsolationSubMenu, "\uE83D"),

                ["BootConfigExplanations"] = new(@"https://learn.microsoft.com/windows-hardware/drivers/devtest/bcdedit--set", App.GetValueFromItemList("BootConfigExplanations"), ConfigurationType.BootConfigurationSubMenu),
                ["AdvancedConfigMustRead"] = new(@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/", App.GetValueFromItemList("AdvancedConfigMustRead"), ConfigurationType.Advanced),
                ["NvidiaDisplayContainerMustReadFirst"] = new(@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/#nvidia-display-container", App.GetValueFromItemList("NvidiaDisplayContainerMustReadFirst"), ConfigurationType.NvidiaDisplayContainerSubMenu),
                ["SecurityDocumentation"] = new(@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/security/", App.GetValueFromItemList("SecurityDocumentation"), ConfigurationType.Security),

                ["WindowsSettingsDocumentation"] = new(@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/windows-settings/", App.GetValueFromItemList("WindowsSettingsDocumentation"), ConfigurationType.Windows),
                ["AutoGpuAffinity"] = new(@"https://github.com/valleyofdoom/AutoGpuAffinity", "AutoGpuAffinity", ConfigurationType.DriverConfigurationSubMenu),
                ["GoInterruptPolicy"] = new(@"https://github.com/spddl/GoInterruptPolicy", "GoInterruptPolicy", ConfigurationType.DriverConfigurationSubMenu),
                ["InterrupAffinityTool"] = new(@"https://www.techpowerup.com/download/microsoft-interrupt-affinity-tool", App.GetValueFromItemList("InterrupAffinityTool"), ConfigurationType.DriverConfigurationSubMenu),
                ["MSIUtilityV3"] = new(@"https://forums.guru3d.com/threads/windows-line-based-vs-message-signaled-based-interrupts-msi-tool.378044", "MSI Utility V3", ConfigurationType.DriverConfigurationSubMenu),
                ["ProcessExplorerApp"] = new(@"https://learn.microsoft.com/en-us/sysinternals/downloads/process-explorer", App.GetValueFromItemList("ProcessExplorerDocumentation"), ConfigurationType.Advanced),
                ["ResetPC"] = new(@"https://docs.atlasos.net/getting-started/reverting-atlas/", App.GetValueFromItemList("ResetPC"), ConfigurationType.Troubleshooting),
                ["TroubleshootingDocumentation"] = new(@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/troubleshooting/", App.GetValueFromItemList("TroubleshootingDocumenation"), ConfigurationType.Troubleshooting),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<LinksViewModel>>(provider =>
                {
                    List<LinksViewModel> viewModels = new();

                    foreach (KeyValuePair<string, Links> item in configurationDictionary)
                    {
                        viewModels.Add(CreateLinksViewModel(item.Value));
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
            Dictionary<string, ConfigurationButton> configurationDictionary = new()
            {
                ["RestartExplorerButton"] = new(buttonCommand = new RestartExplorerCommand(), App.GetValueFromItemList("RestartExplorerButton"), App.GetValueFromItemList("RestartExplorerButton", true), ConfigurationType.Interface, "\uEC50"),
                ["ViewCurrentSettingsBootConfig"] = new(buttonCommand = new ViewCurrentValuesCommand(), App.GetValueFromItemList("ViewCurrentSettingsBootConfig"), App.GetValueFromItemList("ViewCurrentSettingsBootConfig", true), ConfigurationType.BootConfigurationSubMenu, "\uF259"),
                ["VBSCurrentConfig"] = new(buttonCommand = new CurrentVBSConfigurationCommand(), App.GetValueFromItemList("VBSCurrentConfig"), App.GetValueFromItemList("VBSCurrentConfig", true), ConfigurationType.CoreIsolationSubMenu, "\uF259"),
                ["ToggleDefender"] = new(buttonCommand = new ToggleDefenderCommand(), App.GetValueFromItemList("ToggleDefender"), App.GetValueFromItemList("ToggleDefender", true), ConfigurationType.DefenderSubMenu, "\uE83D"),
                ["ResetFTH"] = new(buttonCommand = new ResetFTHCommand(), App.GetValueFromItemList("ResetFTH"), App.GetValueFromItemList("ResetFTH", true), ConfigurationType.MitigationsSubMenu, "\uEBC4"),
                ["InstallOpenShell"] = new(buttonCommand = new InstallOpenShellCommand(), App.GetValueFromItemList("InstallOpenShell"), App.GetValueFromItemList("InstallOpenShell", true), ConfigurationType.StartMenuSubMenu, "\uE8FC"),

                ["FixErrors"] = new(buttonCommand = new FixErrorsCommand(), App.GetValueFromItemList("FixErrors"), App.GetValueFromItemList("FixErrors", true), ConfigurationType.Troubleshooting, "\uE90F"),
                ["RepairWinComponent"] = new(buttonCommand = new RepairWindowsComponentsCommand(), App.GetValueFromItemList("FixErrors"), App.GetValueFromItemList("RepairWinComponent"), ConfigurationType.Troubleshooting, "\uE90F"),
                ["TelemetryComponents"] = new(buttonCommand = new TelemetryComponentsCommand(), App.GetValueFromItemList("FixErrors"), App.GetValueFromItemList("TelemetryComponents"), ConfigurationType.Troubleshooting, "\uE90F"),
                ["AtlasDefault"] = new(buttonCommand = new NetworkAtlasDefaults(), App.GetValueFromItemList("ResetFTH"), App.GetValueFromItemList("AtlasDefault"), ConfigurationType.TroubleshootingNetwork, "\uE839"),
                ["WindowsDefault"] = new(buttonCommand = new NetworkWindowsDefaults(), App.GetValueFromItemList("ResetFTH"), App.GetValueFromItemList("WindowsDefault"), ConfigurationType.TroubleshootingNetwork, "\uE839"),
                ["SetUpdateDeferral"] = new(buttonCommand = new SetUpdateDeferralConfigurationButton(), App.GetValueFromItemList("Set"), App.GetValueFromItemList("WindowsUpdateDeferral"), ConfigurationType.WindowsUpdate, "\uE916"),
                ["ResetUpdateDeferral"] = new(buttonCommand = new ResetWindowsUpdateDeferral(), App.GetValueFromItemList("ResetFTH"), App.GetValueFromItemList("ResetWindowsUpdateDeferral"), ConfigurationType.WindowsUpdate, "\uE81C"),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationButtonViewModel>>(provider =>
                {
                    List<ConfigurationButtonViewModel> viewModels = new();

                    foreach (KeyValuePair<string, ConfigurationButton> item in configurationDictionary)
                    {
                        viewModels.Add(CreateButtonViewModel(item.Value));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} button entries");
                    return viewModels;
                });
            });
            return host;
        }

        /// <summary>
        /// Registers sub-menus
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private static IHostBuilder AddConfigurationSubMenu(this IHostBuilder host)
        {
            Dictionary<string, ConfigurationSubMenu> configurationDictionary = new()
            {
                ["BootConfigAppearance"] = new("BootConfigAppearance", App.GetValueFromItemList("BootConfigAppearance"), App.GetValueFromItemList("BootConfigAppearance", true), ConfigurationType.BootConfigurationSubMenu, "\uE620"),
                ["BootConfigBehavior"] = new("BootConfigBehavior", App.GetValueFromItemList("BootConfigBehavior"), App.GetValueFromItemList("BootConfigBehavior", true), ConfigurationType.BootConfigurationSubMenu, "\uF259"),
                ["NvidiaDisplayContainerSubMenu"] = new("NvidiaDisplayContainerSubMenu", App.GetValueFromItemList("NvidiaDisplayContainerSubMenu"), App.GetValueFromItemList("NvidiaDisplayContainerSubMenu", true), ConfigurationType.ServicesSubMenu),

                ["StartMenuSubMenu"] = new("StartMenuSubMenu", App.GetValueFromItemList("StartMenuSubMenu"), App.GetValueFromItemList("StartMenuSubMenu", true), ConfigurationType.Interface, "\uE8FC"),
                ["ContextMenuSubMenu"] = new("ContextMenuSubMenu", App.GetValueFromItemList("ContextMenuSubMenu"), App.GetValueFromItemList("ContextMenuSubMenu", true), ConfigurationType.Interface),
                ["AiSubMenu"] = new("AiSubMenu", App.GetValueFromItemList("AiSubMenu"), App.GetValueFromItemList("AiSubMenu", true), ConfigurationType.General, "\uF4A5"),
                ["ServicesSubMenu"] = new("ServicesSubMenu", App.GetValueFromItemList("ServicesSubMenu"), App.GetValueFromItemList("ServicesSubMenu", true), ConfigurationType.Advanced, "\uE9F5"),
                ["BootConfigurationSubMenu"] = new("BootConfigurationSubMenu", App.GetValueFromItemList("BootConfigurationSubMenu"), App.GetValueFromItemList("BootConfigurationSubMenu", true), ConfigurationType.Advanced, "\uF259"),
                ["FileExplorerSubMenu"] = new("FileExplorerSubMenu", App.GetValueFromItemList("FileExplorerSubMenu"), App.GetValueFromItemList("FileExplorerSubMenu", true), ConfigurationType.Interface, "\uEC50"),
                ["DriverConfigurationSubMenu"] = new("DriverConfigurationSubMenu", App.GetValueFromItemList("DriverConfigurationSubMenu"), App.GetValueFromItemList("DriverConfigurationSubMenu", true), ConfigurationType.Advanced, "\uE772"),
                ["CoreIsolationSubMenu"] = new("CoreIsolationSubMenu", App.GetValueFromItemList("CoreIsolationSubMenu"), App.GetValueFromItemList("CoreIsolationSubMenu", true), ConfigurationType.Security, "\uEEA1"),
                ["DefenderSubMenu"] = new("DefenderSubMenu", App.GetValueFromItemList("DefenderSubMenu"), App.GetValueFromItemList("DefenderSubMenu", true), ConfigurationType.Security, "\uE83D"),
                ["MitigationsSubMenu"] = new("MitigationsSubMenu", App.GetValueFromItemList("MitigationsSubMenu"), App.GetValueFromItemList("MitigationsSubMenu", true), ConfigurationType.Security, "\uE730"),
                ["TroubleshootingNetwork"] = new("TroubleshootingNetwork", App.GetValueFromItemList("TroubleshootingNetwork"), App.GetValueFromItemList("TroubleshootingNetwork", true), ConfigurationType.Troubleshooting, "\uE90F"),
                ["FileSharingSubMenu"] = new("FileSharingSubMenu", App.GetValueFromItemList("FileSharingSubMenu"), App.GetValueFromItemList("FileSharingSubMenu", true), ConfigurationType.General, "\uF193"),
                ["WindowsUpdate"] = new("WindowsUpdate", App.GetValueFromItemList("WindowsUpdate"), App.GetValueFromItemList("WindowsUpdate", true), ConfigurationType.General, "\uEDAB"),
            };
            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationSubMenuViewModel>>(provider =>
                {
                    List<ConfigurationSubMenuViewModel> viewModels = new();
                    foreach (KeyValuePair<string, ConfigurationSubMenu> subMenu in configurationDictionary)
                    {
                        ObservableCollection<ConfigurationItemViewModel> itemViewModels = new ObservableCollection<ConfigurationItemViewModel>(provider.GetServices<ConfigurationItemViewModel>().Where(item => item.Type.ToString() == subMenu.Key));
                        ObservableCollection<MultiOptionConfigurationItemViewModel> multiOptionItemViewModels = new ObservableCollection<MultiOptionConfigurationItemViewModel>(provider.GetServices<MultiOptionConfigurationItemViewModel>().Where(item => item.Type.ToString() == subMenu.Key));
                        ObservableCollection<LinksViewModel> linksViewModel = new ObservableCollection<LinksViewModel>(provider.GetServices<LinksViewModel>().Where(item => item.Type.ToString() == subMenu.Key));
                        ObservableCollection<ConfigurationSubMenuViewModel> configurationSubMenuViewModels = new ObservableCollection<ConfigurationSubMenuViewModel>(viewModels.Where(item => item.Type.ToString() == subMenu.Key));
                        ObservableCollection<ConfigurationButtonViewModel> configurationButtonViewModels = new ObservableCollection<ConfigurationButtonViewModel>(provider.GetServices<ConfigurationButtonViewModel>().Where(item => item.Type.ToString() == subMenu.Key));

                        ConfigurationSubMenuViewModel viewModel = CreateConfigurationSubMenuViewModel(provider, itemViewModels, multiOptionItemViewModels, linksViewModel, subMenu.Key, subMenu.Value, configurationSubMenuViewModels, configurationButtonViewModels);
                        viewModels.Add(viewModel);
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} submenu entries");
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
            // TODO: Change configuration types
            Dictionary<string, MultiOptionConfiguration> configurationDictionary = new()
            {
                ["ContextMenuTerminals"] = new(App.GetValueFromItemList("ContextMenuTerminals"), "ContextMenuTerminals", ConfigurationType.ContextMenuSubMenu),
                ["ShortcutIcon"] = new(App.GetValueFromItemList("ShortcutIcon"), "ShortcutIcon", ConfigurationType.Interface),
                ["Mitigations"] = new(App.GetValueFromItemList("Mitigations"), "Mitigations", ConfigurationType.MitigationsSubMenu),
                ["SafeMode"] = new(App.GetValueFromItemList("SafeMode"), "SafeMode", ConfigurationType.Troubleshooting),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<MultiOptionConfigurationItemViewModel>>(provider =>
                {
                    List<MultiOptionConfigurationItemViewModel> viewModels = new();

                    foreach (KeyValuePair<string, MultiOptionConfiguration> item in configurationDictionary)
                    {
                        viewModels.Add(CreateMultiOptionConfigurationItemViewModel(provider, item.Key, item.Value));
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
            // TODO: Change configuration types`
            Dictionary<string, Configuration> configurationDictionary = new()
            {
                ["Animations"] = new(App.GetValueFromItemList("Animations"), "Animations", ConfigurationType.Interface),
                ["ExtractContextMenu"] = new(App.GetValueFromItemList("ExtractContextMenu"), "ExtractContextMenu", ConfigurationType.ContextMenuSubMenu),
                ["RunWithPriority"] = new(App.GetValueFromItemList("RunWithPriority"), "RunWithPriority", ConfigurationType.ContextMenuSubMenu),
                ["Bluetooth"] = new("Bluetooth", "Bluetooth", ConfigurationType.ServicesSubMenu),
                ["LanmanWorkstation"] = new(App.GetValueFromItemList("LanmanWorkstation"), "LanmanWorkstation", ConfigurationType.ServicesSubMenu),
                ["NetworkDiscovery"] = new(App.GetValueFromItemList("NetworkDiscovery"), "NetworkDiscovery", ConfigurationType.ServicesSubMenu),
                ["Printing"] = new(App.GetValueFromItemList("Printing"), "Printing", ConfigurationType.ServicesSubMenu),
                ["NvidiaDispayContainer"] = new(App.GetValueFromItemList("NvidiaDispayContainer"), "NvidiaDispayContainer", ConfigurationType.NvidiaDisplayContainerSubMenu),
                ["AddNvidiaDisplayContainerContextMenu"] = new(App.GetValueFromItemList("AddNvidiaDisplayContainerContextMenu"), "AddNvidiaDisplayContainerContextMenu", ConfigurationType.NvidiaDisplayContainerSubMenu),
                ["CpuIdleContextMenu"] = new(App.GetValueFromItemList("CpuIdleContextMenu"), "CpuIdleContextMenu", ConfigurationType.ContextMenuSubMenu),
                ["LockScreen"] = new(App.GetValueFromItemList("LockScreen"), "LockScreen", ConfigurationType.Interface),
                ["ShortcutText"] = new(App.GetValueFromItemList("ShortcutText"), "ShortcutText", ConfigurationType.Interface),
                ["BootLogo"] = new(App.GetValueFromItemList("BootLogo"), "BootLogo", ConfigurationType.BootConfigAppearance),
                ["BootMessages"] = new(App.GetValueFromItemList("BootMessages"), "BootMessages", ConfigurationType.BootConfigAppearance),
                ["NewBootMenu"] = new(App.GetValueFromItemList("NewBootMenu"), "NewBootMenu", ConfigurationType.BootConfigAppearance),
                ["SpinningAnimation"] = new(App.GetValueFromItemList("SpinningAnimation"), "SpinningAnimations", ConfigurationType.BootConfigAppearance),
                ["AdvancedBootOptions"] = new(App.GetValueFromItemList("AdvancedBootOptions"), "AdvancedBootOptions", ConfigurationType.BootConfigBehavior),
                ["AutomaticRepair"] = new(App.GetValueFromItemList("AutomaticRepair"), "AutomaticRepair", ConfigurationType.BootConfigBehavior),
                ["KernelParameters"] = new(App.GetValueFromItemList("KernelParameters"), "KernelParameters", ConfigurationType.BootConfigBehavior),
                ["HighestMode"] = new(App.GetValueFromItemList("HighestMode"), "HighestMode", ConfigurationType.BootConfigBehavior),
                ["CompactView"] = new(App.GetValueFromItemList("CompactView"), "CompactView", ConfigurationType.FileExplorerSubMenu),
                ["RemovableDrivesInSidebar"] = new(App.GetValueFromItemList("RemovableDrivesInSidebar"), "RemovableDrivesInSidebar", ConfigurationType.FileExplorerSubMenu),
                ["BackgroundApps"] = new(App.GetValueFromItemList("BackgroundApps"), "BackgroundApps", ConfigurationType.General),
                ["SearchIndexing"] = new(App.GetValueFromItemList("SearchIndexing"), "SearchIndexing", ConfigurationType.General),
                ["FsoAndGameBar"] = new(App.GetValueFromItemList("FsoAndGameBar"), "FsoAndGameBar", ConfigurationType.General),
                ["AutomaticUpdates"] = new(App.GetValueFromItemList("AutomaticUpdates"), "AutomaticUpdates", ConfigurationType.General),
                ["DeliveryOptimisation"] = new(App.GetValueFromItemList("DeliveryOptimisation"), "DeliveryOptimisation", ConfigurationType.General),
                ["Hibernation"] = new(App.GetValueFromItemList("Hibernation"), "Hibernation", ConfigurationType.General),
                ["Location"] = new(App.GetValueFromItemList("Location"), "Location", ConfigurationType.General),
                ["PhoneLink"] = new(App.GetValueFromItemList("PhoneLink"), "PhoneLink", ConfigurationType.General),
                ["PowerSaving"] = new(App.GetValueFromItemList("PowerSaving"), "PowerSaving", ConfigurationType.General),
                ["Sleep"] = new(App.GetValueFromItemList("Sleep"), "Sleep", ConfigurationType.General),
                ["SystemRestore"] = new(App.GetValueFromItemList("SystemRestore"), "SystemRestore", ConfigurationType.General),
                ["UpdateNotifications"] = new(App.GetValueFromItemList("UpdateNotifications"), "UpdateNotifications", ConfigurationType.General),
                ["WebSearch"] = new(App.GetValueFromItemList("WebSearch"), "WebSearch", ConfigurationType.General),
                ["Widgets"] = new(App.GetValueFromItemList("Widgets"), "Widgets", ConfigurationType.General),
                ["WindowsSpotlight"] = new(App.GetValueFromItemList("WindowsSpotlight"), "WindowsSpotlight", ConfigurationType.General),
                ["AppStoreArchiving"] = new(App.GetValueFromItemList("AppStoreArchiving"), "AppStoreArchiving", ConfigurationType.General),
                ["TakeOwnership"] = new(App.GetValueFromItemList("TakeOwnership"), "TakeOwnership", ConfigurationType.ContextMenuSubMenu),
                ["OldContextMenu"] = new(App.GetValueFromItemList("OldContextMenu"), "OldContextMenu", ConfigurationType.ContextMenuSubMenu),
                ["EdgeSwipe"] = new(App.GetValueFromItemList("EdgeSwipe"), "EdgeSwipe", ConfigurationType.Interface),
                ["AppIconsThumbnail"] = new(App.GetValueFromItemList("AppIconsThumbnail"), "AppIconsThumbnail", ConfigurationType.FileExplorerSubMenu),
                ["AutomaticFolderDiscovery"] = new(App.GetValueFromItemList("AutomaticFolderDiscovery"), "AutomaticFolderDiscovery", ConfigurationType.FileExplorerSubMenu),
                ["Gallery"] = new(App.GetValueFromItemList("Gallery"), "Gallery", ConfigurationType.FileExplorerSubMenu),
                ["SnapLayout"] = new(App.GetValueFromItemList("SnapLayout"), "SnapLayout", ConfigurationType.Interface),
                ["RecentItems"] = new(App.GetValueFromItemList("RecentItems"), "RecentItems", ConfigurationType.Interface),
                ["VerboseStatusMessage"] = new(App.GetValueFromItemList("VerboseStatusMessage"), "VerboseStatusMessage", ConfigurationType.Interface),
                ["SuperFetch"] = new(App.GetValueFromItemList("SuperFetch"), "SuperFetch", ConfigurationType.ServicesSubMenu),
                ["HideAppBrowserControl"] = new(App.GetValueFromItemList("HideAppBrowserControl"), "HideAppBrowserControl", ConfigurationType.DefenderSubMenu),
                ["SecurityHealthTray"] = new(App.GetValueFromItemList("SecurityHealthTray"), "SecurityHealthTray", ConfigurationType.DefenderSubMenu),
                ["FaultTolerantHeap"] = new(App.GetValueFromItemList("FaultTolerantHeap"), "FaultTolerantHeap", ConfigurationType.MitigationsSubMenu),
                ["Copilot"] = new(App.GetValueFromItemList("Copilot"), "Copilot", ConfigurationType.AiSubMenu),
                ["Recall"] = new(App.GetValueFromItemList("Recall"), "recall", ConfigurationType.AiSubMenu),
                ["CpuIdle"] = new(App.GetValueFromItemList("CpuIdle"), "CpuIdle", ConfigurationType.General),
                ["ProcessExplorer"] = new(App.GetValueFromItemList("ProcessExplorer"), "ProcessExplorer", ConfigurationType.Advanced),
                ["VbsState"] = new(App.GetValueFromItemList("VbsState"), "VbsState", ConfigurationType.CoreIsolationSubMenu),
                ["GiveAccessToMenu"] = new(App.GetValueFromItemList("GiveAccessToMenu"), "GiveAccessToMenu", ConfigurationType.FileSharingSubMenu),
                ["NetworkNavigationPane"] = new(App.GetValueFromItemList("NetworkNavigationPane"), "NetworkNavigationPane", ConfigurationType.FileSharingSubMenu),
                ["FileSharing"] = new(App.GetValueFromItemList("FileSharing"), "FileSharing", ConfigurationType.FileSharingSubMenu),
                ["WindowsHello"] = new(App.GetValueFromItemList("WindowsHello"), "WindowsHello", ConfigurationType.General),
                ["ToggleWindowsUpdates"] = new(App.GetValueFromItemList("ToggleWindowsUpdates"), "ToggleWindowsUpdates", ConfigurationType.WindowsUpdate),
                ["MicrosoftStore"] = new(App.GetValueFromItemList("MicrosoftStoreToggle"), "MicrosoftStore", ConfigurationType.Advanced),
            };

            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationItemViewModel>>(provider =>
                {
                    List<ConfigurationItemViewModel> viewModels = new();

                    foreach (KeyValuePair<string, Configuration> item in configurationDictionary)
                    {
                        //Could work, but needs to await for everything to be completed before returning viewModels
                        //Task.Run(() => { viewModels.Add(CreateConfigurationItemViewModel(provider, item.Key, item.Value)); });
                        viewModels.Add(CreateConfigurationItemViewModel(provider, item.Key, item.Value));
                    }
                    App.logger.Info($"[VMHostBuilder] Successfully loaded {viewModels.Count} configuration entries");
                    return viewModels;
                });
            });
            return host;
        }



        private static MultiOptionConfigurationItemViewModel CreateMultiOptionConfigurationItemViewModel(
            IServiceProvider serviceProvider, object key, MultiOptionConfiguration configuration)
        {
            MultiOptionConfigurationItemViewModel viewModel = new(
                configuration, serviceProvider.GetRequiredKeyedService<MultiOptionConfigurationStore>(key), serviceProvider.GetRequiredKeyedService<IMultiOptionConfigurationServices>(key));

            return viewModel;
        }

        private static ConfigurationItemViewModel CreateConfigurationItemViewModel(
            IServiceProvider serviceProvider, object key, Configuration configuration)
        {
            ConfigurationItemViewModel viewModel = new(
                configuration, serviceProvider.GetRequiredKeyedService<ConfigurationStore>(key), serviceProvider.GetRequiredKeyedService<IConfigurationService>(key));

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
                serviceProvider.GetServices<ConfigurationSubMenuViewModel>(),
                serviceProvider.GetServices<ConfigurationButtonViewModel>());
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
        private static ConfigurationSubMenuViewModel CreateConfigurationSubMenuViewModel(
          IServiceProvider serviceProvider, ObservableCollection<ConfigurationItemViewModel> configurationItemViewModels, ObservableCollection<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModel, ObservableCollection<LinksViewModel> linksViewModel, object key, ConfigurationSubMenu configuration, ObservableCollection<ConfigurationSubMenuViewModel> configurationSubMenuViewModel, ObservableCollection<ConfigurationButtonViewModel> configurationButtonViewModels)
        {
            ConfigurationStoreSubMenu configurationStoreSubMenu = serviceProvider.GetRequiredKeyedService<ConfigurationStoreSubMenu>(key);

            ConfigurationSubMenuViewModel viewModel = new(
               configuration, configurationStoreSubMenu, configurationItemViewModels, multiOptionConfigurationItemViewModel, linksViewModel, configurationSubMenuViewModel, configurationButtonViewModels);

            return viewModel;
        }
        #endregion Create ViewModels
    }
}
