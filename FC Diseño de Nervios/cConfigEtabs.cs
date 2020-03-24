using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cConfigEtabs
    {
        public cConfigEtabs(cPoint Point1P, cPoint Point2P)
        {
            this.Point1P = Point1P;
            this.Point2P = Point2P;
        }
        public bool Select { get; set; }

        public cPoint Point1P { get; set; }

        public cPoint Point2P { get; set; }

        public override string ToString()
        {
            return $"{Point1P},{Point2P}";
        }

    }
}