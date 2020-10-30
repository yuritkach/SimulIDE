using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimulIDE.src.gui.circuitwidget.components
{
    public class Component:Canvas // +добавить интерфейсы..
    {


        //        int Component::m_error = 0;

        //        static const char* Component_properties[] = {
        //    QT_TRANSLATE_NOOP("App::Property","id"),
        //    QT_TRANSLATE_NOOP("App::Property","Show id"),
        //    QT_TRANSLATE_NOOP("App::Property","Unit"),
        //    QT_TRANSLATE_NOOP("App::Property","Color")
        //};

        public Component(Canvas parent, string type, string id)
        {
            help = null;
            value = 0;
            unitMult = 1;
            Hflip = 1;
            Vflip = 1;
            mult = " ";
            unit = " ";
            this.type = type;
            color = Color.FromRgb(255, 255, 255);// White;
            showId = false;
            moving = false;
            printable = false;
            BackGround = "";

            if ((type != "Connector") && (type != "Node"))
            {
                 LibraryItem li = ItemLibrary.Self().libraryItem(type);

                 if (li)
                 {
                      if ((type == "Subcircuit")
                       || (type == "AVR")
                       || (type == "PIC")
                       || (type == "Arduino"))
                      {
                           string name = id;
                           name = name.Split('-').First();
                           help = new string(li.GetHelpFile(name));
                      }
                      else help = li.Help();
                 }
            }

            idLabel = new Label(this);
            idLabel.SetDefaultTextColor(Color.FromRgb(0, 0, 128)); // darkBlue
            idLabel.SetFontSize(10);
            SetLabelPos(-16, -24, 0);
            SetShowId(false);

            valLabel = new Label(this);
            valLabel.SetDefaultTextColor(Color.FromRgb(0, 0, 0)); // black
            SetValLabelPos(0, 0, 0);
            valLabel.SetFontSize(10);
            SetShowVal(false);

            //SetObjectName(id);
            SetIdLabel(id);
            SetId(id);

            //setCursor(Qt::OpenHandCursor);
            this.SetFlag(QGraphicsItem::ItemIsSelectable, true);

            //setTransformOriginPoint( boundingRect().center() );

//            if (type == "Connector") Circuit::self()->conList()->append(this);
//            else if (type == "SerialPort") Circuit::self()->compList()->append(this);
//            else if (type == "SerialTerm") Circuit::self()->compList()->append(this);
//            else Circuit::self()->compList()->prepend(this);

        }

     

        //        void Component::mousePressEvent(QGraphicsSceneMouseEvent* event )
        //{
        //            if ( event->button() == Qt::LeftButton )
        //    {
        //        event->accept();
        //        if( event->modifiers() == Qt::ControlModifier ) setSelected( !isSelected() );
        //        else
        //        {
        //            if( !isSelected() )     // Deselecciona los demas
        //            {
        //                QList<QGraphicsItem*> itemlist = Circuit::self()->selectedItems();

        //                for(QGraphicsItem* item : itemlist ) item->setSelected( false );

        //        setSelected( true );
        //    }
        //    QPropertyEditorWidget::self()->setObject(this );
        //    PropertiesWidget::self()->setHelpText(m_help );

        //    setCursor(Qt::ClosedHandCursor );
        //}
        //    }
        //}

        //void Component::mouseDoubleClickEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    if ( event->button() == Qt::LeftButton )
        //    {
        //        QPropertyEditorWidget::self()->setObject(this);
        //        PropertiesWidget::self()->setHelpText(m_help);
        //        //QPropertyEditorWidget::self()->setVisible( true );
        //    }
        //}

        //void Component::mouseMoveEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    event->accept();

        //    QPointF delta = togrid(event->scenePos()) - togrid(event->lastScenePos());

        //    bool deltaH = fabs(delta.x()) > 0;
        //    bool deltaV = fabs(delta.y()) > 0;

        //    if (!deltaH && !deltaV) return;

        //    QList<QGraphicsItem*> itemlist = Circuit::self()->selectedItems();
        //    if (itemlist.size() > 1)
        //    {
        //        if (!m_moving)
        //        {
        //            Circuit::self()->saveState();
        //            m_moving = true;
        //        }
        //        for (QGraphicsItem* item : itemlist)
        //        {
        //            ConnectorLine* line = qgraphicsitem_cast<ConnectorLine*>(item);
        //            if (line->objectName() == "")
        //            {
        //                //line->move( delta );
        //                line->moveSimple(delta);
        //            }

        //        }
        //        for (QGraphicsItem* item : itemlist)
        //        {
        //            Component* comp = qgraphicsitem_cast<Component*>(item);
        //            if (comp && (comp->objectName() != "") && (!comp->objectName().contains("Connector")))
        //            {
        //                comp->move(delta);
        //            }
        //        }
        //        for (Component* comp : *(Circuit::self()->conList()))
        //        {
        //            Connector* con = static_cast<Connector*>(comp);
        //            con->startPin()->isMoved();
        //            con->endPin()->isMoved();
        //        }
        //    }
        //    else this->move(delta);
        //}

        //void Component::move(QPointF delta)
        //{
        //    setPos(pos() + delta);
        //    emit moved();
        //}

        //void Component::moveTo(QPointF pos)
        //{
        //    setPos(pos);
        //    emit moved();
        //}

        //void Component::mouseReleaseEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    event->accept();
        //    setCursor(Qt::OpenHandCursor);

        //    m_moving = false;
        //    Circuit::self()->update();
        //}

        //void Component::contextMenuEvent(QGraphicsSceneContextMenuEvent* event )
        //{
        //    if (!acceptedMouseButtons()) event->ignore();
        //    else
        //    {
        //        event->accept();
        //        QMenu* menu = new QMenu();
        //        contextMenu( event, menu);
        //        menu->deleteLater();
        //    }
        //}

        //void Component::contextMenu(QGraphicsSceneContextMenuEvent* event, QMenu* menu)
        //{
        //    m_eventpoint = mapToScene(togrid(event->pos()));

        //    QAction* copyAction = menu->addAction(QIcon(":/copy.png"), tr("Copy") + "\tCtrl+C");
        //    connect(copyAction, SIGNAL(triggered()), this, SLOT(slotCopy()));

        //    QAction* removeAction = menu->addAction(QIcon(":/remove.png"), tr("Remove") + "\tDel");
        //    connect(removeAction, SIGNAL(triggered()), this, SLOT(slotRemove()));

        //    QAction* propertiesAction = menu->addAction(QIcon(":/properties.png"), tr("Properties"));
        //    connect(propertiesAction, SIGNAL(triggered()), this, SLOT(slotProperties()));
        //    menu->addSeparator();

        //    QAction* rotateCWAction = menu->addAction(QIcon(":/rotateCW.png"), tr("Rotate CW"));
        //    connect(rotateCWAction, SIGNAL(triggered()), this, SLOT(rotateCW()));

        //    QAction* rotateCCWAction = menu->addAction(QIcon(":/rotateCCW.png"), tr("Rotate CCW"));
        //    connect(rotateCCWAction, SIGNAL(triggered()), this, SLOT(rotateCCW()));

        //    QAction* rotateHalfAction = menu->addAction(QIcon(":/rotate180.png"), tr("Rotate 180"));
        //    connect(rotateHalfAction, SIGNAL(triggered()), this, SLOT(rotateHalf()));

        //    QAction* H_flipAction = menu->addAction(QIcon(":/hflip.png"), tr("Horizontal Flip"));
        //    connect(H_flipAction, SIGNAL(triggered()), this, SLOT(H_flip()));

        //    QAction* V_flipAction = menu->addAction(QIcon(":/vflip.png"), tr("Vertical Flip"));
        //    connect(V_flipAction, SIGNAL(triggered()), this, SLOT(V_flip()));

        //    menu->exec(event->screenPos());
        //}

        //void Component::slotCopy()
        //{
        //    if (!isSelected()) Circuit::self()->clearSelection();
        //    setSelected(true);
        //    Circuit::self()->copy(m_eventpoint);
        //}

        //void Component::slotRemove()
        //{
        //    if (!isSelected()) Circuit::self()->clearSelection();
        //    setSelected(true);
        //    Circuit::self()->removeItems();
        //}

        //void Component::remove()
        //{
        //    for (uint i = 0; i < m_pin.size(); i++)   // Remove connectors attached
        //    {
        //        Pin* pin = m_pin[i];
        //        if (!pin) continue;

        //        if (pin && pin->isConnected())
        //        {
        //            Connector* con = pin->connector();
        //            if (con) con->remove();
        //        }
        //    }
        //    Circuit::self()->compRemoved(true);
        //}

        //void Component::slotProperties()
        //{
        //    QPropertyEditorWidget::self()->setObject(this);
        //    PropertiesWidget::self()->setHelpText(m_help);
        //    MainWindow::self()->m_sidepanel->setCurrentIndex(2); // Open Properties tab
        //}

        //void Component::H_flip()
        //{
        //    Circuit::self()->saveState();
        //    m_Hflip = -m_Hflip;
        //    setflip();
        //}

        //void Component::V_flip()
        //{
        //    Circuit::self()->saveState();
        //    m_Vflip = -m_Vflip;
        //    setflip();
        //}

        //void Component::rotateCW()
        //{
        //    Circuit::self()->saveState();
        //    setRotation(rotation() + 90);
        //    emit moved();
        //}

        //void Component::rotateCCW()
        //{
        //    Circuit::self()->saveState();
        //    setRotation(rotation() - 90);
        //    emit moved();
        //}

        //void Component::rotateHalf()
        //{
        //    Circuit::self()->saveState();
        //    setRotation(rotation() - 180);
        //    emit moved();
        //}

        public void UpdateLabel(Label label, string txt)
        {
            int x;
            if (label == idLabel) id = txt;
            else if (label == valLabel)
            {
                StringBuilder value = new StringBuilder();
                for (x = 0; x < txt.Length; ++x)
                {
                    Char c = txt.ElementAt<char>(x);
                    if (Char.IsDigit(c)) value.Append(c);
                    else break;
                }
                string unit = txt.Substring(x, txt.Length);
                SetUnit(unit);
                SetValue(double.Parse(value.ToString()));
            }
        }

        public void SetLabelPos(int x, int y, int rot)
        {
            idLabel.labelx = x;
            idLabel.labely = y;
            idLabel.labelrot = rot;
            idLabel.SetLabelPos();
        }

        public void SetLabelPos()
        {
            idLabel.SetLabelPos();
        }

        public void SetValLabelPos(int x, int y, int rot)
        {
            valLabel.labelx = x;
            valLabel.labely = y;
            valLabel.labelrot = rot;
            valLabel.SetLabelPos();
        }

        public void SetValLabelPos()
        {
            valLabel.SetLabelPos();
        }

        public void SetValue(double val)
        {
            if (Math.Abs(val) < 1e-12)
            {
                value = 0;
                mult = " ";
            }
            else
            {
                val = val * unitMult;

                int index = 4;   // We are in bare units "TGMK munp"
                unitMult = 1;
                while (Math.Abs(val) >= 1000)
                {
                    index--;
                    unitMult = unitMult * 1000;
                    val = val / 1000;
                }
                while (Math.Abs(val) < 1)
                {
                    index++;
                    unitMult = unitMult / 1000;
                    val = val * 1000;
                }
                if (index > 8)
                {
                    index = 8;
                    val = val / 1000;
                }
                mult = multUnits.Substring(index,1);
                if (mult != " ") mult=mult+" ";
                value = val;
            }
            valLabel.SetPlainText(value.ToString() + mult + unit);
        }

        public string Unit() { return mult + unit; }
        
        public void SetUnit(string un)
        {
            string mul = " ";
            un.Replace(" ", "");
            if (un.Length > 0)
            {
                mul = un.Substring(0,1);
                double unitMult = 1e12;        // We start in Tera units "TGMk munp"

                for (int x = 0; x < 9; x++)
                {
                    if (mul == multUnits.Substring(x,1))
                    {
                        this.unitMult = unitMult;
                        mult = mul;
                        if (mult != " ") mult=mult+" ";
                        valLabel.SetPlainText(value.ToString() + mult + unit);
                        return;
                    }
                    unitMult = unitMult / 1000;
                }
            }
            unitMult = 1;
            mult = " ";
            valLabel.SetPlainText(value.ToString() + mult + unit);
        }

        public double GetmultValue() { return value * unitMult; }

        public bool ShowId() { return showId; }
        
        public void SetShowId(bool show)
        {
            idLabel.SetVisible(show);
            showId = show;
        }

        public bool ShowVal() { return showVal; }
        public void SetShowVal(bool show)
        {
            valLabel.SetVisible(show);
            showVal = show;
        }

        public string IdLabel() { return idLabel.ToPlainText(); }
        public void SetIdLabel(string id) { idLabel.SetPlainText(id); }

        public string ItemID() { return id; }
        public void SetId(string id) { id = id; }

        public double LabelX() { return idLabel.labelx; }
        public void SetLabelX(double x) { idLabel.labelx = x; }

        public double LabelY() { return idLabel.labely; }
        public void SetLabelY(double y) { idLabel.labely = y; }

        public double LabelRot() { return idLabel.labelrot; }
        public void SetLabelRot(double rot) { idLabel.labelrot = rot; }

        //int Component::valLabelx() { return m_valLabel->m_labelx; }
        //void Component::setValLabelX(int x) { m_valLabel->m_labelx = x; }

        //int Component::valLabely() { return m_valLabel->m_labely; }
        //void Component::setValLabelY(int y) { m_valLabel->m_labely = y; }

        //int Component::valLabRot() { return m_valLabel->m_labelrot; }
        //void Component::setValLabRot(int rot) { m_valLabel->m_labelrot = rot; }

        //int Component::hflip() { return m_Hflip; }
        //void Component::setHflip(int hf)
        //{
        //    if ((hf != 1) & (hf != -1)) hf = 1;
        //    m_Hflip = hf;
        //    setflip();
        //}

        //int Component::vflip() { return m_Vflip; }
        //void Component::setVflip(int vf)
        //{
        //    if ((vf != 1) & (vf != -1)) vf = 1;
        //    m_Vflip = vf;
        //    setflip();
        //}

        //void Component::setflip()
        //{
        //    setTransform(QTransform::fromScale(m_Hflip, m_Vflip));
        //    m_idLabel->setTransform(QTransform::fromScale(m_Hflip, m_Vflip));
        //    m_valLabel->setTransform(QTransform::fromScale(m_Hflip, m_Vflip));
        //    emit moved();
        //}

        //QString Component::itemType() { return m_type; }
        //QString Component::category() { return m_category; }
        //QIcon Component::icon() { return m_icon; }

        ////bool Component::isChanged(){ return m_changed;}

        //void Component::setPrintable(bool p)
        //{
        //    m_printable = p;
        //}

        //QString Component::print()
        //{
        //    if (!m_printable) return "";

        //    QString str = m_id + " : ";
        //    str += objectName().split("-").first() + " ";
        //    if (m_value > 0) str += QString::number(m_value);
        //    str += m_mult + m_unit + "\n";

        //    return str;
        //}

        //void Component::paint(QPainter* painter, const QStyleOptionGraphicsItem* option, QWidget* widget)
        //{
        //    Q_UNUSED(option); Q_UNUSED(widget);

        //    QPen pen(Qt::black, 1.5, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin);

        //    if (isSelected())
        //    {
        //        pen.setColor(Qt::darkGray);
        //        painter->setBrush(Qt::darkGray);
        //        //label->setBrush( Qt::darkGray );
        //    }
        //    else
        //    {
        //        painter->setBrush(m_color);
        //        //label->setBrush( Qt::darkBlue );
        //    }
        //    //painter->setBrush( Qt::yellow );
        //    //painter->drawRect( boundingRect() );

        //    painter->setPen(pen);
        //}





        //        Q_OBJECT
        //Q_INTERFACES(QGraphicsItem)

        //    Q_PROPERTY(QString itemtype  READ itemType  USER true )
        //    Q_PROPERTY(QString id        READ idLabel   WRITE setIdLabel DESIGNABLE true USER true )
        //    Q_PROPERTY(bool Show_id   READ showId    WRITE setShowId  DESIGNABLE true USER true )
        //    Q_PROPERTY(qreal rotation  READ rotation  WRITE setRotation)
        //    Q_PROPERTY(int x         READ x         WRITE setX)
        //    Q_PROPERTY(int y         READ y         WRITE setY)
        //    Q_PROPERTY(int labelx    READ labelx    WRITE setLabelX)
        //    Q_PROPERTY(int labely    READ labely    WRITE setLabelY)
        //    Q_PROPERTY(int labelrot  READ labelRot  WRITE setLabelRot)
        //    Q_PROPERTY(int valLabelx READ valLabelx WRITE setValLabelX)
        //    Q_PROPERTY(int valLabely READ valLabely WRITE setValLabelY)
        //    Q_PROPERTY(int valLabRot READ valLabRot WRITE setValLabRot)
        //    Q_PROPERTY(int hflip     READ hflip     WRITE setHflip)
        //    Q_PROPERTY(int vflip     READ vflip     WRITE setVflip)

        //    public:
        //        QRectF boundingRect() const { return QRectF(m_area.x()-2, m_area.y()-2, m_area.width()+4 , m_area.height()+4 );
        //    }

        //    Component(QObject* parent, QString type, QString id);
        //    ~Component();

        //    enum { Type = UserType + 1 };
        //    int type() const { return Type; }

        //QString idLabel();
        //void setIdLabel(QString id);

        //QString itemID();
        //void setId(QString id);

        //bool showId();
        //void setShowId(bool show);

        //bool showVal();
        //void setShowVal(bool show);

        //QString unit();
        //void setUnit(QString un);

        //int labelx();
        //void setLabelX(int x);

        //int labely();
        //void setLabelY(int y);

        //int labelRot();
        //void setLabelRot(int rot);

        //void setLabelPos(int x, int y, int rot = 0);
        //void setLabelPos();

        //int valLabelx();
        //void setValLabelX(int x);

        //int valLabely();
        //void setValLabelY(int y);

        //int valLabRot();
        //void setValLabRot(int rot);

        //int hflip();
        //void setHflip(int hf);

        //int vflip();
        //void setVflip(int vf);

        //void setValLabelPos(int x, int y, int rot);
        //void setValLabelPos();

        //void updateLabel(Label* label, QString txt);

        //double getmultValue();

        ////QString getHelp( QString file );
        //virtual void setBackground(QString bck) { m_BackGround = bck; }

        //void setPrintable(bool p);
        //QString print();

        //QString itemType();
        //QString category();
        //QIcon icon();

        //virtual void inStateChanged(int ) { }

        //virtual void move(QPointF delta);
        //void moveTo(QPointF pos);

        //virtual void paint(QPainter* painter, const QStyleOptionGraphicsItem* option, QWidget* widget);

        //signals:
        //        void moved();

        //public slots:
        //        virtual void slotProperties();
        //virtual void rotateCW();
        //virtual void rotateCCW();
        //virtual void rotateHalf();
        //virtual void H_flip();
        //virtual void V_flip();
        //virtual void slotRemove();
        //void slotCopy();

        //virtual void remove();

        //protected:
        //        void mousePressEvent(QGraphicsSceneMouseEvent* event );
        //void mouseDoubleClickEvent(QGraphicsSceneMouseEvent* event );
        //void mouseMoveEvent(QGraphicsSceneMouseEvent* event );
        //void mouseReleaseEvent(QGraphicsSceneMouseEvent* event );
        //void contextMenuEvent(QGraphicsSceneContextMenuEvent* event );
        //void contextMenu(QGraphicsSceneContextMenuEvent* event, QMenu* menu);

        //void setValue(double val);
        //void setflip();

        protected double value;

        protected string multUnits;
        string unit;
        string mult;
        double unitMult;

        int Hflip;
        int Vflip;
        static int error;

        Label idLabel;
        Label valLabel;

        string id;
        string type;
        string category;
        string BackGround;   // BackGround Image

        string help;

        //Icon icon;
        Color color;
   //     Rect area;         // bounding rect
   //     Point eventpoint;

        bool showId;
        bool showVal;
        bool moving;
        bool printable;

    //    Pin[] pin;

        //typedef Component* (* createItemPtr) (QObject* parent, QString type, QString id);
    }
}






//Подсказка

//class CustomCanvas : Canvas
//{
//    protected override void OnRender(DrawingContext dc)
//    {
//        FormattedText someFormattedText = new FormattedText(someText, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
//                someTypeFace, someFontSize, someColor);
//        dc.DrawText(someFormattedText, new Point(15, 15));
//    }
//}