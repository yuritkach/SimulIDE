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
       // public Avr_int_pending pending;
        public byte[] pending = new byte[0]; 
        public byte running_ptr;
        public Avr_int_vector[] running = new Avr_int_vector[64]; // stack of nested interrupts
        // global status for pending + running in interrupt context
        public Avr_irq[] irq = new Avr_irq[Sim_interrupts.AVR_INT_IRQ_COUNT];
    }

    public class Sim_interrupts
    {

        //        DEFINE_FIFO(avr_int_vector_p, avr_int_pending);

        public static void Avr_interrupt_init(ref Avr avr)
        {
            Avr_int_table table = avr.Interrupts;
            string[] names = new string[]{ ">avr.int.pending", ">avr.int.running" };
            Sim_irq.Avr_init_irq(ref avr.irq_pool,ref table.irq,0, AVR_INT_IRQ_COUNT, names);
        }

        public static void Avr_interrupt_reset(Avr avr)
        {
            Avr_int_table table = avr.Interrupts;

            table.running_ptr = 0;
            // avr_int_pending_reset(ref table.pending); (функции нет в исходниках)
            avr.interrupt_state = 0;
            for (int i = 0; i < table.vector_count; i++)
                table.vector[i].pending = 0;
        }

        public static void Avr_register_vector(Avr avr, Avr_int_vector vector)
        {
            if (vector == null)
                vector = new Avr_int_vector();
            if (vector.vector==0)
                return;
            Avr_int_table table = avr.Interrupts;
            string name0 = ">avr.int."+vector.vector.ToString("X2")+".pending";
            string name1 = ">avr.int." + vector.vector.ToString("X2") + ".running";
            string[] names = new string[] { name0, name1 };
            Sim_irq.Avr_init_irq(ref avr.irq_pool,ref vector.irq,(uint)(vector.vector * 256), AVR_INT_IRQ_COUNT, names);
            table.vector[table.vector_count] = vector;
            table.vector_count++;
            if (vector.trace!=0)
                Console.WriteLine("IRQ{0:G} registered (enabled {1:X4}:{2:G})\n",vector.vector, vector.enable.reg, vector.enable.bit);
            //            if (!vector->enable.reg)
            //                AVR_LOG(avr, LOG_WARNING, "IRQ%d No 'enable' bit !\n",vector->vector);
        }

        public static bool Avr_has_pending_interrupts(Avr avr)
        {
                    Avr_int_table table = avr.Interrupts;
                    return table.pending.Length!=0;
        }

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

        public static int Avr_raise_interrupt(Avr avr, Avr_int_vector vector)
        {
            if (vector==null || vector.vector==0)
                return 0;
            if (vector.pending!=0)
            {
                if (vector.trace!=0)
                    Console.WriteLine("IRQ{0:G}:I={1:G} already raised (enabled {2:G}) (cycle {3:G} pc {4:X4})\n",
                        vector.vector, avr.sreg[Sim_core_helper.S_I], Sim_regbit.Avr_regbit_get(avr, vector.enable),
                        avr.cycle, avr.PC);
                return 0;
            }
            if (vector.trace!=0)
                Console.WriteLine("IRQ{0:G} raising (enabled {1:G})\n",
                    vector.vector, Sim_regbit.Avr_regbit_get(avr, vector.enable));
            // always mark the 'raised' flag to one, even if the interrupt is disabled
            // this allow "polling" for the "raised" flag, like for non-interrupt
            // driven UART and so so. These flags are often "write one to clear"
            if (vector.raised.reg!=0)
                Sim_regbit.Avr_regbit_set(avr, vector.raised);

            Sim_irq.Avr_raise_irq(vector.irq[AVR_INT_IRQ_PENDING], 1);
            Sim_irq.Avr_raise_irq(avr.Interrupts.irq[AVR_INT_IRQ_PENDING], 1);

            // If the interrupt is enabled, attempt to wake the core
            if (Sim_regbit.Avr_regbit_get(avr, vector.enable)!=0)
            {
                // Mark the interrupt as pending
                vector.pending = 1;

                Avr_int_table table = avr.Interrupts;

                // Avr_int_pending_write(table.pending, vector); нет в исходниках

                if (avr.sreg[Sim_core_helper.S_I]!=0 && avr.interrupt_state == 0)
                    avr.interrupt_state = 1;
                if (avr.state == Sim_Avr.CoreStates.cpu_Sleeping)
                {
                    if (vector.trace!=0)
                        Console.WriteLine("IRQ{0:G} Waking CPU due to interrupt\n",vector.vector);
                    avr.state = Sim_Avr.CoreStates.cpu_Running;   // in case we were sleeping
                }
            }
            // return 'raised' even if it was already pending
            return 1;
        }

        public static void Avr_clear_interrupt(Avr avr,Avr_int_vector vector)
        {
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
        }

        public static int Avr_clear_interrupt_if(Avr avr, Avr_int_vector vector,byte old)
        {
                    Sim_irq.Avr_raise_irq(avr.Interrupts.irq[AVR_INT_IRQ_PENDING],(uint)(Avr_has_pending_interrupts(avr)?1:0));
                    if (Sim_regbit.Avr_regbit_get(avr, vector.raised)!=0)
                    {
                        Avr_clear_interrupt(avr, vector);
                        return 1;
                    }
                    Sim_regbit.Avr_regbit_setto(avr, vector.raised, old);
                    return 0;
        }

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

        /* this is called uppon RETI. */
        public static void Avr_interrupt_reti(Avr avr)
        {
        	Avr_int_table table = avr.Interrupts;
        	if (table.running_ptr!=0)
            {
        		Avr_int_vector vector = table.running[--table.running_ptr];
                Sim_irq.Avr_raise_irq(vector.irq[AVR_INT_IRQ_RUNNING], 0);
            }
            Sim_irq.Avr_raise_irq(table.irq[AVR_INT_IRQ_RUNNING],
                (uint)(table.running_ptr > 0 ?table.running[table.running_ptr - 1].vector : 0));
            Sim_irq.Avr_raise_irq(avr.Interrupts.irq[AVR_INT_IRQ_PENDING],(uint)(Avr_has_pending_interrupts(avr)?1:0));
        }

        ///*
        // * check whether interrupts are pending. If so, check if the interrupt "latency" is reached,
        // * and if so triggers the handlers and jump to the vector.
        // */
        public static void Avr_service_interrupts(Avr avr)
        {
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
        }

        public static byte AVR_INT_IRQ_PENDING = 0;
        public static byte AVR_INT_IRQ_RUNNING = 1;
        public static byte AVR_INT_IRQ_COUNT = 2;
        public static byte AVR_INT_ANY = 0xFF;

        //DECLARE_FIFO(avr_int_vector_p, avr_int_pending, 64);

    }
}
