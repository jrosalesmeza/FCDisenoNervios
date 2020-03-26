using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cMaterial
    {
        public cMaterial(string Nombre, float fc, float fy)
        {
            this.Nombre = Nombre;
            this.fc = fc;
            this.fy = fy;
        }
        public string Nombre { get; set; }
        public float fc { get; set; }
        public float fy { get; set; }

        public override string ToString()
        {
            return $"{Nombre} Fc= {fc}kgf/cm² Fy={fy}kgf/cm²";
        }
    }
}