
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Models
{
    public class Route
    {
        public string EndPoint { get; set; }
        public string Name { get => App.GetValueFromItemList(EndPoint.Split("/").Last());}
        public string Description { get => App.GetValueFromItemList(EndPoint.Split("/").Last(), true); }
        public FontIcon Icon { get; set; } = new FontIcon();

        public Route(string endPoint, string icon = "\uE897")
        {
            EndPoint = endPoint;
            Icon.Glyph = icon;
        }
    }
}
