﻿using SimulIDE.src.simavr.cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr
{
    public class Sim_core_decl
    {

        //extern avr_kind_t mega164;
        //extern avr_kind_t tiny45;
        //extern avr_kind_t tiny85;
        //extern avr_kind_t mega324;
        //extern avr_kind_t mega1284;
        //extern avr_kind_t mega1280;
        //extern avr_kind_t tiny84;
        //extern avr_kind_t mega168;
        //extern avr_kind_t mega1281;
        //extern avr_kind_t tiny24;
        //extern avr_kind_t mega169p;
        //extern avr_kind_t mega128rfr2;
        //extern avr_kind_t tiny2313a;
        //extern avr_kind_t mega88;
        //extern avr_kind_t tiny4313;
        //extern avr_kind_t usb162;
        //extern avr_kind_t mega128rfa1;
        //extern avr_kind_t mega8;
        //extern avr_kind_t tiny25;
        //extern avr_kind_t mega2560;
        //extern avr_kind_t mega16;
        //extern avr_kind_t mega64;
        protected static Avr_kind mega328 = new Avr_kind();
        //extern avr_kind_t tiny2313;
        //extern avr_kind_t tiny13;
        //extern avr_kind_t mega16m1;
        //extern avr_kind_t mega324a;
        //extern avr_kind_t mega32u4;
        //extern avr_kind_t mega644;
        //extern avr_kind_t usb162;
        //extern avr_kind_t tiny44;
        //extern avr_kind_t mega128;
        //extern avr_kind_t mega48;
        //extern avr_kind_t mega32;
       
        public static Avr_kind[] avr_kind = {
            //&mega164,
            //&tiny45,
            //&tiny85,
            //&mega324,
            //&mega1284,
            //&mega1280,
            //&tiny84,
            //&mega168,
            //&mega1281,
            //&tiny24,
            //&mega169p,
            //&mega128rfr2,
            //&tiny2313a,
            //&mega88,
            //&tiny4313,
            //&usb162,
            //&mega128rfa1,
            //&mega8,
            //&tiny25,
            //&mega2560,
            //&mega16,
            //&mega64,
             Mega328.Self()
            //&tiny2313,
            //&tiny13,
            //&mega16m1,
            //&mega324a,
            //&mega32u4,
            //&mega644,
            //&usb162,
            //&tiny44,
            //&mega128,
            //&mega48,
            //&mega32
        };


    }
}
