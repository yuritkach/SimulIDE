using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class Sim_mega328
    {

        public const string SIM_MMCU = "atmega328";
        public const string SIM_CORENAME = "mcu_mega328";

        public static Avr Make()
        {
            return avr_core_allocate(mcu_mega328.core);
        }

        protected static Avr_kind self = new Avr_kind();
        public static Avr_kind Self() { return self; }
        static Sim_mega328()
        {
            self.Names = new string[2];
            self.Names[0] = "atmega328";
            self.Names[1] = "atmega328p";
            self.Make = Make;
        }

    }
}
