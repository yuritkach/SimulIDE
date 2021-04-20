using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
    public delegate UInt64 Avr_cycle_timer(Avr avr, UInt64 when, object param);

    /*
    * Each timer instance contains the absolute cycle number they
    * are hoping to run at, a function pointer to call and a parameter
    * 
    * it will NEVER be the exact cycle specified, as each instruction is
    * not divisible and might take 2 or more cycles anyway.
    * 
    * However if there was a LOT of cycle lag, the timer migth be called
    * repeteadly until it 'caches up'.
    */
    public class Avr_cycle_timer_slot
    {
        public Avr_cycle_timer_slot Next;
        public UInt64 when;
        public Avr_cycle_timer timer;
        public object param;
    }
    


    /*
     * Timer pool contains a pool of timer slots available, they all
     * start queued into the 'free' qeueue, are migrated to the
     * 'active' queue when needed and are re-queued to the free one
     * when done
     */
    public class Avr_cycle_timer_pool
    {
        public Avr_cycle_timer_slot[] timer_slots = new Avr_cycle_timer_slot[Sim_cycle_timers.MAX_CYCLE_TIMERS];
        public Avr_cycle_timer_slot timer_free;
        public Avr_cycle_timer_slot timer;
    }


    class Sim_cycle_timers
    {
      
        public const int DEFAULT_SLEEP_CYCLES = 1000;

        public static void Avr_cycle_timer_reset(Avr avr)
        {
            Avr_cycle_timer_pool pool = avr.cycle_timers;
            //memset(pool, 0, sizeof(* pool));
            //	// queue all slots into the free queue
            for(int i = 0; i<MAX_CYCLE_TIMERS; i++ )
            {
            //		avr_cycle_timer_slot_p t = &pool->timer_slots[i];
            //QUEUE(pool->timer_free, t);
            }
            avr.run_cycle_count = 1;
            avr.run_cycle_limit = 1;
        }

        public static ulong Avr_cycle_timer_return_sleep_run_cycles_limited(Avr avr,ulong sleep_cycle_count)
        {
            // run_cycle_count is bound to run_cycle_limit but NOT less than 1 cycle...
            //	this is not an error!..  unless you like deadlock.
            ulong run_cycle_count = ((avr.run_cycle_limit >= sleep_cycle_count) ?
                sleep_cycle_count : avr.run_cycle_limit);
            avr.run_cycle_count = run_cycle_count!=0 ? run_cycle_count : 1;
            // sleep cycles are returned unbounded thus preserving original behavior.
            return (sleep_cycle_count);
        }

        public static void Avr_cycle_timer_reset_sleep_run_cycles_limited(Avr avr)
        {
        //    avr_cycle_timer_pool_t* pool = &avr->cycle_timers;
        //    avr_cycle_count_t sleep_cycle_count = DEFAULT_SLEEP_CYCLES;

        //    if (pool->timer)
        //    {
        //        if (pool->timer->when > avr->cycle) sleep_cycle_count = pool->timer->when - avr->cycle;
        //        else sleep_cycle_count = 0;
        //    }
        //    avr_cycle_timer_return_sleep_run_cycles_limited(avr, sleep_cycle_count);
        }

        //// no sanity checks checking here, on purpose
        public static void Avr_cycle_timer_insert(Avr avr,ulong when,Avr_cycle_timer timer,params object[] param)
        {
        //    avr_cycle_timer_pool_t* pool = &avr->cycle_timers;

        //    when += avr->cycle;

        //    avr_cycle_timer_slot_p t = pool->timer_free;

        //    if (!t)
        //    {
        //        AVR_LOG(avr, LOG_ERROR, "CYCLE: %s: ran out of timers (%d)!\n", __func__, MAX_CYCLE_TIMERS);
        //        return;
        //    }
        //    // detach head
        //    pool->timer_free = t->next;
        //    t->next = NULL;
        //    t->timer = timer;
        //    t->param = param;
        //    t->when = when;

        //    // find its place in the list
        //    avr_cycle_timer_slot_p loop = pool->timer, last = NULL;
        //    while (loop)
        //    {
        //        if (loop->when > when) break;

        //        last = loop;
        //        loop = loop->next;
        //    }
        //    INSERT(pool->timer, last, t);
        }

        public static void Avr_cycle_timer_register(Avr avr, ulong when, Avr_cycle_timer timer, params object[] param)
        {
            Avr_cycle_timer_pool pool = avr.cycle_timers;

            // remove it if it was already scheduled
            Avr_cycle_timer_cancel(ref avr, timer, param);

            if (pool.timer_free==null)
            {
                //AVR_LOG(avr, LOG_ERROR, "CYCLE: %s: pool is full (%d)!\n", __func__, MAX_CYCLE_TIMERS);
                return;
            }
            Avr_cycle_timer_insert(avr, when, timer, param);
            Avr_cycle_timer_reset_sleep_run_cycles_limited(avr);
        }

        //void
        //avr_cycle_timer_register_usec(
        //        avr_t* avr,
        //        uint32_t when,
        //        avr_cycle_timer_t timer,
        //        void* param)
        //{
        //    avr_cycle_timer_register(avr, avr_usec_to_cycles(avr, when), timer, param);
        //}

        public static void  Avr_cycle_timer_cancel(ref Avr avr,Avr_cycle_timer timer,object[] param)
        {
        //    avr_cycle_timer_pool_t* pool = &avr->cycle_timers;

            // find its place in the list
        //    avr_cycle_timer_slot_p t = pool->timer, last = NULL;
         //   while (t)
         //   {
         //       if (t->timer == timer && t->param == param)
         //       {
        //            DETACH(pool->timer, last, t);
        //            QUEUE(pool->timer_free, t);
        //            break;
        //        }
        //        last = t;
        //        t = t->next;
        //    }
        //    avr_cycle_timer_reset_sleep_run_cycles_limited(avr);
        }

        ///*
        // * Check to see if a timer is present, if so, return the number (+1) of
        // * cycles left for it to fire, and if not present, return zero
        // */
        //avr_cycle_count_t
        //avr_cycle_timer_status(
        //        avr_t* avr,
        //        avr_cycle_timer_t timer,
        //        void* param)
        //{
        //    avr_cycle_timer_pool_t* pool = &avr->cycle_timers;

        //    // find its place in the list
        //    avr_cycle_timer_slot_p t = pool->timer;
        //    while (t)
        //    {
        //        if (t->timer == timer && t->param == param)
        //        {
        //            return 1 + (t->when - avr->cycle);
        //        }
        //        t = t->next;
        //    }
        //    return 0;
        //}

        ///*
        // * run through all the timers, call the ones that needs it,
        // * clear the ones that wants it, and calculate the next
        // * potential cycle we could sleep for...
        // */
        public static ulong Avr_cycle_timer_process(Avr avr)
        {
        //    avr_cycle_timer_pool_t* pool = &avr->cycle_timers;

        //    if (pool->timer) do
        //        {
        //            avr_cycle_timer_slot_p t = pool->timer;
        //            avr_cycle_count_t when = t->when;

        //            if (when > avr->cycle)
        //                return avr_cycle_timer_return_sleep_run_cycles_limited(avr, when - avr->cycle);

        //            // detach from active timers
        //            pool->timer = t->next;
        //            t->next = NULL;
        //            do
        //            {
        //                avr_cycle_count_t w = t->timer(avr, when, t->param);
        //                // make sure the return value is either zero, or greater
        //                // than the last one to prevent infinite loop here
        //                when = w > when ? w : 0;
        //            } while (when && when <= avr->cycle);

        //            if (when) // reschedule then
        //                avr_cycle_timer_insert(avr, when - avr->cycle, t->timer, t->param);

        //            // requeue this one into the free ones
        //            QUEUE(pool->timer_free, t);
        //        } while (pool->timer);

            // original behavior was to return 1000 cycles when no timers were present...
            // run_cycles are bound to at least one cycle but no more than requested limit...
            //	value passed here is returned unbounded, thus preserving original behavior.
            return Avr_cycle_timer_return_sleep_run_cycles_limited(avr, DEFAULT_SLEEP_CYCLES);
        }


        public static int MAX_CYCLE_TIMERS = 64;


//// register for calling 'timer' in 'when' cycles
//void
//avr_cycle_timer_register(

//        struct avr_t * avr,
//		avr_cycle_count_t when,
//        avr_cycle_timer_t timer,
//		void* param);
//        // register a timer to call in 'when' usec
//        void
//        avr_cycle_timer_register_usec(

//        struct avr_t * avr,
//		uint32_t when,
//        avr_cycle_timer_t timer,
//		void* param);
//        // cancel a previously set timer
//        void
//        avr_cycle_timer_cancel(

//        struct avr_t * avr,
//		avr_cycle_timer_t timer,

//        void* param);
//        /*
//         * Check to see if a timer is present, if so, return the number (+1) of
//         * cycles left for it to fire, and if not present, return zero
//         */
//        avr_cycle_count_t
//        avr_cycle_timer_status(

//        struct avr_t * avr,
//		avr_cycle_timer_t timer,

//        void* param);

//        //
//        // Private, called from the core
//        //
//        avr_cycle_count_t
//        avr_cycle_timer_process(

//        struct avr_t * avr);
//void
//avr_cycle_timer_reset(

//        struct avr_t * avr);

    }
}
