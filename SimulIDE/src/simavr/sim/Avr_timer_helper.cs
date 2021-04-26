using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
    public class Avr_timer_wgm
    {
        public byte top;
        public byte bottom;
        public byte size;
        public byte kind;
    }

    public class Avr_timer_comp
    {
        public Avr_int_vector interrupt;     // interrupt vector
        public Avr_timer timer;			// parent timer
        public uint r_ocr;            // comparator register low byte
        public uint r_ocrh;           // comparator register hi byte
        public Avr_regbit com;           // comparator output mode registers
        public Avr_regbit com_pin;       // where comparator output is connected
        public ulong comp_cycles;
    }

    public class Avr_timer : Avr_io // пытаемся сделать через базовый класс.. TYV
    {
        public Avr_io io;
        public string name;
        public uint trace;     // debug trace

        public Avr_regbit disabled;  // bit in the PRR

        public ushort r_tcnt, r_icr;
        public ushort r_tcnth, r_icrh;

        public Avr_regbit[] wgm = new Avr_regbit[4];
        public Avr_timer_wgm[] wgm_op = new Avr_timer_wgm[16];
        public Avr_timer_wgm mode;
        public int wgm_op_mode_kind;
        public uint wgm_op_mode_size;

        public Avr_regbit as2;       // asynchronous clock 32khz
        public Avr_regbit[] cs = new Avr_regbit[4];     // specify control register bits choosing clock sourcre
        public byte[] cs_div = new byte[16]; // translate control register value to clock prescaler (orders of 2 exponent)
        public uint cs_div_value;

        public Avr_regbit ext_clock_pin; // external clock input pin, to link IRQs
        public byte ext_clock_flags;    // holds AVR_TIMER_EXTCLK_FLAG_ON, AVR_TIMER_EXTCLK_FLAG_EDGE and other ext. clock mode flags
        public float ext_clock;    // external clock frequency, e.g. 32768Hz

        public Avr_regbit icp;       // input capture pin, to link IRQs
        public Avr_regbit ices;      // input capture edge select

        public Avr_timer_comp[] comp = new Avr_timer_comp[Avr_timer_helper.AVR_TIMER_COMP_COUNT];

        public Avr_int_vector overflow;  // overflow
        public Avr_int_vector icr;   // input capture

        public ulong tov_cycles;    // number of cycles from zero to overflow
        public float tov_cycles_fract; // fractional part for external clock with non int ratio to F_CPU
        public float phase_accumulator;
        public ulong tov_base;  // MCU cycle when the last overflow occured; when clocked externally holds external clock count
        public ushort tov_top;   // current top value to calculate tnct

    }


    public class Avr_timer_helper
    {
        /*
         * The timers are /always/ 16 bits here, if the higher byte register
         * is specified it's just added.
         */
        public static ushort _timer_get_ocr(Avr_timer p, int compi)
        {
            return (ushort)(p.io.avr.data[p.comp[compi].r_ocr] |
                      ((p.comp[compi].r_ocrh!=0)?
                              (p.io.avr.data[p.comp[compi].r_ocrh] << 8) : 0));
        }

        public static ushort _timer_get_comp_ocr(Avr avr, Avr_timer_comp comp)
        {

            uint ocrh = comp.r_ocrh;
            return (ushort)(avr.data[comp.r_ocr] | (ocrh!=0? (avr.data[ocrh] << 8) : 0));
        }

        public static ushort _timer_get_tcnt(Avr_timer p)
        {
            return (ushort)(p.io.avr.data[p.r_tcnt] |
                        (p.r_tcnth!=0 ? (p.io.avr.data[p.r_tcnth] << 8) : 0));
        }

        public static ushort _timer_get_icr(Avr_timer p)
        {
            return (ushort)(p.io.avr.data[p.r_icr] |
                        (p.r_tcnth!=0 ? (p.io.avr.data[p.r_icrh] << 8) : 0));
        }
        public static ulong Avr_timer_comp(Avr_timer p, ulong when,byte comp)
        {
            Avr avr = p.io.avr;
            Sim_interrupts.Avr_raise_interrupt(avr, p.comp[comp].interrupt);

            // check output compare mode and set/clear pins
            byte mode = Sim_regbit.Avr_regbit_get(avr, p.comp[comp].com);
            Avr_irq irq = p.io.irq[TIMER_IRQ_OUT_COMP + comp];

            switch (mode)
            {
                case avr_timer_com_normal: // Normal mode OCnA disconnected
                    break;

                case avr_timer_com_toggle: // Toggle OCnA on compare match
                    if (p.comp[comp].com_pin.reg!=0)  // we got a physical pin
                        Sim_irq.Avr_raise_irq(irq, (uint)(Avr_ioports.AVR_IOPORT_OUTPUT | (Sim_regbit.Avr_regbit_get(avr, p.comp[comp].com_pin)!=0 ? 0 : 1)));
                    else // no pin, toggle the IRQ anyway
                        Sim_irq.Avr_raise_irq(irq, (uint)(p.io.irq[TIMER_IRQ_OUT_COMP + comp].value!=0 ? 0 : 1));
                    break;

                case avr_timer_com_clear:
                    Sim_irq.Avr_raise_irq(irq, 0);
                    break;

                case avr_timer_com_set:
                    Sim_irq.Avr_raise_irq(irq, 1);
                    break;
            }

            return p.tov_cycles!=0 ? 0 : (p.comp[comp].comp_cycles!=0 ? when + p.comp[comp].comp_cycles : 0);
        }

        public static void Avr_timer_comp_on_tov(Avr_timer p, ulong when,byte comp)
        {
            Avr avr = p.io.avr;

            // check output compare mode and set/clear pins
            byte mode = Sim_regbit.Avr_regbit_get(avr, p.comp[comp].com);
            Avr_irq irq = p.io.irq[TIMER_IRQ_OUT_COMP + comp];

            switch (mode)
            {
                case avr_timer_com_normal: // Normal mode
                    break;

                case avr_timer_com_toggle: // toggle on compare match => on tov do nothing
                    break;

                case avr_timer_com_clear: // clear on compare match => set on tov
                    Sim_irq.Avr_raise_irq(irq, 1);
                    break;

                case avr_timer_com_set: // set on compare match => clear on tov
                    Sim_irq.Avr_raise_irq(irq, 0);
                    break;
            }
        }

        public static ulong Avr_timer_compa(Avr avr,ulong when,object param)
        {
        	return Avr_timer_comp((Avr_timer) param, when, AVR_TIMER_COMPA);
        }

        public static ulong Avr_timer_compb(Avr avr,ulong when,object param)
        {
    	    return Avr_timer_comp((Avr_timer) param, when, AVR_TIMER_COMPB);
        }

        public static ulong Avr_timer_compc(Avr avr, ulong when,object param)
        {
    	    return Avr_timer_comp((Avr_timer) param, when, AVR_TIMER_COMPC);
        }

        public static void Avr_timer_irq_ext_clock(Avr_irq irq, uint value,object param)
        {

            Avr_timer p = (Avr_timer)param;
            Avr avr = p.io.avr;

        	if ((p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_VIRT)!=0 || (p.tov_top==0))
    		    return;			// we are clocked internally (actually should never come here)

    	    int bing = 0;
    	    if ((p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_EDGE)!=0)  // clock on rising edge
            {
                if (irq.value==0 && value!=0)
    			    bing++;
    	    }
            else
            {	// clock on falling edge
    		   if (irq.value!=0 && value==0)
    	    	    bing++;
        	}
        	if (bing==0)
    		   return;

    	    //AVR_LOG(avr, LOG_TRACE, "%s Timer%c tick, tcnt=%i\n", __func__, p->name, p->tov_base);

    	    p.ext_clock_flags |= AVR_TIMER_EXTCLK_FLAG_STARTED;

    	    Avr_cycle_timer[] dispatch = new Avr_cycle_timer[AVR_TIMER_COMP_COUNT] 
                    { Avr_timer_compa, Avr_timer_compb, Avr_timer_compc };

            bool overflow = false;
    	    /**
    	    *
    	    * Datasheet excerpt (Compare Match Output Unit):
    	    * "The 16-bit comparator continuously compares TCNT1 with the Output Compare Regis-
        	ter (OCR1x). If TCNT equals OCR1x the comparator signals a match. A match will set
        	the Output Compare Flag (OCF1x) at the next timer clock cycle. If enabled (OCIE1x =
    	    1), the Output Compare Flag generates an output compare interrupt."
        	Thus, comparators should go before incementing the counter to use counter value
    		from the previous cycle.
    	    */
    	    for (int compi = 0; compi<AVR_TIMER_COMP_COUNT; compi++)
            {
    		    if (p.wgm_op_mode_kind != avr_timer_wgm_ctc)
                {
    			    if ((p.mode.top == avr_timer_wgm_reg_ocra) && (compi == 0))
    				    continue; // ocra used to define TOP
    		    }
    		    if (p.comp[compi].comp_cycles!=0 && (p.tov_base == p.comp[compi].comp_cycles))
                {
    				dispatch[compi](avr, avr.cycle, param);
    			    if (p.wgm_op_mode_kind == avr_timer_wgm_ctc)
    			    	p.tov_base = 0;
    		    }
    	    }

    	    switch (p.wgm_op_mode_kind)
            {
                case avr_timer_wgm_fc_pwm:	// in the avr_timer_write_ocr comment "OCR is not used here" - why?
    		    case avr_timer_wgm_pwm:
                    if ((p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_REVDIR) != 0)
                    {
    				    --p.tov_base;
                        if (p.tov_base == 0) // overflow occured
                        {
    					    p.ext_clock_flags = (byte)(p.ext_clock_flags & (~AVR_TIMER_EXTCLK_FLAG_REVDIR)); // restore forward count direction
                            overflow = true;
    				    }
    			    }
                    else
                    {
                        if (++p.tov_base >= p.tov_top)
                        {
    					    p.ext_clock_flags |= AVR_TIMER_EXTCLK_FLAG_REVDIR; // prepare to count down
    				    }
    			    }
    			    break;
    		    case avr_timer_wgm_fast_pwm:
    			    if (++p.tov_base == p.tov_top)
                    {
    				    overflow = true;
    				    if (p.mode.top == avr_timer_wgm_reg_icr)
    					    Sim_interrupts.Avr_raise_interrupt(avr, p.icr);
    				    else 
                        if (p.mode.top == avr_timer_wgm_reg_ocra)
                            Sim_interrupts.Avr_raise_interrupt(avr, p.comp[0].interrupt);
    			    }
    			    else 
                    if (p.tov_base > p.tov_top)
                    {
    				    p.tov_base = 0;
    			    }
    			    break;
    		    case avr_timer_wgm_ctc:
    			    {
    				    int max = (1 << p.wgm_op[0].size) - 1;
    				    if ((++p.tov_base) > (ulong)max)
                        {
    					    // overflow occured
    					    p.tov_base = 0;
    					    overflow = true;
    				    }
    			    }
    			    break;
    		    default:
    			    if (++p.tov_base > p.tov_top)
                    {
    				    // overflow occured
    				    p.tov_base = 0;
    				    overflow = true;
    			    }
    			    break;
    	        }

    	    if (overflow)
            {
    		    for (int compi = 0; compi<AVR_TIMER_COMP_COUNT; compi++)
                {
    			    if (p.comp[compi].comp_cycles!=0)
                    {
    				    if (p.mode.top == avr_timer_wgm_reg_ocra && compi == 0)
    					    continue;
    				    Avr_timer_comp_on_tov(p, 0, (byte)compi);
    			    }
    		    }
    		    Sim_interrupts.Avr_raise_interrupt(avr, p.overflow);
    	    }

        }

        // timer overflow
        public static ulong Avr_timer_tov(Avr avr, ulong when,object param)
        {

            Avr_timer p = (Avr_timer)param;
            bool start = p.tov_base == 0;

            ulong next = when;
    	    if (((p.ext_clock_flags & (AVR_TIMER_EXTCLK_FLAG_AS2 | AVR_TIMER_EXTCLK_FLAG_TN)) != 0)
    			&& (p.tov_cycles_fract != 0.0f))
            {
    		    p.phase_accumulator += p.tov_cycles_fract;
    		    if (p.phase_accumulator >= 1.0f)
                {
    			    ++next;
    			    p.phase_accumulator -= 1.0f;
    		    }
                else 
                if (p.phase_accumulator <= -1.0f)
                {
    			    --next;
    			    p.phase_accumulator += 1.0f;
    		    }
    	    }

    	    if (!start)
    		    Sim_interrupts.Avr_raise_interrupt(avr, p.overflow);
            p.tov_base = when;

    	    Avr_cycle_timer[] dispatch = new Avr_cycle_timer[AVR_TIMER_COMP_COUNT] 
                { Avr_timer_compa, Avr_timer_compb, Avr_timer_compc };

    	    for (int compi = 0; compi<AVR_TIMER_COMP_COUNT; compi++)
            {
    		    if (p.comp[compi].comp_cycles!=0)
                {
    			    if (p.comp[compi].comp_cycles<p.tov_cycles && p.comp[compi].comp_cycles >= (avr.cycle - when))
                    {
    				    Avr_timer_comp_on_tov(p, when, (byte)compi);
                        Sim_cycle_timers.Avr_cycle_timer_register(avr,p.comp[compi].comp_cycles - (avr.cycle - next),dispatch[compi], p);
    			    }
                    else 
                    if (p.tov_cycles == p.comp[compi].comp_cycles && !start)
    				    dispatch[compi] (avr, when, param);
    		    }
    	    }

    	    return next + p.tov_cycles;
        }

        public static ushort _avr_timer_get_current_tcnt(Avr_timer p)
        {
            Avr avr = p.io.avr;
            if ((p.ext_clock_flags & (AVR_TIMER_EXTCLK_FLAG_TN | AVR_TIMER_EXTCLK_FLAG_AS2))==0 ||
                    (p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_VIRT)!=0)
            {
                if (p.tov_cycles!=0)
                {
                    ulong when = avr.cycle - p.tov_base;

                    return (ushort)((when * (((uint)p.tov_top) + 1)) / p.tov_cycles);
                }
            }
            else
            {
                if (p.tov_top!=0)
                    return (ushort)p.tov_base;
            }
            return 0;
        }

        public static byte  Avr_timer_tcnt_read(Avr avr,uint addr,object param)
        {

            Avr_timer p = (Avr_timer)param;
            // made to trigger potential watchpoints

            ushort tcnt = _avr_timer_get_current_tcnt(p);

            avr.data[p.r_tcnt] = (byte)(tcnt & 0xFF);  // ПРОВЕРИТЬ!!!!!!!!!!!!!!!!!!!!!
            avr.data[p.r_tcnt+1] = (byte)((tcnt>>8)&0xFF);

            if (p.r_tcnth!=0)
        		avr.data[p.r_tcnth] = (byte)(tcnt >> 8);

        	return Sim_core_helper.Avr_core_watch_read(avr, addr);
        }

        public static void Avr_timer_cancel_all_cycle_timers(Avr avr, Avr_timer timer, byte clear_timers)
        {
    	    if(clear_timers!=0)
            {
    		    for (int compi = 0; compi<AVR_TIMER_COMP_COUNT; compi++)
    		    	timer.comp[compi].comp_cycles = 0;
    		    timer.tov_cycles = 0;
    	    }

            Sim_cycle_timers.Avr_cycle_timer_cancel(ref avr, Avr_timer_tov, timer);
            Sim_cycle_timers.Avr_cycle_timer_cancel(ref avr, Avr_timer_compa, timer);
            Sim_cycle_timers.Avr_cycle_timer_cancel(ref avr, Avr_timer_compb, timer);
            Sim_cycle_timers.Avr_cycle_timer_cancel(ref avr, Avr_timer_compc, timer);
        }

        public static void Avr_timer_tcnt_write(Avr avr,uint addr,byte v,object param)
        {

            Avr_timer p = (Avr_timer)param;
            Sim_core_helper.Avr_core_watch_write(avr, addr, v);
            ushort tcnt = _timer_get_tcnt(p);

        	if (p.tov_top==0)
        		return;

        	if (tcnt >= p.tov_top)
        		tcnt = 0;

        	if ((p.ext_clock_flags & (AVR_TIMER_EXTCLK_FLAG_TN | AVR_TIMER_EXTCLK_FLAG_AS2))==0 ||
        			(p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_VIRT)!=0)
            {
        		// internal or virtual clock

        		// this involves some magicking
        		// cancel the current timers, recalculate the "base" we should be at, reset the
        		// timer base as it should, and re-schedule the timers using that base.

        		Avr_timer_cancel_all_cycle_timers(avr, p, 0);

                ulong cycles = (tcnt * p.tov_cycles) / p.tov_top;

        		////////	printf("%s-%c %d/%d -- cycles %d/%d\n", __FUNCTION__, p->name, tcnt, p->tov_top, (uint32_t)cycles, (uint32_t)p->tov_cycles);

        		// this reset the timers bases to the new base
        		if (p.tov_cycles > 1)
                {
        			Sim_cycle_timers.Avr_cycle_timer_register(avr, p.tov_cycles - cycles, Avr_timer_tov, p);
                    p.tov_base = 0;
        			Avr_timer_tov(avr, avr.cycle - cycles, p);
                }
                //////	tcnt = ((avr->cycle - p->tov_base) * p->tov_top) / p->tov_cycles;
                //////	printf("%s-%c new tnt derive to %d\n", __FUNCTION__, p->name, tcnt);
            }
        	else
            {
        		// clocked externally
        		p.tov_base = tcnt;
        	}
        }

        public static void Avr_timer_configure(Avr_timer p,uint prescaler,uint top,byte reset)
        {
            p.tov_top = (ushort)top;

            Avr avr = p.io.avr;
            float resulting_clock = 0.0f; // used only for trace
            float tov_cycles_exact;

            byte as2 = (byte)(p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_AS2);
            bool use_ext_clock = as2!=0 || (p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_TN)!=0;
            bool virt_ext_clock = use_ext_clock && ((p.ext_clock_flags & AVR_TIMER_EXTCLK_FLAG_VIRT)!=0);

            if (!use_ext_clock)
            {
                if (prescaler != 0)
                    resulting_clock = (float)avr.frequency / prescaler;
                p.tov_cycles = prescaler * (top + 1);
                p.tov_cycles_fract = 0.0f;
                tov_cycles_exact = p.tov_cycles;
            }
            else
            {
                if (!virt_ext_clock)
                {
                    p.tov_cycles = 0;
                    p.tov_cycles_fract = 0.0f;
                }
                else
                {
                    if (prescaler != 0)
                        resulting_clock = p.ext_clock / prescaler;
                    tov_cycles_exact = (float)avr.frequency / p.ext_clock * prescaler * (top + 1);
                    p.tov_cycles = (ulong)Math.Round(tov_cycles_exact);
                    p.tov_cycles_fract = tov_cycles_exact - p.tov_cycles;
                }
            }

            if (p.trace!=0)
            {
                if (!use_ext_clock || virt_ext_clock)
                {
                    // clocked internally
            //        AVR_LOG(avr, LOG_TRACE, "TIMER: %s-%c TOP %.2fHz = %d cycles = %dusec\n", // TOP there means Timer Overflows Persec ?
             //               __FUNCTION__, p->name, ((float)avr->frequency / tov_cycles_exact),
              //              (int)p->tov_cycles, (int)avr_cycles_to_usec(avr, p->tov_cycles));
                }
                else
                {
                    // clocked externally from the Tn pin
               //     AVR_LOG(avr, LOG_TRACE, "TIMER: %s-%c use ext clock, TOP=%d\n",
                 //           __FUNCTION__, p->name, (int)p->tov_top
                   //         );
                }
            }

            for (int compi = 0; compi < AVR_TIMER_COMP_COUNT; compi++)
            {
                if (p.comp[compi].r_ocr==0)
                    continue;
                uint ocr = _timer_get_ocr(p, compi);
                //uint32_t comp_cycles = clock * (ocr + 1);
                uint comp_cycles;
                if (virt_ext_clock)
                    comp_cycles = (uint)((float)avr.frequency / p.ext_clock * prescaler * (ocr + 1));
                else
                    comp_cycles = prescaler * (ocr + 1);

                p.comp[compi].comp_cycles = 0;

                if ((p.trace & (avr_timer_trace_compa << compi))!=0)
                {
                    if (!use_ext_clock || virt_ext_clock)
                    {
                      //  printf("%s-%c clock %f top %d OCR%c %d\n", __FUNCTION__, p->name,
                      //      resulting_clock, top, 'A' + compi, ocr);
                    }
                    else
                    {
                    //    AVR_LOG(avr, LOG_TRACE, "%s timer%c clock via ext pin, TOP=%d OCR%c=%d\n",
                    //            __FUNCTION__, p->name, top, 'A' + compi, ocr);
                    }
                }
                if (ocr <= top)
                {
                    p.comp[compi].comp_cycles = comp_cycles;

//                    if (p.trace & (avr_timer_trace_compa << compi))
//                        printf(
//                            "TIMER: %s-%c %c %.2fHz = %d cycles\n",
//                            __FUNCTION__, p->name,
//                            'A' + compi, resulting_clock / (ocr + 1),
//                            (int)comp_cycles);
                }
            }

            if (!use_ext_clock || virt_ext_clock)
            {
                if (p.tov_cycles > 1)
                {
                    if (reset!=0)
                    {
                        Sim_cycle_timers.Avr_cycle_timer_register(avr, p.tov_cycles, Avr_timer_tov, p);
                        // calling it once, with when == 0 tells it to arm the A/B/C timers if needed
                        p.tov_base = 0;
                        Avr_timer_tov(avr, avr.cycle, p);
                        p.phase_accumulator = 0.0f;
                    }
                    else
                    {
                        ulong orig_tov_base = p.tov_base;
                        Sim_cycle_timers.Avr_cycle_timer_register(avr, p.tov_cycles - (avr.cycle - orig_tov_base), Avr_timer_tov, p);
                        // calling it once, with when == 0 tells it to arm the A/B/C timers if needed
                        p.tov_base = 0;
                        Avr_timer_tov(avr, orig_tov_base, p);
                    }
                }
            }
            else
            {
                if (reset!=0)
                    p.tov_base = 0;
            }

            if (reset!=0)
            {
                Avr_ioport_getirq req = new Avr_ioport_getirq();
                req.bit = p.ext_clock_pin;

                if (Sim_io.Avr_ioctl(p.io.avr, Avr_ioports.AVR_IOCTL_IOPORT_GETIRQ_REGBIT, req) > 0)
                {
                    // got an IRQ for the Tn input clock pin
                    if (use_ext_clock && !virt_ext_clock)
                    {
 //                       if (p.trace!=0)
   //                         AVR_LOG(p->io.avr, LOG_TRACE, "%s: timer%c connecting T%c pin IRQ %d\n", __FUNCTION__, p->name, p->name, req.irq[0]->irq);
                        Sim_irq.Avr_irq_register_notify(req.irq[0], Avr_timer_irq_ext_clock, p);
                    }
                    else
                    {
                        //                        if (p->trace)
                        //                            AVR_LOG(p->io.avr, LOG_TRACE, "%s: timer%c disconnecting T%c pin IRQ %d\n", __FUNCTION__, p->name, p->name, req.irq[0]->irq);
                        Sim_irq.Avr_irq_unregister_notify(req.irq[0], Avr_timer_irq_ext_clock, p);
                    }
                }
            }

        }

        public static void Avr_timer_reconfigure(Avr_timer p, byte reset)
        {
            Avr avr = p.io.avr;

            // cancel everything
            Avr_timer_cancel_all_cycle_timers(avr, p, 1);

            switch (p.wgm_op_mode_kind)
            {
                case avr_timer_wgm_normal:
                    Avr_timer_configure(p, p.cs_div_value, p.wgm_op_mode_size, reset);
                    break;
                case avr_timer_wgm_fc_pwm:
                    Avr_timer_configure(p, p.cs_div_value, p.wgm_op_mode_size, reset);
                    break;
                case avr_timer_wgm_ctc:
                    {
                        Avr_timer_configure(p, p.cs_div_value, _timer_get_ocr(p, AVR_TIMER_COMPA), reset);
                    }
                    break;
                case avr_timer_wgm_pwm:
                    {
                        ushort top = (p.mode.top == avr_timer_wgm_reg_ocra) ?
                            _timer_get_ocr(p, AVR_TIMER_COMPA) : _timer_get_icr(p);
                        Avr_timer_configure(p, p.cs_div_value, top, reset);
                    }
                    break;
                case avr_timer_wgm_fast_pwm:
                    Avr_timer_configure(p, p.cs_div_value, p.wgm_op_mode_size, reset);
                    break;
                case avr_timer_wgm_none:
                    Avr_timer_configure(p, p.cs_div_value, p.wgm_op_mode_size, reset);
                    break;
                default:
                    {
                        byte mode = Sim_regbit.Avr_regbit_get_array(avr, p.wgm, p.wgm.Length);
                        //AVR_LOG(avr, LOG_WARNING, "TIMER: %s-%c unsupported timer mode wgm=%d (%d)\n",
                        //        __FUNCTION__, p->name, mode, p->mode.kind);
                    }; break;
            }
        }

        public static void Avr_timer_write_ocr(Avr avr,uint addr,byte v,object param)
        {

            Avr_timer_comp comp = (Avr_timer_comp)param;
            Avr_timer timer = comp.timer;
            uint oldv;

            /* check to see if the OCR values actually changed */
            oldv = _timer_get_comp_ocr(avr, comp);
            Sim_core_helper.Avr_core_watch_write(avr, addr, v);

        	switch (timer.wgm_op_mode_kind)
            {
        		case avr_timer_wgm_normal:
        			Avr_timer_reconfigure(timer, 0);
        			break;
        		case avr_timer_wgm_fc_pwm:	// OCR is not used here
        			Avr_timer_reconfigure(timer, 0);
        			break;
        		case avr_timer_wgm_ctc:
        			Avr_timer_reconfigure(timer, 0);
        			break;
        		case avr_timer_wgm_pwm:
        			if (timer.mode.top != avr_timer_wgm_reg_ocra)  // ICR is the top, update comp_cycles
                    { 
                        uint ocr = _timer_get_comp_ocr(avr, comp);
                        uint prescaler = timer.cs_div_value;
                        comp.comp_cycles = prescaler* ocr;
                        Sim_irq.Avr_raise_irq(timer.io.irq[TIMER_IRQ_OUT_PWM0], _timer_get_ocr(timer, AVR_TIMER_COMPA));
                    }
                    else
                    {
        				Avr_timer_reconfigure(timer, 0); // if OCRA is the top, reconfigure needed
                    }
                    Sim_irq.Avr_raise_irq(timer.io.irq[TIMER_IRQ_OUT_PWM1], _timer_get_ocr(timer, AVR_TIMER_COMPB));
        			break;
        		case avr_timer_wgm_fast_pwm:
        			if (oldv != _timer_get_comp_ocr(avr, comp))
        				Avr_timer_reconfigure(timer, 0);
                    Sim_irq.Avr_raise_irq(timer.io.irq[TIMER_IRQ_OUT_PWM0],
                    _timer_get_ocr(timer, AVR_TIMER_COMPA));
                    Sim_irq.Avr_raise_irq(timer.io.irq[TIMER_IRQ_OUT_PWM1],
                    _timer_get_ocr(timer, AVR_TIMER_COMPB));
        			break;
        		default:
        			//AVR_LOG(avr, LOG_WARNING, "TIMER: %s-%c mode %d UNSUPPORTED\n",__FUNCTION__, timer->name, timer->mode.kind);
                    Avr_timer_reconfigure(timer, 0);
        			break;
        	}
        }

        public static void Avr_timer_write(Avr avr,uint addr,byte v,object param)
        {

            Avr_timer p = (Avr_timer)param;

            byte as2 = Sim_regbit.Avr_regbit_get(avr, p.as2);
            byte cs = Sim_regbit.Avr_regbit_get_array(avr, p.cs, p.cs.Length);
            byte mode = Sim_regbit.Avr_regbit_get_array(avr, p.wgm, p.wgm.Length);

            Sim_core_helper.Avr_core_watch_write(avr, addr, v);

            byte new_as2 = Sim_regbit.Avr_regbit_get(avr, p.as2);
            byte new_cs = Sim_regbit.Avr_regbit_get_array(avr, p.cs, p.cs.Length);
            byte new_mode = Sim_regbit.Avr_regbit_get_array(avr, p.wgm, p.wgm.Length);

        	// only reconfigure the timer if "relevant" bits have changed
        	// this prevent the timer reset when changing the edge detector
        	// or other minor bits
        	if (new_cs != cs || new_mode != mode || new_as2 != as2) {
        	/* cs */
        		if (new_cs == 0) {
        			p.cs_div_value = 0;		// reset prescaler
        			// cancel everything
        			Avr_timer_cancel_all_cycle_timers(avr, p, 1);

        			//AVR_LOG(avr, LOG_TRACE, "TIMER: %s-%c clock turned off\n",
        			//		__func__, p->name);
        			return;
        		}

                p.ext_clock_flags = (byte)(p.ext_clock_flags & ~(AVR_TIMER_EXTCLK_FLAG_TN | AVR_TIMER_EXTCLK_FLAG_EDGE
        								| AVR_TIMER_EXTCLK_FLAG_AS2 | AVR_TIMER_EXTCLK_FLAG_STARTED));
        		if (p.ext_clock_pin.reg!=0
        				&& (p.cs_div[new_cs] == AVR_TIMER_EXTCLK_CHOOSE))
                {
        			// Special case: external clock source chosen, prescale divider irrelevant.
        			p.cs_div_value = 1;
        			p.ext_clock_flags |= (byte)(AVR_TIMER_EXTCLK_FLAG_TN | (new_cs & AVR_TIMER_EXTCLK_FLAG_EDGE));
        		}
                else
                {
        			p.cs_div_value = (byte)(1 << p.cs_div[new_cs]);
        			if (new_as2!=0)
                    {
        				//p->cs_div_value = (uint32_t)((uint64_t)avr->frequency * (1 << p->cs_div[new_cs]) / 32768);
        				p.ext_clock_flags |= (byte)(AVR_TIMER_EXTCLK_FLAG_AS2 | AVR_TIMER_EXTCLK_FLAG_EDGE);
        			}
        		}

        	/* mode */
        		p.mode = p.wgm_op[new_mode];
        		p.wgm_op_mode_kind = p.mode.kind;
        		p.wgm_op_mode_size = (uint)(1 << p.mode.size) - 1;

        		Avr_timer_reconfigure(p, 1);
        	}
        }

        /*
         * write to the TIFR register. Watch for code that writes "1" to clear
         * pending interrupts.
         */
        public static void Avr_timer_write_pending(Avr avr, uint addr,byte v,object param)
        {

            Avr_timer p = (Avr_timer)param;
            // save old bits values
            byte ov = Sim_regbit.Avr_regbit_get(avr, p.overflow.raised);
            byte ic = Sim_regbit.Avr_regbit_get(avr, p.icr.raised);
            byte[] cp = new byte[AVR_TIMER_COMP_COUNT];

        	for (int compi = 0; compi<AVR_TIMER_COMP_COUNT; compi++)
        		cp[compi] = Sim_regbit.Avr_regbit_get(avr, p.comp[compi].interrupt.raised);

            // write the value
            // avr_core_watch_write(avr, addr, v); // This raises flags instead of clearing it.

            // clear any interrupts & flags
            Sim_interrupts.Avr_clear_interrupt_if(avr, p.overflow, ov);
            Sim_interrupts.Avr_clear_interrupt_if(avr, p.icr, ic);

        	for (int compi = 0; compi<AVR_TIMER_COMP_COUNT; compi++)
                Sim_interrupts.Avr_clear_interrupt_if(avr, p.comp[compi].interrupt, cp[compi]);
        }

        public static void Avr_timer_irq_icp(Avr_irq irq,uint value,object param)
        {

            Avr_timer p = (Avr_timer)param;
            Avr avr = p.io.avr;

    	    // input capture disabled when ICR is used as top
    	    if (p.mode.top == avr_timer_wgm_reg_icr)
    		    return;
    	    int bing = 0;
    	    if (Sim_regbit.Avr_regbit_get(avr, p.ices)!=0)  // rising edge
            { 
                if (irq.value==0 && value!=0)
    			    bing++;
    	    }
            else
            {	// default, falling edge
    		    if (irq.value!=0 && value==0)
    			    bing++;
    	    }
    	    if (bing==0)
    		    return;
    	    // get current TCNT, copy it to ICR, and raise interrupt
    	    ushort tcnt = _avr_timer_get_current_tcnt(p);
            avr.data[p.r_icr] =(byte) tcnt; //ПРОВЕРИТЬ ОДИН ИЛИ ДВА БАЙТА ПИСАТЬ
    	    if (p.r_icrh!=0)
    		    avr.data[p.r_icrh] = (byte)(tcnt >> 8);
    	    Sim_interrupts.Avr_raise_interrupt(avr, p.icr);
        }

    public static int Avr_timer_ioctl(Avr_io port, uint ctl, object io_param)
        {
            Avr_timer p = (Avr_timer)port;
            int res = -1;

            if (ctl == AVR_IOCTL_TIMER_SET_TRACE((byte)p.name[1]))
            {
                /* Allow setting individual trace flags */
                p.trace = (uint)io_param;
                res = 0;
            }
            else if (ctl == AVR_IOCTL_TIMER_SET_FREQCLK((byte)p.name[1]))
            {
                float new_freq = (float)io_param;
                if (new_freq >= 0.0f)
                {
                    if (p.as2.reg!=0)
                    {
                        if (new_freq <= (port.avr.frequency / 4))
                        {
                            p.ext_clock = new_freq;
                            res = 0;
                        }
                    }
                    else 
                    if (p.ext_clock_pin.reg!=0)
                    {
                        if (new_freq <= (port.avr.frequency / 2))
                        {
                            p.ext_clock = new_freq;
                            res = 0;
                        }
                    }
                }
            }
            else 
            if (ctl == AVR_IOCTL_TIMER_SET_VIRTCLK((byte)p.name[1]))
            {
                byte new_val = (byte)io_param;
                if (new_val==0)
                {
                    Avr_ioport_getirq req_timer_clock_pin = new Avr_ioport_getirq();
                    req_timer_clock_pin.bit = p.ext_clock_pin;
                    
                    if (Sim_io.Avr_ioctl(p.io.avr, Avr_ioports.AVR_IOCTL_IOPORT_GETIRQ_REGBIT, req_timer_clock_pin) > 0)
                    {
                        p.ext_clock_flags = (byte)(p.ext_clock_flags & (~((byte)AVR_TIMER_EXTCLK_FLAG_VIRT)));
                        res = 0;
                    }
                }
                else
                {
                    p.ext_clock_flags |= AVR_TIMER_EXTCLK_FLAG_VIRT;
                    res = 0;
                }
            }
            if (res >= 0)
                Avr_timer_reconfigure(p, 0); // virtual clock: attempt to follow frequency change preserving the phase
            return res;
        }

        public static void Avr_timer_reset(Avr_io port)
        {
            Avr_timer p = (Avr_timer)port;
            Avr_timer_cancel_all_cycle_timers(p.io.avr, p, 0);

            // check to see if the comparators have a pin output. If they do,
            // (try) to get the ioport corresponding IRQ and connect them
            // they will automagically be triggered when the comparator raises
            // it's own IRQ
            Avr_ioport_getirq req = new Avr_ioport_getirq();
            for (int compi = 0; compi < AVR_TIMER_COMP_COUNT; compi++)
            {
                p.comp[compi].comp_cycles = 0;
                req.bit = p.comp[compi].com_pin;
                if (Sim_io.Avr_ioctl(port.avr, Avr_ioports.AVR_IOCTL_IOPORT_GETIRQ_REGBIT, req) > 0)
                {
                    // cool, got an IRQ
                    //printf("%s-%c COMP%c Connecting PIN IRQ %d\n",
                    //	__func__, p->name, 'A'+compi, req.irq[0]->irq);
                    Sim_irq.Avr_connect_irq(port.irq[TIMER_IRQ_OUT_COMP + compi], req.irq[0]);
                }
            }

            Sim_irq.Avr_irq_register_notify(port.irq[TIMER_IRQ_IN_ICP], Avr_timer_irq_icp, p);
            req = new Avr_ioport_getirq();
            req.bit = p.icp;
            if (Sim_io.Avr_ioctl(port.avr, Avr_ioports.AVR_IOCTL_IOPORT_GETIRQ_REGBIT, req) > 0)
            {
                // cool, got an IRQ for the input capture pin
                //printf("%s-%c ICP Connecting PIN IRQ %d\n", __func__, p->name, req.irq[0]->irq);
                Sim_irq.Avr_connect_irq(req.irq[0], port.irq[TIMER_IRQ_IN_ICP]);
            }
            p.ext_clock_flags = (byte)(p.ext_clock_flags & (~((byte)(AVR_TIMER_EXTCLK_FLAG_STARTED | AVR_TIMER_EXTCLK_FLAG_TN |
                                    AVR_TIMER_EXTCLK_FLAG_AS2 | AVR_TIMER_EXTCLK_FLAG_REVDIR))));
        }


        public static string[] irq_names;
        protected static Avr_io _io;
        static Avr_timer_helper()
        {
            irq_names = new string[TIMER_IRQ_COUNT];
            irq_names[TIMER_IRQ_OUT_PWM0] = "8>pwm0";
            irq_names[TIMER_IRQ_OUT_PWM1] = "8>pwm1";
            irq_names[TIMER_IRQ_IN_ICP] = "<icp";
            irq_names[TIMER_IRQ_OUT_COMP + 0] = ">compa";
            irq_names[TIMER_IRQ_OUT_COMP + 1] = ">compb";
            irq_names[TIMER_IRQ_OUT_COMP + 2] = ">compc";

            _io = new Avr_io();
            _io.kind = "timer";
            _io.irq_names = irq_names;
            _io.reset = Avr_timer_reset;
            _io.ioctl = Avr_timer_ioctl;
        }

        public static void Avr_timer_init(Avr avr,ref Avr_timer p)
        {
            p.io = _io;

            Sim_io.Avr_register_io(avr, ref p.io);
            Sim_interrupts.Avr_register_vector(avr,ref p.overflow);
            Sim_interrupts.Avr_register_vector(avr, ref p.icr);

            // allocate this module's IRQ
            Sim_io.Avr_io_setirqs(ref p.io, AVR_IOCTL_TIMER_GETIRQ((byte)p.name[1]), TIMER_IRQ_COUNT, null);

            // marking IRQs as "filtered" means they don't propagate if the
            // new value raised is the same as the last one.. in the case of the
            // pwm value it makes sense not to bother.
            p.io.irq[TIMER_IRQ_OUT_PWM0].flags |= Sim_irq.IRQ_FLAG_FILTERED;
            p.io.irq[TIMER_IRQ_OUT_PWM1].flags |= Sim_irq.IRQ_FLAG_FILTERED;

            if (p.wgm[0].reg!=0) // these are not present on older AVRs
                Sim_io.Avr_register_io_write(avr, p.wgm[0].reg, Avr_timer_write, p);
            if (p.wgm[1].reg!=0 &&
                    (p.wgm[1].reg != p.wgm[0].reg))
                Sim_io.Avr_register_io_write(avr, p.wgm[1].reg, Avr_timer_write, p);
            if (p.wgm[2].reg!=0 &&
                    (p.wgm[2].reg != p.wgm[0].reg) &&
                    (p.wgm[2].reg != p.wgm[1].reg))
                Sim_io.Avr_register_io_write(avr, p.wgm[2].reg, Avr_timer_write, p);
            if (p.wgm[3].reg!=0 &&
                    (p.wgm[3].reg != p.wgm[0].reg) &&
                    (p.wgm[3].reg != p.wgm[1].reg) &&
                    (p.wgm[3].reg != p.wgm[2].reg))
                Sim_io.Avr_register_io_write(avr, p.wgm[3].reg, Avr_timer_write, p);

            Sim_io.Avr_register_io_write(avr, p.cs[0].reg, Avr_timer_write, p);
            if (p.cs[1].reg!=0 &&
                    (p.cs[1].reg != p.cs[0].reg))
                Sim_io.Avr_register_io_write(avr, p.cs[1].reg, Avr_timer_write, p);
            if (p.cs[2].reg!=0 &&
                    (p.cs[2].reg != p.cs[0].reg) && (p.cs[2].reg != p.cs[1].reg))
                Sim_io.Avr_register_io_write(avr, p.cs[2].reg, Avr_timer_write, p);
            if (p.cs[3].reg!=0 &&
                    (p.cs[3].reg != p.cs[0].reg) &&
                    (p.cs[3].reg != p.cs[1].reg) &&
                    (p.cs[3].reg != p.cs[2].reg))
                Sim_io.Avr_register_io_write(avr, p.cs[3].reg, Avr_timer_write, p);

            if (p.as2.reg != 0) // as2 signifies timer/counter 2... therefore must check for register.
                Sim_io.Avr_register_io_write(avr, p.as2.reg, Avr_timer_write, p);

            // this assumes all the "pending" interrupt bits are in the same
            // register. Might not be true on all devices ?
            Sim_io.Avr_register_io_write(avr, p.overflow.raised.reg, Avr_timer_write_pending, p);

            /*
             * Even if the timer is 16 bits, we don't care to have watches on the
             * high bytes because the datasheet says that the low address is always
             * the trigger.
             */
            for (int compi = 0; compi < AVR_TIMER_COMP_COUNT; compi++)
            {
                p.comp[compi].timer = p;

                Sim_interrupts.Avr_register_vector(avr, ref p.comp[compi].interrupt);

                if (p.comp[compi].r_ocr!=0) // not all timers have all comparators
                    Sim_io.Avr_register_io_write(avr, p.comp[compi].r_ocr, Avr_timer_write_ocr, p.comp[compi]);
            }
            Sim_io.Avr_register_io_write(avr, p.r_tcnt, Avr_timer_tcnt_write, p);
            Sim_io.Avr_register_io_read(avr, p.r_tcnt, Avr_timer_tcnt_read, p);

            if (p.as2.reg!=0)
            {
                p.ext_clock_flags = AVR_TIMER_EXTCLK_FLAG_VIRT;
                p.ext_clock = 32768.0f;
            }
            else
            {
                p.ext_clock_flags = 0;
                p.ext_clock = 0.0f;
            }
        }


        // Get the internal IRQ corresponding to the INT
        public static uint AVR_IOCTL_TIMER_GETIRQ(byte _name)
        {
            return Sim_io.AVR_IOCTL_DEF((byte)'t',(byte) 'm',(byte) 'r', (_name));
        }

        // add timer number/name (character) to set tracing flags
        public static uint AVR_IOCTL_TIMER_SET_TRACE(byte _number)
        {
            return Sim_io.AVR_IOCTL_DEF((byte)'t',(byte) 'm',(byte) 't', (_number));
        }
        // enforce using virtual clock generator when external clock is chosen by firmware
        public static uint AVR_IOCTL_TIMER_SET_VIRTCLK(byte _number)
        {
            return Sim_io.AVR_IOCTL_DEF((byte)'t',(byte) 'm',(byte) 'v', (_number));
        }
        // set frequency of the virtual clock generator
        public static uint AVR_IOCTL_TIMER_SET_FREQCLK(byte _number)
        {
            return Sim_io.AVR_IOCTL_DEF((byte)'t',(byte) 'm',(byte) 'f', (_number));
        }

        public const int AVR_TIMER_EXTCLK_CHOOSE = 0x80;		// marker value for cs_div specifying ext clock selection
        public const int AVR_TIMER_EXTCLK_FLAG_TN = 0x80;		// Tn external clock chosen
        public const int AVR_TIMER_EXTCLK_FLAG_STARTED = 0x40;	// peripheral started
        public const int AVR_TIMER_EXTCLK_FLAG_REVDIR = 0x20;	// reverse counting (decrement)
        public const int AVR_TIMER_EXTCLK_FLAG_AS2 = 0x10;		// asynchronous external clock chosen
        public const int AVR_TIMER_EXTCLK_FLAG_VIRT = 0x08;		// don't use the input pin, generate clock internally
        public const int AVR_TIMER_EXTCLK_FLAG_EDGE = 0x01;		// use the rising edge


        public static Avr_timer_wgm AVR_TIMER_WGM_NORMAL8()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_normal;
            result.size = 8;
            return result;
        }

        public static Avr_timer_wgm AVR_TIMER_WGM_NORMAL16()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_normal;
            result.size = 16;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_CTC()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_ctc;
            result.top = avr_timer_wgm_reg_ocra;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_ICCTC()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_ctc;
            result.top = avr_timer_wgm_reg_icr;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_FASTPWM8()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_fast_pwm;
            result.size = 8;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_FASTPWM9()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_fast_pwm;
            result.size = 9;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_FASTPWM10()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_fast_pwm;
            result.size = 10;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_FCPWM8()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_fc_pwm;
            result.size = 8;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_FCPWM9()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_fc_pwm;
            result.size = 9;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_FCPWM10()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_fc_pwm;
            result.size = 10;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_OCPWM()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_pwm;
            result.top = avr_timer_wgm_reg_ocra;
            return result;
        }
        public static Avr_timer_wgm AVR_TIMER_WGM_ICPWM()
        {
            Avr_timer_wgm result = new Avr_timer_wgm();
            result.kind = avr_timer_wgm_pwm;
            result.top = avr_timer_wgm_reg_icr;
            return result;
        }

        public const int AVR_TIMER_COMPA = 0;
        public const int AVR_TIMER_COMPB = 1;
        public const int AVR_TIMER_COMPC = 2;
        public const int AVR_TIMER_COMP_COUNT = 3;
        public const int TIMER_IRQ_OUT_PWM0 = 0;
        public const int TIMER_IRQ_OUT_PWM1 = 1;
        public const int TIMER_IRQ_IN_ICP = 2;   // input capture
        public const int TIMER_IRQ_OUT_COMP = 3; // comparator pins output IRQ
        public const int TIMER_IRQ_COUNT = TIMER_IRQ_OUT_COMP + AVR_TIMER_COMP_COUNT;

        // Waveform generation modes
        public const int avr_timer_wgm_none = 0; // invalid mode
        public const int avr_timer_wgm_normal = 1;
        public const int avr_timer_wgm_ctc = 2;
        public const int avr_timer_wgm_pwm = 3;
        public const int avr_timer_wgm_fast_pwm = 4;
        public const int avr_timer_wgm_fc_pwm = 5;

        // Compare output modes
        public const byte avr_timer_com_normal = 0;// Normal mode, OCnx disconnected
        public const byte avr_timer_com_toggle = 1;   // Toggle OCnx on compare match
        public const byte avr_timer_com_clear = 2;    // clear OCnx on compare match
        public const byte avr_timer_com_set = 3;     // set OCnx on compare match

        public const int avr_timer_wgm_reg_constant = 0;
        public const int avr_timer_wgm_reg_ocra = 1;
        public const int avr_timer_wgm_reg_icr = 2;

        public const int avr_timer_trace_ocr = (1 << 0);
        public const int avr_timer_trace_tcnt = (1 << 1);

        public const int avr_timer_trace_compa = (1 << 8);
        public const int avr_timer_trace_compb = (1 << 9);
        public const int avr_timer_trace_compc = (1 << 10);

    }
}
