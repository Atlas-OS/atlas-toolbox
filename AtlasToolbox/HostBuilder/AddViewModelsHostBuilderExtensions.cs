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



        private static IHostBuilder AddProfiles(this IHostBuilder host)
        {
            List<Profiles> configurationDictionary = new List<Profiles>();

            DirectoryInfo profilesDirectory = new DirectoryInfo($"{Environment.GetEnvironmentVariable("windir")}\\AtlasModules\\Toolbox\\Profiles");
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
            return host;
        }

        private static IHostBuilder AddLinksItemViewModels(this IHostBuilder host)
        {
            Dictionary<string, Links> configurationDictionary = new()
            {
                ["ExplorerPatcher"] = new ("https://github.com/valinet/ExplorerPatcher", "ExplorerPatcher", ConfigurationType.StartMenuSubMenu),
                ["StartAllBack"] = new ("https://www.startallback.com/", "StartAllBack", ConfigurationType.StartMenuSubMenu),
                ["OpenShellAtlasPreset"] = new (@"http://github.com/Atlas-OS/Atlas/blob/main/src/playbook/Executables/AtlasDesktop/4.%20Interface%20Tweaks/Start%20Menu/Atlas%20Open-Shell%20Preset.xml", "Open Shell AtlasOS preset", ConfigurationType.StartMenuSubMenu),
                ["InterfaceTweaksDocumentation"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/interface-tweaks/", "Interface tweaks documentation", ConfigurationType.Interface),
                
                ["ActivationPage"] = new (@"ms-settings:activation", "Windows activation status", ConfigurationType.Windows, "\uE713"),
                ["ColorsPage"] = new (@"ms-settings:personalization-colors", "Color personalisation settings", ConfigurationType.Windows, "\uE713"),
                ["DateAndTime"] = new (@"ms-settings:dateandtime", "Date and time settings", ConfigurationType.Windows, "\uE713"),
                ["DefaultApps"] = new (@"ms-settings:defaultapps", "Default Apps", ConfigurationType.Windows, "\uE713"),
                ["DefaultGraphicsSettings"] = new (@"ms-settings:display-advancedgraphics-default", "DefaultGraphicsSettings", ConfigurationType.Windows, "\uE713"),
                ["RegionLanguage"] = new (@"ms-settings:regionlanguage", "Region Properties", ConfigurationType.Windows, "\uE713"),
                ["Privacy"] = new (@"ms-settings:privacy", "Privacy Settings", ConfigurationType.Windows, "\uE713"),
                ["RegionProperties"] = new (@"C:\Windows\System32\rundll32.exe C:\Windows\System32\shell32.dll,Control_RunDLL C:\Windows\System32\intl.cpl", "RegionProperties", ConfigurationType.Windows, "\uE713"),
                ["Taskbar"] = new (@"ms-settings:taskbar", "Taskbar settings", ConfigurationType.Windows, "\uE713"),
                ["CoreIsolation"] = new (@"windowsdefender://coreisolation/", "Core Isolation - Windows Security", ConfigurationType.CoreIsolationSubMenu, "\uE83D"),


                ["WindowsSettingsDocumentation"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/windows-settings/", "Windows Settings Documentation", ConfigurationType.Windows),
                ["BootConfigExplanations"] = new (@"https://learn.microsoft.com/windows-hardware/drivers/devtest/bcdedit--set", "Explanations from Microsoft", ConfigurationType.BootConfigurationSubMenu),
                ["AutoGpuAffinity"] = new (@"https://github.com/valleyofdoom/AutoGpuAffinity", "AutoGpuAffinity", ConfigurationType.DriverConfigurationSubMenu),
                ["GoInterruptPolicy"] = new (@"https://github.com/spddl/GoInterruptPolicy", "GoInterruptPolicy", ConfigurationType.DriverConfigurationSubMenu),
                ["InterrupAffinityTool"] = new (@"https://www.techpowerup.com/download/microsoft-interrupt-affinity-tool", "Interrupt Affinity Tool", ConfigurationType.DriverConfigurationSubMenu),
                ["MSIUtilityV3"] = new (@"https://forums.guru3d.com/threads/windows-line-based-vs-message-signaled-based-interrupts-msi-tool.378044", "MSI Utility V3", ConfigurationType.DriverConfigurationSubMenu),
                ["ProcessExplorer"] = new (@"https://learn.microsoft.com/en-us/sysinternals/downloads/process-explorer", "Process Explorer", ConfigurationType.Advanced),
                ["NvidiaDisplayContainerMustReadFirst"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/#nvidia-display-container", "Must read first", ConfigurationType.NvidiaDisplayContainerSubMenu),
                ["AdvancedConfigMustRead"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/advanced-configuration/", "Must read first (Documentation)", ConfigurationType.Advanced),
                ["SecurityDocumentation"] = new (@"https://docs.atlasos.net/getting-started/post-installation/atlas-folder/security/", "Security documentation", ConfigurationType.Security),
                ["ResetPC"] = new (@"https://docs.atlasos.net/getting-started/reverting-atlas/", "Reset this PC (read first)", ConfigurationType.Troubleshooting),
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

        private static IHostBuilder AddConfigurationButtonItemViewModels(this IHostBuilder host)
        {
            ICommand buttonCommand;
            Dictionary<string, ConfigurationButton> configurationDictionary = new()
            {
                ["RestartExplorerButton"] = new(buttonCommand = new RestartExplorerCommand(), "Restart Explorer.exe", "Some interface settings may require you to restart explorer.exe", ConfigurationType.Interface),
                ["ViewCurrentSettingsBootConfig"] = new(buttonCommand = new ViewCurrentValuesCommand(), "View current values", "See boot configuration values", ConfigurationType.BootConfigurationSubMenu),
                ["VBSCurrentConfig"] = new(buttonCommand = new CurrentVBSConfigurationCommand(), "VBS Current Configuration", "See the current VBS configuration", ConfigurationType.CoreIsolationSubMenu),
                ["ToggleDefender"] = new(buttonCommand = new ToggleDefenderCommand(), "Toggle", "Toggle Windows Defender", ConfigurationType.DefenderSubMenu),
                ["ResetFTH"] = new(buttonCommand = new ResetFTHCommand(), "Reset", "Reset FTH entries", ConfigurationType.MitigationsSubMenu),
                ["InstallOpenShell"] = new(buttonCommand = new InstallOpenShellCommand(), "Install OpenShell", "Install", ConfigurationType.StartMenuSubMenu),

                ["FixErrors"] = new(buttonCommand = new FixErrorsCommand(), "Troubleshoot", "Fix Errors 2502 and 2503", ConfigurationType.Troubleshooting),
                ["RepairWinComponent"] = new(buttonCommand = new RepairWindowsComponentsCommand(), "Troubleshoot", "Repair Windows Components", ConfigurationType.Troubleshooting),
                ["TelemetryComponents"] = new(buttonCommand = new TelemetryComponentsCommand(), "Troubleshoot", "Telemetry Components", ConfigurationType.Troubleshooting),
                ["AtlasDefault"] = new(buttonCommand = new NetworkAtlasDefaults(), "Reset", "Reset Network to Atlas Defaults", ConfigurationType.TroubleshootingNetwork),
                ["WindowsDefault"] = new(buttonCommand = new NetworkWindowsDefaults(), "Reset", "Reset Network to Windows Defaults", ConfigurationType.TroubleshootingNetwork),
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

        private static IHostBuilder AddConfigurationSubMenu(this IHostBuilder host)
        {
            // TODO: Change configuration types
            Dictionary<string, ConfigurationSubMenu> configurationDictionary = new()
            {
                ["BootConfigAppearance"] = new("Boot configuration appearance", "Everything related to the appearance of booting Windows", ConfigurationType.BootConfigurationSubMenu),
                ["BootConfigBehavior"] = new("Boot behavior", "Everything related to booting behavior", ConfigurationType.BootConfigurationSubMenu),
                ["NvidiaDisplayContainerSubMenu"] = new("NVIDIA Display Container", "Everything related to the NVIDIA Display Container", ConfigurationType.ServicesSubMenu),

                ["StartMenuSubMenu"] = new("Start Menu", "Everything related to customizing the Windows Start Menu", ConfigurationType.Interface),
                ["ContextMenuSubMenu"] = new("Context Menu", "Everything related to the context menu", ConfigurationType.Interface),
                ["AiSubMenu"] = new("AI Features", "Everything related to AI features in Windows 11", ConfigurationType.General),
                ["ServicesSubMenu"] = new("Services", "Everything related to services in Windows", ConfigurationType.Advanced),
                ["BootConfigurationSubMenu"] = new("Boot configuration", "Everything related to booting in Windows", ConfigurationType.Advanced),
                ["FileExplorerSubMenu"] = new("File Explorer customization", "Everything related to customizing the Windows File Explorer", ConfigurationType.Interface),
                ["DriverConfigurationSubMenu"] = new("Driver configuration", "Everything related to driver configuration", ConfigurationType.Advanced),
                ["CoreIsolationSubMenu"] = new("Core Isolation (VBS)", "Everything related to core isolation", ConfigurationType.Security),
                ["DefenderSubMenu"] = new("Defender", "Everything related to Windows Defender", ConfigurationType.Security),
                ["MitigationsSubMenu"] = new("Mitigations", "Everything related to mitigations", ConfigurationType.Security),
                ["TroubleshootingNetwork"] = new("Network", "Everything related to troubleshooting network", ConfigurationType.Troubleshooting),
                ["FileSharingSubMenu"] = new("File Sharing", "Everything related to file sharing in Windows", ConfigurationType.General),
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

        private static IHostBuilder AddMultiOptionConfigurationViewModels(this IHostBuilder host)
        {
            // TODO: Change configuration types
            Dictionary<string, MultiOptionConfiguration> configurationDictionary = new()
            {
                ["ContextMenuTerminals"] = new("Add or remove terminals from the context menu", "ContextMenuTerminals", ConfigurationType.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["ShortcutIcon"] = new("Change the icon from shortcuts", "ShortcutIcon", ConfigurationType.Interface, RiskRating.LowRisk),
                ["Mitigations"] = new("Change mitigations status", "Mitigations", ConfigurationType.MitigationsSubMenu, RiskRating.MediumRisk),
                ["SafeMode"] = new("Enter safe mode on startup", "SafeMode", ConfigurationType.Troubleshooting, RiskRating.MediumRisk),
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

        private static IHostBuilder AddConfigurationItemViewModels(this IHostBuilder host)
        {
            // TODO: Change configuration types`
            Dictionary<string, Configuration> configurationDictionary = new()
            {
                ["Animations"] = new ("Animations", "Animations", ConfigurationType.Interface, RiskRating.LowRisk),
                ["ExtractContextMenu"] = new("\"Extract\" option in the context menu", "ExtractContextMenu", ConfigurationType.ContextMenuSubMenu, RiskRating.LowRisk),
                ["RunWithPriority"] = new("Run With Priority in context menu", "RunWithPriority", ConfigurationType.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["Bluetooth"] = new("Bluetooth", "Bluetooth", ConfigurationType.ServicesSubMenu, RiskRating.HighRisk),
                ["LanmanWorkstation"] = new("Lanman Workstation (SMB)", "LanmanWorkstation", ConfigurationType.ServicesSubMenu, RiskRating.HighRisk),
                ["NetworkDiscovery"] = new("Network Discovery", "NetworkDiscovery", ConfigurationType.ServicesSubMenu, RiskRating.HighRisk),
                ["Printing"] = new("Printing", "Printing", ConfigurationType.ServicesSubMenu, RiskRating.LowRisk),
                ["NvidiaDispayContainer"] = new("NVIDIA Display Container", "NvidiaDispayContainer", ConfigurationType.NvidiaDisplayContainerSubMenu, RiskRating.HighRisk),
                ["AddNvidiaDisplayContainerContextMenu"] = new("NVIDIA Display Container in context menu", "AddNvidiaDisplayContainerContextMenu", ConfigurationType.NvidiaDisplayContainerSubMenu, RiskRating.HighRisk),
                ["CpuIdleContextMenu"] = new("CPU Idle toggle in context menu", "CpuIdleContextMenu", ConfigurationType.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["LockScreen"] = new("Enable the lock screen", "LockScreen", ConfigurationType.Interface, RiskRating.LowRisk),
                ["ShortcutText"] = new("Shortcut Text", "ShortcutText", ConfigurationType.Interface, RiskRating.LowRisk),
                ["BootLogo"] = new("Boot Logo", "BootLogo", ConfigurationType.BootConfigAppearance, RiskRating.LowRisk),
                ["BootMessages"] = new("Boot Messages", "BootMessages", ConfigurationType.BootConfigAppearance, RiskRating.LowRisk),
                ["NewBootMenu"] = new("New Boot Menu", "NewBootMenu", ConfigurationType.BootConfigAppearance, RiskRating.LowRisk),
                ["SpinningAnimation"] = new("Spinning Animation", "SpinningAnimations", ConfigurationType.BootConfigAppearance, RiskRating.LowRisk),
                ["AdvancedBootOptions"] = new("Advanced Boot Options on Startup", "AdvancedBootOptions", ConfigurationType.BootConfigBehavior, RiskRating.MediumRisk),
                ["AutomaticRepair"] = new("Automatic Repair", "AutomaticRepair", ConfigurationType.BootConfigBehavior, RiskRating.MediumRisk),
                ["KernelParameters"] = new("Kernel Parameters on Startup", "KernelParameters", ConfigurationType.BootConfigBehavior, RiskRating.LowRisk),
                ["HighestMode"] = new("Highest Mode", "HighestMode", ConfigurationType.BootConfigBehavior, RiskRating.LowRisk),
                ["CompactView"] = new("Compact View", "CompactView", ConfigurationType.FileExplorerSubMenu, RiskRating.LowRisk),
                ["RemovableDrivesInSidebar"] = new("Removable Drives in Sidebar", "RemovableDrivesInSidebar", ConfigurationType.FileExplorerSubMenu, RiskRating.MediumRisk),
                ["BackgroundApps"] = new("Background apps", "BackgroundApps", ConfigurationType.General, RiskRating.MediumRisk),
                ["SearchIndexing"] = new("Search Indexing", "SearchIndexing", ConfigurationType.General, RiskRating.MediumRisk),
                ["FsoAndGameBar"] = new("FSO and Game Bar", "FsoAndGameBar", ConfigurationType.General, RiskRating.LowRisk),
                ["AutomaticUpdates"] = new("Automatic updates", "AutomaticUpdates", ConfigurationType.General, RiskRating.HighRisk),
                ["DeliveryOptimisation"] = new("Delivery optimisation", "DeliveryOptimisation", ConfigurationType.General, RiskRating.LowRisk),
                ["Hibernation"] = new("Hibernation", "Hibernation", ConfigurationType.General, RiskRating.LowRisk),
                ["Location"] = new("Location", "Location", ConfigurationType.General, RiskRating.LowRisk),
                ["PhoneLink"] = new("Phone link and mobile devices", "PhoneLink", ConfigurationType.General, RiskRating.LowRisk),
                ["PowerSaving"] = new("Power saving", "PowerSaving", ConfigurationType.General, RiskRating.LowRisk),
                ["Sleep"] = new("Sleep", "Sleep", ConfigurationType.General, RiskRating.LowRisk),
                ["SystemRestore"] = new("System Restore", "SystemRestore", ConfigurationType.General, RiskRating.HighRisk),
                ["UpdateNotifications"] = new("Update Notifications", "UpdateNotifications", ConfigurationType.General, RiskRating.LowRisk),
                ["WebSearch"] = new("Start Menu Web Search", "WebSearch", ConfigurationType.General, RiskRating.HighRisk),
                ["Widgets"] = new("Desktop widgets", "Widgets", ConfigurationType.General, RiskRating.LowRisk),
                ["WindowsSpotlight"] = new("Windows Spotlight", "WindowsSpotlight", ConfigurationType.General, RiskRating.HighRisk),
                ["AppStoreArchiving"] = new("Microsoft Store archiving", "AppStoreArchiving", ConfigurationType.General, RiskRating.HighRisk),
                ["TakeOwnership"] = new("Add \"Take Ownership\" in the context menu", "TakeOwnership", ConfigurationType.ContextMenuSubMenu, RiskRating.HighRisk),
                ["OldContextMenu"] = new("Legacy context menu (pre-Windows 11)", "OldContextMenu", ConfigurationType.ContextMenuSubMenu, RiskRating.MediumRisk),
                ["EdgeSwipe"] = new("Edge Swipe", "EdgeSwipe", ConfigurationType.Interface, RiskRating.LowRisk),
                ["AppIconsThumbnail"] = new("App icons on thumbnails", "AppIconsThumbnail", ConfigurationType.FileExplorerSubMenu, RiskRating.MediumRisk),
                ["AutomaticFolderDiscovery"] = new("Automatic folder discovery", "AutomaticFolderDiscovery", ConfigurationType.FileExplorerSubMenu, RiskRating.LowRisk),
                ["Gallery"] = new("Enable the gallery", "Gallery", ConfigurationType.FileExplorerSubMenu, RiskRating.LowRisk),
                ["SnapLayout"] = new("Enables snap layouts for windows", "SnapLayout", ConfigurationType.Interface, RiskRating.LowRisk),
                ["RecentItems"] = new("Unlocks recent items on file explorer", "RecentItems", ConfigurationType.Interface, RiskRating.LowRisk),
                ["VerboseStatusMessage"] = new("Verbose status messages", "VerboseStatusMessage", ConfigurationType.Interface, RiskRating.LowRisk),
                ["SuperFetch"] = new("SuperFetch", "SuperFetch", ConfigurationType.ServicesSubMenu, RiskRating.HighRisk),
                ["StaticIp"] = new("Automatically set static IP", "StaticIp", ConfigurationType.Advanced, RiskRating.MediumRisk),
                ["HideAppBrowserControl"] = new("Automatically set static IP", "StaticIp", ConfigurationType.DefenderSubMenu, RiskRating.HighRisk),
                ["SecurityHealthTray"] = new("Automatically set static IP", "StaticIp", ConfigurationType.DefenderSubMenu, RiskRating.MediumRisk),
                ["FaultTolerantHeap"] = new("Fault Tolerant Heap", "FaultTolerantHeap", ConfigurationType.MitigationsSubMenu, RiskRating.MediumRisk),
                ["Copilot"] = new("Enable Microsoft Copilot", "Copilot", ConfigurationType.AiSubMenu, RiskRating.HighRisk),
                ["Recall"] = new("Enable Windows recall", "recall", ConfigurationType.AiSubMenu, RiskRating.HighRisk),
                ["CpuIdle"] = new("Enable CPU Idling", "CpuIdle", ConfigurationType.General, RiskRating.HighRisk),
                ["ProcessExplorer"] = new("Install Process Explorer", "ProcessExplorer", ConfigurationType.Advanced, RiskRating.MediumRisk),
                ["VbsState"] = new("Enable VBS", "VbsState", ConfigurationType.CoreIsolationSubMenu, RiskRating.HighRisk),
                ["GiveAccessToMenu"] = new("Give access to menu", "GiveAccessToMenu", ConfigurationType.FileSharingSubMenu, RiskRating.HighRisk),
                ["NetworkNavigationPane"] = new("Network navigation pane", "NetworkNavigationPane", ConfigurationType.FileSharingSubMenu, RiskRating.HighRisk),
                ["FileSharing"] = new("File Sharing", "FileSharing", ConfigurationType.FileSharingSubMenu, RiskRating.HighRisk),
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
        #endregion Create ViewModels

        private static ConfigurationSubMenuViewModel CreateConfigurationSubMenuViewModel(
          IServiceProvider serviceProvider, ObservableCollection<ConfigurationItemViewModel> configurationItemViewModels, ObservableCollection<MultiOptionConfigurationItemViewModel> multiOptionConfigurationItemViewModel, ObservableCollection<LinksViewModel> linksViewModel, object key, ConfigurationSubMenu configuration, ObservableCollection<ConfigurationSubMenuViewModel> configurationSubMenuViewModel, ObservableCollection<ConfigurationButtonViewModel> configurationButtonViewModels)
        {
            ConfigurationStoreSubMenu configurationStoreSubMenu = serviceProvider.GetRequiredKeyedService<ConfigurationStoreSubMenu>(key);

            ConfigurationSubMenuViewModel  viewModel = new(
               configuration, configurationStoreSubMenu, configurationItemViewModels, multiOptionConfigurationItemViewModel, linksViewModel, configurationSubMenuViewModel, configurationButtonViewModels);

            return viewModel;
        }
    }
}
