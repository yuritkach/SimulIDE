using SimulIDE.src.gui.editor;
using System;
using System.Windows.Controls;

namespace SimulIDE.src.simavr.sim
{
    public class Sim_core_helper : Sim_Avr
    {
        public static TextBox console; 

        static Sim_core_helper()
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

        public static void DUMP_REG(Avr avr)
        { 
            for (int i = 0; i < 32; i++)
                console.AppendText(string.Format("%s=%02x%c", Avr_regname((byte)i), avr.data[i],i==15?'\n':' '));
            console.AppendText(string.Format("\n"));
        	ushort y = (ushort)(avr.data[R_YL] | (avr.data[R_YH]<<8));
        	for (int i = 0; i < 20; i++)
                console.AppendText(string.Format("Y+%02d=%02x ", i, avr.data[y+i]));
            console.AppendText(string.Format("\n"));
        }

        public static void DUMP_STACK(Avr avr)
        {
            for (int i = avr.trace_data.stack_frame_index; i==0; i--)
            {
            	int pci = i-1;
                //var oldColor = console.Foreground;
                //console. Foreground = new ;
                console.AppendText(string.Format("*** %04x: %-25s sp %04x\n",avr.trace_data.stack_frame[pci].pc, 
            		(avr.trace_data.codeline.Length!=0)?avr.trace_data.codeline[avr.trace_data.stack_frame[pci].pc>>1].symbol : "unknown",
                    avr.trace_data.stack_frame[pci].sp));
                //console.ForegroundColor = oldColor;
            }
        }

        ///**
        // * Reconstructs the SREG value from avr.sreg into dst.
        // */
        public static void READ_SREG_INTO(Avr avr, ref byte dst)
        {
            dst = 0;
            for (int i = 0; i < 8; i++)
                if (avr.sreg[i] > 1)
                    console.AppendText("** Invalid SREG!!\n");
                else
                if (avr.sreg[i] != 0)
                    dst |= (byte)(1 << i);
        }

        public static void avr_sreg_set(Avr avr, byte flag, bool ival)
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
                    if (avr.sreg[S_I] != 0)
                        avr.interrupt_state = -2;
                }
                else
                    avr.interrupt_state = 0;
            }

            avr.sreg[flag] = (byte)(ival ? 1 : 0);
        }

        ///**
        // * Splits the SREG value from src into the avr.sreg array.
        // */
        public static void SET_SREG_FROM(Avr avr, byte src)
        {
            for (byte i = 0; i < 8; i++)
                avr_sreg_set(avr, i, (src & (1 << i)) != 0);
        }

        //// SREG bit names
        public static string _sreg_bit_name = "cznvshti";

        ///*
        // * Handle "touching" registers, marking them changed.
        // * This is used only for debugging purposes to be able to
        // * print the effects of each instructions on registers
        // */

        public static void REG_TOUCH(Avr avr, ushort r)
        {
            avr.trace_data.touched[(r) >> 5] |= (ushort)(1 << ((r) & 0x1f));
        }

        public static bool REG_ISTOUCHED(Avr avr, ushort r)
        {
            return (avr.trace_data.touched[(r) >> 5] & (1 << ((r) & 0x1f)))!=0;
        }

        ///*
        // * This allows a "special case" to skip instruction tracing when in these
        // * symbols since printf() is useful to have, but generates a lot of cycles.
        // */
        public static bool dont_trace(string name)
        {
        	return 
        		!(name=="uart_putchar") ||
        		!(name=="fputc") ||
        		!(name=="printf") ||
        		!(name=="vfprintf") ||
        		!(name=="__ultoa_invert") ||
        		!(name=="__prologue_saves__") ||
        		!(name=="__epilogue_restores__");
        }

        public static bool donttrace = false;

        public static void STATE(Avr avr,string command, params object[] param)
        {
            if (avr.trace != 0)
            {
                if ((avr.trace_data.codeline.Length > 0) && (avr.trace_data.codeline[avr.PC >> 1] != null))
                {
                    string symn = avr.trace_data.codeline[avr.PC >> 1].symbol;
                    bool dont = dont_trace(symn);
                    if (dont != donttrace)
                    {
                        donttrace = dont;
                        DUMP_REG(avr);
                    }
                    if (!donttrace)
                        console.AppendText(string.Format("{0:X4}: {1:G} ", avr.PC, symn));
                }
                else
                {
                    console.AppendText(string.Format("0x"+avr.PC.ToString("X4")+" : "));
                    console.AppendText(string.Format(command, param));
                }
                //Console.WriteLine("%s: %04x: ", avr.PC);
            }
        }


        public static void SREG(Avr avr)
        {
            if (avr.trace != 0/* && donttrace == 0*/)
                console.AppendText(string.Format("\t\t\tSREG = "));
            for (int _sbi = 0; _sbi < 8; _sbi++)
                console.AppendText(string.Format(avr.sreg[_sbi] != 0 ? _sreg_bit_name[_sbi].ToString().ToUpper() : "."));
            console.AppendText(string.Format("\n"));
        }

        public static void Crash(Avr avr)
        {
            DUMP_REG(avr);
            console.AppendText(string.Format("*** CYCLE: {0:X4} PC: {1:X4}\n", avr.cycle, avr.PC));
            for (int i = Avr_trace_data.OLD_PC_SIZE-1; i > 0; i--)
            {
                int pci = (avr.trace_data.old_pci + i) & 0xf;
                console.AppendText(string.Format("*** {0:X4}: {1:G} RESET -{2:G}; sp {3:X4}\n",
                    avr.trace_data.old[pci].pc, avr.trace_data.codeline.Length!=0 ? 
                    avr.trace_data.codeline[avr.trace_data.old[pci].pc>>1].symbol : 
                    "unknown",Avr_trace_data.OLD_PC_SIZE-i, avr.trace_data.old[pci].sp));
            }

            console.AppendText(string.Format("Stack Ptr {0:X4}/{1:X4} = {2:G} \n", _avr_sp_get(avr), avr.ramend, avr.ramend - _avr_sp_get(avr)));
        	DUMP_STACK(avr);

        	Sim_Avr.Avr_sadly_crashed(avr, 0);
        }

        protected static ushort _avr_flash_read16le(Avr avr, uint addr)
        {
            return (ushort)(avr.flash[addr] | (avr.flash[addr + 1] << 8));
        }

        public static void Avr_core_watch_write(Avr avr, uint addr, uint v)
        {
            if (addr > avr.ramend)
            {
            //		AVR_LOG(avr, LOG_ERROR, FONT_RED
            //				"CORE: *** Invalid write address "
            //				"PC=%04x SP=%04x O=%04x Address %04x=%02x out of ram\n"
            //				FONT_DEFAULT,
            //				avr.pc, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc), addr, v);
                Crash(avr);
            }
            if (addr < 32)
            {
            //		AVR_LOG(avr, LOG_ERROR, FONT_RED
            //				"CORE: *** Invalid write address PC=%04x SP=%04x O=%04x Address %04x=%02x low registers\n"
            //				FONT_DEFAULT,
            //				avr.pc, _avr_sp_get(avr), _avr_flash_read16le(avr, avr.pc), addr, v);
            	Crash(avr);
            }
            /*
            * this checks that the current "function" is not doctoring the stack frame that is located
            * higher on the stack than it should be. It's a sign of code that has overrun it's stack
            * frame and is munching on it's own return address.
            */
            if (avr.trace_data.stack_frame_index > 1 && addr > avr.trace_data.stack_frame[avr.trace_data.stack_frame_index-2].sp)
            {
                //                var oldColor = Console.ForegroundColor;
                //                Console.ForegroundColor = ConsoleColor.Red;
                console.AppendText(string.Format( "{0:X4} : munching stack  SP {1:X4}, A={2:X4} <= {3:X2}\n",
            				avr.PC, _avr_sp_get(avr), addr, v));
 //               Console.ForegroundColor = oldColor;
            }

            if (avr.gdb!=null)
            {
            //    avr_gdb_handle_watchpoints(avr, addr, AVR_GDB_WATCH_WRITE);
            }

            avr.data[addr] = (byte) v;
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
                Crash(avr);
            }

            if (avr.gdb!=null)
            {
            //        		avr_gdb_handle_watchpoints(avr, addr, AVR_GDB_WATCH_READ);
            }

            return avr.data[addr];
        }

        ///*
        // * Set a register (r < 256)
        // * if it's an IO register (> 31) also (try to) call any callback that was
        // * registered to track changes to that register.
        // */
        public static void _avr_set_r(Avr avr, ushort r, uint v)
        {
            REG_TOUCH(avr, r);

            if (r == R_SREG)
            {
                avr.data[R_SREG] = (byte)v;
                //unsplit the SREG
                SET_SREG_FROM(avr, (byte)v);
                SREG(avr);
            }
            else
            if (r > 31)
            {
                ushort io = (ushort)AVR_DATA_TO_IO(r);
                if (avr.io[io].w.c!=null)
                    avr.io[io].w.c(avr, r, (byte) v, avr.io[io].w.param);
                else
                    avr.data[r] = (byte) v;
                if (avr.io[io].irq.Length!=0)
                {
                    Sim_irq.Avr_raise_irq(avr.io[io].irq[Sim_io.AVR_IOMEM_IRQ_ALL], v);
                    for (int i = 0; i < 8; i++)
                        Sim_irq.Avr_raise_irq(avr.io[io].irq[i], (v >> i) & 1);
                }
            }
            else
                avr.data[r] = (byte)v;
        }

        public static void _avr_set_r16le(Avr avr, ushort r, ushort v)
        {
            _avr_set_r(avr, r, v);
            _avr_set_r(avr, (byte)(r + 1), (byte)(v >> 8));
        }

        public static void _avr_set_r16le_hl(Avr avr, ushort r, ushort v)
        {
            _avr_set_r(avr, (ushort)(r + 1), (byte)(v >> 8));
            _avr_set_r(avr, r, (byte)v);
        }

        ///*
        // * Stack pointer access
        // */
        public static ushort _avr_sp_get(Avr avr)
        {
            return (ushort)(avr.data[R_SPL] | (avr.data[R_SPH] << 8));
        }

        public static void _avr_sp_set(Avr avr, ushort sp)
        {
            _avr_set_r16le(avr, R_SPL, sp);
        }

        ///*
        // * Set any address to a value; split between registers and SRAM
        // */
        public static void _avr_set_ram(Avr avr, ushort addr, uint v)
        {
            if (addr < (MAX_IOs + 31))
                _avr_set_r(avr, addr, v);
            else
                Avr_core_watch_write(avr, addr, v);
        }

        ///*
        // * Get a value from SRAM.
        // */
        public static byte _avr_get_ram(Avr avr, ushort addr)
        {
            if (addr == R_SREG)
            {
                /*
        		 * SREG is special it's reconstructed when read
        		 * while the core itself uses the "shortcut" array
        		 */
                READ_SREG_INTO(avr, ref avr.data[R_SREG]);
            }
            else
            if (addr > 31 && addr < 31 + MAX_IOs)
            {
                ushort io = (ushort)AVR_DATA_TO_IO(addr);

                if (avr.io[io].r.c != null)
                    avr.data[addr] = avr.io[io].r.c(avr, addr, avr.io[io].r.param);

                if (avr.io[io].irq != null)
                {
                    byte v = avr.data[addr];
                    Sim_irq.Avr_raise_irq(avr.io[io].irq[Sim_io.AVR_IOMEM_IRQ_ALL], v);
                    for (int i = 0; i < 8; i++)
                        Sim_irq.Avr_raise_irq(avr.io[io].irq[i], (uint)((v >> i) & 1));
                }
            }
            return Avr_core_watch_read(avr, addr);
        }

        ///*
        // * Stack push accessors.
        // */
        public static void _avr_push8(Avr avr, ushort v)
        {
        	ushort sp = _avr_sp_get(avr);
        	_avr_set_ram(avr, sp, v);
        	_avr_sp_set(avr,(ushort)(sp-1));
        }

        public static byte _avr_pop8(Avr avr)
        {
        	ushort sp = (ushort)(_avr_sp_get(avr) + 1);
        	byte res = _avr_get_ram(avr, sp);
            _avr_sp_set(avr, sp);
        	return res;
        }

        public static int _avr_push_addr(Avr avr, uint addr)
        {
            ushort sp = _avr_sp_get(avr);
            addr >>= 1;
            for (int i = 0; i < avr.address_size; i++, addr >>= 8, sp--)
            {
                _avr_set_ram(avr, sp, addr);
            }
            _avr_sp_set(avr, sp);
            return avr.address_size;
        }

        public static uint _avr_pop_addr(Avr avr)
        {
            ushort sp = (ushort)(_avr_sp_get(avr) + 1);
            uint res = 0;
            for (int i = 0; i < avr.address_size; i++, sp++)
                res = (res << 8) | _avr_get_ram(avr, sp);
            res <<= 1;
            _avr_sp_set(avr, (ushort)(sp - 1));
            return res;
        }

        ///*
        // * "Pretty" register names
        // */
        public static string[] reg_names = new string[255];


        public static string Avr_regname(byte reg)
        {
            if (reg_names[reg] == null)
            {
                string tt;
                if (reg < 32)
                    tt = "r" + reg.ToString();
                else
                    tt = "io:02x" + reg.ToString();
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
        
        ///*
        // * Dump changed registers when tracing
        // */
        
        public static void avr_dump_state(Avr avr)
        {
        	if (avr.trace==0 || donttrace)
        		return;

        	int doit = 0;

        	for (int r = 0; r < 3 && doit==0; r++)
        		if (avr.trace_data.touched[r]!=0)
        			doit = 1;
        	if (doit==0)
        		return;
            console.AppendText(string.Format("                                       ->> "));
        	ushort[] r16 = new ushort[4] { R_SPL, R_XL, R_YL, R_ZL };
        	for (int i = 0; i < 4; i++)
        		if (REG_ISTOUCHED(avr, r16[i]) || REG_ISTOUCHED(avr, r16[i+1]))
                {
        			REG_TOUCH(avr, (ushort)r16[i]);
        			REG_TOUCH(avr, (ushort)r16[i+1]);
        		}
        	for (int i = 0; i < 3*32; i++)
        		if (REG_ISTOUCHED(avr,(ushort) i)) {
                    console.AppendText(string.Format("{0:G}={1:X2} ", Avr_regname((byte)i), avr.data[i]));
        		}
            console.AppendText(string.Format("\n"));
        }
        
        
        /****************************************************************************\
         *
         * Helper functions for calculating the status register bit values.
         * See the Atmel data sheet for the instruction set for more info.
         *
        \****************************************************************************/

        public static void _avr_flags_zns(Avr avr, byte res)
        {
            avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_N] = (byte)((res >> 7) & 1);
            avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static void _avr_flags_zns16(Avr avr, ushort res)
        {
            avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_N] = (byte)((res >> 15) & 1);
            avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static void _avr_flags_add_zns(Avr avr, byte res, byte rd, byte rr)
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


        public static void _avr_flags_sub_zns(Avr avr, byte res, byte rd, byte rr)
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

        public static void _avr_flags_Rzns(Avr avr, byte res)
        {
            if (res != 0)
                avr.sreg[S_Z] = 0;
            avr.sreg[S_N] = (byte)((res >> 7) & 1);
            avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static void _avr_flags_sub_Rzns(Avr avr, byte res, byte rd, byte rr)
        {
            /* carry & half carry */
            byte sub_carry = (byte)((~rd & rr) | (rr & res) | (res & ~rd));
            avr.sreg[S_H] = (byte)((sub_carry >> 3) & 1);
            avr.sreg[S_C] = (byte)((sub_carry >> 7) & 1);
            /* overflow */
            avr.sreg[S_V] = (byte)((((rd & ~rr & ~res) | (~rd & rr & res)) >> 7) & 1);
            _avr_flags_Rzns(avr, res);
        }

        public static void _avr_flags_zcvs(Avr avr, byte res, byte vr)
        {
            avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_C] = (byte)(vr & 1);
            avr.sreg[S_V] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_C]);
            avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static void _avr_flags_zcnvs(Avr avr, byte res, byte vr)
        {
            avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
            avr.sreg[S_C] = (byte)(vr & 1);
            avr.sreg[S_N] = (byte)(res >> 7);
            avr.sreg[S_V] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_C]);
            avr.sreg[S_S] = (byte)(avr.sreg[S_N] ^ avr.sreg[S_V]);
        }

        public static void _avr_flags_znv0s(Avr avr, byte res)
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
            if (avr.PC >= avr.flashend)
            {
                STATE(avr, "CRASH\n");
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
                            case 0x0000:
                                {    // NOP
                                    STATE(avr, "nop\n");
                                }
                                break;
                            default:
                                {
                                    switch (opcode & 0xfc00)
                                    {
                                        case 0x0400:
                                            {  // CPC -- Compare with carry -- 0000 01rd dddd rrrr
                                                byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                                byte d = (byte)((opcode >> 4) & 0x1f);
                                                byte vd = avr.data[d];
                                                byte vr = avr.data[r];
                                                byte res = (byte)(vd - vr - avr.sreg[S_C]);
                                                STATE(avr, "cpc {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                                _avr_flags_sub_Rzns(avr, res, vd, vr);
                                                SREG(avr);
                                            }
                                            break;
                                        case 0x0c00:
                                            {  // ADD -- Add without carry -- 0000 11rd dddd rrrr
                                                byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                                byte d = (byte)((opcode >> 4) & 0x1f);
                                                byte vd = avr.data[d];
                                                byte vr = avr.data[r];
                                                byte res = (byte)(vd + vr);
                                                if (r == d)
                                                {
                                                    STATE(avr, "lsl {0:G}[{1:X2}] = {2:X2}", Avr_regname(d), vd, res & 0xff);
                                                }
                                                else
                                                {
                                                    STATE(avr, "add {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                                }
                                                _avr_set_r(avr, d, res);
                                                _avr_flags_add_zns(avr, res, vd, vr);
                                                SREG(avr);
                                            }
                                            break;
                                        case 0x0800:
                                            {  // SBC -- Subtract with carry -- 0000 10rd dddd rrrr
                                                byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                                byte d = (byte)((opcode >> 4) & 0x1f);
                                                byte vd = avr.data[d];
                                                byte vr = avr.data[r];
                                                byte res = (byte)(vd - vr - avr.sreg[S_C]);
                                                STATE(avr, "sbc {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), avr.data[d], Avr_regname(r), avr.data[r], res);
                                                _avr_set_r(avr, d, res);
                                                _avr_flags_sub_Rzns(avr, res, vd, vr);
                                                SREG(avr);
                                            }
                                            break;
                                        default:
                                            switch (opcode & 0xff00)
                                            {
                                                case 0x0100:
                                                    {  // MOVW -- Copy Register Word -- 0000 0001 dddd rrrr
                                                        byte d = (byte)(((opcode >> 4) & 0xf) << 1);
                                                        byte r = (byte)(((opcode) & 0xf) << 1);
                                                        STATE(avr, "movw {0:G}:{1:G}, {2:G}:{3:G}[{4:X2}{5:X2}]\n", Avr_regname(d), Avr_regname((byte)(d + 1)), Avr_regname(r), Avr_regname((byte)(r + 1)), avr.data[r + 1], avr.data[r]);
                                                        ushort vr = (ushort)(avr.data[r] | (avr.data[r + 1] << 8));
                                                        _avr_set_r16le(avr, d, vr);
                                                    }; break;
                                                case 0x0200:
                                                    {  // MULS -- Multiply Signed -- 0000 0010 dddd rrrr
                                                        byte r = (byte)(16 + (opcode & 0xf));
                                                        byte d = (byte)(16 + ((opcode >> 4) & 0xf));
                                                        ushort res = (ushort)(((byte)avr.data[r]) * ((byte)avr.data[d]));
                                                        STATE(avr, "muls {0:G}[{1:G}], {2:G}[{3:X2}] = {4:G}", Avr_regname(d), (byte)avr.data[d], Avr_regname(r), (byte)avr.data[r], res);
                                                        _avr_set_r16le(avr, 0, res);
                                                        avr.sreg[S_C] = (byte)((res >> 15) & 1);
                                                        avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
                                                        cycle++;
                                                        SREG(avr);
                                                    }; break;
                                                case 0x0300:
                                                    {  // MUL -- Multiply -- 0000 0011 fddd frrr
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
                                                                name = "fmulsu"; break;
                                                        }
                                                        cycle++;
                                                        STATE(avr, "{0:G} {1:G}[{2:G}], {3:G}[{4:X2}] = {5:G}", name, Avr_regname(d), (byte)avr.data[d], Avr_regname(r), (byte)avr.data[r], res);
                                                        _avr_set_r16le(avr, 0, res);
                                                        avr.sreg[S_C] = c;
                                                        avr.sreg[S_Z] = (byte)(res == 0 ? 1 : 0);
                                                        SREG(avr);
                                                    }
                                                    break;
                                                default: _avr_invalid_opcode(avr); break;
                                            }; break;
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case 0x1000:
                    {
                        switch (opcode & 0xfc00)
                        {
                            case 0x1800:
                                {   // SUB -- Subtract without carry -- 0001 10rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd - vr);
                                    STATE(avr, "sub {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                    _avr_set_r(avr, d, res);
                                    _avr_flags_sub_zns(avr, res, vd, vr);
                                    SREG(avr);
                                }; break;
                            case 0x1000:
                                {   // CPSE -- Compare, skip if equal -- 0001 00rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    ushort res = (ushort)(vd == vr ? 1 : 0);
                                    STATE(avr, "cpse {0:G}[{1:X2}], {2:G}[{3:X2}]\t; Will{4:G} skip\n", Avr_regname(d), avr.data[d], Avr_regname(r), avr.data[r], res != 0 ? "" : " not");
                                    if (res != 0)
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
                                }; break;
                            case 0x1400:
                                {   // CP -- Compare -- 0001 01rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd - vr);
                                    STATE(avr, "cp {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                    _avr_flags_sub_zns(avr, res, vd, vr);
                                    SREG(avr);
                                }; break;
                            case 0x1c00:
                                {   // ADD -- Add with carry -- 0001 11rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd + vr + avr.sreg[S_C]);
                                    if (r == d)
                                        STATE(avr, "rol {0:G}[{1:X2}] = {2:X2}", Avr_regname(d), avr.data[d], res);
                                    else
                                        STATE(avr, "addc {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), avr.data[d], Avr_regname(r), avr.data[r], res);
                                    _avr_set_r(avr, d, res);
                                    _avr_flags_add_zns(avr, res, vd, vr);
                                    SREG(avr);
                                }; break;
                            default: _avr_invalid_opcode(avr); break;
                        }
                    }
                    break;

                case 0x2000:
                    {
                        switch (opcode & 0xfc00)
                        {
                            case 0x2000:
                                {   // AND -- Logical AND -- 0010 00rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd & vr);
                                    if (r == d)
                                        STATE(avr, "tst {0:G}[0x{1:X2}] ;", Avr_regname(d), avr.data[d]);
                                    else
                                        STATE(avr, "and {0:G}[0x{1:X2}], {2:G}[0x{3:X2}] = 0x{4:X2} ;", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                    _avr_set_r(avr, d, res);
                                    _avr_flags_znv0s(avr, res);
                                    SREG(avr);
                                }
                                break;
                            case 0x2400:
                                {   // EOR -- Logical Exclusive OR -- 0010 01rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd ^ vr);
                                    if (r == d)
                                        STATE(avr, "clr {0:G}[{1:X2}]", Avr_regname(d), avr.data[d]);
                                    else
                                        STATE(avr, "eor {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                    _avr_set_r(avr, d, res);
                                    _avr_flags_znv0s(avr, res);
                                    SREG(avr);
                                }
                                break;
                            case 0x2800:
                                {   // OR -- Logical OR -- 0010 10rd dddd rrrr
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte vd = avr.data[d];
                                    byte vr = avr.data[r];
                                    byte res = (byte)(vd | vr);
                                    STATE(avr, "or {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X2}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                    _avr_set_r(avr, d, res);
                                    _avr_flags_znv0s(avr, res);
                                    SREG(avr);
                                }
                                break;
                            case 0x2c00:
                                {   // MOV -- 0010 11rd dddd rrrr
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf));
                                    byte vr = avr.data[r];
                                    byte res = vr;
                                    STATE(avr, "mov {0:G}, {1:G}[{2:X2}] = {3:X2}\n", Avr_regname(d), Avr_regname(r), vr, res);
                                    _avr_set_r(avr, d, res);
                                }
                                break;
                            default: _avr_invalid_opcode(avr); break;
                        }
                    }
                    break;

                case 0x3000:
                    {  // CPI -- Compare Immediate -- 0011 kkkk hhhh kkkk
                        byte h = (byte)(16 + ((opcode >> 4) & 0xf));
                        byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                        byte vh = avr.data[h];
                        byte res = (byte)(vh - k);
                        STATE(avr, "cpi {0:G}[{1:X2}], 0x{2:X2}", Avr_regname(h), vh, k);
                        _avr_flags_sub_zns(avr, res, vh, k);
                        SREG(avr);
                    }
                    break;

                case 0x4000:
                    {  // SBCI -- Subtract Immediate With Carry -- 0100 kkkk hhhh kkkk
                        byte h = (byte)(16 + ((opcode >> 4) & 0xf));
                        byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                        byte vh = avr.data[h];
                        byte res = (byte)(vh - k - avr.sreg[S_C]);
                        STATE(avr, "sbci {0:G}[{1:X2}], 0x{2:X2} = {3:X2}", Avr_regname(h), vh, k, res);
                        _avr_set_r(avr, h, res);
                        _avr_flags_sub_Rzns(avr, res, vh, k);
                        SREG(avr);
                    }
                    break;

                case 0x5000:
                    {  // SUBI -- Subtract Immediate -- 0101 kkkk hhhh kkkk
                        byte h = (byte)(16 + ((opcode >> 4) & 0xf));
                        byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                        byte vh = avr.data[h];
                        byte res = (byte)(vh - k);
                        STATE(avr, "subi {0:G}[{1:X2}], 0x{2:X2} = {3:X2}", Avr_regname(h), vh, k, res);
                        _avr_set_r(avr, h, res);
                        _avr_flags_sub_zns(avr, res, vh, k);
                        SREG(avr);
                    }
                    break;

                case 0x6000:
                    {  // ORI aka SBR -- Logical OR with Immediate -- 0110 kkkk hhhh kkkk
                        byte h = (byte)(16 + ((opcode >> 4) & 0xf));
                        byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                        byte vh = avr.data[h];
                        byte res = (byte)(vh | k);
                        STATE(avr, "ori {0:G}[{1:X2}], 0x{2:X2}", Avr_regname(h), vh, k);
                        _avr_set_r(avr, h, res);
                        _avr_flags_znv0s(avr, res);
                        SREG(avr);
                    }
                    break;

                case 0x7000:
                    {  // ANDI	-- Logical AND with Immediate -- 0111 kkkk hhhh kkkk
                        byte h = (byte)(16 + ((opcode >> 4) & 0xf));
                        byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                        byte vh = avr.data[h];
                        byte res = (byte)(vh & k);
                        STATE(avr, "andi {0:G}[{1:X2}], 0x{2:X2}", Avr_regname(h), vh, k);
                        _avr_set_r(avr, h, res);
                        _avr_flags_znv0s(avr, res);
                        SREG(avr);
                    }
                    break;

                case 0xa000:
                case 0x8000:
                    {  /*
                            	 * Load (LDD/STD) store instructions
                            	 *
                        		 * 10q0 qqsd dddd yqqq
                        		 * s = 0 = load, 1 = store
                        		 * y = 16 bits register index, 1 = Y, 0 = X
                        		 * q = 6 bit displacement
                        		 */
                        switch (opcode & 0xd008)
                        {
                            case 0xa000:
                            case 0x8000:
                                {   // LD (LDD) -- Load Indirect using Z -- 10q0 qqsd dddd yqqq
                                    ushort v = (ushort)(avr.data[R_ZL] | (avr.data[R_ZH] << 8));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte q = (byte)(((opcode & 0x2000) >> 8) | ((opcode & 0x0c00) >> 7) | (opcode & 0x7));
                                    if ((opcode & 0x0200) != 0)
                                    {
                                        STATE(avr, "st (Z+{0:G}[{1:X4}]), {2:G}[{3:X2}]\n", q, v + q, Avr_regname(d), avr.data[d]);
                                        _avr_set_ram(avr, (ushort)(v + q), avr.data[d]);
                                    }
                                    else
                                    {
                                        STATE(avr, "ld {0:G}, (Z+{1:G}[{2:X4}])=[{3:X2}]\n", Avr_regname(d), q, v + q, avr.data[v + q]);
                                        _avr_set_r(avr, d, _avr_get_ram(avr, (ushort)(v + q)));
                                    }
                                    cycle += 1; // 2 cycles, 3 for tinyavr
                                }
                                break;
                            case 0xa008:
                            case 0x8008:
                                {   // LD (LDD) -- Load Indirect using Y -- 10q0 qqsd dddd yqqq
                                    ushort v = (ushort)(avr.data[R_YL] | (avr.data[R_YH] << 8));
                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                    byte q = (byte)(((opcode & 0x2000) >> 8) | ((opcode & 0x0c00) >> 7) | (opcode & 0x7));

                                    if ((opcode & 0x0200) != 0)
                                    {
                                        STATE(avr, "st (Y+{0:G}[{1:X4}]), {2:G}[{3:X2}]\n", q, v + q, Avr_regname(d), avr.data[d]);
                                        _avr_set_ram(avr, (ushort)(v + q), avr.data[d]);
                                    }
                                    else
                                    {
                                        STATE(avr, "ld {0:G}, (Y+{1:G}[{2:X4}])=[{3:X2}]\n", Avr_regname(d), q, v + q, avr.data[d + q]);
                                        _avr_set_r(avr, d, _avr_get_ram(avr, (ushort)(v + q)));
                                    }
                                    cycle += 1; // 2 cycles, 3 for tinyavr
                                }
                                break;
                            default: _avr_invalid_opcode(avr); break;
                        }
                    }
                    break;

                case 0x9000:
                    {
                        /* this is an annoying special case, but at least these lines handle all the SREG set/clear opcodes */
                        if ((opcode & 0xff0f) == 0x9408)
                        {
                            byte b = (byte)((opcode >> 4) & 7);
                            STATE(avr, "{0:G}{1:G}", (opcode & 0x0080) != 0 ? "cl" : "se", _sreg_bit_name[b]);
                            avr_sreg_set(avr, b, (opcode & 0x0080) == 0);
                            SREG(avr);
                        }
                        else
                        {
                            switch (opcode)
                            {
                                case 0x9588:
                                    { // SLEEP -- 1001 0101 1000 1000
                                        STATE(avr, "sleep\n");
                                        /* Don't sleep if there are interrupts about to be serviced.
                                        * Without this check, it was possible to incorrectly enter a state
                                        * in which the cpu was sleeping and interrupts were disabled. For more
                                        * details, see the commit message. */
                                        if (!Sim_interrupts.Avr_has_pending_interrupts(avr) || avr.sreg[S_I] != 0)
                                            avr.state = CoreStates.cpu_Sleeping;
                                    }
                                    break;
                                case 0x9598:
                                    { // BREAK -- 1001 0101 1001 1000
                                        STATE(avr, "break\n");
                                        if (avr.gdb != null)
                                        {
                                            // if gdb is on, we break here as in here
                                            // and we do so until gdb restores the instruction
                                            // that was here before
                                            avr.state = CoreStates.cpu_StepDone;
                                            new_pc = avr.PC;
                                            cycle = 0;
                                        }
                                    }
                                    break;
                                case 0x95a8:
                                    { // WDR -- Watchdog Reset -- 1001 0101 1010 1000
                                        STATE(avr, "wdr\n");
                                        Sim_io.Avr_ioctl(avr, Avr_watchdog.AVR_IOCTL_WATCHDOG_RESET, 0);
                                    }
                                    break;
                                case 0x95e8:
                                    { // SPM -- Store Program Memory -- 1001 0101 1110 1000
                                        STATE(avr, "spm\n");
                                        Sim_io.Avr_ioctl(avr, Avr_flash_helper.AVR_IOCTL_FLASH_SPM, 0);
                                    }
                                    break;
                                case 0x9409:   // IJMP -- Indirect jump -- 1001 0100 0000 1001
                                case 0x9419:   // EIJMP -- Indirect jump -- 1001 0100 0001 1001   bit 4 is "indirect"
                                case 0x9509:   // ICALL -- Indirect Call to Subroutine -- 1001 0101 0000 1001
                                case 0x9519:
                                    { // EICALL -- Indirect Call to Subroutine -- 1001 0101 0001 1001   bit 8 is "push pc"
                                        uint e = opcode & 0x10;
                                        uint p = opcode & 0x100;
                                        if (e != 0 && avr.eind != 0)
                                            _avr_invalid_opcode(avr);
                                        uint z = (uint)(avr.data[R_ZL] | (avr.data[R_ZH] << 8));
                                        if (e != 0)
                                            z |= (uint)(avr.data[avr.eind] << 16);
                                        STATE(avr, "{0:G}i{1:G} Z[{2:X4}]\n", e != 0 ? "e" : "", p != 0 ? "call" : "jmp", z << 1);
                                        if (p != 0)
                                            cycle += (uint)(_avr_push_addr(avr, new_pc) - 1);
                                        new_pc = z << 1;
                                        cycle++;
                                        avr.trace_data.old[avr.trace_data.old_pci].pc = avr.PC;
                                        avr.trace_data.old[avr.trace_data.old_pci].sp = _avr_sp_get(avr);
                                        avr.trace_data.old_pci = (avr.trace_data.old_pci + 1) & (Avr_trace_data.OLD_PC_SIZE - 1);

                                    }
                                    break;
                                case 0x9518:
                                    {  // RETI -- Return from Interrupt -- 1001 0101 0001 1000
                                        avr_sreg_set(avr, S_I, true);
                                        Sim_interrupts.Avr_interrupt_reti(avr);
                                    }
                                    break; //!!!!! continue;
                                case 0x9508:
                                    { // RET -- Return -- 1001 0101 0000 1000
                                        new_pc = _avr_pop_addr(avr);
                                        cycle += (ulong)(1 + avr.address_size);
                                        STATE(avr, "ret{0:G}\n", ((opcode & 0x10) != 0) ? "i" : "");
                                        avr.trace_data.old[avr.trace_data.old_pci].pc = avr.PC;
                                        avr.trace_data.old[avr.trace_data.old_pci].sp = _avr_sp_get(avr);
                                        avr.trace_data.old_pci = (avr.trace_data.old_pci + 1) & (Avr_trace_data.OLD_PC_SIZE - 1);
                                        if (avr.trace_data.stack_frame_index > 0)
                                            avr.trace_data.stack_frame_index--;
                                    }
                                    break;
                                case 0x95c8:
                                    {   // LPM -- Load Program Memory R0 <- (Z) -- 1001 0101 1100 1000
                                        ushort z = (ushort)(avr.data[R_ZL] | (avr.data[R_ZH] << 8));
                                        STATE(avr, "lpm {0:G}, (Z[0x{1:X4}])\n", Avr_regname(0), z);
                                        cycle += 2; // 3 cycles
                                        _avr_set_r(avr, 0, avr.flash[z]);
                                    }
                                    break;
                                case 0x95d8:
                                    {   // ELPM -- Load Program Memory R0 <- (Z) -- 1001 0101 1101 1000
                                        if (avr.rampz != 0)
                                            _avr_invalid_opcode(avr);
                                        uint z = (uint)(avr.data[R_ZL] | (avr.data[R_ZH] << 8) | (avr.data[avr.rampz] << 16));
                                        STATE(avr, "elpm {0:G}, (Z[{1:X2}:{2:X4}])\n", Avr_regname(0), z >> 16, z & 0xffff);
                                        _avr_set_r(avr, 0, avr.flash[z]);
                                        cycle += 2; // 3 cycles
                                    }
                                    break;
                                default:
                                    {
                                        switch (opcode & 0xfe0f)
                                        {
                                            case 0x9000:
                                                {  // LDS -- Load Direct from Data Space, 32 bits -- 1001 0000 0000 0000
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    ushort x = _avr_flash_read16le(avr, new_pc);
                                                    new_pc += 2;
                                                    STATE(avr, "lds {0:G}[{1:X2}], 0x{2:X4}\n", Avr_regname(d), avr.data[d], x);
                                                    _avr_set_r(avr, d, _avr_get_ram(avr, x));
                                                    cycle++; // 2 cycles
                                                }
                                                break;
                                            case 0x9005:
                                            case 0x9004:
                                                {  // LPM -- Load Program Memory -- 1001 000d dddd 01oo
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    ushort z = (ushort)(avr.data[R_ZL] | (avr.data[R_ZH] << 8));
                                                    int op = (int)(opcode & 1);
                                                    STATE(avr, "lpm {0:G}, (Z[0x{1:X4}]{2:G})\n", Avr_regname(d), z, op != 0 ? "+" : "");
                                                    _avr_set_r(avr, d, avr.flash[z]);
                                                    if (op != 0)
                                                    {
                                                        z++;
                                                        _avr_set_r16le_hl(avr, R_ZL, z);
                                                    }
                                                    cycle += 2; // 3 cycles
                                                }
                                                break;
                                            case 0x9006:
                                            case 0x9007:
                                                {  // ELPM -- Extended Load Program Memory -- 1001 000d dddd 01oo
                                                    if (avr.rampz!=0)
                                                    	_avr_invalid_opcode(avr);
                                                    uint z = (uint)(avr.data[R_ZL] | (avr.data[R_ZH] << 8) | (avr.data[avr.rampz] << 16));
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    int op = (int)(opcode & 1);
                                                    STATE(avr,"elpm {0:G}, (Z[{1:X2}:{2:X4}]{3:G})\n", Avr_regname(d), z >> 16, z & 0xffff, op!=0 ? "+" : "");
                                                    _avr_set_r(avr, d, avr.flash[z]);
                                                    if (op!=0)
                                                    {
                                                    	z++;
                                                        _avr_set_r(avr, avr.rampz, z >> 16);
                                                        _avr_set_r16le_hl(avr, R_ZL,(ushort)z);
                                                    }
                                                    cycle += 2; // 3 cycles
                                                }
                                                break;
                                            //						/*
                                            //						 * Load store instructions
                                            //						 *
                                            //						 * 1001 00sr rrrr iioo
                                            //						 * s = 0 = load, 1 = store
                                            //						 * ii = 16 bits register index, 11 = X, 10 = Y, 00 = Z
                                            //						 * oo = 1) post increment, 2) pre-decrement
                                            //						 */
                                            case 0x900c:
                                            case 0x900d:
                                            case 0x900e:
                                                {  // LD -- Load Indirect from Data using X -- 1001 000d dddd 11oo
                                                    int op = (int)(opcode & 3);
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    ushort x = (ushort)((avr.data[R_XH] << 8) | avr.data[R_XL]);
                                                    STATE(avr,"ld {0:G}, {1:G}X[0x{2:X4}]{3:G}\n", Avr_regname(d), op == 2 ? "--" : "", x, op == 1 ? "++" : "");
                                                    cycle++; // 2 cycles (1 for tinyavr, except with inc/dec 2)
                                                    if (op == 2)
                                                        x--;
                                                    byte vd = _avr_get_ram(avr, x);
                                                    if (op == 1)
                                                        x++;
                                                    _avr_set_r16le_hl(avr, R_XL, x);
                                                    _avr_set_r(avr, d, vd);
                                                }
                                                break;
                                            case 0x920c:
                                            case 0x920d:
                                            case 0x920e:
                                                {   // ST -- Store Indirect Data Space X -- 1001 001d dddd 11oo
                                                    int op = (int)(opcode & 3);
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    ushort x = (ushort)((avr.data[R_XH] << 8) | avr.data[R_XL]);
                                                    STATE(avr,"st {0:G}X[{1:X4}]{2:G}, {3:G}[{4:X2}] \n", op == 2 ? "--" : "", x, op == 1 ? "++" : "", Avr_regname(d), vd);
                                                    cycle++; // 2 cycles, except tinyavr
                                                    if (op == 2)
                                                        x--;
                                                    _avr_set_ram(avr, x, vd);
                                                    if (op == 1)
                                                        x++;
                                                    _avr_set_r16le_hl(avr, R_XL, x);
                                                }
                                                break;
                                            case 0x9009:
                                            case 0x900a:
                                                {  // LD -- Load Indirect from Data using Y -- 1001 000d dddd 10oo
                                                    int op = (int)(opcode & 3);
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    ushort y = (ushort)((avr.data[R_YH] << 8) | avr.data[R_YL]);
                                                    STATE(avr,"ld {0:G}, {1:G}Y[{2:X4}]{3:G}\n", Avr_regname(d), op == 2 ? "--" : "", y, op == 1 ? "++" : "");
                                                    cycle++; // 2 cycles, except tinyavr
                                                    if (op == 2)
                                                        y--;
                                                    byte vd = _avr_get_ram(avr, y);
                                                    if (op == 1)
                                                        y++;
                                                    _avr_set_r16le_hl(avr, R_YL, y);
                                                    _avr_set_r(avr, d, vd);
                                                }
                                                break;
                                            case 0x9209:
                                            case 0x920a:
                                                {   // ST -- Store Indirect Data Space Y -- 1001 001d dddd 10oo
                                                    int op = (int)(opcode & 3);
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    ushort y = (ushort)((avr.data[R_YH] << 8) | avr.data[R_YL]);
                                                    STATE(avr,"st {0:G}Y[{1:X4}]{2:G}, {3:G}[{4:X2}]\n", op == 2 ? "--" : "", y, op == 1 ? "++" : "", Avr_regname(d), vd);
                                                    cycle++;
                                                    if (op == 2)
                                                        y--;
                                                    _avr_set_ram(avr, y, vd);
                                                    if (op == 1)
                                                        y++;
                                                    _avr_set_r16le_hl(avr, R_YL, y);
                                                }
                                                break;
                                            case 0x9200:
                                                {   // STS -- Store Direct to Data Space, 32 bits -- 1001 0010 0000 0000
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    ushort x = _avr_flash_read16le(avr, new_pc);
                                                    new_pc += 2;
                                                    STATE(avr,"sts 0x{0:X4}, {1:G}[{2:X2}]\n", x, Avr_regname(d), vd);
                                                    cycle++;
                                                    _avr_set_ram(avr, x, vd);
                                                }
                                                break;
                                            case 0x9001:
                                            case 0x9002:
                                                {  // LD -- Load Indirect from Data using Z -- 1001 000d dddd 00oo
                                                    int op = (int)(opcode & 3);
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    ushort z = (ushort)((avr.data[R_ZH] << 8) | avr.data[R_ZL]);
                                                    STATE(avr,"ld {0:G}, {1:G}Z[{2:X4}]{3:G}\n", Avr_regname(d), op == 2 ? "--" : "", z, op == 1 ? "++" : "");
                                                    cycle++;; // 2 cycles, except tinyavr
                                                    if (op == 2) z--;
                                                    byte vd = _avr_get_ram(avr, z);
                                                    if (op == 1) z++;
                                                    _avr_set_r16le_hl(avr, R_ZL, z);
                                                    _avr_set_r(avr, d, vd);
                                                }
                                                break;
                                            case 0x9201:
                                            case 0x9202:
                                                {   // ST -- Store Indirect Data Space Z -- 1001 001d dddd 00oo
                                                    int op = (int)(opcode & 3);
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    ushort z = (ushort)((avr.data[R_ZH] << 8) | avr.data[R_ZL]);
                                                    STATE(avr,"st {0:G}Z[{1:X4}]{2:G}, {3:G}[{4:X2}] \n", op == 2 ? "--" : "", z, op == 1 ? "++" : "", Avr_regname(d), vd);
                                                    cycle++; // 2 cycles, except tinyavr
                                                    if (op == 2) z--;
                                                    _avr_set_ram(avr, z, vd);
                                                    if (op == 1) z++;
                                                    _avr_set_r16le_hl(avr, R_ZL, z);
                                                }
                                                break;
                                            case 0x900f:
                                                {  // POP -- 1001 000d dddd 1111
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    _avr_set_r(avr, d, _avr_pop8(avr));
                                                    ushort sp = _avr_sp_get(avr);
                                                    STATE(avr,"pop {0:G} (@{1:X4})[{2:X2}]\n", Avr_regname(d), sp, avr.data[sp]);
                                                    cycle++;
                                                }
                                                break;
                                            case 0x920f:
                                                {   // PUSH -- 1001 001d dddd 1111
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    _avr_push8(avr, vd);
                                                    ushort sp = _avr_sp_get(avr);
                                                    STATE(avr,"push {0:G}[{1:X2}] (@{2:X4})\n", Avr_regname(d), vd, sp);
                                                    cycle++;
                                                }
                                                break;
                                            case 0x9400:
                                                {   // COM -- One's Complement -- 1001 010d dddd 0000
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)(0xff - vd);
                                                    STATE(avr,"com {0:G}[{1:X2}] = {2:X2}", Avr_regname(d), vd, res);
                                                    _avr_set_r(avr, d, res);
                                                    _avr_flags_znv0s(avr, res);
                                                    avr.sreg[S_C] = 1;
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x9401:
                                                {   // NEG -- Two's Complement -- 1001 010d dddd 0001
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)(0x00 - vd);
                                                    STATE(avr,"neg {0:G}[{1:X2}] = {2:X2}", Avr_regname(d), vd, res);
                                                    _avr_set_r(avr, d, res);
                                                    avr.sreg[S_H] = (byte)(((res >> 3) | (vd >> 3)) & 1);
                                                    avr.sreg[S_V] = (byte)(res == 0x80?1:0);
                                                    avr.sreg[S_C] = (byte)(res != 0?1:0);
                                                    _avr_flags_zns(avr, res);
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x9402:
                                                {   // SWAP -- Swap Nibbles -- 1001 010d dddd 0010
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)((vd >> 4) | (vd << 4)) ;
                                                    STATE(avr,"swap {0:G}[{1:X2}] = {2:X2}\n", Avr_regname(d), vd, res);
                                                    _avr_set_r(avr, d, res);
                                                }
                                                break;
                                            case 0x9403:
                                                {   // INC -- Increment -- 1001 010d dddd 0011
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)(vd + 1);
                                                    STATE(avr,"inc {0:G}[{1:X2}] = {2:X2}", Avr_regname(d), vd, res);
                                                    _avr_set_r(avr, d, res);
                                                    avr.sreg[S_V] = (byte)(res == 0x80?1:0);
                                                    _avr_flags_zns(avr, res);
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x9405:
                                                {   // ASR -- Arithmetic Shift Right -- 1001 010d dddd 0101
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)((vd >> 1) | (vd & 0x80));
                                                    STATE(avr,"asr {0:G}[{1:X2}]", Avr_regname(d), vd);
                                                    _avr_set_r(avr, d, res);
                                                    _avr_flags_zcnvs(avr, res, vd);
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x9406:
                                                {   // LSR -- Logical Shift Right -- 1001 010d dddd 0110
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)(vd >> 1);
                                                    STATE(avr,"lsr {0:G}[{1:X2}]", Avr_regname(d), vd);
                                                    _avr_set_r(avr, d, res);
                                                    avr.sreg[S_N] = 0;
                                                    _avr_flags_zcvs(avr, res, vd);
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x9407:
                                                {   // ROR -- Rotate Right -- 1001 010d dddd 0111
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)((avr.sreg[S_C]!=0? 0x80 : 0) | vd >> 1);
                                                    STATE(avr,"ror {0:G}[{1:X2}]", Avr_regname(d), vd);
                                                    _avr_set_r(avr, d, res);
                                                    _avr_flags_zcnvs(avr, res, vd);
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x940a:
                                                {   // DEC -- Decrement -- 1001 010d dddd 1010
                                                    byte d = (byte)((opcode >> 4) & 0x1f);
                                                    byte vd = avr.data[d];
                                                    byte res = (byte)(vd - 1);
                                                    STATE(avr,"dec {0:G}[{1:X2}] = {2:X2}", Avr_regname(d), vd, res);
                                                    _avr_set_r(avr, d, res);
                                                    avr.sreg[S_V] = (byte)(res == 0x7f?1:0);
                                                    _avr_flags_zns(avr, res);
                                                    SREG(avr);
                                                }
                                                break;
                                            case 0x940c:
                                            case 0x940d:
                                                {  // JMP -- Long Call to sub, 32 bits -- 1001 010a aaaa 110a
                                                    uint a = ((opcode & 0x01f0) >> 3) | (opcode & 1);
                                                    ushort x = _avr_flash_read16le(avr, new_pc);
                                                    a = (a << 16) | x;
                                                    new_pc = a << 1;
                                                    STATE(avr,"jmp {0:X4}\n", new_pc);
                                                    cycle += 2;
                                                    avr.trace_data.old[avr.trace_data.old_pci].pc = avr.PC;
                                                    avr.trace_data.old[avr.trace_data.old_pci].sp = _avr_sp_get(avr);
                                                    avr.trace_data.old_pci = (avr.trace_data.old_pci + 1) & (Avr_trace_data.OLD_PC_SIZE - 1);
                                                }
                                                break;
                                            case 0x940e:
                                            case 0x940f:
                                                {  // CALL -- Long Call to sub, 32 bits -- 1001 010a aaaa 111a
                                                    uint a = ((opcode & 0x01f0) >> 3) | (opcode & 1);
                                                    ushort x = _avr_flash_read16le(avr, new_pc);
                                                    a = (a << 16) | x;
                                                    new_pc += 2;
                                                    cycle += (ulong)(1 + _avr_push_addr(avr, new_pc));
                                                    new_pc = a << 1;
                                                    STATE(avr, "call {0:X4}\n", new_pc);
                                                    avr.trace_data.old[avr.trace_data.old_pci].pc = avr.PC;
                                                    avr.trace_data.old[avr.trace_data.old_pci].sp = _avr_sp_get(avr);
                                                    avr.trace_data.old_pci = (avr.trace_data.old_pci + 1) & (Avr_trace_data.OLD_PC_SIZE - 1);
                                                    avr.trace_data.stack_frame[avr.trace_data.stack_frame_index].pc = avr.PC;
	                                                avr.trace_data.stack_frame[avr.trace_data.stack_frame_index].sp = _avr_sp_get(avr);
	                                                avr.trace_data.stack_frame_index++;
                                                }
                                                break;

                                            default:
                                                {
                                                    switch (opcode & 0xff00)
                                                    {
                                                        case 0x9600:
                                                            {   // ADIW -- Add Immediate to Word -- 1001 0110 KKpp KKKK
                                                                byte p = (byte)(24 + ((opcode >> 3) & 0x6)); 
                                                                byte k = (byte)(((opcode & 0x00c0) >> 2) | (opcode & 0xf)); 
                                                                ushort vp = (ushort) (avr.data[p] | (avr.data[p + 1] << 8));
                                                                ushort res = (ushort)(vp + k);
                                                                STATE(avr,"adiw {0:G}:{1:G}[{2:X4}], 0x{3:X2}", Avr_regname(p), Avr_regname((byte)(p + 1)), vp, k);
                                                                _avr_set_r16le_hl(avr, p, res);
                                                                avr.sreg[S_V] = (byte)(((~vp & res) >> 15) & 1);
                                                                avr.sreg[S_C] = (byte)(((~res & vp) >> 15) & 1);
                                                                _avr_flags_zns16(avr, res);
                                                                SREG(avr);
                                                                cycle++;
                                                            }
                                                            break;
                                                        case 0x9700:
                                                            {   // SBIW -- Subtract Immediate from Word -- 1001 0111 KKpp KKKK
                                                                byte p = (byte)(24 + ((opcode >> 3) & 0x6)); 
                                                                byte k = (byte)(((opcode & 0x00c0) >> 2) | (opcode & 0xf)); 
                                                                ushort vp = (ushort)(avr.data[p] | (avr.data[p + 1] << 8));
                                                                ushort res = (ushort)(vp - k);
                                                                STATE(avr,"sbiw {0:G}:{1:G}[{2:X4}], 0x{3:X2}\n", Avr_regname(p), Avr_regname((byte)(p + 1)), vp, k);
                                                                _avr_set_r16le_hl(avr, p, res);
                                                                avr.sreg[S_V] = (byte)(((vp & ~res) >> 15) & 1);
                                                                avr.sreg[S_C] = (byte)(((res & ~vp) >> 15) & 1);
                                                                _avr_flags_zns16(avr, res);
                                                                SREG(avr);
                                                                cycle++;
                                                            }
                                                            break;
                                                        case 0x9800:
                                                            {   // CBI -- Clear Bit in I/O Register -- 1001 1000 AAAA Abbb
                                                                byte io = (byte)(((opcode >> 3) & 0x1f) + 32);
                                                                byte mask = (byte)(1 << (byte)(opcode & 0x7));
                                                                byte res = (byte)(_avr_get_ram(avr, io) & ~mask);
                                                                STATE(avr,"cbi {0:G}[{1:X4}], 0x{2:X2} = {3:X2}\n", Avr_regname(io), avr.data[io], mask, res);
                                                                _avr_set_ram(avr, io, res);
                                                                cycle++;
                                                            }
                                                            break;
                                                        case 0x9900:
                                                            {   // SBIC -- Skip if Bit in I/O Register is Cleared -- 1001 1001 AAAA Abbb
                                                                byte io = (byte)(((opcode >> 3) & 0x1f) + 32);
                                                                byte mask = (byte)(1 << (byte)(opcode & 0x7));
                                                                byte res = (byte)(_avr_get_ram(avr, io) & mask);
                                                                STATE(avr,"sbic {0:G}[{1:X4}], 0x{2:X2}\t; Will{3:G} branch\n", Avr_regname(io), avr.data[io], mask, res!=0?"":" not");
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
                                                            }
                                                            break;
                                                        case 0x9a00:
                                                            {   // SBI -- Set Bit in I/O Register -- 1001 1010 AAAA Abbb
                                                                byte io = (byte)(((opcode >> 3) & 0x1f) + 32);
                                                                byte mask = (byte)(1 << (byte)(opcode & 0x7));

                                                                byte res = (byte)(_avr_get_ram(avr, io) | mask);
                                                                STATE(avr,"sbi {0:G}[{1:X4}], 0x{2:X2} = {3:X2}\n", Avr_regname(io), avr.data[io], mask, res);
                                                                _avr_set_ram(avr, io, res);
                                                                cycle++;
                                                            }
                                                            break;
                                                        case 0x9b00:
                                                            {   // SBIS -- Skip if Bit in I/O Register is Set -- 1001 1011 AAAA Abbb
                                                                byte io = (byte)(((opcode >> 3) & 0x1f) + 32);
                                                                byte mask = (byte)(1 << (byte)(opcode & 0x7));
                                                                byte res = (byte)(_avr_get_ram(avr, io) & mask);
                                                                STATE(avr,"sbis {0:G}[{1:X4}], 0x{2:X2}\t; Will{3:G} branch\n", Avr_regname(io), avr.data[io], mask, res!=0?"":" not");
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
                                                            }
                                                            break;
                                                        default:
                                                            switch (opcode & 0xfc00)
                                                            {
                                                                case 0x9c00:
                                                                    {   // MUL -- Multiply Unsigned -- 1001 11rd dddd rrrr
                                                                        byte r = (byte)(((opcode >> 5) & 0x10) | (opcode & 0xf)); 
                                                                        byte d = (byte)((opcode >> 4) & 0x1f);
                                                                        byte vd = avr.data[d];
                                                                        byte vr = avr.data[r];
                                                                        ushort res = (ushort)(vd * vr);
                                                                        STATE(avr,"mul {0:G}[{1:X2}], {2:G}[{3:X2}] = {4:X4}", Avr_regname(d), vd, Avr_regname(r), vr, res);
                                                                        cycle++;
                                                                        _avr_set_r16le(avr, 0, res);
                                                                        avr.sreg[S_Z] = (byte)(res == 0?1:0);
                                                                        avr.sreg[S_C] = (byte)((res >> 15) & 1);
                                                                        SREG(avr);
                                                                    }
                                                                    break;
                                                                default: _avr_invalid_opcode(avr); break;
                                                            }; break;
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case 0xb000:
                    {
                        switch (opcode & 0xf800)
                        {
                            case 0xb800: {	// OUT A,Rr -- 1011 1AAd dddd AAAA
                                byte d = (byte)((opcode >> 4) & 0x1f); 
                                byte A = (byte)(((((opcode >> 9) & 3) << 4) | ((opcode) & 0xf)) + 32);
                        		STATE(avr,"out {0:G}, {1:G}[{2:X2}]\n", Avr_regname(A), Avr_regname(d), avr.data[d]);
                        		_avr_set_ram(avr, A, avr.data[d]);
                        	}	break;
                            case 0xb000: {	// IN Rd,A -- 1011 0AAd dddd AAAA
                        	    byte d = (byte)((opcode >> 4) & 0x1f); 
                                byte A = (byte)(((((opcode >> 9) & 3) << 4) | ((opcode) & 0xf)) + 32);
                                STATE(avr,"in {0:G}, {1:G}[{2:X2}]\n", Avr_regname(d), Avr_regname(A), avr.data[A]);
                                _avr_set_r(avr, d, _avr_get_ram(avr, A));
                            }	break;
                            default: _avr_invalid_opcode(avr);break;
                        }
                    }
                    break;

                case 0xc000:
                    {  // RJMP -- 1100 kkkk kkkk kkkk
                        int o = (int)(((ushort)((opcode << 4) & 0xffff)) >> 4); // offset
                        if (o >= 2048)
                            o = (4096 - o) * -1;
                       STATE(avr,"rjmp .{0:G} [{1:X4}]\n", (o << 1), new_pc + (o<<1));
                       new_pc = (uint)(new_pc + (o<<1)) % (avr.flashend+1);
                       cycle++;
                       avr.trace_data.old[avr.trace_data.old_pci].pc = avr.PC;
                       avr.trace_data.old[avr.trace_data.old_pci].sp = _avr_sp_get(avr);
                       avr.trace_data.old_pci = (avr.trace_data.old_pci + 1) & (Avr_trace_data.OLD_PC_SIZE-1);
                    }
                    break;

                case 0xd000:
                    {  // RCALL -- 1101 kkkk kkkk kkkk
                        ushort o = (ushort)(((ushort)((opcode << 4) & 0xffff)) >> 3);
                        STATE(avr,"rcall .{0:G} [{1:X4}]\n", o >> 1, new_pc + o);
                        cycle += (ulong)_avr_push_addr(avr, new_pc);
                        new_pc = (new_pc + o) % (avr.flashend+1);
                        // 'rcall .1' is used as a cheap "push 16 bits of room on the stack"
                        if (o != 0)
                        {
                            avr.trace_data.old[avr.trace_data.old_pci].pc = avr.PC;
                            avr.trace_data.old[avr.trace_data.old_pci].sp = _avr_sp_get(avr);
                            avr.trace_data.old_pci = (avr.trace_data.old_pci + 1) & (Avr_trace_data.OLD_PC_SIZE -1);

                            avr.trace_data.stack_frame[avr.trace_data.stack_frame_index].pc = avr.PC;
	                        avr.trace_data.stack_frame[avr.trace_data.stack_frame_index].sp = _avr_sp_get(avr);
	                        avr.trace_data.stack_frame_index++;
                        }
                    }
                    break;

                case 0xe000:
                    {  // LDI Rd, K aka SER (LDI r, 0xff) -- 1110 kkkk dddd kkkk
                       byte h = (byte)(16 + ((opcode >> 4) & 0xf)); 
                       byte k = (byte)(((opcode & 0x0f00) >> 4) | (opcode & 0xf));
                       STATE(avr,"ldi {0:G}, 0x{1:X2}\n", Avr_regname(h), k);
                       _avr_set_r(avr, h, k);
                    }
                    break;

                case 0xf000:
                    {
                        switch (opcode & 0xfe00)
                        {
                            case 0xf000:
                            case 0xf200:
                            case 0xf400:
                            case 0xf600: {	// BRXC/BRXS -- All the SREG branches -- 1111 0Boo oooo osss
                                int o = (((ushort)(opcode << 6)) >> 9); // offset
                                if (o >= 64) // TYV negative offset
                                    o = (128 - o)*-1;

                                    byte s = (byte)(opcode & 7);
                                int set = (int)((opcode & 0x0400) == 0?1:0);		// this bit means BRXC otherwise BRXS
                                int branch = (int)((((avr.sreg[s]==1) && (set!=0)) || (avr.sreg[s]!=1 && set==0))?1:0);
                                string[,] names = new string[2,8]{
                                    { "brcc", "brne", "brpl", "brvc", null, "brhc", "brtc", "brid"},
                                    { "brcs", "breq", "brmi", "brvs", null, "brhs", "brts", "brie"}};


                                    if (names[set, s] != null)
                                        STATE(avr, "{0:G} .{1:G} [{2:X4}]\t; Will{3:G} branch\n", names[set, s], o << 1, new_pc + (o << 1), branch != 0 ? "" : " not");
                                    else
                                        STATE(avr, "{0:G}{1:G} .{2:G} [{3:X4}]\t; Will{4:G} branch\n", set != 0 ? "brbs" : "brbc", _sreg_bit_name[s], o, new_pc + (o << 1), branch != 0 ? "" : " not");

                                    if (branch!=0) 
                                {
                                    cycle++; // 2 cycles if taken, 1 otherwise
                                    new_pc = (uint)(new_pc + (o << 1));
                                }
                                }
                                break;
                            case 0xf800:
                            case 0xf900: {	// BLD -- Bit Store from T into a Bit in Register -- 1111 100d dddd 0bbb
                                byte d = (byte)((opcode >> 4) & 0x1f); 
                                byte vd = avr.data[d];
                                byte s = (byte)(opcode & 7);
                                byte mask = (byte)(1 << s);
                                byte v = (byte)((vd & ~mask) | (avr.sreg[S_T]!=0 ? mask : 0));
                                STATE(avr,"bld {0:G}[{1:X2}], 0x{2:X2} = {3:X2}\n", Avr_regname(d), vd, mask, v);
                                _avr_set_r(avr, d, v);
                            }	break;
                            case 0xfa00:
                            case 0xfb00:{	// BST -- Bit Store into T from bit in Register -- 1111 101d dddd 0bbb
                                byte d = (byte)((opcode >> 4) & 0x1f); 
                                byte vd = avr.data[d];
                                byte s = (byte)(opcode & 7);
                                STATE(avr,"bst {0:G}[{1:X2}], 0x{3:X2}", Avr_regname(d), vd, 1 << s);
                                avr.sreg[S_T] = (byte)((vd >> s) & 1);
                                SREG(avr);
                            }	break;
                            case 0xfc00:
                            case 0xfe00: {	// SBRS/SBRC -- Skip if Bit in Register is Set/Clear -- 1111 11sd dddd 0bbb
                                byte d = (byte)((opcode >> 4) & 0x1f); 
                                byte vd = avr.data[d];
                                byte s = (byte)(opcode & 7);
                                byte mask = (byte)(1 << s);
                                int set = (opcode & 0x0200) != 0?1:0;
                                int branch = (((vd & mask)!=0 && set!=0) || ((vd & mask)==0 && set==0))?1:0;
                                STATE(avr,"{0:G} {1:G}[0x{2:X2}], 0x{3:X2}\t; Will{4:G} branch\n", set!=0 ? "sbrs" : "sbrc", Avr_regname(d), vd, mask, branch!=0 ? "":" not");
                                if (branch!=0)
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
                            }	break;
                            default: _avr_invalid_opcode(avr); break;
                        }
                    }
                    break;

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