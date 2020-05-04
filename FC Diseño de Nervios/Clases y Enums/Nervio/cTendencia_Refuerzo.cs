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

        [NonSerialized]
        private cNervio nervioOrigen;
        public cNervio NervioOrigen
        {
            get
            {
                if (nervioOrigen == null)
                {
                    AsignarNervioOrigen();
                }
                return nervioOrigen;

            }
            set
            {
                if (nervioOrigen != null)
                {
                    nervioOrigen = value;
                }
            }
        }
        public string NombreNervioOrigen;

        private void AsignarNervioOrigen()
        {
            nervioOrigen = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect;
        }
        public List<cTendencia> TendenciasSuperior = new List<cTendencia>();
        public List<cTendencia> TendenciasInferior = new List<cTendencia>();

        private cTendencia t_supeselect;
        public cTendencia TSupeSelect
        {
            get { return t_supeselect; }
            set
            {
                if (t_supeselect != value)
                {
                    t_supeselect = value;
                }
            }
        }
        private cTendencia t_InfeSelect;
        public cTendencia TInfeSelect
        {
            get { return t_InfeSelect; }
            set
            {
                if (t_InfeSelect != value)
                {
                    t_InfeSelect = value;
                    //t_InfeSelect.Tendencia_Refuerzo_Origen.NervioOrigen.CrearCoordenadasDiagramaMomentosyAreas_Reales_Asignado();
                }
            }
        }

        public override string ToString()
        {
            return $"{Nombre} | {TendenciasSuperior.Count} | {TendenciasInferior.Count} ";
        }


    }
}
