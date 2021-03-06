﻿using System;
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
        public UInt64 when;
        public Avr_cycle_timer timer;
        public object param;
        public bool free;
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
        public List<Avr_cycle_timer_slot> timers= new List<Avr_cycle_timer_slot>();
    }


    class Sim_cycle_timers
    {
      
        public const int DEFAULT_SLEEP_CYCLES = 1000;

        public static void Avr_cycle_timer_reset(Avr avr)
        {
            Avr_cycle_timer_pool pool = avr.cycle_timers;
            if (pool == null)
                pool = new Avr_cycle_timer_pool();
            // queue all slots into the free queue
            for(int i = 0; i<MAX_CYCLE_TIMERS; i++ )
            {
            	Avr_cycle_timer_slot t = pool.timer_slots[i];
                t.free = true;
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
            Avr_cycle_timer_pool pool = avr.cycle_timers;
            ulong sleep_cycle_count = DEFAULT_SLEEP_CYCLES;
            foreach (Avr_cycle_timer_slot t in pool.timers)
            {
                if (t != null && !t.free)
                {
                    if (t.when > avr.cycle)
                        sleep_cycle_count = t.when - avr.cycle;
                    else
                        sleep_cycle_count = 0;
                }
            }
            Avr_cycle_timer_return_sleep_run_cycles_limited(avr, sleep_cycle_count);
        }

        //// no sanity checks checking here, on purpose
        public static void Avr_cycle_timer_insert(Avr avr,ulong when,Avr_cycle_timer timer,params object[] param)
        {
            Avr_cycle_timer_pool pool = avr.cycle_timers;

            when += avr.cycle;
            Avr_cycle_timer_slot t = pool.timer_slots.SingleOrDefault(s => s.free);
            if (t==null)
            {
                // AVR_LOG(avr, LOG_ERROR, "CYCLE: %s: ran out of timers (%d)!\n", __func__, MAX_CYCLE_TIMERS);
                // return;
                throw new Exception(string.Format("CYCLE: {0:G}: ran out of timers ({1:G})!\n", "Avr_cycle_timer_insert", MAX_CYCLE_TIMERS));
            }
            t.timer = timer;
            t.param = param;
            t.when = when;
        }

        public static void Avr_cycle_timer_register(Avr avr, ulong when, Avr_cycle_timer timer, params object[] param)
        {
            Avr_cycle_timer_pool pool = avr.cycle_timers;

            // remove it if it was already scheduled
            Avr_cycle_timer_cancel(ref avr, timer, param);
            Avr_cycle_timer_slot t = null;
            foreach (Avr_cycle_timer_slot s in pool.timer_slots)
            {
                if (s != null && s.free)
                {
                    t = s;
                    break;
                }
            }
            
            if (t==null)
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

        public static void  Avr_cycle_timer_cancel(ref Avr avr,Avr_cycle_timer timer,object param)
        {
            Avr_cycle_timer_pool pool = avr.cycle_timers;
            foreach (Avr_cycle_timer_slot t in pool.timers)
            {
                if (t.timer == timer && t.param == param)
                {
                    t.free = true;
                    break;
                }
            }
            Avr_cycle_timer_reset_sleep_run_cycles_limited(avr);
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
            Avr_cycle_timer_pool pool = avr.cycle_timers;

            if (pool != null)
            {
                foreach (var t in pool.timers)
                {
                    if (!t.free)
                    {
                        ulong when = t.when;
                        if (when > avr.cycle)
                            return Avr_cycle_timer_return_sleep_run_cycles_limited(avr, when - avr.cycle);


                        // detach from active timers
                        //    pool.timer = t.next;
                        //    t->next = NULL;
                        do
                        {
                            ulong w = t.timer(avr, when, t.param);
                            // make sure the return value is either zero, or greater
                            // than the last one to prevent infinite loop here
                            when = w > when ? w : 0;
                        }
                        while ((when == 0) && (when <= avr.cycle));
                        if (when != 0) // reschedule then
                            Avr_cycle_timer_insert(avr, when - avr.cycle, t.timer, t.param);

                        // requeue this one into the free ones
                        t.free = true;
                    }
                }
            
            }
            // original behavior was to return 1000 cycles when no timers were present...
            // run_cycles are bound to at least one cycle but no more than requested limit...
            //	value passed here is returned unbounded, thus preserving original behavior.
            return Avr_cycle_timer_return_sleep_run_cycles_limited(avr, DEFAULT_SLEEP_CYCLES);
        }


        public static int MAX_CYCLE_TIMERS = 64;

    }
}
