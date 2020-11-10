using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{

    public class Avr_eeprom
    {
        //            avr_io_t io;
        public byte[] eeprom;    // actual bytes
        public UInt16 size;  // size for this MCU

        public byte r_eearh;
        public byte r_eearl;
        public byte r_eedr;
        //            eepm -- eeprom write mode
        public byte r_eecr; // shortcut, assumes these bits fit in that register
        public Avr_regbit[] eepm; //[4];
        public Avr_regbit eempe; // eeprom master program enable
        public Avr_regbit eepe;  // eeprom program enable
        public Avr_regbit eere;  // eeprom read enable
        public Avr_int_vector ready; // EERIE vector

        public static void Avr_eeprom_declare(Mcu mcu, byte _vector)
        {

            mcu.eeprom.size = (ushort)(mcu.GetValue("E2END") + 1);
            mcu.eeprom.r_eearh = mcu.GetValue("EEARH");
            mcu.eeprom.r_eearl = mcu.GetValue("EEARL");
            mcu.eeprom.r_eedr = mcu.GetValue("EEDR");
            mcu.eeprom.r_eedr = mcu.GetValue("EECR");
            mcu.eeprom.eepm = new Avr_regbit[2] { Sim_regbit.AVR_IO_REGBIT(mcu.GetValue("EECR"), mcu.GetValue("EEPM0")), Sim_regbit.AVR_IO_REGBIT(mcu.GetValue("EECR"), mcu.GetValue("EEPM1")) };
            mcu.eeprom.eempe = Sim_regbit.AVR_IO_REGBIT(mcu.GetValue("EECR"), mcu.GetValue("EEMPE"));
            mcu.eeprom.eepe = Sim_regbit.AVR_IO_REGBIT(mcu.GetValue("EECR"), mcu.GetValue("EEPE"));
            mcu.eeprom.eere = Sim_regbit.AVR_IO_REGBIT(mcu.GetValue("EECR"), mcu.GetValue("EERE"));
            Avr_int_vector vec = new Avr_int_vector();
            vec.enable = Sim_regbit.AVR_IO_REGBIT(mcu.GetValue("EECR"), mcu.GetValue("EERIE"));
            vec.vector = _vector;
            mcu.eeprom.ready = vec;
        }

    }



    public class Avr_eeprom_desc
    {
        byte[] ee;
        UInt16 offset;
        UInt32 size;
    }



    class Avr_eeprom_ns
    {


//        static avr_cycle_count_t avr_eempe_clear(struct avr_t * avr, avr_cycle_count_t when, void* param)
//{

//    avr_eeprom_t* p = (avr_eeprom_t*)param;
//        avr_regbit_clear(p->io.avr, p->eempe);
//	return 0;
//}

//    static avr_cycle_count_t avr_eei_raise(struct avr_t * avr, avr_cycle_count_t when, void* param)
//{
//	avr_eeprom_t* p = (avr_eeprom_t*)param;
//    avr_raise_interrupt(p->io.avr, &p->ready);
//	return 0;
//}

//static void avr_eeprom_write(avr_t* avr, avr_io_addr_t addr, uint8_t v, void* param)
//{
//    avr_eeprom_t* p = (avr_eeprom_t*)param;
//    uint8_t eempe = avr_regbit_get(avr, p->eempe);

//    avr_core_watch_write(avr, addr, v);

//    if (!eempe && avr_regbit_get(avr, p->eempe))
//    {
//        avr_cycle_timer_register(avr, 4, avr_eempe_clear, p);
//    }

//    uint16_t ee_addr;
//    if (p->r_eearh)
//        ee_addr = avr->data[p->r_eearl] | (avr->data[p->r_eearh] << 8);
//    else
//        ee_addr = avr->data[p->r_eearl];
//    if (((eempe && avr_regbit_get(avr, p->eepe)) || avr_regbit_get(avr, p->eere)) &&
//            ee_addr >= p->size)
//    {
//        AVR_LOG(avr, LOG_ERROR, "EEPROM: *** %s address out of bounds: %04x > %04x,"

//                                " wrapping to %04x (PC=%04x)\n",
//                eempe ? "Write" : "Read",
//                ee_addr, p->size - 1, ee_addr & (p->size - 1),
//                avr->pc);
//        ee_addr = ee_addr & (p->size - 1);
//    }
//    if (eempe && avr_regbit_get(avr, p->eepe))
//    {   // write operation
//        //	printf("eeprom write %04x <- %02x\n", addr, avr->data[p->r_eedr]);
//        p->eeprom[ee_addr] = avr->data[p->r_eedr];
//        // Automatically clears that bit (?)
//        avr_regbit_clear(avr, p->eempe);

//        avr_cycle_timer_register_usec(avr, 3400, avr_eei_raise, p); // 3.4ms here
//    }
//    if (avr_regbit_get(avr, p->eere))
//    {   // read operation
//        avr->data[p->r_eedr] = p->eeprom[ee_addr];
//        //	printf("eeprom read %04x : %02x\n", addr, p->eeprom[addr]);
//    }

//    // autocleared
//    avr_regbit_clear(avr, p->eepe);
//    avr_regbit_clear(avr, p->eere);
//}

//static int avr_eeprom_ioctl(struct avr_io_t * port, uint32_t ctl, void* io_param)
//{
//	avr_eeprom_t* p = (avr_eeprom_t*)port;
//int res = -1;

//	switch(ctl) {
//		case AVR_IOCTL_EEPROM_SET: {
//			avr_eeprom_desc_t* desc = (avr_eeprom_desc_t*)io_param;
//			if (!desc || !desc->size || !desc->ee || (desc->offset + desc->size) > p->size) {
//				AVR_LOG(port->avr, LOG_WARNING, "EEPROM: %s: AVR_IOCTL_EEPROM_SET Invalid argument\n",
//                        __FUNCTION__);
//				return -2;
//			}
//			memcpy(p->eeprom + desc->offset, desc->ee, desc->size);
//AVR_LOG(port->avr, LOG_TRACE, "EEPROM: %s: AVR_IOCTL_EEPROM_SET Loaded %d at offset %d\n",
//        __FUNCTION__, desc->size, desc->offset);
//		}	break;
//		case AVR_IOCTL_EEPROM_GET: {
//			avr_eeprom_desc_t* desc = (avr_eeprom_desc_t*)io_param;
//			if (!desc || (desc->offset + desc->size) > p->size) {
//				AVR_LOG(port->avr, LOG_WARNING, "EEPROM: %s: AVR_IOCTL_EEPROM_GET Invalid argument\n",
//                        __FUNCTION__);
//				return -2;
//			}
//			if (desc->ee)
//				memcpy(desc->ee, p->eeprom + desc->offset, desc->size);
//			else	// allow to get access to the read data, for gdb support
//				desc->ee = p->eeprom + desc->offset;
//		}	break;
//	}
	
//	return res;
//}

//static void avr_eeprom_dealloc(struct avr_io_t * port)
//{
//	avr_eeprom_t* p = (avr_eeprom_t*)port;
//	if (p->eeprom)
//		free(p->eeprom);
//p->eeprom = NULL;
//}

//static avr_io_t _io = {
//	.kind = "eeprom",
//	.ioctl = avr_eeprom_ioctl,
//	.dealloc = avr_eeprom_dealloc,
//};

//void avr_eeprom_init(avr_t* avr, avr_eeprom_t* p)
//{
//    p->io = _io;
//    //	printf("%s init (%d bytes) EEL/H:%02x/%02x EED=%02x EEC=%02x\n",
//    //			__FUNCTION__, p->size, p->r_eearl, p->r_eearh, p->r_eedr, p->r_eecr);

//    p->eeprom = malloc(p->size);
//    memset(p->eeprom, 0xff, p->size);

//    avr_register_io(avr, &p->io);
//    avr_register_vector(avr, &p->ready);

//    avr_register_io_write(avr, p->r_eecr, avr_eeprom_write, p);
//}



//# ifndef __AVR_EEPROM_H__
//#define __AVR_EEPROM_H__

        //#define AVR_IOCTL_EEPROM_GET	AVR_IOCTL_DEF('e','e','g','p')
        //#define AVR_IOCTL_EEPROM_SET	AVR_IOCTL_DEF('e','e','s','p')


        ///*
        // * the eeprom block seems to be very similar across AVRs, 
        // * so here is a macro to declare a "typical" one in a core.
        // */

        

        //	}

        ///*
        // * no EEPM registers in atmega128
        // */
        //#define AVR_EEPROM_DECLARE_NOEEPM(_vector)		\
        //	.eeprom = {\
        //		.size = E2END+1,\
        //		.r_eearh = EEARH,\
        //		.r_eearl = EEARL,\
        //		.r_eedr = EEDR,\
        //		.r_eecr = EECR,\
        //		.eepm = { },		\
        //		.eempe = AVR_IO_REGBIT(EECR, EEMWE),\
        //		.eepe = AVR_IO_REGBIT(EECR, EEWE),\
        //		.eere = AVR_IO_REGBIT(EECR, EERE),\
        //		.ready = {\
        //			.enable = AVR_IO_REGBIT(EECR, EERIE),\
        //			.vector = _vector,\
        //		},\
        //	}


        ///*
        // * macro definition without a high address bit register,
        // * which is not implemented in some tiny AVRs.
        // */

        //#define AVR_EEPROM_DECLARE_8BIT(_vector) \
        //	.eeprom = {\
        //		.size = E2END+1,\
        //		.r_eearl = EEAR,\
        //		.r_eedr = EEDR,\
        //		.r_eecr = EECR,\
        //		.eepm = { AVR_IO_REGBIT(EECR, EEPM0), AVR_IO_REGBIT(EECR, EEPM1) },\
        //		.eempe = AVR_IO_REGBIT(EECR, EEMPE),\
        //		.eepe = AVR_IO_REGBIT(EECR, EEPE),\
        //		.eere = AVR_IO_REGBIT(EECR, EERE),\
        //		.ready = {\
        //			.enable = AVR_IO_REGBIT(EECR, EERIE),\
        //			.vector = _vector,\
        //		},\
        //	}

    }
}
