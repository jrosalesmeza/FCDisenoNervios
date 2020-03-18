using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FC_Diseño_de_Nervios
{

    public delegate void DelegateNotificadorProgram(string Alert);
    public static class cFunctionsProgram
    {
        private static string[] Separadores = { "  ", " ", @"""" };
        public static List<eType> eTypes = new List<eType>(){eType.Beam,eType.Column,eType.Floor,eType.Wall,eType.None };
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
            cDatosEtabs DatosEtabs = new cDatosEtabs();
            DatosEtabs.Lista_Points = DeepClone(CreacionPuntosEtabsV2009(ArchivoE2K));
            DatosEtabs.Lista_Materiales = CreacionMaterialesV2009(ArchivoE2K);
            DatosEtabs.Lista_Secciones = CreacionSeccionesV2009(ArchivoE2K,DatosEtabs.Lista_Materiales);
            
        }

        public static List<cPoint> CreacionPuntosEtabsV2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_POINT_COORDINATES = ArchivoE2K.FindIndex(x => x.Contains("$ POINT COORDINATES")) + 1;
            int IndiceFin_POINT_COORDINATES = Find_FinalIndice(ArchivoE2K, IndiceInicio_POINT_COORDINATES);
            List<string> ArchivoPuntos =RangoDeDatos(IndiceInicio_POINT_COORDINATES, IndiceFin_POINT_COORDINATES, ArchivoE2K);
            List<cPoint> ListaPuntos = new List<cPoint>();

            for(int i=0; i < ArchivoPuntos.Count; i++)
            {
                string[] Point_Separado = ArchivoPuntos[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                cPoint point = new cPoint(Point_Separado[1],(float)Math.Round(Convert.ToSingle(Point_Separado[2]),2), (float)Math.Round(Convert.ToSingle(Point_Separado[3]), 2));
                ListaPuntos.Add(point);
            }
            return ListaPuntos;
        }


        public static List<cMaterial> CreacionMaterialesV2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_MATERIAL_PROPERTIES = ArchivoE2K.FindIndex(x => x.Contains("$ MATERIAL PROPERTIES")) + 1;
            int IndiceFin_MATERIAL_PROPERTIES = Find_FinalIndice(ArchivoE2K, IndiceInicio_MATERIAL_PROPERTIES);
            List<string> ArchivoMateriales = RangoDeDatos(IndiceInicio_MATERIAL_PROPERTIES, IndiceFin_MATERIAL_PROPERTIES, ArchivoE2K);
            List<cMaterial> Lista_Materiales = new List<cMaterial>();
            for (int i = 0; i < ArchivoMateriales.Count; i++)
            {
                string[] Material_Separado = ArchivoMateriales[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
               
                if (Material_Separado.Length == 10)
                {
                    cMaterial material = new cMaterial(Material_Separado[1], Convert.ToSingle(Material_Separado[7])*cConversiones.Esfuerzo_Ton_m_to_kfg_cm, Convert.ToSingle(Material_Separado[5])* cConversiones.Esfuerzo_Ton_m_to_kfg_cm);
                    Lista_Materiales.Add(material);
                }

            }
            return Lista_Materiales;
        }

        public static List<cSeccion> CreacionSeccionesV2009(List<string> ArchivoE2K, List<cMaterial> ListaMateriales)
        {

            int IndiceInicio_FRAME_SECTIONS = ArchivoE2K.FindIndex(x => x.Contains("$ FRAME SECTIONS")) + 1;
            int IndiceFin_FRAME_SECTIONS = Find_FinalIndice(ArchivoE2K, IndiceInicio_FRAME_SECTIONS);
            List<string> ArchivoSecciones = RangoDeDatos(IndiceInicio_FRAME_SECTIONS, IndiceFin_FRAME_SECTIONS, ArchivoE2K);
            List<cSeccion> Lista_Secciones = new List<cSeccion>();
            //Indices $ CONCRETE SECTIONS
            int IndiceInicio_CONCRETE_SECTIONS = ArchivoE2K.FindIndex(x => x.Contains("$ CONCRETE SECTIONS")) + 1;
            int IndiceFin_CONCRETE_SECTIONS = Find_FinalIndice(ArchivoE2K, IndiceInicio_CONCRETE_SECTIONS);
            List<string> ArchivoConcreteSeccions = RangoDeDatos(IndiceInicio_CONCRETE_SECTIONS, IndiceFin_CONCRETE_SECTIONS, ArchivoE2K);
            List<string[]> ConcreteSeccionsSeparate = new List<string[]>();
            foreach(string Line in ArchivoConcreteSeccions) { ConcreteSeccionsSeparate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }

            for (int i = 0; i < ArchivoSecciones.Count; i++)
            {
                string[] Seccion_Separada = ArchivoSecciones[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);

                int IndiceNe= ConcreteSeccionsSeparate.FindIndex(x => x[1] == Seccion_Separada[1]);

                if (Seccion_Separada.Contains("Rectangular"))
                {
                    cMaterial material = ListaMateriales.Find(x => x.Nombre == Seccion_Separada[3]);
                    cSeccion Seccion = new cSeccion(Seccion_Separada[1],(float)Math.Round(Convert.ToSingle(Seccion_Separada[9]) * cConversiones.Dimension_m_to_cm,2), (float)Math.Round(Convert.ToSingle(Seccion_Separada[7]) * cConversiones.Dimension_m_to_cm,2));
                    Seccion.Material = material;
                
                    if (IndiceNe != -1)
                    {
                        Seccion.Type =  ConvertirStringtoeType(ConcreteSeccionsSeparate[IndiceNe][3]);
                        if (Seccion.Type == eType.Beam)
                        {
                            Seccion.R_Top =Convert.ToSingle(ConcreteSeccionsSeparate[IndiceNe][5]) * cConversiones.Dimension_m_to_cm;
                            Seccion.R_Bottom = Convert.ToSingle(ConcreteSeccionsSeparate[IndiceNe][7])*cConversiones.Dimension_m_to_cm;
                        }
                    }
                    Lista_Secciones.Add(Seccion);

                }

            }

            return Lista_Secciones;
        }

        public static List<cLine> CreacionLinesV2009(List<string> ArchivoE2K)
        {

            List<cLine> Lista_Line = new List<cLine>();
            int IndiceInicio_LINE_CONNECTIVITIES = ArchivoE2K.FindIndex(x => x.Contains("$ LINE CONNECTIVITIES")) + 1;
            int IndiceFin_LINE_CONNECTIVITIES = Find_FinalIndice(ArchivoE2K, IndiceInicio_LINE_CONNECTIVITIES);
            List<string> ArchivoLineConnectivities = RangoDeDatos(IndiceInicio_LINE_CONNECTIVITIES, IndiceFin_LINE_CONNECTIVITIES, ArchivoE2K);
            //


            for(int i=0; i< ArchivoLineConnectivities.Count; i++)
            {
                string[] LineConectivitieSeparate = ArchivoLineConnectivities[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);






            }



            return Lista_Line;







        }


        public static List<cPiso> CreacionListaPisosV2009(List<string> ArchivoE2K)
        {
            return new List<cPiso>();

        }











        public static eType ConvertirStringtoeType(string StringType)
        {
            return eTypes.Find(x => x.ToString().ToUpper() == StringType.ToUpper());
        }
        public static string ConvertireTypeToString(eType type)
        {
            return type.ToString();
        }
        public static int Find_FinalIndice(List<string> ArchivoE2K,int IndiceInicio)
        {
            int IndiceFin=-1;
            for (int i = IndiceInicio; i <= ArchivoE2K.Count; i++) { if (ArchivoE2K[i] == "") { IndiceFin = i; break; } }
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

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }



    }
}
