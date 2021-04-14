using SimulIDE.src.simavr;
using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimulIDE.src.simulator.elements.processors
{
    public class AvrProcessor:BaseProcessor
    {
            //int elf_read_firmware_ext(const char* file, elf_firmware_t * firmware);

        public AvrProcessor(object parent ):base()
        {
            self = this;
            avrProcessor = null;
            initGdb = false;
            SetSteps(16);
        }
        

        public override void Terminate()
        {
            base.Terminate();
            if (avrProcessor!=null)
            {
         //       Avr_deinit_gdb(avrProcessor);
         //       Avr_terminate(avrProcessor);
            }
            avrProcessor = null;
        }

        public override bool LoadFirmware(string fileN)
        {
            if (fileN == "") return false;
            //QFileInfo fileInfo(fileN);

            //if (fileInfo.completeSuffix().isEmpty()) fileN.append(".hex");

            if (!File.Exists(fileN))     // File not found
            {
                MessageBox.Show("The file " + fileN + " was not found.");
                return false;
            }

            string name = "";
            name = device;
            
            //char filename[1000] = "";
            //strncpy(filename, fileN.toLatin1(), sizeof(filename) - 1);
            //*(filename + sizeof(filename) - 1) = 0;

            Elf_firmware f = new Elf_firmware();

            string filename = fileN;
            if (fileN.EndsWith("hex"))
            {
                Ihex_chunk[] chunk = new Ihex_chunk[0];
                int cnt = Sim_Hex.Read_ihex_chunks(filename, ref chunk);

                if (cnt <= 0)
                {
                    MessageBox.Show(" Unable to load IHEX file "+fileN);
                    return false;
                }

                int lastFChunk = 0;

                for (int ci = 0; ci < cnt; ci++)
                {
                    if (chunk[ci].baseaddr < (1 * 1024 * 1024)) lastFChunk = ci;
                }
                f.flashbase = chunk[0].baseaddr;
                f.flashsize = chunk[lastFChunk].baseaddr + chunk[lastFChunk].size;
                f.flash = new byte[f.flashsize + 1];

                for (int ci = 0; ci < cnt; ci++)
                {
                    if (chunk[ci].baseaddr < (1 * 1024 * 1024))
                    {
                        for (int i = 0; i < chunk[ci].size; i++)
                        {
                            f.flash[chunk[ci].baseaddr + i] = chunk[ci].data[i];
                        }
                    }
                    if (chunk[ci].baseaddr >= Sim_elf.AVR_SEGMENT_OFFSET_EEPROM)
                    {
                        f.eeprom = chunk[ci].data;
                        f.eesize = chunk[ci].size;
                    }
                }
            }
            else                                    // File extension not valid
            {
                MessageBox.Show(fileN+" should be .hex \n");
                return false;
            }

            string mmcu = f.mmcu;
            if (mmcu!=null)
            {
                if (mmcu != device)
                {
                    MessageBox.Show("Warning on load firmware: \n Incompatible firmware: compiled for "+mmcu+
                        " and your processor is "+device);
                    return false;
                }
            }
            else
            {
                if (name.Length==0)
                {
                    MessageBox.Show("Failed to load firmware: \n The processor model is not specified.\n");
                    return false;
                }
                f.mmcu=name;
            }
            if (avrProcessor==null)
            {
                avrProcessor = Sim_Avr.Avr_make_mcu_by_name(f.mmcu);

                if (avrProcessor==null)
                {
                    MessageBox.Show("Could not Create AVR Processor: "+f.mmcu);
                    return false;
                }
                int started = Sim_Avr.Avr_init(avrProcessor);

            //    m_uartInIrq.resize(6);
            //    m_uartInIrq.fill(0);
            //    for (int i = 0; i < 6; i++)// Uart interface
            //    {
            //        avr_irq_t* src = avr_io_getirq(m_avrProcessor, AVR_IOCTL_UART_GETIRQ('0' + i), UART_IRQ_OUTPUT);
            //        if (src)
            //        {
            //            qDebug() << "    UART" << i;
            //            intptr_t uart = i;
            //            avr_irq_register_notify(src, uart_pty_out_hook, (void*)uart); // Irq to get data coming from AVR

            //            // Irq to send data to AVR:
            //            m_uartInIrq[i] = avr_io_getirq(m_avrProcessor, AVR_IOCTL_UART_GETIRQ('0' + i), UART_IRQ_INPUT);
            //        }
            //    }
            //    qDebug() << "\nAvrProcessor::loadFirmware Avr Init: " << name << (started == 0);
            }

            ///// TODO: Catch possible abort signal here, otherwise application will crash on the invalid firmware load
            ///// Done: Modified simavr to not call abort(), instead it returns error code.
            if (avr_load_firmware(avrProcessor, f) != 0)
            {
                MessageBox.Show("Wrong firmware!! "+f.mmcu);
                return false;
            }
            if (f.flashbase!=0)
                avrProcessor.PC = f.flashbase;

            setEeprom(m_eeprom); // Load EEPROM

            m_avrProcessor->frequency = 16000000;
            m_avrProcessor->cycle = 0;
            m_avrProcessor->gdb_port = 1212;
            m_symbolFile = fileN;

            if (m_initGdb)
            {
                int ok = avr_gdb_init(m_avrProcessor);
                if (ok < 0) qDebug() << "avr_gdb_init ERROR " << ok;
                else qDebug() << "avr_gdb_init OK";
            }

            initialized();

            return true;
        }

        public override void Reset()
        {
            if (!loadStatus) return;
            ////qDebug() << "AvrProcessor::reset("<< eeprom();

            for (int i = 0; i < avrProcessor.ramend; i++) avrProcessor.data[i] = 0;

            Sim_Avr.Avr_reset(avrProcessor);
            avrProcessor.PC = 0;
            avrProcessor.cycle = 0;
            nextCycle = mcuStepsPT;
            extraCycle = 0;
        }

        public override void StepOne()
        {
            //qDebug() <<"AvrProcessor::stepOne()"<<m_avrProcessor->cycle << m_nextCycle;

            if (avrProcessor.state < Sim_Avr.CoreStates.cpu_Done)
                avrProcessor.Run(avrProcessor);

            while (avrProcessor.cycle >= nextCycle)
            {
                nextCycle += mcuStepsPT; //McuComponent::self()->freq(); //
                RunSimuStep(); // 1 simu step = 1uS
            }
        }

        public override uint PC()
        {
            return avrProcessor.PC;
        }

        public int GetRamValue(int address)
        {
            return avrProcessor.data[address];
        }

        protected virtual int Validate(int address)
        {
            if (address < 64) address += 32;
            return address;
        }

        //void AvrProcessor::uartIn(int uart, uint32_t value) // Receive one byte on Uart
        //{
        //    if (!m_avrProcessor) return;

        //    avr_irq_t* uartInIrq = m_uartInIrq[uart];
        //    if (uartInIrq)
        //    {
        //        BaseProcessor::uartIn(uart, value);
        //        avr_raise_irq(uartInIrq, value);
        //        //qDebug() << "AvrProcessor::uartIn: " << value;
        //    }
        //}

        public int[] Eeprom()
        {
            //if (m_avrProcessor)
            //{
            //    int rom_size = m_avrProcessor->e2end + 1;
            //    m_eeprom.resize(rom_size);

            //    avr_eeprom_desc_t ee;
            //    ee.ee = 0;
            //    ee.offset = 0;
            //    ee.size = rom_size;
            //    int ok = avr_ioctl(m_avrProcessor, AVR_IOCTL_EEPROM_GET, &ee);
            //    //qDebug() << "avr epprom read ok =" ;//<< ok ;//<< m_eeprom;
            //    if (ok)
            //    {
            //        //qDebug() << "avr epprom Reading...";
            //        uint8_t* src = ee.ee;

            //        for (int i = 0; i < rom_size; i++) m_eeprom[i] = src[i];
            //    }
            //}
            return eeprom;
        }

        public virtual void SetEeprom(int[] eep)
        {
            eeprom = eep;
            ////qDebug() << "BaseProcessor::setEeprom" <<eep.size()<< eep;

            //if (!m_avrProcessor) return;

            //int rom_size = m_avrProcessor->e2end + 1;
            //int eep_size = m_eeprom.size();

            ////qDebug() << "eeprom size at Load:" << rom_size << eep_size;

            //if (eep_size < rom_size) rom_size = eep_size;

            //if (rom_size)
            //{
            //    uint8_t rep[rom_size];

            //    for (int i = 0; i < rom_size; i++)
            //    {
            //        uint8_t val = m_eeprom[i];
            //        rep[i] = val;
            //        //qDebug() << i << val;
            //    }
            //    avr_eeprom_desc_t ee;
            //    ee.offset = 0;
            //    ee.size = rom_size;
            //    ee.ee = &rep[0];
            //    avr_ioctl(m_avrProcessor, AVR_IOCTL_EEPROM_SET, &ee);
            //}
        }

        public bool InitGdb()
        {
            return initGdb;
        }

        public void SetInitGdb(bool init)
        {
            initGdb = init;
        }

        public void StepCpu() { avrProcessor.Run(avrProcessor); }
        public virtual UInt64 Cycle() { return avrProcessor.cycle; }
        
        public Avr GetCpu() { return avrProcessor; }
        public void SetCpu(Avr avrProc) { avrProcessor = avrProc; }

    //    static void uart_pty_out_hook( struct avr_irq_t* irq, uint32_t value, void* param )
    //    {
    //        Q_UNUSED(irq);
    //    // get the pointer out of param and asign it to AvrProcessor*
    //    //AvrProcessor* ptrAvrProcessor = reinterpret_cast<AvrProcessor*> (param);

    //    //ptrAvrProcessor->uartOut( value );
    //    intptr_t uart = reinterpret_cast<intptr_t>(param);
    //    BaseProcessor::self()->uartOut(uart, value );
    //}


    protected bool initGdb;
    
    protected Avr avrProcessor;
    //avr_irq_t[] uartInIrq;

    }
}
