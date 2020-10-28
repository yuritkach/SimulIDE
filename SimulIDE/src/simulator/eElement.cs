using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator
{
    class eElement
    {

        public eElement(string id )
        {
            elmId = id;
            Simulator.Self().AddToElementList(this );
            //qDebug() << "eElement::eElement" << QString::fromStdString( m_elmId );
        }

        ~eElement()
        {
            Simulator.Self().RemFromElementList(this);
        }

        public virtual void InitEpins()
        {
            SetNumEpins(2); // by default create 2 ePins
        }

        public virtual void SetNumEpins(int n)
        {
            Array.Resize<ePin>(ref ePin,n);
            //qDebug() << "eElement::setNumEpins"<< QString::fromStdString( m_elmId )<<m_ePin.size();
            for (int i = 0; i < n; i++)
            {
                //qDebug() << "eElement::setNumEpins PIN:"<<i<<m_ePin[i];
                if (ePin[i] == null)
                {
                    //qDebug() << "eElement::setNumEpins Creating:"<<i;
                    string ss = elmId + "-ePin" + i.ToString();
                    ePin[i] = new ePin(ss, i);
                }
            }
        }

        public virtual ePin GetEpin(int pin)
        {
            return ePin[pin];
        }

        public virtual ePin GetEpin(string pinName)
        {
            //qDebug() << "eElement::getEpin" << pinName;
            if (pinName == "lPin") return ePin[0];
            else if (pinName == "rPin") return ePin[1];
            else if (pinName.Contains("ePin"))
            {
                int pin = int.Parse(pinName.Split('-').Last().Substring(4));// Remove("ePin").ToInt();
                return ePin[pin];
            }
            return null;
        }

        public virtual void SetEpin(int num, ePin pin)
        {
            ePin[num] = pin;
        }

        public virtual void Initialize() { }
        public virtual void ResetState() { }
        public virtual void Attach() { }
        public virtual void Stamp() { }

        public virtual void SimuClockStep() { }
        public virtual void UpdateStep() { }
        public virtual void SetVChanged() { }

        const double cero_doub = 1e-14;
        const double high_imp = 1e14;
        const double digital_high = 5.0;
        const double digital_low = 0.0;
        const double digital_threshold = 2.5;

        protected ePin[] ePin = new ePin[0];
        protected string elmId;

    }
}
