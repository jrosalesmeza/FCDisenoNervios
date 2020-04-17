using B_EscalaCoordenadas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cGrid
    {
        public string Nombre { get; set; }
        public eDireccionGrid Direccion { get; set; }

        public float CoordenadaInicial { get; set; }
        public float BubbleSize { get; set; }
        public List<PointF> Recta_Real { get; set; }
        public List<PointF> Recta_Escalada { get; set; }

        public PointF PuntoBubble_Real { get; set; } 
        private PointF PuntoBubbleEscalado { get; set; }
        public RectangleF BubleSize_Objeto_Escalado { get; set; }

        public cGrid(string Nombre,float CoordenadaInicial, eDireccionGrid Direccion,float BubbleSize)
        {
            this.Nombre = Nombre;
            this.CoordenadaInicial = CoordenadaInicial;
            this.Direccion = Direccion;
            this.BubbleSize = BubbleSize;
        }

        public void CrearRecta(float Xmax, float Ymax,float Xmin,float Ymin)
        {
            Recta_Real = new List<PointF>();
            PointF Punto1;
            PointF Punto2;
            if (Direccion== eDireccionGrid.X) //VERTICALES
            {
                Punto1 = new PointF(CoordenadaInicial, Ymin);
                Punto2 = new PointF(CoordenadaInicial, Ymax+ BubbleSize*2);
                PuntoBubble_Real = new PointF(CoordenadaInicial, Ymax + BubbleSize * 3 / 2);
            }
            else //HORIZONTAL
            {
                Punto1 = new PointF(Xmin-BubbleSize*2, CoordenadaInicial);
                Punto2 = new PointF(Xmax, CoordenadaInicial);
                PuntoBubble_Real = new PointF(Xmin- BubbleSize * 3/2, CoordenadaInicial);
            }
            Recta_Real.Add(Punto1); Recta_Real.Add(Punto2);
        }

        public void CrearPuntosPlantaEscaladaEtabs(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom,bool EscalaEnEjeY=false,float XI=5f)
        {
            if (EscalaEnEjeY)
            {
                Recta_Escalada = cEscalaCoordenadas.EscalarPuntosEnEjeY(PuntosTodosObjetos, Recta_Real, WidthWindow, HeigthWindow,out float EscalaMayorX, Zoom, Dx, Dy, XI);
                PuntoBubbleEscalado = cEscalaCoordenadas.EscalarPuntoEnEjeY(PuntosTodosObjetos, PuntoBubble_Real, WidthWindow, HeigthWindow, Zoom, Dx, Dy, XI);

            }
            else
            {
                Recta_Escalada = cEscalaCoordenadas.EscalarPuntos(PuntosTodosObjetos, Recta_Real, WidthWindow, HeigthWindow, Zoom, Dx, Dy);
                PuntoBubbleEscalado = cEscalaCoordenadas.EscalarPunto(PuntosTodosObjetos, PuntoBubble_Real, WidthWindow, HeigthWindow, Zoom, Dx, Dy);

            }
            
            float RadioEscalado = Direccion == eDireccionGrid.X
                ? Math.Abs(Recta_Escalada[1].Y - PuntoBubbleEscalado.Y)
                : Math.Abs(Recta_Escalada[0].X - PuntoBubbleEscalado.X);
            BubleSize_Objeto_Escalado = cFunctionsProgram.CrearCirculo(PuntoBubbleEscalado.X, PuntoBubbleEscalado.Y, RadioEscalado);
        }
        public void Paint(Graphics e,float Zoom)
        {
            float TamanoLetra;
            if (Zoom > 0)
            {
                TamanoLetra = 9 * Zoom;
            }
            else
            {
                TamanoLetra = 1;
            }
            Pen Pen_Linea = new Pen(Color.FromArgb(230, 230, 230), 1);
            Pen Pen_Circulo = new Pen(Color.FromArgb(244, 244, 244), 2);
            SolidBrush Brush_String = new SolidBrush(Color.FromArgb(0, 104, 149));
            SolidBrush BrushCirculo = new SolidBrush(Color.FromArgb(254, 217, 132));
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);

            string NombreAMostrar = $"{Nombre}";
            PointF PointString = new PointF(PuntoBubbleEscalado.X - e.MeasureString(NombreAMostrar, Font1).Width / 2, PuntoBubbleEscalado.Y - Font1.Height / 2);
            e.DrawLines(Pen_Linea, Recta_Escalada.ToArray());
            e.DrawEllipse(Pen_Circulo, BubleSize_Objeto_Escalado);
            e.FillEllipse(BrushCirculo, BubleSize_Objeto_Escalado);
            e.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.DrawString(NombreAMostrar, Font1, Brush_String, PointString);
            
        }


        public override string ToString()
        {
            return $"LABEL {Nombre} | COORD {CoordenadaInicial} | ({Recta_Real[0]},{Recta_Real[1]})";
        }







    }


    [Serializable]

    public enum eDireccionGrid
    {
        X,Y
    }

}
