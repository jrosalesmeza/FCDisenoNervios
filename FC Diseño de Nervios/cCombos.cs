using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSolicitacion
    {
        public string Nombre { get; set; }


        public float P { get; set; }


        public float V2 { get; set; }


        public float V3 { get; set; }


        public float M2 { get; set; }


        public float M3 { get; set; }


        public float T { get; set; }

    }
}