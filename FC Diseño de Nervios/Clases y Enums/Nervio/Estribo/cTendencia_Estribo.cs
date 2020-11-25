using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo
{

    [Serializable]
    public class cTendencia_Estribo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public cTendencia_Refuerzo Tendencia_Refuerzo_Origen { get; set; }
        public float Peso { get; private set; }
        public List<cBloqueEstribos> BloqueEstribos { get; set; } = new List<cBloqueEstribos>();
        private List<eNoBarra> barrasAEmplear;
        public List<eNoBarra> BarrasAEmplear
        {
            get
            {
                if (barrasAEmplear == null)
                {
                    return new List<eNoBarra>() { eNoBarra.B2, eNoBarra.B3, eNoBarra.B4, eNoBarra.B5, eNoBarra.B6 };
                }
                return barrasAEmplear;
            }
            set
            {
                if (value!= barrasAEmplear)
                {
                    barrasAEmplear = value;
                }
            }
        }

        public void AgregarBloqueEstribos(cBloqueEstribos bloqueEstribos, bool ACaraApoyo)
        {
            if (!ACaraApoyo)
            {
                if (BloqueEstribos.Count > 0)
                {
                    BloqueEstribos = BloqueEstribos.OrderBy(y => y.XI).ToList();

                    foreach (var bloque in BloqueEstribos)
                    {
                        float CorrerGrupoEstribos = bloque.LongitudZonaEstribos + bloque.Separacion + cVariables.DeltaEstriboBorde;
                        if (bloqueEstribos.IsVisible(bloque))
                        {
                            if (bloque.DireccionEstribo == eLadoDeZona.Derecha)
                                bloqueEstribos.XI += CorrerGrupoEstribos;
                            else
                                bloqueEstribos.XF -= CorrerGrupoEstribos;
                            break;
                        }

                    }
                }
                BloqueEstribos.Add(bloqueEstribos);

            }
            else
            {
                BloqueEstribos.Add(bloqueEstribos);
                bloqueEstribos.MoveraCaraApoyo();

            }

            ActualizarRefuerzoTransversal();
        }
        public void EliminarBloquesEstribos()
        {
            BloqueEstribos.Clear();
            ActualizarRefuerzoTransversal();
        }
        public void ElminarBloqueEstribos(cBloqueEstribos bloqueEstribos)
        {
            BloqueEstribos.Remove(bloqueEstribos);
            ActualizarRefuerzoTransversal();
            Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignadoRefuerzoTransversal();
        }

        public void ActualizarRefuerzoTransversal()
        {
            Peso = 0f;
            BloqueEstribos.ForEach(y => Peso += y.PesoTransversal);
            Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignadoRefuerzoTransversal();
        }


        public void LimpiarTendencia()
        {
            Peso = 0f;
            BloqueEstribos.Clear();
        }



        public cTendencia_Estribo(int ID,cTendencia_Refuerzo Tendencia_Refuerzo_Origen)
        {
            this.ID = ID;
            Nombre = $"Tendencia {ID+1}";
            this.Tendencia_Refuerzo_Origen = Tendencia_Refuerzo_Origen;
        }


        public override string ToString()
        {
            return $"{Nombre} | {BloqueEstribos.Count}";
        }
    }
}
