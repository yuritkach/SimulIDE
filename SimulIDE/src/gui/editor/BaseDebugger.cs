using SimulIDE.src.gui.circuitwidget.components.mcu;
using SimulIDE.src.simulator.elements.processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TextBox = System.Windows.Controls.TextBox;

//#include "basedebugger.h"
//#include "baseprocessor.h"
//#include "editorwindow.h"
//#include "mainwindow.h"
//#include "simulator.h"



namespace SimulIDE.src.gui.editor
{
    public class BaseDebugger
    {
        public BaseDebugger(object parent, TextBox outPane, string filePath)
        {
            this.outPane = outPane;
            appPath = AppDomain.CurrentDomain.BaseDirectory;

            fileDir = System.IO.Path.GetDirectoryName(filePath);
            fileExt = System.IO.Path.GetExtension(filePath);
            fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            processorType = 0;
            type = 0;
            loadStatus = false;

            //connect(&m_compProcess, SIGNAL(readyRead()), SLOT(ProcRead()));

        }

        public List<string> GetVarList()
        {
            return varNames;
        }

        protected void ToolChainNotFound()
        {
            outPane.AppendText(": ToolChain not found"+"\n");
            outPane.AppendText("\n Right-Click on Document Tab to set Path\n\n");
            //QApplication::restoreOverrideCursor();
        }

        protected virtual void getSubs() {; }

        protected string compilerPath;
        public string CompilerPath{ get { return compilerPath; } }

        public void SetCompilerPath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    compilerPath = fbd.SelectedPath;
                    //MainWindow::self()->settings()->setValue(m_compSetting, m_compilerPath);

                    outPane.AppendText("\nUsing Compiler Path: \n");
                    outPane.AppendText(compilerPath + "\n\n");
                }
                else compilerPath = "";
            }
        }

        public bool LoadFirmware()
        {
            if (firmware == "") return false;

            Upload();
            if (loadStatus) return false;

            loadStatus = true;
            return true;
        }

        public void Upload()
        {
            if (loadStatus)
            {
                MessageBox.Show("Debugger already running\nStop active session");
                return;
            }
            outPane.AppendText("-------------------------------------------------------\n");
            outPane.AppendText("\nUploading: \n");
            outPane.AppendText(firmware);
            outPane.AppendText("\n\n");

            if (McuComponent.Self()!=null)
            {
//                McuComponent.Self().Load(firmware);
//                outPane.AppendText("\nFirmWare Uploaded to " + McuComponent.Self().Device() + "\n");
//                outPane.AppendText("\n\n");

//                BaseProcessor.Self().GetRamTable().SetDebugger(this);
//                MapFlashToSource();
            }
            else outPane.AppendText("\nError: No Mcu in Simulator... \n");
        }

        public void Stop()
        {
            loadStatus = false;
        }

        void GetProcName(){}

        int Step()
        {
            int pc = BaseProcessor.Self().PC();

            int i = 0;
            for (i = 0; i < 10; i++) // If runs 10 times and get to same PC return 0
            {
                BaseProcessor.Self().StepOne();

                int pc2 = BaseProcessor.Self().PC();
                //qDebug() <<"BaseDebugger::step "<<pc<<pc2;
                if (pc != pc2)
                {
                    pc = pc2;
                    break;
                }
            }
            //qDebug() <<"BaseDebugger::step PC"<<pc<<i;
            int line = -1;
            if (i == 10) line = 0;        // It ran 10 times and get to same PC 
            else if (flashToSource.ContainsKey(pc)) line = flashToSource[pc];

            return line;
        }

        public int StepOver() { return 0; }

        public int GetValidLine(int line)
        {
            while (!sourceToFlash.ContainsKey(line) && line <= lastLine) line++;
            return line;
        }

        public string GetVarType(string var)
        {
            var = var.ToUpper();
            return varList[var];
        }


        public void ProcRead()
        {
//            while (compProcess.CanReadLine())
            {
//                outPane.AppendText(QString::fromLocal8Bit(m_compProcess.readLine()));
                outPane.AppendText("\n");
            }
        }

        public void ReadSettings()
        {
//            QSettings* settings = MainWindow::self()->settings();

//            if (settings->contains(m_compSetting))
//                m_compilerPath = settings->value(m_compSetting).toString();
        }

  
        public bool DriveCirc()
        {
//            CodeEditor ce = EditorWindow.Self().GetCodeEditor();
//            return ce.DriveCirc();
            return true; //tyv
        }

        public void SetDriveCirc(bool drive)
        {
//            CodeEditor ce = EditorWindow.Self().GetCodeEditor();
//            ce.SetDriveCirc(drive);
        }





        public bool DriveCircuit { get; set; }

        protected System.Windows.Controls.TextBox outPane;

        protected static bool loadStatus;                          // Is symbol file loaded?
        protected bool running;                             // Is processor running?
        protected int processorType;
        protected int lastLine;
        protected string device;
        protected string appPath;
        protected string firmware;
        protected string fileDir;
        protected string fileName;
        protected string fileExt;
        public int type;

        public virtual int Compile() { return -1; }
        protected string compSetting;

        List<string> varNames = new List<string>();
        List<string> subs = new List<string>();
        List<int> subLines = new List<int>();

        protected Dictionary<string, string> typesList = new Dictionary<string, string>();
        protected Dictionary<string, string> varList = new Dictionary<string, string>();
        protected Dictionary<int, int> flashToSource = new Dictionary<int, int>(); // Map flash adress to Source code line
        protected Dictionary<int, int> sourceToFlash = new Dictionary<int, int>(); // Map .asm code line to flash adress
        
        //QProcess m_compProcess;
    }
}






