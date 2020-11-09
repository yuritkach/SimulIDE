using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class Sim_core_declare
    {

        public virtual void DefaultCore(Mcu mcu, byte vectorSize)
        {
          
            if (mcu.Get__SIM_CORE_DECLARE_H__())
            {
                mcu.core.ioend = (ushort)(mcu.GetRamStart() - 1);
                mcu.core.ramend = mcu.GetRamEnd();
                mcu.core.flashend = mcu.GetFlashEnd();
                mcu.core.e2end = mcu.GetE2End();
                mcu.core.vector_size = vectorSize;

                if (mcu.GetSignature_0() != 0)
                {
                    mcu.core.fuse = mcu.GetFuse();
                    mcu.core.signature = mcu.GetSignature();
                    mcu.core.lockbits = mcu.GetLockBits();
                    mcu.core.reset_flags = mcu.GetResetFlags();
                }
            }
        }

    }
}
