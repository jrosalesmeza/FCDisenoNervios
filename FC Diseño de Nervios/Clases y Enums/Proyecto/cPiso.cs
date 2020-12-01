using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FC_BFunctionsAutoCAD;
using System.Drawing;
namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cPiso
    {
        public string Nombre { get; set; }

        private string nombreReal;
        public string NombreReal
        {

            get { return nombreReal; }
            set
            {
                if (nombreReal != value)
                    nombreReal = value;
            }
        }


        private string nivel;
        public string Nivel
        {
            get { return nivel; }
            set
            {
                if (nivel != value)
                {
                    nivel = !value.Contains("N") ? "N" + value : value;
                }
            }
        }

        public float H { get; set; }
        public float Hacum { get; set; }


        public cPiso(string Nombre, float H)
        {
            this.Nombre = Nombre;
            this.H = H;
        }





        public List<cNervio> Nervios { get; set; }

        private cNervio nervioSelect;
        public cNervio NervioSelect
        {
            get { return nervioSelect; }
            set
            {
                if (nervioSelect != null)
                {
                    if (nervioSelect.Nombre != value.Nombre)
                    {
                        F_Base.LimpiarMemoria_Nervio();
                    }
                }
                nervioSelect = value;
                //nervioSelect.Select = true;
            }
        }

        /// <summary>
        /// Propiedad con datos de Etabs
        /// </summary>
        public List<cLine> Lista_Lines { get; set; } = new List<cLine>();

        public void DrawAutoCAD_LinesSelect(float X, float Y)
        {
            var LinesNoSelect = Lista_Lines.FindAll(y => y.isSelect && y.Type== eType.Beam);
            LinesNoSelect.ForEach(L => DrawLine_AutoCAD(L,cVariables.C_Vigas,X,Y));
        
        }

        public void DrawAutoCAD_Nervios(float X,float Y)
        {
            Nervios.ForEach(N =>
            {
                N.Lista_Tramos.ForEach(T =>
                {
                    T.Lista_SubTramos.ForEach(ST =>
                      ST.Lista_Lineas.ForEach(L => DrawLine_AutoCAD(L, cVariables.C_Nervios, X, Y)));

                    var Point1 =ConvertcPointToPointF(T.Lista_SubTramos.First().Lista_Lineas.First().ConfigLinea.Point1P);
                    var Point2 = ConvertcPointToPointF(T.Lista_SubTramos.Last().Lista_Lineas.Last().ConfigLinea.Point2P);
                    float Bmax = T.Lista_SubTramos.Max(y => y.Seccion.B) * cConversiones.Dimension_cm_to_m;
                    PointF pointF = PointF.Empty;
                    string Nombre = N.Nombre;
                    float Rotacion =0; float W_CajonTexto = cVariables.W_LetraAutoCADTextRefuerzo;float H_Texto = cVariables.H_TextoEstribos;
                    if (N.Direccion == eDireccion.Horizontal) 
                    {
                        pointF = new PointF(Point1.X + (Point2.X - Point1.X) / 2F - W_CajonTexto/2f, Point1.Y+ H_Texto+ Bmax);
                    }else if (N.Direccion == eDireccion.Vertical)
                    {
                        Rotacion = 90f;
                        pointF = new PointF(Point1.X- H_Texto- Bmax, Point1.Y + (Point2.Y - Point1.Y) / 2F- W_CajonTexto/2f);
                    }else if (N.Direccion== eDireccion.Diagonal)
                    {
                        Rotacion = Angulo(Point1, Point2);
                        pointF = new PointF(Point1.X + (Point2.X - Point1.X) / 2F - W_CajonTexto / 2f, Point1.Y + (Point2.Y - Point1.Y) / 2F - W_CajonTexto / 2f);
                    }
                    FunctionsAutoCAD.AddText(Nombre, B_Operaciones_Matricialesl. Operaciones.Traslacion(pointF,X,Y), cVariables.W_LetraAutoCADEstribos, H_Texto,cVariables.C_Texto2 , cVariables.Estilo_Texto, Rotacion,Width2: W_CajonTexto, JustifyText: JustifyText.Center);

                });

            });

        }
        private float Angulo(PointF point1, PointF point2)
        {
            float X = point1.X - point2.X;
            float Y = point1.Y - point2.Y;
            return (float) Math.Atan(Y / X)*cConversiones.Angulo_Rad_to_Grad;
        }








        private void DrawLine_AutoCAD(cLine Line,string layer,float X,float Y)
        {
            var point1 = ConvertcPointToPointF(Line.ConfigLinea.Point1P);
            var point2 = ConvertcPointToPointF(Line.ConfigLinea.Point2P);
            var puntos1 = new List<PointF> { point1, point2 };
            var puntos2 = B_Operaciones_Matricialesl.Operaciones.OffSetLine(point1, point2, Line.Seccion.B * cConversiones.Dimension_cm_to_m).ToList();
            FunctionsAutoCAD.AddPolyline2D(B_Operaciones_Matricialesl.Operaciones.Traslacion(puntos1,X,Y).ToArray(), layer, false);
            FunctionsAutoCAD.AddPolyline2D(B_Operaciones_Matricialesl.Operaciones.Traslacion(puntos2, X, Y).ToArray(), layer, false);
        }


        private PointF ConvertcPointToPointF(cPoint point) 
        {
            return new PointF(point.X, point.Y);
        }

        public override string ToString()
        {
            return $"{Nombre} | H={H}m";
        }




    }
}