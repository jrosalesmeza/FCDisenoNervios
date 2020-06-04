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
                if (value != null)
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

        public cTendencia t_Supeselect;
        public cTendencia TSupeSelect
        {
            get { return t_Supeselect; }
            set
            {
                if (t_Supeselect != value)
                {
                    t_Supeselect = value;

                    if (t_InfeSelect != null)
                    {
                        NervioOrigen.CrearAceroAsignadoRefuerzoLongitudinal();
                    }
                }
            }
        }
        public cTendencia t_InfeSelect;
        public cTendencia TInfeSelect
        {
            get { return t_InfeSelect; }
            set
            {
                if (t_InfeSelect != value)
                {
                    t_InfeSelect = value;
                    if (t_Supeselect != null)
                    {
                        NervioOrigen.CrearAceroAsignadoRefuerzoLongitudinal();
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{Nombre} | {TendenciasSuperior.Count} | {TendenciasInferior.Count} ";
        }


    }
}
