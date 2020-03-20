using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cPiso
    {
        public string Nombre { get; set; }

        public float H { get; set; }
        public float Hacum { get; set; }
        
        
        public cPiso(string Nombre ,float H)
        {
            this.Nombre = Nombre;
            this.H = H;
        }




        
        public List<cNervio> Nervios { get; set; }

        public cNervio NervioSelect { get; set; }

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