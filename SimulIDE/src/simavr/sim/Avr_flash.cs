using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
    class Avr_flash
    {


//        static avr_cycle_count_t avr_progen_clear(struct avr_t * avr, avr_cycle_count_t when, void* param)
//{

//    avr_flash_t* p = (avr_flash_t*)param;
//        avr_regbit_clear(p->io.avr, p->selfprgen);
//        AVR_LOG(avr, LOG_WARNING, "FLASH: avr_progen_clear - SPM not received, clearing PRGEN bit\n");
//	return 0;
//}


//    static void avr_flash_write(avr_t* avr, avr_io_addr_t addr, uint8_t v, void* param)
//    {
//        avr_flash_t* p = (avr_flash_t*)param;

//        avr_core_watch_write(avr, addr, v);

//        //	printf("** avr_flash_write %02x\n", v);

//        if (avr_regbit_get(avr, p->selfprgen))
//            avr_cycle_timer_register(avr, 4, avr_progen_clear, p); // 4 cycles is very little!
//    }

//    static void avr_flash_clear_temppage(avr_flash_t* p)
//    {
//        for (int i = 0; i < p->spm_pagesize / 2; i++)
//        {
//            p->tmppage[i] = 0xff;
//            p->tmppage_used[i] = 0;
//        }
//    }

//    static int avr_flash_ioctl(struct avr_io_t * port, uint32_t ctl, void* io_param)
//{
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
//	return 0;
//}

//static void
//avr_flash_reset(avr_io_t* port)
//{
//    avr_flash_t* p = (avr_flash_t*)port;

//    avr_flash_clear_temppage(p);
//}

//static void
//avr_flash_dealloc(struct avr_io_t * port)
//{
//	avr_flash_t* p = (avr_flash_t*)port;

//	if (p->tmppage)
//		free(p->tmppage);

//	if (p->tmppage_used)
//		free(p->tmppage_used);
//}

//static avr_io_t _io = {
//	.kind = "flash",
//	.ioctl = avr_flash_ioctl,
//	.reset = avr_flash_reset,
//	.dealloc = avr_flash_dealloc,
//};

//void avr_flash_init(avr_t* avr, avr_flash_t* p)
//{
//    p->io = _io;
//    //	printf("%s init SPM %04x\n", __FUNCTION__, p->r_spm);

//    if (!p->tmppage)
//        p->tmppage = malloc(p->spm_pagesize);

//    if (!p->tmppage_used)
//        p->tmppage_used = malloc(p->spm_pagesize / 2);

//    avr_register_io(avr, &p->io);
//    avr_register_vector(avr, &p->flash);

//    avr_register_io_write(avr, p->r_spm, avr_flash_write, p);
//}








//# include "sim_avr.h"

///*
// * Handles self-programming subsystem if the core
// * supports it.
// */
//typedef struct avr_flash_t
//        {
//            avr_io_t io;

//            uint16_t flags;
//            uint16_t* tmppage;
//            uint8_t* tmppage_used;
//            uint16_t spm_pagesize;
//            uint8_t r_spm;
//            avr_regbit_t selfprgen;
//            avr_regbit_t pgers;     // page erase
//            avr_regbit_t pgwrt;     // page write
//            avr_regbit_t blbset;    // lock bit set
//            avr_regbit_t rwwsre;    // read while write section read enable
//            avr_regbit_t rwwsb;     // read while write section busy

//            avr_int_vector_t flash; // Interrupt vector
//        }
//        avr_flash_t;

///* Set if the flash supports a Read While Write section */
//#define AVR_SELFPROG_HAVE_RWW (1 << 0)

//void avr_flash_init(avr_t* avr, avr_flash_t* p);


//#define AVR_IOCTL_FLASH_SPM		AVR_IOCTL_DEF('f','s','p','m')

//#define AVR_SELFPROG_DECLARE_INTERNAL(_spmr, _spen, _vector) \
//		.r_spm = _spmr,\
//		.spm_pagesize = SPM_PAGESIZE,\
//		.selfprgen = AVR_IO_REGBIT(_spmr, _spen),\
//		.pgers = AVR_IO_REGBIT(_spmr, PGERS),\
//		.pgwrt = AVR_IO_REGBIT(_spmr, PGWRT),\
//		.blbset = AVR_IO_REGBIT(_spmr, BLBSET),\
//		.flash = {\
//			.enable = AVR_IO_REGBIT(_spmr, SPMIE),\
//			.vector = _vector,\
//		}\

//#define AVR_SELFPROG_DECLARE_NORWW(_spmr, _spen, _vector) \
//	.selfprog = {\
//		.flags = 0,\
//		AVR_SELFPROG_DECLARE_INTERNAL(_spmr, _spen, _vector),\
//	}

//#define AVR_SELFPROG_DECLARE(_spmr, _spen, _vector) \
//	.selfprog = {\
//		.flags = AVR_SELFPROG_HAVE_RWW,\
//		AVR_SELFPROG_DECLARE_INTERNAL(_spmr, _spen, _vector),\
//		.rwwsre = AVR_IO_REGBIT(_spmr, RWWSRE),\
//		.rwwsb = AVR_IO_REGBIT(_spmr, RWWSB),\
//	}


    }
}
