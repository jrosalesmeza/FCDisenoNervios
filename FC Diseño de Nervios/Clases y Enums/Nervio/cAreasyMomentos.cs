using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cAreasyMomentos
    {
        public float Area_Momento { get; set; }
        public float Momento { get; set; }
      
        public float Cortante { get; set; }
        public float Area_Cortante { get; set; }
    }
}
