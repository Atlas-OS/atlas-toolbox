using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels.ConfigurationVM;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using NLog.Filters;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AtlasToolbox.Views;

public sealed partial class ConfigPage : Page
{
    private readonly ConfigPageViewModel _viewModel;
    private object configType;
    private IConfigurationItem _highlightItem;
    private SettingsCard _highlightedCard;
    private object _originalBorderBrush;
    private object _originalBorderThickness;
    private DispatcherTimer _highlightTimer;

    public ConfigPage()
    {
        this.InitializeComponent();
        _viewModel = App._host.Services.GetRequiredService<ConfigPageViewModel>();
        this.DataContext = _viewModel;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ConfigItemsControl.ItemsSource = _viewModel.FilteredItems;
        if (e.Parameter is string route)
        {
            _viewModel.CurrentRoute = route;
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        LayoutUpdated -= ConfigPage_LayoutUpdated;
        _highlightItem = null;
        ClearHighlight();
        ConfigItemsControl.ItemsSource = null;
    }

    public void HighlightItem(IConfigurationItem item)
    {
        ClearHighlight();
        LayoutUpdated -= ConfigPage_LayoutUpdated;
        if (!_viewModel.FilteredItems.Contains(item))
        {
            App.logger.Warn($"Search result {item.Key} was not found on route {item.RouteItem}");
            return;
        }

        _highlightItem = item;
        LayoutUpdated += ConfigPage_LayoutUpdated;
        ConfigPage_LayoutUpdated(this, null);
    }

    private void ConfigPage_LayoutUpdated(object sender, object e)
    {
        int index = _viewModel.FilteredItems.IndexOf(_highlightItem);
        var container = ConfigItemsControl.ContainerFromIndex(index) as DependencyObject;
        var settingsCard = container == null ? null : FindDescendant<SettingsCard>(container);
        if (settingsCard == null || settingsCard.ActualHeight <= 0 || ConfigScrollViewer.ActualHeight <= 0) return;

        LayoutUpdated -= ConfigPage_LayoutUpdated;
        _highlightItem = null;
        var transform = settingsCard.TransformToVisual(ConfigScrollViewer);
        var position = transform.TransformPoint(new Windows.Foundation.Point(0, 0));
        double scrollPosition = ConfigScrollViewer.VerticalOffset + position.Y - (ConfigScrollViewer.ActualHeight / 2) + (settingsCard.ActualHeight / 2);
        ConfigScrollViewer.ChangeView(null, Math.Max(0, scrollPosition), null);

        _highlightedCard = settingsCard;
        _originalBorderBrush = settingsCard.ReadLocalValue(Control.BorderBrushProperty);
        _originalBorderThickness = settingsCard.ReadLocalValue(Control.BorderThicknessProperty);
        settingsCard.BorderBrush = new SolidColorBrush(new Windows.UI.ViewManagement.UISettings().GetColorValue(Windows.UI.ViewManagement.UIColorType.Accent));
        settingsCard.BorderThickness = new Thickness(3);
        _highlightTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _highlightTimer.Tick += HighlightTimer_Tick;
        _highlightTimer.Start();
    }

    private void HighlightTimer_Tick(object sender, object e) => ClearHighlight();

    private void ClearHighlight()
    {
        if (_highlightTimer != null)
        {
            _highlightTimer.Stop();
            _highlightTimer.Tick -= HighlightTimer_Tick;
            _highlightTimer = null;
        }
        if (_highlightedCard == null) return;

        if (_originalBorderBrush == DependencyProperty.UnsetValue)
            _highlightedCard.ClearValue(Control.BorderBrushProperty);
        else
            _highlightedCard.SetValue(Control.BorderBrushProperty, _originalBorderBrush);
        if (_originalBorderThickness == DependencyProperty.UnsetValue)
            _highlightedCard.ClearValue(Control.BorderThicknessProperty);
        else
            _highlightedCard.SetValue(Control.BorderThicknessProperty, _originalBorderThickness);
        _highlightedCard = null;
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

    private void OnCardClicked(object sender, RoutedEventArgs e)
    {
        SettingsCard settingCard = sender as SettingsCard;
        string route = settingCard.Tag.ToString();
        try
        {
            (App.m_window as MainWindow)?.NavigateToRoute(route,
                new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }
        catch (Exception ex)
        {
            App.logger.Error($"Exception when attempting to navigate to route: \n\t{ex.Message}\n\n{ex.InnerException}");
        }
    }

    private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
    {
        ToggleSwitch toggleSwitch = sender as ToggleSwitch;
        toggleSwitch.Toggled -= ToggleSwitchBehavior.OnToggled;
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
        RegistryHelper.SetValue(@"HKLM\SOFTWARE\AtlasOS\Toolbox\Favorites", menuFlyoutItem.Tag.ToString(), true);
    }
}
