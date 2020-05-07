﻿using B_FC_DiseñoVigas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    public delegate void DelegateChangeCrearEnvolvente();
    [Serializable]
    public class cEnvolvente
    {
        private List<cSolicitacion> lista_solicitaciones;
        public List<cSolicitacion> Lista_Solicitaciones
        {
            get { return lista_solicitaciones; }
            set
            {
                if (lista_solicitaciones != value)
                {
                    lista_solicitaciones = value;
                }

            }

        }
        public cCalculos CalculosOrigen { get; set; }

        public cEnvolvente(List<cSolicitacion> lista_solicitaciones,cCalculos CalculosOrigen)
        {
            this.lista_solicitaciones = lista_solicitaciones;
            this.CalculosOrigen = CalculosOrigen;
            CrearEnvolvente();
        }

        private void CrearEnvolvente()
        {
            float M3MaxPositivo = lista_solicitaciones.FindAll(x=>x.SelectEnvolvente).Max(x => x.M3);
            float M3MaxNegativo = lista_solicitaciones.FindAll(x => x.SelectEnvolvente).Min(x => x.M3);
            float V2MaxPositivo = lista_solicitaciones.FindAll(x => x.SelectEnvolvente).Max(x => x.V2);
            float V2MaxNegativo = lista_solicitaciones.FindAll(x => x.SelectEnvolvente).Min(x => x.V2);
            if (M3MaxPositivo < 0) { M3MaxPositivo = 0;}
            if (M3MaxNegativo > 0) { M3MaxNegativo = 0;  }
            if (V2MaxPositivo < 0) { V2MaxPositivo = 0; }
            if (V2MaxNegativo > 0) { V2MaxNegativo = 0; }
            M3 = new float[] { M3MaxPositivo, M3MaxNegativo };
            V2 = new float[] { V2MaxPositivo, V2MaxNegativo };
            Envolvente_CambioCrearEnvolvente();
        }

        private void Envolvente_CambioCrearEnvolvente()
        {
            float B = CalculosOrigen.SubtramoOrigen.Seccion.B; float H = CalculosOrigen.SubtramoOrigen.Seccion.H; float fc = CalculosOrigen.SubtramoOrigen.Seccion.Material.fc;
            float d1 = CalculosOrigen.SubtramoOrigen.TramoOrigen.NervioOrigen.d1; float d2 = CalculosOrigen.SubtramoOrigen.TramoOrigen.NervioOrigen.d2;
            float fy = CalculosOrigen.SubtramoOrigen.Seccion.Material.fy;float M3_0 = Math.Abs(M3[0]);float M3_1 = Math.Abs(M3[1]);
            if (M3_0 == 0)
                M3_0 = 0.001f;
            if (M3_1 == 0)
                M3_1 = 0.001f;
            float[] AreaAporteInferior = DiseñoYRevisonVigasRectangulares.Diseñar(B, H, d1, d2, fc, fy, M3_0 * cConversiones.Momento_Ton_m_to_kgf_cm, DiseñoYRevisonVigasRectangulares.eTipoViga.NoSismica);
            float[] AreaAporteSuperior = DiseñoYRevisonVigasRectangulares.Diseñar(B, H, d1, d2, fc, fy, M3_1 * cConversiones.Momento_Ton_m_to_kgf_cm, DiseñoYRevisonVigasRectangulares.eTipoViga.NoAplica);
            float ASuperior = AreaAporteSuperior[0];

            if (B >= 12) { if (ASuperior < 0.0009f * B * H) { ASuperior = 0.0009f * B * H; } } //Criterios de F'C


            CalculosOrigen.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento = AreaAporteInferior[0];
            CalculosOrigen.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento = ASuperior;
            CalculosOrigen.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Momento = M3[0];
            CalculosOrigen.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Momento = M3[1];
        }


        public string Nombre { get; set; }
        /// <summary>
        /// M3[Inferior, Superior]
        /// </summary>
        public float[] M3 { get; set; }
        public float[] V2 { get; set; }



    }
}