using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
    

    public class Sim_io
    {



        //# include <stdlib.h>
        //# include <stdio.h>
        //# include <string.h>
        //# include <ctype.h>
        //# include <stdint.h>
        //# include "sim_io.h"

        public static uint AVR_IOCTL_DEF(byte _a,byte _b,byte _c,byte _d)
        {
            return (uint)(((_a) << 24) | ((_b) << 16) | ((_c) << 8) | ((_d)));
        }

        public static int Avr_ioctl(Avr avr,uint ctl,object io_param)
        {
            List<Avr_io> ports = avr.io_ports;
            int res = -1;
            for(int i=0;i<ports.Count;i++)
            {
                if (ports[i].ioctl!=null)
                    res = ports[i].ioctl(ports[i], ctl, io_param);
            }
            return res;
        }

//        void
//        avr_register_io(
//                avr_t* avr,
//                avr_io_t* io)
//        {
//            io->next = avr->io_port;
//            io->avr = avr;
//            avr->io_port = io;
//        }

//        void
//        avr_register_io_read(
//                avr_t* avr,
//                avr_io_addr_t addr,
//                avr_io_read_t readp,
//                void* param)
//        {
//            avr_io_addr_t a = AVR_DATA_TO_IO(addr);
//            if (avr->io[a].r.param || avr->io[a].r.c)
//            {
//                if (avr->io[a].r.param != param || avr->io[a].r.c != readp)
//                {
//                    AVR_LOG(avr, LOG_ERROR,
//                            "IO: %s(): Already registered, refusing to override.\n",
//                            __func__);
//                    AVR_LOG(avr, LOG_ERROR,
//                            "IO: %s(%04x : %p/%p): %p/%p\n",
//                            __func__, a,
//                            avr->io[a].r.c, avr->io[a].r.param, readp, param);
//                    abort();
//                }
//            }
//            avr->io[a].r.param = param;
//            avr->io[a].r.c = readp;
//        }

//        static void
//        _avr_io_mux_write(
//                avr_t* avr,
//                avr_io_addr_t addr,
//                uint8_t v,
//                void* param)
//        {
//            int io = (intptr_t)param;
//            for (int i = 0; i < avr->io_shared_io[io].used; i++)
//            {
//                avr_io_write_t c = avr->io_shared_io[io].io[i].c;
//                if (c)
//                    c(avr, addr, v, avr->io_shared_io[io].io[i].param);
//            }
//        }

        public static void Avr_register_io_write(Avr avr, uint addr, Avr_io_write_function writep,object[] param)
        {
            uint a =(UInt16) Sim_Avr.AVR_DATA_TO_IO(addr);

            if (a >= Sim_Avr.MAX_IOs)
                throw new Exception("IO address 0x "+a.ToString()+" out of range ("+ Sim_Avr.MAX_IOs.ToString()+")");
            /*
             * Verifying that some other piece of code is not installed to watch write
             * on this address. If there is, this code installs a "dispatcher" callback
             * instead to handle multiple clients, otherwise, it continues as usual
             */
            if ((avr.io[a].w.param!=null) || (avr.io[a].w.c!=null))
            {
//                if (avr->io[a].w.param != param || avr->io[a].w.c != writep)
//                {
//                    // if the muxer not already installed, allocate a new slot
//                    if (avr->io[a].w.c != _avr_io_mux_write)
//                    {
//                        int no = avr->io_shared_io_count++;
//                        if (avr->io_shared_io_count > ARRAY_SIZE(avr->io_shared_io))
//                        {
//                            AVR_LOG(avr, LOG_ERROR,
//                                    "IO: %s(): Too many shared IO registers.\n", __func__);
//                            abort();
//                        }
//                        AVR_LOG(avr, LOG_TRACE,
//                                "IO: %s(%04x): Installing muxer on register.\n",
//                                __func__, addr);
//                        avr->io_shared_io[no].used = 1;
//                        avr->io_shared_io[no].io[0].param = avr->io[a].w.param;
//                        avr->io_shared_io[no].io[0].c = avr->io[a].w.c;
//                        avr->io[a].w.param = (void*)(intptr_t)no;
//                        avr->io[a].w.c = _avr_io_mux_write;
//                    }
//                    int no = (intptr_t)avr->io[a].w.param;
//                    int d = avr->io_shared_io[no].used++;
//                    if (avr->io_shared_io[no].used > ARRAY_SIZE(avr->io_shared_io[0].io))
//                    {
//                        AVR_LOG(avr, LOG_ERROR,
//                                "IO: %s(): Too many callbacks on %04x.\n",
//                                __func__, addr);
//                        abort();
//                    }
//                    avr->io_shared_io[no].io[d].param = param;
//                    avr->io_shared_io[no].io[d].c = writep;
//                    return;
//                }
            }
             
            avr.io[a].w.param = param;
            avr.io[a].w.c = writep;
        }

//        avr_irq_t*
//        avr_io_getirq(
//                avr_t* avr,
//                uint32_t ctl,
//                int index)
//        {
//            avr_io_t* port = avr->io_port;
//            while (port)
//            {
//                if (port->irq && port->irq_ioctl_get == ctl && port->irq_count > index)
//                    return port->irq + index;
//                port = port->next;
//            }
//            return NULL;
//        }

//        avr_irq_t*
//        avr_iomem_getirq(
//                avr_t* avr,
//                avr_io_addr_t addr,

//        const char* name,

//        int index)
//{
//	if (index > 8)
//		return NULL;
//	avr_io_addr_t a = AVR_DATA_TO_IO(addr);
//	if (avr->io[a].irq == NULL) {
//		/*
//		 * Prepare an array of names for the io IRQs. Ideally we'd love to have
//		 * a proper name for these, but it's not possible at this time.
//		 */
//		char names[9 * 20];
//        char* d = names;
//        const char* namep[9];
//		for (int ni = 0; ni< 9; ni++) {
//			if (ni< 8)

//                sprintf(d, "=avr.io.%04x.%d", addr, ni);
//			else
//				sprintf(d, "8=avr.io.%04x.all", addr);
//        namep[ni] = d;
//			d += strlen(d) + 1;
//		}
//    avr->io[a].irq = avr_alloc_irq(&avr->irq_pool, 0, 9, namep);
//		// mark the pin ones as filtered, so they only are raised when changing
//		for (int i = 0; i< 8; i++)
//			avr->io[a].irq[i].flags |= IRQ_FLAG_FILTERED;
//	}
//	// if given a name, replace the default one...
//	if (name) {
//		int l = strlen(name);
//char n[l + 10];
//sprintf(n, "avr.io.%s", name);
//free((void*) avr->io[a].irq[index].name);
//avr->io[a].irq[index].name = strdup(n);
//	}
//	return avr->io[a].irq + index;
//}

//avr_irq_t*
//avr_io_setirqs(
//        avr_io_t* io,
//        uint32_t ctl,
//        int count,
//        avr_irq_t* irqs)
//{
//    // allocate this module's IRQ
//    io->irq_count = count;

//    if (!irqs)
//    {
//        const char** irq_names = NULL;

//        if (io->irq_names)
//        {
//            irq_names = malloc(count * sizeof(char*));
//            memset(irq_names, 0, count * sizeof(char*));
//            char buf[64];
//            for (int i = 0; i < count; i++)
//            {
//                /*
//				 * this bit takes the io module 'kind' ("port")
//				 * the IRQ name ("=0") and the last character of the ioctl ('p','o','r','A')
//				 * to create a full name "=porta.0"
//				 */
//                char* dst = buf;
//                // copy the 'flags' of the name out
//                const char* kind = io->irq_names[i];
//                while (isdigit(*kind))
//                    *dst++ = *kind++;
//                while (!isalpha(*kind))
//                    *dst++ = *kind++;
//                // add avr name
//                //				strcpy(dst, io->avr->mmcu);
//                strcpy(dst, "avr");
//                dst += strlen(dst);
//                *dst++ = '.';
//                // add module 'kind'
//                strcpy(dst, io->kind);
//                dst += strlen(dst);
//                // add port name, if any
//                if ((ctl & 0xff) > ' ')
//                    *dst++ = tolower(ctl & 0xff);
//                *dst++ = '.';
//                // add the rest of the irq name
//                strcpy(dst, kind);
//                dst += strlen(dst);
//                *dst = 0;

//                //				printf("%s\n", buf);
//                irq_names[i] = strdup(buf);
//            }
//        }
//        irqs = avr_alloc_irq(&io->avr->irq_pool, 0,
//                        count, irq_names);
//        if (irq_names)
//        {
//            for (int i = 0; i < count; i++)
//                free((char*)irq_names[i]);
//            free((char*)irq_names);
//        }
//    }

//    io->irq = irqs;
//    io->irq_ioctl_get = ctl;
//    return io->irq;
//}

//static void
//avr_deallocate_io(
//        avr_io_t* io)
//{
//    if (io->dealloc)
//        io->dealloc(io);
//    avr_free_irq(io->irq, io->irq_count);
//    io->irq_count = 0;
//    io->irq_ioctl_get = 0;
//    io->avr = NULL;
//    io->next = NULL;
//}

//void
//avr_deallocate_ios(
//        avr_t* avr)
//{
//    avr_io_t* port = avr->io_port;
//    while (port)
//    {
//        avr_io_t* next = port->next;
//        avr_deallocate_io(port);
//        port = next;
//    }
//    avr->io_port = NULL;
//}












//        /*
//	sim_io.h

//	Copyright 2008, 2009 Michel Pollet <buserror@gmail.com>

// 	This file is part of simavr.

//	simavr is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.

//	simavr is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with simavr.  If not, see <http://www.gnu.org/licenses/>.
// */

//# ifndef __SIM_IO_H__
//#define __SIM_IO_H__

//# include "sim_avr.h"

//# ifdef __cplusplus
//        extern "C" {
//#endif

///*
// * used by the ioports to implement their own features
// * see avr_eeprom.* for an example, and avr_ioctl().
// */
//#define AVR_IOCTL_DEF(_a,_b,_c,_d) \
//	(((_a) << 24)|((_b) << 16)|((_c) << 8)|((_d)))

///*
// * IO module base struct
// * Modules uses that as their first member in their own struct
// */
//typedef struct avr_io_t
//        {
//            struct avr_io_t * 	next;
//	avr_t* avr;     // avr we are attached to
//            const char* kind;       // pretty name, for debug

//            const char** irq_names; // IRQ names

//            uint32_t irq_ioctl_get; // used to get irqs from this module
//            int irq_count;  // number of (optional) irqs
//            struct avr_irq_t *	irq;		// optional external IRQs
//	// called at reset time
//	void (* reset) (struct avr_io_t *io);
//	// called externally. allow access to io modules and so on
//	int (* ioctl) (struct avr_io_t *io, uint32_t ctl, void* io_param);

//            // optional, a function to free up allocated system resources
//            void (* dealloc) (struct avr_io_t *io);
//}
//        avr_io_t;

///*
// * IO modules helper functions
// */

//// registers an IO module, so it's run(), reset() etc are called
//// this is called by the AVR core init functions, you /could/ register an external
//// one after instantiation, for whatever purpose...
//void
//avr_register_io(
//        avr_t* avr,
//        avr_io_t* io);
//        // Sets an IO module "official" IRQs and the ioctl used to get to them. if 'irqs' is NULL,
//        // 'count' will be allocated
//        avr_irq_t*
//        avr_io_setirqs(
//                avr_io_t* io,
//                uint32_t ctl,
//                int count,
//                avr_irq_t* irqs);

//        // register a callback for when IO register "addr" is read
//        void
//        avr_register_io_read(
//                avr_t* avr,
//                avr_io_addr_t addr,
//                avr_io_read_t read,
//                void* param);
//        // register a callback for when the IO register is written. callback has to set the memory itself
//        void
//        avr_register_io_write(
//                avr_t* avr,
//                avr_io_addr_t addr,
//                avr_io_write_t write,
//                void* param);
//        // call every IO modules until one responds to this
//        int
//        avr_ioctl(
//                avr_t* avr,
//                uint32_t ctl,
//                void* io_param);
//        // get the specific irq for a module, check AVR_IOCTL_IOPORT_GETIRQ for example
//        struct avr_irq_t *
//avr_io_getirq(
//        avr_t* avr,
//        uint32_t ctl,
//        int index);

//        // get the IRQ for an absolute IO address
//        // this allows any code to hook an IRQ in any io address, for example
//        // tracing changes of values into a register
//        // Note that the values do not "magically" change, they change only
//        // when the AVR code attempt to read and write at that address
//        //
//        // the "index" is a bit number, or ALL bits if index == 8
//#define AVR_IOMEM_IRQ_ALL 8
//        avr_irq_t*
//        avr_iomem_getirq(
//                avr_t* avr,
//                avr_io_addr_t addr,

//        const char* name /* Optional, if NULL, "ioXXXX" will be used */ ,

//        int index);

//        // Terminates all IOs and remove from them from the io chain
//        void
//        avr_deallocate_ios(
//                avr_t* avr);

//# ifdef __cplusplus
//    };
//#endif

//#endif /* __SIM_IO_H__ */

}
}
