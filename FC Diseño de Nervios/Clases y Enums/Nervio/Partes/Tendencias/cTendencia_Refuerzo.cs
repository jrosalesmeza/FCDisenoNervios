using FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo;
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
                AsignarNervioOrigen();
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


        private List<cTendencia_Estribo> tendencia_Estribos = new List<cTendencia_Estribo>();
        public List<cTendencia_Estribo> TendenciasEstribos
        {
            get
            {
                CrearTendeestribosProyectosAnteriores();
                return tendencia_Estribos;
            }
            set
            {
                if (tendencia_Estribos != value)
                {
                    tendencia_Estribos = value;
                }
            }
        }

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

        public cTendencia_Estribo t_estriboSelect;

        public cTendencia_Estribo TEstriboSelect
        {
            get
            {
                CrearTendenciaSeleccionada();
                return t_estriboSelect;
            }
            set
            {
                if (t_estriboSelect != value)
                {
                    t_estriboSelect = value;
                    if (t_estriboSelect !=null)
                    {
                        NervioOrigen.CrearAceroAsignadoRefuerzoTransversal();
                    }
                }
            }
        }

        private void CrearTendeestribosProyectosAnteriores()
        {
            if (tendencia_Estribos == null)
            {
                tendencia_Estribos = new List<cTendencia_Estribo>();

                for (int i = 0; i < TendenciasInferior.Count; i++)
                {
                    tendencia_Estribos.Add(new cTendencia_Estribo(i, this));
                }
            }
        }

        private void CrearTendenciaSeleccionada()
        {
            if (t_estriboSelect == null)
            {
                CrearTendeestribosProyectosAnteriores();
                t_estriboSelect = tendencia_Estribos.First();
            }


        }


        public override string ToString()
        {
            return $"{Nombre} | {TendenciasSuperior.Count} | {TendenciasInferior.Count} ";
        }


    }
}
