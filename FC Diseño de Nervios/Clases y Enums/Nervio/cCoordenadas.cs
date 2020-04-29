﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cCoordenadas
    {
        public List<PointF> Reales { get; set; }
        public List<PointF> Escaladas { get; set; }
        public PointF Y0_Escalado { get; set; }
        public bool IsSelect { get; set; }
        public bool IsSelectArrastre { get; set; }

    }


}
