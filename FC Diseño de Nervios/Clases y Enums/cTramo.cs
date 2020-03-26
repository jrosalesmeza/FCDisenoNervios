using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cTramo
    {
        public int ID { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }

        public string Nombre { get; set; }

        public float Longitud { get; set; }


        public cTramo(string Nombre,List<cObjeto> Lista_Objetos)
        {
            this.Nombre = Nombre;
            this.Lista_Objetos = Lista_Objetos;

        }

        public List<cEstribo> Estribo { get; set; }



        public override string ToString()
        {
            return $"{Nombre} | L={Longitud} | CountObjetos= {Lista_Objetos.Count}";
        }


    }
}