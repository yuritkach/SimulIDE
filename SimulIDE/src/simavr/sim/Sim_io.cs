using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.sim
{
    public delegate int ioctl_function(Avr_io io, uint ctl, object[] io_param);
    public delegate void ioportreset_function(Avr_io io);
    public delegate void iodealoc_function(Avr_io io);
    public class Avr_io
    {
   //     public Avr_io next;
        public Avr avr;     // avr we are attached to
        public string kind;       // pretty name, for debug
        public string[] irq_names; // IRQ names

        public uint irq_ioctl_get; // used to get irqs from this module
        public uint irq_count;  // number of (optional) irqs
        public Avr_irq[] irq;       // optional external IRQs
                                    // called at reset time
        public ioportreset_function reset;
        // called externally. allow access to io modules and so on
        public ioctl_function ioctl;
        // optional, a function to free up allocated system resources
        public iodealoc_function dealloc;
    }

    public class Sim_io
    {
        
        public static uint AVR_IOCTL_DEF(byte _a,byte _b,byte _c,byte _d)
        {
            return (uint)(((_a) << 24) | ((_b) << 16) | ((_c) << 8) | ((_d)));
        }

        public static int Avr_ioctl(Avr avr,uint ctl,object io_param)
        {
            int res = -1;
            foreach(Avr_io port in avr.io_ports)
            {
                if (port.ioctl!=null)
                    res = port.ioctl(port, ctl, new object[1] { io_param });
            }
            return res;
        }

        public static void Avr_register_io(Avr avr,Avr_io io)
        {
            io.avr = avr;
            avr.io_ports.Enqueue(io);
        }

        public static void Avr_register_io_read(Avr avr,uint addr,Avr_io_read_function readp,object param)
        {
            uint a = Sim_Avr.AVR_DATA_TO_IO(addr);
            if (avr.io[a].r.param!=null || avr.io[a].r.c!=null)
            {
                if (avr.io[a].r.param != param || avr.io[a].r.c != readp)
                {
//                    AVR_LOG(avr, LOG_ERROR,"IO: %s(): Already registered, refusing to override.\n",__func__);
//                    AVR_LOG(avr, LOG_ERROR,"IO: %s(%04x : %p/%p): %p/%p\n",__func__, a,avr->io[a].r.c, avr->io[a].r.param, readp, param);
//                    abort();
                }
            }
            avr.io[a].r.param = param;
            avr.io[a].r.c = readp;
        }

        public static void _avr_io_mux_write(Avr avr,uint addr,byte v,object param)
        {
            int io = (int)param;
            for (int i = 0; i < avr.io_shared_io[io].used; i++)
            {
                Avr_io_write_function c = avr.io_shared_io[io].ios[i].c;
                if (c!=null)
                    c(avr, addr, v, avr.io_shared_io[io].ios[i].param);
            }
        }

        public static void Avr_register_io_write(Avr avr, uint addr, Avr_io_write_function writep,object param)
        {
            uint a =Sim_Avr.AVR_DATA_TO_IO(addr);
            if (a < 0) return;
            if (a >= Sim_Avr.MAX_IOs)
                throw new Exception("IO address 0x "+a.ToString()+" out of range ("+ Sim_Avr.MAX_IOs.ToString()+")");
            /*
             * Verifying that some other piece of code is not installed to watch write
             * on this address. If there is, this code installs a "dispatcher" callback
             * instead to handle multiple clients, otherwise, it continues as usual
             */
            if ((avr.io[a].w.param!=null) || (avr.io[a].w.c!=null))
            {
                if (avr.io[a].w.param != param || avr.io[a].w.c != writep)
                {
                    // if the muxer not already installed, allocate a new slot
                    int no = avr.io_shared_io_count++;
                    if (avr.io[a].w.c != _avr_io_mux_write)
                    {
                        if (avr.io_shared_io_count > avr.io_shared_io.Length)
                        {
                            //AVR_LOG(avr, LOG_ERROR,
                            //        "IO: %s(): Too many shared IO registers.\n", __func__);
                            //abort();
                            throw new Exception("Too many shared IO registers.\n");

                        }
//                        AVR_LOG(avr, LOG_TRACE,
//                               "IO: %s(%04x): Installing muxer on register.\n",
//                                __func__, addr);
                        avr.io_shared_io[no].used = 1;
                        avr.io_shared_io[no].ios[0].param = avr.io[a].w.param;
                        avr.io_shared_io[no].ios[0].c = avr.io[a].w.c;
                        avr.io[a].w.param = (object)no;
                        avr.io[a].w.c = _avr_io_mux_write;
                    }
                    no = (int)avr.io[a].w.param;
                    int d = avr.io_shared_io[no].used++;
                    if (avr.io_shared_io[no].used > avr.io_shared_io[0].ios.Length)
                    {
                       // AVR_LOG(avr, LOG_ERROR,
                       //         "IO: %s(): Too many callbacks on %04x.\n",
                       //         __func__, addr);
                       // abort();
                        throw new Exception(string.Format("Too many callbacks on {0:X4}\n",addr));
                    }
                    avr.io_shared_io[no].ios[d].param = param;
                    avr.io_shared_io[no].ios[d].c = writep;
                    return;
                }
            }
             
            avr.io[a].w.param = param;
            avr.io[a].w.c = writep;
        }

        public static Avr_irq Avr_io_getirq(Avr avr,uint ctl,int index)
        {
            foreach(Avr_io port in avr.io_ports)
            {
                if ((port.irq.Length>0 && port.irq_ioctl_get == ctl) && (port.irq_count > index))
                    return port.irq[index];
            }
            return null;
        }

        public static Avr_irq Avr_iomem_getirq(Avr avr, uint addr, string name, int index)
        {
            if (index > 8)
                return null;
            uint a = Sim_core_helper.AVR_DATA_TO_IO(addr);
            if (avr.io[a].irq == null)
            {
                /*
                 * Prepare an array of names for the io IRQs. Ideally we'd love to have
                 * a proper name for these, but it's not possible at this time.
                 */
                string[] names = new string[9];
                string d;
                for (int ni = 0; ni < 9; ni++)
                {
                    if (ni < 8)
                        d = string.Format("=avr.io.{0:X4}.{1:G}", addr, ni);
                    else
                        d = string.Format("8=avr.io.{0:X4}.all", addr);
                    names[ni] = d;
                }
                avr.io[a].irq = Sim_irq.Avr_alloc_irq(ref avr.irq_pool, 0, 9, names);
                // mark the pin ones as filtered, so they only are raised when changing
                for (int i = 0; i < 8; i++)
                    avr.io[a].irq[i].flags |= Sim_irq.IRQ_FLAG_FILTERED;
            }
            // if given a name, replace the default one...
            if (name!="")
            {
                int l = name.Length;
                string n = "avr.io."+name;
                avr.io[a].irq[index].name = n;
            }
            return avr.io[a].irq[index];
        }

        public static Avr_irq[] Avr_io_setirqs(Avr_io io,uint ctl,uint count, Avr_irq[] irqs)
        {
            // allocate this module's IRQ
            io.irq_count = count;

            if (irqs==null)
            {
                string[] irq_names = null;

                if (io.irq_names!=null)
                {
                    irq_names = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        /*
                        * this bit takes the io module 'kind' ("port")
                        * the IRQ name ("=0") and the last character of the ioctl ('p','o','r','A')
                        * to create a full name "=porta.0"
                        */

                        string dst = "";
                        dst += io.irq_names[i][0];
                        // copy the 'flags' of the name out
                        string kind = io.irq_names[i].Substring(1);

             //           while (isdigit(*kind))
             //               *dst++ = *kind++;
             //           while (!isalpha(*kind))
             //               *dst++ = *kind++;
                        // add avr name
                        dst += io.avr.mmcu;
                        dst += "avr";
                        dst += '.';
                        // add module 'kind'
                        dst += io.kind;
                        // add port name, if any
                        if ((ctl & 0xff) > ' ')
                            dst += (char)(ctl & 0xff);
                        dst+= '.';
                        // add the rest of the irq name
                        dst += kind;
                        irq_names[i] = dst;
                    }
                }

                irqs = Sim_irq.Avr_alloc_irq(ref io.avr.irq_pool, 0,count,irq_names);
                irq_names = null;
            }
    
            io.irq = irqs;
            io.irq_ioctl_get = ctl;
            return io.irq;
        }

        public static void Avr_deallocate_io(Avr_io io)
        {
            io.dealloc?.Invoke(io);
            Sim_irq.Avr_free_irq(io.irq, io.irq_count);
            io.irq_count = 0;
            io.irq_ioctl_get = 0;
            io.avr = null;
        }

        public static void Avr_deallocate_ios(Avr avr)
        {
            foreach(Avr_io port in avr.io_ports)
                Avr_deallocate_io(port);
            avr.io_ports = null;
        }

        public static int AVR_IOMEM_IRQ_ALL = 8;

    }
}
