using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas
{
    public static class Extensiones
    {

        public static cEstacion EstacionMasCercana(this List<cEstacion> estaciones, float X)
        {
            float MenorDistancia = 99999; cEstacion EstacionCercana = null;
            foreach (cEstacion Estacion in estaciones)
            {
                float Distancia = Math.Abs(Estacion.CoordX + Estacion.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.First().X - X);

                if (Distancia < MenorDistancia)
                {
                    MenorDistancia = Distancia;
                    EstacionCercana = Estacion;
                }
            }
            return EstacionCercana;
        }

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
