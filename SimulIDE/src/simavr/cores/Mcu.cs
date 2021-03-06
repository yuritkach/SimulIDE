﻿using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimulIDE.src.simavr.cores
{
    public class Mcu
    {
        public Avr core;
        public Avr_eeprom eeprom;
        public Avr_flash selfprog;
      //  public Avr_watchdog watchdog;
      //  public Avr_extint extint;
        public Avr_ioport portb, portc, portd;
      //  public Avr_uart uart;
      //  public Avr_acomp acomp;
      //  public Avr_adc adc;
        public Avr_timer timer0, timer1, timer2;
     //   public Avr_spi_t spi;
     //   public Avr_twi_t twi;
        // PORTA exists on m16 and 32, but not on 8. 
        // It is still necessary to declare this as otherwise
        // the core_megax shared constructor will be confused
        public Avr_ioport porta;
    }
}
