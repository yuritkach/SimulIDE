using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avr
{
    [Flags]
    public enum StatusRegister
    {
        C = 1,
        Z = 2,
        N = 4,
        V = 8,
        S = 16,
        H = 32,
        T = 64,
        I = 128
    }

    public class FlashProgrammMemory
    {
        public FlashProgrammMemory()
        {
            data = new byte[0x3FFF];
        }

        public static byte[] GetData(UInt16 address)
        {
            return new byte[2] { data[address], data[address + 1] };
        }

        protected static byte[] data;
    }

    public class ALU
    {
        public ALU()
        {
            PC = 0;
            ClockCounter = 0;
        }

        public void DoClock()
        {
            GetCommand();
            ExecuteCommand();
            ClockCounter++;
        }

        protected void GetCommand()
        {
            byte[] command = FlashProgrammMemory.GetData(PC);
        }

        protected void ExecuteCommand()
        {
            byte[] command = FlashProgrammMemory.GetData(PC);
        }


        protected UInt16 PC;
        protected UInt64 ClockCounter;
        protected StatusRegister SREG;
    }


}
