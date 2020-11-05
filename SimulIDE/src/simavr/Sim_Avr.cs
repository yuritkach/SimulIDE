using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SimulIDE.src.simavr.Sim_Avr;
using static SimulIDE.src.simavr.sim.Avr_vcd_file;

namespace SimulIDE.src.simavr
{

    public struct avr_regbit
    {
        UInt32 Flags;
        //UInt32 reg : 9, 
        //    bit : 3, 
        //    mask : 8;
    }

    public struct ResetFlags
    {
        avr_regbit porf;
        avr_regbit extrf;
        avr_regbit borf;
        avr_regbit wdrf;
    }

    public class Custom
    {
        // called at init time (for special purposes like using a
        // memory mapped file as flash see: simduino)
        public void Init(Avr avr, byte[] data) { }
        // called at termination time ( to clean special initializations)
        public void Deinit(Avr avr, byte[] data) { }
        // value passed to init() and deinit()
        byte[] data;
    }


    public delegate void Avr_run(Avr avr);

    public class Avr
    {
        public string mmcu;   // name of the AVR
                            // these are filled by sim_core_declare from constants in /usr/lib/avr/include/avr/io*.h
        public UInt16 ioend;
        public UInt16 ramend;
        public UInt32 flashend;
        public UInt32 e2end;
        public byte vector_size;
        public byte[] signature;
        public byte[] fuse;
        public byte lockbits;
        public UInt16 rampz;    // optional, only for ELPM/SPM on >64Kb cores
        public UInt16 eind; // optional, only for EIJMP/EICALL on >64Kb cores
        public byte address_size;   // 2, or 3 for cores >128KB in flash
        public ResetFlags reset_flags;

    	// filled by the ELF data, this allow tracking of invalid jumps
    	public UInt32 codeend;

        public CoreStates state;      // stopped, running, sleeping
        public UInt32 frequency; // frequency we are running at
                        // mostly used by the ADC for now
        public UInt32 vcc, avcc, aref; // (optional) voltages in millivolts

        // cycles gets incremented when sleeping and when running; it corresponds
        // not only to "cycles that runs" but also "cycles that might have run"
        // like, sleeping.
        public UInt64 cycle;		// current cycle
        public UInt64 cyclesDone;

        // these next two allow the core to freely run between cycle timers and also allows
        // for a maximum run cycle limit... run_cycle_count is set during cycle timer processing.
        public UInt64 run_cycle_count;  // cycles to run before next timer
        public UInt64 run_cycle_limit;  // maximum run cycle interval limit

        /**
	    * Sleep requests are accumulated in sleep_usec until the minimum sleep value
	    * is reached, at which point sleep_usec is cleared and the sleep request
	    * is passed on to the operating system.
	    */
        public UInt32 sleep_usec;

        // called at init time
        public void Init(Avr avr) { }
        // called at reset time
        public void Reset(Avr avr) { }
        
        public Custom custom;

    	/*!
	    * Default AVR core run function.
	    * Two modes are available, a "raw" run that goes as fast as
	    * it can, and a "gdb" mode that also watchouts for gdb events
	    * and is a little bit slower.
	    */
	    public Avr_run Run;

        /*!
        * Sleep default behaviour.
        * In "raw" mode, it calls usleep, in gdb mode, it waits
        * for howLong for gdb command on it's sockets.
        */
        public virtual void Sleep(Avr avr, UInt64 howLong) { }

	    /*!
	    * Every IRQs will be stored in this pool. It is not
	    * mandatory (yet) but will allow listing IRQs and their connections
	    */
//	    avr_irq_pool_t irq_pool;

        // Mirror of the SREG register, to facilitate the access to bits
        // in the opcode decoder.
        // This array is re-synthesized back/forth when SREG changes
        public byte[] sreg;

        /* Interrupt state:
            00: idle (no wait, no pending interrupts) or disabled
            <0: wait till zero
            >0: interrupt pending */
        public byte interrupt_state; // interrupt state

        /*
        * ** current PC **
        * Note that the PC is representing /bytes/ while the AVR value is
        * assumed to be "words". This is in line with what GDB does...
        * this is why you will see >>1 and <<1 in the decoder to handle jumps.
        * It CAN be a little confusing, so concentrate, young grasshopper.
        */
        public UInt32 PC;
        /*
        * Reset PC, this is the value used to jump to at reset time, this
        * allow support for bootloaders
        */
        public UInt32 reset_pc;

        /*
        * callback when specific IO registers are read/written.
        * There is one drawback here, there is in way of knowing what is the
        * "beginning of useful sram" on a core, so there is no way to deduce
        * what is the maximum IO register for a core, and thus, we can't
        * allocate this table dynamically.
        * If you wanted to emulate the BIG AVRs, and XMegas, this would need
        * work.
        */
//struct {

//        struct avr_irq_t * irq; // optional, used only if asked for with avr_iomem_getirq()
//        struct {

//            void* param;
//            avr_io_read_t c;
//		        } r;
//		struct {

//            void* param;
//            avr_io_write_t c;
//		        } w;
//	} io[MAX_IOs];

	/*
	 * This block allows sharing of the IO write/read on addresses between
	 * multiple callbacks. In 99% of case it's not needed, however on the tiny*
	 * (tiny85 at last) some registers have bits that are used by different
	 * IO modules.
	 * If this case is detected, a special "dispatch" callback is installed that
	 * will handle this particular case, without impacting the performance of the
	 * other, normal cases...
	 */
	public int io_shared_io_count;
//struct {

//        int used;
//struct {

//            void* param;
//void* c;
//		} io[4];
//	} io_shared_io[4];

	// flash memory (initialized to 0xff, and code loaded into it)
	public byte[] flash;
// this is the general purpose registers, IO registers, and SRAM
    public byte[] data;

    // queue of io modules
//    struct avr_io_t io_port;

// Builtin and user-defined commands
    public Avr_cmd_table commands;
// cycle timers tracking & delivery
//avr_cycle_timer_pool_t cycle_timers;
// interrupt vectors and delivery fifo
//avr_int_table_t interrupts;

// DEBUG ONLY -- value ignored if CONFIG_SIMAVR_TRACE = 0
        byte trace = 1,
			log = 4; // log level, default to 1

	// Only used if CONFIG_SIMAVR_TRACE is defined
//	struct avr_trace_data_t * trace_data;

// VALUE CHANGE DUMP file (waveforms)
// this is the VCD file that gets allocated if the
// firmware that is loaded explicitly asks for a trace
// to be generated, and allocates it's own symbols
// using AVR_MMCU_TAG_VCD_TRACE (see avr_mcu_section.h)
  public Avr_vcd vcd;

// gdb hooking structure. Only present when gdb server is active
//struct avr_gdb_t * gdb;

// if non-zero, the gdb server will be started when the core
// crashed even if not activated at startup
// if zero, the simulator will just exit() in case of a crash
int gdb_port;

// buffer for console debugging output from register
//struct {

//        char* buf;
//uint32_t size;
//uint32_t len;
//	} io_console_buffer;
 
    }

        public class Sim_Avr
    {

        //        static void std_logger(avr_t* avr,const int level, const char* format, va_list ap);
        //        static avr_logger_p _avr_global_logger = std_logger;

        //        void avr_global_logger(struct avr_t* avr, const int level, const char* format, ... )
        //        {

        //            va_list args;
        //            va_start(args, format);
        //	        if (_avr_global_logger)
        //                _avr_global_logger(avr, level, format, args);
        //            va_end(args);
        //        }

        //        void  avr_global_logger_set(avr_logger_p logger)
        //        {
        //            _avr_global_logger = logger ? logger : std_logger;
        //        }

        //        avr_logger_p avr_global_logger_get(void)
        //        {
        //            return _avr_global_logger;
        //        }


        // initializes a new AVR instance. Will call the IO registers init(), and then reset()
        public static int  Avr_init(Avr avr)
        {
            avr.flash = new byte[avr.flashend + 1];

            Utils.MemSet(ref avr.flash, 0xff, (int) avr.flashend + 1);
            avr.codeend = avr.flashend;
            avr.data = new byte[avr.ramend + 1];
            Utils.MemSet(ref avr.data, 0, avr.ramend + 1);
        //            # ifdef CONFIG_SIMAVR_TRACE
        //                avr->trace_data = calloc(1, sizeof(struct avr_trace_data_t));
        //            #endif

        //	        AVR_LOG(avr, LOG_TRACE, "%s init\n", avr->mmcu);

        // cpu is in limbo before init is finished.
        avr.state = CoreStates.cpu_Limbo;
        avr.frequency = 1000000;	// can be overridden via avr_mcu_section
        Sim_Cmds.Avr_cmd_init(ref avr);
        //Avr_interrupt_init(ref avr);
        
        //	        if (avr->custom.init)
        //		        avr->custom.init(avr, avr->custom.data);
        //	        if (avr->init)
        //		        avr->init(avr);
        //            // set default (non gdb) fast callbacks
        //            avr->run = avr_callback_run_raw;
        //	        avr->sleep = avr_callback_sleep_raw;
        //	        // number of address bytes to push/pull on/off the stack
        //	        avr->address_size = avr->eind? 3 : 2;
        //	        avr->log = 1;
        //	        avr_reset(avr);
        //            avr_regbit_set(avr, avr->reset_flags.porf);		// by  default set to power-on reset
        	        return 0;
        }

        //        void avr_terminate( avr_t* avr)
        //        {
        //            if (avr->custom.deinit)
        //                avr->custom.deinit(avr, avr->custom.data);
        //            if (avr->gdb)
        //            {  
        //                avr_deinit_gdb(avr);
        //                avr->gdb = NULL;
        //            }
        //            if (avr->vcd)
        //            {
        //                avr_vcd_close(avr->vcd);
        //                avr->vcd = NULL;
        //            }
        //            avr_deallocate_ios(avr);

        //            if (avr->flash) free(avr->flash);
        //            if (avr->data) free(avr->data);
        //            if (avr->io_console_buffer.buf)
        //            {
        //                avr->io_console_buffer.len = 0;
        //                avr->io_console_buffer.size = 0;
        //                free(avr->io_console_buffer.buf);
        //                avr->io_console_buffer.buf = NULL;
        //            }
        //            avr->flash = avr->data = NULL;
        //        }

        //        void avr_reset(avr_t* avr)
        //        {
        //            AVR_LOG(avr, LOG_TRACE, "%s reset\n", avr->mmcu);

        //            avr->cyclesDone = 0;
        //            avr->state = cpu_Running;
        //            for (int i = 0x20; i <= avr->ioend; i++)
        //                avr->data[i] = 0;
        //            _avr_sp_set(avr, avr->ramend);
        //            avr->pc = avr->reset_pc;	// Likely to be zero
        //            for (int i = 0; i < 8; i++) avr->sreg[i] = 0;

        //            avr_interrupt_reset(avr);
        //            avr_cycle_timer_reset(avr);
        //            if (avr->reset) avr->reset(avr);

        //            avr_io_t* port = avr->io_port;
        //            while (port)
        //            {
        //                if (port->reset) port->reset(port);
        //                port = port->next;
        //            }
        //        }

        //        void avr_sadly_crashed(avr_t* avr, uint8_t signal)
        //        {
        //            AVR_LOG(avr, LOG_ERROR, "%s\n", __FUNCTION__);
        //            avr->state = cpu_Stopped;
        //            if (avr->gdb_port)
        //            {
        //                // enable gdb server, and wait
        //                if (!avr->gdb) avr_gdb_init(avr);
        //            }
        //            if (!avr->gdb) avr->state = cpu_Crashed;
        //        }

        //        void avr_set_command_register(avr_t* avr, avr_io_addr_t addr)
        //        {
        //            avr_cmd_set_register(avr, addr);
        //        }

        //        static void _avr_io_console_write(struct avr_t * avr, avr_io_addr_t addr, uint8_t v,  void* param)
        //        {
        //            if (v == '\r' && avr->io_console_buffer.buf)
        //            {
        //		        avr->io_console_buffer.buf[avr->io_console_buffer.len] = 0;
        //		        AVR_LOG(avr, LOG_OUTPUT, "O:" "%s" "" "\n", avr->io_console_buffer.buf);
        //                avr->io_console_buffer.len = 0;
        //		        return;
        //	        }
        //            if (avr->io_console_buffer.len + 1 >= avr->io_console_buffer.size)
        //            {
        //		        avr->io_console_buffer.size += 128;
        //		        avr->io_console_buffer.buf = (char*) realloc(
        //                    avr->io_console_buffer.buf,
        //                    avr->io_console_buffer.size);
        //	        }
        //            if (v >= ' ') avr->io_console_buffer.buf[avr->io_console_buffer.len++] = v;
        //        }

        //        void avr_set_console_register(avr_t* avr, avr_io_addr_t addr)
        //        {
        //            if (addr)
        //                avr_register_io_write(avr, addr, _avr_io_console_write, NULL);
        //        }

        //        int avr_loadcode(avr_t* avr,uint8_t* code, uint32_t size, avr_flashaddr_t address)
        //        {
        //            if ((address + size) > avr->flashend + 1)
        //            {
        //                AVR_LOG(avr, LOG_ERROR, "avr_loadcode(): Attempted to load code of size %d but flash size is only %d.\n",
        //                    size, avr->flashend + 1);
        //                return -1;
        //            }
        //            memcpy(avr->flash + address, code, size);
        //            return 0;
        //        }

        //        /**
        //        * Accumulates sleep requests (and returns a sleep time of 0) until
        //        * a minimum count of requested sleep microseconds are reached
        //        * (low amounts cannot be handled accurately).
        //        */
        //        uint32_t avr_pending_sleep_usec(avr_t* avr, avr_cycle_count_t howLong)
        //        {
        //            avr->sleep_usec += avr_cycles_to_usec(avr, howLong);
        //            uint32_t usec = avr->sleep_usec;
        //            if (usec > 200)
        //            {
        //                avr->sleep_usec = 0;
        //                return usec;
        //            }
        //            return 0;
        //        }

        //        void avr_callback_sleep_gdb(avr_t* avr, avr_cycle_count_t howLong)
        //        {
        //            uint32_t usec = avr_pending_sleep_usec(avr, howLong);
        //            while (avr_gdb_processor(avr, usec)) ;
        //        }

        //        void avr_callback_run_gdb(avr_t* avr)
        //        {
        //            avr_gdb_processor(avr, avr->state == cpu_Stopped);

        //            if (avr->state == cpu_Stopped) return;

        //            // if we are stepping one instruction, we "run" for one..
        //            int step = avr->state == cpu_Step;
        //            if (step) avr->state = cpu_Running;

        //            avr_flashaddr_t new_pc = avr->pc;

        //            if (avr->state == cpu_Running)
        //            {
        //                new_pc = avr_run_one(avr);
        //            #if CONFIG_SIMAVR_TRACE
        //		        avr_dump_state(avr);
        //            #endif
        //            }

        //            // run the cycle timers, get the suggested sleep time
        //            // until the next timer is due
        //            avr_cycle_count_t sleep = avr_cycle_timer_process(avr);

        //            avr->pc = new_pc;

        //            if (avr->state == cpu_Sleeping)
        //            {
        //                if (!avr->sreg[S_I])
        //                {
        //                    if (avr->log) AVR_LOG(avr, LOG_TRACE, "simavr: sleeping with interrupts off, quitting gracefully\n");
        //                    avr->state = cpu_Done;
        //                    return;
        //                }
        //                /*
        //		        * try to sleep for as long as we can (?)
        //		        */
        //                avr->sleep(avr, sleep);
        //                avr->cycle += 1 + sleep;
        //            }
        //            // Interrupt servicing might change the PC too, during 'sleep'
        //            if (avr->state == cpu_Running || avr->state == cpu_Sleeping) avr_service_interrupts(avr);

        //            // if we were stepping, use this state to inform remote gdb
        //            if (step) avr->state = cpu_StepDone;
        //        }

        //        void avr_callback_sleep_raw(avr_t* avr, avr_cycle_count_t howLong)
        //        {
        //            //uint32_t usec = avr_pending_sleep_usec(avr, howLong);
        //            //if (usec > 0) usleep(usec);
        //        }

        //        void avr_callback_run_raw(avr_t* avr)
        //        {
        //            if (avr->state == cpu_Done) return;

        //            if (avr->state == cpu_Running)
        //            {
        //                if (avr->cyclesDone > 1) avr->cyclesDone -= 1;
        //                else avr->pc = avr_run_one(avr);
        //                avr->cycle += 1;
        //            }

        //            // run the cycle timers, get the suggested sleep time until the next timer is due
        //            //avr_cycle_count_t sleep =
        //            avr_cycle_timer_process(avr);

        //            if (avr->state == cpu_Sleeping)
        //            {
        //                if (!avr->sreg[S_I])
        //                {
        //                    if (avr->log) AVR_LOG(avr, LOG_TRACE, "simavr: sleeping with interrupts off, quitting gracefully\n");

        //                    avr->state = cpu_Done;
        //                    return;
        //                }

        //                //avr->sleep(avr, sleep); //try to sleep for as long as we can( ?)
        //                avr->cycle += 1; // + sleep;
        //            }

        //            if (avr->state == cpu_Running || avr->state == cpu_Sleeping) // Interrupts might change the PC too, during 'sleep'
        //            {
        //                /* Note: checking interrupt_state here is completely superfluous, however
        //                    as interrupt_state tells us all we really need to know, here
        //                    a simple check here may be cheaper than a call not needed. */
        //                if (avr->interrupt_state) avr_service_interrupts(avr);
        //            }
        //        }

        //        int avr_run(avr_t* avr)
        //        {
        //            avr->run(avr);
        //            return avr->state;
        //        }

        //        avr_t* avr_core_allocate( const avr_t* core, uint32_t coreLen)
        //        {
        //            uint8_t* b = malloc(coreLen);
        //            memcpy(b, core, coreLen);
        //            return (avr_t*)b;
        //        }

        public static Avr Avr_make_mcu_by_name(string name)
        {
            Avr_kind maker = null;
            for (int i = 0; Sim_core_decl.avr_kind[i]!=null && maker!=null; i++)
            {
                for (int j = 0; j<Sim_core_decl.avr_kind[i].Names[j].Length; j++)
                    if (Sim_core_decl.avr_kind[i].Names[j]==name)
                    {
                        maker = Sim_core_decl.avr_kind[i];
                        break;
                    }
            }
            if (maker!=null)
            {
                MessageBox.Show(" AVR "+name+" not known\n");
                return null;
            }
            Avr avr = maker.Make();
            return avr;
        }

//        static void std_logger(avr_t* avr, const int level, const char* format, va_list ap)
//        {
//            if (!avr || avr->log >= level)
//            {
//                vfprintf((level > LOG_ERROR) ? stdout : stderr, format, ap);
//            }
//        }



//typedef uint32_t avr_flashaddr_t;

//struct avr_t;
//        typedef uint8_t(*avr_io_read_t)(
//        struct avr_t* avr,
//		avr_io_addr_t addr,
//        void* param);

//        typedef void (* avr_io_write_t) (
//        struct avr_t* avr,
//		avr_io_addr_t addr,
//        uint8_t v,
//        void* param);

//        enum {
//            S_C = 0, S_Z, S_N, S_V, S_S, S_H, S_T, S_I,      // SREG bit indexes
//            R_XL = 0x1a, R_XH, R_YL, R_YH, R_ZL, R_ZH, // 16 bits register pairs
//            R_SPL = 32 + 0x3d, R_SPH,                 // stack pointer
//            R_SREG = 32 + 0x3f,                        // real SREG

//            // maximum number of IO registers, on normal AVRs
//            MAX_IOs = 280,  // Bigger AVRs need more than 256-32 (mega1280)
//        };

//#define AVR_DATA_TO_IO(v) ((v) - 32)
//#define AVR_IO_TO_DATA(v) ((v) + 32)

//        /**
//         * Logging macros and associated log levels.
//         * The current log level is kept in avr->log.
//         */
//        enum {
//            LOG_NONE = 0,
//            LOG_OUTPUT,
//            LOG_ERROR,
//            LOG_WARNING,
//            LOG_TRACE,
//            LOG_DEBUG,
//        };


//#ifndef AVR_LOG
//#define AVR_LOG(avr, level, ...) \
//	do { \
//		avr_global_logger(avr, level, __VA_ARGS__); \
//	} while(0)
//#endif
//#define AVR_TRACE(avr, ... ) \
//	AVR_LOG(avr, LOG_TRACE, __VA_ARGS__)

///*
// * Core states.
// */
public enum CoreStates {
        cpu_Limbo = 0,  // before initialization is finished
        cpu_Stopped,    // all is stopped, timers included
        cpu_Running,    // we're free running
        cpu_Sleeping,   // we're now sleeping until an interrupt
        cpu_Step,       // run ONE instruction, then...
        cpu_StepDone,   // tell gdb it's all OK, and give it registers
        cpu_Done,       // avr software stopped gracefully
        cpu_Crashed,    // avr software crashed (watchdog fired)
    };

//    // this is only ever used if CONFIG_SIMAVR_TRACE is defined
//    struct avr_trace_data_t
//    {
//        struct avr_symbol_t ** codeline;

//	/* DEBUG ONLY
//	 * this keeps track of "jumps" ie, call,jmp,ret,reti and so on
//	 * allows dumping of a meaningful data even if the stack is
//	 * munched and so on
//	 */
//	#define OLD_PC_SIZE	32
//	struct {

//        uint32_t pc;
//        uint16_t sp;
//    }
//    old[OLD_PC_SIZE]; // catches reset..
//	int old_pci;

//#if AVR_STACK_WATCH
//#define STACK_FRAME_SIZE	32
//	// this records the call/ret pairs, to try to catch
//	// code that munches the stack -under- their own frame
//	struct {
//		uint32_t	pc;
//		uint16_t 	sp;
//	} stack_frame[STACK_FRAME_SIZE];
//	int			stack_frame_index;
//#endif

//    // DEBUG ONLY
//    // keeps track of which registers gets touched by instructions
//    // reset before each new instructions. Allows meaningful traces
//    uint32_t touched[256 / 32];	// debug
//};

//typedef void (* avr_run_t) ( struct avr_t* avr);

//#define AVR_FUSE_LOW	0
//#define AVR_FUSE_HIGH	1
//#define AVR_FUSE_EXT	2

/*
 * Main AVR instance. Some of these fields are set by the AVR "Core" definition files
 * the rest is runtime data (as little as possible)
 */

//reset_flags;

//	// filled by the ELF data, this allow tracking of invalid jumps
//	uint32_t codeend;

//int state;      // stopped, running, sleeping
//uint32_t frequency; // frequency we are running at
//                    // mostly used by the ADC for now
//uint32_t vcc, avcc, aref; // (optional) voltages in millivolts

//// cycles gets incremented when sleeping and when running; it corresponds
//// not only to "cycles that runs" but also "cycles that might have run"
//// like, sleeping.
//avr_cycle_count_t cycle;        // current cycle
//uint64_t cyclesDone;

//// these next two allow the core to freely run between cycle timers and also allows
//// for a maximum run cycle limit... run_cycle_count is set during cycle timer processing.
//avr_cycle_count_t run_cycle_count;  // cycles to run before next timer
//avr_cycle_count_t run_cycle_limit;  // maximum run cycle interval limit

///**
// * Sleep requests are accumulated in sleep_usec until the minimum sleep value
// * is reached, at which point sleep_usec is cleared and the sleep request
// * is passed on to the operating system.
// */
//uint32_t sleep_usec;

//// called at init time
//void (* init) (struct avr_t * avr);
//	// called at reset time
//	void (* reset) (struct avr_t * avr);

//	struct {
//        // called at init time (for special purposes like using a
//        // memory mapped file as flash see: simduino)
//void (* init) (struct avr_t * avr, void* data);
//		// called at termination time ( to clean special initializations)
//		void (* deinit) (struct avr_t * avr, void* data);
//		// value passed to init() and deinit()
//		void* data;
//	} custom;

//	/*!
//	 * Default AVR core run function.
//	 * Two modes are available, a "raw" run that goes as fast as
//	 * it can, and a "gdb" mode that also watchouts for gdb events
//	 * and is a little bit slower.
//	 */
//	avr_run_t run;

///*!
// * Sleep default behaviour.
// * In "raw" mode, it calls usleep, in gdb mode, it waits
// * for howLong for gdb command on it's sockets.
// */
//void (* sleep) (struct avr_t * avr, avr_cycle_count_t howLong);

//	/*!
//	 * Every IRQs will be stored in this pool. It is not
//	 * mandatory (yet) but will allow listing IRQs and their connections
//	 */
//	avr_irq_pool_t irq_pool;

//// Mirror of the SREG register, to facilitate the access to bits
//// in the opcode decoder.
//// This array is re-synthesized back/forth when SREG changes
//uint8_t sreg[8];

///* Interrupt state:
//    00: idle (no wait, no pending interrupts) or disabled
//    <0: wait till zero
//    >0: interrupt pending */
//int8_t interrupt_state; // interrupt state

///*
// * ** current PC **
// * Note that the PC is representing /bytes/ while the AVR value is
// * assumed to be "words". This is in line with what GDB does...
// * this is why you will see >>1 and <<1 in the decoder to handle jumps.
// * It CAN be a little confusing, so concentrate, young grasshopper.
// */
//avr_flashaddr_t pc;
///*
// * Reset PC, this is the value used to jump to at reset time, this
// * allow support for bootloaders
// */
//avr_flashaddr_t reset_pc;

///*
// * callback when specific IO registers are read/written.
// * There is one drawback here, there is in way of knowing what is the
// * "beginning of useful sram" on a core, so there is no way to deduce
// * what is the maximum IO register for a core, and thus, we can't
// * allocate this table dynamically.
// * If you wanted to emulate the BIG AVRs, and XMegas, this would need
// * work.
// */
//struct {

//        struct avr_irq_t * irq; // optional, used only if asked for with avr_iomem_getirq()
//struct {

//            void* param;
//avr_io_read_t c;
//		} r;
//		struct {

//            void* param;
//avr_io_write_t c;
//		} w;
//	} io[MAX_IOs];

//	/*
//	 * This block allows sharing of the IO write/read on addresses between
//	 * multiple callbacks. In 99% of case it's not needed, however on the tiny*
//	 * (tiny85 at last) some registers have bits that are used by different
//	 * IO modules.
//	 * If this case is detected, a special "dispatch" callback is installed that
//	 * will handle this particular case, without impacting the performance of the
//	 * other, normal cases...
//	 */
//	int io_shared_io_count;
//struct {

//        int used;
//struct {

//            void* param;
//void* c;
//		} io[4];
//	} io_shared_io[4];

//	// flash memory (initialized to 0xff, and code loaded into it)
//	uint8_t* flash;
//// this is the general purpose registers, IO registers, and SRAM
//uint8_t* data;

//// queue of io modules
//struct avr_io_t * io_port;

//// Builtin and user-defined commands
//avr_cmd_table_t commands;
//// cycle timers tracking & delivery
//avr_cycle_timer_pool_t cycle_timers;
//// interrupt vectors and delivery fifo
//avr_int_table_t interrupts;

//// DEBUG ONLY -- value ignored if CONFIG_SIMAVR_TRACE = 0
//uint8_t trace : 1,
//			log : 4; // log level, default to 1

//	// Only used if CONFIG_SIMAVR_TRACE is defined
//	struct avr_trace_data_t * trace_data;

//// VALUE CHANGE DUMP file (waveforms)
//// this is the VCD file that gets allocated if the
//// firmware that is loaded explicitly asks for a trace
//// to be generated, and allocates it's own symbols
//// using AVR_MMCU_TAG_VCD_TRACE (see avr_mcu_section.h)
//struct avr_vcd_t * vcd;

//// gdb hooking structure. Only present when gdb server is active
//struct avr_gdb_t * gdb;

//// if non-zero, the gdb server will be started when the core
//// crashed even if not activated at startup
//// if zero, the simulator will just exit() in case of a crash
//int gdb_port;

//// buffer for console debugging output from register
//struct {

//        char* buf;
//uint32_t size;
//uint32_t len;
//	} io_console_buffer;
//} avr_t;


//// this is a static constructor for each of the AVR devices

//// a symbol loaded from the .elf file
//typedef struct avr_symbol_t
//{
//    uint32_t addr;
//    const char symbol[0];
//}
//avr_symbol_t;

//// locate the maker for mcu "name" and allocates a new avr instance
//avr_t*
//avr_make_mcu_by_name(
//		const char* name);
//// initializes a new AVR instance. Will call the IO registers init(), and then reset()
//int
//avr_init(
//        avr_t* avr);
//// Used by the cores, allocated a mutable avr_t from the const global
//avr_t*
//avr_core_allocate(
//		const avr_t* core,
//        uint32_t coreLen);

//// resets the AVR, and the IO modules
//void avr_reset(avr_t* avr);
//// run one cycle of the AVR, sleep if necessary
//int avr_run(avr_t* avr);
//// finish any pending operations
//void avr_terminate(avr_t* avr);

//// set an IO register to receive commands from the AVR firmware
//// it's optional, and uses the ELF tags
//void avr_set_command_register(avr_t* avr, avr_io_addr_t addr);

//// specify the "console register" -- output sent to this register
//// is printed on the simulator console, without using a UART
//void avr_set_console_register(avr_t* avr, avr_io_addr_t addr);

//// load code in the "flash"
//int avr_loadcode(avr_t* avr, uint8_t* code, uint32_t size, avr_flashaddr_t address);

///*
// * These are accessors for avr->data but allows watchpoints to be set for gdb
// * IO modules use that to set values to registers, and the AVR core decoder uses
// * that to register "public" read by instructions.
// */
//void avr_core_watch_write(avr_t* avr, uint16_t addr, uint8_t v);
//uint8_t avr_core_watch_read(avr_t* avr, uint16_t addr);

//// called when the core has detected a crash somehow.
//// this might activate gdb server
//void
//avr_sadly_crashed(avr_t* avr, uint8_t signal);

///*
// * Logs a message using the current logger
// */
//void
//avr_global_logger(

//        struct avr_t* avr,

//        const int level,

//        const char* format,
//		... );

//#ifndef AVR_CORE
//#include <stdarg.h>
///*
// * Type for custom logging functions
// */
//typedef void (* avr_logger_p) (struct avr_t* avr, const int level, const char* format, va_list ap);

///* Sets a global logging function in place of the default */
//void avr_global_logger_set(avr_logger_p logger);
///* Gets the current global logger function */
//avr_logger_p avr_global_logger_get(void);
//#endif

///*
// * These are callbacks for the two 'main' behaviour in simavr
// */
//void avr_callback_sleep_gdb(avr_t* avr, avr_cycle_count_t howLong);
//void avr_callback_run_gdb(avr_t* avr);
//void avr_callback_sleep_raw(avr_t* avr, avr_cycle_count_t howLong);
//void avr_callback_run_raw(avr_t* avr);

///**
// * Accumulates sleep requests (and returns a sleep time of 0) until
// * a minimum count of requested sleep microseconds are reached
// * (low amounts cannot be handled accurately).
// * This function is an utility function for the sleep callbacks
// */
//uint32_t
//avr_pending_sleep_usec(avr_t* avr, avr_cycle_count_t howLong);

//#ifdef __cplusplus
//};
//#endif

//#include "sim_io.h"
//#include "sim_regbit.h"

//#ifdef __GNUC__

//# ifndef likely
//#  define likely(x) __builtin_expect(!!(x), 1)
//# endif

//# ifndef unlikely
//#  define unlikely(x) __builtin_expect(!!(x), 0)
//# endif

//#else /* ! __GNUC__ */

//# ifndef likely
//#  define likely(x) x
//# endif

//# ifndef unlikely
//#  define unlikely(x) x
//# endif

//#endif /* __GNUC__ */

//#endif /*__SIM_AVR_H__*/


    }

    public delegate Avr Make_delegate();

    public class Avr_kind
    {
        public string[] Names;   // name aliases
        public Make_delegate Make;
    }

}
