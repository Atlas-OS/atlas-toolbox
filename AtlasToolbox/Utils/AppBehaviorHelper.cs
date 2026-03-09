using AtlasToolbox.Views;
using Microsoft.UI.Xaml;
using WinUIEx;

namespace AtlasToolbox.Utils
{
    public static class AppBehaviorHelper
    {
        /// <summary>
        /// Used to change the app's behavior when the background setting is changed or when the app is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HideApp(object sender, WindowEventArgs e)
        {
            e.Handled = true;
            App.m_window.Hide();
        }

        public static void CloseApp(object sender, WindowEventArgs e)
        {
            // Get & save the current app size
            int width, height;
            MainWindow mWindow = App.m_window as MainWindow;

            // Check if app is fullscreen
            // if true then don't save screen size
            if (!mWindow.IsFullscreen())
            {
                mWindow.GetWindowSize(out width, out height);
                RegistryHelper.SetValue(@"HKLM\SOFTWARE\AtlasOS\Services\Toolbox", "AppWidth", width, Microsoft.Win32.RegistryValueKind.String);
                RegistryHelper.SetValue(@"HKLM\SOFTWARE\AtlasOS\Services\Toolbox", "AppHeight", height, Microsoft.Win32.RegistryValueKind.String);
            }
            // Exit the app
            App.Current.Exit();
        }
    }
}
