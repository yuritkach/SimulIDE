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


        protected override void Init()
        {
            base.Init();

            Constants.Set("SIM_MMCU","atmega328");
            Constants.Set("SIM_CORENAME","mcu_mega328");
            Constants.Set("SIM_VECTOR_SIZE", 4);
            /* Signature */
            Constants.Set("SIGNATURE_0", 0x1E);
            Constants.Set("SIGNATURE_1", 0x95);
            Constants.Set("SIGNATURE_2", 0x0F);

            /* Constants */
            Constants.Set("SPM_PAGESIZE", 128);
            Constants.Set("RAMSTART" ,0x100);
            Constants.Set("RAMEND" , 0x8FF);     /* Last On-Chip SRAM Location */
            Constants.Set("XRAMSIZE" , 0);
            Constants.Set("XRAMEND" , Constants.Get("RAMEND"));
            Constants.Set("E2END ", 0x3FF);
            Constants.Set("E2PAGESIZE" , 4);
            Constants.Set("FLASHEND" , 0x7FFF);

            Constants.Set("EEARH" ,Constants._SFR_IO8(0x22));
            Constants.Set("EEARL" , Constants._SFR_IO8(0x21));
            Constants.Set("EEDR" , Constants._SFR_IO8(0x20));
            Constants.Set("EECR" , Constants._SFR_IO8(0x0F));
            Constants.Set("EERE" , 0);
            Constants.Set("EEPE" ,1);
            Constants.Set("EEMPE" , 2);
            Constants.Set("EERIE" , 3);
            Constants.Set("EEPM0" , 4);
            Constants.Set("EEPM1" , 5);

            Constants.Set("MCUSR" , Constants._SFR_IO8(0x34));
            Constants.Set("MCUCSR" , Constants._SFR_IO8(0x35));

            /* Fuses */
            Constants.Set("FUSE_MEMORY_SIZE" , 3);
        }

    }
}
