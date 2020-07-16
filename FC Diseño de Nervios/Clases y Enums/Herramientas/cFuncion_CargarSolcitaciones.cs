using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas
{
    public static class cFuncion_CargarSolcitaciones
    {
        public static void CargarNuevasSolicitacionesANevios(List<string> ArchivoE2K,List<cNervio> nervios)
        {
            List<cEstacion> EstacionesNuevas = cFunctionsProgram.CrearEstaciones(ArchivoE2K);

            nervios.ForEach(N =>
            {
                CargarNuevasSolicitacionesANevio(EstacionesNuevas, N);
            });

        }


        public static void CargarNuevasSolicitacionesANevio(List<cEstacion> EstacionesNuevas, cNervio nervio)
        {
            if (ComprobarAsignacionEstacionesEnNervio(EstacionesNuevas,nervio))
            {
                List<cEstacion> EstacionesNuevas1 = EstacionesNuevas.FindAll(z => nervio.PisoOrigen.Nombre == z.StoryLinea);
                List<cEstacion> Estaciones = new List<cEstacion>();
                nervio.Lista_Elementos.ForEach(elemento =>
                {
                    if (elemento is cSubTramo)
                    {
                        cSubTramo subtramo = (cSubTramo)elemento;
                        Estaciones.AddRange(subtramo.Estaciones);
                    }

                });

                EstacionesNuevas1.ForEach(EstacionN =>
                {
                    Estaciones.ForEach(EstacionA =>
                    {

                        if (EstacionA.Equals(EstacionN))
                        {

                            EstacionN.Lista_Solicitaciones.ForEach(Solic => {
                                AsignacionNombreCombinacionRepetida(Solic.Nombre, Solic, EstacionA.Lista_Solicitaciones, 1);
                                EstacionA.Lista_Solicitaciones.Add(Solic);
                            });
                            
                        }
                    });
                });

            }
            else
            {
                cFunctionsProgram.VentanaEmergenteError($"No se puede agregar las nuevas solicitaciones en el nervio {nervio.Nombre}.");
            }
        }
        private static void AsignacionNombreCombinacionRepetida(string Nombre,cSolicitacion SoliModificar,List<cSolicitacion> Solicitaciones, int C=1)
        {
            if(Solicitaciones.Find(x=>x.Nombre== Nombre) != null)
            {
                if (Nombre.Contains("-"))
                {
                    string AntString = Nombre.Substring(Nombre.IndexOf("-"), Nombre.Length - Nombre.IndexOf("-"));
                    Nombre = Nombre.Replace(AntString, "-"+C.ToString());
                    Nombre.IndexOf("-");

                }
                else
                {
                    Nombre += "-" + C.ToString();
                }
                C++;

                AsignacionNombreCombinacionRepetida(Nombre, SoliModificar, Solicitaciones, C);
                
            }
            else
            {
                SoliModificar.Nombre = Nombre;
            }

        }
        private static bool ComprobarAsignacionEstacionesEnNervio(List<cEstacion> EstacionesNuevas, cNervio nervio)
        {
            List<cEstacion> Estaciones = new List<cEstacion>();
            List<cEstacion> EstacionesNuevas1 = EstacionesNuevas.FindAll(z => nervio.PisoOrigen.Nombre == z.StoryLinea);

            nervio.Lista_Elementos.ForEach(elemento =>
            {
                if (elemento is cSubTramo)
                {
                    cSubTramo subtramo = (cSubTramo)elemento;
                    Estaciones.AddRange(subtramo.Estaciones);
                }

            });

            int CantEstacionEquals = 0;

            EstacionesNuevas1.ForEach(EstacionN =>
            {
                Estaciones.ForEach(EstacionA =>
                {

                    if (EstacionA.Equals(EstacionN))
                    {
                        CantEstacionEquals++;
                    }
                });
            });
            return CantEstacionEquals == Estaciones.Count;
        }
        public static List<cSolicitacion> ListaConMayorSolicitaciones(List<cNervio> Nervios)
        {
            List<List<cSolicitacion>> solicitacions = new List<List<cSolicitacion>>();
            List<cSolicitacion> SolicitacionesFinales = new List<cSolicitacion>();
            List<IElemento> subtramosTotales = new List<IElemento>();
            Nervios.ForEach(nervio => subtramosTotales.AddRange(nervio.Lista_Elementos.FindAll(y => y is cSubTramo)));

            subtramosTotales.ForEach(y => { solicitacions.AddRange(((cSubTramo)y).Estaciones.Select(z => z.Lista_Solicitaciones).ToList()); });
            solicitacions.ForEach(y => SolicitacionesFinales.AddRange(y.DistinctBy(z => z.Nombre).ToList()));

            return SolicitacionesFinales.DistinctBy(m => m.Nombre).ToList() ;
        }


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
