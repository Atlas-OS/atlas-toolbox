using AtlasToolbox.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasToolbox.ViewModels
{
    public class SoftwarePageViewModel
    {
        public ObservableCollection<SoftwareItemViewModel> SoftwareItemViewModels { get; set; }
        public List<SoftwareItemViewModel> SelectedSoftwareItemViewModels { get; set; }

        public SoftwarePageViewModel(IEnumerable<SoftwareItemViewModel> softwareItemViewModels)
        {
            SelectedSoftwareItemViewModels = new List<SoftwareItemViewModel>();
            SoftwareItemViewModels = new ObservableCollection<SoftwareItemViewModel>(softwareItemViewModels);
        }

        public static SoftwarePageViewModel LoadViewModel(IEnumerable<SoftwareItemViewModel> softwareItemViewModels)
        {
            return new(softwareItemViewModels);
        }
    }
}
