using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cAreas_Solicitacion
    {
        public cAreasyMomentos SolicitacionesInferior { get; set; } = new cAreasyMomentos();
        public cAreasyMomentos SolicitacionesSuperior { get; set; } = new cAreasyMomentos();

        public cAreasyMomentos AsingnadoInferior { get; set; } = new cAreasyMomentos();
        public cAreasyMomentos AsignadoSuperior { get; set; } = new cAreasyMomentos();
    }
}