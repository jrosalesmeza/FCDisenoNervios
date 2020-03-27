using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cBarra : cObjetoCoordenadas
    {
        public int ID { get; set; }


        public float AreaTotal { get; set; }


        public int NoBarra { get; set; }


        public int CantBarra { get; set; }


        public eUbicacionRefuerzo UbicacionRefuerzo { get; set; }


        public eTipoGancho GanchoIzquierda { get; set; }


        public eTipoGancho GanchoDerecha { get; set; }

    }
}