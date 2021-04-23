using SimulIDE.src.simavr.cores.avr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class Mega328 : Megax8
    {


        public static Mcu Make()
        {
            // include sim_avr.h !!!

            Constants.Set("SIM_MMCU", "atmega328");
            Constants.Set("SIM_CORENAME", "mcu_mega328");
            Constants.Set("SIM_VECTOR_SIZE", 4);

            Constants.Set("_AVR_IO_H_", 1);
            Constants.Set("__ASSEMBLER__", 1);
            iom328p.InitConstants();

            return new Mega328();
        }

        protected static Avr_kind self = new Avr_kind();
        public static Avr_kind Self() { return self; }
        static Mega328()
        {


            self.Names = new string[2];
            self.Names[0] = "atmega328";
            self.Names[1] = "atmega328p";
            self.Make = Make;
        }
        
    }
}
