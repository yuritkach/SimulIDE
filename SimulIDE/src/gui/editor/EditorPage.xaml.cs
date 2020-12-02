using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// 
    

    public partial class EditorPage : Page
    {
        private static EditorPage editorPage=null;
        public static EditorPage Self() { return editorPage; }

        public delegate void MyEventHandler(object obj);
        protected void DocumentWasChanged(object obj)
        {
            CodeEditor ce = GetCodeEditor();
            bool modified = ce.IsModified;
            int index = tabControl.SelectedIndex;
            string tabText = ((TabItem)tabControl.Items[index]).Header.ToString();

            if (modified && !tabText.EndsWith("*")) tabText+="*";
            else if (!modified && tabText.EndsWith("*")) tabText=tabText.Substring(0,tabText.Length-1);

            ((TabItem)tabControl.Items[index]).Header = tabText; 
            //        redoAct->setEnabled(false);
            //        undoAct->setEnabled(false);
            //        if (doc->isRedoAvailable()) redoAct->setEnabled(true);
            //        if (doc->isUndoAvailable()) undoAct->setEnabled(true);
            ce.Compiled=false;
        }

        public EditorPage()
        {
            InitializeComponent();
            editorPage = this;
            CreateWidgets();
            CreateActions();
            ReadSettings();
        }



        public bool Close()
        {
            WriteSettings();
            for (int i = 0; i < tabControl.Items.Count; i++)CloseTabSheet(i);
            return MaybeSave();
        }

        protected void NewFile()
        {
            CodeEditorWidget editorView = new CodeEditorWidget();
            TabItem item = new TabItem();
            item.Header = "New";
            item.Content = editorView;
            tabControl.SelectedIndex = tabControl.Items.Add(item);
            CodeEditor ce = GetCodeEditor();
            ((CodeEditor)editorView.EditorPanel.Children[0]).OnDocumentChanged += DocumentWasChanged;
            fileList.Add("New");
            EnableFileActs(true);
            EnableDebugActs(true);
      //      TextRange doc = new TextRange(ce.Document.ContentStart, ce.Document.ContentEnd);
   //         doc.ApplyPropertyValue(Paragraph.MarginProperty, new Thickness(0));
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
            ce.SetFile(fileName);

            ce.IsModified = false;
            ce.FileName=fileName;
            lastDir = System.IO.Path.GetDirectoryName(fileName);
            int index = tabControl.SelectedIndex;
            fileList[index] = fileName;
            ((TabItem)tabControl.Items[index]).Header = System.IO.Path.GetFileName(fileName);
            EnableFileActs(true);
            if(ce.HasDebugger())
                EnableDebugActs(true);

            //TextRange doc = new TextRange(ce.Document.ContentStart, ce.Document.ContentEnd);
           // doc.ApplyPropertyValue(Paragraph.MarginProperty, new Thickness(0));
            //Cursor.Current = Cursors.DefaULT;
        }

        protected void Reload()
        {
            LoadFile(fileList[tabControl.SelectedIndex]);
        }

        public bool Save()
        {
                string file = GetCodeEditor().FileName??"";
                if (file=="") return SaveAs();
                else return SaveFile(file);
        }

        public bool SaveAs()
        {
            CodeEditor ce = GetCodeEditor();
            string fileName = ce.FileName;
            string ext = System.IO.Path.GetExtension(fileName);
            string path = System.IO.Path.GetDirectoryName(fileName);
            if (path == "")
                path = lastDir;
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
            CodeEditor ce = GetCodeEditor();
            System.IO.File.WriteAllText(fileName, ce.GetPlainText());
            ((TabItem)tabControl.Items[tabControl.SelectedIndex]).Header = System.IO.Path.GetFileName(fileName);
            ce.IsModified=false;
            return true;
        }

        protected bool MaybeSave()
        {
            if (fileList.Count==0) return true;
            if (GetCodeEditor().IsModified)
            {
                MessageBoxResult dialogResult = MessageBox.Show("\nThe Document has been modified.\nDo you want to save your changes?\n",
                    "Save file", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    Save();
                    return true;
                }
                else if (dialogResult == MessageBoxResult.No) return false;
            }
            return true;
        }
       
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
            editorDebugButton.IsEnabled = enable;
            debuggerRunToBKButton.IsEnabled = enable;
            debuggerStepButton.IsEnabled = enable;
            //        stepOverAct->setEnabled(enable);
            debuggerPauseButton.IsEnabled = enable;
            debuggerResetButton.IsEnabled = enable;
            debuggerStopButton.IsEnabled = enable;
            editorCompileButton.IsEnabled = enable;
            editorLoadButton.IsEnabled = enable;
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

        protected  void SetCompiler()
        {
            CodeEditor ce = GetCodeEditor();
            if (ce!=null) ce.SetCompilerPath();
        }

        protected void CreateWidgets()
        {
              debuggerToolBar.Visibility=Visibility.Hidden;
    //        connect(m_docWidget, SIGNAL(tabCloseRequested(int)),this, SLOT(closeTab(int)));
    //        connect(m_docWidget, SIGNAL(customContextMenuRequested(const QPoint &)),this,SLOT(tabContextMenu(const QPoint &)));
    //        connect(m_docWidget, SIGNAL(currentChanged(int)), this, SLOT(tabChanged(int)));

    //        findRepDiaWidget = new FindReplaceDialog(this);
    //        findRepDiaWidget->setModal(false);
        }

        protected void CreateActions()
        {
            editorSaveButton.IsEnabled = false;
            editorSaveAsButton.IsEnabled = false;
            editorFindButton.IsEnabled = false;
            editorCompileButton.IsEnabled = false;
            editorLoadButton.IsEnabled = false;

            debuggerRunToBKButton.IsEnabled = false;

            //        exitAct = new QAction(QIcon(":/exit.png"), tr("E&xit"), this);
            //        exitAct->setStatusTip(tr("Exit the application"));
            //        connect(exitAct, SIGNAL(triggered()), this, SLOT(close()));

            debuggerStepButton.IsEnabled = false;
            debuggerStepOverButton.IsEnabled = false;
            debuggerStepOverButton.Visibility = Visibility.Hidden;

            debuggerPauseButton.IsEnabled = false;
            debuggerResetButton.IsEnabled = false;
            debuggerStopButton.IsEnabled = false;
            editorDebugButton.IsEnabled = false;

            //        /*aboutAct = new QAction(QIcon(":/info.png"),tr("&About"), this);
            //        aboutAct->setStatusTip(tr("Show the application's About box"));
            //        connect(aboutAct, SIGNAL(triggered()), this, SLOT(about()));*/

            //        /*aboutQtAct = new QAction(QIcon(":/info.png"),tr("About &Qt"), this);
            //        aboutQtAct->setStatusTip(tr("Show the Qt library's About box"));
            //        connect(aboutQtAct, SIGNAL(triggered()), qApp, SLOT(aboutQt()));*/

            //        //connect(m_codeEditor, SIGNAL(copyAvailable(bool)), cutAct, SLOT(setEnabled(bool)));
            //        //connect(m_codeEditor, SIGNAL(copyAvailable(bool)), copyAct, SLOT(setEnabled(bool)));
        }

        public void EnableStepOver(bool en)
        {
            debuggerStepOverButton.Visibility =en? Visibility.Visible:Visibility.Hidden;
        }

        protected CodeEditor GetCodeEditor()
        {
            return (CodeEditor)((CodeEditorWidget)((TabItem)tabControl.Items[tabControl.SelectedIndex]).Content).EditorPanel.Children[0];
        }

        protected void CloseTabSheet(int index)
        {
            tabControl.SelectedIndex=index;
            if (!MaybeSave()) return;

            fileList.RemoveAt(index);

            if (fileList.Count==0)
            {
                EnableFileActs(false); 
                EnableDebugActs(false);
            }
            if (debuggerToolBar.Visibility==Visibility.Visible) Stop();

            tabControl.Items.RemoveAt(index);
    
            int last = tabControl.Items.Count - 1;
            if (index > last) tabControl.SelectedIndex=last;
            else tabControl.SelectedIndex = index;
       }

        protected void Cut() { GetCodeEditor().Cut(); }
        protected void Copy() { GetCodeEditor().Copy(); }
        protected void Paste() { GetCodeEditor().Paste(); }
        protected void Undo() { GetCodeEditor().Undo(); }
        protected void Redo() { GetCodeEditor().Redo(); }

        protected void Debug()
        {
            CodeEditor ce = GetCodeEditor();

            if (ce.InitDebbuger())
            {
                editorToolBar.Visibility = Visibility.Hidden;
                debuggerToolBar.Visibility=Visibility.Visible;
                debuggerRunToBKButton.IsEnabled = true;
                debuggerStepButton.IsEnabled = true;
                debuggerStepOverButton.IsEnabled = true;
                debuggerResetButton.IsEnabled = true;
                debuggerPauseButton.IsEnabled = false;
            }
        }

        protected void Run()
        {
            SetStepActs();
            //        QTimer::singleShot(10, getCodeEditor(), SLOT(run()));
            GetCodeEditor().Run();
        }

        protected void Step()
        {
            SetStepActs();
    //        QTimer::singleShot(10, getCodeEditor(), SLOT(step()));
             GetCodeEditor().Step( false ); 
        }

        protected void StepOver()
        {
            SetStepActs();
    //        QTimer::singleShot(10, getCodeEditor(), SLOT(stepOver()));
            GetCodeEditor().Step( true ); 
        }

        protected void SetStepActs()
        {
            debuggerRunToBKButton.IsEnabled = false;
            debuggerStepButton.IsEnabled = false;
            debuggerStepOverButton.IsEnabled = false;
            debuggerResetButton.IsEnabled = false;
            debuggerPauseButton.IsEnabled = true;
        }

        public void Pause()
        {
            GetCodeEditor().Pause();
            debuggerRunToBKButton.IsEnabled = true;
            debuggerStepButton.IsEnabled = true;
            debuggerStepOverButton.IsEnabled = true;
            debuggerResetButton.IsEnabled = true;
            debuggerPauseButton.IsEnabled = true;
        }

        protected void Reset()
        {
            GetCodeEditor().Reset();
        }

        protected void Stop()
        {
            GetCodeEditor().StopDebbuger();
            debuggerToolBar.Visibility=Visibility.Hidden;
            editorToolBar.Visibility=Visibility.Visible;
        }

        protected void Compile()
        {
            GetCodeEditor().Compile();
        }

        protected void Upload()
        {
            GetCodeEditor().Upload();
        }

        protected void FindReplaceDialog()
        {
    //        CodeEditor* ce = getCodeEditor();

    //        findRepDiaWidget->setTextEdit(ce);

    //        QString text = ce->textCursor().selectedText();
    //        if (text != "") findRepDiaWidget->setTextToFind(text);

    //        findRepDiaWidget->show();
        }

            protected void ReadSettings()
            {
        //        QSettings* settings = MainWindow::self()->settings();
        //        restoreGeometry(settings->value("geometry").toByteArray());
        //        m_docWidget->restoreGeometry(settings->value("docWidget/geometry").toByteArray());
        //        m_lastDir = settings->value("lastDir").toString();
            }

            protected void WriteSettings()
            {
        //        QSettings* settings = MainWindow::self()->settings();
        //        settings->setValue("geometry", saveGeometry());
        //        settings->setValue("docWidget/geometry", m_docWidget->saveGeometry());
        //        settings->setValue("lastDir", m_lastDir);
            }
       

        //    protected void About()
        //    {
        //        /*QMessageBox::about(this, tr("About Application"),
        //                 tr(""));*/
        //        ;
        //    }


        //FindReplaceDialog* findRepDiaWidget;

        protected string lastDir="";
        protected List<string> fileList = new List<string>();

        private void EditorNewButton_Click(object sender, RoutedEventArgs e) => NewFile();
        private void EditorOpenButton_Click(object sender, RoutedEventArgs e) => Open();
        private void EditorSaveButton_Click(object sender, RoutedEventArgs e) => Save();
        private void EditorSaveAsButton_Click(object sender, RoutedEventArgs e) => SaveAs();
        private void EditorFindButton_Click(object sender, RoutedEventArgs e) => FindReplaceDialog();
        private void EditorCompileButton_Click(object sender, RoutedEventArgs e) => Compile();
        private void EditorLoadButton_Click(object sender, RoutedEventArgs e) => Upload();
        private void EditorDebugButton_Click(object sender, RoutedEventArgs e) => Debug();
        private void DebuggerStepButton_Click(object sender, RoutedEventArgs e) => Step();
        private void DebuggerStepOverButton_Click(object sender, RoutedEventArgs e) => StepOver();
        private void DebuggerRunToBKButton_Click(object sender, RoutedEventArgs e) => Run(); 
        private void DebuggerPauseButton_Click(object sender, RoutedEventArgs e) => Pause();
        private void DebuggerResetButton_Click(object sender, RoutedEventArgs e) => Reset();
        private void DebuggerStopButton_Click(object sender, RoutedEventArgs e) => Stop();

        private void TabItemButton_Click(object sender, RoutedEventArgs e)
        {
            string tabText = (string)(sender as Button).DataContext;
            if (tabText.EndsWith("*"))
                tabText = tabText.Substring(0, tabText.Length - 1);
            int index = fileList.IndexOf(tabText);
            CloseTabSheet(index);
        }

        
        
    }
}
