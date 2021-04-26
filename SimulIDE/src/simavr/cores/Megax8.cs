using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimulIDE.src.simavr.sim.Avr_eeprom_ns;

namespace SimulIDE.src.simavr.cores
{
  

    public class Megax8:Mcu
    {
        public Megax8()
        {
            InitStructure(); 
            Mx8_init(core);
        }

        private void InitPortB()
        {
            portb = new Avr_ioport();
            portb.name = "B";
            portb.r_port = (uint)Constants.Get("PORTB");
            portb.r_ddr = (uint)Constants.Get("DDRB");
            portb.r_pin = (uint)Constants.Get("PINB");
            portb.pcint = new Avr_int_vector();
            portb.pcint.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PCICR"), (byte)Constants.Get("PCIE0"));
            portb.pcint.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PCIFR"), (byte)Constants.Get("PCIF0"));
            portb.pcint.vector = (byte)Constants.Get("PCINT0_vect");
            portb.r_pcint = (uint)Constants.Get("PCMSK0");
        }
        private void InitPortC()
        {
            portc = new Avr_ioport();
            portc.name = "C";
            portc.r_port = (uint)Constants.Get("PORTC");
            portc.r_ddr = (uint)Constants.Get("DDRC");
            portc.r_pin = (uint)Constants.Get("PINC");
            portc.pcint = new Avr_int_vector();
            portc.pcint.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PCICR"), (byte)Constants.Get("PCIE1"));
            portc.pcint.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PCIFR"), (byte)Constants.Get("PCIF1"));
            portc.pcint.vector = (byte)Constants.Get("PCINT1_vect");
            portc.r_pcint = (uint)Constants.Get("PCMSK1");
        }
        private void InitPortD()
        {
            portd = new Avr_ioport();
            portd.name = "D";
            portd.r_port =(uint) Constants.Get("PORTD");
            portd.r_ddr = (uint) Constants.Get("DDRD");
            portd.r_pin = (uint) Constants.Get("PIND");
            portd.pcint = new Avr_int_vector();
            portd.pcint.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PCICR"),(byte) Constants.Get("PCIE2"));
            portd.pcint.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PCIFR"),(byte) Constants.Get("PCIF2"));
            portd.pcint.vector = (byte) Constants.Get("PCINT2_vect");
            portd.r_pcint = (uint) Constants.Get("PCMSK2");
        }

        private void InitTimer0()
        {
            timer0 = new Avr_timer();
            timer0.name = "0";
            timer0.disabled = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PRR"),(byte)Constants.Get("PRTIM0"));
            timer0.wgm[0] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR0A"), (byte)Constants.Get("WGM00"));
            timer0.wgm[1] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR0A"), (byte)Constants.Get("WGM01"));
            timer0.wgm[2] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR0B"), (byte)Constants.Get("WGM02"));
            
            timer0.wgm_op[0] = Avr_timer_helper.AVR_TIMER_WGM_NORMAL8();
            timer0.wgm_op[1] = Avr_timer_helper.AVR_TIMER_WGM_FCPWM8();
            timer0.wgm_op[2] = Avr_timer_helper.AVR_TIMER_WGM_CTC();
            timer0.wgm_op[3] = Avr_timer_helper.AVR_TIMER_WGM_FASTPWM8();
            timer0.wgm_op[7] = Avr_timer_helper.AVR_TIMER_WGM_OCPWM();

            timer0.cs[0] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR0B"), (byte)Constants.Get("CS00"));
            timer0.cs[1] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR0B"), (byte)Constants.Get("CS01"));
            timer0.cs[2] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR0B"), (byte)Constants.Get("CS02"));
                
            timer0.cs_div = new byte[] { 0, 0, 3 /* 8 */, 6 /* 64 */, 8 /* 256 */, 10 /* 1024 */, Avr_timer_helper.AVR_TIMER_EXTCLK_CHOOSE, Avr_timer_helper.AVR_TIMER_EXTCLK_CHOOSE };
            timer0.ext_clock_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTD"), 4); /* External clock pin */

            timer0.r_tcnt = Constants.Get("TCNT0");
            timer0.overflow = new Avr_int_vector();
            timer0.overflow.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK0"), (byte)Constants.Get("TOIE0"));
            timer0.overflow.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR0"), (byte)Constants.Get("TOV0"));
            timer0.overflow.vector = Constants.Get("TIMER0_OVF_vect");
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA] = new Avr_timer_comp();
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].r_ocr = Constants.Get("OCR0A");
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].com = Sim_regbit.AVR_IO_REGBITS((uint)Constants.Get("TCCR0A"), (byte)Constants.Get("COM0A0"), 0x3);
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].com_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTD"), 6);
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt = new Avr_int_vector();
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK0"), (byte)Constants.Get("OCIE0A"));
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR0"), (byte)Constants.Get("OCF0A"));
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.vector = Constants.Get("TIMER0_COMPA_vect");

            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB] = new Avr_timer_comp();
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].r_ocr = Constants.Get("OCR0B");
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].com = Sim_regbit.AVR_IO_REGBITS((uint)Constants.Get("TCCR0A"), (byte)Constants.Get("COM0B0"), 0x3);
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].com_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTD"), 5);
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt = new Avr_int_vector();
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK0"), (byte)Constants.Get("OCIE0B"));
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR0"), (byte)Constants.Get("OCF0B"));
            timer0.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.vector = Constants.Get("TIMER0_COMPB_vect");
        }
        private void InitTimer1()
        {
            timer1 = new Avr_timer();
            timer1.name = "1";
            timer1.disabled = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PRR"), (byte)Constants.Get("PRTIM1"));
            timer1.wgm[0] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1A"), (byte)Constants.Get("WGM10"));
            timer1.wgm[1] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1A"), (byte)Constants.Get("WGM11"));
            timer1.wgm[2] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1B"), (byte)Constants.Get("WGM12"));
            
            timer1.wgm_op[0] = Avr_timer_helper.AVR_TIMER_WGM_NORMAL16();
            timer1.wgm_op[1] = Avr_timer_helper.AVR_TIMER_WGM_FCPWM8();
            timer1.wgm_op[2] = Avr_timer_helper.AVR_TIMER_WGM_FCPWM9();
            timer1.wgm_op[3] = Avr_timer_helper.AVR_TIMER_WGM_FCPWM10();
            timer1.wgm_op[4] = Avr_timer_helper.AVR_TIMER_WGM_CTC();
            timer1.wgm_op[5] = Avr_timer_helper.AVR_TIMER_WGM_FASTPWM8();
            timer1.wgm_op[6] = Avr_timer_helper.AVR_TIMER_WGM_FASTPWM9();
            timer1.wgm_op[7] = Avr_timer_helper.AVR_TIMER_WGM_FASTPWM10();
            timer1.wgm_op[8] = Avr_timer_helper.AVR_TIMER_WGM_ICPWM();
            timer1.wgm_op[9] = Avr_timer_helper.AVR_TIMER_WGM_OCPWM();
            timer1.wgm_op[12] = Avr_timer_helper.AVR_TIMER_WGM_ICCTC();
            timer1.wgm_op[14] = Avr_timer_helper.AVR_TIMER_WGM_ICPWM();
            timer1.wgm_op[15] = Avr_timer_helper.AVR_TIMER_WGM_OCPWM();


            timer1.cs[0] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1B"), (byte)Constants.Get("CS10"));
            timer1.cs[1] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1B"), (byte)Constants.Get("CS11"));
            timer1.cs[2] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1B"), (byte)Constants.Get("CS12"));

            timer1.cs_div = new byte[] { 0, 0, 3 /* 8 */, 6 /* 64 */, 8 /* 256 */, 10 /* 1024 */, Avr_timer_helper.AVR_TIMER_EXTCLK_CHOOSE, Avr_timer_helper.AVR_TIMER_EXTCLK_CHOOSE };
            timer1.ext_clock_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTD"), 5); /* External clock pin */

            timer1.r_tcnt = Constants.Get("TCNT1L");
            timer1.r_tcnth = Constants.Get("TCNT1H");
            timer1.r_icr = Constants.Get("ICR1L");
            timer1.r_icrh = Constants.Get("ICR1H");
            timer1.ices = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR1B"), (byte)Constants.Get("ICES1"));
            timer1.icp = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTB"), 0);

            timer1.overflow = new Avr_int_vector();
            timer1.overflow.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK1"), (byte)Constants.Get("TOIE1"));
            timer1.overflow.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR1"), (byte)Constants.Get("TOV1"));
            timer1.overflow.vector = Constants.Get("TIMER1_OVF_vect");

            timer1.icr = new Avr_int_vector();
            timer1.icr.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK1"), (byte)Constants.Get("ICIE1"));
            timer1.icr.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR1"), (byte)Constants.Get("ICF1"));
            timer1.icr.vector = Constants.Get("TIMER1_CAPT_vect");

            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA] = new Avr_timer_comp();
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].r_ocr = Constants.Get("OCR1AL");
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].r_ocrh = Constants.Get("OCR1AH");
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].com = Sim_regbit.AVR_IO_REGBITS((uint)Constants.Get("TCCR1A"), (byte)Constants.Get("COM1A0"), 0x3);
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].com_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTB"), 1);
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt = new Avr_int_vector();
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK1"), (byte)Constants.Get("OCIE1A"));
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR1"), (byte)Constants.Get("OCF1A"));
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.vector = Constants.Get("TIMER1_COMPA_vect");

            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB] = new Avr_timer_comp();
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].r_ocr = Constants.Get("OCR1BL");
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].r_ocrh = Constants.Get("OCR1BH");
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].com = Sim_regbit.AVR_IO_REGBITS((uint)Constants.Get("TCCR1A"), (byte)Constants.Get("COM1B0"), 0x3);
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].com_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTB"), 2);
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt = new Avr_int_vector();
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK1"), (byte)Constants.Get("OCIE1B"));
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR1"), (byte)Constants.Get("OCF1B"));
            timer1.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.vector = Constants.Get("TIMER1_COMPB_vect");
        }
        private void InitTimer2()
        {
            timer2 = new Avr_timer();
            timer2.name = "2";
            timer2.disabled = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PRR"), (byte)Constants.Get("PRTIM2"));
            timer2.wgm[0] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR2A"), (byte)Constants.Get("WGM20"));
            timer2.wgm[1] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR2A"), (byte)Constants.Get("WGM21"));
            timer2.wgm[2] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR2B"), (byte)Constants.Get("WGM22"));
                
            timer2.wgm_op[0] = Avr_timer_helper.AVR_TIMER_WGM_NORMAL8();
            timer2.wgm_op[1] = Avr_timer_helper.AVR_TIMER_WGM_FCPWM8();
            timer2.wgm_op[2] = Avr_timer_helper.AVR_TIMER_WGM_CTC();
            timer2.wgm_op[3] = Avr_timer_helper.AVR_TIMER_WGM_FASTPWM8();
            timer2.wgm_op[7] = Avr_timer_helper.AVR_TIMER_WGM_OCPWM();

            timer2.cs[0] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR2B"), (byte)Constants.Get("CS20"));
            timer2.cs[1] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR2B"), (byte)Constants.Get("CS21"));
            timer2.cs[2] = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TCCR2B"), (byte)Constants.Get("CS22"));
                
            timer2.cs_div = new byte[] { 0, 0, 3 /* 8 */, 5 /* 32 */, 6 /* 64 */, 7 /* 128 */, 8 /* 256 */, 10 /* 1024 */ };
            timer2.ext_clock_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTD"), 4); /* External clock pin */
            timer2.r_tcnt = Constants.Get("TCNT2");
            // asynchronous timer source bit.. if set, use 32khz frequency
            timer2.as2 = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("ASSR"), (byte)Constants.Get("AS2"));
            timer2.overflow = new Avr_int_vector();
            timer2.overflow.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK2"), (byte)Constants.Get("TOIE2"));
            timer2.overflow.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR2"), (byte)Constants.Get("TOV2"));
            timer2.overflow.vector = Constants.Get("TIMER2_OVF_vect");

            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA] = new Avr_timer_comp();
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].r_ocr = Constants.Get("OCR2A");
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].com = Sim_regbit.AVR_IO_REGBITS((uint)Constants.Get("TCCR2A"), (byte)Constants.Get("COM2A0"), 0x3);
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].com_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTB"), 3);
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt = new Avr_int_vector();
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK2"), (byte)Constants.Get("OCIE2A"));
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR2"), (byte)Constants.Get("OCF2A"));
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPA].interrupt.vector = Constants.Get("TIMER2_COMPA_vect");

            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB] = new Avr_timer_comp();
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].r_ocr = Constants.Get("OCR2B");
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].com = Sim_regbit.AVR_IO_REGBITS((uint)Constants.Get("TCCR2A"), (byte)Constants.Get("COM2B0"), 0x3);
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].com_pin = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("PORTD"), 3);
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt = new Avr_int_vector();
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.enable = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIMSK2"), (byte)Constants.Get("OCIE2B"));
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.raised = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("TIFR2"), (byte)Constants.Get("OCF2B"));
            timer2.comp[Avr_timer_helper.AVR_TIMER_COMPB].interrupt.vector = Constants.Get("TIMER2_COMPB_vect");

        }




        protected void InitStructure()
        {
            core = new Avr();
            eeprom = new Avr_eeprom();
            selfprog = new Avr_flash();
            
            InitPortB(); InitPortC(); InitPortD();
            InitTimer0(); InitTimer1(); InitTimer2();

        }

        protected virtual void Avr_core_init(Avr core)
        {
            core.mmcu = (string)Constants.Get("SIM_MMCU");
            Sim_core_declare.InitConstants();
            Sim_core_declare.DEFAULT_CORE(ref core,(byte)Constants.Get("SIM_VECTOR_SIZE"));
            core.Init = Mx8_init;
            core.Reset = Mx8_reset;
            core.custom = new Custom();
        }

        protected virtual void Avr_eeprom_init(Avr core)
        {
            Avr_eeprom.Avr_eeprom_declare((Mcu)this, (byte)Constants.Get("EE_READY_vect"));
        }

        protected void Mx8_init(Avr avr)
        {
            Avr_core_init(avr);
	        Avr_eeprom_init(avr);
            Avr_initSelfprog(ref selfprog);
            Avr_flash_helper.Avr_flash_init(avr, selfprog);
//            avr_watchdog_init(avr, &mcu->watchdog);
//            avr_extint_init(avr, &mcu->extint);
            Avr_ioports.Avr_ioport_init(avr, ref portb);
            Avr_ioports.Avr_ioport_init(avr, ref portc);
            Avr_ioports.Avr_ioport_init(avr, ref portd);
//            avr_uart_init(avr, &mcu->uart);
//            avr_acomp_init(avr, &mcu->acomp);
//            avr_adc_init(avr, &mcu->adc);
            Avr_timer_helper.Avr_timer_init(avr, ref timer0);
            Avr_timer_helper.Avr_timer_init(avr, ref timer1);
            Avr_timer_helper.Avr_timer_init(avr, ref timer2);
//            avr_spi_init(avr, &mcu->spi);
//            avr_twi_init(avr, &mcu->twi);
        }

        protected void Mx8_reset(Avr avr)
        {
            Mx8_init(avr);
        }

        protected void Avr_initSelfprog(ref Avr_flash fl)
        {
            if (Constants.Defined("RWWSRE"))
                Avr_flash_helper.AVR_SELFPROG_DECLARE(ref fl, (uint)Constants.Get("SPMCSR"), (byte)Constants.Get("SELFPRGEN"), (byte)Constants.Get("SPM_READY_vect"));
            else
                Avr_flash_helper.AVR_SELFPROG_DECLARE_NORWW(ref fl,(uint)Constants.Get("SPMCSR"), (byte)Constants.Get("SELFPRGEN"), (byte)Constants.Get("SPM_READY_vect"));
        }


        //const struct mcu_t SIM_CORENAME = {
        //	.core = {
        //		.mmcu = SIM_MMCU,
        //		DEFAULT_CORE(SIM_VECTOR_SIZE),

        //		.init = mx8_init,
        //		.reset = mx8_reset,
        //	},
        //	AVR_EEPROM_DECLARE(EE_READY_vect),


        //#ifdef RWWSRE
        //	AVR_SELFPROG_DECLARE(SPMCSR, SELFPRGEN, SPM_READY_vect),
        //#else
        //	AVR_SELFPROG_DECLARE_NORWW(SPMCSR, SELFPRGEN, SPM_READY_vect),
        //#endif


        //	AVR_WATCHDOG_DECLARE(WDTCSR, WDT_vect),
        //	.extint = {
        //		AVR_EXTINT_DECLARE(0, 'D', 2),
        //		AVR_EXTINT_DECLARE(1, 'D', 3),
        //	},


        //	//PRR/PRUSART0, upe=UPE, reg/bit name index=0, no 'C' in RX/TX vector names
        //	AVR_UART_DECLARE(PRR, PRUSART0, UPE, 0, ),

        //	.acomp = {
        //		.mux_inputs = 8,
        //		.mux = { AVR_IO_REGBIT(ADMUX, MUX0), AVR_IO_REGBIT(ADMUX, MUX1),
        //				AVR_IO_REGBIT(ADMUX, MUX2) },
        //		.pradc = AVR_IO_REGBIT(PRR, PRADC),
        //		.aden = AVR_IO_REGBIT(ADCSRA, ADEN),
        //		.acme = AVR_IO_REGBIT(ADCSRB, ACME),

        //		.r_acsr = ACSR,
        //		.acis = { AVR_IO_REGBIT(ACSR, ACIS0), AVR_IO_REGBIT(ACSR, ACIS1) },
        //		.acic = AVR_IO_REGBIT(ACSR, ACIC),
        //		.aco = AVR_IO_REGBIT(ACSR, ACO),
        //		.acbg = AVR_IO_REGBIT(ACSR, ACBG),
        //		.disabled = AVR_IO_REGBIT(ACSR, ACD),

        //		.timer_name = '1',

        //		.ac = {
        //			.enable = AVR_IO_REGBIT(ACSR, ACIE),
        //			.raised = AVR_IO_REGBIT(ACSR, ACI),
        //			.vector = ANALOG_COMP_vect,
        //		}
        //	},
        //	.adc = {
        //		.r_admux = ADMUX,
        //		.mux = { AVR_IO_REGBIT(ADMUX, MUX0), AVR_IO_REGBIT(ADMUX, MUX1),
        //					AVR_IO_REGBIT(ADMUX, MUX2), AVR_IO_REGBIT(ADMUX, MUX3),},
        //		.ref = { AVR_IO_REGBIT(ADMUX, REFS0), AVR_IO_REGBIT(ADMUX, REFS1)},
        //		.ref_values = { [1] = ADC_VREF_AVCC, [3] = ADC_VREF_V110, },

        //		.adlar = AVR_IO_REGBIT(ADMUX, ADLAR),
        //		.r_adcsra = ADCSRA,
        //		.aden = AVR_IO_REGBIT(ADCSRA, ADEN),
        //		.adsc = AVR_IO_REGBIT(ADCSRA, ADSC),
        //		.adate = AVR_IO_REGBIT(ADCSRA, ADATE),
        //		.adps = { AVR_IO_REGBIT(ADCSRA, ADPS0), AVR_IO_REGBIT(ADCSRA, ADPS1), AVR_IO_REGBIT(ADCSRA, ADPS2),},

        //		.r_adch = ADCH,
        //		.r_adcl = ADCL,

        //		.r_adcsrb = ADCSRB,
        //		.adts = { AVR_IO_REGBIT(ADCSRB, ADTS0), AVR_IO_REGBIT(ADCSRB, ADTS1), AVR_IO_REGBIT(ADCSRB, ADTS2),},
        //		.adts_op = {
        //			[0] = avr_adts_free_running,
        //			[1] = avr_adts_analog_comparator_0,
        //			[2] = avr_adts_external_interrupt_0,
        //			[3] = avr_adts_timer_0_compare_match_a,
        //			[4] = avr_adts_timer_0_overflow,
        //			[5] = avr_adts_timer_1_compare_match_b,
        //			[6] = avr_adts_timer_1_overflow,
        //			[7] = avr_adts_timer_1_capture_event,
        //		},

        //		.muxmode = {
        //			[0] = AVR_ADC_SINGLE(0), [1] = AVR_ADC_SINGLE(1),
        //			[2] = AVR_ADC_SINGLE(2), [3] = AVR_ADC_SINGLE(3),
        //			[4] = AVR_ADC_SINGLE(4), [5] = AVR_ADC_SINGLE(5),
        //			[6] = AVR_ADC_SINGLE(6), [7] = AVR_ADC_SINGLE(7),
        //			[8] = AVR_ADC_TEMP(),
        //			[14] = AVR_ADC_REF(1100),	// 1.1V
        //			[15] = AVR_ADC_REF(0),		// GND
        //		},
        //		.adc = {
        //			.enable = AVR_IO_REGBIT(ADCSRA, ADIE),
        //			.raised = AVR_IO_REGBIT(ADCSRA, ADIF),
        //			.vector = ADC_vect,
        //		},
        //	},
        //	.timer0 = {
        //		.name = '0',
        //		.disabled = AVR_IO_REGBIT(PRR, PRTIM0),
        //		.wgm = { AVR_IO_REGBIT(TCCR0A, WGM00), AVR_IO_REGBIT(TCCR0A, WGM01), AVR_IO_REGBIT(TCCR0B, WGM02) },
        //		.wgm_op = {
        //			[0] = AVR_TIMER_WGM_NORMAL8(),
        //            [1] = AVR_TIMER_WGM_FCPWM8(),
        //			[2] = AVR_TIMER_WGM_CTC(),
        //			[3] = AVR_TIMER_WGM_FASTPWM8(),
        //			[7] = AVR_TIMER_WGM_OCPWM(),
        //		},
        //		.cs = { AVR_IO_REGBIT(TCCR0B, CS00), AVR_IO_REGBIT(TCCR0B, CS01), AVR_IO_REGBIT(TCCR0B, CS02) },
        //		.cs_div = { 0, 0, 3 /* 8 */, 6 /* 64 */, 8 /* 256 */, 10 /* 1024 */, AVR_TIMER_EXTCLK_CHOOSE, AVR_TIMER_EXTCLK_CHOOSE },
        //		.ext_clock_pin = AVR_IO_REGBIT(PORTD, 4), /* External clock pin */

        //		.r_tcnt = TCNT0,

        //		.overflow = {
        //			.enable = AVR_IO_REGBIT(TIMSK0, TOIE0),
        //			.raised = AVR_IO_REGBIT(TIFR0, TOV0),
        //			.vector = TIMER0_OVF_vect,
        //		},
        //		.comp = {
        //			[AVR_TIMER_COMPA] = {
        //				.r_ocr = OCR0A,
        //				.com = AVR_IO_REGBITS(TCCR0A, COM0A0, 0x3),
        //				.com_pin = AVR_IO_REGBIT(PORTD, 6),
        //				.interrupt = {
        //					.enable = AVR_IO_REGBIT(TIMSK0, OCIE0A),
        //					.raised = AVR_IO_REGBIT(TIFR0, OCF0A),
        //					.vector = TIMER0_COMPA_vect,
        //				},
        //			},
        //			[AVR_TIMER_COMPB] = {
        //				.r_ocr = OCR0B,
        //				.com = AVR_IO_REGBITS(TCCR0A, COM0B0, 0x3),
        //				.com_pin = AVR_IO_REGBIT(PORTD, 5),
        //				.interrupt = {
        //					.enable = AVR_IO_REGBIT(TIMSK0, OCIE0B),
        //					.raised = AVR_IO_REGBIT(TIFR0, OCF0B),
        //					.vector = TIMER0_COMPB_vect,
        //				}
        //			}
        //		}
        //	},
        //	.timer1 = {
        //		.name = '1',
        //		.disabled = AVR_IO_REGBIT(PRR, PRTIM1),
        //		.wgm = { AVR_IO_REGBIT(TCCR1A, WGM10), AVR_IO_REGBIT(TCCR1A, WGM11),
        //					AVR_IO_REGBIT(TCCR1B, WGM12), AVR_IO_REGBIT(TCCR1B, WGM13) },
        //		.wgm_op = {
        //			[0] = AVR_TIMER_WGM_NORMAL16(),
        //			[1] = AVR_TIMER_WGM_FCPWM8(),
        //			[2] = AVR_TIMER_WGM_FCPWM9(),
        //			[3] = AVR_TIMER_WGM_FCPWM10(),
        //			[4] = AVR_TIMER_WGM_CTC(),
        //			[5] = AVR_TIMER_WGM_FASTPWM8(),
        //			[6] = AVR_TIMER_WGM_FASTPWM9(),
        //			[7] = AVR_TIMER_WGM_FASTPWM10(),
        //			[8] = AVR_TIMER_WGM_ICPWM(),
        //			[9] = AVR_TIMER_WGM_OCPWM(),
        //			[12] = AVR_TIMER_WGM_ICCTC(),
        //			[14] = AVR_TIMER_WGM_ICPWM(),
        //			[15] = AVR_TIMER_WGM_OCPWM(),
        //		},
        //		.cs = { AVR_IO_REGBIT(TCCR1B, CS10), AVR_IO_REGBIT(TCCR1B, CS11), AVR_IO_REGBIT(TCCR1B, CS12) },
        //		.cs_div = { 0, 0, 3 /* 8 */, 6 /* 64 */, 8 /* 256 */, 10 /* 1024 */, AVR_TIMER_EXTCLK_CHOOSE, AVR_TIMER_EXTCLK_CHOOSE },
        //		.ext_clock_pin = AVR_IO_REGBIT(PORTD, 5), /* External clock pin */

        //		.r_tcnt = TCNT1L,
        //		.r_tcnth = TCNT1H,
        //		.r_icr = ICR1L,
        //		.r_icrh = ICR1H,

        //		.ices = AVR_IO_REGBIT(TCCR1B, ICES1),
        //		.icp = AVR_IO_REGBIT(PORTB, 0),

        //		.overflow = {
        //			.enable = AVR_IO_REGBIT(TIMSK1, TOIE1),
        //			.raised = AVR_IO_REGBIT(TIFR1, TOV1),
        //			.vector = TIMER1_OVF_vect,
        //		},
        //		.icr = {
        //			.enable = AVR_IO_REGBIT(TIMSK1, ICIE1),
        //			.raised = AVR_IO_REGBIT(TIFR1, ICF1),
        //			.vector = TIMER1_CAPT_vect,
        //		},
        //		.comp = {
        //			[AVR_TIMER_COMPA] = {
        //				.r_ocr = OCR1AL,
        //				.r_ocrh = OCR1AH,	// 16 bits timers have two bytes of it
        //				.com = AVR_IO_REGBITS(TCCR1A, COM1A0, 0x3),
        //				.com_pin = AVR_IO_REGBIT(PORTB, 1),
        //				.interrupt = {
        //					.enable = AVR_IO_REGBIT(TIMSK1, OCIE1A),
        //					.raised = AVR_IO_REGBIT(TIFR1, OCF1A),
        //					.vector = TIMER1_COMPA_vect,
        //				},
        //			},
        //			[AVR_TIMER_COMPB] = {
        //				.r_ocr = OCR1BL,
        //				.r_ocrh = OCR1BH,
        //				.com = AVR_IO_REGBITS(TCCR1A, COM1B0, 0x3),
        //				.com_pin = AVR_IO_REGBIT(PORTB, 2),
        //				.interrupt = {
        //					.enable = AVR_IO_REGBIT(TIMSK1, OCIE1B),
        //					.raised = AVR_IO_REGBIT(TIFR1, OCF1B),
        //					.vector = TIMER1_COMPB_vect,
        //				},
        //			},
        //		},
        //	},
        //	.timer2 = {
        //		.name = '2',
        //		.disabled = AVR_IO_REGBIT(PRR, PRTIM2),
        //		.wgm = { AVR_IO_REGBIT(TCCR2A, WGM20), AVR_IO_REGBIT(TCCR2A, WGM21), AVR_IO_REGBIT(TCCR2B, WGM22) },
        //		.wgm_op = {
        //			[0] = AVR_TIMER_WGM_NORMAL8(),
        //			[1] = AVR_TIMER_WGM_FCPWM8(),
        //			[2] = AVR_TIMER_WGM_CTC(),
        //			[3] = AVR_TIMER_WGM_FASTPWM8(),
        //			[7] = AVR_TIMER_WGM_OCPWM(),
        //		},

        //		.cs = { AVR_IO_REGBIT(TCCR2B, CS20), AVR_IO_REGBIT(TCCR2B, CS21), AVR_IO_REGBIT(TCCR2B, CS22) },
        //		.cs_div = { 0, 0, 3 /* 8 */, 5 /* 32 */, 6 /* 64 */, 7 /* 128 */, 8 /* 256 */, 10 /* 1024 */ },

        //		.r_tcnt = TCNT2,

        //		// asynchronous timer source bit.. if set, use 32khz frequency
        //		.as2 = AVR_IO_REGBIT(ASSR, AS2),

        //		.overflow = {
        //			.enable = AVR_IO_REGBIT(TIMSK2, TOIE2),
        //			.raised = AVR_IO_REGBIT(TIFR2, TOV2),
        //			.vector = TIMER2_OVF_vect,
        //		},
        //		.comp = {
        //			[AVR_TIMER_COMPA] = {
        //				.r_ocr = OCR2A,
        //				.com = AVR_IO_REGBITS(TCCR2A, COM2A0, 0x3),
        //				.com_pin = AVR_IO_REGBIT(PORTB, 3),
        //				.interrupt = {
        //					.enable = AVR_IO_REGBIT(TIMSK2, OCIE2A),
        //					.raised = AVR_IO_REGBIT(TIFR2, OCF2A),
        //					.vector = TIMER2_COMPA_vect,
        //				}
        //			},
        //			[AVR_TIMER_COMPB] = {
        //				.r_ocr = OCR2B,
        //				.com = AVR_IO_REGBITS(TCCR2A, COM2B0, 0x3),
        //				.com_pin = AVR_IO_REGBIT(PORTD, 3),
        //				.interrupt = {
        //					.enable = AVR_IO_REGBIT(TIMSK2, OCIE2B),
        //					.raised = AVR_IO_REGBIT(TIFR2, OCF2B),
        //					.vector = TIMER2_COMPB_vect,
        //				}
        //			}
        //		}
        //	},
        //	AVR_SPI_DECLARE(PRR, PRSPI, 'B', 5, 4, 3, 2),
        //	.twi = {
        //		.disabled = AVR_IO_REGBIT(PRR, PRTWI),

        //		.r_twcr = TWCR,
        //		.r_twsr = TWSR,
        //		.r_twbr = TWBR,
        //		.r_twdr = TWDR,
        //		.r_twar = TWAR,
        //		.r_twamr = TWAMR,

        //		.twen = AVR_IO_REGBIT(TWCR, TWEN),
        //		.twea = AVR_IO_REGBIT(TWCR, TWEA),
        //		.twsta = AVR_IO_REGBIT(TWCR, TWSTA),
        //		.twsto = AVR_IO_REGBIT(TWCR, TWSTO),
        //		.twwc = AVR_IO_REGBIT(TWCR, TWWC),

        //		.twsr = AVR_IO_REGBITS(TWSR, TWS3, 0x1f),	// 5 bits
        //		.twps = AVR_IO_REGBITS(TWSR, TWPS0, 0x3),	// 2 bits

        //		.twi = {
        //			.enable = AVR_IO_REGBIT(TWCR, TWIE),
        //			.raised = AVR_IO_REGBIT(TWCR, TWINT),
        //			.raise_sticky = 1,
        //			.vector = TWI_vect,
        //		},
        //	},

        //};



    }
}
