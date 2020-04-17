using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{

    [Serializable]
    public class cTendencia_Refuerzo
    {
        public string Nombre { get; set; } = "Tendencias";
        public cNervio NervioOrigen { get; set; }

        public List<cTendencia> TendenciasSuperior = new List<cTendencia>();
        public List<cTendencia> TendenciasInferior = new List<cTendencia>();
        public cTendencia TSupeSelect { get; set; }
        public cTendencia TInfeSelect { get; set; }

        public override string ToString()
        {
            return $"{Nombre} | {TendenciasSuperior.Count} | {TendenciasInferior.Count} ";
        }


    }
}
