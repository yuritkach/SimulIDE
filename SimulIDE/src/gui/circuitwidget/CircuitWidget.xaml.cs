using SharpGL;
using SharpGL.WPF;
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

namespace SimulIDE.src.gui.circuitwidget
{
    /// <summary>
    /// Interaction logic for CircuitPage.xaml
    /// </summary>
    public partial class CircuitWidget : Page
    {
        public CircuitWidget()
        {
            InitializeComponent();
            control = (OpenGLControl)this.FindName("openGLControl");
            Init();
        }
        

        protected static CircuitWidget self = null;
        public static CircuitWidget Self() { return self; }

        protected void Init()
        {
            self = this;
            circView = new CircuitView(control);
//            rateLabel = new CircLabel(canvas);
//            rateLabel.SetFontSize(10);
            //            string appPath = QCoreApplication::applicationDirPath();
            //            lastCircDir = MainWindow.Self().Settings().Value("lastCircDir");
            //if (lastCircDir=="") lastCircDir = appPath + "..share/simulide/examples";

            NewCircuit();
            SetRate(0);

        }


        public void Clear()
        {
            circView.Clear();
            circView.SetCircTime(0);
        }

        public bool NewCircuit()
        {
            PowerCircOff();

        //    if (MainWindow::self()->windowTitle().endsWith('*'))
        //    {
        //        const QMessageBox::StandardButton ret
        //        = QMessageBox::warning(this, "MainWindow::closeEvent",
        //                               tr("\nCircuit has been modified.\n"

        //                                  "Do you want to save your changes?\n"),
        //          QMessageBox::Save | QMessageBox::Discard | QMessageBox::Cancel);

        //        if (ret == QMessageBox::Save) saveCirc();
        //        else if (ret == QMessageBox::Cancel) return false;
        //    }
              Clear();
        //    Circuit::self()->setAutoBck(MainWindow::self()->autoBck());
        //    m_curCirc = "";

        //    MainWindow::self()->setTitle(tr("New Circuit"));
        //    MainWindow::self()->settings()->setValue("lastCircDir", m_lastCircDir);

            return true;
        }

        public void OpenCirc()
        {
        //    const QString dir = m_lastCircDir;
        //    QString fileName
        //    = QFileDialog::getOpenFileName(0l, tr("Load Circuit"), dir,
        //                                        tr("Circuits (*.simu);;All files (*.*)"));

        //    loadCirc(fileName);
        }

        public void LoadCirc(string path)
        {
        //    if (!path.isEmpty() && path.endsWith(".simu"))
        //    {
        //        newCircuit();
        //        Circuit::self()->loadCircuit(path);

        //        m_curCirc = path;
        //        m_lastCircDir = path;
        //        MainWindow::self()->setTitle(path.split("/").last());
        //        MainWindow::self()->settings()->setValue("lastCircDir", m_lastCircDir);
        //        //FileBrowser::self()->setPath(m_lastCircDir);
        //        m_circView.setCircTime(0);
        //    }
        }

        public void SaveCirc()
        {
        //    bool saved = false;
        //    if (m_curCirc.isEmpty()) saved = saveCircAs();
        //    else saved = Circuit::self()->saveCircuit(m_curCirc);

        //    if (saved)
        //    {
        //        QString fileName = m_curCirc;
        //        MainWindow::self()->setTitle(fileName.split("/").last());
        //    }
        }

        public bool SaveCircAs()
        {
            //    const QString dir = m_lastCircDir;
            //    QString fileName = QFileDialog::getSaveFileName(this, tr("Save Circuit"), dir,
            //                                                     tr("Circuits (*.simu);;All files (*.*)"));
            //    if (fileName.isEmpty()) return false;

            //    m_curCirc = fileName;
            //    m_lastCircDir = fileName;

            //    bool saved = Circuit::self()->saveCircuit(fileName);
            //    if (saved)
            //    {
            //        QString fileName = m_curCirc;
            //        MainWindow::self()->setTitle(fileName.split("/").last());
            //        MainWindow::self()->settings()->setValue("lastCircDir", m_lastCircDir);
            //        //FileBrowser::self()->setPath(m_lastCircDir);
            //    }
            //   return saved;
            return false;
        }

        public void PowerCirc()
        {
        //    if (powerCircAct->iconText() == "Off") powerCircOn();
        //    else if (powerCircAct->iconText() == "On") powerCircOff();
        }

        protected void PowerCircOn()
        {
        //    powerCircAct->setIcon(QIcon(":/poweron.png"));
        //    powerCircAct->setEnabled(true);
        //    powerCircAct->setIconText("On");
        //    pauseSimAct->setEnabled(true);
        //    Simulator::self()->runContinuous();
        }

        protected void PowerCircOff()
        {
        //    if (Simulator::self()->isPaused()) Simulator::self()->resumeSim();
        //    Simulator::self()->stopSim();

        //    powerCircAct->setIcon(QIcon(":/poweroff.png"));
        //    powerCircAct->setIconText("Off");
        //    powerCircAct->setEnabled(true);
        //    pauseSimAct->setEnabled(false);
        }

        protected void PowerCircDebug(bool run)
        {
        //    powerCircAct->setIcon(QIcon(":/powerdeb.png"));
        //    powerCircAct->setIconText("Debug");
        //    powerCircAct->setEnabled(true);

        //    Simulator::self()->debug(run);
        //    m_rateLabel->setText(tr("    Real Speed: Debugger"));
        }

        public void PauseSim()
        {
        //    if (Simulator::self()->isRunning())
        //    {
        //        Simulator::self()->pauseSim();
        //        powerCircAct->setEnabled(false);
        //    }
        //    else if (Simulator::self()->isPaused())
        //    {
        //        Simulator::self()->resumeSim();
        //        powerCircAct->setEnabled(true);
        //    }

        }

        public void OpenInfo()
        {
        //    QDesktopServices::openUrl(QUrl("http://simulide.blogspot.com"));
        }

        protected void SetRate(int rate)
        {
        //    if (rate < 0) m_rateLabel->setText(tr("Circuit ERROR!!!"));
        //    else
        //        m_rateLabel->setText(tr("    Real Speed: ") + QString::number(rate) + " %");
        }

        //        signals:
        //        void dataAvailable(int uart, const QByteArray &data );

        
        protected CircuitView circView;
        //        //TerminalWidget    m_terminal;
        //        PlotterWidget m_plotter;
        //        QToolBar m_circToolBar;
                CircLabel rateLabel;
        //        QMenu m_infoMenu;
        //        QString m_curCirc;
        private string lastCircDir;
        protected OpenGLControl control;


        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)=>circView.OnMouseMove(e, sender as IInputElement);
        private void OpenGLControl_MouseDown(object sender, MouseButtonEventArgs e)=>circView.OnMouseDown(sender as IInputElement,e);
        private void OpenGLControl_MouseUp(object sender, MouseButtonEventArgs e)=>circView.OnMouseUp(this,e);
        private void OpenGLControl_MouseLeave(object sender, MouseEventArgs e)=>circView.OnMouseLeave(this,e);
        private void OpenGLControl_MouseWheel(object sender, MouseWheelEventArgs e)=>circView.OnMouseWheel(this,e);
        private void OpenGLControl_MouseEnter(object sender, MouseEventArgs e)=>circView.OnMouseEnter(this,e);
        
        private void CircuitNewButton_Click(object sender, RoutedEventArgs e) => NewCircuit();
        private void CircuitOpenButton_Click(object sender, RoutedEventArgs e) => OpenCirc();
        private void CircuitSaveButton_Click(object sender, RoutedEventArgs e) => SaveCirc();
        private void CircuitSaveAsButton_Click(object sender, RoutedEventArgs e) => SaveCircAs();
        private void CircuitPowerButton_Click(object sender, RoutedEventArgs e) => PowerCirc();
        private void CircuitPauseButton_Click(object sender, RoutedEventArgs e) => PauseSim();
        private void CircuitOpenInfoButton_Click(object sender, RoutedEventArgs e) => OpenInfo();

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            OpenGL gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, 0.0f);
            circView.Draw(gl);
            gl.Flush();
        }

        private void OpenGLControl_Resized(object sender, OpenGLRoutedEventArgs args)
        {
            circView.SetViewPortSize(control.ActualWidth, control.ActualHeight);
        }
    }
}

