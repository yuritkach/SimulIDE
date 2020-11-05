using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{

    public class Argv
    {
        public UInt32 size;
        public UInt32 argc;
        public string line;
        public string[] argv;
    }

    class Sim_utils
    {
        /*
         * Allocate a argv_t structure, split 'line' into words (destructively)
         * and fills up argc, and argv fields with pointers to the individual
         * words. The line is stripped of any \r\n as well
         * You can pass an already allocated argv_t for it to be reused (and
         * grown to fit).
         */
         public static Argv Argv_realloc(ref Argv argv, UInt32 size)
         {
             Array.Resize<string>(ref argv.argv,(int)size);
             argv.size = size;
             return argv;
        }


        /*
        * You are still responsible, as the caller, to (free) the resulting
        * pointer, and the 'line' text, if appropriate, no duplication is made
        */
        public static Argv Argv_parse(ref Argv argv,string line)
        {
        //    if (argv!=null)
        //        argv = Argv_realloc(ref argv, 8);
        //    argv.argc = 0;

        //    /* strip end of lines and trailing spaces */
        //char* d = line + strlen(line);
        //    while ((d - line) > 0 && *(--d) <= ' ')
        //        *d = 0;
        //    /* stop spaces + tabs */
        //    char* s = line;
        //    while (*s && *s <= ' ')
        //        s++;
        //    argv->line = s;
        //    char* a = NULL;
        //    do
        //    {
        //        if (argv->argc == argv->size)
        //            argv = argv_realloc(argv, argv->size + 8);
        //        if ((a = strsep(&s, " \t")) != NULL)
        //            argv->argv[argv->argc++] = a;
        //    } while (a);
        //    argv->argv[argv->argc] = NULL;
            return argv;
        }

    }
}
