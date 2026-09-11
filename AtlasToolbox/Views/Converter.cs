using System;
using AtlasToolbox.Models;
using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace AtlasToolbox.Views
{
    internal class FontIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            FontIcon icon = (FontIcon)value;
            return icon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class BitmapIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapIcon BitMapIcon = new BitmapIcon();
            BitMapIcon.UriSource = new Uri(value.ToString());
            BitMapIcon.ShowAsMonochrome = false;
            return BitMapIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ConfigItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ConfigurationItem { get; set; }
        public DataTemplate MultiOptionConfigurationItem { get; set; }
        public DataTemplate RouteItem { get; set; }
        public DataTemplate ConfigurationButton { get; set; }
        public DataTemplate ConfiguartionLink { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ConfigurationItemViewModel)
            {
                return ConfigurationItem;
            }
            if (item is MultiConfigViewModel)
            {
                return MultiOptionConfigurationItem;
            }
            if (item is LinksViewModel)
            {
                return ConfiguartionLink;
            }
            if (item is ConfigurationButtonViewModel)
            {
                return ConfigurationButton;
            }
            if (item is Route)
            {
                return RouteItem;
            }

            return base.SelectTemplateCore(item, container);
        }
    }

    public class FavoriteItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ConfigurationItem { get; set; }
        public DataTemplate MultiOptionConfigurationItem { get; set; }
        public DataTemplate ConfigurationButton { get; set; }
        public DataTemplate ConfiguartionLink { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ConfigurationItemViewModel)
            {
                return ConfigurationItem;
            }
            if (item is MultiConfigViewModel)
            {
                return MultiOptionConfigurationItem;
            }
            if (item is LinksViewModel)
            {
                return ConfiguartionLink;
            }
            if (item is ConfigurationButtonViewModel)
            {
                return ConfigurationButton;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
