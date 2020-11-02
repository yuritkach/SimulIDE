using SimulIDE.src.gui.circuitwidget;
using SimulIDE.src.gui.editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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

namespace SimulIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected static MainWindow self = null;
        public static MainWindow Self() { return self; }

        public MainWindow()
        {
            InitializeComponent();
            self = this;

            LoadCircHelp();
            CreateWidgets();
            ReadSettings();
            LoadPlugins();

            //            string backPath = m_settings.value("backupPath").toString();
            //            if (!backPath.isEmpty())
            //            {
            //                if (QFile::exists(backPath))
            //                    CircuitWidget::self()->loadCirc(backPath);
            //            }


        }


        protected void ReadSettings()
        {
            //        restoreGeometry(m_settings.value("geometry").toByteArray());
            //        restoreState(m_settings.value("windowState").toByteArray());
            //        m_Centralsplitter->restoreState(m_settings.value("Centralsplitter/geometry").toByteArray());

            //        int autoBck = 15;
            //        if (m_settings.contains("autoBck")) autoBck = m_settings.value("autoBck").toInt();
            //        Circuit::self()->setAutoBck(autoBck);
        }

        protected void WriteSettings()
        {
            //        m_settings.setValue("autoBck", m_autoBck);
            //        m_settings.setValue("fontScale", m_fontScale);
            //        m_settings.setValue("geometry", saveGeometry());
            //        m_settings.setValue("windowState", saveState());
            //        m_settings.setValue("Centralsplitter/geometry", m_Centralsplitter->saveState());

            //        QList<QTreeWidgetItem*> list = m_components->findItems("", Qt::MatchStartsWith | Qt::MatchRecursive);

            //        for (QTreeWidgetItem* item : list)
            //            m_settings.setValue(item->text(0) + "/collapsed", !item->isExpanded());

            //        FileWidget::self()->writeSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //            if (!m_editor->close()) { event->ignore(); return; }
            //            if( !m_circuit->newCircuit()) { event->ignore(); return; }
            WriteSettings();
            e.Cancel = false;
        }



        protected void CreateWidgets()
        {
            //            QWidget centralWidget = new QWidget(this);
            //            centralWidget->setObjectName("centralWidget");
            //            setCentralWidget(centralWidget);

            //            QGridLayout* baseWidgetLayout = new QGridLayout(centralWidget);
            //            baseWidgetLayout->setSpacing(0);
            //            baseWidgetLayout->setContentsMargins(0, 0, 0, 0);
            //            baseWidgetLayout->setObjectName("gridLayout");

            //            m_Centralsplitter = new QSplitter(this);
            //            m_Centralsplitter->setObjectName("Centralsplitter");
            //            m_Centralsplitter->setOrientation(Qt::Horizontal);

            //            m_sidepanel = new QTabWidget(this);
            //            m_sidepanel->setObjectName("sidepanel");
            //            m_sidepanel->setTabPosition(QTabWidget::West);
            //            QString fontSize = QString::number(int(11 * m_fontScale));
            //            m_sidepanel->tabBar()->setStyleSheet("QTabBar { font-size:" + fontSize + "px; }");
            //            m_Centralsplitter->addWidget(m_sidepanel);

            //            m_components = new ComponentSelector(m_sidepanel);
            //            m_components->setObjectName("components");
            //            m_sidepanel->addTab(m_components, tr("Components"));

            //            m_ramTabWidget = new QWidget(this);
            //            m_ramTabWidget->setObjectName("ramTabWidget");
            //            m_ramTabWidgetLayout = new QGridLayout(m_ramTabWidget);
            //            m_ramTabWidgetLayout->setSpacing(0);
            //            m_ramTabWidgetLayout->setContentsMargins(0, 0, 0, 0);
            //            m_ramTabWidgetLayout->setObjectName("ramTabWidgetLayout");
            //            m_sidepanel->addTab(m_ramTabWidget, tr("RamTable"));

            //            m_itemprop = new PropertiesWidget(this);
            //            m_itemprop->setObjectName("properties");
            //            m_sidepanel->addTab(m_itemprop, tr("Properties"));

            //            m_fileSystemTree = new FileWidget(this);
            //            m_fileSystemTree->setObjectName("fileExplorer");
            //            m_sidepanel->addTab(m_fileSystemTree, tr("File explorer"));

            m_circuit = new CircuitWidget();
            CircuitFrame.Content = m_circuit;
            //            m_circuit->setObjectName("circuit");
            //            m_Centralsplitter->addWidget(m_circuit);

            m_editor = new EditorPage();
            EditorFrame.Content = m_editor;
            //            m_editor->setObjectName(QString::fromUtf8("editor"));
            //            m_Centralsplitter->addWidget(m_editor);

            //            baseWidgetLayout->addWidget(m_Centralsplitter, 0, 0);

            //            QList<int> sizes;
            //            sizes << 150 << 350 << 500;
            //            m_Centralsplitter->setSizes(sizes);
        }

        protected void LoadCircHelp()
        {
            string locale = "_" + CultureInfo.CurrentCulture.Name.Split('_').First();
            string dfPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location)
                + "/help/" + locale + "/circuit" + locale + ".txt";

            if (File.Exists(dfPath)) {
                FileStream fs = new FileStream(dfPath, FileMode.Open);
                m_circHelp = fs.ToString();
                fs.Close();
            }
            else m_circHelp = "";
        }

        protected string CircHelp()
        {
            return m_circHelp;
        }

            void LoadPlugins()
            {
    //            // Load main Plugins
    //            QDir pluginsDir(qApp->applicationDirPath() );

    //            pluginsDir.cd("../lib/simulide/plugins");

    //            loadPluginsAt(pluginsDir);

    //            // Load main Component Sets
    //            QDir compSetDir = SIMUAPI_AppPath::self()->RODataFolder();

    //            if (compSetDir.exists()) ComponentSelector::self()->LoadCompSetAt(compSetDir);

    //            // Load Addons
    //            QString userPluginsPath = SIMUAPI_AppPath::self()->RWDataFolder().absoluteFilePath("addons");

    //            pluginsDir.setPath(userPluginsPath);

    //            if (!pluginsDir.exists()) return;

    //            for (QString pluginFolder : pluginsDir.entryList(QDir::Dirs))
    //            {
    //                if (pluginFolder.contains(".")) continue;
    //                //qDebug() << pluginFolder;
    //                pluginsDir.cd(pluginFolder);

    //                ComponentSelector::self()->LoadCompSetAt(pluginsDir);

    //                if (pluginsDir.entryList(QDir::Dirs).contains("lib"))
    //                {
    //                    pluginsDir.cd("lib");
    //                    loadPluginsAt(pluginsDir);
    //                    pluginsDir.cd("../");
    //                }
    //                pluginsDir.cd("../");
    //            }
            }

    //        void MainWindow::loadPluginsAt(QDir pluginsDir)
    //        {
    //            QString pluginName = "*plugin";

    //# ifndef Q_OS_UNIX
    //            pluginName += ".dll";
    //#else
    //            pluginName += ".so";
    //#endif

    //            pluginsDir.setNameFilters(QStringList(pluginName));

    //            QStringList fileList = pluginsDir.entryList(QDir::Files);

    //            if (fileList.isEmpty()) return;                                    // No plugins to load

    //            qDebug() << "\n    Loading Plugins at:\n" << pluginsDir.absolutePath() << "\n";

    //            for (QString libName : fileList)
    //            {
    //                pluginName = libName.split(".").first().remove("lib").remove("plugin").toUpper();

    //                if (m_plugins.contains(pluginName)) continue;

    //                QPluginLoader* pluginLoader = new QPluginLoader(pluginsDir.absoluteFilePath(libName));
    //                QObject* plugin = pluginLoader->instance();

    //                if (plugin)
    //                {
    //                    AppIface* item = qobject_cast<AppIface*>(plugin);

    //                    if (item)
    //                    {
    //                        item->initialize();
    //                        m_plugins[pluginName] = pluginLoader;
    //                        qDebug() << "        Plugin Loaded Successfully:\t" << pluginName;
    //                    }
    //                    else
    //                    {
    //                        pluginLoader->unload();
    //                        delete pluginLoader;
    //                    }
    //                }
    //                else
    //                {
    //                    QString errorMsg = pluginLoader->errorString();
    //                    qDebug() << "        " << pluginName << "\tplugin FAILED: " << errorMsg;

    //                    if (errorMsg.contains("libQt5SerialPort"))
    //                        errorMsg = tr(" Qt5SerialPort is not installed in your system\n\n    Mcu SerialPort will not work\n    Just Install libQt5SerialPort package\n    To have Mcu Serial Port Working");

    //                    QMessageBox::warning(0, tr("Plugin Error:"), errorMsg);
    //                }
    //            }
    //            qDebug() << "\n";
    //        }

    //        void MainWindow::unLoadPugin(QString pluginName)
    //        {
    //            if (m_plugins.contains(pluginName))
    //            {
    //                QPluginLoader* pluginLoader = m_plugins[pluginName];
    //                QObject* plugin = pluginLoader->instance();
    //                AppIface* item = qobject_cast<AppIface*>(plugin);
    //                item->terminate();
    //                pluginLoader->unload();
    //                m_plugins.remove(pluginName);
    //                delete pluginLoader;
    //            }
    //        }

    //        void MainWindow::applyStile()
    //        {
    //            QFile file(":/simulide.qss");
    //            file.open(QFile::ReadOnly);

    //            m_styleSheet = QLatin1String(file.readAll());

    //            qApp->setStyleSheet(m_styleSheet);
    //        }

    //        QSettings* MainWindow::settings() { return &m_settings; }







    double fontScale() { return m_fontScale; }
    void setFontScale(double scale) { m_fontScale = scale; }


    //        QTabWidget* m_sidepanel;
    //        QWidget* m_ramTabWidget;
    //        QGridLayout* m_ramTabWidgetLayout;

    protected bool m_blocked;
    protected double m_fontScale;
    protected int m_autoBck;

    //        Settings m_settings;

    protected string m_version;
    protected string m_styleSheet;
    protected string m_circHelp;

    //        QHash<QString, QPluginLoader*> m_plugins;

    protected CircuitWidget m_circuit;
    //        ComponentSelector m_components;
    //        PropertiesWidget m_itemprop;
    protected EditorPage m_editor;

    //        QSplitter m_Centralsplitter;
    //        FileWidget m_fileSystemTree;
   
    }

}
