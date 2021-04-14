using Avr;
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
        protected Core core;
        protected MCU mcu;

        public MainWindow()
        {
            InitializeComponent();
            mcu = new MCU();
            core = new Core(mcu);
            core.LoadProgram("d:\\1.dat");
            core.Run();
        }
    }
}
