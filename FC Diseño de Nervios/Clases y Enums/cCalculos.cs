using System;
using B_FC_DiseñoVigas;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cCalculos
    {
        public cCalculos(cSubTramo SubtramoOrigen)
        {
            this.SubtramoOrigen = SubtramoOrigen;
        }

        public float Phi_Vc { get; set; }
        public cSolicitacion_Asignado_Momentos Solicitacion_Asingado_Momentos { get; set; } = new cSolicitacion_Asignado_Momentos();
        public cAreasyCortante Solicitacion_Asingado_Corante { get; set; } = new cAreasyCortante();
        public cEnvolvente Envolvente { get; set; }

        public cSubTramo SubtramoOrigen { get; }


        public override string ToString()
        {
            return $"Calculos | {SubtramoOrigen.ToString()}";
        }
    }
}