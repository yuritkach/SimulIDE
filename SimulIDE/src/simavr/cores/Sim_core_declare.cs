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
                mcu.core.ioend = (ushort)(mcu.GetValue("RAMSTART") - 1);
                mcu.core.ramend = mcu.GetValue("RAMEND");
                mcu.core.flashend = mcu.GetValue("FLASHEND");
                mcu.core.e2end = mcu.GetValue("E2END");
                mcu.core.vector_size = vectorSize;

                if (mcu.GetValue("Signature_0")!=0)
                {
                    mcu.core.fuse = mcu.GetFuse();
                    mcu.core.signature = mcu.GetValue("SIGNATURE");
                    mcu.core.lockbits = mcu.GetValue("LOCKBITS");
                    mcu.core.reset_flags = mcu.GetValue("RESETFLAGS");
                }
            }
        }

    }
}
