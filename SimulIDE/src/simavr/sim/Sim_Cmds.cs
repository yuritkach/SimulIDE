﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulIDE.src.simavr.sim.avr;

namespace SimulIDE.src.simavr.sim
{
    public delegate int avr_cmd_handler(ref Avr avr,byte v, object[] param);

    public class Avr_cmd
    {
        public avr_cmd_handler handler;
        public object[] param;
    }

    public class  Avr_cmd_table
    {
        public Avr_cmd[] table;
        public Avr_cmd[] pending; // Holds a reference to a pending multi-byte command
    }

    class Sim_Cmds
    {
        static void _avr_cmd_io_write( avr_t* avr, avr_io_addr_t addr, uint8_t v, void* param)
        {
            avr_cmd_table_t* commands = &avr->commands;
            avr_cmd_t* command = commands->pending;

            if (!command)
            {
                if (v > MAX_AVR_COMMANDS)
                {
                    AVR_LOG(avr, LOG_ERROR, LOG_PREFIX
        
                        "%s: code 0x%02x outside permissible range (>0x%02x)\n",
                        __FUNCTION__, v, MAX_AVR_COMMANDS - 1);
                    return;
                }
                command = &commands->table[v];
            }
            if (!command->handler)
            {
                AVR_LOG(avr, LOG_ERROR, LOG_PREFIX
        
                    "%s: code 0x%02x has no handler (wrong MMCU config)\n",
                    __FUNCTION__, v);
                return;
            }

            if (command)
            {
                if (command->handler(avr, v, command->param))
                    commands->pending = command;
                else
                    commands->pending = NULL;
            }
            else
                AVR_LOG(avr, LOG_TRACE, LOG_PREFIX "%s: unknown command 0x%02x\n",
                    __FUNCTION__, v);
        }


        void avr_cmd_set_register( avr_t* avr, avr_io_addr_t addr)
        {
            if (addr) avr_register_io_write(avr, addr, &_avr_cmd_io_write, NULL);
        }

        public static void Avr_cmd_register( ref Avr avr, SIMAVR_CMDS code, avr_cmd_handler handler, object[] param)
        {
            Avr_cmd_table commands = avr.commands;
            Avr_cmd command;

            if (handler==null) return;

            if ((int)code > MAX_AVR_COMMANDS)
                throw new Exception("Code " + code.ToString() + " (" + ((int)code).ToString() + ") outside permissible range)\n");

            command = commands.table[(int)code];
            if (command.handler!=null)
                throw new Exception("Code " + code.ToString() + " (" + ((int)code).ToString() + ") already registered)\n");

            command.handler = handler;
            command.param = param;
        }

        void avr_cmd_unregister(ref Avr avr, SIMAVR_CMDS code)
        {
            avr_cmd_table_t* commands = &avr->commands;
            avr_cmd_t* command;

            if (code > MAX_AVR_COMMANDS)
            {
                AVR_LOG(avr, LOG_ERROR, LOG_PREFIX
        
                    "%s: code 0x%02x outside permissible range (>0x%02x)\n",
                    __FUNCTION__, code, MAX_AVR_COMMANDS - 1);
                return;
            }

            command = &commands->table[code];
            if (command->handler)
            {
                if (command->param)
                    free(command->param);

                command->handler = NULL;
                command->param = NULL;
            }
            else
                AVR_LOG(avr, LOG_ERROR, LOG_PREFIX
        
                    "%s: no command registered for code 0x%02x\n",
                    __FUNCTION__, code);
        }

        public static int _simavr_cmd_vcd_start_trace( Avr avr, byte v, object[] param)
        {
            if (avr.vcd!=null)
                avr_vcd_start(avr.vcd);
            return 0;
        }

        static int _simavr_cmd_vcd_stop_trace( avr_t* avr, uint8_t v, void* param)
        {
            if (avr->vcd)
                avr_vcd_stop(avr->vcd);

            return 0;
        }

        static int _simavr_cmd_uart_loopback( avr_t* avr, uint8_t v, void* param)
        {
            avr_irq_t* src = avr_io_getirq(avr, AVR_IOCTL_UART_GETIRQ('0'), UART_IRQ_OUTPUT);
            avr_irq_t* dst = avr_io_getirq(avr, AVR_IOCTL_UART_GETIRQ('0'), UART_IRQ_INPUT);

            if (src && dst)
            {
                AVR_LOG(avr, LOG_TRACE, LOG_PREFIX
        
                    "%s: activating uart local echo; IRQ src %p dst %p\n",
                    __FUNCTION__, src, dst);
                avr_connect_irq(src, dst);
            }

            return 0;
        }

        public static void Avr_cmd_init(ref Avr avr)
        {
            avr.commands.table = new Avr_cmd[MAX_AVR_COMMANDS];
            avr.commands.pending = new Avr_cmd[0];
            // Register builtin commands
            Avr_cmd_register(ref avr, SIMAVR_CMDS.SIMAVR_CMD_VCD_START_TRACE, _simavr_cmd_vcd_start_trace, null);
            Avr_cmd_register(ref avr, SIMAVR_CMDS.SIMAVR_CMD_VCD_STOP_TRACE, _simavr_cmd_vcd_stop_trace, null);
            Avr_cmd_register(ref avr, SIMAVR_CMDS.SIMAVR_CMD_UART_LOOPBACK, _simavr_cmd_uart_loopback, null);
        }

        public const int MAX_AVR_COMMANDS = 32;







// Called by avr_set_command_register()
void
avr_cmd_set_register(

        struct avr_t * avr,
		avr_io_addr_t addr);

        /*
         * Register a command distinguished by 'code'.
         *
         * When 'code' is written to the configured IO address, 'handler' is executed
         * with the value written, as well as 'param'.
         * 'handler' can return non-zero, to indicate, that this is a multi-byte command.
         * Subsequent writes are then dispatched to the same handler, until 0 is returned.
         */
        void
        avr_cmd_register(

        struct avr_t * avr,
		uint8_t code,
        avr_cmd_handler_t handler,
		void* param);

        void
        avr_cmd_unregister(

        struct avr_t * avr,
		uint8_t code);

        // Private functions

        // Called from avr_init() to initialize the avr_cmd_table_t and register builtin commands.
        void avr_cmd_init(struct avr_t * avr);


    }
}
