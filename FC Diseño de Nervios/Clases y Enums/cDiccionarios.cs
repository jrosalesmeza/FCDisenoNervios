using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public class cDiccionarios
    {
        public static Dictionary<eNoBarra, float> DiametrosBarras = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_210 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_280 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_350 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_420 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_490 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> Ld_560 = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> AceroBarras = new Dictionary<eNoBarra, float>();
        public static Dictionary<eNoBarra, float> G90;
        public static Dictionary<eNoBarra, float> G135;
        public static Dictionary<eNoBarra, float> G180;


        public cDiccionarios()
        {
            LlenarDiccionarioDiametrosBarras();
            LlenarDiccionarioAceroBarras();
            LlenarDiccionariosGanchos();
        }

        private static void LlenarDiccionarioDiametrosBarras()
        {
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
            AceroBarras.Add(eNoBarra.B2, 0.32f );
            AceroBarras.Add(eNoBarra.B3, 0.71f );
            AceroBarras.Add(eNoBarra.B4, 1.29f );
            AceroBarras.Add(eNoBarra.B5, 1.99f );
            AceroBarras.Add(eNoBarra.B6, 2.84f );
            AceroBarras.Add(eNoBarra.B7, 3.87f );
            AceroBarras.Add(eNoBarra.B8, 5.10f );
            AceroBarras.Add(eNoBarra.B10, 8.09f);
        }
        private static void LlenarDiccionariosGanchos()
        {
            G90 = new Dictionary<eNoBarra, float>
            {
                { eNoBarra.B2, 0.09f },
                { eNoBarra.B3, 0.14f },
                { eNoBarra.B4, 0.18f },
                { eNoBarra.B5, 0.23f },
                { eNoBarra.B6, 0.27f },
                { eNoBarra.B7, 0.32f },
                { eNoBarra.B8, 0.36f },
                { eNoBarra.B10, 0.47f }
            };

            G135 = new Dictionary<eNoBarra, float>
            {
                { eNoBarra.B2, 0.063f },
                { eNoBarra.B3, 0.094f },
                { eNoBarra.B4, 0.125f },
                { eNoBarra.B5, 0.157f },
                { eNoBarra.B6, 0.214f },
                { eNoBarra.B7, 0.249f },
                { eNoBarra.B8, 0.286f },
                { eNoBarra.B10, 0.363f }
            };
            G180 = new Dictionary<eNoBarra, float>
            {
                { eNoBarra.B2, 0.116f },
                { eNoBarra.B3, 0.14f },
                { eNoBarra.B4, 0.167f },
                { eNoBarra.B5, 0.192f },
                { eNoBarra.B6, 0.228f },
                { eNoBarra.B7, 0.266f },
                { eNoBarra.B8, 0.305f },
                { eNoBarra.B10, 0.457f }
            };
        }

        public static float FindTraslapo(eNoBarra NoBarra, float FC, bool isNervio)
        {
            if (isNervio) { return cVariables.TraslapoNervio; }
            if (FC == 210f) { return Ld_210[NoBarra]; }
            if (FC == 280f) { return  Ld_280[NoBarra]; }
            if (FC == 350f) { return Ld_350[NoBarra]; }
            if (FC == 420f) { return Ld_420[NoBarra]; }
            if (FC == 490f) { return Ld_490[NoBarra]; }
            if (FC == 560f) { return Ld_560[NoBarra]; }
            return 0;
        }

        public static float Find_Gancho(eNoBarra NoBarra, eTipoGancho TipoGancho)
        {
            if (TipoGancho== eTipoGancho.G90)
            {
                return G90[NoBarra];
            }
            else if (TipoGancho == eTipoGancho.G135)
            {
                return G135[NoBarra];
            }
            else if(TipoGancho == eTipoGancho.G180)
            {
                return G180[NoBarra];
            }
            else
            {
                return 0f;
            }
        }
    }
}
