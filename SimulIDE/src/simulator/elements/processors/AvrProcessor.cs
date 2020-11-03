using SimulIDE.src.simavr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator.elements.processors
{
    class AvrProcessor:BaseProcessor
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
        //    base.Terminate();
        //    if (avrProcessor)
        //    {
        //        Avr_deinit_gdb(avrProcessor);
        //        Avr_terminate(avrProcessor);
        //    }
        //    avrProcessor = null;
        }

        public bool LoadFirmware(string fileN)
        {
            //if (fileN == "") return false;
            //QFileInfo fileInfo(fileN);

            //if (fileInfo.completeSuffix().isEmpty()) fileN.append(".hex");

            //if (!QFile::exists(fileN))     // File not found
            //{
            //    QMessageBox::warning(0, tr("File Not Found"),
            //                             tr("The file \"%1\" was not found.").arg(fileN));
            //    return false;
            //}

            //char name[20] = "";
            //strncpy(name, m_device.toLatin1(), sizeof(name) - 1);
            //*(name + sizeof(name) - 1) = 0;

            //char filename[1000] = "";
            //strncpy(filename, fileN.toLatin1(), sizeof(filename) - 1);
            //*(filename + sizeof(filename) - 1) = 0;

            //elf_firmware_t f = { { 0 } };

            //if (fileN.endsWith("hex"))
            //{
            //    ihex_chunk_p chunk = NULL;
            //    int cnt = read_ihex_chunks(filename, &chunk);

            //    if (cnt <= 0)
            //    {
            //        QMessageBox::warning(0, tr("Error:"), tr(" Unable to load IHEX file %1\n").arg(fileN));
            //        return false;
            //    }

            //    int lastFChunk = 0;

            //    for (int ci = 0; ci < cnt; ci++)
            //    {
            //        if (chunk[ci].baseaddr < (1 * 1024 * 1024)) lastFChunk = ci;
            //    }
            //    f.flashbase = chunk[0].baseaddr;
            //    f.flashsize = chunk[lastFChunk].baseaddr + chunk[lastFChunk].size;
            //    f.flash = (uint8_t*)malloc(f.flashsize + 1);

            //    for (int ci = 0; ci < cnt; ci++)
            //    {
            //        if (chunk[ci].baseaddr < (1 * 1024 * 1024))
            //        {
            //            memcpy(f.flash + chunk[ci].baseaddr,
            //                    chunk[ci].data,
            //                    chunk[ci].size);
            //        }
            //        if (chunk[ci].baseaddr >= AVR_SEGMENT_OFFSET_EEPROM)
            //        {
            //            f.eeprom = chunk[ci].data;
            //            f.eesize = chunk[ci].size;
            //        }
            //    }
            //}
            ////#ifndef _WIN32
            //else if (fileN.endsWith(".elf"))
            //{
            //    f.flashsize = 0;
            //    elf_read_firmware_ext(filename, &f);

            //    if (!f.flashsize)
            //    {
            //        QMessageBox::warning(0, tr("Failed to load firmware: "), tr("File %1 is not in valid ELF format\n").arg(fileN));
            //        return false;
            //    }
            //}
            ////#endif
            //else                                    // File extension not valid
            //{
            //    QMessageBox::warning(0, tr("Error:"), tr("%1 should be .hex or .elf\n").arg(fileN));
            //    return false;
            //}

            //QString mmcu(f.mmcu );
            //if (!mmcu.isEmpty())
            //{
            //    if (mmcu != m_device)
            //    {
            //        QMessageBox::warning(0, tr("Warning on load firmware: "), tr("Incompatible firmware: compiled for %1 and your processor is %2\n").arg(mmcu).arg(m_device));
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (!strlen(name))
            //    {
            //        QMessageBox::warning(0, tr("Failed to load firmware: "), tr("The processor model is not specified.\n"));
            //        return false;
            //    }
            //    strcpy(f.mmcu, name);
            //}
            //if (!m_avrProcessor)
            //{
            //    m_avrProcessor = avr_make_mcu_by_name(f.mmcu);

            //    if (!m_avrProcessor)
            //    {
            //        QMessageBox::warning(0, tr("Unkown Error:")
            //                               , tr("Could not Create AVR Processor: \"%1\"").arg(f.mmcu));
            //        return false;
            //    }
            //    int started = avr_init(m_avrProcessor);

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
            //}

            ///// TODO: Catch possible abort signal here, otherwise application will crash on the invalid firmware load
            ///// Done: Modified simavr to not call abort(), instead it returns error code.
            //if (avr_load_firmware(m_avrProcessor, &f) != 0)
            //{
            //    QMessageBox::warning(0, tr("Error:"), tr("Wrong firmware!!").arg(f.mmcu));
            //    return false;
            //}
            //if (f.flashbase) m_avrProcessor->pc = f.flashbase;

            //setEeprom(m_eeprom); // Load EEPROM

            //m_avrProcessor->frequency = 16000000;
            //m_avrProcessor->cycle = 0;
            //m_avrProcessor->gdb_port = 1212;
            //m_symbolFile = fileN;

            //if (m_initGdb)
            //{
            //    int ok = avr_gdb_init(m_avrProcessor);
            //    if (ok < 0) qDebug() << "avr_gdb_init ERROR " << ok;
            //    else qDebug() << "avr_gdb_init OK";
            //}

            //initialized();

            return true;
        }

        public void Reset()
        {
            if (!loadStatus) return;
            ////qDebug() << "AvrProcessor::reset("<< eeprom();

            //for (int i = 0; i < avrProcessor->ramend; i++) m_avrProcessor->data[i] = 0;

            //Avr_reset(avrProcessor);
            //avrProcessor.pc = 0;
            //avrProcessor.cycle = 0;
            //nextCycle = mcuStepsPT;
            //extraCycle = 0;
        }

        public void StepOne()
        {
            //qDebug() <<"AvrProcessor::stepOne()"<<m_avrProcessor->cycle << m_nextCycle;

            //if (avrProcessor.state < cpu_Done)
            //    avrProcessor.Run(avrProcessor);

            //while (avrProcessor.cycle >= nextCycle)
            //{
            //    nextCycle += mcuStepsPT; //McuComponent::self()->freq(); //
            //    RunSimuStep(); // 1 simu step = 1uS
            //}
        }

        public int pc()
        {
            return avrProcessor.pc;
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
