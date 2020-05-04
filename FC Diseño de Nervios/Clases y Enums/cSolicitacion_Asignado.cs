using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSolicitacion_Asignado_Momentos
    {
        public cAreasyMomentos SolicitacionesInferior { get; set; } 
        public cAreasyMomentos SolicitacionesSuperior { get; set; } 

        public cAreasyMomentos AsignadoInferior { get; set; } 
        public cAreasyMomentos AsignadoSuperior { get; set; } 
        public cCalculos CalculosOrigen { get; set; }

        public cSolicitacion_Asignado_Momentos(cCalculos CalculosOrigen)
        {
            this.CalculosOrigen = CalculosOrigen;
            SolicitacionesInferior = new cAreasyMomentos(this);
            SolicitacionesSuperior = new cAreasyMomentos(this);
            AsignadoInferior = new cAreasyMomentos(this);
            AsignadoSuperior = new cAreasyMomentos(this);


        }
        public override string ToString()
        {
            return $"Inferior y Superior | Solicitación|Asignado";
        }

    }
}