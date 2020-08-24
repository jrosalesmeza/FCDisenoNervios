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
        /// Establece la escala a una lista de puntos en su eje mayor, centrando el dibujo en ambos ejes. 
        /// </summary>
        /// <param name="Points">Lista de Puntos.</param>
        /// <param name="PuntosObjeto">Lista de Puntos pertenecientes a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <returns></returns>
        public static List<PointF> EscalarPuntos(List<PointF> Points,List<PointF> PuntosObjeto, float WidthWindow, float HeigthWindow, float Zoom = 1, float Dx = 0,float Dy=0, float XI=5f, float YI=5f)
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


            foreach (PointF point in PuntosObjeto)
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

        /// <summary>
        /// Establece la escala a una lista de puntos en su eje mayor, centrando el dibujo en ambos ejes, Para OPENGL. 
        /// </summary>
        /// <param name="Points">Lista de Puntos.</param>
        /// <param name="PuntosObjeto">Lista de Puntos pertenecientes a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <returns></returns>
        public static List<PointF> EscalarPuntosEjeYNormal(List<PointF> Points, List<PointF> PuntosObjeto, float WidthWindow, float HeigthWindow, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SX = (WidthWindow) / Math.Abs(MaxPositivoX - MaxNegativoX) * Zoom;
            float SY = (HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY) * Zoom;

            float EscalaMayor;
            if (SX < SY)
            {
                EscalaMayor = SX;
                YI = (HeigthWindow - MaxPositivoY * EscalaMayor) / 2;
            }
            else
            {
                EscalaMayor = SY;
                XI = (WidthWindow - MaxPositivoX * EscalaMayor) / 2;
            }


            SX = EscalaMayor; SY = EscalaMayor;

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;
            List<PointF> PuntosEscalados = new List<PointF>();


            foreach (PointF point in PuntosObjeto)
            {
                float X = 0; float Y = 0;
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
                    Y = MaxNegativoY * SY + Math.Abs(point.Y) * SY;
                }
                else
                {
                    Y = MaxNegativoY * SY - Math.Abs(point.Y) * SY;
                }

                X += Dx + XI; Y += Dy + YI;
                PointF PuntoEscalado = new PointF(X, Y);
                PuntosEscalados.Add(PuntoEscalado);
            }

            return PuntosEscalados;

        }




        /// <summary>
        /// Establece la escala a una lista de puntos en sus dos ejes. 
        /// </summary>
        /// <param name="Points">Lista de Puntos.</param>
        /// <param name="PuntosObjeto">Lista de Puntos pertenecientes a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <param name="SX">Devuelve la escala en X </param>
        ///  <param name="Tamano">Parametro para reducir la altura del dibujo. </param>
        /// <returns></returns>
        public static List<PointF> EscalaEnDosDirecciones(List<PointF> Points, List<PointF> PuntosObjeto, float WidthWindow, float HeigthWindow,out float SX, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f,float Tamano=2f)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            SX = (WidthWindow) / Math.Abs(MaxPositivoX - MaxNegativoX) * Zoom;
            float SY = ((HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY)/ Tamano) * Zoom;
            
            
            YI = -(HeigthWindow - MaxPositivoY * SY) / 2;


            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;
            List<PointF> PuntosEscalados = new List<PointF>();


            foreach (PointF point in PuntosObjeto)
            {
                float X = 0; float Y = 0;
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

                X += Dx + XI; Y += Dy + YI;
                PointF PuntoEscalado = new PointF(X, Y);
                PuntosEscalados.Add(PuntoEscalado);
            }

            return PuntosEscalados;

        }


        /// <summary>
        /// Establece la escala a un Punto en ambos ejes. 
        /// </summary>
        /// <param name="Points">Lista de Puntos</param>
        /// <param name="PuntoObjeto">Punto perteneciente a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        ///<param name="Tamano">Parametro para reducir la altura del dibujo. </param>
        /// <returns></returns>
        public static PointF EscalarPuntoEnDosDirecciones(List<PointF> Points, PointF PuntoObjeto, float WidthWindow, float HeigthWindow, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f,float Tamano =2f)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SX = (WidthWindow) / Math.Abs(MaxPositivoX - MaxNegativoX) * Zoom;
            float SY = ((HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY)/ Tamano) * Zoom;

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;

            float X = 0; float Y = 0;
            if (PuntoObjeto.X < 0)
            {
                X = MaxNegativoX * SX - Math.Abs(PuntoObjeto.X) * SX;
            }
            else
            {
                X = MaxNegativoX * SX + Math.Abs(PuntoObjeto.X) * SX;
            }
            if (PuntoObjeto.Y < 0)
            {
                Y = -MaxNegativoY * SY + Math.Abs(PuntoObjeto.Y) * SY + HeigthWindow;
            }
            else
            {
                Y = -MaxNegativoY * SY - Math.Abs(PuntoObjeto.Y) * SY + HeigthWindow;
            }

            X += Dx + XI; Y += Dy + YI;



            return new PointF(X, Y);

        }

        

        /// <summary>
        /// Establece la escala a un Punto en su eje mayor, centrando el dibujo en el eje Y. 
        /// </summary>
        /// <param name="Points">Lista de Puntos</param>
        /// <param name="PuntoObjeto">Punto perteneciente a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <returns></returns>
        public static PointF EscalarPunto(List<PointF> Points, PointF PuntoObjeto, float WidthWindow, float HeigthWindow, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SX = (WidthWindow) / Math.Abs(MaxPositivoX - MaxNegativoX) * Zoom;
            float SY = (HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY) * Zoom;
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

            SX = EscalaMayor; SY = EscalaMayor;

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;

            float X = 0; float Y = 0;
            if (PuntoObjeto.X < 0)
            {
                X = MaxNegativoX * SX - Math.Abs(PuntoObjeto.X) * SX;
            }
            else
            {
                X = MaxNegativoX * SX + Math.Abs(PuntoObjeto.X) * SX;
            }
            if (PuntoObjeto.Y < 0)
            {
                Y = -MaxNegativoY * SY + Math.Abs(PuntoObjeto.Y) * SY + HeigthWindow;
            }
            else
            {
                Y = -MaxNegativoY * SY - Math.Abs(PuntoObjeto.Y) * SY + HeigthWindow;
            }

            X += Dx + XI; Y += Dy + YI;



            return new PointF(X, Y);

        }



        #region Escala de Puntos centrando el dibujo en eje Y

        /// <summary>
        /// Establece la escala a un Punto en su eje mayor, centrando el dibujo ambos ejes.
        /// </summary>
        /// <param name="Points">Lista de Puntos</param>
        /// <param name="PuntoObjeto">Punto perteneciente a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <returns></returns>
        public static PointF EscalarPuntoEnEjeY(List<PointF> Points, PointF PuntoObjeto, float WidthWindow,float HeigthWindow, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SX = (WidthWindow) / Math.Abs(MaxPositivoX - MaxNegativoX) * Zoom;
            float SY = (HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY) * Zoom;
            float EscalaMayor;
            if (SX < SY)
            {
                EscalaMayor = SX;
                YI= - (HeigthWindow - MaxPositivoY * EscalaMayor) / 2;
            }
            else
            {
                EscalaMayor = SY;
            }

            SX = EscalaMayor; SY = EscalaMayor;

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;

            float X = 0; float Y = 0;
            if (PuntoObjeto.X < 0)
            {
                X = MaxNegativoX * SX - Math.Abs(PuntoObjeto.X) * SX;
            }
            else
            {
                X = MaxNegativoX * SX + Math.Abs(PuntoObjeto.X) * SX;
            }
            if (PuntoObjeto.Y < 0)
            {
                Y = -MaxNegativoY * SY + Math.Abs(PuntoObjeto.Y) * SY + HeigthWindow;
            }
            else
            {
                Y = -MaxNegativoY * SY - Math.Abs(PuntoObjeto.Y) * SY + HeigthWindow;
            }

            X += Dx + XI; Y += Dy + YI;



            return new PointF(X, Y);

        }


        /// <summary>
        /// Establece la escala a una lista de puntos en su eje mayor, centrando el dibujo en el eje Y. 
        /// </summary>
        /// <param name="Points">Lista de Puntos.</param>
        /// <param name="PuntosObjeto">Lista de Puntos pertenecientes a <paramref name="Points"/></param>
        /// <param name="WidthWindow">Ancho de la ventana. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <param name="EscalaMayor">Devuelve la Escala Mayor</param>
        /// <returns></returns>
        public static List<PointF> EscalarPuntosEnEjeY(List<PointF> Points, List<PointF> PuntosObjeto, float WidthWindow, float HeigthWindow, out float EscalaMayor, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f)
        {
            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SX = (WidthWindow) / Math.Abs(MaxPositivoX - MaxNegativoX) * Zoom;
            float SY = (HeigthWindow) / Math.Abs(MaxPositivoY - MaxNegativoY) * Zoom;

            if (SX < SY)
            {
                EscalaMayor = SX;
                YI=-(HeigthWindow - MaxPositivoY * EscalaMayor) / 2;
            }
            else
            {
                EscalaMayor = SY;
            }
            SX = EscalaMayor; SY = EscalaMayor;

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;
            List<PointF> PuntosEscalados = new List<PointF>();


            foreach (PointF point in PuntosObjeto)
            {
                float X = 0; float Y = 0;
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

                X += Dx + XI; Y += Dy + YI;
                PointF PuntoEscalado = new PointF(X, Y);
                PuntosEscalados.Add(PuntoEscalado);
            }

            return PuntosEscalados;

        }



        /// <summary>
        /// Establece la escala a una Lista de Puntos en ambos ejes, dependiendo en el sentido X de una escala dada y el dibujo se centra en el eje Y.
        /// </summary>
        /// <param name="Points">Lista de Puntos</param>
        /// <param name="Puntos">Puntos perteneciente a <paramref name="Points"/></param>
        /// <param name="SY">Devuelve la Escala en Y. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="SX">Escala en X.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <returns></returns>

        public static List<PointF> EscalarPuntosConEscalasDependientes(List<PointF> Points, List<PointF> Puntos, out float SY, float HeigthWindow,float SX, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f)
        {

            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            SY = (HeigthWindow / Math.Abs(MaxPositivoY - MaxNegativoY) )/2* Zoom;


            YI = -(HeigthWindow - (MaxPositivoY - MaxNegativoY) * SY) / 2;
           

            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;
            List<PointF> PuntosEscalados = new List<PointF>();

            foreach (PointF point in Puntos)
            {
                float X = 0; float Y = 0;
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

                X += Dx + XI; Y += Dy + YI;
                PointF PuntoEscalado = new PointF(X, Y);
                PuntosEscalados.Add(PuntoEscalado);
            }
            return PuntosEscalados;


        }




        /// <summary>
        /// Establece la escala a un Punto en ambos ejes, dependiendo en el sentido X de una escala dada y el dibujo se centra en el eje Y.
        /// </summary>
        /// <param name="Points">Lista de Puntos</param>
        /// <param name="Punto">Punto perteneciente a <paramref name="Points"/></param>
        /// <param name="HeigthDraw">Alto del Dibujo. </param>
        /// <param name="HeigthWindow">Alto de la ventana.</param>
        /// <param name="Zoom">Factor de ampliación.</param>
        /// <param name="SX">Escala en X.</param>
        /// <param name="Dx">Factor de desplazamiento en X.</param>
        /// <param name="Dy">Factor de desplazamiento en Y.</param>
        /// <param name="XI">Coordenada en X inicial en pixeles.</param>
        /// <param name="YI">Coordenada en Y inicial en pixeles</param>
        /// <returns></returns>

        public static PointF EscalarPuntoConEscalasDependientes(List<PointF> Points, PointF Punto, float HeigthDraw, float HeigthWindow, float SX, float Zoom = 1, float Dx = 0, float Dy = 0, float XI = 5f, float YI = 5f)
        {

            float MaxPositivoX = Points.Max(x => x.X);
            float MaxPositivoY = Points.Max(x => x.Y);
            float MaxNegativoX = Points.Min(x => x.X);
            float MaxNegativoY = Points.Min(x => x.Y);

            if (MaxPositivoX < 0) { MaxPositivoX = 0; }
            if (MaxPositivoY < 0) { MaxPositivoY = 0; }
            if (MaxNegativoX > 0) { MaxNegativoX = 0; }
            if (MaxNegativoY > 0) { MaxNegativoY = 0; }

            float SY = (HeigthWindow / Math.Abs(MaxPositivoY - MaxNegativoY)) / 2 * Zoom;


            YI = -(HeigthWindow - (MaxPositivoY - MaxNegativoY) * SY) / 2;


            MaxNegativoX = -MaxNegativoX;
            MaxNegativoY = -MaxNegativoY;


            float X = 0; float Y = 0;
            if (Punto.X < 0)
            {
                X = MaxNegativoX * SX - Math.Abs(Punto.X) * SX;
            }
            else
            {
                X = MaxNegativoX * SX + Math.Abs(Punto.X) * SX;
            }
            if (Punto.Y < 0)
            {
                Y = -MaxNegativoY * SY + Math.Abs(Punto.Y) * SY + HeigthWindow;
            }
            else
            {
                Y = -MaxNegativoY * SY - Math.Abs(Punto.Y) * SY + HeigthWindow;
            }

            X += Dx + XI; Y += Dy + YI;
      
            return new PointF(X, Y); 


        }












        #endregion
    }
}
