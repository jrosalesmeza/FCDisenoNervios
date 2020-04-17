using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cTendencia
    {
        public string Nombre { get; set; }
        public List<cBarra> Refuerzos { get; set; }
        public cTendencia_Refuerzo Tendencia_Refuerzo { get; set; }

        public cBarra NoMaximoBarra { get; set; }
        public cBarra NoMinimoBarra { get; set; }
        public float PesoRefuerzo { get; set; }
        public eUbicacionRefuerzo UbicacionRefuerzo { get; set; }
        public float DeltaAlargamientoBarras { get; set; }

        public override string ToString()
        {
            return $"{Nombre} | {Refuerzos.Count}";
        }
    }
}
