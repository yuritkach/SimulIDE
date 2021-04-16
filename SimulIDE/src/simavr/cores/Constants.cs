using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class Constants
    {

        public static byte[] FUSE
        {
            get
            {
                switch (Constants.FUSE_MEMORY_SIZE)
                {
                    case 6: return new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                    case 3: return new byte[] { Constants.LFUSE_DEFAULT, Constants.HFUSE_DEFAULT, Constants.EFUSE_DEFAULT };
                    case 2: return new byte[] { Constants.LFUSE_DEFAULT, Constants.HFUSE_DEFAULT };
                    case 1: return new byte[] { Constants.FUSE_DEFAULT };
                    default: return new byte[] { 0 };
                }
            }
        }
        public static byte[] SIGNATURE
        {
            get
            {
                return new byte[3] { SIGNATURE_0, SIGNATURE_1, SIGNATURE_2 };
            }
        }

        public static int MCU_STATUS_REG
        {
            get
            {
                return (int)(MCUSR != null ? MCUSR : MCUCSR);
            }
        }

        public static ResetFlags RESETFLAGS
        {
            get
            {
                ResetFlags result = new ResetFlags();
                result.porf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, PORF);
                result.extrf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, EXTRF);
                result.borf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, BORF);
                result.wdrf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, WDRF);
                return result;
            }
        }

        public static byte _SFR_IO8(byte param)
        {
            return (byte)(param + 32);
        }

        public static ushort _SFR_IO16(ushort param)
        {
            return (ushort)(param + 32);
        }

        public static byte _SFR_MEM8(byte param)
        {
            return param;
        }

        public static ushort _SFR_MEM16(ushort param)
        {
            return param;
        }

        public static byte _VECTOR(byte param)
        {
            return param;
        }

        public static byte _BV(byte param)
        {
            return param;
        }

      
        public virtual bool Get__SIM_CORE_DECLARE_H__() { return true; }
        /* we have to declare this, as none of the distro but debian has a modern
         * toolchain and avr-libc. This affects a lot of names, like MCUSR etc
        */
        public virtual bool Get__AVR_LIBC_DEPRECATED_ENABLE__() { return true; }

        public static byte SIGNATURE_0;
        public static byte SIGNATURE_1;
        public static byte SIGNATURE_2;
        public static byte LOCKBITS;
        public static byte LFUSE_DEFAULT;
        public static byte HFUSE_DEFAULT;
        public static byte EFUSE_DEFAULT;
        public static byte FUSE_DEFAULT;
        public static int? MCUSR = null;
        public static int? MCUCSR = null;

        public static bool? __SIM_CORE_DECLARE_H__ = null;
        public static ushort SPM_PAGESIZE;
        public static ushort RAMSTART;
        public static ushort RAMEND;     /* Last On-Chip SRAM Location */
        public static ushort XRAMSIZE;
        public static ushort XRAMEND;
        public static ushort E2END;
        public static ushort E2PAGESIZE;
        public static ushort FLASHEND;
        public static byte FUSE_MEMORY_SIZE;

        public static byte EEARH;
        public static byte EEARL;
        public static byte EEDR;
        public static byte EECR;
        public static byte EERE;
        public static byte EEPE;
        public static byte EEMPE;
        public static byte EERIE;
        public static byte EEPM0;
        public static byte EEPM1;


        public static string SIM_CORENAME;

        /* Registers and associated bit numbers */
        public static byte PINB { get { return _SFR_IO8(0x03); } }
        public static byte PINB0 = 0;
        public static byte PINB1 = 1;
        public static byte PINB2 = 2;
        public static byte PINB3 = 3;
        public static byte PINB4 = 4;
        public static byte PINB5 = 5;
        public static byte PINB6 = 6;
        public static byte PINB7 = 7;

        public static byte DDRB { get { return _SFR_IO8(0x04); } }
        public static byte DDB0 = 0;
        public static byte DDB1 = 1;
        public static byte DDB2 = 2;
        public static byte DDB3 = 3;
        public static byte DDB4 = 4;
        public static byte DDB5 = 5;
        public static byte DDB6 = 6;
        public static byte DDB7 = 7;

        public static byte PORTB { get { return _SFR_IO8(0x05); } }
        public static byte PORTB0 = 0;
        public static byte PORTB1 = 1;
        public static byte PORTB2 = 2;
        public static byte PORTB3 = 3;
        public static byte PORTB4 = 4;
        public static byte PORTB5 = 5;
        public static byte PORTB6 = 6;
        public static byte PORTB7 = 7;

        public static byte PINC { get { return _SFR_IO8(0x06); } }
        public static byte PINC0 = 0;
        public static byte PINC1 = 1;
        public static byte PINC2 = 2;
        public static byte PINC3 = 3;
        public static byte PINC4 = 4;
        public static byte PINC5 = 5;
        public static byte PINC6 = 6;

        public static byte DDRC { get { return _SFR_IO8(0x07); } }
        public static byte DDC0 = 0;
        public static byte DDC1 = 1;
        public static byte DDC2 = 2;
        public static byte DDC3 = 3;
        public static byte DDC4 = 4;
        public static byte DDC5 = 5;
        public static byte DDC6 = 6;

        public static byte PORTC { get { return _SFR_IO8(0x08); } }
        public static byte PORTC0 = 0;
        public static byte PORTC1 = 1;
        public static byte PORTC2 = 2;
        public static byte PORTC3 = 3;
        public static byte PORTC4 = 4;
        public static byte PORTC5 = 5;
        public static byte PORTC6 = 6;

        public static byte PIND { get { return _SFR_IO8(0x09); } }
        public static byte PIND0 = 0;
        public static byte PIND1 = 1;
        public static byte PIND2 = 2;
        public static byte PIND3 = 3;
        public static byte PIND4 = 4;
        public static byte PIND5 = 5;
        public static byte PIND6 = 6;
        public static byte PIND7 = 7;

        public static byte DDRD { get { return _SFR_IO8(0x0A); } }
        public static byte DDD0 = 0;
        public static byte DDD1 = 1;
        public static byte DDD2 = 2;
        public static byte DDD3 = 3;
        public static byte DDD4 = 4;
        public static byte DDD5 = 5;
        public static byte DDD6 = 6;
        public static byte DDD7 = 7;

        public static byte PORTD { get { return _SFR_IO8(0x0B); } }
        public static byte PORTD0 = 0;
        public static byte PORTD1 = 1;
        public static byte PORTD2 = 2;
        public static byte PORTD3 = 3;
        public static byte PORTD4 = 4;
        public static byte PORTD5 = 5;
        public static byte PORTD6 = 6;
        public static byte PORTD7 = 7;

        public static byte TIFR0 { get { return _SFR_IO8(0x15); } }
        public static byte TOV0 = 0;
        public static byte OCF0A = 1;
        public static byte OCF0B = 2;


        public static byte TIFR1 { get { return _SFR_IO8(0x16); } }
        public static byte TOV1 = 0;
        public static byte OCF1A = 1;
        public static byte OCF1B = 2;
        public static byte ICF1 = 5;

        public static byte TIFR2 { get { return _SFR_IO8(0x17); } }
        public static byte TOV2 = 0;
        public static byte OCF2A = 1;
        public static byte OCF2B = 2;

        public static byte PCIFR { get { return _SFR_IO8(0x1B); } }
        public static byte PCIF0 = 0;
        public static byte PCIF1 = 1;
        public static byte PCIF2 = 2;


        public static byte EIFR { get { return _SFR_IO8(0x1C); } }
        public static byte INTF0 = 0;
        public static byte INTF1 = 1;

        public static byte EIMSK { get { return _SFR_IO8(0x1D); } }
        public static byte INT0 = 0;
        public static byte INT1 = 1;

        public static byte GPIOR0 { get { return _SFR_IO8(0x0E); } }
        public static byte GPIOR00 = 0;
        public static byte GPIOR01 = 1;
        public static byte GPIOR02 = 2;
        public static byte GPIOR03 = 3;
        public static byte GPIOR04 = 4;
        public static byte GPIOR05 = 5;
        public static byte GPIOR06 = 6;
        public static byte GPIOR07 = 7;

        public static byte EEDR0 = 0;
        public static byte EEDR1 = 1;
        public static byte EEDR2 = 2;
        public static byte EEDR3 = 3;
        public static byte EEDR4 = 4;
        public static byte EEDR5 = 5;
        public static byte EEDR6 = 6;
        public static byte EEDR7 = 7;

        public ushort EEAR { get { return _SFR_IO16(0x21); } }


        public static byte EEAR0 = 0;
        public static byte EEAR1 = 1;
        public static byte EEAR2 = 2;
        public static byte EEAR3 = 3;
        public static byte EEAR4 = 4;
        public static byte EEAR5 = 5;
        public static byte EEAR6 = 6;
        public static byte EEAR7 = 7;


        public static byte EEAR8 = 0;
        public static byte EEAR9 = 1;


        public UInt32 _EEPROM_REG_LOCATIONS_ = 0x1F2021;

        public static byte GTCCR { get { return _SFR_IO8(0x23); } }
        public static byte PSRSYNC = 0;
        public static byte PSRASY = 1;
        public static byte TSM = 7;

        public static byte TCCR0A { get { return _SFR_IO8(0x24); } }
        public static byte WGM00 = 0;
        public static byte WGM01 = 1;
        public static byte COM0B0 = 4;
        public static byte COM0B1 = 5;
        public static byte COM0A0 = 6;
        public static byte COM0A1 = 7;


        public static byte TCCR0B { get { return _SFR_IO8(0x25); } }
        public static byte CS00 = 0;
        public static byte CS01 = 1;
        public static byte CS02 = 2;
        public static byte WGM02 = 3;
        public static byte FOC0B = 6;
        public static byte FOC0A = 7;

        public static byte TCNT0 { get { return _SFR_IO8(0x26); } }
        public static byte TCNT0_0 = 0;
        public static byte TCNT0_1 = 1;
        public static byte TCNT0_2 = 2;
        public static byte TCNT0_3 = 3;
        public static byte TCNT0_4 = 4;
        public static byte TCNT0_5 = 5;
        public static byte TCNT0_6 = 6;
        public static byte TCNT0_7 = 7;

        public static byte OCR0A { get { return _SFR_IO8(0x27); } }
        public static byte OCR0A_0 = 0;
        public static byte OCR0A_1 = 1;
        public static byte OCR0A_2 = 2;
        public static byte OCR0A_3 = 3;
        public static byte OCR0A_4 = 4;
        public static byte OCR0A_5 = 5;
        public static byte OCR0A_6 = 6;
        public static byte OCR0A_7 = 7;

        public static byte OCR0B { get { return _SFR_IO8(0x28); } }
        public static byte OCR0B_0 = 0;
        public static byte OCR0B_1 = 1;
        public static byte OCR0B_2 = 2;
        public static byte OCR0B_3 = 3;
        public static byte OCR0B_4 = 4;
        public static byte OCR0B_5 = 5;
        public static byte OCR0B_6 = 6;
        public static byte OCR0B_7 = 7;

        public static byte GPIOR1 { get { return _SFR_IO8(0x2A); } }
        public static byte GPIOR10 = 0;
        public static byte GPIOR11 = 1;
        public static byte GPIOR12 = 2;
        public static byte GPIOR13 = 3;
        public static byte GPIOR14 = 4;
        public static byte GPIOR15 = 5;
        public static byte GPIOR16 = 6;
        public static byte GPIOR17 = 7;

        public static byte GPIOR2 { get { return _SFR_IO8(0x2B); } }
        public static byte GPIOR20 = 0;
        public static byte GPIOR21 = 1;
        public static byte GPIOR22 = 2;
        public static byte GPIOR23 = 3;
        public static byte GPIOR24 = 4;
        public static byte GPIOR25 = 5;
        public static byte GPIOR26 = 6;
        public static byte GPIOR27 = 7;

        public static byte SPCR { get { return _SFR_IO8(0x2C); } }
        public static byte SPR0 = 0;
        public static byte SPR1 = 1;
        public static byte CPHA = 2;
        public static byte CPOL = 3;
        public static byte MSTR = 4;
        public static byte DORD = 5;
        public static byte SPE = 6;
        public static byte SPIE = 7;

        public static byte SPSR { get { return _SFR_IO8(0x2D); } }
        public static byte SPI2X = 0;
        public static byte WCOL = 6;
        public static byte SPIF = 7;

        public static byte SPDR { get { return _SFR_IO8(0x2E); } }
        public static byte SPDR0 = 0;
        public static byte SPDR1 = 1;
        public static byte SPDR2 = 2;
        public static byte SPDR3 = 3;
        public static byte SPDR4 = 4;
        public static byte SPDR5 = 5;
        public static byte SPDR6 = 6;
        public static byte SPDR7 = 7;

        public static byte ACSR { get { return _SFR_IO8(0x30); } }
        public static byte ACIS0 = 0;
        public static byte ACIS1 = 1;
        public static byte ACIC = 2;
        public static byte ACIE = 3;
        public static byte ACI = 4;
        public static byte ACO = 5;
        public static byte ACBG = 6;
        public static byte ACD = 7;


        public static byte SMCR { get { return _SFR_IO8(0x33); } }
        public static byte SE = 0;
        public static byte SM0 = 1;
        public static byte SM1 = 2;
        public static byte SM2 = 3;

        // public static byte MCUSR { get { return _SFR_IO8(0x34); } }
        public static byte PORF = 0;
        public static byte EXTRF = 1;
        public static byte BORF = 2;
        public static byte WDRF = 3;

        //public static byte MCUCSR { get { return _SFR_IO8(0x35); } }
        public static byte IVCE = 0;
        public static byte IVSEL = 1;
        public static byte PUD = 4;
        public static byte BODSE = 5;
        public static byte BODS = 6;

        public static byte SPMCSR { get { return _SFR_IO8(0x37); } }
        public static byte SELFPRGEN = 0; /* only for backwards compatibility with previous
                                         *avr - libc versions; not an official name */
        public static byte SPMEN = 0;
        public static byte PGERS = 1;
        public static byte PGWRT = 2;
        public static byte BLBSET = 3;
        public static byte RWWSRE = 4;
        public static byte SIGRD = 5;
        public static byte RWWSB = 6;
        public static byte SPMIE = 7;

        public static byte WDTCSR { get { return _SFR_MEM8(0x60); } }
        public static byte WDP0 = 0;
        public static byte WDP1 = 1;
        public static byte WDP2 = 2;
        public static byte WDE = 3;
        public static byte WDCE = 4;
        public static byte WDP3 = 5;
        public static byte WDIE = 6;
        public static byte WDIF = 7;

        public static byte CLKPR { get { return _SFR_MEM8(0x61); } }
        public static byte CLKPS0 = 0;
        public static byte CLKPS1 = 1;
        public static byte CLKPS2 = 2;
        public static byte CLKPS3 = 3;
        public static byte CLKPSE = 7;


        public static byte PRR { get { return _SFR_MEM8(0x64); } }
        public static byte PRADC = 0;
        public static byte PRUSART0 = 1;
        public static byte PRSPI = 2;
        public static byte PRTIM1 = 3;
        public static byte PRTIM0 = 5;
        public static byte PRTIM2 = 6;
        public static byte PRTWI = 7;

        public bool __AVR_HAVE_PRR_PRADC = true;
        public bool __AVR_HAVE_PRR_PRUSART0 = true;
        public bool __AVR_HAVE_PRR_PRSPI = true;
        public bool __AVR_HAVE_PRR_PRTIM1 = true;
        public bool __AVR_HAVE_PRR_PRTIM0 = true;
        public bool __AVR_HAVE_PRR_PRTIM2 = true;
        public bool __AVR_HAVE_PRR_PRTWI = true;

        public static byte OSCCAL { get { return _SFR_MEM8(0x66); } }
        public static byte CAL0 = 0;
        public static byte CAL1 = 1;
        public static byte CAL2 = 2;
        public static byte CAL3 = 3;
        public static byte CAL4 = 4;
        public static byte CAL5 = 5;
        public static byte CAL6 = 6;
        public static byte CAL7 = 7;

        public static byte PCICR { get { return _SFR_MEM8(0x68); } }
        public static byte PCIE0 = 0;
        public static byte PCIE1 = 1;
        public static byte PCIE2 = 2;

        public static byte EICRA { get { return _SFR_MEM8(0x69); } }
        public static byte ISC00 = 0;
        public static byte ISC01 = 1;
        public static byte ISC10 = 2;
        public static byte ISC11 = 3;

        public static byte PCMSK0 { get { return _SFR_MEM8(0x6B); } }
        public static byte PCINT0 = 0;
        public static byte PCINT1 = 1;
        public static byte PCINT2 = 2;
        public static byte PCINT3 = 3;
        public static byte PCINT4 = 4;
        public static byte PCINT5 = 5;
        public static byte PCINT6 = 6;
        public static byte PCINT7 = 7;


        public static byte PCMSK1 { get { return _SFR_MEM8(0x6C); } }
        public static byte PCINT8 = 0;
        public static byte PCINT9 = 1;
        public static byte PCINT10 = 2;
        public static byte PCINT11 = 3;
        public static byte PCINT12 = 4;
        public static byte PCINT13 = 5;
        public static byte PCINT14 = 6;
        public static byte PCINT15 = 7;

        public static byte PCMSK2 { get { return _SFR_MEM8(0x6D); } } // было PCMSK1 !!!!!!!!!!!!!!!!!!!!!!!!
        public static byte PCINT16 = 0;
        public static byte PCINT17 = 1;
        public static byte PCINT18 = 2;
        public static byte PCINT19 = 3;
        public static byte PCINT20 = 4;
        public static byte PCINT21 = 5;
        public static byte PCINT22 = 6;
        public static byte PCINT23 = 7;


        public static byte TIMSK0 { get { return _SFR_MEM8(0x6E); } }
        public static byte TOIE0 = 0;
        public static byte OCIE0A = 1;
        public static byte OCIE0B = 2;

        public static byte TIMSK1 { get { return _SFR_MEM8(0x6F); } }
        public static byte TOIE1 = 0;
        public static byte OCIE1A = 1;
        public static byte OCIE1B = 2;
        public static byte ICIE1 = 5;

        public static byte TIMSK2 { get { return _SFR_MEM8(0x70); } }
        public static byte TOIE2 = 0;
        public static byte OCIE2A = 1;
        public static byte OCIE2B = 2;


        public ushort ADC { get { return _SFR_MEM16(0x78); } }
        public ushort ADW { get { return _SFR_MEM16(0x78); } }

        public static byte ADCL { get { return _SFR_MEM8(0x78); } }
        public static byte ADCL0 = 0;
        public static byte ADCL1 = 1;
        public static byte ADCL2 = 2;
        public static byte ADCL3 = 3;
        public static byte ADCL4 = 4;
        public static byte ADCL5 = 5;
        public static byte ADCL6 = 6;
        public static byte ADCL7 = 7;

        public static byte ADCH { get { return _SFR_MEM8(0x79); } }
        public static byte ADCH0 = 0;
        public static byte ADCH1 = 1;
        public static byte ADCH2 = 2;
        public static byte ADCH3 = 3;
        public static byte ADCH4 = 4;
        public static byte ADCH5 = 5;
        public static byte ADCH6 = 6;
        public static byte ADCH7 = 7;

        public static byte ADCSRA { get { return _SFR_MEM8(0x7A); } }
        public static byte ADPS0 = 0;
        public static byte ADPS1 = 1;
        public static byte ADPS2 = 2;
        public static byte ADIE = 3;
        public static byte ADIF = 4;
        public static byte ADATE = 5;
        public static byte ADSC = 6;
        public static byte ADEN = 7;

        public static byte ADCSRB { get { return _SFR_MEM8(0x7B); } }
        public static byte ADTS0 = 0;
        public static byte ADTS1 = 1;
        public static byte ADTS2 = 2;
        public static byte ACME = 6;

        public static byte ADMUX { get { return _SFR_MEM8(0x7C); } }
        public static byte MUX0 = 0;
        public static byte MUX1 = 1;
        public static byte MUX2 = 2;
        public static byte MUX3 = 3;
        public static byte ADLAR = 5;
        public static byte REFS0 = 6;
        public static byte REFS1 = 7;

        public static byte DIDR0 { get { return _SFR_MEM8(0x7E); } }
        public static byte ADC0D = 0;
        public static byte ADC1D = 1;
        public static byte ADC2D = 2;
        public static byte ADC3D = 3;
        public static byte ADC4D = 4;
        public static byte ADC5D = 5;

        public static byte DIDR1 { get { return _SFR_MEM8(0x7F); } }
        public static byte AIN0D = 0;
        public static byte AIN1D = 1;


        public static byte TCCR1A { get { return _SFR_MEM8(0x80); } }
        public static byte WGM10 = 0;
        public static byte WGM11 = 1;
        public static byte COM1B0 = 4;
        public static byte COM1B1 = 5;
        public static byte COM1A0 = 6;
        public static byte COM1A1 = 7;


        public static byte TCCR1B { get { return _SFR_MEM8(0x81); } }
        public static byte CS10 = 0;
        public static byte CS11 = 1;
        public static byte CS12 = 2;
        public static byte WGM12 = 3;
        public static byte WGM13 = 4;
        public static byte ICES1 = 6;
        public static byte ICNC1 = 7;


        public static byte TCCR1C { get { return _SFR_MEM8(0x82); } }
        public static byte FOC1B = 6;
        public static byte FOC1A = 7;

        public ushort TCNT1 { get { return _SFR_MEM16(0x84); } }
        public static byte TCNT1L { get { return _SFR_MEM8(0x84); } }
        public static byte TCNT1L0 = 0;
        public static byte TCNT1L1 = 1;
        public static byte TCNT1L2 = 2;
        public static byte TCNT1L3 = 3;
        public static byte TCNT1L4 = 4;
        public static byte TCNT1L5 = 5;
        public static byte TCNT1L6 = 6;
        public static byte TCNT1L7 = 7;

        public static byte TCNT1H { get { return _SFR_MEM8(0x85); } }
        public static byte TCNT1H0 = 0;
        public static byte TCNT1H1 = 1;
        public static byte TCNT1H2 = 2;
        public static byte TCNT1H3 = 3;
        public static byte TCNT1H4 = 4;
        public static byte TCNT1H5 = 5;
        public static byte TCNT1H6 = 6;
        public static byte TCNT1H7 = 7;

        public ushort ICR1 { get { return _SFR_MEM16(0x86); } }
        public static byte ICR1L { get { return _SFR_MEM8(0x86); } }
        public static byte ICR1L0 = 0;
        public static byte ICR1L1 = 1;
        public static byte ICR1L2 = 2;
        public static byte ICR1L3 = 3;
        public static byte ICR1L4 = 4;
        public static byte ICR1L5 = 5;
        public static byte ICR1L6 = 6;
        public static byte ICR1L7 = 7;

        public static byte ICR1H { get { return _SFR_MEM8(0x87); } }
        public static byte ICR1H0 = 0;
        public static byte ICR1H1 = 1;
        public static byte ICR1H2 = 2;
        public static byte ICR1H3 = 3;
        public static byte ICR1H4 = 4;
        public static byte ICR1H5 = 5;
        public static byte ICR1H6 = 6;
        public static byte ICR1H7 = 7;

        public ushort OCR1A { get { return _SFR_MEM16(0x88); } }
        public static byte OCR1AL { get { return _SFR_MEM8(0x88); } }
        public static byte OCR1AL0 = 0;
        public static byte OCR1AL1 = 1;
        public static byte OCR1AL2 = 2;
        public static byte OCR1AL3 = 3;
        public static byte OCR1AL4 = 4;
        public static byte OCR1AL5 = 5;
        public static byte OCR1AL6 = 6;
        public static byte OCR1AL7 = 7;

        public static byte OCR1AH { get { return _SFR_MEM8(0x89); } }
        public static byte OCR1AH0 = 0;
        public static byte OCR1AH1 = 1;
        public static byte OCR1AH2 = 2;
        public static byte OCR1AH3 = 3;
        public static byte OCR1AH4 = 4;
        public static byte OCR1AH5 = 5;
        public static byte OCR1AH6 = 6;
        public static byte OCR1AH7 = 7;

        public ushort OCR1B { get { return _SFR_MEM16(0x8A); } }
        public static byte OCR1BL { get { return _SFR_MEM8(0x8A); } }
        public static byte OCR1BL0 = 0;
        public static byte OCR1BL1 = 1;
        public static byte OCR1BL2 = 2;
        public static byte OCR1BL3 = 3;
        public static byte OCR1BL4 = 4;
        public static byte OCR1BL5 = 5;
        public static byte OCR1BL6 = 6;
        public static byte OCR1BL7 = 7;

        public static byte OCR1BH { get { return _SFR_MEM8(0x8B); } }
        public static byte OCR1BH0 = 0;
        public static byte OCR1BH1 = 1;
        public static byte OCR1BH2 = 2;
        public static byte OCR1BH3 = 3;
        public static byte OCR1BH4 = 4;
        public static byte OCR1BH5 = 5;
        public static byte OCR1BH6 = 6;
        public static byte OCR1BH7 = 7;

        public static byte TCCR2A { get { return _SFR_MEM8(0xB0); } }
        public static byte WGM20 = 0;
        public static byte WGM21 = 1;
        public static byte COM2B0 = 4;
        public static byte COM2B1 = 5;
        public static byte COM2A0 = 6;
        public static byte COM2A1 = 7;

        public static byte TCCR2B { get { return _SFR_MEM8(0xB1); } }
        public static byte CS20 = 0;
        public static byte CS21 = 1;
        public static byte CS22 = 2;
        public static byte WGM22 = 3;
        public static byte FOC2B = 6;
        public static byte FOC2A = 7;


        public static byte TCNT2 { get { return _SFR_MEM8(0xB2); } }
        public static byte TCNT2_0 = 0;
        public static byte TCNT2_1 = 1;
        public static byte TCNT2_2 = 2;
        public static byte TCNT2_3 = 3;
        public static byte TCNT2_4 = 4;
        public static byte TCNT2_5 = 5;
        public static byte TCNT2_6 = 6;
        public static byte TCNT2_7 = 7;


        public static byte OCR2A { get { return _SFR_MEM8(0xB3); } }
        public static byte OCR2A_0 = 0;
        public static byte OCR2A_1 = 1;
        public static byte OCR2A_2 = 2;
        public static byte OCR2A_3 = 3;
        public static byte OCR2A_4 = 4;
        public static byte OCR2A_5 = 5;
        public static byte OCR2A_6 = 6;
        public static byte OCR2A_7 = 7;

        public static byte OCR2B { get { return _SFR_MEM8(0xB4); } }
        public static byte OCR2B_0 = 0;
        public static byte OCR2B_1 = 1;
        public static byte OCR2B_2 = 2;
        public static byte OCR2B_3 = 3;
        public static byte OCR2B_4 = 4;
        public static byte OCR2B_5 = 5;
        public static byte OCR2B_6 = 6;
        public static byte OCR2B_7 = 7;

        public static byte ASSR { get { return _SFR_MEM8(0xB6); } }
        public static byte TCR2BUB = 0;
        public static byte TCR2AUB = 1;
        public static byte OCR2BUB = 2;
        public static byte OCR2AUB = 3;
        public static byte TCN2UB = 4;
        public static byte AS2 = 5;
        public static byte EXCLK = 6;

        public static byte TWBR { get { return _SFR_MEM8(0xB8); } }
        public static byte TWBR0 = 0;
        public static byte TWBR1 = 1;
        public static byte TWBR2 = 2;
        public static byte TWBR3 = 3;
        public static byte TWBR4 = 4;
        public static byte TWBR5 = 5;
        public static byte TWBR6 = 6;
        public static byte TWBR7 = 7;

        public static byte TWSR { get { return _SFR_MEM8(0xB9); } }
        public static byte TWPS0 = 0;
        public static byte TWPS1 = 1;
        public static byte TWPS2 = 2;
        public static byte TWPS3 = 3;
        public static byte TWPS4 = 4;
        public static byte TWPS5 = 5;
        public static byte TWPS6 = 6;
        public static byte TWPS7 = 7;

        public static byte TWAR { get { return _SFR_MEM8(0xBA); } }
        public static byte TWGCE = 0;
        public static byte TWA0 = 1;
        public static byte TWA1 = 2;
        public static byte TWA2 = 3;
        public static byte TWA3 = 4;
        public static byte TWA4 = 5;
        public static byte TWA5 = 6;
        public static byte TWA6 = 7;

        public static byte TWDR { get { return _SFR_MEM8(0xBB); } }
        public static byte TWD0 = 0;
        public static byte TWD1 = 1;
        public static byte TWD2 = 2;
        public static byte TWD3 = 3;
        public static byte TWD4 = 4;
        public static byte TWD5 = 5;
        public static byte TWD6 = 6;
        public static byte TWD7 = 7;

        public static byte TWCR { get { return _SFR_MEM8(0xBC); } }
        public static byte TWIE = 0;
        public static byte TWEN = 2;
        public static byte TWWC = 3;
        public static byte TWSTO = 4;
        public static byte TWSTA = 5;
        public static byte TWEA = 6;
        public static byte TWINT = 7;


        public static byte TWAMR { get { return _SFR_MEM8(0xBD); } }
        public static byte TWAM0 = 0;
        public static byte TWAM1 = 1;
        public static byte TWAM2 = 2;
        public static byte TWAM3 = 3;
        public static byte TWAM4 = 4;
        public static byte TWAM5 = 5;
        public static byte TWAM6 = 6;
        public static byte TWAM7 = 7;


        public static byte UCSR0A { get { return _SFR_MEM8(0xC0); } }
        public static byte MPCM0 = 0;
        public static byte U2X0 = 1;
        public static byte UPE0 = 2;
        public static byte DOR0 = 3;
        public static byte FE0 = 4;
        public static byte UDRE0 = 5;
        public static byte TXC0 = 6;
        public static byte RXC0 = 7;

        public static byte UCSR0B { get { return _SFR_MEM8(0xC1); } }
        public static byte TXB80 = 0;
        public static byte RXB80 = 1;
        public static byte UCSZ02 = 2;
        public static byte TXEN0 = 3;
        public static byte RXEN0 = 4;
        public static byte UDRIE0 = 5;
        public static byte TXCIE0 = 6;
        public static byte RXCIE0 = 7;

        public static byte UCSR0C { get { return _SFR_MEM8(0xC2); } }
        public static byte UCPOL0 = 0;
        public static byte UCSZ00 = 1;
        public static byte UCPHA0 = 1;
        public static byte UCSZ01 = 2;
        public static byte UDORD0 = 2;
        public static byte USBS0 = 3;
        public static byte UPM00 = 4;
        public static byte UPM01 = 5;
        public static byte UMSEL00 = 6;
        public static byte UMSEL01 = 7;



        public ushort UBRR0 { get { return _SFR_MEM16(0xC4); } }
        public static byte UBRR0L { get { return _SFR_MEM8(0xC4); } }
        public static byte UBRR0_0 = 0;
        public static byte UBRR0_1 = 1;
        public static byte UBRR0_2 = 2;
        public static byte UBRR0_3 = 3;
        public static byte UBRR0_4 = 4;
        public static byte UBRR0_5 = 5;
        public static byte UBRR0_6 = 6;
        public static byte UBRR0_7 = 7;


        public static byte UBRR0H { get { return _SFR_MEM8(0xC5); } }
        public static byte UBRR0_8 = 0;
        public static byte UBRR0_9 = 1;
        public static byte UBRR0_10 = 2;
        public static byte UBRR0_11 = 3;

        public static byte UDR0 { get { return _SFR_MEM8(0xC6); } }
        public static byte UDR0_0 = 0;
        public static byte UDR0_1 = 1;
        public static byte UDR0_2 = 2;
        public static byte UDR0_3 = 3;
        public static byte UDR0_4 = 4;
        public static byte UDR0_5 = 5;
        public static byte UDR0_6 = 6;
        public static byte UDR0_7 = 7;


        /* Interrupt Vectors */
        /* Interrupt Vector 0 is the reset vector. */

        public static byte INT0_vect_num = 1;
        public static byte INT0_vect { get { return _VECTOR(1); } }

        public static byte INT1_vect_num = 2;
        public static byte INT1_vect { get { return _VECTOR(2); } } /* External Interrupt Request 1 */

        public static byte PCINT0_vect_num = 3;
        public static byte PCINT0_vect { get { return _VECTOR(3); } } /* Pin Change Interrupt Request 0 */

        public static byte PCINT1_vect_num = 4;
        public static byte PCINT1_vect { get { return _VECTOR(4); } } /* Pin Change Interrupt Request 0 */

        public static byte PCINT2_vect_num = 5;
        public static byte PCINT2_vect { get { return _VECTOR(5); } } /* Pin Change Interrupt Request 1 */

        public static byte WDT_vect_num = 6;
        public static byte WDT_vect { get { return _VECTOR(6); } } /* Watchdog Time-out Interrupt */

        public static byte TIMER2_COMPA_vect_num = 7;
        public static byte TIMER2_COMPA_vect { get { return _VECTOR(7); } } /* Timer/Counter2 Compare Match A */

        public static byte TIMER2_COMPB_vect_num = 8;
        public static byte TIMER2_COMPB_vect { get { return _VECTOR(8); } } /* Timer/Counter2 Compare Match B */

        public static byte TIMER2_OVF_vect_num = 9;
        public static byte TIMER2_OVF_vect { get { return _VECTOR(9); } } /* Timer/Counter2 Overflow */

        public static byte TIMER2_CAPT_vect_num = 10;
        public static byte TIMER2_CAPT_vect { get { return _VECTOR(10); } } /* Timer/Counter2 Capture event */

        public static byte TIMER1_COMPA_vect_num = 11;
        public static byte TIMER1_COMPA_vect { get { return _VECTOR(11); } } /* Timer/Counter1 Compare Match A */

        public static byte TIMER1_COMPB_vect_num = 12;
        public static byte TIMER1_COMPB_vect { get { return _VECTOR(12); } } /* Timer/Counter1 Compare Match B */

        public static byte TIMER1_OVF_vect_num = 13;
        public static byte TIMER1_OVF_vect { get { return _VECTOR(13); } } /* Timer/Counter1 Overflow */

        public static byte TIMER0_COMPA_vect_num = 14;
        public static byte TIMER0_COMPA_vect { get { return _VECTOR(14); } } /* Timer/Counter0 Compare Match A */

        public static byte TIMER0_COMPB_vect_num = 15;
        public static byte TIMER0_COMPB_vect { get { return _VECTOR(15); } }/* Timer/Counter0 Compare Match B */

        public static byte TIMER0_OVF_vect_num = 16;
        public static byte TIMER0_OVF_vect { get { return _VECTOR(16); } }/* Timer/Counter0 Overflow */

        public static byte SPI_STC_vect_num = 17;
        public static byte SPI_STC_vect { get { return _VECTOR(17); } } /* SPI Serial Transfer Complete */

        public static byte USART_RX_vect_num = 18;
        public static byte USART_RX_vect { get { return _VECTOR(18); } } /* USART Rx Complete */

        public static byte USART_UDRE_vect_num = 19;
        public static byte USART_UDRE_vect { get { return _VECTOR(19); } } /* USART, Data Register Empty */

        public static byte USART_TX_vect_num = 20;
        public static byte USART_TX_vect { get { return _VECTOR(20); } } /* USART Tx Complete */

        public static byte ADC_vect_num = 21;
        public static byte ADC_vect { get { return _VECTOR(21); } } /* ADC Conversion Complete */

        public static byte EE_READY_vect_num = 22;
        public static byte EE_READY_vect { get { return _VECTOR(22); } } /* EEPROM Ready */

        public static byte ANALOG_COMP_vect_num = 23;
        public static byte ANALOG_COMP_vect { get { return _VECTOR(23); } } /* Analog Comparator */

        public static byte TWI_vect_num = 24;
        public static byte TWI_vect { get { return _VECTOR(24); } }/* Two-wire Serial Interface */

        public static byte SPM_READY_vect_num = 25;
        public static byte SPM_READY_vect { get { return _VECTOR(25); } } /* Store Program Memory Read */

        public static byte _VECTORS_SIZE = 26 * 4;




        /* Low Fuse Byte */

        //#define FUSE_CKSEL0 (unsigned char)~_BV(0)  /* Select Clock Source */
        //#define FUSE_CKSEL1 (unsigned char)~_BV(1)  /* Select Clock Source */
        //#define FUSE_CKSEL2 (unsigned char)~_BV(2)  /* Select Clock Source */
        //#define FUSE_CKSEL3 (unsigned char)~_BV(3)  /* Select Clock Source */
        //#define FUSE_SUT0   (unsigned char)~_BV(4)  /* Select start-up time */
        //#define FUSE_SUT1   (unsigned char)~_BV(5)  /* Select start-up time */
        //#define FUSE_CKOUT  (unsigned char)~_BV(6)  /* Clock output */
        //#define FUSE_CKDIV8 (unsigned char)~_BV(7) /* Divide clock by 8 */
        //#define LFUSE_DEFAULT (FUSE_CKSEL0 & FUSE_CKSEL2 & FUSE_CKSEL3 & FUSE_SUT0 & FUSE_CKDIV8)

        //        /* High Fuse Byte */
        //#define FUSE_BOOTRST (unsigned char)~_BV(0)
        //#define FUSE_BOOTSZ0 (unsigned char)~_BV(1)
        //#define FUSE_BOOTSZ1 (unsigned char)~_BV(2)
        //#define FUSE_EESAVE    (unsigned char)~_BV(3)  /* EEPROM memory is preserved through chip erase */
        //#define FUSE_WDTON     (unsigned char)~_BV(4)  /* Watchdog Timer Always On */
        //#define FUSE_SPIEN     (unsigned char)~_BV(5)  /* Enable Serial programming and Data Downloading */
        //#define FUSE_DWEN      (unsigned char)~_BV(6)  /* debugWIRE Enable */
        //#define FUSE_RSTDISBL  (unsigned char)~_BV(7)  /* External reset disable */
        //#define HFUSE_DEFAULT (FUSE_BOOTSZ0 & FUSE_BOOTSZ1 & FUSE_SPIEN)

        //        /* Extended Fuse Byte */
        //#define FUSE_BODLEVEL0 (unsigned char)~_BV(0)  /* Brown-out Detector trigger level */
        //#define FUSE_BODLEVEL1 (unsigned char)~_BV(1)  /* Brown-out Detector trigger level */
        //#define FUSE_BODLEVEL2 (unsigned char)~_BV(2)  /* Brown-out Detector trigger level */
        //#define EFUSE_DEFAULT  (0xFF)



        /* Lock Bits */
        public bool __LOCK_BITS_EXIST = true;
        public bool __BOOT_LOCK_BITS_0_EXIST = true;
        public bool __BOOT_LOCK_BITS_1_EXIST = true;

        public static byte SLEEP_MODE_IDLE = (0x00 << 1);
        public static byte SLEEP_MODE_ADC = (0x01 << 1);
        public static byte SLEEP_MODE_PWR_DOWN = (0x02 << 1);
        public static byte SLEEP_MODE_PWR_SAVE = (0x03 << 1);
        public static byte SLEEP_MODE_STANDBY = (0x06 << 1);
        public static byte SLEEP_MODE_EXT_STANDBY = (0x07 << 1);

        public const byte _AVR_IOM328P_H_ = 1;
        public static string SIM_MMCU;
        public static byte SIM_VECTOR_SIZE;

        public static byte __AVR_HAVE_PRR
        {
            get
            {
                return (byte)((1 << Constants.PRADC) |
                              (1 << Constants.PRUSART0) |
                              (1 << Constants.PRSPI) |
                              (1 << Constants.PRTIM1) |
                              (1 << Constants.PRTIM0) |
                              (1 << Constants.PRTIM2) |
                              (1 << Constants.PRTWI));
            }
        }

    }
}
