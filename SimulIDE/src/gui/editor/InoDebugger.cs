using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimulIDE.src.gui.editor
{
    public class InoDebugger:BaseDebugger,INamedObject
    {


        //    static const char* InoDebugger_properties[] = {
        //QT_TRANSLATE_NOOP("App::Property","Board"),
        //QT_TRANSLATE_NOOP("App::Property","Custom Board")
        //};
        public InoDebugger(object parent, TextBox outPane, string filePath):base(parent, outPane, filePath )
        {
            //Q_UNUSED(InoDebugger_properties);
            SetObjectName("Arduino Compiler/Debugger");
            compilerPath = "";
            compSetting = "arduino_Path";
            ReadSettings();
            board = BoardType.Uno;

            typesList["char"] = "int8";
            typesList["uchar"] = "uint8";
            typesList["int"] = "int16";
            typesList["uint"] = "uint16";
            typesList["short"] = "int16";
            typesList["ushort"] = "uint16";
            typesList["long"] = "int32";
            typesList["ulong"] = "uint32";
            typesList["float"] = "float32";
        }
        

        public void Upload()
        {
            //TYV            string circDir = Circuit.Self().GetFileName();
            string firmPath = firmware;

            //TYV            if (circDir != "")
            //TYV            {
            //TYVQDir circuitDir = QFileInfo(circDir).absoluteDir();
            //TYVm_firmware = circuitDir.absolutePath() + "/" + m_fileName + ".hex";
            //TYV//qDebug() <<"InoDebugger::upload"<<m_firmware<<firmPath;
            //TYVcircuitDir.remove(m_fileName + ".hex");
            //TYVQFile::copy(firmPath, m_firmware);
            //TYV}

            base.Upload();
            firmware = firmPath;
        }

        public int Compile()
        {
            //           // QApplication::setOverrideCursor(Qt::WaitCursor);

            //            QDir arduinoDir(m_compilerPath );
            //            if (!arduinoDir.exists())
            //            {
            //                m_outPane->appendText("\nArduino");
            //                toolChainNotFound();
            //                return -1;
            //            }
            //            QString filePath = m_fileDir + m_fileName + m_fileExt;
            //            // Doing build in the user directory
            //            QString buildPath = SIMUAPI_AppPath::self()->RWDataFolder().absoluteFilePath("codeeditor/buildIno");

            //            QDir dir(buildPath);
            //            dir.removeRecursively();                         // Remove old files
            //            dir.mkpath(buildPath + "/" + m_fileName);        // Create sketch folder

            //            QDir directory(m_fileDir );
            //            QStringList fileList = directory.entryList(QDir::Files);

            //            for (QString fileName : fileList) // Copy files to sketch folder
            //            {
            //                QFile::copy(m_fileDir + fileName, buildPath + "/" + m_fileName + "/" + fileName);
            //            }
            //            QString ProcInoFile = buildPath + "/" + m_fileName + "/" + m_fileName + m_fileExt;
            //            QFile file(ProcInoFile );

            //            if (!file.open(QFile::WriteOnly | QFile::Text))
            //            {
            //                QMessageBox::warning(0l, "InoDebugger::compile",
            //                tr("Cannot write file %1:\n%2.").arg(ProcInoFile).arg(file.errorString()));
            //                return -1;
            //            }
            //            QTextStream out(&file);
            //    out.setCodec("UTF-8");

            //            QStringList inoLines = fileToStringList(filePath, "InoDebugger::compile");
            //            QString line;
            //            int inoLineNumber = 0;

            //            m_varList.clear();
            //            for (QString inoLine : inoLines)                        // Get Variables
            //            {
            //                line = inoLine;
            //                line = line.replace("\t", " ").remove(";");
            //                QStringList wordList = line.split(" ");
            //                wordList.removeAll("");
            //                if (!wordList.isEmpty())      // Fix crash on empty list.first()
            //                {
            //                    QString type = wordList.takeFirst();
            //                    if (type == "unsigned") type = "u" + wordList.takeFirst();

            //                    if (m_typesList.contains(type))
            //                    {
            //                        QString varName = wordList.at(0);
            //                        if (!m_varList.contains(varName))
            //                            m_varList[varName] = m_typesList[type];

            //                        //qDebug() << "InoDebugger::compile  variable "<<type<<varName<<m_typesList[ type ];
            //                    }
            //                }
            //                if (inoLine.contains("loop()")) m_loopInoLine = inoLineNumber;
            //                inoLineNumber++;

            //        out << inoLine << " // INOLINE " << inoLineNumber << "\n";
            //            }
            //            file.close();

            //            ///TODO: verify arduino version, older versions can compile, but no sorce code emited into .lst file
            //            /// , then debugger will hang!
            //            QString cBuildPath = buildPath;
            //            QString preferencesPath = SIMUAPI_AppPath::self()->availableDataFilePath("codeeditor/preferences.txt");
            //            QString command = m_compilerPath + "arduino";

            //# ifndef Q_OS_UNIX
            //            command += "_debug";
            //            command = addQuotes(command);
            //            cBuildPath = addQuotes(cBuildPath);
            //            ProcInoFile = addQuotes(ProcInoFile);
            //#endif

            //            QString boardName;

            //            if (m_board < Custom) boardName = boardList.at(m_board);
            //            else boardName = m_customBoard;

            //            command += " -v --board arduino:avr:" + boardName + " --pref build.path=" + cBuildPath;
            //            if (!preferencesPath.isEmpty())
            //                command += " --preferences-file " + preferencesPath;
            //            command += " --preserve-temp-files --verify " + ProcInoFile;
            //            m_firmware = "";

            //            m_outPane->appendText(command);
            //            m_outPane->writeText("\n\n");

            //            m_compProcess.start(command);
            //            m_compProcess.waitForFinished(-1);

            //            QString p_stderr = m_compProcess.readAllStandardError();
            //            //m_outPane->appendText( p_stderr );
            //            //m_outPane->writeText( "\n\n" );

            //            int error = -1;
            //            if (p_stderr == "")
            //            {
            //                m_outPane->appendText("\nArduino");
            //                toolChainNotFound();
            //                error = -1;
            //            }
            //            else if (p_stderr.toUpper().contains("ERROR:"))
            //            {
            //                QStringList lines = p_stderr.split("\n");
            //                for (QString line : lines)
            //                {
            //                    if (!(line.contains("error:"))) continue;

            //                    m_outPane->appendText(line);
            //                    m_outPane->writeText("\n\n");
            //                    QStringList words = line.split(":");
            //                    error = words.at(1).toInt() - 1;
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                m_firmware = buildPath + "/" + m_fileName + ".ino.hex";
            //                error = 0;
            //            }
            //            QApplication::restoreOverrideCursor();
            //            return error;
            return 0;
        }

        public void GetVariables()
        {                                                // Get dissassemble
//            QString buildPath = SIMUAPI_AppPath::self()->RWDataFolder().absoluteFilePath("codeeditor/buildIno");

//            QString objdump = m_compilerPath + "hardware/tools/avr/bin/avr-objdump";
//            QString elfPath = buildPath + "/" + m_fileName + ".ino.elf";

//# ifndef Q_OS_UNIX
//            objdump = addQuotes(objdump);
//            elfPath = addQuotes(elfPath);
//#endif

//            QString command = objdump + " -S -j .text " + elfPath;
//            QProcess compIno( 0l );
//            compIno.setStandardOutputFile(buildPath + "/" + m_fileName + ".ino.lst");
//            compIno.start(command);
//            compIno.waitForFinished(-1);

//            QProcess getBss( 0l );      // Get var address from .bss section
//            command = objdump + " -t -j.bss " + elfPath;
//            getBss.start(command);
//            getBss.waitForFinished(-1);

//            QString p_stdout = getBss.readAllStandardOutput();
//            QStringList lines = p_stdout.split("\n");

//            for (QString line : lines)
//            {
//                //qDebug() << line;

//                QStringList words = line.split(" ");
//                if (words.size() < 4) continue;
//                QString addr = words.takeFirst();
//                QString symbol = words.takeLast();

//                QHashIterator<QString, QString> i(m_varList );
//                while (i.hasNext())                        // Find Variable 
//                {
//                    i.next();
//                    QString varName = i.key();
//                    if (varName == symbol)          // Get variable address
//                    {
//                        bool ok = false;
//                        int address = addr.toInt(&ok, 16);
//                        if (ok)
//                        {
//                            address -= 0x800000;          // 0x800000 offset
//                            BaseProcessor* proc = BaseProcessor::self();
//                            if (proc) proc->addWatchVar(varName, address, i.value());
//                            //qDebug() << "InoDebugger::compile  variable "<<addr<<varName<<address<<i.value();
//                        }
//                        break;
//                    }
//                }
//            }
        }

        public void MapFlashToSource()
        {
            //getVariables();

            //m_flashToSource.clear();
            //m_sourceToFlash.clear();
            //QString buildPath = SIMUAPI_AppPath::self()->RWDataFolder().absoluteFilePath("codeeditor/buildIno");

            ///*QString elfFileName = buildPath+"/"+ m_fileName + ".ino.elf";
            //QProcess flashToLine( 0l );
            //for( int i=0; i<10000; i++ )
            //{
            //    QString addr = val2hex( i );
            //    QString command  = m_compilerPath+"hardware/tools/avr/bin/avr-addr2line -e "+ elfFileName+" "+addr;
            //    flashToLine.start( command );
            //    flashToLine.waitForFinished(-1);

            //    QString p_stdout = flashToLine.readAllStandardOutput();
            //    if( p_stdout.contains(".ino") ) qDebug() << p_stdout;
            //}*/

            //QString lstFileName = buildPath + "/" + m_fileName + ".ino.lst";
            //QStringList lstLines = fileToStringList(lstFileName, "InoDebugger::mapInoToFlash");

            //m_lastLine = 0;

            //bool readFlasAddr = false;
            //bool isInoLIne = false;
            //int inoLineNum = -1;

            //for (QString line : lstLines)
            //{
            //    if (readFlasAddr) // Last line contained source line
            //    {
            //        bool ok = false;
            //        int flashAddr = line.split(":").first().toInt(&ok, 16);
            //        if (ok)
            //        {
            //            m_flashToSource[flashAddr] = inoLineNum;
            //            m_sourceToFlash[inoLineNum] = flashAddr;
            //            if (inoLineNum > m_lastLine) m_lastLine = inoLineNum;
            //            readFlasAddr = false;
            //            //qDebug()<<"InoDebugger::mapInoToFlash ino-flash:" << inoLineNum << flashAddr ;
            //        }
            //        if (isInoLIne)
            //        {
            //            readFlasAddr = false;
            //            isInoLIne = false;
            //        }
            //    }
            //    if (line.contains("INOLINE"))
            //    {
            //        inoLineNum = line.split(" ").last().toInt() - 1;
            //        readFlasAddr = true;
            //        isInoLIne = true;
            //    }
            //    else if (line.contains("loop();"))
            //    {
            //        inoLineNum = m_loopInoLine;
            //        readFlasAddr = true;
            //    }
            //    /*QHashIterator<QString, QString> i( m_varList );
            //    while (i.hasNext())                             // Find Variable 
            //    {
            //        i.next();
            //        QString varName = "<"+i.key()+">";
            //        if( line.contains( varName ) )       // Get variable address
            //        {
            //            line = line.remove( " "+varName ).split( " " ).last().remove( "0x80" );
            //            bool ok = false;
            //            int address = line.toInt( &ok, 16 );
            //            if( ok ) BaseProcessor::self()->addWatchVar( i.key(), address, i.value() );

            //            qDebug() << "InoDebugger::mapInoToFlash  variable "<<line<<i.key()<<address<<i.value();

            //            break;
            //        }
            //    }*/

            //}
        }


        public enum BoardType{
            Uno = 0,
            Mega,
            Nano,
            Duemilanove,
            Leonardo,
            Custom
        };

        public string CustomBoard() { return customBoard; }
        public void SetCustomBoard(string b) { customBoard = b; }
        BoardType Board() { return board; }
        public void SetBoard(BoardType b) { board = b; }

        public void SetObjectName(string name)
        {
            objectName=name;
        }

        public string GetObjectName()
        {
            return objectName;
        }

        private int lastInoLine;
        private int loopInoLine;
        private int processorType;

        private List<string> boardList = new List<string>() { "uno", "megaADK", "nano", "diecimila", "leonardo" };

        private string customBoard;
        private BoardType board;
        private string objectName;

    }
}
