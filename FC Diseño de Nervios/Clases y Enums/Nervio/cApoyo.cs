using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cApoyo : IElemento
    {
        public string Nombre { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Apoyo;
        public cSeccion Seccion { get; set; }
        public cVistas Vistas { get; set; } = new cVistas();


        public cApoyo(string Nombre,cSeccion Seccion)
        {
            this.Nombre = Nombre;
            this.Seccion = Seccion;
        }

        public override string ToString()
        {
            return $"{Nombre} | {Seccion.ToString()}";
        }
    }
}
