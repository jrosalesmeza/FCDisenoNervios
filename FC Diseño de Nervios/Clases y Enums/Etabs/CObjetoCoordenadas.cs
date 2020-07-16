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

        public List<PointF> Planta_Escalado { get; set; }
        public List<PointF> Planta_Real { get; set; }
        public bool Select { get; set; }
        public bool isSelect { get; set; } = true;





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
            Pen PenElementisSelectFalse = new Pen(Color.FromArgb(227, 227, 227), 2);
            if (Select)
            {
                e.DrawLines(PenElementSeleccionar, Planta_Escalado.ToArray());
            }
            else
            {
                if (isSelect)
                {
                    e.DrawLines(PenElementSinSeleccionar, Planta_Escalado.ToArray());
                }
                else
                {
                    e.DrawLines(PenElementisSelectFalse, Planta_Escalado.ToArray());
                }
            }

        }


        public void PaintPlantaEscalada(Graphics e, Pen pen2, bool select, Pen penPredet = null, Pen penSelect=null)
        {
            if (penPredet == null)
                penPredet = new Pen(Color.FromArgb(0, 104, 149), 2);
            if(penSelect==null)
                penSelect = new Pen(Color.FromArgb(220, 136, 21), 2);

            if (!select)
            {
                e.DrawLines(penPredet, Planta_Escalado.ToArray());
            }
            else
            {
                e.DrawLines(penSelect, Planta_Escalado.ToArray());
            }
        }









        public void MouseDownSelectLineEtabs(MouseEventArgs e, int EnteroFinal)
        {
            if (isSelect)
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
                     
                        Select = true;
                        IndexSelect = EnteroFinal + 1;
                    }
                }

            }
        }


        public bool MouseInLineEtabs(Point Point)
        {

            if (Planta_Escalado != null)
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
            else
            {
                return false;
            }
        }




    }
}