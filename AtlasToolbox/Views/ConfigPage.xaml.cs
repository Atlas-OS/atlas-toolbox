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
        // Gets all the items for the choosen category
        Enum.TryParse(new ConfigurationType().GetType(), App.CurrentCategory, out configType);
        _viewModel.ShowForType((ConfigurationType)configType);

        this.DataContext = _viewModel;

        ConfigurationType type = (ConfigurationType)configType;
        BreadcrumbBar.ItemsSource = new ObservableCollection<Folder> {
            new Folder {Name = type.GetDescription()}
        };
        BreadcrumbBar.ItemClicked += BreadcrumbBar_ItemClicked;
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
