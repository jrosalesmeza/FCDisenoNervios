﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public interface IElemento
    {
        string Nombre { get; set; }
        eSoporte Soporte { get; }
        cSeccion Seccion { get; set; }
        cVistas Vistas { get; set; }
    }
}
