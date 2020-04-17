using System;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSolicitacion
    {
        public cEstacion EstacionOrigen { get; set; }
        public string Nombre { get; set; }

        public float P { get; set; }

        public float V2 { get; set; }

        public float V3 { get; set; }

        public float M2 { get; set; }

        public float M3 { get; set; }

        public float T { get; set; }
        public bool SelectEnvolvente { get; set; } = true;

        public cSolicitacion(string Nombre, float P, float V2, float V3, float M2, float M3, float T)
        {
            this.Nombre = Nombre;
            this.P = P;
            this.V2 = V2;
            this.V3 = V3;
            this.M2 = M2;
            this.M3 = M3;
            this.T = T;
        }

        public override string ToString()
        {
            return $"{Nombre} | V2={V2} | V3={V3} | M3= {M3} ";
        }
    }
}