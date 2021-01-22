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
                new ADIWCommand(mcu),
                new ANDCommand(mcu),
                new ANDICommand(mcu),
                new ASRCommand(mcu),
                new BCLRCommand(mcu),
                new BLDCommand(mcu),
                new BRBCCommand(mcu),
                new BRBSCommand(mcu),
                new BRCCCommand(mcu),
                new BRCSCommand(mcu),
                new BREAKCommand(mcu),
                new BREQCommand(mcu),
                new BRGECommand(mcu),
                new BRHCCommand(mcu),
                new BRHSCommand(mcu),
                new BRIDCommand(mcu),
                new BRIECommand(mcu),
                new BRLOCommand(mcu),
                new BRLTCommand(mcu),
                new BRMICommand(mcu),
                new BRNECommand(mcu),
                new BRPLCommand(mcu),
                new BRSHCommand(mcu),
                new BRTCCommand(mcu),
                new BRTSCommand(mcu),
                new BRVCCommand(mcu),
                new BRVSCommand(mcu),
                new BSETCommand(mcu),
                new BSTCommand(mcu),
                new CALLCommand(mcu),
                new CBICommand(mcu),
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
                new ICALLCommand(mcu),
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
                new RCALLCommand(mcu),
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

        protected bool GetBitValueOnIndex(byte value, byte index)
        {
            uint v = (uint)(1 << index);
            return ((value & v) == v);
        }

        protected void SetBitValueOnIndex(ref byte value, byte index)
        {
            uint v = (uint)(1 << index);
            value = (byte)(value | v);
        }
        protected void ClearBitValueOnIndex(ref byte value, byte index)
        {
            uint v = (uint)(1 << index);
            value = (byte)(value - v);
        }

        protected MCU mcu;

    }

    public class ADCCommand : BaseCommand
    {
        public ADCCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "ADC R"+rdindex.ToString()+", R"+rrindex.ToString() ;
        public override bool ItsMe(ushort command) => (command & 0b1111110000000000) == 0b0001110000000000;

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
            mcu.DataMemory.SetByteByOffset(rdindex,r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        

        private byte rrindex;
        private byte rdindex;

    }

    public class ADDCommand : BaseCommand
    {
        public ADDCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "ADD R" + rdindex.ToString()+ ", R" + rrindex.ToString();
        public override bool ItsMe(ushort command) => (command & 0b1111110000000000) == 0b0000110000000000;

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
            mcu.DataMemory.SetByteByOffset(rdindex, r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
        private byte rrindex;
        private byte rdindex;

    }

    public class ADIWCommand : BaseCommand
    {
        public ADIWCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            switch (ridx)
            {
                case 0: return "ADIW R25:R24, "+rvalue.ToString()+ " ; Add "+ rvalue.ToString() + " to R25:R24"; 
                case 1: return "ADIW XH:XL, " + rvalue.ToString() + " ; Add " + rvalue.ToString() + " to X-Pointer(R27:R26)";
                case 2: return "ADIW YH:YL, " + rvalue.ToString() + " ; Add " + rvalue.ToString() + " to Y-Pointer(R29:R28)";
                case 3: return "ADIW ZH:ZL, " + rvalue.ToString() + " ; Add " + rvalue.ToString() + " to Z-Pointer(R31:R30)";
                default:throw new ApplicationException("Invalid instruction!");
            }
        }
        
        public override bool ItsMe(ushort command) => (command & 0b1111111100000000) == 0b1001011000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            rvalue = (byte) GetValueOnCommandMask(command, 0b0000000011001111);
            ridx = (byte) GetValueOnCommandMask(command, 0b0000000000110000);
            byte laddr =(byte)(24 + (ridx << 1));
            byte haddr = (byte)(24 + (ridx << 1)+1);

            byte l = mcu.DataMemory.GetByteByOffset(laddr);
            byte h = mcu.DataMemory.GetByteByOffset(haddr);
            uint res = (uint) (h << 8) + l + rvalue;
            byte rl = (byte)(res & 0xFF);
            byte rh = (byte)((res >> 8) & 0xFF);
            mcu.DataMemory.SetByteByOffset(laddr, rl);
            mcu.DataMemory.SetByteByOffset(haddr, rh);

            mcu.SREG.N = (rh & 0x80) == 0x80;
            mcu.SREG.V = ((h & 0x80) == 0) && mcu.SREG.N;
            mcu.SREG.S = mcu.SREG.N ^ mcu.SREG.V;
            mcu.SREG.Z = res == 0;
            mcu.SREG.C = ((h & 0x80) == 0x80) && ((rh & 0x80) == 0);

            mcu.ClockCounter ++;
            mcu.PC +=2; // Комманды двухбайтовые
        }
        protected byte rvalue;
        protected byte ridx;
    }

    public class ANDCommand : BaseCommand
    {
        public ANDCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "AND R" + rdindex.ToString() + ", R" + rrindex.ToString();
        public override bool ItsMe(ushort command) => (command & 0b1111110000000000) == 0b0010000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            rrindex = (byte)GetValueOnCommandMask(command, 0b0000001000001111);
            rdindex = (byte)GetValueOnCommandMask(command, 0b0000000111110000);
            byte rr = GetDataMemoryByteOnRegistrIndex(rrindex);
            byte rd = GetDataMemoryByteOnRegistrIndex(rdindex);
            byte r = (byte)(rr & rd);
            
            mcu.SREG.V = false;
            mcu.SREG.N = (r & 0b10000000) == 0b10000000;
            mcu.SREG.S = mcu.SREG.V ^ mcu.SREG.N;
            mcu.SREG.Z = r == 0;
            mcu.SREG.C = mcu.SREG.CalcC(rr, rd, r);
            mcu.DataMemory.SetByteByOffset(rdindex, r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
        private byte rrindex;
        private byte rdindex;

    }

    public class ANDICommand : BaseCommand
    {
        public ANDICommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "ANDI R" + rdindex.ToString() + ", " + rvalue.ToString("x2");
        public override bool ItsMe(ushort command) => (command & 0b1111000000000000) == 0b0111000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            rvalue = (byte) GetValueOnCommandMask(command, 0b0000111100001111);
            rdindex = (byte)(GetValueOnCommandMask(command, 0b0000000011110000) +16);
            byte rd = GetDataMemoryByteOnRegistrIndex(rdindex);
            byte r = (byte)(rvalue & rd);

            mcu.SREG.V = false;
            mcu.SREG.N = (r & 0b10000000) == 0b10000000;
            mcu.SREG.S = mcu.SREG.V ^ mcu.SREG.N;
            mcu.SREG.Z = r == 0;
            mcu.DataMemory.SetByteByOffset(rdindex, r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
        private byte rdindex;
        private byte rvalue;

    }

    public class ASRCommand : BaseCommand
    {
        public ASRCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "ASR R" + rdindex.ToString();
        public override bool ItsMe(ushort command) => (command & 0b1111111000001111) == 0b1001010000000101;

        public override void Execute(MCU mcu, ushort command)
        {
            rdindex = (byte)GetValueOnCommandMask(command, 0b0000000111110000);
            byte rd = GetDataMemoryByteOnRegistrIndex(rdindex);
            byte sbit = (byte)(rd & 0b10000000);
            byte cbit = (byte)(rd & 0b00000001);
                       
            byte r = (byte)((rd>>1)|sbit);

            mcu.SREG.C = cbit==1;
            mcu.SREG.N = (r & 0b10000000) == 0b10000000;
            mcu.SREG.V = mcu.SREG.C ^ mcu.SREG.N; ;
            mcu.SREG.S = mcu.SREG.V ^ mcu.SREG.N;
            mcu.SREG.Z = r == 0;

            mcu.DataMemory.SetByteByOffset(rdindex, r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
        private byte rdindex;

    }

    public class BCLRCommand : BaseCommand
    {
        public BCLRCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            string r;
            switch (rvalue)
            {
                case 0: r = "Clear Carry Flag"; break;
                case 1: r = "Clear Zero Flag"; break;
                case 2: r = "Clear Negative Flag"; break;
                case 3: r = "Clear Overflow Flag"; break;
                case 4: r = "Clear Sign Flag"; break;
                case 5: r = "Clear Half Carry Flag"; break;
                case 6: r = "Clear Transfer bit"; break;
                case 7: r = "Disable interrupts"; break;
                default: throw new ApplicationException();
            }
            return "BCLR " + rvalue.ToString() + "; " + r;
        }
            
        
        public override bool ItsMe(ushort command) => (command & 0b1111111110001111) == 0b1001010010001000;

        public override void Execute(MCU mcu, ushort command)
        {
            rvalue = (byte)GetValueOnCommandMask(command, 0b0000000001110000);
            switch (rvalue)
            {
                case 0: mcu.SREG.C = false;break;
                case 1: mcu.SREG.Z = false; break;
                case 2: mcu.SREG.N = false; break;
                case 3: mcu.SREG.V = false; break;
                case 4: mcu.SREG.S = false; break;
                case 5: mcu.SREG.H = false; break;
                case 6: mcu.SREG.T = false; break;
                case 7: mcu.SREG.I = false; break;
            }

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
        private byte rvalue;

    }

    public class BLDCommand : BaseCommand
    {
        public BLDCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "BLD R" + rdindex.ToString() + ", " + bitindex.ToString()
              + " ; Load T Flag into bit "+ bitindex.ToString() + " of R"+ rdindex.ToString();
        public override bool ItsMe(ushort command) => (command & 0b1111111000001000) == 0b1111100000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            rdindex = (byte)GetValueOnCommandMask(command, 0b0000000111110000);
            byte rd = GetDataMemoryByteOnRegistrIndex(rdindex);
            bitindex = (byte)GetValueOnCommandMask(command, 0b0000000000000111);
            byte mask = (byte)(1 << bitindex);
            byte v = (byte)((mcu.SREG.T ? 1 : 0)<<bitindex); 
            byte r = (byte)(rd-mask+v);
            mcu.DataMemory.SetByteByOffset(rdindex, r);

            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
        private byte bitindex;
        private byte rdindex;

    }

    public class BRBCCommand : BaseCommand
    {
        public BRBCCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRBC " + bitindex.ToString() + ", " + offset.ToString("x2") + " ; Branch if "+mcu.SREG.FlagNameByIndex(bitindex)+" Flag cleared";
        }
        
        public override bool ItsMe(ushort command) => (command & 0b1111110000000000) == 0b1111010000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            bitindex = (byte) GetValueOnCommandMask(command, 0b0000000000000111);
            if (!mcu.SREG.GetByIndex(bitindex))
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter++;  // два цикла если истина
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
            
        }
        protected uint offset;
        protected byte bitindex;

    }

    public class BRBSCommand : BaseCommand
    {
        public BRBSCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRBS " + bitindex.ToString() + ", " + offset.ToString("x2") + " ; Branch if " + mcu.SREG.FlagNameByIndex(bitindex) + " Flag was set";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000000) == 0b1111000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            bitindex = (byte)GetValueOnCommandMask(command, 0b0000000000000111);
            if (mcu.SREG.GetByIndex(bitindex))
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter+=2; 
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
            

        }
        protected uint offset;
        protected byte bitindex;

    }

    public class BRCCCommand : BaseCommand
    {
        public BRCCCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRCC "  + offset.ToString("x2") + " ; Branch if Carry Flag cleared";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.GetByIndex(0))
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter+=2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRCSCommand : BaseCommand
    {
        public BRCSCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRCS " + offset.ToString("x2") + " ; Branch if Carry Flag was set";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.GetByIndex(0))
                mcu.PC = mcu.PC + offset + 2;

            mcu.PC += 2;
            mcu.ClockCounter = mcu.ClockCounter + 1;
        }
        protected uint offset;
    }

    public class BREAKCommand : BaseCommand
    {
        public BREAKCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "BREAK ; Stop mode on cheap debugging";
        public override bool ItsMe(ushort command) => command == 0b1001010110011000;

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SetStopMode();
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

    }

    public class BREQCommand : BaseCommand
    {
        public BREQCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BREQ " + offset.ToString("x4") + " ; Branch if Zerro Flag set";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000001;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.Z)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRGECommand : BaseCommand
    {
        public BRGECommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRGE " + offset.ToString("x4") + " ; Branch if Signed Flag clear";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000100;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.S)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRHCCommand : BaseCommand
    {
        public BRHCCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRHC " + offset.ToString("x4") + " ; Branch if half carry Flag clear";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000101;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.H)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRHSCommand : BaseCommand
    {
        public BRHSCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRHS " + offset.ToString("x4") + " ; Branch if half carry Flag set";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000101;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.H)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRIDCommand : BaseCommand
    {
        public BRIDCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRID " + offset.ToString("x4") + " ; Branch if global inerrupt disabled";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000111;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.H)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRIECommand : BaseCommand
    {
        public BRIECommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRIE " + offset.ToString("x4") + " ; Branch if global interrupt enabled";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000111;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.H)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRLOCommand : BaseCommand
    {
        public BRLOCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRLO " + offset.ToString("x4") + " ; Branch if lower (Carry Flag is set)";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.C)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRLTCommand : BaseCommand
    {
        public BRLTCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRLT " + offset.ToString("x4") + " ; Branch if less than (Signed Flag is set)";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000100;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.S)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRMICommand : BaseCommand
    {
        public BRMICommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRMI " + offset.ToString("x4") + " ; Branch if minus (Negative Flag is set)";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000010;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.N)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRNECommand : BaseCommand
    {
        public BRNECommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRNE " + offset.ToString("x4") + " ; Branch if Zerro Flag cleared";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000001;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.Z)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRPLCommand : BaseCommand
    {
        public BRPLCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRPL " + offset.ToString("x4") + " ; Branch if Plus (Negative Flag cleared)";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000010;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.N)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRSHCommand : BaseCommand
    {
        public BRSHCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRSH " + offset.ToString("x4") + " ; Branch if Same or Higher (Carry Flag cleared)";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.C)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRTCCommand : BaseCommand
    {
        public BRTCCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRTC " + offset.ToString("x4") + " ; Branch if T-Flag cleared)";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000110;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.T)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRTSCommand : BaseCommand
    {
        public BRTSCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRTS " + offset.ToString("x4") + " ; Branch if T-Flag set";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000110;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.T)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRVCCommand : BaseCommand
    {
        public BRVCCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRVC " + offset.ToString("x4") + " ; Branch if Overflow Flag is clear";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111010000000011;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (!mcu.SREG.V)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }

    public class BRVSCommand : BaseCommand
    {
        public BRVSCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BRVS " + offset.ToString("x4") + " ; Branch if Overflow Flag set";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111110000000111) == 0b1111000000000011;

        public override void Execute(MCU mcu, ushort command)
        {
            offset = GetValueOnCommandMask(command, 0b0000001111111000);
            if (mcu.SREG.V)
            {
                mcu.PC = mcu.PC + offset + 2;
                mcu.ClockCounter += 2;
            }
            else
            {
                mcu.PC += 2;
                mcu.ClockCounter++;
            }
        }
        protected uint offset;
    }


    public class BSETCommand : BaseCommand
    {
        public BSETCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BSET " + idx.ToString() + " ; Bit set in SREG ("+mcu.SREG.FlagNameByIndex(idx) +")";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111111110001111) == 0b1001010000001000;

        public override void Execute(MCU mcu, ushort command)
        {
            idx = (int)GetValueOnCommandMask(command, 0b0000000001110000);
            mcu.SREG.SetByIndex(idx,true);

            mcu.PC += 2;
            mcu.ClockCounter++;
            
        }
        protected int idx;
    }


    public class BSTCommand : BaseCommand
    {
        public BSTCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "BST R" + regoffset.ToString() + ", "+bitoffset.ToString()+" ; Bit store";
        }

        public override bool ItsMe(ushort command) => (command & 0b1111111000001000) == 0b1111101000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            regoffset = (byte)GetValueOnCommandMask(command, 0b0000000111110000);
            bitoffset = (byte)GetValueOnCommandMask(command, 0b0000000000000111);
            byte rr = GetDataMemoryByteOnRegistrIndex(regoffset);
            bool value = GetBitValueOnIndex(rr, bitoffset);
            mcu.SREG.T = value;

            mcu.PC += 2;
            mcu.ClockCounter++;

        }
        protected byte regoffset;
        protected byte bitoffset;
    }



    public class CALLCommand : BaseCommand
    {
        public CALLCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "CALL " + address.ToString("X8");
        public override bool ItsMe(ushort command) => (command & 0b1111111000001110) == 0b1001010000001110;

        public override void Execute(MCU mcu, ushort command)
        {
            uint bigcommand = (uint)((command << 16) + mcu.ProgramMemory.GetData(mcu.PC + 2)); //берем следующие 2 байта после PC
            address = GetValueOnCommandMask(bigcommand, 0b00000001111100011111111111111111);
            uint retAddress = mcu.PC + 4;
            mcu.DataMemory.SetByteByOffset(mcu.SP, (byte)((retAddress >> 8) & 0xFF));
            mcu.DataMemory.SetByteByOffset(mcu.SP+1, (byte)(retAddress & 0xFF));
            mcu.SP -= 2;
            mcu.ClockCounter = mcu.ClockCounter + 4;
            mcu.PC = address; // Комманды двухбайтовые
        }
        protected uint address;

    }

    public class CBICommand : BaseCommand
    {
        public CBICommand(MCU mcu) : base(mcu) { }
        public override string Disasemble()
        {
            return "CBI " + regoffset.ToString() + ", " + bitoffset.ToString() + " ; Clear bit "+ bitoffset.ToString()+ " in "+mcu.IO.GetNameByAddress(regoffset);
        }

        public override bool ItsMe(ushort command) => (command & 0b1111111100000000) == 0b1001100000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            regoffset = (byte)(GetValueOnCommandMask(command, 0b0000000011111000) + 0x20);
            bitoffset = (byte)GetValueOnCommandMask(command, 0b0000000000000111);
            byte rr = GetDataMemoryByteOnRegistrIndex(regoffset);
            ClearBitValueOnIndex(ref rr, bitoffset);
            mcu.PC += 2;
            mcu.ClockCounter++;

        }
        protected byte regoffset;
        protected byte bitoffset;
    }




    public class ICALLCommand : BaseCommand
    {
        public ICALLCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "ICALL ; Call on address in Z (" + address.ToString("X8")+")";
        public override bool ItsMe(ushort command) => command == 0b1001010100001001;

        public override void Execute(MCU mcu, ushort command)
        {
            byte h = mcu.DataMemory.GetByteByOffset(31);
            byte l = mcu.DataMemory.GetByteByOffset(30);
            address = (uint)(h * 256 + l);
            uint retAddress = mcu.PC + 2;
            mcu.DataMemory.SetByteByOffset(mcu.SP, (byte)((retAddress >> 8) & 0xFF));
            mcu.DataMemory.SetByteByOffset(mcu.SP + 1, (byte)(retAddress & 0xFF));
            mcu.SP -= 2;
            mcu.ClockCounter = mcu.ClockCounter + 3;
            mcu.PC = address; // Комманды двухбайтовые
        }
        protected uint address;

    }

    public class JMPCommand : BaseCommand
    {
        public JMPCommand(MCU mcu) : base(mcu){ }
        public override string Disasemble() => "JMP " + address.ToString("X8");
        public override bool ItsMe(ushort command) => (command & 0b1111111000001110) == 0b1001010000001100;

        public override void Execute(MCU mcu, ushort command)
        {
            uint bigcommand = (uint)((command<<16)+mcu.ProgramMemory.GetData(mcu.PC + 2)); //берем следующие 2 байта после PC
            address = GetValueOnCommandMask(bigcommand,0b00000001111100011111111111111111);

            mcu.ClockCounter= mcu.ClockCounter+3;
            mcu.PC = address; // Комманды двухбайтовые
        }
        protected uint address;

    }

    public class NOPCommand : BaseCommand
    {
        public NOPCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() =>  "NOP";
        public override bool ItsMe(ushort command) => command == 0b0000000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

    }

    public class RCALLCommand : BaseCommand
    {
        public RCALLCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "RCALL ; Call on address in Z (" + address.ToString("X8") + ")";
        public override bool ItsMe(ushort command) => (command & 0b1111000000000000) == 0b1101000000000000;

        public override void Execute(MCU mcu, ushort command)
        {
            address = GetValueOnCommandMask(command, 0b0000011111111111);
            uint retAddress = mcu.PC + 2;
            mcu.DataMemory.SetByteByOffset(mcu.SP, (byte)((retAddress >> 8) & 0xFF));
            mcu.DataMemory.SetByteByOffset(mcu.SP + 1, (byte)(retAddress & 0xFF));
            mcu.SP -= 2;
            mcu.ClockCounter = mcu.ClockCounter + 3;
            mcu.PC = address; // Комманды двухбайтовые
        }
        protected uint address;

    }



    public class SECCommand : BaseCommand
    {
        public SECCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "SEC";
        public override bool ItsMe(ushort command) => command == 0b1001010000001000;

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.C = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
    }

    public class SEHCommand : BaseCommand
    {
        public SEHCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "SEH";
        public override bool ItsMe(ushort command) => command == 0b1001010001011000;

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.H = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
    }

    public class SEICommand : BaseCommand
    {
        public SEICommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() =>"SEI";
        public override bool ItsMe(ushort command) => command == 0b1001010001111000;

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.I = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }
    }

    public class SENCommand : BaseCommand
    {
        public SENCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "SEN";
        public override bool ItsMe(ushort command) => command == 0b1001010000111000;

        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.N = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
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

        public override bool ItsMe(ushort command) =>(command & 0b1111111100001111) == 0b1110111100001111;

        private byte rdindex;
    }

    public class SESCommand : BaseCommand
    {
        public SESCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "SES";
        public override bool ItsMe(ushort command) => command == 0b1001010001001000;


        public override void Execute(MCU mcu, ushort command)
        {
            mcu.SREG.S = true;
            mcu.ClockCounter++;
            mcu.PC += 2; // Комманды двухбайтовые
        }

        
    }



    public class XCHCommand : BaseCommand
    {
        public XCHCommand(MCU mcu) : base(mcu) { }
        public override string Disasemble() => "XCH Z,R" + rdindex.ToString("x2");
        public override bool ItsMe(ushort command) => (command & 0b1111111000001111) == 0b1001001000000100;

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


                
        private byte rdindex;

    }


}
