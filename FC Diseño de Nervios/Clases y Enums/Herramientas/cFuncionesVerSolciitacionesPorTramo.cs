using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;

namespace FC_Diseño_de_Nervios
{
    public static class cFuncionesVerSolciitacionesPorTramo
    {

        /// <summary>
        ///[0. MNegativos, 1. MPositivos, 2. AceroRequeridoNegativo, 3. AceroRequeridoPosivo,
        /// 4. AceroAsignadoNegativo, 5. AceroAsignadoPositivo, 6. Cortante Positivo, 7. Cortante Negativo, 8. A/S Requerido Cortante, 9. A/S Asignado Cortante]        /// </summary>
        /// <param name="subTramo"></param>
        /// <param name="CantidadEstaciones"></param>
        /// <returns></returns>
        private static List<List<float[]>> InterpolacionSolicitaciones(cSubTramo subTramo, int CantidadEstaciones)
        {

            var ValuesX = subTramo.Estaciones.Select(y => y.CoordX).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYMomentosPosit = subTramo.Estaciones.Select(y => y.Calculos.Envolvente.M3[0]).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYMomentosNega = subTramo.Estaciones.Select(y => y.Calculos.Envolvente.M3[1]).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYCortate1 = subTramo.Estaciones.Select(y => y.Calculos.Envolvente.V2[1]).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYCortante2 = subTramo.Estaciones.Select(y => y.Calculos.Envolvente.V2[0]).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYArNegativo = subTramo.Estaciones.Select(y => y.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYArPositivo = subTramo.Estaciones.Select(y => y.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYAAsNegativo = subTramo.Estaciones.Select(y => y.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYAAsPositivo = subTramo.Estaciones.Select(y => y.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYARCortante = subTramo.Estaciones.Select(y => y.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S).ToList().ConvertAll(Convert.ToDouble);
            var ValuesYAAsCortante = subTramo.Estaciones.Select(y => y.Calculos.Solicitacion_Asignado_Cortante.AsignadoSuperior.Area_S).ToList().ConvertAll(Convert.ToDouble);

            var InterpoleteMPositivos = Interpolate.Linear(ValuesX, ValuesYMomentosPosit);
            var InterpoleteMPNega = Interpolate.Linear(ValuesX, ValuesYMomentosNega);
            var InterpoleteYCortante1 = Interpolate.Linear(ValuesX, ValuesYCortate1);
            var InterpoleteYCortante2 = Interpolate.Linear(ValuesX, ValuesYCortante2);
            var InterpoleteArNegativo = Interpolate.Linear(ValuesX, ValuesYArNegativo);
            var InterpoleteArPositivo = Interpolate.Linear(ValuesX, ValuesYArPositivo);
            var InterpoleteAAsNegativo = Interpolate.Linear(ValuesX, ValuesYAAsNegativo);
            var InterpoleteAAsPositivo = Interpolate.Linear(ValuesX, ValuesYAAsPositivo);
            var InterpoleteARCortante = Interpolate.Linear(ValuesX, ValuesYARCortante);
            var InterpoleteAAsCortante = Interpolate.Linear(ValuesX, ValuesYAAsCortante);


            float L = subTramo.Longitud;
            float DeltaL = L / (CantidadEstaciones-1);
            float Xmin = subTramo.Vistas.Perfil_AutoCAD.Reales.First().X;

            List<float[]> R1 = new List<float[]>();
            List<float[]> R2 = new List<float[]>();
            List<float[]> R3 = new List<float[]>();
            List<float[]> R4 = new List<float[]>();
            List<float[]> R5 = new List<float[]>();
            List<float[]> R6 = new List<float[]>();
            List<float[]> R7 = new List<float[]>();
            List<float[]> R8 = new List<float[]>();
            List<float[]> R9 = new List<float[]>();
            List<float[]> R10 = new List<float[]>();

            for (float Delta = 0; Math.Round(Delta,2) <= L; Delta += DeltaL)
            {
                R1.Add(Interpolacion(Xmin, Delta, InterpoleteMPNega));
                R2.Add(Interpolacion(Xmin, Delta, InterpoleteMPositivos));
                R3.Add(Interpolacion(Xmin, Delta, InterpoleteArNegativo));
                R4.Add(Interpolacion(Xmin, Delta, InterpoleteArPositivo));
                R5.Add(Interpolacion(Xmin, Delta, InterpoleteAAsNegativo));
                R6.Add(Interpolacion(Xmin, Delta, InterpoleteAAsPositivo));
                R7.Add(Interpolacion(Xmin, Delta, InterpoleteYCortante1));
                R8.Add(Interpolacion(Xmin, Delta, InterpoleteYCortante2));
                R9.Add(Interpolacion(Xmin, Delta, InterpoleteARCortante));
                R10.Add(Interpolacion(Xmin, Delta, InterpoleteAAsCortante));
            }

            List<List<float[]>> Resultados = new List<List<float[]>>() { R1, R2, R3, R4, R5, R6, R7, R8, R9, R10 };
            return Resultados;
        }


        public static DataTable TablaSolicitaciones(cSubTramo subTramo, int CantidadEstaciones)
        {
            var Resultados = InterpolacionSolicitaciones(subTramo, CantidadEstaciones);
            var MPositvos = Resultados.First();
            DataTable DataTable = new DataTable("Solicitaciones");
            for (int i = -1; i < Resultados.First().Count; i++) 
            {
                if (i != -1)
                {
                    DataTable.Columns.Add(string.Format("{0:0.00}", Resultados.First()[i][0]));
                }
                else
                {
                    DataTable.Columns.Add("Localización [m]");
                }
            }

            List<string> EncabezadosFilas = new List<string>() { "M- [Ton·m]", "M+ [Ton·m]", 
                "AsReq- [cm²]", "AsReq+ [cm²]", "AsAsig- [cm²]", 
                "AsAsig+ [cm²]", "V+ [Ton]", "V- [Ton]", "As/S Req [cm²/cm]", "As/S Asig [cm²/cm]" };

            for (int i = 0; i < EncabezadosFilas.Count; i++)
            {
                DataRow row = DataTable.NewRow();
                string Formato = "{0:0.00}";
                if (i > 7)
                {
                    Formato = "{0:0.000}";
                }
                int c = 0;
                Resultados[i].ForEach(R =>
                {
                    if (c == 0)
                    {

                        row[c] =  "   " +EncabezadosFilas[i];
                    }
 
                    row[c + 1] = " "+string.Format(Formato, R[1]); c++;
                }
                
                );
                DataTable.Rows.Add(row);
            }



            return DataTable;

        }







        private static float[] Interpolacion(float Xmin,float value, IInterpolation interpolate)
        {
            return new float[]{Xmin+value,(float)interpolate.Interpolate(value)};

        }


 






    }
}
