using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public override int Compile()
        {
            // QApplication::setOverrideCursor(Qt::WaitCursor);
            compilerPath = "C:\\Program Files (x86)\\Arduino";  //Временно!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!Directory.Exists(compilerPath))
            {
                outPane.AppendText("\nArduino");
                ToolChainNotFound();
                return -1;
            }
            string filePath = fileDir +"\\"+ fileName + fileExt;
            // Doing build in the user directory
            string buildPath = AppDomain.CurrentDomain.BaseDirectory+"codeeditor\\buildIno";

            if (Directory.Exists(buildPath))
                Directory.Delete(buildPath, true);
            else
                Directory.CreateDirectory(buildPath);
            Directory.CreateDirectory(buildPath+'\\'+fileName);
            var fileList = Directory.EnumerateFiles(fileDir);
            foreach (string fileName in fileList) // Copy files to sketch folder
            {
                string fn = System.IO.Path.GetFileName(fileName);
                string fne = System.IO.Path.GetFileNameWithoutExtension(fileName);

                File.Copy(fileDir +"\\"+ fn, buildPath + "\\" +fne+"\\"+ fn );
            }
            string ProcInoFile = buildPath + "\\" + fileName + "\\" + fileName + fileExt;

            var inoLines = File.ReadLines(filePath);
            String line;
            int inoLineNumber = 0;

            varList.Clear();
            foreach (string inoLine in inoLines)                        // Get Variables
            {
                line = inoLine;
                line = line.Replace("\t", " ");
                int idx = line.IndexOf(';');
                if (idx!=-1)
                    line = line.Remove(idx);
                var wordList = line.Split(' ').ToList<string>();
                wordList.RemoveAll((a)=>a=="");

                if (wordList.Count!=0)      // Fix crash on empty list.first()
                {
                    string type = wordList[0];
                    if (type == "unsigned")
                        type = "u" + wordList[1];
                                
                    if (typesList.ContainsKey(type))
                    {
                         string varName = wordList[1];
                         if (!varList.ContainsKey(varName))
                             varList[varName] = typesList[type];
                             //qDebug() << "InoDebugger::compile  variable "<<type<<varName<<m_typesList[ type ];
                    }
                }
                if (inoLine.Contains("loop()")) loopInoLine = inoLineNumber;
                inoLineNumber++;

                Console.WriteLine(inoLine.ToString()+" // INOLINE "+inoLineNumber.ToString()+"\n");
            }
            

            ///TODO: verify arduino version, older versions can compile, but no sorce code emited into .lst file
            /// , then debugger will hang!
            string cBuildPath = buildPath;
            //string preferencesPath = SIMUAPI_AppPath::self()->availableDataFilePath("codeeditor/preferences.txt");
            string cmd = compilerPath + "\\"+"arduino";


//            # ifndef Q_OS_UNIX
//                    command += "_debug";
//                    command = addQuotes(command);
//                    cBuildPath = addQuotes(cBuildPath);
//                     ProcInoFile = addQuotes(ProcInoFile);
//            #endif

            string boardName;

            if (board < BoardType.Custom)
                boardName = boardList[(int)board];
            else boardName = customBoard;
            string args=" -v --board arduino:avr:" + boardName + " --pref build.path=" + cBuildPath;
//            if (!preferencesPath.isEmpty())
//                command += " --preferences-file " + preferencesPath;
            args += " --preserve-temp-files --verify " + ProcInoFile;
            firmware = "";

            outPane.AppendText(cmd+args);
            outPane.AppendText("\n\n");


            Process compProc = new Process();
            compProc.StartInfo.WorkingDirectory = fileDir;
            compProc.StartInfo.FileName = cmd;
            compProc.StartInfo.Arguments = args;
            compProc.StartInfo.UseShellExecute = false;
            compProc.StartInfo.CreateNoWindow = false;
            compProc.StartInfo.RedirectStandardOutput = true;
            compProc.StartInfo.RedirectStandardError = true;
            compProc.Start();
            string stdout = compProc.StandardOutput.ReadToEnd();
            string stderr = compProc.StandardError.ReadToEnd();

            compProc.WaitForExit();
            compProc.Close();

            outPane.AppendText(stderr);
            outPane.AppendText( "\n\n" );

            int error = -1;
            if (stderr == "")
            {
               outPane.AppendText("\nArduino");
               ToolChainNotFound();
               error = -1;
            }
            else if (stderr.ToUpper().Contains("ERROR:"))
            {
                string[] lines = stderr.Split('\n');
                foreach (string lin in lines)
                {
                    if (!(lin.Contains("error:"))) continue;

                    outPane.AppendText(lin);
                    outPane.AppendText("\n\n");
                    string[] words = lin.Split(':');
                    error = int.Parse(words[1]) - 1;
                    break;
                }
            }
            else
            {
                 firmware = buildPath + "/" + fileName + ".ino.hex";
                 error = 0;
            }
            //QApplication::restoreOverrideCursor();
            return error;
            
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
