using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    class Sim_core_declare
    {

        public static void InitConstants()
        {
            if (!Constants.Defined("__SIM_CORE_DECLARE_H__"))
            {
                Constants.Set("__SIM_CORE_DECLARE_H__", 1);
                if (Constants.Get("FUSE_MEMORY_SIZE") == 6)
                {
                    if (!Constants.Defined("FUSE0_DEFAULT")) Constants.Set("FUSE0_DEFAULT", 0xFF);
                    if (!Constants.Defined("FUSE1_DEFAULT")) Constants.Set("FUSE1_DEFAULT", 0xFF);
                    if (!Constants.Defined("FUSE2_DEFAULT")) Constants.Set("FUSE2_DEFAULT", 0xFF);
                    if (!Constants.Defined("FUSE3_DEFAULT")) Constants.Set("FUSE3_DEFAULT", 0xFF);
                    if (!Constants.Defined("FUSE4_DEFAULT")) Constants.Set("FUSE4_DEFAULT", 0xFF);
                    if (!Constants.Defined("FUSE5_DEFAULT")) Constants.Set("FUSE5_DEFAULT", 0xFF);
                    Constants.Set("_FUSE_HELPER",
                        new byte[] {
                        (byte)Constants.Get("FUSE0_DEFAULT"),
                        (byte)Constants.Get("FUSE1_DEFAULT"),
                        (byte)Constants.Get("FUSE2_DEFAULT"),
                        (byte)Constants.Get("FUSE3_DEFAULT"),
                        (byte)Constants.Get("FUSE4_DEFAULT"),
                        (byte)Constants.Get("FUSE5_DEFAULT")});
                }
                else
                if (Constants.Get("FUSE_MEMORY_SIZE") == 3)
                {
                    Constants.Set("_FUSE_HELPER", new byte[] {
                        (byte)Constants.Get("LFUSE_DEFAULT"),
                        (byte)Constants.Get("HFUSE_DEFAULT"),
                        (byte)Constants.Get("EFUSE_DEFAULT")
                    });
                }
                else
                if (Constants.Get("FUSE_MEMORY_SIZE") == 2)
                {
                    Constants.Set("_FUSE_HELPER", new byte[] {
                        (byte)Constants.Get("LFUSE_DEFAULT"),
                        (byte)Constants.Get("HFUSE_DEFAULT")
                    });
                }
                else
                if (Constants.Get("FUSE_MEMORY_SIZE") == 1)
                {
                    Constants.Set("_FUSE_HELPER", new byte[] {
                         (byte)Constants.Get("FUSE_DEFAULT")
                    });
                }
                else
                    Constants.Set("_FUSE_HELPER", new byte[] { 0 });


                if (Constants.Defined("MCUSR"))
                    Constants.Set("MCU_STATUS_REG",Constants.Get("MCUSR"));
                else
                    Constants.Set("MCU_STATUS_REG", Constants.Get("MCUCSR"));

            }
        }
        public static void DEFAULT_CORE(ref Avr avr, byte _vector_size)
        {
            if (avr == null)
                avr = new Avr();
            avr.ioend = (ushort)(Constants.Get("RAMSTART") - 1);
            avr.ramend = (ushort)Constants.Get("RAMEND");
            avr.flashend = (ushort)Constants.Get("FLASHEND");
            avr.e2end = (ushort)Constants.Get("E2END");
            avr.vector_size = _vector_size;

            if (Constants.Defined("SIGNATURE_0"))
            {
                avr.fuse = Constants.Get("_FUSE_HELPER");
                avr.signature = new byte[] {
                    (byte)Constants.Get("SIGNATURE_0"),
                    (byte)Constants.Get("SIGNATURE_1"),
                    (byte)Constants.Get("SIGNATURE_2")
                    };
                avr.lockbits = 0xFF;
                avr.reset_flags = new ResetFlags();
                avr.reset_flags.porf = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("MCU_STATUS_REG"), (byte)Constants.Get("PORF"));
                avr.reset_flags.extrf = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("MCU_STATUS_REG"), (byte)Constants.Get("EXTRF"));
                avr.reset_flags.borf = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("MCU_STATUS_REG"), (byte)Constants.Get("BORF"));
                avr.reset_flags.wdrf = Sim_regbit.AVR_IO_REGBIT((uint)Constants.Get("MCU_STATUS_REG"), (byte)Constants.Get("WDRF"));
            }
        }
    }
}
