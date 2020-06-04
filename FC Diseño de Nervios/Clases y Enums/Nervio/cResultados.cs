using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cResultados
    {
        public bool Diseñado { get; set; } = false;
        public List<string> Errores { get; set; } = new List<string>();


    }
}
