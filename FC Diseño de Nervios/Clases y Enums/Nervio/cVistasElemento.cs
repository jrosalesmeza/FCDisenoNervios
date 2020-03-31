using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cVistas
    {
        public cCoordenadas Perfil_Original { get; set; } = new cCoordenadas();
        public cCoordenadas Perfil_AutoCAD { get; set; } = new cCoordenadas();
               
       // public List<PointF> Coordenadas_Planta { get; set; }
    }






}
