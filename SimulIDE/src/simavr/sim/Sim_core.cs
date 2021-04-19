using System;

namespace SimulIDE.src.simavr.sim
{
    public class Sim_core:Sim_Avr
    {
        static Sim_core()
        {
            reg_names[R_XH] = "XH";
            reg_names[R_XL] = "XL";
            reg_names[R_YH] = "YH";
            reg_names[R_YL] = "YL";
            reg_names[R_ZH] = "ZH";
            reg_names[R_ZL] = "ZL";
            reg_names[R_SPH] = "SPH";
            reg_names[R_SPL] = "SPL";
            reg_names[R_SREG] = "SREG";
        }

     

    
    //# ifdef __cplusplus
    //extern "C" {
    //#endif

    //#ifdef NO_COLOR
    //	#define FONT_GREEN
    //	#define FONT_RED
    //	#define FONT_DEFAULT
    //#else
    //	#define FONT_GREEN		"\e[32m"
    //	#define FONT_RED		"\e[31m"
    //	#define FONT_DEFAULT	"\e[0m"
    //#endif

    ///*
    // * Instruction decoder, run ONE instruction
    // */
    
    ///*
    // * These are for internal access to the stack (for interrupts)
    // */
    //uint16_t _avr_sp_get(avr_t* avr);
    //void _avr_sp_set(avr_t* avr, uint16_t sp);
    //int _avr_push_addr(avr_t* avr, avr_flashaddr_t addr);

    //#if CONFIG_SIMAVR_TRACE

    ///*
    // * Get a "pretty" register name
    // */
    //const char * avr_regname(byte reg);

    ///*
    // * DEBUG bits follow
    // * These will disappear when gdb arrives
    // */
    //void avr_dump_state(avr_t * avr);

    //#define DUMP_REG() { \
    //				for (int i = 0; i < 32; i++) printf("%s=%02x%c", avr_regname(i), avr.data[i],i==15?'\n':' ');\
    //				printf("\n");\
    //				uint16_t y = avr.data[R_YL] | (avr.data[R_YH]<<8);\
    //				for (int i = 0; i < 20; i++) printf("Y+%02d=%02x ", i, avr.data[y+i]);\
    //				printf("\n");\
    //		}


    //#if AVR_STACK_WATCH
    //#define DUMP_STACK() \
    //		for (int i = avr.trace_data->stack_frame_index; i; i--) {\
    //			int pci = i-1;\
    //			printf(FONT_RED "*** %04x: %-25s sp %04x\n" FONT_DEFAULT,\
    //					avr.trace_data->stack_frame[pci].pc, \
    //					avr.trace_data->codeline ? avr.trace_data->codeline[avr.trace_data->stack_frame[pci].pc>>1]->symbol : "unknown", \
    //							avr.trace_data->stack_frame[pci].sp);\
    //		}
    //#else
    //#define DUMP_STACK()
    //#endif

    //#else /* CONFIG_SIMAVR_TRACE */

    //#define DUMP_STACK()
    //#define DUMP_REG();

    //#endif

    ///**
    // * Reconstructs the SREG value from avr.sreg into dst.
    // */
    //#define READ_SREG_INTO(avr, dst) { \
    //dst = 0; \
    //			for (int i = 0; i< 8; i++) \
    //				if (avr.sreg[i] > 1) { \
    //					printf("** Invalid SREG!!\n"); \
    //				} else if (avr.sreg[i]) \
    //					dst |= (1 << i); \
    //		}

    public void avr_sreg_set(Avr avr, byte flag, bool ival)
        {
        /*
        *	clear interrupt_state if disabling interrupts.
        *	set wait if enabling interrupts.
        *	no change if interrupt flag does not change.
        */

            if (flag == S_I)
            {
                if (ival)
                {
                    if (avr.sreg[S_I]!=0)
                        avr.interrupt_state = -2;
                }
                else
                    avr.interrupt_state = 0;
            }

            avr.sreg[flag] =(byte)(ival?1:0);
        }

        ///**
        // * Splits the SREG value from src into the avr.sreg array.
        // */
        public void SET_SREG_FROM(Avr avr, byte src)
        { 
        			for (byte i = 0; i< 8; i++) 
        				avr_sreg_set(avr, i, (src & (1 << i)) != 0); 
        }

        //#ifdef __cplusplus
        //};
        //#endif

        //#endif /*__SIM_CORE_H__*/






        //// SREG bit names
        public static string _sreg_bit_name = "cznvshti";

        ///*
        // * Handle "touching" registers, marking them changed.
        // * This is used only for debugging purposes to be able to
        // * print the effects of each instructions on registers
        // */
        //#if CONFIG_SIMAVR_TRACE

        //#define T(w) w

        //#define REG_TOUCH(a, r) (a)->trace_data->touched[(r) >> 5] |= (1 << ((r) & 0x1f))
        //#define REG_ISTOUCHED(a, r) ((a)->trace_data->touched[(r) >> 5] & (1 << ((r) & 0x1f)))

        ///*
        // * This allows a "special case" to skip instruction tracing when in these
        // * symbols since printf() is useful to have, but generates a lot of cycles.
        // */
        //int dont_trace(const char * name)
        //{
        //	return (
        //		!strcmp(name, "uart_putchar") ||
        //		!strcmp(name, "fputc") ||
        //		!strcmp(name, "printf") ||
        //		!strcmp(name, "vfprintf") ||
        //		!strcmp(name, "__ultoa_invert") ||
        //		!strcmp(name, "__prologue_saves__") ||
        //		!strcmp(name, "__epilogue_restores__"));
        //}

        //int donttrace = 0;

        public static void STATE(Avr avr,params object[] param)
        { 
        	if (avr.trace!=0)
            {
//        		if (avr.trace_data.codeline && avr.trace_data.codeline[avr.PC>>1])
//                {
//        			const char * symn = avr.trace_data->codeline[avr.pc>>1]->symbol; \
//        			int dont = 0 && dont_trace(symn);\
//        			if (dont!=donttrace) { \
//        				donttrace = dont;\
//        				DUMP_REG();\
//        			}\
//        			if (donttrace==0)\
//        				printf("%04x: %-25s " _f, avr.pc, symn, ## args);\
//        		} else \
//        			printf("%s: %04x: " _f, __FUNCTION__, avr.pc, ## args);\
        	}
        }


        public static void SREG(Avr avr)
        {
            if (avr.trace!=0/* && donttrace == 0*/)
                Console.WriteLine("%04x: \t\t\t\t\t\t\t\t\tSREG = ", avr.PC); 
        	for (int _sbi = 0; _sbi < 8; _sbi++)
        		Console.WriteLine(avr.sreg[_sbi]!=0 ?_sreg_bit_name[_sbi].ToString().ToUpper() : ".");
        	Console.WriteLine("\n");
        }

        public static void Crash(Avr avr)
        {
            //	DUMP_REG();
            Console.WriteLine("*** CYCLE: %04x PC: %04x\n", avr.cycle, avr.PC);
            

//        	for (int i = OLD_PC_SIZE-1; i > 0; i--) {
//        		int pci = (avr.trace_data.old_pci + i) & 0xf;
//                Console.WriteLine("*** %04x: %-25s RESET -%d; sp %04x\n",
//        				avr.trace_data.old[pci].pc, avr.trace_data.codeline ? 
//                          avr.trace_data.codeline[avr.trace_data.old[pci].pc>>1]->symbol : 
//                          "unknown", OLD_PC_SIZE-i, avr.trace_data.old[pci].sp);
        }

        //	printf("Stack Ptr %04x/%04x = %d \n", _avr_sp_get(avr), avr.ramend, avr.ramend - _avr_sp_get(avr));
        //	DUMP_STACK();

        //	avr_sadly_crashed(avr, 0);
        //}
        //#else
        //#define T(w)
        //#define REG_TOUCH(a, r)
        //#define STATE(_f, args...)
        //#define SREG()

        //void crash(avr_t* avr)
        //{
        //	avr_sadly_crashed(avr, 0);

        //}
        //#endif

        protected static ushort _avr_flash_read16le(Avr avr,uint addr)
        {
        	return (ushort) (avr.flash[addr] | (avr.flash[addr + 1] << 8));
        }

        public static void Avr_core_watch_write(Avr avr, uint addr, byte v)
        {
        //	if (addr > avr.ramend) {
        //		AVR_LOG(avr, LOG_ERROR, FONT_RED
        //				"CORE: *** Invalid write address "
        //				"PC=%04x SP=%04x O=%04x Address %04x=%02x out of ram\n"
        //				FONT_DEFAULT,
        //				avr.pc, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc), addr, v);
        //		crash(avr);
        //	}
        //	if (addr < 32) {
        //		AVR_LOG(avr, LOG_ERROR, FONT_RED
        //				"CORE: *** Invalid write address PC=%04x SP=%04x O=%04x Address %04x=%02x low registers\n"
        //				FONT_DEFAULT,
        //				avr.pc, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc), addr, v);
        //		crash(avr);
        //	}
        //#if AVR_STACK_WATCH
        //	/*
        //	 * this checks that the current "function" is not doctoring the stack frame that is located
        //	 * higher on the stack than it should be. It's a sign of code that has overrun it's stack
        //	 * frame and is munching on it's own return address.
        //	 */
        //	if (avr.trace_data->stack_frame_index > 1 && addr > avr.trace_data->stack_frame[avr.trace_data->stack_frame_index-2].sp) {
        //		printf( FONT_RED "%04x : munching stack "
        //				"SP %04x, A=%04x <= %02x\n" FONT_DEFAULT,
        //				avr.pc, _avr_sp_get(avr), addr, v);
        //	}
        //#endif

        //	if (avr.gdb) {
        //		avr_gdb_handle_watchpoints(avr, addr, AVR_GDB_WATCH_WRITE);
        //	}

        //	avr.data[addr] = v;
        }

        public static byte Avr_core_watch_read(Avr avr, uint addr)
        {
        	if (addr > avr.ramend)
            {
//        		AVR_LOG(avr, LOG_ERROR, FONT_RED
//        				"CORE: *** Invalid read address "
//        				"PC=%04x SP=%04x O=%04x Address %04x out of ram (%04x)\n"
//        				FONT_DEFAULT,
//        				avr.pc, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc), addr, avr.ramend);
//        		crash(avr);
        	}

//        	if (avr.gdb)
//            {
//        		avr_gdb_handle_watchpoints(avr, addr, AVR_GDB_WATCH_READ);
//        	}

        	return avr.data[addr];
        }

        ///*
        // * Set a register (r < 256)
        // * if it's an IO register (> 31) also (try to) call any callback that was
        // * registered to track changes to that register.
        // */
        public static void _avr_set_r(Avr avr, int r, int v)
        {
        //	REG_TOUCH(avr, r);

        	if (r == R_SREG) {
        		avr.data[R_SREG] = (byte) v;
        		// //unsplit the SREG
        		//SET_SREG_FROM(avr, v);
        		//SREG();
        	} 
            else
        	if (r > 31) {
        //		avr_io_addr_t io = AVR_DATA_TO_IO(r);
        //		if (avr.io[io].w.c)
        //			avr.io[io].w.c(avr, r, v, avr.io[io].w.param);
        //		else
        //			avr.data[r] = v;
        //		if (avr.io[io].irq) {
        //			avr_raise_irq(avr.io[io].irq + AVR_IOMEM_IRQ_ALL, v);
        //			for (int i = 0; i < 8; i++)
        //				avr_raise_irq(avr.io[io].irq + i, (v >> i) & 1);
        //		}
        	} 
            else
        		avr.data[r] = (byte)v;
        }

        public static void _avr_set_r16le(Avr avr, int r,int v)
        {
        	_avr_set_r(avr, r, v);
        	_avr_set_r(avr, r + 1, (v >> 8));
        }

        //static inline void
        //_avr_set_r16le_hl(
        //	avr_t * avr,
        //	uint16_t r,
        //	uint16_t v)
        //{
        //	_avr_set_r(avr, r + 1, v >> 8);
        //	_avr_set_r(avr, r , v);
        //}

        ///*
        // * Stack pointer access
        // */
        //inline uint16_t _avr_sp_get(avr_t * avr)
        //{
        //	return avr.data[R_SPL] | (avr.data[R_SPH] << 8);
        //}

        public static void _avr_sp_set(Avr avr, int sp)
        {
            _avr_set_r16le(avr, R_SPL, sp);
        }

        ///*
        // * Set any address to a value; split between registers and SRAM
        // */
        //static inline void _avr_set_ram(avr_t * avr, uint16_t addr, byte v)
        //{
        //	if (addr < MAX_IOs + 31)
        //		_avr_set_r(avr, addr, v);
        //	else
        //		avr_core_watch_write(avr, addr, v);
        //}

        ///*
        // * Get a value from SRAM.
        // */
        //static inline byte _avr_get_ram(avr_t * avr, uint16_t addr)
        //{
        //	if (addr == R_SREG) {
        //		/*
        //		 * SREG is special it's reconstructed when read
        //		 * while the core itself uses the "shortcut" array
        //		 */
        //		READ_SREG_INTO(avr, avr.data[R_SREG]);

        //	} else if (addr > 31 && addr < 31 + MAX_IOs) {
        //		avr_io_addr_t io = AVR_DATA_TO_IO(addr);

        //		if (avr.io[io].r.c)
        //			avr.data[addr] = avr.io[io].r.c(avr, addr, avr.io[io].r.param);

        //		if (avr.io[io].irq) {
        //			byte v = avr.data[addr];
        //			avr_raise_irq(avr.io[io].irq + AVR_IOMEM_IRQ_ALL, v);
        //			for (int i = 0; i < 8; i++)
        //				avr_raise_irq(avr.io[io].irq + i, (v >> i) & 1);
        //		}
        //	}
        //	return avr_core_watch_read(avr, addr);
        //}

        ///*
        // * Stack push accessors.
        // */
        //static inline void _avr_push8(avr_t * avr, uint16_t v)
        //{
        //	uint16_t sp = _avr_sp_get(avr);
        //	_avr_set_ram(avr, sp, v);
        //	_avr_sp_set(avr, sp-1);
        //}

        //static inline byte _avr_pop8(avr_t * avr)
        //{
        //	uint16_t sp = _avr_sp_get(avr) + 1;
        //	byte res = _avr_get_ram(avr, sp);
        //	_avr_sp_set(avr, sp);
        //	return res;
        //}

        //int _avr_push_addr(avr_t * avr, avr_flashaddr_t addr)
        //{
        //	uint16_t sp = _avr_sp_get(avr);
        //	addr >>= 1;
        //	for (int i = 0; i < avr.address_size; i++, addr >>= 8, sp--) {
        //		_avr_set_ram(avr, sp, addr);
        //	}
        //	_avr_sp_set(avr, sp);
        //	return avr.address_size;
        //}

        //avr_flashaddr_t _avr_pop_addr(avr_t * avr)
        //{
        //	uint16_t sp = _avr_sp_get(avr) + 1;
        //	avr_flashaddr_t res = 0;
        //	for (int i = 0; i < avr.address_size; i++, sp++) {
        //		res = (res << 8) | _avr_get_ram(avr, sp);
        //	}
        //	res <<= 1;
        //	_avr_sp_set(avr, sp -1);
        //	return res;
        //}

        ///*
        // * "Pretty" register names
        // */
        public static string[] reg_names = new string[255];
        

        public static string Avr_regname(byte reg)
        {
        	if (reg_names[reg]=="")
            {
        		string tt;
        		if (reg < 32)
        			tt="r"+reg.ToString();
        		else
        			tt="io:02x"+reg.ToString();
        		reg_names[reg] = tt;
        	}
        	return reg_names[reg];
        }

        ///*
        // * Called when an invalid opcode is decoded
        // */
        protected static void _avr_invalid_opcode(Avr avr)
        {
        //#if CONFIG_SIMAVR_TRACE
        //	printf( FONT_RED "*** %04x: %-25s Invalid Opcode SP=%04x O=%04x \n" FONT_DEFAULT,
        //			avr.pc, avr.trace_data->codeline[avr.pc>>1]->symbol, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc));
        //#else
        //	AVR_LOG(avr, LOG_ERROR, FONT_RED "CORE: *** %04x: Invalid Opcode SP=%04x O=%04x \n" FONT_DEFAULT,
        //			avr.pc, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc));
        //#endif
        }

        //#if CONFIG_SIMAVR_TRACE
        ///*
        // * Dump changed registers when tracing
        // */
        //void avr_dump_state(avr_t * avr)
        //{
        //	if (!avr.trace || donttrace)
        //		return;

        //	int doit = 0;

        //	for (int r = 0; r < 3 && !doit; r++)
        //		if (avr.trace_data->touched[r])
        //			doit = 1;
        //	if (!doit)
        //		return;
        //	printf("                                       ->> ");
        //	const int r16[] = { R_SPL, R_XL, R_YL, R_ZL };
        //	for (int i = 0; i < 4; i++)
        //		if (REG_ISTOUCHED(avr, r16[i]) || REG_ISTOUCHED(avr, r16[i]+1)) {
        //			REG_TOUCH(avr, r16[i]);
        //			REG_TOUCH(avr, r16[i]+1);
        //		}

        //	for (int i = 0; i < 3*32; i++)
        //		if (REG_ISTOUCHED(avr, i)) {
        //			printf("%s=%02x ", avr_regname(i), avr.data[i]);
        //		}
        //	printf("\n");
        //}
        //#endif

        //#define get_io5_b3(o) \
        //        const byte io = ((o >> 3) & 0x1f) + 32;
       // 		const byte b = o & 0x7;

        ////	const int16_t o = ((int16_t)(op << 4)) >> 3; // CLANG BUG!

        ///*
        // * Add a "jump" address to the jump trace buffer
        // */
        //#if CONFIG_SIMAVR_TRACE
        //#define TRACE_JUMP()\
        //	avr.trace_data->old[avr.trace_data->old_pci].pc = avr.pc;\
        //	avr.trace_data->old[avr.trace_data->old_pci].sp = _avr_sp_get(avr);\
        //	avr.trace_data->old_pci = (avr.trace_data->old_pci + 1) & (OLD_PC_SIZE-1);\

        //#if AVR_STACK_WATCH
        //#define STACK_FRAME_PUSH()\
        //	avr.trace_data->stack_frame[avr.trace_data->stack_frame_index].pc = avr.pc;\
        //	avr.trace_data->stack_frame[avr.trace_data->stack_frame_index].sp = _avr_sp_get(avr);\
        //	avr.trace_data->stack_frame_index++;
        //#define STACK_FRAME_POP()\
        //	if (avr.trace_data->stack_frame_index > 0) \
        //		avr.trace_data->stack_frame_index--;
        //#else
        //#define STACK_FRAME_PUSH()
        //#define STACK_FRAME_POP()
        //#endif
        //#else /* CONFIG_SIMAVR_TRACE */

        //#define TRACE_JUMP()
        //#define STACK_FRAME_PUSH()
        //#define STACK_FRAME_POP()

        //#endif

        /****************************************************************************\
         *
         * Helper functions for calculating the status register bit values.
         * See the Atmel data sheet for the instruction set for more info.
         *
        \****************************************************************************/

        public static  void _avr_flags_zns (Avr avr, byte res)
        {
        	avr.sreg[S_Z] = (byte)(res == 0?1:0);
        	avr.sreg[S_N] = (byte)((res >> 7) & 1);
        	avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static  void _avr_flags_zns16 (Avr avr, ushort res)
        {
        	avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_N] = (byte)((res >> 15) & 1);
        	avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static  void _avr_flags_add_zns (Avr avr, byte res, byte rd, byte rr)
        {
        	/* carry & half carry */
        	byte add_carry = (byte)((rd & rr) | (rr & ~res) | (~res & rd));
        	avr.sreg[S_H] = (byte)((add_carry >> 3) & 1);
        	avr.sreg[S_C] = (byte)((add_carry >> 7) & 1);

        	/* overflow */
        	avr.sreg[S_V] = (byte)((((rd & rr & ~res) | (~rd & ~rr & res)) >> 7) & 1);

        	/* zns */
        	_avr_flags_zns(avr, res);
        }


        public static  void _avr_flags_sub_zns(Avr avr, byte res, byte rd, byte rr)
        {
        	/* carry & half carry */
        	byte sub_carry = (byte)((~rd & rr) | (rr & res) | (res & ~rd));
        	avr.sreg[S_H] = (byte)((sub_carry >> 3) & 1);
        	avr.sreg[S_C] = (byte)((sub_carry >> 7) & 1);

        	/* overflow */
        	avr.sreg[S_V] = (byte)((((rd & ~rr & ~res) | (~rd & rr & res)) >> 7) & 1);

        	/* zns */
        	_avr_flags_zns(avr, res);
        }

        public static void _avr_flags_Rzns (Avr avr, byte res)
        {
        	if (res!=0)
        		avr.sreg[S_Z] = 0;
        	avr.sreg[S_N] = (byte)((res >> 7) & 1);
        	avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static  void _avr_flags_sub_Rzns (Avr avr, byte res, byte rd, byte rr)
        {
        	/* carry & half carry */
        	byte sub_carry = (byte)((~rd & rr) | (rr & res) | (res & ~rd));
        	avr.sreg[S_H] = (byte)((sub_carry >> 3) & 1);
        	avr.sreg[S_C] = (byte)((sub_carry >> 7) & 1);
        	/* overflow */
        	avr.sreg[S_V] = (byte)((((rd & ~rr & ~res) | (~rd & rr & res)) >> 7) & 1);
        	_avr_flags_Rzns(avr, res);
        }

        public static void _avr_flags_zcvs (Avr avr, byte res, byte vr)
        {
        	avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_C] = (byte)(vr & 1);
        	avr.sreg[S_V] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_C]);
        	avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static  void _avr_flags_zcnvs (Avr avr, byte res, byte vr)
        {
        	avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_C] = (byte)(vr & 1);
        	avr.sreg[S_N] = (byte)(res >> 7);
        	avr.sreg[S_V] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_C]);
        	avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static  void _avr_flags_znv0s (Avr avr, byte res)
        {
        	avr.sreg[S_V] = 0;
        	_avr_flags_zns(avr, res);
        }

        public static bool _avr_is_instruction_32_bits(Avr avr, uint pc)
        {
        	ushort o = (ushort)(_avr_flash_read16le(avr, pc) & 0xfc0f);
            return (o == 0x9200 || // STS ! Store Direct to Data Space
                    o == 0x9000 || // LDS Load Direct from Data Space
                    o == 0x940c || // JMP Long Jump
                    o == 0x940d || // JMP Long Jump
                    o == 0x940e ||  // CALL Long Call to sub
                    o == 0x940f); // CALL Long Call to sub
        }

        ///*
        // * Main opcode decoder
        // *
        // * The decoder was written by following the datasheet in no particular order.
        // * As I went along, I noticed "bit patterns" that could be used to factor opcodes
        // * However, a lot of these only became apparent later on, so SOME instructions
        // * (skip of bit set etc) are compact, and some could use some refactoring (the ALU
        // * ones scream to be factored).
        // * I assume that the decoder could easily be 2/3 of it's current size.
        // *
        // * + It lacks the "extended" XMega jumps.
        // * + It also doesn't check whether the core it's
        // *   emulating is supposed to have the fancy instructions, like multiply and such.
        // *
        // * The number of cycles taken by instruction has been added, but might not be
        // * entirely accurate.
        // */
        public static uint Avr_run_one(Avr avr)
        {
            //run_one_again:

            // Ensure we don't crash simavr due to a bad instruction reading past the end of the flash.
            if(avr.PC >= avr.flashend) 
            {
                STATE(avr,"CRASH\n");
                Crash(avr);
                return 0;
            }

            uint opcode = _avr_flash_read16le(avr, avr.PC);
            uint new_pc = avr.PC + 2;	// future "default" pc
            ulong cycle = 1;

        	switch (opcode & 0xf000)
            {
        		case 0x0000:
                {
        			switch (opcode)
                    {
        				case 0x0000: {    // NOP
                            STATE(avr,"nop\n");
        				}	break;
        				default: {
        					switch (opcode & 0xfc00)
                            {
        						case 0x0400: {  // CPC -- Compare with carry -- 0000 01rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res =(byte)(vd - vr - avr.sreg[S_C]);
        							STATE(avr,"cpc %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
        							_avr_flags_sub_Rzns(avr, res, vd, vr);
        							SREG(avr);
        						}	break;
        						case 0x0c00: {  // ADD -- Add without carry -- 0000 11rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd + vr);
        							if (r == d) {
        								STATE(avr,"lsl %s[%02x] = %02x\n", Avr_regname(d), vd, res & 0xff);
        							} else {
        								STATE(avr,"add %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
        							}
        							_avr_set_r(avr, d, res);
        							_avr_flags_add_zns(avr, res, vd, vr);
        							SREG(avr);
        						}	break;
        						case 0x0800: {  // SBC -- Subtract with carry -- 0000 10rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd - vr - avr.sreg[S_C]);
        							STATE(avr,"sbc %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), avr.data[d], Avr_regname(r), avr.data[r], res);
        							_avr_set_r(avr, d, res);
        							_avr_flags_sub_Rzns(avr, res, vd, vr);
        							SREG(avr);
        						}	break;
        						default: switch (opcode & 0xff00)
                                {
                                    case 0x0100: {  // MOVW -- Copy Register Word -- 0000 0001 dddd rrrr
                                        byte d = (byte)(((opcode >> 4) & 0xf) << 1);
                                        byte r = (byte)(((opcode) & 0xf) << 1);
                                        STATE(avr,"movw %s:%s, %s:%s[%02x%02x]\n", Avr_regname(d), Avr_regname((byte)(d + 1)), Avr_regname(r), Avr_regname((byte)(r + 1)), avr.data[r + 1], avr.data[r]);
                                        ushort vr = (ushort)(avr.data[r] | (avr.data[r + 1] << 8));
                                        _avr_set_r16le(avr, d, vr);
                                        }; break;
                                    case 0x0200: {  // MULS -- Multiply Signed -- 0000 0010 dddd rrrr
                                        byte r = (byte)(16 + (opcode & 0xf));
                                        byte d = (byte)(16 + ((opcode >> 4) & 0xf));
                                        ushort res = (ushort)(((byte)avr.data[r]) * ((byte)avr.data[d]));
                                        STATE(avr,"muls %s[%d], %s[%02x] = %d\n", Avr_regname(d), (byte)avr.data[d], Avr_regname(r), (byte)avr.data[r], res);
                                        _avr_set_r16le(avr, 0, res);
                                        avr.sreg[S_C] = (byte)((res >> 15) & 1);
                                        avr.sreg[S_Z] = (byte)(res == 0?1:0);
                                        cycle++;
                                        SREG(avr);
                                        }; break;
                                    case 0x0300: {  // MUL -- Multiply -- 0000 0011 fddd frrr
                                        byte r = (byte)(16 + (opcode & 0x7));
                                        byte d = (byte)(16 + ((opcode >> 4) & 0x7));
                                        ushort res = 0;
                                        byte c = 0;
                                        string name = "";
        								switch (opcode & 0x88)
                                        {
                                            case 0x00:  // MULSU -- Multiply Signed Unsigned -- 0000 0011 0ddd 0rrr
                                                res = (ushort)(((byte)avr.data[r]) * ((byte)avr.data[d]));
                                                c = (byte)((res >> 15) & 1);
                                                name = "mulsu"; break;
                                            case 0x08:  // FMUL -- Fractional Multiply Unsigned -- 0000 0011 0ddd 1rrr
                                                res = (ushort)(((byte)avr.data[r]) * ((byte)avr.data[d]));
                                                c = (byte)((res >> 15) & 1);
                                                res <<= 1;
                                                name = "fmul"; break;
                                            case 0x80:  // FMULS -- Multiply Signed -- 0000 0011 1ddd 0rrr
                                                res = (ushort)(((byte)avr.data[r]) * ((byte)avr.data[d]));
                                                c = (byte)((res >> 15) & 1);
                                                res <<= 1;
                                                name = "fmuls"; break;
                                            case 0x88:  // FMULSU -- Multiply Signed Unsigned -- 0000 0011 1ddd 1rrr
                                                res = (ushort)(((byte)avr.data[r]) * ((byte)avr.data[d]));
                                                c = (byte)((res >> 15) & 1);
                                                res <<= 1;
                                                name = "fmulsu";break;
                                        }
                                        cycle++;
                                        STATE(avr,"%s %s[%d], %s[%02x] = %d\n", name, Avr_regname(d), (byte)avr.data[d], Avr_regname(r), (byte)avr.data[r], res);
                                        _avr_set_r16le(avr, 0, res);
                                        avr.sreg[S_C] = c;
                                        avr.sreg[S_Z] = (byte)(res == 0?1:0);
                                        SREG(avr);
                                        } break;
                                    default: _avr_invalid_opcode(avr); break;
                                }; break;
        					}
        				} break;
        			} 
        		}	break;

        		case 0x1000:
                {
                    switch (opcode & 0xfc00)
                    {
                        case 0x1800: {	// SUB -- Subtract without carry -- 0001 10rd dddd rrrr
                        	byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf)); 
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                            byte res = (byte)(vd - vr);
                        	STATE(avr,"sub %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
                        	_avr_set_r(avr, d, res);
                        	_avr_flags_sub_zns(avr, res, vd, vr);
                        	SREG(avr);
                        };	break;
                        case 0x1000: {   // CPSE -- Compare, skip if equal -- 0001 00rd dddd rrrr
                            byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                            ushort res = (ushort)(vd == vr?1:0);
                            STATE(avr,"cpse %s[%02x], %s[%02x]\t; Will%s skip\n", Avr_regname(d), avr.data[d], Avr_regname(r), avr.data[r], res!=0 ? "" : " not");
                            if (res!=0)
                            {
                                if (_avr_is_instruction_32_bits(avr, new_pc))
                                {
                                    new_pc += 4;
                                    cycle += 2;
                                }
                                else
                                {
                                    new_pc += 2;
                                    cycle++;
                                }
                            }
                        };	break;
                        case 0x1400: {	// CP -- Compare -- 0001 01rd dddd rrrr
                        	byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf)); 
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                        	byte res = (byte)(vd - vr);
                        	STATE(avr,"cp %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
                        	_avr_flags_sub_zns(avr, res, vd, vr);
                        	SREG(avr);
                        };	break;
                        case 0x1c00: {	// ADD -- Add with carry -- 0001 11rd dddd rrrr
                        	byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf)); 
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                        	byte res = (byte)(vd + vr + avr.sreg[S_C]);
                        	if (r == d)
                            	STATE(avr,"rol %s[%02x] = %02x\n", Avr_regname(d), avr.data[d], res);
                        	else
                            	STATE(avr,"addc %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), avr.data[d], Avr_regname(r), avr.data[r], res);
                        	_avr_set_r(avr, d, res);
                        	_avr_flags_add_zns(avr, res, vd, vr);
                        	SREG(avr);
                        };	break;
                        default: _avr_invalid_opcode(avr);break;
                    }
                }	break;

        		case 0x2000:
                {
                    switch (opcode & 0xfc00)
                    {
                        case 0x2000: {	// AND -- Logical AND -- 0010 00rd dddd rrrr
                            byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf)); 
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                            byte res = (byte)(vd & vr);
                            if (r == d) 
                                STATE(avr,"tst %s[%02x]\n", Avr_regname(d), avr.data[d]);
                            else 
                        		STATE(avr,"and %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
                        	_avr_set_r(avr, d, res);
                        	_avr_flags_znv0s(avr, res);
                        	SREG(avr);
                        }	break;
                        case 0x2400: {	// EOR -- Logical Exclusive OR -- 0010 01rd dddd rrrr
                            byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf)); 
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                            byte res = (byte)(vd ^ vr);
                            if (r==d) 
                            	STATE(avr,"clr %s[%02x]\n", Avr_regname(d), avr.data[d]);
                            else 
                        		STATE(avr,"eor %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
                            _avr_set_r(avr, d, res);
                            _avr_flags_znv0s(avr, res);
                        	SREG(avr);
                        }	break;
                        case 0x2800: {   // OR -- Logical OR -- 0010 10rd dddd rrrr
                            byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                            byte d = (byte)((opcode >> 4) & 0x1f);
                            byte vd = avr.data[d];
                            byte vr = avr.data[r];
                            byte res = (byte)(vd | vr);
                            STATE(avr,"or %s[%02x], %s[%02x] = %02x\n", Avr_regname(d), vd, Avr_regname(r), vr, res);
                            _avr_set_r(avr, d, res);
                            _avr_flags_znv0s(avr, res);
                            SREG(avr);
                        }	break;
                        case 0x2c00: {	// MOV -- 0010 11rd dddd rrrr
                        	byte d = (byte)((opcode >> 4) & 0x1f); 
                            byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                            byte vr = avr.data[r];
                       		byte res = vr;
                        	STATE(avr,"mov %s, %s[%02x] = %02x\n", Avr_regname(d), Avr_regname(r), vr, res);
                        	_avr_set_r(avr, d, res);
                        }	break;
                        default: _avr_invalid_opcode(avr); break;
                    }
                }	break;

        		case 0x3000: {  // CPI -- Compare Immediate -- 0011 kkkk hhhh kkkk
                    byte h = (byte)(16 + ((opcode >> 4) & 0xf)); 
        		    byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                    byte vh = avr.data[h];
                    byte res = (byte)(vh - k);
                    STATE(avr,"cpi %s[%02x], 0x%02x\n", Avr_regname(h), vh, k);
                    _avr_flags_sub_zns(avr, res, vh, k);
                    SREG(avr);
                }   break;

        		case 0x4000: {  // SBCI -- Subtract Immediate With Carry -- 0100 kkkk hhhh kkkk
                    byte h = (byte)(16 + ((opcode >> 4) & 0xf)); 
        		    byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                    byte vh = avr.data[h];
                    byte res = (byte)(vh - k - avr.sreg[S_C]);
                    STATE(avr,"sbci %s[%02x], 0x%02x = %02x\n", Avr_regname(h), vh, k, res);
                    _avr_set_r(avr, h, res);
                    _avr_flags_sub_Rzns(avr, res, vh, k);
                    SREG(avr);
                }   break;

        		case 0x5000: {  // SUBI -- Subtract Immediate -- 0101 kkkk hhhh kkkk
                    byte h = (byte)(16 + ((opcode >> 4) & 0xf)); 
        		    byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                    byte vh = avr.data[h];
                    byte res = (byte)(vh - k);
                    STATE(avr,"subi %s[%02x], 0x%02x = %02x\n", Avr_regname(h), vh, k, res);
                    _avr_set_r(avr, h, res);
                    _avr_flags_sub_zns(avr, res, vh, k);
                    SREG(avr);
                }   break;

        		case 0x6000: {  // ORI aka SBR -- Logical OR with Immediate -- 0110 kkkk hhhh kkkk
                    byte h = (byte)(16 + ((opcode >> 4) & 0xf)); 
        		    byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                    byte vh = avr.data[h];

                    byte res = (byte)(vh | k);
                    STATE(avr,"ori %s[%02x], 0x%02x\n", Avr_regname(h), vh, k);
                    _avr_set_r(avr, h, res);
                    _avr_flags_znv0s(avr, res);
                    SREG(avr);
                }   break;

        		case 0x7000: {  // ANDI	-- Logical AND with Immediate -- 0111 kkkk hhhh kkkk
                        //const byte h = 16 + ((o >> 4) & 0xf); \
        		//const byte k = ((o & 0x0f00) >> 4) | (o & 0xf);
                   //     const byte vh = avr.data[h];

                        //			byte res = vh & k;
                        //			STATE("andi %s[%02x], 0x%02x\n", avr_regname(h), vh, k);
                        //			_avr_set_r(avr, h, res);
                        //			_avr_flags_znv0s(avr, res);
                        //			SREG();
                    }
                    break;

        		case 0xa000:
        		case 0x8000: {
                        //			/*
                        //			 * Load (LDD/STD) store instructions
                        //			 *
                        //			 * 10q0 qqsd dddd yqqq
                        //			 * s = 0 = load, 1 = store
                        //			 * y = 16 bits register index, 1 = Y, 0 = X
                        //			 * q = 6 bit displacement
                        //			 */
                        //			switch (opcode & 0xd008) {
                        //				case 0xa000:
                        //				case 0x8000: {	// LD (LDD) -- Load Indirect using Z -- 10q0 qqsd dddd yqqq
                        //					uint16_t v = avr.data[R_ZL] | (avr.data[R_ZH] << 8);
                //        const byte d = (opcode >> 4) & 0x1f; \
        		//const byte q = ((o & 0x2000) >> 8) | ((o & 0x0c00) >> 7) | (o & 0x7);

                        //					if (opcode & 0x0200) {
                        //						STATE("st (Z+%d[%04x]), %s[%02x]\n", q, v+q, avr_regname(d), avr.data[d]);
                        //						_avr_set_ram(avr, v+q, avr.data[d]);
                        //					} else {
                        //						STATE("ld %s, (Z+%d[%04x])=[%02x]\n", avr_regname(d), q, v+q, avr.data[v+q]);
                        //						_avr_set_r(avr, d, _avr_get_ram(avr, v+q));
                        //					}
                        //					cycle += 1; // 2 cycles, 3 for tinyavr
                        //				}	break;
                        //				case 0xa008:
                        //				case 0x8008: {	// LD (LDD) -- Load Indirect using Y -- 10q0 qqsd dddd yqqq
                        //					uint16_t v = avr.data[R_YL] | (avr.data[R_YH] << 8);
                  //      const byte d = (opcode >> 4) & 0x1f; \
        		//const byte q = ((o & 0x2000) >> 8) | ((o & 0x0c00) >> 7) | (o & 0x7);

                        //					if (opcode & 0x0200) {
                        //						STATE("st (Y+%d[%04x]), %s[%02x]\n", q, v+q, avr_regname(d), avr.data[d]);
                        //						_avr_set_ram(avr, v+q, avr.data[d]);
                        //					} else {
                        //						STATE("ld %s, (Y+%d[%04x])=[%02x]\n", avr_regname(d), q, v+q, avr.data[d+q]);
                        //						_avr_set_r(avr, d, _avr_get_ram(avr, v+q));
                        //					}
                        //					cycle += 1; // 2 cycles, 3 for tinyavr
                        //				}	break;
                        //				default: _avr_invalid_opcode(avr);
                        //			}
                    }
                    break;

        		case 0x9000: {
                        //			/* this is an annoying special case, but at least these lines handle all the SREG set/clear opcodes */
                        //			if ((opcode & 0xff0f) == 0x9408) {
                  //      const byte b = (o >> 4) & 7;
                        //				STATE("%s%c\n", opcode & 0x0080 ? "cl" : "se", _sreg_bit_name[b]);
                        //				avr_sreg_set(avr, b, (opcode & 0x0080) == 0);
                        //				SREG();
                        //			} else switch (opcode) {
                        //				case 0x9588: { // SLEEP -- 1001 0101 1000 1000
                        //					STATE("sleep\n");
                        //					/* Don't sleep if there are interrupts about to be serviced.
                        //					 * Without this check, it was possible to incorrectly enter a state
                        //					 * in which the cpu was sleeping and interrupts were disabled. For more
                        //					 * details, see the commit message. */
                        //					if (!avr_has_pending_interrupts(avr) || !avr.sreg[S_I])
                        //						avr.state = cpu_Sleeping;
                        //				}	break;
                        //				case 0x9598: { // BREAK -- 1001 0101 1001 1000
                        //					STATE("break\n");
                        //					if (avr.gdb) {
                        //						// if gdb is on, we break here as in here
                        //						// and we do so until gdb restores the instruction
                        //						// that was here before
                        //						avr.state = cpu_StepDone;
                        //						new_pc = avr.pc;
                        //						cycle = 0;
                        //					}
                        //				}	break;
                        //				case 0x95a8: { // WDR -- Watchdog Reset -- 1001 0101 1010 1000
                        //					STATE("wdr\n");
                        //					avr_ioctl(avr, AVR_IOCTL_WATCHDOG_RESET, 0);
                        //				}	break;
                        //				case 0x95e8: { // SPM -- Store Program Memory -- 1001 0101 1110 1000
                        //					STATE("spm\n");
                        //					avr_ioctl(avr, AVR_IOCTL_FLASH_SPM, 0);
                        //				}	break;
                        //				case 0x9409:   // IJMP -- Indirect jump -- 1001 0100 0000 1001
                        //				case 0x9419:   // EIJMP -- Indirect jump -- 1001 0100 0001 1001   bit 4 is "indirect"
                        //				case 0x9509:   // ICALL -- Indirect Call to Subroutine -- 1001 0101 0000 1001
                        //				case 0x9519: { // EICALL -- Indirect Call to Subroutine -- 1001 0101 0001 1001   bit 8 is "push pc"
                        //					int e = opcode & 0x10;
                        //					int p = opcode & 0x100;
                        //					if (e && !avr.eind)
                        //						_avr_invalid_opcode(avr);
                        //					uint32_t z = avr.data[R_ZL] | (avr.data[R_ZH] << 8);
                        //					if (e)
                        //						z |= avr.data[avr.eind] << 16;
                        //					STATE("%si%s Z[%04x]\n", e?"e":"", p?"call":"jmp", z << 1);
                        //					if (p)
                        //						cycle += _avr_push_addr(avr, new_pc) - 1;
                        //					new_pc = z << 1;
                        //					cycle++;
                        //					TRACE_JUMP();
                        //				}	break;
                        //				case 0x9518: 	// RETI -- Return from Interrupt -- 1001 0101 0001 1000
                        //					avr_sreg_set(avr, S_I, 1);
                        //					avr_interrupt_reti(avr);
                        //					FALLTHROUGH
                        //				case 0x9508: {	// RET -- Return -- 1001 0101 0000 1000
                        //					new_pc = _avr_pop_addr(avr);
                        //					cycle += 1 + avr.address_size;
                        //					STATE("ret%s\n", opcode & 0x10 ? "i" : "");
                        //					TRACE_JUMP();
                        //					STACK_FRAME_POP();
                        //				}	break;
                        //				case 0x95c8: {	// LPM -- Load Program Memory R0 <- (Z) -- 1001 0101 1100 1000
                        //					uint16_t z = avr.data[R_ZL] | (avr.data[R_ZH] << 8);
                        //					STATE("lpm %s, (Z[%04x])\n", avr_regname(0), z);
                        //					cycle += 2; // 3 cycles
                        //					_avr_set_r(avr, 0, avr.flash[z]);
                        //				}	break;
                        //				case 0x95d8: {	// ELPM -- Load Program Memory R0 <- (Z) -- 1001 0101 1101 1000
                        //					if (!avr.rampz)
                        //						_avr_invalid_opcode(avr);
                        //					uint32_t z = avr.data[R_ZL] | (avr.data[R_ZH] << 8) | (avr.data[avr.rampz] << 16);
                        //					STATE("elpm %s, (Z[%02x:%04x])\n", avr_regname(0), z >> 16, z & 0xffff);
                        //					_avr_set_r(avr, 0, avr.flash[z]);
                        //					cycle += 2; // 3 cycles
                        //				}	break;
                        //				default:  {
                        //					switch (opcode & 0xfe0f) {
                        //						case 0x9000: {	// LDS -- Load Direct from Data Space, 32 bits -- 1001 0000 0000 0000
                        //							get_d5(opcode);
                        //							uint16_t x = _avr_flash_read16le(avr, new_pc);
                        //							new_pc += 2;
                        //							STATE("lds %s[%02x], 0x%04x\n", avr_regname(d), avr.data[d], x);
                        //							_avr_set_r(avr, d, _avr_get_ram(avr, x));
                        //							cycle++; // 2 cycles
                        //						}	break;
                        //						case 0x9005:
                        //						case 0x9004: {	// LPM -- Load Program Memory -- 1001 000d dddd 01oo
                        //							get_d5(opcode);
                        //							uint16_t z = avr.data[R_ZL] | (avr.data[R_ZH] << 8);
                        //							int op = opcode & 1;
                        //							STATE("lpm %s, (Z[%04x]%s)\n", avr_regname(d), z, op ? "+" : "");
                        //							_avr_set_r(avr, d, avr.flash[z]);
                        //							if (op) {
                        //								z++;
                        //								_avr_set_r16le_hl(avr, R_ZL, z);
                        //							}
                        //							cycle += 2; // 3 cycles
                        //						}	break;
                        //						case 0x9006:
                        //						case 0x9007: {	// ELPM -- Extended Load Program Memory -- 1001 000d dddd 01oo
                        //							if (!avr.rampz)
                        //								_avr_invalid_opcode(avr);
                        //							uint32_t z = avr.data[R_ZL] | (avr.data[R_ZH] << 8) | (avr.data[avr.rampz] << 16);
                        //							get_d5(opcode);
                        //							int op = opcode & 1;
                        //							STATE("elpm %s, (Z[%02x:%04x]%s)\n", avr_regname(d), z >> 16, z & 0xffff, op ? "+" : "");
                        //							_avr_set_r(avr, d, avr.flash[z]);
                        //							if (op) {
                        //								z++;
                        //								_avr_set_r(avr, avr.rampz, z >> 16);
                        //								_avr_set_r16le_hl(avr, R_ZL, z);
                        //							}
                        //							cycle += 2; // 3 cycles
                        //						}	break;
                        //						/*
                        //						 * Load store instructions
                        //						 *
                        //						 * 1001 00sr rrrr iioo
                        //						 * s = 0 = load, 1 = store
                        //						 * ii = 16 bits register index, 11 = X, 10 = Y, 00 = Z
                        //						 * oo = 1) post increment, 2) pre-decrement
                        //						 */
                        //						case 0x900c:
                        //						case 0x900d:
                        //						case 0x900e: {	// LD -- Load Indirect from Data using X -- 1001 000d dddd 11oo
                        //							int op = opcode & 3;
                        //							get_d5(opcode);
                        //							uint16_t x = (avr.data[R_XH] << 8) | avr.data[R_XL];
                        //							STATE("ld %s, %sX[%04x]%s\n", avr_regname(d), op == 2 ? "--" : "", x, op == 1 ? "++" : "");
                        //							cycle++; // 2 cycles (1 for tinyavr, except with inc/dec 2)
                        //							if (op == 2) x--;
                        //							byte vd = _avr_get_ram(avr, x);
                        //							if (op == 1) x++;
                        //							_avr_set_r16le_hl(avr, R_XL, x);
                        //							_avr_set_r(avr, d, vd);
                        //						}	break;
                        //						case 0x920c:
                        //						case 0x920d:
                        //						case 0x920e: {	// ST -- Store Indirect Data Space X -- 1001 001d dddd 11oo
                        //							int op = opcode & 3;
                        //							get_vd5(opcode);
                        //							uint16_t x = (avr.data[R_XH] << 8) | avr.data[R_XL];
                        //							STATE("st %sX[%04x]%s, %s[%02x] \n", op == 2 ? "--" : "", x, op == 1 ? "++" : "", avr_regname(d), vd);
                        //							cycle++; // 2 cycles, except tinyavr
                        //							if (op == 2) x--;
                        //							_avr_set_ram(avr, x, vd);
                        //							if (op == 1) x++;
                        //							_avr_set_r16le_hl(avr, R_XL, x);
                        //						}	break;
                        //						case 0x9009:
                        //						case 0x900a: {	// LD -- Load Indirect from Data using Y -- 1001 000d dddd 10oo
                        //							int op = opcode & 3;
                        //							get_d5(opcode);
                        //							uint16_t y = (avr.data[R_YH] << 8) | avr.data[R_YL];
                        //							STATE("ld %s, %sY[%04x]%s\n", avr_regname(d), op == 2 ? "--" : "", y, op == 1 ? "++" : "");
                        //							cycle++; // 2 cycles, except tinyavr
                        //							if (op == 2) y--;
                        //							byte vd = _avr_get_ram(avr, y);
                        //							if (op == 1) y++;
                        //							_avr_set_r16le_hl(avr, R_YL, y);
                        //							_avr_set_r(avr, d, vd);
                        //						}	break;
                        //						case 0x9209:
                        //						case 0x920a: {	// ST -- Store Indirect Data Space Y -- 1001 001d dddd 10oo
                        //							int op = opcode & 3;
                        //							get_vd5(opcode);
                        //							uint16_t y = (avr.data[R_YH] << 8) | avr.data[R_YL];
                        //							STATE("st %sY[%04x]%s, %s[%02x]\n", op == 2 ? "--" : "", y, op == 1 ? "++" : "", avr_regname(d), vd);
                        //							cycle++;
                        //							if (op == 2) y--;
                        //							_avr_set_ram(avr, y, vd);
                        //							if (op == 1) y++;
                        //							_avr_set_r16le_hl(avr, R_YL, y);
                        //						}	break;
                        //						case 0x9200: {	// STS -- Store Direct to Data Space, 32 bits -- 1001 0010 0000 0000
                        //							get_vd5(opcode);
                        //							uint16_t x = _avr_flash_read16le(avr, new_pc);
                        //							new_pc += 2;
                        //							STATE("sts 0x%04x, %s[%02x]\n", x, avr_regname(d), vd);
                        //							cycle++;
                        //							_avr_set_ram(avr, x, vd);
                        //						}	break;
                        //						case 0x9001:
                        //						case 0x9002: {	// LD -- Load Indirect from Data using Z -- 1001 000d dddd 00oo
                        //							int op = opcode & 3;
                        //							get_d5(opcode);
                        //							uint16_t z = (avr.data[R_ZH] << 8) | avr.data[R_ZL];
                        //							STATE("ld %s, %sZ[%04x]%s\n", avr_regname(d), op == 2 ? "--" : "", z, op == 1 ? "++" : "");
                        //							cycle++;; // 2 cycles, except tinyavr
                        //							if (op == 2) z--;
                        //							byte vd = _avr_get_ram(avr, z);
                        //							if (op == 1) z++;
                        //							_avr_set_r16le_hl(avr, R_ZL, z);
                        //							_avr_set_r(avr, d, vd);
                        //						}	break;
                        //						case 0x9201:
                        //						case 0x9202: {	// ST -- Store Indirect Data Space Z -- 1001 001d dddd 00oo
                        //							int op = opcode & 3;
                        //							get_vd5(opcode);
                        //							uint16_t z = (avr.data[R_ZH] << 8) | avr.data[R_ZL];
                        //							STATE("st %sZ[%04x]%s, %s[%02x] \n", op == 2 ? "--" : "", z, op == 1 ? "++" : "", avr_regname(d), vd);
                        //							cycle++; // 2 cycles, except tinyavr
                        //							if (op == 2) z--;
                        //							_avr_set_ram(avr, z, vd);
                        //							if (op == 1) z++;
                        //							_avr_set_r16le_hl(avr, R_ZL, z);
                        //						}	break;
                        //						case 0x900f: {	// POP -- 1001 000d dddd 1111
                        //							get_d5(opcode);
                        //							_avr_set_r(avr, d, _avr_pop8(avr));
                        //							T(uint16_t sp = _avr_sp_get(avr);)
                        //							STATE("pop %s (@%04x)[%02x]\n", avr_regname(d), sp, avr.data[sp]);
                        //							cycle++;
                        //						}	break;
                        //						case 0x920f: {	// PUSH -- 1001 001d dddd 1111
                        //							get_vd5(opcode);
                        //							_avr_push8(avr, vd);
                        //							T(uint16_t sp = _avr_sp_get(avr);)
                        //							STATE("push %s[%02x] (@%04x)\n", avr_regname(d), vd, sp);
                        //							cycle++;
                        //						}	break;
                        //						case 0x9400: {	// COM -- One's Complement -- 1001 010d dddd 0000
                        //							get_vd5(opcode);
                        //							byte res = 0xff - vd;
                        //							STATE("com %s[%02x] = %02x\n", avr_regname(d), vd, res);
                        //							_avr_set_r(avr, d, res);
                        //							_avr_flags_znv0s(avr, res);
                        //							avr.sreg[S_C] = 1;
                        //							SREG();
                        //						}	break;
                        //						case 0x9401: {	// NEG -- Two's Complement -- 1001 010d dddd 0001
                        //							get_vd5(opcode);
                        //							byte res = 0x00 - vd;
                        //							STATE("neg %s[%02x] = %02x\n", avr_regname(d), vd, res);
                        //							_avr_set_r(avr, d, res);
                        //							avr.sreg[S_H] = ((res >> 3) | (vd >> 3)) & 1;
                        //							avr.sreg[S_V] = res == 0x80;
                        //							avr.sreg[S_C] = res != 0;
                        //							_avr_flags_zns(avr, res);
                        //							SREG();
                        //						}	break;
                        //						case 0x9402: {	// SWAP -- Swap Nibbles -- 1001 010d dddd 0010
                        //							get_vd5(opcode);
                        //							byte res = (vd >> 4) | (vd << 4) ;
                        //							STATE("swap %s[%02x] = %02x\n", avr_regname(d), vd, res);
                        //							_avr_set_r(avr, d, res);
                        //						}	break;
                        //						case 0x9403: {	// INC -- Increment -- 1001 010d dddd 0011
                        //							get_vd5(opcode);
                        //							byte res = vd + 1;
                        //							STATE("inc %s[%02x] = %02x\n", avr_regname(d), vd, res);
                        //							_avr_set_r(avr, d, res);
                        //							avr.sreg[S_V] = res == 0x80;
                        //							_avr_flags_zns(avr, res);
                        //							SREG();
                        //						}	break;
                        //						case 0x9405: {	// ASR -- Arithmetic Shift Right -- 1001 010d dddd 0101
                        //							get_vd5(opcode);
                        //							byte res = (vd >> 1) | (vd & 0x80);
                        //							STATE("asr %s[%02x]\n", avr_regname(d), vd);
                        //							_avr_set_r(avr, d, res);
                        //							_avr_flags_zcnvs(avr, res, vd);
                        //							SREG();
                        //						}	break;
                        //						case 0x9406: {	// LSR -- Logical Shift Right -- 1001 010d dddd 0110
                        //							get_vd5(opcode);
                        //							byte res = vd >> 1;
                        //							STATE("lsr %s[%02x]\n", avr_regname(d), vd);
                        //							_avr_set_r(avr, d, res);
                        //							avr.sreg[S_N] = 0;
                        //							_avr_flags_zcvs(avr, res, vd);
                        //							SREG();
                        //						}	break;
                        //						case 0x9407: {	// ROR -- Rotate Right -- 1001 010d dddd 0111
                        //							get_vd5(opcode);
                        //							byte res = (avr.sreg[S_C] ? 0x80 : 0) | vd >> 1;
                        //							STATE("ror %s[%02x]\n", avr_regname(d), vd);
                        //							_avr_set_r(avr, d, res);
                        //							_avr_flags_zcnvs(avr, res, vd);
                        //							SREG();
                        //						}	break;
                        //						case 0x940a: {	// DEC -- Decrement -- 1001 010d dddd 1010
                        //							get_vd5(opcode);
                        //							byte res = vd - 1;
                        //							STATE("dec %s[%02x] = %02x\n", avr_regname(d), vd, res);
                        //							_avr_set_r(avr, d, res);
                        //							avr.sreg[S_V] = res == 0x7f;
                        //							_avr_flags_zns(avr, res);
                        //							SREG();
                        //						}	break;
                        //						case 0x940c:
                        //						case 0x940d: {	// JMP -- Long Call to sub, 32 bits -- 1001 010a aaaa 110a
                        //							avr_flashaddr_t a = ((opcode & 0x01f0) >> 3) | (opcode & 1);
                        //							uint16_t x = _avr_flash_read16le(avr, new_pc);
                        //							a = (a << 16) | x;
                        //							STATE("jmp 0x%06x\n", a);
                        //							new_pc = a << 1;
                        //							cycle += 2;
                        //							TRACE_JUMP();
                        //						}	break;
                        //						case 0x940e:
                        //						case 0x940f: {	// CALL -- Long Call to sub, 32 bits -- 1001 010a aaaa 111a
                        //							avr_flashaddr_t a = ((opcode & 0x01f0) >> 3) | (opcode & 1);
                        //							uint16_t x = _avr_flash_read16le(avr, new_pc);
                        //							a = (a << 16) | x;
                        //							STATE("call 0x%06x\n", a);
                        //							new_pc += 2;
                        //							cycle += 1 + _avr_push_addr(avr, new_pc);
                        //							new_pc = a << 1;
                        //							TRACE_JUMP();
                        //							STACK_FRAME_PUSH();
                        //						}	break;

                        //						default: {
                        //							switch (opcode & 0xff00) {
                        //								case 0x9600: {	// ADIW -- Add Immediate to Word -- 1001 0110 KKpp KKKK
                    //    const byte p = 24 + ((o >> 3) & 0x6); \
        		//const byte k = ((o & 0x00c0) >> 2) | (o & 0xf); \
        		//const uint16_t vp = avr.data[p] | (avr.data[p + 1] << 8);

                        //									uint16_t res = vp + k;
                        //									STATE("adiw %s:%s[%04x], 0x%02x\n", avr_regname(p), avr_regname(p + 1), vp, k);
                        //									_avr_set_r16le_hl(avr, p, res);
                        //									avr.sreg[S_V] = ((~vp & res) >> 15) & 1;
                        //									avr.sreg[S_C] = ((~res & vp) >> 15) & 1;
                        //									_avr_flags_zns16(avr, res);
                        //									SREG();
                        //									cycle++;
                        //								}	break;
                        //								case 0x9700: {	// SBIW -- Subtract Immediate from Word -- 1001 0111 KKpp KKKK
                  //      const byte p = 24 + ((o >> 3) & 0x6); \
        		//const byte k = ((o & 0x00c0) >> 2) | (o & 0xf); \
        		//const uint16_t vp = avr.data[p] | (avr.data[p + 1] << 8);

                        //									uint16_t res = vp - k;
                        //									STATE("sbiw %s:%s[%04x], 0x%02x\n", avr_regname(p), avr_regname(p + 1), vp, k);
                        //									_avr_set_r16le_hl(avr, p, res);
                        //									avr.sreg[S_V] = ((vp & ~res) >> 15) & 1;
                        //									avr.sreg[S_C] = ((res & ~vp) >> 15) & 1;
                        //									_avr_flags_zns16(avr, res);
                        //									SREG();
                        //									cycle++;
                        //								}	break;
                        //								case 0x9800: {	// CBI -- Clear Bit in I/O Register -- 1001 1000 AAAA Abbb
                  //      const byte io = ((o >> 3) & 0x1f) + 32;
                    //    const byte mask = 1 << (o & 0x7);

                        //									byte res = _avr_get_ram(avr, io) & ~mask;
                        //									STATE("cbi %s[%04x], 0x%02x = %02x\n", avr_regname(io), avr.data[io], mask, res);
                        //									_avr_set_ram(avr, io, res);
                        //									cycle++;
                        //								}	break;
                        //								case 0x9900: {	// SBIC -- Skip if Bit in I/O Register is Cleared -- 1001 1001 AAAA Abbb
                      //  const byte io = ((o >> 3) & 0x1f) + 32;
                      //  const byte mask = 1 << (o & 0x7);

                        //									byte res = _avr_get_ram(avr, io) & mask;
                        //									STATE("sbic %s[%04x], 0x%02x\t; Will%s branch\n", avr_regname(io), avr.data[io], mask, !res?"":" not");
                        //									if (!res) {
                        //										if (_avr_is_instruction_32_bits(avr, new_pc)) {
                        //											new_pc += 4; cycle += 2;
                        //										} else {
                        //											new_pc += 2; cycle++;
                        //										}
                        //									}
                        //								}	break;
                        //								case 0x9a00: {	// SBI -- Set Bit in I/O Register -- 1001 1010 AAAA Abbb
                     //   const byte io = ((o >> 3) & 0x1f) + 32;
                     //   const byte mask = 1 << (o & 0x7);

                        //									byte res = _avr_get_ram(avr, io) | mask;
                        //									STATE("sbi %s[%04x], 0x%02x = %02x\n", avr_regname(io), avr.data[io], mask, res);
                        //									_avr_set_ram(avr, io, res);
                        //									cycle++;
                        //								}	break;
                        //								case 0x9b00: {	// SBIS -- Skip if Bit in I/O Register is Set -- 1001 1011 AAAA Abbb
                     //   const byte io = ((o >> 3) & 0x1f) + 32;
                     //   const byte mask = 1 << (o & 0x7);

                        //									byte res = _avr_get_ram(avr, io) & mask;
                        //									STATE("sbis %s[%04x], 0x%02x\t; Will%s branch\n", avr_regname(io), avr.data[io], mask, res?"":" not");
                        //									if (res) {
                        //										if (_avr_is_instruction_32_bits(avr, new_pc)) {
                        //											new_pc += 4; cycle += 2;
                        //										} else {
                        //											new_pc += 2; cycle++;
                        //										}
                        //									}
                        //								}	break;
                        //								default:
                        //									switch (opcode & 0xfc00) {
                        //										case 0x9c00: {	// MUL -- Multiply Unsigned -- 1001 11rd dddd rrrr
                        //											byte r = ((opcode >> 5) & 0x10) | (opcode & 0xf); 
                    //    byte d = (opcode >> 4) & 0x1f;
                    //    byte vd = avr.data[d];
                     //   byte vr = avr.data[r];
                        //											uint16_t res = vd * vr;
                        //											STATE("mul %s[%02x], %s[%02x] = %04x\n", avr_regname(d), vd, avr_regname(r), vr, res);
                        //											cycle++;
                        //											_avr_set_r16le(avr, 0, res);
                        //											avr.sreg[S_Z] = res == 0;
                        //											avr.sreg[S_C] = (res >> 15) & 1;
                        //											SREG();
                        //										}	break;
                        //										default: _avr_invalid_opcode(avr);
                        //									}
                        //							}
                        //						}	break;
                        //					}
                        //				}	break;
                        //			}
                    }	break;

        		case 0xb000: {
                        //			switch (opcode & 0xf800) {
                        //				case 0xb800: {	// OUT A,Rr -- 1011 1AAd dddd AAAA
                        //					byte d = (opcode >> 4) & 0x1f; \
                     //   byte A = ((((opcode >> 9) & 3) << 4) | ((opcode) & 0xf)) + 32;
                        //					STATE("out %s, %s[%02x]\n", avr_regname(A), avr_regname(d), avr.data[d]);
                        //					_avr_set_ram(avr, A, avr.data[d]);
                        //				}	break;
                        //				case 0xb000: {	// IN Rd,A -- 1011 0AAd dddd AAAA
                        //					byte d = (opcode >> 4) & 0x1f; \
                     //   byte A = ((((opcode >> 9) & 3) << 4) | ((opcode) & 0xf)) + 32;
                        //					STATE("in %s, %s[%02x]\n", avr_regname(d), avr_regname(A), avr.data[A]);
                        //					_avr_set_r(avr, d, _avr_get_ram(avr, A));
                        //				}	break;
                        //				default: _avr_invalid_opcode(avr);
                        //			}
                    }	break;

        		case 0xc000: {  // RJMP -- 1100 kkkk kkkk kkkk
                     //   const int16_t o = ((int16_t)((op << 4) & 0xffff)) >> 3;
                        //			STATE("rjmp .%d [%04x]\n", o >> 1, new_pc + o);
                        //			new_pc = (new_pc + o) % (avr.flashend+1);
                        //			cycle++;
                        //			TRACE_JUMP();
                    }	break;

        		case 0xd000: {  // RCALL -- 1101 kkkk kkkk kkkk
                      //  const int16_t o = ((int16_t)((op << 4) & 0xffff)) >> 3;
                        //			STATE("rcall .%d [%04x]\n", o >> 1, new_pc + o);
                        //			cycle += _avr_push_addr(avr, new_pc);
                        //			new_pc = (new_pc + o) % (avr.flashend+1);
                        //			// 'rcall .1' is used as a cheap "push 16 bits of room on the stack"
                        //			if (o != 0) {
                        //				TRACE_JUMP();
                        //				STACK_FRAME_PUSH();
                        //			}
                    }	break;

        		case 0xe000: {  // LDI Rd, K aka SER (LDI r, 0xff) -- 1110 kkkk dddd kkkk
                       // const byte h = 16 + ((o >> 4) & 0xf); \
        		       // const byte k = ((o & 0x0f00) >> 4) | (o & 0xf);
                        //			STATE("ldi %s, 0x%02x\n", avr_regname(h), k);
                        //			_avr_set_r(avr, h, k);
                    }	break;

        		case 0xf000: {
                        //			switch (opcode & 0xfe00) {
                        //				case 0xf000:
                        //				case 0xf200:
                        //				case 0xf400:
                        //				case 0xf600: {	// BRXC/BRXS -- All the SREG branches -- 1111 0Boo oooo osss
                        //					int16_t o = ((int16_t)(opcode << 6)) >> 9; // offset
                        //					byte s = opcode & 7;
                        //					int set = (opcode & 0x0400) == 0;		// this bit means BRXC otherwise BRXS
                        //					int branch = (avr.sreg[s] && set) || (!avr.sreg[s] && !set);
                        //					const char *names[2][8] = {
                        //							{ "brcc", "brne", "brpl", "brvc", NULL, "brhc", "brtc", "brid"},
                        //							{ "brcs", "breq", "brmi", "brvs", NULL, "brhs", "brts", "brie"},
                        //					};
                        //					if (names[set][s]) {
                        //						STATE("%s .%d [%04x]\t; Will%s branch\n", names[set][s], o, new_pc + (o << 1), branch ? "":" not");
                        //					} else {
                        //						STATE("%s%c .%d [%04x]\t; Will%s branch\n", set ? "brbs" : "brbc", _sreg_bit_name[s], o, new_pc + (o << 1), branch ? "":" not");
                        //					}
                        //					if (branch) {
                        //						cycle++; // 2 cycles if taken, 1 otherwise
                        //						new_pc = new_pc + (o << 1);
                        //					}
                        //				}	break;
                        //				case 0xf800:
                        //				case 0xf900: {	// BLD -- Bit Store from T into a Bit in Register -- 1111 100d dddd 0bbb
                        //					byte d = (opcode >> 4) & 0x1f; \
                       // byte vd = avr.data[d];
                       // byte s = opcode & 7;
                       // byte mask = 1 << s;
                        //					byte v = (vd & ~mask) | (avr.sreg[S_T] ? mask : 0);
                        //					STATE("bld %s[%02x], 0x%02x = %02x\n", avr_regname(d), vd, mask, v);
                        //					_avr_set_r(avr, d, v);
                        //				}	break;
                        //				case 0xfa00:
                        //				case 0xfb00:{	// BST -- Bit Store into T from bit in Register -- 1111 101d dddd 0bbb
                        //					byte d = (opcode >> 4) & 0x1f; \
                       // byte vd = avr.data[d];
                       // byte s = opcode & 7;
                        //					STATE("bst %s[%02x], 0x%02x\n", avr_regname(d), vd, 1 << s);
                        //					avr.sreg[S_T] = (vd >> s) & 1;
                        //					SREG();
                        //				}	break;
                        //				case 0xfc00:
                        //				case 0xfe00: {	// SBRS/SBRC -- Skip if Bit in Register is Set/Clear -- 1111 11sd dddd 0bbb
                        //					byte d = (opcode >> 4) & 0x1f; \
                        //byte vd = avr.data[d];
                        //byte s = opcode & 7;
                        //byte mask = 1 << s;
                        //					int set = (opcode & 0x0200) != 0;
                        //					int branch = ((vd & mask) && set) || (!(vd & mask) && !set);
                        //					STATE("%s %s[%02x], 0x%02x\t; Will%s branch\n", set ? "sbrs" : "sbrc", avr_regname(d), vd, mask, branch ? "":" not");
                        //					if (branch) {
                        //						if (_avr_is_instruction_32_bits(avr, new_pc)) {
                        //							new_pc += 4; cycle += 2;
                        //						} else {
                        //							new_pc += 2; cycle++;
                        //						}
                        //					}
                        //				}	break;
                        //				default: _avr_invalid_opcode(avr);
                        //			}
                    }	break;

        		default: _avr_invalid_opcode(avr); break;

        	}
            avr.cyclesDone = cycle;
            ////avr.cycle += cycle;

            ////
            /*if( (avr.state == cpu_Running)
             && (avr.run_cycle_count > cycle)
             && (avr.interrupt_state == 0))
            {
                avr.run_cycle_count -= cycle;
                avr.pc = new_pc;
                goto run_one_again;
            }*/

            return new_pc;
        }

    }
}