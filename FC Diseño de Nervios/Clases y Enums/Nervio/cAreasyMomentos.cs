using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cAreasyMomentos
    {
        private float area_momento;
        public float Area_Momento { 
            get { return area_momento; }
            
            set { if (area_momento != value) 
                {
                    area_momento = value;
                    CalcularMomento();
                } 
            
            } 
        
        }
        public float Momento { get; set; }


        public void CalcularMomento()
        {
            cCalculos CalculosOrigen = Solicitacion_Asignado_Momentos.CalculosOrigen;
            float B = CalculosOrigen.SubtramoOrigen.Seccion.B; float H = CalculosOrigen.SubtramoOrigen.Seccion.H; float fc = CalculosOrigen.SubtramoOrigen.Seccion.Material.fc;
            float d1 = CalculosOrigen.SubtramoOrigen.TramoOrigen.NervioOrigen.d1; float d2 = CalculosOrigen.SubtramoOrigen.TramoOrigen.NervioOrigen.d2;
            float fy = CalculosOrigen.SubtramoOrigen.Seccion.Material.fy;
            Momento = B_FC_DiseñoVigas.DiseñoYRevisonVigasRectangulares.Revision(B,H,d1,d2,fc,fy,area_momento,0f)[0]* cConversiones.Momento__kgf_cm_to_Ton_m;


        }



        public cSolicitacion_Asignado_Momentos Solicitacion_Asignado_Momentos { get; set; }

        public cAreasyMomentos(cSolicitacion_Asignado_Momentos Solicitacion_Asignado_Momentos)
        {
            this.Solicitacion_Asignado_Momentos = Solicitacion_Asignado_Momentos;
        }

        public override string ToString() => $"A={Math.Round(Area_Momento, 2)} | M={Math.Round(Momento, 2)}";

    }
    [Serializable]
    public class cAreasyCortante
    {
        public float Cortante { get; set; }
        public float Area_Cortante { get; set; }
        public cCalculos CalculosOrigen { get; set; }

        public cAreasyCortante(cCalculos CalculosOrigen)
        {
            this.CalculosOrigen = CalculosOrigen;
        }
        public override string ToString() => $"A={Math.Round(Area_Cortante, 2)} | M={Math.Round(Cortante, 2)}";
    }
}
