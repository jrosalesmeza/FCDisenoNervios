using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebTools;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas
{
    #region Seguridad del Programa
    public static class cSafe
    {
        public static string Ruta_OpcionesSeguridad = Path.Combine(Application.StartupPath, "_opcje");

        private static string[] TextoPlanoWeb;
        private static string[] TextoPlanoLocal;
        public static bool ComprobarAccesoPrograma()
        {
            try
            {
                TextoPlanoLocal = LeerArchivoTextoPlano(Ruta_OpcionesSeguridad);
                string LinkServidorEncriptado = TextoPlanoLocal[0];
                TextoPlanoWeb = FuncitionsWeb.DevolverTextoPlanoDesdeServer(Safe.DesEncriptar(LinkServidorEncriptado)).ToArray();
                return Safe.VerificarAccesoConConexionLocalServer("FCSAS.COM", "servidor.fcsas.com") | Safe.VerificarAccesoConListaMacs(TextoPlanoWeb.ToList());
            }
            catch
            {
                return false;
            }
        }
        public static bool ComprobarVersionPrograma()
        {
            try
            {
                string VersionProgramaLocal = TextoPlanoLocal.Last();
                string VersionProgramaWeb = TextoPlanoWeb.Last();
                if (VersionProgramaLocal == VersionProgramaWeb)
                    return true;
            }
            catch { }
            return false;
        }
        private static string[] LeerArchivoTextoPlano(string Path)
        {
            List<string> ListaTextos = new List<string>();
            try
            {
                StreamReader reader = new StreamReader(Path);
                string colLineReader;
                do
                {
                    colLineReader = reader.ReadLine();
                    ListaTextos.Add(colLineReader);
                } while (!(colLineReader == null));
                reader.Close();
                ListaTextos.RemoveAll(y => y == null);
            }
            catch { }
            return ListaTextos.ToArray();
        }
        public static async Task<Tuple<bool, string>> ComprobarVersionProgramaAsync()
        {
            string VersionProgramaWeb = Program.Version.ToString();
            try
            {
                TextoPlanoLocal = LeerArchivoTextoPlano(Ruta_OpcionesSeguridad);
                string LinkServidorEncriptado = TextoPlanoLocal[0];
                var TextoPlanoWeb = await FuncitionsWeb.DevolverTextoPlanoDesdeServerAsync(Safe.DesEncriptar(LinkServidorEncriptado));
                string VersionProgramaLocal = TextoPlanoLocal.Last();
                VersionProgramaWeb = TextoPlanoWeb.Last();
                if (VersionProgramaLocal == VersionProgramaWeb)
                    return new Tuple<bool, string>(true, VersionProgramaWeb);
            }
            catch { return new Tuple<bool, string>(true, VersionProgramaWeb); }
            return new Tuple<bool, string>(false, VersionProgramaWeb);
        }

    }
    #endregion

}
