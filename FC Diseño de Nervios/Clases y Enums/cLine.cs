using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cLine:cObjetoCoordenadas
    {
        public string Nombre { get; set; }

        public cLine(string Nombre, eType Type)
        {
            this.Nombre = Nombre;
            this.Type = Type;
        }
        public List<cEstacion> Estaciones { get; set; }

        public cSeccion Seccion { get; set; }

        public eType Type { get; set; }

        


        public cLine ObjDerecha { get; set; }
        public cConfigLinea ConfigLinea { get; set; }
        public string Story { get; set; }

        public int IndiceConjuntoSeleccion { get; set; }


        #region Para Datos de Etabs
        private bool SelectMouseMove = false;
        public void PaintMouseMove(Graphics e, float HeigthWindow, float WidthWindow)
        {
            if (SelectMouseMove && isSelect)
            {
                Font Font1 = new Font("Calibri", 9, FontStyle.Bold);
                PointF PointString = new PointF(WidthWindow/2 -e.MeasureString(Seccion.Nombre,Font1).Width/2 , HeigthWindow / 2 - Font1.Height / 2);
                e.DrawString(Seccion.Nombre.ToString(), Font1, Brushes.Black, PointString);
            }

        }
        public void MouseMoveElementoSinEnumerar(Point Point)
        {
            if (isSelect)
            {
                SelectMouseMove = (MouseInLineEtabs(Point));
            }     
        }

        #endregion





        public override string ToString()
        {
            return $"{Nombre} | {Type} | {Story}";
        }


    }
}