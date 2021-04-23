using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class Avr_timer
    {
        
        //            avr_io_t io;
        //            char name;
        //            uint32_t trace;     // debug trace

        //            avr_regbit_t disabled;  // bit in the PRR

        //            avr_io_addr_t r_tcnt, r_icr;
        //            avr_io_addr_t r_tcnth, r_icrh;

        //            avr_regbit_t wgm[4];
        //            avr_timer_wgm_t wgm_op[16];
        //            avr_timer_wgm_t mode;
        //            int wgm_op_mode_kind;
        //            uint32_t wgm_op_mode_size;

        //            avr_regbit_t as2;       // asynchronous clock 32khz
        //            avr_regbit_t cs[4];     // specify control register bits choosing clock sourcre
        //            uint8_t cs_div[16]; // translate control register value to clock prescaler (orders of 2 exponent)
        //            uint32_t cs_div_value;

        //            avr_regbit_t ext_clock_pin; // external clock input pin, to link IRQs
        //            uint8_t ext_clock_flags;    // holds AVR_TIMER_EXTCLK_FLAG_ON, AVR_TIMER_EXTCLK_FLAG_EDGE and other ext. clock mode flags
        //            float ext_clock;    // external clock frequency, e.g. 32768Hz

        //            avr_regbit_t icp;       // input capture pin, to link IRQs
        //            avr_regbit_t ices;      // input capture edge select

        //            avr_timer_comp_t comp[AVR_TIMER_COMP_COUNT];

        //            avr_int_vector_t overflow;  // overflow
        //            avr_int_vector_t icr;   // input capture

        //            uint64_t tov_cycles;    // number of cycles from zero to overflow
        //            float tov_cycles_fract; // fractional part for external clock with non int ratio to F_CPU
        //            float phase_accumulator;
        //            uint64_t tov_base;  // MCU cycle when the last overflow occured; when clocked externally holds external clock count
        //            uint16_t tov_top;   // current top value to calculate tnct

    }
}
