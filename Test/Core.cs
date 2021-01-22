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

        public bool GetByIndex(int index)
        {
            switch (index)
            {
                case 0: return C;
                case 1: return Z;
                case 2: return N;
                case 3: return V;
                case 4: return S;
                case 5: return H;
                case 6: return T;
                case 7: return I;
                default: throw new IndexOutOfRangeException();
            }
        }

        public void SetByIndex(int index, bool value)
        {
            switch (index)
            {
                case 0: C=value;break;
                case 1: Z=value;break;
                case 2: N=value;break;
                case 3: V=value;break;
                case 4: S=value;break;
                case 5: H=value;break;
                case 6: T=value;break;
                case 7: I=value;break;
                default: throw new IndexOutOfRangeException();
            }
        }


        public string FlagNameByIndex(int index)
        {
            switch (index)
            {
                case 0: return "Carry";
                case 1: return "Zerro";
                case 2: return "Negative";
                case 3: return "Overflow";
                case 4: return "Sign";
                case 5: return "Half Carry";
                case 6: return "Transfer";
                case 7: return "Interrupt";
                default: throw new IndexOutOfRangeException();
            }
        }


    }

    public class IO
    {
        public string GetNameByAddress(int address)
        {
            switch (address)
            {
                case 0x23:return "PINB";
                case 0x24: return "DDRB";
                case 0x25: return "PORTB";
                case 0x26: return "PINC";
                case 0x27: return "DDRC";
                case 0x28: return "PORTC";
                case 0x29: return "PIND";
                case 0x2A: return "DDRD";
                case 0x2B: return "PORTD";
                case 0x35: return "TIFR0";
                case 0x36: return "TIFR1";
                case 0x37: return "TIFR2";
                case 0x3B: return "PCIFR";
                case 0x3C: return "EIFR";
                case 0x3D: return "EIMSK";
                case 0x3E: return "GPIOR0";
                case 0x3F: return "EECR";
                case 0x40: return "EEDR";
                case 0x41: return "EEARL";
                case 0x42: return "EEARH";
                case 0x43: return "GTCCR";
                case 0x44: return "TCCR0A";
                case 0x45: return "TCCR0B";
                case 0x46: return "TCNT0";
                case 0x47: return "OCR0A";
                case 0x48: return "OCR0B";
                case 0x4A: return "GPIOR1";
                case 0x4B: return "GPIOR2";
                case 0x4C: return "SPCR";
                case 0x4D: return "SPSR";
                case 0x4E: return "SPDR";
                case 0x50: return "ACSR";
                case 0x53: return "SMCR";
                case 0x54: return "MCUSR";
                case 0x55: return "MCUCR";
                case 0x57: return "SPMCSR";
                case 0x5D: return "SPL";
                case 0x5E: return "SPH";
                case 0x5F: return "SREG";
                case 0x60: return "WDTCSR";
                case 0x61: return "CLKPR";
                case 0x64: return "PRR";
                case 0x66: return "OSCCAL";
                case 0x68: return "PCICR";
                case 0x69: return "EICRA";
                case 0x6B: return "PCMSK0";
                case 0x6C: return "PCMSK1";
                case 0x6D: return "PCMSK2";
                case 0x6E: return "TIMSK0";
                case 0x6F: return "TIMSK1";
                case 0x70: return "TIMSK2";
                case 0x78: return "ADCL";
                case 0x79: return "ADCH";
                case 0x7A: return "ADCSRA";
                case 0x7B: return "ADCSRB";
                case 0x7C: return "ADMUX";
                case 0x7E: return "DIDR0";
                case 0x7F: return "DIDR1";
                case 0x80: return "TCCR1A";
                case 0x81: return "TCCR1B";
                case 0x82: return "TCCR1C";
                case 0x84: return "TCNT1L";
                case 0x85: return "TCNT1H";
                case 0x86: return "ICR1L";
                case 0x87: return "ICR1H";
                case 0x88: return "OCR1AL";
                case 0x89: return "OCR1AH";
                case 0x8A: return "OCR1BL";
                case 0x8B: return "OCR1BH";
                case 0xB0: return "TCCR2A";
                case 0xB1: return "TCCR2B";
                case 0xB2: return "TCNT2";
                case 0xB3: return "OCR2A";
                case 0xB4: return "OCR2B";
                case 0xB6: return "ASSR";
                case 0xB8: return "TWBR";
                case 0xB9: return "TWSR";
                case 0xBA: return "TWAR";
                case 0xBB: return "TWDR";
                case 0xBC: return "TWCR";
                case 0xBD: return "TWAMR";
                case 0xC0: return "UCSR0A";
                case 0xC1: return "UCSR0B";
                case 0xC2: return "UCSR0C";
                case 0xC4: return "UBRR0L";
                case 0xC5: return "UBRR0H";
                case 0xC6: return "UDR0";
                default: throw new ApplicationException("Not used. But reserved!");
            }
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
            IO = new IO();
        }

        public void DoClock()
        {
            // Внимание! Конвеер. на одном такте идет выборка команды и исполнение предыдущей выбраной команды
            var command = ProgramMemory.GetData(PC);
            core.ExecuteCommand(command);
        }

        public void SetStopMode()
        {
            throw new NotImplementedException();
        }


        protected Core core;
        
        
        public uint PC { get; set; } // Programm counter
        public uint SP { get; set; } // Stack pointer

        public UInt64 ClockCounter { get; set; }
        public StatusReg SREG { get; set; }
        public DataMemory DataMemory { get; set; }
        public FlashProgrammMemory ProgramMemory { get; set; }
        public IO IO { get; set; }
    }


}
