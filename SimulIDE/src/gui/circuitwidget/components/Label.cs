using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimulIDE.src.gui.circuitwidget.components
{
    public class Label:Canvas
    {

        public Label(Canvas parent,string text,double x, double y,double rotate,int fontsize, Color color)  //     : QGraphicsTextItem(parent )
        {
            parentComp = parent;
            parentComp.Children.Add(this);
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            labelrot = rotate;
            this.text = text;
            this.color = color;
            this.fontsize = fontsize;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        public void SetPlainText(string txt)
        {
            text = txt;
            InvalidateVisual();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Typeface typeface = new Typeface("arial");
            Brush brush = new SolidColorBrush(color);
            FormattedText txt = new FormattedText(text,
                  System.Globalization.CultureInfo.CurrentCulture,
                  FlowDirection.LeftToRight, typeface, 10, brush);
            Transform transform = new RotateTransform(labelrot);
            dc.PushTransform(transform);

            height = txt.Height;
            width = txt.Width;
            dc.DrawText(txt, new Point(0, 0));
            
            Pen pen = new Pen();
            pen.Thickness = 1;
            pen.Brush = brush;
            Rect rect = new Rect(0, 0, width, height);
            dc.DrawRectangle(null, pen, rect);
        }

        //void Label::focusOutEvent(QFocusEvent*event )
        //{
        //    setTextInteractionFlags(Qt::NoTextInteraction);
        //    m_parentComp->updateLabel(this, document()->toPlainText());

        //    QGraphicsTextItem::focusOutEvent(event);
        //}

        //void Label::mouseDoubleClickEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    if (!isEnabled()) return;
        //    //setTextInteractionFlags(Qt::TextEditorInteraction);

        //    QGraphicsTextItem::mouseDoubleClickEvent(event);
        //}

        //void Label::mousePressEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    if ( event->button() == Qt::LeftButton )
        //    {
        //        event->accept();
        //        setCursor(Qt::ClosedHandCursor);
        //        grabMouse();
        //    }
        //}

        //void Label::mouseMoveEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    event->accept();
        //    setPos(pos() + mapToItem(m_parentComp, event->pos()) - mapToItem(m_parentComp, event->lastPos()));
        //    m_labelx = int(pos().x());
        //    m_labely = int(pos().y());
        //}

        //void Label::mouseReleaseEvent(QGraphicsSceneMouseEvent* event )
        //{
        //    event->accept();
        //    setCursor(Qt::OpenHandCursor);
        //    ungrabMouse();
        //}

        //void Label::contextMenuEvent(QGraphicsSceneContextMenuEvent* event )
        //{
        //    if (!acceptedMouseButtons()) event->ignore();
        //    else
        //    {
        //        event->accept();
        //        QMenu menu;

        //        QAction* rotateCWAction = menu.addAction(QIcon(":/rotateCW.png"), "Rotate CW");
        //        connect(rotateCWAction, SIGNAL(triggered()), this, SLOT(rotateCW()));

        //        QAction* rotateCCWAction = menu.addAction(QIcon(":/rotateCCW.png"), "Rotate CCW");
        //        connect(rotateCCWAction, SIGNAL(triggered()), this, SLOT(rotateCCW()));

        //        QAction* rotate180Action = menu.addAction(QIcon(":/rotate180.png"), "Rotate 180º");
        //        connect(rotate180Action, SIGNAL(triggered()), this, SLOT(rotate180()));

        //        /*QAction* selectedAction = */
        //        menu.exec(event->screenPos());
        //    }
        //}

        public void SetLabelPos()
        {
//            SetX(labelx);
//            SetY(labely);
//            SetRotation(labelrot);
//            AdjustSize();
        }

        //void Label::rotateCW()
        //{
        //    if (!isEnabled()) return;
        //    setRotation(rotation() + 90);
        //    m_labelrot = int(rotation());
        //}

        //void Label::rotateCCW()
        //{
        //    if (!isEnabled()) return;
        //    setRotation(rotation() - 90);
        //    m_labelrot = int(rotation());
        //}

        //void Label::rotate180()
        //{
        //    if (!isEnabled()) return;
        //    setRotation(rotation() - 180);
        //    m_labelrot = int(rotation());
        //}

        //void Label::H_flip(int hf)
        //{
        //    if (!isEnabled()) return;
        //    setTransform(QTransform::fromScale(hf, 1));
        //    //m_idLabel->rotateCCW();
        //}

        //void Label::V_flip(int vf)
        //{
        //    if (!isEnabled()) return;
        //    setTransform(QTransform::fromScale(1, vf));
        //}

        ///*void Label::paint(QPainter* painter, const QStyleOptionGraphicsItem* option, QWidget* widget)
        //{
        //    painter->setBrush( Qt::blue );
        //    painter->drawRect( boundingRect() );
        //    QGraphicsTextItem::paint( painter, option, widget );
        //}*/


        //virtual void paint(QPainter* painter, const QStyleOptionGraphicsItem* option, QWidget* widget);

        //public slots:
        //        void rotateCW();
        //void rotateCCW();
        //void rotate180();
        //void H_flip(int hf);
        //void V_flip(int vf);
        //void updateGeometry(int, int, int);

        //protected:
        //        void mouseDoubleClickEvent(QGraphicsSceneMouseEvent*event);
        //void mousePressEvent(QGraphicsSceneMouseEvent* event);
        //void mouseMoveEvent(QGraphicsSceneMouseEvent* event);
        //void mouseReleaseEvent(QGraphicsSceneMouseEvent* event);
        //void contextMenuEvent(QGraphicsSceneContextMenuEvent* event);
        //void focusOutEvent(QFocusEvent*event);

        
        private Canvas parentComp;
        private Color color;
        private int fontsize;
        private double labelrot;
        private string text;
        private double height;
        private double width;

    }
}
