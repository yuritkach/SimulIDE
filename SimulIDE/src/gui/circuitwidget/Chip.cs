﻿using SimulIDE.src.gui.circuitwidget.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimulIDE.src.gui.circuitwidget
{
    public class Chip:Component
    {


//        static const char* Chip_properties[] = {
//    QT_TRANSLATE_NOOP("App::Property","Logic Symbol")
//};

        public Chip(string type, string id):base(type, id )
        {
            numpins = 0;
            isLS = false;
            initialized = false;
            pkgeFile = "";
            lsColor = Color.FromRgb(255, 255, 255);
            icColor = Color.FromRgb(50, 50, 70);
            SetLabelPos(area.Left, area.Top - 20, 0);
        }


        public virtual void InitChip()
        {
//            //qDebug() << "Chip::initChip m_pkgeFile"<<m_pkgeFile;

            error = 0;

//            QDir circuitDir = QFileInfo(Circuit::self()->getFileName()).absoluteDir();
//            QString fileNameAbs = circuitDir.absoluteFilePath(m_pkgeFile);

//            QFile pfile(fileNameAbs );
//            if (!pfile.exists())   // Check if package file exist, if not try LS or no LS
//            {
//                if (m_pkgeFile.endsWith("_LS.package")) m_pkgeFile.replace("_LS.package", ".package");
//                else if (m_pkgeFile.endsWith(".package")) m_pkgeFile.replace(".package", "_LS.package");
//                else qDebug() << "SubPackage::setPackage: No package files found.\nTODO: create dummy package\n";
//                fileNameAbs = circuitDir.absoluteFilePath(m_pkgeFile);
//            }
//            QFile file(fileNameAbs );
//            if (!file.open(QFile::ReadOnly | QFile::Text))
//            {
//                MessageBoxNB("Chip::initChip",
//                          tr("Cannot read file:\n%1:\n%2.").arg(m_pkgeFile).arg(file.errorString()));
//                m_error = 1;
//                return;
//            }

//            QDomDocument domDoc;
//            if (!domDoc.setContent(&file))
//            {
//                MessageBoxNB("Chip::initChip",
//                          tr("Cannot set file:\n%1\nto DomDocument").arg(m_pkgeFile));
//                file.close();
//                m_error = 2;
//                return;
//            }
//            file.close();

//            QDomElement root = domDoc.documentElement();

//            if (root.tagName() != "package")
//            {
//                MessageBoxNB("Chip::initChip",
//                          tr("Error reading Chip file:\n%1\nNo valid Chip").arg(m_pkgeFile));
//                m_error = 3;
//                return;
//            }

//            m_width = root.attribute("width").toInt();
//            m_height = root.attribute("height").toInt();
//            m_numpins = root.attribute("pins").toInt();

//            for (Pin* pin : m_pin)
//            {
//                if (!pin) continue;
//                if (pin->connector()) pin->connector()->remove();
//                if (pin->scene()) Circuit::self()->removeItem(pin);
//                pin->reset();
//                delete pin;
//            }
//            m_ePin.clear();
//            m_pin.clear();
//            m_ePin.resize(m_numpins);
//            m_pin.resize(m_numpins);

//            m_rigPin.clear();
//            m_topPin.clear();
//            m_lefPin.clear();
//            m_botPin.clear();

//            if (m_pkgeFile.endsWith("_LS.package")) m_isLS = true;
//            else m_isLS = false;

//            if (m_isLS) m_color = m_lsColor;
//            else m_color = m_icColor;

 //           area = QRect(0, 0, 8 * m_width, 8 * m_height);
//            //setTransformOriginPoint( togrid( boundingRect().center()) );

//            setShowId(true);

//            QDomNode node = root.firstChild();

//            int chipPos = 0;

//            while (!node.isNull())
//            {
//                QDomElement element = node.toElement();
//                if (element.tagName() == "pin")
//                {
//                    QString type = element.attribute("type");
//                    QString label = element.attribute("label");
//                    QString id = element.attribute("id");
//                    QString side = element.attribute("side");
//                    int pos = element.attribute("pos").toInt();

//                    int xpos = 0;
//                    int ypos = 0;
//                    int angle = 0;

//                    if (side == "left")
//                    {
//                        xpos = -8;
//                        ypos = 8 * pos;
//                        angle = 180;
//                    }
//                    else if (side == "top")
//                    {
//                        xpos = 8 * pos;
//                        ypos = -8;
//                        angle = 90;
//                    }
//                    else if (side == "right")
//                    {
//                        xpos = m_width * 8 + 8;
//                        ypos = 8 * pos;
//                        angle = 0;
//                    }
//                    else if (side == "bottom")
//                    {
//                        xpos = 8 * pos;
//                        ypos = m_height * 8 + 8;
//                        angle = 270;
//                    }
//                    chipPos++;
//                    addPin(id, type, label, chipPos, xpos, ypos, angle);
//                }
//                node = node.nextSibling();
//            }
//            m_initialized = true;
        }

        protected virtual void AddPin(string id, string type, string label, int pos, int xpos, int ypos, int angle)
        {
//            Pin* pin = new Pin(angle, QPoint(xpos, ypos), m_id + "-" + id, pos - 1, this); // pos in package starts at 1

//            //m_pinMap[id] = pin;

//            pin->setLabelText(label);

//            if (type == "inverted") pin->setInverted(true);
//            else if (type == "unused") pin->setUnused(true);
//            else if (type == "null")
//            {
//                pin->setVisible(false);
//                pin->setLabelText("");
//            }
//            if (angle == 0) m_rigPin.append(pin);
//            else if (angle == 90) m_topPin.append(pin);
//            else if (angle == 180) m_lefPin.append(pin);
//            else if (angle == 270) m_botPin.append(pin);

//            if (m_isLS) pin->setLabelColor(QColor(0, 0, 0));

//            m_ePin[pos - 1] = pin;
//            m_pin[pos - 1] = pin;
//        }

//        bool Chip::logicSymbol()
//        {
//            return m_isLS;
        }

        public virtual void SetLogicSymbol(bool ls)
        {
//            if (m_initialized && (m_isLS == ls)) return;

//            if (ls && m_pkgeFile.endsWith(".package")) m_pkgeFile.replace(".package", "_LS.package");
//            if (!ls && m_pkgeFile.endsWith("_LS.package")) m_pkgeFile.replace("_LS.package", ".package");

//            m_error = 0;
//            Chip::initChip();

//            if (m_error == 0) Circuit::self()->update();
        }

        public virtual void Remove()
        {
//            /*for( uint i=0; i<m_ePin.size(); i++ )
//            {
//                Pin* pin = static_cast<Pin*>(m_ePin[i]);
//                if( pin->connector() ) pin->connector()->remove();
//            }*/
//            Component::remove();
        }

//        void Chip::contextMenuEvent(QGraphicsSceneContextMenuEvent* event )
//{
//    event->accept();
//    QMenu* menu = new QMenu();
//        /*QAction *loadAction = menu->addAction( QIcon(":/fileopen.png"),tr("Load firmware") );
//        connect( loadAction, SIGNAL(triggered()), this, SLOT(slotLoad()) );

//        QAction *reloadAction = menu->addAction( QIcon(":/fileopen.png"),tr("Reload firmware") );
//        connect( reloadAction, SIGNAL(triggered()), this, SLOT(slotReload()) );*/

//        menu->addSeparator();

//        Component::contextMenu( event, menu );
//        menu->deleteLater();
//}

//        public virtual void Paint(QPainter* p, const QStyleOptionGraphicsItem* option, QWidget* widget)
//        {
//        Component::paint(p, option, widget);

//        p->drawRoundedRect(m_area, 1, 1);

//        if (!m_isLS)
//        {
//            p->setPen(QColor(170, 170, 150));
//            p->drawArc(boundingRect().width() / 2 - 6, -4, 8, 8, 0, -2880 /* -16*180 */ );
//        }
//        }







    //        class MAINMODULE_EXPORT Chip : public Component, public eElement
    //{
    //    Q_OBJECT
    //    Q_PROPERTY(bool Logic_Symbol READ logicSymbol WRITE setLogicSymbol DESIGNABLE true USER true )

    //    public:

    //        bool logicSymbol();
    //        virtual void setLogicSymbol(bool ls);

    public virtual void InitEpins() {}

    //        protected:
    //        virtual void contextMenuEvent(QGraphicsSceneContextMenuEvent* event);

        protected virtual void UpdatePin(string id, string type, string label,int pos, int xpos, int ypos, int angle){ }

        protected int numpins;
        protected int width;
        protected int height;

        protected bool isLS;
        protected bool initialized;

        protected Color lsColor;
        protected Color icColor;

        protected string pkgeFile;     // file containig package defs
                                       //QString m_dataFile;     // xml file containig entry

        protected List<Pin> topPin;
        protected List<Pin> botPin;
        protected List<Pin> lefPin;
        protected List<Pin> rigPin;
        


    }
}
