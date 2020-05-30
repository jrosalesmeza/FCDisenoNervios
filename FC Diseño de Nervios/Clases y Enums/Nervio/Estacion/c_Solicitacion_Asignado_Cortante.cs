using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class c_Solicitacion_Asignado_Cortante
    {
        public cAreasyCortante SolicitacionesInferior { get; set; }
        public cAreasyCortante SolicitacionesSuperior { get; set; }

        public cAreasyCortante AsignadoInferior { get; set; }
        public cAreasyCortante AsignadoSuperior { get; set; }
        public cCalculos CalculosOrigen { get; set; }

        public c_Solicitacion_Asignado_Cortante(cCalculos CalculosOrigen)
        {
            this.CalculosOrigen = CalculosOrigen;
            SolicitacionesInferior = new cAreasyCortante(this);
            SolicitacionesSuperior = new cAreasyCortante(this);
            AsignadoInferior = new cAreasyCortante(this);
            AsignadoSuperior = new cAreasyCortante(this);
        }
        public override string ToString()
        {
            return $"Inferior y Superior | Solicitación|Asignado";
        }

    }
}
