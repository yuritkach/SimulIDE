using System;

namespace SimulIDE.src.simavr.cores.avr
{
    public class iom328p
    {
        public static void InitConstants()
        {
            if (!Constants.Defined("_AVR_IO_H_"))
                throw new Exception("Include < avr / io.h > instead of this file.");
            if (!Constants.Defined("_AVR_IOXXX_H_"))
                Constants.Set("_AVR_IOXXX_H_", "iom328p.h");
            else throw new Exception("Attempt to include more than one <avr/ioXXX.h> file.");

            if (!Constants.Defined("_AVR_IOM328P_H_"))
            {
                Constants.Set("_AVR_IOM328P_H_", 1);
                /* Registers and associated bit numbers */

                Constants.Set("PINB", Constants._SFR_IO8(0x03));
                Constants.Set("PINB0", 0);
                Constants.Set("PINB1", 1);
                Constants.Set("PINB2", 2);
                Constants.Set("PINB3", 3);
                Constants.Set("PINB4", 4);
                Constants.Set("PINB5", 5);
                Constants.Set("PINB6", 6);
                Constants.Set("PINB7", 7);

                Constants.Set("DDRB", Constants._SFR_IO8(0x04));
                Constants.Set("DDB0", 0);
                Constants.Set("DDB1", 1);
                Constants.Set("DDB2", 2);
                Constants.Set("DDB3", 3);
                Constants.Set("DDB4", 4);
                Constants.Set("DDB5", 5);
                Constants.Set("DDB6", 6);
                Constants.Set("DDB7", 7);

                Constants.Set("PORTB", Constants._SFR_IO8(0x05));
                Constants.Set("PORTB0", 0);
                Constants.Set("PORTB1", 1);
                Constants.Set("PORTB2", 2);
                Constants.Set("PORTB3", 3);
                Constants.Set("PORTB4", 4);
                Constants.Set("PORTB5", 5);
                Constants.Set("PORTB6", 6);
                Constants.Set("PORTB7", 7);

                Constants.Set("PINC", Constants._SFR_IO8(0x06));
                Constants.Set("PINC0", 0);
                Constants.Set("PINC1", 1);
                Constants.Set("PINC2", 2);
                Constants.Set("PINC3", 3);
                Constants.Set("PINC4", 4);
                Constants.Set("PINC5", 5);
                Constants.Set("PINC6", 6);

                Constants.Set("DDRC", Constants._SFR_IO8(0x07));
                Constants.Set("DDC0", 0);
                Constants.Set("DDC1", 1);
                Constants.Set("DDC2", 2);
                Constants.Set("DDC3", 3);
                Constants.Set("DDC4", 4);
                Constants.Set("DDC5", 5);
                Constants.Set("DDC6", 6);

                Constants.Set("PORTC", Constants._SFR_IO8(0x08));
                Constants.Set("PORTC0", 0);
                Constants.Set("PORTC1", 1);
                Constants.Set("PORTC2", 2);
                Constants.Set("PORTC3", 3);
                Constants.Set("PORTC4", 4);
                Constants.Set("PORTC5", 5);
                Constants.Set("PORTC6", 6);

                Constants.Set("PIND", Constants._SFR_IO8(0x09));
                Constants.Set("PIND0", 0);
                Constants.Set("PIND1", 1);
                Constants.Set("PIND2", 2);
                Constants.Set("PIND3", 3);
                Constants.Set("PIND4", 4);
                Constants.Set("PIND5", 5);
                Constants.Set("PIND6", 6);
                Constants.Set("PIND7", 7);

                Constants.Set("DDRD", Constants._SFR_IO8(0x0A));
                Constants.Set("DDD0", 0);
                Constants.Set("DDD1", 1);
                Constants.Set("DDD2", 2);
                Constants.Set("DDD3", 3);
                Constants.Set("DDD4", 4);
                Constants.Set("DDD5", 5);
                Constants.Set("DDD6", 6);
                Constants.Set("DDD7", 7);

                Constants.Set("PORTD", Constants._SFR_IO8(0x0B));
                Constants.Set("PORTD0", 0);
                Constants.Set("PORTD1", 1);
                Constants.Set("PORTD2", 2);
                Constants.Set("PORTD3", 3);
                Constants.Set("PORTD4", 4);
                Constants.Set("PORTD5", 5);
                Constants.Set("PORTD6", 6);
                Constants.Set("PORTD7", 7);

                Constants.Set("TIFR0", Constants._SFR_IO8(0x15));
                Constants.Set("TOV0", 0);
                Constants.Set("OCF0A", 1);
                Constants.Set("OCF0B", 2);

                Constants.Set("TIFR1", Constants._SFR_IO8(0x16));
                Constants.Set("TOV1", 0);
                Constants.Set("OCF1A", 1);
                Constants.Set("OCF1B", 2);
                Constants.Set("ICF1", 5);

                Constants.Set("TIFR2", Constants._SFR_IO8(0x17));
                Constants.Set("TOV2", 0);
                Constants.Set("OCF2A", 1);
                Constants.Set("OCF2B", 2);

                Constants.Set("PCIFR", Constants._SFR_IO8(0x1B));
                Constants.Set("PCIF0", 0);
                Constants.Set("PCIF1", 1);
                Constants.Set("PCIF2", 2);

                Constants.Set("EIFR", Constants._SFR_IO8(0x1C));
                Constants.Set("INTF0", 0);
                Constants.Set("INTF1", 1);

                Constants.Set("EIMSK", Constants._SFR_IO8(0x1D));
                Constants.Set("INT0", 0);
                Constants.Set("INT1", 1);

                Constants.Set("GPIOR0", Constants._SFR_IO8(0x1E));
                Constants.Set("GPIOR00", 0);
                Constants.Set("GPIOR01", 1);
                Constants.Set("GPIOR02", 2);
                Constants.Set("GPIOR03", 3);
                Constants.Set("GPIOR04", 4);
                Constants.Set("GPIOR05", 5);
                Constants.Set("GPIOR06", 6);
                Constants.Set("GPIOR07", 7);

                Constants.Set("EECR", Constants._SFR_IO8(0x1F));
                Constants.Set("EERE", 0);
                Constants.Set("EEPE", 1);
                Constants.Set("EEMPE", 2);
                Constants.Set("EERIE", 3);
                Constants.Set("EEPM0", 4);
                Constants.Set("EEPM1", 5);

                Constants.Set("EEDR", Constants._SFR_IO8(0x20));
                Constants.Set("EEDR0", 0);
                Constants.Set("EEDR1", 1);
                Constants.Set("EEDR2", 2);
                Constants.Set("EEDR3", 3);
                Constants.Set("EEDR4", 4);
                Constants.Set("EEDR5", 5);
                Constants.Set("EEDR6", 6);
                Constants.Set("EEDR7", 7);

                Constants.Set("EEAR", Constants._SFR_IO16(0x21));

                Constants.Set("EEARL", Constants._SFR_IO8(0x21));
                Constants.Set("EEAR0", 0);
                Constants.Set("EEAR1", 1);
                Constants.Set("EEAR2", 2);
                Constants.Set("EEAR3", 3);
                Constants.Set("EEAR4", 4);
                Constants.Set("EEAR5", 5);
                Constants.Set("EEAR6", 6);
                Constants.Set("EEAR7", 7);

                Constants.Set("EEARH", Constants._SFR_IO8(0x22));
                Constants.Set("EEAR8", 0);
                Constants.Set("EEAR9", 1);

                Constants.Set("_EEPROM_REG_LOCATIONS_", 0x1F2021);

                Constants.Set("GTCCR", Constants._SFR_IO8(0x23));
                Constants.Set("PSRSYNC", 0);
                Constants.Set("PSRASY", 1);
                Constants.Set("TSM", 7);

                Constants.Set("TCCR0A", Constants._SFR_IO8(0x24));
                Constants.Set("WGM00", 0);
                Constants.Set("WGM01", 1);
                Constants.Set("COM0B0", 4);
                Constants.Set("COM0B1", 5);
                Constants.Set("COM0A0", 6);
                Constants.Set("COM0A1", 7);

                Constants.Set("TCCR0B", Constants._SFR_IO8(0x25));
                Constants.Set("CS00", 0);
                Constants.Set("CS01", 1);
                Constants.Set("CS02", 2);
                Constants.Set("WGM02", 3);
                Constants.Set("FOC0B", 6);
                Constants.Set("FOC0A", 7);

                Constants.Set("TCNT0", Constants._SFR_IO8(0x26));
                Constants.Set("TCNT0_0", 0);
                Constants.Set("TCNT0_1", 1);
                Constants.Set("TCNT0_2", 2);
                Constants.Set("TCNT0_3", 3);
                Constants.Set("TCNT0_4", 4);
                Constants.Set("TCNT0_5", 5);
                Constants.Set("TCNT0_6", 6);
                Constants.Set("TCNT0_7", 7);

                Constants.Set("OCR0A", Constants._SFR_IO8(0x27));
                Constants.Set("OCR0A_0", 0);
                Constants.Set("OCR0A_1", 1);
                Constants.Set("OCR0A_2", 2);
                Constants.Set("OCR0A_3", 3);
                Constants.Set("OCR0A_4", 4);
                Constants.Set("OCR0A_5", 5);
                Constants.Set("OCR0A_6", 6);
                Constants.Set("OCR0A_7", 7);

                Constants.Set("OCR0B", Constants._SFR_IO8(0x28));
                Constants.Set("OCR0B_0", 0);
                Constants.Set("OCR0B_1", 1);
                Constants.Set("OCR0B_2", 2);
                Constants.Set("OCR0B_3", 3);
                Constants.Set("OCR0B_4", 4);
                Constants.Set("OCR0B_5", 5);
                Constants.Set("OCR0B_6", 6);
                Constants.Set("OCR0B_7", 7);

                Constants.Set("GPIOR1", Constants._SFR_IO8(0x2A));
                Constants.Set("GPIOR10", 0);
                Constants.Set("GPIOR11", 1);
                Constants.Set("GPIOR12", 2);
                Constants.Set("GPIOR13", 3);
                Constants.Set("GPIOR14", 4);
                Constants.Set("GPIOR15", 5);
                Constants.Set("GPIOR16", 6);
                Constants.Set("GPIOR17", 7);

                Constants.Set("GPIOR2", Constants._SFR_IO8(0x2B));
                Constants.Set("GPIOR20", 0);
                Constants.Set("GPIOR21", 1);
                Constants.Set("GPIOR22", 2);
                Constants.Set("GPIOR23", 3);
                Constants.Set("GPIOR24", 4);
                Constants.Set("GPIOR25", 5);
                Constants.Set("GPIOR26", 6);
                Constants.Set("GPIOR27", 7);

                Constants.Set("SPCR", Constants._SFR_IO8(0x2C));
                Constants.Set("SPR0", 0);
                Constants.Set("SPR1", 1);
                Constants.Set("CPHA", 2);
                Constants.Set("CPOL", 3);
                Constants.Set("MSTR", 4);
                Constants.Set("DORD", 5);
                Constants.Set("SPE", 6);
                Constants.Set("SPIE", 7);

                Constants.Set("SPSR", Constants._SFR_IO8(0x2D));
                Constants.Set("SPI2X", 0);
                Constants.Set("WCOL", 6);
                Constants.Set("SPIF", 7);

                Constants.Set("SPDR", Constants._SFR_IO8(0x2E));
                Constants.Set("SPDR0", 0);
                Constants.Set("SPDR1", 1);
                Constants.Set("SPDR2", 2);
                Constants.Set("SPDR3", 3);
                Constants.Set("SPDR4", 4);
                Constants.Set("SPDR5", 5);
                Constants.Set("SPDR6", 6);
                Constants.Set("SPDR7", 7);

                Constants.Set("ACSR", Constants._SFR_IO8(0x30));
                Constants.Set("ACIS0", 0);
                Constants.Set("ACIS1", 1);
                Constants.Set("ACIC", 2);
                Constants.Set("ACIE", 3);
                Constants.Set("ACI", 4);
                Constants.Set("ACO", 5);
                Constants.Set("ACBG", 6);
                Constants.Set("ACD", 7);

                Constants.Set("SMCR", Constants._SFR_IO8(0x33));
                Constants.Set("SE", 0);
                Constants.Set("SM0", 1);
                Constants.Set("SM1", 2);
                Constants.Set("SM2", 3);

                Constants.Set("MCUSR", Constants._SFR_IO8(0x34));
                Constants.Set("PORF", 0);
                Constants.Set("EXTRF", 1);
                Constants.Set("BORF", 2);
                Constants.Set("WDRF", 3);

                Constants.Set("MCUCR", Constants._SFR_IO8(0x35));
                Constants.Set("IVCE", 0);
                Constants.Set("IVSEL", 1);
                Constants.Set("PUD", 4);
                Constants.Set("BODSE", 5);
                Constants.Set("BODS", 6);

                Constants.Set("SPMCSR", Constants._SFR_IO8(0x37));
                Constants.Set("SELFPRGEN", 0); /* only for backwards compatibility with previous
                                                * avr - libc versions; not an official name */
                Constants.Set("SPMEN", 0);
                Constants.Set("PGERS", 1);
                Constants.Set("PGWRT", 2);
                Constants.Set("BLBSET", 3);
                Constants.Set("RWWSRE", 4);
                Constants.Set("SIGRD", 5);
                Constants.Set("RWWSB", 6);
                Constants.Set("SPMIE", 7);

                Constants.Set("WDTCSR", Constants._SFR_MEM8(0x60));
                Constants.Set("WDP0", 0);
                Constants.Set("WDP1", 1);
                Constants.Set("WDP2", 2);
                Constants.Set("WDE", 3);
                Constants.Set("WDCE", 4);
                Constants.Set("WDP3", 5);
                Constants.Set("WDIE", 6);
                Constants.Set("WDIF", 7);

                Constants.Set("CLKPR", Constants._SFR_MEM8(0x61));
                Constants.Set("CLKPS0", 0);
                Constants.Set("CLKPS1", 1);
                Constants.Set("CLKPS2", 2);
                Constants.Set("CLKPS3", 3);
                Constants.Set("CLKPCE", 7);

                Constants.Set("PRR", Constants._SFR_MEM8(0x64));
                Constants.Set("PRADC", 0);
                Constants.Set("PRUSART0", 1);
                Constants.Set("PRSPI", 2);
                Constants.Set("PRTIM1", 3);
                Constants.Set("PRTIM0", 5);
                Constants.Set("PRTIM2", 6);
                Constants.Set("PRTWI", 7);

                Constants.Set("__AVR_HAVE_PRR", (byte)(
                                                ((byte)(1 << (int)Constants.Get("PRADC"))) |
                                                ((byte)(1 << (int)Constants.Get("PRUSART0"))) |
                                                ((byte)(1 << (int)Constants.Get("PRSPI"))) |
                                                ((byte)(1 << (int)Constants.Get("PRTIM1"))) |
                                                ((byte)(1 << (int)Constants.Get("PRTIM0"))) |
                                                ((byte)(1 << (int)Constants.Get("PRTIM2"))) |
                                                ((byte)(1 << (int)Constants.Get("PRTWI")))
                                                ));
                Constants.Set("__AVR_HAVE_PRR_PRADC", 1);
                Constants.Set("__AVR_HAVE_PRR_PRUSART0", 1);
                Constants.Set("__AVR_HAVE_PRR_PRSPI", 1);
                Constants.Set("__AVR_HAVE_PRR_PRTIM1", 1);
                Constants.Set("__AVR_HAVE_PRR_PRTIM0", 1);
                Constants.Set("__AVR_HAVE_PRR_PRTIM2", 1);
                Constants.Set("__AVR_HAVE_PRR_PRTWI", 1);

                Constants.Set("OSCCAL", Constants._SFR_MEM8(0x66));
                Constants.Set("CAL0", 0);
                Constants.Set("CAL1", 1);
                Constants.Set("CAL2", 2);
                Constants.Set("CAL3", 3);
                Constants.Set("CAL4", 4);
                Constants.Set("CAL5", 5);
                Constants.Set("CAL6", 6);
                Constants.Set("CAL7", 7);

                Constants.Set("PCICR", Constants._SFR_MEM8(0x68));
                Constants.Set("PCIE0", 0);
                Constants.Set("PCIE1", 1);
                Constants.Set("PCIE2", 2);

                Constants.Set("EICRA", Constants._SFR_MEM8(0x69));
                Constants.Set("ISC00", 0);
                Constants.Set("ISC01", 1);
                Constants.Set("ISC10", 2);
                Constants.Set("ISC11", 3);

                Constants.Set("PCMSK0", Constants._SFR_MEM8(0x6B));
                Constants.Set("PCINT0", 0);
                Constants.Set("PCINT1", 1);
                Constants.Set("PCINT2", 2);
                Constants.Set("PCINT3", 3);
                Constants.Set("PCINT4", 4);
                Constants.Set("PCINT5", 5);
                Constants.Set("PCINT6", 6);
                Constants.Set("PCINT7", 7);

                Constants.Set("PCMSK1", Constants._SFR_MEM8(0x6C));
                Constants.Set("PCINT8", 0);
                Constants.Set("PCINT9", 1);
                Constants.Set("PCINT10", 2);
                Constants.Set("PCINT11", 3);
                Constants.Set("PCINT12", 4);
                Constants.Set("PCINT13", 5);
                Constants.Set("PCINT14", 6);

                Constants.Set("PCMSK2", Constants._SFR_MEM8(0x6D));
                Constants.Set("PCINT16", 0);
                Constants.Set("PCINT17", 1);
                Constants.Set("PCINT18", 2);
                Constants.Set("PCINT19", 3);
                Constants.Set("PCINT20", 4);
                Constants.Set("PCINT21", 5);
                Constants.Set("PCINT22", 6);
                Constants.Set("PCINT23", 7);

                Constants.Set("TIMSK0", Constants._SFR_MEM8(0x6E));
                Constants.Set("TOIE0", 0);
                Constants.Set("OCIE0A", 1);
                Constants.Set("OCIE0B", 2);

                Constants.Set("TIMSK1", Constants._SFR_MEM8(0x6F));
                Constants.Set("TOIE1", 0);
                Constants.Set("OCIE1A", 1);
                Constants.Set("OCIE1B", 2);
                Constants.Set("ICIE1", 5);

                Constants.Set("TIMSK2", Constants._SFR_MEM8(0x70));
                Constants.Set("TOIE2", 0);
                Constants.Set("OCIE2A", 1);
                Constants.Set("OCIE2B", 2);

                if (!Constants.Defined("__ASSEMBLER__"))
                    Constants.Set("ADC", Constants._SFR_MEM16(0x78));

                Constants.Set("ADCW", Constants._SFR_MEM16(0x78));

                Constants.Set("ADCL", Constants._SFR_MEM8(0x78));
                Constants.Set("ADCL0", 0);
                Constants.Set("ADCL1", 1);
                Constants.Set("ADCL2", 2);
                Constants.Set("ADCL3", 3);
                Constants.Set("ADCL4", 4);
                Constants.Set("ADCL5", 5);
                Constants.Set("ADCL6", 6);
                Constants.Set("ADCL7", 7);

                Constants.Set("ADCH", Constants._SFR_MEM8(0x79));
                Constants.Set("ADCH0", 0);
                Constants.Set("ADCH1", 1);
                Constants.Set("ADCH2", 2);
                Constants.Set("ADCH3", 3);
                Constants.Set("ADCH4", 4);
                Constants.Set("ADCH5", 5);
                Constants.Set("ADCH6", 6);
                Constants.Set("ADCH7", 7);

                Constants.Set("ADCSRA", Constants._SFR_MEM8(0x7A));
                Constants.Set("ADPS0", 0);
                Constants.Set("ADPS1", 1);
                Constants.Set("ADPS2", 2);
                Constants.Set("ADIE", 3);
                Constants.Set("ADIF", 4);
                Constants.Set("ADATE", 5);
                Constants.Set("ADSC", 6);
                Constants.Set("ADEN", 7);

                Constants.Set("ADCSRB", Constants._SFR_MEM8(0x7B));
                Constants.Set("ADTS0", 0);
                Constants.Set("ADTS1", 1);
                Constants.Set("ADTS2", 2);
                Constants.Set("ACME", 6);

                Constants.Set("ADMUX", Constants._SFR_MEM8(0x7C));
                Constants.Set("MUX0", 0);
                Constants.Set("MUX1", 1);
                Constants.Set("MUX2", 2);
                Constants.Set("MUX3", 3);
                Constants.Set("ADLAR", 5);
                Constants.Set("REFS0", 6);
                Constants.Set("REFS1", 7);

                Constants.Set("DIDR0", Constants._SFR_MEM8(0x7E));
                Constants.Set("ADC0D", 0);
                Constants.Set("ADC1D", 1);
                Constants.Set("ADC2D", 2);
                Constants.Set("ADC3D", 3);
                Constants.Set("ADC4D", 4);
                Constants.Set("ADC5D", 5);

                Constants.Set("DIDR1", Constants._SFR_MEM8(0x7F));
                Constants.Set("AIN0D", 0);
                Constants.Set("AIN1D", 1);

                Constants.Set("TCCR1A", Constants._SFR_MEM8(0x80));
                Constants.Set("WGM10", 0);
                Constants.Set("WGM11", 1);
                Constants.Set("COM1B0", 4);
                Constants.Set("COM1B1", 5);
                Constants.Set("COM1A0", 6);
                Constants.Set("COM1A1", 7);
                

                Constants.Set("TCCR1B", Constants._SFR_MEM8(0x81));
                Constants.Set("CS10", 0);
                Constants.Set("CS11", 1);
                Constants.Set("CS12", 2);
                Constants.Set("WGM12", 3);
                Constants.Set("WGM13", 4);
                Constants.Set("ICES1", 6);
                Constants.Set("ICNC1", 7);

                Constants.Set("TCCR1C", Constants._SFR_MEM8(0x82));
                Constants.Set("FOC1B", 6);
                Constants.Set("FOC1A", 7);

                Constants.Set("TCNT1", Constants._SFR_MEM16(0x84));
                Constants.Set("TCNT1L", Constants._SFR_MEM8(0x84));
                Constants.Set("TCNT1L0", 0);
                Constants.Set("TCNT1L1", 1);
                Constants.Set("TCNT1L2", 2);
                Constants.Set("TCNT1L3", 3);
                Constants.Set("TCNT1L4", 4);
                Constants.Set("TCNT1L5", 5);
                Constants.Set("TCNT1L6", 6);
                Constants.Set("TCNT1L7", 7);

                Constants.Set("TCNT1H", Constants._SFR_MEM8(0x85));
                Constants.Set("TCNT1H0", 0);
                Constants.Set("TCNT1H1", 1);
                Constants.Set("TCNT1H2", 2);
                Constants.Set("TCNT1H3", 3);
                Constants.Set("TCNT1H4", 4);
                Constants.Set("TCNT1H5", 5);
                Constants.Set("TCNT1H6", 6);
                Constants.Set("TCNT1H7", 7);

                Constants.Set("ICR1", Constants._SFR_MEM16(0x86));
                Constants.Set("ICR1L", Constants._SFR_MEM8(0x86));
                Constants.Set("ICR1L0", 0);
                Constants.Set("ICR1L1", 1);
                Constants.Set("ICR1L2", 2);
                Constants.Set("ICR1L3", 3);
                Constants.Set("ICR1L4", 4);
                Constants.Set("ICR1L5", 5);
                Constants.Set("ICR1L6", 6);
                Constants.Set("ICR1L7", 7);

                Constants.Set("ICR1H", Constants._SFR_MEM8(0x87));
                Constants.Set("ICR1H0", 0);
                Constants.Set("ICR1H1", 1);
                Constants.Set("ICR1H2", 2);
                Constants.Set("ICR1H3", 3);
                Constants.Set("ICR1H4", 4);
                Constants.Set("ICR1H5", 5);
                Constants.Set("ICR1H6", 6);
                Constants.Set("ICR1H7", 7);

                Constants.Set("OCR1A", Constants._SFR_MEM16(0x88));
                Constants.Set("OCR1AL", Constants._SFR_MEM8(0x88));
                Constants.Set("OCR1AL0", 0);
                Constants.Set("OCR1AL1", 1);
                Constants.Set("OCR1AL2", 2);
                Constants.Set("OCR1AL3", 3);
                Constants.Set("OCR1AL4", 4);
                Constants.Set("OCR1AL5", 5);
                Constants.Set("OCR1AL6", 6);
                Constants.Set("OCR1AL7", 7);

                Constants.Set("OCR1AH", Constants._SFR_MEM8(0x89));
                Constants.Set("OCR1AH0", 0);
                Constants.Set("OCR1AH1", 1);
                Constants.Set("OCR1AH2", 2);
                Constants.Set("OCR1AH3", 3);
                Constants.Set("OCR1AH4", 4);
                Constants.Set("OCR1AH5", 5);
                Constants.Set("OCR1AH6", 6);
                Constants.Set("OCR1AH7", 7);

                Constants.Set("OCR1B", Constants._SFR_MEM16(0x8A));
                Constants.Set("OCR1BL", Constants._SFR_MEM8(0x8A));
                Constants.Set("OCR1BL0", 0);
                Constants.Set("OCR1BL1", 1);
                Constants.Set("OCR1BL2", 2);
                Constants.Set("OCR1BL3", 3);
                Constants.Set("OCR1BL4", 4);
                Constants.Set("OCR1BL5", 5);
                Constants.Set("OCR1BL6", 6);
                Constants.Set("OCR1BL7", 7);

                Constants.Set("OCR1BH", Constants._SFR_MEM8(0x8B));
                Constants.Set("OCR1BH0", 0);
                Constants.Set("OCR1BH1", 1);
                Constants.Set("OCR1BH2", 2);
                Constants.Set("OCR1BH3", 3);
                Constants.Set("OCR1BH4", 4);
                Constants.Set("OCR1BH5", 5);
                Constants.Set("OCR1BH6", 6);
                Constants.Set("OCR1BH7", 7);

                Constants.Set("TCCR2A", Constants._SFR_MEM8(0xB0));
                Constants.Set("WGM20", 0);
                Constants.Set("WGM21", 1);
                Constants.Set("COM2B0", 4);
                Constants.Set("COM2B1", 5);
                Constants.Set("COM2A0", 6);
                Constants.Set("COM2A1", 7);

                Constants.Set("TCCR2B", Constants._SFR_MEM8(0xB1));
                Constants.Set("CS20", 0);
                Constants.Set("CS21", 1);
                Constants.Set("CS22", 2);
                Constants.Set("WGM22", 3);
                Constants.Set("FOC2B", 6);
                Constants.Set("FOC2A", 7);

                Constants.Set("TCNT2", Constants._SFR_MEM8(0xB2));
                Constants.Set("TCNT2_0", 0);
                Constants.Set("TCNT2_1", 1);
                Constants.Set("TCNT2_2", 2);
                Constants.Set("TCNT2_3", 3);
                Constants.Set("TCNT2_4", 4);
                Constants.Set("TCNT2_5", 5);
                Constants.Set("TCNT2_6", 6);
                Constants.Set("TCNT2_7", 7);

                Constants.Set("OCR2A", Constants._SFR_MEM8(0xB3));
                Constants.Set("OCR2_0", 0);
                Constants.Set("OCR2_1", 1);
                Constants.Set("OCR2_2", 2);
                Constants.Set("OCR2_3", 3);
                Constants.Set("OCR2_4", 4);
                Constants.Set("OCR2_5", 5);
                Constants.Set("OCR2_6", 6);
                Constants.Set("OCR2_7", 7);

                Constants.Set("OCR2B", Constants._SFR_MEM8(0xB4));
                Constants.Set("OCR2_0", 0);
                Constants.Set("OCR2_1", 1);
                Constants.Set("OCR2_2", 2);
                Constants.Set("OCR2_3", 3);
                Constants.Set("OCR2_4", 4);
                Constants.Set("OCR2_5", 5);
                Constants.Set("OCR2_6", 6);
                Constants.Set("OCR2_7", 7);

                Constants.Set("ASSR", Constants._SFR_MEM8(0xB6));
                Constants.Set("TCR2BUB", 0);
                Constants.Set("TCR2AUB", 1);
                Constants.Set("OCR2BUB", 2);
                Constants.Set("OCR2AUB", 3);
                Constants.Set("TCN2UB", 4);
                Constants.Set("AS2", 5);
                Constants.Set("EXCLK", 6);

                Constants.Set("TWBR", Constants._SFR_MEM8(0xB8));
                Constants.Set("TWBR0", 0);
                Constants.Set("TWBR1", 1);
                Constants.Set("TWBR2", 2);
                Constants.Set("TWBR3", 3);
                Constants.Set("TWBR4", 4);
                Constants.Set("TWBR5", 5);
                Constants.Set("TWBR6", 6);
                Constants.Set("TWBR7", 7);

                Constants.Set("TWSR", Constants._SFR_MEM8(0xB9));
                Constants.Set("TWPS0", 0);
                Constants.Set("TWPS1", 1);
                Constants.Set("TWS3", 3);
                Constants.Set("TWS4", 4);
                Constants.Set("TWS5", 5);
                Constants.Set("TWS6", 6);
                Constants.Set("TWS7", 7);

                Constants.Set("TWAR", Constants._SFR_MEM8(0xBA));
                Constants.Set("TWGCE", 0);
                Constants.Set("TWA0", 1);
                Constants.Set("TWA1", 2);
                Constants.Set("TWA2", 3);
                Constants.Set("TWA3", 4);
                Constants.Set("TWA4", 5);
                Constants.Set("TWA5", 6);
                Constants.Set("TWA6", 7);
                

                Constants.Set("TWDR", Constants._SFR_MEM8(0xBB));
                Constants.Set("TWD0", 0);
                Constants.Set("TWD1", 1);
                Constants.Set("TWD2", 2);
                Constants.Set("TWD3", 3);
                Constants.Set("TWD4", 4);
                Constants.Set("TWD5", 5);
                Constants.Set("TWD6", 6);
                Constants.Set("TWD7", 7);

                Constants.Set("TWCR", Constants._SFR_MEM8(0xBC));
                Constants.Set("TWIE", 0);
                Constants.Set("TWEN", 2);
                Constants.Set("TWWC", 3);
                Constants.Set("TWSTO", 4);
                Constants.Set("TWSTA", 5);
                Constants.Set("TWEA", 6);
                Constants.Set("TWINT", 7);

                Constants.Set("TWAMR", Constants._SFR_MEM8(0xBD));
                Constants.Set("TWAM0", 0);
                Constants.Set("TWAM1", 1);
                Constants.Set("TWAM2", 2);
                Constants.Set("TWAM3", 3);
                Constants.Set("TWAM4", 4);
                Constants.Set("TWAM5", 5);
                Constants.Set("TWAM6", 6);

                Constants.Set("UCSR0A", Constants._SFR_MEM8(0xC0));
                Constants.Set("MPCM0", 0);
                Constants.Set("U2X0", 1);
                Constants.Set("UPE0", 2);
                Constants.Set("DOR0", 3);
                Constants.Set("FE0", 4);
                Constants.Set("UDRE0", 5);
                Constants.Set("TXC0", 6);
                Constants.Set("RXC0", 7);

                Constants.Set("UCSR0B", Constants._SFR_MEM8(0xC1));
                Constants.Set("TXB80", 0);
                Constants.Set("RXB80", 1);
                Constants.Set("UCSZ02", 2);
                Constants.Set("TXEN0", 3);
                Constants.Set("RXEN0", 4);
                Constants.Set("UDRIE0", 5);
                Constants.Set("TXCIE0", 6);
                Constants.Set("RXCIE0", 7);

                Constants.Set("UCSR0C", Constants._SFR_MEM8(0xC2));
                Constants.Set("UCPOL0", 0);
                Constants.Set("UCSZ00", 1);
                Constants.Set("UCPHA0", 1);
                Constants.Set("UCSZ01", 2);
                Constants.Set("UDORD0", 2);
                Constants.Set("USBS0", 3);
                Constants.Set("UPM00", 4);
                Constants.Set("UPM01", 5);
                Constants.Set("UMSEL00", 6);
                Constants.Set("UMSEL01", 7);
                

                Constants.Set("UBRR0", Constants._SFR_MEM16(0xC4));
                Constants.Set("UBRR0L", Constants._SFR_MEM8(0xC4));
                Constants.Set("UBRR0_0", 0);
                Constants.Set("UBRR0_1", 1);
                Constants.Set("UBRR0_2", 2);
                Constants.Set("UBRR0_3", 3);
                Constants.Set("UBRR0_4", 4);
                Constants.Set("UBRR0_5", 5);
                Constants.Set("UBRR0_6", 6);
                Constants.Set("UBRR0_7", 7);

                Constants.Set("UBRR0H", Constants._SFR_MEM8(0xC5));
                Constants.Set("UBRR0_8", 0);
                Constants.Set("UBRR0_9", 1);
                Constants.Set("UBRR0_10", 2);
                Constants.Set("UBRR0_11", 3);

                Constants.Set("UDR0", Constants._SFR_MEM8(0xC6));
                Constants.Set("UDR0_0", 0);
                Constants.Set("UDR0_1", 1);
                Constants.Set("UDR0_2", 2);
                Constants.Set("UDR0_3", 3);
                Constants.Set("UDR0_4", 4);
                Constants.Set("UDR0_5", 5);
                Constants.Set("UDR0_6", 6);
                Constants.Set("UDR0_7", 7);

                /* Interrupt Vectors */
                /* Interrupt Vector 0 is the reset vector. */

                Constants.Set("INT0_vect_num", 1);
                Constants.Set("INT0_vect", Constants._VECTOR(1));   /* External Interrupt Request 0 */

                Constants.Set("INT1_vect_num", 2);
                Constants.Set("INT1_vect", Constants._VECTOR(2));   /* External Interrupt Request 1 */

                Constants.Set("PCINT0_vect_num", 3);
                Constants.Set("PCINT0_vect", Constants._VECTOR(3));   /* Pin Change Interrupt Request 0 */

                Constants.Set("PCINT1_vect_num", 4);
                Constants.Set("PCINT1_vect", Constants._VECTOR(4));   /* Pin Change Interrupt Request 0 */

                Constants.Set("PCINT2_vect_num", 5);
                Constants.Set("PCINT2_vect", Constants._VECTOR(5));   /* Pin Change Interrupt Request 1 */

                Constants.Set("WDT_vect_num", 6);
                Constants.Set("WDT_vect", Constants._VECTOR(6));   /* Watchdog Time-out Interrupt */

                Constants.Set("TIMER2_COMPA_vect_num", 7);
                Constants.Set("TIMER2_COMPA_vect", Constants._VECTOR(7));   /* Timer/Counter2 Compare Match A */

                Constants.Set("TIMER2_COMPB_vect_num", 8);
                Constants.Set("TIMER2_COMPB_vect", Constants._VECTOR(8));   /* Timer/Counter2 Compare Match A */

                Constants.Set("TIMER2_OVF_vect_num",  9);
                Constants.Set("TIMER2_OVF_vect", Constants._VECTOR(9));   /* Timer/Counter2 Overflow */

                Constants.Set("TIMER1_CAPT_vect_num", 10);
                Constants.Set("TIMER1_CAPT_vect", Constants._VECTOR(10));  /* Timer/Counter1 Capture Event */

                Constants.Set("TIMER1_COMPA_vect_num", 11);
                Constants.Set("TIMER1_COMPA_vect", Constants._VECTOR(11));  /* Timer/Counter1 Compare Match A */

                Constants.Set("TIMER1_COMPB_vect_num", 12);
                Constants.Set("TIMER1_COMPB_vect", Constants._VECTOR(12));  /* Timer/Counter1 Compare Match B */

                Constants.Set("TIMER1_OVF_vect_num", 13);
                Constants.Set("TIMER1_OVF_vect", Constants._VECTOR(13));  /* Timer/Counter1 Overflow */

                Constants.Set("TIMER0_COMPA_vect_num", 14);
                Constants.Set("TIMER0_COMPA_vect", Constants._VECTOR(14));  /* TimerCounter0 Compare Match A */

                Constants.Set("TIMER0_COMPB_vect_num", 15);
                Constants.Set("TIMER0_COMPB_vect", Constants._VECTOR(15));  /* TimerCounter0 Compare Match B */

                Constants.Set("TIMER0_OVF_vect_num", 16);
                Constants.Set("TIMER0_OVF_vect", Constants._VECTOR(16));  /* Timer/Couner0 Overflow */

                Constants.Set("SPI_STC_vect_num", 17);
                Constants.Set("SPI_STC_vect", Constants._VECTOR(17));  /* SPI Serial Transfer Complete */

                Constants.Set("USART_RX_vect_num", 18);
                Constants.Set("USART_RX_vect", Constants._VECTOR(18));  /* USART Rx Complete */

                Constants.Set("USART_UDRE_vect_num", 19);
                Constants.Set("USART_UDRE_vect", Constants._VECTOR(19));  /* USART, Data Register Empty */

                Constants.Set("USART_TX_vect_num", 20);
                Constants.Set("USART_TX_vect", Constants._VECTOR(20));  /* USART Tx Complete */

                Constants.Set("ADC_vect_num", 21);
                Constants.Set("ADC_vect", Constants._VECTOR(21));  /* ADC Conversion Complete */

                Constants.Set("EE_READY_vect_num", 22);
                Constants.Set("EE_READY_vect", Constants._VECTOR(22));  /* EEPROM Ready */

                Constants.Set("ANALOG_COMP_vect_num", 23);
                Constants.Set("ANALOG_COMP_vect", Constants._VECTOR(23));  /* Analog Comparator */

                Constants.Set("TWI_vect_num", 24);
                Constants.Set("TWI_vect", Constants._VECTOR(24));  /* Two-wire Serial Interface */

                Constants.Set("SPM_READY_vect_num", 25);
                Constants.Set("SPM_READY_vect", Constants._VECTOR(25));  /* Store Program Memory Read */

                Constants.Set("_VECTORS_SIZE",(26 * 4));

                /* Constants */
                Constants.Set("SPM_PAGESIZE", 128);
                Constants.Set("RAMSTART", (0x100));
                Constants.Set("RAMEND", 0x8FF);     /* Last On-Chip SRAM Location */
                Constants.Set("XRAMSIZE", 0);
                Constants.Set("XRAMEND", Constants.Get("RAMEND"));
                Constants.Set("E2END", 0x3FF);
                Constants.Set("E2PAGESIZE", 4);
                Constants.Set("FLASHEND", 0x7FFF);

                /* Fuses */
                Constants.Set("FUSE_MEMORY_SIZE", 3);

                /* Low Fuse Byte */
                Constants.Set("FUSE_CKSEL0", ~Constants._BV(0));  /* Select Clock Source */
                Constants.Set("FUSE_CKSEL1", ~Constants._BV(1));  /* Select Clock Source */
                Constants.Set("FUSE_CKSEL2", ~Constants._BV(2));  /* Select Clock Source */
                Constants.Set("FUSE_CKSEL3", ~Constants._BV(3));  /* Select Clock Source */
                Constants.Set("FUSE_SUT0", ~Constants._BV(4));  /* Select start-up time */
                Constants.Set("FUSE_SUT1", ~Constants._BV(5));  /* Select start-up time */
                Constants.Set("FUSE_CKOUT", ~Constants._BV(6));  /* Clock output */
                Constants.Set("FUSE_CKDIV8", ~Constants._BV(7)); /* Divide clock by 8 */
                Constants.Set("LFUSE_DEFAULT", ((int)Constants.Get("FUSE_CKSEL0") & 
                                                (int)Constants.Get("FUSE_CKSEL2") & 
                                                (int)Constants.Get("FUSE_CKSEL3") & 
                                                (int)Constants.Get("FUSE_SUT0") & 
                                                (int)Constants.Get("FUSE_CKDIV8")));

                /* High Fuse Byte */
                Constants.Set("FUSE_BOOTRST", ~Constants._BV(0));
                Constants.Set("FUSE_BOOTSZ0", ~Constants._BV(1));
                Constants.Set("FUSE_BOOTSZ1", ~Constants._BV(2));
                Constants.Set("FUSE_EESAVE", ~Constants._BV(3));  /* EEPROM memory is preserved through chip erase */
                Constants.Set("FUSE_WDTON", ~Constants._BV(4));  /* Watchdog Timer Always On */
                Constants.Set("FUSE_SPIEN", ~Constants._BV(5));  /* Enable Serial programming and Data Downloading */
                Constants.Set("FUSE_DWEN", ~Constants._BV(6));  /* debugWIRE Enable */
                Constants.Set("FUSE_RSTDISBL", ~Constants._BV(7));  /* External reset disable */
                Constants.Set("HFUSE_DEFAULT", ((int)Constants.Get("FUSE_BOOTSZ0") & 
                                                (int)Constants.Get("FUSE_BOOTSZ1") & 
                                                (int)Constants.Get("FUSE_SPIEN")));

                /* Extended Fuse Byte */
                Constants.Set("FUSE_BODLEVEL0", ~Constants._BV(0));  /* Brown-out Detector trigger level */
                Constants.Set("FUSE_BODLEVEL1", ~Constants._BV(1));  /* Brown-out Detector trigger level */
                Constants.Set("FUSE_BODLEVEL2", ~Constants._BV(2));  /* Brown-out Detector trigger level */
                Constants.Set("EFUSE_DEFAULT", (0xFF));

                /* Lock Bits */
                Constants.Set("__LOCK_BITS_EXIST", 1);
                Constants.Set("__BOOT_LOCK_BITS_0_EXIST", 1);
                Constants.Set("__BOOT_LOCK_BITS_1_EXIST", 1);

                /* Signature */
                Constants.Set("SIGNATURE_0", 0x1E);
                Constants.Set("SIGNATURE_1", 0x95);

                if (Constants.Defined("__AVR_ATmega328__"))
                    Constants.Set("SIGNATURE_2", 0x14);
                else /* ATmega328P */
                    Constants.Set("SIGNATURE_2", 0x0F);


                Constants.Set("SLEEP_MODE_IDLE", (0x00 << 1));
                Constants.Set("SLEEP_MODE_ADC", (0x01 << 1));
                Constants.Set("SLEEP_MODE_PWR_DOWN", (0x02 << 1));
                Constants.Set("SLEEP_MODE_PWR_SAVE", (0x03 << 1));
                Constants.Set("SLEEP_MODE_STANDBY", (0x06 << 1));
                Constants.Set("SLEEP_MODE_EXT_STANDBY", (0x07 << 1));

            }  /* _AVR_IOM328P_H_ */
        }
    }
}