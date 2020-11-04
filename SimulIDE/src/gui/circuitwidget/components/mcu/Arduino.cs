using SimulIDE.src.simulator;
using SimulIDE.src.simulator.elements.processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimulIDE.src.gui.circuitwidget.components.mcu
{
    public class Arduino:AvrCompBase
    {
        public Arduino(object parent, string type, string id) : base(parent, type, id)
        {

            self = this;

            processor = AvrProcessor.Self();

        //    setLabelPos(100, -21, 0); // X, Y, Rot

            InitChip();
            if (error == 0)
            {
                InitBoard();
                SetFreq(16);
                //initBootloader();
            }
//            SetTransformOriginPoint(togrid(boundingRect().center()));
        }



        //LibraryItem* Arduino::libraryItem()
        //{
        //    return new LibraryItem(
        //        tr("Arduino"),
        //        tr("Micro"),
        //        "arduinoUnoIcon.png",
        //        "Arduino",
        //        Arduino::construct);
        //}

        public static Component Construct(object parent, string type, string id)
        {
            if (canCreate)
            {
                Arduino ard = new Arduino(parent, type, id);
                if (error > 0)
                {
                    Circuit.Self().CompList().Remove(ard);
                  //TYV  ard.DeleteLater();
                    ard = null;
                    error = 0;
                    self = null;
                    canCreate = true;
                }
                return ard;
            }
            MessageBox.Show("Error! Only 1 Mcu allowed\n to be in the Circuit.");
            return null;
        }

  
        public new virtual void Remove()
        {
            //m_pb5Pin->setEnode(0l);
            //ePin* ledPin0 = m_boardLed->getEpin(0);
            //ledPin0->setEnode(0l);
            //delete ledPin0;

            //ePin* ledPin1 = m_boardLed->getEpin(1);
            //ledPin1->setEnode(0l);
            //delete ledPin1;

            //delete m_groundEnode;
            //delete m_boardLedBuffer;

            //McuComponent::remove();
            //if (m_boardLedEnode) Simulator::self()->remFromEnodeList(m_boardLedEnode, true);
            //Simulator::self()->remFromEnodeList(m_bufferEnode, true);
            //Simulator::self()->remFromUpdateList(m_boardLed);
        }

        public void Attach()
        {
            //eNode* enod = m_pb5Pin->getEnode();

            //if (!enod)                        // Not connected: Create boardLed eNode
            //{
            //    m_boardLedEnode = new eNode(m_id + "-boardLedeNode");
            //    enod = m_boardLedEnode;
            //    m_pb5Pin->setEnode(m_boardLedEnode);
            //}
            //else if (enod != m_boardLedEnode) // Connected to external eNode: Delete boardLed eNode
            //{
            //    m_boardLedEnode = enod;
            //}
            //else return;                       // Already connected to boardLed eNode: Do nothing

            //m_boardLedBuffer->getEpin("input0")->setEnode(enod);
        }

        protected void InitBoard()
        {
            // Create Led eNodes
            groundEnode = new eNode(id + "-Gnod");
            groundEnode.SetNodeNumber(0);
            Simulator.Self().RemFromEnodeList(groundEnode, false);
            bufferEnode = new eNode(id + "-Lnod");

            // Create Led Buffer
            //boardLedBuffer = new eGate((m_id + "boardLedBuffer").toStdString(), 0);
            //boardLedBuffer.CreatePins(1, 1);
            //boardLedBuffer.GetEpin("output0").SetEnode(bufferEnode);

            // Create board led
            //m_boardLed = new LedSmd(this, "LEDSMD", m_id + "boardled", QRectF(0, 0, 4, 3));
            //m_boardLed->setNumEpins(2);
            //m_boardLed->setParentItem(this);
            //m_boardLed->setEnabled(false);
            //m_boardLed->setMaxCurrent(0.003);
            //m_boardLed->setRes(1000);
            //Circuit::self()->compList()->removeOne(m_boardLed);

            //m_boardLed->getEpin(0)->setEnode(m_bufferEnode);
            //m_boardLed->getEpin(1)->setEnode(m_groundEnode); // Connect board led to ground

            //if (objectName().contains("Mega")) m_boardLed->setPos(35 + 12, 125 + 105);
            //else m_boardLed->setPos(35, 125);

            //for (int i = 0; i < numpins; i++)                      // Create Pins
            //{
            //    McuComponentPin* mcuPin = m_pinList.at(i);

            //    if (mcuPin->angle() == 0) mcuPin->move(-16, 0);
            //    else if (mcuPin->angle() == 180) mcuPin->move(16, 0);
            //    else if (mcuPin->angle() == 90) mcuPin->move(0, 32);
            //    else mcuPin->move(0, -320);

            //    Pin* pin = mcuPin->pin();
            //    pin->setLength(0);
            //    pin->setFlag(QGraphicsItem::ItemStacksBehindParent, false);

            //    QString pinId = pin->pinId();
            //    QString type = mcuPin->ptype();
            //    if (pinId.contains("GND"))                   // Gnd Pins
            //    {
            //        mcuPin->setImp(0.01);
            //    }
            //    else if (pinId.contains("V3V"))                  // 3.3V Pins
            //    {
            //        mcuPin->setImp(0.1);
            //        mcuPin->setVoltHigh(3.3);
            //        mcuPin->setVoltLow(3.3);
            //    }
            //    else if (pinId.contains("V5V"))                    // 5V Pins
            //    {
            //        mcuPin->setImp(0.1);
            //        mcuPin->setVoltHigh(5);
            //        mcuPin->setVoltLow(5);
            //    }
            //    else if (pinId.contains("Vin"))          // Vin Pins ( 12 V )
            //    {
            //        mcuPin->setImp(0.1);
            //        mcuPin->setVoltHigh(12);
            //        mcuPin->setVoltLow(12);
            //    }
            //    else if (type.contains("led"))                      // Pin 13
            //    {
            //        //pin->setEnode( m_boardLedEnode );
            //        m_pb5Pin = pin;
            //    }
            //    else if (pinId.toUpper().contains("RST"))       // Reset Pins
            //    {
            //        mcuPin->setImp(20000);
            //        mcuPin->setVoltHigh(5);
            //        mcuPin->setVoltLow(5);
            //    }
            //}
        }

//    void Arduino::paint(QPainter* p, const QStyleOptionGraphicsItem* option, QWidget* widget )
//{
//    Component::paint(p, option, widget);

//    int ox = m_area.x();
//        int oy = m_area.y();

//        p->drawPixmap(ox, oy, QPixmap(m_BackGround ));
//    }

        //static LibraryItem* libraryItem();


        eNode groundEnode;
        eNode bufferEnode;
//        LedSmd m_boardLed;
        eNode boardLedEnode;
//        eGate m_boardLedBuffer;
        Pin pb5Pin;
    }

}
