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
            Solicitacion_Asignado_Momentos = new cSolicitacion_Asignado_Momentos(this);
            Solicitacion_Asignado_Cortante = new c_Solicitacion_Asignado_Cortante(this);
        }
        public cSolicitacion_Asignado_Momentos Solicitacion_Asignado_Momentos { get; set; }
        public c_Solicitacion_Asignado_Cortante Solicitacion_Asignado_Cortante { get; set; } 
        public cEnvolvente Envolvente { get; set; }
        public cSubTramo SubtramoOrigen { get; }


        public override string ToString() => $"Calculos | {SubtramoOrigen.ToString()}";
    }
}