using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas
{
    public static class Extensiones
    {
        public static bool IsNumeric(this string value)
        {
            int.TryParse(value, out int r);
            if (r == 0)
                return false;
            else
                return true;
        }

    }
}
