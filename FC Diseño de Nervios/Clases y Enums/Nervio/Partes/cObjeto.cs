using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cObjeto
    {
        public int ID { get; set; }
        public cLine Line { get; set; }
        public float Longitud { get; set; }
        public cVistas Vistas { get; set; } = new cVistas();
        public eSoporte Soporte { get; set; }
        public cObjeto(cLine Line, eSoporte Soporte)
        {
            this.Line = Line;
            this.Soporte = Soporte;
        }
        public override string ToString()
        {
            return $"{Line.Nombre} | {Soporte}";
        }

    }

    [Serializable]
    public enum eSoporte
    {
        Vano,
        Apoyo
    }


}
