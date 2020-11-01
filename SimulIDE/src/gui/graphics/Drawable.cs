using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimulIDE.src.gui.graphics
{
   
        public class Drawable
        {
            public double viewPortWidth { get; set; }
            public double viewPortHeight { get; set; }

            protected double XX(OpenGL gl, double x) { return 2 / (double)gl.RenderContextProvider.Width * x - 1; }
            protected double YY(OpenGL gl, double y) { return (2 / (double)gl.RenderContextProvider.Height * y - 1) * -1; }

            public double scaleCoef { get; set; }
            public double offsetX { get; set; }
            public double offsetY { get; set; }
            public double rotateUngle { get; set; }


        public Drawable()
            {
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

        }

}
