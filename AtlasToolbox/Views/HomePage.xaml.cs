using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels;
using AtlasToolbox.ViewModels.ConfigurationVM;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AtlasToolbox.Views
{
    public sealed partial class HomePage : Page
    {
        private HomePageViewModel _viewModel;
        private List<IConfigurationItem> _configurationItems;
        public HomePage()
        {
            OperatingSystem os = Environment.OSVersion;

            //RecentTogglesHelper.LoadRecentToggles();
            this.InitializeComponent();
            _viewModel = App._host.Services.GetRequiredService<HomePageViewModel>();
            this.DataContext = _viewModel;
            LoadText();
            LoadFavorites();
            this.SizeChanged += MainWindow_SizeChanged;

            ProfilesListView.ItemsSource = _viewModel.ProfilesList;
            ProfilesListView.SelectedItem = _viewModel.ProfileSelected;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualWidth <= 640)
            {
                Grid.SetRow(ProfilesPanel, 2);
                Grid.SetColumn(ProfilesPanel, 0);
                Grid.SetColumnSpan(FavoritesPanel, 2);
                ProfilesPanel.Margin = new Thickness{ Left = 36, Right = 5, Top = 0, Bottom = 0 }; 
            }
            if (this.ActualWidth >= 640)
            {
                Grid.SetRow(ProfilesPanel, 1);
                Grid.SetColumn(ProfilesPanel, 1);
                Grid.SetColumnSpan(FavoritesPanel, 1);
                ProfilesPanel.Margin = new Thickness { Left = 16, Right = 5, Top = 0, Bottom = 0 };
            }
        }
        private void LoadFavorites()
        {
            _configurationItems = new List<IConfigurationItem>();
            // Get all values in the Favorites reg key
            string keyPath = @"SOFTWARE\AtlasOS\Toolbox\Favorites";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        try
                        {
                            _configurationItems.Add(App.RootList.Where(item => item.Key == valueName).FirstOrDefault());
                        }
                        catch
                        {
                            App.logger.Error(@$"Value {valueName} was not found in RootList when trying to initialize favorites");
                        }
                    }
                }
                else
                {
                    App.logger.Warn(@$"Key ""HKLM\SOFTWARE\AtlasOS\Services\Toolbox\Favorites"" was not found");
                }
            }
            if (_configurationItems.Count == 0)
            {
                NoFavoritesText.Visibility = Visibility.Visible;
                FavoritesPanel.MinHeight = 300;
            }
            else
            {
                NoFavoritesText.Visibility = Visibility.Collapsed;
                FavoritesPanel.MinHeight = 50;
            }
            FavoritesControl.ItemsSource = _configurationItems;
        }
        private void LoadText()
        {
            // Home Header
            HomeHeaderText.Text = App.GetValueFromItemList("Home_HeaderText");
            //Other
            ProfilesHeader.Text = App.GetValueFromItemList("Home_ProfilesText");
            FavoritesHeader.Text = App.GetValueFromItemList("Home_Favorites");
            NewProfileButton.Content = App.GetValueFromItemList("NewProfilesButton");
            NoFavoritesText.Text = App.GetValueFromItemList("NoFavorites");
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
        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            toggleSwitch.Toggled += ToggleSwitchBehavior.OnToggled;
        }

        private async void LinkCard_Click(object sender, RoutedEventArgs e)
        {
            SettingsCard linkCard = sender as SettingsCard;
            LinksViewModel linkVM = linkCard.DataContext as LinksViewModel;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(linkVM.Link));
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            try
            {
                RegistryHelper.DeleteValue(@"HKLM\SOFTWARE\\AtlasOS\\Toolbox\\Favorites", menuFlyoutItem.Tag.ToString());
                LoadFavorites();
            }
            catch
            {
                App.logger.Error($@"{menuFlyoutItem.Tag.ToString()} value was not found");
            }
        }
    }
}
