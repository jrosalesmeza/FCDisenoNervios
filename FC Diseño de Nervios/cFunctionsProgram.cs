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

        public static string Empresa = "efe Prima Ce";



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


        public static List<string> ComprobarErroresArchivoE2K(List<string> ArchivoE2K)
        {
            List<string> Errores = new List<string>();
            int Inicio_ProgramInformation = ArchivoE2K.FindIndex(x => x.Contains("$ PROGRAM INFORMATION")) + 1;
            int Final_ProgramInformation = Find_FinalIndice(ArchivoE2K, Inicio_ProgramInformation);
            List<string> ArchivoProgramInformation = RangoDeDatosArchivoTextoPlano(Inicio_ProgramInformation, Final_ProgramInformation, ArchivoE2K);
            foreach (string Line in ArchivoProgramInformation)
            {
                string[] Line_Separate = Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                if (Line_Separate[3].Contains("9.5"))
                {
                    break;
                }
                else
                {
                    Errores.Add("Versión Inválida");
                }
            }

            int Inicio_Controls = ArchivoE2K.FindIndex(x => x.Contains("$ CONTROLS")) + 1;
            int Final_Controls = Find_FinalIndice(ArchivoE2K, Inicio_Controls);
            List<string> ArchivoControls = RangoDeDatosArchivoTextoPlano(Inicio_Controls, Final_Controls, ArchivoE2K);


            string[] Line_Separate2 = ArchivoControls[0].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
            if (Line_Separate2[1].Contains("TON")==false || Line_Separate2[2].Contains("M") == false)
            {
                Errores.Add("Asignar unidades en Ton-m");
            }
            return Errores;

        }


        public static cDatosEtabs CrearObjetosEtabs(List<string> ArchivoE2K)
        {
            cDatosEtabs DatosEtabs = new cDatosEtabs();
            DatosEtabs.Lista_Points = DeepClone(CreacionPuntosEtabsV2009(ArchivoE2K));
            DatosEtabs.Lista_Materiales = CreacionMaterialesV2009(ArchivoE2K);
            DatosEtabs.Lista_Secciones = CreacionSeccionesV2009(ArchivoE2K,DatosEtabs.Lista_Materiales);
            DatosEtabs.Lista_Pisos = CreacionListaPisosV2009(ArchivoE2K);
            CreacionLinesV2009(ArchivoE2K, DatosEtabs.Lista_Pisos,DatosEtabs.Lista_Points,DatosEtabs.Lista_Secciones);
            return DatosEtabs;
        }

        public static List<cPoint> CreacionPuntosEtabsV2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_POINT_COORDINATES = ArchivoE2K.FindIndex(x => x.Contains("$ POINT COORDINATES")) + 1;
            int IndiceFin_POINT_COORDINATES = Find_FinalIndice(ArchivoE2K, IndiceInicio_POINT_COORDINATES);
            List<string> ArchivoPuntos =RangoDeDatosArchivoTextoPlano(IndiceInicio_POINT_COORDINATES, IndiceFin_POINT_COORDINATES, ArchivoE2K);
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
            List<string> ArchivoMateriales = RangoDeDatosArchivoTextoPlano(IndiceInicio_MATERIAL_PROPERTIES, IndiceFin_MATERIAL_PROPERTIES, ArchivoE2K);
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
            List<string> ArchivoSecciones = RangoDeDatosArchivoTextoPlano(IndiceInicio_FRAME_SECTIONS, IndiceFin_FRAME_SECTIONS, ArchivoE2K);
            List<cSeccion> Lista_Secciones = new List<cSeccion>();
            //Indices $ CONCRETE SECTIONS
            int IndiceInicio_CONCRETE_SECTIONS = ArchivoE2K.FindIndex(x => x.Contains("$ CONCRETE SECTIONS")) + 1;
            int IndiceFin_CONCRETE_SECTIONS = Find_FinalIndice(ArchivoE2K, IndiceInicio_CONCRETE_SECTIONS);
            List<string> ArchivoConcreteSeccions = RangoDeDatosArchivoTextoPlano(IndiceInicio_CONCRETE_SECTIONS, IndiceFin_CONCRETE_SECTIONS, ArchivoE2K);
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

        public static void CreacionLinesV2009(List<string> ArchivoE2K,List<cPiso> Lista_Pisos,List<cPoint> Lista_Puntos,List<cSeccion> Lista_Secciones)
        {

            List<cLine> Lista_Line = new List<cLine>();
            int IndiceInicio_LINE_CONNECTIVITIES = ArchivoE2K.FindIndex(x => x.Contains("$ LINE CONNECTIVITIES")) + 1;
            int IndiceFin_LINE_CONNECTIVITIES = Find_FinalIndice(ArchivoE2K, IndiceInicio_LINE_CONNECTIVITIES);
            List<string> ArchivoLineConnectivities = RangoDeDatosArchivoTextoPlano(IndiceInicio_LINE_CONNECTIVITIES, IndiceFin_LINE_CONNECTIVITIES, ArchivoE2K);
           
            
            //LINE ASSIGNS
            int IndiceInicio_LINE_ASSIGNS = ArchivoE2K.FindIndex(x => x.Contains("$ LINE ASSIGNS")) + 1;
            int IndiceFin_LINE_ASSIGNS = Find_FinalIndice(ArchivoE2K, IndiceInicio_LINE_ASSIGNS);
            List<string> ArchivoLineAssigns = RangoDeDatosArchivoTextoPlano(IndiceInicio_LINE_ASSIGNS, IndiceFin_LINE_ASSIGNS, ArchivoE2K);
            List<string[]> LineAssignsSeparate = new List<string[]>();
            foreach (string Line in ArchivoLineAssigns) { LineAssignsSeparate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }



            for (int i = 0; i < ArchivoLineConnectivities.Count; i++)
            {
                string[] LineConectivitieSeparate = ArchivoLineConnectivities[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                string NombreLine = LineConectivitieSeparate[1];
                cPoint Point1 = Lista_Puntos.Find(x=>x.Nombre== LineConectivitieSeparate[3]);
                cPoint Point2 = Lista_Puntos.Find(x => x.Nombre == LineConectivitieSeparate[4]);
                cLine Line = new cLine(NombreLine, ConvertirStringtoeType(LineConectivitieSeparate[2]));
                Line.ConfigEtabs = new cConfigEtabs(Point1, Point2);
                Lista_Line.Add(Line);
            }

            foreach(string[] Elemento in LineAssignsSeparate)
            {
                if (Elemento.Length > 12)
                {
                    string NombrePiso = Elemento[2];
                    string NombreElmento = Elemento[1];
                    string NombreSeccion = Elemento[4];
                    cPiso PisoEncontrado= Lista_Pisos.Find(x => x.Nombre == NombrePiso);
                    cLine ElementoEncontrado = DeepClone(Lista_Line.Find(x =>x.Nombre== NombreElmento));
                    ElementoEncontrado.Seccion = Lista_Secciones.Find(x => x.Nombre == NombreSeccion);
                    ElementoEncontrado.Story = NombrePiso;
                    PisoEncontrado.Lista_Lines.Add(ElementoEncontrado);

                }

            }
   
    







        }

        public static List<cPiso> CreacionListaPisosV2009(List<string> ArchivoE2K)
        {


            List<cPiso> Pisos = new List<cPiso>();

            int IndiceInicio_STORIES_IN_SEQUENCE_FROM_TOP = ArchivoE2K.FindIndex(x => x.Contains("$ STORIES - IN SEQUENCE FROM TOP")) + 1;
            int IndiceFin_STORIES_IN_SEQUENCE_FROM_TOP = Find_FinalIndice(ArchivoE2K, IndiceInicio_STORIES_IN_SEQUENCE_FROM_TOP) - 1;
            List<string> ArchivoPisos = RangoDeDatosArchivoTextoPlano(IndiceInicio_STORIES_IN_SEQUENCE_FROM_TOP, IndiceFin_STORIES_IN_SEQUENCE_FROM_TOP, ArchivoE2K);


            for (int i = 0; i < ArchivoPisos.Count; i++)
            {
                string[] Pisos_Separados = ArchivoPisos[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                cPiso piso = new cPiso(Pisos_Separados[1], Convert.ToSingle(Pisos_Separados[3]));
                Pisos.Add(piso);
            }

            return Pisos;


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
        public static List<string> RangoDeDatosArchivoTextoPlano(int IndiceInical, int IndiceFinal,List<string> Archivo)
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




        //Funciones de Selección
        public static void SeleccionInteligente(cLine LineMadre, List<cLine> Lista_Lines)
        {
            foreach (cLine LineaHija in Lista_Lines)
            {

                if (LineMadre.Nombre != LineaHija.Nombre && LineaHija.Select != true && LineaHija.ConfigEtabs.Direccion == LineMadre.ConfigEtabs.Direccion)
                {
                    if (LineMadre.ConfigEtabs.Point1P.X == LineaHija.ConfigEtabs.Point1P.X && LineMadre.ConfigEtabs.Point1P.Y == LineaHija.ConfigEtabs.Point1P.Y)
                    {
                        LineaHija.Select = true;
                        SeleccionInteligente(LineaHija, Lista_Lines);
                    }
                    if (LineMadre.ConfigEtabs.Point1P.X == LineaHija.ConfigEtabs.Point2P.X && LineMadre.ConfigEtabs.Point1P.Y == LineaHija.ConfigEtabs.Point2P.Y)
                    {
                        LineaHija.Select = true;
                        SeleccionInteligente(LineaHija, Lista_Lines);
                    }
                    if (LineMadre.ConfigEtabs.Point2P.X == LineaHija.ConfigEtabs.Point1P.X && LineMadre.ConfigEtabs.Point2P.Y == LineaHija.ConfigEtabs.Point1P.Y)
                    {
                        LineaHija.Select = true;
                        SeleccionInteligente(LineaHija, Lista_Lines);
                    }
                    if (LineMadre.ConfigEtabs.Point2P.X == LineaHija.ConfigEtabs.Point2P.X && LineMadre.ConfigEtabs.Point2P.Y == LineaHija.ConfigEtabs.Point2P.Y)
                    {
                        LineaHija.Select = true;
                        SeleccionInteligente(LineaHija, Lista_Lines);
                    }
                }


            }





        }















    }
}
