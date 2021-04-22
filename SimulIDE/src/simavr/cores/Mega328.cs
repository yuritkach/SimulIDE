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

            ConstantsX328.SIM_MMCU = "atmega328";
            ConstantsX328.SIM_CORENAME = "mcu_mega328";
            ConstantsX328.SIM_VECTOR_SIZE = 4;
            /* Signature */
            ConstantsX328.SIGNATURE_0 = 0x1E;
            ConstantsX328.SIGNATURE_1 = 0x95;
            ConstantsX328.SIGNATURE_2 = 0x0F;

            /* Constants */
            ConstantsX328.SPM_PAGESIZE = 128;
            ConstantsX328.RAMSTART = 0x100;
            ConstantsX328.RAMEND = 0x8FF;     /* Last On-Chip SRAM Location */
            ConstantsX328.XRAMSIZE = 0;
            ConstantsX328.XRAMEND = ConstantsX328.RAMEND;
            ConstantsX328.E2END = 0x3FF;
            ConstantsX328.E2PAGESIZE = 4;
            ConstantsX328.FLASHEND = 0x7FFF;

            ConstantsX328.EEARH =ConstantsX328._SFR_IO8(0x22);
            ConstantsX328.EEARL = ConstantsX328._SFR_IO8(0x21);
            ConstantsX328.EEDR = ConstantsX328._SFR_IO8(0x20);
            ConstantsX328.EECR = ConstantsX328._SFR_IO8(0x0F);
            ConstantsX328.EERE = 0;
            ConstantsX328.EEPE = 1;
            ConstantsX328.EEMPE = 2;
            ConstantsX328.EERIE = 3;
            ConstantsX328.EEPM0 = 4;
            ConstantsX328.EEPM1 = 5;

            ConstantsX328.MCUSR = ConstantsX328._SFR_IO8(0x34);
            ConstantsX328.MCUCSR = ConstantsX328._SFR_IO8(0x35);

            /* Fuses */
            ConstantsX328.FUSE_MEMORY_SIZE = 3;
        }

    }
}
