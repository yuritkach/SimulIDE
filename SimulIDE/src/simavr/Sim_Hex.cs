using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public static int Read_hex_string(string src, ref byte[] buffer, int maxlen)
        {
            buffer = Enumerable.Range(0, src.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(src.Substring(x, 2), 16))
                     .ToArray();
            return buffer.Length;
        }

        /*
        * Read a .hex file, detects the various different chunks in it from their starting
        * addresses and allocate an array of ihex_chunk_t returned in 'chunks'.
        * Returns the number of chunks found, or -1 if an error occurs.
        */

        public static int Read_ihex_chunks(string fname, ref Ihex_chunk[] chunks)
        {
            if (fname=="" || chunks==null)
                return -1;
            FileStream fileStream = null;
            try
            {
                fileStream = File.OpenRead(fname);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error on open .hex file! "+e.Message);
                return -1;
            }

            UInt32 segment = 0;   // segment address
            int chunk = 0, max_chunks = 0;
            chunks = null;
            TextReader tr = new StreamReader(fileStream);
            while (fileStream.CanRead)
            {
                string line = tr.ReadLine();
                if (line[0] != ':')
                {
                    MessageBox.Show("AVR: "+ fname + " invalid ihex format ("+line+")\n");
                    break;
                }
            
                byte[] bline = new byte[64];

                int len = Read_hex_string(line + 1, ref bline,64);
                if (len <= 0)
                    continue;

                byte chk = 0;
               // calculate checksum
                int tlen = len - 1;
                for (int i=0; i<tlen;i++)
                    chk += bline[i];
                chk =(byte) (0x100 - chk);
            
            
                if (chk != bline[len - 1])
                {
                    MessageBox.Show("AVR: " + fname + " invalid checksumm (" + line + ")\n");
                    break;
                }
                UInt32 addr = 0;
                switch (bline[3])
                {
                    case 0: // normal data
                        addr = (UInt32)((int)segment | bline[1] << 8 | bline[2]);
                        break;
                    case 1: // end of file
                        continue;
                    case 2: // extended address 2 bytes
                        segment = (UInt32)((int)(bline[4] << 8) | bline[5]) << 4;
                        continue;
                    case 4:
                        segment = (UInt32)((bline[4] << 8) | bline[5]) << 16;
                        continue;
                    default:
                        MessageBox.Show("AVR: " + fname + " unsupported check type (" + line + ")\n");
                        continue;
                }

                if (chunk < max_chunks && addr != (chunks[chunk].baseaddr + chunks[chunk].size))
                {
                    if (chunks[chunk].size>0)
                        chunk++;
                }
            
                if (chunk >= max_chunks)
                {
                    max_chunks++;
                    Array.Resize<Ihex_chunk>(ref chunks, 1 + max_chunks);
                    chunks[chunk].baseaddr = addr;
                }

                Array.Resize<byte>(ref chunks[chunk].data, (int)(chunks[chunk].size + bline[0]));
                Array.Copy(bline,4,chunks[chunk].data,0,chunks[chunk].size);
                chunks[chunk].size += bline[0];
            }
            fileStream.Close();
            return max_chunks;
        }


        // reads IHEX file 'fname', puts it's decoded size in *'dsize' and returns
        // a newly allocated buffer with the binary data (or NULL, if error)

        public byte[] Read_ihex_file(string fname, ref UInt32 dsize, ref UInt32 start)
        {
            Ihex_chunk[] chunks = new Ihex_chunk[0];
            int count = Read_ihex_chunks(fname, ref chunks);
            byte[] res = null;
            if (count > 0)
            {
                dsize = chunks[0].size;
                start = chunks[0].baseaddr;
                res = chunks[0].data;
                chunks[0].data = null;
            }
            if (count > 1)
            {
                MessageBox.Show("AVR: " + fname + " ihex contains more chunks than loaded (" + count.ToString() + ")\n");
            }
            return res;
        }

    }
}
