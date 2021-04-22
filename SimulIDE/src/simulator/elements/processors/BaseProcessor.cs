using SimulIDE.src.gui;
using SimulIDE.src.gui.circuitwidget.components.mcu;
using SimulIDE.src.gui.editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//# include "baseprocessor.h"
//# include "mcucomponent.h"
//# include "circuitwidget.h"
//# include "mainwindow.h"
//# include "simulator.h"
//# include "utils.h"
//# include "simuapi_apppath.h"


namespace SimulIDE.src.simulator.elements.processors
{
    public class BaseProcessor
    {

        protected static BaseProcessor self = null;
        public static BaseProcessor Self() { return self; }

        public BaseProcessor() 
        {
            loadStatus = false;
            resetStatus = false;
            symbolFile = "";
            device = "";
            self = this;

            ramTable = new RamTable(this);
            //MainWindow::self()->m_ramTabWidgetLayout->addWidget(m_ramTable);
        }

        public virtual void Terminate()
        {
            //qDebug() <<"\nBaseProcessor::terminate "<<m_device<<m_symbolFile<<"\n";

            self = null;
            loadStatus = false;
            symbolFile = "";
        }

        protected virtual void Initialized()
        {
            //qDebug() << "\nBaseProcessor::initialized  Firmware: " << m_symbolFile;
            //qDebug() << "\nBaseProcessor::initialized Data File: " << m_dataFile;

            loadStatus = true;
            msimStep = 0;
            extraCycle = 0;
        }

        protected virtual void SetExtraStep() // Run Extra Simulation Step If MCU clock speed > Simulation speed
        {
            if (mcuStepsPT > 1) extraCycle = Cycle();
        }

        public virtual void Step()
        {
            if (!loadStatus || resetStatus) return;

            while (nextCycle >= 1)
            {
                StepCpu();
                nextCycle -= 1;

                if (extraCycle!=0)
                {
                    //qDebug() << "-" << m_extraCycle;qDebug() << " ";
                    Simulator.Self().RunExtraStep(extraCycle);
                    extraCycle = 0;
                }
            }
            nextCycle += mcuStepsPT;
        }

        protected virtual void RunSimuStep()
        {
            Simulator.Self().RunCircuit();

            msimStep++;
            if (msimStep >= 50000 * Simulator.Self().StepsPerus()) // 20 fps
            {
                msimStep = 0;
                Simulator.Self().RunGraphicStep2();
            }
        }

        public virtual void SetSteps(double steps) { mcuStepsPT = steps; }

        protected virtual string GetFileName() { return symbolFile; }

        public virtual void SetDevice(string device) { this.device = device; }

        public virtual string GetDevice() { return device; }

        public virtual void SetDataFile(string datafile)
        {
            dataFile = datafile;
            SetRegisters();
        }

        public virtual void HardReset(bool rst)
        {
            resetStatus = rst;
            if (rst) McuComponent.Self().Reset();
        }

        protected virtual int GetRegAddress(string name)
        {
            name = name.ToUpper();
            if (regsTable.ContainsKey(name)) return regsTable[name];
            return -1;
        }

        public virtual void UpdateRamValue(string name)
        {
            if (!loadStatus) return;

            name = name.ToUpper();
            string type = "";
            if (typeTable.ContainsKey(name)) type = typeTable[name];
            else return;

            byte[] ba = new byte[4];
            int address = GetRegAddress(name);
            if (address < 0) return;

            int bits = 8;

            if (type.Contains("32"))    // 4 bytes
            {
                bits = 32;
                ba[0] = GetRamValue(address);
                ba[1] = GetRamValue(address + 1);
                ba[2] = GetRamValue(address + 2);
                ba[3] = GetRamValue(address + 3);
            }
            else if (type.Contains("16"))  // 2 bytes
            {
                bits = 16;
                ba[0] = GetRamValue(address);
                ba[1] = GetRamValue(address + 1);
                ba[2] = 0;
                ba[3] = 0;
            }
            else                                  // 1 byte
            {
                ba[0] = GetRamValue(address);
                ba[1] = 0;
                ba[2] = 0;
                ba[3] = 0;
            }
            if (type.Contains("f"))                          // float, double
            {
                float value = (float)BitConverter.ToDouble(ba, 0);
                ramTable.SetItemValue(1, value);
            }
            else                                              // char, int, long
            {
                Int32 value = 0;

                if (type.Contains("u"))
                {
                    UInt32 val = BitConverter.ToUInt32(ba, 0);
                    value = (Int32)val;
                }
                else
                {
                    if (bits == 32)
                    {
                        Int32 val = BitConverter.ToInt32(ba, 0);
                        value = val;
                    }
                    else if (bits == 16)
                    {
                        Int16 val = BitConverter.ToInt16(ba, 0);
                        value = val;
                    }
                    else
                    {
                        byte val = ba[0];
                        value = val;
                    }
                }
                ramTable.SetItemValue(2, value);

                if (type.Contains("8")) ramTable.SetItemValue(3, Utils.DecToBase(value, 2, 8));
                else if (type.Contains("string"))
                {
                    string strVal = "";
                    for (int i = address; i <= address + value; i++)
                    {
                        string str = "";
                        char cha = (char)GetRamValue(i);
                        str = cha.ToString();
                        strVal += str; //QByteArray::fromHex( getRamValue( i ) );
                    }
                    //qDebug() << "string" << name << value << strVal;
                    ramTable.SetItemValue(3, strVal);
                }

            }
            //qDebug()<<name<<type <<address<<value;
            //if( !type.contains( "8" ) ) 
            ramTable.SetItemValue(1, type);
        }


        public virtual int GetRamValue(string name)
        {
            if (regsTable.Count == 0) return -1;

            bool isNumber = false;
            isNumber = int.TryParse(name, out int address);
            if (!isNumber)
                address = regsTable[name.ToUpper()];  // Is a register name

            return GetRamValue(address); 
        }

        public virtual void AddWatchVar(string name, int address, string type)
        {
            name = name.ToUpper();
            if (!regsTable.ContainsKey(name)) regList.Add(name);
            regsTable[name] = address;
            typeTable[name] = type;
        }

        protected virtual void SetRegisters() // get register addresses from data file
        {
            List<string> lineList = Utils.FileToStringList(dataFile);
            if (regsTable.Count != 0)
            {
                regList.Clear();
                regsTable.Clear();
                typeTable.Clear();
            }

            for (int i = 0; i < lineList.Count; i++)
            {
                string line = lineList[i];
                if (line.Contains("EQU "))   // This line contains a definition
                {
                    line = line.Replace("\t", " ");

                    string name = "";
                    string addrtxt = "";
                    int address = 0;
                    bool isNumber = false;

                    line.Remove(' ');
                    string[] wordList = line.Split("EQU".ToCharArray());
                    if (wordList.Length < 2) continue;

                    name = wordList[0];
                    while (addrtxt == "") addrtxt = wordList[0];

                    isNumber = int.TryParse(addrtxt, out address);
                    if (isNumber)        // If found a valid address add to map
                    {
                        address = Validate(address);
                        AddWatchVar(name, address, "u8");        // type uint8 
                    }
                    //qDebug() << name << address<<"\n";
                }
            }
        }

        protected virtual void UartOut(int uart, UInt32 value) // Send value to OutPanelText
        {
  //tyv          UartDataOut?.Invoke(uart, value);
        }

        protected virtual void UartIn(int uart, UInt32 value) // Receive one byte on Uart
        {
  //tyv          UartDataIn?.Invoke(uart, value);
        }

        public virtual bool LoadFirmware(string file) { return false; }
        public virtual bool GetLoadStatus() { return loadStatus; }
        public virtual void StepOne() { }
        public virtual void StepCpu() { }
        public virtual void Reset() { }
        public virtual uint PC(){return 0;}
        public virtual uint Cycle() { return 0; }
        public virtual byte GetRamValue(int address) { return 0; }

        protected virtual List<string> GetRegList() { return regList; }

        public virtual RamTable GetRamTable() { return ramTable; }

        protected virtual List<int> Eeprom() { return null; }
        protected virtual void SetEeprom(List<int> eep) { }

        protected virtual int Validate(int address) { return 0; }

        protected string symbolFile;
        protected string dataFile;
        protected string device;

        protected double nextCycle;
        protected double mcuStepsPT;
        protected int msimStep;
        protected uint extraCycle;

        protected RamTable ramTable;
        protected List<string> regList=new List<string>();
        protected Dictionary<string, int> regsTable = new Dictionary<string, int>();     // int max 32 bits
        protected Dictionary<string, float> floatTable = new Dictionary<string, float>();  // float 32 bits
        protected Dictionary<string, string> typeTable = new Dictionary<string, string>();

        protected int[] eeprom;

        protected bool resetStatus;
        protected bool loadStatus;

    }
}



