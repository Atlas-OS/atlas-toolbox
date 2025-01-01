﻿using AtlasToolbox.Models;
using AtlasToolbox.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AtlasToolbox.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private HomePageViewModel _viewModel;

        public HomePage()
        {
            this.InitializeComponent();
            _viewModel = App._host.Services.GetRequiredService<HomePageViewModel>();
            this.DataContext = _viewModel;

            ProfilesListView.ItemsSource = _viewModel.ProfilesList;
            ProfilesListView.SelectedItem = _viewModel.ProfileSelected;
        }

        private void AddProfile(object sender, RoutedEventArgs e)
        {
            _viewModel.AddProfileCommand.Execute(null);
            ProfileNameTextBox.Text = "";
        }

        private async void DeleteProfile(object sender, RoutedEventArgs e)
        {
            if (ProfilesListView.SelectedItem != null)
            {
                var selectedItem = ProfilesListView.SelectedItem as Profiles;

                if (selectedItem.Key != "default.txt")
                {
                    ContentDialog dialog = new ContentDialog();

                    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                    dialog.XamlRoot = this.XamlRoot;
                    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    dialog.Title = "Do you really wish to delete this profile?";
                    dialog.PrimaryButtonText = "Yes";
                    dialog.CloseButtonText = "Cancel";
                    dialog.DefaultButton = ContentDialogButton.Primary;
                    dialog.PrimaryButtonCommand = _viewModel.RemoveProfileCommand;

                    var result = await dialog.ShowAsync();
                }
                else
                {
                    ContentDialog dialog = new ContentDialog();

                    // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                    dialog.XamlRoot = this.XamlRoot;
                    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    dialog.Title = "You cannot delete the default profile.";
                    dialog.CloseButtonText = "Ok";
                    dialog.DefaultButton = ContentDialogButton.Primary;

                    var result = await dialog.ShowAsync();
                }
            }
        }


        private async void ContactDeleteMenuyItem_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Do you really wish to set this profile?";
            dialog.PrimaryButtonText = "Yes";
            dialog.CloseButtonText = "No";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.PrimaryButtonCommand = _viewModel.SetProfileCommand;

            var result = await dialog.ShowAsync();
        }
        private void ProfileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.Name = ProfileNameTextBox.Text;
        }
    }
}
