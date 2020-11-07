using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class AbstractException : ApplicationException
    {
        public override string Message => "Not overriden method! "+base.Message;
    }

    public class McuBaseConst
    {
        public virtual ushort GetRamStart() { throw new AbstractException(); }
        public virtual ushort GetRamEnd() { throw new AbstractException(); }
        public virtual ushort GetE2END() { throw new AbstractException(); }
        public virtual byte GetEEARH() { throw new AbstractException(); }
        public virtual byte GetEEARL() { throw new AbstractException(); }
        public virtual byte GetEEDR() { throw new AbstractException(); }
        public virtual byte GetEECR() { throw new AbstractException(); }
        public virtual byte GetEEPM0() { throw new AbstractException(); }
        public virtual byte GetEEPM1() { throw new AbstractException(); }
        public virtual byte GetEEMPE() { throw new AbstractException(); }
        public virtual byte GetEEPE() { throw new AbstractException(); }
        public virtual byte GetEERE() { throw new AbstractException(); }
        public virtual byte GetEERIE() { throw new AbstractException(); }

        public virtual uint GetFlashEnd() { throw new AbstractException(); }
        public virtual uint GetE2End() { throw new AbstractException(); }
        public virtual byte GetFuseMemorySize() { throw new AbstractException(); }
        public virtual byte GetFUSE_DEFAULT() { throw new AbstractException(); }
        public virtual byte GetLFUSE_DEFAULT() { throw new AbstractException(); }
        public virtual byte GetHFUSE_DEFAULT() { throw new AbstractException(); }
        public virtual byte GetEFUSE_DEFAULT() { throw new AbstractException(); }
        public virtual byte[] GetFuse()
        {
            switch (GetFuseMemorySize())
            {
                case 6: return new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                case 3: return new byte[] { GetLFUSE_DEFAULT(), GetHFUSE_DEFAULT(), GetEFUSE_DEFAULT() };
                case 2: return new byte[] { GetLFUSE_DEFAULT(), GetHFUSE_DEFAULT() };
                case 1: return new byte[] { GetFUSE_DEFAULT()};
                default: return new byte[] { 0 };
            }
        }
        public virtual byte GetSignature_0() { return 0; }
        public virtual byte GetSignature_1() { return 0; }
        public virtual byte GetSignature_2() { return 0; }
        public virtual byte[] GetSignature()
        {
            return new byte[3] { GetSignature_0(), GetSignature_1(), GetSignature_2() };
        }
        public virtual byte Get_SIM_VECTOR_SIZE() { throw new AbstractException(); }
        public virtual byte GetLockBits() { return 0xFF; }

        public virtual int GetMCU_STATUS_REG()
        {
            if (GetMCUSR()!=-1)
                return GetMCUSR();
            else return GetMCUCSR();
        }

        public virtual int GetMCUSR() { return -1; }
        public virtual int GetMCUCSR() { return -1; }
        public virtual byte GetPORF() { throw new AbstractException(); }
        public virtual byte GetEXTRF() { throw new AbstractException(); }
        public virtual byte GetBORF() { throw new AbstractException(); }
        public virtual byte GetWDRF() { throw new AbstractException(); }

        public virtual ResetFlags GetResetFlags()
        {
            ResetFlags result = new ResetFlags();
            result.porf = Sim_regbit.AVR_IO_REGBIT(GetMCU_STATUS_REG(), GetPORF());
            result.extrf = Sim_regbit.AVR_IO_REGBIT(GetMCU_STATUS_REG(), GetEXTRF());
            result.borf = Sim_regbit.AVR_IO_REGBIT(GetMCU_STATUS_REG(), GetBORF());
            result.wdrf = Sim_regbit.AVR_IO_REGBIT(GetMCU_STATUS_REG(), GetWDRF());
            return result;
        }

        public virtual bool Get__SIM_CORE_DECLARE_H__() { return true; }
        /* we have to declare this, as none of the distro but debian has a modern
         * toolchain and avr-libc. This affects a lot of names, like MCUSR etc
        */
        public virtual bool Get__AVR_LIBC_DEPRECATED_ENABLE__() { return true; }

        //not implemented
        //#define _SFR_IO8(v) ((v)+32)
        //#define _SFR_IO16(v) ((v)+32)
        //#define _SFR_MEM8(v) (v)
        //#define _BV(v) (v)
        //#define _VECTOR(v) (v)


    }
}
