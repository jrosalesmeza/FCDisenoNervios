﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cViga
    {
        public List<cTramo> Lista_Tramos { get; set; }


        public string Nombre { get; set; }


        public string Prefijo { get; set; }

    }
}