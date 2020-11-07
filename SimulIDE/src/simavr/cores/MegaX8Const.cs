using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class MegaX8Const:McuBaseConst
    {
        public override ushort GetRamStart() { }
        public override ushort GetRamEnd() { }
        public override uint GetFlashEnd() { }
        public override uint GetE2End() { }
        public override byte[] GetFuse() { }
        public override byte[] GetSignature() { }
        public override byte GetLockBits() { }
        public override ResetFlags GetResetFlags() { }
        public override byte Get_SIM_VECTOR_SIZE()
        {

        }

    }
}
