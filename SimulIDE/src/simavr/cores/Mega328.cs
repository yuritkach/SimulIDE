using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class Mega328:Megax8
    {

        public const string SIM_MMCU = "atmega328";
        public const string SIM_CORENAME = "mcu_mega328";

        public static Mcu Make()
        {
            var mega328 = new Mega328();
            return (Mcu) mega328;
            //return avr_core_allocate(mcu_mega328.core);
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

            constants["_AVR_IOM328P_H_"] = 1;
            constants["SIM_MMCU"] = "atmega328";
            constants["SIM_VECTOR_SIZE"] = 4;

            /* Registers and associated bit numbers */
            constants["PINB"] = new DefFunc(_SFR_IO8, new object[1]{0x03});
            constants["PINB0"] = 0;
            constants["PINB1"] = 1;
            constants["PINB2"] = 2;
            constants["PINB3"] = 3;
            constants["PINB4"] = 4;
            constants["PINB5"] = 5;
            constants["PINB6"] = 6;
            constants["PINB7"] = 7;

            constants["DDRB"] = new DefFunc(_SFR_IO8, new object[1] { 0x04 });
            constants["DDB0"] = 0;
            constants["DDB1"] = 1;
            constants["DDB2"] = 2;
            constants["DDB3"] = 3;
            constants["DDB4"] = 4;
            constants["DDB5"] = 5;
            constants["DDB6"] = 6;
            constants["DDB7"] = 7;

            constants["PORTB"] = new DefFunc(_SFR_IO8, new object[1] { 0x05 });
            constants["PORTB0"] = 0;
            constants["PORTB1"] = 1;
            constants["PORTB2"] = 2;
            constants["PORTB3"] = 3;
            constants["PORTB4"] = 4;
            constants["PORTB5"] = 5;
            constants["PORTB6"] = 6;
            constants["PORTB7"] = 7;

            constants["PINC"] = new DefFunc(_SFR_IO8, new object[1] { 0x06 });
            constants["PINC0"] = 0;
            constants["PINC1"] = 1;
            constants["PINC2"] = 2;
            constants["PINC3"] = 3;
            constants["PINC4"] = 4;
            constants["PINC5"] = 5;
            constants["PINC6"] = 6;

            constants["DDRC"] = new DefFunc(_SFR_IO8, new object[1] { 0x07 });
            constants["DDC0"] = 0;
            constants["DDC1"] = 1;
            constants["DDC2"] = 2;
            constants["DDC3"] = 3;
            constants["DDC4"] = 4;
            constants["DDC5"] = 5;
            constants["DDC6"] = 6;

            constants["PORTC"] = new DefFunc(_SFR_IO8, new object[1] { 0x08 });
            constants["PORTC0"] = 0;
            constants["PORTC1"] = 1;
            constants["PORTC2"] = 2;
            constants["PORTC3"] = 3;
            constants["PORTC4"] = 4;
            constants["PORTC5"] = 5;
            constants["PORTC6"] = 6;

            constants["PIND"] = new DefFunc(_SFR_IO8, new object[1] { 0x09 });
            constants["PIND0"] = 0;
            constants["PIND1"] = 1;
            constants["PIND2"] = 2;
            constants["PIND3"] = 3;
            constants["PIND4"] = 4;
            constants["PIND5"] = 5;
            constants["PIND6"] = 6;
            constants["PIND7"] = 7;

            constants["DDRD"] = new DefFunc(_SFR_IO8, new object[1] { 0x0A });
            constants["DDD0"] = 0;
            constants["DDD1"] = 1;
            constants["DDD2"] = 2;
            constants["DDD3"] = 3;
            constants["DDD4"] = 4;
            constants["DDD5"] = 5;
            constants["DDD6"] = 6;
            constants["DDD7"] = 7;

            constants["PORTD"] = new DefFunc(_SFR_IO8, new object[1] { 0x0B });
            constants["PORTD0"] = 0;
            constants["PORTD1"] = 1;
            constants["PORTD2"] = 2;
            constants["PORTD3"] = 3;
            constants["PORTD4"] = 4;
            constants["PORTD5"] = 5;
            constants["PORTD6"] = 6;
            constants["PORTD7"] = 7;

            constants["TIFR0"] = new DefFunc(_SFR_IO8, new object[1] { 0x15 });
            constants["TOV0"] = 0;
            constants["OCF0A"] = 1;
            constants["OCF0B"] = 2;


            constants["TIFR1"] = new DefFunc(_SFR_IO8, new object[1] { 0x16 });
            constants["TOV1"] = 0;
            constants["OCF1A"] = 1;
            constants["OCF1B"] = 2;
            constants["ICF1"] = 5;

            constants["TIFR2"] = new DefFunc(_SFR_IO8, new object[1] { 0x17 });
            constants["TOV2"] = 0;
            constants["OCF2A"] = 1;
            constants["OCF2B"] = 2;

            constants["PCIFR"] = new DefFunc(_SFR_IO8, new object[1] { 0x1B });
            constants["PCIF0"] = 0;
            constants["PCIF1"] = 1;
            constants["PCIF2"] = 2;


            constants["EIFR"] = new DefFunc(_SFR_IO8, new object[1] { 0x1C });
            constants["INTF0"] = 0;
            constants["INTF1"] = 1;

            constants["EIMSK"] = new DefFunc(_SFR_IO8, new object[1] { 0x1D });
            constants["INT0"] = 0;
            constants["INT1"] = 1;

            constants["GPIOR0"] = new DefFunc(_SFR_IO8, new object[1] { 0x0E });
            constants["GPIOR00"] = 0;
            constants["GPIOR01"] = 1;
            constants["GPIOR02"] = 2;
            constants["GPIOR03"] = 3;
            constants["GPIOR04"] = 4;
            constants["GPIOR05"] = 5;
            constants["GPIOR06"] = 6;
            constants["GPIOR07"] = 7;

            constants["EECR"] = new DefFunc(_SFR_IO8, new object[1] { 0x0F });
            constants["EERE"] = 0;
            constants["EEPE"] = 1;
            constants["EEMPE"] = 2;
            constants["EERIE"] = 3;
            constants["EEPM0"] = 4;
            constants["EEPM1"] = 5;


            constants["EEDR"] = new DefFunc(_SFR_IO8, new object[1] { 0x20 });
            constants["EEDR0"] = 0;
            constants["EEDR1"] = 1;
            constants["EEDR2"] = 2;
            constants["EEDR3"] = 3;
            constants["EEDR4"] = 4;
            constants["EEDR5"] = 5;
            constants["EEDR6"] = 6;
            constants["EEDR7"] = 7;

            constants["EEAR"] = new DefFunc(_SFR_IO16, new object[1] { 0x21 });

            constants["EEARL"] = new DefFunc(_SFR_IO8, new object[1] { 0x21 });
            constants["EEAR0"] = 0;
            constants["EEAR1"] = 1;
            constants["EEAR2"] = 2;
            constants["EEAR3"] = 3;
            constants["EEAR4"] = 4;
            constants["EEAR5"] = 5;
            constants["EEAR6"] = 6;
            constants["EEAR7"] = 7;

            constants["EEARH"] = new DefFunc(_SFR_IO8, new object[1] { 0x22 });
            constants["EEAR8"] = 0;
            constants["EEAR9"] = 1;


            constants["_EEPROM_REG_LOCATIONS_"] = 0x1F2021;

            constants["GTCCR"] = new DefFunc(_SFR_IO8, new object[1] { 0x23 });
            constants["PSRSYNC"] = 0;
            constants["PSRASY"] = 1;
            constants["TSM"] = 7;

            constants["TCCR0A"] = new DefFunc(_SFR_IO8, new object[1] { 0x24 });
            constants["WGM00"] = 0;
            constants["WGM01"] = 1;

            constants["COM0B0"] = 4;
            constants["COM0B1"] = 5;
            constants["COM0A0"] = 6;
            constants["COM0A1"] = 7;


            constants["TCCR0B"] = new DefFunc(_SFR_IO8, new object[1] { 0x25 });
            constants["CS00"] = 0;
            constants["CS01"] = 1;
            constants["CS02"] = 2;
            constants["WGM02"] = 3;
            constants["FOC0B"] = 6;
            constants["FOC0A"] = 7;

            constants["TCNT0"] = new DefFunc(_SFR_IO8, new object[1] { 0x26 });
            constants["TCNT0_0"] = 0;
            constants["TCNT0_1"] = 1;
            constants["TCNT0_2"] = 2;
            constants["TCNT0_3"] = 3;
            constants["TCNT0_4"] = 4;
            constants["TCNT0_5"] = 5;
            constants["TCNT0_6"] = 6;
            constants["TCNT0_7"] = 7;

            constants["OCR0A"] = new DefFunc(_SFR_IO8, new object[1] { 0x27 });
            constants["OCR0A_0"] = 0;
            constants["OCR0A_1"] = 1;
            constants["OCR0A_2"] = 2;
            constants["OCR0A_3"] = 3;
            constants["OCR0A_4"] = 4;
            constants["OCR0A_5"] = 5;
            constants["OCR0A_6"] = 6;
            constants["OCR0A_7"] = 7;

            constants["OCR0B"] = new DefFunc(_SFR_IO8, new object[1] { 0x28 });
            constants["OCR0B_0"] = 0;
            constants["OCR0B_1"] = 1;
            constants["OCR0B_2"] = 2;
            constants["OCR0B_3"] = 3;
            constants["OCR0B_4"] = 4;
            constants["OCR0B_5"] = 5;
            constants["OCR0B_6"] = 6;
            constants["OCR0B_7"] = 7;

            constants["GPIOR1"] = new DefFunc(_SFR_IO8, new object[1] { 0x2A });
            constants["GPIOR10"] = 0;
            constants["GPIOR11"] = 1;
            constants["GPIOR12"] = 2;
            constants["GPIOR13"] = 3;
            constants["GPIOR14"] = 4;
            constants["GPIOR15"] = 5;
            constants["GPIOR16"] = 6;
            constants["GPIOR17"] = 7;

            constants["GPIOR2"] = new DefFunc(_SFR_IO8, new object[1] { 0x2B });
            constants["GPIOR20"] = 0;
            constants["GPIOR21"] = 1;
            constants["GPIOR22"] = 2;
            constants["GPIOR23"] = 3;
            constants["GPIOR24"] = 4;
            constants["GPIOR25"] = 5;
            constants["GPIOR26"] = 6;
            constants["GPIOR27"] = 7;

            constants["SPCR"] = new DefFunc(_SFR_IO8, new object[1] { 0x2C });
            constants["SPR0"] = 0;
            constants["SPR1"] = 1;
            constants["CPHA"] = 2;
            constants["CPOL"] = 3;
            constants["MSTR"] = 4;
            constants["DORD"] = 5;
            constants["SPE"] = 6;
            constants["SPIE"] = 7;

            constants["SPSR"] = new DefFunc(_SFR_IO8, new object[1] { 0x2D });
            constants["SPI2X"] = 0;
            constants["WCOL"] = 6;
            constants["SPIF"] = 7;

            constants["SPDR"] = new DefFunc(_SFR_IO8, new object[1] { 0x2E });
            constants["SPDR0"] = 0;
            constants["SPDR1"] = 1;
            constants["SPDR2"] = 2;
            constants["SPDR3"] = 3;
            constants["SPDR4"] = 4;
            constants["SPDR5"] = 5;
            constants["SPDR6"] = 6;
            constants["SPDR7"] = 7;

            constants["ACSR"] = new DefFunc(_SFR_IO8, new object[1] { 0x30 });
            constants["ACIS0"] = 0;
            constants["ACIS1"] = 1;
            constants["ACIC"] = 2;
            constants["ACIE"] = 3;
            constants["ACI"] = 4;
            constants["ACO"] = 5;
            constants["ACBG"] = 6;
            constants["ACD"] = 7;


            constants["SMCR"] = new DefFunc(_SFR_IO8, new object[1] { 0x33 });
            constants["SE"] = 0;
            constants["SM0"] = 1;
            constants["SM1"] = 2;
            constants["SM2"] = 3;

            constants["MCUSR"] = new DefFunc(_SFR_IO8, new object[1] { 0x34 });
            constants["PORF"] = 0;
            constants["EXTRF"] = 1;
            constants["BORF"] = 2;
            constants["WDRF"] = 3;

            constants["MCUCR"] = new DefFunc(_SFR_IO8, new object[1] { 0x35 });
            constants["IVCE"] = 0;
            constants["IVSEL"] = 1;
            constants["PUD"] = 4;
            constants["BODSE"] = 5;
            constants["BODS"] = 6;

            constants["SPMCSR"] = new DefFunc(_SFR_IO8, new object[1] { 0x37 });
            constants["SELFPRGEN"] = 0; /* only for backwards compatibility with previous
                                         *avr - libc versions; not an official name */
            constants["SPMEN"] = 0;
            constants["PGERS"] = 1;
            constants["PGWRT"] = 2;
            constants["BLBSET"] = 3;
            constants["RWWSRE"] = 4;
            constants["SIGRD"] = 5;
            constants["RWWSB"] = 6;
            constants["SPMIE"] = 7;

            constants["WDTCSR"] = new DefFunc(_SFR_MEM8, new object[1] { 0x60 });
            constants["WDP0"] = 0;
            constants["WDP1"] = 1;
            constants["WDP2"] = 2;
            constants["WDE"] = 3;
            constants["WDCE"] = 4;
            constants["WDP3"] = 5;
            constants["WDIE"] = 6;
            constants["WDIF"] = 7;

            constants["CLKPR"] = new DefFunc(_SFR_MEM8, new object[1] { 0x61 });
            constants["CLKPS0"] = 0;
            constants["CLKPS1"] = 1;
            constants["CLKPS2"] = 2;
            constants["CLKPS3"] = 3;
            constants["CLKPSE"] = 7;


            constants["PRR"] = new DefFunc(_SFR_MEM8, new object[1] { 0x64 });
            constants["PRADC"] = 0;
            constants["PRUSART0"] = 1;
            constants["PRSPI"] = 2;
            constants["PRTIM1"] = 3;
            constants["PRTIM0"] = 5;
            constants["PRTIM2"] = 6;
            constants["PRTWI"] = 7;


            constants["__AVR_HAVE_PRR"] = new DefFunc(Get__AVR_HAVE_PRR, null);
            constants["__AVR_HAVE_PRR_PRADC"] = true;
            constants["__AVR_HAVE_PRR_PRUSART0"] = true;
            constants["__AVR_HAVE_PRR_PRSPI"] = true;
            constants["__AVR_HAVE_PRR_PRTIM1"] = true;
            constants["__AVR_HAVE_PRR_PRTIM0"] = true;
            constants["__AVR_HAVE_PRR_PRTIM2"] = true;
            constants["__AVR_HAVE_PRR_PRTWI"] = true;

            constants["OSCCAL"] = new DefFunc(_SFR_MEM8, new object[1] { 0x66 });
            constants["CAL0"] = 0;
            constants["CAL1"] = 1;
            constants["CAL2"] = 2;
            constants["CAL3"] = 3;
            constants["CAL4"] = 4;
            constants["CAL5"] = 5;
            constants["CAL6"] = 6;
            constants["CAL7"] = 7;

            constants["PCICR"] = new DefFunc(_SFR_MEM8, new object[1] { 0x68 });
            constants["PCIE0"] = 0;
            constants["PCIE1"] = 1;
            constants["PCIE2"] = 2;

            constants["EICRA"] = new DefFunc(_SFR_MEM8, new object[1] { 0x69 });
            constants["ISC00"] = 0;
            constants["ISC01"] = 1;
            constants["ISC10"] = 2;
            constants["ISC11"] = 3;

            constants["PCMSK0"] = new DefFunc(_SFR_MEM8, new object[1] { 0x6B });
            constants["PCINT0"] = 0;
            constants["PCINT1"] = 1;
            constants["PCINT2"] = 2;
            constants["PCINT3"] = 3;
            constants["PCINT4"] = 4;
            constants["PCINT5"] = 5;
            constants["PCINT6"] = 6;
            constants["PCINT7"] = 7;


            constants["PCMSK1"] = new DefFunc(_SFR_MEM8, new object[1] { 0x6C });
            constants["PCINT8"] = 0;
            constants["PCINT9"] = 1;
            constants["PCINT10"] = 2;
            constants["PCINT11"] = 3;
            constants["PCINT12"] = 4;
            constants["PCINT13"] = 5;
            constants["PCINT14"] = 6;
            constants["PCINT15"] = 7;

            constants["PCMSK1"] = new DefFunc(_SFR_MEM8, new object[1] { 0x6D });
            constants["PCINT16"] = 0;
            constants["PCINT17"] = 1;
            constants["PCINT18"] = 2;
            constants["PCINT19"] = 3;
            constants["PCINT20"] = 4;
            constants["PCINT21"] = 5;
            constants["PCINT22"] = 6;
            constants["PCINT23"] = 7;


            constants["TIMSK0"] = new DefFunc(_SFR_MEM8, new object[1] { 0x6E });
            constants["TOIE0"] = 0;
            constants["OCIE0A"] = 1;
            constants["OCIE0B"] = 2;

            constants["TIMSK1"] = new DefFunc(_SFR_MEM8, new object[1] { 0x6F });
            constants["TOIE1"] = 0;
            constants["OCIE1A"] = 1;
            constants["OCIE1B"] = 2;
            constants["ICIE1"] = 5;

            constants["TIMSK2"] = new DefFunc(_SFR_MEM8, new object[1] { 0x70 });
            constants["TOIE2"] = 0;
            constants["OCIE2A"] = 1;
            constants["OCIE2B"] = 2;


            constants["ADC"] = new DefFunc(_SFR_MEM16, new object[1] { 0x78 });
            constants["ADW"] = new DefFunc(_SFR_MEM16, new object[1] { 0x78 });

            constants["ADCL"] = new DefFunc(_SFR_MEM8, new object[1] { 0x78 });
            constants["ADCL0"] = 0;
            constants["ADCL1"] = 1;
            constants["ADCL2"] = 2;
            constants["ADCL3"] = 3;
            constants["ADCL4"] = 4;
            constants["ADCL5"] = 5;
            constants["ADCL6"] = 6;
            constants["ADCL7"] = 7;

            constants["ADCH"] = new DefFunc(_SFR_MEM8, new object[1] { 0x79 });
            constants["ADCH0"] = 0;
            constants["ADCH1"] = 1;
            constants["ADCH2"] = 2;
            constants["ADCH3"] = 3;
            constants["ADCH4"] = 4;
            constants["ADCH5"] = 5;
            constants["ADCH6"] = 6;
            constants["ADCH7"] = 7;

            constants["ADCSRA"] = new DefFunc(_SFR_MEM8, new object[1] { 0x7A });
            constants["ADPS0"] = 0;
            constants["ADPS1"] = 1;
            constants["ADPS2"] = 2;
            constants["ADIE"] = 3;
            constants["ADIF"] = 4;
            constants["ADATE"] = 5;
            constants["ADSC"] = 6;
            constants["ADEN"] = 7;

            constants["ADCSRB"] = new DefFunc(_SFR_MEM8, new object[1] { 0x7B });
            constants["ADTS0"] = 0;
            constants["ADTS1"] = 1;
            constants["ADTS2"] = 2;
            constants["ACME"] = 6;

            constants["ADMUX"] = new DefFunc(_SFR_MEM8, new object[1] { 0x7C });
            constants["MUX0"] = 0;
            constants["MUX1"] = 1;
            constants["MUX2"] = 2;
            constants["MUX3"] = 3;
            constants["ADLAR"] = 5;
            constants["REFS0"] = 6;
            constants["REFS1"] = 7;

            constants["DIDR0"] = new DefFunc(_SFR_MEM8, new object[1] { 0x7E });
            constants["ADC0D"] = 0;
            constants["ADC1D"] = 1;
            constants["ADC2D"] = 2;
            constants["ADC3D"] = 3;
            constants["ADC4D"] = 4;
            constants["ADC5D"] = 5;

            constants["DIDR1"] = new DefFunc(_SFR_MEM8, new object[1] { 0x7F });
            constants["AIN0D"] = 0;
            constants["AIN1D"] = 1;


            constants["TCCR1A"] = new DefFunc(_SFR_MEM8, new object[1] { 0x80 });
            constants["WGM10"] = 0;
            constants["WGM11"] = 1;
            constants["COM1B0"] = 4;
            constants["COM1B1"] = 5;
            constants["COM1A0"] = 6;
            constants["COM1A1"] = 7;


            constants["TCCR1B"] = new DefFunc(_SFR_MEM8, new object[1] { 0x81 });
            constants["CS10"] = 0;
            constants["CS11"] = 1;
            constants["CS12"] = 2;
            constants["WGM12"] = 3;
            constants["WGM13"] = 4;
            constants["ICES1"] = 6;
            constants["ICNC1"] = 7;


            constants["TCCR1C"] = new DefFunc(_SFR_MEM8, new object[1] { 0x82 });
            constants["FOC1B"] = 6;
            constants["FOC1A"] = 7;

            constants["TCNT1"] = new DefFunc(_SFR_MEM16, new object[1] { 0x84 });
            constants["TCNT1L"] = new DefFunc(_SFR_MEM8, new object[1] { 0x84 });
            constants["TCNT1L0"] = 0;
            constants["TCNT1L1"] = 1;
            constants["TCNT1L2"] = 2;
            constants["TCNT1L3"] = 3;
            constants["TCNT1L4"] = 4;
            constants["TCNT1L5"] = 5;
            constants["TCNT1L6"] = 6;
            constants["TCNT1L7"] = 7;

            constants["TCNT1H"] = new DefFunc(_SFR_MEM8, new object[1] { 0x85 });
            constants["TCNT1H0"] = 0;
            constants["TCNT1H1"] = 1;
            constants["TCNT1H2"] = 2;
            constants["TCNT1H3"] = 3;
            constants["TCNT1H4"] = 4;
            constants["TCNT1H5"] = 5;
            constants["TCNT1H6"] = 6;
            constants["TCNT1H7"] = 7;

            constants["ICR1"] = new DefFunc(_SFR_MEM16, new object[1] { 0x86 });
            constants["ICR1L"] = new DefFunc(_SFR_MEM8, new object[1] { 0x86 });
            constants["ICR1L0"] = 0;
            constants["ICR1L1"] = 1;
            constants["ICR1L2"] = 2;
            constants["ICR1L3"] = 3;
            constants["ICR1L4"] = 4;
            constants["ICR1L5"] = 5;
            constants["ICR1L6"] = 6;
            constants["ICR1L7"] = 7;

            constants["ICR1H"] = new DefFunc(_SFR_MEM8, new object[1] { 0x87 });
            constants["ICR1H0"] = 0;
            constants["ICR1H1"] = 1;
            constants["ICR1H2"] = 2;
            constants["ICR1H3"] = 3;
            constants["ICR1H4"] = 4;
            constants["ICR1H5"] = 5;
            constants["ICR1H6"] = 6;
            constants["ICR1H7"] = 7;




            constants["OCR1A"] = new DefFunc(_SFR_MEM16, new object[1] { 0x88 });
            constants["OCR1AL"] = new DefFunc(_SFR_MEM8, new object[1] { 0x88 });
            constants["OCR1AL0"] = 0;
            constants["OCR1AL1"] = 1;
            constants["OCR1AL2"] = 2;
            constants["OCR1AL3"] = 3;
            constants["OCR1AL4"] = 4;
            constants["OCR1AL5"] = 5;
            constants["OCR1AL6"] = 6;
            constants["OCR1AL7"] = 7;

            constants["OCR1AH"] = new DefFunc(_SFR_MEM8, new object[1] { 0x89 });
            constants["OCR1AH0"] = 0;
            constants["OCR1AH1"] = 1;
            constants["OCR1AH2"] = 2;
            constants["OCR1AH3"] = 3;
            constants["OCR1AH4"] = 4;
            constants["OCR1AH5"] = 5;
            constants["OCR1AH6"] = 6;
            constants["OCR1AH7"] = 7;

            constants["OCR1B"] = new DefFunc(_SFR_MEM16, new object[1] { 0x8A });
            constants["OCR1BL"] = new DefFunc(_SFR_MEM8, new object[1] { 0x8A });
            constants["OCR1BL0"] = 0;
            constants["OCR1BL1"] = 1;
            constants["OCR1BL2"] = 2;
            constants["OCR1BL3"] = 3;
            constants["OCR1BL4"] = 4;
            constants["OCR1BL5"] = 5;
            constants["OCR1BL6"] = 6;
            constants["OCR1BL7"] = 7;

            constants["OCR1BH"] = new DefFunc(_SFR_MEM8, new object[1] { 0x8B });
            constants["OCR1BH0"] = 0;
            constants["OCR1BH1"] = 1;
            constants["OCR1BH2"] = 2;
            constants["OCR1BH3"] = 3;
            constants["OCR1BH4"] = 4;
            constants["OCR1BH5"] = 5;
            constants["OCR1BH6"] = 6;
            constants["OCR1BH7"] = 7;

            //#define TCCR2A _SFR_MEM8(0xB0)
            //#define WGM20 0
            //#define WGM21 1
            //#define COM2B0 4
            //#define COM2B1 5
            //#define COM2A0 6
            //#define COM2A1 7

            //#define TCCR2B _SFR_MEM8(0xB1)
            //#define CS20 0
            //#define CS21 1
            //#define CS22 2
            //#define WGM22 3
            //#define FOC2B 6
            //#define FOC2A 7

            //#define TCNT2 _SFR_MEM8(0xB2)
            //#define TCNT2_0 0
            //#define TCNT2_1 1
            //#define TCNT2_2 2
            //#define TCNT2_3 3
            //#define TCNT2_4 4
            //#define TCNT2_5 5
            //#define TCNT2_6 6
            //#define TCNT2_7 7

            //#define OCR2A _SFR_MEM8(0xB3)
            //#define OCR2_0 0
            //#define OCR2_1 1
            //#define OCR2_2 2
            //#define OCR2_3 3
            //#define OCR2_4 4
            //#define OCR2_5 5
            //#define OCR2_6 6
            //#define OCR2_7 7

            //#define OCR2B _SFR_MEM8(0xB4)
            //#define OCR2_0 0
            //#define OCR2_1 1
            //#define OCR2_2 2
            //#define OCR2_3 3
            //#define OCR2_4 4
            //#define OCR2_5 5
            //#define OCR2_6 6
            //#define OCR2_7 7

            //#define ASSR _SFR_MEM8(0xB6)
            //#define TCR2BUB 0
            //#define TCR2AUB 1
            //#define OCR2BUB 2
            //#define OCR2AUB 3
            //#define TCN2UB 4
            //#define AS2 5
            //#define EXCLK 6

            //#define TWBR _SFR_MEM8(0xB8)
            //#define TWBR0 0
            //#define TWBR1 1
            //#define TWBR2 2
            //#define TWBR3 3
            //#define TWBR4 4
            //#define TWBR5 5
            //#define TWBR6 6
            //#define TWBR7 7

            //#define TWSR _SFR_MEM8(0xB9)
            //#define TWPS0 0
            //#define TWPS1 1
            //#define TWS3 3
            //#define TWS4 4
            //#define TWS5 5
            //#define TWS6 6
            //#define TWS7 7

            //#define TWAR _SFR_MEM8(0xBA)
            //#define TWGCE 0
            //#define TWA0 1
            //#define TWA1 2
            //#define TWA2 3
            //#define TWA3 4
            //#define TWA4 5
            //#define TWA5 6
            //#define TWA6 7

            //#define TWDR _SFR_MEM8(0xBB)
            //#define TWD0 0
            //#define TWD1 1
            //#define TWD2 2
            //#define TWD3 3
            //#define TWD4 4
            //#define TWD5 5
            //#define TWD6 6
            //#define TWD7 7

            //#define TWCR _SFR_MEM8(0xBC)
            //#define TWIE 0
            //#define TWEN 2
            //#define TWWC 3
            //#define TWSTO 4
            //#define TWSTA 5
            //#define TWEA 6
            //#define TWINT 7

            //#define TWAMR _SFR_MEM8(0xBD)
            //#define TWAM0 0
            //#define TWAM1 1
            //#define TWAM2 2
            //#define TWAM3 3
            //#define TWAM4 4
            //#define TWAM5 5
            //#define TWAM6 6

            //#define UCSR0A _SFR_MEM8(0xC0)
            //#define MPCM0 0
            //#define U2X0 1
            //#define UPE0 2
            //#define DOR0 3
            //#define FE0 4
            //#define UDRE0 5
            //#define TXC0 6
            //#define RXC0 7

            //#define UCSR0B _SFR_MEM8(0xC1)
            //#define TXB80 0
            //#define RXB80 1
            //#define UCSZ02 2
            //#define TXEN0 3
            //#define RXEN0 4
            //#define UDRIE0 5
            //#define TXCIE0 6
            //#define RXCIE0 7

            //#define UCSR0C _SFR_MEM8(0xC2)
            //#define UCPOL0 0
            //#define UCSZ00 1
            //#define UCPHA0 1
            //#define UCSZ01 2
            //#define UDORD0 2
            //#define USBS0 3
            //#define UPM00 4
            //#define UPM01 5
            //#define UMSEL00 6
            //#define UMSEL01 7

            //#define UBRR0 _SFR_MEM16(0xC4)

            //#define UBRR0L _SFR_MEM8(0xC4)
            //#define UBRR0_0 0
            //#define UBRR0_1 1
            //#define UBRR0_2 2
            //#define UBRR0_3 3
            //#define UBRR0_4 4
            //#define UBRR0_5 5
            //#define UBRR0_6 6
            //#define UBRR0_7 7

            //#define UBRR0H _SFR_MEM8(0xC5)
            //#define UBRR0_8 0
            //#define UBRR0_9 1
            //#define UBRR0_10 2
            //#define UBRR0_11 3

            //#define UDR0 _SFR_MEM8(0xC6)
            //#define UDR0_0 0
            //#define UDR0_1 1
            //#define UDR0_2 2
            //#define UDR0_3 3
            //#define UDR0_4 4
            //#define UDR0_5 5
            //#define UDR0_6 6
            //#define UDR0_7 7







            //        }


            //        /* Interrupt Vectors */
            //        /* Interrupt Vector 0 is the reset vector. */

            //#define INT0_vect_num     1
            //#define INT0_vect         _VECTOR(1)   /* External Interrupt Request 0 */

            //#define INT1_vect_num     2
            //#define INT1_vect         _VECTOR(2)   /* External Interrupt Request 1 */

            //#define PCINT0_vect_num   3
            //#define PCINT0_vect       _VECTOR(3)   /* Pin Change Interrupt Request 0 */

            //#define PCINT1_vect_num   4
            //#define PCINT1_vect       _VECTOR(4)   /* Pin Change Interrupt Request 0 */

            //#define PCINT2_vect_num   5
            //#define PCINT2_vect       _VECTOR(5)   /* Pin Change Interrupt Request 1 */

            //#define WDT_vect_num      6
            //#define WDT_vect          _VECTOR(6)   /* Watchdog Time-out Interrupt */

            //#define TIMER2_COMPA_vect_num 7
            //#define TIMER2_COMPA_vect _VECTOR(7)   /* Timer/Counter2 Compare Match A */

            //#define TIMER2_COMPB_vect_num 8
            //#define TIMER2_COMPB_vect _VECTOR(8)   /* Timer/Counter2 Compare Match A */

            //#define TIMER2_OVF_vect_num   9
            //#define TIMER2_OVF_vect   _VECTOR(9)   /* Timer/Counter2 Overflow */

            //#define TIMER1_CAPT_vect_num  10
            //#define TIMER1_CAPT_vect  _VECTOR(10)  /* Timer/Counter1 Capture Event */

            //#define TIMER1_COMPA_vect_num 11
            //#define TIMER1_COMPA_vect _VECTOR(11)  /* Timer/Counter1 Compare Match A */

            //#define TIMER1_COMPB_vect_num 12
            //#define TIMER1_COMPB_vect _VECTOR(12)  /* Timer/Counter1 Compare Match B */ 

            //#define TIMER1_OVF_vect_num   13
            //#define TIMER1_OVF_vect   _VECTOR(13)  /* Timer/Counter1 Overflow */

            //#define TIMER0_COMPA_vect_num 14
            //#define TIMER0_COMPA_vect _VECTOR(14)  /* TimerCounter0 Compare Match A */

            //#define TIMER0_COMPB_vect_num 15
            //#define TIMER0_COMPB_vect _VECTOR(15)  /* TimerCounter0 Compare Match B */

            //#define TIMER0_OVF_vect_num  16
            //#define TIMER0_OVF_vect   _VECTOR(16)  /* Timer/Couner0 Overflow */

            //#define SPI_STC_vect_num  17
            //#define SPI_STC_vect      _VECTOR(17)  /* SPI Serial Transfer Complete */

            //#define USART_RX_vect_num 18
            //#define USART_RX_vect     _VECTOR(18)  /* USART Rx Complete */

            //#define USART_UDRE_vect_num   19
            //#define USART_UDRE_vect   _VECTOR(19)  /* USART, Data Register Empty */

            //#define USART_TX_vect_num 20
            //#define USART_TX_vect     _VECTOR(20)  /* USART Tx Complete */

            //#define ADC_vect_num      21
            //#define ADC_vect          _VECTOR(21)  /* ADC Conversion Complete */

            //#define EE_READY_vect_num 22
            //#define EE_READY_vect     _VECTOR(22)  /* EEPROM Ready */

            //#define ANALOG_COMP_vect_num  23
            //#define ANALOG_COMP_vect  _VECTOR(23)  /* Analog Comparator */

            //#define TWI_vect_num      24
            //#define TWI_vect          _VECTOR(24)  /* Two-wire Serial Interface */

            //#define SPM_READY_vect_num    25
            //#define SPM_READY_vect    _VECTOR(25)  /* Store Program Memory Read */

            //#define _VECTORS_SIZE (26 * 4)



            //        /* Constants */
            //#define SPM_PAGESIZE 128
            //#define RAMSTART     (0x100)
            //#define RAMEND       0x8FF     /* Last On-Chip SRAM Location */
            //#define XRAMSIZE     0
            //#define XRAMEND      RAMEND
            //#define E2END        0x3FF
            //#define E2PAGESIZE   4
            //#define FLASHEND     0x7FFF



            //        /* Fuses */
            //#define FUSE_MEMORY_SIZE 3

            //        /* Low Fuse Byte */
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



            //        /* Lock Bits */
            //#define __LOCK_BITS_EXIST
            //#define __BOOT_LOCK_BITS_0_EXIST
            //#define __BOOT_LOCK_BITS_1_EXIST


            //        /* Signature */
            //#define SIGNATURE_0 0x1E
            //#define SIGNATURE_1 0x95
            //#if defined(__AVR_ATmega328__)
            //#define SIGNATURE_2 0x14
            //#else /* ATmega328P */
            //#define SIGNATURE_2 0x0F
            //#endif


            //#define SLEEP_MODE_IDLE (0x00<<1)
            //#define SLEEP_MODE_ADC (0x01<<1)
            //#define SLEEP_MODE_PWR_DOWN (0x02<<1)
            //#define SLEEP_MODE_PWR_SAVE (0x03<<1)
            //#define SLEEP_MODE_STANDBY (0x06<<1)
            //#define SLEEP_MODE_EXT_STANDBY (0x07<<1)


            //#endif  /* _AVR_IOM328P_H_ */ 

        }

        public virtual object Get__AVR_HAVE_PRR(object[] param)
        {
            return (byte)((1 << GetValue("PRADC")) |
                          (1 << GetValue("PRUSART0")) |
                          (1 << GetValue("PRSPI")) |
                          (1 << GetValue("PRTIM1")) |
                          (1 << GetValue("PRTIM0")) |
                          (1 << GetValue("PRTIM2")) |
                          (1 << GetValue("PRTWI"))); 
        }

    }
}
