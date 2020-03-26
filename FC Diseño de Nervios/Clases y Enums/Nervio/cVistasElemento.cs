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
        public List<PointF> Coordenadas_Perfil { get; set; }
        public List<PointF> Coordenadas_Planta { get; set; }

        public eCambioenAltura CambioenAltura { get; set; }
        public eCambioenAncho CambioenAncho { get; set; }




    }

    [Serializable]
    public enum eCambioenAltura
    {
        Superior,
        Inferior
    }
    [Serializable]
    public enum eCambioenAncho
    {
        Central,    
        Superior,  //Izquierda
        Inferior  //Derecha
    }

    public enum eSoporte
    {
        Vano,
        Apoyo
    }



}
