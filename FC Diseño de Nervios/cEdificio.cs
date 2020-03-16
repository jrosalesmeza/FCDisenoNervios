using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cEdificio
    {
        public List<cPiso> Lista_Pisos { get; set; }
        public cPiso PisoSelect { get; set; }
 
    }
}