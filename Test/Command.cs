using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avr
{
    public abstract class BaseCommand
    {
        private readonly ALU alu;
        protected BaseCommand(ALU alu)
        {
            this.alu = alu;
        }
        public abstract bool ItsMe(ushort word);
        public abstract void Execute();
        public abstract string Disasemble();
    }

    public class ADCCommand : BaseCommand
    {
        ADCCommand(ALU alu):base(alu){}

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
