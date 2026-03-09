using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.Models
{
    public class BreadcrumbItem(string route, string name)
    {
        public string Route { get; set; } = route;
        public string Name { get; set; } = name;
    }
}
