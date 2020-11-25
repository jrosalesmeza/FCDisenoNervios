using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public static class cVariables
    {
        public const float ValueNull = -999999f;

        public const float BubblesizePlano = 1.25f;
        public const float ToleranciaVentanaDiseno = 5f;



        public const float BNervioBorde = 12f;
        /// <summary>
        /// Tolerancia de longitud para similitud entre Nervios
        /// </summary>
        public const float ToleranciaLSimilar = 0.2f;
        public const float AnchoApoyoPredefinido = 40f;
        public const float DiametroEstriboPredeterminado = 1f;
        public const float RecubrimientoNervios = 2f;
        public const float Porc_LongOffset = 0.85f;
        public const float LongMinimaElemento = 0.05f;
        public const float MomentoMinimoPIExtremos = 0.06f;


        #region Variables de Perfil Longitudinal AutoCAD
        public const float AltoMinimoNervio = 3f;
        public const float DeltaNivel = 0.30f;
        public const float DeltaSubNivel = 0.05f;
        public const float RExtremoDerecho = 0.05f;
        public const float RExtremoIzquierdo = 0.05f;
        #endregion


        #region Variables de Barras
        public const float MinimaLongitud = 0.50f;
        public const float MaximaLongitud = 12f;
        public const float DeltaAlargamitoBarras = 0.05f;
        public const float TraslapoNervio = 0.40f;
        public const float ToleranciaTraslapo = 0.1f;
        public const int CifrasDeciLongBarra = 2;
        #endregion





        #region Variables de Tendencia de Refuerzo

        public const float CuantiaMinimaInferior = 0.0018f;
        public const float CuantiaMinimaSuperior = 0.0009f;
        #endregion

        #region Variables de Diseño a Flexión Automatico
        public const float ToleranciaFlexion =5f;
        public const float ToleranciaFlexionPInflexion = 3f;
        public const float ToleranciaFlexionBarras = 3f;
        public const float DeltaUnionCercanas = 0.5f;
        public const float ToleranciAceroFlexion = -0.1f;
        #endregion



        #region Criterios EFE PRMA CE
        public const float Porc_LongAlargamientoExtremo = 0.24f; //Porcentaje de alargamiento desde la cara del apoyo
        public const float LongDelCriterioSubTramo = 4f; //Si el tramo del nervio es <= a 4m
        public const float AlargamientoExtremosEfePrimaCe = 1f; // Si es <=4 alargamiento de 1 m
        #endregion


        #region Variables Cortante

        public const float fi_Cortante = 0.75f;
        public const float d_CaraApoyo = 0.05f;
        public const float ToleranciaDistanciaEstribos = 2f;

        public const int CifrasDeciSepEstribos = 3;
        public const float W_Recuadro_MoverEstribos = 0.08f;
        public const float DeltaEstriboBorde = 0.1f;
        public const float Separacion_MaximaEstribos = 0.30f;



        public const float DeltaH_Estribos_AutoCAD = 0.1f;


        #endregion




        #region Variables AutoCAD
        public const string C_Bordes = "FC_BORDES";
        public const string C_Proyeccion = "FC_PROYECCION";
        public const string C_Cotas = "FC_COTAS";
        public const string C_Ejes = "FC_EJES";
        public const string C_Refuerzo = "FC_REFUERZO";
        public const string C_TextRefuerzo = "FC_R-80";
        public const string C_Texto1 = "FC_R-175";
        public const string Estilo_Texto = "FC_TEXT";
        public const string Estilo_Cotas = "FC_TEXT1";
        public const string C_Estribos = "FC_R-60";


        public const float H_TextoTituloViga = 0.255f;
        public const float H_TextoBarra = 0.15f;
        public const float H_TextoEstribos = 0.1125f;
        public const float H_CuadroTextoBarra = 0.20f;
        public const float W_TextoTituloViga = 0.5f;
        public const float Desplazamiento_Cotas = 0.3f;
        public const float Desplazamiento_Cotas_RefuerzoInferior = 0.15f;
        public const float Desplazamiento_Cotas_RefuerzoSuperior = 0.25f;
        public const float DesplazamientoTexto_Cotas = 0.15f;
        public const float H1_Eje = 0.95f;
        public const float HCentro_Eje = 1.29f;
        public const float DeltaEntreNervios = 3f;

        public const float Ancho_Cajon_Titulo = 1f;

        public const float Dimension_InfoNervio = 2.90f;
        public const float W_LetraAutoCADTitle = 0.08f;
        public const float W_LetraAutoCADEstribos= 0.1f;
        public const float W_LetraAutoCADTextRefuerzo = 0.30f;
        #endregion



        #region Pens
        public static Pen PenElementSinSeleccionar = new Pen(Color.FromArgb(0, 104, 149), 2);
        public static Pen PenElementSeleccionar = new Pen(Color.FromArgb(220, 136, 21), 2);
        public static Pen PenElementisSelectFalse = new Pen(Color.FromArgb(227, 227, 227), 2);

        #endregion

    }
}
