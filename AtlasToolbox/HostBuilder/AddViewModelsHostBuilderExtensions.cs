﻿using AtlasToolbox.Services.ConfigurationServices;
using AtlasToolbox.Enums;
using AtlasToolbox.Models;
using AtlasToolbox.Services;
using AtlasToolbox.Stores;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using Windows.Services.Maps;
using Windows.Security.Cryptography.Core;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace AtlasToolbox.HostBuilder
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices((_, services) =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddTransient(CreateConfigurationViewModel);
            });

            host.AddConfigurationItemViewModels();
            //host.AddConfigurationMenuItemsViewModels(); 

            return host;
        }

        //private static IHostBuilder AddConfigurationMenuItemsViewModels(this IHostBuilder host)
        //{
        //    // TODO: Change configuration types
        //    Dictionary<string, Configuration> configurationDictionary = new()
        //    {
        //        ["AppStoreArchiving"] = new("App Store Archiving", new AppStoreArchivingConfigurationService(), ConfigurationType.General),
        //    };
        //    host.ConfigureServices((_, services) =>
        //    {
        //        services.AddSingleton<IEnumerable<ConfigurationMenuItemsViewModel>>(provider =>
        //        {
        //            List<ConfigurationMenuItemsViewModel> viewModels = new();

        //            foreach (KeyValuePair<string, Configuration> item in configurationDictionary)
        //            {
        //                ConfigurationMenuItemsViewModel viewModel = CreateConfigurationMenuItemsViewModel(provider, item.Key, item.Value);
        //                viewModels.Add(viewModel);
        //            }
        //            return viewModels;
        //        });
        //    });

        //    return host;
        //}

        private static IHostBuilder AddConfigurationItemViewModels(this IHostBuilder host)
        {
            // TODO: Change configuration types
            Dictionary<string, Configuration> configurationDictionary = new()
            {
                ["Animations"] = new ("Animations", ConfigurationType.General),
                //["Bluetooth"] = new("Bluetooth", ConfigurationType.Performance),
                //["FsoAndGameBar"] = new("FSO and Game Bar", ConfigurationType.Performance),
                //["WindowsFirewall"] = new("Firewall", ConfigurationType.Privacy),
                //["GameMode"] = new("Game Mode", ConfigurationType.Performance),
                //["Hags"] = new("HAGS", ConfigurationType.Advanced),
                //["LanmanWorkstation"] = new("Lanman Workstation (SMB)", ConfigurationType.Customization),
                //["MicrosoftStore"] = new("Microsoft Store", ConfigurationType.Customization),
                //["NetworkDiscovery"] = new("Network Discovery", ConfigurationType.Customization),
                //["Notifications"] = new("Notifications", ConfigurationType.Customization),
                //["Printing"] = new("Printing", ConfigurationType.Customization),
                //["SearchIndexing"] = new("Search Indexing", ConfigurationType.Customization),
                //["Troubleshooting"] = new("Troubleshooting", ConfigurationType.Customization),
                //["Uwp"] = new("UWP", ConfigurationType.Customization),
                //["Vpn"] = new("VPN", ConfigurationType.Customization),
                //["ModernAltTab"] = new("Modern Alt-Tab", ConfigurationType.Customization),
                //["CpuIdleContextMenu"] = new("CPU Idle toggle in context menu", ConfigurationType.Customization),
                //["DarkTitlebars"] = new("Dark Titlebars", ConfigurationType.Customization),
                //["LockScreen"] = new("Lock Screen", ConfigurationType.Customization),
                //["ModernVolumeFlyout"] = new("Modern Volume Flyout", ConfigurationType.Customization),
                //["RunWithPriorityContextMenu"] = new("Run With Priority in context menu", ConfigurationType.Customization),
                //["ShortcutText"] = new("Shortcut Text", ConfigurationType.Customization),
                //["BootLogo"] = new("Boot Logo", ConfigurationType.Advanced),
                //["BootMessages"] = new("Boot Messages", ConfigurationType.Advanced),
                //["NewBootMenu"] = new("New Boot Menu", ConfigurationType.Advanced),
                //["SpinningAnimation"] = new("Spinning Animation", ConfigurationType.Advanced),
                //["AdvancedBootOptions"] = new("Advanced Boot Options on Startup", ConfigurationType.Advanced),
                //["AutomaticRepair"] = new("Automatic Repair", ConfigurationType.Advanced),
                //["KernelParameters"] = new("Kernel Parameters on Startup", ConfigurationType.Advanced),
                //["HighestMode"] = new("Highest Mode", ConfigurationType.Advanced),
                //["CompactView"] = new("Compact View", ConfigurationType.Advanced),
                //["QuickAccess"] = new("Quick Access", ConfigurationType.Advanced),
                //["RemovableDrivesInSidebar"] = new("Removable Drives in Sidebar", ConfigurationType.Advanced),
                ////["Copilot"] = new("Copilot feature", ConfigurationType.Privacy),
                //["AutomaticUpdates"] = new("Automatic updates", ConfigurationType.Advanced),
                //["BackgroundApps"] = new("Background apps", ConfigurationType.Advanced),
                //["DeliveryOptimization"] = new("Delivery optimization", ConfigurationType.Advanced),
                //["Hibernation"] = new("Hibernation", ConfigurationType.Advanced),
                //["Location"] = new("Location", ConfigurationType.Privacy),
                //["PhoneLink"] = new("Phone link and mobile devices", ConfigurationType.Privacy),
                //["PowerSaving"] = new("Power saving", ConfigurationType.Performance),
                //["Sleep"] = new("Sleep", ConfigurationType.Performance),
                //["AppStoreArchiving"] = new("Microsoft Store Archiving", ConfigurationType.Performance),
                //["SystemRestore"] = new("System Restore", ConfigurationType.Performance),
                //["UpdateNotifications"] = new("Update Notifications", ConfigurationType.Performance),
                //["WebSearch"] = new("Start Menu Web Search", ConfigurationType.Advanced),
                //["Widgets"] = new("Desktop widgets", ConfigurationType.Customization),
                //["WindowsSpotlight"] = new("Windows Spotlight", ConfigurationType.Customization),
                //["ExtractContextMenu"] = new("Extract context menu", ConfigurationType.ContextMenu),
                //["AppStoreArchiving"] = new("AppStoreArchiving", ConfigurationType.General),

            };

            host.ConfigureServices((_,services) =>
            {
                services.AddSingleton<IEnumerable<ConfigurationItemViewModel>>(provider =>
                {
                    List<ConfigurationItemViewModel> viewModels = new();

                    foreach (KeyValuePair<string, Configuration> item in configurationDictionary)
                    {
                        ConfigurationItemViewModel viewModel = CreateConfigurationItemViewModel(provider, item.Key, item.Value);
                        viewModels.Add(viewModel);
                    }
                    return viewModels;
                });
            });

            return host;
        }

        private static ConfigurationItemViewModel CreateConfigurationItemViewModel(
            IServiceProvider serviceProvider, object key, Configuration configuration)
        {
                ConfigurationStore configurationStore = serviceProvider.GetRequiredKeyedService<ConfigurationStore>(key);
                IConfigurationService configurationService = serviceProvider.GetRequiredKeyedService<IConfigurationService>(key);

                ConfigurationItemViewModel viewModel = new(
                    configuration, configurationStore, configurationService);

                return viewModel;
        }
        private static GeneralConfigViewModel CreateConfigurationViewModel(IServiceProvider serviceProvider)
        {
            return GeneralConfigViewModel.LoadViewModel(
                serviceProvider.GetServices<ConfigurationItemViewModel>(),
                serviceProvider.GetServices<ConfigurationItemMenuViewModel>());
        }

        //private static ConfigurationMenuItemsViewModel CreateConfigurationMenuItemsViewModel(
        //    IServiceProvider serviceProvider, object? key, Configuration configuration)
        //{
        //    ConfigurationMenu configurationMenu = serviceProvider.GetRequiredKeyedService<ConfigurationMenu>(key);
        //    IConfigurationMenu configurationService = serviceProvider.GetRequiredKeyedService<IConfigurationMenu>(key);

        //    ConfigurationMenuItemsViewModel viewModel = new(
        //        configuration, configurationMenu, configurationService);

        //    return viewModel;
        //}

        //public static ConfigurationMenuItemsMenuViewModel CreateConfigurationMenuItemsMenuViewModel(IServiceProvider serviceProvider, object? key)
        //{
        //    return ConfigurationMenuItemsMenuViewModel.LoadViewModel(
        //         serviceProvider.GetServices<ConfigurationItemViewModel>(),
        //         serviceProvider.GetServices<ConfigurationMenuItemsViewModel>(),
        //         serviceProvider.GetRequiredService<CloseModalNavigationService>());
        //}

        //private static HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
        //{
        //    INavigationService softwareNavigationService =
        //        CreateNavigationService<SoftwareViewModel>(serviceProvider);

        //    return new(
        //        softwareNavigationService,
        //        CreateNavigationService<DriversViewModel>(serviceProvider),
        //        CreateNavigationService<ConfigurationViewModel>(serviceProvider),
        //            CreateNoInternetNavigationService(serviceProvider, softwareNavigationService));
        //}

        public static ConfigurationItemMenuViewModel CreateConfigurationItemMenuViewModel(IServiceProvider serviceProvider, object key)
        {
            return new(
                serviceProvider.GetRequiredKeyedService<IConfigurationMenu>(key),
                serviceProvider.GetRequiredKeyedService<IEnumerable<ConfigurationItemViewModel>>(key));
        }
    }
}
