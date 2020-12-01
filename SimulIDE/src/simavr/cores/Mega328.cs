using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class Mega328 : Megax8
    {


        public static Mcu Make()
        {
            return new Mega328();
        }

        protected static Avr_kind self = new Avr_kind();
        public static Avr_kind Self() { return self; }
        static Mega328()
        {
            self.Names = new string[2];
            self.Names[0] = "atmega328";
            self.Names[1] = "atmega328p";
            self.Make = Make;
        }


        protected override void InitConstants()
        {
            base.InitConstants();

            SIM_MMCU = "atmega328";
            SIM_CORENAME = "mcu_mega328";
            SIM_VECTOR_SIZE = 4;
            /* Signature */
            SIGNATURE_0 = 0x1E;
            SIGNATURE_1 = 0x95;
            SIGNATURE_2 = 0x0F;

            /* Constants */
            SPM_PAGESIZE = 128;
            RAMSTART = 0x100;
            RAMEND = 0x8FF;     /* Last On-Chip SRAM Location */
            XRAMSIZE = 0;
            XRAMEND = RAMEND;
            E2END = 0x3FF;
            E2PAGESIZE = 4;
            FLASHEND = 0x7FFF;

            EEARH=_SFR_IO8(0x22);
            EEARL=_SFR_IO8(0x21);
            EEDR=_SFR_IO8(0x20);
            EECR=_SFR_IO8(0x0F);
            EERE = 0;
            EEPE = 1;
            EEMPE = 2;
            EERIE = 3;
            EEPM0 = 4;
            EEPM1 = 5;

            /* Fuses */
            FUSE_MEMORY_SIZE = 3;

        //#define SLEEP_MODE_IDLE (0x00<<1)
        //#define SLEEP_MODE_ADC (0x01<<1)
        //#define SLEEP_MODE_PWR_DOWN (0x02<<1)
        //#define SLEEP_MODE_PWR_SAVE (0x03<<1)
        //#define SLEEP_MODE_STANDBY (0x06<<1)
        //#define SLEEP_MODE_EXT_STANDBY (0x07<<1)

        }

        public byte __AVR_HAVE_PRR
        {
            get
            {
                return (byte)((1 << PRADC) |
                              (1 << PRUSART0) |
                              (1 << PRSPI) |
                              (1 << PRTIM1) |
                              (1 << PRTIM0) |
                              (1 << PRTIM2) |
                              (1 << PRTWI));
            }
        }

        public string SIM_CORENAME;

        /* Registers and associated bit numbers */
        public byte PINB { get { return _SFR_IO8(0x03); } }
        public byte PINB0 = 0;
        public byte PINB1 = 1;
        public byte PINB2 = 2;
        public byte PINB3 = 3;
        public byte PINB4 = 4;
        public byte PINB5 = 5;
        public byte PINB6 = 6;
        public byte PINB7 = 7;

        public byte DDRB { get { return _SFR_IO8(0x04); } }
        public byte DDB0 = 0;
        public byte DDB1 = 1;
        public byte DDB2 = 2;
        public byte DDB3 = 3;
        public byte DDB4 = 4;
        public byte DDB5 = 5;
        public byte DDB6 = 6;
        public byte DDB7 = 7;

        public byte PORTB { get { return _SFR_IO8(0x05); } }
        public byte PORTB0 = 0;
        public byte PORTB1 = 1;
        public byte PORTB2 = 2;
        public byte PORTB3 = 3;
        public byte PORTB4 = 4;
        public byte PORTB5 = 5;
        public byte PORTB6 = 6;
        public byte PORTB7 = 7;

        public byte PINC { get { return _SFR_IO8(0x06); } }
        public byte PINC0 = 0;
        public byte PINC1 = 1;
        public byte PINC2 = 2;
        public byte PINC3 = 3;
        public byte PINC4 = 4;
        public byte PINC5 = 5;
        public byte PINC6 = 6;

        public byte DDRC { get { return _SFR_IO8(0x07); } }
        public byte DDC0 = 0;
        public byte DDC1 = 1;
        public byte DDC2 = 2;
        public byte DDC3 = 3;
        public byte DDC4 = 4;
        public byte DDC5 = 5;
        public byte DDC6 = 6;

        public byte PORTC { get { return _SFR_IO8(0x08); } }
        public byte PORTC0 = 0;
        public byte PORTC1 = 1;
        public byte PORTC2 = 2;
        public byte PORTC3 = 3;
        public byte PORTC4 = 4;
        public byte PORTC5 = 5;
        public byte PORTC6 = 6;

        public byte PIND { get { return _SFR_IO8(0x09); } }
        public byte PIND0 = 0;
        public byte PIND1 = 1;
        public byte PIND2 = 2;
        public byte PIND3 = 3;
        public byte PIND4 = 4;
        public byte PIND5 = 5;
        public byte PIND6 = 6;
        public byte PIND7 = 7;

        public byte DDRD { get { return _SFR_IO8(0x0A); } }
        public byte DDD0 = 0;
        public byte DDD1 = 1;
        public byte DDD2 = 2;
        public byte DDD3 = 3;
        public byte DDD4 = 4;
        public byte DDD5 = 5;
        public byte DDD6 = 6;
        public byte DDD7 = 7;

        public byte PORTD { get { return _SFR_IO8(0x0B); } }
        public byte PORTD0 = 0;
        public byte PORTD1 = 1;
        public byte PORTD2 = 2;
        public byte PORTD3 = 3;
        public byte PORTD4 = 4;
        public byte PORTD5 = 5;
        public byte PORTD6 = 6;
        public byte PORTD7 = 7;

        public byte TIFR0 { get { return _SFR_IO8(0x15); } }
        public byte TOV0 = 0;
        public byte OCF0A = 1;
        public byte OCF0B = 2;


        public byte TIFR1 { get { return _SFR_IO8(0x16); } }
        public byte TOV1 = 0;
        public byte OCF1A = 1;
        public byte OCF1B = 2;
        public byte ICF1 = 5;

        public byte TIFR2 { get { return _SFR_IO8(0x17); } }
        public byte TOV2 = 0;
        public byte OCF2A = 1;
        public byte OCF2B = 2;

        public byte PCIFR { get { return _SFR_IO8(0x1B); } }
        public byte PCIF0 = 0;
        public byte PCIF1 = 1;
        public byte PCIF2 = 2;


        public byte EIFR { get { return _SFR_IO8(0x1C); } }
        public byte INTF0 = 0;
        public byte INTF1 = 1;

        public byte EIMSK { get { return _SFR_IO8(0x1D); } }
        public byte INT0 = 0;
        public byte INT1 = 1;

        public byte GPIOR0 { get { return _SFR_IO8(0x0E); } }
        public byte GPIOR00 = 0;
        public byte GPIOR01 = 1;
        public byte GPIOR02 = 2;
        public byte GPIOR03 = 3;
        public byte GPIOR04 = 4;
        public byte GPIOR05 = 5;
        public byte GPIOR06 = 6;
        public byte GPIOR07 = 7;

        public byte EEDR0 = 0;
        public byte EEDR1 = 1;
        public byte EEDR2 = 2;
        public byte EEDR3 = 3;
        public byte EEDR4 = 4;
        public byte EEDR5 = 5;
        public byte EEDR6 = 6;
        public byte EEDR7 = 7;

        public ushort EEAR { get { return _SFR_IO16(0x21); } }

        
        public byte EEAR0 = 0;
        public byte EEAR1 = 1;
        public byte EEAR2 = 2;
        public byte EEAR3 = 3;
        public byte EEAR4 = 4;
        public byte EEAR5 = 5;
        public byte EEAR6 = 6;
        public byte EEAR7 = 7;

        
        public byte EEAR8 = 0;
        public byte EEAR9 = 1;


        public UInt32 _EEPROM_REG_LOCATIONS_ = 0x1F2021;

        public byte GTCCR { get { return _SFR_IO8(0x23); } }
        public byte PSRSYNC = 0;
        public byte PSRASY = 1;
        public byte TSM = 7;

        public byte TCCR0A { get { return _SFR_IO8(0x24); } }
        public byte WGM00 = 0;
        public byte WGM01 = 1;
        public byte COM0B0 = 4;
        public byte COM0B1 = 5;
        public byte COM0A0 = 6;
        public byte COM0A1 = 7;


        public byte TCCR0B { get { return _SFR_IO8(0x25); } }
        public byte CS00 = 0;
        public byte CS01 = 1;
        public byte CS02 = 2;
        public byte WGM02 = 3;
        public byte FOC0B = 6;
        public byte FOC0A = 7;

        public byte TCNT0 { get { return _SFR_IO8(0x26); } }
        public byte TCNT0_0 = 0;
        public byte TCNT0_1 = 1;
        public byte TCNT0_2 = 2;
        public byte TCNT0_3 = 3;
        public byte TCNT0_4 = 4;
        public byte TCNT0_5 = 5;
        public byte TCNT0_6 = 6;
        public byte TCNT0_7 = 7;

        public byte OCR0A { get { return _SFR_IO8(0x27); } }
        public byte OCR0A_0 = 0;
        public byte OCR0A_1 = 1;
        public byte OCR0A_2 = 2;
        public byte OCR0A_3 = 3;
        public byte OCR0A_4 = 4;
        public byte OCR0A_5 = 5;
        public byte OCR0A_6 = 6;
        public byte OCR0A_7 = 7;

        public byte OCR0B { get { return _SFR_IO8(0x28); } }
        public byte OCR0B_0 = 0;
        public byte OCR0B_1 = 1;
        public byte OCR0B_2 = 2;
        public byte OCR0B_3 = 3;
        public byte OCR0B_4 = 4;
        public byte OCR0B_5 = 5;
        public byte OCR0B_6 = 6;
        public byte OCR0B_7 = 7;

        public byte GPIOR1 { get { return _SFR_IO8(0x2A); } }
        public byte GPIOR10 = 0;
        public byte GPIOR11 = 1;
        public byte GPIOR12 = 2;
        public byte GPIOR13 = 3;
        public byte GPIOR14 = 4;
        public byte GPIOR15 = 5;
        public byte GPIOR16 = 6;
        public byte GPIOR17 = 7;

        public byte GPIOR2 { get { return _SFR_IO8(0x2B); } }
        public byte GPIOR20 = 0;
        public byte GPIOR21 = 1;
        public byte GPIOR22 = 2;
        public byte GPIOR23 = 3;
        public byte GPIOR24 = 4;
        public byte GPIOR25 = 5;
        public byte GPIOR26 = 6;
        public byte GPIOR27 = 7;

        public byte SPCR { get { return _SFR_IO8(0x2C); } }
        public byte SPR0 = 0;
        public byte SPR1 = 1;
        public byte CPHA = 2;
        public byte CPOL = 3;
        public byte MSTR = 4;
        public byte DORD = 5;
        public byte SPE = 6;
        public byte SPIE = 7;

        public byte SPSR { get { return _SFR_IO8(0x2D); } }
        public byte SPI2X = 0;
        public byte WCOL = 6;
        public byte SPIF = 7;

        public byte SPDR { get { return _SFR_IO8(0x2E); } }
        public byte SPDR0 = 0;
        public byte SPDR1 = 1;
        public byte SPDR2 = 2;
        public byte SPDR3 = 3;
        public byte SPDR4 = 4;
        public byte SPDR5 = 5;
        public byte SPDR6 = 6;
        public byte SPDR7 = 7;

        public byte ACSR { get { return _SFR_IO8(0x30); } }
        public byte ACIS0 = 0;
        public byte ACIS1 = 1;
        public byte ACIC = 2;
        public byte ACIE = 3;
        public byte ACI = 4;
        public byte ACO = 5;
        public byte ACBG = 6;
        public byte ACD = 7;


        public byte SMCR { get { return _SFR_IO8(0x33); } }
        public byte SE = 0;
        public byte SM0 = 1;
        public byte SM1 = 2;
        public byte SM2 = 3;

        public byte MCUSR { get { return _SFR_IO8(0x34); } }
        public byte PORF = 0;
        public byte EXTRF = 1;
        public byte BORF = 2;
        public byte WDRF = 3;

        public byte MCUCR { get { return _SFR_IO8(0x35); } }
        public byte IVCE = 0;
        public byte IVSEL = 1;
        public byte PUD = 4;
        public byte BODSE = 5;
        public byte BODS = 6;

        public byte SPMCSR { get { return _SFR_IO8(0x37); } }
        public byte SELFPRGEN = 0; /* only for backwards compatibility with previous
                                         *avr - libc versions; not an official name */
        public byte SPMEN = 0;
        public byte PGERS = 1;
        public byte PGWRT = 2;
        public byte BLBSET = 3;
        public byte RWWSRE = 4;
        public byte SIGRD = 5;
        public byte RWWSB = 6;
        public byte SPMIE = 7;

        public byte WDTCSR { get { return _SFR_MEM8(0x60); } }
        public byte WDP0 = 0;
        public byte WDP1 = 1;
        public byte WDP2 = 2;
        public byte WDE = 3;
        public byte WDCE = 4;
        public byte WDP3 = 5;
        public byte WDIE = 6;
        public byte WDIF = 7;

        public byte CLKPR { get { return _SFR_MEM8(0x61); } }
        public byte CLKPS0 = 0;
        public byte CLKPS1 = 1;
        public byte CLKPS2 = 2;
        public byte CLKPS3 = 3;
        public byte CLKPSE = 7;


        public byte PRR { get { return _SFR_MEM8(0x64); } }
        public byte PRADC = 0;
        public byte PRUSART0 = 1;
        public byte PRSPI = 2;
        public byte PRTIM1 = 3;
        public byte PRTIM0 = 5;
        public byte PRTIM2 = 6;
        public byte PRTWI = 7;

        public bool __AVR_HAVE_PRR_PRADC = true;
        public bool __AVR_HAVE_PRR_PRUSART0 = true;
        public bool __AVR_HAVE_PRR_PRSPI = true;
        public bool __AVR_HAVE_PRR_PRTIM1 = true;
        public bool __AVR_HAVE_PRR_PRTIM0 = true;
        public bool __AVR_HAVE_PRR_PRTIM2 = true;
        public bool __AVR_HAVE_PRR_PRTWI = true;

        public byte OSCCAL { get { return _SFR_MEM8(0x66); } }
        public byte CAL0 = 0;
        public byte CAL1 = 1;
        public byte CAL2 = 2;
        public byte CAL3 = 3;
        public byte CAL4 = 4;
        public byte CAL5 = 5;
        public byte CAL6 = 6;
        public byte CAL7 = 7;

        public byte PCICR { get { return _SFR_MEM8(0x68); } }
        public byte PCIE0 = 0;
        public byte PCIE1 = 1;
        public byte PCIE2 = 2;

        public byte EICRA { get { return _SFR_MEM8(0x69); } }
        public byte ISC00 = 0;
        public byte ISC01 = 1;
        public byte ISC10 = 2;
        public byte ISC11 = 3;

        public byte PCMSK0 { get { return _SFR_MEM8(0x6B); } }
        public byte PCINT0 = 0;
        public byte PCINT1 = 1;
        public byte PCINT2 = 2;
        public byte PCINT3 = 3;
        public byte PCINT4 = 4;
        public byte PCINT5 = 5;
        public byte PCINT6 = 6;
        public byte PCINT7 = 7;


        public byte PCMSK1 { get { return _SFR_MEM8(0x6C); } }
        public byte PCINT8 = 0;
        public byte PCINT9 = 1;
        public byte PCINT10 = 2;
        public byte PCINT11 = 3;
        public byte PCINT12 = 4;
        public byte PCINT13 = 5;
        public byte PCINT14 = 6;
        public byte PCINT15 = 7;

        public byte PCMSK2 { get { return _SFR_MEM8(0x6D); } } // было PCMSK1 !!!!!!!!!!!!!!!!!!!!!!!!
        public byte PCINT16 = 0;
        public byte PCINT17 = 1;
        public byte PCINT18 = 2;
        public byte PCINT19 = 3;
        public byte PCINT20 = 4;
        public byte PCINT21 = 5;
        public byte PCINT22 = 6;
        public byte PCINT23 = 7;


        public byte TIMSK0 { get { return _SFR_MEM8(0x6E); } }
        public byte TOIE0 = 0;
        public byte OCIE0A = 1;
        public byte OCIE0B = 2;

        public byte TIMSK1 { get { return _SFR_MEM8(0x6F); } }
        public byte TOIE1 = 0;
        public byte OCIE1A = 1;
        public byte OCIE1B = 2;
        public byte ICIE1 = 5;

        public byte TIMSK2 { get { return _SFR_MEM8(0x70); } }
        public byte TOIE2 = 0;
        public byte OCIE2A = 1;
        public byte OCIE2B = 2;


        public ushort ADC { get { return _SFR_MEM16(0x78); } }
        public ushort ADW { get { return _SFR_MEM16(0x78); } }

        public byte ADCL { get { return _SFR_MEM8(0x78); } }
        public byte ADCL0 = 0;
        public byte ADCL1 = 1;
        public byte ADCL2 = 2;
        public byte ADCL3 = 3;
        public byte ADCL4 = 4;
        public byte ADCL5 = 5;
        public byte ADCL6 = 6;
        public byte ADCL7 = 7;

        public byte ADCH { get { return _SFR_MEM8(0x79); } }
        public byte ADCH0 = 0;
        public byte ADCH1 = 1;
        public byte ADCH2 = 2;
        public byte ADCH3 = 3;
        public byte ADCH4 = 4;
        public byte ADCH5 = 5;
        public byte ADCH6 = 6;
        public byte ADCH7 = 7;

        public byte ADCSRA { get { return _SFR_MEM8(0x7A); } }
        public byte ADPS0 = 0;
        public byte ADPS1 = 1;
        public byte ADPS2 = 2;
        public byte ADIE = 3;
        public byte ADIF = 4;
        public byte ADATE = 5;
        public byte ADSC = 6;
        public byte ADEN = 7;

        public byte ADCSRB { get { return _SFR_MEM8(0x7B); } }
        public byte ADTS0 = 0;
        public byte ADTS1 = 1;
        public byte ADTS2 = 2;
        public byte ACME = 6;

        public byte ADMUX { get { return _SFR_MEM8(0x7C); } }
        public byte MUX0 = 0;
        public byte MUX1 = 1;
        public byte MUX2 = 2;
        public byte MUX3 = 3;
        public byte ADLAR = 5;
        public byte REFS0 = 6;
        public byte REFS1 = 7;

        public byte DIDR0 { get { return _SFR_MEM8(0x7E); } }
        public byte ADC0D = 0;
        public byte ADC1D = 1;
        public byte ADC2D = 2;
        public byte ADC3D = 3;
        public byte ADC4D = 4;
        public byte ADC5D = 5;

        public byte DIDR1 { get { return _SFR_MEM8(0x7F); } }
        public byte AIN0D = 0;
        public byte AIN1D = 1;


        public byte TCCR1A { get { return _SFR_MEM8(0x80); } }
        public byte WGM10 = 0;
        public byte WGM11 = 1;
        public byte COM1B0 = 4;
        public byte COM1B1 = 5;
        public byte COM1A0 = 6;
        public byte COM1A1 = 7;


        public byte TCCR1B { get { return _SFR_MEM8(0x81); } }
        public byte CS10 = 0;
        public byte CS11 = 1;
        public byte CS12 = 2;
        public byte WGM12 = 3;
        public byte WGM13 = 4;
        public byte ICES1 = 6;
        public byte ICNC1 = 7;


        public byte TCCR1C { get { return _SFR_MEM8(0x82); } }
        public byte FOC1B = 6;
        public byte FOC1A = 7;

        public ushort TCNT1 { get { return _SFR_MEM16(0x84); } }
        public byte TCNT1L { get { return _SFR_MEM8(0x84); } }
        public byte TCNT1L0 = 0;
        public byte TCNT1L1 = 1;
        public byte TCNT1L2 = 2;
        public byte TCNT1L3 = 3;
        public byte TCNT1L4 = 4;
        public byte TCNT1L5 = 5;
        public byte TCNT1L6 = 6;
        public byte TCNT1L7 = 7;

        public byte TCNT1H { get { return _SFR_MEM8(0x85); } }
        public byte TCNT1H0 = 0;
        public byte TCNT1H1 = 1;
        public byte TCNT1H2 = 2;
        public byte TCNT1H3 = 3;
        public byte TCNT1H4 = 4;
        public byte TCNT1H5 = 5;
        public byte TCNT1H6 = 6;
        public byte TCNT1H7 = 7;

        public ushort ICR1 { get { return _SFR_MEM16(0x86); } }
        public byte ICR1L { get { return _SFR_MEM8(0x86); } }
        public byte ICR1L0 = 0;
        public byte ICR1L1 = 1;
        public byte ICR1L2 = 2;
        public byte ICR1L3 = 3;
        public byte ICR1L4 = 4;
        public byte ICR1L5 = 5;
        public byte ICR1L6 = 6;
        public byte ICR1L7 = 7;

        public byte ICR1H { get { return _SFR_MEM8(0x87); } }
        public byte ICR1H0 = 0;
        public byte ICR1H1 = 1;
        public byte ICR1H2 = 2;
        public byte ICR1H3 = 3;
        public byte ICR1H4 = 4;
        public byte ICR1H5 = 5;
        public byte ICR1H6 = 6;
        public byte ICR1H7 = 7;

        public ushort OCR1A { get { return _SFR_MEM16(0x88); } }
        public byte OCR1AL { get { return _SFR_MEM8(0x88); } }
        public byte OCR1AL0 = 0;
        public byte OCR1AL1 = 1;
        public byte OCR1AL2 = 2;
        public byte OCR1AL3 = 3;
        public byte OCR1AL4 = 4;
        public byte OCR1AL5 = 5;
        public byte OCR1AL6 = 6;
        public byte OCR1AL7 = 7;

        public byte OCR1AH { get { return _SFR_MEM8(0x89); } }
        public byte OCR1AH0 = 0;
        public byte OCR1AH1 = 1;
        public byte OCR1AH2 = 2;
        public byte OCR1AH3 = 3;
        public byte OCR1AH4 = 4;
        public byte OCR1AH5 = 5;
        public byte OCR1AH6 = 6;
        public byte OCR1AH7 = 7;

        public ushort OCR1B { get { return _SFR_MEM16(0x8A); } }
        public byte OCR1BL { get { return _SFR_MEM8(0x8A); } }
        public byte OCR1BL0 = 0;
        public byte OCR1BL1 = 1;
        public byte OCR1BL2 = 2;
        public byte OCR1BL3 = 3;
        public byte OCR1BL4 = 4;
        public byte OCR1BL5 = 5;
        public byte OCR1BL6 = 6;
        public byte OCR1BL7 = 7;

        public byte OCR1BH { get { return _SFR_MEM8(0x8B); } }
        public byte OCR1BH0 = 0;
        public byte OCR1BH1 = 1;
        public byte OCR1BH2 = 2;
        public byte OCR1BH3 = 3;
        public byte OCR1BH4 = 4;
        public byte OCR1BH5 = 5;
        public byte OCR1BH6 = 6;
        public byte OCR1BH7 = 7;

        public byte TCCR2A { get { return _SFR_MEM8(0xB0); } }
        public byte WGM20 = 0;
        public byte WGM21 = 1;
        public byte COM2B0 = 4;
        public byte COM2B1 = 5;
        public byte COM2A0 = 6;
        public byte COM2A1 = 7;

        public byte TCCR2B { get { return _SFR_MEM8(0xB1); } }
        public byte CS20 = 0;
        public byte CS21 = 1;
        public byte CS22 = 2;
        public byte WGM22 = 3;
        public byte FOC2B = 6;
        public byte FOC2A = 7;


        public byte TCNT2 { get { return _SFR_MEM8(0xB2); } }
        public byte TCNT2_0 = 0;
        public byte TCNT2_1 = 1;
        public byte TCNT2_2 = 2;
        public byte TCNT2_3 = 3;
        public byte TCNT2_4 = 4;
        public byte TCNT2_5 = 5;
        public byte TCNT2_6 = 6;
        public byte TCNT2_7 = 7;


        public byte OCR2A { get { return _SFR_MEM8(0xB3); } }
        public byte OCR2A_0 = 0;
        public byte OCR2A_1 = 1;
        public byte OCR2A_2 = 2;
        public byte OCR2A_3 = 3;
        public byte OCR2A_4 = 4;
        public byte OCR2A_5 = 5;
        public byte OCR2A_6 = 6;
        public byte OCR2A_7 = 7;

        public byte OCR2B { get { return _SFR_MEM8(0xB4); } }
        public byte OCR2B_0 = 0;
        public byte OCR2B_1 = 1;
        public byte OCR2B_2 = 2;
        public byte OCR2B_3 = 3;
        public byte OCR2B_4 = 4;
        public byte OCR2B_5 = 5;
        public byte OCR2B_6 = 6;
        public byte OCR2B_7 = 7;

        public byte ASSR { get { return _SFR_MEM8(0xB6); } }
        public byte TCR2BUB = 0;
        public byte TCR2AUB = 1;
        public byte OCR2BUB = 2;
        public byte OCR2AUB = 3;
        public byte TCN2UB = 4;
        public byte AS2 = 5;
        public byte EXCLK = 6;

        public byte TWBR { get { return _SFR_MEM8(0xB8); } }
        public byte TWBR0 = 0;
        public byte TWBR1 = 1;
        public byte TWBR2 = 2;
        public byte TWBR3 = 3;
        public byte TWBR4 = 4;
        public byte TWBR5 = 5;
        public byte TWBR6 = 6;
        public byte TWBR7 = 7;

        public byte TWSR { get { return _SFR_MEM8(0xB9); } }
        public byte TWPS0 = 0;
        public byte TWPS1 = 1;
        public byte TWPS2 = 2;
        public byte TWPS3 = 3;
        public byte TWPS4 = 4;
        public byte TWPS5 = 5;
        public byte TWPS6 = 6;
        public byte TWPS7 = 7;

        public byte TWAR { get { return _SFR_MEM8(0xBA); } }
        public byte TWGCE = 0;
        public byte TWA0 = 1;
        public byte TWA1 = 2;
        public byte TWA2 = 3;
        public byte TWA3 = 4;
        public byte TWA4 = 5;
        public byte TWA5 = 6;
        public byte TWA6 = 7;

        public byte TWDR { get { return _SFR_MEM8(0xBB); } }
        public byte TWD0 = 0;
        public byte TWD1 = 1;
        public byte TWD2 = 2;
        public byte TWD3 = 3;
        public byte TWD4 = 4;
        public byte TWD5 = 5;
        public byte TWD6 = 6;
        public byte TWD7 = 7;

        public byte TWCR { get { return _SFR_MEM8(0xBC); } }
        public byte TWIE = 0;
        public byte TWEN = 2;
        public byte TWWC = 3;
        public byte TWSTO = 4;
        public byte TWSTA = 5;
        public byte TWEA = 6;
        public byte TWINT = 7;


        public byte TWAMR { get { return _SFR_MEM8(0xBD); } }
        public byte TWAM0 = 0;
        public byte TWAM1 = 1;
        public byte TWAM2 = 2;
        public byte TWAM3 = 3;
        public byte TWAM4 = 4;
        public byte TWAM5 = 5;
        public byte TWAM6 = 6;
        public byte TWAM7 = 7;


        public byte UCSR0A { get { return _SFR_MEM8(0xC0); } }
        public byte MPCM0 = 0;
        public byte U2X0 = 1;
        public byte UPE0 = 2;
        public byte DOR0 = 3;
        public byte FE0 = 4;
        public byte UDRE0 = 5;
        public byte TXC0 = 6;
        public byte RXC0 = 7;

        public byte UCSR0B { get { return _SFR_MEM8(0xC1); } }
        public byte TXB80 = 0;
        public byte RXB80 = 1;
        public byte UCSZ02 = 2;
        public byte TXEN0 = 3;
        public byte RXEN0 = 4;
        public byte UDRIE0 = 5;
        public byte TXCIE0 = 6;
        public byte RXCIE0 = 7;

        public byte UCSR0C { get { return _SFR_MEM8(0xC2); } }
        public byte UCPOL0 = 0;
        public byte UCSZ00 = 1;
        public byte UCPHA0 = 1;
        public byte UCSZ01 = 2;
        public byte UDORD0 = 2;
        public byte USBS0 = 3;
        public byte UPM00 = 4;
        public byte UPM01 = 5;
        public byte UMSEL00 = 6;
        public byte UMSEL01 = 7;



        public ushort UBRR0 { get { return _SFR_MEM16(0xC4); } }
        public byte UBRR0L { get { return _SFR_MEM8(0xC4); } }
        public byte UBRR0_0 = 0;
        public byte UBRR0_1 = 1;
        public byte UBRR0_2 = 2;
        public byte UBRR0_3 = 3;
        public byte UBRR0_4 = 4;
        public byte UBRR0_5 = 5;
        public byte UBRR0_6 = 6;
        public byte UBRR0_7 = 7;


        public byte UBRR0H { get { return _SFR_MEM8(0xC5); } }
        public byte UBRR0_8 = 0;
        public byte UBRR0_9 = 1;
        public byte UBRR0_10 = 2;
        public byte UBRR0_11 = 3;

        public byte UDR0 { get { return _SFR_MEM8(0xC6); } }
        public byte UDR0_0 = 0;
        public byte UDR0_1 = 1;
        public byte UDR0_2 = 2;
        public byte UDR0_3 = 3;
        public byte UDR0_4 = 4;
        public byte UDR0_5 = 5;
        public byte UDR0_6 = 6;
        public byte UDR0_7 = 7;


        /* Interrupt Vectors */
        /* Interrupt Vector 0 is the reset vector. */

        public byte INT0_vect_num = 1;
        public byte INT0_vect { get { return _VECTOR(1); } }

        public byte INT1_vect_num = 2;
        public byte INT1_vect { get { return _VECTOR(2); } } /* External Interrupt Request 1 */

        public byte PCINT0_vect_num = 3;
        public byte PCINT0_vect { get { return _VECTOR(3); } } /* Pin Change Interrupt Request 0 */

        public byte PCINT1_vect_num = 4;
        public byte PCINT1_vect { get { return _VECTOR(4); } } /* Pin Change Interrupt Request 0 */

        public byte PCINT2_vect_num = 5;
        public byte PCINT2_vect { get { return _VECTOR(5); } } /* Pin Change Interrupt Request 1 */

        public byte WDT_vect_num = 6;
        public byte WDT_vect { get { return _VECTOR(6); } } /* Watchdog Time-out Interrupt */

        public byte TIMER2_COMPA_vect_num = 7;
        public byte TIMER2_COMPA_vect { get { return _VECTOR(7); } } /* Timer/Counter2 Compare Match A */

        public byte TIMER2_COMPB_vect_num = 8;
        public byte TIMER2_COMPB_vect { get { return _VECTOR(8); } } /* Timer/Counter2 Compare Match B */

        public byte TIMER2_OVF_vect_num = 9;
        public byte TIMER2_OVF_vect { get { return _VECTOR(9); } } /* Timer/Counter2 Overflow */

        public byte TIMER2_CAPT_vect_num = 10;
        public byte TIMER2_CAPT_vect { get { return _VECTOR(10); } } /* Timer/Counter2 Capture event */

        public byte TIMER1_COMPA_vect_num = 11;
        public byte TIMER1_COMPA_vect { get { return _VECTOR(11); } } /* Timer/Counter1 Compare Match A */

        public byte TIMER1_COMPB_vect_num = 12;
        public byte TIMER1_COMPB_vect { get { return _VECTOR(12); } } /* Timer/Counter1 Compare Match B */

        public byte TIMER1_OVF_vect_num = 13;
        public byte TIMER1_OVF_vect { get { return _VECTOR(13); } } /* Timer/Counter1 Overflow */

        public byte TIMER0_COMPA_vect_num = 14;
        public byte TIMER0_COMPA_vect { get { return _VECTOR(14); } } /* Timer/Counter0 Compare Match A */

        public byte TIMER0_COMPB_vect_num = 15;
        public byte TIMER0_COMPB_vect { get { return _VECTOR(15); } }/* Timer/Counter0 Compare Match B */

        public byte TIMER0_OVF_vect_num = 16;
        public byte TIMER0_OVF_vect { get { return _VECTOR(16); } }/* Timer/Counter0 Overflow */

        public byte SPI_STC_vect_num = 17;
        public byte SPI_STC_vect { get { return _VECTOR(17); } } /* SPI Serial Transfer Complete */

        public byte USART_RX_vect_num = 18;
        public byte USART_RX_vect { get { return _VECTOR(18); } } /* USART Rx Complete */

        public byte USART_UDRE_vect_num = 19;
        public byte USART_UDRE_vect { get { return _VECTOR(19); } } /* USART, Data Register Empty */

        public byte USART_TX_vect_num = 20;
        public byte USART_TX_vect { get { return _VECTOR(20); } } /* USART Tx Complete */

        public byte ADC_vect_num = 21;
        public byte ADC_vect { get { return _VECTOR(21); } } /* ADC Conversion Complete */

        public byte EE_READY_vect_num = 22;
        public byte EE_READY_vect { get { return _VECTOR(22); } } /* EEPROM Ready */

        public byte ANALOG_COMP_vect_num = 23;
        public byte ANALOG_COMP_vect { get { return _VECTOR(23); } } /* Analog Comparator */

        public byte TWI_vect_num = 24;
        public byte TWI_vect { get { return _VECTOR(24); } }/* Two-wire Serial Interface */

        public byte SPM_READY_vect_num = 25;
        public byte SPM_READY_vect { get { return _VECTOR(25); } } /* Store Program Memory Read */

        public byte _VECTORS_SIZE = 26*4;


     

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



        public const byte _AVR_IOM328P_H_ = 1;

    }
}
