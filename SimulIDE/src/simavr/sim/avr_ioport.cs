using SimulIDE.src.simavr.cores;
using System;

namespace SimulIDE.src.simavr.sim {

    public delegate void IOPortResetDelegate(Avr_io port);

    public delegate int IOPortIOCtlDelegate(Avr_io port, uint ctl, object parms);

    public class Avr_ioport_external
    {
        public byte name;
        public byte mask;
        public byte value;
    } 
   
    public class Avr_ioport
    {
        public Avr_io io;
        public string name;
        public uint r_port;
        public uint r_ddr;
        public uint r_pin;

        public Avr_int_vector pcint;   // PCINT vector
        public uint r_pcint;        // pcint 8 pins mask

        // this represent the default IRQ value when
        // the port is set as input.
        // If the mask is not set, no output value is sent
        // on the output IRQ. If the mask is set, the specified
        // value is sent.
        public byte pull_mask;
        public byte pull_value;
    }



    public class Avr_ioports {

        public static byte Avr_ioport_read(Avr avr,uint addr,object param)
        {
        	Avr_ioport p = (Avr_ioport)param;
        	byte ddr = avr.data[p.r_ddr];
        	byte v = (byte)((avr.data[p.r_pin] & ~ddr) | (avr.data[p.r_port] & ddr));
        	avr.data[addr] = v;
        	// made to trigger potential watchpoints
        	v = Sim_core.Avr_core_watch_read(avr, addr);
        	Sim_irq.Avr_raise_irq(p.io.irq[IOPORT_IRQ_REG_PIN], v);
        	if (avr.data[addr] != v)
                Console.WriteLine("** PIN%c(%02x) = %02x\r\n", p.name, addr, v);

        	return v;
        }

        public static void Avr_ioport_update_irqs(ref Avr_ioport p)
        {
        //	avr_t * avr = p->io.avr;
        //	uint8_t ddr = avr->data[p->r_ddr];
        //	// Set the PORT value if the pin is marked as output
        //	// otherwise, if there is an 'external' pullup, set it
        //	// otherwise, if the PORT pin was 1 to indicate an
        //	// internal pullup, set that.
        //	for (int i = 0; i < 8; i++) {
        //		if (ddr & (1 << i))
        //			avr_raise_irq(p->io.irq + i, (avr->data[p->r_port] >> i) & 1);
        //		else if (p->external.pull_mask & (1 << i))
        //			avr_raise_irq(p->io.irq + i, (p->external.pull_value >> i) & 1);
        //		else if ((avr->data[p->r_port] >> i) & 1)
        //			avr_raise_irq(p->io.irq + i, 1);
        //	}
        //	uint8_t pin = (avr->data[p->r_pin] & ~ddr) | (avr->data[p->r_port] & ddr);
        //	pin = (pin & ~p->external.pull_mask) | p->external.pull_value;
        //	avr_raise_irq(p->io.irq + IOPORT_IRQ_PIN_ALL, pin);

        //	// if IRQs are registered on the PORT register (for example, VCD dumps) send
        //	// those as well
        //	avr_io_addr_t port_io = AVR_DATA_TO_IO(p->r_port);
        //	if (avr->io[port_io].irq) {
        //		avr_raise_irq(avr->io[port_io].irq + AVR_IOMEM_IRQ_ALL, avr->data[p->r_port]);
        //		for (int i = 0; i < 8; i++)
        //			avr_raise_irq(avr->io[port_io].irq + i, (avr->data[p->r_port] >> i) & 1);
        // 	}
        }

        public static void Avr_ioport_write(Avr avr, uint addr,byte v,object param)
        {
        	Avr_ioport p = (Avr_ioport) param;

        	if (avr.data[addr] != v)
                Console.WriteLine("** PORT%c(%02x) = %02x\r\n", p.name, addr, v);
        	Sim_core.Avr_core_watch_write(avr, addr, v);
        	Sim_irq.Avr_raise_irq(p.io.irq[IOPORT_IRQ_REG_PORT], v);
            Avr_ioport_update_irqs(ref p);
        }

        ///*
        // * This is a reasonably new behaviour for the io-ports. Writing 1's to the PIN register
        // * toggles the PORT equivalent bit (regardless of direction
        // */
        public static void Avr_ioport_pin_write(Avr avr, uint addr,byte v,object param)
        {
        	Avr_ioport p = (Avr_ioport)param;
        	Avr_ioport_write(avr, p.r_port, (byte)(avr.data[p.r_port]^v), param);
        }

        ///*
        // * This is a the callback for the DDR register. There is nothing much to do here, apart
        // * from triggering an IRQ in case any 'client' code is interested in the information,
        // * and restoring all PIN bits marked as output to PORT values.
        // */
        public static void Avr_ioport_ddr_write(Avr avr,uint addr,byte v,object param)
        {
        	Avr_ioport p = (Avr_ioport)param;

        	if (avr.data[addr] != v)
                Console.WriteLine("** DDR{0:G}({1:X2}) = {2:X2}\r\n", p.name, addr, v);
        	Avr_raise_irq(p.io.irq + IOPORT_IRQ_DIRECTION_ALL, v);
        	Avr_core_watch_write(avr, addr, v);
        	Avr_ioport_update_irqs(p);
        }

        ///*
        // * this is our "main" pin change callback, it can be triggered by either the
        // * AVR code, or any external piece of code that see fit to do it.
        // * Either way, this will raise pin change interrupts, if needed
        // */
        public static void Avr_ioport_irq_notify(ref Avr_irq irq, uint value,object[] param)
        {
        //	avr_ioport_t * p = (avr_ioport_t *)param;
        //	avr_t * avr = p->io.avr;

        //	int output = value & AVR_IOPORT_OUTPUT;
        //	value &= 0xff;
        //	uint8_t mask = 1 << irq->irq;
        //		// set the real PIN bit. ddr doesn't matter here as it's masked when read.
        //	avr->data[p->r_pin] &= ~mask;
        //	if (value)
        //		avr->data[p->r_pin] |= mask;

        //	if (output)	// if the IRQ was marked as Output, also do the IO write
        //		avr_ioport_write(avr, p->r_port, (avr->data[p->r_port] & ~mask) | (value ? mask : 0), p);

        //	if (p->r_pcint) {
        //		// if the pcint bit is on, try to raise it
        //		int raisedata = avr->data[p->r_pcint];
        //		uint8_t uiRegMask = p->pcint.mask;
        //		int8_t iShift = p->pcint.shift;
        //		if (uiRegMask) // If mask is 0, do nothing (backwards compat)
        //			raisedata &= uiRegMask; // Mask off

        //		if (iShift>0) // Shift data if necessary for alignment.
        //			raisedata <<= iShift;
        //		else if (iShift<0)
        //			raisedata >>= -iShift;

        //		int raise = raisedata & mask;
        //		if (raise)
        //			avr_raise_interrupt(avr, &p->pcint);
        //	}
        }

        public static void Avr_ioport_reset(Avr_io port)
        {
            Avr_ioport p = new Avr_ioport();
            p.name = port.irq_names[0];
            p.io = port;

            for (int i = 0; i < IOPORT_IRQ_PIN_ALL; i++)
                Sim_irq.Avr_irq_register_notify(p.io.irq[i], Avr_ioport_irq_notify, new object[1] { p });
        }

        public static int Avr_ioport_ioctl(Avr_io port, uint ctl, object[] io_param)
        {
            //	avr_ioport_t * p = (avr_ioport_t *)port;
            //	avr_t * avr = p->io.avr;
            int res = -1;

            //	// all IOCTls require some sort of valid parameter, bail if not
            //	if (!io_param)
            //		return -1;

            //	switch(ctl) {
            //		case AVR_IOCTL_IOPORT_GETIRQ_REGBIT: {
            //			avr_ioport_getirq_t * r = (avr_ioport_getirq_t*)io_param;

            //			if (r->bit.reg == p->r_port || r->bit.reg == p->r_pin || r->bit.reg == p->r_ddr) {
            //				// it's us ! check the special case when the "all pins" irq is requested
            //				int o = 0;
            //				if (r->bit.mask == 0xff)
            //					r->irq[o++] = &p->io.irq[IOPORT_IRQ_PIN_ALL];
            //				else {
            //					// otherwise fill up the ones needed
            //					for (int bi = 0; bi < 8; bi++)
            //						if (r->bit.mask & (1 << bi))
            //							r->irq[o++] = &p->io.irq[r->bit.bit + bi];
            //				}
            //				if (o < 8)
            //					r->irq[o] = NULL;
            //				return o;
            //			}
            //		}	break;
            //		default: {
            //			/*
            //			 * Return the port state if the IOCTL matches us.
            //			 */
            //			if (ctl == AVR_IOCTL_IOPORT_GETSTATE(p->name)) {
            //				avr_ioport_state_t state = {
            //					.name = p->name,
            //					.port = avr->data[p->r_port],
            //					.ddr = avr->data[p->r_ddr],
            //					.pin = avr->data[p->r_pin],
            //				};
            //				if (io_param)
            //					*((avr_ioport_state_t*)io_param) = state;
            //				res = 0;
            //			}
            //			/*
            //			 * Set the default IRQ values when pin is set as input
            //			 */
            //			if (ctl == AVR_IOCTL_IOPORT_SET_EXTERNAL(p->name)) {
            //				avr_ioport_external_t * m = (avr_ioport_external_t*)io_param;
            //				p->external.pull_mask = m->mask;
            //				p->external.pull_value = m->value;
            //				res = 0;
            //			}
            //		}
            //	}

            return res;
        }


        public const int IOPORT_IRQ_PIN0 = 0;
        public const int IOPORT_IRQ_PIN1 = 1;
        public const int IOPORT_IRQ_PIN2 = 2;
        public const int IOPORT_IRQ_PIN3 = 3;
        public const int IOPORT_IRQ_PIN4 = 4;
        public const int IOPORT_IRQ_PIN5 = 5;
        public const int IOPORT_IRQ_PIN6 = 6;
        public const int IOPORT_IRQ_PIN7 = 7;
        public const int IOPORT_IRQ_PIN_ALL = 8;
        public const int IOPORT_IRQ_DIRECTION_ALL = 9;
        public const int IOPORT_IRQ_REG_PORT = 10;
        public const int IOPORT_IRQ_REG_PIN = 11;
        public const int IOPORT_IRQ_COUNT = 12;

        protected static Avr_io _io;

        public static string[] irq_names = new string[IOPORT_IRQ_COUNT];

        static Avr_ioports()
        {
            irq_names[IOPORT_IRQ_PIN0] = "=pin0";
            irq_names[IOPORT_IRQ_PIN1] = "=pin1";
            irq_names[IOPORT_IRQ_PIN2] = "=pin2";
            irq_names[IOPORT_IRQ_PIN3] = "=pin3";
            irq_names[IOPORT_IRQ_PIN4] = "=pin4";
            irq_names[IOPORT_IRQ_PIN5] = "=pin5";
            irq_names[IOPORT_IRQ_PIN6] = "=pin6";
            irq_names[IOPORT_IRQ_PIN7] = "=pin7";
            irq_names[IOPORT_IRQ_PIN_ALL] = "8=all";
            irq_names[IOPORT_IRQ_DIRECTION_ALL] = "8>ddr";
            irq_names[IOPORT_IRQ_REG_PORT] = "8>port";
            irq_names[IOPORT_IRQ_REG_PIN] = "8>pin";

            _io = new Avr_io();
            _io.kind = "port";
            _io.ioctl = Avr_ioport_ioctl;
            _io.irq_names = irq_names;
            _io.reset = Avr_ioport_reset;

    }




        public static void Avr_ioport_init(Avr avr, ref Avr_ioport p)
        {
        	if (p.r_port==0) {
                Console.WriteLine("skipping PORT%c for core %s\n", p.name, avr.mmcu);
        		return;
        	}
        	p.io = _io;
            Console.WriteLine("Avr_ioport_init PIN%c 0x%02x DDR%c 0x%02x PORT%c 0x%02x\n", 
        		p.name, p.r_pin,p.name, p.r_ddr,p.name, p.r_port);

        	Sim_io.Avr_register_io(avr, ref p.io);
        	Sim_interrupts.Avr_register_vector(avr, ref p.pcint);
        	// allocate this module's IRQ
        	Sim_io.Avr_io_setirqs(ref p.io, AVR_IOCTL_IOPORT_GETIRQ((byte)p.name[0]), IOPORT_IRQ_COUNT, null);

        	for (int i = 0; i < IOPORT_IRQ_COUNT; i++)
        		p.io.irq[i].flags |= Sim_irq.IRQ_FLAG_FILTERED;

        	Sim_io.Avr_register_io_write(avr, p.r_port, Avr_ioport_write, p);
            Sim_io.Avr_register_io_read(avr, p.r_pin, Avr_ioport_read, p);
            Sim_io.Avr_register_io_write(avr, p.r_pin, Avr_ioport_pin_write, p);
            Sim_io.Avr_register_io_write(avr, p.r_ddr, Avr_ioport_ddr_write, p);
        }

        //#define AVR_IOPORT_OUTPUT 0x100

        // add port name (uppercase) to get the real IRQ
        public static uint AVR_IOCTL_IOPORT_GETIRQ(byte _name)
        {
            return Sim_io.AVR_IOCTL_DEF((byte)'i',(byte) 'o',(byte) 'g', _name);
        }


        //// this ioctl takes a avr_regbit_t, compares the register address
        //// to PORT/PIN/DDR and return the corresponding IRQ(s) if it matches
        //typedef struct avr_ioport_getirq_t {
        //	avr_regbit_t bit;	// bit wanted
        //	avr_irq_t * irq[8];	// result, terminated by NULL if < 8
        //} avr_ioport_getirq_t;

        //#define AVR_IOCTL_IOPORT_GETIRQ_REGBIT AVR_IOCTL_DEF('i','o','g','r')

        ///*
        // * ioctl used to get a port state.
        // *
        // * for (int i = 'A'; i <= 'F'; i++) {
        // * 	avr_ioport_state_t state;
        // * 	if (avr_ioctl(AVR_IOCTL_IOPORT_GETSTATE(i), &state) == 0)
        // * 		printf("PORT%c %02x DDR %02x PIN %02x\n",
        // * 			state.name, state.port, state.ddr, state.pin);
        // * }
        // */
        //typedef struct avr_ioport_state_t {
        //	unsigned long name : 7,
        //		port : 8, ddr : 8, pin : 8;
        //} avr_ioport_state_t;

        //// add port name (uppercase) to get the port state
        //#define AVR_IOCTL_IOPORT_GETSTATE(_name) AVR_IOCTL_DEF('i','o','s',(_name))

        ///*
        // * ioctl used to set default port state when set as input.
        // *
        // */
        

        //// add port name (uppercase) to set default input pin IRQ values
        //#define AVR_IOCTL_IOPORT_SET_EXTERNAL(_name) AVR_IOCTL_DEF('i','o','p',(_name))

        ///**
        // * pin structure
        // */
        //typedef struct avr_iopin_t {
        //	uint16_t port : 8;			///< port e.g. 'B'
        //	uint16_t pin : 8;		///< pin number
        //} avr_iopin_t;
        //#define AVR_IOPIN(_port, _pin)	{ .port = _port, .pin = _pin }


        //void avr_ioport_init(avr_t * avr, avr_ioport_t * port);

        //#define AVR_IOPORT_DECLARE(_lname, _cname, _uname) \
        //	.port ## _lname = { \
        //		.name = _cname, .r_port = PORT ## _uname, .r_ddr = DDR ## _uname, .r_pin = PIN ## _uname, \
        //	}

        

    }
}