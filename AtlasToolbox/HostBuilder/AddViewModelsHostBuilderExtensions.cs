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

namespace AtlasToolbox.HostBuilder
{
    public static class AddViewModelsHostBuilderExtensions
    {
        private static List<Object> subMenuOnlyItems = new List<Object>();
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

            return host;
        }

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
            } catch
            {
                Directory.CreateDirectory($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles");
            } finally
            {
                FileInfo[] profileFile = profilesDirectory.GetFiles();
                foreach (FileInfo file in profileFile)
                {
                    configurationDictionary.Add(ProfileSerializing.DeserializeProfile(file.FullName));
                };
                host.ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IEnumerable<Profiles>> (provider =>
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
                ["ExplorerPatcher"] = new ("https://github.com/valinet/ExplorerPatcher", "ExplorerPatcher", ConfigurationCategory.StartMenuSubMenu),
                ["StartAllBack"] = new ("https://www.startallback.com/", "StartAllBack", ConfigurationCategory.StartMenuSubMenu),
                ["OpenShellAtlasPreset"] = new (@"http://github.com/Atlas-OS/Atlas/blob/main/src/playbook/Executables/AtlasDesktop/4.%20Interface%20Tweaks/Start%20Menu/Atlas%20Open-Shell%20Preset.xml", "Open Shell AtlasOS preset", ConfigurationCategory.StartMenuSubMenu),
                ["InterfaceTweaksDocumentation"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/interface-tweaks/", "Interface tweaks documentation", ConfigurationCategory.Interface),
                
                ["ActivationPage"] = new (@"ms-settings:activation", "Windows activation status", ConfigurationCategory.Windows, "\uE713"),
                ["ColorsPage"] = new (@"ms-settings:personalization-colors", "Color personalisation settings", ConfigurationCategory.Windows, "\uE713"),
                ["DateAndTime"] = new (@"ms-settings:dateandtime", "Date and time settings", ConfigurationCategory.Windows, "\uE713"),
                ["DefaultApps"] = new (@"ms-settings:defaultapps", "Default Apps", ConfigurationCategory.Windows, "\uE713"),
                ["DefaultGraphicsSettings"] = new (@"ms-settings:display-advancedgraphics-default", "DefaultGraphicsSettings", ConfigurationCategory.Windows, "\uE713"),
                ["RegionLanguage"] = new (@"ms-settings:regionlanguage", "Region Properties", ConfigurationCategory.Windows, "\uE713"),
                ["Privacy"] = new (@"ms-settings:privacy", "Privacy Settings", ConfigurationCategory.Windows, "\uE713"),
                ["RegionProperties"] = new (@"C:\Windows\System32\rundll32.exe C:\Windows\System32\shell32.dll,Control_RunDLL C:\Windows\System32\intl.cpl", "RegionProperties", ConfigurationCategory.Windows, "\uE713"),
                ["Taskbar"] = new (@"ms-settings:taskbar", "Taskbar settings", ConfigurationCategory.Windows, "\uE713"),
                ["CoreIsolation"] = new (@"windowsdefender://coreisolation/", "Core Isolation - Windows Security", ConfigurationCategory.CoreIsolationSubMenu, "\uE83D"),


                ["WindowsSettingsDocumentation"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/windows-settings/", "Windows Settings Documentation", ConfigurationCategory.Windows),
                ["BootConfigExplanations"] = new (@"https://learn.microsoft.com/windows-hardware/drivers/devtest/bcdedit--set", "Explanations from Microsoft", ConfigurationCategory.BootConfigurationSubMenu),
                ["AutoGpuAffinity"] = new (@"https://github.com/valleyofdoom/AutoGpuAffinity", "AutoGpuAffinity", ConfigurationCategory.DriverConfigurationSubMenu),
                ["GoInterruptPolicy"] = new (@"https://github.com/spddl/GoInterruptPolicy", "GoInterruptPolicy", ConfigurationCategory.DriverConfigurationSubMenu),
                ["InterrupAffinityTool"] = new (@"https://www.techpowerup.com/download/microsoft-interrupt-affinity-tool", "Interrupt Affinity Tool", ConfigurationCategory.DriverConfigurationSubMenu),
                ["MSIUtilityV3"] = new (@"https://forums.guru3d.com/threads/windows-line-based-vs-message-signaled-based-interrupts-msi-tool.378044", "MSI Utility V3", ConfigurationCategory.DriverConfigurationSubMenu),
                ["ProcessExplorer"] = new (@"https://learn.microsoft.com/en-us/sysinternals/downloads/process-explorer", "Process Explorer", ConfigurationCategory.Advanced),
                ["NvidiaDisplayContainerMustReadFirst"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/#nvidia-display-container", "Must read first", ConfigurationCategory.NvidiaDisplayContainerSubMenu),
                ["AdvancedConfigMustRead"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/", "Must read first (Documentation)", ConfigurationCategory.Advanced),
                ["SecurityDocumentation"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/security/", "Security documentation", ConfigurationCategory.Security),
                ["ResetPC"] = new (@"https://docs.atlasos.net/getting-started/reverting-atlas/", "Reset this PC (read first)", ConfigurationCategory.Troubleshooting),
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
                ["RestartExplorerButton"] = new(buttonCommand = new RestartExplorerCommand(), "Restart Explorer.exe", "Some interface settings may require you to restart explorer.exe", ConfigurationCategory.Interface),
                ["ViewCurrentSettingsBootConfig"] = new(buttonCommand = new ViewCurrentValuesCommand(), "View current values", "See boot configuration values", ConfigurationCategory.BootConfigurationSubMenu),
                ["VBSCurrentConfig"] = new(buttonCommand = new CurrentVBSConfigurationCommand(), "VBS Current Configuration", "See the current VBS configuration", ConfigurationCategory.CoreIsolationSubMenu),
                ["ToggleDefender"] = new(buttonCommand = new ToggleDefenderCommand(), "Toggle", "Toggle Windows Defender", ConfigurationCategory.DefenderSubMenu),
                ["ResetFTH"] = new(buttonCommand = new ResetFTHCommand(), "Reset", "Reset FTH entries", ConfigurationCategory.MitigationsSubMenu),
                ["InstallOpenShell"] = new(buttonCommand = new InstallOpenShellCommand(), "Install OpenShell", "Install", ConfigurationCategory.StartMenuSubMenu),

                ["FixErrors"] = new(buttonCommand = new FixErrorsCommand(), "Troubleshoot", "Fix Errors 2502 and 2503", ConfigurationCategory.Troubleshooting),
                ["RepairWinComponent"] = new(buttonCommand = new RepairWindowsComponentsCommand(), "Troubleshoot", "Repair Windows Components", ConfigurationCategory.Troubleshooting),
                ["TelemetryComponents"] = new(buttonCommand = new TelemetryComponentsCommand(), "Troubleshoot", "Telemetry Components", ConfigurationCategory.Troubleshooting),
                ["AtlasDefault"] = new(buttonCommand = new NetworkAtlasDefaults(), "Reset", "Reset Network to Atlas Defaults", ConfigurationCategory.TroubleshootingNetwork),
                ["WindowsDefault"] = new(buttonCommand = new NetworkWindowsDefaults(), "Reset", "Reset Network to Windows Defaults", ConfigurationCategory.TroubleshootingNetwork),
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
            // TODO: Change configuration types
            Dictionary<string, ConfigurationSubMenu> configurationDictionary = new()
            {
                ["BootConfigAppearance"] = new("Boot configuration appearance", "Everything related to the appearance of booting Windows", ConfigurationCategory.BootConfigurationSubMenu),
                ["BootConfigBehavior"] = new("Boot behavior", "Everything related to booting behavior", ConfigurationCategory.BootConfigurationSubMenu),
                ["NvidiaDisplayContainerSubMenu"] = new("NVIDIA Display Container", "Everything related to the NVIDIA Display Container", ConfigurationCategory.ServicesSubMenu),

                ["StartMenuSubMenu"] = new("Start Menu", "Everything related to customizing the Windows Start Menu", ConfigurationCategory.Interface),
                ["ContextMenuSubMenu"] = new("Context Menu", "Everything related to the context menu", ConfigurationCategory.Interface),
                ["AiSubMenu"] = new("AI Features", "Everything related to AI features in Windows 11", ConfigurationCategory.General),
                ["ServicesSubMenu"] = new("Services", "Everything related to services in Windows", ConfigurationCategory.Advanced),
                ["BootConfigurationSubMenu"] = new("Boot configuration", "Everything related to booting in Windows", ConfigurationCategory.Advanced),
                ["FileExplorerSubMenu"] = new("File Explorer customization", "Everything related to customizing the Windows File Explorer", ConfigurationCategory.Interface),
                ["DriverConfigurationSubMenu"] = new("Driver configuration", "Everything related to driver configuration", ConfigurationCategory.Advanced),
                ["CoreIsolationSubMenu"] = new("Core Isolation (VBS)", "Everything related to core isolation", ConfigurationCategory.Security),
                ["DefenderSubMenu"] = new("Defender", "Everything related to Windows Defender", ConfigurationCategory.Security),
                ["MitigationsSubMenu"] = new("Mitigations", "Everything related to mitigations", ConfigurationCategory.Security),
                ["TroubleshootingNetwork"] = new("Network", "Everything related to troubleshooting network", ConfigurationCategory.Troubleshooting),
                ["FileSharingSubMenu"] = new("File Sharing", "Everything related to file sharing in Windows", ConfigurationCategory.General),
            };
            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationSubMenuViewModel>>(provider =>
                {
                    List<ConfigurationSubMenuViewModel> viewModels = new();
                    foreach (KeyValuePair<string, ConfigurationSubMenu> subMenu in configurationDictionary)
                    {
                        ObservableCollection<ConfigurationItemViewModel> itemViewModels = new ObservableCollection<ConfigurationItemViewModel>(provider.GetServices<ConfigurationItemViewModel>().Where(item => item.Category.ToString() == subMenu.Key));
                        ObservableCollection<MultiOptionConfigurationItemViewModel> multiOptionItemViewModels = new ObservableCollection<MultiOptionConfigurationItemViewModel>(provider.GetServices<MultiOptionConfigurationItemViewModel>().Where(item => item.Type.ToString() == subMenu.Key));
                        ObservableCollection<LinksViewModel> linksViewModel = new ObservableCollection<LinksViewModel>(provider.GetServices<LinksViewModel>().Where(item => item.ConfigurationType.ToString() == subMenu.Key));
                        ObservableCollection<ConfigurationSubMenuViewModel> configurationSubMenuViewModels = new ObservableCollection<ConfigurationSubMenuViewModel>(viewModels.Where(item => item.Type.ToString() == subMenu.Key));
                        ObservableCollection<ConfigurationButtonViewModel> configurationButtonViewModels = new ObservableCollection<ConfigurationButtonViewModel>(provider.GetServices<ConfigurationButtonViewModel>().Where(item => item.Type.ToString() == subMenu.Key));

                        ConfigurationSubMenuViewModel viewModel = CreateConfigurationSubMenuViewModel(provider, itemViewModels, multiOptionItemViewModels, linksViewModel, subMenu.Key, subMenu.Value, configurationSubMenuViewModels, configurationButtonViewModels);
                        viewModels.Add(viewModel);
                    }
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
                ["ContextMenuTerminals"] = new("Add or remove terminals from the context menu", "ContextMenuTerminals", ConfigurationCategory.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["ShortcutIcon"] = new("Change the icon from shortcuts", "ShortcutIcon", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["Mitigations"] = new("Change mitigations status", "Mitigations", ConfigurationCategory.MitigationsSubMenu, RiskRating.MediumRisk),
                ["SafeMode"] = new("Enter safe mode on startup", "SafeMode", ConfigurationCategory.Troubleshooting, RiskRating.MediumRisk),
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
                ["Animations"] = new ("Animations", "Animations", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["ExtractContextMenu"] = new("\"Extract\" option in the context menu", "ExtractContextMenu", ConfigurationCategory.ContextMenuSubMenu, RiskRating.LowRisk),
                ["RunWithPriority"] = new("Run With Priority in context menu", "RunWithPriority", ConfigurationCategory.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["Bluetooth"] = new("Bluetooth", "Bluetooth", ConfigurationCategory.ServicesSubMenu, RiskRating.HighRisk),
                ["LanmanWorkstation"] = new("Lanman Workstation (SMB)", "LanmanWorkstation", ConfigurationCategory.ServicesSubMenu, RiskRating.HighRisk),
                ["NetworkDiscovery"] = new("Network Discovery", "NetworkDiscovery", ConfigurationCategory.ServicesSubMenu, RiskRating.HighRisk),
                ["Printing"] = new("Printing", "Printing", ConfigurationCategory.ServicesSubMenu, RiskRating.LowRisk),
                ["NvidiaDispayContainer"] = new("NVIDIA Display Container", "NvidiaDispayContainer", ConfigurationCategory.NvidiaDisplayContainerSubMenu, RiskRating.HighRisk),
                ["AddNvidiaDisplayContainerContextMenu"] = new("NVIDIA Display Container in context menu", "AddNvidiaDisplayContainerContextMenu", ConfigurationCategory.NvidiaDisplayContainerSubMenu, RiskRating.HighRisk),
                ["CpuIdleContextMenu"] = new("CPU Idle toggle in context menu", "CpuIdleContextMenu", ConfigurationCategory.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["LockScreen"] = new("Enable the lock screen", "LockScreen", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["ShortcutText"] = new("Shortcut Text", "ShortcutText", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["BootLogo"] = new("Boot Logo", "BootLogo", ConfigurationCategory.BootConfigAppearance, RiskRating.LowRisk),
                ["BootMessages"] = new("Boot Messages", "BootMessages", ConfigurationCategory.BootConfigAppearance, RiskRating.LowRisk),
                ["NewBootMenu"] = new("New Boot Menu", "NewBootMenu", ConfigurationCategory.BootConfigAppearance, RiskRating.LowRisk),
                ["SpinningAnimation"] = new("Spinning Animation", "SpinningAnimations", ConfigurationCategory.BootConfigAppearance, RiskRating.LowRisk),
                ["AdvancedBootOptions"] = new("Advanced Boot Options on Startup", "AdvancedBootOptions", ConfigurationCategory.BootConfigBehavior, RiskRating.MediumRisk),
                ["AutomaticRepair"] = new("Automatic Repair", "AutomaticRepair", ConfigurationCategory.BootConfigBehavior, RiskRating.MediumRisk),
                ["KernelParameters"] = new("Kernel Parameters on Startup", "KernelParameters", ConfigurationCategory.BootConfigBehavior, RiskRating.LowRisk),
                ["HighestMode"] = new("Highest Mode", "HighestMode", ConfigurationCategory.BootConfigBehavior, RiskRating.LowRisk),
                ["CompactView"] = new("Compact View", "CompactView", ConfigurationCategory.FileExplorerSubMenu, RiskRating.LowRisk),
                ["RemovableDrivesInSidebar"] = new("Removable Drives in Sidebar", "RemovableDrivesInSidebar", ConfigurationCategory.FileExplorerSubMenu, RiskRating.MediumRisk),
                ["BackgroundApps"] = new("Background apps", "BackgroundApps", ConfigurationCategory.General, RiskRating.MediumRisk),
                ["SearchIndexing"] = new("Search Indexing", "SearchIndexing", ConfigurationCategory.General, RiskRating.MediumRisk),
                ["FsoAndGameBar"] = new("FSO and Game Bar", "FsoAndGameBar", ConfigurationCategory.General, RiskRating.LowRisk),
                ["AutomaticUpdates"] = new("Automatic updates", "AutomaticUpdates", ConfigurationCategory.General, RiskRating.HighRisk),
                ["DeliveryOptimisation"] = new("Delivery optimisation", "DeliveryOptimisation", ConfigurationCategory.General, RiskRating.LowRisk),
                ["Hibernation"] = new("Hibernation", "Hibernation", ConfigurationCategory.General, RiskRating.LowRisk),
                ["Location"] = new("Location", "Location", ConfigurationCategory.General, RiskRating.LowRisk),
                ["PhoneLink"] = new("Phone link and mobile devices", "PhoneLink", ConfigurationCategory.General, RiskRating.LowRisk),
                ["PowerSaving"] = new("Power saving", "PowerSaving", ConfigurationCategory.General, RiskRating.LowRisk),
                ["Sleep"] = new("Sleep", "Sleep", ConfigurationCategory.General, RiskRating.LowRisk),
                ["SystemRestore"] = new("System Restore", "SystemRestore", ConfigurationCategory.General, RiskRating.HighRisk),
                ["UpdateNotifications"] = new("Update Notifications", "UpdateNotifications", ConfigurationCategory.General, RiskRating.LowRisk),
                ["WebSearch"] = new("Start Menu Web Search", "WebSearch", ConfigurationCategory.General, RiskRating.HighRisk),
                ["Widgets"] = new("Desktop widgets", "Widgets", ConfigurationCategory.General, RiskRating.LowRisk),
                ["WindowsSpotlight"] = new("Windows Spotlight", "WindowsSpotlight", ConfigurationCategory.General, RiskRating.HighRisk),
                ["AppStoreArchiving"] = new("Microsoft Store archiving", "AppStoreArchiving", ConfigurationCategory.General, RiskRating.HighRisk),
                ["TakeOwnership"] = new("Add \"Take Ownership\" in the context menu", "TakeOwnership", ConfigurationCategory.ContextMenuSubMenu, RiskRating.HighRisk),
                ["OldContextMenu"] = new("Legacy context menu (pre-Windows 11)", "OldContextMenu", ConfigurationCategory.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["EdgeSwipe"] = new("Edge Swipe", "EdgeSwipe", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["AppIconsThumbnail"] = new("App icons on thumbnails", "AppIconsThumbnail", ConfigurationCategory.FileExplorerSubMenu, RiskRating.MediumRisk),
                ["AutomaticFolderDiscovery"] = new("Automatic folder discovery", "AutomaticFolderDiscovery", ConfigurationCategory.FileExplorerSubMenu, RiskRating.LowRisk),
                ["Gallery"] = new("Enable the gallery", "Gallery", ConfigurationCategory.FileExplorerSubMenu, RiskRating.LowRisk),
                ["SnapLayout"] = new("Enables snap layouts for windows", "SnapLayout", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["RecentItems"] = new("Unlocks recent items on file explorer", "RecentItems", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["VerboseStatusMessage"] = new("Verbose status messages", "VerboseStatusMessage", ConfigurationCategory.Interface, RiskRating.LowRisk),
                ["SuperFetch"] = new("SuperFetch", "SuperFetch", ConfigurationCategory.ServicesSubMenu, RiskRating.HighRisk),
                ["StaticIp"] = new("Automatically set static IP", "StaticIp", ConfigurationCategory.Advanced, RiskRating.MediumRisk),
                ["HideAppBrowserControl"] = new("Hide app browser control", "HideAppBrowserControl", ConfigurationCategory.DefenderSubMenu, RiskRating.HighRisk),
                ["SecurityHealthTray"] = new("Security Health button in the task bar tray", "SecurityHealthTray", ConfigurationCategory.DefenderSubMenu, RiskRating.MediumRisk),
                ["FaultTolerantHeap"] = new("Fault Tolerant Heap", "FaultTolerantHeap", ConfigurationCategory.MitigationsSubMenu, RiskRating.MediumRisk),
                ["Copilot"] = new("Enable Microsoft Copilot", "Copilot", ConfigurationCategory.AiSubMenu, RiskRating.HighRisk),
                ["Recall"] = new("Enable Windows recall", "recall", ConfigurationCategory.AiSubMenu, RiskRating.HighRisk),
                ["CpuIdle"] = new("Enable CPU Idling", "CpuIdle", ConfigurationCategory.General, RiskRating.HighRisk),
                ["ProcessExplorer"] = new("Install Process Explorer", "ProcessExplorer", ConfigurationCategory.Advanced, RiskRating.MediumRisk),
                ["VbsState"] = new("Enable VBS", "VbsState", ConfigurationCategory.CoreIsolationSubMenu, RiskRating.HighRisk),
                ["GiveAccessToMenu"] = new("Give access to menu", "GiveAccessToMenu", ConfigurationCategory.FileSharingSubMenu, RiskRating.HighRisk),
                ["NetworkNavigationPane"] = new("Network navigation pane", "NetworkNavigationPane", ConfigurationCategory.FileSharingSubMenu, RiskRating.HighRisk),
                ["FileSharing"] = new("File Sharing", "FileSharing", ConfigurationCategory.FileSharingSubMenu, RiskRating.HighRisk),
            };

            host.ConfigureServices((_,services) =>
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

            ConfigurationSubMenuViewModel  viewModel = new(
               configuration, configurationStoreSubMenu, configurationItemViewModels, multiOptionConfigurationItemViewModel, linksViewModel, configurationSubMenuViewModel, configurationButtonViewModels);

            return viewModel;
        }
        #endregion Create ViewModels
    }
}
