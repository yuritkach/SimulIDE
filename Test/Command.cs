using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avr
{
    public class BaseCommand
    {
        public virtual bool ItsMe(ushort word) { return false; }
        public virtual void Execute() { }
        public virtual string Disasemble() { return null; }
    }

    public class ADCCommand : BaseCommand
    {
        public override string Disasemble()
        {
            return "ADC R1,R2"; // R1-R2 нужно привести к реальным названиями (по младшим битам команды)
        }

        public override void Execute()
        {
            // Нужно:
            // 1. расчитать результат и положить его в регистр (OnChanged)
            // 2. Увеличить PC (OnChanged)
            // 3. Изменить состояние SREG (через свойство, чтоб послать event OnChanged)
            // 4. Увеличить значение счетчика Clock (OnChanged)
        }

        public override bool ItsMe(ushort command)
        {
            return (command & 0b0001110000000000) == 0b0001110000000000;
        }
    }
    

}
