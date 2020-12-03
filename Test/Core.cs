using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class DataMemory
    {
        public DataMemory()
        {
            data = new byte[0x08FF];
        }
        protected static byte[] data;

        [DisplayName("R0")]
        public byte R0 { get{ return data[0]; } set { data[0] = value; } }
        [DisplayName("R1")]
        public byte R1 { get { return data[1]; } set { data[1] = value; } }
        [DisplayName("R2")]
        public byte R2 { get { return data[2]; } set { data[2] = value; } }
        [DisplayName("R3")]
        public byte R3 { get { return data[3]; } set { data[3] = value; } }
        [DisplayName("R4")]
        public byte R4 { get { return data[4]; } set { data[4] = value; } }
        [DisplayName("R5")]
        public byte R5 { get { return data[5]; } set { data[5] = value; } }
        [DisplayName("R6")]
        public byte R6 { get { return data[6]; } set { data[6] = value; } }
        [DisplayName("R7")]
        public byte R7 { get { return data[7]; } set { data[7] = value; } }
        [DisplayName("R8")]
        public byte R8 { get { return data[8]; } set { data[8] = value; } }
        [DisplayName("R9")]
        public byte R9 { get { return data[9]; } set { data[9] = value; } }
        [DisplayName("R10")]
        public byte R10 { get { return data[10]; } set { data[10] = value; } }
        [DisplayName("R11")]
        public byte R11 { get { return data[11]; } set { data[11] = value; } }
        [DisplayName("R12")]
        public byte R12 { get { return data[12]; } set { data[12] = value; } }
        [DisplayName("R13")]
        public byte R13 { get { return data[13]; } set { data[13] = value; } }
        [DisplayName("R14")]
        public byte R14 { get { return data[14]; } set { data[14] = value; } }
        [DisplayName("R15")]
        public byte R15 { get { return data[15]; } set { data[15] = value; } }
        [DisplayName("R16")]
        public byte R16 { get { return data[16]; } set { data[16] = value; } }
        [DisplayName("R17")]
        public byte R17 { get { return data[17]; } set { data[17] = value; } }
        [DisplayName("R18")]
        public byte R18 { get { return data[18]; } set { data[18] = value; } }
        [DisplayName("R19")]
        public byte R19 { get { return data[19]; } set { data[19] = value; } }
        [DisplayName("R20")]
        public byte R20 { get { return data[20]; } set { data[20] = value; } }
        [DisplayName("R21")]
        public byte R21 { get { return data[21]; } set { data[21] = value; } }
        [DisplayName("R22")]
        public byte R22 { get { return data[22]; } set { data[22] = value; } }
        [DisplayName("R23")]
        public byte R23 { get { return data[23]; } set { data[23] = value; } }
        [DisplayName("R24")]
        public byte R24 { get { return data[24]; } set { data[24] = value; } }
        [DisplayName("R25")]
        public byte R25 { get { return data[25]; } set { data[25] = value; } }
        [DisplayName("R26")]
        public byte R26 { get { return data[26]; } set { data[26] = value; } }
        [DisplayName("R27")]
        public byte R27 { get { return data[27]; } set { data[27] = value; } }
        [DisplayName("R28")]
        public byte R28 { get { return data[28]; } set { data[28] = value; } }
        [DisplayName("R29")]
        public byte R29 { get { return data[29]; } set { data[29] = value; } }
        [DisplayName("R30")]
        public byte R30 { get { return data[30]; } set { data[30] = value; } }
        [DisplayName("R31")]
        public byte R31 { get { return data[31]; } set { data[31] = value; } }
        

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
