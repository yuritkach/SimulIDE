using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.gui.circuitwidget.components.mcu
{
    class AvrCompBase:McuComponent
    {

        public AvrCompBase(object parent, string type, string id):base(type,id)// , m_avrI2C("avrI2C")
        {
            //avrI2C.setEnabled(false);
            //avrI2C.setComponent(this);
            //sda = null;
            //scl = null;
        }
        
        

        protected void AttachPins()
        {
            AvrProcessor ap = (AvrProcessor) processor;
            avr_t cpu = ap.GetCpu();
            for (int i = 0; i < numpins; i++)
            {
                AVRComponentPin pin = (AVRComponentPin) pinList[i];
                pin.attach(cpu);
            }
            cpu.vcc = 5000;
            cpu.avcc = 5000;

            // ADC irq
//            avr_irq adcIrq = avr_io_getirq(cpu, AVR_IOCTL_ADC_GETIRQ, ADC_IRQ_OUT_TRIGGER);
//            avr_irq_register_notify(adcIrq, adc_hook, this);

            //if (sda && scl)
            //{
            //    // I2C Out irq
            //    avr_irq_t* i2cOutIrq = avr_io_getirq(cpu, AVR_IOCTL_TWI_GETIRQ(0), TWI_IRQ_OUTPUT);
            //    avr_irq_register_notify(i2cOutIrq, i2c_out_hook, this);

            //    // TWEN change irq
            //    int twcrAddr = BaseProcessor::self()->getRegAddress("TWCR");
            //    if (twcrAddr < 0) qDebug() << "AvrCompBase::attachPins: TWCR Register Not found";
            //    else
            //    {
            //        qDebug() << "AvrCompBase::attachPins Found SDA SCL";
            //        m_avrI2C.setInput(0, m_sda);         // Input SDA
            //        m_avrI2C.setClockPin(m_scl);         // Input SCL

            //        avr_irq_t* twenIrq = avr_iomem_getirq(cpu, twcrAddr, 0l, 2);
            //        avr_irq_register_notify(twenIrq, twen_hook, this);

            //        m_i2cInIrq = avr_io_getirq(cpu, AVR_IOCTL_TWI_GETIRQ(0), TWI_IRQ_INPUT);
            //    }
            //}
            attached = true;
        }

        protected void AddPin(string id, string type, string label, int pos, int xpos, int ypos, int angle)
        {
            if (initialized)
            {
                UpdatePin(id, type, label, pos, xpos, ypos, angle);
            }
            else
            {
                //qDebug()<<pos<<id<<label;
                AVRComponentPin newPin = new AVRComponentPin(this, id, type, label, pos, xpos, ypos, angle);
                pinList.Add(newPin);

                string ty = GetType(type, "adc");
                //if (ty!="") ADCpinList[ty.remove("adc").toInt()] = newPin;

                ty = GetType(type, "sda");
                if (ty!="") sda = newPin;

                ty = GetType(type, "scl");
                if (ty!="") scl = newPin;
            }
        }

        protected string GetType(string type, string t)
        {
            if (type.Contains(t))
            {
                var types = type.Split(',');
                foreach (string ty in types)
                {
                    if (ty.StartsWith(t)) return ty;
                }
            }
            return "";
        }

        public void Adcread(int channel)
        {
            //qDebug() << "ADC Read channel:" << channel;
            AVRComponentPin pin = ADCpinList.Value(channel);
            if (pin!=null) pin.Adcread();
        }

        //void I2cOut(uint32_t value)
        //{
        //    avr_twi_msg_irq_t v;
        //    v.u.v = value;
        //    uint32_t msg = v.u.twi.msg;

        //    //qDebug() << "AvrCompBase::i2cOut" << value<< msg;

        //    if (msg & TWI_COND_START)
        //    {
        //        //m_slvAddr = v.u.twi.addr;
        //        m_avrI2C.masterStart(0);
        //    }
        //    else if (msg & TWI_COND_ADDR)
        //    {
        //        m_slvAddr = v.u.twi.addr;
        //        m_avrI2C.masterWrite(m_slvAddr);
        //    }
        //    else if (msg & TWI_COND_WRITE)
        //    {
        //        m_avrI2C.masterWrite(v.u.twi.data);
        //    }
        //    else if (msg & TWI_COND_STOP)
        //    {
        //        m_avrI2C.masterStop();
        //    }
        //    else if (msg & TWI_COND_READ)
        //    {
        //        m_avrI2C.masterRead();
        //    }
        //    else
        //        qDebug() << "AvrCompBase::i2cOut UNKNOWN ACTION";
        //}

        public void InStateChanged(int value)
        {
            if (value < 128) return;

            //if (value & 256) // ACK received
            //{
            //    uint32_t irqMsg = avr_twi_irq_msg(TWI_COND_ACK, m_slvAddr, value & 1);
            //    avr_raise_irq(m_i2cInIrq, irqMsg);
            //}
            //else if (value == 128) // Start Condition sent
            //{
            //    uint32_t irqMsg = avr_twi_irq_msg(TWI_COND_START, m_slvAddr, 0);
            //    avr_raise_irq(m_i2cInIrq, irqMsg);
            //}
            //else if (value == 130) // Received a byte
            //{
            //    uint32_t irqMsg = avr_twi_irq_msg(TWI_COND_READ, m_slvAddr, m_avrI2C.byteReceived());
            //    avr_raise_irq(m_i2cInIrq, irqMsg);
            //}
            //else if (value == 132) // Stop Condition sent
            //{
            //    uint32_t irqMsg = avr_twi_irq_msg(TWI_COND_STOP, m_slvAddr, 0);
            //    avr_raise_irq(m_i2cInIrq, irqMsg);
            //}
        }

        public void TwenChanged(UInt32 value)
        {
            //if (!(sda && scl)) return;

            //qDebug() << "AvrCompBase::twenChanged Enable:" << value;

            if (value!=0)
            {
                //avrI2C.setEnabled(true);
                //avrI2C.setMaster(true);
                //sda.EnableIO(false);
                //scl.EnableIO(false);

                double i2cFreq = 4e5;

                int twbr = BaseProcessor.Self().GetRamValue("TWBR");
                int twsr = BaseProcessor.Self().GetRamValue("TWSR");

                if ((twbr < 0) || (twsr < 0))
                {
                 //   qDebug() << "AvrCompBase::twenChanged: TWBR or TWSR not found";
                }
                else                       // Calculate Prescaler and Frequency
                {
                    int pr = (int)Math.Pow(4, (twsr & 3));
                    double dpr = 16 + 2 * twbr * pr;
//                    i2cFreq = this.freq() * 1e6 / dpr;
                }
  //              avrI2C.setFreq(i2cFreq);

//                qDebug() << "AvrCompBase::twenChanged i2cFreq:" << i2cFreq;
            }
            else
            {
                //avrI2C.setEnabled(false);
                //avrI2C.setMaster(false);
                //sda->enableIO(true);
                //scl->enableIO(true);
            }
        }

        public bool InitGdb()
        {
            AvrProcessor ap = (AvrProcessor)(processor);
            return ap.InitGdb();
        }

        public void SetInitGdb(bool init)
        {
            AvrProcessor ap = (AvrProcessor)(processor);
            ap.SetInitGdb(init);
        }

        //int getRamValue( int address );

        
//        static void adc_hook( struct avr_irq_t* irq, uint32_t value, void* param )
//        {
//            Q_UNUSED(irq);
//        AvrCompBase* ptrAvr = reinterpret_cast<AvrCompBase*>(param);

//        int channel = int(value / 524288);
//        ptrAvr->adcread(channel );
//    }

//    static void i2c_out_hook( struct avr_irq_t* irq, uint32_t value, void* param )
//        {
//            Q_UNUSED(irq);
//    AvrCompBase* ptrAvr = reinterpret_cast<AvrCompBase*>(param);

//    ptrAvr->i2cOut(value );
//}

//        public static void Twen_hook(avr_irq irq, UInt32 value/*, void* param*/ )
//        {
//            Q_UNUSED(irq);
//           AvrCompBase ptrAvr = reinterpret_cast<AvrCompBase*>(param);
//           ptrAvr->twenChanged(value );
//        }


        protected Dictionary<int, AVRComponentPin> ADCpinList;
        protected eI2C avrI2C;
        protected AVRComponentPin sda;
        protected AVRComponentPin scl;
        protected byte slvAddr;
        protected avr_irq_t i2cInIrq;
        protected AvrProcessor avr;

    }
}
