using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cEdificio
    {
        public float d1 { get; set; }
        public float d2 { get; set; }
        public List<cPiso> Lista_Pisos { get; set; }
        public cPiso PisoSelect { get; set; }
        public List<cGrid> Lista_Grids { get; set; }
 
    }
}