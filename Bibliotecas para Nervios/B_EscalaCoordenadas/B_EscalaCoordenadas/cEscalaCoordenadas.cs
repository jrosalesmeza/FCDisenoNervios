using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_EscalaCoordenadas
{
    /// <summary>
    /// Liberaría útil para controles de WindowsForm.
    /// </summary>
    public static class cEscalaCoordenadas
    {

        /// <summary>
        /// Establece la escala a una lista de puntos. 
        /// </summary>
        /// <param name="Points">Lista de Puntos.</param>
        /// <param name="PuntoObjeto">Punto perteneciente a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <returns></returns>
        public static List<PointF> EscalarPuntos(List<PointF> Points,List<PointF> PuntoObjeto, float WidthWindow, float HeigthWindow, float Zoom = 1, float Dx = 0,float Dy=0)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SX = (WidthWindow)/Math.Abs(MaxPositivoX-MaxNegativoX)*Zoom;
            float SY = (HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY)*Zoom ;
            float XI; float YI;
            XI = 5;YI = 5;
            float EscalaMayor;
            if (SX < SY)
            {
                EscalaMayor = SX;
                YI = -(HeigthWindow - MaxPositivoY * EscalaMayor) / 2;
            }
            else
            {
                EscalaMayor = SY;
                XI = (WidthWindow - MaxPositivoX * EscalaMayor) / 2;
            }


            SX = EscalaMayor;SY = EscalaMayor;

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;
            List<PointF> PuntosEscalados = new List<PointF>();


            foreach (PointF point in PuntoObjeto)
            {
                float X = 0;float Y=0;
                if (point.X < 0)
                {
                    X = MaxNegativoX * SX - Math.Abs(point.X) * SX;
                }
                else
                {
                    X = MaxNegativoX * SX + Math.Abs(point.X) * SX;
                }
                if (point.Y < 0)
                {
                    Y = -MaxNegativoY * SY + Math.Abs(point.Y) * SY + HeigthWindow;
                }
                else
                {
                    Y = -MaxNegativoY * SY - Math.Abs(point.Y) * SY + HeigthWindow;
                }

                X +=Dx +XI; Y += Dy+YI;
                PointF PuntoEscalado = new PointF(X, Y);
                PuntosEscalados.Add(PuntoEscalado);
            }

            return PuntosEscalados;

        }



    }
}
