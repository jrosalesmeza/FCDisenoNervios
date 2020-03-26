using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cEstacion: cObjetoCoordenadas
    {
        public string Nombre { get; set; }


        public float Localizacion { get; set; }


        public List<cSolicitacion> Combos { get; set; }


        public cCalculos Calculos { get; set; }


        public List<cSolicitacion> Lista_Solicitaciones { get; set; }


        public bool isVisible { get; set; }


        public cAreas_Solicitacion Areas_Solicitacion { get; set; }

    }
}