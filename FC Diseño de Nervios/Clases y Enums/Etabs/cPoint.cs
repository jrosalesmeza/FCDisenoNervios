using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cPoint
    {
        public cPoint(string Nombre,float X, float Y,float DeltaZ)
        {
            this.Nombre = Nombre;
            this.X = X;
            this.Y = Y;
            this.DeltaZ = DeltaZ;
        }

        public string Nombre { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float DeltaZ { get; set; }
        public override string ToString()
        {
            return $"{Nombre} ({X},{Y},{Z})";
        }
    }
}