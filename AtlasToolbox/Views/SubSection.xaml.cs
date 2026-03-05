
using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels.Configuration;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.AI.MachineLearning;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;

namespace AtlasToolbox.Views
{
    public sealed partial class SubSection : Page
    {
        private object configType;
        private ConfigurationSubMenuViewModel _viewModel;

        public SubSection()
        {
            this.InitializeComponent();
            this.Loaded += ConfigPage_Loaded;
        }
        private string oldCat { get; set; }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Tuple<ConfigurationSubMenuViewModel, DataTemplate, object> parameter)
            {
                var item = parameter.Item1;
                _viewModel = item;
                ObservableCollection<Folder> item2 = parameter.Item3 as ObservableCollection<Folder>;
                // Gets all the configuration services
                ItemsControl.ItemsSource = item.ConfigurationItems;
                MultiOptionItemsControl.ItemsSource = item.MultiOptionConfigurationItems;
                Links.ItemsSource = item.LinksViewModels;
                SubMenuItems.ItemsSource = item.ConfigurationSubMenuViewModels;
                ConfigurationButton.ItemsSource = item.ConfigurationButtonViewModels;
                bool addFolderItem = true;
                Folder folder = new Folder
                {
                    Name = item.Name,
                };
                foreach (Folder folder1 in item2)
                {
                    if (folder1.Name == item.Name)
                    {
                        addFolderItem = false;
                    }
                }
                if (addFolderItem)
                {
                    item2.Add(folder);
                }
                if (!addFolderItem && item2.Last().Name != item.Name)
                {
                    item2.Remove(item2.Last());
                }
                BreadcrumbBar.ItemsSource = item2;
                BreadcrumbBar.ItemClicked += BreadcrumbBar_ItemClicked;

                oldCat = App.CurrentCategory;
            }
        }



        private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            var items = BreadcrumbBar.ItemsSource as ObservableCollection<Folder>;
            for (int i = items.Count - 1; i >= args.Index + 1; i--)
            {
                items.RemoveAt(i);
                App.CurrentCategory = oldCat;
                MainWindow window = App.m_window as MainWindow;
                window.GoBack();
            }
        }

        private void OnCardClicked(object sender, RoutedEventArgs e)
        {
            var settingCard = sender as SettingsCard;
            var item = settingCard.DataContext as ConfigurationSubMenuViewModel;
            var template = SubMenuItems.ItemTemplate;

            var breadcrumbItems = BreadcrumbBar.ItemsSource as ObservableCollection<Folder>;
            if (breadcrumbItems == null)
            {
                breadcrumbItems = new ObservableCollection<Folder>();
            }

            Frame.Navigate(typeof(SubSection), new Tuple<ConfigurationSubMenuViewModel, DataTemplate, object>(item, template, breadcrumbItems), new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;
            toggleSwitch.Toggled += ToggleSwitchBehavior.OnToggled;
        }

        private async void LinkCard_Click(object sender, RoutedEventArgs e)
        {
            var linkCard = sender as SettingsCard;
            var linkVM = linkCard.DataContext as LinksViewModel;

            await Windows.System.Launcher.LaunchUriAsync(new Uri(linkVM.Link));
        }
        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            RegistryHelper.SetValue(@"HKLM\SOFTWARE\\AtlasOS\\Toolbox\\Favorites", menuFlyoutItem.Tag.ToString(), true);
        }

        private async void ConfigPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Check if there's an item to highlight from search
            if (!string.IsNullOrEmpty(App.SearchHighlightItemKey))
            {
                string targetKey = App.SearchHighlightItemKey;
                App.SearchHighlightItemKey = null;

                await System.Threading.Tasks.Task.Delay(100);

                ScrollToAndHighlightItem(targetKey);
            }
        }

        private void ScrollToAndHighlightItem(string itemKey)
        {
            if (_viewModel == null) return;

            // Find the index of the item in ConfigurationItems
            int index = -1;
            Microsoft.UI.Xaml.Controls.ItemsControl targetItemsControl = null;

            for (int i = 0; i < _viewModel.ConfigurationItems.Count; i++)
            {
                if (_viewModel.ConfigurationItems[i].Key == itemKey)
                {
                    index = i;
                    targetItemsControl = ItemsControl;
                    break;
                }
            }

            // If not found, check MultiOptionConfigurationItems
            if (index < 0)
            {
                for (int i = 0; i < _viewModel.MultiOptionConfigurationItems.Count; i++)
                {
                    if (_viewModel.MultiOptionConfigurationItems[i].Key == itemKey)
                    {
                        index = i;
                        targetItemsControl = MultiOptionItemsControl;
                        break;
                    }
                }
            }

            if (index < 0 || targetItemsControl == null) return;

            // Get the item SettingsCard
            var container = targetItemsControl.ContainerFromIndex(index) as ContentPresenter;
            if (container == null) return;

            var settingsCard = FindDescendant<SettingsCard>(container);
            if (settingsCard == null) return;

            // Scroll to the item
            var transform = settingsCard.TransformToVisual(ConfigScrollViewer);
            var position = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

            double scrollPosition = ConfigScrollViewer.VerticalOffset + position.Y - (ConfigScrollViewer.ActualHeight / 2) + (settingsCard.ActualHeight / 2);
            ConfigScrollViewer.ChangeView(null, Math.Max(0, scrollPosition), null);

            HighlightSettingsCard(settingsCard);
        }

        private void HighlightSettingsCard(SettingsCard settingsCard)
        {
            var originalBrush = settingsCard.BorderBrush;
            var originalThickness = settingsCard.BorderThickness;

            var highlightBrush = new SolidColorBrush(Microsoft.UI.Colors.Gold);
            highlightBrush.Opacity = 0.3;
            settingsCard.BorderBrush = highlightBrush;
            settingsCard.BorderThickness = new Thickness(3);

            // Create a timer to fade out the highlight
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1500);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                settingsCard.BorderBrush = originalBrush;
                settingsCard.BorderThickness = originalThickness;
            };
            timer.Start();
        }

        private T FindDescendant<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }

                var descendant = FindDescendant<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }
            return null;
        }
    }
}
