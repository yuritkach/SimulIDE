#define _GNU_SOURCE
#include <pthread.h>

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#include "avr_ioport.h"
#include "sim_avr.h"
#include "sim_elf.h"
#include "sim_gdb.h"
#include "sim_vcd_file.h"

avr_t *avr = NULL;
avr_vcd_t vcd_file;

/*
 * called when the AVR change any of the pins on port B
 */
void
pin_changed_hook (struct avr_irq_t *irq, uint32_t value, void *param)
{
  printf("%u\n", value);
}

static void *
avr_run_thread (void *oaram)
{
  avr_cycle_count_t cycles = 0;
  while (cycles < 10000)
    {
      int state = avr_run (avr);
      if (state == cpu_Done || state == cpu_Crashed)
        break;
      printf("AVR cycle: %u\n", avr->cycle);
      cycles++;
    }
  printf("AVR Terminate\n");
  avr_terminate (avr);
  
  return NULL;
}

int
main (int argc, char *argv[])
{
  elf_firmware_t firmware;
  const char *firmware_path = "../../../firmwares/fade-led.elf";
  int rc = elf_read_firmware (firmware_path, &firmware);
  if (rc == -1)
    exit (1);
  printf ("firmware %s f=%d mmcu=%s\n", firmware_path, (int) firmware.frequency, firmware.mmcu);

  firmware.frequency = 16e6;
  avr = avr_make_mcu_by_name ("atmega2560"); // firmware.mmcu
  if (!avr)
    {
      fprintf (stderr, "%s: AVR '%s' not known\n", argv[0], firmware.mmcu);
      exit (1);
    }
  avr_init (avr);
  avr_load_firmware (avr, &firmware);

  // Fixme: UART output ?
  avr->log = LOG_TRACE;
  avr->trace = 1;
  
  // connect all the pins on port B to our callback
  // Fixme: only PWM13
  for (int i = 0; i < 8; i++)
    avr_irq_register_notify (avr_io_getirq (avr, AVR_IOCTL_IOPORT_GETIRQ ('B'), i),
                             pin_changed_hook, NULL);

  /*
   * VCD file initialization
   * 
   * This will allow you to create a "wave" file and display it in gtkwave Pressing "r" and "s"
   * during the demo will start and stop recording the pin changes
   */
  avr_vcd_init (avr, "gtkwave_output.vcd", &vcd_file, 1000000); // us
  avr_vcd_add_signal (&vcd_file,
                      avr_io_getirq (avr, AVR_IOCTL_IOPORT_GETIRQ ('B'), IOPORT_IRQ_PIN_ALL),
                      8, // bits
                      "portb");

  printf ("   Press 'q' to quit\n\n"
          "   Press 'r' to start recording a 'wave' file\n"
	  "   Press 's' to stop recording\n");

  // the AVR run on it's own thread. it even allows for debugging!
  pthread_t run;
  pthread_create (&run, NULL, avr_run_thread, NULL);
  
  void *retval = NULL;
  while (pthread_tryjoin_np (run, &retval))
    {
      switch (getchar())
	{
	case 'q':
	  exit(0);
	  break;
	case 'r':
	  printf ("Starting VCD trace\n");
	  avr_vcd_start (&vcd_file);
	  break;
	case 's':
	  printf ("Stopping VCD trace\n");
	  avr_vcd_stop (&vcd_file);
	  break;
	}
      fflush(stdin);
      fflush(stdout);
      sleep (1); // s
    }
}
