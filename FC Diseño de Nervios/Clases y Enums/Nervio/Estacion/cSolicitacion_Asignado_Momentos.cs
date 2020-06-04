using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSolicitacion_Asignado_Momentos
    {
        public cAreasyMomentos SolicitacionesInferior { get; set; } 
        public cAreasyMomentos SolicitacionesSuperior { get; set; } 

        public cAreasyMomentos AsignadoInferior { get; set; } 
        public cAreasyMomentos AsignadoSuperior { get; set; } 
        public cCalculos CalculosOrigen { get; set; }
        public float AceroFaltanteFlexion_Superior { get; set; }
        public float AceroFaltanteFlexion_Inferior { get; set; }



        private float porcentajeAceroFlexion_Superior;
        public float PorcentajeAceroFlexion_Superior 
        {
            get { return porcentajeAceroFlexion_Superior; }

            set
            {
                porcentajeAceroFlexion_Superior = value;
                AsignarPorcentaje(ref porcentajeAceroFlexion_Superior, SolicitacionesSuperior.Area_Momento);
            }
        }
        private float porcentajeAceroFlexion_Inferior;
        public float PorcentajeAceroFlexion_Inferior
        {
            get { return porcentajeAceroFlexion_Inferior; }
            set
            {
                porcentajeAceroFlexion_Inferior = value;
                AsignarPorcentaje(ref porcentajeAceroFlexion_Inferior, SolicitacionesInferior.Area_Momento);

            }
        }





        private void AsignarPorcentaje(ref float Porcentaje, float AreaMomento)
        {
            if (Porcentaje == float.PositiveInfinity | Porcentaje == float.NegativeInfinity | Porcentaje==-100f)
            {
                if (Math.Round(AreaMomento,2)== 0f)
                {
                    Porcentaje = 0;

                }
                else
                {
                    Porcentaje = -100;


                }
            }

        }

        public cSolicitacion_Asignado_Momentos(cCalculos CalculosOrigen)
        {
            this.CalculosOrigen = CalculosOrigen;
            SolicitacionesInferior = new cAreasyMomentos(this);
            SolicitacionesSuperior = new cAreasyMomentos(this);
            AsignadoInferior = new cAreasyMomentos(this);
            AsignadoSuperior = new cAreasyMomentos(this);


        }
        public override string ToString()
        {
            return $"Inferior y Superior | Solicitación|Asignado";
        }

    }
}