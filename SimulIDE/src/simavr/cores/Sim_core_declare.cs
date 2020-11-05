using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class Sim_core_declare
    {

        public virtual void DefaultCore(ref Avr core, byte vectorSize)
        {
            var con = GetConstants();
            if (con.Get__SIM_CORE_DECLARE_H__())
            {
                core.ioend = (ushort)(con.GetRamStart() - 1);
                core.ramend = con.GetRamEnd();
                core.flashend = con.GetFlashEnd();
                core.e2end = con.GetE2End();
                core.vector_size = vectorSize;

                if (con.GetSignature_0() != 0)
                {
                    core.fuse = con.GetFuse();
                    core.signature = con.GetSignature();
                    core.lockbits = con.GetLockBits();
                    core.reset_flags = con.GetResetFlags();
                }
            }
        }

        public virtual McuBaseConst GetConstants()
        {
            return new McuBaseConst();
        }

    }
}
