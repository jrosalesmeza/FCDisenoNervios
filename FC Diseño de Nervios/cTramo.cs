using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cTramo
    {
        public List<cLine> Lineas { get; set; }


        public string Nombre { get; set; }


        public float Longitud { get; set; }


        public List<cEstribo> Estribo { get; set; }
    }
}