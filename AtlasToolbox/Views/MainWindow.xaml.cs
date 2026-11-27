
using AtlasToolbox.Models;
using AtlasToolbox.Utils;
using AtlasToolbox.ViewModels.ConfigurationVM;
using AtlasToolbox.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using WinUIEx;

namespace AtlasToolbox.Views
{
    public sealed partial class MainWindow : Window
    {
        public ObservableCollection<BreadcrumbItem> BreadCrumbBarList { get; set; } = new();
        public Stack<BreadcrumbBarItem> breadcrumbBarItems = new();
        private bool _isNavigating;
        private bool _isUpdatingBreadcrumb = true;
        private bool _isSyncingSelection;
        private ConfigPageViewModel _configViewModel;

        public MainWindow()
        {
            this.InitializeComponent();

            OverlappedPresenter presenter = OverlappedPresenter.Create();
            WindowManager.Get(this).Width = 1250;
            WindowManager.Get(this).Height = 850;
            presenter.IsMaximizable = true;

            AppWindow.SetPresenter(presenter);
            AppWindow.TitleBar.PreferredTheme = TitleBarTheme.UseDefaultAppMode;

            SetWindowPosSize();
            ExtendsContentIntoTitleBar = true;

            LoadText();
            // LoadExperiments();

            _configViewModel = App._host.Services.GetRequiredService<ConfigPageViewModel>();
            App.RootList = new List<IConfigurationItem>();
            App.RootList.AddRange(_configViewModel.ConfigurationItems.OfType<LinksViewModel>());
            App.RootList.AddRange(_configViewModel.ConfigurationItems.OfType<ConfigurationItemViewModel>());
            App.RootList.AddRange(_configViewModel.ConfigurationItems.OfType<MultiConfigViewModel>());
            App.RootList.AddRange(_configViewModel.ConfigurationItems.OfType<ConfigurationButtonViewModel>());

            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();
            ContentFrame.Navigate(
                       typeof(Views.HomePage),
                       null,
                       new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo()
                       );
            SetTitleBar(AppTitleBar);
            // Dialogs need the window's XAML root, also before anything was navigated to.
            if (this.Content is FrameworkElement content) content.Loaded += (_, _) => App.XamlRoot = content.XamlRoot;
            CheckUpdates();
            if (RegistryHelper.IsMatch("HKLM\\SOFTWARE\\AtlasOS\\Toolbox", "OnStartup", 1)) this.Closed += AppBehaviorHelper.HideApp;
            else this.Closed += AppBehaviorHelper.CloseApp;
        }

        public void LoadExperiments() { }

        public bool IsFullscreen()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                if (presenter.State == OverlappedPresenterState.Maximized)
                {
                    return true;
                }
            }
            return false;
        }

        private async void CheckUpdates()
        {
            bool update = await Task.Run(() => ToolboxUpdateHelper.CheckUpdates());
            if (update)
            {
                UpdateTitleBar.IsOpen = true;
            }
        }
        public void LoadText()
        {
            // Updates
            UpdateTitleBar.Title = App.GetValueFromItemList("NewUpdateDesc");
            LearnMoreBtn.Content = App.GetValueFromItemList("LearnMore");

            // Navigation Items
            Home.Content = App.GetValueFromItemList("Home_HeaderText");
            Software.Content = App.GetValueFromItemList("Software");
            General.Content = App.GetValueFromItemList("General");
            Interface.Content = App.GetValueFromItemList("Interface");
            Windows.Content = App.GetValueFromItemList("Windows");
            Advanced.Content = App.GetValueFromItemList("Advanced");
            Security.Content = App.GetValueFromItemList("Security");
            Troubleshooting.Content = App.GetValueFromItemList("Troubleshooting");
            Setting.Content = App.GetValueFromItemList("Settings");

            // Search Box
            SearchBox.PlaceholderText = App.GetValueFromItemList("SearchPlaceholder");
        }

        /// <summary>
        /// Gets the window Xaml root for ContentDialogs
        /// </summary>
        /// <returns></returns>
        public XamlRoot GetXamlRoot()
        {
            return this.Content.XamlRoot;
        }

        #region Navigation Control
        /// <summary>
        /// navigates to the correct page when a navigation item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (_isSyncingSelection || args.SelectedItemContainer is null)
            {
                return;
            }

            string selectedItem = args.SelectedItemContainer.Tag.ToString() ?? "";
            switch (selectedItem)
            {
                case "SettingsPage":
                    Navigate(typeof(SettingsPage));
                    SwitchContentVisibility(false);
                    break;
                case "AtlasToolbox.Views.SoftwarePage":
                    Navigate(typeof(SoftwarePage));
                    SwitchContentVisibility(false);
                    break;
                case "AtlasToolbox.Views.HomePage":
                    Navigate(typeof(HomePage));
                    SwitchContentVisibility(false);
                    break;
                default:
                    Navigate(typeof(ConfigPage), selectedItem);
                    SwitchContentVisibility(true);
                    break;
            }
            App.XamlRoot = this.Content.XamlRoot;
        }

        /// <summary>
        /// Switches the maincontent visibilites
        /// </summary>
        /// <param name="showForConfigPage">True if is on a config page, else False</param>
        private void SwitchContentVisibility(bool showForConfigPage)
        {
            if (BreadcrumbBar.Visibility == Visibility.Collapsed && !showForConfigPage) return;
            if (showForConfigPage)
            {
                BreadcrumbBar.Visibility = Visibility.Visible;
                Grid.SetRow(ContentFrame, 1);
                Grid.SetRowSpan(ContentFrame, 1);
                MainGrid.Margin = new Thickness(55, 0, 0, 0);
                return;
            }
            BreadcrumbBar.Visibility = Visibility.Collapsed;
            Grid.SetRow(ContentFrame, 0);
            Grid.SetRowSpan(ContentFrame, 2);
            MainGrid.Margin = new Thickness(0);
        }

        /// <summary>
        /// Navigates the ContentFrame to the selected page
        /// </summary>
        /// <param name="tag"></param>
        private async void Navigate(Type type, string route = null)
        {
            if (_isNavigating)
            {
                return;
            }

            try
            {
                _isNavigating = true;
                ContentFrame.Navigate(type, route, new DrillInNavigationTransitionInfo());
            }
            finally
            {
                _isNavigating = false;
            }
        }

        public bool NavigateToRoute(string route, NavigationTransitionInfo transitionInfo)
        {
            if (!ContentFrame.Navigate(typeof(ConfigPage), route, transitionInfo))
            {
                App.logger.Error($"Failed to navigate to configuration route: {route}");
                return false;
            }
            // Remove previous ConfigPage entries from back-stack to prevent memory accumulation
            for (int i = ContentFrame.BackStack.Count - 1; i >= 0; i--)
            {
                if (ContentFrame.BackStack[i].SourcePageType == typeof(ConfigPage))
                    ContentFrame.BackStack.RemoveAt(i);
            }
            NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;
            return true;
        }

        public void GoBack()
        {
            if (ContentFrame.CanGoBack) ContentFrame.GoBack();
        }
        public void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack) ContentFrame.GoBack();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;
            NavigationViewControl.Header = null;

            string tag;
            if (e.SourcePageType == typeof(ConfigPage) && e.Parameter is string route)
            {
                tag = route.Split('/')[0];
                GenerateBreadcrumBar(route);
                SwitchContentVisibility(true);
            }
            else
            {
                tag = e.SourcePageType == typeof(SettingsPage) ? "SettingsPage" : e.SourcePageType.FullName;
                SwitchContentVisibility(false);
            }

            var item = NavigationViewControl.MenuItems.OfType<NavigationViewItem>()
                .Concat(NavigationViewControl.FooterMenuItems.OfType<NavigationViewItem>())
                .FirstOrDefault(n => Equals(n.Tag, tag));
            if (item != null)
            {
                _isSyncingSelection = true;
                try { NavigationViewControl.SelectedItem = item; }
                finally { _isSyncingSelection = false; }
            }
        }

        public void DisableBreadcrumbBar()
        {
            BreadcrumbBar.IsEnabled = false;
        }

        public void GenerateBreadcrumBar(string route)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(route))
                {
                    BreadCrumbBarList.Clear();
                    BreadcrumbBar.IsEnabled = false;
                    return;
                }

                string[] segments = route.Split('/');
                string currentRoute = string.Empty;
                List<BreadcrumbItem> newItems = new(segments.Length);

                for (int i = 0; i < segments.Length; i++)
                {
                    currentRoute = i == 0 ? segments[i] : $"{currentRoute}/{segments[i]}";
                    newItems.Add(new(currentRoute, App.GetValueFromItemList(segments[i])));
                }

                BreadCrumbBarList.Clear();
                foreach (BreadcrumbItem item in newItems)
                {
                    BreadCrumbBarList.Add(item);
                }

                BreadcrumbBar.IsEnabled = true;
            }
            catch
            {
                App.logger.Info("Failed to generate breadcrumb bar");
            }
        }
       
        private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            var route = ((BreadcrumbItem)args.Item).Route;
            NavigateToRoute(route, new DrillInNavigationTransitionInfo());
        }

        #endregion Navigation Control

        private void AppTitleBar_PaneToggleRequested(Microsoft.UI.Xaml.Controls.TitleBar sender, object args)
        {
            NavigationViewControl.IsPaneOpen = !NavigationViewControl.IsPaneOpen;
        }

        /// <summary>
        /// Creates a ContentDialog with the required type
        /// </summary>
        /// <param name="type">type of content dialog</param>
        /// <exception cref="Exception"></exception>
        public async void ContentDialogContoller(string type)
        {
            string title = "", desc = "", primBtnTxt = "";
            ICommand command = null;

            switch (type)
            {
                case "newUpdate":
                    title = App.GetValueFromItemList("NewUpdate");
                    desc = App.GetValueFromItemList("NewUpdateDesc");
                    primBtnTxt = App.GetValueFromItemList("Yes");
                    command = new RelayCommand(ToolboxUpdateHelper.InstallUpdate);
                    break;
                case "restartApp":
                    title = App.GetValueFromItemList("RestartApp");
                    desc = App.GetValueFromItemList("RestartAppDesc");
                    primBtnTxt = App.GetValueFromItemList("RestartAppBtn");
                    command = new RelayCommand(ComputerStateHelper.RestartApp);
                    break;
                case "restart":
                    title = App.GetValueFromItemList("RestartPC");
                    desc = App.GetValueFromItemList("RestartPCDesc");
                    primBtnTxt = App.GetValueFromItemList("Restart");
                    command = new RelayCommand(ComputerStateHelper.RestartComputer);
                    break;
                case "logoff":
                    title = App.GetValueFromItemList("RelogApply");
                    desc = App.GetValueFromItemList("RelogApplyDesc");
                    primBtnTxt = App.GetValueFromItemList("RelogBtn");
                    command = new RelayCommand(ComputerStateHelper.LogOffComputer);
                    break;
                default:
                    throw new Exception("ContentDialog type was not set or does not match any possible type");
            }
            await DispatcherQueue.EnqueueAsync(() =>
            {
                ContentDialog dialog = new ContentDialog();

                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                dialog.XamlRoot = App.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = title;
                dialog.Content = desc;
                dialog.PrimaryButtonText = primBtnTxt;
                dialog.CloseButtonText = App.GetValueFromItemList("Later");
                dialog.DefaultButton = ContentDialogButton.Primary;
                dialog.PrimaryButtonCommand = command;

                try
                {
                    var result = dialog.ShowAsync();
                }
                catch
                { App.logger.Error("Program tried to open more than one ContentDialog"); }
            });
        }

        /// <summary>
        /// Sets the window position and size
        /// </summary>
        private void SetWindowPosSize()
        {
            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);
            int width, height;
            try
            {
                // Get Window size
                width = int.Parse((string)ToolboxSettings.Get("AppWidth"));
                height = int.Parse((string)ToolboxSettings.Get("AppHeight"));
            }
            catch (Exception ex)
            {
                width = 1250;
                height = 850;
                // Reset the registry
                ToolboxSettings.Set("AppWidth", width, Microsoft.Win32.RegistryValueKind.String);
                ToolboxSettings.Set("AppHeight", height, Microsoft.Win32.RegistryValueKind.String);
                // Log the error
                App.logger.Warn("Window size values were incorrect. Reverting to defaults\n\n" + ex.Message);
            }

            if (width == 1250 && height == 850)
            {
                // Calculate size
                // Scaled from a 1920 x 1080 screen, in floating point: integer division
                // made the window tiny on any smaller screen.
                width = (int)(screenWidth / 1920.0 * 1250);
                height = (int)(screenHeight / 1080.0 * 850);
            }
            width = Math.Clamp(width, 800, Math.Max(800, screenWidth));
            height = Math.Clamp(height, 600, Math.Max(600, screenHeight));

            // Calculate position to put on screen
            double centerX = (screenWidth - width) / 2;
            double centerY = (screenHeight - height) / 2;

            AppWindow.Resize(new Windows.Graphics.SizeInt32(width, height));
            this.Move((int)centerX, (int)centerY);
        }
        /// <summary>
        /// Formats a double into an int 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int FormatDoubleInt(double value)
        {
            string valueString = value.ToString();
            string[] valueArr = valueString.Split('.');
            return int.Parse(valueArr[0]);
        }

        public void GetWindowSize(out int width, out int height)
        {
            width = AppWindow.Size.Width;
            height = AppWindow.Size.Height;
        }

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        int timesClicked;
        private void AtlasButton_Click(object sender, RoutedEventArgs e)
        {
            App.f_window = new FWindow();
            App.f_window.Activate();
            timesClicked = 0;
        }

        #region Search
        public sealed class SearchResult
        {
            public IConfigurationItem Item { get; }
            public string Name => Item.Name;
            public string Location { get; }

            public SearchResult(IConfigurationItem item)
            {
                Item = item;
                Location = string.Join(" / ", item.RouteItem.Split('/').Select(segment => App.GetValueFromItemList(segment)));
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var result = args.ChosenSuggestion as SearchResult;
            if (result == null && !string.IsNullOrWhiteSpace(args.QueryText))
            {
                result = (sender.ItemsSource as IEnumerable<SearchResult>)?.FirstOrDefault();
            }
            if (result == null) return;

            if (NavigateToRoute(result.Item.RouteItem, new DrillInNavigationTransitionInfo()) &&
                ContentFrame.Content is ConfigPage page)
            {
                page.HighlightItem(result.Item);
                sender.Text = string.Empty;
                sender.ItemsSource = null;
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput || _configViewModel == null) return;

            string[] terms = sender.Text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            sender.ItemsSource = terms.Length == 0
                ? Array.Empty<SearchResult>()
                : _configViewModel.ConfigurationItems
                    .Where(item => !string.IsNullOrWhiteSpace(item.Name) &&
                        terms.All(term => item.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase)))
                    .Select(item => new SearchResult(item))
                    .ToList();
        }
        #endregion Search
    }
}
