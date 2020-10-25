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
    class BaseProcessor
    {


        protected static BaseProcessor self = null;

        public BaseProcessor(object parent ):base()
        {
            loadStatus = false;
            resetStatus = false;
            symbolFile = "";
            device = "";

            //ramTable = new RamTable(this);
            //MainWindow::self()->m_ramTabWidgetLayout->addWidget(m_ramTable);
        }

        public void Terminate()
        {
            //qDebug() <<"\nBaseProcessor::terminate "<<m_device<<m_symbolFile<<"\n";

            self = null;
            m_loadStatus = false;
            symbolFile = "";
        }

        public void Initialized()
        {
            //qDebug() << "\nBaseProcessor::initialized  Firmware: " << m_symbolFile;
            //qDebug() << "\nBaseProcessor::initialized Data File: " << m_dataFile;

            loadStatus = true;
            msimStep = 0;
            extraCycle = 0;
        }

        public void SetExtraStep() // Run Extra Simulation Step If MCU clock speed > Simulation speed
        {
            if (mcuStepsPT > 1) extraCycle = Cycle();
        }

        public void Step()
        {
            if (!loadStatus || resetStatus) return;

            while (nextCycle >= 1)
            {
                StepCpu();
                nextCycle -= 1;

                if (extraCycle)
                {
                    //qDebug() << "-" << m_extraCycle;qDebug() << " ";
                    Simulator.Self().RunExtraStep(extraCycle);
                    extraCycle = 0;
                }
            }
            nextCycle += mcuStepsPT;
        }

        public void RunSimuStep()
        {
            Simulator.Self().RunCircuit();

            msimStep++;
            if (msimStep >= 50000 * Simulator.Self().stepsPerus()) // 20 fps
            {
                msimStep = 0;
                Simulator.Self().RunGraphicStep2();
            }
        }

        public void SetSteps(double steps) { mcuStepsPT = steps; }

        public string GetFileName() { return symbolFile; }

        public void SetDevice(string device) { this.device = device; }

        public string GetDevice() { return device; }

        public void SetDataFile(string datafile)
        {
            dataFile = datafile;
            SetRegisters();
        }

        public void HardReset(bool rst)
        {
            resetStatus = rst;
            if (rst) McuComponent.Self()->Reset();
        }

        public int GetRegAddress(string name)
        {
            name = name.ToUpper();
            if (regsTable.ContainsKey(name)) return regsTable.value(name);
            return -1;
        }

        void BaseProcessor::updateRamValue(QString name)
        {
            if (!m_loadStatus) return;

            name = name.toUpper();
            QString type = "";
            if (m_typeTable.contains(name)) type = m_typeTable[name];
            else return;

            QByteArray ba;
            ba.resize(4);
            int address = getRegAddress(name);
            if (address < 0) return;

            int bits = 8;

            if (type.contains("32"))    // 4 bytes
            {
                bits = 32;
                ba[0] = getRamValue(address);
                ba[1] = getRamValue(address + 1);
                ba[2] = getRamValue(address + 2);
                ba[3] = getRamValue(address + 3);
            }
            else if (type.contains("16"))  // 2 bytes
            {
                bits = 16;
                ba[0] = getRamValue(address);
                ba[1] = getRamValue(address + 1);
                ba[2] = 0;
                ba[3] = 0;
            }
            else                                  // 1 byte
            {
                ba[0] = getRamValue(address);
                ba[1] = 0;
                ba[2] = 0;
                ba[3] = 0;
            }
            if (type.contains("f"))                          // float, double
            {
                float value = 0;
                memcpy(&value, ba, 4);
                m_ramTable->setItemValue(1, value);
            }
            else                                              // char, int, long
            {
                int32_t value = 0;

                if (type.contains("u"))
                {
                    uint32_t val = 0;
                    memcpy(&val, ba, 4);
                    value = val;
                }
                else
                {
                    if (bits == 32)
                    {
                        int32_t val = 0;
                        memcpy(&val, ba, 4);

                        value = val;
                    }
                    else if (bits == 16)
                    {
                        int16_t val = 0;
                        memcpy(&val, ba, 2);

                        value = val;
                    }
                    else
                    {
                        int8_t val = 0;
                        memcpy(&val, ba, 1);

                        value = val;
                    }
                }
                m_ramTable->setItemValue(2, value);

                if (type.contains("8")) m_ramTable->setItemValue(3, decToBase(value, 2, 8));
                else if (type.contains("string"))
                {
                    QString strVal = "";
                    for (int i = address; i <= address + value; i++)
                    {
                        QString str = "";
                        const QChar cha = getRamValue(i);
                        str.setRawData(&cha, 1);

                        strVal += str; //QByteArray::fromHex( getRamValue( i ) );
                    }
                    //qDebug() << "string" << name << value << strVal;
                    m_ramTable->setItemValue(3, strVal);
                }

            }
            //qDebug()<<name<<type <<address<<value;
            //if( !type.contains( "8" ) ) 
            m_ramTable->setItemValue(1, type);
        }


        int BaseProcessor::getRamValue(QString name)
        {
            if (m_regsTable.isEmpty()) return -1;

            bool isNumber = false;
            int address = name.toInt(&isNumber);      // Try to convert to integer

            if (!isNumber)
            {
                address = m_regsTable[name.toUpper()];  // Is a register name
            }

            return getRamValue(address);
        }

        void BaseProcessor::addWatchVar(QString name, int address, QString type)
        {
            name = name.toUpper();
            if (!m_regsTable.contains(name)) m_regList.append(name);
            m_regsTable[name] = address;
            m_typeTable[name] = type;
        }

        void BaseProcessor::setRegisters() // get register addresses from data file
        {
            QStringList lineList = fileToStringList(m_dataFile, "BaseProcessor::setRegisters");

            if (!m_regsTable.isEmpty())
            {
                m_regList.clear();
                m_regsTable.clear();
                m_typeTable.clear();
            }

            for (QString line : lineList)
            {
                if (line.contains("EQU "))   // This line contains a definition
                {
                    line = line.replace("\t", " ");

                    QString name = "";
                    QString addrtxt = "";
                    int address = 0;
                    bool isNumber = false;

                    line.remove(" ");
                    QStringList wordList = line.split("EQU"); // Split in words
                    if (wordList.size() < 2) continue;

                    name = wordList.takeFirst();
                    while (addrtxt.isEmpty()) addrtxt = wordList.takeFirst();

                    address = addrtxt.toInt(&isNumber, 10);

                    if (isNumber)        // If found a valid address add to map
                    {
                        address = validate(address);
                        addWatchVar(name, address, "u8");        // type uint8 
                    }
                    //qDebug() << name << address<<"\n";
                }
            }
        }

        void BaseProcessor::uartOut(int uart, uint32_t value) // Send value to OutPanelText
        {
            emit uartDataOut(uart, value );

            //qDebug()<<"BaseProcessor::uartOut" << uart << value;
        }

        void BaseProcessor::uartIn(int uart, uint32_t value) // Receive one byte on Uart
        {
            //qDebug()<<"BaseProcessor::uartIn" << uart << value;
            emit uartDataIn(uart, value );
        }


        protected virtual int Validate(int address) { return 0; }

        protected void RunSimuStep() { }

        protected string symbolFile;
        protected string dataFile;
        protected string device;

        protected double nextCycle;
        protected double mcuStepsPT;
        protected int msimStep;
        protected UInt64 m_extraCycle;

        protected RamTable ramTable;
        protected List<string> regList=new List<string>();
        protected Dictionary<string, int> regsTable = new Dictionary<string, int>();     // int max 32 bits
        protected Dictionary<string, float> floatTable = new Dictionary<string, float>();  // float 32 bits
        protected Dictionary<string, string> typeTable = new Dictionary<string, string>();

        protected List<int> eeprom = new List<int>();

        protected bool m_resetStatus;
        protected bool m_loadStatus;

    }
}



