using SimulIDE.src.gui.circuitwidget.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CircLabel label = new CircLabel();
            label.SetPlainText("blablabla");
            label.labelx = 100;
            label.labely = 100;
            label.labelrot = 45;
            label.SetLabelPos();
        }
    }
}
