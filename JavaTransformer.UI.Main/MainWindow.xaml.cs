using System.Diagnostics;
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
using JavaTransformer.Core.ModelViews;
using JavaTransformer.Core.UIWidget.Style;

namespace JavaTransformer.UI.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainModelView(
                name: "Java Transformer Core", 
                version: "1.0.0");

           
                MStyle style = new MStyle();
                style.Background = LColor.of("#121212");
                style.Foreground = LColor.of("#ffffff");
                style.HoverBackground = LColor.of("#FF00FF");

                TestButton.injectStyle(new MStyle(TestButton, style));

          
        }
    }
}