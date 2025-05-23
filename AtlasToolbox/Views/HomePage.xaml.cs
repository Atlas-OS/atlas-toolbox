﻿using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.AtlasToolboxVtableClasses;

namespace AtlasToolbox.Views
{
    public sealed partial class HomePage : Page
    {
        private HomePageViewModel _viewModel;

        public HomePage()
        {
            OperatingSystem os = Environment.OSVersion;

            RecentTogglesHelper.LoadRecentToggles();
            this.InitializeComponent();
            _viewModel = App._host.Services.GetRequiredService<HomePageViewModel>();
            this.DataContext = _viewModel;
            LoadText();

            List<object> list = new();

            for (int i = 0; i < 9; i++)
            {
                try
                {
                    list.Add(RecentTogglesHelper.recentToggles[i]);
                    if (RecentTogglesHelper.recentToggles.Count == i + 1) i = 10; 

                }catch {
                    break;
                }
            }
            RecentTogglesList.ItemsSource = list;
            ProfilesListView.ItemsSource = _viewModel.ProfilesList;
            ProfilesListView.SelectedItem = _viewModel.ProfileSelected;
        }

        private void LoadText()
        {
            // Home Header
            WinVer.Text = App.GetValueFromItemList("Home_WinVer") + ": " + RegistryHelper.GetValue("HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "DisplayVersion").ToString();
            AtlasVer.Text = App.GetValueFromItemList("Home_PlaybookVer") + ": " + RegistryHelper.GetValue("HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "RegisteredOrganization").ToString();
            HomeHeaderText.Text = App.GetValueFromItemList("Home_HeaderText");

            //Other
            ProfilesHeader.Text = App.GetValueFromItemList("Home_ProfilesText");
            RecentTogglesHeader.Text = App.GetValueFromItemList("Home_RecentTogglesText");
            NewProfileButton.Content = App.GetValueFromItemList("NewProfilesButton");
        }

        /// <summary>
        /// Deletes the profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteProfile(object sender, RoutedEventArgs e)
        {
            if (ProfilesListView.SelectedItem != null)
            {
                var selectedItem = ProfilesListView.SelectedItem as Profiles;

                if (selectedItem.Key != "default.json")
                {
                    ContentDialog dialog = new ContentDialog();

                    dialog.XamlRoot = this.XamlRoot;
                    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    dialog.Title = App.GetValueFromItemList("DeleteProfileConfirmation");
                    dialog.PrimaryButtonText = App.GetValueFromItemList("Yes");
                    dialog.CloseButtonText = App.GetValueFromItemList("Cancel");
                    dialog.DefaultButton = ContentDialogButton.Primary;
                    dialog.PrimaryButtonCommand = _viewModel.RemoveProfileCommand;

                    var result = await dialog.ShowAsync();
                }
                else
                {
                    ContentDialog dialog = new ContentDialog();

                    dialog.XamlRoot = this.XamlRoot;
                    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    dialog.Title = App.GetValueFromItemList("TryDeleteDefaultProfile");
                    dialog.CloseButtonText = App.GetValueFromItemList("Ok");
                    dialog.DefaultButton = ContentDialogButton.Primary;

                    var result = await dialog.ShowAsync();
                }
            }
        }


        private async void SetProfile_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = App.GetValueFromItemList("Home_SetProfileConfirm");
            dialog.PrimaryButtonText = App.GetValueFromItemList("Yes"); ;
            dialog.CloseButtonText = App.GetValueFromItemList("No"); ;
            dialog.DefaultButton = ContentDialogButton.Primary;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                RestartPCPrompt();
                _viewModel.SetProfileCommand.Execute(this);
            }
        }

        /// <summary>
        /// Prompts the user to restart their PC
        /// </summary>
        private async void RestartPCPrompt()
        {
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = App.GetValueFromItemList("RestartPCPromptHeader");
            dialog.PrimaryButtonText = App.GetValueFromItemList("Restart"); ;
            dialog.CloseButtonText = App.GetValueFromItemList("Later"); ;
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.PrimaryButtonCommand = new RelayCommand(ComputerStateHelper.RestartComputer);

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ComputerStateHelper.RestartComputer();
            }
        }

        private async void NewProfile()
        {
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = App.GetValueFromItemList("NewProfilesButton");
            dialog.PrimaryButtonText = App.GetValueFromItemList("Create");
            dialog.CloseButtonText = App.GetValueFromItemList("Cancel");
            dialog.Content = new NewProfilePage(_viewModel);
            dialog.DefaultButton = ContentDialogButton.Primary;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                _viewModel.AddProfileCommand.Execute(null);
            }
            Name = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewProfile();
        }

        private void SetProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuFlyoutItem;
            button.Text = App.GetValueFromItemList("Home_SetProfileBtn");
        }

        private void DeleteProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuFlyoutItem;
            button.Text = App.GetValueFromItemList("Home_DeleteProfileBtn");
        }
    }
}
