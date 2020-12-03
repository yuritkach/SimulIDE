using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avr
{
    public class CommandExecutor
    {
        public CommandExecutor(MCU mcu)
        {
            this.mcu = mcu;
            availableCommands = new List<BaseCommand>()
            {
                new ADCCommand(mcu)
            };

        }

        

        public void Execute(ushort command)
        {
            foreach (var el in availableCommands)
            {
                if (el.ItsMe(command))
                {
                    el.Execute(mcu,command);
                    break;
                }
            }
        }

        protected List<BaseCommand> availableCommands;
        protected MCU mcu;

    }

    public class BaseCommand
    {
        public BaseCommand(MCU mcu)
        {
            this.mcu = mcu;
        }

        public virtual bool ItsMe(ushort word) { return false; }
        public virtual void Execute(MCU mcu, ushort command) { }
        public virtual string Disasemble() { return null; }

        protected byte GetRegistrIndexOnBits(int i5, int i4, int i3, int i2, int i1)
        {
            return (byte)((1 << i1) | (1 << i2) | (1 << i3) | (1 << i4) | (1 << i5));
        }

        protected byte GetByteOnRegistrIndex(byte index)
        {
            return mcu.DataMemory.GetByteByOffset(index);
        }

        protected MCU mcu;

    }

    public class ADCCommand : BaseCommand
    {
        public ADCCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "ADC R"+rdindex.ToString()+", R"+rrindex.ToString() ; 
        }

        public override void Execute(MCU mcu, ushort command)
        {
            rrindex = GetRegistrIndexOnBits(9,3,2,1,0);
            rdindex = GetRegistrIndexOnBits(8,7,6,5,4);
            byte rr = GetByteOnRegistrIndex(rrindex);
            byte rd = GetByteOnRegistrIndex(rdindex);
            byte r = (byte)(rr + rd+(mcu.SREG.C?1:0));

            mcu.SREG.H = mcu.SREG.CalcH(rr, rd, r);
            mcu.SREG.V = mcu.SREG.CalcV(rr, rd, r);
            mcu.SREG.N = (r & 0b10000000) == 0b10000000;
            mcu.SREG.S = mcu.SREG.V ^ mcu.SREG.N;
            mcu.SREG.Z = r == 0;
            mcu.SREG.C = mcu.SREG.CalcC(rr, rd, r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return (command & 0b0001110000000000) == 0b0001110000000000;
        }

        private byte rrindex;
        private byte rdindex;

    }


    public class NOPCommand : BaseCommand
    {
        public NOPCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "NOP";
        }

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return command == 0b0000000000000000;
        }

    }


}
