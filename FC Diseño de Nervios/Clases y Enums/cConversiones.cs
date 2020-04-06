﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public static class cConversiones
    {
        public const float Esfuerzo_Ton_m_to_kfg_cm = 1/10f;
        public const float Esfuerzo_kfg_cm_to_Ton_m = 10f;
        public const float Dimension_m_to_cm = 100f;
        public const float Dimension_cm_to_m = 1/100f;
        public const float Angulo_Rad_to_Grad = 180f / (float)Math.PI;
    }
}
