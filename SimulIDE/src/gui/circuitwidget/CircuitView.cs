using SimulIDE.src.gui.circuitwidget.components;
using SimulIDE.src.gui.graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimulIDE.src.gui.circuitwidget
{
    public class CircuitView:Drawable
    {
        private static CircuitView self = null;
        public static CircuitView Self() { return self; }

        public CircuitView(Canvas parent)
        {
            self = this;

     //       circuit = null;
            enterItem = null;
            Clear();

            //viewport()->setFixedSize(3200, 2400);
            //setViewportUpdateMode( QGraphicsView::FullViewportUpdate );
            //setCacheMode( CacheBackground );
            //setRenderHint( QPainter::Antialiasing );
            //            setRenderHints(QPainter::HighQualityAntialiasing | QPainter::TextAntialiasing | QPainter::SmoothPixmapTransform);
            //            //setRenderHint( QPainter::SmoothPixmapTransform );
            //            setTransformationAnchor(AnchorUnderMouse);
            //            setResizeAnchor(AnchorUnderMouse);
            //            setDragMode(QGraphicsView::RubberBandDrag);
            //            setAcceptDrops(true);
            //            m_info = new QPlainTextEdit(this);
            //            m_info->setLineWrapMode(QPlainTextEdit::NoWrap);
            //            m_info->setMinimumSize(500, 30);
            //            m_info->setPlainText("Time: 00:00:00.000000");
            //            m_info->setWindowFlags(Qt::FramelessWindowHint);
            //            m_info->setAttribute(Qt::WA_NoSystemBackground);
            //            m_info->setAttribute(Qt::WA_TranslucentBackground);
            //            m_info->setAttribute(Qt::WA_TransparentForMouseEvents);
            //            m_info->setStyleSheet("color: #884433;background-color: rgba(0,0,0,0)");
            //            m_info->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);

            //            double fontScale = MainWindow::self()->fontScale();
            //            QFont font = m_info->font();
            //            font.setBold(true);
            //            font.setPixelSize(int(10 * fontScale));
            //            m_info->setFont(font);

            //            m_info->setMaximumSize(320 * fontScale, 20 * fontScale);
            //            m_info->setMinimumSize(400, 25);
            //            m_info->show();

        }


        public void SetCircTime(UInt64 step)
        {
        //            int hours = step / 3600e6;
        //            step -= hours * 3600e6;
        //            int mins = step / 60e6;
        //            step -= mins * 60e6;
        //            int secs = step / 1e6;
        //            int mSecs = step - secs * 1e6;

        //            QString strH = QString::number(hours);
        //            if (strH.length() < 2) strH = "0" + strH;
        //            QString strM = QString::number(mins);
        //            if (strM.length() < 2) strM = "0" + strM;
        //            QString strS = QString::number(secs);
        //            if (strS.length() < 2) strS = "0" + strS;
        //            QString strMS = QString::number(mSecs);
        //            while (strMS.length() < 6) strMS = "0" + strMS;

        //            QString strMcu = "";

        //            if (McuComponent::self())
        //            {
        //                QString device = McuComponent::self()->device();
        //                QString freq = QString::number(McuComponent::self()->freq());
        //                strMcu = "      Mcu: " + device + " at " + freq + " MHz";
        //            }
        //            m_info->setPlainText(tr("Time: ") + strH + ":" + strM + ":" + strS + "." + strMS + strMcu);
        }

        public void Clear()
        {
        //            if (m_circuit)
        //            {
        //                m_circuit->remove();
        //                m_circuit->deleteLater();
        //            }
        //            resetMatrix();

        //            m_enterItem = 0l;

        //            m_circuit = new Circuit(-1600, -1200, 3200, 2400, this);
        //            setScene(m_circuit);
        //            centerOn(900, 600);
        //            //setCircTime( 0 );
        }

        //        void CircuitView::wheelEvent(QWheelEvent*event ) 
        //{
        //            qreal scaleFactor = pow(2.0, event->delta() / 700.0);
        //    scale(scaleFactor, scaleFactor );
        //    }

        //    void CircuitView::dragEnterEvent(QDragEnterEvent*event)
        //    {
        //    event->accept();
        //        m_enterItem = 0l;

        //        QString type = event->mimeData()->html();
        //        QString id = event->mimeData()->text();

        //        if (type.isEmpty() || id.isEmpty()) return;

        //        m_enterItem = m_circuit->createItem(type, id + "-" + m_circuit->newSceneId());
        //        if (m_enterItem)
        //        {
        //            Circuit::self()->saveState();
        //            if (type == "Subcircuit")
        //            {
        //                SubCircuit* subC = static_cast<SubCircuit*>(m_enterItem);
        //                subC->setLogicSymbol(true);
        //            }

        //            m_enterItem->setPos(mapToScene( event->pos()));
        //            m_circuit->addItem(m_enterItem);
        //            //qDebug()<<"CircuitView::dragEnterEvent"<<m_enterItem->itemID()<< type<< id;
        //        }
        //    }

        //    void CircuitView::dragMoveEvent(QDragMoveEvent*event)
        //    {
        //    event->accept();
        //        if (m_enterItem) m_enterItem->moveTo(togrid(mapToScene( event->pos())));
        //    }

        //    void CircuitView::dragLeaveEvent(QDragLeaveEvent*event)
        //    {
        //    event->accept();
        //        if (m_enterItem)
        //        {
        //            m_circuit->removeComp(m_enterItem);
        //            m_enterItem = 0l;
        //        }
        //    }

        //    void CircuitView::resizeEvent(QResizeEvent*event )
        //    {
        //        int width = event->size().width();
        //        int height = event->size().height();

        //        m_circuit->setSceneRect(-width / 2 + 2, -height / 2 + 2, width - 4, height - 4);

        //        QGraphicsView::resizeEvent(event);
        //    }

        //    void CircuitView::mousePressEvent(QMouseEvent* event )
        //    {
        //        if ( event->button() == Qt::MidButton )
        //    {
        //        event->accept();
        //            setDragMode(QGraphicsView::ScrollHandDrag);

        //            QGraphicsView::mousePressEvent( event );
        //            //qDebug() << "CircuitView::mousePressEvent"<<event->isAccepted();
        //            if (!event->isAccepted() )
        //        {
        //                QMouseEvent eve(QEvent::MouseButtonPress, event->pos(),
        //                    Qt::LeftButton, Qt::LeftButton, Qt::NoModifier   );
        //                QGraphicsView::mousePressEvent(&eve);
        //            }
        //        }
        //    else
        //    {
        //            QGraphicsView::mousePressEvent( event );
        //            //viewport()->setCursor( Qt::ArrowCursor );
        //        }
        //    }

        //    void CircuitView::mouseReleaseEvent(QMouseEvent* event )
        //    {
        //        if ( event->button() == Qt::MidButton )
        //    {
        //        event->accept();
        //            QMouseEvent eve(QEvent::MouseButtonRelease, event->pos(),
        //                Qt::LeftButton, Qt::LeftButton, Qt::NoModifier   );

        //            QGraphicsView::mouseReleaseEvent(&eve);
        //        }
        //    else
        //    {
        //            QGraphicsView::mouseReleaseEvent( event );
        //            //viewport()->setCursor( Qt::ArrowCursor );
        //        }
        //        viewport()->setCursor(Qt::ArrowCursor);
        //        setDragMode(QGraphicsView::RubberBandDrag);
        //    }

        //    void CircuitView::contextMenuEvent(QContextMenuEvent* event)
        //    {
        //        QGraphicsView::contextMenuEvent( event );

        //        if (m_circuit->is_constarted()) m_circuit->deleteNewConnector();
        //        else if (!event->isAccepted() )
        //    {
        //            QPointF eventPos = mapToScene( event->globalPos());
        //            m_eventpoint = mapToScene( event->pos());

        //            QMenu menu;

        //            QAction* pasteAction = menu.addAction(QIcon(":/paste.png"), tr("Paste") + "\tCtrl+V");
        //            connect(pasteAction, SIGNAL(triggered()), this, SLOT(slotPaste()));

        //            QAction* undoAction = menu.addAction(QIcon(":/undo.png"), tr("Undo") + "\tCtrl+Z");
        //            connect(undoAction, SIGNAL(triggered()), Circuit::self(), SLOT(undo()));

        //            QAction* redoAction = menu.addAction(QIcon(":/redo.png"), tr("Redo") + "\tCtrl+Y");
        //            connect(redoAction, SIGNAL(triggered()), Circuit::self(), SLOT(redo()));
        //            menu.addSeparator();

        //            /*QAction* openCircAct = menu.addAction(QIcon(":/opencirc.png"), tr("Open Circuit")+"\tCtrl+O" );
        //            connect(openCircAct, SIGNAL(triggered()), CircuitWidget::self(), SLOT(openCirc()));

        //            QAction* newCircAct = menu.addAction( QIcon(":/newcirc.png"), tr("New Circuit")+"\tCtrl+N" );
        //            connect( newCircAct, SIGNAL(triggered()), CircuitWidget::self(), SLOT(newCircuit()));

        //            QAction* saveCircAct = menu.addAction(QIcon(":/savecirc.png"), tr("Save Circuit")+"\tCtrl+S" );
        //            connect(saveCircAct, SIGNAL(triggered()), CircuitWidget::self(), SLOT(saveCirc()));

        //            QAction* saveCircAsAct = menu.addAction(QIcon(":/savecircas.png"),tr("Save Circuit As...")+"\tCtrl+Shift+S" );
        //            connect(saveCircAsAct, SIGNAL(triggered()), CircuitWidget::self(), SLOT(saveCircAs()));
        //            menu.addSeparator();*/

        //            QAction* importCircAct = menu.addAction(QIcon(":/opencirc.png"), tr("Import Circuit"));
        //            connect(importCircAct, SIGNAL(triggered()), this, SLOT(importCirc()));

        //            QAction* saveImgAct = menu.addAction(QIcon(":/saveimage.png"), tr("Save Circuit as Image"));
        //            connect(saveImgAct, SIGNAL(triggered()), this, SLOT(saveImage()));

        //            QAction* createSubCircAct = menu.addAction(QIcon(":/load.png"), tr("Create SubCircuit"));
        //            connect(createSubCircAct, SIGNAL(triggered()), Circuit::self(), SLOT(createSubcircuit()));

        //            QAction* createBomAct = menu.addAction(QIcon(":/savecirc.png"), tr("Bill of Materials"));
        //            connect(createBomAct, SIGNAL(triggered()), Circuit::self(), SLOT(bom()));

        //            menu.exec(mapFromScene(eventPos));
        //        }
        //    }

        public void ImportCirc()
        {
        //        Circuit::self()->importCirc(m_eventpoint);
        }

        public void SlotPaste()
        {
        //        Circuit::self()->paste(m_eventpoint);
        }

        public void SaveImage()
        {
        //        QString circPath = Circuit::self()->getFileName();
        //        circPath.replace(".simu", ".png");

        //        QString fileName = QFileDialog::getSaveFileName(this
        //                                , tr("Save as Image")
        //                                , circPath
        //                                , "PNG (*.png);;JPEG (*.jpeg);;BMP (*.bmp);;SVG (*.svg);;All (*.*)");
        //        if (!fileName.isNull())
        //        {
        //            if (fileName.endsWith(".svg"))
        //            {
        //                QSvgGenerator svgGen;

        //                svgGen.setFileName(fileName);
        //                svgGen.setSize(QSize(3200, 2400));
        //                svgGen.setViewBox(QRect(0, 0, 3200, 2400));
        //                svgGen.setTitle(tr("Circuit Name"));
        //                svgGen.setDescription(tr("Generated by SimulIDE"));

        //                QPainter painter( &svgGen );
        //                Circuit::self()->render(&painter);
        //            }
        //            else
        //            {
        //                QPixmap pixMap = this->grab();
        //                pixMap.save(fileName);
        //            }
        //        }
        }

        //        QPlainTextEdit* m_info;

        Component enterItem;
    //    Circuit circuit;

        Point eventpoint;
        


    }
}
