using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class ConstantsX328:ConstantsX8
    {
     
        /* Registers and associated bit numbers */
        public static new byte PINB = _SFR_IO8(0x03);
        public static new byte PINB0 = 0;
        public static new byte PINB1 = 1;
        public static new byte PINB2 = 2;
        public static new byte PINB3 = 3;
        public static new byte PINB4 = 4;
        public static new byte PINB5 = 5;
        public static new byte PINB6 = 6;
        public static new byte PINB7 = 7;

        public static new byte DDRB = _SFR_IO8(0x04); 
        public static new byte DDB0 = 0;
        public static new byte DDB1 = 1;
        public static new byte DDB2 = 2;
        public static new byte DDB3 = 3;
        public static new byte DDB4 = 4;
        public static new byte DDB5 = 5;
        public static new byte DDB6 = 6;
        public static new byte DDB7 = 7;

        public static new byte PORTB = _SFR_IO8(0x05); 
        public static new byte PORTB0 = 0;
        public static new byte PORTB1 = 1;
        public static new byte PORTB2 = 2;
        public static new byte PORTB3 = 3;
        public static new byte PORTB4 = 4;
        public static new byte PORTB5 = 5;
        public static new byte PORTB6 = 6;
        public static new byte PORTB7 = 7;

        public static new byte PINC = _SFR_IO8(0x06); 
        public static new byte PINC0 = 0;
        public static new byte PINC1 = 1;
        public static new byte PINC2 = 2;
        public static new byte PINC3 = 3;
        public static new byte PINC4 = 4;
        public static new byte PINC5 = 5;
        public static new byte PINC6 = 6;

        public static new byte DDRC = _SFR_IO8(0x07); 
        public static new byte DDC0 = 0;
        public static new byte DDC1 = 1;
        public static new byte DDC2 = 2;
        public static new byte DDC3 = 3;
        public static new byte DDC4 = 4;
        public static new byte DDC5 = 5;
        public static new byte DDC6 = 6;

        public static new byte PORTC = _SFR_IO8(0x08); 
        public static new byte PORTC0 = 0;
        public static new byte PORTC1 = 1;
        public static new byte PORTC2 = 2;
        public static new byte PORTC3 = 3;
        public static new byte PORTC4 = 4;
        public static new byte PORTC5 = 5;
        public static new byte PORTC6 = 6;

        public static new byte PIND = _SFR_IO8(0x09); 
        public static new byte PIND0 = 0;
        public static new byte PIND1 = 1;
        public static new byte PIND2 = 2;
        public static new byte PIND3 = 3;
        public static new byte PIND4 = 4;
        public static new byte PIND5 = 5;
        public static new byte PIND6 = 6;
        public static new byte PIND7 = 7;

        public static new byte DDRD = _SFR_IO8(0x0A); 
        public static new byte DDD0 = 0;
        public static new byte DDD1 = 1;
        public static new byte DDD2 = 2;
        public static new byte DDD3 = 3;
        public static new byte DDD4 = 4;
        public static new byte DDD5 = 5;
        public static new byte DDD6 = 6;
        public static new byte DDD7 = 7;

        public static new byte PORTD = _SFR_IO8(0x0B); 
        public static new byte PORTD0 = 0;
        public static new byte PORTD1 = 1;
        public static new byte PORTD2 = 2;
        public static new byte PORTD3 = 3;
        public static new byte PORTD4 = 4;
        public static new byte PORTD5 = 5;
        public static new byte PORTD6 = 6;
        public static new byte PORTD7 = 7;

        public static new byte TIFR0 = _SFR_IO8(0x15); 
        public static new byte TOV0 = 0;
        public static new byte OCF0A = 1;
        public static new byte OCF0B = 2;

        public static new byte TIFR1 = _SFR_IO8(0x16); 
        public static new byte TOV1 = 0;
        public static new byte OCF1A = 1;
        public static new byte OCF1B = 2;
        public static new byte ICF1 = 5;

        public static new byte TIFR2 =_SFR_IO8(0x17); 
        public static new byte TOV2 = 0;
        public static new byte OCF2A = 1;
        public static new byte OCF2B = 2;

        public static new byte PCIFR =_SFR_IO8(0x1B); 
        public static new byte PCIF0 = 0;
        public static new byte PCIF1 = 1;
        public static new byte PCIF2 = 2;

        public static new byte EIFR =_SFR_IO8(0x1C); 
        public static new byte INTF0 = 0;
        public static new byte INTF1 = 1;

        public static new byte EIMSK =_SFR_IO8(0x1D); 
        public static new byte INT0 = 0;
        public static new byte INT1 = 1;

        public static new byte GPIOR0 =_SFR_IO8(0x1E); 
        public static new byte GPIOR00 = 0;
        public static new byte GPIOR01 = 1;
        public static new byte GPIOR02 = 2;
        public static new byte GPIOR03 = 3;
        public static new byte GPIOR04 = 4;
        public static new byte GPIOR05 = 5;
        public static new byte GPIOR06 = 6;
        public static new byte GPIOR07 = 7;

        public static new byte EECR =_SFR_IO8(0x1F); 
        public static new byte EERE = 0;
        public static new byte EEPE = 1;
        public static new byte EEMPE = 2;
        public static new byte EERIE = 3;
        public static new byte EEPM0 = 4;
        public static new byte EEPM1 = 5;

        public static new byte EEDR =_SFR_IO8(0x20); 
        public static new byte EEDR0 = 0;
        public static new byte EEDR1 = 1;
        public static new byte EEDR2 = 2;
        public static new byte EEDR3 = 3;
        public static new byte EEDR4 = 4;
        public static new byte EEDR5 = 5;
        public static new byte EEDR6 = 6;
        public static new byte EEDR7 = 7;

        public static new ushort EEAR =_SFR_IO16(0x21); 

        public static new byte EEARL =_SFR_IO8(0x21); 
        public static new byte EEAR0 = 0;
        public static new byte EEAR1 = 1;
        public static new byte EEAR2 = 2;
        public static new byte EEAR3 = 3;
        public static new byte EEAR4 = 4;
        public static new byte EEAR5 = 5;
        public static new byte EEAR6 = 6;
        public static new byte EEAR7 = 7;

        public static new byte EEARH =_SFR_IO8(0x22); 
        public static new byte EEAR8 = 0;
        public static new byte EEAR9 = 1;

        public static new uint _EEPROM_REG_LOCATIONS_ = 0x1F2021;

        public static new byte GTCCR =_SFR_IO8(0x23); 
        public static new byte PSRSYNC = 0;
        public static new byte PSRASY = 1;
        public static new byte TSM = 7;

        public static new byte TCCR0A =_SFR_IO8(0x24); 
        public static new byte WGM00 = 0;
        public static new byte WGM01 = 1;
        public static new byte COM0B0 = 4;
        public static new byte COM0B1 = 5;
        public static new byte COM0A0 = 6;
        public static new byte COM0A1 = 7;

        public static new byte TCCR0B =_SFR_IO8(0x25); 
        public static new byte CS00 = 0;
        public static new byte CS01 = 1;
        public static new byte CS02 = 2;
        public static new byte WGM02 = 3;
        public static new byte FOC0B = 6;
        public static new byte FOC0A = 7;

        public static new byte TCNT0 =_SFR_IO8(0x26); 
        public static new byte TCNT0_0 = 0;
        public static new byte TCNT0_1 = 1;
        public static new byte TCNT0_2 = 2;
        public static new byte TCNT0_3 = 3;
        public static new byte TCNT0_4 = 4;
        public static new byte TCNT0_5 = 5;
        public static new byte TCNT0_6 = 6;
        public static new byte TCNT0_7 = 7;

        public static new byte OCR0A =_SFR_IO8(0x27); 
        public static new byte OCR0A_0 = 0;
        public static new byte OCR0A_1 = 1;
        public static new byte OCR0A_2 = 2;
        public static new byte OCR0A_3 = 3;
        public static new byte OCR0A_4 = 4;
        public static new byte OCR0A_5 = 5;
        public static new byte OCR0A_6 = 6;
        public static new byte OCR0A_7 = 7;

        public static new byte OCR0B =_SFR_IO8(0x28); 
        public static new byte OCR0B_0 = 0;
        public static new byte OCR0B_1 = 1;
        public static new byte OCR0B_2 = 2;
        public static new byte OCR0B_3 = 3;
        public static new byte OCR0B_4 = 4;
        public static new byte OCR0B_5 = 5;
        public static new byte OCR0B_6 = 6;
        public static new byte OCR0B_7 = 7;

        public static new byte GPIOR1 =_SFR_IO8(0x2A); 
        public static new byte GPIOR10 = 0;
        public static new byte GPIOR11 = 1;
        public static new byte GPIOR12 = 2;
        public static new byte GPIOR13 = 3;
        public static new byte GPIOR14 = 4;
        public static new byte GPIOR15 = 5;
        public static new byte GPIOR16 = 6;
        public static new byte GPIOR17 = 7;

        public static new byte GPIOR2 =_SFR_IO8(0x2B); 
        public static new byte GPIOR20 = 0;
        public static new byte GPIOR21 = 1;
        public static new byte GPIOR22 = 2;
        public static new byte GPIOR23 = 3;
        public static new byte GPIOR24 = 4;
        public static new byte GPIOR25 = 5;
        public static new byte GPIOR26 = 6;
        public static new byte GPIOR27 = 7;
    
        public static new byte SPCR =_SFR_IO8(0x2C); 
        public static new byte SPR0 = 0;
        public static new byte SPR1 = 1;
        public static new byte CPHA = 2;
        public static new byte CPOL = 3;
        public static new byte MSTR = 4;
        public static new byte DORD = 5;
        public static new byte SPE = 6;
        public static new byte SPIE = 7;
    
        public static new byte SPSR =_SFR_IO8(0x2D); 
        public static new byte SPI2X = 0;
        public static new byte WCOL = 6;
        public static new byte SPIF = 7;

        public static new byte SPDR =_SFR_IO8(0x2E); 
        public static new byte SPDR0 = 0;
        public static new byte SPDR1 = 1;
        public static new byte SPDR2 = 2;
        public static new byte SPDR3 = 3;
        public static new byte SPDR4 = 4;
        public static new byte SPDR5 = 5;
        public static new byte SPDR6 = 6;
        public static new byte SPDR7 = 7;

        public static new byte ACSR =_SFR_IO8(0x30); 
        public static new byte ACIS0 = 0;
        public static new byte ACIS1 = 1;
        public static new byte ACIC = 2;
        public static new byte ACIE = 3;
        public static new byte ACI = 4;
        public static new byte ACO = 5;
        public static new byte ACBG = 6;
        public static new byte ACD = 7;

        public static new byte SMCR =_SFR_IO8(0x33); 
        public static new byte SE = 0;
        public static new byte SM0 = 1;
        public static new byte SM1 = 2;
        public static new byte SM2 = 3;

        public static new byte MCUSR =_SFR_IO8(0x34); 
        public static new byte PORF = 0;
        public static new byte EXTRF = 1;
        public static new byte BORF = 2;
        public static new byte WDRF = 3;

        public static byte MCUCR =_SFR_IO8(0x35); 
        public static new byte IVCE = 0;
        public static new byte IVSEL = 1;
        public static new byte PUD = 4;
        public static new byte BODSE = 5;
        public static new byte BODS = 6;

        public static new byte SPMCSR =_SFR_IO8(0x37); 
        public static new byte SELFPRGEN = 0; /* only for backwards compatibility with previous
             * avr-libc versions; not an official name */
        public static new byte SPMEN = 0;
        public static new byte PGERS = 1;
        public static new byte PGWRT = 2;
        public static new byte BLBSET = 3;
        public static new byte RWWSRE = 4;
        public static new byte SIGRD = 5;
        public static new byte RWWSB = 6;
        public static new byte SPMIE = 7;

        public static new byte WDTCSR =_SFR_MEM8(0x60); 
        public static new byte WDP0 = 0;
        public static new byte WDP1 = 1;
        public static new byte WDP2 = 2;
        public static new byte WDE = 3;
        public static new byte WDCE = 4;
        public static new byte WDP3 = 5;
        public static new byte WDIE = 6;
        public static new byte WDIF = 7;

        public static new byte CLKPR =_SFR_MEM8(0x61); 
        public static new byte CLKPS0 = 0;
        public static new byte CLKPS1 = 1;
        public static new byte CLKPS2 = 2;
        public static new byte CLKPS3 = 3;
        public static byte CLKPCE = 7;

        public static new byte PRR =_SFR_MEM8(0x64); 
        public static new byte PRADC = 0;
        public static new byte PRUSART0 = 1;
        public static new byte PRSPI = 2;
        public static new byte PRTIM1 = 3;
        public static new byte PRTIM0 = 5;
        public static new byte PRTIM2 = 6;
        public static new byte PRTWI = 7;

        public static new byte __AVR_HAVE_PRR	=(byte)((1<<PRADC) |(1<<PRUSART0) |(1<<PRSPI) |(1<<PRTIM1) |(1<<PRTIM0) |(1<<PRTIM2) |(1<<PRTWI)); 
        public static new byte __AVR_HAVE_PRR_PRADC = 1;
        public static new byte __AVR_HAVE_PRR_PRUSART0 = 1;
        public static new byte __AVR_HAVE_PRR_PRSPI = 1;
        public static new byte __AVR_HAVE_PRR_PRTIM1 = 1;
        public static new byte __AVR_HAVE_PRR_PRTIM0 = 1;
        public static new byte __AVR_HAVE_PRR_PRTIM2 = 1;
        public static new byte __AVR_HAVE_PRR_PRTWI = 1;

        public static new byte OSCCAL =_SFR_MEM8(0x66); 
        public static new byte CAL0 = 0;
        public static new byte CAL1 = 1;
        public static new byte CAL2 = 2;
        public static new byte CAL3 = 3;
        public static new byte CAL4 = 4;
        public static new byte CAL5 = 5;
        public static new byte CAL6 = 6;
        public static new byte CAL7 = 7;

        public static new byte PCICR =_SFR_MEM8(0x68); 
        public static new byte PCIE0 = 0;
        public static new byte PCIE1 = 1;
        public static new byte PCIE2 = 2;

        public static new byte EICRA =_SFR_MEM8(0x69); 
        public static new byte ISC00 = 0;
        public static new byte ISC01 = 1;
        public static new byte ISC10 = 2;
        public static new byte ISC11 = 3;

        public static new byte PCMSK0 =_SFR_MEM8(0x6B); 
        public static new byte PCINT0 = 0;
        public static new byte PCINT1 = 1;
        public static new byte PCINT2 = 2;
        public static new byte PCINT3 = 3;
        public static new byte PCINT4 = 4;
        public static new byte PCINT5 = 5;
        public static new byte PCINT6 = 6;
        public static new byte PCINT7 = 7;

        public static new byte PCMSK1 =_SFR_MEM8(0x6C); 
        public static new byte PCINT8 = 0;
        public static new byte PCINT9 = 1;
        public static new byte PCINT10 = 2;
        public static new byte PCINT11 = 3;
        public static new byte PCINT12 = 4;
        public static new byte PCINT13 = 5;
        public static new byte PCINT14 = 6;

        public static new byte PCMSK2 =_SFR_MEM8(0x6D); 
        public static new byte PCINT16 = 0;
        public static new byte PCINT17 = 1;
        public static new byte PCINT18 = 2;
        public static new byte PCINT19 = 3;
        public static new byte PCINT20 = 4;
        public static new byte PCINT21 = 5;
        public static new byte PCINT22 = 6;
        public static new byte PCINT23 = 7;

        public static new byte TIMSK0 =_SFR_MEM8(0x6E); 
        public static new byte TOIE0 = 0;
        public static new byte OCIE0A = 1;
        public static new byte OCIE0B = 2;
    
        public static new byte TIMSK1 =_SFR_MEM8(0x6F); 
        public static new byte TOIE1 = 0;
        public static new byte OCIE1A = 1;
        public static new byte OCIE1B = 2;
        public static new byte ICIE1 = 5;
    
        public static new byte TIMSK2 =_SFR_MEM8(0x70); 
        public static new byte TOIE2 = 0;
        public static new byte OCIE2A = 1;
        public static new byte OCIE2B = 2;

        public static ushort ADCW = _SFR_MEM16(0x78); 

        public static new byte ADCL =_SFR_MEM8(0x78); 
        public static new byte ADCL0 = 0;
        public static new byte ADCL1 = 1;
        public static new byte ADCL2 = 2;
        public static new byte ADCL3 = 3;
        public static new byte ADCL4 = 4;
        public static new byte ADCL5 = 5;
        public static new byte ADCL6 = 6;
        public static new byte ADCL7 = 7;

        public static new byte ADCH =_SFR_MEM8(0x79); 
        public static new byte ADCH0 = 0;
        public static new byte ADCH1 = 1;
        public static new byte ADCH2 = 2;
        public static new byte ADCH3 = 3;
        public static new byte ADCH4 = 4;
        public static new byte ADCH5 = 5;
        public static new byte ADCH6 = 6;
        public static new byte ADCH7 = 7;
    
        public static new byte ADCSRA =_SFR_MEM8(0x7A); 
        public static new byte ADPS0 = 0;
        public static new byte ADPS1 = 1;
        public static new byte ADPS2 = 2;
        public static new byte ADIE = 3;
        public static new byte ADIF = 4;
        public static new byte ADATE = 5;
        public static new byte ADSC = 6;
        public static new byte ADEN = 7;

        public static new byte ADCSRB =_SFR_MEM8(0x7B); 
        public static new byte ADTS0 = 0;
        public static new byte ADTS1 = 1;
        public static new byte ADTS2 = 2;
        public static new byte ACME = 6;

        public static new byte ADMUX =_SFR_MEM8(0x7C); 
        public static new byte MUX0 = 0;
        public static new byte MUX1 = 1;
        public static new byte MUX2 = 2;
        public static new byte MUX3 = 3;
        public static new byte ADLAR = 5;
        public static new byte REFS0 = 6;
        public static new byte REFS1 = 7;

        public static new byte DIDR0 =_SFR_MEM8(0x7E); 
        public static new byte ADC0D = 0;
        public static new byte ADC1D = 1;
        public static new byte ADC2D = 2;
        public static new byte ADC3D = 3;
        public static new byte ADC4D = 4;
        public static new byte ADC5D = 5;

        public static new byte DIDR1 =_SFR_MEM8(0x7F); 
        public static new byte AIN0D = 0;
        public static new byte AIN1D = 1;

        public static new byte TCCR1A =_SFR_MEM8(0x80); 
        public static new byte WGM10 = 0;
        public static new byte WGM11 = 1;
        public static new byte COM1B0 = 4;
        public static new byte COM1B1 = 5;
        public static new byte COM1A0 = 6;
        public static new byte COM1A1 = 7;

        public static new byte TCCR1B =_SFR_MEM8(0x81); 
        public static new byte CS10 = 0;
        public static new byte CS11 = 1;
        public static new byte CS12 = 2;
        public static new byte WGM12 = 3;
        public static new byte WGM13 = 4;
        public static new byte ICES1 = 6;
        public static new byte ICNC1 = 7;

        public static new byte TCCR1C =_SFR_MEM8(0x82); 
        public static new byte FOC1B = 6;
        public static new byte FOC1A = 7;
    
        public static new ushort TCNT1 =_SFR_MEM16(0x84); 

        public static new byte TCNT1L =_SFR_MEM8(0x84); 
        public static new byte TCNT1L0 = 0;
        public static new byte TCNT1L1 = 1;
        public static new byte TCNT1L2 = 2;
        public static new byte TCNT1L3 = 3;
        public static new byte TCNT1L4 = 4;
        public static new byte TCNT1L5 = 5;
        public static new byte TCNT1L6 = 6;
        public static new byte TCNT1L7 = 7;

        public static new byte TCNT1H =_SFR_MEM8(0x85); 
        public static new byte TCNT1H0 = 0;
        public static new byte TCNT1H1 = 1;
        public static new byte TCNT1H2 = 2;
        public static new byte TCNT1H3 = 3;
        public static new byte TCNT1H4 = 4;
        public static new byte TCNT1H5 = 5;
        public static new byte TCNT1H6 = 6;
        public static new byte TCNT1H7 = 7;

        public static new ushort ICR1 =_SFR_MEM16(0x86); 

        public static new byte ICR1L =_SFR_MEM8(0x86); 
        public static new byte ICR1L0 = 0;
        public static new byte ICR1L1 = 1;
        public static new byte ICR1L2 = 2;
        public static new byte ICR1L3 = 3;
        public static new byte ICR1L4 = 4;
        public static new byte ICR1L5 = 5;
        public static new byte ICR1L6 = 6;
        public static new byte ICR1L7 = 7;

        public static new byte ICR1H =_SFR_MEM8(0x87); 
        public static new byte ICR1H0 = 0;
        public static new byte ICR1H1 = 1;
        public static new byte ICR1H2 = 2;
        public static new byte ICR1H3 = 3;
        public static new byte ICR1H4 = 4;
        public static new byte ICR1H5 = 5;
        public static new byte ICR1H6 = 6;
        public static new byte ICR1H7 = 7;

        public static new ushort OCR1A =_SFR_MEM16(0x88); 

        public static new byte OCR1AL =_SFR_MEM8(0x88); 
        public static new byte OCR1AL0 = 0;
        public static new byte OCR1AL1 = 1;
        public static new byte OCR1AL2 = 2;
        public static new byte OCR1AL3 = 3;
        public static new byte OCR1AL4 = 4;
        public static new byte OCR1AL5 = 5;
        public static new byte OCR1AL6 = 6;
        public static new byte OCR1AL7 = 7;

        public static new byte OCR1AH =_SFR_MEM8(0x89); 
        public static new byte OCR1AH0 = 0;
        public static new byte OCR1AH1 = 1;
        public static new byte OCR1AH2 = 2;
        public static new byte OCR1AH3 = 3;
        public static new byte OCR1AH4 = 4;
        public static new byte OCR1AH5 = 5;
        public static new byte OCR1AH6 = 6;
        public static new byte OCR1AH7 = 7;

        public static new ushort OCR1B =_SFR_MEM16(0x8A); 

        public static new byte OCR1BL =_SFR_MEM8(0x8A); 
        public static new byte OCR1BL0 = 0;
        public static new byte OCR1BL1 = 1;
        public static new byte OCR1BL2 = 2;
        public static new byte OCR1BL3 = 3;
        public static new byte OCR1BL4 = 4;
        public static new byte OCR1BL5 = 5;
        public static new byte OCR1BL6 = 6;
        public static new byte OCR1BL7 = 7;

        public static new byte OCR1BH =_SFR_MEM8(0x8B); 
        public static new byte OCR1BH0 = 0;
        public static new byte OCR1BH1 = 1;
        public static new byte OCR1BH2 = 2;
        public static new byte OCR1BH3 = 3;
        public static new byte OCR1BH4 = 4;
        public static new byte OCR1BH5 = 5;
        public static new byte OCR1BH6 = 6;
        public static new byte OCR1BH7 = 7;

        public static new byte TCCR2A =_SFR_MEM8(0xB0); 
        public static new byte WGM20 = 0;
        public static new byte WGM21 = 1;
        public static new byte COM2B0 = 4;
        public static new byte COM2B1 = 5;
        public static new byte COM2A0 = 6;
        public static new byte COM2A1 = 7;

        public static new byte TCCR2B =_SFR_MEM8(0xB1); 
        public static new byte CS20 = 0;
        public static new byte CS21 = 1;
        public static new byte CS22 = 2;
        public static new byte WGM22 = 3;
        public static new byte FOC2B = 6;
        public static new byte FOC2A = 7;

        public static new byte TCNT2 =_SFR_MEM8(0xB2); 
        public static new byte TCNT2_0 = 0;
        public static new byte TCNT2_1 = 1;
        public static new byte TCNT2_2 = 2;
        public static new byte TCNT2_3 = 3;
        public static new byte TCNT2_4 = 4;
        public static new byte TCNT2_5 = 5;
        public static new byte TCNT2_6 = 6;
        public static new byte TCNT2_7 = 7;

        public static new byte OCR2A =_SFR_MEM8(0xB3); 
        public static new byte OCR2A_0 = 0;
        public static new byte OCR2A_1 = 1;
        public static new byte OCR2A_2 = 2;
        public static new byte OCR2A_3 = 3;
        public static new byte OCR2A_4 = 4;
        public static new byte OCR2A_5 = 5;
        public static new byte OCR2A_6 = 6;
        public static new byte OCR2A_7 = 7;

        public static new byte OCR2B =_SFR_MEM8(0xB4); 
        public static new byte OCR2B_0 = 0;
        public static new byte OCR2B_1 = 1;
        public static new byte OCR2B_2 = 2;
        public static new byte OCR2B_3 = 3;
        public static new byte OCR2B_4 = 4;
        public static new byte OCR2B_5 = 5;
        public static new byte OCR2B_6 = 6;
        public static new byte OCR2B_7 = 7;

        public static new byte ASSR =_SFR_MEM8(0xB6); 
        public static new byte TCR2BUB = 0;
        public static new byte TCR2AUB = 1;
        public static new byte OCR2BUB = 2;
        public static new byte OCR2AUB = 3;
        public static new byte TCN2UB = 4;
        public static new byte AS2 = 5;
        public static new byte EXCLK = 6;

        public static new byte TWBR =_SFR_MEM8(0xB8); 
        public static new byte TWBR0 = 0;
        public static new byte TWBR1 = 1;
        public static new byte TWBR2 = 2;
        public static new byte TWBR3 = 3;
        public static new byte TWBR4 = 4;
        public static new byte TWBR5 = 5;
        public static new byte TWBR6 = 6;
        public static new byte TWBR7 = 7;

        public static new byte TWSR =_SFR_MEM8(0xB9); 
        public static new byte TWPS0 = 0;
        public static new byte TWPS1 = 1;
        public static byte TWS3 = 3;
        public static byte TWS4 = 4;
        public static byte TWS5 = 5;
        public static byte TWS6 = 6;
        public static byte TWS7 = 7;

        public static new byte TWAR =_SFR_MEM8(0xBA); 
        public static new byte TWGCE = 0;
        public static new byte TWA0 = 1;
        public static new byte TWA1 = 2;
        public static new byte TWA2 = 3;
        public static new byte TWA3 = 4;
        public static new byte TWA4 = 5;
        public static new byte TWA5 = 6;
        public static new byte TWA6 = 7;

        public static new byte TWDR =_SFR_MEM8(0xBB); 
        public static new byte TWD0 = 0;
        public static new byte TWD1 = 1;
        public static new byte TWD2 = 2;
        public static new byte TWD3 = 3;
        public static new byte TWD4 = 4;
        public static new byte TWD5 = 5;
        public static new byte TWD6 = 6;
        public static new byte TWD7 = 7;

        public static new byte TWCR =_SFR_MEM8(0xBC); 
        public static new byte TWIE = 0;
        public static new byte TWEN = 2;
        public static new byte TWWC = 3;
        public static new byte TWSTO = 4;
        public static new byte TWSTA = 5;
        public static new byte TWEA = 6;
        public static new byte TWINT = 7;

        public static new byte TWAMR =_SFR_MEM8(0xBD); 
        public static new byte TWAM0 = 0;
        public static new byte TWAM1 = 1;
        public static new byte TWAM2 = 2;
        public static new byte TWAM3 = 3;
        public static new byte TWAM4 = 4;
        public static new byte TWAM5 = 5;
        public static new byte TWAM6 = 6;

        public static new byte UCSR0A =_SFR_MEM8(0xC0); 
        public static new byte MPCM0 = 0;
        public static new byte U2X0 = 1;
        public static new byte UPE0 = 2;
        public static new byte DOR0 = 3;
        public static new byte FE0 = 4;
        public static new byte UDRE0 = 5;
        public static new byte TXC0 = 6;
        public static new byte RXC0 = 7;

        public static new byte UCSR0B =_SFR_MEM8(0xC1); 
        public static new byte TXB80 = 0;
        public static new byte RXB80 = 1;
        public static new byte UCSZ02 = 2;
        public static new byte TXEN0 = 3;
        public static new byte RXEN0 = 4;
        public static new byte UDRIE0 = 5;
        public static new byte TXCIE0 = 6;
        public static new byte RXCIE0 = 7;

        public static new byte UCSR0C =_SFR_MEM8(0xC2); 
        public static new byte UCPOL0 = 0;
        public static new byte UCSZ00 = 1;
        public static new byte UCPHA0 = 1;
        public static new byte UCSZ01 = 2;
        public static new byte UDORD0 = 2;
        public static new byte USBS0 = 3;
        public static new byte UPM00 = 4;
        public static new byte UPM01 = 5;
        public static new byte UMSEL00 = 6;
        public static new byte UMSEL01 = 7;

        public static new ushort UBRR0 =_SFR_MEM16(0xC4); 

        public static new byte UBRR0L =_SFR_MEM8(0xC4); 
        public static new byte UBRR0_0 = 0;
        public static new byte UBRR0_1 = 1;
        public static new byte UBRR0_2 = 2;
        public static new byte UBRR0_3 = 3;
        public static new byte UBRR0_4 = 4;
        public static new byte UBRR0_5 = 5;
        public static new byte UBRR0_6 = 6;
        public static new byte UBRR0_7 = 7;

        public static new byte UBRR0H =_SFR_MEM8(0xC5); 
        public static new byte UBRR0_8 = 0;
        public static new byte UBRR0_9 = 1;
        public static new byte UBRR0_10 = 2;
        public static new byte UBRR0_11 = 3;

        public static new byte UDR0 =_SFR_MEM8(0xC6); 
        public static new byte UDR0_0 = 0;
        public static new byte UDR0_1 = 1;
        public static new byte UDR0_2 = 2;
        public static new byte UDR0_3 = 3;
        public static new byte UDR0_4 = 4;
        public static new byte UDR0_5 = 5;
        public static new byte UDR0_6 = 6;
        public static new byte UDR0_7 = 7;



        /* Interrupt Vectors */
        /* Interrupt Vector = 0; is the reset vector. */

        public static new byte INT0_vect_num     = 1;
        public static new byte INT0_vect= _VECTOR(1);   /* External Interrupt Request = 0; */

        public static new byte INT1_vect_num     = 2;
        public static new byte INT1_vect=         _VECTOR(2) ;   /* External Interrupt Request = 1; */

        public static new byte PCINT0_vect_num   = 3;
        public static new byte PCINT0_vect =      _VECTOR(3) ;   /* Pin Change Interrupt Request = 0; */

        public static new byte PCINT1_vect_num = 4;
        public static new byte PCINT1_vect =      _VECTOR(4) ;   /* Pin Change Interrupt Request = 0; */

        public static new byte PCINT2_vect_num   = 5;
        public static new byte PCINT2_vect = _VECTOR(5) ;   /* Pin Change Interrupt Request = 1; */

        public static new byte WDT_vect_num      = 6;
        public static new byte WDT_vect = _VECTOR(6);   /* Watchdog Time-out Interrupt */

        public static new byte TIMER2_COMPA_vect_num = 7;
        public static new byte TIMER2_COMPA_vect =_VECTOR(7) ;   /* Timer/Counter2 Compare Match A */

        public static new byte TIMER2_COMPB_vect_num =8;
        public static new byte TIMER2_COMPB_vect =_VECTOR(8) ;   /* Timer/Counter2 Compare Match A */

        public static new byte TIMER2_OVF_vect_num  = 9;
        public static new byte TIMER2_OVF_vect  = _VECTOR(9);   /* Timer/Counter2 Overflow */

        public static byte TIMER1_CAPT_vect_num = 10;
        public static byte TIMER1_CAPT_vect = _VECTOR(10) ;  /* Timer/Counter1 Capture Event */

        public static new byte TIMER1_COMPA_vect_num = 11;
        public static new byte TIMER1_COMPA_vect = _VECTOR(11) ;  /* Timer/Counter1 Compare Match A */

        public static new byte TIMER1_COMPB_vect_num = 12;
        public static new byte TIMER1_COMPB_vect = _VECTOR(12);  /* Timer/Counter1 Compare Match B */

        public static new byte TIMER1_OVF_vect_num = 13;
        public static new byte TIMER1_OVF_vect = _VECTOR(13);  /* Timer/Counter1 Overflow */

        public static new byte TIMER0_COMPA_vect_num = 14;
        public static new byte TIMER0_COMPA_vect = _VECTOR(14);  /* TimerCounter0 Compare Match A */

        public static new byte TIMER0_COMPB_vect_num = 15;
        public static new byte TIMER0_COMPB_vect = _VECTOR(15);  /* TimerCounter0 Compare Match B */

        public static new byte TIMER0_OVF_vect_num = 16;
        public static new byte TIMER0_OVF_vect = _VECTOR(16);  /* Timer/Couner0 Overflow */

        public static new byte SPI_STC_vect_num = 17;
        public static new byte SPI_STC_vect = _VECTOR(17);  /* SPI Serial Transfer Complete */

        public static new byte USART_RX_vect_num = 18;
        public static new byte USART_RX_vect = _VECTOR(18);  /* USART Rx Complete */

        public static new byte USART_UDRE_vect_num = 19;
        public static new byte USART_UDRE_vect =  _VECTOR(19);  /* USART, Data Register Empty */

        public static new byte USART_TX_vect_num = 20;
        public static new byte USART_TX_vect =  _VECTOR(20);  /* USART Tx Complete */

        public static new byte ADC_vect_num = 21;
        public static new byte ADC_vect = _VECTOR(21);  /* ADC Conversion Complete */

        public static new byte EE_READY_vect_num = 22;
        public static new byte EE_READY_vect = _VECTOR(22);  /* EEPROM Ready */

        public static new byte ANALOG_COMP_vect_num = 23;
        public static new byte ANALOG_COMP_vect =_VECTOR(23);  /* Analog Comparator */

        public static new byte TWI_vect_num = 24;
        public static new byte TWI_vect = _VECTOR(24);  /* Two-wire Serial Interface */

        public static new byte SPM_READY_vect_num = 25;
        public static new byte SPM_READY_vect = _VECTOR(25);  /* Store Program Memory Read */

        public static new byte _VECTORS_SIZE  = (26 * 4);

        /* Constants */
        public static new ushort SPM_PAGESIZE = 128;
        public static new ushort RAMSTART =    (0x100);
        public static new ushort RAMEND = 0x8FF;     /* Last On-Chip SRAM Location */
        public static new ushort XRAMSIZE     = 0;
        public static new ushort XRAMEND = RAMEND;
        public static new ushort E2END = 0x3FF;
        public static new ushort E2PAGESIZE = 4;
        public static new ushort FLASHEND = 0x7FFF;

        /* Fuses */
        public static new byte FUSE_MEMORY_SIZE = 3;

        /* Low Fuse Byte */
        //public static new byte FUSE_CKSEL0 (unsigned char); ~_BV(0); ;  /* Select Clock Source */
        //public static new byte FUSE_CKSEL1 (unsigned char); ~_BV(1); ;  /* Select Clock Source */
        //public static new byte FUSE_CKSEL2 (unsigned char); ~_BV(2); ;  /* Select Clock Source */
        //public static new byte FUSE_CKSEL3 (unsigned char); ~_BV(3); ;  /* Select Clock Source */
        //public static new byte FUSE_SUT0   (unsigned char); ~_BV(4); ;  /* Select start-up time */
        //public static new byte FUSE_SUT1   (unsigned char); ~_BV(5); ;  /* Select start-up time */
        //public static new byte FUSE_CKOUT  (unsigned char); ~_BV(6); ;  /* Clock output */
        //public static new byte FUSE_CKDIV8 (unsigned char); ~_BV(7); ; /* Divide clock by 8 */
        //public static new byte LFUSE_DEFAULT (FUSE_CKSEL0 & FUSE_CKSEL2 & FUSE_CKSEL3 & FUSE_SUT0 & FUSE_CKDIV8); 

        /* High Fuse Byte */
        //public static new byte FUSE_BOOTRST (unsigned char); ~_BV(0); 
        //public static new byte FUSE_BOOTSZ0 (unsigned char); ~_BV(1); 
        //public static new byte FUSE_BOOTSZ1 (unsigned char); ~_BV(2); 
        //public static new byte FUSE_EESAVE    (unsigned char); ~_BV(3); ;  /* EEPROM memory is preserved through chip erase */
        //public static new byte FUSE_WDTON     (unsigned char); ~_BV(4); ;  /* Watchdog Timer Always On */
        //public static new byte FUSE_SPIEN     (unsigned char); ~_BV(5); ;  /* Enable Serial programming and Data Downloading */
        //public static new byte FUSE_DWEN      (unsigned char); ~_BV(6); ;  /* debugWIRE Enable */
        //public static new byte FUSE_RSTDISBL  (unsigned char); ~_BV(7); ;  /* External reset disable */
        //public static new byte HFUSE_DEFAULT (FUSE_BOOTSZ0 & FUSE_BOOTSZ1 & FUSE_SPIEN); 

        /* Extended Fuse Byte */
        //public static new byte FUSE_BODLEVEL0 (unsigned char); ~_BV(0); ;  /* Brown-out Detector trigger level */
        //public static new byte FUSE_BODLEVEL1 (unsigned char); ~_BV(1); ;  /* Brown-out Detector trigger level */
        //public static new byte FUSE_BODLEVEL2 (unsigned char); ~_BV(2); ;  /* Brown-out Detector trigger level */
        //public static new byte EFUSE_DEFAULT  (0xFF); 

        /* Lock Bits */
        public static new byte __LOCK_BITS_EXIST = 1;
        public static new byte __BOOT_LOCK_BITS_0_EXIST = 1;
        public static new byte __BOOT_LOCK_BITS_1_EXIST = 1;

        /* Signature */
        public static new byte SIGNATURE_0 = 0x1E;
        public static new byte SIGNATURE_1 = 0x95;
        public static new byte SIGNATURE_2 = 0x14;


        public static new byte SLEEP_MODE_IDLE =(0x00<<1); 
        public static new byte SLEEP_MODE_ADC =(0x01<<1); 
        public static new byte SLEEP_MODE_PWR_DOWN =(0x02<<1); 
        public static new byte SLEEP_MODE_PWR_SAVE =(0x03<<1); 
        public static new byte SLEEP_MODE_STANDBY =(0x06<<1); 
        public static new byte SLEEP_MODE_EXT_STANDBY =(0x07<<1); 

    }
}
