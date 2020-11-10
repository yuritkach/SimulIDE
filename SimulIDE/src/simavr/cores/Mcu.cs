using SimulIDE.src.simavr.sim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simavr.cores
{
    public class AbstractException : ApplicationException
    {
        public override string Message => "Not overriden method! " + base.Message;
    }

    public class DefFunc
    {
        public Func<object[],object> Func { get; set; }
        public object[] Param { get; set; }
        public DefFunc(Func<object[], object>func, object[] param=null)
        {
            Func = func;
            Param = param;
        }

    }
    
    public class Mcu
    {
        protected Dictionary<string, object> constants ;

        public Avr core;
        public Avr_eeprom eeprom;

        public Mcu()
        {
            constants = new Dictionary<string, object>();
            InitConstants();
        }

        protected virtual void InitConstants()
        {
            constants["FUSE"] = new DefFunc(GetFuse);
            constants["SIGNATURE_0"] = 0;
            constants["SIGNATURE_1"] = 0;
            constants["SIGNATURE_2"] = 0;
            constants["LOCKBITS"] = 0xFF;
            constants["SIGNATURE"] = new DefFunc(GetSignature);
            constants["MCU_STATUS_REG"] = new DefFunc(GetMCU_STATUS_REG);
            constants["MCU_STATUS_REG"] = new DefFunc(GetResetFlags);
        }

        public virtual dynamic GetValue(string name)
        {

            var v = constants[name];
            if (v.GetType() == typeof(DefFunc))
            {
                DefFunc defin = (DefFunc)v;
                var func = defin.Func;
                MethodInfo methodInfo = func.GetMethodInfo();
                dynamic result = null;
                if (methodInfo != null)
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    result = methodInfo.Invoke(this, parameters.Length == 0 ? null : defin.Param);
                    return result;
                }
                else throw new Exception("Can't get value for method "+func.ToString()); 
            }
            else return v;
        }

        public virtual byte[] GetFuse(object[] param=null)
        {
            switch (GetValue("FuseMemorySize"))
            {
                case 6: return new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                case 3: return new byte[] { GetValue("LFUSE_DEFAULT"), GetValue("HFUSE_DEFAULT"), GetValue("EFUSE_DEFAULT") };
                case 2: return new byte[] { GetValue("LFUSE_DEFAULT"), GetValue("HFUSE_DEFAULT") };
                case 1: return new byte[] { GetValue("FUSE_DEFAULT") };
                default: return new byte[] { 0 };
            }
        }
        protected virtual byte[] GetSignature(object[] param=null)
        {
            return new byte[3] { GetValue("SIGNATURE_0"), GetValue("SIGNATURE_1"), GetValue("SIGNATURE_2") };
        }
        
        protected virtual object GetMCU_STATUS_REG(object[] param=null)
        {
            object result;
            result = (int)(GetValue("MCUSR") != null ? GetValue("MCUSR") : GetValue("MCUCSR"));
            return result;
        }
        
        public virtual object GetResetFlags(object[] param=null)
        {
            ResetFlags result = new ResetFlags();
            result.porf = Sim_regbit.AVR_IO_REGBIT(GetValue("MCU_STATUS_REG"), GetValue("PORF"));
            result.extrf = Sim_regbit.AVR_IO_REGBIT(GetValue("MCU_STATUS_REG"), GetValue("EXTRF"));
            result.borf = Sim_regbit.AVR_IO_REGBIT(GetValue("MCU_STATUS_REG"), GetValue("BORF"));
            result.wdrf = Sim_regbit.AVR_IO_REGBIT(GetValue("MCU_STATUS_REG"), GetValue("WDRF"));
            return result;
        }

        public virtual object _SFR_IO8(object[] param)
        {
            return (byte) ((byte)param[0] + 32);
        }

        public virtual object _SFR_IO16(UInt16 v)
        {
            return (UInt16)(v+32);
        }
        
        public virtual bool Get__SIM_CORE_DECLARE_H__() { return true; }
        /* we have to declare this, as none of the distro but debian has a modern
         * toolchain and avr-libc. This affects a lot of names, like MCUSR etc
        */
        public virtual bool Get__AVR_LIBC_DEPRECATED_ENABLE__() { return true; }
        public virtual byte GetEE_READY_vect() { throw new AbstractException(); }


        //#define _SFR_MEM8(v) (v)
        //#define _BV(v) (v)
        //#define _VECTOR(v) (v)



    }
}
