using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cDatosEtabs
    {
        public List<cLine> List_Line { get; set; }
      
        public List<cPoint> Lista_Points { get; set; }

        public List<cSeccion> Lista_Secciones { get; set; }

        public List<cMaterial> Lista_Materiales { get; set; }

        public List<cPiso> Lista_Pisos { get; set; }

        
    }
}