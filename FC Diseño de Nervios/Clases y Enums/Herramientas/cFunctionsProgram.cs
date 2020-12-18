using FC_BFunctionsAutoCAD;
using FC_Diseño_de_Nervios.Controles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using WebTools;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Threading;
using FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas;
using FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo;

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

        public static PerformanceCounter cpuCounter =new PerformanceCounter("Processor", "% Processor Time", "_Total");


        public static float ToleranciaHorizontal = 25f;
        public static float ToleranciaVertical = 75f;

        public const string Empresa = "efe Prima Ce";
        public const string Ext = ".nrv";
        public const string NombrePrograma = "Diseño de Nervios";
        public const string Ext_ConfigVentana = "ConfigVentanaNervios.config";
        public const string Ext_ConfigVentanaSelectNervios = "ConfigVentanaSelectNervios.config";
        public static string Ruta_CarpetaLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Empresa, NombrePrograma);
        public const string Ext_MemoriaCalculos = "MemoriaCalculos.pdf";
        public const string Ext_BackupNervios = ".bkp";

        public static void Notificar(string Text)
        {
            Notificador?.Invoke(Text);
        }
        public static void VentanaEmergenteInformacion(string Text)
        {
            EventoVentanaEmergente?.Invoke(Text, MessageBoxIcon.Information);
        }
        public static void VentanaEmergenteError(string Text)
        {
            EventoVentanaEmergente?.Invoke(Text, MessageBoxIcon.Error);
        }
        public static void VentanaEmergenteExclamacion(string Text)
        {
            EventoVentanaEmergente?.Invoke(Text, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// Tuple(string, List(String))
        /// </summary>
        /// <param name="FilterTitle">Ejemplo: "Archivo V9.5| *.e2k;.$et"</param>
        public static Tuple<string, List<string>> CagarArchivoTextoPlanoWindowsForm(string FilterTitle, string TitleText)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog() { Filter = FilterTitle, Title = TitleText };
                OpenFileDialog.ShowDialog();
                string Ruta = OpenFileDialog.FileName;
                if (Ruta != "")
                {
                    List<string> Archivo = new List<string>();
                    Notificador?.Invoke("Cargando " + FilterTitle);
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
                    Notificador?.Invoke("Listo");
                    return new Tuple<string, List<string>>(Ruta, Archivo);
                }

                Notificador?.Invoke("Listo");
            }
            catch (Exception exception) { EventoVentanaEmergente?.Invoke(exception.Message, MessageBoxIcon.Exclamation); Notificador?.Invoke("Listo"); }
            return null;
        }

        #region Para Etabs
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
                    Errores.Add("Archivo CSV inválido.");
                    return Errores;
                }
            }
            if (ArchivoCSV.Count == 0)
            {
                Errores.Add("Archivo CSV sin información");
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
                         Line.Estaciones.Last().LineaOrigen = Line;
                     }
                 });
            });

            Piso.Lista_Lines.ForEach(Line =>
            {
                if (Line.ConfigLinea.Activar_Cambio_Ejes)
                {
                    int Inidice = 0;
                    List<float> Localizaciones = Line.Estaciones.Select(x => x.Localizacion).ToList(); Localizaciones.Reverse();
                    Line.Estaciones.ForEach(x => { x.Localizacion = Localizaciones[Inidice]; Inidice++; });
                    Line.Estaciones.Reverse();
                }
            });
        }

        public static List<cEstacion> CrearEstaciones(List<string> ArchivoCSV)
        {
            Notificador?.Invoke("Cargando...");
            List<cEstacion> ListaEstaciones = new List<cEstacion>();
            List<string[]> ListaFuerzas = new List<string[]>();
            ArchivoCSV.ForEach(x => ListaFuerzas.Add(x.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)));
            ListaFuerzas.RemoveAt(0);

            var ListaEstacionesResumidas = from Estacion in ListaFuerzas
                                           group Estacion by new { Piso = Estacion[0], Elemento = Estacion[1], Loc = Estacion[3] } into Solicitaciones
                                           select new { Piso = Solicitaciones.Key, Estaciones = Solicitaciones.ToList() };

            foreach (var Estacion in ListaEstacionesResumidas)
            {

                string NombreStory = Estacion.Piso.Piso.Replace(" ", "");
                string NombreElemento = Estacion.Piso.Elemento;
                float Localizacion = (float)Math.Round(Convert.ToSingle(Estacion.Piso.Loc), 3);
                cEstacion estacion = new cEstacion(NombreStory, NombreElemento, Localizacion);
                estacion.Lista_Solicitaciones = new List<cSolicitacion>();
                Estacion.Estaciones.ForEach(LineaArchivoCSV =>
                {
                    string NombreLoad = LineaArchivoCSV[2];
                    float P = -Convert.ToSingle(LineaArchivoCSV[4]);
                    float V2 = Convert.ToSingle(LineaArchivoCSV[5]);
                    float V3 = Convert.ToSingle(LineaArchivoCSV[6]);
                    float T = Convert.ToSingle(LineaArchivoCSV[7]);
                    float M2 = Convert.ToSingle(LineaArchivoCSV[8]);
                    float M3 = Convert.ToSingle(LineaArchivoCSV[9]);
                    cSolicitacion Solicitacion = new cSolicitacion(NombreLoad, P, V2, V3, M2, M3, T);
                    AgregarSolicitacioneaEstacion(Solicitacion, estacion.Lista_Solicitaciones);

                });
                ListaEstaciones.Add(estacion);
            }
            Notificador?.Invoke("Listo");
            return ListaEstaciones;
        }

        private static void AgregarSolicitacioneaEstacion(cSolicitacion Solic,List<cSolicitacion> SolicitacionesAntiguas)
        {
            cSolicitacion SolicitacionRepetida = SolicitacionesAntiguas.Find(y => y.Nombre == Solic.Nombre);

            if (SolicitacionRepetida != null)
            {
                float MaxV2Final = Math.Abs(SolicitacionRepetida.V2) > Math.Abs(Solic.V2) ? SolicitacionRepetida.V2 : Solic.V2;
                float MaxM3Final = Math.Abs(SolicitacionRepetida.M3) > Math.Abs(Solic.M3) ? SolicitacionRepetida.M3 : Solic.M3;

                SolicitacionRepetida.V2 = MaxV2Final;
                SolicitacionRepetida.M3 = MaxM3Final;
            }
            else
            {
                SolicitacionesAntiguas.Add(Solic);
            }
        }


        public static cDatosEtabs CrearObjetosEtabs(List<string> ArchivoE2K)
        {
            var datosEtabs= FCB_Etabs.Funciones.cFuncionesImportacion.CrearObjetosEtabs(ArchivoE2K);
            return datosEtabs.ConvertDatosEtabs();
            //cDatosEtabs DatosEtabs = new cDatosEtabs();
            //DatosEtabs.Lista_Grids = CrearGridsEtabs2009(ArchivoE2K);
            //DatosEtabs.Lista_Points = DeepClone(CreacionPuntosEtabsV2009(ArchivoE2K));
            //DatosEtabs.Lista_Materiales = CreacionMaterialesV2009(ArchivoE2K);
            //DatosEtabs.Lista_Secciones = CreacionSeccionesV2009(ArchivoE2K, DatosEtabs.Lista_Materiales);
            //DatosEtabs.Lista_Pisos = CreacionListaPisosV2009(ArchivoE2K);
            //CreacionLinesV2009(ArchivoE2K, DatosEtabs.Lista_Pisos, DatosEtabs.Lista_Points, DatosEtabs.Lista_Secciones);
            //return DatosEtabs;
        }

        private static cDatosEtabs ConvertDatosEtabs(this FCB_Etabs.Elementos.cDatosEtabs datosEtabs)
        {
            cDatosEtabs datosEtabs1 = new cDatosEtabs()
            {
                Lista_Points = datosEtabs.Lista_Points.ConvertAll(ConvertcPoint),
                Lista_Secciones = datosEtabs.Lista_Secciones.ConvertAll(ConvertTocSeccion),
                Lista_Grids = datosEtabs.Lista_Grids.ConvertAll(ConvertcGrid),
                Lista_Materiales = datosEtabs.Lista_Materiales.ConvertAll(ConvertMaterial),

            };
            datosEtabs1.Lista_Pisos= datosEtabs.Lista_Pisos.ConvertTocPisoList(datosEtabs1.Lista_Secciones);
            List<float> X = datosEtabs1.Lista_Grids.FindAll(x => x.Direccion == eDireccionGrid.X).Select(x => x.CoordenadaInicial).ToList();
            List<float> Y = datosEtabs1.Lista_Grids.FindAll(x => x.Direccion == eDireccionGrid.Y).Select(x => x.CoordenadaInicial).ToList(); ;
            datosEtabs1.Lista_Grids.ForEach(x => x.CrearRecta(X.Max(), Y.Max(), X.Min(), Y.Min()));
            return datosEtabs1;
        }
        private static cGrid ConvertcGrid(FCB_Etabs.Elementos.cGrid gridb)
        {
            eDireccionGrid direccion = gridb.Direccion == FCB_Etabs.Enums.eDireccionGrid.X ? eDireccionGrid.X : eDireccionGrid.Y;
            return new cGrid(gridb.Nombre, gridb.Coord, direccion, gridb.BubbleSize);
        }

        private static cPoint ConvertcPoint(this FCB_Etabs.Elementos.cPoint pointb)
        {
            return new cPoint(pointb.Nombre, pointb.X, pointb.Y, pointb.DeltaZ) { Z= pointb.Z};
        }

        private static cMaterial ConvertMaterial(this FCB_Etabs.Elementos.Materiales.IMaterial material)
        {
            if (material.Material == FCB_Etabs.Enums.eMaterial.CONCRETE)
            {
                cMaterial cMaterial = new cMaterial(material.Nombre, (material as FCB_Etabs.Elementos.Materiales.cMaterialConcrete).fc, (material as FCB_Etabs.Elementos.Materiales.cMaterialConcrete).fy);
                return cMaterial;
            }
            return default;
        }
       
        private static cSeccion ConvertTocSeccion(this FCB_Etabs.Elementos.Seccion.cSeccion seccion)
        {
            float coverBottom = 0f; float coverTop = 0f;
            if (seccion.ConcreteSection != null)
            {
                if (seccion.ConcreteSection.DesignType == FCB_Etabs.Enums.eDesignType.BEAM)
                {
                    coverBottom = (seccion.ConcreteSection as FCB_Etabs.Elementos.Seccion.Concrete_Sections.cBeam).CoverBottom;
                    coverTop = (seccion.ConcreteSection as FCB_Etabs.Elementos.Seccion.Concrete_Sections.cBeam).CoverTop;
                }
                else if (seccion.ConcreteSection.DesignType == FCB_Etabs.Enums.eDesignType.COLUMN)
                {
                    coverBottom = (seccion.ConcreteSection as FCB_Etabs.Elementos.Seccion.Concrete_Sections.cColumn).Cover;
                    coverTop = (seccion.ConcreteSection as FCB_Etabs.Elementos.Seccion.Concrete_Sections.cColumn).Cover;
                }
            }
            if (seccion.Shape.TypeShape == FCB_Etabs.Enums.eTypeShape.RECTANGLE)
            {
                var shape = seccion.Shape as FCB_Etabs.Elementos.Seccion.Shapes.cRectangle;
                return new cSeccion(seccion.Nombre, shape.B * cConversiones.Dimension_m_to_cm , shape.D* cConversiones.Dimension_m_to_cm) { Material= ConvertMaterial(seccion.Material), R_Bottom= coverBottom * cConversiones.Dimension_m_to_cm, R_Top = coverTop* cConversiones.Dimension_m_to_cm};
            }
            return null;
        }
        
        private static List<cPiso> ConvertTocPisoList(this List<FCB_Etabs.Elementos.cPiso> pisosb, List<cSeccion> seccions)
        {
            List<cPiso> pisos = new List<cPiso>();
            pisosb.ForEach(P => pisos.Add(P.ConvertTocPiso(seccions)));
            return pisos;
        }
        private static cPiso ConvertTocPiso(this FCB_Etabs.Elementos.cPiso pisob, List<cSeccion> seccions)
        {
            return new cPiso(pisob.Nombre, pisob.H) { Hacum = pisob.Hacum, Lista_Lines = pisob.Lista_Lines.ConvertTocLines(seccions) };
        }

        private static List<cLine> ConvertTocLines(this List<FCB_Etabs.Elementos.Line.cLine> linesb, List<cSeccion> seccions)
        {
            List<cLine> linesFinal = new List<cLine>();
            linesb.ForEach(l => linesFinal.Add(l.ConvertToCLine(seccions)));
            return linesFinal;
        }
        private static cLine ConvertToCLine(this FCB_Etabs.Elementos.Line.cLine lineb, List<cSeccion> seccions)
        {
            seccions.RemoveAll(y => y == null);
            eType type= eType.None;
            switch (lineb.Type)
            {
                case FCB_Etabs.Enums.eTypeElement.NONE:
                    type = eType.None;
                    break;
                case FCB_Etabs.Enums.eTypeElement.COLUMN:
                    type = eType.Column;
                    break;
                case FCB_Etabs.Enums.eTypeElement.BEAM:
                    type = eType.Beam;
                    break;
                case FCB_Etabs.Enums.eTypeElement.BRACE:
                    type = eType.Brace;
                    break;
                case FCB_Etabs.Enums.eTypeElement.FLOOR:
                    type = eType.Floor;
                    break;
                case FCB_Etabs.Enums.eTypeElement.PANEL:
                    type = eType.None;
                    break;
                case FCB_Etabs.Enums.eTypeElement.RAMP:
                    type = eType.None;
                    break;
                case FCB_Etabs.Enums.eTypeElement.AREA:
                    type = eType.Wall;
                    break;
                default:
                    break;
            }

            cLine line = new cLine(lineb.Nombre, type)
            {
                ConfigLinea = ConverTo(lineb.ConfigLinea),
                Seccion = seccions.Find(y=>y.Nombre== lineb.Seccion.Nombre),
                Story = lineb.Story,
                
            };
            line.ConfigLinea.Activar_Cambio_Ejes = lineb.CambioEjes;
            line.ConfigLinea.Point1P.Z = lineb.ConfigLinea.Point1P.Z;
            line.ConfigLinea.Point2P.Z = lineb.ConfigLinea.Point2P.Z;

            ControlarSeccionesNoRectangulares(line);

            cConfigLinea ConverTo(FCB_Etabs.Elementos.Line.cConfigLine clb)
            {
                return new cConfigLinea(clb.Point1P.ConvertcPoint(), clb.Point2P.ConvertcPoint())
                {
                    Angulo = clb.Angulo, OffSetI = clb.OffSetI, OffSetJ = clb.OffSetJ,
                };

            }
            return line;
        }


        public static List<cGrid> CrearGridsEtabs2009(List<string> ArchivoE2K)
        {
            int IndiceInicio_GRIDS = ArchivoE2K.FindIndex(x => x.Contains("$ GRIDS")) + 1;
            int IndiceFin_GRIDS = Find_FinalIndice(ArchivoE2K, IndiceInicio_GRIDS);
            List<string> ArchivoGrids = RangoDeDatosArchivoTextoPlano(IndiceInicio_GRIDS, IndiceFin_GRIDS, ArchivoE2K);
            List<cGrid> GRIDS = new List<cGrid>();
            List<string[]> GridsSeparate = new List<string[]>();
            foreach (string Line in ArchivoGrids) { GridsSeparate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }
            float BubbleSize = Convert.ToSingle(GridsSeparate[0][GridsSeparate[0].ToList().FindIndex(y => y.Contains("BUBBLESIZE")) + 1]);

            foreach (string[] Grids_Separado in GridsSeparate)
            {
                if (Grids_Separado.Contains("LABEL") && Grids_Separado.Contains("DIR"))
                {
                    string NombreGrid = Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("LABEL")) + 1];
                    string DireccionGrid = Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("DIR")) + 1];
                    float Coordenada = Convert.ToSingle(Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("COORD")) + 1]);
                    cGrid Grid = new cGrid(NombreGrid, Coordenada, ConvertirStringtoeDireccionGrid(DireccionGrid), BubbleSize);
                    GRIDS.Add(Grid);
                }else if (Grids_Separado.Contains("LABEL") && Grids_Separado.Contains("LINE"))
                {
                    string NombreGrid = Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("LABEL")) + 1];
                    float XA = Convert.ToSingle(Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("XA")) + 1]);
                    float XB = Convert.ToSingle(Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("XB")) + 1]);
                    float YA = Convert.ToSingle(Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("YA")) + 1]);
                    float YB = Convert.ToSingle(Grids_Separado[Grids_Separado.ToList().FindIndex(z => z.Contains("YB")) + 1]);
                    cGrid Grid = new cGrid(NombreGrid, XA, XB, YA, YB, BubbleSize);
                    GRIDS.Add(Grid);
                }
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
            List<string[]> Punto_Separates = new List<string[]>();
            foreach (string Line in ArchivoPuntos) { Punto_Separates.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }
            List<cPoint> ListaPuntos = new List<cPoint>();
            foreach (string[] Point_Separado in Punto_Separates)
            {
                float DeltaZ = 0f;
                if (Point_Separado.Length > 4)
                {
                    DeltaZ = (float)Math.Round(Convert.ToSingle(Point_Separado[4]), 2);
                }
                cPoint point = new cPoint(Point_Separado[1], (float)Math.Round(Convert.ToSingle(Point_Separado[2]), 2), (float)Math.Round(Convert.ToSingle(Point_Separado[3]), 2), DeltaZ);
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
            List<string[]> Materiales_Separate = new List<string[]>();
            foreach (string Line in ArchivoMateriales) { Materiales_Separate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }
            foreach (string[] Material_Separado in Materiales_Separate)
            {
                string NombreMaterial = Material_Separado[1];

                if (Material_Separado.Contains("DESIGNTYPE"))
                {
                    if (Material_Separado.Contains("CONCRETE"))
                    {
                        float FC = Convert.ToSingle(Material_Separado[Material_Separado.ToList().FindIndex(y => y.Contains("FC")) + 1]) * cConversiones.Esfuerzo_Ton_m_to_kgf_cm;
                        float FY = Convert.ToSingle(Material_Separado[Material_Separado.ToList().FindIndex(y => y.Contains("FY")) + 1]) * cConversiones.Esfuerzo_Ton_m_to_kgf_cm;
                        cMaterial Concrete = new cMaterial(NombreMaterial, FC, FY);
                        Lista_Materiales.Add(Concrete);
                    }
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
            List<string[]> LineConnectivitiesSeparate = new List<string[]>();
            foreach (string Line in ArchivoLineConnectivities) { LineConnectivitiesSeparate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }

            foreach (string[] LineConectivitieSeparate in LineConnectivitiesSeparate)
            {
                string NombreLine = LineConectivitieSeparate[1];
                cPoint Point1 = DeepClone(Lista_Puntos.Find(x => x.Nombre == LineConectivitieSeparate[3]));
                cPoint Point2 = DeepClone(Lista_Puntos.Find(x => x.Nombre == LineConectivitieSeparate[4]));
                cLine Line = new cLine(NombreLine, ConvertirStringtoeType(LineConectivitieSeparate[2]));
                Line.ConfigLinea = new cConfigLinea(Point1, Point2);
                Lista_Line.Add(Line);
            }
            
            int IndiceInicio_LINE_ASSIGNS = ArchivoE2K.FindIndex(x => x.Contains("$ LINE ASSIGNS")) + 1;
            int IndiceFin_LINE_ASSIGNS = Find_FinalIndice(ArchivoE2K, IndiceInicio_LINE_ASSIGNS);
            List<string> ArchivoLineAssigns = RangoDeDatosArchivoTextoPlano(IndiceInicio_LINE_ASSIGNS, IndiceFin_LINE_ASSIGNS, ArchivoE2K);
            List<string[]>  LineAssignsSeparate = new List<string[]>();
            foreach (string Line in ArchivoLineAssigns) { LineAssignsSeparate.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }
            foreach (string[] Elemento in LineAssignsSeparate)
            {
                if (Elemento.Contains("SECTION"))
                {
                    string NombreElemento = Elemento[1];
                    int IndiceSection = Elemento.ToList().FindIndex(y => y.Contains("SECTION"));
                    int IndiceANG = Elemento.ToList().FindIndex(y => y.Contains("ANG"));
                    float OffSetI = 0; float OffSetJ = 0;
                    string NombrePiso = "";
                    for (int i = 2; i < IndiceSection; i++)
                        NombrePiso += Elemento[i];
                    string NombreSeccion = Elemento[IndiceSection + 1];

                    float Angulo = Convert.ToSingle(Elemento[IndiceANG + 1]);


                    if (Elemento.Contains("LENGTHOFFI"))
                    {
                        OffSetI = Convert.ToSingle(Elemento[Elemento.ToList().FindIndex(y => y.Contains("LENGTHOFFI")) + 1]);
                    }
                    if (Elemento.Contains("LENGTHOFFJ"))
                    {
                        OffSetJ = Convert.ToSingle(Elemento[Elemento.ToList().FindIndex(y => y.Contains("LENGTHOFFJ")) + 1]);
                    }
                    
                        
                    cPiso PisoEncontrado = Lista_Pisos.Find(x => x.Nombre == NombrePiso);
                    if (PisoEncontrado != null)
                    {
                        cLine ElementoEncontrado = DeepClone(Lista_Line.Find(x => x.Nombre == NombreElemento));
                        ElementoEncontrado.Seccion = Lista_Secciones.Find(x => x.Nombre == NombreSeccion);
                        ControlarSeccionesNoRectangulares(ElementoEncontrado);
                        ElementoEncontrado.Story = NombrePiso;
                        ElementoEncontrado.ConfigLinea.OffSetI = OffSetI;
                        ElementoEncontrado.ConfigLinea.OffSetJ = OffSetJ;
                        ElementoEncontrado.ConfigLinea.Angulo = Angulo;
                        ElementoEncontrado.ConfigLinea.Direccionar_Ejes();
                        PisoEncontrado.Lista_Lines.Add(ElementoEncontrado);
                    }


                }
            }

            //Z a cada Punto
            Lista_Pisos.ForEach(Piso => {
                Piso.Lista_Lines.ForEach(Line => {
                    if (Line.Type == eType.Column | Line.Type == eType.Brace)
                    {
                        Line.ConfigLinea.Point1P.Z = Line.ConfigLinea.Point1P.DeltaZ == 0
                        ? (float)Math.Round(Piso.Hacum - Piso.H, 2)
                        : (float)Math.Round(Piso.Hacum - Line.ConfigLinea.Point1P.DeltaZ, 2);
                        Line.ConfigLinea.Point2P.Z = (float)Math.Round(Piso.Hacum - Line.ConfigLinea.Point2P.DeltaZ, 2);

                    }
                    else if (Line.Type == eType.Beam)
                    {
                        Line.ConfigLinea.Point1P.Z = (float)Math.Round(Piso.Hacum - Line.ConfigLinea.Point1P.DeltaZ, 2);
                        Line.ConfigLinea.Point2P.Z = (float)Math.Round(Piso.Hacum - Line.ConfigLinea.Point2P.DeltaZ, 2);
                    }
                    Line.ConfigLinea.CalcularLongitud();
                });
            });
        }

        private static void ControlarSeccionesNoRectangulares(cLine Line)
        {
            if (Line.Seccion== null)
            {
                Line.Seccion = new cSeccion("SecciónPredifinida", 20f, 20f) { Material = new cMaterial("H210", 210, 4220) };
                EventoVentanaEmergente($"Sección no Rectangular encontrada, se remplazara por una sección predifinida, Elemento: {Line.Nombre} | Piso: {Line.Story}", MessageBoxIcon.Information);

            }
        }
        public static List<cPiso> CreacionListaPisosV2009(List<string> ArchivoE2K)
        {
            List<cPiso> Pisos = new List<cPiso>();

            int IndiceInicio_STORIES_IN_SEQUENCE_FROM_TOP = ArchivoE2K.FindIndex(x => x.Contains("$ STORIES - IN SEQUENCE FROM TOP")) + 1;
            int IndiceFin_STORIES_IN_SEQUENCE_FROM_TOP = Find_FinalIndice(ArchivoE2K, IndiceInicio_STORIES_IN_SEQUENCE_FROM_TOP);
            List<string> ArchivoPisos = RangoDeDatosArchivoTextoPlano(IndiceInicio_STORIES_IN_SEQUENCE_FROM_TOP, IndiceFin_STORIES_IN_SEQUENCE_FROM_TOP, ArchivoE2K);

            List<string[]> Pisos_Separates = new List<string[]>();
            foreach (string Line in ArchivoPisos) { Pisos_Separates.Add(Line.Split(Separadores, StringSplitOptions.RemoveEmptyEntries)); }

            foreach (string[] Piso_Separado in Pisos_Separates)
            {
                if (Piso_Separado.Contains("STORY") && Piso_Separado.Contains("HEIGHT"))
                {
                    int IndiceHeight = Piso_Separado.ToList().FindIndex(y => y.Contains("HEIGHT"));
                    float HPiso = Convert.ToSingle(Piso_Separado[IndiceHeight + 1]);
                    string NamePiso = "";
                    for (int i = 1; i < IndiceHeight; i++)
                        NamePiso += Piso_Separado[i];
                    cPiso piso = new cPiso(NamePiso, HPiso);
                    Pisos.Add(piso);
                }
            }

            List<float> Hs = Pisos.Select(y => y.H).ToList();
            float Hacum = 0;
            for (int i = Pisos.Count - 1; i >= 0; i--)
            {
                Hacum += Pisos[i].H;
                Pisos[i].Hacum = Hacum;
            }
            return Pisos;
        }
        #endregion
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
                T temp = (T)formatter.Deserialize(ms);
                if (temp is cBarra)
                    CambiarPropBarraClonada((cBarra)Convert.ChangeType(temp, typeof(cBarra)));

                return temp;
            }
        }

        public static T DeepCloneFast<T>(T obj)
        {
            T temp = Force.DeepCloner.DeepClonerExtensions.DeepClone(obj);
            if (temp is cBarra)
                CambiarPropBarraClonada((cBarra)Convert.ChangeType(temp, typeof(cBarra)));
            if (temp is cBloqueEstribos)
                CambiarPropBloqueEstribos((cBloqueEstribos)Convert.ChangeType(temp, typeof(cBloqueEstribos)));
            return temp;
        }


        private static void CambiarPropBarraClonada(cBarra Barra)
        {
            Barra.C_Barra.IsSelect = false;
            Barra.C_Barra.IsSelectArrastre = false;
        }
        private static void CambiarPropBloqueEstribos(cBloqueEstribos bloque)
        {
            bloque.Recuadro_ModoEdicion.IsSelect = false;
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

        #region Crear Nervios
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

        public static List<cGrid> FindGridPertencientesalNervio(float XYo, float XYi, eDireccion DireccionNervio, List<cGrid> Grids)
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
                    LineApoyo.IsApoyo = true;
                }
                else
                {
                    ListaObjetosOrganizadaDefinitiva.Add(LineApoyo);
                    LineApoyo.IsApoyo = true;
                }
            }
            else
            {
                if (InicioIFinJ == 0)
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice, LineApoyo);
                    LineApoyo.IsApoyo = true;
                }
                else
                {
                    ListaObjetosOrganizadaDefinitiva.Insert(Indice + 1, LineApoyo);
                    LineApoyo.IsApoyo = true;
                }
            }
        }

        public static cNervio CrearNervio(string Prefijo, int ID, List<cLine> LineasQComponenAlNervio, List<cLine> TodasLasLineas, List<cGrid> TodosLosGrids, cPiso Piso, float WidthWindow, float HeightWindow,string NombreNervio = "")
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
            ListaObjetosOrganizada.ForEach(y => y.IsApoyo = false);
            for (int Indice = 0; Indice < ListaObjetosOrganizada.Count; Indice++)
            {
                cLine Objeto = ListaObjetosOrganizada[Indice];
                int IndiceAdicional = 0;
                if (!Objeto.IsApoyo)
                {
                    if (Objeto.ConfigLinea.OffSetI != 0)
                    {

                        cLine LineApoyo = FindApoyoOffSet(Objeto, TodasLasLineas, 0);
                        if (LineApoyo != null && !ListaObjetosOrganizada.Exists(x => x.Nombre == LineApoyo.Nombre) &&
                             LineApoyo.ConfigLinea.Direccion != DireccionNervio)// LineApoyo.Type != eType.Column && Agregar Linea de Codigo OJO
                        {
                            AsignarApoyosAListaConOffSet(0, DeepClone(LineApoyo), ListaObjetosOrganizada, Indice, ref ListaObjetosOrganizada);
                            IndiceAdicional++;
                        }

                    }
                    if (Objeto.ConfigLinea.OffSetJ != 0)
                    {

                        cLine LineApoyo = FindApoyoOffSet(Objeto, TodasLasLineas, 1);
                        if (LineApoyo != null && !ListaObjetosOrganizada.Exists(x => x.Nombre == LineApoyo.Nombre)
                            && LineApoyo.ConfigLinea.Direccion != DireccionNervio) // LineApoyo.Type != eType.Column && Agregar Linea de Codigo OJO
                        {
                            AsignarApoyosAListaConOffSet(1, DeepClone(LineApoyo), ListaObjetosOrganizada, Indice + IndiceAdicional, ref ListaObjetosOrganizada);
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
            if (ComprobarLineasConEstacionesParaCrearNervios(ListaObjetosFinal) && ListaObjetosFinal.Count>1)
            {
                ElementosSinConsiderar(ListaObjetosFinal);
                List<cGrid> GridsParaNervios = DeepClone(FindGridPertencientesalNervio(XYo, XYi, DireccionNervio, TodosLosGrids));
                List<PointF> PuntosSinEscalar = new List<PointF>(); TodasLasLineas.ForEach(y => { if (y.Type == eType.Beam) { PuntosSinEscalar.AddRange(y.Planta_Real); } });
                TodosLosGrids.ForEach(y => { PuntosSinEscalar.AddRange(y.Recta_Real); });
                ListaObjetosFinal.ForEach(x => { TodasLasLineas.FindAll(y => y.Nombre == x.Line.Nombre && x.Soporte == eSoporte.Vano).ForEach(z => { z.isSelect = false; z.Select = false; z.IndexSelect = 0; }); x.Line.CrearPuntosPlantaEscaladaEtabsLine(PuntosSinEscalar, WidthWindow, HeightWindow, 0, 0, 1); });


                cNervio NervioNew = null;

                try
                {
                    NervioNew = new cNervio(ID, Prefijo, ListaObjetosFinal, DireccionNervio, GridsParaNervios, Piso);
                    if (NombreNervio != string.Empty)
                    {
                        NervioNew.Nombre = NombreNervio;
                        NervioNew.NombrarNervioDiferente = true;
                    }
                }
                catch 
                {
                    VentanaEmergenteExclamacion($"Desde el elemento {ListaObjetosFinal.First(y => y.Soporte == eSoporte.Vano).Line.Nombre} hasta el {ListaObjetosFinal.Last(y => y.Soporte == eSoporte.Vano).Line.Nombre} no pueden ser enumerados.");
                    ListaObjetosFinal.ForEach(x => { TodasLasLineas.FindAll(y => y.Nombre == x.Line.Nombre && x.Soporte == eSoporte.Vano).ForEach(z => { z.isSelect = true; z.Select = false; z.IndexSelect = 0; }); x.Line.CrearPuntosPlantaEscaladaEtabsLine(PuntosSinEscalar, WidthWindow, HeightWindow, 0, 0, 1); });
                }
                return NervioNew;
            }
            else
            {
                VentanaEmergenteExclamacion("Alguno de los elementos seleccionados no tiene estaciones.");
            }
            return null;
        }

        private static void ElementosSinConsiderar(List<cObjeto> ListaObjetos)
        {
            var Objetos = ListaObjetos.FindAll(y => y.Soporte == eSoporte.Vano && 
                            ((y.Line.ConfigLinea.Longitud* cVariables.Porc_LongOffset <= y.Line.ConfigLinea.OffSetI + y.Line.ConfigLinea.OffSetJ)
                              || (y.Line.ConfigLinea.Longitud  <= cVariables.LongMinimaElemento))).ToList();
            if (Objetos != null)
            {
                Objetos.ForEach(Objeto =>
                    {
                        int IndiceObjeto = ListaObjetos.FindIndex(y => y == Objeto);

                        if(IndiceObjeto>0 && IndiceObjeto< ListaObjetos.Count - 1)
                        {
                            cObjeto OAtras = ListaObjetos[IndiceObjeto - 1];
                            cObjeto OAdelante = ListaObjetos[IndiceObjeto + 1];

                            if(OAtras.Soporte== eSoporte.Apoyo && OAdelante.Soporte== eSoporte.Apoyo)
                            {
                                if (OAtras.Line.Seccion.B >= OAdelante.Line.Seccion.B)
                                {
                                    ListaObjetos.Remove(OAdelante);
                                }
                                else
                                {
                                    ListaObjetos.Remove(OAdelante);
                                }
                            }

                        }

                        ListaObjetos.Remove(Objeto);
                    }
                
                );
            }
        }

        public static bool ComprobarLineasConEstacionesParaCrearNervios(List<cObjeto> objetos)
        {
            foreach(cObjeto objeto in objetos)
            {
                if (objeto.Soporte== eSoporte.Vano && objeto.Line.Estaciones.Count == 0)
                    return false;
            }
            return true;
        }

        public static void RenombrarNervios(List<cNervio> Nervios, eNomenclatura NomeclaturaHztal, eNomenclatura NomenclaturaVertical)
        {
            List<cNervio> NerviosHztales_Diago = Nervios.FindAll(x => x.Direccion == eDireccion.Horizontal | x.Direccion == eDireccion.Diagonal && !x.NombrarNervioDiferente);
            List<cNervio> NerviosVerticales = Nervios.FindAll(x => x.Direccion == eDireccion.Vertical && !x.NombrarNervioDiferente);

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
            List<char> Letras = new List<char>();
            Letras.Add('A');
            int Contador = 1;
            foreach (cNervio nervio in Nervios)
            {
                if (Nomenclatura == eNomenclatura.Alfabética)
                {
                    string NombreFinal = string.Join("", Letras);
                    nervio.Cambio_Nombre(NombreFinal);

                    if (Letras.Last() == 'Z')
                    {
                        for(int i=0;i< Letras.Count;i++)
                        {
                            Letras[i] = 'A';
                        }
                        Letras.Add('A');
                    }
                    else 
                    {
                        for (int i = 0; i < Letras.Count; i++)
                        {
                            Letras[i]++;
                        }
                    }
                }
                else
                {
                    nervio.Cambio_Nombre(Contador.ToString());
                    Contador++;
                }
            }
        }
        public static cTendencia CrearTendenciaDefault(int IDTendencia, cTendencia_Refuerzo Tendencia_Refuerzo_Origen, eUbicacionRefuerzo Ubicacion)
        {
            cTendencia Tendencia = new cTendencia(IDTendencia, Tendencia_Refuerzo_Origen);
            Tendencia.BarrasAEmplearBase = new List<eNoBarra>() { eNoBarra.B3, eNoBarra.B4, eNoBarra.B5, eNoBarra.B6 };
            Tendencia.BarrasAEmplearAdicional = new List<eNoBarra>() { eNoBarra.B2, eNoBarra.B3, eNoBarra.B4, eNoBarra.B5, eNoBarra.B6 };
            Tendencia.UbicacionRefuerzo = Ubicacion;
            Tendencia.CuantiaMinima = Ubicacion == eUbicacionRefuerzo.Inferior ? cVariables.CuantiaMinimaInferior : cVariables.CuantiaMinimaSuperior;
            Tendencia.MinimaLongitud = cVariables.MinimaLongitud;
            Tendencia.MaximaLongitud = cVariables.MaximaLongitud;
            Tendencia.DeltaAlargamientoBarras = cVariables.DeltaAlargamitoBarras;
            return Tendencia;
        }

        public static cTendencia_Estribo CrearTendenciaEstriboDefault(int IDTendencia, cTendencia_Refuerzo Tendencia_Refuerzo_Origen)
        {
            cTendencia_Estribo tendencia_Estribo = new cTendencia_Estribo(IDTendencia, Tendencia_Refuerzo_Origen);
            tendencia_Estribo.BarrasAEmplear= new List<eNoBarra>() { eNoBarra.B2, eNoBarra.B3, eNoBarra.B4, eNoBarra.B5, eNoBarra.B6 };
            return tendencia_Estribo;

        }

        #endregion
        public static bool IsPuntoInSeccion(IElemento Elementos, PointF Punto, bool Reales = true)
        {
            GraphicsPath Path = new GraphicsPath();

            if (Reales)
                Path.AddPolygon(Elementos.Vistas.Perfil_AutoCAD.Reales.ToArray());
            else
                Path.AddPolygon(Elementos.Vistas.Perfil_AutoCAD.Escaladas.ToArray());


            if (Path.PathPoints.Min(x => x.X) <= Punto.X && Punto.X <= Path.PathPoints.Max(x => x.X))
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
            return CrearBarra(0, TendenciaOrigen, eNoBarra.B3, UbicacionRefuerzo, 1, xi, xf);
        }
        public static cBarra CrearBarra(int ID, cTendencia TendenciaOrigen, eNoBarra NoBarra, eUbicacionRefuerzo UbicacionRefuerzo, int CantBarra, float xi, float xf)
        {
            return new cBarra(ID, TendenciaOrigen, NoBarra, UbicacionRefuerzo, CantBarra, xi, xf);

        }

        public static cBloqueEstribos CrearGrupoEstribosDefault(cTendencia_Estribo tendencia_Estribo_origien)
        {
            var nervio = tendencia_Estribo_origien.Tendencia_Refuerzo_Origen.NervioOrigen;
            var SubtramoI = nervio.Lista_Elementos.First(y => y is cSubTramo) as cSubTramo;
            float d = SubtramoI.Seccion.H - nervio.r1 - cDiccionarios.DiametrosBarras[eNoBarra.B2] / 2f - cVariables.DiametroEstriboPredeterminado;
            float Smin = (float)Math.Round( d / 2f* cConversiones.Dimension_cm_to_m,cVariables.CifrasDeciSepEstribos);
            float xcentro = SubtramoI.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) + cVariables.d_CaraApoyo;
            return  new cBloqueEstribos(0, eNoBarra.B2, 3, Smin, 1, xcentro,eLadoDeZona.Derecha, tendencia_Estribo_origien);
        }

        public static void ReasingarEjesaNervios(cNervio Nervio, List<cGrid> Lista_Grids)
        {
            float XYo, XYi;
            if (Nervio.Direccion == eDireccion.Horizontal || Nervio.Direccion == eDireccion.Diagonal)
            {
                XYo = Nervio.Lista_Objetos[0].Line.ConfigLinea.Point1P.X;
                XYi = Nervio.Lista_Objetos[Nervio.Lista_Objetos.Count - 1].Line.ConfigLinea.Point2P.X;
            }
            else
            {
                XYo = Nervio.Lista_Objetos[0].Line.ConfigLinea.Point1P.Y;
                XYi = Nervio.Lista_Objetos[Nervio.Lista_Objetos.Count - 1].Line.ConfigLinea.Point2P.Y;
            }
            Nervio.Grids = DeepClone(FindGridPertencientesalNervio(XYo, XYi, Nervio.Direccion, Lista_Grids));
        }




        #region Librerias Weifo Luo

        public static void CambiarSkins(DockContent dock)
        {
            if (dock != null && dock.DockPanel != null)
            {
                dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = SystemColors.ControlLight;
                dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = SystemColors.GrayText;
                dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = SystemColors.ControlLight;
                dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = SystemColors.ControlLight;
                dock.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.TextColor = SystemColors.ControlLightLight;
            }
        }
        public static void CambiarSkins(DockPanel DockPanel)
        {
            if (DockPanel != null)
            {
                DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = SystemColors.ControlLight;
                DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = SystemColors.GrayText;
                DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = SystemColors.ControlLight;
                DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = SystemColors.ControlLight;
                DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.TextColor = SystemColors.ControlLightLight;
            }
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

        public static void SeleccionDataGridView(DataGridView data, int ColumnaAValidar, int CeldaHabilar = -1)
        {
            bool TodasSonSelect = false;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if ((bool)data.Rows[i].Cells[ColumnaAValidar].Value == true)
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
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    data.Rows[i].Cells[ColumnaAValidar].Value = true;
                }
            }

            if (TodasSonSelect)
            {
                if (data.Rows.Count != 0)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        data.Rows[i].Cells[ColumnaAValidar].Value = false;
                    }
                }
            }
            if (data.Rows.Count != 0 && CeldaHabilar < data.Rows.Count && CeldaHabilar != -1)
            {

                data.Rows[CeldaHabilar].Cells[ColumnaAValidar].Value = true;
            }


        }
        public static void EstiloDatGridView(DataGridView DataGrid, bool AsignarEstiloRows = true, bool AsignarEstiloColumns = true)
        {
            DataGridViewCellStyle StyleC = new DataGridViewCellStyle();
            StyleC.Alignment = DataGridViewContentAlignment.MiddleCenter;
            StyleC.Font = new Font("Vderdana", 8, FontStyle.Bold);

            DataGridViewCellStyle StyleR = new DataGridViewCellStyle();
            StyleR.Alignment = DataGridViewContentAlignment.MiddleCenter;
            StyleR.Font = new Font("Vderdana", 8, FontStyle.Regular);
            if (AsignarEstiloColumns)
            {
                foreach (DataGridViewColumn column in DataGrid.Columns)
                {
                    column.HeaderCell.Style = StyleC;
                }
            }
            if (AsignarEstiloRows)
            {
                foreach (DataGridViewRow row in DataGrid.Rows)
                {
                    row.DefaultCellStyle = StyleR;
                }
            }
        }

        public static void AgregarEstiloRow(DataGridViewRow row)
        {
            DataGridViewCellStyle StyleR = new DataGridViewCellStyle();
            StyleR.Alignment = DataGridViewContentAlignment.MiddleCenter;
            StyleR.Font = new Font("Vderdana", 8, FontStyle.Regular);
            row.DefaultCellStyle = StyleR;
        }
        #endregion WindowsForms
        //Serializar y Deserializar

        public static void Serializar(string Ruta, object Objeto)
        {
            Notificador?.Invoke("Guardando...");
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Ruta, FileMode.Create, FileAccess.Write, FileShare.None);
                if (Objeto is cProyecto)
                    ((cProyecto)Objeto).VersionPrograma = Program.Version;
                formatter.Serialize(stream, Objeto);
                stream.Close();
            }
            catch (Exception ex) { EventoVentanaEmergente?.Invoke(ex.Message, MessageBoxIcon.Exclamation); }
            Notificador?.Invoke("Listo");
        }


        public static void Deserealizar<T>(string Ruta, ref T Objeto)
        {
            Notificador?.Invoke("Cargando...");
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream streamReader = new FileStream(Ruta, FileMode.Open, FileAccess.Read, FileShare.None);
                var proyectoDeserializado = (T)formatter.Deserialize(streamReader);
                Objeto = proyectoDeserializado;
                cProyecto proyecto = Objeto as cProyecto;
                if (proyecto != null && proyecto.VersionPrograma < 1.05f)
                    ActualizarEstribosVersionesAnteriores(proyecto);
                if(proyecto!=null && proyecto.VersionPrograma<= 1.1f)
                    ActualizarTendenciasEnNervios(proyecto);
                streamReader.Close();
            }
            catch (Exception ex) { EventoVentanaEmergente?.Invoke(ex.Message, MessageBoxIcon.Exclamation); }
            Notificador?.Invoke("Listo");
        }

        #region Metodos Para Calculos
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
        /// en Cm
        /// </summary>
        /// <param name="Barras"></param>
        /// <param name="UR"></param>
        /// <param name="Nervio"></param>
        /// <returns></returns>
        public static float CalcularDCentroide(List<cBarra> Barras, eUbicacionRefuerzo UR, cNervio Nervio)
        {
            Barras = Barras.OrderBy(x => x.Nivel).ToList();
            float AreaT = Barras.Sum(y => y.AreaTotal);
            float R = UR == eUbicacionRefuerzo.Inferior ? Nervio.r2 : Nervio.r1;
            R += cVariables.DiametroEstriboPredeterminado;
            float SumD = 0; float SUMFC = 0; //Factor = Area * Centroide De cada barra
            Barras.ForEach(Barra =>
            {
                float d = SumD + R + cDiccionarios.DiametrosBarras[Barra.NoBarra] / 2;
                SumD += cDiccionarios.DiametrosBarras[Barra.NoBarra];
                SUMFC += Barra.AreaTotal * d;
            });
            return SUMFC / AreaT;
        }


        /// <summary>
        /// Determina la Longitud de una Polilinea
        /// </summary>
        /// <param name="Puntos">Lista de Puntos</param>
        /// <returns></returns>
        public static float Long(List<PointF> Puntos)
        {
            float Longitud = 0;
            for (int i = 0; i < Puntos.Count; i++)
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

        public static float PrecisarNumero(float Numero, float Precision, int CantDecimales, bool RedondearMas = false)
        {
            if (RedondearMas)
            {
                if (Numero < 0)
                {
                    Precision = -Precision;
                }
                float Numero1 = (float)Math.Round(Numero, CantDecimales);
                if (Math.Abs(Numero1 / Precision) >= 1)
                {
                    if (Numero1 % Precision == 0) return Numero1;
                    return Precision - Numero1 % Precision + Numero1;
                }
                else
                {
                    return Precision;
                }
            }
            else
            {
                if (Numero < 0)
                {
                    Precision = -Precision;
                }
                float Numero1 = (float)Math.Round(Numero, CantDecimales);

                if (Math.Abs(Numero1 / Precision) >= 1)
                {
                    if (Numero1 % Precision == 0) return Numero1;
                    return -Numero1 % Precision + Numero1;
                }
                else
                {
                    return Precision;
                }
            }
        }

        public static float DevolverResiduoRedondeado(float NumeroOriginal, float Precision, int CantDecimales)
        {
            float ReondearMas = PrecisarNumero(NumeroOriginal, Precision, CantDecimales, true);
            float RedondearMenos = PrecisarNumero(NumeroOriginal, Precision, CantDecimales);
            return Math.Abs(ReondearMas - NumeroOriginal) > Math.Abs(RedondearMenos - NumeroOriginal) ? RedondearMenos - NumeroOriginal : ReondearMas - NumeroOriginal;
        }

        public static cSubTramo ElementoASubtramo(IElemento elemento)
        {
            return (cSubTramo)elemento;

        }


        /// <summary>
        /// Devuleve float[] ----> float[] ---> Inferior{float[]{As,As,As}  , float[]{Mu,Mu,Mu} , float[]{Asr,Asr,Asr}}
        ///                        float[] -----> Superior{float[]{As,As}  , float[]{Mu,Mu} , float[]{Asr,Asr}} 
        /// </summary>
        /// <param name="Estaciones"></param>
        /// <param name="Ubicacion"></param>
        /// <returns></returns>
        public static object[][][] EstacionesResumidas(List<cEstacion> Estaciones)
        {

            int DeltaE = Estaciones.Count / 3;
            if (DeltaE == 0)
                DeltaE = 1;
            float As0, As1, As2;
            float Mu0, Mu1, Mu2;
            float Asr0, Asr1, Asr2;
            List<float> As = new List<float>(); List<float> Mu = new List<float>(); List<float> Asr = new List<float>();


            As0 = Estaciones.First().Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento;
            Mu0 = Estaciones.First().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Momento;
            Asr0 = Estaciones.First().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento;

            for (int i = DeltaE; i < DeltaE * 2; i++)
            {
                As.Add(Estaciones[i].Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento);
                Mu.Add(Estaciones[i].Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Momento);
                Asr.Add(Estaciones[i].Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento);
            }
            As1 = As.Max(); Mu1 = Mu.Max(); Asr1 = Asr.Max();

            As2 = Estaciones.Last().Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento;
            Mu2 = Estaciones.Last().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Momento;
            Asr2 = Estaciones.Last().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento;

            object[] AcerosSolicitados = new object[] { (float)Math.Round(As0, 2), (float)Math.Round(As1, 2), (float)Math.Round(As2, 2) };
            object[] MomentosSolicitados = new object[] { (float)Math.Round(Mu0, 2), (float)Math.Round(Mu1, 2), (float)Math.Round(Mu2, 2) };
            object[] AcerosRequerdios = new object[] { (float)Math.Round(Asr0, 2), (float)Math.Round(Asr1, 2), (float)Math.Round(Asr2, 2) };

            object[][] Resultados1 = new object[][] { AcerosSolicitados, MomentosSolicitados, AcerosRequerdios };



            As0 = Estaciones.First().Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento;
            Mu0 = Estaciones.First().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Momento;
            Asr0 = Estaciones.First().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento;

            As1 = Estaciones.Last().Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento;
            Mu1 = Estaciones.Last().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Momento;
            Asr1 = Estaciones.Last().Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento;


            object[] AcerosSolicitados2 = new object[] { (float)Math.Round(As0, 2), (float)Math.Round(As1, 2) };
            object[] MomentosSolicitados2 = new object[] { (float)Math.Round(Math.Abs(Mu0), 2), (float)Math.Round(Math.Abs(Mu1), 2) };
            object[] AcerosRequerdios2 = new object[] { (float)Math.Round(Asr0, 2), (float)Math.Round(Asr1, 2) };
            object[][] Resultados2 = new object[][] { AcerosSolicitados2, MomentosSolicitados2, AcerosRequerdios2 };


            float Vu1 = Estaciones.First().Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Cortante;
            float Vu2 = Estaciones.Last().Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Cortante;

            string EstribosI = "";
            string EstribosD = "";
            cTramo Tramo = Estaciones.First().SubTramoOrigen.TramoOrigen;
            var GrupoEstribos = Tramo.GrupoEstribosIzquierda_Derecha();

            if (GrupoEstribos.Item1.Count > 0)
            {
                EstribosI = $"{GrupoEstribos.Item1.First().Cantidad}E{ConvertireNoBarraToString(GrupoEstribos.Item1.First().NoBarra)}@{string.Format("{0:0.00}", GrupoEstribos.Item1.First().Separacion * cConversiones.Dimension_m_to_cm)}";
            }

            if (GrupoEstribos.Item2.Count > 0)
            {
                EstribosD = $"{GrupoEstribos.Item2.Last().Cantidad}E{ConvertireNoBarraToString(GrupoEstribos.Item2.Last().NoBarra)}@{string.Format("{0:0.00}", GrupoEstribos.Item2.Last().Separacion * cConversiones.Dimension_m_to_cm)}";
            }

            object[][] Resultados3 = new object[][] { new object[] { Math.Abs(Vu1), Math.Abs(Vu2) }, new object[] { EstribosI, EstribosD } };

            return new object[][][] { Resultados1, Resultados2, Resultados3 };

        }


        #endregion

        #region Metodos Paint
        public static RectangleF CrearCirculo(float Xc, float Yc, float Radio)
        {
            if (Radio < 4)
                Radio=4f;
            return new RectangleF(Xc - Radio, Yc - Radio, Radio * 2, Radio * 2);
        }
        public static RectangleF CrearCirculo(List<PointF> Puntos)
        {
            PointF P = Puntos.First(); PointF P2 = Puntos.Last();
            return CrearCirculo(P.X, P.Y, Math.Abs(P.X - P2.X));
        }

        public static void CerrarPoligonoParaMomentos(ref List<PointF> Puntos, List<PointF> PuntosSinEscalar, float YIgualCeroEscalado)
        {
            float YInicial = PuntosSinEscalar[0].Y;
            float YFinal = PuntosSinEscalar[PuntosSinEscalar.Count - 1].Y;
            float X, YInicialSinEscala, YFinalSinEscala;
            if (YInicial != 0)
            {
                YInicialSinEscala = Puntos[0].Y;
                X = Puntos.Find(x => x.Y == YInicialSinEscala).X;
                Puntos.Insert(0, new PointF(X, YIgualCeroEscalado));
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
        public static float Dist(PointF Punto1_Recta, PointF Punto2_Recta, PointF Punto)
        {
            float m = (Punto1_Recta.Y - Punto2_Recta.Y) / (Punto1_Recta.X - Punto2_Recta.X);
            float Numerador = Math.Abs(-m * Punto.X + Punto.Y + m * Punto1_Recta.X - Punto1_Recta.Y);
            float Denominador = (float)Math.Sqrt(Math.Pow(-m, 2) + Math.Pow(1, 2));
            return Numerador / Denominador;

        }

        #endregion

        #region DiseñarNervio
        public static void DiseñarNervioFlexion(cNervio Nervio)
        {

            #region Diseño A Flexión
            Nervio.Tendencia_Refuerzos.TInfeSelect.EliminarBarras();
            Nervio.Tendencia_Refuerzos.TSupeSelect.EliminarBarras();
            #region Refuerzo Inferior

            #region Refuerzo Base
            List<float> AsminSubTramo = new List<float>();

            Nervio.Lista_Elementos.ForEach((x =>
            {
                if (x is cSubTramo)
                {
                    cSubTramo subtramo = (cSubTramo)x;
                    float AminAx = subtramo.Seccion.B * subtramo.Seccion.H * Nervio.Tendencia_Refuerzos.TInfeSelect.CuantiaMinima;
                    AsminSubTramo.Add(AminAx);
                }

            }));
            float Asmin = AsminSubTramo.Min(); //Barra del Refuerzo Base Inferior

            var ListaElementos = CrearListaElementos(Nervio, RCondicion2: true, RCondicion3: true, RCondicion4: true);


            eNoBarra BarraPropuesta = ProponerUnaBarra(Asmin, Nervio.Tendencia_Refuerzos.TInfeSelect.BarrasAEmplearBase, Nervio);
            List<cPuntoInfelxion> puntoInfelxions = PuntosInfelxions(Nervio.Lista_Tramos, eUbicacionRefuerzo.Inferior);
            if (BarraPropuesta!= eNoBarra.BNone)
            {
                foreach (List<IElemento> elementos in ListaElementos)
                {
                    if (elementos.First(x => x is cSubTramo).Seccion.B > 12 && Nervio.CambioenAltura == eCambioenAltura.Inferior || Nervio.NervioBorde)
                    {
                        float AminAux = elementos.First(y => y is cSubTramo).Seccion.B * elementos.First(y => y is cSubTramo).Seccion.H * Nervio.Tendencia_Refuerzos.TInfeSelect.CuantiaMinima;
                        eNoBarra BarraP2 = ProponerUnaBarra(AminAux / 2f, Nervio.Tendencia_Refuerzos.TInfeSelect.BarrasAEmplearBase, Nervio);
                        CrearBarraContinuaInferior(elementos, 2, BarraP2, Nervio.Tendencia_Refuerzos.TInfeSelect, puntoInfelxions);
                    }
                    else
                    {
                        CrearBarraContinuaInferior(elementos, 1, BarraPropuesta, Nervio.Tendencia_Refuerzos.TInfeSelect, puntoInfelxions);
                    }
                }
            }

            //Elementos Con Cambios en B
            var ListaElementos1 = CrearListaElementos(Nervio, RCondicion1: true, RCondicion3: true, RCondicion4: true);
            foreach (List<IElemento> elementos in ListaElementos1)
            {
                if (elementos.First(x => x is cSubTramo).Seccion.B > 12f || Nervio.NervioBorde)
                {
                    cBarra BarraEncontrada = Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.Find(x => x.IsVisible(elementos.First(y => y is cSubTramo)));

                    if (BarraEncontrada != null && BarraEncontrada.CantBarra == 1)
                    {
                        float AminAx = elementos.First(y => y is cSubTramo).Seccion.B * elementos.First(y => y is cSubTramo).Seccion.H * Nervio.Tendencia_Refuerzos.TInfeSelect.CuantiaMinima;
                        float AsFaltante = AminAx - BarraEncontrada.AreaTotal;
                        eNoBarra BarraFinal = BarraPropuesta;
                        if (AsFaltante > 0)
                        {
                            BarraFinal = ProponerUnaBarra(AsFaltante, Nervio.Tendencia_Refuerzos.TInfeSelect.BarrasAEmplearBase, Nervio);
                        }
                        else
                        {
                            BarraFinal = Nervio.Tendencia_Refuerzos.TInfeSelect.BarrasAEmplearBase.Min();
                            if (ListaElementos1.Count == 1)
                            {
                                BarraEncontrada.NoBarra = Nervio.Tendencia_Refuerzos.TInfeSelect.BarrasAEmplearBase.Min();
                                float AceroPor2 = BarraEncontrada.AreaTotal * 2f;
                                float Porcentaje = (AceroPor2 - AminAx) / AceroPor2 * 100f;
                                if (AceroPor2 >= AminAx | Math.Abs(Porcentaje) <= cVariables.ToleranciaFlexionBarras)
                                {
                                    BarraEncontrada.CantBarra = 2;
                                    break;
                                }
                                else
                                {

                                    CrearBarraContinuaInferior(elementos, 1, eNoBarra.B4, Nervio.Tendencia_Refuerzos.TInfeSelect, puntoInfelxions);
                                    break;
                                }

                            }
                        }
                        if (BarraPropuesta == BarraFinal && Nervio.CambioenAltura == eCambioenAltura.Inferior)
                        {
                            BarraEncontrada.CantBarra = 2;
                        }
                        else
                        {
                            CrearBarraContinuaInferior(elementos, 1, BarraFinal, Nervio.Tendencia_Refuerzos.TInfeSelect, puntoInfelxions);
                        }
                    }
                }
            }

            AlargamientoAmbosLadosLds(ListaElementos, Nervio.Tendencia_Refuerzos.TInfeSelect);
            #endregion

            AgegarBarrasAdicionalesRefuerzoInferior(Nervio.Lista_Tramos, Nervio.Tendencia_Refuerzos.TInfeSelect, Nervio);

            #endregion
            #region Refuerzo Superior

            AgregarBarrasRefuerzoSuperior(Nervio, Nervio.Tendencia_Refuerzos.TSupeSelect);

            
            VerificarTraslapoMinimoBarras(Nervio.Tendencia_Refuerzos.TInfeSelect.Barras);
            VerificarTraslapoMinimoBarras(Nervio.Tendencia_Refuerzos.TSupeSelect.Barras);

            #endregion
            #endregion


        }


        private static void VerificarTraslapoMinimoBarras(List<cBarra> Barras)
        {
            Barras = Barras.OrderBy(y => y.XI).ToList();
            Barras.ForEach(y =>  CompararBarras(y));
            
            void CompararBarras(cBarra barra, int Contador=0)
            {
                if (barra.TraslapoIzquierda && Contador<10)
                {
                    cBarra BarraFind = Barras.Find(y => y != barra && y.Nivel == barra.Nivel && cTendencia.XiPerteneceaX0X1(y.XI, y.XF, barra.XI));

                    if (BarraFind != null)
                    {
                        float traslapo = cTendencia.DeterminarLongBarraRecta(barra.XI, BarraFind.XF);

                        if (traslapo < cVariables.TraslapoNervio)
                        {
                            barra.XI -= 0.01f;
                            Contador++;
                            //BarraFind.XF += 0.01f;
                            CompararBarras(barra,Contador);

                        }
                    }

                }

                //if (barra.TraslpaoDerecha)
                //{
                //    cBarra BarraFind = Barras.Find(y => y != barra && y.Nivel == barra.Nivel && cTendencia.XiPerteneceaX0X1(barra.XI, barra.XF, y.XI));

                //    if (BarraFind != null)
                //    {
                //        float traslapo = cTendencia.DeterminarLongBarraRecta(BarraFind.XI, barra.XF);

                //        if (traslapo < cVariables.TraslapoNervio)
                //        {
                //            barra.XF += 0.01f;

                //            CompararBarras(barra);
                //        }
                //    }

                //}

            }

   
        }




        public static void DiseñarCorte(cNervio Nervio)
        {
            cTendencia_Estribo TES = Nervio.Tendencia_Refuerzos.TEstriboSelect;
            TES.EliminarBloquesEstribos();
            eNoBarra BarraE = TES.BarrasAEmplear.Min();
            CrearEstacionesTemporaneasTramos(Nervio.Lista_Tramos);
            Nervio.Lista_Tramos.ForEach(Tramo =>
            {

                List<cEstacion> EstacionesIzquierda = new List<cEstacion>();
                List<cEstacion> EstacionesDerecha = new List<cEstacion>();

                //Tramo.Estaciones = Tramo.Estaciones.OrderBy(y => y.CoordX + y.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)).ToList();
                foreach (var E in Tramo.Estaciones)
                {
                    float A = E.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S;
                    if (A != 0)
                    {
                        EstacionesIzquierda.Add(E);
                    }
                    else
                    {
                        break;
                    }
                }
                List<cEstacion> EstacionesReverse = Tramo.Estaciones;EstacionesReverse.Reverse();
                foreach (var E in EstacionesReverse)
                {
                    float A = E.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S;
                    if (A != 0)
                    {
                        EstacionesDerecha.Add(E);
                    }
                    else
                    {
                        break;
                    }
                }

                if(Tramo.Estaciones.Count> EstacionesIzquierda.Count + EstacionesDerecha.Count)
                {
                    AgregarEstribosSegunEstaciones(Nervio, TES,EstacionesIzquierda, eLadoDeZona.Izquierda, BarraE);
                    AgregarEstribosSegunEstaciones(Nervio, TES, EstacionesDerecha, eLadoDeZona.Derecha, BarraE);
                }
                else
                {
                    AgregarEstribosSegunEstaciones(Nervio, TES, EstacionesIzquierda, eLadoDeZona.Izquierda, BarraE);
                }

                AgregarEstribosMinimos(Tramo,Nervio,BarraE,TES);
                UnirZonaEstribos(Tramo, BarraE, TES);
                CrearEstribosEnVoladizos(Tramo,BarraE,TES, Nervio);
                CrearEstribosConinutos(Tramo, BarraE, TES, Nervio);
            });

        }

        private static void CrearEstribosConinutos(cTramo tramo, eNoBarra noBarra, cTendencia_Estribo TE, cNervio nervio)
        {
            if (nervio.NervioBorde || tramo.Lista_SubTramos.First().Seccion.B>cVariables.BNervioBorde)
            {
                bool Izq = tramo.IsVoladizoIzquierda(); bool Derecha = tramo.IsVoladizoDerecha();
                var Grupo = tramo.GrupoEstribosIzquierda_Derecha();
                var GruposTotal = Grupo.Item1.Concat(Grupo.Item2).ToList();

                float S = SdefinitivaZonas(nervio, tramo.Lista_SubTramos.SelectMany(y => y.Estaciones).ToList(), cDiccionarios.AceroBarras[noBarra]) * cConversiones.Dimension_cm_to_m;
                float L = tramo.Longitud - 2 * cVariables.d_CaraApoyo - cVariables.RExtremoIzquierdo;
                if (GruposTotal.Count > 0)
                {
                    float Saux = GruposTotal.Min(y => y.Separacion);
                    if (Saux < S)
                        S = Saux;
                }


                tramo.LimpiarEstribosEnTramo();
                eLadoDeZona LadoZona = Izq ? eLadoDeZona.Derecha : eLadoDeZona.Izquierda;
                CrearEstribos2(tramo.Lista_SubTramos.First().Estaciones.First(), L, LadoZona, S, TE, noBarra);
            }
        }


        private static void CrearEstribosEnVoladizos(cTramo tramo, eNoBarra noBarra,cTendencia_Estribo TE,cNervio nervio)
        {
            bool Izq = tramo.IsVoladizoIzquierda(); bool Derecha = tramo.IsVoladizoDerecha();
            var Grupo = tramo.GrupoEstribosIzquierda_Derecha();
            var GruposTotal = Grupo.Item1.Concat(Grupo.Item2).ToList();
            if ( Izq || Derecha )
            {
                float S = SdefinitivaZonas(nervio, tramo.Lista_SubTramos.SelectMany(y => y.Estaciones).ToList(), cDiccionarios.AceroBarras[noBarra])*cConversiones.Dimension_cm_to_m;
                float L = tramo.Longitud - 2*cVariables.d_CaraApoyo-cVariables.RExtremoIzquierdo;
                if (GruposTotal.Count > 0)
                {
                    float Saux = GruposTotal.Min(y => y.Separacion);
                    if (Saux < S)
                        S = Saux;
                }

                tramo.LimpiarEstribosEnTramo();
                eLadoDeZona LadoZona = Izq ? eLadoDeZona.Derecha : eLadoDeZona.Izquierda;
                CrearEstribos2(tramo.Lista_SubTramos.First().Estaciones.First(), L, LadoZona, S, TE, noBarra);
            }


        }
        private static void UnirZonaEstribos(cTramo Tramo, eNoBarra NoBarra, cTendencia_Estribo TE)
        {
            var estribosIzqDer = Tramo.GrupoEstribosIzquierda_Derecha();

            var estribosIzq = estribosIzqDer.Item1;
            var estribosDer = estribosIzqDer.Item2;
            if(estribosIzq.Count>0 && estribosDer.Count > 0)
            {
                var bloqueEstribosZona1Izq = estribosIzq.First();
                var bloqueEstribosZona2Izq = estribosIzq.Last();
                var bloqeEstribosZona1Dr = estribosDer.Last();
                var bloqueEstribosZona2Dr = estribosDer.First();

                if(bloqueEstribosZona2Izq.XF>= bloqueEstribosZona2Dr.XI)
                {
                    float Smin = estribosIzq.Concat(estribosDer).ToList().Min(y => y.Separacion);
                    float L = Tramo.Longitud - 2 * cVariables.d_CaraApoyo;
                    Tramo.LimpiarEstribosEnTramo();

                    CrearEstribos2(Tramo.Lista_SubTramos.First().Estaciones.First(), L, eLadoDeZona.Izquierda, Smin, TE, NoBarra);


                }
            }


        }
        private static void AgregarEstribosMinimos(cTramo tramo,cNervio Nervio, eNoBarra NoBarra,cTendencia_Estribo TE)
        {
            var BloqueID = tramo.GrupoEstribosIzquierda_Derecha();
            var EsI = BloqueID.Item1;
            var EsD = BloqueID.Item2;
            int CantidadEstribosIz = EsI.Sum(y => y.Cantidad);
            int CantidadEstribosDr = EsD.Sum(y => y.Cantidad);
            if (!tramo.IsVoladizoIzquierda())
            {
                cSubTramo cSubTramo = tramo.Lista_SubTramos.First();
                int CantidadMinima=MinEstribosSegunEfePrimaCe(CantidadEstribosIz, tramo);

                if(CantidadEstribosIz!=0 &&  CantidadMinima!= CantidadEstribosIz)
                {
                    EsI.Last().Cantidad += CantidadMinima - CantidadEstribosIz;
                }
                else if (CantidadEstribosIz == 0)
                {
                    float d = cSubTramo.Seccion.H - Nervio.r1 - cDiccionarios.DiametrosBarras[NoBarra] / 2f - cVariables.DiametroEstriboPredeterminado;
                    float Smin = d / 2f * cConversiones.Dimension_cm_to_m;float LongZona = Smin * (CantidadMinima - 1);
                    CrearEstribos2(cSubTramo.Estaciones.First(), LongZona, eLadoDeZona.Izquierda, Smin, TE, NoBarra);
                }
            }
            if (!tramo.IsVoladizoDerecha())
            {
                cSubTramo cSubTramo = tramo.Lista_SubTramos.Last();
                int CantidadMinima = MinEstribosSegunEfePrimaCe(CantidadEstribosDr, tramo);

                if (CantidadEstribosDr != 0 && CantidadMinima != CantidadEstribosDr)
                {
                    EsD.First().Cantidad += CantidadMinima - CantidadEstribosDr;
                }
                else if (CantidadEstribosDr == 0)
                {
                    float d = cSubTramo.Seccion.H - Nervio.r1 - cDiccionarios.DiametrosBarras[NoBarra] / 2f - cVariables.DiametroEstriboPredeterminado;
                    float Smin = d / 2f * cConversiones.Dimension_cm_to_m; float LongZona = Smin * (CantidadMinima - 1);
                    CrearEstribos2(cSubTramo.Estaciones.Last(), LongZona, eLadoDeZona.Derecha, Smin, TE, NoBarra);
                }
            }
            

        }

        private static void AgregarEstribosSegunEstaciones(cNervio nervio, cTendencia_Estribo tendencia_Estribo,List<cEstacion> estaciones, eLadoDeZona LadoZona, eNoBarra NoBarraE)
        {
            List<cEstacion> EstacionesMayoresAMin = estaciones.FindAll(y => y.Calculos.IsAreasCortanteMayoresAmin());
            List<cEstacion> EstacionesIgualesAmin = estaciones.FindAll(y => !y.Calculos.IsAreasCortanteMayoresAmin());

            float AreaEstribo = cDiccionarios.AceroBarras[NoBarraE];
            float SMayoresAMin = cVariables.ValueNull; float LongMayoresAMin = cVariables.ValueNull;
            float SIgualesAMin = cVariables.ValueNull; float LongIgualesAMin = cVariables.ValueNull;

            if (EstacionesMayoresAMin.Count > 0)
            {
                float xi = EstacionesMayoresAMin.First().CoordX + EstacionesMayoresAMin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
                float xf = EstacionesMayoresAMin.Last().CoordX + EstacionesMayoresAMin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
                LongMayoresAMin = Math.Abs(xf - xi);
                SMayoresAMin = SdefinitivaZonas(nervio, EstacionesMayoresAMin, AreaEstribo) * cConversiones.Dimension_cm_to_m;
                
            }
            if (EstacionesIgualesAmin.Count > 0)
            {
                float xi = EstacionesIgualesAmin.First().CoordX + EstacionesIgualesAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
                float xf = EstacionesIgualesAmin.Last().CoordX + EstacionesIgualesAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
                LongIgualesAMin = Math.Abs(xf - xi);
                SIgualesAMin = SdefinitivaZonas(nervio, EstacionesIgualesAmin, AreaEstribo) * cConversiones.Dimension_cm_to_m;
            }

            if(SMayoresAMin!= cVariables.ValueNull && SIgualesAMin== cVariables.ValueNull) 
            {

                CrearEstribos2(EstacionesMayoresAMin.First(), LongMayoresAMin, LadoZona, SMayoresAMin, tendencia_Estribo, NoBarraE);
            }
            else if(SMayoresAMin == SIgualesAMin && LongMayoresAMin != cVariables.ValueNull && LongIgualesAMin != cVariables.ValueNull)
            {
                float L = LongMayoresAMin + LongIgualesAMin;
                cTramo Tramo = EstacionesMayoresAMin.First().SubTramoOrigen.TramoOrigen;

                if(LongMayoresAMin + LongIgualesAMin> Tramo.Longitud)
                {
                    L = Tramo.Longitud - 2 * cVariables.d_CaraApoyo;
                }
                CrearEstribos2(EstacionesMayoresAMin.First(), L, LadoZona, SMayoresAMin, tendencia_Estribo, NoBarraE);
            }
            else if (SMayoresAMin != SIgualesAMin && LongMayoresAMin != cVariables.ValueNull && LongIgualesAMin != cVariables.ValueNull)
            {
                CrearEstribos2(EstacionesMayoresAMin.First(), LongMayoresAMin, LadoZona, SMayoresAMin, tendencia_Estribo, NoBarraE);
                CrearEstribos2(EstacionesIgualesAmin.First(), LongIgualesAMin, LadoZona, SIgualesAMin, tendencia_Estribo, NoBarraE);

            }
            else if(LongIgualesAMin != cVariables.ValueNull && LongMayoresAMin == cVariables.ValueNull)
            {
                CrearEstribos2(EstacionesIgualesAmin.First(), LongIgualesAMin, LadoZona, SIgualesAMin, tendencia_Estribo, NoBarraE);
            }

        }

        private static void CrearEstribos2(cEstacion estacion, float LongitudZona, eLadoDeZona LadoZona, float S, cTendencia_Estribo tendenciaE, eNoBarra noBarra)
        {
            cNervio nervio = estacion.SubTramoOrigen.TramoOrigen.NervioOrigen;
            float X = LadoZona == eLadoDeZona.Izquierda ? estacion.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) + cVariables.d_CaraApoyo :
                                                          estacion.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X) - cVariables.d_CaraApoyo;
            int CantidadEspacios = (int)PrecisarNumero(LongitudZona / S, 1, 1, true);
            int CantidadEstribos = CantidadEspacios + 1;
            if (CantidadEspacios == 1)
                CantidadEstribos = CantidadEspacios;
            eLadoDeZona DireccionEstribo = LadoZona == eLadoDeZona.Izquierda ? eLadoDeZona.Derecha : eLadoDeZona.Izquierda;
            bool CrearEstribo=CrearBloqueEstribos(tendenciaE, S, CantidadEstribos, noBarra, 1, X, DireccionEstribo);
            if (CrearEstribo)
            {
                cBloqueEstribos bloquelast = tendenciaE.BloqueEstribos.Last(y => y.ID == tendenciaE.BloqueEstribos.Max(z => z.ID));
                float CoorUltEstribo = bloquelast.DireccionEstribo== eLadoDeZona.Derecha?  bloquelast.ListaEstribos.Last().Coordenadas.Reales.First().X:
                                       bloquelast.ListaEstribos.First().Coordenadas.Reales.First().X;
                IElemento elemento = nervio.Lista_Elementos.Find(y => y.IsVisibleCoordAutoCAD(CoorUltEstribo));
                if (elemento is cApoyo)
                {
                    float d_caraApoyo = cVariables.d_CaraApoyo;
                    float Xmodificar = bloquelast.DireccionEstribo == eLadoDeZona.Derecha?  elemento.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) - d_caraApoyo:
                                                                                            elemento.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X) + d_caraApoyo; 

                    float XpenultimoEstribo = cVariables.ValueNull;
                    if (bloquelast.ListaEstribos.Count - 2 > 0)
                    {
                        XpenultimoEstribo = bloquelast.ListaEstribos[bloquelast.ListaEstribos.Count - 2].Coordenadas.Reales.First().X;
                    }
                    if (XpenultimoEstribo != cVariables.ValueNull && Math.Abs(Xmodificar - XpenultimoEstribo) > 0.06f)
                    {
                        bloquelast.CoordXEstriboEstriboFinal = Xmodificar;
                    }
                    else if (XpenultimoEstribo != cVariables.ValueNull)
                    {
                        bloquelast.Cantidad -= 1;
                    }
                }
            }
        }



        private static bool CrearBloqueEstribos(cTendencia_Estribo tendenciaE, float S, int Cantidad, eNoBarra noBarra, int NoRamas, float x, eLadoDeZona DireccionEstribo)
        {
            cNervio nervio = tendenciaE.Tendencia_Refuerzo_Origen.NervioOrigen;
            float LimiteNervioIzq = nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.Min(y=>y.X);
            float LimiteNervioDr = nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Max(y => y.X);
            int IDMax = 0;
            if (tendenciaE.BloqueEstribos.Count > 0)
            {
                IDMax = tendenciaE.BloqueEstribos.Max(y => y.ID) + 1;
            }

            if ((DireccionEstribo == eLadoDeZona.Derecha && x + S * (Cantidad-1) < LimiteNervioDr) ||
                (DireccionEstribo == eLadoDeZona.Izquierda && x - S * (Cantidad-1) > LimiteNervioIzq))
            {

                if (Cantidad > 0)
                {
                    tendenciaE.AgregarBloqueEstribos(new cBloqueEstribos(IDMax, noBarra, Cantidad, S, NoRamas, x, DireccionEstribo, tendenciaE), true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Cantidad -= 1;
                if (Cantidad > 0)
                {
                    return CrearBloqueEstribos(tendenciaE, S, Cantidad, noBarra, NoRamas, x, DireccionEstribo);
                }
                else
                {
                    return false;
                }
            }

        }

        private static int MinEstribosSegunEfePrimaCe(int NoEstribos, cTramo Tramo)
        {
            int NoEstribos1 = NoEstribos;
            if (NoEstribos < 3)
                NoEstribos1 = 3;
            if (Tramo.Longitud >= 4f && NoEstribos1 <= 3)
            {
                int CantiMas = (int)Math.Round(Tramo.Longitud / 4f, 0);
                NoEstribos1 += CantiMas;
            }
            return NoEstribos1;
        }


        public static void DiseñarNervioCortante(cNervio Nervio)
        {
            #region Diseño a Cortante

            eNoBarra BarraE = eNoBarra.B2; float AreaEstribo = cDiccionarios.AceroBarras[BarraE];
            if (Nervio.Lista_Tramos.First().Estaciones == null) 
                CrearEstacionesTemporaneasTramos(Nervio.Lista_Tramos);
            Nervio.Lista_Tramos.ForEach(Tramo =>
            {
                Tramo.EstribosDerecha = null;
                Tramo.EstribosIzquierda = null;
                float DZ1_Izquierda = 0;
                float DZ2_Izquierda = 0;
                float DZ1_Derecha = 0;
                float DZ2_Derecha = 0;

                List<cEstacion> Es_Izquierda_MayoresaAmin = new List<cEstacion>();
                List<cEstacion> Es_Izquierda_IgualesaAmin = new List<cEstacion>();

                List<cEstacion> Es_Derecha_MayoresaAmin = new List<cEstacion>();
                List<cEstacion> Es_Derecha_IgualesaAmin = new List<cEstacion>();
                Tramo.Estaciones.ForEach(Estacion =>
                {

                    //Izquierda
                    float AreaS_Izquierda = Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S;
                    float AreaSMin_Izquierda = Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.AreaMin;

                    if (AreaS_Izquierda > AreaSMin_Izquierda)
                        Es_Izquierda_MayoresaAmin.Add(Estacion);
                    else if (AreaS_Izquierda == AreaSMin_Izquierda)
                        Es_Izquierda_IgualesaAmin.Add(Estacion);

                    //Derecha
                    float AreaS_Derecha = Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S;
                    float AreaSMin_Derecha = Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.AreaMin;
                    if (AreaS_Derecha > AreaSMin_Derecha)
                        Es_Derecha_MayoresaAmin.Add(Estacion);
                    else if (AreaS_Derecha == AreaSMin_Derecha)
                        Es_Derecha_IgualesaAmin.Add(Estacion);

                });


                if (Es_Izquierda_MayoresaAmin.Count > 0)
                {
                    DZ1_Izquierda = (Es_Izquierda_MayoresaAmin.Last().CoordX + Es_Izquierda_MayoresaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Es_Izquierda_MayoresaAmin.First().CoordX + Es_Izquierda_MayoresaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                }
                if (Es_Izquierda_IgualesaAmin.Count > 0)
                {
                    DZ2_Izquierda = (Es_Izquierda_IgualesaAmin.Last().CoordX + Es_Izquierda_IgualesaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Es_Izquierda_IgualesaAmin.First().CoordX + Es_Izquierda_IgualesaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                }
                if (Es_Derecha_MayoresaAmin.Count > 0)
                {
                    DZ1_Derecha = (Es_Derecha_MayoresaAmin.Last().CoordX + Es_Derecha_MayoresaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Es_Derecha_MayoresaAmin.First().CoordX + Es_Derecha_MayoresaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                }
                if (Es_Derecha_IgualesaAmin.Count > 0)
                {
                    DZ2_Derecha = (Es_Derecha_IgualesaAmin.Last().CoordX + Es_Derecha_IgualesaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Es_Derecha_IgualesaAmin.First().CoordX + Es_Derecha_IgualesaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                }

                float SDefinitivaZ1Izquierda = 0;
                float SDefinitivaZ2Izquierda = 0;
                float SDefinitivaZ1Derecha = 0;
                float SDefinitivaZ2Derecha = 0;

                #region Estribos Izquierda
                if (DZ1_Izquierda != 0)
                {
                    //SDefinitivaZ1Izquierda = SdefinitivaZonas(Nervio, Es_Izquierda_MayoresaAmin, AreaEstribo, eLadoDeZona.Izquierda);
                }
                if (DZ2_Izquierda != 0)
                {
                    //SDefinitivaZ2Izquierda = SdefinitivaZonas(Nervio, Es_Izquierda_IgualesaAmin, AreaEstribo, eLadoDeZona.Izquierda);
                }
                if (Es_Izquierda_IgualesaAmin.Count == 1)
                    SDefinitivaZ2Izquierda = SDefinitivaZ1Izquierda;

                if (SDefinitivaZ1Izquierda != 0 && SDefinitivaZ2Izquierda != 0 && SDefinitivaZ1Izquierda == SDefinitivaZ2Izquierda)
                {
                    Tramo.EstribosIzquierda = new cLadoDeEstribos(eLadoDeZona.Izquierda, Tramo);
                    float S = SDefinitivaZ1Izquierda * cConversiones.Dimension_cm_to_m;
                    List<cEstacion> Estaciones = new List<cEstacion>();
                    Estaciones.AddRange(Es_Izquierda_MayoresaAmin); Estaciones.AddRange(Es_Izquierda_IgualesaAmin);
                    float LongitudIzquierda = (Estaciones.Last().CoordX + Estaciones.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Estaciones.First().CoordX + Estaciones.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                    float DeltaX = Estaciones.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.d_CaraApoyo;
                    CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Izquierda, LongitudIzquierda, S, BarraE, DeltaX);

                }
                else if (SDefinitivaZ2Izquierda != 0 && SDefinitivaZ1Izquierda == 0)
                {
                    Tramo.EstribosIzquierda = new cLadoDeEstribos(eLadoDeZona.Izquierda, Tramo);
                    float S = SDefinitivaZ2Izquierda * cConversiones.Dimension_cm_to_m;
                    float Longitud = DZ2_Izquierda;
                    float DeltaX = Es_Izquierda_IgualesaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.d_CaraApoyo;
                    CrearEstribos(Tramo, eZonas.Zona2, eLadoDeZona.Izquierda, Longitud, S, BarraE, DeltaX);

                }
                else if (SDefinitivaZ1Izquierda != 0 && SDefinitivaZ2Izquierda != 0 && SDefinitivaZ1Izquierda != SDefinitivaZ2Izquierda)
                {
                    Tramo.EstribosIzquierda = new cLadoDeEstribos(eLadoDeZona.Izquierda, Tramo);

                    //Zona 1
                    float S1 = SDefinitivaZ1Izquierda * cConversiones.Dimension_cm_to_m;
                    float Longitud = DZ1_Izquierda;
                    float DeltaX = Es_Izquierda_MayoresaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.d_CaraApoyo;
                    CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Izquierda, Longitud, S1, BarraE, DeltaX);

                    //Zona 2
                    float S2 = SDefinitivaZ2Izquierda * cConversiones.Dimension_cm_to_m;
                    Longitud = DZ2_Izquierda;
                    DeltaX = Tramo.EstribosIzquierda.Zona1.Estribos.Last().CoordX + S2;
                    CrearEstribos(Tramo, eZonas.Zona2, eLadoDeZona.Izquierda, Longitud, S2, BarraE, DeltaX);


                    if (Math.Abs(SDefinitivaZ1Izquierda - SDefinitivaZ2Izquierda) <= cVariables.ToleranciaDistanciaEstribos)
                    {
                        Tramo.EstribosIzquierda.Zona2.EliminarEstribos(true);
                        List<cEstacion> Estaciones = new List<cEstacion>();
                        Estaciones.AddRange(Es_Izquierda_MayoresaAmin); Estaciones.AddRange(Es_Izquierda_IgualesaAmin);
                        float LongitudIzquierda = (Estaciones.Last().CoordX + Estaciones.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Estaciones.First().CoordX + Estaciones.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                        float S = SDefinitivaZ1Izquierda * cConversiones.Dimension_cm_to_m;
                        DeltaX = Es_Izquierda_MayoresaAmin.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.d_CaraApoyo;
                        CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Izquierda, LongitudIzquierda, S, BarraE, DeltaX);
                    }

                }

                #endregion

                #region Estribos Derecha
                if (DZ1_Derecha != 0)
                {
                    //SDefinitivaZ1Derecha = SdefinitivaZonas(Nervio, Es_Derecha_MayoresaAmin, AreaEstribo, eLadoDeZona.Derecha);
                }
                if (DZ2_Derecha != 0)
                {
                    //SDefinitivaZ2Derecha = SdefinitivaZonas(Nervio, Es_Derecha_IgualesaAmin, AreaEstribo, eLadoDeZona.Derecha);
                }
                if (Es_Derecha_IgualesaAmin.Count == 1)
                    SDefinitivaZ2Derecha = SDefinitivaZ1Derecha;
                if (SDefinitivaZ1Derecha != 0 && SDefinitivaZ2Derecha != 0 && SDefinitivaZ1Derecha == SDefinitivaZ2Derecha)
                {
                    Tramo.EstribosDerecha = new cLadoDeEstribos(eLadoDeZona.Derecha, Tramo);
                    float S = SDefinitivaZ1Derecha * cConversiones.Dimension_cm_to_m;
                    List<cEstacion> Estaciones = new List<cEstacion>();
                    Estaciones.AddRange(Es_Derecha_IgualesaAmin); Estaciones.AddRange(Es_Derecha_MayoresaAmin);
                    float LongitudDerecha = (Estaciones.Last().CoordX + Estaciones.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Estaciones.First().CoordX + Estaciones.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                    float DeltaX = Estaciones.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.d_CaraApoyo;
                    CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Derecha, LongitudDerecha, S, BarraE, DeltaX);

                }
                else if (SDefinitivaZ2Derecha != 0 && SDefinitivaZ1Derecha == 0)
                {
                    Tramo.EstribosDerecha = new cLadoDeEstribos(eLadoDeZona.Derecha, Tramo);
                    float S = SDefinitivaZ2Derecha * cConversiones.Dimension_cm_to_m;
                    float Longitud = DZ2_Derecha;
                    float DeltaX = Es_Derecha_IgualesaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.d_CaraApoyo;
                    CrearEstribos(Tramo, eZonas.Zona2, eLadoDeZona.Derecha, Longitud, S, BarraE, DeltaX);

                }
                else if (SDefinitivaZ1Derecha != 0 && SDefinitivaZ2Derecha != 0 && SDefinitivaZ1Derecha != SDefinitivaZ2Derecha)
                {
                    Tramo.EstribosDerecha = new cLadoDeEstribos(eLadoDeZona.Derecha, Tramo);

                    //Zona 1
                    float S1 = SDefinitivaZ1Derecha * cConversiones.Dimension_cm_to_m;
                    float Longitud = DZ1_Derecha;
                    float DeltaX = Es_Derecha_MayoresaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.d_CaraApoyo;
                    CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Derecha, Longitud, S1, BarraE, DeltaX);

                    //Zona 2
                    float S2 = SDefinitivaZ2Derecha * cConversiones.Dimension_cm_to_m;
                    Longitud = DZ2_Derecha;
                    DeltaX = Tramo.EstribosDerecha.Zona1.Estribos.Last().CoordX - S2;
                    CrearEstribos(Tramo, eZonas.Zona2, eLadoDeZona.Derecha, Longitud, S2, BarraE, DeltaX);


                    if (Math.Abs(SDefinitivaZ1Derecha - SDefinitivaZ2Derecha) <= cVariables.ToleranciaDistanciaEstribos)
                    {
                        Tramo.EstribosDerecha.Zona2.EliminarEstribos(true);
                        List<cEstacion> Estaciones = new List<cEstacion>();
                        Estaciones.AddRange(Es_Derecha_IgualesaAmin); Estaciones.AddRange(Es_Derecha_MayoresaAmin);
                        float LongitudDerecha = (Estaciones.Last().CoordX + Estaciones.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X)) - (Estaciones.First().CoordX + Estaciones.First().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X));
                        float S = SDefinitivaZ1Derecha * cConversiones.Dimension_cm_to_m;
                        DeltaX = Es_Derecha_MayoresaAmin.Last().SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.d_CaraApoyo;
                        CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Derecha, LongitudDerecha, S, BarraE, DeltaX);
                    }

                }

                #endregion

                EditarZonasEstribosCantidadMinima(Tramo.EstribosDerecha, Tramo);
                EditarZonasEstribosCantidadMinima(Tramo.EstribosIzquierda, Tramo);


            });

           
            // Estribos en Voladizos

            //Voladizo Izquierda
            if (Nervio.Lista_Elementos.First() is cSubTramo)
            {
                cSubTramo Subtramo = (cSubTramo)Nervio.Lista_Elementos.First();
                if (Subtramo.TramoOrigen.EstribosDerecha != null)
                {
                    float SZ1 = Subtramo.TramoOrigen.EstribosDerecha.Zona1.Separacion;
                    float SZ2 = Subtramo.TramoOrigen.EstribosDerecha.Zona2.Separacion;
                    if (SZ2 == 0)
                        SZ2 = 999f;
                    if (SZ1 == 0)
                        SZ1 = 999f;
                    float S = SZ1 < SZ2 ? SZ1 : SZ2;

                    eNoBarra noBarra = Subtramo.TramoOrigen.EstribosDerecha.Zona1.NoBarra > Subtramo.TramoOrigen.EstribosDerecha.Zona2.NoBarra
                        ? Subtramo.TramoOrigen.EstribosDerecha.Zona1.NoBarra
                        : Subtramo.TramoOrigen.EstribosDerecha.Zona2.NoBarra;
                    float Longitud = Subtramo.TramoOrigen.Longitud - Nervio.r1 * cConversiones.Dimension_cm_to_m;
                    float DeltaX = Subtramo.TramoOrigen.Lista_SubTramos.Max(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X)) - cVariables.d_CaraApoyo;
                    Subtramo.TramoOrigen.EstribosDerecha.Zona2.EliminarEstribos(true);
                    CrearEstribos(Subtramo.TramoOrigen, eZonas.Zona1, eLadoDeZona.Derecha, Longitud, S, noBarra, DeltaX);
                }
                else
                {
                    float S = 0;
                    Subtramo.TramoOrigen.EstribosDerecha = new cLadoDeEstribos(eLadoDeZona.Derecha, Subtramo.TramoOrigen);
                    eNoBarra noBarra = eNoBarra.B2;
                    //float S = SdefinitivaZonas(Nervio, Subtramo.Estaciones, cDiccionarios.AceroBarras[noBarra], eLadoDeZona.Derecha) * cConversiones.Dimension_cm_to_m;
                    float Longitud = Subtramo.TramoOrigen.Longitud - Nervio.r1 * cConversiones.Dimension_cm_to_m;
                    float DeltaX = Subtramo.TramoOrigen.Lista_SubTramos.Max(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X)) - cVariables.d_CaraApoyo;
                    CrearEstribos(Subtramo.TramoOrigen, eZonas.Zona1, eLadoDeZona.Derecha, Longitud, S, noBarra, DeltaX);
                }


            }


            //Voladizo Derecha
            if (Nervio.Lista_Elementos.Last() is cSubTramo)
            {
                cSubTramo Subtramo = (cSubTramo)Nervio.Lista_Elementos.Last();
                if (Subtramo.TramoOrigen.EstribosIzquierda != null)
                {
                    float SZ1 = Subtramo.TramoOrigen.EstribosIzquierda.Zona1.Separacion;
                    float SZ2 = Subtramo.TramoOrigen.EstribosIzquierda.Zona2.Separacion;
                    if (SZ2 == 0)
                        SZ2 = 999f;
                    if (SZ1 == 0)
                        SZ1 = 999f;
                    float S = SZ1 < SZ2 ? SZ1 : SZ2;

                    eNoBarra noBarra = Subtramo.TramoOrigen.EstribosIzquierda.Zona1.NoBarra > Subtramo.TramoOrigen.EstribosIzquierda.Zona2.NoBarra
                        ? Subtramo.TramoOrigen.EstribosIzquierda.Zona1.NoBarra
                        : Subtramo.TramoOrigen.EstribosIzquierda.Zona2.NoBarra;
                    float Longitud = Subtramo.TramoOrigen.Longitud - Nervio.r1 * cConversiones.Dimension_cm_to_m;
                    float DeltaX = Subtramo.TramoOrigen.Lista_SubTramos.Min(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X)) + cVariables.d_CaraApoyo;
                    Subtramo.TramoOrigen.EstribosIzquierda.Zona2.EliminarEstribos(true);
                    CrearEstribos(Subtramo.TramoOrigen, eZonas.Zona1, eLadoDeZona.Izquierda, Longitud, S, noBarra, DeltaX);
                }
                else
                {
                    Subtramo.TramoOrigen.EstribosIzquierda = new cLadoDeEstribos(eLadoDeZona.Izquierda, Subtramo.TramoOrigen);
                    eNoBarra noBarra = eNoBarra.B2;
                    //   float S = SdefinitivaZonas(Nervio, Subtramo.Estaciones, cDiccionarios.AceroBarras[noBarra], eLadoDeZona.Izquierda) * cConversiones.Dimension_cm_to_m;
                    float Longitud = Subtramo.TramoOrigen.Longitud - Nervio.r1 * cConversiones.Dimension_cm_to_m;
                    float DeltaX = Subtramo.TramoOrigen.Lista_SubTramos.Min(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X)) + cVariables.d_CaraApoyo;
                    float S = 0;
                    CrearEstribos(Subtramo.TramoOrigen, eZonas.Zona1, eLadoDeZona.Izquierda, Longitud, S, noBarra, DeltaX);
                }


            }

            //Estribos Faltantes --> Donde no existe Estribos Izquierda o Derecha

            Nervio.Lista_Tramos.ForEach(Tramo => CrearEstribosFaltantes(Tramo, BarraE));

            //Unir Zonas de Estribos
            Nervio.Lista_Tramos.ForEach(Tramo => Tramo.UnirZonaDeEstribos());

            Nervio.CrearAceroAsignadoRefuerzoTransversal();
            #endregion
        }



        #region Metodos para el Diseño Automatico a Cortante
        /// <summary>
        /// Devuelve Separación en Cm
        /// </summary>
        /// <param name="Nervio"></param>
        /// <param name="Estaciones"></param>
        /// <param name="AreaEstribo"></param>
        /// <returns></returns>
        private static float SdefinitivaZonas(cNervio Nervio, List<cEstacion> Estaciones, float AreaEstribo)
        {
            float Factor = Estaciones.Max(x => x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S);

            List<float> Sminimos = new List<float>();
            float SZ1Izquierda = AreaEstribo / Factor;

            foreach (cEstacion Estacion1 in Estaciones)
            {
                cBarra FindBarra = Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.Find(x => x.IsVisible(Estacion1.SubTramoOrigen));
                eNoBarra NoBarra = eNoBarra.B3;
                if (FindBarra != null)
                {
                    NoBarra = FindBarra.NoBarra;
                }
                float d = Estacion1.SubTramoOrigen.Seccion.H - Nervio.r1 - cDiccionarios.DiametrosBarras[NoBarra] / 2f-cVariables.DiametroEstriboPredeterminado;
                float Smin = d / 2f;
                Sminimos.Add(Smin);
            }
            float SeparD = SZ1Izquierda > Sminimos.Min() ? Sminimos.Min() : SZ1Izquierda;
            return PrecisarNumero(SeparD, 0.5f, 1);
        }

        public static void CrearEstribos(cTramo Tramo, eZonas Zona, eLadoDeZona LadoDeZona, float LongitudZona, float S, eNoBarra BarraE, float DeltaXInicial)
        {

            int CantidadEspacios = (int)PrecisarNumero((LongitudZona) / S, 1, 1, true);
            int CantidadEstribos = CantidadEspacios + 1;
            //MinEstribosSegunEfePrimaCe(ref CantidadEstribos, Tramo);
            if (LadoDeZona == eLadoDeZona.Izquierda)
            {
                if (Zona == eZonas.Zona1)
                {
                    Tramo.EstribosIzquierda.Zona1.EliminarEstribos(true);
                    Tramo.EstribosIzquierda.Zona1.NoBarra = BarraE;
                    Tramo.EstribosIzquierda.Zona1.Separacion = S;
                    for (int i = 0; i < CantidadEstribos; i++)
                    {

                        cSubTramo SubTramo = Tramo.Lista_SubTramos.Find(x => x.IsVisibleCoordAutoCAD(DeltaXInicial));
                        if (SubTramo != null)
                        {
                            Tramo.EstribosIzquierda.Zona1.CrearEstribo(SubTramo, DeltaXInicial);
                        }
                        DeltaXInicial += S;
                    }
                }
                else if (Zona == eZonas.Zona2)
                {
                    Tramo.EstribosIzquierda.Zona2.EliminarEstribos(true);
                    Tramo.EstribosIzquierda.Zona2.NoBarra = BarraE;
                    Tramo.EstribosIzquierda.Zona2.Separacion = S;
                    for (int i = 0; i < CantidadEstribos; i++)
                    {

                        cSubTramo SubTramo = Tramo.Lista_SubTramos.Find(x => x.IsVisibleCoordAutoCAD(DeltaXInicial));
                        if (SubTramo != null)
                        {
                            Tramo.EstribosIzquierda.Zona2.CrearEstribo(SubTramo, DeltaXInicial);
                        }
                        DeltaXInicial += S;
                    }
                }
            }
            else if (LadoDeZona == eLadoDeZona.Derecha)
            {
                if (Zona == eZonas.Zona1)
                {
                    Tramo.EstribosDerecha.Zona1.EliminarEstribos(true);
                    Tramo.EstribosDerecha.Zona1.NoBarra = BarraE;
                    Tramo.EstribosDerecha.Zona1.Separacion = S;
                    for (int i = 0; i < CantidadEstribos; i++)
                    {
                        cSubTramo SubTramo = Tramo.Lista_SubTramos.Find(x => x.IsVisibleCoordAutoCAD(DeltaXInicial));
                        if (SubTramo != null)
                        {
                            Tramo.EstribosDerecha.Zona1.CrearEstribo(SubTramo, DeltaXInicial);
                        }
                        DeltaXInicial -= S;
                    }
                }
                else if (Zona == eZonas.Zona2)
                {
                    Tramo.EstribosDerecha.Zona2.EliminarEstribos(true);
                    Tramo.EstribosDerecha.Zona2.NoBarra = BarraE;
                    Tramo.EstribosDerecha.Zona2.Separacion = S;
                    for (int i = 0; i < CantidadEstribos; i++)
                    {
                        cSubTramo SubTramo = Tramo.Lista_SubTramos.Find(x => x.IsVisibleCoordAutoCAD(DeltaXInicial));
                        if (SubTramo != null)
                        {
                            Tramo.EstribosDerecha.Zona2.CrearEstribo(SubTramo, DeltaXInicial);
                        }
                        DeltaXInicial -= S;
                    }
                }
            }
        }

        private static void EditarZonasEstribosCantidadMinima(cLadoDeEstribos LadoEstribos, cTramo Tramo)
        {
            if (LadoEstribos != null)
            {
                int CantidadEstribos = LadoEstribos.Zona1.Cantidad + LadoEstribos.Zona2.Cantidad;
                int CantidadFinal = MinEstribosSegunEfePrimaCe(CantidadEstribos, Tramo);
                int RestanteEstribos = CantidadFinal - CantidadEstribos;
                if (RestanteEstribos != 0)
                {
                    if (LadoEstribos.Zona1.NoBarra != eNoBarra.BNone && LadoEstribos.Zona2.NoBarra != eNoBarra.BNone || LadoEstribos.Zona2.NoBarra != eNoBarra.BNone)
                    {
                        CrearEstribos_2Completar(LadoEstribos.Zona2, Tramo, RestanteEstribos);

                    }
                    else if (LadoEstribos.Zona1.NoBarra != eNoBarra.BNone)
                    {
                        CrearEstribos_2Completar(LadoEstribos.Zona1, Tramo, RestanteEstribos);
                    }
                }
            }
        }
        private static void CrearEstribos_2Completar(cZonaEstribos ZonaEstribos, cTramo Tramo, int CantEstribos)
        {
            float S = ZonaEstribos.Separacion; float CoordX;
            if (ZonaEstribos.LadoDeZona == eLadoDeZona.Derecha)
                S = -S;
            CoordX = ZonaEstribos.Estribos.Last().CoordX + S;
            for (int i = 0; i < CantEstribos; i++)
            {
                cSubTramo SubTramoFind = Tramo.Lista_SubTramos.Find(x => x.IsVisibleCoordAutoCAD(CoordX));
                if (SubTramoFind != null)
                {
                    ZonaEstribos.CrearEstribo(SubTramoFind, CoordX);
                }
                CoordX += S;
            }

        }
        private static void CrearEstribosFaltantes(cTramo Tramo, eNoBarra BarraE)
        {
            if (Tramo.Lista_SubTramos.First().Indice != Tramo.NervioOrigen.Lista_Elementos.First().Indice && Tramo.Lista_SubTramos.Last().Indice != Tramo.NervioOrigen.Lista_Elementos.Last().Indice) //Que No sea Voladizo
            {

                if (Tramo.EstribosIzquierda == null)
                {
                    Tramo.EstribosIzquierda = new cLadoDeEstribos(eLadoDeZona.Izquierda, Tramo);
                    float CoordX = CoordXInicialEstribo(eLadoDeZona.Izquierda, Tramo);
                    int CantidaMinEstribos = MinEstribosSegunEfePrimaCe(0, Tramo);
                    float S = 0;
                    //float S = SdefinitivaZonas(Tramo.NervioOrigen, Tramo.Lista_SubTramos.First().Estaciones, cDiccionarios.AceroBarras[BarraE], eLadoDeZona.Izquierda) * cConversiones.Dimension_cm_to_m;
                    float Longitud = (CantidaMinEstribos - 1) * S;
                    CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Izquierda, Longitud, S, BarraE, CoordX);
                }
                if (Tramo.EstribosDerecha == null)
                {
                    Tramo.EstribosDerecha = new cLadoDeEstribos(eLadoDeZona.Derecha, Tramo);
                    float CoordX = CoordXInicialEstribo(eLadoDeZona.Derecha, Tramo);
                    int CantidaMinEstribos = MinEstribosSegunEfePrimaCe(0, Tramo);
                    //float S = SdefinitivaZonas(Tramo.NervioOrigen, Tramo.Lista_SubTramos.Last().Estaciones, cDiccionarios.AceroBarras[BarraE], eLadoDeZona.Derecha) * cConversiones.Dimension_cm_to_m;
                    float S = 0;
                    float Longitud = (CantidaMinEstribos - 1) * S;
                    CrearEstribos(Tramo, eZonas.Zona1, eLadoDeZona.Derecha, Longitud, S, BarraE, CoordX);
                }
            }

        }
        public static float CoordXInicialEstribo(eLadoDeZona LadoDeZona, cTramo Tramo)
        {
            return LadoDeZona == eLadoDeZona.Izquierda
                ? Tramo.Lista_SubTramos.First().Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.d_CaraApoyo
                : Tramo.Lista_SubTramos.Last().Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.d_CaraApoyo;
        }

        #endregion

        #region Metodos para el Diseño Automatico a Flexión
        private static void AgregarBarrasRefuerzoSuperior(cNervio Nervio, cTendencia tendencia)
        {

            bool SeAsingaBarraContinuaSuperior = false;
            List<List<IElemento>> Lista_Elementos = CrearListaElementos(Nervio, true, false, true, true);


            foreach (List<IElemento> elementos in Lista_Elementos)
            {
                if (elementos.First(x => x is cSubTramo).Seccion.B > cVariables.BNervioBorde || Nervio.NervioBorde)
                {
                    int Cantidad = 2;
                    if (Nervio.NervioBorde && elementos.First(x => x is cSubTramo).Seccion.B <= cVariables.BNervioBorde)
                        Cantidad = 1;
                    CrearBarraContinuaSuperior(elementos, Cantidad, eNoBarra.B3, tendencia, PuntosMitadTramo(elementos));
                    SeAsingaBarraContinuaSuperior = true;
                }

            }
            List<cPuntoInfelxion> PuntosInfexion2 = PuntosInfelxions(Nervio.Lista_Tramos, eUbicacionRefuerzo.Superior);
            List<eNoBarra> NoBarras2 = NoBarras.ToList(); ; NoBarras2.Remove(eNoBarra.BNone);

            if (PuntosInfexion2.Count > 0)
            {
                // ReorganizarPuntosInfelxionRSuperior(ref PuntosInfexion2, Nervio);

                for (int i = 0; i < PuntosInfexion2.Count; i++)
                {
                    cPuntoInfelxion PI1 = PuntosInfexion2[i];
                    cPuntoInfelxion PI2 = null;
                    if (i + 1 < PuntosInfexion2.Count)
                    {
                        PI2 = PuntosInfexion2[i + 1];
                    }

                    if (EncontraApoyoEntreDosPIs(PI1, PI2, Nervio) && !PI1.Agrego && !PI2.Agrego)
                    {
                        List<cEstacion> Estaciones = new List<cEstacion>();
                        Nervio.Lista_Tramos.ForEach(Tramo =>
                        {
                            Tramo.Lista_SubTramos.ForEach(Subtramo =>
                            {
                                Subtramo.Estaciones.ForEach(Estacion =>
                                {

                                    float CoordEsta = Subtramo.Vistas.Perfil_Original.Reales.Min(y => y.X) + Estacion.CoordX;
                                    if (PI1.CoordX <= CoordEsta && CoordEsta <= PI2.CoordX)
                                    {
                                        Estaciones.Add(Estacion);
                                    }
                                });
                            });
                        });

                        cEstacion FindEstacion = Estaciones.Find(x => x.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior == Estaciones.Min(y => y.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior));


                        float AceroFaltante = FindEstacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior;
                        float Area = Math.Abs(AceroFaltante); eNoBarra BarraPropuesta3 = ProponerUnaBarra(Area, NoBarras2, Nervio);
                        float Porcentaje = FindEstacion.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior;

                        if ((AceroFaltante < 0 && Math.Abs(Porcentaje) >= cVariables.ToleranciaFlexion) && (BarraPropuesta3 >= eNoBarra.B4 | BarraPropuesta3 == eNoBarra.BNone))
                        {
                            List<eNoBarra> BarrasPropuestas = ProponerDosBarras(Area, Nervio.Tendencia_Refuerzos.TSupeSelect.BarrasAEmplearBase, Nervio.Tendencia_Refuerzos.TSupeSelect.BarrasAEmplearAdicional, Nervio);
                            if (BarrasPropuestas != null)
                            {

                                eNoBarra BarraMinima = BarrasPropuestas.Min();
                                eNoBarra BarraMaxima = BarrasPropuestas.Max();
                                eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.None;
                                    List<float> CoordX = new List<float>(); CoordX.Add(PI1.CoordX); CoordX.Add(PI2.CoordX);
                                    CoordenadasBarraAdicional(ref CoordX, BarraMaxima, PI1.SubTramo, Nervio, ref GI, ref GD,tendencia);
                                    CrearBarra2(BarraMaxima, tendencia.UbicacionRefuerzo, 1, CoordX.First(), CoordX.Last(), ref tendencia, GI, GD);
                                

                                #region Barra Adicional 
                                float MenorDelta = Estaciones.Min(x => x.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior); //El Acero Que mas requiero
                                float PorcentajeMenorDelta = Estaciones.Min(x => x.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior);
                                List<float> ListaCoordX = new List<float>();

                                if (PorcentajeMenorDelta < 0 && Math.Abs(PorcentajeMenorDelta) >= cVariables.ToleranciaFlexion)
                                {
                                    for (int j = 0; j < Estaciones.Count; j++)
                                    {
                                        cEstacion EstaActual = Estaciones[j];
                                        cEstacion EstaAnterior = null; cEstacion EstaSig = null;
                                        if (j - 1 >= 0)
                                        {
                                            EstaAnterior = Estaciones[j - 1];
                                        }

                                        if (j + 1 < Estaciones.Count)
                                        {
                                            EstaSig = Estaciones[j + 1];
                                        }

                                        float CoordX2 = DevolverXRefSuperior(EstaAnterior, EstaActual, EstaSig);
                                        if (CoordX2 != float.PositiveInfinity)
                                        {
                                            ListaCoordX.Add(EstaActual.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + CoordX2);
                                        }
                                    }
                                    if (ListaCoordX.Count > 1)
                                    {
                                        eTipoGancho GI2 = eTipoGancho.None; eTipoGancho GD2 = eTipoGancho.None;
                                        CoordenadasBarraAdicional(ref ListaCoordX, BarraMaxima, PI1.SubTramo, Nervio, ref GI2, ref GD2,tendencia);
                                        CrearBarra2(BarraMinima, eUbicacionRefuerzo.Superior, 1, ListaCoordX.First(), ListaCoordX.Last(), ref tendencia, GI2, GD2);
                                    }
                                }

                                #endregion
                            }


                        }
                        else if ((AceroFaltante < 0 && Math.Abs(Porcentaje) >= cVariables.ToleranciaFlexion))
                        {
                            if (!SeAsingaBarraContinuaSuperior)
                            {
                                List<float> CoordX = new List<float>(); CoordX.Add(PI1.CoordX); CoordX.Add(PI2.CoordX);
                                eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.None;
                                CoordenadasBarraAdicional(ref CoordX, BarraPropuesta3, PI1.SubTramo, Nervio, ref GI, ref GD,tendencia);
                                CrearBarra2(BarraPropuesta3, eUbicacionRefuerzo.Superior, 1, CoordX.First(), CoordX.Last(), ref tendencia, GI, GD);
                            }
                            else
                            {
                                List<float> ListaCoordX = new List<float>();
                                eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.None;
                                for (int j = 0; j < Estaciones.Count; j++)
                                {
                                    cEstacion EstaActual = Estaciones[j];
                                    cEstacion EstaAnterior = null; cEstacion EstaSig = null;
                                    if (j - 1 >= 0)
                                    {
                                        EstaAnterior = Estaciones[j - 1];
                                    }

                                    if (j + 1 < Estaciones.Count)
                                    {
                                        EstaSig = Estaciones[j + 1];
                                    }

                                    float CoordX2 = DevolverXRefSuperior(EstaAnterior, EstaActual, EstaSig);
                                    if (CoordX2 != float.PositiveInfinity)
                                    {
                                        ListaCoordX.Add(EstaActual.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + CoordX2);
                                    }
                                }
                                if (ListaCoordX.Count > 1)
                                {
                                    eTipoGancho GI2 = eTipoGancho.None; eTipoGancho GD2 = eTipoGancho.None;
                                    CoordenadasBarraAdicional(ref ListaCoordX, BarraPropuesta3, PI1.SubTramo, Nervio, ref GI2, ref GD2,tendencia);
                                    CrearBarra2(BarraPropuesta3, eUbicacionRefuerzo.Superior, 1, ListaCoordX.First(), ListaCoordX.Last(), ref tendencia, GI2, GD2);
                                }

                            }
                        }

                        PI1.Agrego = true;
                        PI2.Agrego = true;
                    }
                }

            }
            else
            {

            }
        
           

            // Agregar Barras Esquineras

            IElemento UElemento = Nervio.Lista_Elementos.Last(x => x is cSubTramo);

            IElemento PElemento = Nervio.Lista_Elementos.First(x => x is cSubTramo);

            float ExtremoIzquierda = Nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.First().X + cVariables.RExtremoIzquierdo;
            float ExtremoDerecha = Nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Max(y=>y.X) - cVariables.RExtremoDerecho;
            #region Extremo Izquierda
            if (Nervio.Lista_Elementos.First() is cApoyo) //No Es un Voladizo Superior Izquierda
            {
                List<cBarra> BarraFindsPElemento = tendencia.Barras.FindAll(x => x.IsVisible(PElemento));
                float Xini_Apoyo = Nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.Min(x => x.X); //XI apoyo
                float Xf_Apoyo = Nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.Max(x => x.X); //Xf apoyo
                float LongPrimerSubTramo = ((cSubTramo)PElemento).TramoOrigen.Longitud;
                float XfinBar = 0;
                if (LongPrimerSubTramo <= cVariables.LongDelCriterioSubTramo)
                {
                    XfinBar = Xf_Apoyo + cVariables.AlargamientoExtremosEfePrimaCe;
                }
                else
                {
                    XfinBar = Xf_Apoyo + cVariables.Porc_LongAlargamientoExtremo * LongPrimerSubTramo;  //(0.24*longitud)
                }

                if (BarraFindsPElemento != null && BarraFindsPElemento.Count != 0)
                {
                    cBarra BarraFind = BarraFindsPElemento.Find(y => y.XI == BarraFindsPElemento.Min(x => x.XI));
                    //Posible barra a colocar
                    float R = cVariables.RExtremoIzquierdo;
                    float Xini_NewBarra = Xini_Apoyo + R;
                    float Xini_BarraExistente = BarraFind.XI;

                    if ((Xini_BarraExistente - XfinBar > 0 && Xini_BarraExistente - XfinBar <= cVariables.DeltaUnionCercanas) || Xini_BarraExistente - XfinBar < 0)
                    {
                        float LongTotal = (BarraFind.XF - Xini_NewBarra) + cDiccionarios.LDGancho(BarraFind.NoBarra, eTipoGancho.G90);

                        if (LongTotal <= tendencia.MaximaLongitud)
                        {

                            Xini_BarraExistente = Xini_Apoyo + R;
                            BarraFind.XI = Xini_BarraExistente;
                            BarraFind.GanchoIzquierda = eTipoGancho.G90;
                            ModificarBarraParaLongMinima(BarraFind, ExtremoIzquierda, ExtremoDerecha, tendencia);
                        }
                        else
                        {
                            List<float> CoordX = new List<float>() { Xini_Apoyo, XfinBar }; eTipoGancho GI = eTipoGancho.G90; eTipoGancho GD = eTipoGancho.None;
                            CorregirCoordXBarra(ref CoordX, Nervio, ref GI, ref GD);
                            float Xi = CoordX.First(); float Xf = CoordX.Last();
                            ModificarCoordenadasParaCumplirLongMinima(ExtremoIzquierda, ExtremoDerecha, eNoBarra.B3, ref GI, ref GD,ref Xi,ref Xf,tendencia.MinimaLongitud );
                            CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1,Xi , Xf, ref tendencia, GI, GD);
                        }
                    }
                    else
                    {
                        List<float> CoordX = new List<float>() { Xini_Apoyo, XfinBar }; eTipoGancho GI = eTipoGancho.G90; eTipoGancho GD = eTipoGancho.None;
                        CorregirCoordXBarra(ref CoordX, Nervio, ref GI, ref GD);
                        float Xi = CoordX.First(); float Xf = CoordX.Last();
                        ModificarCoordenadasParaCumplirLongMinima(ExtremoIzquierda, ExtremoDerecha, eNoBarra.B3, ref GI, ref GD, ref Xi, ref Xf, tendencia.MinimaLongitud);
                        CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xi, Xf , ref tendencia, GI, GD);
                    }

                }
                else
                {
                    List<float> CoordX = new List<float>() { Xini_Apoyo, XfinBar }; eTipoGancho GI = eTipoGancho.G90; eTipoGancho GD = eTipoGancho.None;
                    CorregirCoordXBarra(ref CoordX, Nervio, ref GI, ref GD);
                    float Xi = CoordX.First(); float Xf = CoordX.Last();
                    ModificarCoordenadasParaCumplirLongMinima(ExtremoIzquierda, ExtremoDerecha, eNoBarra.B3, ref GI, ref GD, ref Xi, ref Xf, tendencia.MinimaLongitud);
                    CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xi, Xf, ref tendencia, GI, GD);

                }
            }
            else   //Voladizo
            {
                List<cBarra> BarraFindsPElemento = tendencia.Barras.FindAll(x => x.IsVisible(PElemento));
                if (BarraFindsPElemento != null && BarraFindsPElemento.Count != 0) //Si Encontró Barras
                {
                    cBarra BarraFind = BarraFindsPElemento.Find(y => y.XI == BarraFindsPElemento.Min(x => x.XI));
                    float R = cVariables.RExtremoIzquierdo;
                    float XIProbale = Nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + R;
                    float LongProbable = (BarraFind.XF - XIProbale) + (cDiccionarios.LDGancho(BarraFind.NoBarra, eTipoGancho.G90));
                    if (LongProbable >= tendencia.MaximaLongitud)
                    {
                        float Xo = XIProbale; float Xf = BarraFind.XI;
                        eTipoGancho GI = eTipoGancho.G90; eTipoGancho GD = eTipoGancho.None;
                        CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xo, Xf, ref tendencia, GI, GD);
                        BarraFind.XI -= BarraFind.Traslapo;
                        ModificarBarraParaLongMinima(BarraFind, ExtremoIzquierda, ExtremoDerecha, tendencia);
                    }
                    else
                    {
                        BarraFind.XI = XIProbale;
                        BarraFind.GanchoIzquierda = eTipoGancho.G90;
                        ModificarBarraParaLongMinima(BarraFind, ExtremoIzquierda, ExtremoDerecha, tendencia);
                    }
                }
            }


            #endregion

            #region Extremo Derecha

            if (Nervio.Lista_Elementos.Last() is cApoyo) // No Voladizo Superior Derecha
            {
                List<cBarra> BarraFindsUElemento = tendencia.Barras.FindAll(x => x.IsVisible(UElemento));
                float Xini_Apoyo = Nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Min(x => x.X); //XI apoyo
                float Xf_Apoyo = Nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Max(x => x.X); //Xf apoyo
                float LongUltimoSubTramo = ((cSubTramo)UElemento).TramoOrigen.Longitud;
                float XiniBar = 0; float R = cVariables.RExtremoDerecho;
                if (LongUltimoSubTramo <= cVariables.LongDelCriterioSubTramo)// L < 4m
                {
                    XiniBar = Xini_Apoyo - cVariables.AlargamientoExtremosEfePrimaCe;  // + 1m
                }
                else
                {
                    XiniBar = Xini_Apoyo - cVariables.Porc_LongAlargamientoExtremo * LongUltimoSubTramo;  //(0.24*longitud)
                }

                float Xfin_NewBarra = Xf_Apoyo - R;
                if (BarraFindsUElemento != null && BarraFindsUElemento.Count != 0)
                {
                    cBarra BarraFind = BarraFindsUElemento.Find(y => y.XF == BarraFindsUElemento.Max(x => x.XF));
                    //Posible barra a colocar
                    R = Nervio.r1 * cConversiones.Dimension_cm_to_m;
                    float Xfin_BarraExistente = BarraFind.XF;

                    if ((XiniBar - Xfin_BarraExistente > 0 && XiniBar - Xfin_BarraExistente <= cVariables.DeltaUnionCercanas) || XiniBar - Xfin_BarraExistente < 0)
                    {
                        float LongTotal = (Xfin_NewBarra - BarraFind.XI) + cDiccionarios.LDGancho(BarraFind.NoBarra, eTipoGancho.G90);

                        if (LongTotal <= tendencia.MaximaLongitud)
                        {
                            BarraFind.GanchoDerecha = eTipoGancho.G90;
                            BarraFind.XF = Xfin_NewBarra;
                            ModificarBarraParaLongMinima(BarraFind, ExtremoIzquierda, ExtremoDerecha, tendencia);

                        }
                        else
                        {
                            eTipoGancho GI = eTipoGancho.None;  eTipoGancho GD = eTipoGancho.G90;
                            float Xi = XiniBar; float Xf = Xfin_NewBarra;
                            ModificarCoordenadasParaCumplirLongMinima(ExtremoIzquierda, ExtremoDerecha, eNoBarra.B3, ref GI, ref GD, ref Xi, ref Xf, tendencia.MinimaLongitud);
                            CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xi, Xf, ref tendencia, GI, GD);
                        }
                    }
                    else
                    {
                        R = cVariables.RExtremoDerecho;
                        eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.G90;
                        float Xi = XiniBar; float Xf = Xfin_NewBarra;
                        ModificarCoordenadasParaCumplirLongMinima(ExtremoIzquierda, ExtremoDerecha, eNoBarra.B3, ref GI, ref GD, ref Xi, ref Xf, tendencia.MinimaLongitud);
                        CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xi, Xf, ref tendencia, GI, GD);
                    }

                }
                else
                {
                    eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.G90;
                    float Xi = XiniBar; float Xf = Xfin_NewBarra;
                    ModificarCoordenadasParaCumplirLongMinima(ExtremoIzquierda, ExtremoDerecha, eNoBarra.B3, ref GI, ref GD, ref Xi, ref Xf, tendencia.MinimaLongitud);
                    CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xi, Xf, ref tendencia, GI, GD);

                }


            }
            else // Voladizo
            {
                List<cBarra> BarraFindsUElemento = tendencia.Barras.FindAll(x => x.IsVisible(UElemento));
                if (BarraFindsUElemento != null && BarraFindsUElemento.Count != 0) //Si Encontró Barras
                {
                    cBarra BarraFind = BarraFindsUElemento.Find(y => y.XI == BarraFindsUElemento.Min(x => x.XI));
                    float R = cVariables.RExtremoDerecho;
                    float XFProbable = Nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - R;
                    float LongProbable = (XFProbable - BarraFind.XI) + (cDiccionarios.LDGancho(BarraFind.NoBarra, eTipoGancho.G90));
                    if (LongProbable >= tendencia.MaximaLongitud)
                    {
                        float Xo = BarraFind.XF; float Xf = XFProbable;
                        CrearBarra2(eNoBarra.B3, tendencia.UbicacionRefuerzo, 1, Xo, Xf, ref tendencia, eTipoGancho.None, eTipoGancho.G90);
                        BarraFind.XF += BarraFind.Traslapo;
                        ModificarBarraParaLongMinima(BarraFind,ExtremoIzquierda,ExtremoDerecha,tendencia);
                    }
                    else
                    {
                        BarraFind.XF = XFProbable;
                        BarraFind.GanchoDerecha = eTipoGancho.G90;
                        ModificarBarraParaLongMinima(BarraFind, ExtremoIzquierda, ExtremoDerecha, tendencia);
                    }
                }
            }

            // CrearListaElementos()
            #endregion

        }
        private static void ModificarBarraParaLongMinima(cBarra  barra,float ExtremoIzq, float ExtremoDer, cTendencia tendencia)
        {
            float Xi = barra.XI;
            float Xf = barra.XF;
            eTipoGancho GI = barra.GanchoIzquierda;
            eTipoGancho GD = barra.GanchoDerecha;
            ModificarCoordenadasParaCumplirLongMinima(ExtremoIzq, ExtremoDer, barra.NoBarra, ref GI, ref GD, ref Xi, ref Xf, tendencia.MinimaLongitud);
            barra.GanchoIzquierda = GI;barra.GanchoDerecha = GD;
            barra.XI = Xi;barra.XF = barra.XF;

        }
        private static List<cPuntoInfelxion> PuntosMitadTramo(List<IElemento> elementos)
        {
            List<cPuntoInfelxion> puntoInfelxions = new List<cPuntoInfelxion>();

            var Tramos= elementos.FindAll(y => y is cSubTramo).ToList().ConvertAll(ElementoASubtramo).ToList().Select(y => y.TramoOrigen).ToList().DistinctBy(y => y.Nombre).ToList();
            foreach(var Tramo in Tramos)
            {
                float Xmin = Tramo.Lista_SubTramos.First().Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
                float Xmax = Tramo.Lista_SubTramos.Last().Vistas.Perfil_AutoCAD.Reales.Max(y => y.X);
                float X = Xmin + (Xmax - Xmin)/2f;
                cEstacion estacion = Tramo.Estaciones.EstacionMasCercana(X);
                cPuntoInfelxion Pi = new cPuntoInfelxion(estacion);
                puntoInfelxions.Add(Pi);
            }
            return puntoInfelxions;
        }
        private static bool EncontraApoyoEntreDosPIs(cPuntoInfelxion P1,cPuntoInfelxion P2,cNervio nervio)
        {
            var Apoyos = nervio.Lista_Elementos.FindAll(y => y.Soporte == eSoporte.Apoyo).ToList();
            bool Encontro = false;
            if (P1 != null && P2 != null)
            {
                Apoyos.ForEach(a =>
                {
                    float Xmin = a.Vistas.Perfil_AutoCAD.Reales.Min(y=>y.X);
                    if (P1.CoordX <= Xmin && P2.CoordX >= Xmin)
                    {
                        Encontro = true;
                    }
                });
            }
            return Encontro;

        }


        public static void CorregirCoordXBarra(ref List<float> CoordX, cNervio Nervio, ref eTipoGancho GI, ref eTipoGancho GD)
        {
            float Xo = CoordX.First(); float Xf = CoordX.Last();
            float LimiteXIzquierda = Nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.First().X + cVariables.RExtremoIzquierdo;
            float LimiteXDerecha = Nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Last().X - cVariables.RExtremoDerecho;
            IElemento ApoyoIzquierda = Nervio.Lista_Elementos.Find(x => x is cApoyo && x.IsVisibleCoordAutoCAD(Xo));

            IElemento ApoyoDerecha = Nervio.Lista_Elementos.Find(x => x is cApoyo && x.IsVisibleCoordAutoCAD(Xf));

            if (ApoyoIzquierda != null)
            {
                Xo = ApoyoIzquierda.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.RExtremoIzquierdo;
                GI = eTipoGancho.G90;
            }
            else if (Xo <= LimiteXIzquierda)// Se Sale del Nervio a la Izquierda
            {
                Xo = LimiteXIzquierda;
                GI = eTipoGancho.G90;
            }
            if (ApoyoDerecha != null)
            {
                GD = eTipoGancho.G90;
                Xf = ApoyoDerecha.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.RExtremoDerecho;
            }
            else if (Xf >= LimiteXDerecha)// Se Sale del Nervio a la Derecha
            {
                GD = eTipoGancho.G90;
                Xf = LimiteXDerecha;
            }

            CoordX = new List<float>() { Xo, Xf };

        }
        private static void AgegarBarrasAdicionalesRefuerzoInferior(List<cTramo> Tramos, cTendencia tendencia, cNervio Nervio)
        {
            Tramos.ForEach(Tramo =>
            {

                float MenorDelta = Tramo.Estaciones.Min(x => x.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior); //El Acero Que mas requiero
                float PorcentajeMenorDelta = Tramo.Estaciones.Min(x => x.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior);
                List<float> ListaCoordX = new List<float>();

                if (PorcentajeMenorDelta < 0 && Math.Abs(PorcentajeMenorDelta) >= cVariables.ToleranciaFlexion)
                {
                    for (int i = 0; i < Tramo.Estaciones.Count; i++)
                    {
                        cEstacion EstaActual = Tramo.Estaciones[i];
                        cEstacion EstaAnterior = null; cEstacion EstaSig = null;
                        if (i - 1 >= 0)
                        {
                            EstaAnterior = Tramo.Estaciones[i - 1];
                        }

                        if (i + 1 < Tramo.Estaciones.Count)
                        {
                            EstaSig = Tramo.Estaciones[i + 1];
                        }

                        float CoordX = DevolverXRefInferior(EstaAnterior, EstaActual, EstaSig);
                        if (CoordX != float.PositiveInfinity)
                        {
                            ListaCoordX.Add(EstaActual.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + CoordX);
                        }
                    }
                    if (ListaCoordX.Count > 1)
                    {
                        Tramo.CoordX = float.PositiveInfinity;
                        eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.None;
                        eNoBarra NoBarraAProponer = ProponerUnaBarra(Math.Abs(MenorDelta), tendencia.BarrasAEmplearAdicional, Nervio);
                        if (NoBarraAProponer != eNoBarra.BNone)
                        {
                            CoordenadasBarraAdicional(ref ListaCoordX, NoBarraAProponer, Tramo.Lista_SubTramos.First(), Nervio, ref GI, ref GD,tendencia);
                            CrearBarra2(NoBarraAProponer, eUbicacionRefuerzo.Inferior, 1, ListaCoordX.First(), ListaCoordX.Last(), ref tendencia, GI, GD);
                        }
                    }
                    else if (ListaCoordX.Count == 1)
                    {
                        Tramo.CoordX = ListaCoordX[0];
                        Tramo.DeltaAcero = Math.Abs(MenorDelta);
                    }
                    else
                    {
                        Tramo.CoordX = float.PositiveInfinity;
                    }

                }
                else
                {
                    Tramo.CoordX = float.PositiveInfinity;
                }

            });


            for (int i = 0; i < Tramos.Count; i++)
            {
                cTramo Tramo = Tramos[i];
                if (Tramo.CoordX != float.PositiveInfinity)
                {
                    if (i + 1 < Tramos.Count && Tramos[i + 1].CoordX != float.PositiveInfinity)
                    {
                        eTipoGancho GI = eTipoGancho.None; eTipoGancho GD = eTipoGancho.None;
                        eNoBarra NoBarraAProponer = ProponerUnaBarra(Tramo.DeltaAcero, tendencia.BarrasAEmplearAdicional, Nervio);
                        List<float> ListaCoordX = new List<float>() { Tramo.CoordX, Tramos[i + 1].CoordX };
                        CoordenadasBarraAdicional(ref ListaCoordX, NoBarraAProponer, Tramo.Lista_SubTramos.First(), Nervio, ref GI, ref GD,tendencia);
                        CrearBarra2(NoBarraAProponer, eUbicacionRefuerzo.Inferior, 1, ListaCoordX.First(), ListaCoordX.Last(), ref tendencia, GI, GD);
                        Tramo.CoordX = float.PositiveInfinity;
                    }
                }


            }



        }
        private static void CoordenadasBarraAdicional(ref List<float> CoordX, eNoBarra noBarra, cSubTramo SubTramo, cNervio Nervio, ref eTipoGancho GI, ref eTipoGancho GD,cTendencia tendencia)
        {
            float Ld = cDiccionarios.FindLdBarra(noBarra, SubTramo.Seccion.Material.fc,true);
            float Xo = CoordX.First(); float Xf = CoordX.Last();
            float LimiteXIzquierda = Nervio.Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.RExtremoIzquierdo;
            float LimiteXDerecha = Nervio.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.RExtremoDerecho;
            float d = (SubTramo.Seccion.H - Nervio.r1 - cDiccionarios.DiametrosBarras[noBarra] / 2f) * cConversiones.Dimension_cm_to_m;

            if (d >= cVariables.TraslapoNervio)
                d = Ld;

            Xo -= d; Xf += d;

            IElemento ApoyoIzquierda = Nervio.Lista_Elementos.Find(x => x is cApoyo && x.IsVisibleCoordAutoCAD(Xo));

            IElemento ApoyoDerecha = Nervio.Lista_Elementos.Find(x => x is cApoyo && x.IsVisibleCoordAutoCAD(Xf));

            if (ApoyoIzquierda != null)
            {
                Xo = ApoyoIzquierda.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) + cVariables.RExtremoIzquierdo;
                GI = eTipoGancho.G90;
            }
            else if (Xo <= LimiteXIzquierda)// Se Sale del Nervio a la Izquierda
            {
                Xo = LimiteXIzquierda;
                GI = eTipoGancho.G90;
            }
            if (ApoyoDerecha != null)
            {
                GD = eTipoGancho.G90;
                Xf = ApoyoDerecha.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X) - cVariables.RExtremoDerecho;
            }
            else if (Xf >= LimiteXDerecha)// Se Sale del Nervio a la Derecha
            {
                GD = eTipoGancho.G90;
                Xf = LimiteXDerecha;
            }
            ModificarCoordenadasParaCumplirLongMinima(LimiteXIzquierda, LimiteXDerecha, noBarra, ref GI, ref GD, ref Xo, ref Xf, tendencia.MinimaLongitud);
            CoordX = new List<float>() { Xo, Xf };
        }

        private static void ModificarCoordenadasParaCumplirLongMinima(float ExtremoIzqu, float ExtremoDer,eNoBarra noBarra,ref eTipoGancho GI,ref  eTipoGancho GD, ref float Xi, ref float Xf, float LongMinima)
        {

            float LongBarra = Xf - Xi + cDiccionarios.LDGancho(noBarra, GI) + cDiccionarios.LDGancho(noBarra, GD);
            if (LongBarra < LongMinima)
            {
                float Faltante = LongMinima - LongBarra;

                float AlargamientoMaximoIzquierda = Xi- ExtremoIzqu;
                float AlargamientoMaximoDerecha = ExtremoDer - Xf;

                if(Xi-Faltante/2f< ExtremoIzqu)
                {
                    Xi -= AlargamientoMaximoIzquierda; GI = eTipoGancho.G90;
                    float LongBarra2 = Xf - Xi + cDiccionarios.LDGancho(noBarra, GI) + cDiccionarios.LDGancho(noBarra, GD);
                    float Delta = LongMinima - LongBarra2;
                    if (Xf + Delta > ExtremoDer)
                    {
                        Xf = ExtremoDer;
                        GD = eTipoGancho.G90;
                    }
                    else
                    {
                        Xf += Delta;
                    }
                }
                else if (Xf+Faltante/2f> ExtremoDer)
                {
                    Xf += AlargamientoMaximoDerecha; GD = eTipoGancho.G90;
                    float LongBarra2 = Xf - Xi + cDiccionarios.LDGancho(noBarra, GI) + cDiccionarios.LDGancho(noBarra, GD);
                    float Delta = LongMinima - LongBarra2;
                    if (Xi - Delta < ExtremoIzqu)
                    {
                        Xi = ExtremoIzqu;
                        GI = eTipoGancho.G90;
                    }
                    else
                    {
                        Xi -= Delta;
                    }
                }
                else
                {
                    Xi -= Faltante / 2f;
                    Xf += Faltante / 2f;
                    float LongBarra2 = Xf - Xi;
                }
            }



        }







        private static float DevolverXRefInferior(cEstacion EstacionAnterior, cEstacion EstacionActual, cEstacion EstacionPosterior)
        {
            if (EstacionAnterior != null && EstacionPosterior != null) //Elemento del Medio
            {
                if ((EstacionActual.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior <= 0 && Math.Abs(EstacionActual.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior) <= cVariables.ToleranciaFlexionPInflexion) 
                    || (EstacionActual.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior >= cVariables.ToleranciAceroFlexion))
                {
                    if (EstacionPosterior.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior < 0 && Math.Abs(EstacionPosterior.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior) > cVariables.ToleranciaFlexionPInflexion)
                    {
                        return EstacionActual.CoordX;
                    }
                    else if (EstacionAnterior.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior < 0 && Math.Abs(EstacionAnterior.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior) > cVariables.ToleranciaFlexionPInflexion)
                    {
                        return EstacionActual.CoordX;
                    }
                }
            }
            else if(EstacionAnterior!=null && EstacionPosterior== null)
            {
                if (EstacionAnterior.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior < 0 
                    && Math.Abs(EstacionAnterior.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior) > cVariables.ToleranciaFlexionPInflexion)
                {
                    return EstacionActual.CoordX;
                }
            }else if (EstacionAnterior== null && EstacionPosterior != null)
            {
                if (EstacionPosterior.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior < 0 
                    && Math.Abs(EstacionPosterior.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior) > cVariables.ToleranciaFlexionPInflexion)
                {
                    return EstacionActual.CoordX;
                }
            }
            return float.PositiveInfinity;
        }
        private static float DevolverXRefSuperior(cEstacion EstacionAnterior, cEstacion EstacionActual, cEstacion EstacionPosterior)
        {
            if (EstacionAnterior != null && EstacionPosterior != null) //Elemento del Medio
            {
                if ((EstacionActual.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior <= 0 && Math.Abs(EstacionActual.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior) <= cVariables.ToleranciaFlexionPInflexion) || EstacionActual.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior >= 0)
                {
                    if (EstacionPosterior.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior < 0 && Math.Abs(EstacionPosterior.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior) > cVariables.ToleranciaFlexionPInflexion)
                    {
                        return EstacionActual.CoordX;
                    }
                    else if (EstacionAnterior.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior < 0 && Math.Abs(EstacionAnterior.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior) > cVariables.ToleranciaFlexionPInflexion)
                    {
                        return EstacionActual.CoordX;
                    }
                }
            }
            return float.PositiveInfinity;
        }
        private static void AlargamientoAmbosLadosLds(List<List<IElemento>> ListaElementos, cTendencia Tendecia)
        {


            if (ListaElementos.Count > 1)
            {
                //Alargamiento a la Derecha
                for (int i = 0; i < ListaElementos.Count; i++)
                {
                    List<IElemento> Elementos = ListaElementos[i];
                    IElemento Elemento1 = Elementos.Last();
                    if (Elemento1 is cSubTramo && Elemento1.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y) == 0) // Comprobar si el Elemento esta en el nivel cero y por ende se puede alargar LD.
                    {
                        if (i + 1 < ListaElementos.Count)
                        {
                            List<IElemento> Elementos1 = ListaElementos[i + 1];
                            IElemento Elemento2 = Elementos1.First(); //Ecuentro el Primer Elemento de la Lista Siguiente 
                            if (Elemento2 is cSubTramo)
                            {
                                cBarra FindBarra = Tendecia.Barras.Find(x => x.IsVisible(Elemento1) && x.Nivel == 1); //Encuentro la Barra que correspona al Elemento con nivel cero y con barra en Nivel 1
                                if (FindBarra != null && ((FindBarra.XF + FindBarra.Traslapo) - (FindBarra.XI) <= Tendecia.MaximaLongitud - cDiccionarios.LDGancho(FindBarra.NoBarra, FindBarra.GanchoIzquierda))) // Se Comprueba que la Barra no supere la longitud maxima
                                {
                                    FindBarra.GanchoDerecha = eTipoGancho.None;
                                    FindBarra.XF += FindBarra.Traslapo;
                                }
                            }
                        }
                    }

                }

                //Alargamiento a la Izquierda
                for (int i = 0; i < ListaElementos.Count; i++)
                {
                    List<IElemento> Elementos = ListaElementos[i];
                    IElemento Elemento1 = Elementos.First();
                    if (Elemento1 is cSubTramo && Elemento1.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y) == 0) // Comprobar si el Elemento esta en el nivel cero y por ende se puede alargar LD.
                    {
                        if (i - 1 >= 0)
                        {
                            List<IElemento> Elementos1 = ListaElementos[i - 1];
                            IElemento Elemento2 = Elementos1.Last(); //Ecuentro el Ultimo Elemento de la Lista Anterior
                            if (Elemento2 is cSubTramo)
                            {
                                cBarra FindBarra = Tendecia.Barras.Find(x => x.IsVisible(Elemento1) && x.Nivel == 1); //Encuentro la Barra que correspona al Elemento con nivel cero y con barra en Nivel 1
                                if (FindBarra != null && (FindBarra.XF - (FindBarra.XI - FindBarra.Traslapo) <= Tendecia.MaximaLongitud - cDiccionarios.LDGancho(FindBarra.NoBarra, FindBarra.GanchoDerecha))) // Se Comprueba que la Barra no supere la longitud maxima
                                {
                                    FindBarra.GanchoIzquierda = eTipoGancho.None;
                                    FindBarra.XI -= FindBarra.Traslapo;
                                }
                            }
                        }
                    }

                }


            }






        }
        private static bool CondicionTotal1(cSeccion Seccion1, cSeccion Seccion2, cNervio Nervio, bool RemoverCondicion)
        {
            return RemoverCondicion
                ? true
                : (Seccion1.H != Seccion2.H && (Nervio.CambioenAltura == eCambioenAltura.Inferior | Nervio.CambioenAltura == eCambioenAltura.Central)) || (Seccion1.H != Seccion2.H && Nervio.CambioenAncho == eCambioenAncho.Central);
        }
        private static bool CondicionTotal2(cSeccion Seccion1, cSeccion Seccion2, bool RemoverCondicion)
        {
            return RemoverCondicion ? true : Seccion1.B != Seccion2.B | Seccion1.H != Seccion2.H;
        }
        private static bool CondicionTotal3(cSeccion Seccion1, cSeccion Seccion2, cNervio Nervio, bool RemoverCondicion)
        {
            return RemoverCondicion ? true : Nervio.CambioenAncho != eCambioenAncho.Central && Nervio.CambioenAltura == eCambioenAltura.Inferior && Seccion1.B == Seccion2.B; //Condicion para colocar 2 Barras superiores en anchos > 12cm
                                                                                                                                                                              //solo aplica si no hay barras en el nervio.
        }
        private static bool CondicionTotal4(cSeccion Seccion1, cSeccion Seccion2, bool RemoverCondicion)
        {
            return RemoverCondicion ? true : Seccion1.B != Seccion2.B;//Condicion para colocar solo una barra continua y luego con otro metodo se agregará la faltante
        }
        public static List<List<IElemento>> CrearListaElementos(cNervio Nervio, bool RCondicion1 = false, bool RCondicion2 = false, bool RCondicion3 = false, bool RCondicion4 = false, bool AgregarApoyos = true)
        {
            List<List<IElemento>> ListaElementos = new List<List<IElemento>>();
            Nervio.Lista_Elementos.ForEach(Elem =>
            {

                if (Elem is cSubTramo)
                {

                    if (ListaElementos.Count > 0)
                    {
                        cSubTramo subTramo1;

                        try { subTramo1 = (cSubTramo)ListaElementos.Last().Last(x => x is cSubTramo); }
                        catch { subTramo1 = null; }


                        if (subTramo1 != null)
                        {
                            if (CondicionTotal1(subTramo1.Seccion, Elem.Seccion, Nervio, RCondicion1) && CondicionTotal2(subTramo1.Seccion, Elem.Seccion, RCondicion2) && CondicionTotal3(subTramo1.Seccion, Elem.Seccion, Nervio, RCondicion3) && CondicionTotal4(subTramo1.Seccion, Elem.Seccion, RCondicion4))
                            {
                                List<IElemento> Elementos = new List<IElemento>();
                                Elementos.Add(Elem); ListaElementos.Add(Elementos);
                            }
                            else
                            {
                                ListaElementos.Last().Add(Elem);
                            }
                        }
                        else
                        {
                            ListaElementos.Last().Add(Elem);
                        }
                    }
                    else
                    {
                        List<IElemento> Elementos = new List<IElemento>();
                        Elementos.Add(Elem);
                        ListaElementos.Add(Elementos);
                    }

                }
                else
                {
                    if (ListaElementos.Count > 0)
                    {
                        ListaElementos.Last().Add(Elem);
                    }
                    else
                    {
                        List<IElemento> Elementos = new List<IElemento>();
                        Elementos.Add(Elem); ListaElementos.Add(Elementos);
                    }
                }
            });


            //Agregar Apoyos Faltantes
            if (AgregarApoyos)
            {
                if (ListaElementos.Count > 1)
                {
                    for (int i = 1; i < ListaElementos.Count; i++)
                    {
                        List<IElemento> Elementos = ListaElementos[i];
                        IElemento PrimeElemento = Elementos.First();
                        if (!(PrimeElemento is cApoyo))
                        {
                            IElemento UltimoElemento = ListaElementos[i - 1].Last();
                            if (UltimoElemento is cApoyo)
                            {
                                Elementos.Insert(0, UltimoElemento);
                            }
                        }
                    }
                }
            }





            return ListaElementos;
        }
        private static void CrearEstacionesTemporaneasTramos(List<cTramo> Tramos)
        {
            Tramos.ForEach(Tramo =>
            {

                List<cEstacion> Estaciones = new List<cEstacion>();
                Tramo.Lista_SubTramos.ForEach(Subtramo =>
                {
                    Estaciones.AddRange(Subtramo.Estaciones);
                });
                Tramo.Estaciones = Estaciones;
            });
        }
        private static List<cPuntoInfelxion> PuntosInfelxions(List<cTramo> Tramos, eUbicacionRefuerzo UBR)
        {
            List<cPuntoInfelxion> PIF = new List<cPuntoInfelxion>();

            CrearEstacionesTemporaneasTramos(Tramos);
            Tramos.ForEach(Tramo =>
            {
                List<cPuntoInfelxion> PIs = new List<cPuntoInfelxion>();
                for (int i = 0; i < Tramo.Estaciones.Count; i++)
                {
                    cEstacion EstaActual = Tramo.Estaciones[i];
                    cEstacion EstaAnterior = null; cEstacion EstaSig = null;
                    if (i - 1 >= 0)
                    {
                        EstaAnterior = Tramo.Estaciones[i - 1];
                    }

                    if (i + 1 < Tramo.Estaciones.Count)
                    {
                        EstaSig = Tramo.Estaciones[i + 1];
                    }

                    cPuntoInfelxion PI = PuntoInflexion(EstaAnterior, EstaActual, EstaSig, UBR);
                    if (PI != null)
                        PIs.Add(PI);
                }
                int ID = 0;
                PIs.ForEach(x => { x.ID = ID; ID += 1; });
                PIF.AddRange(PIs);
            });

            return PIF;
        }
        private static cPuntoInfelxion PuntoInflexion(cEstacion EstacionAnterior, cEstacion EstacionActual, cEstacion EstacionPosterior, eUbicacionRefuerzo ubicacionRefuerzo)
        {
            int I = ubicacionRefuerzo.GetHashCode();

            if (EstacionAnterior != null && EstacionPosterior != null) //Elemento del Medio
            {
                if (EstacionActual.Calculos.Envolvente.M3[I] == 0)
                {
                    if (EstacionPosterior.Calculos.Envolvente.M3[I] != 0)
                    {
                        return new cPuntoInfelxion(EstacionActual);
                    }
                    else if (EstacionAnterior.Calculos.Envolvente.M3[I] != 0)
                    {
                        return new cPuntoInfelxion(EstacionActual);
                    }
                }

            }
            else if (EstacionAnterior == null && EstacionPosterior != null)
            {
                if (EstacionActual.Calculos.Envolvente.M3[I] <= cVariables.MomentoMinimoPIExtremos && 
                    -cVariables.MomentoMinimoPIExtremos<= EstacionActual.Calculos.Envolvente.M3[I])
                {
                    if (EstacionPosterior.Calculos.Envolvente.M3[I] != 0)
                    {
                        return new cPuntoInfelxion(EstacionActual);
                    }
                }

            }
            else if (EstacionAnterior != null && EstacionPosterior == null)
            {

                if (EstacionActual.Calculos.Envolvente.M3[I] <= cVariables.MomentoMinimoPIExtremos &&
                    -cVariables.MomentoMinimoPIExtremos <= EstacionActual.Calculos.Envolvente.M3[I]) 
                { 
                    if (EstacionAnterior.Calculos.Envolvente.M3[I] != 0)
                    {
                        return new cPuntoInfelxion(EstacionActual);
                    }
                }

            }
            return null;
            //}
        }
        private static void CrearBarraContinuaInferior(List<IElemento> Elementos, int CantBarras, eNoBarra NoBarra, cTendencia tendencia, List<cPuntoInfelxion> PuntosInfelxion)
        {
            float LongitudTotal = Elementos.Sum(x => x.Longitud);
            float d1 = (tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.r1 + cDiccionarios.DiametrosBarras[NoBarra] / 2f + cVariables.DiametroEstriboPredeterminado) * cConversiones.Dimension_cm_to_m ;
            eUbicacionRefuerzo ubicacionRefuerzo = tendencia.UbicacionRefuerzo;
            float Xo = Elementos.Min(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X)) + cVariables.RExtremoIzquierdo;
            float Xf = Xo + tendencia.MaximaLongitud;
            float X = Elementos.Max(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X)) - cVariables.RExtremoDerecho;

            int Contador = 0;

            do
            {
                if (Xf - Xo > LongitudTotal)
                {
                    Xf = X;
                    CrearBarra2(NoBarra, ubicacionRefuerzo, CantBarras, Xo, Xf, ref tendencia, eTipoGancho.G90, eTipoGancho.G90);
                }
                else
                {
                    float MaximaLongitud2 = tendencia.MaximaLongitud - 2 * cDiccionarios.LDGancho(NoBarra, eTipoGancho.G90);
                    float MinimaLongitud = tendencia.MinimaLongitud;
                    for (int i = PuntosInfelxion.Count - 1; i >= 0; i--)
                    {
                        cPuntoInfelxion PI = PuntosInfelxion[i];
                        float d = PI.SubTramo.Seccion.H * cConversiones.Dimension_cm_to_m - d1;
                        float Traslapo = cDiccionarios.FindLdBarra(NoBarra, PI.SubTramo.Seccion.Material.fc, true);
                        float Delta = 0;
                        Xf = PI.CoordX;

                        if (PI.ID == 0)
                        {
                            Delta = Xf - PI.CordApoyoAsociada;
                            Xf = PI.CoordX - d;
                        }
                        else
                        {
                            Delta = PI.CordApoyoAsociada - Xf;
                            Xf = PI.CoordX + d + Traslapo;

                        }

                        if (Xf - Xo <= MaximaLongitud2 && Xf- Xo>= MinimaLongitud)
                        {
                            if (Delta >= d + Traslapo)
                            {

                                CrearBarra2(NoBarra, ubicacionRefuerzo, CantBarras, Xo, Xf, ref tendencia, eTipoGancho.None, eTipoGancho.None);
                                Xo = Xf - Traslapo;
                                Xf = Xo + tendencia.MaximaLongitud;
                                if (Xf > X)
                                {
                                    Xf = X;
                                    if (Xf - Xo <= MaximaLongitud2)
                                    {
                                        CrearBarra2(NoBarra, ubicacionRefuerzo, CantBarras, Xo, Xf, ref tendencia, eTipoGancho.None, eTipoGancho.G90); //Asingar Gancho Al Final
                                    }
                                    else
                                    {
                                        Xf = X - tendencia.MaximaLongitud;
                                        break;
                                    }
                                }
                                break;

                            }
                        }

                    }
                }

                if (Contador > 45)
                {
                    tendencia.EliminarBarras(y => y.Longitud < tendencia.MinimaLongitud);
                    tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.Resultados.Diseñado = false;
                    tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.Resultados.Errores.Add($"Longitud de barra máxima o mínima insuficiente en la {tendencia.Nombre}, {tendencia.UbicacionRefuerzo}.");
                    break;

                }
                Contador++;
            }
            while (Xf < X);
            if (tendencia.Barras.Count > 0)
            {
                tendencia.Barras.First().GanchoIzquierda = eTipoGancho.G90;
            }

        }
        private static void CrearBarraContinuaSuperior(List<IElemento> Elementos, int CantBarras, eNoBarra NoBarra, cTendencia tendencia, List<cPuntoInfelxion> PuntosInfelxion)
        {
            float LongitudTotal = Elementos.Sum(x => x.Longitud);
            float d1 = (tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.r1 + cDiccionarios.DiametrosBarras[NoBarra] / 2f+cVariables.DiametroEstriboPredeterminado) * cConversiones.Dimension_cm_to_m;
            eUbicacionRefuerzo ubicacionRefuerzo = tendencia.UbicacionRefuerzo;
            float Xo = Elementos.Min(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X)) + cVariables.RExtremoIzquierdo;
            float Xf = Xo + tendencia.MaximaLongitud;
            float X = Elementos.Max(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X)) - cVariables.RExtremoDerecho;
            int Contador = 0;
            do
            {
                if (Xf - Xo > LongitudTotal)
                {
                    Xf = X;
                    CrearBarra2(NoBarra, ubicacionRefuerzo, CantBarras, Xo, Xf, ref tendencia, eTipoGancho.G90, eTipoGancho.G90);
                }
                else
                {
                    float MaximaLongitud2 = tendencia.MaximaLongitud - 2 * cDiccionarios.LDGancho(NoBarra, eTipoGancho.G90);
                    float MinimaLongitud = tendencia.MinimaLongitud;
                    for (int i = PuntosInfelxion.Count - 1; i >= 0; i--)
                    {
                        cPuntoInfelxion PI = PuntosInfelxion[i];
                        float d = PI.SubTramo.Seccion.H * cConversiones.Dimension_cm_to_m - d1;
                        float Traslapo = cDiccionarios.FindLdBarra(NoBarra, PI.SubTramo.Seccion.Material.fc, true);
                        Xf = PI.CoordX;

                        if (PI.ID == 0)
                        {

                            Xf = PI.CoordX + d;
                        }
                        else
                        {
                            Xf = PI.CoordX - d - Traslapo;

                        }

                        if (Xf - Xo <= MaximaLongitud2 && Xf-Xo>= MinimaLongitud)
                        {

                            CrearBarra2(NoBarra, ubicacionRefuerzo, CantBarras, Xo, Xf, ref tendencia, eTipoGancho.None, eTipoGancho.None);
                            Xo = Xf - Traslapo;
                            Xf = Xo + tendencia.MaximaLongitud;
                            if (Xf > X)
                            {
                                Xf = X;
                                if (Xf - Xo <= MaximaLongitud2)
                                {
                                    CrearBarra2(NoBarra, ubicacionRefuerzo, CantBarras, Xo, Xf, ref tendencia, eTipoGancho.None, eTipoGancho.G90); //Asingar Gancho Al Final
                                }
                                else
                                {
                                    Xf = X - tendencia.MaximaLongitud;
                                    break;
                                }
                            }
                            break;


                        }

                    }
                }

                if (Contador > 30)
                {
                    tendencia.EliminarBarras(y => y.Longitud < tendencia.MinimaLongitud);
                    tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.Resultados.Diseñado = false;
                    tendencia.Tendencia_Refuerzo_Origen.NervioOrigen.Resultados.Errores.Add($"Longitud de barra máxima o mínima insuficiente en la {tendencia.Nombre}, {tendencia.UbicacionRefuerzo}.");
                    break;
                }
                Contador++;

            }
            while (Xf < X);
            if (tendencia.Barras.Count > 0)
            {
                tendencia.Barras.First().GanchoIzquierda = eTipoGancho.G90;
            }
        }
        private static void CrearBarra2(eNoBarra noBarra, eUbicacionRefuerzo ubicacionRefuerzo, int CantBarras, float Xo, float Xf, ref cTendencia Tendencia, eTipoGancho GI, eTipoGancho GD)
        {
            cBarra BarraInicial; int ID = 0;
            if (Tendencia.Barras.Count > 0)
            {
                ID = Tendencia.Barras.Max(x => x.ID) + 1;
            }
            BarraInicial = new cBarra(ID, Tendencia, noBarra, ubicacionRefuerzo, CantBarras, Xo, Xf);
            BarraInicial.GanchoDerecha = GD;
            BarraInicial.GanchoIzquierda = GI;
            Tendencia.AgregarBarra(BarraInicial);
        }
        private static eNoBarra ProponerUnaBarra(float As, List<eNoBarra> Barras, cNervio Nervio)
        {
            List<eNoBarra> BarrasFinales = new List<eNoBarra>();
            Barras.ForEach(Barra =>
            {

                float Delta = cDiccionarios.AceroBarras[Barra] - As;
                float Porcenaje = (cDiccionarios.AceroBarras[Barra] - As) / cDiccionarios.AceroBarras[Barra] * 100f;
                if (Delta > 0 || Delta < 0 && Math.Abs(Porcenaje) <= cVariables.ToleranciaFlexion)
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
        private static List<eNoBarra> ProponerDosBarras(float As, List<eNoBarra> BarrasBase, List<eNoBarra> BarrasAdicionales, cNervio Nervio)
        {
            List<Tuple<Tuple<eNoBarra, eNoBarra>, float>> NoBarrasDelta = new List<Tuple<Tuple<eNoBarra, eNoBarra>, float>>();

            BarrasBase.ForEach(Barra =>
            {

                BarrasAdicionales.ForEach(Barra1 =>
                {
                    if (Math.Abs(Convert.ToInt32(Barra.ToString().Replace("B", "")) - Convert.ToInt32(Barra1.ToString().Replace("B", ""))) <= 2)
                    {
                        float Delta = (cDiccionarios.AceroBarras[Barra] + cDiccionarios.AceroBarras[Barra1]) - As;
                        float Porcenaje = ((cDiccionarios.AceroBarras[Barra] + cDiccionarios.AceroBarras[Barra1]) - As) / cDiccionarios.AceroBarras[Barra] * 100f;
                        if (Delta > 0 || Delta < 0 && Math.Abs(Porcenaje) <= cVariables.ToleranciaFlexion)
                            NoBarrasDelta.Add(new Tuple<Tuple<eNoBarra, eNoBarra>, float>(new Tuple<eNoBarra, eNoBarra>(Barra, Barra1), Math.Abs(Delta)));
                    }
                });
            });

            try
            {
                Tuple<eNoBarra, eNoBarra> Barras = NoBarrasDelta.Find(x => x.Item2 == NoBarrasDelta.Min(y => y.Item2)).Item1;
                return new eNoBarra[] { Barras.Item1, Barras.Item2 }.ToList();
            }
            catch
            {
                EventoVentanaEmergente?.Invoke($"No se encontró el refuerzo necesario para las solicitaciones del Nervio: {Nervio.Nombre}", MessageBoxIcon.Error);
                return null;
            }


        }

        #endregion


        private static void ControlErroresDiseño(cNervio Nervio, cTendencia tendencia)
        {
            bool FaltaI = false; bool FaltaS = false;
            foreach (IElemento Eleme in Nervio.Lista_Elementos)
            {
                if (Eleme is cSubTramo)
                {
                    cSubTramo subTramo = (cSubTramo)Eleme;
                    float FaltanteI = subTramo.Estaciones.Min(x => x.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior);
                    float PorcenajeI = subTramo.Estaciones.Find(y => y.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior == FaltanteI).Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior;

                    //Inferior
                    if (FaltanteI < 0 && Math.Abs(PorcenajeI) > cVariables.ToleranciaFlexion)
                    {
                        FaltaI = true;
                    }

                    float FaltanteS = subTramo.Estaciones.Min(x => x.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior);
                    float PorcenajeS = subTramo.Estaciones.Find(y => y.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior == FaltanteS).Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior;
                    if (FaltanteS < 0 && Math.Abs(PorcenajeS) > cVariables.ToleranciaFlexion)
                    {
                        FaltaS = true;
                    }

                    if (FaltaI | FaltaS)
                        break;
                }
            }
            if (FaltaI)
            {
                Nervio.Resultados.Diseñado = false;
                Nervio.Resultados.Errores.Add($"Se debe suplir mayor cantidad de refuerzo, {tendencia.Nombre}, Inferior");
            }

            if (FaltaS)
            {
                Nervio.Resultados.Diseñado = false;
                Nervio.Resultados.Errores.Add($"Se debe suplir mayor cantidad de refuerzo, {tendencia.Nombre}, Superior");
            }
        }
        private static void ControlErroresApoyos(cNervio Nervio)
        {
            if (Nervio.Lista_Elementos.First() is cSubTramo) // Voladizo
            {
                cSubTramo Subtramo = (cSubTramo)Nervio.Lista_Elementos.First();
                bool Voladizo = false;
                foreach (cEstacion Estacion in Subtramo.Estaciones)
                {
                    if ((float)Math.Round(Estacion.Calculos.Envolvente.M3[0], 2) == 0)
                    {
                        Voladizo = true;
                    }
                    else
                    {
                        Voladizo = false; break;
                    }
                }

                if (!Voladizo)
                {
                    Nervio.Resultados.Errores.Add($"Se debe asignar el primer apoyo.");
                    Nervio.Resultados.Diseñado = false;
                }

            }
            if (Nervio.Lista_Elementos.Last() is cSubTramo) // Voladizo
            {
                cSubTramo Subtramo = (cSubTramo)Nervio.Lista_Elementos.Last();
                bool Voladizo = false;
                foreach (cEstacion Estacion in Subtramo.Estaciones)
                {
                    if ((float)Math.Round(Estacion.Calculos.Envolvente.M3[0], 2) == 0)
                    {
                        Voladizo = true;
                    }
                    else
                    {
                        Voladizo = false; break;
                    }
                }
                if (!Voladizo)
                {
                    Nervio.Resultados.Errores.Add($"Se debe asignar el último apoyo.");
                    Nervio.Resultados.Diseñado = false;
                }

            }

        }

  

        public static float InterpolacionY(PointF P1, PointF P2, float X)
        {
            return P1.Y + ((X - P1.X) / (P2.X - P1.X) * (P2.Y - P1.Y));
        }
        public static float InterpolacionY(float Xo, float Yo, float X1, float Y1, float X)
        {
            return Yo + ((X - Xo) / (X1 - Xo) * (Y1 - Yo));
        }


        #endregion

        private static void GraficarNervioAutoCAD(cNervio Nervio, float X, float Y)
        {
            Nervio.GraficarEnAutoCAD(X, Y);

        }
        public static void GraficarNervios(List<cNervio> Nervios)
        {

            cNervio NervioInicial = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect;
            Nervios = Nervios.FindAll(y => !y.SimilitudNervioCompleto.BoolSoySimiarA);
            double[] CoordXYZ = new double[] { };
            FunctionsAutoCAD.OpenAutoCAD();
            EventoVentanaEmergente("Posicione el puntero en AutoCAD.", MessageBoxIcon.Information);
            FunctionsAutoCAD.GetPoint(ref CoordXYZ);
            FunctionsAutoCAD.SetScale("1:75");
            if (CoordXYZ.Length > 0)
            {

                float Y = (float)CoordXYZ[1];
                Nervios= OrdenarNervios(Nervios);
                foreach (cNervio Nervio in Nervios)
                {
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = Nervio;
                    GraficarNervioAutoCAD(Nervio, (float)CoordXYZ[0], Y);
                    float H = Nervio.Lista_Elementos.Max(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.Y)) - Nervio.Lista_Elementos.Min(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.Y));
                    Y += -H - cVariables.DeltaEntreNervios;
                }
                if (Nervios.Count > 1)
                {
                    EventoVentanaEmergente("Nervios graficados con Éxito.", MessageBoxIcon.Information);
                }
                else
                {
                    EventoVentanaEmergente("Nervio graficado con Éxito.", MessageBoxIcon.Information);
                }
            }

            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = NervioInicial;
        }
        
        public static void GraficarPlantaAutoCAD(cProyecto proyecto)
        {
            var PisoSelecEdificio = proyecto.Edificio.PisoSelect;
            var PisoSelectDatosEtabs = proyecto.DatosEtabs.Lista_Pisos.Find(y => y.Nombre == PisoSelecEdificio.Nombre);

            FunctionsAutoCAD.OpenAutoCAD();
            VentanaEmergenteInformacion("Posicione el puntero en AutoCAD.");
            try
            {
                double[] CoordXYZ = new double[] { };
                FunctionsAutoCAD.GetPoint(ref CoordXYZ);
                FunctionsAutoCAD.SetScale("1:50");
                float X = (float)CoordXYZ[0]; float Y = (float)CoordXYZ[1];

                PisoSelectDatosEtabs.DrawAutoCAD_LinesSelect(X, Y);
                PisoSelecEdificio.DrawAutoCAD_Nervios(X, Y);
                VentanaEmergenteInformacion("Planta graficada con éxito.");
            }
            catch
            {
            }
        }



        public static void DiseñarNervios(List<cNervio> Nervios, eTipoRefuerzo Disenar)
        {
            Notificador("Diseñando...");
            cNervio NervioInicial = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect;
            Nervios.ForEach(Nervio => { Nervio.Resultados.Diseñado = true; Nervio.Resultados.Errores.Clear(); });

            Nervios= Nervios.FindAll(y => !y.SimilitudNervioGeometria.BoolSoySimiarA);

            Nervios.AddRange(Nervios.SelectMany(y => y.SimilitudNervioCompleto.NerviosSimilares).ToList());

            foreach (cNervio Nervio in Nervios)
            {
                for (int i = 0; i < Nervio.Tendencia_Refuerzos.TendenciasInferior.Count; i++)
                {
                    Nervio.Tendencia_Refuerzos.TInfeSelect = Nervio.Tendencia_Refuerzos.TendenciasInferior[i];
                    Nervio.Tendencia_Refuerzos.TSupeSelect = Nervio.Tendencia_Refuerzos.TendenciasSuperior[i];
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = Nervio;

                    switch (Disenar)
                    {
                        case eTipoRefuerzo.Longitudinal:
                            try
                            {
                                DiseñarNervioFlexion(Nervio);
                            }
                            catch {    }
                            break;
                        case eTipoRefuerzo.Transversal:
                            //try
                            //{
                                DiseñarCorte(Nervio);
                            //}
                            //catch { }
                    break;
                        case eTipoRefuerzo.Todo:
                            try
                            {
                                DiseñarNervioFlexion(Nervio);
                            }
                            catch
                            {

                            }
                            //try
                            //{
                                DiseñarCorte(Nervio);
                            //}
                            //catch
                            //{

//                            }
                            break;
                        default:
                            break;
                    }
                }

                ControlErroresDiseño(Nervio, Nervio.Tendencia_Refuerzos.TInfeSelect);
                ControlErroresApoyos(Nervio);
            }

            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = NervioInicial;
            

            if (Nervios.Count > 1)
            {
                EventoVentanaEmergente("Nervios Diseñados con Éxito.", MessageBoxIcon.Information);
            }
            else
            {
                EventoVentanaEmergente("Nervio Diseñado con Éxito.", MessageBoxIcon.Information);
            }
            Notificador("Listo");
        }

        public static void RevisarNervios(List<cNervio> Nervios)
        {
            Nervios.ForEach(Nervio => { Nervio.Resultados.Diseñado = true; Nervio.Resultados.Errores.Clear(); });
            foreach (cNervio Nervio in Nervios)
            {
                foreach (cTendencia tendencia in Nervio.Tendencia_Refuerzos.TendenciasInferior)
                {
                    ControlErroresDiseño(Nervio, tendencia);
                }
                ControlErroresApoyos(Nervio);
            }
        }

        public static void AsignarSimilitud(cNervio NervioMaestro, cNervio NervioSimilar, ref List<string> MensajeAlerta,bool Geometria=true)
        {
            cSimilitudNervio Similar = NervioSimilar.SimilitudNervioGeometria;
            if (!Geometria)            
                Similar = NervioSimilar.SimilitudNervioCompleto;
            

            if (NervioMaestro != null)
            {
                cSimilitudNervio Maestro = NervioMaestro.SimilitudNervioGeometria;
                if (!Geometria)
                    Maestro = NervioMaestro.SimilitudNervioCompleto;

                if (Maestro.Similares_List_SimilarA == null)
                {
                    Maestro.Similares_List_SimilarA = new List<cSimilitudNervio.cSimilar>();
                }

                bool SimilarBool = AplicarSimilitud(NervioMaestro, NervioSimilar);

                if (SimilarBool)
                {
                    Maestro.Similares_List_SimilarA.Add(new cSimilitudNervio.cSimilar(NervioSimilar.Nombre, NervioSimilar.PisoOrigen.Nombre));
                    Similar.SoySimiarA = new cSimilitudNervio.cSimilar(NervioMaestro.Nombre, NervioMaestro.PisoOrigen.Nombre);
                    Similar.BoolSoySimiarA = true;
                }
                else
                {
                    Similar.BoolSoySimiarA = false;
                    MensajeAlerta.Add($"{NervioSimilar.Nombre} | {NervioSimilar.PisoOrigen.Nombre} "
                        + $"no posee similitudes con {NervioMaestro.Nombre} | {NervioMaestro.PisoOrigen.Nombre}");
                }

            }
            else
            {
                Similar.BoolSoySimiarA = false;
                //NervioSimilar.Maestro.SimilaresG_String = null;
            }
        }

        public static bool AplicarSimilitud(cNervio NervioMaestro, cNervio NervioSimilar)
        {
            bool Similar = false;
            if (NervioMaestro.Lista_Tramos.Count == NervioSimilar.Lista_Tramos.Count &&
                NervioMaestro.Lista_Elementos.FindAll(y => y is cSubTramo).Count == NervioSimilar.Lista_Elementos.FindAll(y => y is cSubTramo).Count)
            {
                int c = 0;

                foreach (cTramo TramoMaestro in NervioMaestro.Lista_Tramos)
                {
                    if (Math.Abs(TramoMaestro.Longitud - NervioSimilar.Lista_Tramos[c].Longitud) > cVariables.ToleranciaLSimilar)
                    {
                        Similar = false;
                        break;
                    }
                    else
                    {
                        Similar = true;

                    }

                    c += 1;
                }
            }
            return Similar;
        }


        public static void DesacoplarSimilitudMaestro(cNervio Nervio)
        {
            cSimilitudNervio Similitud = Nervio.SimilitudNervioGeometria;
            if (!Similitud.IsMaestro && !Similitud.BoolSoySimiarA)
                Similitud = Nervio.SimilitudNervioCompleto;

            if (Similitud.IsMaestro) 
            {
                Similitud.NerviosSimilares.ForEach(y => {
                    cSimilitudNervio Similitud2 = y.SimilitudNervioGeometria;
                    if (!Similitud2.IsMaestro && !Similitud2.BoolSoySimiarA)
                        Similitud2 = y.SimilitudNervioCompleto;
                    Similitud2.BoolSoySimiarA = false;
                    y.CrearEnvolvente();
                    y.BloquearNervio = false;
                });

                Similitud.Similares_List_SimilarA = null;
                Nervio.CrearEnvolvente();
            }
            else
            {
                Similitud.BoolSoySimiarA = false;
            }
            Nervio.BloquearNervio = false;
        }

        public static List<cNervio> OrdenarNervios(List<cNervio> nervios)
        {
            List<cNervio> NerviosOrdenados = new List<cNervio>();
            List<cNervio> DiagonalesHorizontales = nervios.FindAll(y => (y.Direccion == eDireccion.Horizontal || y.Direccion == eDireccion.Diagonal) && !y.NombrarNervioDiferente);
            List<cNervio> DiagonalesVerticales = nervios.FindAll(y => y.Direccion == eDireccion.Vertical && !y.NombrarNervioDiferente);

            List<cNervio> DiagonalesHorizontales2 = nervios.FindAll(y => (y.Direccion == eDireccion.Horizontal || y.Direccion == eDireccion.Diagonal) && y.NombrarNervioDiferente);
            List<cNervio> DiagonalesVerticales2 = nervios.FindAll(y => y.Direccion == eDireccion.Vertical && y.NombrarNervioDiferente);


            OdernarNervios(ref DiagonalesHorizontales); OdernarNervios(ref DiagonalesVerticales);

            NerviosOrdenados.AddRange(DiagonalesHorizontales); NerviosOrdenados.AddRange(DiagonalesVerticales);
            NerviosOrdenados.AddRange(DiagonalesHorizontales2); NerviosOrdenados.AddRange(DiagonalesVerticales2);

            return NerviosOrdenados;
        }

        private static void OdernarNervios(ref List<cNervio> nervios)
        {
            nervios= nervios.OrderBy
                        (y =>
                        {
                            object Variable = y.Nombre;
                            if (y.Nombre.Replace(y.Prefijo, "").IsNumeric())
                            {
                                Variable = int.Parse(y.Nombre.Replace(y.Prefijo, ""));
                            }
                            return Variable;
                        }).ToList();
        }


        #region Exportar DL NET NIMBUS
        public static void CrearArhivotxtDLNET(List<cNervio> Nervios, string Ruta)
        {
            if (Ruta != "")
            {
                List<string> Archivo = new List<string>();

                Archivo.Add(Nervios.Count.ToString());
                Nervios.ForEach(Nervio =>
                {
                    Archivo.Add(Nervio.Nombre);
                    Archivo.Add(1.ToString());//Cantidad de Estribos
                    List<string> Barras = NomenclaturaDLNETBarrasNervio(Nervio);
                    List<string> Estribos = NomenclaturaDLNETEstribosNervio(Nervio);
                    int FilasRefuerzos = Barras.Count + Estribos.Count;
                    Archivo.Add(FilasRefuerzos.ToString());
                    Archivo.AddRange(Barras);
                    Archivo.AddRange(Estribos);
                });

                //Escritor
                StreamWriter Writer = new StreamWriter(Ruta);
                Archivo.ForEach(Line => Writer.WriteLine(Line));
                Writer.Close();
                System.Diagnostics.Process Process = new System.Diagnostics.Process();
                Process.StartInfo.FileName = Ruta;
                Process.Start();
            }
        }

        private static List<string> NomenclaturaDLNETEstribosNervio(cNervio Nervio)
        {
            List<Tuple<int, string>> ListaaDepurar = new List<Tuple<int, string>>();
            List<string> ListaFinal = new List<string>();
            Nervio.Lista_Tramos.ForEach(Tramo =>
            {

                if (Tramo.EstribosDerecha != null)
                {
                    Tramo.EstribosDerecha.Zona1.Estribos.ForEach(x => ListaaDepurar.Add(new Tuple<int, string>(1, NomenclaturaDLNETEstribo(x))));
                    Tramo.EstribosDerecha.Zona2.Estribos.ForEach(x => ListaaDepurar.Add(new Tuple<int, string>(1, NomenclaturaDLNETEstribo(x))));
                }
                if (Tramo.EstribosIzquierda != null)
                {
                    Tramo.EstribosIzquierda.Zona1.Estribos.ForEach(x => ListaaDepurar.Add(new Tuple<int, string>(1, NomenclaturaDLNETEstribo(x))));
                    Tramo.EstribosIzquierda.Zona2.Estribos.ForEach(x => ListaaDepurar.Add(new Tuple<int, string>(1, NomenclaturaDLNETEstribo(x))));
                }
            });

            RetornarListaDepurada(ref ListaFinal, ListaaDepurar);
            return ListaFinal;
        }
        private static string NomenclaturaDLNETEstribo(cEstribo Estribo)
        {
            string Text = "";
            if (Estribo.Ramas == 1)
            {
                Text = $" G  {ConvertireNoBarraToString(Estribo.NoBarra)}  {string.Format("{0:0.00}", Estribo.H)}   G{string.Format("{0:0.00}", Estribo.LGancho)}   F0/0";
            }
            else if (Estribo.Ramas == 2)
            {
                Text = $" E  {ConvertireNoBarraToString(Estribo.NoBarra)}  {string.Format("{0:0.00}", Estribo.B)}*{string.Format("{0:0.00}", Estribo.H)}   G{string.Format("{0:0.00}", Estribo.LGancho)}  F0/45";
            }
            return Text;
        }
        private static List<string> NomenclaturaDLNETBarrasNervio(cNervio Nervio)
        {
            List<Tuple<int, string>> ListaaDepurar = new List<Tuple<int, string>>();
            List<string> ListaFinal = new List<string>();
            Nervio.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => ListaaDepurar.Add(new Tuple<int, string>(1, NomenclaturaDLNETBarra(x))));
            Nervio.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => ListaaDepurar.Add(new Tuple<int, string>(1, NomenclaturaDLNETBarra(x))));
            RetornarListaDepurada(ref ListaFinal, ListaaDepurar);
            return ListaFinal;
        }
        private static string NomenclaturaDLNETBarra(cBarra Barra)
        {
            return $" {ConvertireNoBarraToString(Barra.NoBarra)}  " +
                $"{string.Format("{0:0.00}", Barra.Longitud)}" +
                $"{NomenclaturaDLNETBarra(Barra.GanchoIzquierda, Barra.LGanchoIzquierda)}" +
                $"{NomenclaturaDLNETBarra(Barra.GanchoDerecha, Barra.LGanchoDerecha)}";
        }
        private static string NomenclaturaDLNETBarra(eTipoGancho Gancho, float LG)
        {
            if (Gancho == eTipoGancho.G180)
                return $"  L{string.Format("{0:0.00}", LG)}";
            else if (Gancho == eTipoGancho.G90)
                return $"  U{string.Format("{0:0.00}", LG)}";
            else
                return $"";
        }

        private static void RetornarListaDepurada(ref List<string> Lista_DllNetFinal, List<Tuple<int, string>> ListaaDepurar)
        {
            List<int> VectorIndices = new List<int>();

            for (int i = 0; i < ListaaDepurar.Count; i++)
            {
                int Cantidad1 = ListaaDepurar[i].Item1;
                string Nomenclatura1 = ListaaDepurar[i].Item2;

                if (VectorIndices.Exists(x => x == i) == false)
                {
                    for (int j = i + 1; j < ListaaDepurar.Count; j++)
                    {
                        string Nomenclatura2 = ListaaDepurar[j].Item2;
                        if (Nomenclatura1 == Nomenclatura2)
                        {
                            Cantidad1 += ListaaDepurar[j].Item1;
                            VectorIndices.Add(j);
                        }
                    }
                    if (Cantidad1 != 0)
                    {
                        Lista_DllNetFinal.Add(Cantidad1 + Nomenclatura1);
                    }
                }
            }
        }
        #endregion

        public static void CheckCPUUsageAndSleepThread(PerformanceCounter cpuCounter)
        {
            if (cpuCounter.NextValue() > 80)
            {
                Thread.Sleep(F_Base.PropiedadesPrograma.TimerSleep);
            }
        }

        public static void AbrirUnidadGoogleDrive()
        {
            bool ExisteUnidad = false;
            string PathDrive1 = string.Empty;
            string PathDrive2 = string.Empty;
            var Drives = DriveInfo.GetDrives();

            foreach (var drive in Drives)
            {
                if (drive.VolumeLabel == Program._GOOGLEDRIVE)
                {
                    ExisteUnidad = true;
                    PathDrive1 = Path.Combine(drive.RootDirectory.FullName, "Mi unidad", "Instaladores", "Diseño de Nervios");
                    PathDrive2 = Path.Combine(drive.RootDirectory.FullName, "Mi unidad");
                    break;
                }
            }

            if (ExisteUnidad)
            {
                try
                {
                    Process.Start(PathDrive1);
                }
                catch
                {
                    try
                    {
                        Process.Start(PathDrive2);
                    }
                    catch
                    {

                    }
                }
            }


        }

        private static void ActualizarTendenciasEnNervios(cProyecto proyecto)
        {
            cPiso PisoSelect = proyecto.Edificio.PisoSelect;
            cNervio NervioSelect = proyecto.Edificio.PisoSelect.NervioSelect;
            proyecto.Edificio.Lista_Pisos.ForEach(P =>
            {
                proyecto.Edificio.PisoSelect = P;
                P.Nervios.ForEach(N => N.Tendencia_Refuerzos.NervioOrigen = N);
            });
            proyecto.Edificio.PisoSelect = PisoSelect;
            proyecto.Edificio.PisoSelect.NervioSelect = NervioSelect;
            proyecto.VersionPrograma = Program.Version;
        }

        public static void ActualizarEstribosVersionesAnteriores(cProyecto proyecto)
        {
            cPiso PisoSelect = proyecto.Edificio.PisoSelect;
            cNervio NervioSelect = proyecto.Edificio.PisoSelect.NervioSelect;
            proyecto.Edificio.Lista_Pisos.ForEach(P =>
            {
                proyecto.Edificio.PisoSelect = P;
                ActualizarEstribosDeVersionesAnteriores(P.Nervios,P);
            });
            proyecto.Edificio.PisoSelect = PisoSelect;
            proyecto.Edificio.PisoSelect.NervioSelect = NervioSelect;
            proyecto.VersionPrograma = Program.Version;
        }
        private static void ActualizarEstribosDeVersionesAnteriores(List<cNervio> nervios,cPiso Piso)
        {
            nervios.ForEach(N =>
            {
                N.CrearEnvolvente();
                Piso.NervioSelect = N;
                N.Tendencia_Refuerzos.TEstriboSelect.EliminarBloquesEstribos();
                N.Lista_Tramos.ForEach(T =>
                {
                    if (T.EstribosIzquierda!= null)
                    {
                        ConvertirZonaEstribosEnEstribosActuales(N.Tendencia_Refuerzos.TEstriboSelect, T.EstribosIzquierda.Zona1);
                        ConvertirZonaEstribosEnEstribosActuales(N.Tendencia_Refuerzos.TEstriboSelect, T.EstribosIzquierda.Zona2);
                    }
                    if (T.EstribosDerecha != null)
                    {
                        ConvertirZonaEstribosEnEstribosActuales(N.Tendencia_Refuerzos.TEstriboSelect, T.EstribosDerecha.Zona1);
                        ConvertirZonaEstribosEnEstribosActuales(N.Tendencia_Refuerzos.TEstriboSelect, T.EstribosDerecha.Zona2);
                    }
                    T.EliminarRefuerzoTransversal();
                });

            });



        }

        private static void ConvertirZonaEstribosEnEstribosActuales(cTendencia_Estribo TE,cZonaEstribos zonaEstribos)
        {
            if (zonaEstribos.Estribos.Count > 0)
            {
                float X = zonaEstribos.Estribos.First().CoordX;
                int Cantidad = zonaEstribos.Cantidad;
                float S = zonaEstribos.Separacion;
                eLadoDeZona DireccionEstribo = eLadoDeZona.Izquierda;
                if (zonaEstribos.LadoDeZona == eLadoDeZona.Izquierda)
                {
                    DireccionEstribo = eLadoDeZona.Derecha;
                }
                CrearBloqueEstribos(TE, S, Cantidad, zonaEstribos.NoBarra, 1, X, DireccionEstribo);
            }
            
        }


    }
}