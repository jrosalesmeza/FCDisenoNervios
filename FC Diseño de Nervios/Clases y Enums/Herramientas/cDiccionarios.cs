using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public class cDiccionarios
    {
        /// <summary>
        /// Diametros en [cm]
        /// </summary>
        public static Dictionary<eNoBarra, float> DiametrosBarras = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_210 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_280 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_350 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_420 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_490 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_560 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> AceroBarras = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> G90;
        public static Dictionary<eNoBarra, float> PesoBarras = new Dictionary<eNoBarra, float>();

        /// <summary>
        /// Diccionario [NoBarra, (L,D)]
        /// </summary>
        public static Dictionary<eNoBarra, Tuple<float, float>> G135;
        /// <summary>
        /// Diccionario [NoBarra, (L,D)]
        /// </summary>
        public static Dictionary<eNoBarra, Tuple<float, float>> G180;
        public static Dictionary<eNoBarra, Color> ColorBarra;

        public cDiccionarios()
        {
            LlenarDiccionarioDiametrosBarras();
            LlenarDiccionarioAceroBarras();
            LlenarDiccionariosGanchos();
            LlenarColoresBarra();
            LLenarDiccionariosLds(); LlenarDiccionarioPesoBarras();
        }

        private static void LlenarDiccionarioDiametrosBarras()
        {
            DiametrosBarras.Add(eNoBarra.BNone, 0f);
            DiametrosBarras.Add(eNoBarra.B2, 0.64f);
            DiametrosBarras.Add(eNoBarra.B3, 0.95f);
            DiametrosBarras.Add(eNoBarra.B4, 1.27f);
            DiametrosBarras.Add(eNoBarra.B5, 1.59f);
            DiametrosBarras.Add(eNoBarra.B6, 1.91f);
            DiametrosBarras.Add(eNoBarra.B7, 2.22f);
            DiametrosBarras.Add(eNoBarra.B8, 2.54f);
            DiametrosBarras.Add(eNoBarra.B10, 3.23f);
        }
        private static void LlenarDiccionarioAceroBarras()
        {
            AceroBarras.Add(eNoBarra.BNone, 0f);
            AceroBarras.Add(eNoBarra.B2, 0.32f);
            AceroBarras.Add(eNoBarra.B3, 0.71f);
            AceroBarras.Add(eNoBarra.B4, 1.29f);
            AceroBarras.Add(eNoBarra.B5, 1.99f);
            AceroBarras.Add(eNoBarra.B6, 2.84f);
            AceroBarras.Add(eNoBarra.B7, 3.87f);
            AceroBarras.Add(eNoBarra.B8, 5.10f);
            AceroBarras.Add(eNoBarra.B10, 8.09f);
        }
        private static void LlenarDiccionariosGanchos()
        {
            G90 = new Dictionary<eNoBarra, float>
            {
                { eNoBarra.BNone, 0.0f },
                { eNoBarra.B2, 0.09f },
                { eNoBarra.B3, 0.14f },
                { eNoBarra.B4, 0.18f },
                { eNoBarra.B5, 0.23f },
                { eNoBarra.B6, 0.27f },
                { eNoBarra.B7, 0.32f },
                { eNoBarra.B8, 0.36f },
                { eNoBarra.B10, 0.47f }
            };

            G135 = new Dictionary<eNoBarra, Tuple<float, float>>
            {
                { eNoBarra.BNone, new  Tuple<float, float>(0f,0f)},
                { eNoBarra.B2, new  Tuple<float, float>(0.063f,0.026f)},
                { eNoBarra.B3, new  Tuple<float, float>(0.094f,0.038f)},
                { eNoBarra.B4, new  Tuple<float, float>(0.125f,0.051f)},
                { eNoBarra.B5, new  Tuple<float, float>(0.157f,0.064f)},
                { eNoBarra.B6, new  Tuple<float, float>(0.214f,0.115f)},
                { eNoBarra.B7, new  Tuple<float, float>(0.249f,0.133f)},
                { eNoBarra.B8, new  Tuple<float, float>(0.286f,0.152f)},
                { eNoBarra.B10,new  Tuple<float, float>(0.363f,0.194f) }
            };

            G180 = new Dictionary<eNoBarra, Tuple<float, float>>
            {
                { eNoBarra.BNone, new  Tuple<float, float>(0f,0f)},
                { eNoBarra.B2, new  Tuple<float, float>(0.116f,0.038f) },
                { eNoBarra.B3, new  Tuple<float, float>(0.140f,0.057f)},
                { eNoBarra.B4, new  Tuple<float, float>(0.167f,0.076f) },
                { eNoBarra.B5, new  Tuple<float, float>(0.192f,0.095f) },
                { eNoBarra.B6, new  Tuple<float, float>(0.228f,0.115f) },
                { eNoBarra.B7, new  Tuple<float, float>(0.266f,0.133f) },
                { eNoBarra.B8, new  Tuple<float, float>(0.305f,0.152f) },
                { eNoBarra.B10,new  Tuple<float, float>(0.457f,0.258f) }
            };
        }
        private static void LLenarDiccionariosLds()
        {
            Ld_210.Add(eNoBarra.B2, 0.55f);
            Ld_210.Add(eNoBarra.B3, 0.55f);
            Ld_210.Add(eNoBarra.B4, 0.75f);
            Ld_210.Add(eNoBarra.B5, 0.9f);
            Ld_210.Add(eNoBarra.B6, 1.1f);
            Ld_210.Add(eNoBarra.B7, 1.6f);
            Ld_210.Add(eNoBarra.B8, 1.8f);
            Ld_210.Add(eNoBarra.B10, 2.3f);

            Ld_280.Add(eNoBarra.B2, 0.5f);
            Ld_280.Add(eNoBarra.B3, 0.5f);
            Ld_280.Add(eNoBarra.B4, 0.65f);
            Ld_280.Add(eNoBarra.B5, 0.8f);
            Ld_280.Add(eNoBarra.B6, 0.95f);
            Ld_280.Add(eNoBarra.B7, 1.4f);
            Ld_280.Add(eNoBarra.B8, 1.6f);
            Ld_280.Add(eNoBarra.B10, 2.0f);

            Ld_350.Add(eNoBarra.B2, 0.45f);
            Ld_350.Add(eNoBarra.B3, 0.45f);
            Ld_350.Add(eNoBarra.B4, 0.55f);
            Ld_350.Add(eNoBarra.B5, 0.7f);
            Ld_350.Add(eNoBarra.B6, 0.85f);
            Ld_350.Add(eNoBarra.B7, 1.25f);
            Ld_350.Add(eNoBarra.B8, 1.4f);
            Ld_350.Add(eNoBarra.B10, 1.8f);

            Ld_420.Add(eNoBarra.B2, 0.4f);
            Ld_420.Add(eNoBarra.B3, 0.4f);
            Ld_420.Add(eNoBarra.B4, 0.55f);
            Ld_420.Add(eNoBarra.B5, 0.65f);
            Ld_420.Add(eNoBarra.B6, 0.8f);
            Ld_420.Add(eNoBarra.B7, 1.15f);
            Ld_420.Add(eNoBarra.B8, 1.3f);
            Ld_420.Add(eNoBarra.B10, 1.65f);

            Ld_490.Add(eNoBarra.B2, 0.4f);
            Ld_490.Add(eNoBarra.B3, 0.4f);
            Ld_490.Add(eNoBarra.B4, 0.5f);
            Ld_490.Add(eNoBarra.B5, 0.6f);
            Ld_490.Add(eNoBarra.B6, 0.75f);
            Ld_490.Add(eNoBarra.B7, 1.05f);
            Ld_490.Add(eNoBarra.B8, 1.2f);
            Ld_490.Add(eNoBarra.B10, 1.5f);

            Ld_560.Add(eNoBarra.B2, 0.4f);
            Ld_560.Add(eNoBarra.B3, 0.4f);
            Ld_560.Add(eNoBarra.B4, 0.45f);
            Ld_560.Add(eNoBarra.B5, 0.6f);
            Ld_560.Add(eNoBarra.B6, 0.7f);
            Ld_560.Add(eNoBarra.B7, 1.0f);
            Ld_560.Add(eNoBarra.B8, 1.1f);
            Ld_560.Add(eNoBarra.B10, 1.4f);

        }
        public static void LlenarColoresBarra()
        {
            ColorBarra = new Dictionary<eNoBarra, Color>()
            {
                { eNoBarra.B2, Color.FromArgb(20,150,250)},
                { eNoBarra.B3, Color.FromArgb(20,150,230)},
                { eNoBarra.B4, Color.FromArgb(20,150,210)},
                { eNoBarra.B5, Color.FromArgb(20,150,190)},
                { eNoBarra.B6, Color.FromArgb(20,150,170)},
                { eNoBarra.B7, Color.FromArgb(20,150,150)},
                { eNoBarra.B8, Color.FromArgb(20,150,130)},
                { eNoBarra.B10,Color.FromArgb(20,150,110) }
            };
        }
        private static void LlenarDiccionarioPesoBarras()
        {
            PesoBarras.Add(eNoBarra.B2, 0.25f);
            PesoBarras.Add(eNoBarra.B3, 0.560f);
            PesoBarras.Add(eNoBarra.B4, 1.0f);
            PesoBarras.Add(eNoBarra.B5, 1.56f);
            PesoBarras.Add(eNoBarra.B6, 2.25f);
            PesoBarras.Add(eNoBarra.B7, 3.06f);
            PesoBarras.Add(eNoBarra.B8, 4f);
            PesoBarras.Add(eNoBarra.B10, 6.40f);

        }
        public static float FindTraslapo(eNoBarra NoBarra, float FC, bool isNervio)
        {
            if (isNervio) { return cVariables.TraslapoNervio; }
            if (FC == 210f) { return Ld_210[NoBarra]; }
            if (FC == 280f) { return Ld_280[NoBarra]; }
            if (FC == 350f) { return Ld_350[NoBarra]; }
            if (FC == 420f) { return Ld_420[NoBarra]; }
            if (FC == 490f) { return Ld_490[NoBarra]; }
            if (FC == 560f) { return Ld_560[NoBarra]; }
            return 0;
        }
        public static float LDGancho(eNoBarra NoBarra, eTipoGancho Gancho)
        {
            switch (Gancho)
            {
                case eTipoGancho.G90:
                    return G90[NoBarra];
                case eTipoGancho.G180:
                    return G180[NoBarra].Item1;
                case eTipoGancho.G135:
                    return G135[NoBarra].Item1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Devuelve la longitud de anclaje en [m]
        /// </summary>
        /// <param name="NoBarra">Numero de la Barra.</param>
        /// <param name="fy">Resistencia del acero [kgf/cm²].</param>
        /// <returns></returns>
        /// <param name="fc">Resistencia del concreto. [kgf/cm²]</param>
        public static float Ldh(eNoBarra NoBarra,float fy,float fc)
        {
            return 0.075f * fy / ((float)Math.Sqrt(fc)) * DiametrosBarras[NoBarra] * cConversiones.Dimension_cm_to_m;
        }

    }
}
