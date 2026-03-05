using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using AtlasToolbox.ViewModels.Configuration;

namespace AtlasToolbox
{
    public static class ToggleSwitchBehavior
    {
        /// <summary>
        /// Gives the toggled behavior to its DataContext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnToggled(object sender, RoutedEventArgs e)
        {

           if (sender is ToggleSwitch toggleSwitch)
           {
               ConfigurationItemViewModel item = toggleSwitch.DataContext as ConfigurationItemViewModel;
               if (toggleSwitch.IsOn)
               {
                   item.CurrentSetting = true;
               }
               else
               {
                   item.CurrentSetting = false;
               }
           }
        }
    }
}
