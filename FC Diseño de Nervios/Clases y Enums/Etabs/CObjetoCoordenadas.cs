using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using B_EscalaCoordenadas;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

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

        public void CrearPuntosPlantaEscaladaEtabsLine(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom,bool OpenGL=false)
        {
            if (!OpenGL)
            {
                Planta_Escalado = cEscalaCoordenadas.EscalarPuntos(PuntosTodosObjetos, Planta_Real, WidthWindow, HeigthWindow, Zoom, Dx, Dy);
            }
            else
            {
                Planta_Escalado = cEscalaCoordenadas.EscalarPuntosEjeYNormal(PuntosTodosObjetos, Planta_Real, WidthWindow, HeigthWindow, Zoom, Dx, Dy);
            }
            
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


        public void PaintPlantaEscalada(bool select,bool selectMaestroG=false,bool selectsimilarG=false,bool selectMaestroT=false,bool selectsimilarT=false,bool isSelect=true)
        {

            float OffeSetLine = 2f;
            Color ColorPred = Color.FromArgb(0, 104, 149);
            Color ColorSelect = Color.FromArgb(220, 136, 21);
            Color ColorIsSelectFalse = Color.FromArgb(227, 227, 227);

            Color ColorSelectMaestroGeometria = Color.FromArgb(196, 4, 4);
            Color ColorSelectSimilarGeometria = Color.FromArgb(9, 149, 26);

            Color ColorSelectMaestroTodo = Color.FromArgb(140,70,250); 
            Color ColorSelectSimilarTodo = Color.FromArgb(185, 188, 7); 


            List<PointF> LineOffSET = B_Operaciones_Matricialesl.Operaciones.OffSetLine(Planta_Escalado.First(), Planta_Escalado.Last(), OffeSetLine).ToList();

            GL.Begin(PrimitiveType.Polygon);

            if (isSelect)
            {
                if (!select)
                {
                    GL.Color3(ColorPred);
                }
                else
                {
                    GL.Color3(ColorSelect);
                }
                if(selectMaestroG)
                    GL.Color3(ColorSelectMaestroGeometria);
                if (selectsimilarG)
                    GL.Color3(ColorSelectSimilarGeometria);
                if (selectMaestroT)
                    GL.Color3(ColorSelectMaestroTodo);
                if (selectsimilarT)
                    GL.Color3(ColorSelectSimilarTodo);
            }
            else
            {
                GL.Color3(ColorIsSelectFalse);
            }
            GL.Vertex3(Planta_Escalado.First().X, Planta_Escalado.First().Y, 0.0f);
            GL.Vertex3(LineOffSET.First().X, LineOffSET.First().Y, 0.0f);
            GL.Vertex3(LineOffSET.Last().X, LineOffSET.Last().Y, 0.0f);
            GL.Vertex3(Planta_Escalado.Last().X, Planta_Escalado.Last().Y, 0.0f);
            GL.End();

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