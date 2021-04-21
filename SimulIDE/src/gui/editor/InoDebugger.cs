using SimulIDE.src.gui.circuitwidget;
using SimulIDE.src.simulator.elements.processors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        

        public override void Upload()
        {
            string circDir = Circuit.Self().GetFileName();
            string firmPath = firmware;
            if (circDir != "")
            {
                string circuitDir = System.IO.Path.GetDirectoryName(circDir);
              //TYV  firmware = circuitDir+ "\\" + fileName + ".hex";
              //  if (File.Exists(firmPath+"\\"+firmware))
              //      File.Delete(firmPath + "\\" + firmware);
              //  File.Copy(firmPath, firmware);
            }
            base.Upload();
            firmware = firmPath;
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

        private string compError;

        public override int Compile()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
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
            foreach (string filName in fileList) // Copy files to sketch folder
            {
                string fn = System.IO.Path.GetFileName(filName);
                File.Copy(fileDir +"\\"+ fn, buildPath + "\\" +fileName+"\\"+ fn );
            }
            string ProcInoFile = buildPath + "\\" + fileName + "\\" + fileName + fileExt;
            List<string> inoText = new List<string>();

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
                inoText.Add(inoLine.ToString()+" // INOLINE "+inoLineNumber.ToString()+"\n");
            }

            System.IO.File.WriteAllLines(ProcInoFile, inoText);


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
            string args= " -v --board arduino:avr:" + boardName + " --pref build.path=" + cBuildPath;
//            if (!preferencesPath.isEmpty())
//                command += " --preferences-file " + preferencesPath;
            args += " --preserve-temp-files --verify " + ProcInoFile;
            firmware = "";

            outPane.AppendText(cmd+args);
            outPane.AppendText("\n\n");

            //   / Applications/Arduino.app/Contents/Java/hardware/tools/avr/bin/avr-g++ -c -g -Os -Wall -Wextra -std=gnu++11 -fpermissive -fno-exceptions -ffunction -sections -fdata-sections -fno-threadsafe-statics -Wno-error=narrowing -MMD -flto -mmcu=atmega328p -DF_CPU=16000000L -DARDUINO=10810 -DARDUINO_AVR_UNO -DARDUINO_ARCH_AVR -I /Applications/Arduino.app/Contents/Java/ hardware / arduino / avr / cores / arduino - I / Applications / Arduino.app / Contents / Java / hardware / arduino / avr / variants / standard - I / Users / john / Documents / Arduino / libraries / IRremote / var / folders / 2v / _q7vxn794_d8th6w010cnmv00000gn / T / arduino_build_771667 / sketch / sketch_dec25a.ino.cpp - o / var / folders / 2v / _q7vxn794_d8th6w010cnmv00000gn / T / arduino_build_771667 / sketch / sketch_dec25a.ino.cpp.o

            compError = "";

            Process compProc = new Process();
            compProc.StartInfo.WorkingDirectory = fileDir;
            compProc.StartInfo.FileName = cmd;
            compProc.StartInfo.Arguments = args;
            compProc.StartInfo.UseShellExecute = false;
            compProc.StartInfo.CreateNoWindow = true;
            compProc.StartInfo.RedirectStandardOutput = true;
            compProc.StartInfo.RedirectStandardError = true;
            compProc.ErrorDataReceived += CompProc_ErrorDataReceived;
         
            compProc.Start();
            compProc.BeginErrorReadLine();
            while (!compProc.StandardOutput.EndOfStream)
            {
                string lin = Encoding.UTF8.GetString(Encoding.Default.GetBytes(compProc.StandardOutput.ReadLine()));
                outPane.AppendText(lin+"\n");
                outPane.ScrollToEnd();
                DoEvents();
            }

            compProc.WaitForExit();
            compProc.Close();
            string stderr = compError;

            byte[] bytes = Encoding.Default.GetBytes(stderr);
            stderr = Encoding.UTF8.GetString(bytes);

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
                 firmware = buildPath + "\\" + fileName + ".ino.hex";
                 error = 0;
            }
            outPane.ScrollToEnd();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            return error;
        }

        private void CompProc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            compError += e.Data+"\n";
        }

        public void GetVariables()
        {                                                // Get dissassemble
            string buildPath = AppDomain.CurrentDomain.BaseDirectory + "codeeditor\\buildIno";
            string objdump = compilerPath + "\\hardware\\tools\\avr\\bin\\avr-objdump";
            string elfPath = buildPath + "\\" + fileName + ".ino.elf";

//# ifndef Q_OS_UNIX
//            objdump = addQuotes(objdump);
//            elfPath = addQuotes(elfPath);
//#endif

            string command = objdump + " -S -j .text " + elfPath;

            StringBuilder outfile = new StringBuilder();
            Process compIno_ = new Process();
            compIno_.StartInfo.WorkingDirectory = fileDir;
            compIno_.StartInfo.FileName = objdump;
            compIno_.StartInfo.Arguments = " -S -d -j .text " + elfPath;
          //  compIno_.StartInfo.Arguments = " -S -x -m -d " + elfPath;

            compIno_.StartInfo.UseShellExecute = false;
            compIno_.StartInfo.CreateNoWindow = true;
            compIno_.StartInfo.RedirectStandardOutput = true;
            compIno_.StartInfo.RedirectStandardError = true;
            compIno_.ErrorDataReceived += CompProc_ErrorDataReceived;

            compIno_.Start();
            compIno_.BeginErrorReadLine();
            int ofs = 0;
            while (!compIno_.StandardOutput.EndOfStream)
            {
                string s = Encoding.UTF8.GetString(Encoding.Default.GetBytes(compIno_.StandardOutput.ReadLine()));
                if (ofs < 2)  // пропускаем первые две строки
                    ofs++;
                else 
                    outfile.AppendLine(s);
            }
            compIno_.WaitForExit();
            compIno_.Close();

            System.IO.File.WriteAllText(buildPath + "\\" + fileName + ".ino.lst", outfile.ToString());


            List<string> lines = new List<string>();
            Process getBss = new Process();
            getBss.StartInfo.WorkingDirectory = fileDir;
            getBss.StartInfo.FileName = objdump;
            getBss.StartInfo.Arguments = " -t -j.bss " + elfPath;
            getBss.StartInfo.UseShellExecute = false;
            getBss.StartInfo.CreateNoWindow = true;
            getBss.StartInfo.RedirectStandardOutput = true;
            getBss.StartInfo.RedirectStandardError = true;
            getBss.ErrorDataReceived += CompProc_ErrorDataReceived;

            getBss.Start();
            getBss.BeginErrorReadLine();
            ofs = 0;
            while (!getBss.StandardOutput.EndOfStream)
            {
                string s = Encoding.UTF8.GetString(Encoding.Default.GetBytes(getBss.StandardOutput.ReadLine()));
                if (ofs < 2) // пропускаем первые две строки
                    ofs++;
                else
                    lines.Add(s);
            }
            getBss.WaitForExit();
            getBss.Close();
            System.IO.File.WriteAllLines(buildPath + "\\" + fileName + ".ino.lst.bss", lines);

            foreach (string line in lines)
            {
                //qDebug() << line;

                var words = line.Split(' ');
                if (words.Length < 4) continue;
                string addr = words.First();
                string symbol = words.Last();

                foreach (var i in varList)
                {
                    string varName = i.Key;
                    if (varName == symbol)          // Get variable address
                    {
                        bool ok = false;
                        int address;
                        ok = int.TryParse(addr, System.Globalization.NumberStyles.HexNumber, null, out address);
                        if (ok)
                        {
                            address -= 0x800000;          // 0x800000 offset
                            BaseProcessor proc = BaseProcessor.Self();
                            if (proc!=null) proc.AddWatchVar(varName, address, i.Value);
                            //qDebug() << "InoDebugger::compile  variable "<<addr<<varName<<address<<i.value();
                        }
                        break;
                    }

                }
            }
        }

        public override void MapFlashToSource()
        {
            GetVariables();

            flashToSource.Clear();
            sourceToFlash.Clear();
            string buildPath = AppDomain.CurrentDomain.BaseDirectory + "codeeditor\\buildIno";

            /*QString elfFileName = buildPath+"/"+ m_fileName + ".ino.elf";
            QProcess flashToLine( 0l );
            for( int i=0; i<10000; i++ )
            {
                QString addr = val2hex( i );
                QString command  = m_compilerPath+"hardware/tools/avr/bin/avr-addr2line -e "+ elfFileName+" "+addr;
                flashToLine.start( command );
                flashToLine.waitForFinished(-1);

                QString p_stdout = flashToLine.readAllStandardOutput();
                if( p_stdout.contains(".ino") ) qDebug() << p_stdout;
            }*/

            string lstFileName = buildPath + "\\" + fileName + ".ino.lst";
            List<string> lstLines = File.ReadAllLines(lstFileName).ToList();
            lastLine = 0;

            bool readFlasAddr = false;
            bool isInoLIne = false;
            int inoLineNum = -1;

            foreach (string line in lstLines)
            {
                if (readFlasAddr) // Last line contained source line
                {
                    bool ok = false;

                    int flashAddr;
                    string addr = line.Split(':').First();
                    ok = int.TryParse(addr, System.Globalization.NumberStyles.HexNumber, null, out flashAddr);
                    if (ok)
                    {
                        flashToSource[flashAddr] = inoLineNum;
                        sourceToFlash[inoLineNum] = flashAddr;
                        if (inoLineNum > lastLine) lastLine = inoLineNum;
                        readFlasAddr = false;
                        //qDebug()<<"InoDebugger::mapInoToFlash ino-flash:" << inoLineNum << flashAddr ;
                    }
                    if (isInoLIne)
                    {
                        readFlasAddr = false;
                        isInoLIne = false;
                    }
                }
                if (line.Contains("INOLINE"))
                {
                    inoLineNum = int.Parse(line.Split(' ').Last()) - 1;
                    readFlasAddr = true;
                    isInoLIne = true;
                }
                else if (line.Contains("loop()"))
                {
                    inoLineNum = loopInoLine;
                    readFlasAddr = true;
                }
                /*QHashIterator<QString, QString> i( m_varList );
                while (i.hasNext())                             // Find Variable 
                {
                    i.next();
                    QString varName = "<"+i.key()+">";
                    if( line.contains( varName ) )       // Get variable address
                    {
                        line = line.remove( " "+varName ).split( " " ).last().remove( "0x80" );
                        bool ok = false;
                        int address = line.toInt( &ok, 16 );
                        if( ok ) BaseProcessor::self()->addWatchVar( i.key(), address, i.value() );

                        qDebug() << "InoDebugger::mapInoToFlash  variable "<<line<<i.key()<<address<<i.value();

                        break;
                    }
                }*/

            }
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
