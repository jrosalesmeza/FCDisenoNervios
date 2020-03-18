using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cPiso
    {
        public string Nombre { get; set; }

        public float H { get; set; }
        public float Hacum { get; set; }

        public List<cNervio> Nervios { get; set; }

        public cNervio NervioSelect { get; set; }

        
    }
}