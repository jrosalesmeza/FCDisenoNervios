using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cPiso
    {
        public string Nombre
        {
            get => default;
            set
            {
            }
        }

        public List<cNervio> Nervios
        {
            get => default;
            set
            {
            }
        }

        public cNervio NervioSelect
        {
            get => default;
            set
            {
            }
        }
    }
}