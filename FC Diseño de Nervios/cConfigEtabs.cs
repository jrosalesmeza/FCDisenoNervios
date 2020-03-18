using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cConfigEtabs
    {

        public bool Select { get; set; }

        public cPoint Point1P { get; set; }

        public cPoint Point2P { get; set; }

    }
}