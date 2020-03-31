using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace FC_Diseño_de_Nervios
{

    public delegate void DelegateNotificadorProgram(string Alert);
    public delegate void DelegateVentanasEmergentes(string Alert, MessageBoxIcon Icono);
    public static class cFunctionsProgram
    {
        private static string[] Separadores = { "  ", " ", @"""" };
        public static List<eType> eTypes = new List<eType>() { eType.Beam, eType.Column, eType.Floor, eType.Wall, eType.None };
        public static event DelegateNotificadorProgram Notificador;
        public static event DelegateVentanasEmergentes EventoVentanaEmergente;


        public const string Empresa = "efe Prima Ce";
        public const string Ext = ".nrv";
        public const string NombrePrograma = "FC-JOISTS";

        /// <summary>
        /// Tuple(string, List(String))
        /// </summary>
        /// <param name="FilterTitle">Ejemplo: "Archivo | *.e2k;.$et"</param>
        public static Tuple<string, List<string>> CagarArchivoTextoPlano(string FilterTitle, string TitleText)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog() { Filter = FilterTitle, Title = TitleText };
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
            if (Line_Separate2[1].Contains("TON") == false || Line_Separate2[2].Contains("M") == false)
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
            DatosEtabs.Lista_Secciones = CreacionSeccionesV2009(ArchivoE2K, DatosEtabs.Lista_Materiales);
            DatosEtabs.Lista_Pisos = CreacionListaPisosV2009(ArchivoE2K);
            CreacionLinesV2009(ArchivoE2K, DatosEtabs.Lista_Pisos, DatosEtabs.Lista_Points, DatosEtabs.Lista_Secciones);
            return DatosEtabs;
        }

        public static List<cPoint> CreacionPuntosEtabsV2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_POINT_COORDINATES = ArchivoE2K.FindIndex(x => x.Contains("$ POINT COORDINATES")) + 1;
            int IndiceFin_POINT_COORDINATES = Find_FinalIndice(ArchivoE2K, IndiceInicio_POINT_COORDINATES);
            List<string> ArchivoPuntos = RangoDeDatosArchivoTextoPlano(IndiceInicio_POINT_COORDINATES, IndiceFin_POINT_COORDINATES, ArchivoE2K);
            List<cPoint> ListaPuntos = new List<cPoint>();

            for (int i = 0; i < ArchivoPuntos.Count; i++)
            {
                string[] Point_Separado = ArchivoPuntos[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries); ;
                cPoint point = new cPoint(Point_Separado[1], (float)Math.Round(Convert.ToSingle(Point_Separado[2]), 2), (float)Math.Round(Convert.ToSingle(Point_Separado[3]), 2));
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
                    cMaterial material = new cMaterial(Material_Separado[1], Convert.ToSingle(Material_Separado[7]) * cConversiones.Esfuerzo_Ton_m_to_kfg_cm, Convert.ToSingle(Material_Separado[5]) * cConversiones.Esfuerzo_Ton_m_to_kfg_cm);
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
            foreach (string Line in ArchivoConcreteSeccions) { ConcreteSeccionsSeparate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }

            for (int i = 0; i < ArchivoSecciones.Count; i++)
            {
                string[] Seccion_Separada = ArchivoSecciones[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);

                int IndiceNe = ConcreteSeccionsSeparate.FindIndex(x => x[1] == Seccion_Separada[1]);

                if (Seccion_Separada.Contains("Rectangular"))
                {
                    cMaterial material = ListaMateriales.Find(x => x.Nombre == Seccion_Separada[3]);
                    cSeccion Seccion = new cSeccion(Seccion_Separada[1], (float)Math.Round(Convert.ToSingle(Seccion_Separada[9]) * cConversiones.Dimension_m_to_cm, 2), (float)Math.Round(Convert.ToSingle(Seccion_Separada[7]) * cConversiones.Dimension_m_to_cm, 2));
                    Seccion.Material = material;

                    if (IndiceNe != -1)
                    {
                        Seccion.Type = ConvertirStringtoeType(ConcreteSeccionsSeparate[IndiceNe][3]);
                        if (Seccion.Type == eType.Beam)
                        {
                            Seccion.R_Top = Convert.ToSingle(ConcreteSeccionsSeparate[IndiceNe][5]) * cConversiones.Dimension_m_to_cm;
                            Seccion.R_Bottom = Convert.ToSingle(ConcreteSeccionsSeparate[IndiceNe][7]) * cConversiones.Dimension_m_to_cm;
                        }
                    }
                    Lista_Secciones.Add(Seccion);

                }

            }

            return Lista_Secciones;
        }

        public static void CreacionLinesV2009(List<string> ArchivoE2K, List<cPiso> Lista_Pisos, List<cPoint> Lista_Puntos, List<cSeccion> Lista_Secciones)
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
                cPoint Point1 = Lista_Puntos.Find(x => x.Nombre == LineConectivitieSeparate[3]);
                cPoint Point2 = Lista_Puntos.Find(x => x.Nombre == LineConectivitieSeparate[4]);
                cLine Line = new cLine(NombreLine, ConvertirStringtoeType(LineConectivitieSeparate[2]));
                Line.ConfigLinea = new cConfigLinea(Point1, Point2);
                Lista_Line.Add(Line);
            }

            foreach (string[] Elemento in LineAssignsSeparate)
            {
                if (Elemento.Length > 12)
                {
                    string NombrePiso;
                    int IndiceAMayor = 0;

                    if (Elemento[3] == "SECTION")
                    {
                        NombrePiso = Elemento[2];
                    }
                    else
                    {
                        NombrePiso = Elemento[2] + " " + Elemento[3];
                        IndiceAMayor = 1;
                    }
                    string NombreElemento = Elemento[1];
                    string NombreSeccion = Elemento[4 + IndiceAMayor];
                    cPiso PisoEncontrado = Lista_Pisos.Find(x => x.Nombre == NombrePiso);
                    float OffSetI = 0; float OffSetJ = 0;
                    if (Elemento.Contains("LENGTHOFFI"))
                    {
                        OffSetI = Convert.ToSingle(Elemento[10 + IndiceAMayor]);
                        OffSetJ = Convert.ToSingle(Elemento[12 + IndiceAMayor]);
                    }

                    cLine ElementoEncontrado = DeepClone(Lista_Line.Find(x => x.Nombre == NombreElemento));
                    ElementoEncontrado.Seccion = Lista_Secciones.Find(x => x.Nombre == NombreSeccion);
                    ElementoEncontrado.Story = NombrePiso;
                    ElementoEncontrado.ConfigLinea.OffSetI = OffSetI;
                    ElementoEncontrado.ConfigLinea.OffSetJ = OffSetJ;
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
                string NamePiso; float HPiso;
                if (Pisos_Separados.Length == 4 | Pisos_Separados.Length == 6)
                {
                    NamePiso = Pisos_Separados[1];
                    HPiso = Convert.ToSingle(Pisos_Separados[3]);
                }
                else
                {
                    NamePiso = Pisos_Separados[1] + " " + Pisos_Separados[2];
                    HPiso = Convert.ToSingle(Pisos_Separados[4]);
                }
                cPiso piso = new cPiso(NamePiso, HPiso);
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
        public static int Find_FinalIndice(List<string> ArchivoE2K, int IndiceInicio)
        {
            int IndiceFin = -1;
            for (int i = IndiceInicio; i <= ArchivoE2K.Count; i++) { if (ArchivoE2K[i] == "") { IndiceFin = i; break; } }
            return IndiceFin;
        }
        public static List<string> RangoDeDatosArchivoTextoPlano(int IndiceInical, int IndiceFinal, List<string> Archivo)
        {
            List<string> ArchivoStrings = new List<string>();
            for (int i = IndiceInical; i < IndiceFinal; i++)
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
        public static void SeleccionInteligente(cLine LineMadre, List<cLine> Lista_Lines, int IndiceSeleccion, bool CondicionSelect)
        {
            foreach (cLine LineaHija in Lista_Lines)
            {

                if (LineMadre.Nombre != LineaHija.Nombre && LineaHija.Select != CondicionSelect && LineaHija.ConfigLinea.Direccion == LineMadre.ConfigLinea.Direccion && LineaHija.isSelect == true && LineMadre.isSelect == true)
                {
                    if (LineMadre.ConfigLinea.Point1P.X == LineaHija.ConfigLinea.Point1P.X && LineMadre.ConfigLinea.Point1P.Y == LineaHija.ConfigLinea.Point1P.Y)
                    {
                        LineaHija.Select = CondicionSelect; LineaHija.IndexSelect = IndiceSeleccion;
                        SeleccionInteligente(LineaHija, Lista_Lines, IndiceSeleccion, CondicionSelect);
                    }
                    if (LineMadre.ConfigLinea.Point1P.X == LineaHija.ConfigLinea.Point2P.X && LineMadre.ConfigLinea.Point1P.Y == LineaHija.ConfigLinea.Point2P.Y)
                    {
                        LineaHija.Select = CondicionSelect; LineaHija.IndexSelect = IndiceSeleccion;
                        SeleccionInteligente(LineaHija, Lista_Lines, IndiceSeleccion, CondicionSelect);
                    }
                    if (LineMadre.ConfigLinea.Point2P.X == LineaHija.ConfigLinea.Point1P.X && LineMadre.ConfigLinea.Point2P.Y == LineaHija.ConfigLinea.Point1P.Y)
                    {
                        LineaHija.Select = CondicionSelect; LineaHija.IndexSelect = IndiceSeleccion;
                        SeleccionInteligente(LineaHija, Lista_Lines, IndiceSeleccion, CondicionSelect);
                    }
                    if (LineMadre.ConfigLinea.Point2P.X == LineaHija.ConfigLinea.Point2P.X && LineMadre.ConfigLinea.Point2P.Y == LineaHija.ConfigLinea.Point2P.Y)
                    {
                        LineaHija.Select = CondicionSelect; LineaHija.IndexSelect = IndiceSeleccion;
                        SeleccionInteligente(LineaHija, Lista_Lines, IndiceSeleccion, CondicionSelect);
                    }
                }


            }

        }

        public static void SeleccionInteligente_2(List<cLine> Lineas)
        {
            int Contador1 = 0;
            foreach (cLine Linea in Lineas)
            {
                if (Linea.IndiceConjuntoSeleccion == 0)
                {
                    Contador1 += 1;
                    Linea.IndiceConjuntoSeleccion = Contador1;
                }
                AsignarContadorALineaSucesiva(Linea, Lineas, Contador1);
            }
            void AsignarContadorALineaSucesiva(cLine LineMadre, List<cLine> Lista_Lines, int Contador)
            {
                foreach (cLine LineaHija in Lista_Lines)
                {
                    if (LineMadre.Nombre != LineaHija.Nombre && LineaHija.IndiceConjuntoSeleccion == 0 && LineaHija.ConfigLinea.Direccion == LineMadre.ConfigLinea.Direccion)
                    {
                        if (LineMadre.ConfigLinea.Point1P.X == LineaHija.ConfigLinea.Point1P.X && LineMadre.ConfigLinea.Point1P.Y == LineaHija.ConfigLinea.Point1P.Y)
                        {
                            LineaHija.IndiceConjuntoSeleccion = LineMadre.IndiceConjuntoSeleccion;
                            AsignarContadorALineaSucesiva(LineaHija, Lista_Lines, Contador);
                        }
                        else if (LineMadre.ConfigLinea.Point1P.X == LineaHija.ConfigLinea.Point2P.X && LineMadre.ConfigLinea.Point1P.Y == LineaHija.ConfigLinea.Point2P.Y)
                        {
                            LineaHija.IndiceConjuntoSeleccion = LineMadre.IndiceConjuntoSeleccion;
                            AsignarContadorALineaSucesiva(LineaHija, Lista_Lines, Contador);
                        }
                        else if (LineMadre.ConfigLinea.Point2P.X == LineaHija.ConfigLinea.Point1P.X && LineMadre.ConfigLinea.Point2P.Y == LineaHija.ConfigLinea.Point1P.Y)
                        {
                            LineaHija.IndiceConjuntoSeleccion = LineMadre.IndiceConjuntoSeleccion;
                            AsignarContadorALineaSucesiva(LineaHija, Lista_Lines, Contador);
                        }
                        else if (LineMadre.ConfigLinea.Point2P.X == LineaHija.ConfigLinea.Point2P.X && LineMadre.ConfigLinea.Point2P.Y == LineaHija.ConfigLinea.Point2P.Y)
                        {
                            LineaHija.IndiceConjuntoSeleccion = LineMadre.IndiceConjuntoSeleccion;
                            AsignarContadorALineaSucesiva(LineaHija, Lista_Lines, Contador);
                        }

                    }


                }

            }
        }

        public static List<List<cLine>> LineasParaCrearNervios(List<cLine> Lista_LineasSeleccionadas)
        {
            List<List<cLine>> Lista_Lista_Lineas = new List<List<cLine>>();

            SeleccionInteligente_2(Lista_LineasSeleccionadas);

            var GrupoBeams = from Beam in Lista_LineasSeleccionadas
                             group Beam by Beam.IndiceConjuntoSeleccion into ListaBeams
                             select new { IndiceConjuntoSeleccion = ListaBeams.Key, ListaBeams = ListaBeams.ToList() };

            foreach (var LineGrupoBeams in GrupoBeams)
            {
                Lista_Lista_Lineas.Add((List<cLine>)DeepClone(LineGrupoBeams.ListaBeams));
            }

            return Lista_Lista_Lineas;
        }
        private static cLine FindApoyo(cLine LineaMadre, List<cLine> Lineas, int InicioIFinJ)
        {
            foreach (cLine LineaHija in Lineas)
            {

                if (LineaMadre.Nombre != LineaHija.Nombre && LineaHija.ConfigLinea.Direccion != LineaMadre.ConfigLinea.Direccion)
                {
                    if (LineaMadre.ConfigLinea.Point1P.X == LineaHija.ConfigLinea.Point1P.X && LineaMadre.ConfigLinea.Point1P.Y == LineaHija.ConfigLinea.Point1P.Y || LineaMadre.ConfigLinea.Point1P.X == LineaHija.ConfigLinea.Point2P.X && LineaMadre.ConfigLinea.Point1P.Y == LineaHija.ConfigLinea.Point2P.Y
                    || LineaMadre.ConfigLinea.Point2P.X == LineaHija.ConfigLinea.Point1P.X && LineaMadre.ConfigLinea.Point2P.Y == LineaHija.ConfigLinea.Point1P.Y || LineaMadre.ConfigLinea.Point2P.X == LineaHija.ConfigLinea.Point2P.X && LineaMadre.ConfigLinea.Point2P.Y == LineaHija.ConfigLinea.Point2P.Y)
                    {

                        if (LineaMadre.ConfigLinea.Direccion == eDireccion.Vertical)
                        {

                            if (InicioIFinJ == 0)
                            {
                                if (LineaMadre.Planta_Real.Min(x => x.Y) == LineaHija.ConfigLinea.Point1P.Y)
                                {
                                    return LineaHija;
                                }
                                else if (LineaMadre.Planta_Real.Min(x => x.Y) == LineaHija.ConfigLinea.Point2P.Y)
                                {
                                    return LineaHija;
                                }
                            }
                            else
                            {
                                if (LineaMadre.Planta_Real.Max(x => x.Y) == LineaHija.ConfigLinea.Point1P.Y)
                                {
                                    return LineaHija;
                                }
                                else if (LineaMadre.Planta_Real.Max(x => x.Y) == LineaHija.ConfigLinea.Point2P.Y)
                                {
                                    return LineaHija;
                                }
                            }

                        }
                        else
                        {
                            if (InicioIFinJ == 0)
                            {
                                if (LineaMadre.Planta_Real.Min(x => x.X) == LineaHija.ConfigLinea.Point1P.X)
                                {
                                    return LineaHija;
                                }
                                else if (LineaMadre.Planta_Real.Min(x => x.X) == LineaHija.ConfigLinea.Point2P.X)
                                {
                                    return LineaHija;
                                }
                            }
                            else
                            {
                                if (LineaMadre.Planta_Real.Max(x => x.X) == LineaHija.ConfigLinea.Point1P.X)
                                {
                                    return LineaHija;
                                }
                                else if (LineaMadre.Planta_Real.Max(x => x.X) == LineaHija.ConfigLinea.Point2P.X)
                                {
                                    return LineaHija;
                                }
                            }
                        }


                    }

                }
            }
            return null;
        }
        private static void AsignarApoyosAListaConOffSet(cLine Objeto, cLine LineApoyo, List<cLine> ListaObjetosOrganizada, int Indice, ref List<cLine> ListaObjetosOrganizadaDefinitiva)
        {
            if (Indice + 1 > ListaObjetosOrganizada.Count)
            {
                if (Objeto.ConfigLinea.OffSetI != 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice, DeepClone(LineApoyo));
                }
                if (Objeto.ConfigLinea.OffSetJ != 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Add(DeepClone(LineApoyo));
                }

            }
            else
            {
                if (Objeto.ConfigLinea.OffSetI != 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice, DeepClone(LineApoyo));
                }
                if (Objeto.ConfigLinea.OffSetJ != 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice + 1, DeepClone(LineApoyo));
                }

            }
        }
        public static cNervio CrearNervio(string Prefijo, int ID, List<cLine> LineasQComponenAlNervio, List<cLine> TodasLasLineas, float WidthWindow, float HeightWindow)
        {
            eDireccion DireccionNervio = LineasQComponenAlNervio.First().ConfigLinea.Direccion;
            List<cLine> ListaObjetosOrganizada;

            if (DireccionNervio == eDireccion.Horizontal)
            {
                ListaObjetosOrganizada = LineasQComponenAlNervio.OrderBy(x => x.Planta_Real.Min(y => y.X)).ToList();
            }
            else
            {
                ListaObjetosOrganizada = LineasQComponenAlNervio.OrderBy(x => x.Planta_Real.Min(y => y.Y)).ToList();
            }

            for (int Indice = 0; Indice < ListaObjetosOrganizada.Count; Indice++)
            {
                cLine Objeto = ListaObjetosOrganizada[Indice];

                if (Objeto.ConfigLinea.OffSetI != 0)
                {
                    foreach (cLine Line in TodasLasLineas)
                    {
                        cLine LineApoyo = FindApoyo(Objeto, TodasLasLineas, 0);
                        if (LineApoyo != null && ListaObjetosOrganizada.Exists(x => x.Nombre == LineApoyo.Nombre) == false && LineApoyo.Type != eType.Column && LineApoyo.ConfigLinea.Direccion != DireccionNervio)
                        {
                            AsignarApoyosAListaConOffSet(Objeto, DeepClone(LineApoyo), ListaObjetosOrganizada, Indice, ref ListaObjetosOrganizada);
                            break;
                        }
                    }

                }
                if (Objeto.ConfigLinea.OffSetJ != 0)
                {
                    foreach (cLine Line in TodasLasLineas)
                    {
                        cLine LineApoyo = FindApoyo(Objeto, TodasLasLineas, 1);
                        if (LineApoyo != null && ListaObjetosOrganizada.Exists(x => x.Nombre == LineApoyo.Nombre) == false && LineApoyo.Type != eType.Column && LineApoyo.ConfigLinea.Direccion != DireccionNervio)
                        {
                            AsignarApoyosAListaConOffSet(Objeto, LineApoyo, ListaObjetosOrganizada, Indice, ref ListaObjetosOrganizada);
                            break;
                        }
                    }

                }

            }

            List<cObjeto> ListaObjetosFinal = new List<cObjeto>();

            foreach (cLine LineObjetosACrear in ListaObjetosOrganizada)
            {
                eSoporte Soporte = eSoporte.Vano;
                if (LineObjetosACrear.ConfigLinea.Direccion != DireccionNervio)
                {
                    Soporte = eSoporte.Apoyo;
                }
                cObjeto Objeto = new cObjeto(LineObjetosACrear, Soporte);
                ListaObjetosFinal.Add(Objeto);
            }

            List<PointF> PuntosSinEscalar = new List<PointF>(); TodasLasLineas.ForEach(y => { if (y.Type == eType.Beam) { PuntosSinEscalar.AddRange(y.Planta_Real); } });

            ListaObjetosFinal.ForEach(x => { TodasLasLineas.FindAll(y => y.Nombre == x.Line.Nombre && x.Soporte == eSoporte.Vano).ForEach(z => { z.isSelect = false; z.Select = false; z.IndexSelect = 0; }); x.Line.CrearPuntosPlantaEscaladaEtabsLine(PuntosSinEscalar, WidthWindow, HeightWindow, 0, 0, 1); });

            return new cNervio(ID, Prefijo, ListaObjetosFinal, DireccionNervio);
        }



        //Serializar y Deserializar

        public static void Serializar(string Ruta, object Objeto)
        {
            Notificador("Guardando...");
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Ruta, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, Objeto);
                stream.Close();
          
            }
            catch (Exception ex){ EventoVentanaEmergente(ex.Message,MessageBoxIcon.Exclamation); }
            Notificador("Listo");
        }
        public static void Deserealizar<T>(string Ruta, ref T Objeto)
        {
            Notificador("Cargando...");
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream streamReader = new FileStream(Ruta, FileMode.Open, FileAccess.Read, FileShare.None);
                var proyectoDeserializado = (T)formatter.Deserialize(streamReader);
                Objeto = proyectoDeserializado;
                streamReader.Close();
            }
            catch (Exception ex) { EventoVentanaEmergente(ex.Message, MessageBoxIcon.Exclamation); }
            Notificador("Listo");
        }



        /// <summary>
        /// Determina la distancia entre dos puntos.
        /// </summary>
        /// <param name="Punto1"></param>
        /// <param name="Punto2"></param>
        /// <returns></returns>
        public static float Long(cPoint Punto1, cPoint Punto2)
        {
            return (float)Math.Sqrt(Math.Pow(Punto1.X - Punto2.X, 2) + Math.Pow(Punto1.Y - Punto2.Y, 2));

        }
        /// <summary>
        /// Determina la distancia entre dos puntos.
        /// </summary>
        /// <param name="Punto1"></param>
        /// <param name="Punto2"></param>
        /// <returns></returns>
        public static float Long(PointF Punto1, PointF Punto2)
        {
            return (float)Math.Sqrt(Math.Pow(Punto1.X - Punto2.X, 2) + Math.Pow(Punto1.Y - Punto2.Y, 2));

        }






    }
}
