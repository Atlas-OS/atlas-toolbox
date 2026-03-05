using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasToolbox.ViewModels;
using AtlasToolbox.ViewModels.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace AtlasToolbox.Views
{
    internal class FontIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            FontIcon icon = new();
            icon.Glyph = (string)value;
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
        public DataTemplate ConfigurationSubMenu { get; set; }
        public DataTemplate ConfigurationButton { get; set; }
        public DataTemplate ConfiguartionLink { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ConfigurationItemViewModel)
            {
                return ConfigurationItem;
            }
            if (item is MultiOptionConfigurationItemViewModel)
            {
                return MultiOptionConfigurationItem;
            }
            if (item is ConfigurationSubMenuViewModel)
            {
                return ConfigurationSubMenu;
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
            if (item is MultiOptionConfigurationItemViewModel)
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
