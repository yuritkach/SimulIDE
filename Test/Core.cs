using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avr
{
    public class StatusReg
    {
        public bool C { get; set; } // Carry Flag
        public bool Z { get; set; } // Zero Flag
        public bool N { get; set; } // Negative Flag
        public bool V { get; set; } // Two’s complement overflow indicator
        public bool S { get; set; } // N ⊕ V, for signed tests (xor)
        public bool H { get; set; } // Half Carry Flag
        public bool T { get; set; } // Transfer bit used by BLD and BST instructions
        public bool I { get; set; } // Global Interrupt Enable/Disable Flag

        public bool CalcH(byte rr, byte rd, byte r)
        {
            bool rd3 = (rd & 0b00001000) == 0b00001000;
            bool rr3 = (rr & 0b00001000) == 0b00001000;
            bool r3 = (r & 0b00001000) == 0b00001000;
            //Rd3 • Rr3 + Rr3 • !R3 + !R3 • Rd3
            return (rd3 && rr3) || (rr3&&!r3) || (rd3&&!r3);
        }

        public bool CalcV(byte rr, byte rd, byte r)
        {
            bool rd7 = (rd & 0b10000000) == 0b10000000;
            bool rr7 = (rr & 0b10000000) == 0b10000000;
            bool r7 = (r & 0b10000000) == 0b10000000;
            //Rd7 • Rr7 • !R7 + !Rd7 • !Rr7 • R7
            return (rd7 && rr7 && !r7) || (!rr7 && !rd7 && r7);
        }

        public bool CalcC(byte rr, byte rd, byte r)
        {
            bool rd7 = (rd & 0b10000000) == 0b10000000;
            bool rr7 = (rr & 0b10000000) == 0b10000000;
            bool r7 = (r & 0b10000000) == 0b10000000;
            // Rd7 • Rr7 + Rr7 • !R7 + !R7 • Rd7
            return (rd7 && rr7) || (rr7 && !r7) || (rd7 && !r7);
        }

        

    }

    public class FlashProgrammMemory
    {
        public FlashProgrammMemory()
        {
            data = new byte[0x3FFF];
        }

        public ushort GetData(uint address)
        {
            return (ushort) (data[address + 1] * 256 + data[address]); 
        }

        protected static byte[] data;
    }

    public class DataMemory
    {
        public DataMemory()
        {
            data = new byte[0x08FF];
        }
        protected byte[] data;

        public byte GetByteByOffset(uint offset)
        {
            return data[offset];
        }

        public void SetByteByOffset(uint offset,byte value)
        {
            data[offset]= value;
        }

    }


    public class Core
    {
        public Core(MCU mcu)
        {
            this.mcu = mcu;
            commandExecutor = new CommandExecutor(mcu);
        }

        public void ExecuteCommand(ushort command)
        {
            commandExecutor.Execute(command);
        }

        protected CommandExecutor commandExecutor;
        protected MCU mcu;

    }

    public class MCU
    {
        public MCU()
        {
            PC = 0;
            ClockCounter = 0;
            SREG = new StatusReg();
            core = new Core(this);
            ProgramMemory = new FlashProgrammMemory();
            DataMemory = new DataMemory();
        }

        public void DoClock()
        {
            // Внимание! Конвеер. на одном такте идет выборка команды и исполнение предыдущей выбраной команды
            var command = ProgramMemory.GetData(PC);
            core.ExecuteCommand(command);
        }

        protected Core core;
        
        
        public uint PC { get; set; }
        public UInt64 ClockCounter { get; set; }
        public StatusReg SREG { get; set; }
        public DataMemory DataMemory { get; set; }
        public FlashProgrammMemory ProgramMemory { get; set; }

    }


}
