using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator
{
    class eNode
    {

        public eNode(string id )
        {
            this.id = id;
            nodeNum = 0;
            numCons = 0;
            volt    = 0;
            isBus = false;

            Initialize();
                //qDebug() << "+eNode" << m_id;

            Simulator.Self().AddToEnodeList(this );
        }

        

        public void PinChanged(ePin epin, int enodeComp) // Add node at other side of pin
        {
            //qDebug() << "eNode::pinChanged" << m_id << epin << enodeComp;
            nodeList[epin] = enodeComp;
        }

        public void Initialize()
        {
            switched = false;
            single = false;
            changed = false;
            currChanged = false;
            admitChanged = false;

            changedFast.Clear();
            nonLinear.Clear();
            reactiveList.Clear();
            admitList.Clear();
            currList.Clear();
            nodeList.Clear();

            volt = 0;

            if (isBus)
            {
                eBusPinList.Clear();
                eNodeList.Clear();
                //qDebug() << "\neNode::initialize"<<this << m_eBusPinList.size();
            }
        }

        public void StampCurrent(ePin* epin, double data)
        {
            if (nodeList[epin] == nodeNum) return; // Be sure msg doesn't come from this node

            currList[epin] = data;

            //qDebug()<< m_nodeNum << epin << data << m_totalCurr;

            currChanged = true;

            if (!changed)
            {
                changed = true;
                Simulator.Self().AddToChangedNodeList(this);
            }
        }

        public void StampAdmitance(ePin epin, double data)
        {
            if (nodeList[epin] == nodeNum) return; // Be sure msg doesn't come from this node

            admitList[epin] = data;

            admitChanged = true;

            if (!changed)
            {
                changed = true;
                Simulator.Self().AddToChangedNodeList(this);
            }
        }

        public void SetNodeNumber(int n) { nodeNum = n; }

        public void StampMatrix()
        {
            if (nodeNum == 0) return;

            changed = false;

            if (admitChanged)
            {
                m_admit.clear();
                m_totalAdmit = 0;

                QHashIterator<ePin, double> i(admitList); // ePin-Admit
                while (i.hasNext())
                {
                    i.next();

                    double adm = i.value();

                    ePin epin = i.key();
                    int enode = nodeList[epin];

                    admit[enode] += adm;
                    totalAdmit += adm;
                }
                if (!single || switched) StampAdmit();

                admitChanged = false;
            }

            if (currChanged)
            {
                totalCurr = 0;
                foreach(var current in currList) totalCurr += current.Value;

                if (!single || switched) StampCurr();

                currChanged = false;
            }
            if (single) SolveSingle();
        }

        public void StampAdmit()
        {
            int nonCero = 0;
            QHashIterator<int, double> ai(admit); // iterate admitance hash: eNode-Admit
            while (ai.hasNext())
            {
                ai.next();
                int enode = ai.key();
                double admit = ai.value();
                if (enode > 0) CircMatrix.Self()->StampMatrix(nodeNum, enode, -admit);

                if (switched)                       // Find open/close events
                {
                    if (admit > 0) nonCero++;
                    double admitP = admitPrev[enode];

                    if ((admit != admitP)
                        && ((admit == 0) || (admitP == 0))) CircMatrix.Self().SetCircChanged();
                }
            }
            if (switched)
            {
                admitPrev = admit;
                if (nonCero < 2) totalAdmit += 1e-12; //pnpBias example error
            }
            CircMatrix.Self().StampMatrix(nodeNum, nodeNum, totalAdmit);
        }

        public void StampCurr()
        {
            CircMatrix.Self().StampCoef(nodeNum, totalCurr);
        }

        public void SolveSingle()
        {
            double volt = 0;

            if (totalAdmit > 0) volt = totalCurr / totalAdmit;
            SetVolt(volt);
        }

        public List<int> GetConnections()
        {
            List<int> cons=new List<int>();
            foreach(var nodeNum in nodeList)
            {
                if (admit[nodeNum] > 0) cons.Add(nodeNum.Value);
            }
            return cons;
        }

        public void SetVolt(double v)
        {
            //qDebug() << m_id << m_volt << v;
            if (fabs(volt - v) > 1e-9) //( m_volt != v )
            {
                //qDebug() << m_id << "setVChanged";
                voltChanged = true;
                volt = v;

                foreach (eElement el in changedFast) Simulator.Self().AddToChangedFast(el); // el->setVChanged();
                foreach (eElement el in reactiveList) Simulator.Self().AddToReactiveList(el);
                foreach (eElement el in nonLinear) Simulator.Self().AddToNoLinList(el);
            }
        }
    double eNode::getVolt() { return m_volt; }

    void eNode::setIsBus(bool bus)
    {
        m_isBus = bus;

        Simulator::self()->remFromEnodeList(this, /*delete=*/ false);
        Simulator::self()->addToEnodeBusList(this);
    }

    bool eNode::isBus()
    {
        return m_isBus;
    }

    void eNode::createBus()
    {
        int busSize = m_eBusPinList.size();

        //qDebug()<<"\neNode::createBus"<< this <<busSize << m_eBusPinList;

        m_eNodeList.clear();
        for (int i = 0; i < busSize; i++)
        {
            QList<ePin*> pinList = m_eBusPinList.at(i);

            eNode* enode = 0l;

            if (!pinList.isEmpty())
            {
                enode = new eNode(m_id + "-eNode-" + QString::number(i));

                for (ePin* epin : pinList)
                {
                    //qDebug() <<"Pin eNode"<< QString::fromStdString(epin->getId())<<enode;
                    epin->setEnode(enode);
                }
            }
            m_eNodeList.append(enode);
        }
    }

    void eNode::addBusPinList(QList<ePin*> list, int line)
    {
        int size = line + 1;
        int busSize = m_eBusPinList.size();

        if (size > busSize)
        {
            for (int i = 0; i < size - busSize; i++)
            {
                QList<ePin*> newList;
                m_eBusPinList.append(newList);
            }
        }

        QList<ePin*> pinList = m_eBusPinList.at(line);
        for (ePin* epin : list)
        {
            if (!pinList.contains(epin))
            {
                pinList.append(epin);
                //epin->setEnode( this );
            }
        }
        //qDebug() << "eNode::addBusPinList" <<this<< line << busSize<<"\n"<<pinList;
        m_eBusPinList.replace(line, pinList);
    }

    QList<ePin*> eNode::getEpins() { return m_ePinList; }

    void eNode::addEpin(ePin* epin)
    {
        //qDebug() << "eNode::addEpin" << m_id << QString::fromStdString(epin->getId());
        if (!m_ePinList.contains(epin)) m_ePinList.append(epin);
    }

    void eNode::remEpin(ePin* epin)
    {
        //qDebug() << "eNode::remEpin" << m_id << QString::fromStdString(epin->getId());
        if (m_ePinList.contains(epin)) m_ePinList.removeOne(epin);

        //qDebug() << "eNode::remEpin" << m_id << QString::fromStdString(epin->getId())<<m_ePinList.size();

        // If No epins then remove this enode
        if (m_ePinList.isEmpty())
        {
            if (m_isBus) Simulator::self()->remFromEnodeBusList(this, true);
            else Simulator::self()->remFromEnodeList(this, true);
        }
    }

    void eNode::addToChangedFast(eElement* el)
    {
        if (!m_changedFast.contains(el)) m_changedFast.append(el);
    }

    void eNode::remFromChangedFast(eElement* el)
    {
        m_changedFast.removeOne(el);
    }

    void eNode::addToReactiveList(eElement* el)
    {
        if (!m_reactiveList.contains(el)) m_reactiveList.append(el);
    }

    void eNode::remFromReactiveList(eElement* el)
    {
        m_reactiveList.removeOne(el);
    }

    void eNode::addToNoLinList(eElement* el)
    {
        if (!m_nonLinear.contains(el)) m_nonLinear.append(el);
    }

    void eNode::remFromNoLinList(eElement* el)
    {
        m_nonLinear.removeOne(el);
    }

    void eNode::setSingle(bool single) { m_single = single; }      // This eNode can calculate it's own Volt
    bool eNode::isSingle() { return m_single; }

    void eNode::setSwitched(bool switched) { m_switched = switched; } // This eNode has switches attached
    bool eNode::isSwitched() { return m_switched; }

    int eNode::getNodeNumber() { return m_nodeNum; }

    QString eNode::itemId() { return m_id; }





        private List<ePin> ePinList;
        //QList<ePin*>     m_ePinSubList;  // Used by Connector to find connected dpins

        private List<List<ePin>> eBusPinList;
        private List<eNode> eNodeList;

        private List<eElement> changedFast;
        private List<eElement> reactiveList;
        private List<eElement> nonLinear;

        private Dictionary<ePin, double> admitList;
        private Dictionary<ePin, double> currList;
        private Dictionary<ePin, int> nodeList;

        private Dictionary<int, double> admit;
        private Dictionary<int, double> admitPrev;

        private double totalCurr;
        private double totalAdmit;

        private double volt;
        private int nodeNum;
        private int numCons;

        private string id;

        private bool currChanged;
        private bool admitChanged;
        private bool voltChanged;
        private bool changed;
        private bool single;
        private bool switched;
        private bool isBus;
    }
}
