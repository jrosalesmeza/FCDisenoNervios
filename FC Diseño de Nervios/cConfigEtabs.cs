using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cConfigEtabs
    {
        public cConfigEtabs(cPoint Point1P, cPoint Point2P)
        {
            this.Point1P = Point1P;
            this.Point2P = Point2P;
            ClasificacionDireccionElemento();
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

        public override string ToString()
        {
            return $"{Point1P},{Point2P}";
        }




    }
}