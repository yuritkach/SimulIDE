namespace SimulIDE.src.simavr.sim
{
    public class Sim_time_helper
    {

        //// converts a number of usec to a number of machine cycles, at current speed
        public static uint Avr_usec_to_cycles(Avr avr, uint usec)
        {
        	return avr.frequency * usec / 1000000;
        }

        //// converts back a number of cycles to usecs (for usleep)
        public static ulong Avr_cycles_to_usec(Avr avr, ulong cycles)
        {
        	return 1000000L * cycles / avr.frequency;
        }

        //// converts back a number of cycles to nsecs
        //static inline uint64_t
        //avr_cycles_to_nsec(struct avr_t * avr, avr_cycle_count_t cycles)
        //{
        //	return (uint64_t)1E6 * (uint64_t)cycles / (avr->frequency/1000);
        //}

        //// converts a number of hz (to megahertz etc) to a number of cycle
        //static inline avr_cycle_count_t
        //avr_hz_to_cycles(avr_t * avr, uint32_t hz)
        //{
        //	return avr->frequency / hz;
        //}

        //#ifdef __cplusplus
        //};
        //#endif

        //#endif /* __SIM_TIME_H___ */
    }
}