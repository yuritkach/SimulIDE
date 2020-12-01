using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class Mcu
    {
        protected Dictionary<string, object> constants ;

        public Avr core;
        public Avr_eeprom eeprom;

        public Mcu()
        {
            constants = new Dictionary<string, object>();
            InitConstants();
        }

        protected virtual void InitConstants()
        {
            SIGNATURE_0 = 0;
            SIGNATURE_1 = 0;
            SIGNATURE_2 = 0;
            LOCKBITS = 0xFF;
        }

        public byte[] FUSE
        {
            get
            {
                switch (FUSE_MEMORY_SIZE)
                {
                    case 6: return new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                    case 3: return new byte[] { LFUSE_DEFAULT, HFUSE_DEFAULT, EFUSE_DEFAULT };
                    case 2: return new byte[] { LFUSE_DEFAULT, HFUSE_DEFAULT };
                    case 1: return new byte[] { FUSE_DEFAULT };
                    default: return new byte[] { 0 };
                }
            }
        }
        public byte[] SIGNATURE
        {
            get
            {
                return new byte[3] { SIGNATURE_0, SIGNATURE_1, SIGNATURE_2 };
            }
        }
        
        public int MCU_STATUS_REG
        {
            get
            {
                return (int) (MCUSR != null ? MCUSR : MCUCSR);
            }
        } 
        
        public ResetFlags RESETFLAGS
        {
            get
            {
                ResetFlags result = new ResetFlags();
                result.porf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, PORF);
                result.extrf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, EXTRF);
                result.borf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, BORF);
                result.wdrf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, WDRF);
                return result;
            }
        }

        public virtual byte _SFR_IO8(byte param)
        {
            return (byte) (param + 32);
        }

        public virtual ushort _SFR_IO16(ushort param)
        {
            return (ushort)(param+32);
        }

        public virtual byte _SFR_MEM8(byte param)
        {
            return param;
        }

        public virtual ushort _SFR_MEM16(ushort param)
        {
            return param;
        }

        public virtual byte _VECTOR(byte param)
        {
            return param;
        }

        public virtual byte _BV(byte param)
        {
            return param;
        }

        public virtual void DefaultCore(byte vectorSize)
        {

            if (__SIM_CORE_DECLARE_H__!=null)
            {
                core.ioend = (ushort)(RAMSTART - 1);
                core.ramend = RAMEND;
                core.flashend = FLASHEND;
                core.e2end = E2END;
                core.vector_size = vectorSize;

                if (SIGNATURE_0 != 0)
                {
                    core.fuse = FUSE;
                    core.signature = SIGNATURE;
                    core.lockbits = LOCKBITS;
                    core.reset_flags = RESETFLAGS;
                }
            }
        }

        public virtual bool Get__SIM_CORE_DECLARE_H__() { return true; }
        /* we have to declare this, as none of the distro but debian has a modern
         * toolchain and avr-libc. This affects a lot of names, like MCUSR etc
        */
        public virtual bool Get__AVR_LIBC_DEPRECATED_ENABLE__() { return true; }
        public byte EE_READY_vect;

        public byte SIGNATURE_0;
        public byte SIGNATURE_1;
        public byte SIGNATURE_2;
        public byte LOCKBITS;
        public byte LFUSE_DEFAULT;
        public byte HFUSE_DEFAULT;
        public byte EFUSE_DEFAULT;
        public byte FUSE_DEFAULT;
        public int? MCUSR = null;
        public int? MCUCSR = null;
        public byte PORF;
        public byte EXTRF;
        public byte BORF;
        public byte WDRF;
        public bool? __SIM_CORE_DECLARE_H__ = null;
        public ushort SPM_PAGESIZE;
        public ushort RAMSTART;
        public ushort RAMEND;     /* Last On-Chip SRAM Location */
        public ushort XRAMSIZE;
        public ushort XRAMEND;
        public ushort E2END;
        public ushort E2PAGESIZE;
        public ushort FLASHEND;
        public byte FUSE_MEMORY_SIZE;

        public byte EEARH;
        public byte EEARL;
        public byte EEDR;
        public byte EECR;
        public byte EERE;
        public byte EEPE;
        public byte EEMPE;
        public byte EERIE;
        public byte EEPM0;
        public byte EEPM1;

    }
}
