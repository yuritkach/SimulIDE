using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
    public class Sim_regbit
    {






//#define ARRAY_SIZE(_aa) (sizeof(_aa) / sizeof((_aa)[0]))


//        /*
//         * These accessors are inlined and are used to perform the operations on
//         * avr_regbit_t definitions. This is the "official" way to access bits into registers
//         * The small footprint costs brings much better versatility for functions/bits that are
//         * not always defined in the same place on real AVR cores
//         */
//        /*
//         * set/get/clear io register bits in one operation
//         */
//        static inline uint8_t avr_regbit_set(avr_t* avr, avr_regbit_t rb)
//        {
//            uint16_t a = rb.reg;
//            uint8_t m;

//            if (!a)
//                return 0;
//            m = rb.mask << rb.bit;
//            avr_core_watch_write(avr, a, avr->data[a] | m);
//            return (avr->data[a] >> rb.bit) & rb.mask;
//        }

//        static inline uint8_t avr_regbit_setto(avr_t* avr, avr_regbit_t rb, uint8_t v)
//        {
//            uint16_t a = rb.reg;
//            uint8_t m;

//            if (!a) return 0;

//            m = rb.mask << rb.bit;
//            avr_core_watch_write(avr, a, (avr->data[a] & ~(m)) | ((v << rb.bit) & m));
//            return (avr->data[a] >> rb.bit) & rb.mask;
//        }

//        /*
//         * Set the 'raw' bits, if 'v' is the unshifted value of the bits
//         */
//        static inline uint8_t avr_regbit_setto_raw(avr_t* avr, avr_regbit_t rb, uint8_t v)
//        {
//            uint16_t a = rb.reg;
//            uint8_t m;

//            if (!a) return 0;

//            m = rb.mask << rb.bit;
//            avr_core_watch_write(avr, a, (avr->data[a] & ~(m)) | ((v) & m));
//            return (avr->data[a]) & (rb.mask << rb.bit);
//        }

//        static inline uint8_t avr_regbit_get(avr_t* avr, avr_regbit_t rb)
//        {
//            uint16_t a = rb.reg;
//            if (!a)
//                return 0;
//            //uint8_t m = rb.mask << rb.bit;
//            return (avr->data[a] >> rb.bit) & rb.mask;
//        }

//        /*
//         * Using regbit from value eliminates some of the
//         * set to test then clear register operations.
//         * makes cheking register bits before setting easier.
//         */
//        static inline uint8_t avr_regbit_from_value(
//            avr_t* avr __attribute__((unused)),
//            avr_regbit_t rb,
//            uint8_t value)
//        {
//            uint16_t a = rb.reg;
//            if (!a)
//                return 0;
//            return (value >> rb.bit) & rb.mask;
//        }

//        /*
//         * Return the bit(s) 'in position' instead of zero based
//         */
//        static inline uint8_t avr_regbit_get_raw(avr_t* avr, avr_regbit_t rb)
//        {
//            uint16_t a = rb.reg;
//            if (!a)
//                return 0;
//            //uint8_t m = rb.mask << rb.bit;
//            return (avr->data[a]) & (rb.mask << rb.bit);
//        }

//        static inline uint8_t avr_regbit_clear(avr_t* avr, avr_regbit_t rb)
//        {
//            uint16_t a = rb.reg;
//            uint8_t m = rb.mask << rb.bit;
//            avr_core_watch_write(avr, a, avr->data[a] & ~m);
//            return avr->data[a];
//        }


//        /*
//         * This reads the bits for an array of avr_regbit_t, make up a "byte" with them.
//         * This allows reading bits like CS0, CS1, CS2 etc even if they are not in the same
//         * physical IO register.
//         */
//        static inline uint8_t avr_regbit_get_array(avr_t* avr, avr_regbit_t* rb, int count)
//        {
//            uint8_t res = 0;
//            int i;

//            for (i = 0; i < count; i++, rb++) if (rb->reg)
//                {
//                    uint16_t a = rb->reg;
//                    res |= ((avr->data[a] >> rb->bit) & rb->mask) << i;
//                }
//            return res;
//        }

//        /*
//         * Does the reverse of avr_regbit_get_array
//         */
//        static inline void avr_regbit_set_array_from_value(
//            avr_t* avr,
//            avr_regbit_t* rb,
//            uint8_t count,
//            uint8_t value)
//        {
//            int i;
//            for (i = 0; i < count; i++, rb++) if (rb->reg)
//                {
//                    uint8_t rbv = (value >> (count - i)) & 1;
//                    avr_regbit_setto(avr, *rb, rbv);
//                }
//        }

        public static Avr_regbit  AVR_IO_REGBIT(int _io, byte _bit)
        {
            Avr_regbit result = new Avr_regbit();
            result.reg = _io;
            result.bit = _bit; 
            result.mask = 1;
            return result;
        }

        public static Avr_regbit AVR_IO_REGBITS(int _io, byte _bit, byte _mask)
        {
            Avr_regbit result = new Avr_regbit();
            result.reg = _io;
            result.bit = _bit; 
            result.mask = _mask;
            return result;
        }

    }
}
