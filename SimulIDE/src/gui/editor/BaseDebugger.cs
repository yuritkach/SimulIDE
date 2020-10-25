using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimulIDE.src.gui.editor
{
    class BaseDebugger
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

            //connect(&m_compProcess, SIGNAL(readyRead()), SLOT(ProcRead()));

        }

        protected List<string> GetVarList()
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
        public string CompilerPath
        {
            get
            {
                return compilerPath;
            }
            set
            {
                compilerPath = value;
                compilerPath = QFileDialog::getExistingDirectory(0L,
                           tr("Select Compiler toolchain directory"),
                           m_compilerPath,
                           QFileDialog::ShowDirsOnly
                         | QFileDialog::DontResolveSymlinks);

                if (m_compilerPath != "") m_compilerPath += "/";

                MainWindow::self()->settings()->setValue(m_compSetting, m_compilerPath);

                m_outPane->appendText("\n" + tr("Using Compiler Path: ") + "\n");
                m_outPane->writeText(m_compilerPath + "\n\n");

            }
        }
        public bool DriveCircuit { get; set; }

        protected TextBox outPane;

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

        
        protected string compSetting;

        List<string> varNames = new List<string>();
        List<string> subs = new List<string>();
        List<int> subLines = new List<int>();

        Dictionary<string, string> typesList = new Dictionary<string, string>();
        Dictionary<string, string> varList = new Dictionary<string, string>();
        Dictionary<int, int> flashToSource = new Dictionary<int, int>(); // Map flash adress to Source code line
        Dictionary<int, int> sourceToFlash = new Dictionary<int, int>(); // Map .asm code line to flash adress
        
        //QProcess m_compProcess;
    }
}

//#include "mcucomponent.h"

class BaseDebugger : public QObject    // Base Class for all debuggers
{
    Q_OBJECT
    Q_PROPERTY(bool Drive_Circuit READ driveCirc    WRITE setDriveCirc    DESIGNABLE true USER true )
    Q_PROPERTY(QString Compiler_Path READ compilerPath WRITE setCompilerPath DESIGNABLE true USER true )


bool driveCirc();
void setDriveCirc(bool drive);


virtual void setCompilerPath(QString path);

virtual bool loadFirmware();
virtual void upload();
virtual int step();        // Run 1 step,returns current source line number
virtual int stepOver();    // Run until next source file
virtual void stop();

virtual void getProcName();
virtual void setProcType(int type) { m_processorType = type; }

virtual int compile()=0;
        virtual void mapFlashToSource()=0;
        
        virtual int getValidLine(int line);

virtual void getCompilerPath();

virtual void readSettings();

virtual QString getVarType(QString var);

virtual QStringList getVarList();

virtual QList<int> getSubLines() { return m_subLines; }

int type;

public slots:
        void ProcRead();

protected:

};

#include "basedebugger.h"
#include "baseprocessor.h"
#include "editorwindow.h"
#include "mainwindow.h"
#include "simulator.h"

static const char* BaseDebugger_properties[] = {
    QT_TRANSLATE_NOOP("App::Property","Drive Circuit"),
    QT_TRANSLATE_NOOP("App::Property","Compiler Path")
};

bool BaseDebugger::m_loadStatus = false;

BaseDebugger::BaseDebugger(QObject* parent, OutPanelText* outPane, QString filePath) 
            : QObject(parent )
            , m_compProcess( 0l )
{
    Q_UNUSED(BaseDebugger_properties);

   
}
BaseDebugger::~BaseDebugger()
{
    if (BaseProcessor::self()) BaseProcessor::self()->getRamTable()->remDebugger(this);
}

bool BaseDebugger::loadFirmware()
{
    if (m_firmware == "") return false;

    upload();
    if (m_loadStatus) return false;

    m_loadStatus = true;

    return true;
}

void BaseDebugger::upload()
{
    if (m_loadStatus)
    {
        QMessageBox::warning(0, "BaseDebugger::loadFirmware",
                                tr("Debugger already running") + "\n" + tr("Stop active session"));
        return;
    }
    m_outPane->writeText("-------------------------------------------------------\n");
    m_outPane->appendText("\n" + tr("Uploading: ") + "\n");
    m_outPane->appendText(m_firmware);
    m_outPane->writeText("\n\n");

    if (McuComponent::self())
    {
        McuComponent::self()->load(m_firmware);
        m_outPane->appendText("\n" + tr("FirmWare Uploaded to ") + McuComponent::self()->device() + "\n");
        m_outPane->writeText("\n\n");

        BaseProcessor::self()->getRamTable()->setDebugger(this);
        mapFlashToSource();
    }
    else m_outPane->writeText("\n" + tr("Error: No Mcu in Simulator... ") + "\n");
}

void BaseDebugger::stop()
{
    m_loadStatus = false;
}

void BaseDebugger::getProcName()
{
}

int BaseDebugger::step()
{
    int pc = BaseProcessor::self()->pc();

    int i = 0;
    for (i = 0; i < 10; i++) // If runs 10 times and get to same PC return 0
    {
        BaseProcessor::self()->stepOne();

        int pc2 = BaseProcessor::self()->pc();
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
    else if (m_flashToSource.contains(pc)) line = m_flashToSource[pc];

    return line;
}

int BaseDebugger::stepOver() { return 0; }

int BaseDebugger::getValidLine(int line)
{
    while (!m_sourceToFlash.contains(line) && line <= m_lastLine) line++;
    return line;
}

QString BaseDebugger::getVarType(QString var)
{
    var = var.toUpper();
    return m_varList[var];
}


void BaseDebugger::ProcRead()
{
    while (m_compProcess.canReadLine())
    {
        m_outPane->appendText(QString::fromLocal8Bit(m_compProcess.readLine()));
        m_outPane->writeText("\n");
    }
}

void BaseDebugger::readSettings()
{
    QSettings* settings = MainWindow::self()->settings();

    if (settings->contains(m_compSetting))
        m_compilerPath = settings->value(m_compSetting).toString();
}

void BaseDebugger::getCompilerPath()
{
    
}

bool BaseDebugger::driveCirc()
{
    CodeEditor* ce = EditorWindow::self()->getCodeEditor();
    return ce->driveCirc();
}

void BaseDebugger::setDriveCirc(bool drive)
{
    CodeEditor* ce = EditorWindow::self()->getCodeEditor();
    ce->setDriveCirc(drive);
}



