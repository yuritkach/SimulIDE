namespace SimulIDE.src.simavr.cores.avr
{

    public class Sfr_defs
    {
        public static void InitConstants()
        {
            if (Constants.Get("_AVR_SFR_DEFS_H_") == null)
                Constants.Set("_AVR_SFR_DEFS_H_", 1);
        }

    }

}