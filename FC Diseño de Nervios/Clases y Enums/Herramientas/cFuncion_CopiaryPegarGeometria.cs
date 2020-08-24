using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas
{
    public static class cFuncion_CopiaryPegarGeometria
    {
        public static List<IElemento> Elementos = new List<IElemento>();
        public static void Copiar(cNervio nervio)
        {
            Elementos.Clear();
            nervio.Lista_Elementos.ForEach(elemento => {
                Elementos.Add(elemento);
            });
        }


        public static void Pegar(cNervio nervio)
        {
            if(cFunctionsProgram.AplicarSimilitud(((cSubTramo)Elementos.Find(y=>y is cSubTramo)).TramoOrigen.NervioOrigen,nervio))
            {
                int C = 0;
                Elementos.ForEach(elemento => {
                    cSeccion Seccion = nervio.Lista_Elementos[C].Seccion;
                    Seccion.B = elemento.Seccion.B; Seccion.H = elemento.Seccion.H;

                    nervio.Lista_Elementos[C].Seccion = Seccion;
                    nervio.Lista_Elementos[C].Longitud = elemento.Longitud;
                    C++;
                });
            }
            else
            {
                cFunctionsProgram.VentanaEmergenteExclamacion("No se puede copiar esta geometría en el nervio.");
            }

        }





    }
}
