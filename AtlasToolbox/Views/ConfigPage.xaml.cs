using System;
using System.Collections.ObjectModel;
using System.Linq;
using AtlasToolbox.Enums;
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
        Enum.TryParse(new ConfigurationCategory().GetType(), App.CurrentCategory, out configType);
        _viewModel.ShowForType((ConfigurationCategory)configType);

        this.DataContext = _viewModel;

        ConfigurationCategory type = (ConfigurationCategory)configType;
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

        DataTemplate template = SubMenuItems.ItemTemplate;

        Frame.Navigate(typeof(SubSection), new Tuple<ConfigurationSubMenuViewModel, DataTemplate, object>(item, template, this.BreadcrumbBar.ItemsSource), new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
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
}
