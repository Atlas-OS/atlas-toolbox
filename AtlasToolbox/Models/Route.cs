using AtlasToolbox.ViewModels.ConfigurationVM;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Linq;

namespace AtlasToolbox.Models
{
    public class Route : IConfigurationItem
    {
        public string EndPoint { get; set; }
        public string Name { get => App.GetValueFromItemList(EndPoint.Split("/").Last()); }
        public string Description { get => App.GetValueFromItemList(EndPoint.Split("/").Last(), true); }
        public bool RootRoute { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Key => EndPoint;
        public string RouteItem => string.Join("/", EndPoint.Split('/').SkipLast(1));

        public Route(string endPoint, string icon = "\uE897", bool rootRoute = false)
        {
            EndPoint = endPoint;
            Icon = icon;
            RootRoute = rootRoute;
        }
    }
}
