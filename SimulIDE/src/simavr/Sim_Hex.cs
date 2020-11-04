using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr
{

    // a .hex file chunk (base address + size)
    public class  Ihex_chunk
    {
        public UInt32 baseaddr;  // offset it started at in the .hex file
        public byte[] data;      // read data
        public UInt32 size;      // read data size
    }

    class Sim_Hex
    {

        // hex dump from pointer 'b' for 'l' bytes with string prefix 'w'
        //public void hdump(string w, ref byte[] b, int l)
        //{

        //    UInt32 i;
	       // if (l< 16)
        //    {
		      //  printf("%s: ", w);
		      //  for (i = 0; i<l; i++) printf("%02x", b[i]);
        //    }
        //    else
        //    {
		      //  printf("%s:\n", w);
		      //  for (i = 0; i<l; i++)
        //        {
			     //   if (!(i & 0x1f))
        //                printf("    ");
        //            printf("%02x", b[i]);
			     //   if ((i & 0x1f) == 0x1f)
        //            {
				    //    printf(" ");
        //                printf("\n");
        //            }
		      //  }
	       // }
	       // printf("\n");
        //}

        // parses a hex text string 'src' of at max 'maxlen' characters, decodes it into 'buffer'
        public static int read_hex_string(string src, ref byte[] buffer, int maxlen)
        {
            byte[] dst = buffer;
            int ls = 0;
            byte b = 0;
            while (*src && maxlen)
            {
                char c = *src++;
                switch (c)
                {
                    case 'a'... 'f':   b = (b << 4) | (c - 'a' + 0xa); break;
                    case 'A'... 'F':   b = (b << 4) | (c - 'A' + 0xa); break;
                    case '0'... '9':   b = (b << 4) | (c - '0'); break;
                    default:
                        if (c > ' ')
                        {
                            fprintf(stderr, "%s: huh '%c' (%s)\n", __FUNCTION__, c, src);
                            return -1;
                        }
                    continue;
                }
                if (ls & 1)
                {
                    *dst++ = b; b = 0;
                    maxlen--;
                }
                ls++;
            }

            return dst - buffer;
        }

        /* Frees previously allocated chunks */
        public void free_ihex_chunks(Ihex_chunk[] chunks)
        {
            if (chunks==null) return;
            foreach (var chunk in chunks)
                chunk.data = null;
        }

        /*
        * Read a .hex file, detects the various different chunks in it from their starting
        * addresses and allocate an array of ihex_chunk_t returned in 'chunks'.
        * Returns the number of chunks found, or -1 if an error occurs.
        */

        public static int Read_ihex_chunks(string fname, ref Ihex_chunk[] chunks)
        {
            if (!fname || !chunks)
                return -1;
            FILE* f = fopen(fname, "r");
            if (!f)
            {
                perror(fname);
                return -1;
            }
            uint32_t segment = 0;   // segment address
            int chunk = 0, max_chunks = 0;
            *chunks = NULL;

            while (!feof(f))
            {
                char line[128];
                if (!fgets(line, sizeof(line) - 1, f))
                    continue;
                if (line[0] != ':')
                {
                    fprintf(stderr, "AVR: '%s' invalid ihex format (%.4s)\n", fname, line);
                    break;
                }
                uint8_t bline[64];

                int len = read_hex_string(line + 1, bline, sizeof(bline));
                if (len <= 0)
                    continue;

                uint8_t chk = 0;
                {   // calculate checksum
                    uint8_t* src = bline;
                    int tlen = len - 1;
                    while (tlen--)
                        chk += *src++;
                    chk = 0x100 - chk;
                }
                if (chk != bline[len - 1])
                {
                    fprintf(stderr, "%s: %s, invalid checksum %02x/%02x\n", __FUNCTION__, fname, chk, bline[len - 1]);
                    break;
                }
                uint32_t addr = 0;
                switch (bline[3])
                {
                    case 0: // normal data
                        addr = segment | (bline[1] << 8) | bline[2];
                        break;
                    case 1: // end of file
                        continue;
                    case 2: // extended address 2 bytes
                        segment = ((bline[4] << 8) | bline[5]) << 4;
                        continue;
                    case 4:
                        segment = ((bline[4] << 8) | bline[5]) << 16;
                        continue;
                    default:
                        fprintf(stderr, "%s: %s, unsupported check type %02x\n", __FUNCTION__, fname, bline[3]);
                        continue;
                }
                if (chunk < max_chunks && addr != ((*chunks)[chunk].baseaddr + (*chunks)[chunk].size))
                {
                    if ((*chunks)[chunk].size)
                        chunk++;
                }
                if (chunk >= max_chunks)
                {
                    max_chunks++;
                    /* Here we allocate and zero an extra chunk, to act as terminator */
                    *chunks = realloc(*chunks, (1 + max_chunks) * sizeof(ihex_chunk_t));
                    memset(*chunks + chunk, 0, (1 + (max_chunks - chunk)) * sizeof(ihex_chunk_t));
                    (*chunks)[chunk].baseaddr = addr;
                }
                (*chunks)[chunk].data = realloc((*chunks)[chunk].data, (*chunks)[chunk].size + bline[0]);
                memcpy((*chunks)[chunk].data + (*chunks)[chunk].size, bline + 4, bline[0]);
                (*chunks)[chunk].size += bline[0];
            }
            fclose(f);
            return max_chunks;
        }


        // reads IHEX file 'fname', puts it's decoded size in *'dsize' and returns
        // a newly allocated buffer with the binary data (or NULL, if error)

        public byte[] read_ihex_file(string fname, UInt32 dsize, UInt32 start)
        {
            ihex_chunk_p chunks = NULL;
            int count = read_ihex_chunks(fname, &chunks);
            uint8_t* res = NULL;

            if (count > 0)
            {
                *dsize = chunks[0].size;
                *start = chunks[0].baseaddr;
                res = chunks[0].data;
                chunks[0].data = NULL;
            }
            if (count > 1)
            {
                fprintf(stderr, "AVR: '%s' ihex contains more chunks than loaded (%d)\n",fname, count);
            }
            free_ihex_chunks(chunks);
            return res;
        }

    }
}
