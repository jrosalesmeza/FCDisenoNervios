using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public static class cVariables
    {

        public const float ToleranciaVentanaDiseno = 5f;




        #region Variables de Perfil Longitudinal AutoCAD
        public const float AltoMinimoNervio = 3f;
        public const float DeltaNivel = 0.30f;
        public const float DeltaSubNivel = 0.05f;
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

        public const float DeltaH_Estribos_AutoCAD = 0.1f;

        #endregion




        #region Variables AutoCAD
        public const string C_Bordes = "FC_BORDES";
        public const string C_Proyeccion = "FC_PROYECCION";
        public const string C_Cotas = "FC_COTAS";

        public const float Dimension_InfoNervio = 2.90f;
        #endregion

    }
}
