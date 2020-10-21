using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Programa
{

    [Serializable]
    public class cPropiedades
    {
        public static string Ruta_Propeidades = Path.Combine(cFunctionsProgram.Ruta_CarpetaLocal, "nrv.prop");
        public static cPropiedades CargarPropiedades()
        {
            cPropiedades propiedades = new cPropiedades();
            if (File.Exists(Ruta_Propeidades)) 
            {
                cFunctionsProgram.Deserealizar(Ruta_Propeidades, ref propiedades);
            }
            else
            {
                cFunctionsProgram.Serializar(Ruta_Propeidades, propiedades);
            }
            return propiedades;
        }
        public static void GuardarPropiedades(cPropiedades propiedades)
        {
            cFunctionsProgram.Serializar(Ruta_Propeidades, propiedades);
        }
        public float VersionPrograma { get; set; } = Program.Version;
        public bool FuncionesEnParalelo { get; set; } = false;
        public bool LineasPretrazado { get; set; } = true;
        public bool DeshacerRehacer { get; set; } = true;
        public bool AutoGuardado { get; set; } = true;
        public TiempoAutoGuardado IntervaloMinAutoGuarado { get; set; } = new TiempoAutoGuardado(15);
        public int TimerSleep { get; set; } = 100;

        [Serializable]
        public struct TiempoAutoGuardado
        {
            public TiempoAutoGuardado(int Minutos)
            {
                this.Minutos = Minutos;
                MiliSegundos = Minutos * cConversiones.Tiempo_Min_to_MilSeg;
            }

            public int Minutos { get; set; }
            public int MiliSegundos { get; set; }
        }
    }
  
}
