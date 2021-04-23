using SimulIDE.src.simavr.cores.avr;
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
        protected static Dictionary<string, object> constants;

        public static void Set(string name, object value)
        {
            if (constants.ContainsKey(name))
                constants[name] = value;
            else constants.Add(name, value);
        }

        public static object Get(string name)
        {
            if (constants.TryGetValue(name, out object result))
                return result;
            else return null;
        }

        public static bool Defined(string name)
        {
            return Get(name) != null;
        }
       

        public static void InitConstants()
        {
            io.InitConstants();

           



















//            Set("_AVR_IOMX8_H_", 1);

//            /* I/O registers */

//            /* Port B */
//            Set("PINB", _SFR_IO8(0x03));
//            /* PINB */
//            Set("PINB7", 7);
//            Set("PINB6", 6);
//            Set("PINB5", 5);
//            Set("PINB4", 4);
//            Set("PINB3", 3);
//            Set("PINB2", 2);
//            Set("PINB1", 1);
//            Set("PINB0", 0);

//            Set("DDRB", _SFR_IO8(0x04));
//            /* DDRB */
//            Set("DDB7    7
//Set("DDB6    6
//Set("DDB5    5
//Set("DDB4    4
//Set("DDB3    3
//Set("DDB2    2
//Set("DDB1    1
//Set("DDB0    0

//Set("PORTB   _SFR_IO8 (0x05)
//            /* PORTB */
//Set("PB7     7
//Set("PB6     6
//Set("PB5     5
//Set("PB4     4
//Set("PB3     3
//Set("PB2     2
//Set("PB1     1
//Set("PB0     0

//            /* Port C */

//Set("PINC    _SFR_IO8 (0x06)
//            /* PINC */
//Set("PINC6   6
//Set("PINC5   5
//Set("PINC4   4
//Set("PINC3   3
//Set("PINC2   2
//Set("PINC1   1
//Set("PINC0   0

//Set("DDRC    _SFR_IO8 (0x07)
//            /* DDRC */
//Set("DDC6    6
//Set("DDC5    5
//Set("DDC4    4
//Set("DDC3    3
//Set("DDC2    2
//Set("DDC1    1
//Set("DDC0    0

//Set("PORTC   _SFR_IO8 (0x08)
//            /* PORTC */
//Set("PC6     6
//Set("PC5     5
//Set("PC4     4
//Set("PC3     3
//Set("PC2     2
//Set("PC1     1
//Set("PC0     0

//            /* Port D */

//Set("PIND    _SFR_IO8 (0x09)
//            /* PIND */
//Set("PIND7   7
//Set("PIND6   6
//Set("PIND5   5
//Set("PIND4   4
//Set("PIND3   3
//Set("PIND2   2
//Set("PIND1   1
//Set("PIND0   0

//Set("DDRD    _SFR_IO8 (0x0A)
//            /* DDRD */
//Set("DDD7    7
//Set("DDD6    6
//Set("DDD5    5
//Set("DDD4    4
//Set("DDD3    3
//Set("DDD2    2
//Set("DDD1    1
//Set("DDD0    0

//Set("PORTD   _SFR_IO8 (0x0B)
//            /* PORTD */
//Set("PD7     7
//Set("PD6     6
//Set("PD5     5
//Set("PD4     4
//Set("PD3     3
//Set("PD2     2
//Set("PD1     1
//Set("PD0     0

//Set("TIFR0   _SFR_IO8 (0x15)
//            /* TIFR0 */
//Set("OCF0B   2
//Set("OCF0A   1
//Set("TOV0    0

//Set("TIFR1   _SFR_IO8 (0x16)
//            /* TIFR1 */
//Set("ICF1    5
//Set("OCF1B   2
//Set("OCF1A   1
//Set("TOV1    0

//Set("TIFR2   _SFR_IO8 (0x17)
//            /* TIFR2 */
//Set("OCF2B   2
//Set("OCF2A   1
//Set("TOV2    0

//Set("PCIFR   _SFR_IO8 (0x1B)
//            /* PCIFR */
//Set("PCIF2   2
//Set("PCIF1   1
//Set("PCIF0   0

//Set("EIFR    _SFR_IO8 (0x1C)
//            /* EIFR */
//Set("INTF1   1
//Set("INTF0   0

//Set("EIMSK   _SFR_IO8 (0x1D)
//            /* EIMSK */
//Set("INT1    1
//Set("INT0    0

//Set("GPIOR0  _SFR_IO8 (0x1E)

//Set("EECR    _SFR_IO8(0x1F)
//            /* EECT - EEPROM Control Register */
//Set("EEPM1   5
//Set("EEPM0   4
//Set("EERIE   3
//Set("EEMPE   2
//Set("EEPE    1
//Set("EERE    0

//Set("EEDR    _SFR_IO8(0X20)

//            /* Combine EEARL and EEARH */
//Set("EEAR    _SFR_IO16(0x21)
//Set("EEARL   _SFR_IO8(0x21)
//Set("EEARH   _SFR_IO8(0X22)
//            /* 
//            Even though EEARH is not used by the mega48, the EEAR8 bit in the register
//            must be written to 0, according to the datasheet, hence the EEARH register
//            must be defined for the mega48.
//            */
//            /* 6-char sequence denoting where to find the EEPROM registers in memory space.
//               Adresses denoted in hex syntax with uppercase letters. Used by the EEPROM
//               subroutines.
//               First two letters:  EECR address.
//               Second two letters: EEDR address.
//               Last two letters:   EEAR address.  */
//Set("__EEPROM_REG_LOCATIONS__ 1F2021


//Set("GTCCR   _SFR_IO8 (0x23)
//            /* GTCCR */
//Set("TSM     7
//Set("PSRASY  1
//Set("PSRSYNC 0

//Set("TCCR0A  _SFR_IO8 (0x24)
//            /* TCCR0A */
//Set("COM0A1  7
//Set("COM0A0  6
//Set("COM0B1  5
//Set("COM0B0  4
//Set("WGM01   1
//Set("WGM00   0

//Set("TCCR0B  _SFR_IO8 (0x25)
//            /* TCCR0A */
//Set("FOC0A   7
//Set("FOC0B   6
//Set("WGM02   3
//Set("CS02    2
//Set("CS01    1
//Set("CS00    0

//Set("TCNT0   _SFR_IO8 (0x26)
//Set("OCR0A   _SFR_IO8 (0x27)
//Set("OCR0B   _SFR_IO8 (0x28)

//Set("GPIOR1  _SFR_IO8 (0x2A)
//Set("GPIOR2  _SFR_IO8 (0x2B)

//Set("SPCR    _SFR_IO8 (0x2C)
//            /* SPCR */
//Set("SPIE    7
//Set("SPE     6
//Set("DORD    5
//Set("MSTR    4
//Set("CPOL    3
//Set("CPHA    2
//Set("SPR1    1
//Set("SPR0    0

//Set("SPSR    _SFR_IO8 (0x2D)
//            /* SPSR */
//Set("SPIF    7
//Set("WCOL    6
//Set("SPI2X   0

//Set("SPDR    _SFR_IO8 (0x2E)

//Set("ACSR    _SFR_IO8 (0x30)
//            /* ACSR */
//Set("ACD     7
//Set("ACBG    6
//Set("ACO     5
//Set("ACI     4
//Set("ACIE    3
//Set("ACIC    2
//Set("ACIS1   1
//Set("ACIS0   0

//Set("MONDR   _SFR_IO8 (0x31)

//Set("SMCR    _SFR_IO8 (0x33)
//            /* SMCR */
//Set("SM2     3
//Set("SM1     2
//Set("SM0     1
//Set("SE      0

//Set("MCUSR   _SFR_IO8 (0x34)
//            /* MCUSR */
//Set("WDRF    3
//Set("BORF    2
//Set("EXTRF   1
//Set("PORF    0

//Set("MCUCR   _SFR_IO8 (0x35)
//            /* MCUCR */
//Set("PUD     4
//#if defined (__AVR_ATmega88__) || defined (__AVR_ATmega168__) 
//Set("IVSEL   1
//Set("IVCE    0
//#endif

//Set("SPMCSR  _SFR_IO8 (0x37)
//            /* SPMCSR */
//Set("SPMIE     7
//#if defined (__AVR_ATmega88__) || defined (__AVR_ATmega168__) || (__AVR_ATmega88P__) || defined (__AVR_ATmega168P__) || (__AVR_ATmega88A__) || defined (__AVR_ATmega168A__) || (__AVR_ATmega88PA__) || defined (__AVR_ATmega168PA__)
//Set("RWWSB   6
//Set("RWWSRE  4
//#endif
//#if defined(__AVR_ATmega48A) || defined(__AVR_ATmega48PA) || defined(__AVR_ATmega88A) || defined(__AVR_ATmega88PA) || defined(__AVR_ATmega168A) || defined(__AVR_ATmega168PA)
//Set("SIGRD 5
//#endif
//Set("BLBSET    3
//Set("PGWRT     2
//Set("PGERS     1
//Set("SELFPRGEN 0
//Set("SPMEN     0

//            /* 0x3D..0x3E SP  [defined in <avr/io.h>] */
//            /* 0x3F SREG      [defined in <avr/io.h>] */

//Set("WDTCSR  _SFR_MEM8 (0x60)
//            /* WDTCSR */
//Set("WDIF    7
//Set("WDIE    6
//Set("WDP3    5
//Set("WDCE    4
//Set("WDE     3
//Set("WDP2    2
//Set("WDP1    1
//Set("WDP0    0

//Set("CLKPR   _SFR_MEM8 (0x61)
//            /* CLKPR */
//Set("CLKPCE  7
//Set("CLKPS3  3
//Set("CLKPS2  2
//Set("CLKPS1  1
//Set("CLKPS0  0

//Set("PRR     _SFR_MEM8 (0x64)
//            /* PRR */
//Set("PRTWI    7
//Set("PRTIM2   6
//Set("PRTIM0   5
//Set("PRTIM1   3
//Set("PRSPI    2
//Set("PRUSART0 1
//Set("PRADC    0

//Set("__AVR_HAVE_PRR	((1<<PRADC)|(1<<PRUSART0)|(1<<PRSPI)|(1<<PRTIM1)|(1<<PRTIM0)|(1<<PRTIM2)|(1<<PRTWI))
//Set("__AVR_HAVE_PRR_PRADC
//Set("__AVR_HAVE_PRR_PRUSART0
//Set("__AVR_HAVE_PRR_PRSPI
//Set("__AVR_HAVE_PRR_PRTIM1
//Set("__AVR_HAVE_PRR_PRTIM0
//Set("__AVR_HAVE_PRR_PRTIM2
//Set("__AVR_HAVE_PRR_PRTWI

//Set("OSCCAL  _SFR_MEM8 (0x66)

//Set("PCICR   _SFR_MEM8 (0x68)
//            /* PCICR */
//Set("PCIE2   2
//Set("PCIE1   1
//Set("PCIE0   0

//Set("EICRA   _SFR_MEM8 (0x69)
//            /* EICRA */
//Set("ISC11   3
//Set("ISC10   2
//Set("ISC01   1
//Set("ISC00   0

//Set("PCMSK0  _SFR_MEM8 (0x6B)
//            /* PCMSK0 */
//Set("PCINT7    7
//Set("PCINT6    6
//Set("PCINT5    5
//Set("PCINT4    4
//Set("PCINT3    3
//Set("PCINT2    2
//Set("PCINT1    1
//Set("PCINT0    0

//Set("PCMSK1  _SFR_MEM8 (0x6C)
//            /* PCMSK1 */
//Set("PCINT14   6
//Set("PCINT13   5
//Set("PCINT12   4
//Set("PCINT11   3
//Set("PCINT10   2
//Set("PCINT9    1
//Set("PCINT8    0

//Set("PCMSK2  _SFR_MEM8 (0x6D)
//            /* PCMSK2 */
//Set("PCINT23   7
//Set("PCINT22   6
//Set("PCINT21   5
//Set("PCINT20   4
//Set("PCINT19   3
//Set("PCINT18   2
//Set("PCINT17   1
//Set("PCINT16   0

//Set("TIMSK0  _SFR_MEM8 (0x6E)
//            /* TIMSK0 */
//Set("OCIE0B  2
//Set("OCIE0A  1
//Set("TOIE0   0

//Set("TIMSK1  _SFR_MEM8 (0x6F)
//            /* TIMSK1 */
//Set("ICIE1   5
//Set("OCIE1B  2
//Set("OCIE1A  1
//Set("TOIE1   0

//Set("TIMSK2  _SFR_MEM8 (0x70)
//            /* TIMSK2 */
//Set("OCIE2B  2
//Set("OCIE2A  1
//Set("TOIE2   0

//# ifndef __ASSEMBLER__
//Set("ADC     _SFR_MEM16 (0x78)
//#endif
//Set("ADCW    _SFR_MEM16 (0x78)
//Set("ADCL    _SFR_MEM8 (0x78)
//Set("ADCH    _SFR_MEM8 (0x79)

//Set("ADCSRA  _SFR_MEM8 (0x7A)
//            /* ADCSRA */
//Set("ADEN    7
//Set("ADSC    6
//Set("ADATE   5
//Set("ADIF    4
//Set("ADIE    3
//Set("ADPS2   2
//Set("ADPS1   1
//Set("ADPS0   0

//Set("ADCSRB  _SFR_MEM8 (0x7B)
//            /* ADCSRB */
//Set("ACME    6
//Set("ADTS2   2
//Set("ADTS1   1
//Set("ADTS0   0

//Set("ADMUX   _SFR_MEM8 (0x7C)
//            /* ADMUX */
//Set("REFS1   7
//Set("REFS0   6
//Set("ADLAR   5
//Set("MUX3    3
//Set("MUX2    2
//Set("MUX1    1
//Set("MUX0    0

//Set("DIDR0   _SFR_MEM8 (0x7E)
//            /* DIDR0 */
//Set("ADC5D   5
//Set("ADC4D   4
//Set("ADC3D   3
//Set("ADC2D   2
//Set("ADC1D   1
//Set("ADC0D   0

//Set("DIDR1   _SFR_MEM8 (0x7F)
//            /* DIDR1 */
//Set("AIN1D   1
//Set("AIN0D   0

//Set("TCCR1A  _SFR_MEM8 (0x80)
//            /* TCCR1A */
//Set("COM1A1  7
//Set("COM1A0  6
//Set("COM1B1  5
//Set("COM1B0  4
//Set("WGM11   1
//Set("WGM10   0

//Set("TCCR1B  _SFR_MEM8 (0x81)
//            /* TCCR1B */
//Set("ICNC1   7
//Set("ICES1   6
//Set("WGM13   4
//Set("WGM12   3
//Set("CS12    2
//Set("CS11    1
//Set("CS10    0

//Set("TCCR1C  _SFR_MEM8 (0x82)
//            /* TCCR1C */
//Set("FOC1A   7
//Set("FOC1B   6

//Set("TCNT1   _SFR_MEM16 (0x84)
//Set("TCNT1L  _SFR_MEM8 (0x84)
//Set("TCNT1H  _SFR_MEM8 (0x85)

//Set("ICR1    _SFR_MEM16 (0x86)
//Set("ICR1L   _SFR_MEM8 (0x86)
//Set("ICR1H   _SFR_MEM8 (0x87)

//Set("OCR1A   _SFR_MEM16 (0x88)
//Set("OCR1AL  _SFR_MEM8 (0x88)
//Set("OCR1AH  _SFR_MEM8 (0x89)

//Set("OCR1B   _SFR_MEM16 (0x8A)
//Set("OCR1BL  _SFR_MEM8 (0x8A)
//Set("OCR1BH  _SFR_MEM8 (0x8B)

//Set("TCCR2A  _SFR_MEM8 (0xB0)
//            /* TCCR2A */
//Set("COM2A1  7
//Set("COM2A0  6
//Set("COM2B1  5
//Set("COM2B0  4
//Set("WGM21   1
//Set("WGM20   0

//Set("TCCR2B  _SFR_MEM8 (0xB1)
//            /* TCCR2B */
//Set("FOC2A   7
//Set("FOC2B   6
//Set("WGM22   3
//Set("CS22    2
//Set("CS21    1
//Set("CS20    0

//Set("TCNT2   _SFR_MEM8 (0xB2)
//Set("OCR2A   _SFR_MEM8 (0xB3)
//Set("OCR2B   _SFR_MEM8 (0xB4)

//Set("ASSR    _SFR_MEM8 (0xB6)
//            /* ASSR */
//Set("EXCLK    6
//Set("AS2      5
//Set("TCN2UB   4
//Set("OCR2AUB  3
//Set("OCR2BUB  2
//Set("TCR2AUB  1
//Set("TCR2BUB  0

//Set("TWBR    _SFR_MEM8 (0xB8)

//Set("TWSR    _SFR_MEM8 (0xB9)
//            /* TWSR */
//Set("TWS7    7
//Set("TWS6    6
//Set("TWS5    5
//Set("TWS4    4
//Set("TWS3    3
//Set("TWPS1   1
//Set("TWPS0   0

//Set("TWAR    _SFR_MEM8 (0xBA)
//            /* TWAR */
//Set("TWA6    7
//Set("TWA5    6
//Set("TWA4    5
//Set("TWA3    4
//Set("TWA2    3
//Set("TWA1    2
//Set("TWA0    1
//Set("TWGCE   0

//Set("TWDR    _SFR_MEM8 (0xBB)

//Set("TWCR    _SFR_MEM8 (0xBC)
//            /* TWCR */
//Set("TWINT   7
//Set("TWEA    6
//Set("TWSTA   5
//Set("TWSTO   4
//Set("TWWC    3
//Set("TWEN    2
//Set("TWIE    0

//Set("TWAMR   _SFR_MEM8 (0xBD)
//            /* TWAMR */
//Set("TWAM6   7
//Set("TWAM5   6
//Set("TWAM4   5
//Set("TWAM3   4
//Set("TWAM2   3
//Set("TWAM1   2
//Set("TWAM0   1

//Set("UCSR0A  _SFR_MEM8 (0xC0)
//            /* UCSR0A */
//Set("RXC0    7
//Set("TXC0    6
//Set("UDRE0   5
//Set("FE0     4
//Set("DOR0    3
//Set("UPE0    2
//Set("U2X0    1
//Set("MPCM0   0

//Set("UCSR0B  _SFR_MEM8 (0xC1)
//            /* UCSR0B */
//Set("RXCIE0  7
//Set("TXCIE0  6
//Set("UDRIE0  5
//Set("RXEN0   4
//Set("TXEN0   3
//Set("UCSZ02  2
//Set("RXB80   1
//Set("TXB80   0

//Set("UCSR0C  _SFR_MEM8 (0xC2)
//            /* UCSR0C */
//Set("UMSEL01  7
//Set("UMSEL00  6
//Set("UPM01    5
//Set("UPM00    4
//Set("USBS0    3
//Set("UCSZ01   2
//Set("UDORD0   2
//Set("UCSZ00   1
//Set("UCPHA0   1
//Set("UCPOL0   0

//Set("UBRR0   _SFR_MEM16 (0xC4)
//Set("UBRR0L  _SFR_MEM8 (0xC4)
//Set("UBRR0H  _SFR_MEM8 (0xC5)
//Set("UDR0    _SFR_MEM8 (0xC6)

//            /* Interrupt vectors */

//            /* External Interrupt Request 0 */
//Set("INT0_vect_num		1
//Set("INT0_vect			_VECTOR(1)
//Set("SIG_INTERRUPT0			_VECTOR(1)

//            /* External Interrupt Request 1 */
//Set("INT1_vect_num		2
//Set("INT1_vect			_VECTOR(2)
//Set("SIG_INTERRUPT1			_VECTOR(2)

//            /* Pin Change Interrupt Request 0 */
//Set("PCINT0_vect_num		3
//Set("PCINT0_vect			_VECTOR(3)
//Set("SIG_PIN_CHANGE0			_VECTOR(3)

//            /* Pin Change Interrupt Request 0 */
//Set("PCINT1_vect_num		4
//Set("PCINT1_vect			_VECTOR(4)
//Set("SIG_PIN_CHANGE1			_VECTOR(4)

//            /* Pin Change Interrupt Request 1 */
//Set("PCINT2_vect_num		5
//Set("PCINT2_vect			_VECTOR(5)
//Set("SIG_PIN_CHANGE2			_VECTOR(5)

//            /* Watchdog Time-out Interrupt */
//Set("WDT_vect_num		6
//Set("WDT_vect			_VECTOR(6)
//Set("SIG_WATCHDOG_TIMEOUT		_VECTOR(6)

//            /* Timer/Counter2 Compare Match A */
//Set("TIMER2_COMPA_vect_num	7
//Set("TIMER2_COMPA_vect		_VECTOR(7)
//Set("SIG_OUTPUT_COMPARE2A		_VECTOR(7)

//            /* Timer/Counter2 Compare Match A */
//Set("TIMER2_COMPB_vect_num	8
//Set("TIMER2_COMPB_vect		_VECTOR(8)
//Set("SIG_OUTPUT_COMPARE2B		_VECTOR(8)

//            /* Timer/Counter2 Overflow */
//Set("TIMER2_OVF_vect_num		9
//Set("TIMER2_OVF_vect			_VECTOR(9)
//Set("SIG_OVERFLOW2			_VECTOR(9)

//            /* Timer/Counter1 Capture Event */
//Set("TIMER1_CAPT_vect_num	10
//Set("TIMER1_CAPT_vect		_VECTOR(10)
//Set("SIG_INPUT_CAPTURE1		_VECTOR(10)

//            /* Timer/Counter1 Compare Match A */
//Set("TIMER1_COMPA_vect_num	11
//Set("TIMER1_COMPA_vect		_VECTOR(11)
//Set("SIG_OUTPUT_COMPARE1A		_VECTOR(11)

//            /* Timer/Counter1 Compare Match B */
//Set("TIMER1_COMPB_vect_num	12
//Set("TIMER1_COMPB_vect		_VECTOR(12)
//Set("SIG_OUTPUT_COMPARE1B		_VECTOR(12)

//            /* Timer/Counter1 Overflow */
//Set("TIMER1_OVF_vect_num		13
//Set("TIMER1_OVF_vect			_VECTOR(13)
//Set("SIG_OVERFLOW1			_VECTOR(13)

//            /* TimerCounter0 Compare Match A */
//Set("TIMER0_COMPA_vect_num	14
//Set("TIMER0_COMPA_vect		_VECTOR(14)
//Set("SIG_OUTPUT_COMPARE0A		_VECTOR(14)

//            /* TimerCounter0 Compare Match B */
//Set("TIMER0_COMPB_vect_num	15
//Set("TIMER0_COMPB_vect		_VECTOR(15)
//Set("SIG_OUTPUT_COMPARE0B		_VECTOR(15)

//            /* Timer/Couner0 Overflow */
//Set("TIMER0_OVF_vect_num		16
//Set("TIMER0_OVF_vect			_VECTOR(16)
//Set("SIG_OVERFLOW0			_VECTOR(16)

//            /* SPI Serial Transfer Complete */
//Set("SPI_STC_vect_num		17
//Set("SPI_STC_vect			_VECTOR(17)
//Set("SIG_SPI				_VECTOR(17)

//            /* USART Rx Complete */
//Set("USART_RX_vect_num		18
//Set("USART_RX_vect			_VECTOR(18)
//Set("SIG_USART_RECV			_VECTOR(18)

//            /* USART, Data Register Empty */
//Set("USART_UDRE_vect_num		19
//Set("USART_UDRE_vect			_VECTOR(19)
//Set("SIG_USART_DATA			_VECTOR(19)

//            /* USART Tx Complete */
//Set("USART_TX_vect_num		20
//Set("USART_TX_vect			_VECTOR(20)
//Set("SIG_USART_TRANS			_VECTOR(20)

//            /* ADC Conversion Complete */
//Set("ADC_vect_num		21
//Set("ADC_vect			_VECTOR(21)
//Set("SIG_ADC				_VECTOR(21)

//            /* EEPROM Ready */
//Set("EE_READY_vect_num		22
//Set("EE_READY_vect			_VECTOR(22)
//Set("SIG_EEPROM_READY		_VECTOR(22)

//            /* Analog Comparator */
//Set("ANALOG_COMP_vect_num	23
//Set("ANALOG_COMP_vect		_VECTOR(23)
//Set("SIG_COMPARATOR			_VECTOR(23)

//            /* Two-wire Serial Interface */
//Set("TWI_vect_num		24
//Set("TWI_vect			_VECTOR(24)
//Set("SIG_TWI				_VECTOR(24)
//Set("SIG_2WIRE_SERIAL		_VECTOR(24)

//            /* Store Program Memory Read */
//Set("SPM_READY_vect_num		25
//Set("SPM_READY_vect			_VECTOR(25)
//Set("SIG_SPM_READY			_VECTOR(25)

//            /* The mega48 and mega88 vector tables are single instruction entries (16 bits
//               per entry for an RJMP) while the mega168 table has double instruction
//               entries (32 bits per entry for a JMP). */

//#if defined (__AVR_ATmega168__) || defined (__AVR_ATmega168A__)
//Set("_VECTORS_SIZE 104
//#else
//Set("_VECTORS_SIZE 52
//#endif


//#endif /* _AVR_IOM8_H_ */ 






//            /* Low Fuse Byte */

//            //#define FUSE_CKSEL0 (unsigned char)~_BV(0)  /* Select Clock Source */
//            //#define FUSE_CKSEL1 (unsigned char)~_BV(1)  /* Select Clock Source */
//            //#define FUSE_CKSEL2 (unsigned char)~_BV(2)  /* Select Clock Source */
//            //#define FUSE_CKSEL3 (unsigned char)~_BV(3)  /* Select Clock Source */
//            //#define FUSE_SUT0   (unsigned char)~_BV(4)  /* Select start-up time */
//            //#define FUSE_SUT1   (unsigned char)~_BV(5)  /* Select start-up time */
//            //#define FUSE_CKOUT  (unsigned char)~_BV(6)  /* Clock output */
//            //#define FUSE_CKDIV8 (unsigned char)~_BV(7) /* Divide clock by 8 */
//            //#define LFUSE_DEFAULT (FUSE_CKSEL0 & FUSE_CKSEL2 & FUSE_CKSEL3 & FUSE_SUT0 & FUSE_CKDIV8)

//            //        /* High Fuse Byte */
//            //#define FUSE_BOOTRST (unsigned char)~_BV(0)
//            //#define FUSE_BOOTSZ0 (unsigned char)~_BV(1)
//            //#define FUSE_BOOTSZ1 (unsigned char)~_BV(2)
//            //#define FUSE_EESAVE    (unsigned char)~_BV(3)  /* EEPROM memory is preserved through chip erase */
//            //#define FUSE_WDTON     (unsigned char)~_BV(4)  /* Watchdog Timer Always On */
//            //#define FUSE_SPIEN     (unsigned char)~_BV(5)  /* Enable Serial programming and Data Downloading */
//            //#define FUSE_DWEN      (unsigned char)~_BV(6)  /* debugWIRE Enable */
//            //#define FUSE_RSTDISBL  (unsigned char)~_BV(7)  /* External reset disable */
//            //#define HFUSE_DEFAULT (FUSE_BOOTSZ0 & FUSE_BOOTSZ1 & FUSE_SPIEN)

//            //        /* Extended Fuse Byte */
//            //#define FUSE_BODLEVEL0 (unsigned char)~_BV(0)  /* Brown-out Detector trigger level */
//            //#define FUSE_BODLEVEL1 (unsigned char)~_BV(1)  /* Brown-out Detector trigger level */
//            //#define FUSE_BODLEVEL2 (unsigned char)~_BV(2)  /* Brown-out Detector trigger level */
//            //#define EFUSE_DEFAULT  (0xFF)



//            /* Lock Bits */
//        public bool __LOCK_BITS_EXIST = true;
//        public bool __BOOT_LOCK_BITS_0_EXIST = true;
//        public bool __BOOT_LOCK_BITS_1_EXIST = true;

//        Add(" SLEEP_MODE_IDLE = (0x00 << 1);
//        Add(" SLEEP_MODE_ADC = (0x01 << 1);
//        Add(" SLEEP_MODE_PWR_DOWN = (0x02 << 1);
//        Add(" SLEEP_MODE_PWR_SAVE = (0x03 << 1);
//        Add(" SLEEP_MODE_STANDBY = (0x06 << 1);
//        Add(" SLEEP_MODE_EXT_STANDBY = (0x07 << 1);

//        public const byte _AVR_IOM328P_H_ = 1;
//        public static string SIM_MMCU;
//        Add(" SIM_VECTOR_SIZE;


    }

//    Add("[] FUSE
//        {
//            get
//            {
//                switch (Constants.FUSE_MEMORY_SIZE)
//                {
//                    case 6: return new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
//                    case 3: return new byte[] { Constants.LFUSE_DEFAULT, Constants.HFUSE_DEFAULT, Constants.EFUSE_DEFAULT };
//                    case 2: return new byte[] { Constants.LFUSE_DEFAULT, Constants.HFUSE_DEFAULT };
//                    case 1: return new byte[] { Constants.FUSE_DEFAULT };
//                    default: return new byte[] { 0 };
//                }
//            }
//        }

//        Add("[] SIGNATURE
//        {
//            get
//            {
//                return new byte[3] { SIGNATURE_0, SIGNATURE_1, SIGNATURE_2 };
//            }
//        }

//        Add(" MCU_STATUS_REG
//        {
//            get
//            {
//                return (byte)(MCUSR != null ? MCUSR : MCUCSR);
//            }
//        }

//        public static ResetFlags RESETFLAGS
//        {
//            get
//            {
//                ResetFlags result = new ResetFlags();
//                result.porf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, PORF);
//                result.extrf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, EXTRF);
//                result.borf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, BORF);
//                result.wdrf = Sim_regbit.AVR_IO_REGBIT(MCU_STATUS_REG, WDRF);
//                return result;
//            }
//        }

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

//        Add(" GetByte(string name)
//        {
//            return 0;
//        }

//        Add(" GetUShort(string name)
//        {
//            return 0;
//        }

//        public virtual bool Get__SIM_CORE_DECLARE_H__() { return true; }
//        /* we have to declare this, as none of the distro but debian has a modern
//         * toolchain and avr-libc. This affects a lot of names, like MCUSR etc
//        */
//        public virtual bool Get__AVR_LIBC_DEPRECATED_ENABLE__() { return true; }

        
//        Add(" __AVR_HAVE_PRR
//        {
//            get
//            {
//                return (byte)((1 << Constants.PRADC) |
//                              (1 << Constants.PRUSART0) |
//                              (1 << Constants.PRSPI) |
//                              (1 << Constants.PRTIM1) |
//                              (1 << Constants.PRTIM0) |
//                              (1 << Constants.PRTIM2) |
//                              (1 << Constants.PRTWI));
//            }

    }
}
