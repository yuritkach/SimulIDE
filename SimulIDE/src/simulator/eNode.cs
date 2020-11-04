using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator
{
    public class eNode
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

        public void StampCurrent(ePin epin, double data)
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
                admit.Clear();
                totalAdmit = 0;
                foreach (var el in admitList)
                {
                    double adm = el.Value;
                    ePin epin = el.Key;
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
            foreach (var el in admit)
            {
                int enode = el.Key;
                double admit = el.Value;
                if (enode > 0) CircMatrix.Self().StampMatrix(nodeNum, enode, -admit);

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
                foreach (var el in admit)
                    if (el.Value > 0) cons.Add((int)el.Value);
            }
            return cons;
        }

        public void SetVolt(double v)
        {
            //qDebug() << m_id << m_volt << v;
            if (Math.Abs(volt - v) > 1e-9) //( m_volt != v )
            {
                //qDebug() << m_id << "setVChanged";
                voltChanged = true;
                volt = v;

                foreach (eElement el in changedFast) Simulator.Self().AddToChangedFast(el); // el->setVChanged();
                foreach (eElement el in reactiveList) Simulator.Self().AddToReactiveList(el);
                foreach (eElement el in nonLinear) Simulator.Self().AddToNoLinList(el);
            }
        }

        public double GetVolt() { return volt; }

        public void SetIsBus(bool bus)
        {
            isBus = bus;

            Simulator.Self().RemFromEnodeList(this, /*delete=*/ false);
            Simulator.Self().AddToEnodeBusList(this);
        }

        public bool IsBus()
        {
            return isBus;
        }

        public void CreateBus()
        {
            int busSize = eBusPinList.Capacity;

            //qDebug()<<"\neNode::createBus"<< this <<busSize << m_eBusPinList;

            eNodeList.Clear();
            for (int i = 0; i < busSize; i++)
            {
                List<ePin> pinList = eBusPinList[i];

                eNode enode = null;

                if (pinList.Capacity!=0)
                {
                    enode = new eNode(id + "-eNode-" + i.ToString());

                    foreach (ePin epin in pinList)
                    {
                        //qDebug() <<"Pin eNode"<< QString::fromStdString(epin->getId())<<enode;
                        epin.SetEnode(enode);
                    }
                }
                eNodeList.Add(enode);
            }
        }

        public void AddBusPinList(List<ePin> list, int line)
        {
            int size = line + 1;
            int busSize = eBusPinList.Capacity;

            if (size > busSize)
            {
                for (int i = 0; i < size - busSize; i++)
                {
                    List<ePin> newList = new List<ePin>();
                    eBusPinList.Add(newList);
                }
            }

            List<ePin> pinList = eBusPinList[line];
            foreach (ePin epin in list)
            {
                if (!pinList.Contains(epin))
                {
                    pinList.Add(epin);
                    //epin->setEnode( this );
                }
            }
            //qDebug() << "eNode::addBusPinList" <<this<< line << busSize<<"\n"<<pinList;
            eBusPinList[line]=pinList;
        }

        public List<ePin> GetEpins() { return ePinList; }

        public void AddEpin(ePin epin)
        {
            //qDebug() << "eNode::addEpin" << m_id << QString::fromStdString(epin->getId());
            if (!ePinList.Contains(epin)) ePinList.Add(epin);
        }

        public void RemEpin(ePin epin)
        {
            //qDebug() << "eNode::remEpin" << m_id << QString::fromStdString(epin->getId());
            if (ePinList.Contains(epin)) ePinList.Remove(epin);

            //qDebug() << "eNode::remEpin" << m_id << QString::fromStdString(epin->getId())<<m_ePinList.size();

            // If No epins then remove this enode
            if (ePinList.Capacity==0)
            {
                if (isBus) Simulator.Self().RemFromEnodeBusList(this, true);
                else Simulator.Self().RemFromEnodeList(this, true);
            }
        }

        public void AddToChangedFast(eElement el)
        {
            if (!changedFast.Contains(el)) changedFast.Add(el);
        }

        public void RemFromChangedFast(eElement el)
        {
            changedFast.Remove(el);
        }

        public void AddToReactiveList(eElement el)
        {
            if (!reactiveList.Contains(el)) reactiveList.Add(el);
        }

        public void RemFromReactiveList(eElement el)
        {
            reactiveList.Remove(el);
        }

        public void AddToNoLinList(eElement el)
        {
            if (!nonLinear.Contains(el)) nonLinear.Add(el);
        }

        public void RemFromNoLinList(eElement el)
        {
            nonLinear.Remove(el);
        }

        public void SetSingle(bool single) { this.single = single; }      // This eNode can calculate it's own Volt
        public bool IsSingle() { return single; }

        public void SetSwitched(bool switched) { this.switched = switched; } // This eNode has switches attached
        public bool IsSwitched() { return switched; }

        public int GetNodeNumber() { return nodeNum; }

        public string ItemId() { return id; }

        private List<ePin> ePinList= new List<ePin>();
        //QList<ePin*>     m_ePinSubList;  // Used by Connector to find connected dpins

        private List<List<ePin>> eBusPinList = new List<List<ePin>>();
        private List<eNode> eNodeList = new List<eNode>();

        private List<eElement> changedFast = new List<eElement>();
        private List<eElement> reactiveList = new List<eElement>();
        private List<eElement> nonLinear = new List<eElement>();

        private Dictionary<ePin, double> admitList = new Dictionary<ePin, double>();
        private Dictionary<ePin, double> currList = new Dictionary<ePin, double>();
        private Dictionary<ePin, int> nodeList = new Dictionary<ePin, int>();

        private Dictionary<int, double> admit = new Dictionary<int, double>();
        private Dictionary<int, double> admitPrev = new Dictionary<int, double>();

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
