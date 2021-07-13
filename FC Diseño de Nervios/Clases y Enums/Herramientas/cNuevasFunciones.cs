using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public static partial class cFunctionsProgram
    {
        private static cSubTramo toSubtramo(IElemento elemento)
        {
            if (elemento is cSubTramo)
                return (cSubTramo)elemento;
            return null;
        }
        public static void UnirNervios(List<cNervio> nerviosaUnir,ref List<cNervio> nervios,List<cLine> Todaslines,List<cGrid> grids,cPiso piso,float WWindows,float HWindows,string Prefijo="N-")
        {
            cNervio nervioVertical = nerviosaUnir.Find(y => y.Direccion == eDireccion.Vertical);
            cNervio nervioHorizontal = nerviosaUnir.Find(y => y.Direccion == eDireccion.Horizontal);
            
            if (nervioVertical!=null && nervioHorizontal != null)
            {
                VentanaEmergenteExclamacion("Nervios no compatibles para ser unidos.");
            }
            else
            {
                if(nervioVertical != null)
                {
                    nerviosaUnir = nerviosaUnir.OrderBy(x => x.Lista_Objetos.First(y => y.Soporte == eSoporte.Vano).Line.Planta_Real.Min(z => z.Y)).ToList();
                    CrearNervios(ref nervios);
                }
                else if(nervioHorizontal!=null)
                {
                    nerviosaUnir = nerviosaUnir.OrderBy(x => x.Lista_Objetos.First(y => y.Soporte == eSoporte.Vano).Line.Planta_Real.Min(z => z.X)).ToList();
                    CrearNervios(ref nervios);
                }
                else
                {
                    nerviosaUnir = nerviosaUnir.OrderBy(x => x.Lista_Objetos.First(y => y.Soporte == eSoporte.Vano).Line.Planta_Real.Min(z => z.X)).ToList();
                    CrearNervios(ref nervios);

                }
            }

            void CrearNervios(ref List<cNervio> nervios1)
            {
                if (SePuedeUnirNervios(nerviosaUnir))
                {
                    int ID = nervios1.Max(y => y.ID) + 1;
                    var lines = nerviosaUnir.SelectMany(y => y.Lista_Elementos).ToList().FindAll(y => y.Soporte == eSoporte.Vano).ConvertAll(toSubtramo).SelectMany(y => y.Lista_Lineas).ToList();
                    nervios1.Add(CrearNervio(Prefijo, ID, lines, Todaslines, grids, piso, WWindows, HWindows));
                    foreach(var nervio in nerviosaUnir)
                    {
                        nervios1.Remove(nervio);
                    }
                }
                else
                {
                    VentanaEmergenteExclamacion("Nervios no compatibles para ser unidos.");
                }
            }

        }

        public static bool SePuedeUnirNervios(List<cNervio> nerviosaUnir)
        {
            bool sePuede = true;
            for(int i=0; i < nerviosaUnir.Count; i++)
            {
                cNervio nervioActual = nerviosaUnir[i];
                cNervio nervioDespues = null;

                if (i + 1 < nerviosaUnir.Count) nervioDespues = nerviosaUnir[i + 1];

                if (nervioDespues != null ) //El elemento Acutal es el Primero
                {
                    cLine lineFinalNervioActual= (nervioActual.Lista_Elementos.Last(y => y.Soporte == eSoporte.Vano) as cSubTramo).Lista_Lineas.Last();
                    cLine lineInicialNervioDespues = (nervioDespues.Lista_Elementos.Find(y => y.Soporte == eSoporte.Vano) as cSubTramo).Lista_Lineas.First();

                    if(lineFinalNervioActual.ConfigLinea.Point2P.X != lineInicialNervioDespues.ConfigLinea.Point1P.X || 
                        lineFinalNervioActual.ConfigLinea.Point2P.Y != lineInicialNervioDespues.ConfigLinea.Point1P.Y)
                    {
                        sePuede = false;
                        break;
                    }
                }
  
            }
            return sePuede;
        }


    }
}
