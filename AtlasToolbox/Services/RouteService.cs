using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Services
{
    public class RouteService : BaseServiceRegister
    {
        public bool IsRootRoute { get; set; }

        public RouteService(string route, string icon, bool isRootRoute = false)
        {
            Route = route;
            Icon.Glyph = icon;
            IsRootRoute = isRootRoute;
        }
    }
}
