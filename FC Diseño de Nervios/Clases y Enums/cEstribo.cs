using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cEstribo:cObjetoCoordenadas
    {
        public cSeccion Seccion { get; set; }
        public int Ramas { get; set; }

    }
}