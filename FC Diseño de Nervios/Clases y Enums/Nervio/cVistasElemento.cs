﻿using System;
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




    }


    [Serializable]
    public enum eSoporte
    {
        Vano,
        Apoyo
    }



}
