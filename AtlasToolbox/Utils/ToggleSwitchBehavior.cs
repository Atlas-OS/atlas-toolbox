using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using AtlasToolbox.ViewModels.ConfigurationVM;

namespace AtlasToolbox
{
    public static class ToggleSwitchBehavior
    {
        /// <summary>
        /// Applies the state the user moved the switch to. The switch also raises Toggled
        /// when it shows the applied state again, which matches its item and changes nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static async void OnToggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch { DataContext: ConfigurationItemViewModel item } toggleSwitch &&
                toggleSwitch.IsOn != item.CurrentSetting && !item.IsBusy)
            {
                await item.ApplyAsync(toggleSwitch.IsOn);
            }
        }
    }
}
