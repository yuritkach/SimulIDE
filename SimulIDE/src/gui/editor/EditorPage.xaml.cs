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
//            CreateToolBars();
//            ReadSettings();
        }



    //    public bool Close()
    //    {
    //        WriteSettings();
    //        for (int i = 0; i < m_docWidget.Count(); i++)
    //        {
    //            CloseTab(m_docWidget[i]);
    //        }
    //        return MaybeSave();
    //    }

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
            CodeEditor editor = new CodeEditor();
            TabItem item = new TabItem();
            Frame pageFrame = new Frame();
            item.Header = "New";
            pageFrame.Content = editor;
            item.Content = pageFrame;
            var index=tabControl.Items.Add(item);
            tabControl.SelectedIndex = index;

            //        connect(baseWidget->m_codeEditor->document(), SIGNAL(contentsChanged()),
            //             this, SLOT(documentWasModified()));

            //        m_fileList << "New";
            EnableFileActs(true);
            EnableDebugActs(true);
        }

        protected void Open()
        {
    //        const string dir = m_lastDir;
    //        string fileName = FileDialog::getOpenFileName(this, tr("Load File"), dir,
    //                       tr("All files") + " (*);;Arduino (*.ino);;Asm (*.asm);;GcBasic (*.gcb)");

    //        if (fileName!="") LoadFile(fileName);
        }

    //    protected void LoadFile(string fileName )
    //    {
    //        if (m_fileList.contains(fileName))
    //        {
    //            m_docWidget->setCurrentIndex(m_fileList.indexOf(fileName));
    //            //return;
    //        }
    //        else newFile();

            
    //        //Cursor.Current = Cursors.WaitCursor;

    //        CodeEditor* ce = getCodeEditor();
    //        ce->setPlainText(fileToString(fileName, "EditorWindow"));
    //        ce->setFile(fileName);

    //        m_lastDir = fileName;
    //        int index = m_docWidget->currentIndex();
    //        m_fileList.replace(index, fileName);
    //        m_docWidget->setTabText(index, strippedName(fileName));
    //        enableFileActs(true);   // enable file actions
    //                            //if( ce->hasDebugger() )
    //        enableDebugActs(true);

    //    //Cursor.Current = Cursors.DefaULT;
    //    }

    //    protected void Reload()
    //    {
    //        string fileName = m_fileList.at(m_docWidget->currentIndex());
    //        LoadFile(fileName);
    //    }

    //    protected bool Save()
    //    {
    //        QString file = getCodeEditor()->getFilePath();
    //        if (file.isEmpty()) return saveAs();
    //        else return saveFile(file);
    //    }

    //    protected bool SaveAs()
    //    {
    //        CodeEditor* ce = getCodeEditor();

    //        QFileInfo fi = QFileInfo(ce->getFilePath());
    //        QString ext = fi.suffix();
    //        QString path = fi.absolutePath();
    //        if (path == "") path = m_lastDir;
    //        //qDebug() << "EditorWindow::saveAs" << path;

    //        QString extensions = "";
    //        if (ext == "") extensions = tr("All files") + " (*);;Arduino (*.ino);;Asm (*.asm);;GcBasic (*.gcb)";
    //        else extensions = "." + ext + "(*." + ext + ");;" + tr("All files") + " (*.*)";
    
    //        QString fileName = QFileDialog::getSaveFileName(this, tr("Save Document As"), path, extensions);
    //        if (fileName.isEmpty()) return false;

    //        m_fileList.replace(m_docWidget->currentIndex(), fileName);

    //        return saveFile(fileName);
    //    }

    //    protected bool SaveFile(const QString &fileName)
    //    {
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
    //        return true;
    //    }

    //    protected bool MaybeSave()
    //    {
    //        if (m_fileList.isEmpty()) return true;
    //        if (getCodeEditor()->document()->isModified())
    //        {
    //            QMessageBox::StandardButton ret;
    //            ret = QMessageBox::warning(this, "EditorWindow::saveFile",
    //              tr("\nThe Document has been modified.\nDo you want to save your changes?\n"),
    //              QMessageBox::Save | QMessageBox::Discard | QMessageBox::Cancel);

    //            if (ret == QMessageBox::Save) return save();
    //            else if (ret == QMessageBox::Cancel) return false;
    //        }
    //        return true;
    //    }

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
//            newAct = new Action( QIcon(":/new.png"), tr("&New\tCtrl+N"), this);
    //        newAct->setStatusTip(tr("Create a new file"));
    //        connect(newAct, SIGNAL(triggered()), this, SLOT(newFile()));

    //        openAct = new QAction(QIcon(":/open.png"), tr("&Open...\tCtrl+O"), this);
    //        openAct->setStatusTip(tr("Open an existing file"));
    //        connect(openAct, SIGNAL(triggered()), this, SLOT(open()));

    //        saveAct = new QAction(QIcon(":/save.png"), tr("&Save\tCtrl+S"), this);
    //        saveAct->setStatusTip(tr("Save the document to disk"));
    //        saveAct->setEnabled(false);
    //        connect(saveAct, SIGNAL(triggered()), this, SLOT(save()));

    //        saveAsAct = new QAction(QIcon(":/saveas.png"), tr("Save &As...\tCtrl+Shift+S"), this);
    //        saveAsAct->setStatusTip(tr("Save the document under a new name"));
    //        saveAsAct->setEnabled(false);
    //        connect(saveAsAct, SIGNAL(triggered()), this, SLOT(saveAs()));

    //        exitAct = new QAction(QIcon(":/exit.png"), tr("E&xit"), this);
    //        exitAct->setStatusTip(tr("Exit the application"));
    //        connect(exitAct, SIGNAL(triggered()), this, SLOT(close()));

    //        cutAct = new QAction(QIcon(":/cut.png"), tr("Cu&t\tCtrl+X"), this);
    //        cutAct->setStatusTip(tr("Cut the current selection's contents to the clipboard"));
    //        cutAct->setEnabled(false);
    //        connect(cutAct, SIGNAL(triggered()), this, SLOT(cut()));

    //        copyAct = new QAction(QIcon(":/copy.png"), tr("&Copy\tCtrl+C"), this);
    //        copyAct->setStatusTip(tr("Copy the current selection's contents to the clipboard"));
    //        copyAct->setEnabled(false);
    //        connect(copyAct, SIGNAL(triggered()), this, SLOT(copy()));

    //        pasteAct = new QAction(QIcon(":/paste.png"), tr("&Paste\tCtrl+V"), this);
    //        pasteAct->setStatusTip(tr("Paste the clipboard's contents into the current selection"));
    //        pasteAct->setEnabled(false);
    //        connect(pasteAct, SIGNAL(triggered()), this, SLOT(paste()));

    //        undoAct = new QAction(QIcon(":/undo.png"), tr("Undo\tCtrl+Z"), this);
    //        undoAct->setStatusTip(tr("Undo the last action"));
    //        undoAct->setEnabled(false);
    //        connect(undoAct, SIGNAL(triggered()), this, SLOT(undo()));

    //        redoAct = new QAction(QIcon(":/redo.png"), tr("Redo\tCtrl+Shift+Z"), this);
    //        redoAct->setStatusTip(tr("Redo the last action"));
    //        redoAct->setEnabled(false);
    //        connect(redoAct, SIGNAL(triggered()), this, SLOT(redo()));

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

    //    protected CodeEditor* getCodeEditor()
    //    {
    //        CodeEditorWidget* actW = dynamic_cast<CodeEditorWidget*>(m_docWidget->currentWidget());
    //        if (actW) return actW->m_codeEditor;
    //        else return 0l;
    //    }

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

        protected void CreateToolBars()
        {
            //m_editorToolBar->addAction(newAct);
            //m_editorToolBar->addAction(openAct);
            //m_editorToolBar->addAction(saveAct);
            //m_editorToolBar->addAction(saveAsAct);
            //m_editorToolBar->addSeparator();

    //        /*m_editorToolBar->addAction(cutAct);
    //        m_editorToolBar->addAction(copyAct);
    //        m_editorToolBar->addAction(pasteAct);
    //        m_editorToolBar->addSeparator();
    //        m_editorToolBar->addAction(undoAct);
    //        m_editorToolBar->addAction(redoAct);*/
    
            //m_editorToolBar->addAction(findQtAct);
            //m_editorToolBar->addSeparator();

            //m_editorToolBar->addAction(compileAct);
            //m_editorToolBar->addAction(loadAct);
            //m_editorToolBar->addSeparator();

            //m_editorToolBar->addAction(debugAct);

            //m_debuggerToolBar->addAction(stepAct);
            //m_debuggerToolBar->addAction(stepOverAct);
            //m_debuggerToolBar->addAction(runAct);
            //m_debuggerToolBar->addAction(pauseAct);
            //m_debuggerToolBar->addAction(resetAct);
            //m_debuggerToolBar->addSeparator();
            //m_debuggerToolBar->addAction(stopAct);
        }

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

    //QString m_lastDir;
    //QStringList m_fileList;

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



     
        private void TabItemButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

    }
}
