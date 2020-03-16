using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cObjetoCoordenadas
    {
        public List<PointF> Longitudinal_Real
        {
            get => default;
            set
            {
            }
        }

        public List<PointF> Longitudinal_Escalado
        {
            get => default;
            set
            {
            }
        }

        public List<PointF> Planta_Escalado
        {
            get => default;
            set
            {
            }
        }

        public List<PointF> Planta_Real
        {
            get => default;
            set
            {
            }
        }

        public bool Select
        {
            get => default;
            set
            {
            }
        }
    }
}