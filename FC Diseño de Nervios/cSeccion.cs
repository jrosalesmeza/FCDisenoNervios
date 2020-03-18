using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSeccion
    {

        public cSeccion(string Nombre, float B, float H)
        {
            this.Nombre = Nombre;
            this.B = B;
            this.H = H;

        }


        public string Nombre { get; set; }
        public float B { get; set; }
        public float H { get; set; }


        public cMaterial Material { get; set; }

        public eType Type { get; set; }

        public override string ToString()
        {
            return $"{Nombre} B={B}cm H={H}cm Material ";
        }

    }
}