using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src
{
    static class Utils
    {
        public static List<string> FileToStringList(string fileName)
        {
            return File.ReadAllLines(fileName).ToList();
        }


        public static string Val2Hex(int d)
        {
            string Hex = "0123456789ABCDEF";
            string h = Hex.Substring(d & 15, 1);
            while (d > 15)
            {
                d >>= 4;
                h = Hex.Substring(d & 15, 1) + h;
            }
            return h;
        }


        public static string DecToBase(int value, int outbase, int digits)
        {
            string converted = "";
            for (int i = 0; i < digits; i++)
            {
                if (value >= outbase) converted = Val2Hex(value % outbase) + converted;
                else converted = Val2Hex(value) + converted;

                if (i + 1 == 4) converted = " " + converted;
                //if( (i+1)%8 == 0 ) converted = " " + converted;

                value = Math.Floor(value / outbase);
            }
            return converted;
        }

    }
}
