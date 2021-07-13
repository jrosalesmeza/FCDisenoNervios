using FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo;
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
        public static List<cBloqueEstribos> Estribos = new List<cBloqueEstribos>();
        public static void CopiarBarras(cNervio Nervio)
        {
            Refuerzos.Clear();
            Nervio.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(Barra => {

                Refuerzos.Add(Barra);
            });

            Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(Barra => {

                Refuerzos.Add(Barra);
            });
        }
        public static void CopiarEstribos(cNervio nervio)
        {
            Estribos.Clear();
            nervio.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.ForEach(B => Estribos.Add(B));
        }



        public static void PegarRefuerzos(cNervio Nervio)
        {
            Refuerzos.ForEach(BC => {

                cBarra BarraClonada = BC.Clone();
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


        public static void PegarEstribos(cNervio nervio)
        {
            Estribos.ForEach(E =>
            {
                if (SaberSiBloqueEstribosEstaPorFuera(E, nervio))
                {
                    int IDF = 0;
                    var BloquEstribosClone = E.Clone();
                    if (nervio.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Count > 0)
                        IDF = nervio.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Max(y => y.ID) + 1;
                    BloquEstribosClone.ID = IDF;
                    BloquEstribosClone.Tendencia_Estribo_Origen = nervio.Tendencia_Refuerzos.TEstriboSelect;
                    nervio.Tendencia_Refuerzos.TEstriboSelect.AgregarBloqueEstribos(BloquEstribosClone, false);
                }
            }); 
        }

     
        private static bool SaberSiEstaPorFueraLaBarra(cBarra barra, cNervio nervio)
        {
            IElemento ElementoFirst = nervio.Lista_Elementos.First();
            IElemento ElementoLast = nervio.Lista_Elementos.Last();
            float Izquierda = ElementoFirst.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) + cVariables.RExtremoIzquierdo;
            float Derecha = ElementoLast.Vistas.Perfil_AutoCAD.Reales.Max(z => z.X) - cVariables.RExtremoDerecho;
            return Izquierda <=(float)Math.Round(barra.XI,cVariables.CifrasDeciLongBarra) && (float)Math.Round(barra.XF, cVariables.CifrasDeciLongBarra) <= Derecha;
        }




        private static bool SaberSiBloqueEstribosEstaPorFuera(cBloqueEstribos bloqueEstribos,cNervio nervio)
        {
            IElemento ElementoFirst = nervio.Lista_Elementos.First();
            IElemento ElementoLast = nervio.Lista_Elementos.Last();
            float Izquierda = ElementoFirst.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) + cVariables.d_CaraApoyo;
            float Derecha = ElementoLast.Vistas.Perfil_AutoCAD.Reales.Max(z => z.X) - cVariables.d_CaraApoyo;
            return Izquierda <= (float)Math.Round(bloqueEstribos.XI, cVariables.CifrasDeciLongBarra) && (float)Math.Round(bloqueEstribos.XF, cVariables.CifrasDeciLongBarra) <= Derecha;
        }





    }
}
