using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator
{
    public class ePin
    {

        public ePin(string id, int index )
        {
            this.id = id;
            this.index = index;
            enode    = null;
            enodeCon = null;
            connected = false;
            inverted  = false;
        }

        ~ePin()
        {
            //qDebug() << "deleting" << QString::fromStdString( m_id );
            if (enode!=null) enode.RemEpin(this);
        }

        public void Reset()
        {
            SetEnode(null);
        }

        public eNode GetEnode()
        {
            //qDebug() << "ePin::getEnode" << m_connected<<m_enode;
            return enode;
        }

        public void SetEnode(eNode enode)
        {
            if (enode == this.enode) return;

            //qDebug() << "ePin::setEnode" << QString::fromStdString(m_id) << enode <<m_enode;

            if (this.enode!=null) this.enode.RemEpin(this);
            if (enode!=null) enode.AddEpin(this);

            this.enode = enode;
            connected = (enode != null);
        }

        //eNode* ePin::getEnodeComp() { return m_enodeCon; }

        public void SetEnodeComp(eNode enode)
        {
            //std::cout << "\nePin::setEnodeComp "<< m_id << m_connected ;
            enodeCon = enode;
            int enodeConNum = 0;
            if (enode!=null) enodeConNum = enode.GetNodeNumber();
            if (connected) this.enode.PinChanged(this, enodeConNum);
        }

        public void StampCurrent(double data)
        {
            //qDebug() << "ePin::stampCurrent connected" << m_connected << data;
            if (connected) enode.StampCurrent(this, data);
        }

        public void StampAdmitance(double data)
        {
            if (connected)
            {
                if (enodeCon!=null) data = 1e-12;
                enode.StampAdmitance(this, data);
            }
        }

        public double GetVolt()
        {
            //std::cout << "\nePin::getVolt "<< m_id << m_connected ;
            if (connected) return enode.GetVolt();
            if (enodeCon!=null) return enodeCon.GetVolt();
            return 0;
        }

        public void SetConnected(bool connected) { this.connected = connected; }

        public bool IsConnected() { return connected; }

        public eNode GetEnodeComp() { return enodeCon; }

        public bool Inverted() { return inverted; }

        public void SetInverted(bool inverted) { this.inverted = inverted; }

        public string GetId() { return this.id; }

        public void SetId(string id)
        {
            //Circuit::self()->removePin( m_id );
      //TYV      Circuit.Self().UpdatePin(this, id);
            this.id = id;
        }

        protected eNode enode;
        protected eNode enodeCon;

        protected string id;
        protected int index;

        protected bool connected;
        protected bool inverted;
    }
}
