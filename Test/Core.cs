using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avr
{

    public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Map()
        {
            Forward = new Indexer<T1, T2>(_forward);
            Reverse = new Indexer<T2, T1>(_reverse);
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public T2 this[T1 index]
        {
            get { return _forward[index]; }
            set { _forward[index] = value; }
        }

        public T1 this[T2 index]
        {
            get { return _reverse[index]; }
            set { _reverse[index] = value; }
        }


        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void Remove(T1 t1)
        {
            T2 revKey = Forward[t1];
            _forward.Remove(t1);
            _reverse.Remove(revKey);
        }

        public void Remove(T2 t2)
        {
            T1 forwardKey = Reverse[t2];
            _reverse.Remove(t2);
            _forward.Remove(forwardKey);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return _forward.GetEnumerator();
        }

        public class Indexer<T3, T4>
        {
            private readonly Dictionary<T3, T4> _dictionary;

            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }

            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }

            public bool Contains(T3 key)
            {
                return _dictionary.ContainsKey(key);
            }
        }
    }

    public class StatusReg
    {
        private bool c;  // Carry Flag
        private bool z;  // Zero Flag
        private bool n;  // Negative Flag
        private bool v;  // Two’s complement overflow indicator
        private bool s;  // N ⊕ V, for signed tests (xor)
        private bool h;  // Half Carry Flag
        private bool t;  // Transfer bit used by BLD and BST instructions
        private bool i;  // Global Interrupt Enable/Disable Flag

        protected Dictionary<string, bool> sreg;

        public event Action<byte> OnSaveSREG;
        public event Func<byte> OnLoadSREG;

        protected void LoadSREG()
        {
            if (OnLoadSREG != null)
            {
                byte bits = OnLoadSREG();
                sreg["C"] = (bits & 0b00000001) == 0b00000001;
                sreg["Z"] = (bits & 0b00000010) == 0b00000010;
                sreg["N"] = (bits & 0b00000100) == 0b00000100;
                sreg["V"] = (bits & 0b00001000) == 0b00001000;
                sreg["S"] = (bits & 0b00010000) == 0b00010000;
                sreg["H"] = (bits & 0b00100000) == 0b00100000;
                sreg["T"] = (bits & 0b01000000) == 0b01000000;
                sreg["I"] = (bits & 0b10000000) == 0b100000000;
            }
            else throw new ApplicationException("OnLoadSREG not init!");
        }

        protected void SaveSREG()
        {
            if (OnSaveSREG != null)
            {
                byte bits = 0;
                if (sreg["C"]) bits |= 0b00000001;
                if (sreg["Z"]) bits |= 0b00000010;
                if (sreg["N"]) bits |= 0b00000100;
                if (sreg["V"]) bits |= 0b00001000;
                if (sreg["S"]) bits |= 0b00010000;
                if (sreg["H"]) bits |= 0b00100000;
                if (sreg["T"]) bits |= 0b01000000;
                if (sreg["I"]) bits |= 0b10000000;
                OnSaveSREG(bits);
            }
            else throw new ApplicationException("OnSaveSREG not init!");
        }

        protected bool GetBit(string bitName)
        {
            LoadSREG();
            return sreg[bitName];
        }
        protected void SetBit(string bitName, bool value)
        {
            sreg[bitName] = value;
            SaveSREG();
        }


        public bool C { get { return GetBit("C"); } set { SetBit("C",value); } } 
        public bool Z { get { return GetBit("Z"); } set { SetBit("Z", value); } }
        public bool N { get { return GetBit("N"); } set { SetBit("N", value); } }
        public bool V { get { return GetBit("V"); } set { SetBit("V", value); } }
        public bool S { get { return GetBit("S"); } set { SetBit("S", value); } }
        public bool H { get { return GetBit("H"); } set { SetBit("H", value); } }
        public bool T { get { return GetBit("T"); } set { SetBit("T", value); } }
        public bool I { get { return GetBit("I"); } set { SetBit("I", value); } }

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

        public string GetFlagLetterByIndex(int index)
        {
            switch (index)
            {
                case 0: return "C";
                case 1: return "Z";
                case 2: return "N";
                case 3: return "V";
                case 4: return "S";
                case 5: return "H";
                case 6: return "T";
                case 7: return "I";
                default: throw new IndexOutOfRangeException();
            }
        }


        public bool GetByIndex(int index)
        {
            LoadSREG();
            return sreg[GetFlagLetterByIndex(index)];
        }


        public void SetByIndex(int index, bool value) 
        {
            sreg[GetFlagLetterByIndex(index)] = value;
            SaveSREG();
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

    public class CellNames: Map<int, string>
    {
        public CellNames()
        {
            Add(0x23,"PINB");
            Add(0x24,"DDRB");
            Add(0x25,"PORTB");
            Add(0x26,"PINC");
            Add(0x27,"DDRC");
            Add(0x28,"PORTC");
            Add(0x29,"PIND");
            Add(0x2A,"DDRD");
            Add(0x2B, "PORTD");
            Add(0x35, "TIFR0");
            Add(0x36, "TIFR1");
            Add(0x37, "TIFR2");
            Add(0x3B, "PCIFR");
            Add(0x3C, "EIFR");
            Add(0x3D, "EIMSK");
            Add(0x3E, "GPIOR0");
            Add(0x3F, "EECR");
            Add(0x40, "EEDR");
            Add(0x41, "EEARL");
            Add(0x42, "EEARH");
            Add(0x43, "GTCCR");
            Add(0x44, "TCCR0A");
            Add(0x45, "TCCR0B");
            Add(0x46, "TCNT0");
            Add(0x47, "OCR0A");
            Add(0x48, "OCR0B");
            Add(0x4A, "GPIOR1");
            Add(0x4B, "GPIOR2");
            Add(0x4C, "SPCR");
            Add(0x4D, "SPSR");
            Add(0x4E, "SPDR");
            Add(0x50, "ACSR");
            Add(0x53, "SMCR");
            Add(0x54, "MCUSR");
            Add(0x55, "MCUCR");
            Add(0x57, "SPMCSR");
            Add(0x5D, "SPL");
            Add(0x5E, "SPH");
            Add(0x5F, "SREG");
            Add(0x60, "WDTCSR");
            Add(0x61, "CLKPR");
            Add(0x64, "PRR");
            Add(0x66, "OSCCAL");
            Add(0x68, "PCICR");
            Add(0x69, "EICRA");
            Add(0x6B, "PCMSK0");
            Add(0x6C, "PCMSK1");
            Add(0x6D, "PCMSK2");
            Add(0x6E, "TIMSK0");
            Add(0x6F, "TIMSK1");
            Add(0x70, "TIMSK2");
            Add(0x78, "ADCL");
            Add(0x79, "ADCH");
            Add(0x7A, "ADCSRA");
            Add(0x7B, "ADCSRB");
            Add(0x7C, "ADMUX");
            Add(0x7E, "DIDR0");
            Add(0x7F, "DIDR1");
            Add(0x80, "TCCR1A");
            Add(0x81, "TCCR1B");
            Add(0x82, "TCCR1C");
            Add(0x84, "TCNT1L");
            Add(0x85, "TCNT1H");
            Add(0x86, "ICR1L");
            Add(0x87, "ICR1H");
            Add(0x88, "OCR1AL");
            Add(0x89, "OCR1AH");
            Add(0x8A, "OCR1BL");
            Add(0x8B, "OCR1BH");
            Add(0xB0, "TCCR2A");
            Add(0xB1, "TCCR2B");
            Add(0xB2, "TCNT2");
            Add(0xB3, "OCR2A");
            Add(0xB4, "OCR2B");
            Add(0xB6, "ASSR");
            Add(0xB8, "TWBR");
            Add(0xB9, "TWSR");
            Add(0xBA, "TWAR");
            Add(0xBB, "TWDR");
            Add(0xBC, "TWCR");
            Add(0xBD, "TWAMR");
            Add(0xC0, "UCSR0A");
            Add(0xC1, "UCSR0B");
            Add(0xC2, "UCSR0C");
            Add(0xC4, "UBRR0L");
            Add(0xC5, "UBRR0H");
            Add(0xC6, "UDR0");

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

        public byte this[uint offset]
        {
            get
            {
                return data[offset];
            }
            set
            {
                data[offset] = value;
            }
            
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
        private uint SREGIndex;
        public CellNames CellNames { get; }
        public MCU()
        {
            PC = 0;
            ClockCounter = 0;
            SREG = new StatusReg();
            core = new Core(this);
            ProgramMemory = new FlashProgrammMemory();
            DataMemory = new DataMemory();

            SREG.OnLoadSREG += SREG_OnLoadSREG;
            SREG.OnSaveSREG += SREG_OnSaveSREG;
            CellNames = new CellNames();
            SREGIndex = (uint) CellNames["SREG"];
            
        }

        private void SREG_OnSaveSREG(byte bits)
        {
            DataMemory[SREGIndex] = bits;
        }

        private byte SREG_OnLoadSREG()
        {
            return DataMemory[SREGIndex];
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
    }


}
