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


        protected override void InitConstants()
        {
            base.InitConstants();

            Constants.SIM_MMCU = "atmega328";
            Constants.SIM_CORENAME = "mcu_mega328";
            Constants.SIM_VECTOR_SIZE = 4;
            /* Signature */
            Constants.SIGNATURE_0 = 0x1E;
            Constants.SIGNATURE_1 = 0x95;
            Constants.SIGNATURE_2 = 0x0F;

            /* Constants */
            Constants.SPM_PAGESIZE = 128;
            Constants.RAMSTART = 0x100;
            Constants.RAMEND = 0x8FF;     /* Last On-Chip SRAM Location */
            Constants.XRAMSIZE = 0;
            Constants.XRAMEND = Constants.RAMEND;
            Constants.E2END = 0x3FF;
            Constants.E2PAGESIZE = 4;
            Constants.FLASHEND = 0x7FFF;

            Constants.EEARH =Constants._SFR_IO8(0x22);
            Constants.EEARL = Constants._SFR_IO8(0x21);
            Constants.EEDR = Constants._SFR_IO8(0x20);
            Constants.EECR = Constants._SFR_IO8(0x0F);
            Constants.EERE = 0;
            Constants.EEPE = 1;
            Constants.EEMPE = 2;
            Constants.EERIE = 3;
            Constants.EEPM0 = 4;
            Constants.EEPM1 = 5;

            Constants.MCUSR = Constants._SFR_IO8(0x34);
            Constants.MCUCSR = Constants._SFR_IO8(0x35);

            /* Fuses */
            Constants.FUSE_MEMORY_SIZE = 3;
        }

    }
}
