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
                new ADCCommand(mcu),
                new ADDCommand(mcu),

                //new ADIWCommand(mcu),
                //new ANDCommand(mcu),
                //new ANDICommand(mcu),
                //new ASRCommand(mcu),
                //new BCLRCommand(mcu),
                //new BLDCommand(mcu),
                //new BRBCCommand(mcu),
                //new BRBSCommand(mcu),
                //new BRCCCommand(mcu),
                //new BRCSCommand(mcu),
                //new BREAKCommand(mcu),
                //new BREQCommand(mcu),
                //new BRGECommand(mcu),
                //new BRHCCommand(mcu),
                //new BRHSCommand(mcu),
                //new BRIDCommand(mcu),
                //new BRIECommand(mcu),
                //new BRLOCommand(mcu),
                //new BRLTCommand(mcu),
                //new BRMICommand(mcu),
                //new BRNECommand(mcu),
                //new BRPLCommand(mcu),
                //new BRSHCommand(mcu),
                //new BRTCCommand(mcu),
                //new BRTSCommand(mcu),
                //new BRVCCommand(mcu),
                //new BRVSCommand(mcu),
                //new BSETCommand(mcu),
                //new BSTCommand(mcu),
                //new CALLCommand(mcu),
                //new CBICommand(mcu),
                //new CBRCommand(mcu),
                //new CLCCommand(mcu),
                //new CLHCommand(mcu),
                //new CLICommand(mcu),
                //new CLNCommand(mcu),
                //new CLRCommand(mcu),
                //new CLSCommand(mcu),
                //new CLTCommand(mcu),
                //new CLVCommand(mcu),
                //new CLZCommand(mcu),
                //new CPCommand(mcu),
                //new CPCCommand(mcu),
                //new CPICommand(mcu),
                //new CPSECommand(mcu),
                //new DECCommand(mcu),
                //new DESCommand(mcu),
                //new EICALLCommand(mcu),
                //new EIJMPCommand(mcu),
                //new ELPMCommand(mcu),
                //new EORCommand(mcu),
                //new FMULCommand(mcu),
                //new FMULSCommand(mcu),
                //new FMULSUCommand(mcu),
                //new ICALLCommand(mcu),
                //new IJMPCommand(mcu),
                //new INCommand(mcu),
                //new INCCommand(mcu),
                new JMPCommand(mcu),
                //new LACCommand(mcu),
                //new LASCommand(mcu),
                //new LATCommand(mcu),
                //new LDCommand(mcu),
                //new LDLDDCommand(mcu),
                //new LDLDDZCommand(mcu),
                //new LDICommand(mcu),
                //new LDSCommand(mcu),
                //new LDS16Command(mcu),
                //new LPMCommand(mcu),
                //new LSLCommand(mcu),
                //new LSRCommand(mcu),
                //new MOVCommand(mcu),
                //new MOVWCommand(mcu),
                //new MULCommand(mcu),
                //new MULSCommand(mcu),
                //new MULSUCommand(mcu),
                //new NEGCommand(mcu),
                new NOPCommand(mcu),
                //new ORCommand(mcu),
                //new ORICommand(mcu),
                //new OUTCommand(mcu),
                //new POPCommand(mcu),
                //new PUSHCommand(mcu),
                //new RCALLCommand(mcu),
                //new RETCommand(mcu),
                //new RETICommand(mcu),
                //new RJMPCommand(mcu),
                //new ROLCommand(mcu),
                //new RORCommand(mcu),
                //new SBCCommand(mcu),
                //new SBCICommand(mcu),
                //new SBICommand(mcu),
                //new SBICCommand(mcu),
                //new SBISCommand(mcu),
                //new SBIWCommand(mcu),
                //new SBRCommand(mcu),
                //new SBRCCommand(mcu),
                //new SBRSCommand(mcu),
                new SECCommand(mcu),
                new SEHCommand(mcu),
                new SEICommand(mcu),
                new SENCommand(mcu),
                new SERCommand(mcu),
                new SESCommand(mcu),
                //new SETCommand(mcu),
                //new SEVCommand(mcu),
                //new SEZCommand(mcu),
                //new SLEEPCommand(mcu),
                //new SPMCommand(mcu),
                //new SPM2Command(mcu),
                //new STCommand(mcu),
                //new STSTDCommand(mcu),
                //new STSTDZCommand(mcu),
                //new STSCommand(mcu),
                //new STS16Command(mcu),
                //new SUBCommand(mcu),
                //new SUBICommand(mcu),
                //new SWAPCommand(mcu),
                //new TSTCommand(mcu),
                //new WDRCommand(mcu),
                new XCHCommand(mcu)
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

        protected uint GetValueOnCommandMask(uint command, uint bitmask)
        {
            int idx = 0;
            uint res = 0;
        
            for (int i = 0; i < 16; i++)
            {
                uint v = (uint)(1 << i);
                if ((bitmask & v) == v)
                {
                    if ((command&v)==v)
                        res = res + (uint)(1<<idx);
                    idx++;
                }
            }
            return res;
        }

        protected byte GetDataMemoryByteOnRegistrIndex(byte index)
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
            return "ADC R"+rdindex.ToString("x2")+", R"+rrindex.ToString("x2") ; 
        }

        public override void Execute(MCU mcu, ushort command)
        {
            rrindex = (byte) GetValueOnCommandMask(command,0b0000001000001111);
            rdindex = (byte) GetValueOnCommandMask(command,0b0000000111110000);
            byte rr = GetDataMemoryByteOnRegistrIndex(rrindex);
            byte rd = GetDataMemoryByteOnRegistrIndex(rdindex);
            byte r = (byte)(rr + rd+(mcu.SREG.C?1:0));

            mcu.SREG.H = mcu.SREG.CalcH(rr, rd, r);
            mcu.SREG.V = mcu.SREG.CalcV(rr, rd, r);
            mcu.SREG.N = (r & 0b10000000) == 0b10000000;
            mcu.SREG.S = mcu.SREG.V ^ mcu.SREG.N;
            mcu.SREG.Z = r == 0;
            mcu.SREG.C = mcu.SREG.CalcC(rr, rd, r);

            mcu.DataMemory.SetByteByOffset(rdindex,rd);

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

    public class ADDCommand : BaseCommand
    {
        public ADDCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "ADD R" + rdindex.ToString("x2")+ ", R" + rrindex.ToString("x2");
        }

        public override void Execute(MCU mcu, ushort command)
        {
            rrindex = (byte)GetValueOnCommandMask(command, 0b0000001000001111);
            rdindex = (byte)GetValueOnCommandMask(command, 0b0000000111110000);
            byte rr = GetDataMemoryByteOnRegistrIndex(rrindex);
            byte rd = GetDataMemoryByteOnRegistrIndex(rdindex);
            byte r = (byte)(rr + rd);

            mcu.SREG.H = mcu.SREG.CalcH(rr, rd, r);
            mcu.SREG.V = mcu.SREG.CalcV(rr, rd, r);
            mcu.SREG.N = (r & 0b10000000) == 0b10000000;
            mcu.SREG.S = mcu.SREG.V ^ mcu.SREG.N;
            mcu.SREG.Z = r == 0;
            mcu.SREG.C = mcu.SREG.CalcC(rr, rd, r);

            mcu.DataMemory.SetByteByOffset(rdindex, rd);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return (command & 0b0000110000000000) == 0b0000110000000000;
        }

        private byte rrindex;
        private byte rdindex;

    }




    public class JMPCommand : BaseCommand
    {
        public JMPCommand(MCU mcu) : base(mcu){ }

        public override string Disasemble()
        {
            return "JMP " + address.ToString("X8");
        }

        public override void Execute(MCU mcu, ushort command)
        {
            uint bigcommand = (uint)((command<<16)+mcu.ProgramMemory.GetData(mcu.PC + 2)); //берем следующие 2 байта после PC
            address = GetValueOnCommandMask(bigcommand,0b00000001111100011111111111111111);

            mcu.ClockCounter= mcu.ClockCounter+3;
            mcu.PC = address; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return (command & 0b1001010000001100) == 0b1001010000001100;
        }

        protected uint address;

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

    public class SECCommand : BaseCommand
    {
        public SECCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "SEC";
        }

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.C = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return command== 0b1001010000001000;
        }
    }

    public class SEHCommand : BaseCommand
    {
        public SEHCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "SEH";
        }

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.H = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return command == 0b1001010001011000;
        }
    }

    public class SEICommand : BaseCommand
    {
        public SEICommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "SEI";
        }

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.I = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return command == 0b1001010001111000;
        }
    }

    public class SENCommand : BaseCommand
    {
        public SENCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "SEN";
        }

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.N = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return command == 0b1001010000111000;
        }
    }

    public class SERCommand : BaseCommand
    {
        public SERCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "SER R"+rdindex.ToString("x2");
        }

        public override void Execute(MCU mcu, ushort command)
        {
            rdindex = (byte)GetValueOnCommandMask(command, 0b0000000011110000);
            mcu.DataMemory.SetByteByOffset((uint)(rdindex + 16), 0xFF);
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return (command & 0b1110111100001111) == 0b1110111100001111;
        }

        private byte rdindex;
    }

    public class SESCommand : BaseCommand
    {
        public SESCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "SES";
        }

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.S = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return command == 0b1001010001001000;
        }
    }



    public class XCHCommand : BaseCommand
    {
        public XCHCommand(MCU mcu) : base(mcu) { }

        public override string Disasemble()
        {
            return "XCH Z,R" + rdindex.ToString("x2");
        }

        public override void Execute(MCU mcu, ushort command)
        {
            rdindex = (byte)GetValueOnCommandMask(command, 0b0000000111110000);
            byte fromRD = GetDataMemoryByteOnRegistrIndex(rdindex);
            uint address =(uint)(GetDataMemoryByteOnRegistrIndex(31) * 256 + GetDataMemoryByteOnRegistrIndex(30));
            byte fromRAM = mcu.DataMemory.GetByteByOffset(address);
            mcu.DataMemory.SetByteByOffset(rdindex, fromRAM);
            mcu.DataMemory.SetByteByOffset(address, fromRD);

            mcu.ClockCounter+=2; // Два цикла
            mcu.PC += 2; // Комманды двухбайтовые
        }

        public override bool ItsMe(ushort command)
        {
            return (command & 0b1001001000000100) == 0b1001001000000100;
        }
        
        private byte rdindex;

    }


}
