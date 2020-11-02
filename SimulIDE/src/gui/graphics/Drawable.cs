using SharpGL;
using SharpGL.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SimulIDE.src.gui.graphics
{

    public class Drawable
    {
        public List<Drawable> Children = new List<Drawable>();

        public double ViewPortWidth { get; set; }
        public double ViewPortHeight { get; set; }


        protected double XX(OpenGL gl, double x) { return 2 / (double)gl.RenderContextProvider.Width * x - 1; }
        protected double YY(OpenGL gl, double y) { return (2 / (double)gl.RenderContextProvider.Height * y - 1) * -1; }

        public double ScaleCoef { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double RotateUngle { get; set; }


        private OpenGLControl control;
        public Drawable(OpenGLControl control)
        {
            this.control = control;
        }

        protected void DrawLine(OpenGL gl, double x1, double y1, double x2, double y2, Color clr)
        {
            gl.LineWidth(2);
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(clr.R, clr.G, clr.B, clr.A);

            var xx1 = XX(gl, x1);
            var xx2 = XX(gl, x2);
            var yy1 = YY(gl, y1);
            var yy2 = YY(gl, y2);
            gl.Vertex(xx1, yy1, -0.2f);
            gl.Vertex(xx2, yy2, -0.2f);

            gl.End();
        }

        public double VP2CX(double x)
        {
            double offset = (ViewPortWidth * ScaleCoef - ViewPortWidth) / 2f;
            //return ((x + offset) / ScaleCoef - OffsetX - OffsetLeft) / backgroundScale;
            return 0;
        }
        public double VP2CY(double y)
        {
            double offset = (ViewPortHeight * ScaleCoef - ViewPortHeight) / 2f;
            //return ((y + offset) / ScaleCoef - OffsetY - OffsetTop) / backgroundScale;
            return 0;
        }


        public virtual void SetViewPortSize(double width, double height)
        {
          ViewPortWidth = width;
          ViewPortHeight = height;
        }

        public virtual void OnMouseMove(object sender,MouseEventArgs e) { }
        public virtual void OnMouseDown(object sender, MouseButtonEventArgs e) { }
        public virtual void OnMouseUp(object sender, MouseButtonEventArgs e) { }
        public virtual void OnMouseLeave(object sender, MouseEventArgs e) { }
        public virtual void OnMouseWheel(object sender, MouseWheelEventArgs e) { }
        public virtual void OnMouseEnter(object sender, MouseEventArgs e) { }

        protected virtual void DrawSelf(OpenGL gl)
        {
        }

        protected virtual void DrawChild(OpenGL gl)
        {
            foreach (var el in Children)
                el.Draw(gl);
        }

        public virtual void Draw(OpenGL gl)
        {
            DrawSelf(gl);
            DrawChild(gl);
        }




    }

}
