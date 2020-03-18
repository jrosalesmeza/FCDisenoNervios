using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cNervio
    {
        public string Nombre { get; set; }


        public string Prefijo { get; set; }


        public List<cTramo> Lista_Tramos { get; set; }


        public cTramo TramoSelect { get; set; }


        public int ID { get; set; }


        public List<cBarra> ListaBarras { get; set; }

    }
}