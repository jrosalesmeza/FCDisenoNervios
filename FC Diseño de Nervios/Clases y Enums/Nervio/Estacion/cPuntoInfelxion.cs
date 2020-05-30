using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace FC_Diseño_de_Nervios
{
    public class cPuntoInfelxion
    {

        public float CoordX { get; set; }
        
        public cSubTramo SubTramo { get; set; }
        public float CordApoyoAsociada { get; set; }


        private int id;
        public int ID {

            get { return id; }

            set {

                id = value;
                AsignarApoyoAsociado();


            }

        }

        private void AsignarApoyoAsociado()
        {
       
             float CoordXMin=  SubTramo.TramoOrigen.Lista_SubTramos.Min(x => x.Vistas.Perfil_Original.Reales.Min(y => y.X));

             float CoordXMax = SubTramo.TramoOrigen.Lista_SubTramos.Max(x => x.Vistas.Perfil_Original.Reales.Max(y => y.X));
            if(Math.Abs(CoordXMax - CoordX)< Math.Abs(CoordXMin - CoordX))
            {
                CordApoyoAsociada = CoordXMax;
            }
            else
            {
                CordApoyoAsociada = CoordXMin;
            }
       
        }

        public cPuntoInfelxion(cEstacion Estacion)
        {
            
            SubTramo = Estacion.SubTramoOrigen;
            CoordX = Estacion.CoordX+ SubTramo.Vistas.Perfil_Original.Reales.Min(x=>x.X);
        }

        public override string ToString()
        {
            return $"PI | CordX= {CoordX}| CordXApoyo = {CordApoyoAsociada}";
        }

    }
}
