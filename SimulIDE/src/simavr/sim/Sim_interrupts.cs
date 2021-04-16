using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{

    public class Avr_int_vector
    {
        public byte vector;       // vector number, zero (reset) is reserved
        public Avr_regbit enable; // IO register index for the "interrupt enable" flag for this vector
        public Avr_regbit raised; // IO register index for the register where the "raised" flag is (optional)

        // 'pending' IRQ, and 'running' status as signaled here
        public Avr_irq[] irq;     //[AVR_INT_IRQ_COUNT];
        public byte pending;      // 1 while scheduled in the fifo
        public byte trace;        // only for debug of a vector
        public byte raise_sticky; // 1 if the interrupt flag (= the raised regbit) is not cleared
                                  // by the hardware when executing the interrupt routine (see TWINT)
    }


    public class Avr_int_table
    {
        public Avr_int_vector[] vector = new Avr_int_vector[64];
        public byte vector_count;
        //public Avr_int_pending pending;
        public byte pending;
        public byte running_ptr;
        public Avr_int_vector[] running = new Avr_int_vector[64]; // stack of nested interrupts
        // global status for pending + running in interrupt context
        public Avr_irq[] irq = new Avr_irq[Sim_interrupts.AVR_INT_IRQ_COUNT];
    }

    public class Sim_interrupts
    {

        //        DEFINE_FIFO(avr_int_vector_p, avr_int_pending);

        //        void
        //        avr_interrupt_init(
        //                avr_t* avr)
        //        {
        //            avr_int_table_p table = &avr->interrupts;
        //            memset(table, 0, sizeof(*table));

        //            static const char* names[] = { ">avr.int.pending", ">avr.int.running" };
        //            avr_init_irq(&avr->irq_pool, table->irq,
        //                    0, // base number
        //                    AVR_INT_IRQ_COUNT, names);
        //        }

        public static void Avr_interrupt_reset(Avr avr)
        {
            Avr_int_table table = avr.Interrupts;

            table.running_ptr = 0;
            // avr_int_pending_reset(ref table.pending); (функции нет в исходниках)
            avr.interrupt_state = 0;
            for (int i = 0; i < table.vector_count; i++)
                table.vector[i].pending = 0;
        }

        public static void Avr_register_vector(Avr avr, ref Avr_int_vector vector)
        {
        //            if (!vector->vector)
        //                return;

        //            avr_int_table_p table = &avr->interrupts;

        //            char name0[48], name1[48];
        //            sprintf(name0, ">avr.int.%02x.pending", vector->vector);
        //            sprintf(name1, ">avr.int.%02x.running", vector->vector);
        //            const char* names[2] = { name0, name1 };
        //            avr_init_irq(&avr->irq_pool, vector->irq,
        //                    vector->vector * 256, // base number
        //                    AVR_INT_IRQ_COUNT, names);
        //            table->vector[table->vector_count++] = vector;
        //            if (vector->trace)
        //                printf("IRQ%d registered (enabled %04x:%d)\n",
        //                    vector->vector, vector->enable.reg, vector->enable.bit);

        //            if (!vector->enable.reg)
        //                AVR_LOG(avr, LOG_WARNING, "IRQ%d No 'enable' bit !\n",
        //                    vector->vector);
        //        }

        //        int
        //        avr_has_pending_interrupts(
        //                avr_t* avr)
        //        {
        //            avr_int_table_p table = &avr->interrupts;
        //            return !avr_int_pending_isempty(&table->pending);
        //        }

        //        int
        //        avr_is_interrupt_pending(
        //                avr_t* avr,
        //                avr_int_vector_t* vector)
        //        {
        //            return vector->pending;
        //        }

        //        int
        //        avr_is_interrupt_enabled(
        //                avr_t* avr,
        //                avr_int_vector_t* vector)
        //        {
        //            return avr_regbit_get(avr, vector->enable);
        //        }

        //        int
        //        avr_raise_interrupt(
        //                avr_t* avr,
        //                avr_int_vector_t* vector)
        //        {
        //            if (!vector || !vector->vector)
        //                return 0;
        //            if (vector->pending)
        //            {
        //                if (vector->trace)
        //                    printf("IRQ%d:I=%d already raised (enabled %d) (cycle %lld pc 0x%x)\n",
        //                        vector->vector, !!avr->sreg[S_I], avr_regbit_get(avr, vector->enable),
        //                        (long long int)avr->cycle, avr->pc);
        //                return 0;
        //            }
        //            if (vector->trace)
        //                printf("IRQ%d raising (enabled %d)\n",
        //                    vector->vector, avr_regbit_get(avr, vector->enable));
        //            // always mark the 'raised' flag to one, even if the interrupt is disabled
        //            // this allow "polling" for the "raised" flag, like for non-interrupt
        //            // driven UART and so so. These flags are often "write one to clear"
        //            if (vector->raised.reg)
        //                avr_regbit_set(avr, vector->raised);

        //            avr_raise_irq(vector->irq + AVR_INT_IRQ_PENDING, 1);
        //            avr_raise_irq(avr->interrupts.irq + AVR_INT_IRQ_PENDING, 1);

        //            // If the interrupt is enabled, attempt to wake the core
        //            if (avr_regbit_get(avr, vector->enable))
        //            {
        //                // Mark the interrupt as pending
        //                vector->pending = 1;

        //                avr_int_table_p table = &avr->interrupts;

        //                avr_int_pending_write(&table->pending, vector);

        //                if (avr->sreg[S_I] && avr->interrupt_state == 0)
        //                    avr->interrupt_state = 1;
        //                if (avr->state == cpu_Sleeping)
        //                {
        //                    if (vector->trace)
        //                        printf("IRQ%d Waking CPU due to interrupt\n",
        //                            vector->vector);
        //                    avr->state = cpu_Running;   // in case we were sleeping
        //                }
        //            }
        //            // return 'raised' even if it was already pending
        //            return 1;
        //        }

        //        void
        //        avr_clear_interrupt(
        //                avr_t* avr,
        //                avr_int_vector_t* vector)
        //        {
        //            if (!vector)
        //                return;
        //            if (vector->trace)
        //                printf("IRQ%d cleared\n", vector->vector);
        //            vector->pending = 0;

        //            avr_raise_irq(vector->irq + AVR_INT_IRQ_PENDING, 0);
        //            avr_raise_irq_float(avr->interrupts.irq + AVR_INT_IRQ_PENDING,
        //                    avr_has_pending_interrupts(avr) ?
        //                            avr_int_pending_read_at(
        //                                    &avr->interrupts.pending, 0)->vector : 0,
        //                                    !avr_has_pending_interrupts(avr));

        //            if (vector->raised.reg && !vector->raise_sticky)
        //                avr_regbit_clear(avr, vector->raised);
        //        }

        //        int
        //        avr_clear_interrupt_if(
        //                avr_t* avr,
        //                avr_int_vector_t* vector,
        //                uint8_t old)
        //        {
        //            avr_raise_irq(avr->interrupts.irq + AVR_INT_IRQ_PENDING,
        //                    avr_has_pending_interrupts(avr));
        //            if (avr_regbit_get(avr, vector->raised))
        //            {
        //                avr_clear_interrupt(avr, vector);
        //                return 1;
        //            }
        //            avr_regbit_setto(avr, vector->raised, old);
        //            return 0;
        //        }

        //        avr_irq_t*
        //        avr_get_interrupt_irq(
        //                avr_t* avr,
        //                uint8_t v)
        //        {
        //            avr_int_table_p table = &avr->interrupts;
        //            if (v == AVR_INT_ANY)
        //                return table->irq;
        //            for (int i = 0; i < table->vector_count; i++)
        //                if (table->vector[i]->vector == v)
        //                    return table->vector[i]->irq;
        //            return NULL;
        //        }

        //        /* this is called uppon RETI. */
        //        void
        //        avr_interrupt_reti(

        //        struct avr_t * avr)
        //{
        //	avr_int_table_p table = &avr->interrupts;
        //	if (table->running_ptr) {
        //		avr_int_vector_t* vector = table->running[--table->running_ptr];
        //        avr_raise_irq(vector->irq + AVR_INT_IRQ_RUNNING, 0);
        //    }
        //    avr_raise_irq(table->irq + AVR_INT_IRQ_RUNNING,
        //            table->running_ptr > 0 ?

        //                    table->running[table->running_ptr - 1]->vector : 0);
        //    avr_raise_irq(avr->interrupts.irq + AVR_INT_IRQ_PENDING,
        //            avr_has_pending_interrupts(avr));
        }

        ///*
        // * check whether interrupts are pending. If so, check if the interrupt "latency" is reached,
        // * and if so triggers the handlers and jump to the vector.
        // */
        //void
        //avr_service_interrupts(
        //        avr_t* avr)
        //{
        //    if (!avr->sreg[S_I] || !avr->interrupt_state)
        //        return;

        //    if (avr->interrupt_state < 0)
        //    {
        //        avr->interrupt_state++;
        //        if (avr->interrupt_state == 0)
        //            avr->interrupt_state = avr_has_pending_interrupts(avr);
        //        return;
        //    }

        //    avr_int_table_p table = &avr->interrupts;

        //    // how many are pending...
        //    int cnt = avr_int_pending_get_read_size(&table->pending);
        //    // locate the highest priority one
        //    int min = 0xff;
        //    int mini = 0;
        //    for (int ii = 0; ii < cnt; ii++)
        //    {
        //        avr_int_vector_t* v = avr_int_pending_read_at(&table->pending, ii);
        //        if (v->vector < min)
        //        {
        //            min = v->vector;
        //            mini = ii;
        //        }
        //    }
        //    avr_int_vector_t* vector = avr_int_pending_read_at(&table->pending, mini);

        //    // now move the one at the front of the fifo in the slot of
        //    // the one we service
        //    table->pending.buffer[(table->pending.read + mini) % avr_int_pending_fifo_size] =
        //            avr_int_pending_read(&table->pending);
        //    avr_raise_irq(avr->interrupts.irq + AVR_INT_IRQ_PENDING,
        //            avr_has_pending_interrupts(avr));

        //    // if that single interrupt is masked, ignore it and continue
        //    // could also have been disabled, or cleared
        //    if (!avr_regbit_get(avr, vector->enable) || !vector->pending)
        //    {
        //        vector->pending = 0;
        //        avr->interrupt_state = avr_has_pending_interrupts(avr);
        //    }
        //    else
        //    {
        //        if (vector && vector->trace)
        //            printf("IRQ%d calling\n", vector->vector);
        //        _avr_push_addr(avr, avr->pc);
        //        avr_sreg_set(avr, S_I, 0);
        //        avr->pc = vector->vector * avr->vector_size;

        //        avr_raise_irq(vector->irq + AVR_INT_IRQ_RUNNING, 1);
        //        avr_raise_irq(table->irq + AVR_INT_IRQ_RUNNING, vector->vector);
        //        if (table->running_ptr == ARRAY_SIZE(table->running))
        //        {
        //            AVR_LOG(avr, LOG_ERROR, "%s run out of nested stack!", __func__);
        //        }
        //        else
        //        {
        //            table->running[table->running_ptr++] = vector;
        //        }
        //        avr_clear_interrupt(avr, vector);
        //    }
        //}



        //# ifndef __SIM_INTERRUPTS_H__
        //#define __SIM_INTERRUPTS_H__

        //# include "sim_avr_types.h"
        //# include "sim_irq.h"
        //# include "fifo_declare.h"

        //# ifdef __cplusplus
        //        extern "C" {
        //#endif

        //enum {
        //            AVR_INT_IRQ_PENDING = 0,
        //            AVR_INT_IRQ_RUNNING,
        //            AVR_INT_IRQ_COUNT,
        //            AVR_INT_ANY = 0xff, // for avr_get_interrupt_irq()
        //        };
        //        // interrupt vector for the IO modules
        public static byte AVR_INT_IRQ_PENDING = 0;
        public static byte AVR_INT_IRQ_RUNNING = 1;
        public static byte AVR_INT_IRQ_COUNT = 2;
        public static byte AVR_INT_ANY = 0xFF;




        

    //// Size needs to be >= max number of vectors, and a power of two
    //DECLARE_FIFO(avr_int_vector_p, avr_int_pending, 64);

    //        // interrupt vectors, and their enable/clear registers
    

    ///*
    // * Interrupt Helper Functions
    // */
    //// register an interrupt vector. It's only needed if you want to use the "r_raised" flags
    //void
    //avr_register_vector(

    //        struct avr_t *avr,
    //		avr_int_vector_t* vector);
    //        // raise an interrupt (if enabled). The interrupt is latched and will be called later
    //        // return non-zero if the interrupt was raised and is now pending
    //        int
    //        avr_raise_interrupt(

    //        struct avr_t * avr,
    //		avr_int_vector_t* vector);
    //        // return non-zero if the AVR core has any pending interrupts
    //        int
    //        avr_has_pending_interrupts(

    //        struct avr_t * avr);
    //// return nonzero if a specific interrupt vector is pending
    //int
    //avr_is_interrupt_pending(

    //        struct avr_t * avr,
    //		avr_int_vector_t* vector);
    //        // clear the "pending" status of an interrupt
    //        void
    //        avr_clear_interrupt(

    //        struct avr_t * avr,
    //		avr_int_vector_t* vector);
    //        // called by the core at each cycle to check whether an interrupt is pending
    //        void
    //        avr_service_interrupts(

    //        struct avr_t * avr);
    //// called by the core when RETI opcode is ran
    //void
    //avr_interrupt_reti(

    //        struct avr_t * avr);
    //// clear the interrupt (inc pending) if "raised" flag is 1
    //int
    //avr_clear_interrupt_if(

    //        struct avr_t * avr,
    //		avr_int_vector_t* vector,
    //        uint8_t old);

    //// return the IRQ that is raised when the vector is enabled and called/cleared
    //// this allows tracing of pending interrupts
    //avr_irq_t*
    //avr_get_interrupt_irq(

    //        struct avr_t * avr,
    //		uint8_t v);

    //        // Initializes the interrupt table
    //        void
    //        avr_interrupt_init(

    //        struct avr_t * avr );

    //// reset the interrupt table and the fifo
    //void
    //avr_interrupt_reset(

    //        struct avr_t * avr );

    }
}
