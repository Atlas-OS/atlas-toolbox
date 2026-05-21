using System;
using System.Collections.ObjectModel;
using System.Linq;
using AtlasToolbox.Enums;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using NLog.Filters;

namespace AtlasToolbox.Views;

public sealed partial class ConfigPage : Page
{
    private readonly ConfigPageViewModel _viewModel;
    private object configType;

    public ConfigPage()
    {
        this.InitializeComponent();

        _viewModel = App._host.Services.GetRequiredService<ConfigPageViewModel>();
        this.DataContext = _viewModel;

        BreadcrumbBar.ItemClicked += BreadcrumbBar_ItemClicked;
        this.Loaded += ConfigPage_Loaded;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Read category from the navigation parameter (stored when Navigate() was called)
        string category = e.Parameter as string ?? App.CurrentCategory;
        if (Enum.TryParse(typeof(ConfigurationType), category, out configType))
        {
            _viewModel.ShowForType((ConfigurationType)configType);
            BreadcrumbBar.ItemsSource = new ObservableCollection<Folder>
            {
                new Folder { Name = ((ConfigurationType)configType).GetDescription() }
            };
        }
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
        // Find the index of the item in the vm
        int index = -1;
        for (int i = 0; i < _viewModel.ConfigurationItems.Count; i++)
        {
            if (_viewModel.ConfigurationItems[i].Key == itemKey)
            {
                index = i;
                break;
            }
        }

        if (index < 0) return;

        // Get the item SettingsCard
        var container = ConfigItemsControl.ContainerFromIndex(index) as ContentPresenter;
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

    private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        var items = BreadcrumbBar.ItemsSource as ObservableCollection<string>;
        for (int i = items.Count - 1; i >= args.Index + 1; i--)
        {
            items.RemoveAt(i);
        }
    }

    private void OnCardClicked(object sender, RoutedEventArgs e)
    {
        SettingsCard settingCard = sender as SettingsCard;
        ConfigurationSubMenuViewModel item = settingCard.DataContext as ConfigurationSubMenuViewModel;

        DataTemplate template = (DataTemplate)MainGrid.Resources["ConfigurationSubMenuTemplate"];

        try
        {
            Frame.Navigate(typeof(SubSection), new Tuple<ConfigurationSubMenuViewModel, DataTemplate, object>(item, template, this.BreadcrumbBar.ItemsSource), new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
        catch (Exception ex)
        {
            App.logger.Error($"Exception when attempting to navigate to {item.Type}: \n\t{ex.Message}\n\n{ex.InnerException}");
        }
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
        RegistryHelper.SetValue(@"HKLM\SOFTWARE\\AtlasOS\\Toolbox\\Favorites", menuFlyoutItem.Tag.ToString(), true);
    }
}
