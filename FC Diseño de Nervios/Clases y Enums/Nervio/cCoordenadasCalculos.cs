using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
 
    [Serializable]
    public class cCoordenadasCalculos
    {
        public cCoordenadas Momentos_Positivos { get; set; } = new cCoordenadas();
        public cCoordenadas Cortante_Positvos { get; set; } = new cCoordenadas();
        public cCoordenadas Momentos_Negativos { get; set; } = new cCoordenadas();
        public cCoordenadas Cortante_Negativos { get; set; } = new cCoordenadas();
        public cCoordenadas Areas_Momentos_Positivos { get; set; } = new cCoordenadas();
        public cCoordenadas Areas_Momentos_Negativos { get; set; } = new cCoordenadas();



    }
}
