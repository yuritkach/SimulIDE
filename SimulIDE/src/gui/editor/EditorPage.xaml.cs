using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace SimulIDE.src.gui.editor
{
    /// <summary>
    /// Interaction logic for EditorPage.xaml
    /// </summary>
    public partial class EditorPage : Page
    {
        public EditorPage()
        {
            InitializeComponent();

            CreateWidgets();
            CreateActions();
//            ReadSettings();
        }



    public bool Close()
    {
      //     WriteSettings();
           for (int i = 0; i < tabControl.Items.Count; i++)
                tabControl.Items.RemoveAt(i);
            return MaybeSave();
    }

    //    protected override void keyPressEvent(QKeyEvent* event )
    //    {
    //        if ( event->key() == Qt::Key_N && (event->modifiers() & Qt::ControlModifier))
    //        {
    //            newFile();
    //        }
    //        else 
    //        if( event->key() == Qt::Key_S && (event->modifiers() & Qt::ControlModifier))
    //        {
    //            if ( event->modifiers() & Qt::ShiftModifier) 
    //                saveAs();
    //            else
    //                save();
    //        }
    //        else 
    //        if( event->key() == Qt::Key_O && (event->modifiers() & Qt::ControlModifier))
    //        {
    //            open();
    //        }
    //    }

        protected void NewFile()
        {
            CodeEditorWidget editorView = new CodeEditorWidget();
            TabItem item = new TabItem();
            item.Header = "New";
            item.Content = editorView;
            tabControl.SelectedIndex = tabControl.Items.Add(item);
            //        connect(baseWidget->m_codeEditor->document(), SIGNAL(contentsChanged()),
            //             this, SLOT(documentWasModified()));
            fileList.Add("New");
            EnableFileActs(true);
            EnableDebugActs(true);
        }

        protected void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string dir = lastDir;
            openFileDialog.InitialDirectory = dir;
            openFileDialog.Filter = "Arduino (*.ino)|*.ino|Asm (*.asm)|*.asm|GcBasic (*.gcb)|*.gcb|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog()==true)
            {
                string fileName = openFileDialog.FileName;
                if (fileName != "") LoadFile(fileName);
            }
        }

        protected void LoadFile(string fileName )
        {
            if (fileList.Contains(fileName))
                tabControl.SelectedIndex = fileList.IndexOf(fileName);
            else NewFile();
            //Cursor.Current = Cursors.WaitCursor;
            CodeEditor ce = GetCodeEditor();
            ce.AppendText(File.ReadAllText(fileName));
            ce.FileName=fileName;
            lastDir = System.IO.Path.GetDirectoryName(fileName);
            int index = tabControl.SelectedIndex;
            fileList[index] = fileName;
            ((TabItem)tabControl.Items[index]).Header = System.IO.Path.GetFileName(fileName);
            EnableFileActs(true);
            //if(ce.HasDebugger())
            EnableDebugActs(true);
            //Cursor.Current = Cursors.DefaULT;
        }

        protected void Reload()
        {
                string fileName = fileList[tabControl.SelectedIndex];
                LoadFile(fileName);
        }

        protected bool Save()
        {
                string file = GetCodeEditor().FileName;
                if (file=="") return SaveAs();
                else return SaveFile(file);
        }

        protected bool SaveAs()
        {
            CodeEditor ce = GetCodeEditor();
            string fileName = ce.FileName;
            string ext = System.IO.Path.GetExtension(fileName);
            string path = System.IO.Path.GetDirectoryName(fileName);
            if (path == "")
                path = lastDir;
            //qDebug() << "EditorWindow::saveAs" << path;


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            if (ext == "")
                saveFileDialog.Filter = "Arduino (*.ino)|*.ino|Asm (*.asm)|*.asm|GcBasic (*.gcb)|*.gcb|All files (*.*)|*.*";
            else
                saveFileDialog.Filter = ext+"(*"+ext+")|*"+ext+"|All files (*.*)|*.*";
            
            bool? result = saveFileDialog.ShowDialog();
            fileName = saveFileDialog.FileName;
            if (fileName=="") return false;
            if (result == true)
            {
                fileList[tabControl.SelectedIndex]= fileName;
                return SaveFile(fileName);
            }
            return false;
        }

        protected bool SaveFile(string fileName)
        {
        //        QFile file(fileName);
        //        if (!file.open(QFile::WriteOnly | QFile::Text))
        //        {
        //            QMessageBox::warning(this, "EditorWindow::saveFile",
        //                             tr("Cannot write file %1:\n%2.")
        //                             .arg(fileName)
        //                             .arg(file.errorString()));
        //            return false;
        //        }
        //        QTextStream out(&file);
        //        out.setCodec("UTF-8");
        //        QApplication::setOverrideCursor(Qt::WaitCursor);
        //        CodeEditor* ce = getCodeEditor();
        //        out << ce->toPlainText();
        //        ce->setFile(fileName);
        //        QApplication::restoreOverrideCursor();

        //        ce->document()->setModified(false);
        //        documentWasModified();

        //        m_docWidget->setTabText(m_docWidget->currentIndex(), strippedName(fileName));
               return true;
        }

        protected bool MaybeSave()
        {
            if (fileList.Count==0) return true;
            if (GetCodeEditor().IsModified())
            {
                MessageBoxResult dialogResult = MessageBox.Show("\nThe Document has been modified.\nDo you want to save your changes?\n",
                    "Save file", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    Save();
                    return true;

                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        //    protected void DocumentWasModified()
        //    {
        //        CodeEditor* ce = getCodeEditor();
        //        QTextDocument* doc = ce->document();

        //        bool modified = doc->isModified();
        //        int index = m_docWidget->currentIndex();
        //        QString tabText = m_docWidget->tabText(index);

        //        if (modified && !tabText.endsWith("*")) tabText.append("*");
        //        else if (!modified && tabText.endsWith("*")) tabText.remove("*");

        //        m_docWidget->setTabText(index, tabText);

        //        redoAct->setEnabled(false);
        //        undoAct->setEnabled(false);
        //        if (doc->isRedoAvailable()) redoAct->setEnabled(true);
        //        if (doc->isUndoAvailable()) undoAct->setEnabled(true);

        //        ce->setCompiled(false);
        //    }

        protected void EnableFileActs(bool enable)
        {
            editorSaveButton.IsEnabled = enable;
            editorSaveAsButton.IsEnabled = enable;
    //        cutAct->setEnabled(enable);
    //        copyAct->setEnabled(enable);
    //        pasteAct->setEnabled(enable);
    //        undoAct->setEnabled(enable);
    //        redoAct->setEnabled(enable);
           editorFindButton.IsEnabled=enable;
        }

        protected void EnableDebugActs(bool enable)
        {
    //        debugAct->setEnabled(enable);
    //        runAct->setEnabled(enable);
    //        stepAct->setEnabled(enable);
    //        stepOverAct->setEnabled(enable);
    //        pauseAct->setEnabled(enable);
    //        resetAct->setEnabled(enable);
    //        stopAct->setEnabled(enable);
    //        compileAct->setEnabled(enable);
    //        loadAct->setEnabled(enable);
        }

    //    protected void TabContextMenu( const QPoint &eventpoint )
    //    {
    //        CodeEditor* ce = getCodeEditor();
    //        if (!ce) return;

    //        QMenu* menu = new QMenu();
    //        QAction* setCompilerAction = menu->addAction(QIcon(":/copy.png"), tr("Set Compiler Path"));
    //        connect(setCompilerAction, SIGNAL(triggered()), this, SLOT(setCompiler()));

    //        QAction* reloadAction = menu->addAction(QIcon(":/reload.png"), tr("Reload"));
    //        connect(reloadAction, SIGNAL(triggered()), this, SLOT(reload()));

    //        menu->exec(mapToGlobal(eventpoint));
    //        menu->deleteLater();
    //    }

    //    void EditorWindow::tabChanged(int tab)
    //    {
    //        qDebug() << "EditorWindow::tabChanged" << m_docWidget->currentIndex() << tab;
    //    }

    //    void EditorWindow::setCompiler()
    //    {
    //        CodeEditor* ce = getCodeEditor();
    //        if (ce) ce->setCompilerPath();
    //    }

        protected void CreateWidgets()
        {
    //        baseWidgetLayout = new QGridLayout(this);
    //        baseWidgetLayout->setSpacing(0);
    //        baseWidgetLayout->setContentsMargins(0, 0, 0, 0);
    //        baseWidgetLayout->setObjectName("gridLayout");

    //        m_editorToolBar = new QToolBar(this);
    //        baseWidgetLayout->addWidget(m_editorToolBar);

    //        m_debuggerToolBar = new QToolBar(this);
    //        m_debuggerToolBar->setVisible(false);
    //        baseWidgetLayout->addWidget(m_debuggerToolBar);

    //        m_docWidget = new QTabWidget(this);
    //        m_docWidget->setObjectName("docWidget");
    //        m_docWidget->setTabPosition(QTabWidget::North);
    //        m_docWidget->setTabsClosable(true);
    //        m_docWidget->setContextMenuPolicy(Qt::CustomContextMenu);
    //        double fontScale = MainWindow::self()->fontScale();
    //        QString fontSize = QString::number(int(10 * fontScale));
    //        m_docWidget->tabBar()->setStyleSheet("QTabBar { font-size:" + fontSize + "px; }");
    //        //m_docWidget->setMovable( true );
    //        baseWidgetLayout->addWidget(m_docWidget);

    //        connect(m_docWidget, SIGNAL(tabCloseRequested(int)),
    //             this, SLOT(closeTab(int)));

    //        connect(m_docWidget, SIGNAL(customContextMenuRequested(const QPoint &)), 
    //             this,        SLOT(tabContextMenu(const QPoint &)));

    //        connect(m_docWidget, SIGNAL(currentChanged(int)), this, SLOT(tabChanged(int)));

    //        setLayout(baseWidgetLayout);

    //        findRepDiaWidget = new FindReplaceDialog(this);
    //        findRepDiaWidget->setModal(false);
        }

        protected void CreateActions()
        {
            editorSaveButton.IsEnabled = false;
            editorSaveAsButton.IsEnabled = false;

            //        exitAct = new QAction(QIcon(":/exit.png"), tr("E&xit"), this);
            //        exitAct->setStatusTip(tr("Exit the application"));
            //        connect(exitAct, SIGNAL(triggered()), this, SLOT(close()));

            //        runAct = new QAction(QIcon(":/runtobk.png"), tr("Run To Breakpoint"), this);
            //        runAct->setStatusTip(tr("Run to next breakpoint"));
            //        runAct->setEnabled(false);
            //        connect(runAct, SIGNAL(triggered()), this, SLOT(run()));

            //        stepAct = new QAction(QIcon(":/step.png"), tr("Step"), this);
            //        stepAct->setStatusTip(tr("Step debugger"));
            //        stepAct->setEnabled(false);
            //        connect(stepAct, SIGNAL(triggered()), this, SLOT(step()));

            //        stepOverAct = new QAction(QIcon(":/rotateCW.png"), tr("StepOver"), this);
            //        stepOverAct->setStatusTip(tr("Step Over"));
            //        stepOverAct->setEnabled(false);
            //        stepOverAct->setVisible(false);
            //        connect(stepOverAct, SIGNAL(triggered()), this, SLOT(stepOver()));

            //        pauseAct = new QAction(QIcon(":/pause.png"), tr("Pause"), this);
            //        pauseAct->setStatusTip(tr("Pause debugger"));
            //        pauseAct->setEnabled(false);
            //        connect(pauseAct, SIGNAL(triggered()), this, SLOT(pause()));

            //        resetAct = new QAction(QIcon(":/reset.png"), tr("Reset"), this);
            //        resetAct->setStatusTip(tr("Reset debugger"));
            //        resetAct->setEnabled(false);
            //        connect(resetAct, SIGNAL(triggered()), this, SLOT(reset()));

            //        stopAct = new QAction(QIcon(":/stop.png"), tr("Stop Debugger"), this);
            //        stopAct->setStatusTip(tr("Stop debugger"));
            //        stopAct->setEnabled(false);
            //        connect(stopAct, SIGNAL(triggered()), this, SLOT(stop()));

            //        compileAct = new QAction(QIcon(":/compile.png"), tr("Compile"), this);
            //        compileAct->setStatusTip(tr("Compile Source"));
            //        compileAct->setEnabled(false);
            //        connect(compileAct, SIGNAL(triggered()), this, SLOT(compile()));

            //        loadAct = new QAction(QIcon(":/load.png"), tr("UpLoad"), this);
            //        loadAct->setStatusTip(tr("Load Firmware"));
            //        loadAct->setEnabled(false);
            //        connect(loadAct, SIGNAL(triggered()), this, SLOT(upload()));

            //        /*aboutAct = new QAction(QIcon(":/info.png"),tr("&About"), this);
            //        aboutAct->setStatusTip(tr("Show the application's About box"));
            //        connect(aboutAct, SIGNAL(triggered()), this, SLOT(about()));*/

            //        /*aboutQtAct = new QAction(QIcon(":/info.png"),tr("About &Qt"), this);
            //        aboutQtAct->setStatusTip(tr("Show the Qt library's About box"));
            //        connect(aboutQtAct, SIGNAL(triggered()), qApp, SLOT(aboutQt()));*/

            //        //connect(m_codeEditor, SIGNAL(copyAvailable(bool)), cutAct, SLOT(setEnabled(bool)));
            //        //connect(m_codeEditor, SIGNAL(copyAvailable(bool)), copyAct, SLOT(setEnabled(bool)));

            //        findQtAct = new QAction(QIcon(":/find.png"), tr("Find Replace"), this);
            //        findQtAct->setStatusTip(tr("Find Replace"));
            //        findQtAct->setEnabled(false);
            //        connect(findQtAct, SIGNAL(triggered()), this, SLOT(findReplaceDialog()));

            //        debugAct = new QAction(QIcon(":/play.png"), tr("Debug"), this);
            //        debugAct->setStatusTip(tr("Start Debugger"));
            //        debugAct->setEnabled(false);
            //        connect(debugAct, SIGNAL(triggered()), this, SLOT(debug()));
        }

    //    protected void EnableStepOver(bool en)
    //    {
    //        stepOverAct->setVisible(en);
    //    }

        protected CodeEditor GetCodeEditor()
        {
            return ((CodeEditorWidget)((TabItem)tabControl.Items[tabControl.SelectedIndex]).Content).EditorView;
        }

    //    protected void СloseTab(int index)
    //    {
    //        m_docWidget->setCurrentIndex(index);
    //        if (!MaybeSave()) return;

    //        m_fileList.removeAt(index);

    //        if (m_fileList.isEmpty())
    //        {
    //            enableFileActs(false); // disable file actions
    //            enableDebugActs(false);
    //        }
    //        if (m_debuggerToolBar->isVisible()) stop();

    //        CodeEditorWidget* actW = dynamic_cast<CodeEditorWidget*>(m_docWidget->widget(index));
    //        m_docWidget->removeTab(index);
    //        delete actW;

    //        int last = m_docWidget->count() - 1;
    //        if (index > last) m_docWidget->setCurrentIndex(last);
    //        else m_docWidget->setCurrentIndex(index);
    //    }

    //    protected void Cut() { getCodeEditor()->cut(); }
    //    protected void EditorWindow::copy() { getCodeEditor()->copy(); }
    //    protected void EditorWindow::paste() { getCodeEditor()->paste(); }
    //    protected void EditorWindow::undo() { getCodeEditor()->undo(); }
    //    protected void EditorWindow::redo() { getCodeEditor()->redo(); }

    //    protected void Debug()
    //    {
    //        CodeEditor* ce = getCodeEditor();

    //        if (ce->initDebbuger())
    //        {
    //            m_editorToolBar->setVisible(false);
    //            m_debuggerToolBar->setVisible(true);

    //            runAct->setEnabled(true);
    //            stepAct->setEnabled(true);
    //            stepOverAct->setEnabled(true);
    //            resetAct->setEnabled(true);
    //            pauseAct->setEnabled(false);
    //        }
    //    }

    //    protected void Run()
    //    {
    //        setStepActs();
    //        QTimer::singleShot(10, getCodeEditor(), SLOT(run()));
    //    }

    //    protected void EditorWindow::step()
    //    {
    //        setStepActs();
    //        QTimer::singleShot(10, getCodeEditor(), SLOT(step()));
    //        //getCodeEditor()->step( false ); 
    //    }

    //    protected void StepOver()
    //    {
    //        setStepActs();
    //        QTimer::singleShot(10, getCodeEditor(), SLOT(stepOver()));
    //        //getCodeEditor()->step( true ); 
    //    }

    //    protected void SetStepActs()
    //    {
    //        runAct->setEnabled(false);
    //        stepAct->setEnabled(false);
    //        stepOverAct->setEnabled(false);
    //        resetAct->setEnabled(false);
    //        pauseAct->setEnabled(true);
    //    }

    //    protected void Pause()
    //    {
    //        getCodeEditor()->pause();
    //        runAct->setEnabled(true);
    //        stepAct->setEnabled(true);
    //        stepOverAct->setEnabled(true);
    //        resetAct->setEnabled(true);
    //        pauseAct->setEnabled(false);
    //    }

    //    protected void Reset()
    //    {
    //        getCodeEditor()->reset();
    //    }

    //    protected void Stop()
    //    {
    //        getCodeEditor()->stopDebbuger();
    //        m_debuggerToolBar->setVisible(false);
    //        m_editorToolBar->setVisible(true);
    //    }

    //    protected void Compile()
    //    {
    //        getCodeEditor()->compile();
    //    }

    //    protected void Upload()
    //    {
    //        getCodeEditor()->upload();
    //    }

    //    protected void FindReplaceDialog()
    //    {
    //        CodeEditor* ce = getCodeEditor();

    //        findRepDiaWidget->setTextEdit(ce);

    //        QString text = ce->textCursor().selectedText();
    //        if (text != "") findRepDiaWidget->setTextToFind(text);

    //        findRepDiaWidget->show();
    //    }

        //    protected void ReadSettings()
        //    {
        //        QSettings* settings = MainWindow::self()->settings();
        //        restoreGeometry(settings->value("geometry").toByteArray());
        //        m_docWidget->restoreGeometry(settings->value("docWidget/geometry").toByteArray());
        //        m_lastDir = settings->value("lastDir").toString();
        //    }

        //    protected void EditorWindow::writeSettings()
        //    {
        //        QSettings* settings = MainWindow::self()->settings();
        //        settings->setValue("geometry", saveGeometry());
        //        settings->setValue("docWidget/geometry", m_docWidget->saveGeometry());
        //        settings->setValue("lastDir", m_lastDir);
        //    }

        //    protected QString StrippedName(const QString &fullFileName)
        //    {
        //        return QFileInfo(fullFileName).fileName();
        //    }

        //    protected void About()
        //    {
        //        /*QMessageBox::about(this, tr("About Application"),
        //                 tr(""));*/
        //        ;
        //    }



        //QGridLayout* baseWidgetLayout;
        //QTabWidget* m_docWidget;

        //FindReplaceDialog* findRepDiaWidget;

        protected string lastDir="";
        protected List<string> fileList = new List<string>();

    //QToolBar* m_editorToolBar;
    //QToolBar* m_debuggerToolBar;

    Action newAct;
    Action openAct;
    Action saveAct;
    Action saveAsAct;
    Action exitAct;
    Action aboutAct;
    Action aboutQtAct;
    Action undoAct;
    Action redoAct;

    Action cutAct;
    Action copyAct;
    Action pasteAct;

    Action debugAct;

    Action stepAct;
    Action stepOverAct;
    Action runAct;
    Action pauseAct;
    Action resetAct;
    Action stopAct;
    Action compileAct;
    Action loadAct;
    Action findQtAct;

        private void EditorNewButton_Click(object sender, RoutedEventArgs e) => NewFile();
        private void EditorOpenButton_Click(object sender, RoutedEventArgs e) => Open();
        private void EditorSaveButton_Click(object sender, RoutedEventArgs e) => Save();
        private void EditorSaveAsButton_Click(object sender, RoutedEventArgs e) => SaveAs();

        private void TabItemButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void EditorFindButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
