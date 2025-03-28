using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolboxInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Button Back { get; set; }
        public static Button Next { get; set; }
        public static Button Cancel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Back = BackButton;
            Next = NextButton;
            Cancel = CancelButton;
        }
    }
}