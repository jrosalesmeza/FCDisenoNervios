using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cLine:cObjetoCoordenadas
    {
        public string Nombre { get; set; }


        public List<cEstacion> Estaciones { get; set; }

        public cSeccion Seccion { get; set; }


        public eType Type { get; set; }


        public eTipoSoporte Soporte { get; set; }


        public cLine ObjDerecha { get; set; }


        public cConfigEtabs ConfigEtabs { get; set; }


        public string Story { get; set; }

    }
}