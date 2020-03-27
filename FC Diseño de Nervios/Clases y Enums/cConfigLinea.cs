using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cConfigLinea
    {
        public cConfigLinea(cPoint Point1P, cPoint Point2P)
        {
            this.Point1P = Point1P;
            this.Point2P = Point2P;
            ClasificacionDireccionElemento();
            CalcularLongitud();
        }

        private void ClasificacionDireccionElemento()
        {
            if(Point1P.X== Point2P.X)
            {
                Direccion = eDireccion.Vertical;
            }
            else if(Point1P.Y== Point2P.Y)
            {
                Direccion = eDireccion.Horizontal;
            }else
            {
                float DistX = Math.Abs(Point1P.X - Point2P.X);
                float DistY = Math.Abs(Point1P.Y - Point2P.Y);

                if (DistX > DistY)
                {
                    Direccion = eDireccion.Horizontal;
                }
                else
                {
                    Direccion = eDireccion.Vertical;
                }

            }



        }


        public eDireccion Direccion { get; set; }
        public bool Select { get; set; }


        public cPoint Point1P { get; set; }

        public cPoint Point2P { get; set; }
        public float Longitud { get; set; }
        public float OffSetI { get; set; } = 0;
        public float OffSetJ { get; set; } = 0;


        public void CalcularLongitud()
        {
            Longitud =(float)Math.Round(cFunctionsProgram.Long(Point1P, Point2P),2);
        }
        public override string ToString()
        {
            return $"{Point1P},{Point2P}, L= {Longitud}";
        }




    }
}