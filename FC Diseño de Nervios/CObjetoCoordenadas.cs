using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using B_EscalaCoordenadas;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cObjetoCoordenadas
    {
        public List<PointF> Longitudinal_Real { get; set; }


        public List<PointF> Longitudinal_Escalado { get; set; }


        public List<PointF> Planta_Escalado { get; set; }


        public List<PointF> Planta_Real { get; set; }


        public bool Select { get; set; }
        public int IndexSelect { get; set; } = 0;
        public void CrearPuntosPlantaRealEtabsLine(cPoint Point1, cPoint Point2)
        {
            Planta_Real = new List<PointF>();
            Planta_Real.Add(new PointF(Point1.X,Point1.Y));
            Planta_Real.Add(new PointF(Point2.X, Point2.Y));
        }

        public void CrearPuntosPlantaEscaladaEtabsLine(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom)
        {
            Planta_Escalado=cEscalaCoordenadas.EscalarPuntos(PuntosTodosObjetos, Planta_Real, WidthWindow, HeigthWindow,Zoom,Dx,Dy);
        }

        public void PaintPlantaEscaladaEtabsLine(Graphics e) 
        {
           
            Pen PenElementSinSeleccionar = new Pen(Color.FromArgb(0, 104, 149), 2);
            Pen PenElementSeleccionar = new Pen(Color.FromArgb(220, 136, 21), 2);

            if (Select)
            {
              e.DrawLines(PenElementSeleccionar, Planta_Escalado.ToArray());
            }
            else
            {
                e.DrawLines(PenElementSinSeleccionar, Planta_Escalado.ToArray());
            }


        }

        public void MouseDownSelectLineEtabs(MouseEventArgs e, int EnteroFinal)
        {
            if (MouseInLineEtabs(e.Location))
            {
                if (Select)
                {
                    Select = false;
                    IndexSelect = 0;
                }
                else
                {
                    F_Base.cUndoRedo.EnviarEstado(F_Base.Proyecto);
                    Select = true;
                    IndexSelect = EnteroFinal + 1;
                }
             }
       

        }

 

        public bool MouseInLineEtabs(Point Point)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            float Espesor = 5;
            graphicsPath.AddLine(Planta_Escalado[0].X - Espesor, Planta_Escalado[0].Y - Espesor, Planta_Escalado[1].X - Espesor, Planta_Escalado[1].Y - Espesor);
            graphicsPath.AddLine(Planta_Escalado[1].X + Espesor, Planta_Escalado[1].Y + Espesor, Planta_Escalado[0].X + Espesor, Planta_Escalado[0].Y + Espesor);

            if (graphicsPath.IsVisible(Point))
            {
                return true;
            }
            else
                return false;
        }




    }
}