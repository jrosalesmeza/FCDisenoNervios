using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas
{
    public static class cFuncion_CopiaryPegarRefuerzos
    {
        public static List<cBarra> Refuerzos = new List<cBarra>();

        public static void Copiar(cNervio Nervio)
        {
            Refuerzos.Clear();
            Nervio.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(Barra => {

                Refuerzos.Add(Barra);
            });

            Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(Barra => {

                Refuerzos.Add(Barra);
            });
        }

        public static void Pegar(cNervio Nervio)
        {
            Refuerzos.ForEach(BC => {

                cBarra BarraClonada = cFunctionsProgram.DeepClone(BC);
                if (SaberSiEstaPorFueraLaBarra(BarraClonada, Nervio))
                {
                    if (BarraClonada.UbicacionRefuerzo == eUbicacionRefuerzo.Inferior)
                    {
                        int IDF = 0; cTendencia Tendencia = Nervio.Tendencia_Refuerzos.TInfeSelect;
                        if (Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.Count > 0)
                        {
                            IDF = Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.Max(y => y.ID) + 1;
                            Tendencia = Nervio.Tendencia_Refuerzos.TInfeSelect;
                        }

                        BarraClonada.ID = IDF;
                        BarraClonada.TendenciaOrigen = Tendencia;
                        Nervio.Tendencia_Refuerzos.TInfeSelect.AgregarBarra(BarraClonada);
                    }
                    else
                    {
                        int IDF = 0; cTendencia Tendencia = Nervio.Tendencia_Refuerzos.TSupeSelect;
                        if (Nervio.Tendencia_Refuerzos.TSupeSelect.Barras.Count > 0)
                        {
                            IDF = Nervio.Tendencia_Refuerzos.TSupeSelect.Barras.Max(y => y.ID) + 1;
                            Tendencia = Nervio.Tendencia_Refuerzos.TSupeSelect.Barras.Last().TendenciaOrigen;
                        }
                        BarraClonada.ID = IDF;
                        BarraClonada.TendenciaOrigen = Tendencia;
                        Nervio.Tendencia_Refuerzos.TSupeSelect.AgregarBarra(BarraClonada);
                    }
                }
            });



        }


     
        private static bool SaberSiEstaPorFueraLaBarra(cBarra barra, cNervio nervio)
        {
            IElemento ElementoFirst = nervio.Lista_Elementos.First();
            IElemento ElementoLast = nervio.Lista_Elementos.Last();
            float Izquierda = ElementoFirst.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) + nervio.r1 * cConversiones.Dimension_cm_to_m;
            float Derecha = ElementoLast.Vistas.Perfil_AutoCAD.Reales.Max(z => z.X) - nervio.r1 * cConversiones.Dimension_cm_to_m;
            return Izquierda <=(float)Math.Round(barra.XI,cVariables.CifrasDeciLongBarra) && (float)Math.Round(barra.XF, cVariables.CifrasDeciLongBarra) <= Derecha;
        }



        

    }
}
