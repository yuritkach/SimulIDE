using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
  


//        // internal structure for a hook, never seen by the notify procs
        public class Avr_irq_hook
        {
            public Avr_irq_hook next;
        	public int busy;   // prevent reentrance of callbacks

            public Avr_irq chain;	// raise the IRQ on this too - optional if "notify" is on
            public Avr_irq_notify notify;    // called when IRQ is raised - optional if "chain" is on
            object[] param;                // "notify" parameter
        }

    
        //# ifndef __SIM_IRQ_H__
        //#define __SIM_IRQ_H__

        //# include <stdint.h>

        //# ifdef __cplusplus
        //        extern "C" {
        //#endif

        ///*
        // * Internal IRQ system
        // *
        // * This subsystem allows any piece of code to "register" a hook to be called when an IRQ is
        // * raised. The IRQ definition is up to the module defining it, for example a IOPORT pin change
        // * might be an IRQ in which case any piece of code can be notified when a pin has changed state
        // *
        // * The notify hooks are chained, and duplicates are filtered out so you can't register a
        // * notify hook twice on one particular IRQ
        // *
        // * IRQ calling order is not defined, so don't rely on it.
        // *
        // * IRQ hook needs to be registered in reset() handlers, ie after all modules init() bits
        // * have been called, to prevent race condition of the initialization order.
        // */
        //struct avr_irq_t;

        public delegate void Avr_irq_notify(ref Avr_irq irq, UInt32 value, object[] param);
    //        typedef void (* avr_irq_notify_t) (

    //        struct avr_irq_t * irq,
    //		uint32_t value,

    //        void* param);


        

        //        /*
        //         * IRQ Pool structure
        //         */
        public class Avr_irq_pool
        {
            public int count;                      //!< number of irqs living in the pool
            public Avr_irq[] irq;		//!< irqs belonging in this pool
        }


///*!
// * Public IRQ structure
// */
          public class Avr_irq
          {
                public Avr_irq_pool pool;
            	public char name;
                public UInt32 irq;       //!< any value the user needs
                public UInt32 value;     //!< current value
                public byte flags;      //!< IRQ_* flags
                public Avr_irq_hook[] hook;	//!< list of hooks to be notified
          }


    ////! allocates 'count' IRQs, initializes their "irq" starting from 'base' and increment
    //avr_irq_t*
    //avr_alloc_irq(
    //        avr_irq_pool_t* pool,
    //        uint32_t base,
    //        uint32_t count,

    //        const char** names /* optional */);
    //        void
    //        avr_free_irq(
    //                avr_irq_t* irq,
    //                uint32_t count);

    //        //! init 'count' IRQs, initializes their "irq" starting from 'base' and increment
    //        void
    //        avr_init_irq(
    //                avr_irq_pool_t* pool,
    //                avr_irq_t* irq,
    //                uint32_t base,
    //                uint32_t count,

    //        const char** names /* optional */);
    //        //! Returns the current IRQ flags
    //        uint8_t
    //        avr_irq_get_flags(
    //                avr_irq_t* irq);
    //        //! Sets this irq's flags
    //        void
    //        avr_irq_set_flags(
    //                avr_irq_t* irq,
    //                uint8_t flags);
    
    /* 'raise' an IRQ. Ie call their 'hooks', and raise any chained IRQs, and set the new 'value' */
    
    //        //! Same as avr_raise_irq(), but also allow setting the float status
    //        void
    //        avr_raise_irq_float(
    //                avr_irq_t* irq,
    //                uint32_t value,
    //                int floating);
    //        //! this connects a "source" IRQ to a "destination" IRQ
    //        void
    //        avr_connect_irq(
    //                avr_irq_t* src,
    //                avr_irq_t* dst);
    //        void
    //        avr_unconnect_irq(
    //                avr_irq_t* src,
    //                avr_irq_t* dst);

    //        //! register a notification 'hook' for 'irq' -- 'param' is anything that your want passed back as argument
    //        void
    //        avr_irq_register_notify(
    //                avr_irq_t* irq,
    //                avr_irq_notify_t notify,
    //                void* param);

    //        void
    //        avr_irq_unregister_notify(
    //                avr_irq_t* irq,
    //                avr_irq_notify_t notify,
    //                void* param);

    class Sim_irq
    {

        ///*
        // * Internal IRQ system
        // *
        // * This subsystem allows any piece of code to "register" a hook to be called when an IRQ is
        // * raised. The IRQ definition is up to the module defining it, for example a IOPORT pin change
        // * might be an IRQ in which case any piece of code can be notified when a pin has changed state
        // *
        // * The notify hooks are chained, and duplicates are filtered out so you can't register a
        // * notify hook twice on one particular IRQ
        // *
        // * IRQ calling order is not defined, so don't rely on it.
        // *
        // * IRQ hook needs to be registered in reset() handlers, ie after all modules init() bits
        // * have been called, to prevent race condition of the initialization order.
        // */

        public static void _avr_irq_pool_add(ref Avr_irq_pool pool,ref Avr_irq irq)
        {
            if (pool.irq == null)
                pool.irq = new Avr_irq[0];
            Array.Resize<Avr_irq>(ref pool.irq, pool.irq.Length + 1);
            pool.count = pool.irq.Length;
            pool.irq[pool.count - 1] = irq;
            irq.pool = pool;
        }

        public static void _avr_irq_pool_remove(ref Avr_irq_pool pool,Avr_irq irq)
        {
            pool.irq = pool.irq.Where((val, idx) => val != irq).ToArray();
        }

        //! allocates 'count' IRQs, initializes their "irq" starting from '_base' and increment
        public static Avr_irq[] Avr_alloc_irq(ref Avr_irq_pool pool, uint _base, uint count, char[] names/* optional */)
        {
            Avr_irq[] irq = new Avr_irq[count];
            Avr_init_irq(ref pool, ref irq, _base, count, names);
            for (int i = 0; i < count; i++)
                irq[i].flags |= IRQ_FLAG_ALLOC;
            return irq;
        }

        /* init 'count' IRQs, initializes their "irq" starting from 'base' and increment */
        public static void Avr_init_irq(ref Avr_irq_pool pool, ref Avr_irq[] irq, uint _base, uint count, char[] names /* optional */)
        {
            irq = new Avr_irq[count];

            for (int i = 0; i < count; i++)
            {
                irq[i].irq = (uint)(_base + i);
                irq[i].flags = IRQ_FLAG_INIT;
                if (pool!=null)
                    _avr_irq_pool_add(ref pool, ref irq[i]);
                if ((names != null) && (names[i]!=0))
                    irq[i].name = names[i];
                else
                {
                    Console.WriteLine("WARNING 'Avr_init_irq' with NULL name for irq %d.\n", irq[i].irq);
                }
            }
        }


        //static avr_irq_hook_t*
        //_avr_alloc_irq_hook(
        //        avr_irq_t* irq)
        //{
        //    avr_irq_hook_t* hook = malloc(sizeof(avr_irq_hook_t));
        //    memset(hook, 0, sizeof(avr_irq_hook_t));
        //    hook->next = irq->hook;
        //    irq->hook = hook;
        //    return hook;
        //}

        //void
        //avr_free_irq(
        //        avr_irq_t* irq,
        //        uint32_t count)
        //{
        //    if (!irq || !count)
        //        return;
        //    for (int i = 0; i < count; i++)
        //    {
        //        avr_irq_t* iq = irq + i;
        //        if (iq->pool)
        //            _avr_irq_pool_remove(iq->pool, iq);
        //        if (iq->name)
        //            free((char*)iq->name);
        //        iq->name = NULL;
        //        // purge hooks
        //        avr_irq_hook_t* hook = iq->hook;
        //        while (hook)
        //        {
        //            avr_irq_hook_t* next = hook->next;
        //            free(hook);
        //            hook = next;
        //        }
        //        iq->hook = NULL;
        //    }
        //    // if that irq list was allocated by us, free it
        //    if (irq->flags & IRQ_FLAG_ALLOC)
        //        free(irq);
        //}

        //void
        //avr_irq_register_notify(
        //        avr_irq_t* irq,
        //        avr_irq_notify_t notify,
        //        void* param)
        //{
        //    if (!irq || !notify)
        //        return;

        //    avr_irq_hook_t* hook = irq->hook;
        //    while (hook)
        //    {
        //        if (hook->notify == notify && hook->param == param)
        //            return; // already there
        //        hook = hook->next;
        //    }
        //    hook = _avr_alloc_irq_hook(irq);
        //    hook->notify = notify;
        //    hook->param = param;
        //}

        //void
        //avr_irq_unregister_notify(
        //        avr_irq_t* irq,
        //        avr_irq_notify_t notify,
        //        void* param)
        //{
        //    avr_irq_hook_t* hook, *prev;
        //    if (!irq || !notify)
        //        return;

        //    hook = irq->hook;
        //    prev = NULL;
        //    while (hook)
        //    {
        //        if (hook->notify == notify && hook->param == param)
        //        {
        //            if (prev)
        //                prev->next = hook->next;
        //            else
        //                irq->hook = hook->next;
        //            free(hook);
        //            return;
        //        }
        //        prev = hook;
        //        hook = hook->next;
        //    }
        //}

        public static void Avr_raise_irq_float(Avr_irq irq,uint value,int floating)
        {
        //    if (!irq)
        //        return;
        //    uint32_t output = (irq->flags & IRQ_FLAG_NOT) ? !value : value;
        //    // if value is the same but it's the first time, raise it anyway
        //    if (irq->value == output &&
        //            (irq->flags & IRQ_FLAG_FILTERED) && !(irq->flags & IRQ_FLAG_INIT))
        //        return;
        //    irq->flags &= ~(IRQ_FLAG_INIT | IRQ_FLAG_FLOATING);
        //    if (floating)
        //        irq->flags |= IRQ_FLAG_FLOATING;
        //    avr_irq_hook_t* hook = irq->hook;
        //    while (hook)
        //    {
        //        avr_irq_hook_t* next = hook->next;
        //        // prevents reentrance / endless calling loops
        //        if (hook->busy == 0)
        //        {
        //            hook->busy++;
        //            if (hook->notify)
        //                hook->notify(irq, output, hook->param);
        //            if (hook->chain)
        //                avr_raise_irq_float(hook->chain, output, floating);
        //            hook->busy--;
        //        }
        //        hook = next;
        //    }
        //    // the value is set after the callbacks are called, so the callbacks
        //    // can themselves compare for old/new values between their parameter
        //    // they are passed (new value) and the previous irq->value
        //    irq->value = output;
        }

        /* 'raise' an IRQ. Ie call their 'hooks', and raise any chained IRQs, and set the new 'value' */
        public static void Avr_raise_irq(Avr_irq irq,uint value)
        {
            Avr_raise_irq_float(irq, value, (irq.flags & IRQ_FLAG_FLOATING));
        }

        //void
        //avr_connect_irq(
        //        avr_irq_t* src,
        //        avr_irq_t* dst)
        //{
        //    if (!src || !dst || src == dst)
        //    {
        //        fprintf(stderr, "error: %s invalid irq %p/%p", __FUNCTION__, src, dst);
        //        return;
        //    }
        //    avr_irq_hook_t* hook = src->hook;
        //    while (hook)
        //    {
        //        if (hook->chain == dst)
        //            return; // already there
        //        hook = hook->next;
        //    }
        //    hook = _avr_alloc_irq_hook(src);
        //    hook->chain = dst;
        //}

        //void
        //avr_unconnect_irq(
        //        avr_irq_t* src,
        //        avr_irq_t* dst)
        //{
        //    avr_irq_hook_t* hook, *prev;

        //    if (!src || !dst || src == dst)
        //    {
        //        fprintf(stderr, "error: %s invalid irq %p/%p", __FUNCTION__, src, dst);
        //        return;
        //    }
        //    hook = src->hook;
        //    prev = NULL;
        //    while (hook)
        //    {
        //        if (hook->chain == dst)
        //        {
        //            if (prev)
        //                prev->next = hook->next;
        //            else
        //                src->hook = hook->next;
        //            free(hook);
        //            return;
        //        }
        //        prev = hook;
        //        hook = hook->next;
        //    }
        //}

        //uint8_t
        //avr_irq_get_flags(
        //        avr_irq_t* irq)
        //{
        //    return irq->flags;
        //}

        //void
        //avr_irq_set_flags(
        //        avr_irq_t* irq,
        //        uint8_t flags)
        //{
        //    irq->flags = flags;
        //}










        //struct avr_irq_t;

        //        typedef void (* avr_irq_notify_t) (

        //        struct avr_irq_t * irq,
        //		uint32_t value,

        //        void* param);


        //        enum {
        //            IRQ_FLAG_NOT = (1 << 0),    //!< change polarity of the IRQ
        //            IRQ_FLAG_FILTERED = (1 << 1),   //!< do not "notify" if "value" is the same as previous raise
        //            IRQ_FLAG_ALLOC = (1 << 2), //!< this irq structure was malloced via avr_alloc_irq
        //            IRQ_FLAG_INIT = (1 << 3), //!< this irq hasn't been used yet
        //            IRQ_FLAG_FLOATING = (1 << 4), //!< this 'pin'/signal is floating
        //            IRQ_FLAG_USER = (1 << 5), //!< Can be used by irq users
        //        };

        public static byte IRQ_FLAG_NOT = (1 << 0);    //!< change polarity of the IRQ
        public static byte IRQ_FLAG_FILTERED = (1 << 1);   //!< do not "notify" if "value" is the same as previous raise
        public static byte IRQ_FLAG_ALLOC = (1 << 2); //!< this irq structure was malloced via avr_alloc_irq
        public static byte IRQ_FLAG_INIT = (1 << 3); //!< this irq hasn't been used yet
        public static byte IRQ_FLAG_FLOATING = (1 << 4); //!< this 'pin'/signal is floating
        public static byte IRQ_FLAG_USER = (1 << 5); //!< Can be used by irq users


        //        /*
        //         * IRQ Pool structure
        //         */
        //        typedef struct avr_irq_pool_t
        //        {
        //            int count;                      //!< number of irqs living in the pool
        //            struct avr_irq_t ** irq;		//!< irqs belonging in this pool
        //}
        //        avr_irq_pool_t;

        ///*!
        // * Public IRQ structure
        // */
        //typedef struct avr_irq_t
        //        {
        //            struct avr_irq_pool_t *	pool;
        //	const char* name;
        //            uint32_t irq;       //!< any value the user needs
        //            uint32_t value;     //!< current value
        //            uint8_t flags;      //!< IRQ_* flags
        //            struct avr_irq_hook_t * hook;	//!< list of hooks to be notified
        //}
        //        avr_irq_t;

      


        //        const char** names /* optional */);
        //        void
        //        avr_free_irq(
        //                avr_irq_t* irq,
        //                uint32_t count);

     
        //        //! Returns the current IRQ flags
        //        uint8_t
        //        avr_irq_get_flags(
        //                avr_irq_t* irq);
        //        //! Sets this irq's flags
        //        void
        //        avr_irq_set_flags(
        //                avr_irq_t* irq,
        //                uint8_t flags);
        //        //! 'raise' an IRQ. Ie call their 'hooks', and raise any chained IRQs, and set the new 'value'
        //        void
        //        avr_raise_irq(
        //                avr_irq_t* irq,
        //                uint32_t value);
        //        //! Same as avr_raise_irq(), but also allow setting the float status
        //        void
        //        avr_raise_irq_float(
        //                avr_irq_t* irq,
        //                uint32_t value,
        //                int floating);
        //        //! this connects a "source" IRQ to a "destination" IRQ
        //        void
        //        avr_connect_irq(
        //                avr_irq_t* src,
        //                avr_irq_t* dst);
        //        void
        //        avr_unconnect_irq(
        //                avr_irq_t* src,
        //                avr_irq_t* dst);

        //        //! register a notification 'hook' for 'irq' -- 'param' is anything that your want passed back as argument
        //        void
        //        avr_irq_register_notify(
        //                avr_irq_t* irq,
        //                avr_irq_notify_t notify,
        //                void* param);

        //        void
        //        avr_irq_unregister_notify(
        //                avr_irq_t* irq,
        //                avr_irq_notify_t notify,
        //                void* param);

        //# ifdef __cplusplus
        //    };
        //#endif

        //#endif /* __SIM_IRQ_H__ */ 


    }
}
