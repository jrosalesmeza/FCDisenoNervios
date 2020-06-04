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
                } 
            
            } 
        
        }
        public float Momento { get; set; }

        public void CalcularMomento(float d2)
        {
            cSubTramo SubTramo = Solicitacion_Asignado_Momentos.CalculosOrigen.SubtramoOrigen;
            float B = SubTramo.Seccion.B; float H = SubTramo.Seccion.H; float fc = SubTramo.Seccion.Material.fc;
            float fy = SubTramo.Seccion.Material.fy;
            Momento = B_FC_DiseñoVigas.DiseñoYRevisonVigasRectangulares.Revision(B, H, d2, d2, fc, fy, area_momento, 0f)[0] * cConversiones.Momento__kgf_cm_to_Ton_m;

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
        public float AreaMin { get; set; }
        private float cortante;
        public float Cortante { 
            get { return cortante; }
            set
            {
                cortante = value;
                AgregarArea_SSegunCortante();

            }
        }
        public float Area_S { get; set; }
        public c_Solicitacion_Asignado_Cortante Solicitacion_Asignado_Cortante { get; set; }
        public cAreasyCortante(c_Solicitacion_Asignado_Cortante Solicitacion_Asignado_Cortante)
        {
            this.Solicitacion_Asignado_Cortante = Solicitacion_Asignado_Cortante;
        }

        private void AgregarArea_SSegunCortante()
        {
            float FI_VC = Solicitacion_Asignado_Cortante.CalculosOrigen.Envolvente.FI_Vc;
            float FI_VS = Math.Abs(cortante) - FI_VC;
            float Fc = Solicitacion_Asignado_Cortante.CalculosOrigen.SubtramoOrigen.Seccion.Material.fc;
            float b = Solicitacion_Asignado_Cortante.CalculosOrigen.SubtramoOrigen.Seccion.B;
            float Fy = Solicitacion_Asignado_Cortante.CalculosOrigen.SubtramoOrigen.Seccion.Material.fy;
            float d = Solicitacion_Asignado_Cortante.CalculosOrigen.SubtramoOrigen.Seccion.H - Solicitacion_Asignado_Cortante.CalculosOrigen.SubtramoOrigen.TramoOrigen.NervioOrigen.r1;
            AreaMin= 0.2f * (float)Math.Sqrt(Fc) * b / Fy;
            if (Math.Abs(cortante) > FI_VC)
            {
                Area_S = (FI_VS*cConversiones.Fuerza_Ton_to_kgf) / (Fy * d * cVariables.fi_Cortante);
                if(Area_S< AreaMin)
                {
                    Area_S = AreaMin;
                }
            }
            else if (Math.Abs(cortante) <= FI_VC && FI_VC/2f <= Math.Abs(cortante))
            {
                Area_S = AreaMin;
            }
            else if(Math.Abs(cortante) < FI_VC / 2f)
            {
                Area_S = 0f;
            }
        }
        public override string ToString() => $"A={Math.Round(Area_S, 2)} | V={Math.Round(Cortante, 2)}";
    }
}
