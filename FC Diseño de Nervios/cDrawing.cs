using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cDrawing
    {
        public List<PointF> ListaPuntos { get; set; }


        public bool Select { get; set; }

    }
}