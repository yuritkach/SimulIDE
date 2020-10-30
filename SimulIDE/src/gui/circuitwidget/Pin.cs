using SimulIDE.src.gui.circuitwidget.components;
using SimulIDE.src.simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimulIDE.src.gui.circuitwidget
{
    public class Pin:CircLabel,INamedObject
    {

        public Pin(int angle, Point pos, string id, int index, Canvas parent):base(parent)
        {
            epin = new ePin(id, index);

            component = null; // parent;
            blocked = false;
            isBus = false;
            unused = false;
            //connector = null;
            conPin = null;
            enode = null;
            this.angle = angle;
            color = Color.FromRgb(0,0,0);// black
            area = new Rect(-3, -3, 9, 6);
//            string compName = Circuit.Self().GetCompId(id);
//            id.Replace(compName, parent.GetObjectName());
            SetObjectName(id);
            SetConnector(null);
            this.labelx = pos.X;
            this.labely = pos.Y;
            this.labelrot = 180 - angle;
            SetLabelPos();// SetPos(pos);
            SetLength(8);
            //SetCursor(Qt::CrossCursor);
            ////////itemStacksBehindParent = true;
            ////////itemIsSelectable = false;
            this.SetFontSize(6);
            this.SetText("");
            this.SetDefaultTextColor(Color.FromRgb(250, 250, 200));
            //Circuit.Self().AddPin(this, id);
            //connect(parent, SIGNAL(moved()), this, SLOT(isMoved()));
        }
        ~Pin()
        {
        //            Circuit.Self().RemovePin(m_id);
        }

//        void Pin::reset()
//        {
//            //qDebug() << "Pin::reset "<<my_connector->objectName();
//            if (my_connector) setConnector(0l);
//            m_connected = false;

//            //if( !Circuit::self()->deleting() )
//            {
//                //qDebug() << "ePin::reset new:" << m_numConections;
//                m_component->inStateChanged(1);          // Used by node to remove

//                if (m_isBus) m_component->inStateChanged(3); // Used by Bus to remove
//            }
//            ePin::reset();
//        }

//        void Pin::setUnused(bool unused)
//        {
//            m_unused = unused;
//            if (unused) setCursor(Qt::ArrowCursor);
//        }

//        void Pin::registerPinsW(eNode* enode)     // Called by connector closing or other pin
//        {
//            if (m_blocked) return;
//            m_blocked = true;

//            //qDebug() <<"Pin::registerPinsW "<<m_component->itemID();

//            ePin::setEnode(enode);

//            if (m_conPin) m_conPin->registerPins(enode); // Call pin at other side of Connector

//            m_blocked = false;
//        }

//        void Pin::registerPins(eNode* enode)     // Called by connector closing or other pin
//        {
//            if (m_blocked) return;
//            m_blocked = true;
//            //qDebug() <<"Pin::registerPinsW "<<m_component->itemID();
//            ePin::setEnode(enode);

//            if (m_component->itemType() == "Node")
//            {
//                Node* node = dynamic_cast<Node*>(m_component);
//                node->registerPins(enode);
//            }
//            else if (m_component->itemType() == "Bus")
//            {
//                Bus* bus = dynamic_cast<Bus*>(m_component);
//                bus->registerPins(enode);
//            }
//            m_blocked = false;
//        }

        public void SetConnector(Connector connector)
        {
            this.connector = connector;

            if (this.connector!=null)
            {
//                setCursor(Qt::ArrowCursor);
                if (isBus)
                {
                    this.connector.SetIsBus(true);
                    //m_component->inStateChanged( 2 );
                }
            }
//            else setCursor(Qt::CrossCursor);
        }

//        Connector* Pin::connector() { return my_connector; }

//        void Pin::isMoved()
//        {
//            if (my_connector) my_connector->updateConRoute(this, scenePos());
//            else
//            {
//                if (m_isBus) return;
//                if (QApplication::queryKeyboardModifiers() & Qt::ControlModifier)
//                {
//                    QList<QGraphicsItem*> list = this->collidingItems();
//                    for (QGraphicsItem* it : list)
//                    {
//                        if (it->type() == 65536 + 3)                         // Pin found
//                        {
//                            Pin* pin = qgraphicsitem_cast<Pin*>(it);

//                            if (m_isBus != pin->isBus()) continue;
//                            if (!pin->connector())
//                            {
//                                Circuit::self()->newconnector(this);
//                                Circuit::self()->closeconnector(pin);
//                            }
//                            //qDebug() << " Pin: Pin found";
//                            break;
//                        }
//                    }
//                }
//            }
//        }

//        void Pin::mousePressEvent(QGraphicsSceneMouseEvent* event )
//{
//            if (m_unused) return;

//            if ( event->button() == Qt::LeftButton )
//    {
//        if(my_connector==0l )
//        {
//            if(Circuit::self()->is_constarted() )
//            {
//                Connector* con = Circuit::self()->getNewConnector();
//                if(con->isBus() != m_isBus ) // Avoid connect Bus with no-Bus
//                {
//                    event->ignore();
//                    return;
//                }
//}
//event->accept();
//            if( Circuit::self()->is_constarted() ) Circuit::self()->closeconnector( this );
//            else                                   Circuit::self()->newconnector( this );
//}
//        else event->ignore();
//}
//}

//QString Pin::getLabelText()
//{
//    return m_labelText; //m_label.text();
//}

//void Pin::setLabelText(QString label)
//{
//    m_labelText = label;

//    if (label.contains("!"))
//    {
//        QString text;
//        bool inv = false;
//        for (int i = 0; i < label.size(); i++)
//        {
//            QString e = "!";
//            QChar ch = label[i];
//            if (ch == e[0])
//            {
//                inv = true;
//                continue;
//            }
//            e = " ";
//            text.append(ch);
//            if (inv && (ch != e[0])) text.append("\u0305");
//        }
//        //qDebug() << "Pin::setLabelText" <<text<< label<<"\n";
//        label = text;
//    }
//    //qDebug() << "Pin::setLabelText" << label<<"\n";
//    m_label.setText(label);
//    setLabelPos();
//}
//void Pin::setLabelPos()
//{
//    QFontMetrics fm(m_label.font() );

//    int xlabelpos = pos().x();
//    int ylabelpos = pos().y();

//    if (m_angle == 0)   // Right side
//    {
//        m_label.setRotation(0);
//        xlabelpos -= fm.width(m_label.text()) + m_length + 1;
//        ylabelpos -= fm.height() * 2 / 3;
//    }
//    if (m_angle == 90)   // Top
//    {
//        m_label.setRotation(m_angle);
//        xlabelpos += 5;
//        ylabelpos += m_length + 1;
//    }
//    if (m_angle == 180)   // Left
//    {
//        m_label.setRotation(0);
//        xlabelpos += m_length + 1;
//        ylabelpos -= fm.height() * 2 / 3;
//    }
//    if (m_angle == 270)   // Bottom
//    {
//        m_label.setRotation(m_angle);
//        xlabelpos -= 5;
//        ylabelpos -= m_length + 1;

//    }
//    m_label.setPos(xlabelpos, ylabelpos);
//}

//void Pin::setLabelColor(QColor color)
//{
//    m_label.setBrush(color);
//}

//void Pin::setFontSize(int size)
//{
//    QFont font = m_label.font();
//    font.setPixelSize(size);
//    m_label.setFont(font);
//}

//void Pin::setPinAngle(int angle)
//{
//    m_angle = angle;
//    setRotation(180 - angle);
//}

//void Pin::moveBy(int dx, int dy)
//{
//    m_label.moveBy(dx, dy);
//    QGraphicsItem::moveBy(dx, dy);
//}

//void Pin::setPinId(QString id)
//{
//    m_id = id.toStdString();
//}

//QString Pin::pinId()
//{
//    return QString::fromStdString(m_id);
//}

public void SetLength(int length)
{
    if (length < 1) length = 1;
    this.length = length;
    SetLabelPos();
}

//void Pin::setConPin(Pin* pin) { m_conPin = pin; }
//Pin* Pin::conPin() { return m_conPin; }

//void Pin::setBoundingRect(QRect area)
//{
//    m_area = area;
//}

//void Pin::setIsBus(bool bus)
//{
//    if (m_isBus == bus) return;
//    if (!bus) return;
//    m_isBus = bus;

//    if (my_connector) my_connector->setIsBus(true);
//    if (m_conPin) m_conPin->setIsBus(true);

//    m_component->inStateChanged(2);         // Propagate Is Bus (Node)
//}

//bool Pin::isBus()
//{
//    return m_isBus;
//}

//void Pin::setVisible(bool visible)
//{
//    m_label.setVisible(visible);
//    QGraphicsItem::setVisible(visible);
//}

//void Pin::paint(QPainter* painter, const QStyleOptionGraphicsItem* option, QWidget* widget)
//{
//    Q_UNUSED(option); Q_UNUSED(widget);


//    QPen pen(m_color, 3, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin);

//    //painter->setBrush( Qt::red );
//    //painter->drawRect( boundingRect() );

//    if (m_unused) pen.setColor(QColor(75, 120, 170));
//    if (isSelected()) pen.setColor(Qt::darkGray);

//    painter->setPen(pen);
//    painter->drawLine(0, 0, m_length - 1, 0);

//    if (m_inverted)
//    {
//        //Component::paint( p, option, widget );
//        painter->setBrush(Qt::white);
//        QPen pen = painter->pen();
//        pen.setWidth(2);
//        //if( isSelected() ) pen.setColor( Qt::darkGray);
//        painter->setPen(pen);
//        QRectF rect( 3,-2.5, 5, 5 );
//        painter->drawEllipse(rect);
//    }
//}


        public Rect BoundingRect() { return area; }

//    enum { Type = UserType + 3 };
 //  public int Type({ return Type; }


        public bool Unused() { return unused; }
        public void SetColor(Color color) { this.color = color; }
        public int PinAngle() { return angle; }
        public Component Component() { return component; }

        public void SetObjectName(string name) { objectName = name; }
        public string GetObjectName() { return objectName; }
        

        int angle;
        int length;
        bool blocked;
        bool isBus;
        bool unused;
        string labelText;
        Color color;
        Rect area;
        Connector connector;
        Component component;
        Pin conPin;          // Pin at the other side of connector
        ePin epin;
        eNode enode;

        private string objectName;
    }
}
