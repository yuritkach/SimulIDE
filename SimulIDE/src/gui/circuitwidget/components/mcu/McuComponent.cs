using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.gui.circuitwidget.components.mcu
{
    public class McuComponent:Chip
    {


////        static const char* McuComponent_properties[] = {
////    QT_TRANSLATE_NOOP("App::Property","Program"),
////    QT_TRANSLATE_NOOP("App::Property","Mhz"),
////    QT_TRANSLATE_NOOP("App::Property","Auto Load"),
////    QT_TRANSLATE_NOOP("App::Property","Init gdb")
////};

        private static McuComponent self = null;
        public static McuComponent Self() { return self; }
        public bool canCreate = true;

        public McuComponent(object parent, string type, string id):base(parent, type, id )
//??????            , MemData()
        {
            //    Q_UNUSED(McuComponent_properties);

            //            qDebug() << "        Initializing" << m_id << "...";
            self = this;
            canCreate = false;
//            attached = false;
//            autoLoad = false;

//            processor = null;
//            symbolFile = "";
//            device = "";
//            error = 0;

            // Id Label Pos set in Chip::initChip
//            m_color = QColor(50, 50, 70);

//            QSettings* settings = MainWindow::self()->settings();
//            m_lastFirmDir = settings->value("lastFirmDir").toString();

//            if (m_lastFirmDir.isEmpty())
//                m_lastFirmDir = QCoreApplication::applicationDirPath();

//            Simulator::self()->addToUpdateList(this);
        }

//        void McuComponent::runAutoLoad()
//        {
//            if (m_processor->getLoadStatus())
//            {
//                //qDebug() << "McuComponent::runAutoLoad Autoreloading";
//                if (m_autoLoad) load(m_symbolFile);
//                emit openSerials();
//            }
//        }

//        void McuComponent::updateStep()
//        {
//            m_processor->getRamTable()->updateValues();
//        }

//        void McuComponent::initChip()
//        {
//            QString compName = m_id.split("-").first(); // for example: "atmega328-1" to: "atmega328"

//            QString dataFile = ComponentSelector::self()->getXmlFile(compName);

//            QFile file(dataFile );

//            if ((dataFile == "") || (!file.exists()))
//            {
//                m_error = 1;
//                return;
//            }
//            if (!file.open(QFile::ReadOnly | QFile::Text))
//            {
//                MessageBoxNB("Error", tr("Cannot read file %1:\n%2.").arg(dataFile).arg(file.errorString()));
//                m_error = 1;
//                return;
//            }
//            QDomDocument domDoc;

//            if (!domDoc.setContent(&file))
//            {
//                MessageBoxNB("Error", tr("Cannot set file %1\nto DomDocument").arg(dataFile));
//                file.close();
//                m_error = 1;
//                return;
//            }
//            file.close();

//            QDomElement root = domDoc.documentElement();
//            QDomNode rNode = root.firstChild();
//            QString package;

//            while (!rNode.isNull())
//            {
//                QDomElement element = rNode.toElement();
//                QDomNode node = element.firstChild();

//                while (!node.isNull())
//                {
//                    QDomElement element = node.toElement();
//                    if (element.attribute("name") == compName)
//                    {
//                        // Get package file
//                        QDir dataDir(dataFile );
//                        dataDir.cdUp();             // Indeed it doesn't cd, just take out file name
//                        m_pkgeFile = dataDir.filePath(element.attribute("package")) + ".package";

//                        // Get device
//                        m_device = element.attribute("device");
//                        m_processor->setDevice(m_device);

//                        // Get data file
//                        QString dataFile = dataDir.filePath(element.attribute("data")) + ".data";

//                        m_processor->setDataFile(dataFile);
//                        if (element.hasAttribute("icon")) m_BackGround = ":/" + element.attribute("icon");

//                        break;
//                    }
//                    node = node.nextSibling();
//                }
//                rNode = rNode.nextSibling();
//            }
//            if (m_device != "") Chip::initChip();
//            else
//            {
//                m_error = 1;
//                qDebug() << compName << "ERROR!! McuComponent::initChip Chip not Found: " << package;
//            }
//        }

//        void McuComponent::updatePin(QString id, QString type, QString label, int pos, int xpos, int ypos, int angle)
//        {
//            Pin* pin = 0l;
//            for (int i = 0; i < m_pinList.size(); i++)
//            {
//                McuComponentPin* mcuPin = m_pinList[i];
//                QString pinId = mcuPin->id();

//                if (id == pinId)
//                {
//                    pin = mcuPin->pin();
//                    break;
//                }
//            }
//            if (!pin)
//            {
//                qDebug() << "McuComponent::updatePin Pin Not Found:" << id << type << label;
//                return;
//            }

//            pin->setLabelText(label);
//            pin->setPos(QPoint(xpos, ypos));

//            if (angle == 0) m_rigPin.append(pin);
//            else if (angle == 90) m_topPin.append(pin);
//            else if (angle == 180) m_lefPin.append(pin);
//            else if (angle == 270) m_botPin.append(pin);

//            pin->setPinAngle(angle);
//            pin->setLabelPos();

//            type = type.toLower();

//            if (type == "gnd"
//             || type == "vdd"
//             || type == "vcc"
//             || type == "unused"
//             || type == "nc") pin->setUnused(true);
//            else pin->setUnused(false);

//            if (type == "inverted") pin->setInverted(true);
//            else pin->setInverted(false);

//            if (type == "null")
//            {
//                pin->setVisible(false);
//                pin->setLabelText("");
//            }
//            else pin->setVisible(true);

//            if (m_isLS) pin->setLabelColor(QColor(0, 0, 0));
//            else pin->setLabelColor(QColor(250, 250, 200));

//            pin->isMoved();
//        }

//        double McuComponent::freq()
//        {
//            return m_freq;
//        }

//        void McuComponent::setFreq(double freq)
//        {
//            if (freq < 0) freq = 0;
//            else if (freq > 100) freq = 100;

//            m_freq = freq;
//        }

//        void McuComponent::reset()
//        {
//            for (int i = 0; i < m_pinList.size(); i++) // Reset pins states
//                m_pinList[i]->resetState();

//            m_processor->reset();
//        }

//        void McuComponent::terminate()
//        {
//            qDebug() << "        Terminating" << m_id << "...";

//            m_processor->terminate();

//            m_pSelf = 0l;

//            qDebug() << "     ..." << m_id << "Terminated\n";
//        }

//        void McuComponent::remove()
//        {
//            emit closeSerials();

//            Simulator::self()->remFromUpdateList(this);
//            for (McuComponentPin* mcupin : m_pinList)
//            {
//                Pin* pin = mcupin->pin();
//                if (pin->connector()) pin->connector()->remove();
//            }
//            terminate();
//            m_pinList.clear();

//            m_canCreate = true;

//            Component::remove();
//        }

//        void McuComponent::contextMenuEvent(QGraphicsSceneContextMenuEvent* event )
//{
//            if (!acceptedMouseButtons()) event->ignore();
//    else
//    {
//        event->accept();
//        QMenu* menu = new QMenu();
//        contextMenu( event, menu );
//        menu->deleteLater();
//    }
//}

//void McuComponent::contextMenu(QGraphicsSceneContextMenuEvent* event, QMenu* menu)
//{
//    QAction* loadAction = menu->addAction(QIcon(":/load.png"), tr("Load firmware"));
//    connect(loadAction, SIGNAL(triggered()), this, SLOT(slotLoad()));

//    QAction* reloadAction = menu->addAction(QIcon(":/reload.png"), tr("Reload firmware"));
//    connect(reloadAction, SIGNAL(triggered()), this, SLOT(slotReload()));

//    QAction* loadDaAction = menu->addAction(QIcon(":/load.png"), tr("Load EEPROM data"));
//    connect(loadDaAction, SIGNAL(triggered()), this, SLOT(loadData()));

//    QAction* saveDaAction = menu->addAction(QIcon(":/save.png"), tr("Save EEPROM data"));
//    connect(saveDaAction, SIGNAL(triggered()), this, SLOT(saveData()));

//    menu->addSeparator();

//    QAction* openTerminal = menu->addAction(QIcon(":/terminal.png"), tr("Open Serial Monitor."));
//    connect(openTerminal, SIGNAL(triggered()), this, SLOT(slotOpenTerm()));

//    QAction* openSerial = menu->addAction(QIcon(":/terminal.png"), tr("Open Serial Port."));
//    connect(openSerial, SIGNAL(triggered()), this, SLOT(slotOpenSerial()));

//    menu->addSeparator();

//    Component::contextMenu( event, menu);
//}

//void McuComponent::slotOpenSerial()
//{
//    Component* ser = Circuit::self()->createItem("SerialPort", "SerialPort-" + Circuit::self()->newSceneId());
//    ser->setPos(pos());
//    Circuit::self()->addItem(ser);

//    /*m_shield = new Shield( this, "Shield1", "shield1");
//    m_shield->setBackground( ":/shield_uno.png");
//    m_shield->setPos( 0,0  );
//    Circuit::self()->addItem( m_shield );
//    Circuit::self()->compList()->removeOne( m_shield );
//    m_shield->setParentItem( this );*/
//}

//void McuComponent::slotOpenTerm()
//{
//    Component* ser = Circuit::self()->createItem("SerialTerm", "SerialTerm-" + Circuit::self()->newSceneId());
//    ser->setPos(pos());
//    Circuit::self()->addItem(ser);
//}

//void McuComponent::slotLoad()
//{
//    const QString dir = m_lastFirmDir;
//    QString fileName = QFileDialog::getOpenFileName(0l, tr("Load Firmware"), dir,
//                       tr("All files (*.*);;ELF Files (*.elf);;Hex Files (*.hex)"));

//    if (fileName.isEmpty()) return; // User cancels loading

//    load(fileName);
//}

//void McuComponent::slotReload()
//{
//    if (m_processor->getLoadStatus()) load(m_symbolFile);
//    else QMessageBox::warning(0, tr("No File:"), tr("No File to reload "));
//}

//void McuComponent::load(QString fileName)
//{
//    QDir circuitDir = QFileInfo(Circuit::self()->getFileName()).absoluteDir();
//    QString fileNameAbs = circuitDir.absoluteFilePath(fileName);
//    QString cleanPathAbs = circuitDir.cleanPath(fileNameAbs);

//    bool pauseSim = Simulator::self()->isRunning();
//    if (pauseSim) Simulator::self()->pauseSim();

//    if (m_processor->loadFirmware(cleanPathAbs))
//    {
//        if (!m_attached) attachPins();
//        reset();

//        m_symbolFile = circuitDir.relativeFilePath(fileName);
//        m_lastFirmDir = cleanPathAbs;

//        QSettings* settings = MainWindow::self()->settings();
//        settings->setValue("lastFirmDir", m_symbolFile);

//        //checkPcLink();
//    }
//    //else QMessageBox::warning( 0, tr("Error:"), tr("Could not load: \n")+ fileName );

//    if (pauseSim) Simulator::self()->runContinuous();
//}

//void McuComponent::setProgram(QString pro)
//{
//    if (pro == "") return;
//    m_symbolFile = pro;

//    QDir circuitDir = QFileInfo(Circuit::self()->getFileName()).absoluteDir();
//    QString fileNameAbs = circuitDir.absoluteFilePath(m_symbolFile);

//    if (QFileInfo::exists(fileNameAbs)  // Load firmware at circuit load
//     && !m_processor->getLoadStatus())
//    {
//        load(m_symbolFile);
//    }
//}

//void McuComponent::setEeprom(QVector<int> eep)
//{
//    m_processor->setEeprom(eep);
//}

//QVector<int> McuComponent::eeprom()
//{
//    QVector<int> eep = m_processor->eeprom();
//    return eep;
//}

//void McuComponent::loadData()
//{
//    QVector<int> data = m_processor->eeprom();

//    bool resize = false;
//    if (!m_processor->getLoadStatus()) resize = true; // No eeprom initialized yet

//    MemData::loadData(&data, resize);
//    m_processor->setEeprom(data);
//}

//void McuComponent::saveData()
//{
//    QVector<int> data = m_processor->eeprom();
//    MemData::saveData(data);
//}

//void McuComponent::setLogicSymbol(bool ls)
//{
//    if (!m_initialized) return;
//    if (m_isLS == ls) return;
//    //qDebug() <<"SubCircuit::setLogicSymbol"<<ls<<m_pkgeFile;

//    bool pauseSim = Simulator::self()->isRunning();
//    if (pauseSim) Simulator::self()->pauseSim();

//    if (ls && m_pkgeFile.endsWith(".package")) m_pkgeFile.replace(".package", "_LS.package");
//    if (!ls && m_pkgeFile.endsWith("_LS.package")) m_pkgeFile.replace("_LS.package", ".package");

//    m_error = 0;
//    Chip::initChip();

//    if (m_error == 0)
//    {
//        Circuit::self()->saveState();
//        Circuit::self()->update();
//    }

//    if (pauseSim) Simulator::self()->runContinuous();
//}

//void McuComponent::paint(QPainter* p, const QStyleOptionGraphicsItem* option, QWidget* widget)
//{
//    Chip::paint(p, option, widget);
//}







//Q_PROPERTY(QVector<int> eeprom  READ eeprom   WRITE setEeprom)
//    Q_PROPERTY(QString Program     READ program  WRITE setProgram  DESIGNABLE true  USER true )
//    Q_PROPERTY(double Mhz         READ freq     WRITE setFreq     DESIGNABLE true  USER true )
//    Q_PROPERTY(bool Auto_Load   READ autoLoad WRITE setAutoLoad DESIGNABLE true  USER true )

//    public:

//        McuComponent(QObject* parent, QString type, QString id);
//        ~McuComponent();

//        static McuComponent* self() { return m_pSelf; }

//        virtual void updateStep();
//        virtual void runAutoLoad();

//        QString program()   const { return  m_symbolFile; }
//    void setProgram(QString pro);

//    double freq();
//    virtual void setFreq(double freq);

//    bool autoLoad() { return m_autoLoad; }
//    void setAutoLoad(bool al) { m_autoLoad = al; }

//    QString device() { return m_device; }

//    virtual void initChip();

//    void setEeprom(QVector<int> eep);
//    QVector<int> eeprom();

//    virtual void setLogicSymbol(bool ls);

//    QList<McuComponentPin*> getPinList() { return m_pinList; }

//    virtual void paint(QPainter* p, const QStyleOptionGraphicsItem* option, QWidget* widget);

//    signals:
//        void closeSerials();
//    void openSerials();

//    public slots:
//        virtual void terminate();
//    virtual void remove();
//    virtual void reset();
//    virtual void load(QString fileName);
//    void slotLoad();
//    void slotReload();
//    void slotOpenTerm();
//    void slotOpenSerial();

//    void loadData();
//    void saveData();

//    void contextMenu(QGraphicsSceneContextMenuEvent* event, QMenu* menu);

//    protected:
// static McuComponent* m_pSelf;
//    static bool m_canCreate;

//    virtual void contextMenuEvent(QGraphicsSceneContextMenuEvent* event);

//    virtual void addPin(QString id, QString type, QString label,
//                         int pos, int xpos, int ypos, int angle)=0;

//        virtual void updatePin(QString id, QString type, QString label,
//                                int pos, int xpos, int ypos, int angle);

//    virtual void attachPins()=0;

//        BaseProcessor* m_processor;

//    double m_freq;           // Clock Frequency Mhz

//    bool m_attached;
//    bool m_autoLoad;

//    QString m_device;       // Name of device
//    QString m_symbolFile;   // firmware file loaded
//    QString m_lastFirmDir;  // Last firmware folder used

//    QList<McuComponentPin*> m_pinList;

//    Shield* m_shield;

//    friend class Shield;
//};




    }
}
