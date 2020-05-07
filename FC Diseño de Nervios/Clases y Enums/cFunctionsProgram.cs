﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FC_Diseño_de_Nervios
{
    public delegate void DelegateNotificadorProgram(string Alert);

    public delegate void DelegateVentanasEmergentes(string Alert, MessageBoxIcon Icono);

    public static class cFunctionsProgram
    {
       
        private static string[] Separadores = { "  ", " ", @"""" };
        public static List<eType> eTypes = ((eType[])Enum.GetValues(typeof(eType))).ToList();
        public static List<eNomenclatura> Nomenclaturas_Nervios = ((eNomenclatura[])Enum.GetValues(typeof(eNomenclatura))).ToList();
        public static List<eDireccionGrid> Direcciones_Grid = ((eDireccionGrid[])Enum.GetValues(typeof(eDireccionGrid))).ToList();
        public static List<eCambioenAltura> CambioAltura = ((eCambioenAltura[])Enum.GetValues(typeof(eCambioenAltura))).ToList();
        public static List<eCambioenAncho> CambioAncho = ((eCambioenAncho[])Enum.GetValues(typeof(eCambioenAncho))).ToList();
        public static List<eDireccion> Direcciones = ((eDireccion[])Enum.GetValues(typeof(eDireccion))).ToList();
        public static List<eNoBarra> NoBarras = ((eNoBarra[])Enum.GetValues(typeof(eNoBarra))).ToList();


        public static List<cSolicitacion> Solicitaciones;

        public static event DelegateNotificadorProgram Notificador;

        public static event DelegateVentanasEmergentes EventoVentanaEmergente;

        public static float ToleranciaHorizontal = 25f;
        public static float ToleranciaVertical = 75f;

        public const string Empresa = "efe Prima Ce";
        public const string Ext = ".nrv";
        public const string ExtFuerzas = ".fzs";
        public const string NombrePrograma = "FC-JOISTS";


 
        /// <summary>
        /// Tuple(string, List(String))
        /// </summary>
        /// <param name="FilterTitle">Ejemplo: "Archivo | *.e2k;.$et"</param>
        public static Tuple<string, List<string>> CagarArchivoTextoPlano(string FilterTitle, string TitleText)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog() { Filter = FilterTitle, Title = TitleText };
                OpenFileDialog.ShowDialog();
                string Ruta = OpenFileDialog.FileName;
                if (Ruta != "")
                {
                    List<string> Archivo = new List<string>();
                    Notificador("Cargando " + FilterTitle);
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
            }
            catch (Exception exception) { EventoVentanaEmergente(exception.Message, MessageBoxIcon.Exclamation); Notificador("Listo"); }
            return null;
        }

        public static List<string> ComprobarErroresArchivoE2K(List<string> ArchivoE2K)
        {
            List<string> Errores = new List<string>();
            try
            {
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
            }
            catch { Errores.Add("Archivo sin información"); }
            return Errores;
        }

        public static List<string> CoprobarErroresArchivoCSV(List<string> ArchivoCSV)
        {
            List<string> Errores = new List<string>();

            foreach (string Line in ArchivoCSV)
            {
                string[] LineSeparete = Line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (LineSeparete.Count() != 10)
                {
                    Errores.Add("Cantidad de columnas de datos inválida");
                    return Errores;
                }
            }
            if (Errores.Count == 0)
            {
                Errores.Add("Archivo sin Información");
            }

            return Errores;
        }


        public static void AsignarSolicitacionesLineas(List<cEstacion> ListaEstaciones, cPiso Piso)
        {
            ListaEstaciones.ForEach(Estacion =>
            {
                Piso.Lista_Lines.ForEach(Line =>
                 {
                     if (Line.Nombre == Estacion.NombreLinea && Piso.Nombre == Estacion.StoryLinea)
                     {
                         Line.Estaciones.Add(Estacion);
                         Line.Estaciones[Line.Estaciones.Count - 1].LineaOrigen = Line;
                     }
                 });
            });

            Piso.Lista_Lines.ForEach(Line => {
                if (Line.ConfigLinea.Activar_Cambio_Ejes)
                {
                    int Inidice = 0;
                    List<float> Localizaciones = Line.Estaciones.Select(x => x.Localizacion).ToList(); Localizaciones.Reverse();
                    Line.Estaciones.ForEach(x => { x.Localizacion = Localizaciones[Inidice]; Inidice++; }) ;
                    Line.Estaciones.Reverse();
                } });
        }

        public static List<cEstacion> CrearEstaciones(List<string> ArchivoCSV)
        {
            Notificador("Cargando...");
            List<cEstacion> ListaEstaciones = new List<cEstacion>();
            List<string[]> ListaFuerzas = new List<string[]>();
            ArchivoCSV.ForEach(x => ListaFuerzas.Add(x.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)));
            ListaFuerzas.RemoveAt(0);
            foreach (string[] LineaArchivoCSV in ListaFuerzas)
            {
                string NombreStory = LineaArchivoCSV[0];
                string NombreElemento = LineaArchivoCSV[1];
                string NombreLoad = LineaArchivoCSV[2];
                float Localizacion = Convert.ToSingle(LineaArchivoCSV[3]);
                float P = Convert.ToSingle(LineaArchivoCSV[4]);
                float V2 = Convert.ToSingle(LineaArchivoCSV[5]);
                float V3 = Convert.ToSingle(LineaArchivoCSV[6]);
                float T = Convert.ToSingle(LineaArchivoCSV[7]);
                float M2 = Convert.ToSingle(LineaArchivoCSV[8]);
                float M3 = Convert.ToSingle(LineaArchivoCSV[9]);
                cEstacion estacion = new cEstacion(NombreStory, NombreElemento, Localizacion);
                cSolicitacion Solicitacion = new cSolicitacion(NombreLoad, P, V2, V3, M2, M3, T);
                if (!ListaEstaciones.Exists(x => estacion.Equals(x)))
                {
                    estacion.Lista_Solicitaciones = new List<cSolicitacion>();
                    Solicitacion.EstacionOrigen = estacion;
                    estacion.Lista_Solicitaciones.Add(Solicitacion);
                    ListaEstaciones.Add(estacion);
                }
                else
                {
                    cEstacion EstaiconFind = ListaEstaciones.Find(x => estacion.Equals(x));
                    Solicitacion.EstacionOrigen = EstaiconFind;
                    EstaiconFind.Lista_Solicitaciones.Add(Solicitacion);

                }
            }
            Notificador("Listo");
            return ListaEstaciones;
        }

        public static cDatosEtabs CrearObjetosEtabs(List<string> ArchivoE2K)
        {
            cDatosEtabs DatosEtabs = new cDatosEtabs();
            DatosEtabs.Lista_Grids = CrearGridsEtabs2009(ArchivoE2K);
            DatosEtabs.Lista_Points = DeepClone(CreacionPuntosEtabsV2009(ArchivoE2K));
            DatosEtabs.Lista_Materiales = CreacionMaterialesV2009(ArchivoE2K);
            DatosEtabs.Lista_Secciones = CreacionSeccionesV2009(ArchivoE2K, DatosEtabs.Lista_Materiales);
            DatosEtabs.Lista_Pisos = CreacionListaPisosV2009(ArchivoE2K);
            CreacionLinesV2009(ArchivoE2K, DatosEtabs.Lista_Pisos, DatosEtabs.Lista_Points, DatosEtabs.Lista_Secciones);
            return DatosEtabs;
        }

        public static List<cGrid> CrearGridsEtabs2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_GRIDS = ArchivoE2K.FindIndex(x => x.Contains("$ GRIDS")) + 1;
            int IndiceFin_GRIDS = Find_FinalIndice(ArchivoE2K, IndiceInicio_GRIDS);
            List<string> ArchivoGrids = RangoDeDatosArchivoTextoPlano(IndiceInicio_GRIDS, IndiceFin_GRIDS, ArchivoE2K);
            List<cGrid> GRIDS = new List<cGrid>();

            float BubbleSize = Convert.ToSingle(ArchivoGrids[0].Split(Separadores, StringSplitOptions.RemoveEmptyEntries)[5]);
            for (int i = 1; i < ArchivoGrids.Count; i++)
            {
                string[] Grids_Separado = ArchivoGrids[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
                string NombreGrid = Grids_Separado[3];
                string DireccionGrid = Grids_Separado[5];
                float Coordenada = Convert.ToSingle(Grids_Separado[7]);
                cGrid Grid = new cGrid(NombreGrid, Coordenada, ConvertirStringtoeDireccionGrid(DireccionGrid), BubbleSize);
                GRIDS.Add(Grid);
            }
            List<float> X = GRIDS.FindAll(x => x.Direccion == eDireccionGrid.X).Select(x => x.CoordenadaInicial).ToList();
            List<float> Y = GRIDS.FindAll(x => x.Direccion == eDireccionGrid.Y).Select(x => x.CoordenadaInicial).ToList(); ;
            GRIDS.ForEach(x => x.CrearRecta(X.Max(), Y.Max(), X.Min(), Y.Min()));
            return GRIDS;
        }

        public static List<cPoint> CreacionPuntosEtabsV2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_POINT_COORDINATES = ArchivoE2K.FindIndex(x => x.Contains("$ POINT COORDINATES")) + 1;
            int IndiceFin_POINT_COORDINATES = Find_FinalIndice(ArchivoE2K, IndiceInicio_POINT_COORDINATES);
            List<string> ArchivoPuntos = RangoDeDatosArchivoTextoPlano(IndiceInicio_POINT_COORDINATES, IndiceFin_POINT_COORDINATES, ArchivoE2K);
            List<cPoint> ListaPuntos = new List<cPoint>();

            for (int i = 0; i < ArchivoPuntos.Count; i++)
            {
                string[] Point_Separado = ArchivoPuntos[i].Split(Separadores, StringSplitOptions.RemoveEmptyEntries);
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
                    cMaterial material = new cMaterial(Material_Separado[1], Convert.ToSingle(Material_Separado[7]) * cConversiones.Esfuerzo_Ton_m_to_kgf_cm, Convert.ToSingle(Material_Separado[5]) * cConversiones.Esfuerzo_Ton_m_to_kgf_cm);
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
                    ElementoEncontrado.ConfigLinea.Direccionar_Ejes();
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

        public static eNomenclatura ConvertirStringtoeNomenclatura(string StringNomenclatura)
        {
            return Nomenclaturas_Nervios.Find(x => x.ToString().ToUpper() == StringNomenclatura.ToUpper());
        }

        public static eDireccionGrid ConvertirStringtoeDireccionGrid(string StringGrid)
        {
            return Direcciones_Grid.Find(x => x.ToString().ToUpper() == StringGrid.ToUpper());
        }

        public static eCambioenAncho ConvertirStringtoeCambioAncho(string StringAncho)
        {
            return CambioAncho.Find(x => x.ToString().ToUpper() == StringAncho.ToUpper());
        }

        public static eCambioenAltura ConvertirStringtoeCambioAlto(string StringAlto)
        {
            return CambioAltura.Find(x => x.ToString().ToUpper() == StringAlto.ToUpper());
        }
        public static string ConvertireNoBarraToString(eNoBarra NoBarra)
        {
            return NoBarra.ToString().Replace("B", "#");
        }
        public static eNoBarra ConvertirStringToeNoBarra(string NoBarra)
        {
            return NoBarras.Find(x => x.ToString() == NoBarra.Replace("#", "B"));
        }
        public static eDireccion ConvertirStringtoeDireccion(string StringDireccion)
        {
            return Direcciones.Find(x => x.ToString().ToUpper() == StringDireccion.ToUpper());
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

        #region Funciones de Selección
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

#endregion
        public static List<List<cLine>> LineasParaCrearNervios(List<cLine> Lista_LineasSeleccionadas)
        {
            List<List<cLine>> Lista_Lista_Lineas = new List<List<cLine>>();

            SeleccionInteligente_2(Lista_LineasSeleccionadas);

            var GrupoBeams = from Beam in Lista_LineasSeleccionadas
                             group Beam by Beam.IndiceConjuntoSeleccion into ListaBeams
                             select new { IndiceConjuntoSeleccion = ListaBeams.Key, ListaBeams = ListaBeams.ToList() };

            foreach (var LineGrupoBeams in GrupoBeams)
            {
                Lista_Lista_Lineas.Add(DeepClone(LineGrupoBeams.ListaBeams));
            }

            return Lista_Lista_Lineas;
        }

        private static cLine FindApoyoOffSet(cLine LineaMadre, List<cLine> Lineas, int InicioIFinJ)
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

        private static List<cGrid> FindGridPertencientesalNervio(float XYo, float XYi, eDireccion DireccionNervio, List<cGrid> Grids)
        {
            List<cGrid> GridsFinales = new List<cGrid>();
            foreach (cGrid Grid in Grids)
            {
                if (DireccionNervio == eDireccion.Horizontal || DireccionNervio == eDireccion.Diagonal)
                {
                    if (Grid.Direccion == eDireccionGrid.X)
                    {
                        if (Grid.CoordenadaInicial >= XYo && XYi >= Grid.CoordenadaInicial)
                        {
                            GridsFinales.Add(Grid);
                        }
                    }
                }
                else if (DireccionNervio == eDireccion.Vertical)
                {
                    if (Grid.Direccion == eDireccionGrid.Y)
                    {
                        if (Grid.CoordenadaInicial >= XYo && XYi >= Grid.CoordenadaInicial)
                        {
                            GridsFinales.Add(Grid);
                        }
                    }
                }
            }
            return GridsFinales;
        }

        private static void AsignarApoyosAListaConOffSet(int InicioIFinJ, cLine LineApoyo, List<cLine> ListaObjetosOrganizada, int Indice, ref List<cLine> ListaObjetosOrganizadaDefinitiva)
        {
            if (Indice + 1 > ListaObjetosOrganizada.Count)
            {
                if (InicioIFinJ == 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice, LineApoyo);
                }
                else
                {
                    ListaObjetosOrganizadaDefinitiva.Add(LineApoyo);
                }
            }
            else
            {
                if (InicioIFinJ == 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice, LineApoyo);
                }
                else
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice + 1, LineApoyo);
                }
            }
        }

        public static cNervio CrearNervio(string Prefijo, int ID, List<cLine> LineasQComponenAlNervio, List<cLine> TodasLasLineas, List<cGrid> TodosLosGrids, cPiso Piso, float WidthWindow, float HeightWindow)
        {
            eDireccion DireccionNervio = LineasQComponenAlNervio.First().ConfigLinea.Direccion;
            List<cLine> ListaObjetosOrganizada;

            if (DireccionNervio == eDireccion.Horizontal || DireccionNervio == eDireccion.Diagonal)
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
                        cLine LineApoyo = FindApoyoOffSet(Objeto, TodasLasLineas, 0);
                        if (LineApoyo != null && ListaObjetosOrganizada.Exists(x => x.Nombre == LineApoyo.Nombre) == false && LineApoyo.Type != eType.Column && LineApoyo.ConfigLinea.Direccion != DireccionNervio)
                        {
                            AsignarApoyosAListaConOffSet(0, DeepClone(LineApoyo), ListaObjetosOrganizada, Indice, ref ListaObjetosOrganizada);
                            break;
                        }
                    }
                }
                if (Objeto.ConfigLinea.OffSetJ != 0)
                {
                    foreach (cLine Line in TodasLasLineas)
                    {
                        cLine LineApoyo = FindApoyoOffSet(Objeto, TodasLasLineas, 1);
                        if (LineApoyo != null && ListaObjetosOrganizada.Exists(x => x.Nombre == LineApoyo.Nombre) == false && LineApoyo.Type != eType.Column && LineApoyo.ConfigLinea.Direccion != DireccionNervio)
                        {
                            AsignarApoyosAListaConOffSet(1, LineApoyo, ListaObjetosOrganizada, Indice, ref ListaObjetosOrganizada);
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

            float XYo, XYi;
            if (DireccionNervio == eDireccion.Horizontal || DireccionNervio == eDireccion.Diagonal)
            {
                XYo = ListaObjetosOrganizada[0].ConfigLinea.Point1P.X;
                XYi = ListaObjetosOrganizada[ListaObjetosOrganizada.Count - 1].ConfigLinea.Point2P.X;
            }
            else
            {
                XYo = ListaObjetosOrganizada[0].ConfigLinea.Point1P.Y;
                XYi = ListaObjetosOrganizada[ListaObjetosOrganizada.Count - 1].ConfigLinea.Point2P.Y;
            }
            List<cGrid> GridsParaNervios = DeepClone(FindGridPertencientesalNervio(XYo, XYi, DireccionNervio, TodosLosGrids));
            List<PointF> PuntosSinEscalar = new List<PointF>(); TodasLasLineas.ForEach(y => { if (y.Type == eType.Beam) { PuntosSinEscalar.AddRange(y.Planta_Real); } });
            TodosLosGrids.ForEach(y => { PuntosSinEscalar.AddRange(y.Recta_Real); });
            ListaObjetosFinal.ForEach(x => { TodasLasLineas.FindAll(y => y.Nombre == x.Line.Nombre && x.Soporte == eSoporte.Vano).ForEach(z => { z.isSelect = false; z.Select = false; z.IndexSelect = 0; }); x.Line.CrearPuntosPlantaEscaladaEtabsLine(PuntosSinEscalar, WidthWindow, HeightWindow, 0, 0, 1); });

            return new cNervio(ID, Prefijo, ListaObjetosFinal, DireccionNervio, GridsParaNervios, Piso);
        }

        public static void RenombrarNervios(List<cNervio> Nervios, eNomenclatura NomeclaturaHztal, eNomenclatura NomenclaturaVertical)
        {
            List<cNervio> NerviosHztales_Diago = Nervios.FindAll(x => x.Direccion == eDireccion.Horizontal | x.Direccion == eDireccion.Diagonal);
            List<cNervio> NerviosVerticales = Nervios.FindAll(x => x.Direccion == eDireccion.Vertical);

            if (NerviosHztales_Diago != null)
            {
                NerviosHztales_Diago = NerviosHztales_Diago.OrderBy(x => x.Lista_Objetos.First(y => y.Soporte == eSoporte.Vano).Line.Planta_Real.Min(z => z.Y)).ToList();
                RenombrarNervios2(ref NerviosHztales_Diago, NomeclaturaHztal);
            }

            if (NerviosVerticales != null)
            {
                NerviosVerticales = NerviosVerticales.OrderBy(x => x.Lista_Objetos.First(y => y.Soporte == eSoporte.Vano).Line.Planta_Real.Min(z => z.X)).ToList();
                RenombrarNervios2(ref NerviosVerticales, NomenclaturaVertical);
            }
        }

        private static void RenombrarNervios2(ref List<cNervio> Nervios, eNomenclatura Nomenclatura)
        {
            char Letra = 'A';
            char Letra2 = ' ';

            int Contador = 1;
            foreach (cNervio nervio in Nervios)
            {
                if (Nomenclatura == eNomenclatura.Alfabética)
                {
                    string NombreFinal = Letra.ToString() + Letra2.ToString().Replace(" ", "");
                    nervio.Cambio_Nombre(NombreFinal);

                    if (Letra != 'Z')
                    {
                        Letra++;
                        if (Letra2 != ' ')
                        {
                            Letra2++;
                        }
                    }
                    else
                    {
                        Letra = 'A';
                        Letra2 = 'A';
                    }
                }
                else
                {
                    nervio.Cambio_Nombre(Contador.ToString());
                    Contador++;
                }
            }
        }
        
        public static cTendencia CrearTendenciaDefault(int IDTendencia, cTendencia_Refuerzo Tendencia_Refuerzo_Origen)
        {
            cTendencia Tendencia = new cTendencia(IDTendencia, Tendencia_Refuerzo_Origen);
            Tendencia.BarrasAEmplearBase = new List<eNoBarra>() { eNoBarra.B3, eNoBarra.B4, eNoBarra.B5, eNoBarra.B6 };
            Tendencia.BarrasAEmplearAdicional= new List<eNoBarra>() { eNoBarra.B2, eNoBarra.B3, eNoBarra.B4,eNoBarra.B5, eNoBarra.B6 };
            Tendencia.CuantiaMinima = cVariables.CuantiaMinimaSuperior;
            Tendencia.MinimaLongitud = cVariables.MinimaLongitud;
            Tendencia.MaximaLongitud = cVariables.MaximaLongitud;
            Tendencia.DeltaAlargamientoBarras = cVariables.DeltaAlargamitoBarras;
            return Tendencia;
        }

        public static bool IsPuntoInSeccion(IElemento Elemento, PointF Punto)
        {
            GraphicsPath Path = new GraphicsPath();
            Path.AddPolygon(Elemento.Vistas.Perfil_AutoCAD.Reales.ToArray());
            if (Path.IsVisible(Punto))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static cBarra CrearBarraDefault(cTendencia TendenciaOrigen, eUbicacionRefuerzo UbicacionRefuerzo)
        {
            float xi = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.First(x => x is cSubTramo).Seccion.R_Bottom * cConversiones.Dimension_cm_to_m;
            float xf = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.First(x => x is cSubTramo).Seccion.R_Bottom * cConversiones.Dimension_cm_to_m + cVariables.MinimaLongitud;
            return CrearBarra(0, TendenciaOrigen, eNoBarra.B3, UbicacionRefuerzo, 1,xi,xf);
        }
        public static cBarra CrearBarra(int ID, cTendencia TendenciaOrigen, eNoBarra NoBarra,eUbicacionRefuerzo UbicacionRefuerzo, int CantBarra, float xi,float xf)
        {
            return new cBarra(ID, TendenciaOrigen, NoBarra,UbicacionRefuerzo, CantBarra, xi,xf);

        }

        #region Librerias Weifo Luo

        public static void CambiarSkins(DockContent dock)
        {
            dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = SystemColors.ControlLight;
            dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = SystemColors.GrayText;
            dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = SystemColors.ControlLight;
            dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = SystemColors.ControlLight;
            dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.TextColor = SystemColors.ControlLightLight;
        }

        #endregion Librerias Weifo Luo

        #region Paint- Dibujar Regla

        public static Tuple<List<float>, List<float>> LongitudesElementos(List<IElemento> Elementos)
        {
            List<float> Longitudes = new List<float>();
            List<float> Longitudes_Escaladas = new List<float>();

            Elementos.ForEach(x =>
            {
                Longitudes.Add(x.Vistas.Perfil_Original.Reales.Min(y => y.X));
                Longitudes.Add(x.Vistas.Perfil_Original.Reales.Max(y => y.X));
                Longitudes_Escaladas.Add(x.Vistas.Perfil_Original.Escaladas.Min(y => y.X));
                Longitudes_Escaladas.Add(x.Vistas.Perfil_Original.Escaladas.Max(y => y.X));
            });
            return new Tuple<List<float>, List<float>>(Longitudes, Longitudes_Escaladas);
        }

        public static void DrawRegla(Graphics e, PointF PuntoInicialEscalado, PointF PuntoFinalEscalado, Tuple<List<float>, List<float>> DistanciasElementos, float Zoom)
        {
            Pen Pen1 = new Pen(Color.DarkRed);

            List<float> DistanciasElementosReales = DistanciasElementos.Item1.Distinct().ToList();
            List<float> DistanciasElementosEscalados = DistanciasElementos.Item2.Distinct().ToList();

            float IncrementoEscalado;
            float IncrementoNoEscalado;
            float TamanoLetra;
            if (Zoom > 0)
            {
                TamanoLetra = 9 * Zoom;
            }
            else
            {
                TamanoLetra = 1;
            }

            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
            float AltoLinea = 5f;

            e.DrawLine(Pen1, new PointF(PuntoInicialEscalado.X, PuntoInicialEscalado.Y + AltoLinea * 2), new PointF(PuntoFinalEscalado.X, PuntoFinalEscalado.Y + AltoLinea * 2));
            for (int i = 0; i < DistanciasElementosEscalados.Count; i++)
            {
                float IncrementoSrting = 0;
                IncrementoEscalado = DistanciasElementosEscalados[i];
                IncrementoNoEscalado = DistanciasElementosReales[i];
                if (i == 0)
                {
                    IncrementoSrting += AltoLinea * 1.5f;
                }

                string Texto = string.Format("{0:0.00}", IncrementoNoEscalado, 2);

                SizeF MessureText = e.MeasureString(Texto, Font1);
                PointF PuntoString = new PointF(IncrementoEscalado - MessureText.Width / 2 + IncrementoSrting, PuntoInicialEscalado.Y + AltoLinea + MessureText.Height);
                e.DrawLine(Pen1, IncrementoEscalado, PuntoInicialEscalado.Y + AltoLinea * 2, IncrementoEscalado, PuntoInicialEscalado.Y + AltoLinea * 2 + AltoLinea);
                e.DrawLine(Pen1, IncrementoEscalado, PuntoInicialEscalado.Y + AltoLinea * 2, IncrementoEscalado, PuntoInicialEscalado.Y + AltoLinea * 2 - AltoLinea);
                e.DrawString(Texto, Font1, Brushes.Black, PuntoString);
            }
        }

        #endregion Paint- Dibujar Regla

        #region WindowsForms

        public static void AcopleFormEnunPanel(Control Control, F_ModificarSeccion Form)
        {
            if (Form.IsDisposed) { Form = new F_ModificarSeccion(); }
            Form.TopLevel = false;
            Form.FormBorderStyle = FormBorderStyle.None;

            Control.Controls.Clear();
            Control.Controls.Add(Form);
            Control.Tag = Form;
            Form.Show();
        }

        #endregion WindowsForms
        public static void SeleccionDataGridView(DataGridView data, int CeldaAValidar, bool DejarHabilitadoUno)
        {
            int Descontar = 0;
            if (DejarHabilitadoUno){Descontar=1;}
    
            bool TodasSonSelect = false;

            for (int i = Descontar; i < data.Rows.Count; i++)
            {
                if ((bool)data.Rows[i].Cells[CeldaAValidar].Value == true)
                {
                    if (i != data.Rows.Count - 1)
                    {
                        TodasSonSelect = true;
                    }
                }
                else { TodasSonSelect = false; }
            }

            if (data.Rows.Count != 0)
            {
                for (int i = Descontar; i < data.Rows.Count; i++)
                {
                    data.Rows[i].Cells[CeldaAValidar].Value = true;
                }
            }

            if (TodasSonSelect)
            {
                if (data.Rows.Count != 0)
                {
                    for (int i = Descontar; i < data.Rows.Count; i++)
                    {
                        data.Rows[i].Cells[CeldaAValidar].Value = false;
                    }
                }
            }
        }
        public static void EstiloDatGridView(DataGridView DataGrid)
        {
            DataGridViewCellStyle StyleC = new DataGridViewCellStyle();
            StyleC.Alignment = DataGridViewContentAlignment.MiddleCenter;
            StyleC.Font = new Font("Vderdana", 8, FontStyle.Bold);

            DataGridViewCellStyle StyleR = new DataGridViewCellStyle();
            StyleR.Alignment = DataGridViewContentAlignment.MiddleCenter;
            StyleR.Font = new Font("Vderdana", 8, FontStyle.Regular);

            foreach (DataGridViewColumn column in DataGrid.Columns)
            {
                column.HeaderCell.Style = StyleC;
            }
            foreach (DataGridViewRow row in DataGrid.Rows)
            {
                row.DefaultCellStyle = StyleR;
            }
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
            catch (Exception ex) { EventoVentanaEmergente(ex.Message, MessageBoxIcon.Exclamation); }
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
        /// Determina la Longitud de una Polilinea
        /// </summary>
        /// <param name="Puntos">Lista de Puntos</param>
        /// <returns></returns>
        public static float Long(List<PointF> Puntos)
        {
            float Longitud = 0;
            for (int i=0;i<Puntos.Count;i++)
            {
                if (i + 1 < Puntos.Count)
                {
                    PointF PuntoInicial = Puntos[i];
                    PointF PuntoFinal = Puntos[i + 1];
                    Longitud += Long(PuntoInicial, PuntoFinal);
                }

            }
            return Longitud;
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

        #region Metodos Paint
        public static RectangleF CrearCirculo(float Xc, float Yc, float Radio)
        {
            return new RectangleF(Xc - Radio, Yc - Radio, Radio * 2, Radio * 2);
        }
        public static RectangleF CrearCirculo(List<PointF> Puntos)
        {
            PointF P = Puntos.First(); PointF P2 = Puntos.Last();
            return CrearCirculo(P.X, P.Y, Math.Abs(P.X-P2.X));
        }

        public static void CerrarPoligonoParaMomentos(ref List<PointF> Puntos,List<PointF> PuntosSinEscalar,float YIgualCeroEscalado)
        {
            float YInicial = PuntosSinEscalar[0].Y;
            float YFinal = PuntosSinEscalar[PuntosSinEscalar.Count-1].Y;
            float X,YInicialSinEscala,YFinalSinEscala;
            if (YInicial != 0)
            {
                YInicialSinEscala = Puntos[0].Y;
                X = Puntos.Find(x => x.Y == YInicialSinEscala).X;
                Puntos.Insert(0,new PointF(X, YIgualCeroEscalado));
            }
            if (YFinal != 0)
            {
                YFinalSinEscala = Puntos[Puntos.Count - 1].Y;
                X = Puntos.FindLast(x => x.Y == YFinalSinEscala).X;
                Puntos.Add(new PointF(X, YIgualCeroEscalado));
            }
        }

        #endregion


        #region Vectorial
        /// <summary>
        /// Distancia Minima Entre Una Recta y Un Punto
        /// </summary>
        /// <param name="Punto1_Recta">Punto Inicial de la Recta.</param>
        /// <param name="Punto2_Recta">Punto Final de la Recta.</param>
        /// <param name="Punto">Punto </param>
        public static float Dist(PointF Punto1_Recta,PointF Punto2_Recta, PointF Punto)
        {
            float m = (Punto1_Recta.Y - Punto2_Recta.Y) / (Punto1_Recta.X - Punto2_Recta.X);
            float Numerador = Math.Abs(-m * Punto.X + Punto.Y + m * Punto1_Recta.X - Punto1_Recta.Y);
            float Denominador = (float)Math.Sqrt(Math.Pow(-m, 2) + Math.Pow(1, 2));
            return Numerador / Denominador;

        }



        #endregion



        public static void DiseñarNervio(cNervio Nervio)
        {
            //Acero inferior 


            //Determinar Area Minima 

            List<float> AsminSubTramo = new List<float>();

            Nervio.Lista_Elementos.ForEach((x => {
                if (x is cSubTramo)
                {
                    cSubTramo subtramo = (cSubTramo)x;
                    float AminAx = subtramo.Seccion.B * subtramo.Seccion.H * Nervio.Tendencia_Refuerzos.TInfeSelect.CuantiaMinima;
                    AsminSubTramo.Add(AminAx);
                }
            
            }));
            float Asmin = AsminSubTramo.Min(); //Barra de Refuerzo Base
            eNoBarra BarraPropuesta= ProponerUnaBarra(Asmin, Nervio.Tendencia_Refuerzos.TInfeSelect.BarrasAEmplearBase, Nervio);
            AgregarBarraContinua(Nervio.Lista_Elementos, Nervio.Tendencia_Refuerzos.TInfeSelect,BarraPropuesta,eUbicacionRefuerzo.Inferior);


        }



        private static void AgregarBarraContinua(List<IElemento> Elementos,cTendencia tendencia,eNoBarra NoBarra,eUbicacionRefuerzo Ubicacion)
        {
            float LongitudElementos = Elementos.Sum(x => x.Longitud);
            float d1 = tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.d1*cConversiones.Dimension_cm_to_m;
         
            if(LongitudElementos< tendencia.MaximaLongitud)
            {
                cBarra BarraInicial = new cBarra(0, tendencia, NoBarra, Ubicacion, 1, d1, LongitudElementos-d1);
                BarraInicial.GanchoDerecha = eTipoGancho.G90;
                BarraInicial.GanchoIzquierda = eTipoGancho.G90;
                tendencia.AgregarBarra(BarraInicial);
            }

        }








        private static eNoBarra ProponerUnaBarra(float As, List<eNoBarra> Barras,cNervio Nervio)
        {
            List<eNoBarra> BarrasFinales = new List<eNoBarra>();
            Barras.ForEach(Barra => {

                float Delta = cDiccionarios.AceroBarras[Barra] - As;
                if (Delta > 0)
                    BarrasFinales.Add(Barra);
            });


            if (BarrasFinales.Count == 0)
            {
                EventoVentanaEmergente?.Invoke($"No se encontró el refuerzo necesario para las solicitaciones del Nervio: {Nervio.Nombre}", MessageBoxIcon.Error);
                return eNoBarra.BNone;
            }
            else
            {
                return BarrasFinales.Min(); 
            }

       }





    }
}