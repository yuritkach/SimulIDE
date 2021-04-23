using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class Avr_timer
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

        public Avr_timer_comp[] comp = new Avr_timer_comp[AVR_TIMER_COMP_COUNT];

        public Avr_int_vector overflow;  // overflow
        public Avr_int_vector icr;   // input capture

        public ulong tov_cycles;    // number of cycles from zero to overflow
        public float tov_cycles_fract; // fractional part for external clock with non int ratio to F_CPU
        public float phase_accumulator;
        public ulong tov_base;  // MCU cycle when the last overflow occured; when clocked externally holds external clock count
        public ushort tov_top;   // current top value to calculate tnct

    }
}
