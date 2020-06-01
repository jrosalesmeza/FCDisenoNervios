using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cPiso
    {
        public string Nombre { get; set; }

        private string nombreReal;
        public string NombreReal
        {

            get { return nombreReal; }
            set
            {
                if (nombreReal != value)
                    nombreReal = value;
            }
        }


        private string nivel;
        public string Nivel
        {
            get { return nivel; }
            set
            {
                if (nivel != value)
                {
                    nivel = !value.Contains("N") ? "N" + value : value;
                }
            }
        }

        public float H { get; set; }
        public float Hacum { get; set; }


        public cPiso(string Nombre, float H)
        {
            this.Nombre = Nombre;
            this.H = H;
        }





        public List<cNervio> Nervios { get; set; }

        private cNervio nervioSelect;
        public cNervio NervioSelect
        {
            get { return nervioSelect; }
            set
            {
                if (nervioSelect != null)
                {
                    if (nervioSelect.Nombre != value.Nombre)
                    {

                        F_Base.LimpiarMemoria_Nervio();
                    }
                }
                nervioSelect = value;
                nervioSelect.Select = true;
            }
        }

        /// <summary>
        /// Propiedad con datos de Etabs
        /// </summary>
        public List<cLine> Lista_Lines { get; set; } = new List<cLine>();
        public override string ToString()
        {
            return $"{Nombre} | H={H}m";
        }

    }
}