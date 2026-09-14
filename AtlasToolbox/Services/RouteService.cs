using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services
{
    public class RouteService : BaseServiceRegister
    {
        public bool IsRootRoute { get; set; }
        public string FullRoute { get; set; }
        public RouteService(string route, string icon, bool isRootRoute = false)
        {
            Route = string.Join("/", route.Split('/').SkipLast(1));
            Key = route.Split("/").Last();
            FullRoute = route;
            Icon.Glyph = icon;
            IsRootRoute = isRootRoute;
        }
    }
}
