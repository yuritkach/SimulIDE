﻿using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{

    ///*
    // * Handles self-programming subsystem if the core
    // * supports it.
    // */
    public class Avr_flash:Avr_io
    {
        public Avr_io io;
        public ushort flags;
        public ushort[] tmppage;
        public byte[] tmppage_used;
        public ushort spm_pagesize;
        public uint r_spm;
        public Avr_regbit selfprgen;
        public Avr_regbit pgers;     // page erase
        public Avr_regbit pgwrt;     // page write
        public Avr_regbit blbset;    // lock bit set
        public Avr_regbit rwwsre;    // read while write section read enable
        public Avr_regbit rwwsb;     // read while write section busy
        public Avr_int_vector flash; // Interrupt vector
    }

    public class Avr_flash_helper
    {
        public static ulong Avr_progen_clear(Avr avr, ulong when, object param)
        {
            Avr_flash p = (Avr_flash)param;
            Sim_regbit.Avr_regbit_clear(p.io.avr, p.selfprgen);
        //    AVR_LOG(avr, LOG_WARNING, "FLASH: avr_progen_clear - SPM not received, clearing PRGEN bit\n");
        	return 0;
        }


        public static void Avr_flash_write(Avr avr, uint addr, byte v, object param)
        {
            Avr_flash p = (Avr_flash) param;
            Sim_core_helper.Avr_core_watch_write(avr, addr, v);
            //	printf("** avr_flash_write %02x\n", v);
            if (Sim_regbit.Avr_regbit_get(avr, p.selfprgen)!=0)
                Sim_cycle_timers.Avr_cycle_timer_register(avr, 4, Avr_progen_clear, p); // 4 cycles is very little!
        }

        public static void Avr_flash_clear_temppage(Avr_flash p)
        {
            for (int i = 0; i < p.spm_pagesize / 2; i++)
            {
                p.tmppage[i] = 0xff;
                p.tmppage_used[i] = 0;
            }
        }

        public static int Avr_flash_ioctl(Avr_io port, uint ctl, params object[] io_param)
        {
        //	if (ctl != AVR_IOCTL_FLASH_SPM)
        //		return -1;

        //	avr_flash_t* p = (avr_flash_t*)port;
        //    avr_t* avr = p->io.avr;

        //    avr_flashaddr_t z = avr->data[R_ZL] | (avr->data[R_ZH] << 8);
        //	if (avr->rampz)
        //		z |= avr->data[avr->rampz] << 16;
        //	uint16_t r01 = avr->data[0] | (avr->data[1] << 8);

        ////	printf("AVR_IOCTL_FLASH_SPM %02x Z:%04x R01:%04x\n", avr->data[p->r_spm], z,r01);
        //	if (avr_regbit_get(avr, p->selfprgen)) {
        //		avr_cycle_timer_cancel(avr, avr_progen_clear, p);

        //		if (avr_regbit_get(avr, p->pgers)) {
        //			z &= ~1;
        //			AVR_LOG(avr, LOG_TRACE, "FLASH: Erasing page %04x (%d)\n", (z / p->spm_pagesize), p->spm_pagesize);
        //			for (int i = 0; i<p->spm_pagesize; i++)
        //				avr->flash[z++] = 0xff;
        //		} else if (avr_regbit_get(avr, p->pgwrt)) {
        //			z &= ~(p->spm_pagesize - 1);
        //			AVR_LOG(avr, LOG_TRACE, "FLASH: Writing page %04x (%d)\n", (z / p->spm_pagesize), p->spm_pagesize);
        //			for (int i = 0; i<p->spm_pagesize / 2; i++) {
        //				avr->flash[z++] = p->tmppage[i];
        //				avr->flash[z++] = p->tmppage[i] >> 8;
        //			}
        //			avr_flash_clear_temppage(p);
        //		} else if (avr_regbit_get(avr, p->blbset)) {
        //			AVR_LOG(avr, LOG_TRACE, "FLASH: Setting lock bits (ignored)\n");
        //		} else if (p->flags & AVR_SELFPROG_HAVE_RWW && avr_regbit_get(avr, p->rwwsre)) {
        //			avr_flash_clear_temppage(p);
        //		} else {
        //			AVR_LOG(avr, LOG_TRACE, "FLASH: Writing temppage %08x (%04x)\n", z, r01);
        //z >>= 1;
        //			if (!p->tmppage_used[z % (p->spm_pagesize / 2)]) {
        //				p->tmppage[z % (p->spm_pagesize / 2)] = r01;
        //				p->tmppage_used[z % (p->spm_pagesize / 2)] = 1;
        //			}
        //		}
        //	}
        //	avr_regbit_clear(avr, p->selfprgen);
        	return 0;
        }

        public static void Avr_flash_reset(Avr_io port)
        {
         //   Avr_flash p = (Avr_flash) port;
         //   Avr_flash_clear_temppage(p);
        }

        public static void Avr_flash_dealloc(Avr_io port)
        {
        //	avr_flash_t* p = (avr_flash_t*)port;

        //	if (p->tmppage)
        //		free(p->tmppage);

        //	if (p->tmppage_used)
        //		free(p->tmppage_used);
        }

        protected static Avr_io _io;
        static Avr_flash_helper()
        {
            _io = new Avr_io();
            _io.kind = "flash";
            _io.ioctl = Avr_flash_ioctl;
            _io.reset = Avr_flash_reset;
            _io.dealloc = Avr_flash_dealloc;


            /* Set if the flash supports a Read While Write section */
            Constants.Set("AVR_SELFPROG_HAVE_RWW", (1 << 0));
        }

        public static void Avr_flash_init(Avr avr, Avr_flash p)
        {
            p.io = _io;
            Console.WriteLine("{0:G} init SPM {1:X4}\n", "Avr_flash_init", p.r_spm);

            if (p.tmppage==null)
                p.tmppage = new ushort[p.spm_pagesize];

            if (p.tmppage_used==null)
                p.tmppage_used = new byte[p.spm_pagesize / 2];

            Sim_io.Avr_register_io(avr, p);
            Sim_interrupts.Avr_register_vector(avr, p.flash);
            Sim_io.Avr_register_io_write(avr, p.r_spm, Avr_flash_write, p);
        }


        public static uint AVR_IOCTL_FLASH_SPM = Sim_io.AVR_IOCTL_DEF((byte)'f', (byte)'s', (byte)'p', (byte)'m');

        public static void AVR_SELFPROG_DECLARE_INTERNAL(ref Avr_flash fl, uint _spmr, byte _spen, byte _vector)
        {
            fl.r_spm = _spmr;
            fl.spm_pagesize = (ushort)Constants.Get("SPM_PAGESIZE");
            fl.selfprgen = Sim_regbit.AVR_IO_REGBIT(_spmr, _spen);
            fl.pgers = Sim_regbit.AVR_IO_REGBIT(_spmr, (byte)Constants.Get("PGERS"));
            fl.pgwrt = Sim_regbit.AVR_IO_REGBIT(_spmr, (byte)Constants.Get("PGWRT"));
            fl.blbset = Sim_regbit.AVR_IO_REGBIT(_spmr, (byte)Constants.Get("BLBSET"));

            if (fl.flash == null)
                fl.flash = new Avr_int_vector();
            fl.flash.enable = Sim_regbit.AVR_IO_REGBIT(_spmr, (byte)Constants.Get("SPMIE"));
            fl.flash.vector = _vector;
        }   


        public static void AVR_SELFPROG_DECLARE_NORWW(ref Avr_flash selfprog, uint _spmr, byte _spen, byte _vector)
        {
            if (selfprog == null)
                selfprog = new Avr_flash();
            selfprog.flags = 0;
            AVR_SELFPROG_DECLARE_INTERNAL(ref selfprog, _spmr, _spen, _vector);
        }

        public static void AVR_SELFPROG_DECLARE(ref Avr_flash selfprog, uint _spmr, byte _spen, byte _vector)
        {
            if (selfprog == null)
                selfprog = new Avr_flash();
            selfprog.flags = (ushort) Constants.Get("AVR_SELFPROG_HAVE_RWW");
            AVR_SELFPROG_DECLARE_INTERNAL(ref selfprog, _spmr, _spen, _vector);
            selfprog.rwwsre = Sim_regbit.AVR_IO_REGBIT(_spmr, (byte)Constants.Get("RWWSRE"));
            selfprog.rwwsb = Sim_regbit.AVR_IO_REGBIT(_spmr, (byte)Constants.Get("RWWSB"));
        
        }


    }
}
