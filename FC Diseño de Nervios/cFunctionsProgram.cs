using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FC_Diseño_de_Nervios
{
    public delegate void DelegateNotificadorProgram(string Alert);
    public static class cFunctionsProgram
    {
        public static event DelegateNotificadorProgram Notificador;

        public static cProyecto NuevoProyecto()
        {
            return new cProyecto();

        }



        /// <summary>
        /// Tuple(string, List(String))
        /// </summary>
        /// <param name="FilterTitle">Ejemplo: "Archivo | *.e2k;.$et"</param>
        public static Tuple<string,List<string>> CagarArchivoTextoPlano(string FilterTitle,string TitleText)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog() { Filter = FilterTitle,Title=TitleText};
            OpenFileDialog.ShowDialog();
            string Ruta = OpenFileDialog.FileName;
            if (Ruta != "")
            {
                List<string> Archivo = new List<string>();
                Notificador("Cargando " + TitleText);
                StreamReader colReader = new StreamReader(Ruta);
                string colLineReader;
                do
                {
                   colLineReader = colReader.ReadLine();
                   Archivo.Add(colLineReader);
                } while (!(colLineReader == null));
                colReader.Close();
                //Control de Errores
                Archivo.RemoveAll(x => x == null);
                // Fin
                Notificador("Listo");
                return new Tuple<string, List<string>>(Ruta, Archivo);
            }

            Notificador("Listo");
            return null;
        }


        public static void CrearObjetosEtabs(List<string> ArchivoE2K)
        {
            CreacionPuntosEtabsV2009(ArchivoE2K);
        }

        public static void CreacionPuntosEtabsV2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_POINT_COORDINATES = ArchivoE2K.FindIndex(x => x.Contains("$ POINT COORDINATES")) + 1;
            int IndiceFin_POINT_COORDINATES = Find_FinalIndice(ArchivoE2K, IndiceInicio_POINT_COORDINATES);

            List<string> ArchivoPuntos =RangoDeDatos(IndiceInicio_POINT_COORDINATES, IndiceFin_POINT_COORDINATES, ArchivoE2K);



        }







        public static int Find_FinalIndice(List<string> ArchivoE2K,int IndiceInicio)
        {
            int IndiceFin=-1;
            for (int i = IndiceInicio; i <= ArchivoE2K.Count; i++) { if (ArchivoE2K[i] == "") { IndiceFin = i-1; break; } }
            return IndiceFin;
        }
        public static List<string> RangoDeDatos(int IndiceInical, int IndiceFinal,List<string> Archivo)
        {
            List<string> ArchivoStrings = new List<string>();
            for(int i = IndiceInical; i< IndiceFinal; i++)
            {
                ArchivoStrings.Add(Archivo[i]);

            }
            return ArchivoStrings;
        }













    }
}
