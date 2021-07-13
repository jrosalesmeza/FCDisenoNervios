using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using FC_BFunctionsAutoCAD;
using OpenTK.Graphics.OpenGL;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cNervio
    {

        public float Pendiente()
        {
            return (Lista_Elementos.Find(y => y.Soporte == eSoporte.Vano) as cSubTramo).Lista_Lineas.First().ConfigLinea.Pendiente();
        }
        
        public static Pen PenBloquearNervio=  new Pen(Color.Gray, 3);
        private bool bloquearNevio = false;
        public bool BloquearNervio
        {

            get
            {
                return bloquearNevio;
            }
            set
            {
                if (bloquearNevio != value)
                {
                    bloquearNevio = value;
                }
            }
        }

        public string NombreAnterior { get; set; }
        public bool NombrarNervioDiferente { get; set; } = false;

        public float EscalaMayorenX;
        public float EscalaMayorenY;
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string NombreSinPrefijo { get; set; }
        public string Prefijo { get; set; } = "N-";
        public int CantApoyos { get; set; } = 0;

        public bool ActivarBoolNervioBorde { get; set; }
        private bool nervioBorde = false;
        public bool NervioBorde
        {
            get { return nervioBorde; }
            set
            {
                if (nervioBorde != value)
                {
                    nervioBorde = value;
                }
            }
        }

        public float r1_ = cVariables.RecubrimientoNervios;
        public float r1
        {
            get { return r1_; }
            set
            {
                if (value != r1_)
                {
                    r1_ = value;
                    CrearEnvolvente();
                    CrearAceroAsignadoRefuerzoLongitudinal();
                    AsignarCambiosANerviosRecubrimientoSimilares();
                }
            }
        }

        public float r2_= cVariables.RecubrimientoNervios;

        public float r2
        {
            get { return r2_; }
            set
            {
                if (value != r2_)
                {
                    r2_ = value;
                    CrearEnvolvente();
                    CrearAceroAsignadoRefuerzoLongitudinal();
                    AsignarCambiosANerviosRecubrimientoSimilares();
                }
            }
        }
        public float Longitud { get; set; }
        public eDireccion Direccion { get; set; }
        public List<cTramo> Lista_Tramos { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }
        private List<cGrid> grid;
        public List<cGrid> Grids
        {

            get { return grid; }
            set
            {
                if (value != grid)
                {
                    if (CantApoyos > 0)
                    {
                        grid = value;
                        ModificarGridsCoordenadasReales();
                    }
                }
            }


        }
        public List<IElemento> Lista_Elementos { get; set; }
        public bool Bool_CambioAltura { get; set; }
        public bool Bool_CambioAncho { get; set; }
        public bool Select { get; set; } 
        public bool SelectPlantaEnumeracion { get; set; }
        private bool ElementoEnumerado_MouseMove = false;
        public cObjeto ObjetoSelect { get; set; }
        public cTendencia_Refuerzo Tendencia_Refuerzos { get; set; } = new cTendencia_Refuerzo();
        public cPiso PisoOrigen { get; set; }

        public float PesoTotalRefuerzoTransversal { get; set; }

        public cSimilitudNervio SimilitudNervioGeometria { get; set; } = new cSimilitudNervio();
        public cSimilitudNervio SimilitudNervioCompleto { get; set; } = new cSimilitudNervio();

        public cPropiedadesDisenoAutoCAD Propiedades { get; set; } = new cPropiedadesDisenoAutoCAD(); 

        private eCambioenAltura cambioenAltura;
        private eCambioenAncho cambioenAncho;

        public eCambioenAltura CambioenAltura
        {
            get { return cambioenAltura; }
            set
            {
                if (value != cambioenAltura)
                {
                    cambioenAltura = value;
                    CrearCoordenadasPerfilLongitudinalReales();
                    CrearCoordenadasPerfilLongitudinalAutoCAD();
                    AsignarCambiosANerviosSimilares();
                }
            }
        }

        public eCambioenAncho CambioenAncho
        {
            get { return cambioenAncho; }
            set
            {
                if (value != cambioenAncho)
                {
                    // CrearCoordenadasPerfilLongitudinalReales();
                }
                cambioenAncho = value;
            }
        }

        public cResultados Resultados { get; set; } = new cResultados();

        public cNervio(int ID, string Prefijo, List<cObjeto> Lista_Objetos, eDireccion Direccion, List<cGrid> Grids, cPiso PisoOrigen)
        {
            this.ID = ID;
            if (Prefijo != "")
            {
                this.Prefijo = Prefijo;
            }
            this.Lista_Objetos = Lista_Objetos;
            this.Lista_Objetos.ForEach(x => { if (x.Soporte == eSoporte.Apoyo) { CantApoyos += 1; } });
            this.Direccion = Direccion;
            this.PisoOrigen = PisoOrigen; Tendencia_Refuerzos.NervioOrigen = this;
            AsignarCambioAlturayCambioAnchoObjetos();
            CrearTramos();
            CrearElementos();

            Tendencia_Refuerzos.TendenciasInferior.Add(cFunctionsProgram.CrearTendenciaDefault(1, Tendencia_Refuerzos, eUbicacionRefuerzo.Inferior)); Tendencia_Refuerzos.t_InfeSelect = Tendencia_Refuerzos.TendenciasInferior.First();
            Tendencia_Refuerzos.TendenciasSuperior.Add(cFunctionsProgram.CrearTendenciaDefault(1, Tendencia_Refuerzos, eUbicacionRefuerzo.Superior)); Tendencia_Refuerzos.t_Supeselect = Tendencia_Refuerzos.TendenciasSuperior.First();
            Tendencia_Refuerzos.TendenciasEstribos.Add(cFunctionsProgram.CrearTendenciaEstriboDefault(1, Tendencia_Refuerzos)); Tendencia_Refuerzos.t_estriboSelect = Tendencia_Refuerzos.TendenciasEstribos.First();
            if (CantApoyos > 0)
            {
                CrearCoordenadasPerfilLongitudinalReales();
                CrearCoordenadasPerfilLongitudinalAutoCAD();
                this.Grids = Grids;
                CrearEnvolvente();
                CrearAceroAsignadoRefuerzoLongitudinal();
                CrearAceroAsignadoRefuerzoTransversal();
                AsignarMaximaLongitudTendencias();
            }


        }

        public void Cambio_Nombre(string Nombre_)
        {
            Nombre = Prefijo + Nombre_;
            NombreSinPrefijo = Nombre_;
            //Tendencia_Refuerzos.NombreNervioOrigen = Nombre;
        }

        private void ModificarGridsCoordenadasReales(float BubbleSize = 0.25f)
        {
            Grids.ForEach(Grid =>
            {
                cObjeto PrimerObjeto = Lista_Objetos.First();
                if (Direccion == eDireccion.Horizontal | Direccion == eDireccion.Diagonal)
                {
                    if (PrimerObjeto.Soporte == eSoporte.Apoyo)
                    {
                        Grid.CoordenadaInicial = Grid.CoordenadaInicial - PrimerObjeto.Line.ConfigLinea.Point1P.X + PrimerObjeto.Line.Seccion.B * cConversiones.Dimension_cm_to_m / 2;
                    }
                    else
                    {
                        Grid.CoordenadaInicial = Grid.CoordenadaInicial - PrimerObjeto.Line.ConfigLinea.Point1P.X;
                    }
                }
                else
                {
                    if (PrimerObjeto.Soporte == eSoporte.Apoyo)
                    {
                        Grid.CoordenadaInicial = Grid.CoordenadaInicial - PrimerObjeto.Line.ConfigLinea.Point1P.Y + PrimerObjeto.Line.Seccion.B * cConversiones.Dimension_cm_to_m / 2;
                    }
                    else
                    {
                        Grid.CoordenadaInicial = Grid.CoordenadaInicial - PrimerObjeto.Line.ConfigLinea.Point1P.Y;
                    }
                }

                Grid.Direccion = eDireccionGrid.X;
                Grid.BubbleSize = BubbleSize;
            }
            );

            float ExtensionInferior = 0.2f;
            float Ymin = Lista_Elementos.Min(x => x.Vistas.Perfil_Original.Reales.Min(y => y.Y)) - ExtensionInferior;
            float Ymax = Lista_Elementos.Max(x => x.Vistas.Perfil_Original.Reales.Max(y => y.Y));
            Grids.ForEach(x => x.CrearRecta(0, Ymax, 0, Ymin));
        }

        private void AsignarCambioAlturayCambioAnchoObjetos()
        {
            List<float> Hs = new List<float>();
            List<float> Bs = new List<float>();
            for (int i = 0; i < Lista_Objetos.Count; i++)
            {
                cObjeto ObjetoActual = Lista_Objetos[i];
                if (ObjetoActual.Soporte == eSoporte.Vano)
                {
                    Hs.Add(ObjetoActual.Line.Seccion.H);
                    Bs.Add(ObjetoActual.Line.Seccion.B);
                }
            }
            if (Bs.Distinct().ToList().Count > 1)
            {
                Bool_CambioAncho = true;
            }
            if (Hs.Distinct().ToList().Count > 1)
            {
                Bool_CambioAltura = true;
            }

            cambioenAltura = Bool_CambioAltura ? eCambioenAltura.Inferior : eCambioenAltura.Ninguno;
            cambioenAncho = Bool_CambioAncho ? eCambioenAncho.Inferior : eCambioenAncho.Ninguno; // Cambios Inferiores Predeterminados
        }

        public void AsignarCambioAlturayCambioAnchoElementos()
        {
            List<float> Hs = new List<float>();
            List<float> Bs = new List<float>();
            for (int i = 0; i < Lista_Elementos.Count; i++)
            {
                IElemento ObjetoActual = Lista_Elementos[i];
                if (ObjetoActual is cSubTramo)
                {
                    Hs.Add((float)Math.Round(ObjetoActual.Seccion.H,3));
                    Bs.Add((float)Math.Round(ObjetoActual.Seccion.B,3));
                }
            }
            if (Bs.Distinct().ToList().Count > 1)
            {
                Bool_CambioAncho = true;
            }
            else
            {
                Bool_CambioAncho = false;
            }
            if (Hs.Distinct().ToList().Count > 1)
            {
                Bool_CambioAltura = true;
            }
            else
            {
                Bool_CambioAltura = false;
            }
            cambioenAltura = Bool_CambioAltura ? eCambioenAltura.Inferior : eCambioenAltura.Ninguno;
            cambioenAncho = Bool_CambioAncho ? eCambioenAncho.Inferior : eCambioenAncho.Ninguno; // Cambios Inferiores Predeterminados
        }

        public override string ToString()
        {
            return $"{Nombre} | CountTramos = {Lista_Tramos.Count} | {Direccion}";
        }

        private void CrearTramos()
        {
            Lista_Tramos = new List<cTramo>();
            List<cObjeto> Objetos = new List<cObjeto>();
            int Contador = 0; int Contador2 = 0;
            foreach (cObjeto Objeto in Lista_Objetos)
            {
                if (Objeto.Soporte != eSoporte.Apoyo)
                {
                    Objetos.Add(Objeto);
                }
                else
                {
                    if (Objetos.Count != 0)
                    {
                        Contador += 1;
                        cTramo Tramo = new cTramo(Contador - 1, Objetos, this);
                        Lista_Tramos.Add(Tramo);
                    }
                    Objetos = new List<cObjeto>();
                }
                Contador2 += 1;

                if (Contador2 == Lista_Objetos.Count)
                {
                    if (Objeto.Soporte == eSoporte.Vano)
                    {
                        if (Objetos.Count != 0)
                        {
                            Contador += 1;
                            cTramo Tramo = new cTramo(Contador - 1, Objetos, this);
                            Lista_Tramos.Add(Tramo);
                        }
                    }
                }
            }
        }

        private void CrearElementos()
        {
            Lista_Elementos = new List<IElemento>();
            int CountApoyo = 0;
            foreach (cObjeto Objeto in Lista_Objetos)
            {

                if (Objeto.Soporte == eSoporte.Apoyo)
                {
                    CountApoyo += 1;
                    cApoyo Apoyo = new cApoyo($"Apoyo {CountApoyo}", cFunctionsProgram.DeepClone(Objeto.Line.Seccion), this);
                    Lista_Elementos.Add(Apoyo);
                }
                else
                {
                    foreach (cTramo Tramo in Lista_Tramos)
                    {
                        cSubTramo SubTramo = Tramo.Lista_SubTramos.Find(x => x.Lista_Lineas.Contains(Objeto.Line));
                        if (SubTramo != null)
                        {
                            if (!Lista_Elementos.Contains(SubTramo))
                            {
                                Lista_Elementos.Add(SubTramo);
                            }
                        }
                    }
                }
            }
            int Contador = 0;
            Lista_Elementos.ForEach(x => { x.Indice = Contador; Contador++; });
        }

        public void AsignarMaximaLongitudTendencias()
        {

            //if (Longitud < Tendencia_Refuerzos.TInfeSelect.MaximaLongitud)
            //{
            //    if (Longitud + 2 * cDiccionarios.G90[eNoBarra.B6] < Tendencia_Refuerzos.TInfeSelect.MaximaLongitud)
            //    {

            //        Tendencia_Refuerzos.TInfeSelect.MaximaLongitud = Longitud + 2 * cDiccionarios.G90[eNoBarra.B6];

            //    }

            //}
            //if (Longitud < Tendencia_Refuerzos.TSupeSelect.MaximaLongitud)
            //{
            //    if (Longitud + 2 * cDiccionarios.G90[eNoBarra.B6] < Tendencia_Refuerzos.TSupeSelect.MaximaLongitud)
            //    {

            //        Tendencia_Refuerzos.TSupeSelect.MaximaLongitud = Longitud + 2 * cDiccionarios.G90[eNoBarra.B6];
            //    }

            //}
        }


        #region Coordenadas Perfil Longitudinal AutoCAD
        public void CrearCoordenadasPerfilLongitudinalAutoCAD()
        {

            List<float> DiferentesAlturas = Lista_Elementos.FindAll(x => x is cSubTramo).Select(x => x.Seccion.H).Distinct().ToList();
            DiferentesAlturas = DiferentesAlturas.OrderBy(x => x).ToList();
            List<Tuple<float, float>> Altura_Real_AutoCAD = new List<Tuple<float, float>>();
            int Contador = 0;
            foreach (float H in DiferentesAlturas)
            {
                Tuple<float, float> Tuple = new Tuple<float, float>(H, cVariables.AltoMinimoNervio + (Contador * cVariables.DeltaNivel));
                Altura_Real_AutoCAD.Add(Tuple);
                Contador++;
            }

            for (int i = 0; i < Lista_Elementos.Count; i++)
            {
                IElemento ElementoAnterior = null; IElemento Elemento_Posterior = null;
                if (i - 1 >= 0)
                {
                    ElementoAnterior = Lista_Elementos[i - 1];
                }
                if (i + 1 < Lista_Elementos.Count)
                {
                    Elemento_Posterior = Lista_Elementos[i + 1];
                }
                AsignarAlturaVitual_PerfilLongitudinalAutoCAD(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior, Altura_Real_AutoCAD);
            }

            for (int i = 0; i < Lista_Elementos.Count; i++)
            {
                IElemento ElementoAnterior = null; IElemento Elemento_Posterior = null;
                if (i - 1 >= 0)
                {
                    ElementoAnterior = Lista_Elementos[i - 1];
                }
                if (i + 1 < Lista_Elementos.Count)
                {
                    Elemento_Posterior = Lista_Elementos[i + 1];
                }
                CrearCoordenadasLongitudinal_AutoCAD(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior, 1);
            }
        }
        private void AsignarAlturaVitual_PerfilLongitudinalAutoCAD(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior, List<Tuple<float, float>> Altura_Real_AutoCAD)
        {
            if (ElementoAnterior == null && ElementoPosterior != null) //Primer Elemento
            {
                if (ElementoActual is cApoyo)
                {
                    ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoPosterior.Seccion.H).Item2;
                }
                else
                {
                    ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoActual.Seccion.H).Item2;
                }
            }
            else if (ElementoAnterior != null && ElementoPosterior != null) //Elemento del Medio
            {
                if (ElementoActual is cApoyo)
                {
                    float DeltaH = ElementoPosterior.Seccion.H - ElementoAnterior.Seccion.H;

                    if (DeltaH < 0)
                    {
                        ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoAnterior.Seccion.H).Item2;
                    }
                    else
                    {
                        ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoPosterior.Seccion.H).Item2;
                    }
                }
                else
                {
                    ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoActual.Seccion.H).Item2;
                }
            }
            else if (ElementoAnterior != null && ElementoPosterior == null)  //Ultimo Elemento
            {
                if (ElementoActual is cApoyo)
                {
                    ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoAnterior.Seccion.H).Item2;
                }
                else
                {
                    ElementoActual.HVirtual_AutoCAD = Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoActual.Seccion.H).Item2;
                }
            }
        }

        private void CrearCoordenadasLongitudinal_AutoCAD(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior, float FE)
        {
            PointF PuntoInicial;
            if (ElementoAnterior == null && ElementoPosterior != null) //Primer Elemento
            {
                PuntoInicial = new PointF(0, 0);
            }
            else //(ElementoAnterior != null && ElementoPosterior != null) //Elemento del Medio y Ultimo
            {
                float DeltaH = ElementoActual.HVirtual_AutoCAD - ElementoAnterior.HVirtual_AutoCAD;
                PuntoInicial = ElementoAnterior.Vistas.Perfil_AutoCAD.Reales[ElementoAnterior.Vistas.Perfil_AutoCAD.Reales.Count - 1];

                if (CambioenAltura == eCambioenAltura.Inferior)
                {
                    if (DeltaH > 0)
                    {
                        DeltaH = -Math.Abs(DeltaH);
                    }
                    else
                    {
                        DeltaH = Math.Abs(DeltaH);
                    }
                }
                else if (CambioenAltura == eCambioenAltura.Superior | CambioenAltura == eCambioenAltura.Ninguno)
                {
                    DeltaH = 0;
                }
                else if (CambioenAltura == eCambioenAltura.Central)
                {
                    if (DeltaH > 0)
                    {
                        DeltaH = -Math.Abs(DeltaH) / 2;
                    }
                    else
                    {
                        DeltaH = Math.Abs(DeltaH) / 2;
                    }
                }

                PuntoInicial.Y += DeltaH * FE;
            }

            CrearCoordenadasLongitudinal_Elemento_AutoCAD(ElementoActual, PuntoInicial);
        }

        private void CrearCoordenadasLongitudinal_Elemento_AutoCAD(IElemento Elemento, PointF PuntoInicial)
        {
            Elemento.Vistas.Perfil_AutoCAD.Reales = new List<PointF>();

            float H = Elemento.HVirtual_AutoCAD;
            float B = Elemento.Seccion.B * cConversiones.Dimension_cm_to_m;
            if (Elemento.Soporte == eSoporte.Vano)
            {
                cSubTramo SubTramo = (cSubTramo)Elemento;

                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(PuntoInicial);
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(new PointF(PuntoInicial.X, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(new PointF(PuntoInicial.X + SubTramo.Longitud, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(new PointF(PuntoInicial.X + SubTramo.Longitud, PuntoInicial.Y));
            }
            else if (Elemento.Soporte == eSoporte.Apoyo)
            {
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(PuntoInicial);
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(new PointF(PuntoInicial.X, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_AutoCAD.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y));
            }
        }


        #endregion





        #region Coordenadas Perfil Longitudinal Reales

        public void CrearCoordenadasPerfilLongitudinalReales()
        {
            for (int i = 0; i < Lista_Elementos.Count; i++)
            {
                IElemento ElementoAnterior = null; IElemento Elemento_Posterior = null;
                if (i - 1 >= 0)
                {
                    ElementoAnterior = Lista_Elementos[i - 1];
                }
                if (i + 1 < Lista_Elementos.Count)
                {
                    Elemento_Posterior = Lista_Elementos[i + 1];
                }
                AsignarAlturaVitual_PerfilLongitudinalReal(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior);
            }

            for (int i = 0; i < Lista_Elementos.Count; i++)
            {
                IElemento ElementoAnterior = null; IElemento Elemento_Posterior = null;
                if (i - 1 >= 0)
                {
                    ElementoAnterior = Lista_Elementos[i - 1];
                }
                if (i + 1 < Lista_Elementos.Count)
                {
                    Elemento_Posterior = Lista_Elementos[i + 1];
                }
                CrearCoordenadasLongitudinal_Reales(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior, 1);
            }

            Longitud = Lista_Elementos.Sum(x => x.Longitud);
            IElemento E1 = Lista_Elementos.FindAll(x => x is cSubTramo).Find(x => x.Seccion.B >= cVariables.BNervioBorde);
            if (E1 != null)
                ActivarBoolNervioBorde = true;
            else
                ActivarBoolNervioBorde = false;
        }
        private void AsignarAlturaVitual_PerfilLongitudinalReal(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior)
        {
            if (ElementoAnterior == null && ElementoPosterior != null) //Primer Elemento
            {
                if (ElementoActual is cApoyo)
                {
                    ElementoActual.HVirtual_Real = ElementoPosterior.Seccion.H;
                }
                else
                {
                    ElementoActual.HVirtual_Real = ElementoActual.Seccion.H;
                }
            }
            else if (ElementoAnterior != null && ElementoPosterior != null) //Elemento del Medio
            {
                if (ElementoActual is cApoyo)
                {
                    float DeltaH = ElementoPosterior.Seccion.H - ElementoAnterior.Seccion.H;

                    if (DeltaH < 0)
                    {
                        ElementoActual.HVirtual_Real = ElementoAnterior.Seccion.H;
                    }
                    else
                    {
                        ElementoActual.HVirtual_Real = ElementoPosterior.Seccion.H;
                    }
                }
                else
                {
                    ElementoActual.HVirtual_Real = ElementoActual.Seccion.H;
                }
            }
            else if (ElementoAnterior != null && ElementoPosterior == null)  //Ultimo Elemento
            {
                if (ElementoActual is cApoyo)
                {
                    ElementoActual.HVirtual_Real = ElementoAnterior.Seccion.H;
                }
                else
                {
                    ElementoActual.HVirtual_Real = ElementoActual.Seccion.H;
                }
            }
        }

        private void CrearCoordenadasLongitudinal_Reales(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior, float FE)
        {
            PointF PuntoInicial;
            if (ElementoAnterior == null && ElementoPosterior != null) //Primer Elemento
            {
                PuntoInicial = new PointF(0, 0);
            }
            else //(ElementoAnterior != null && ElementoPosterior != null) //Elemento del Medio y Ultimo
            {
                float DeltaH = ElementoActual.HVirtual_Real - ElementoAnterior.HVirtual_Real;
                PuntoInicial = ElementoAnterior.Vistas.Perfil_Original.Reales[ElementoAnterior.Vistas.Perfil_Original.Reales.Count - 1];

                if (CambioenAltura == eCambioenAltura.Inferior)
                {
                    if (DeltaH > 0)
                    {
                        DeltaH = -Math.Abs(DeltaH);
                    }
                    else
                    {
                        DeltaH = Math.Abs(DeltaH);
                    }
                }
                else if (CambioenAltura == eCambioenAltura.Superior | CambioenAltura == eCambioenAltura.Ninguno)
                {
                    DeltaH = 0;
                }
                else if (CambioenAltura == eCambioenAltura.Central)
                {
                    if (DeltaH > 0)
                    {
                        DeltaH = -Math.Abs(DeltaH) / 2;
                    }
                    else
                    {
                        DeltaH = Math.Abs(DeltaH) / 2;
                    }
                }

                PuntoInicial.Y += DeltaH * FE * cConversiones.Dimension_cm_to_m;
            }

            CrearCoordenadasLongitudinal_Elemento_Reales(ElementoActual, PuntoInicial);
        }

        private void CrearCoordenadasLongitudinal_Elemento_Reales(IElemento Elemento, PointF PuntoInicial)
        {
            Elemento.Vistas.Perfil_Original.Reales = new List<PointF>();

            float H = Elemento.HVirtual_Real * cConversiones.Dimension_cm_to_m;
            float B = Elemento.Seccion.B * cConversiones.Dimension_cm_to_m;
            if (Elemento.Soporte == eSoporte.Vano)
            {
                cSubTramo SubTramo = (cSubTramo)Elemento;

                Elemento.Vistas.Perfil_Original.Reales.Add(PuntoInicial);
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + SubTramo.Longitud, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + SubTramo.Longitud, PuntoInicial.Y));
            }
            else if (Elemento.Soporte == eSoporte.Apoyo)
            {
                Elemento.Vistas.Perfil_Original.Reales.Add(PuntoInicial);
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y + H));
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y));
            }
        }

        #endregion

        #region Calculos

        public void CrearEnvolvente()
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.Estaciones.ForEach(Estacion =>
                    {
                        Estacion.Calculos.Envolvente = new cEnvolvente(Estacion.Lista_Solicitaciones, Estacion.Calculos,SimilitudNervioCompleto.BoolSoySimiarA | SimilitudNervioCompleto.IsMaestro);
                    });
                }
            });
            CrearCoordenadasDiagramaMomentosyCortantesyAreas_Reales_Envolvente();
        }

        public void CrearCoordenadasDiagramaMomentosyCortantesyAreas_Reales_Envolvente()
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales = new List<PointF>();

                    SubtramoAux.Coordenadas_FI_Vc_Positivos.Reales = new List<PointF>();
                    SubtramoAux.Coordenadas_FI_Vc_Negativos.Reales = new List<PointF>();
                    SubtramoAux.Coordenadas_FI_Vc2_Positivos.Reales = new List<PointF>();
                    SubtramoAux.Coordenadas_FI_Vc2_Negativos.Reales = new List<PointF>();


                    float CoordenaXMenor = SubtramoAux.Vistas.Perfil_Original.Reales.Min(X => X.X);
                    SubtramoAux.Estaciones.ForEach(Estacion =>
                    {
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Envolvente.M3[0]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Envolvente.M3[1]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.V2[0]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.V2[1]));
                        Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Cortante = Estacion.Calculos.Envolvente.V2[0];
                        Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Cortante =  Math.Abs(Estacion.Calculos.Envolvente.V2[1]) ;
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento));
                        SubtramoAux.Coordenadas_FI_Vc_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.FI_Vc));
                        SubtramoAux.Coordenadas_FI_Vc_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Envolvente.FI_Vc));
                        SubtramoAux.Coordenadas_FI_Vc2_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.FI_Vc2));
                        SubtramoAux.Coordenadas_FI_Vc2_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Envolvente.FI_Vc2));
                    });
                }
            });
            AgregarArea_S_Estaciones_Cortante();
        }
        private void AgregarArea_S_Estaciones_Cortante()
        {

            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    List<float> CoordXEntreFiVc_FiVc2_I = new List<float>();
                    List<float> CoordXEntreFiVc_FiVc2_S = new List<float>();
                    List<float> CoordXMayoresFiVc_I = new List<float>();
                    List<float> CoordXMayoresFiVc_S = new List<float>();
                    cSubTramo SubTramoAux = (cSubTramo)Elemento;
                    float CoordenaXMenor = SubTramoAux.Vistas.Perfil_Original.Reales.Min(X => X.X);
                    SubTramoAux.Estaciones.ForEach(Estacion =>
                    {
                        float CortanteI = Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Cortante;
                        float CortanteS = Estacion.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Cortante;
                        float FiVc = Estacion.Calculos.Envolvente.FI_Vc;
                        float FiVc2 = Estacion.Calculos.Envolvente.FI_Vc2;

                        if (Math.Abs(CortanteI) >= FiVc)
                            CoordXMayoresFiVc_I.Add(Estacion.CoordX);
                        if (Math.Abs(CortanteI) < FiVc && Math.Abs(CortanteI) >= FiVc2)
                            CoordXEntreFiVc_FiVc2_I.Add(Estacion.CoordX);
                        if (Math.Abs(CortanteS) >= FiVc)
                            CoordXMayoresFiVc_S.Add(Estacion.CoordX);
                        if (Math.Abs(CortanteS) < FiVc && Math.Abs(CortanteS) >= FiVc2)
                            CoordXEntreFiVc_FiVc2_S.Add(Estacion.CoordX);
                    });

                    float CoordXMaxFiVc = 99999f; float CoordXMaxFiVc2 = 99999f;
                    float CoordYMaxFiVc = SubTramoAux.Estaciones.Max(y => y.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S);
                    if (CoordXMayoresFiVc_I.Count > 0)
                        CoordXMaxFiVc = CoordXMayoresFiVc_I.Min();
                    if (CoordXEntreFiVc_FiVc2_I.Count > 0)
                        CoordXMaxFiVc2 = CoordXEntreFiVc_FiVc2_I.Min();

                    float CoordXMaxFiVc_S = -99999f; float CoordXMaxFiVc2_S = -99999f;
                    float CoordYMaxFiVc2_S = SubTramoAux.Estaciones.Max(y => y.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S);
                    if (CoordXMayoresFiVc_S.Count > 0)
                        CoordXMaxFiVc_S = CoordXMayoresFiVc_S.Max();
                    if (CoordXEntreFiVc_FiVc2_S.Count > 0)
                        CoordXMaxFiVc2_S = CoordXEntreFiVc_FiVc2_S.Max();

                    SubTramoAux.Estaciones.ForEach(x =>
                    {


                        if (x.CoordX > CoordXMaxFiVc)
                        {
                            x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S = CoordYMaxFiVc;
                        }
                        else if (x.CoordX <= CoordXMaxFiVc && x.CoordX >= CoordXMaxFiVc2)
                        {
                            x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S = x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.AreaMin;
                        }


                        if (x.CoordX < CoordXMaxFiVc_S)
                        {
                            x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S = CoordYMaxFiVc2_S;
                        }
                        else if (x.CoordX >= CoordXMaxFiVc_S && x.CoordX <= CoordXMaxFiVc2_S)
                        {
                            x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S = x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.AreaMin;
                        }

                        float[] Areas = new float[] { x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S, x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S };

                        x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S = Areas.Max();
                        x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S = Areas.Max();


                        SubTramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Reales.Add(new PointF(CoordenaXMenor + x.CoordX, x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesInferior.Area_S));
                        SubTramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Reales.Add(new PointF(CoordenaXMenor + x.CoordX, x.Calculos.Solicitacion_Asignado_Cortante.SolicitacionesSuperior.Area_S));
                    });

                    
                    

                }
            });



        }

        public void CrearAceroAsignadoRefuerzoLongitudinal()
        {

            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.Estaciones.ForEach(Estacion =>
                    {
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento = 0f;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento = 0f;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Momento = 0f;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Momento = 0f;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior = -100f;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior = -100f;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior = Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento - Estacion.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento;
                        Estacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior = Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento - Estacion.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento;
                        List<cBarra> BarrasSuperiores = Tendencia_Refuerzos.TSupeSelect.Barras.FindAll(x => x.EstacionEnBarra(Estacion, SubtramoAux));
                        List<cBarra> BarrasInferiores = Tendencia_Refuerzos.TInfeSelect.Barras.FindAll(x => x.EstacionEnBarra(Estacion, SubtramoAux));
                        if (BarrasSuperiores != null && BarrasSuperiores.Count > 0)
                        {
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento = BarrasSuperiores.Sum(x => x.AporteAceroAEstacion(Estacion, SubtramoAux));
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior = Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento - Estacion.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesSuperior.Area_Momento;
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.CalcularMomento(cFunctionsProgram.CalcularDCentroide(BarrasSuperiores, eUbicacionRefuerzo.Superior, this));
                            //Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Momento = BarrasSuperiores.Sum(x => x.AporteMomentoAEstacion(Estacion, SubtramoAux));
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Superior = Estacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Superior / Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento * 100f;
                        }
                        if (BarrasInferiores != null && BarrasInferiores.Count > 0)
                        {
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento = BarrasInferiores.Sum(x => x.AporteAceroAEstacion(Estacion, SubtramoAux));
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.CalcularMomento(cFunctionsProgram.CalcularDCentroide(BarrasInferiores, eUbicacionRefuerzo.Inferior, this));
                            //Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Momento = BarrasInferiores.Sum(x => x.AporteMomentoAEstacion(Estacion, SubtramoAux));
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior = Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento - Estacion.Calculos.Solicitacion_Asignado_Momentos.SolicitacionesInferior.Area_Momento;
                            Estacion.Calculos.Solicitacion_Asignado_Momentos.PorcentajeAceroFlexion_Inferior = Estacion.Calculos.Solicitacion_Asignado_Momentos.AceroFaltanteFlexion_Inferior / Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento * 100f;

                        }

                    });
                }
            });
            CrearCoordenadasDiagramaMomentosyAreas_Reales_Asignado();

        }
        public void CrearCoordenadasDiagramaMomentosyAreas_Reales_Asignado()
        {
            Lista_Elementos.ForEach(Elemento =>
              {
                  if (Elemento is cSubTramo)
                  {
                      cSubTramo SubtramoAux = (cSubTramo)Elemento;
                      SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Reales = new List<PointF>();
                      SubtramoAux.CoordenadasCalculosAsignado.Momentos_Negativos.Reales = new List<PointF>();
                      SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Reales = new List<PointF>();
                      SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Reales = new List<PointF>();
                      float CoordenaXMenor = SubtramoAux.Vistas.Perfil_Original.Reales.Min(X => X.X);
                      SubtramoAux.Estaciones.ForEach(Estacion =>
                      {
                          SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Momento));
                          SubtramoAux.CoordenadasCalculosAsignado.Momentos_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Momento));
                          SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoSuperior.Area_Momento));
                          SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Solicitacion_Asignado_Momentos.AsignadoInferior.Area_Momento));
                      });
                  }
              });


        }
        public void CrearCoordenadasDiagramaAreasCortante_Reales_Asigando()
        {

            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Reales = new List<PointF>();
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Reales = new List<PointF>();
                    float CoordenaXMenor = SubtramoAux.Vistas.Perfil_Original.Reales.Min(X => X.X);
                    SubtramoAux.Estaciones.ForEach(Estacion =>
                    {
                        SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Solicitacion_Asignado_Cortante.AsignadoSuperior.Area_S));
                        //SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Solicitacion_Asignado_Cortante.AsignadoInferior.Area_S));
                    });
                }
            });

        }

        #region Refuerzo Transveral
        public void CrearAceroAsignadoRefuerzoTransversal()
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    float CoordenaXMenor = SubtramoAux.Vistas.Perfil_Original.Reales.Min(X => X.X);
                    SubtramoAux.Estaciones.ForEach(Estacion =>
                    {
                        Estacion.Calculos.Solicitacion_Asignado_Cortante.AsignadoInferior.Area_S = 0f;
                        Estacion.Calculos.Solicitacion_Asignado_Cortante.AsignadoSuperior.Area_S = 0f;
                        Estacion.Calculos.Solicitacion_Asignado_Cortante.AsignadoSuperior.Area_S = Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Sum(y => y.AporteRelacion_As_S_aEstacion(Estacion,Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos));
                    });
                }
            });
            CrearCoordenadasDiagramaAreasCortante_Reales_Asigando();
        }
        private float CalcularPesoTransversal()
        {
            PesoTotalRefuerzoTransversal = 0;
            Lista_Tramos.ForEach(x => { x.CalcularPesoTransversal(); PesoTotalRefuerzoTransversal += x.PesoRefuerzoTransversal; });
            return PesoTotalRefuerzoTransversal;
        }

        public void ActualizarRefuerzoTransversal()
        {
            CrearAceroAsignadoRefuerzoTransversal();
        }
        #endregion



        public void CrearCoordenadasDiagramaMomentos_Escaladas_Envolvente(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
           {
               if (Elemento is cSubTramo)
               {
                   cSubTramo SubtramoAux = (cSubTramo)Elemento;
                   SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                   SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                   SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
               }
           });
        }

        public void CrearCoordenadasDiagramaAreasMomentos_Escaladas_Envolvente(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                }
            });
        }

        public void CrearCoordenadasDiagramaCortante_Escaladas_Envolvente(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.Coordenadas_FI_Vc_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.Coordenadas_FI_Vc_Positivos.Reales, out EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.Coordenadas_FI_Vc_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.Coordenadas_FI_Vc_Negativos.Reales, out EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.Coordenadas_FI_Vc2_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.Coordenadas_FI_Vc2_Positivos.Reales, out EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.Coordenadas_FI_Vc2_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.Coordenadas_FI_Vc2_Negativos.Reales, out EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                }
            });
        }

        public void CrearCoordenadasDiagramaAreasCortante_Escaladas_Envolvente(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    //SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                }
            });
        }

        public void CrearCoordenadasDiagramaMomentos_Escaladas_Asignado(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosAsignado.Momentos_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosAsignado.Momentos_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                }
            });
        }
        public void CrearCoordenadasDiagramaAreasMomentos_Escaladas_Asignado(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                }
            });
        }


        public void CrearCoordenadasDiagramaAreasCortante_Escalada_Asignado(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                    SubtramoAux.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
                }
            });
        }


        #endregion Calculos


        public void CrearCoordenadasLongitudinal_Elementos_Escalados_Original(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento => Elemento.Vistas.Perfil_Original.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosEnEjeY(PuntosTodosObjetos, Elemento.Vistas.Perfil_Original.Reales, WidthWindow, HeigthWindow, out EscalaMayorenX, Zoom, Dx, Dy, XI));
            Grids.ForEach(x => x.CrearPuntosPlantaEscaladaEtabs(PuntosTodosObjetos, WidthWindow, HeigthWindow, Dx, Dy, Zoom, true, XI));
        }

        private void CrearCoordenadasLongitudinal_Elementos_Escalados_AutoCAD(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento => Elemento.Vistas.Perfil_AutoCAD.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, Elemento.Vistas.Perfil_AutoCAD.Reales, out EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f));
            Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI));
            Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI));
            Lista_Tramos.ForEach(Tramo =>
            {
                if (Tramo.EstribosDerecha != null)
                {
                    Tramo.EstribosDerecha.Zona1.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI);
                    Tramo.EstribosDerecha.Zona2.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI);
                }
                if (Tramo.EstribosIzquierda != null)
                {
                    Tramo.EstribosIzquierda.Zona1.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI);
                    Tramo.EstribosIzquierda.Zona2.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI);

                }

            });
        }


        #region Metodos Mouse

        public void MouseMoveNervioPlantaEtabs(Point Point)
        {
            foreach (cTramo Tramo in Lista_Tramos)
            {
                foreach (cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    ElementoEnumerado_MouseMove = Objeto.Line.MouseInLineEtabs(Point);
                    if (ElementoEnumerado_MouseMove) { return; } else { ElementoEnumerado_MouseMove = false; }
                }
                //if (ElementoEnumerado_MouseMove) { break; } else { ElementoEnumerado_MouseMove = false; }
            }
        }

        public void MouseDownSelectPlantaEnumeracion(Point Point)
        {
            bool EncotroResultado = false;
            foreach (cTramo Tramo in Lista_Tramos)
            {
                foreach (cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    if (Objeto.Line.MouseInLineEtabs(Point))
                    {
                        if (SelectPlantaEnumeracion)
                        {
                            SelectPlantaEnumeracion = false;
                        }
                        else
                        {
                            SelectPlantaEnumeracion = true;
                        }

                        EncotroResultado = true;
                    }

                    if (EncotroResultado) { break; }
                }
                if (EncotroResultado) { break; }
            }
            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(z => z.Line.Select = SelectPlantaEnumeracion));
        }

        public bool MouseDownSelect(Point Point)
        {
            bool Select = false;
            foreach (cTramo Tramo in Lista_Tramos)
            {
                foreach (cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    if (Objeto.Line.MouseInLineEtabs(Point))
                    {
                        Select = true;
                    }
                    else
                    {
                        Select = false;
                    }

                    if (Select) { break; }
                }
                if (Select) { break; }
            }

            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(z => z.Line.Select = Select));
            return Select;
        }


        #endregion Metodos Mouse

        #region Metodos Paint
        public void ChangeSelect()
        {
            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(z => z.Line.Select = Select));
        }
        public void PaintNombreElementosEnumerados_MouseMove(Graphics e, float HeigthWindow, float WidthWindow)
        {
            if (ElementoEnumerado_MouseMove)
            {
                Font Font1 = new Font("Calibri", 9, FontStyle.Bold);
                PointF PointString = new PointF(WidthWindow / 2 - e.MeasureString(Nombre, Font1).Width / 2, HeigthWindow / 2 - Font1.Height / 2);
                e.DrawString(Nombre, Font1, Brushes.Black, PointString);
            }
        }

        public void Paint_Planta_ElementosEnumerados(Graphics e, float WidthW, float Zoom = 1)
        {
            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(z => { z.Line.PaintPlantaEscaladaEtabsLine(e); }));
            CrearCuadrodeInfo(e, Zoom, PuntoInMousePointLines_Escalado_Real, "Y", "", WidthW);
        }

        public void Paint_Longitudinal_Elementos_Escalados_Original(Graphics e, float Zoom, float HeightForm)
        {
            Grids.ForEach(x => x.Paint(e, Zoom));
            Pen Pen_SinSeleccionar = new Pen(Color.Black, 2);
            Pen Pen__Seleccionado = new Pen(Color.FromArgb(0, 3, 100), 3);
            Pen Pen_Definitivo;
            SolidBrush Brush_Selecciondo_Apoyo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(59, 57, 57)));
            SolidBrush Brush_SinSeleccionar_Apoyo = new SolidBrush(Color.FromArgb(85, 85, 85));
            SolidBrush Brush_SinSeleccionado_Subtramos = new SolidBrush(Color.FromArgb(187, 211, 238));
            SolidBrush Brush_Seleccionado_Subtramos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(73, 115, 163)));
            SolidBrush Brush_Definitivo;

            float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);

            Lista_Elementos.ForEach(Elemento =>
            {
                float Largo = Elemento.Vistas.Perfil_Original.Escaladas.Max(y => y.X) - Elemento.Vistas.Perfil_Original.Escaladas.Min(y => y.X);
                float XI = Elemento.Vistas.Perfil_Original.Escaladas.Min(y => y.X);
                float YI = Elemento.Vistas.Perfil_Original.Escaladas.Max(y => y.Y);
                Pen_Definitivo = Elemento.Vistas.SelectPerfilLongitudinal ? Pen__Seleccionado : Pen_SinSeleccionar;
                if (Elemento is cApoyo)
                {
                    Brush_Definitivo = Elemento.Vistas.SelectPerfilLongitudinal ? Brush_Selecciondo_Apoyo : Brush_SinSeleccionar_Apoyo;
                    string Texto = $"({Elemento.Seccion.B}x{Elemento.Seccion.H})";
                    SizeF MeasureString = e.MeasureString(Texto, Font1);
                    PointF PuntoString = new PointF(XI + Largo / 2 - MeasureString.Width / 2, YI + MeasureString.Height / 2);
                    e.DrawPolygon(Pen_Definitivo, Elemento.Vistas.Perfil_Original.Escaladas.ToArray());
                    e.FillPolygon(Brush_Definitivo, Elemento.Vistas.Perfil_Original.Escaladas.ToArray());
                    e.DrawString(Texto, Font1, Brushes.Black, PuntoString);
                }
                else
                {
                    Brush_Definitivo = Elemento.Vistas.SelectPerfilLongitudinal ? Brush_Seleccionado_Subtramos : Brush_SinSeleccionado_Subtramos;
                    cSubTramo ElementoTramo = (cSubTramo)Elemento;
                    e.DrawPolygon(Pen_Definitivo, Elemento.Vistas.Perfil_Original.Escaladas.ToArray());
                    e.FillPolygon(Brush_Definitivo, Elemento.Vistas.Perfil_Original.Escaladas.ToArray());
                    string Texto = $"({Elemento.Seccion.B}x{Elemento.Seccion.H})\n L={string.Format("{0:0.00}", ElementoTramo.Longitud)}";
                    SizeF MeasureString = e.MeasureString(Texto, Font1);
                    PointF PuntoString = new PointF(XI + Largo / 2 - MeasureString.Width / 2, YI + MeasureString.Height / 4);
                    e.DrawString(Texto, Font1, Brushes.Black, PuntoString);
                }
            });
            GraficarRectaApoyos(e, HeightForm);
        }

        public void Paint_Longitudinal_DrawMomentos(Graphics e, float Zoom, float HeightForm, float WidthW)
        {
            SolidBrush Brush_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(38, 86, 158)));
            SolidBrush Brush_Negativo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(227, 88, 88)));

            SolidBrush Brush_Asignado_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(90, 90, 140)));
            SolidBrush Brush_Asignado_Negativos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(90, 90, 140)));

            Pen PenBlack = new Pen(Brushes.Black, 1.5f); PenBlack.LineJoin = LineJoin.Round;

            #region Momentos Solicictaciones 
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    List<PointF> Momento_Negativos_Escalados = new List<PointF>(); List<PointF> Momento_Positivos_Escalados = new List<PointF>();
                    List<PointF> Momento_Negativos_SinEscalados = new List<PointF>(); List<PointF> Momento_Positivos_SinEscalados = new List<PointF>();

                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    Momento_Negativos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales);
                    Momento_Positivos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales);
                    Momento_Negativos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Escaladas);
                    Momento_Positivos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Escaladas);

                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Momento_Negativos_Escalados, Momento_Negativos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Y0_Escalado.Y);
                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Momento_Positivos_Escalados, Momento_Positivos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Y0_Escalado.Y);
                    e.FillPolygon(Brush_Positivos, Momento_Positivos_Escalados.ToArray());
                    e.FillPolygon(Brush_Negativo, Momento_Negativos_Escalados.ToArray());

                    e.DrawLines(PenBlack, Momento_Positivos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Momento_Negativos_Escalados.ToArray());
                }
            });

            #endregion


            #region Momentos Asignados
            List<PointF> Momento_Negativos_Escalados2 = new List<PointF>(); List<PointF> Momento_Positivos_Escalados2 = new List<PointF>();
            List<PointF> Momento_Negativos_SinEscalados2 = new List<PointF>(); List<PointF> Momento_Positivos_SinEscalados2 = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    Momento_Negativos_SinEscalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Momentos_Negativos.Reales);
                    Momento_Positivos_SinEscalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Reales);
                    Momento_Negativos_Escalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Momentos_Negativos.Escaladas);
                    Momento_Positivos_Escalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Momentos_Positivos.Escaladas);
                }
            });

            cSubTramo SubtramoAux2 = (cSubTramo)Lista_Elementos.Find(x => x is cSubTramo);


            cFunctionsProgram.CerrarPoligonoParaMomentos(ref Momento_Negativos_Escalados2, Momento_Negativos_SinEscalados2, SubtramoAux2.CoordenadasCalculosAsignado.Momentos_Positivos.Y0_Escalado.Y);
            cFunctionsProgram.CerrarPoligonoParaMomentos(ref Momento_Positivos_Escalados2, Momento_Positivos_SinEscalados2, SubtramoAux2.CoordenadasCalculosAsignado.Momentos_Positivos.Y0_Escalado.Y);
            e.FillPolygon(Brush_Asignado_Positivos, Momento_Positivos_Escalados2.ToArray());
            e.FillPolygon(Brush_Asignado_Negativos, Momento_Negativos_Escalados2.ToArray());
            e.DrawLines(PenBlack, Momento_Positivos_Escalados2.ToArray());
            e.DrawLines(PenBlack, Momento_Negativos_Escalados2.ToArray());

            #endregion




            float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
            GraficarRectaApoyos(e, HeightForm);

            #region Muestra Valores de Momentos Solictados con el Mouse
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseMomentos_Escalado_Real, "M", "Ton-m", WidthW);
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseAreasMomentosAsignado_Escalado_Real, "M", "Ton-m", WidthW);
            #endregion


        }
        public void Paint_Longitudinal_DrawAreasMomentos(Graphics e, float Zoom, float HeightForm, float WidthW)
        {
            SolidBrush Brush_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(38, 86, 158)));
            SolidBrush Brush_Negativo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(227, 88, 88)));
            Pen PenBlack = new Pen(Brushes.Black, 1.5f); PenBlack.LineJoin = LineJoin.Round;
            SolidBrush Brush_Asignado_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(90, 90, 140)));
            SolidBrush Brush_Asignado_Negativos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(90, 90, 140)));

            #region Areas Rqueridas
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    List<PointF> Areas_Momento_Negativos_Escalados = new List<PointF>(); List<PointF> Areas_Momento_Positivos_Escalados = new List<PointF>();
                    List<PointF> Areas_Momento_Negativos_SinEscalados = new List<PointF>(); List<PointF> Areas_Momento_Positivos_SinEscalados = new List<PointF>();
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    Areas_Momento_Negativos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales);
                    Areas_Momento_Positivos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales);
                    Areas_Momento_Negativos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Escaladas);
                    Areas_Momento_Positivos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Escaladas);

                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Momento_Negativos_Escalados, Areas_Momento_Negativos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Y0_Escalado.Y);
                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Momento_Positivos_Escalados, Areas_Momento_Positivos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Y0_Escalado.Y);
                    float YInicial = Areas_Momento_Negativos_Escalados[0].Y; float YFinal = Areas_Momento_Negativos_Escalados[0].Y;


                    e.FillPolygon(Brush_Positivos, Areas_Momento_Positivos_Escalados.ToArray());
                    e.FillPolygon(Brush_Negativo, Areas_Momento_Negativos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Areas_Momento_Positivos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Areas_Momento_Negativos_Escalados.ToArray());
                }
            });
            #endregion



            #region Areas Asignadas
            List<PointF> Areas_Momento_Negativos_Escalados2 = new List<PointF>(); List<PointF> Areas_Momento_Positivos_Escalados2 = new List<PointF>();
            List<PointF> Areas_Momento_Negativos_SinEscalados2 = new List<PointF>(); List<PointF> Areas_Momento_Positivos_SinEscalados2 = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    Areas_Momento_Negativos_SinEscalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Reales);
                    Areas_Momento_Positivos_SinEscalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Reales);
                    Areas_Momento_Negativos_Escalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Escaladas);
                    Areas_Momento_Positivos_Escalados2.AddRange(SubtramoAux.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Escaladas);
                }
            });

            cSubTramo SubtramoAux2 = (cSubTramo)Lista_Elementos.Find(x => x is cSubTramo);

            cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Momento_Negativos_Escalados2, Areas_Momento_Negativos_SinEscalados2, SubtramoAux2.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Y0_Escalado.Y);
            cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Momento_Positivos_Escalados2, Areas_Momento_Positivos_SinEscalados2, SubtramoAux2.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Y0_Escalado.Y);
            e.FillPolygon(Brush_Asignado_Positivos, Areas_Momento_Positivos_Escalados2.ToArray());
            e.FillPolygon(Brush_Asignado_Negativos, Areas_Momento_Negativos_Escalados2.ToArray());
            e.DrawLines(PenBlack, Areas_Momento_Positivos_Escalados2.ToArray());
            e.DrawLines(PenBlack, Areas_Momento_Negativos_Escalados2.ToArray());

            #endregion

            GraficarRectaApoyos(e, HeightForm);
            #region Muestra Valores de Areas Requeridas con el Mouse

            CrearCuadrodeInfo(e, Zoom, PuntoInMouseAreasMomentos_Escalado_Real, "A", "cm²", WidthW);
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseAreasMomentosAsignado_Escaldo_Real, "A", "cm²", WidthW);
            #endregion

        }

        public void Paint_Longitudinal_DrawCortante(Graphics e, float Zoom, float HeightForm, float WidthW)
        {

            #region Graficar Fi_Vc

            SolidBrush BrushFiVc = new SolidBrush(Color.FromArgb(60, Color.FromArgb(0, 0, 0)));
            SolidBrush BrushFiVc2 = new SolidBrush(Color.FromArgb(30, Color.FromArgb(10, 10, 10)));
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    List<PointF> Cortante_FiVc_Positivos_Escalados = new List<PointF>(); List<PointF> Cortante_FiVc_Negativos_Escalados = new List<PointF>();
                    List<PointF> Cortante_FiVc_Positivos_Reales = new List<PointF>(); List<PointF> Cortante_FiVc_Negativos_Reales = new List<PointF>();


                    List<PointF> Cortante_FiVc2_Positivos_Escalados = new List<PointF>(); List<PointF> Cortante_FiVc2_Negativos_Escalados = new List<PointF>();
                    List<PointF> Cortante_FiVc2_Positivos_Reales = new List<PointF>(); List<PointF> Cortante_FiVc2_Negativos_Reales = new List<PointF>();


                    cSubTramo SubtramoAux = (cSubTramo)Elemento;


                    Cortante_FiVc2_Negativos_Escalados.AddRange(SubtramoAux.Coordenadas_FI_Vc2_Negativos.Escaladas);
                    Cortante_FiVc2_Positivos_Escalados.AddRange(SubtramoAux.Coordenadas_FI_Vc2_Positivos.Escaladas);
                    Cortante_FiVc2_Negativos_Reales.AddRange(SubtramoAux.Coordenadas_FI_Vc2_Negativos.Reales);
                    Cortante_FiVc2_Positivos_Reales.AddRange(SubtramoAux.Coordenadas_FI_Vc2_Positivos.Reales);





                    List<PointF> PuntosFiVc2Auxiliar_Negativos = Cortante_FiVc2_Negativos_Escalados.ToList(); PuntosFiVc2Auxiliar_Negativos.Reverse();

                    Cortante_FiVc_Negativos_Escalados.AddRange(SubtramoAux.Coordenadas_FI_Vc_Negativos.Escaladas);
                    Cortante_FiVc_Negativos_Escalados.AddRange(PuntosFiVc2Auxiliar_Negativos);


                    List<PointF> PuntosFiVc2Auxiliar_Positivos = Cortante_FiVc2_Positivos_Escalados.ToList(); PuntosFiVc2Auxiliar_Positivos.Reverse();
                    Cortante_FiVc_Positivos_Escalados.AddRange(SubtramoAux.Coordenadas_FI_Vc_Positivos.Escaladas);
                    Cortante_FiVc_Positivos_Escalados.AddRange(PuntosFiVc2Auxiliar_Positivos);




                    Cortante_FiVc_Negativos_Reales.AddRange(SubtramoAux.Coordenadas_FI_Vc_Negativos.Reales);
                    Cortante_FiVc_Positivos_Reales.AddRange(SubtramoAux.Coordenadas_FI_Vc_Positivos.Reales);




                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Cortante_FiVc2_Negativos_Escalados, Cortante_FiVc2_Negativos_Reales, SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Y0_Escalado.Y);
                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Cortante_FiVc2_Positivos_Escalados, Cortante_FiVc2_Positivos_Reales, SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Y0_Escalado.Y);

                    e.FillPolygon(BrushFiVc, Cortante_FiVc_Positivos_Escalados.ToArray());
                    e.FillPolygon(BrushFiVc, Cortante_FiVc_Negativos_Escalados.ToArray());

                    e.FillPolygon(BrushFiVc2, Cortante_FiVc2_Negativos_Escalados.ToArray());
                    e.FillPolygon(BrushFiVc2, Cortante_FiVc2_Positivos_Escalados.ToArray());


                }
            });



            #endregion





            SolidBrush Brush_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(38, 86, 158)));
            SolidBrush Brush_Negativo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(227, 88, 88)));
            Pen PenBlack = new Pen(Brushes.Black, 1.5f); PenBlack.LineJoin = LineJoin.Round;


            #region Cortante Solicictaciones 
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    List<PointF> Cortante_Negativos_Escalados = new List<PointF>(); List<PointF> Cortante_Positivos_Escalados = new List<PointF>();
                    List<PointF> Cortante_Negativos_SinEscalados = new List<PointF>(); List<PointF> Cortante_Positivos_SinEscalados = new List<PointF>();

                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    Cortante_Negativos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales);
                    Cortante_Positivos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales);
                    Cortante_Negativos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Escaladas);
                    Cortante_Positivos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Escaladas);

                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Cortante_Negativos_Escalados, Cortante_Negativos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Y0_Escalado.Y);
                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Cortante_Positivos_Escalados, Cortante_Positivos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Y0_Escalado.Y);
                    e.FillPolygon(Brush_Positivos, Cortante_Positivos_Escalados.ToArray());
                    e.FillPolygon(Brush_Negativo, Cortante_Negativos_Escalados.ToArray());

                    e.DrawLines(PenBlack, Cortante_Positivos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Cortante_Negativos_Escalados.ToArray());
                }
            });

            #endregion




            GraficarRectaApoyos(e, HeightForm);
            #region Muestra Valores de Cortante con el Mouse
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseCortante_Escalado_Real, "V", "Ton", WidthW);
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseFiCortante_Escalado_Real, "ΦVc", "Ton", WidthW);
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseFiCortante2_Escalado_Real, "ΦVc/2", "Ton", WidthW);
            #endregion





        }

        public void Paint_Longitudinal_DrawAreasCortante(Graphics e, float Zoom, float HeightForm, float WidthW)
        {
            SolidBrush Brush_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(38, 86, 158)));
            SolidBrush Brush_Negativo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(227, 88, 88)));
            Pen PenBlack = new Pen(Brushes.Black, 1.5f); PenBlack.LineJoin = LineJoin.Round;
            Pen PenPunteado_Black = new Pen(Brushes.Black, 1f); PenPunteado_Black.DashStyle = DashStyle.Dash;
            Pen PenPunteado_Red = new Pen(Brushes.Red, 1f); PenPunteado_Red.DashStyle = DashStyle.Solid;
            SolidBrush Brush_Asignado_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(90, 90, 140)));
            SolidBrush Brush_Asignado_Negativos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(90, 90, 140)));

            #region Areas Cortante Solicictaciones 
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                   // List<PointF> Areas_Cortante_Negativos_Escalados = new List<PointF>(); 
                    List<PointF> Areas_Cortante_Positivos_Escalados = new List<PointF>();
                   // List<PointF> Areas_Cortante_Negativos_SinEscalados = new List<PointF>(); 
                    List<PointF> Areas_Cortante_Positivos_SinEscalados = new List<PointF>();

                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    //Areas_Cortante_Negativos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Reales);
                    Areas_Cortante_Positivos_SinEscalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Reales);
                    //Areas_Cortante_Negativos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Escaladas);
                    Areas_Cortante_Positivos_Escalados.AddRange(SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Escaladas);

                    //cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Cortante_Negativos_Escalados, Areas_Cortante_Negativos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Y0_Escalado.Y);
                    cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Cortante_Positivos_Escalados, Areas_Cortante_Positivos_SinEscalados, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Y0_Escalado.Y);
                    e.FillPolygon(Brush_Negativo, Areas_Cortante_Positivos_Escalados.ToArray());
                    //e.FillPolygon(Brush_Negativo, Areas_Cortante_Negativos_Escalados.ToArray());

                    e.DrawLines(PenBlack, Areas_Cortante_Positivos_Escalados.ToArray());
                    //e.DrawLines(PenBlack, Areas_Cortante_Negativos_Escalados.ToArray());
                }
            });

            #endregion


            #region Areas Cortante Asignado

            Lista_Tramos.ForEach(Tramo =>
            {


                //List<PointF> Areas_Cortante_Negativos_Escalados = new List<PointF>(); 
                List<PointF> Areas_Cortante_Positivos_Escalados = new List<PointF>();
                //List<PointF> Areas_Cortante_Negativos_SinEscalados = new List<PointF>(); 
                List<PointF> Areas_Cortante_Positivos_SinEscalados = new List<PointF>();

                Tramo.Lista_SubTramos.ForEach(SubTramo =>
                {

                    //Areas_Cortante_Negativos_SinEscalados.AddRange(SubTramo.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Reales);
                    Areas_Cortante_Positivos_SinEscalados.AddRange(SubTramo.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Reales);
                  //  Areas_Cortante_Negativos_Escalados.AddRange(SubTramo.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Escaladas);
                    Areas_Cortante_Positivos_Escalados.AddRange(SubTramo.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Escaladas);
                });

                //cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Cortante_Negativos_Escalados, Areas_Cortante_Negativos_SinEscalados, Tramo.Lista_SubTramos.First().CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Y0_Escalado.Y);
                cFunctionsProgram.CerrarPoligonoParaMomentos(ref Areas_Cortante_Positivos_Escalados, Areas_Cortante_Positivos_SinEscalados, Tramo.Lista_SubTramos.First().CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Y0_Escalado.Y);
                e.FillPolygon(Brush_Asignado_Positivos, Areas_Cortante_Positivos_Escalados.ToArray());
                //e.FillPolygon(Brush_Asignado_Negativos, Areas_Cortante_Negativos_Escalados.ToArray());

                e.DrawLines(PenBlack, Areas_Cortante_Positivos_Escalados.ToArray());
               // e.DrawLines(PenBlack, Areas_Cortante_Negativos_Escalados.ToArray());

            });


            #endregion



            GraficarRectaApoyos(e, HeightForm);
            #region Muestra Valores de Cortante con el Mouse
            CrearCuadrodeInfo(e, Zoom, PuntoInMouseAreasCortante_Escalado_Real, "A/S", "cm²/cm", WidthW, 4);
            #endregion




        }

        private void CrearCuadrodeInfo(Graphics e, float Zoom, PointF[] Valores, string VariableEjeY, string UnidadesEjeY, float WidthW, int CantDecimales = 2)
        {
            float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
            if (Valores != null && Valores.Length != 0)
            {

                string Text = $"(X= {Math.Round(Valores[1].X, 2)}, {VariableEjeY}= {Math.Abs(Math.Round(Valores[1].Y, CantDecimales))}{UnidadesEjeY})";
                SizeF MeasureString = e.MeasureString(Text, Font1); float BordeRectangulo = 3f;
                if (Valores[0].X + MeasureString.Width + BordeRectangulo >= WidthW)
                {
                    Valores[0].X = Valores[0].X - 1.3f * MeasureString.Width - BordeRectangulo;
                }
                float XString = Valores[0].X + MeasureString.Width / 4; float YString = Valores[0].Y;
                PointF PuntoString = new PointF(XString, YString);

                PointF[] PuntosRectangulo = new PointF[] { new PointF(XString-BordeRectangulo, YString+MeasureString.Height+ BordeRectangulo),
                                                            new PointF(XString+ MeasureString.Width+BordeRectangulo, YString+MeasureString.Height+ BordeRectangulo),
                                                            new PointF(XString+ MeasureString.Width+BordeRectangulo, YString- BordeRectangulo),
                                                            new PointF(XString-BordeRectangulo, YString- BordeRectangulo) };
                e.FillPolygon(new SolidBrush(Color.FromArgb(253, 255, 220)), PuntosRectangulo);
                e.DrawPolygon(Pens.Black, PuntosRectangulo);
                e.DrawString(Text, Font1, Brushes.Black, PuntoString);
            }

        }

        public void Paint_Longitudinal_Elementos_Escalados_AutoCAD(Graphics e, List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeightForm, float Dx, float Dy, float Zoom, float XI)
        {
            CrearCoordenadasLongitudinal_Elementos_Escalados_AutoCAD(PuntosTodosObjetos, HeigthDraw, HeightForm, Dx, Dy, Zoom, XI);
            Pen Pen_Borde_Subtramo = new Pen(Color.Black, 1);
            //Pen_Borde_Subtramo.DashStyle = DashStyle.Dot;
            Pen Pen_Borde_Apoyo = new Pen(Color.Black, 1);
            //Pen_Borde_Apoyo.DashStyle = DashStyle.Dash;
            Brush relleno = new SolidBrush(Color.FromArgb(200, 200, 200));
            Brush rellenoApoyo = new SolidBrush(Color.FromArgb(150, 150, 150));

            float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cApoyo)
                {

                    e.DrawPolygon(Pen_Borde_Apoyo, Elemento.Vistas.Perfil_AutoCAD.Escaladas.ToArray());
                    e.FillPolygon(rellenoApoyo, Elemento.Vistas.Perfil_AutoCAD.Escaladas.ToArray());
                }
                else
                {
                    e.DrawPolygon(Pen_Borde_Subtramo, Elemento.Vistas.Perfil_AutoCAD.Escaladas.ToArray());
                    e.FillPolygon(relleno, Elemento.Vistas.Perfil_AutoCAD.Escaladas.ToArray());
                }
            });

            GraficarRectaApoyos(e, HeightForm);

            Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.Paint(e, Zoom, HeightForm));
            Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.Paint(e, Zoom, HeightForm));

            Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.ForEach(y => y.Paint(e, PuntosTodosObjetos, EscalaMayorenX, HeightForm, Dx, Dy, Zoom, XI));


            Lista_Tramos.ForEach(Tramo =>
            {
                if (Tramo.EstribosDerecha != null)
                {
                    Tramo.EstribosDerecha.Zona1.PaintEstribos(e, Zoom);
                    Tramo.EstribosDerecha.Zona2.PaintEstribos(e, Zoom);
                }
                if (Tramo.EstribosIzquierda != null)
                {
                    Tramo.EstribosIzquierda.Zona1.PaintEstribos(e, Zoom);
                    Tramo.EstribosIzquierda.Zona2.PaintEstribos(e, Zoom);

                }

            });


        }

        private void GraficarRectaApoyos(Graphics e, float HeightForm)
        {
            float AltoLineaApoyo = HeightForm;
            Pen Pen_Apoyo = new Pen(Color.Black); Pen_Apoyo.DashStyle = DashStyle.Dot;
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cApoyo)
                {
                    cApoyo Apoyo = (cApoyo)Elemento;
                    if (Apoyo.Vistas.Perfil_Original.Escaladas != null)
                    {
                        e.DrawLine(Pen_Apoyo, Apoyo.Vistas.Perfil_Original.Escaladas.First().X, AltoLineaApoyo, Apoyo.Vistas.Perfil_Original.Escaladas.First().X, 0f);
                        e.DrawLine(Pen_Apoyo, Apoyo.Vistas.Perfil_Original.Escaladas.Last().X, AltoLineaApoyo, Apoyo.Vistas.Perfil_Original.Escaladas.Last().X, 0f);
                    }
                }
            });
        }

        private PointF[] PuntoInMouseMomentos_Escalado_Real = new PointF[] { };

        public void IsPointMouseMomentos(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Momentos_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Momentos_Negativos.Escaladas);
                }
            });

            PuntoInMouseMomentos_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseMomentos_Escalado_Real == null | PuntoInMouseMomentos_Escalado_Real.Length == 0)
                PuntoInMouseMomentos_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);
        }

        private PointF[] PuntoInMouseAreasMomentos_Escalado_Real = new PointF[] { };
        public void IsPointMouseAreasMomentos(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Escaladas);
                }
            });

            PuntoInMouseAreasMomentos_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseAreasMomentos_Escalado_Real == null | PuntoInMouseAreasMomentos_Escalado_Real.Length == 0)
                PuntoInMouseAreasMomentos_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);

        }


        private PointF[] PuntoInMouseCortante_Escalado_Real = new PointF[] { };

        public void IsPointMouseCortante(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Cortante_Positvos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Cortante_Negativos.Escaladas);
                }
            });

            PuntoInMouseCortante_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseCortante_Escalado_Real == null | PuntoInMouseCortante_Escalado_Real.Length == 0)
                PuntoInMouseCortante_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);
        }


        private PointF[] PuntoInMouseFiCortante_Escalado_Real = new PointF[] { };
        public void IsPointMouseFiCortante(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc_Negativos.Escaladas);
                }
            });

            PuntoInMouseFiCortante_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseFiCortante_Escalado_Real == null | PuntoInMouseFiCortante_Escalado_Real.Length == 0)
                PuntoInMouseFiCortante_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);

        }

        private PointF[] PuntoInMouseFiCortante2_Escalado_Real = new PointF[] { };
        public void IsPointMouseFiCortante2(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc2_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc2_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc2_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).Coordenadas_FI_Vc2_Negativos.Escaladas);
                }
            });

            PuntoInMouseFiCortante2_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseFiCortante2_Escalado_Real == null | PuntoInMouseFiCortante2_Escalado_Real.Length == 0)
                PuntoInMouseFiCortante2_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);

        }

        private PointF[] PuntoInMouseAreasCortante_Escalado_Real = new PointF[] { };

        public void IsPointMouseAreasCortanteSolicitado(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Escaladas);
                }
            });

            PuntoInMouseAreasCortante_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseAreasCortante_Escalado_Real == null | PuntoInMouseAreasCortante_Escalado_Real.Length == 0)
                PuntoInMouseAreasCortante_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);

        }


        private PointF[] PuntoInMouseAreasMomentosAsignado_Escaldo_Real = new PointF[] { };
        public void IsPointMouseAreasMomentosAsignado(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Escaladas);
                }
            });

            PuntoInMouseAreasMomentosAsignado_Escaldo_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseAreasMomentosAsignado_Escaldo_Real == null | PuntoInMouseAreasMomentosAsignado_Escaldo_Real.Length == 0)
                PuntoInMouseAreasMomentosAsignado_Escaldo_Real = IsMousePoint(Location, RealesN, EscaladasN);

        }


        private PointF[] PuntoInMouseAreasMomentosAsignado_Escalado_Real = new PointF[] { };
        public void IsPointMouseMomentosAsignado(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            List<PointF> RealesN = new List<PointF>(); List<PointF> EscaladasN = new List<PointF>();
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cSubTramo)
                {
                    RealesP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Momentos_Positivos.Reales);
                    EscaladasP.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Momentos_Positivos.Escaladas);
                    RealesN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Momentos_Negativos.Reales);
                    EscaladasN.AddRange(((cSubTramo)Elemento).CoordenadasCalculosAsignado.Momentos_Negativos.Escaladas);
                }
            });

            PuntoInMouseAreasMomentosAsignado_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
            if (PuntoInMouseAreasMomentosAsignado_Escalado_Real == null | PuntoInMouseAreasMomentosAsignado_Escalado_Real.Length == 0)
                PuntoInMouseAreasMomentosAsignado_Escalado_Real = IsMousePoint(Location, RealesN, EscaladasN);
        }


        private PointF[] PuntoInMousePointLines_Escalado_Real = new PointF[] { };
        public void IsPointMousePointLines(PointF Location)
        {
            List<PointF> RealesP = new List<PointF>(); List<PointF> EscaladasP = new List<PointF>();
            Lista_Objetos.ForEach(Objeto =>
            {
                RealesP.AddRange(Objeto.Line.Planta_Real);
                EscaladasP.AddRange(Objeto.Line.Planta_Escalado);
            });
            PuntoInMousePointLines_Escalado_Real = IsMousePoint(Location, RealesP, EscaladasP);
        }






        private PointF[] IsMousePoint(PointF Location, List<PointF> PuntosReales, List<PointF> PuntosEscalados)
        {
            float Delta = 5f; int Indice = 0;
            foreach (PointF Punto in PuntosEscalados)
            {
                PointF[] PointsPaths_Positivo = new PointF[] {new PointF( Punto.X-Delta,Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y -Delta),
                                           new PointF(Punto.X-Delta, Punto.Y - Delta) };
                GraphicsPath Path_Po = new GraphicsPath();
                Path_Po.AddPolygon(PointsPaths_Positivo);
                if (Path_Po.IsVisible(Location))
                {
                    return new PointF[] { Punto, PuntosReales[Indice] };

                }

                Indice++;
            }
            return new PointF[] { };
        }








        #endregion Metodos Paint



        #region Metodos Para Graficar en AutoCAD
        public void GraficarEnAutoCAD(float X, float Y)
        {
            float LCuadroINFO = cVariables.Dimension_InfoNervio;
            GraficarAutoCADEjes(X + LCuadroINFO, Y);
            GraficarAutoCADContornoDeNervioyProyecciones(X, Y, LCuadroINFO);
            Tendencia_Refuerzos.TInfeSelect.Paint_AutoCAD(X + LCuadroINFO, Y);
            Tendencia_Refuerzos.TSupeSelect.Paint_AutoCAD(X + LCuadroINFO, Y);
            GraficarAutoCADCotasBordes(X + LCuadroINFO, Y);
            GraficarAutoCADEstribos(X + LCuadroINFO, Y);
        }

        private void GraficarAutoCADContornoDeNervioyProyecciones(float X, float Y, float LCuadroINFO)
        {

            List<PointF> VerticesPoligono = new List<PointF>();
            int C = 0;

            //Puntos de Polilinea Parte Superior
            Lista_Elementos.ForEach(Elemento =>
            {
                if (C == 0)
                {
                    PointF PuntoI = new PointF(0, Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y));
                    PointF PuntoI2 = new PointF(0, Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y));
                    PointF PuntoI3 = new PointF(LCuadroINFO, Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y));
                    PointF PuntoI4 = new PointF(LCuadroINFO + Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X), Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y));
                    VerticesPoligono.Add(PuntoI); VerticesPoligono.Add(PuntoI2); VerticesPoligono.Add(PuntoI3); VerticesPoligono.Add(PuntoI4);
                }
                else
                {
                    PointF Punto1 = new PointF(LCuadroINFO + Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X), Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y));
                    PointF Punto2 = new PointF(LCuadroINFO + Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X), Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y));
                    VerticesPoligono.Add(Punto1);
                    VerticesPoligono.Add(Punto2);
                }
                C += 1;
            });

            //Puntos de Polilinea Parte Inferior
            for (int i = Lista_Elementos.Count - 1; i >= 0; i--)
            {
                IElemento Elemento = Lista_Elementos[i];
                PointF Punto1 = new PointF(LCuadroINFO + Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X), Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y));
                PointF Punto2 = new PointF(LCuadroINFO + Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X), Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y));
                VerticesPoligono.Add(Punto1);
                VerticesPoligono.Add(Punto2);
            }
            List<PointF> PuntosTrasladadosF = B_Operaciones_Matricialesl.Operaciones.Traslacion(VerticesPoligono, X, Y);
            FunctionsAutoCAD.AddPolyline2D(PuntosTrasladadosF.ToArray(), cVariables.C_Bordes);

            //Crear Lineas de Proyección de Apoyos

            C = 0;
            Lista_Elementos.ForEach(Elemento =>
            {
                if (Elemento is cApoyo)
                {

                    List<PointF> PuntosLinea1 = new List<PointF>();
                    PuntosLinea1.Add(Elemento.Vistas.Perfil_AutoCAD.Reales.First());
                    PuntosLinea1.Add(Elemento.Vistas.Perfil_AutoCAD.Reales[1]);
                    List<PointF> PuntosLinea2 = new List<PointF>();
                    PuntosLinea2.Add(Elemento.Vistas.Perfil_AutoCAD.Reales[3]);
                    PuntosLinea2.Add(Elemento.Vistas.Perfil_AutoCAD.Reales[2]);
                    FunctionsAutoCAD.AddPolyline2D(B_Operaciones_Matricialesl.Operaciones.Traslacion(PuntosLinea1, X + LCuadroINFO, Y).ToArray(), cVariables.C_Proyeccion, false);
                    if (C != Lista_Elementos.Count - 1)
                        FunctionsAutoCAD.AddPolyline2D(B_Operaciones_Matricialesl.Operaciones.Traslacion(PuntosLinea2, X + LCuadroINFO, Y).ToArray(), cVariables.C_Proyeccion, false);
                }
                C++;
            });

            //Crear Linea Inicial

            if (!(Lista_Elementos.First() is cApoyo))
            {
                PointF[] Puntos = new PointF[] { Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.First(), Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales[1] };
                FunctionsAutoCAD.AddPolyline2D(B_Operaciones_Matricialesl.Operaciones.Traslacion(Puntos.ToList(), X + LCuadroINFO, Y).ToArray(), cVariables.C_Bordes, false);
            }


            //Agregar Nombre de Nervio y Dimensiones 

            List<string> NombresNervios= SimilitudNervioCompleto.NerviosSimilares.Select(y => y.Nombre).ToList();
            NombresNervios.Insert(0,Nombre);
            NombresNervios = NombresNervios.OrderBy(y => y).ToList();
            string TituloNervio_AutoCAD=string.Empty;
            string Espacio = string.Empty;
            if (NombresNervios.Count > 1)
                Espacio = @", \P";
            int c = 0;
            NombresNervios.ForEach(N => {
                TituloNervio_AutoCAD += @"{\L" + N + "}" + Espacio ;
                c++;
                if (c == NombresNervios.Count - 1)
                    Espacio = string.Empty;
            });

            float MinY = Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales.First().Y;
            float MaxY = Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales[1].Y;
            float LargoTexto = TituloNervio_AutoCAD.Length * cVariables.W_LetraAutoCADTextRefuerzo;
            double[] PString = new double[] { X + LCuadroINFO / 2f - LargoTexto / 2, Y + MinY + (MaxY - MinY) * 0.70f, 0 };
            FunctionsAutoCAD.AddText(TituloNervio_AutoCAD, PString, cVariables.W_TextoTituloViga,
                                     cVariables.H_TextoTituloViga, cVariables.C_Texto1, cVariables.Estilo_Texto, 0, Width2: LargoTexto, JustifyText: JustifyText.Center);

            string TextB, TextH, TextoFinal;

            cSubTramo Subtramo1 = (cSubTramo)Lista_Elementos.First(x => x is cSubTramo);

            TextB = Bool_CambioAncho ? "Vble" : Subtramo1.Seccion.B.ToString();
            TextH = Bool_CambioAncho ? "Vble" : Subtramo1.Seccion.H.ToString();

            cPiso PisoOrigen1 = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == PisoOrigen.Nombre);

            TextoFinal = $@"({TextB}x{TextH})\P{PisoOrigen1.NombreReal}\P{PisoOrigen1.Nivel}";
            LargoTexto = cVariables.Ancho_Cajon_Titulo; ;
            PointF P = new PointF(X + LCuadroINFO / 2f - LargoTexto / 2f, Y + MinY + (MaxY - MinY) * 0.55f);

            FunctionsAutoCAD.AddText(TextoFinal, P, cVariables.W_TextoTituloViga,
                           cVariables.H_TextoBarra, cVariables.C_TextRefuerzo, cVariables.Estilo_Texto, 0,
                           Width2: LargoTexto, JustifyText: JustifyText.Center);



        }

        private void GraficarAutoCADCotasBordes(float X, float Y)
        {
            //Cotas Superiores 


            Lista_Elementos.ForEach(Elemento =>
            {

                PointF P1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(Elemento.Vistas.Perfil_AutoCAD.Reales[1], X, Y);
                PointF P2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(Elemento.Vistas.Perfil_AutoCAD.Reales[2], X, Y);
                FunctionsAutoCAD.AddCota(P1, P2, cVariables.C_Cotas, cVariables.Estilo_Cotas, cVariables.Desplazamiento_Cotas,
                                         DeplazaTextY: cVariables.DesplazamientoTexto_Cotas);
            });

            //Cotas Inferiores por Cambio de Sección

            if (Bool_CambioAltura | Bool_CambioAncho)
            {
                float YminCota = Lista_Elementos.Min(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.Y));

                List<List<IElemento>> Lista_Elementos1 = cFunctionsProgram.CrearListaElementos(this, true, false, true, true, false);

                foreach (List<IElemento> elementos in Lista_Elementos1)
                {
                    float DesplaCota = -cVariables.Desplazamiento_Cotas;
                    PointF P1 = elementos.First().Vistas.Perfil_AutoCAD.Reales[0];
                    PointF P2 = elementos.Last().Vistas.Perfil_AutoCAD.Reales[3];
                    cSubTramo SubTramo = (cSubTramo)elementos.First(x => x is cSubTramo);
                    string Text = $"({SubTramo.Seccion.B}x{SubTramo.Seccion.H})";

                    if (P1.Y != YminCota)
                    {
                        P1 = new PointF(P1.X, YminCota);
                        P2 = new PointF(P2.X, YminCota);
                    }


                    FunctionsAutoCAD.AddCota(B_Operaciones_Matricialesl.Operaciones.Traslacion(P1, X, Y),
                                             B_Operaciones_Matricialesl.Operaciones.Traslacion(P2, X, Y),
                                             cVariables.C_Cotas, cVariables.Estilo_Cotas, DesplaCota, Text: Text);
                }

            }

        }


        private void GraficarAutoCADEjes(float X, float Y)
        {
            float Ymax = Lista_Elementos.Max(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.Y));
            Grids.ForEach(Grid =>
            {

                //Graficar Circulos
                float CoordX = Grid.Recta_Real.First().X;
                PointF Punto = new PointF(X + CoordX, Y + Ymax + cVariables.HCentro_Eje);
                FunctionsAutoCAD.B_Ejes(Punto, Grid.Nombre, cVariables.C_Texto1, 75, 75, 75, 0);

                float YminEje;
                IElemento Elemento = Lista_Elementos.Find(x => x.IsVisibleCoordAutoCAD(CoordX));
                if (Elemento != null)
                {
                    YminEje = Elemento is cApoyo ? Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y) : Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y);

                    PointF Punto1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(CoordX, Ymax + cVariables.H1_Eje), X, Y);
                    PointF Punto2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(CoordX, YminEje), X, Y);
                    PointF[] Puntos = new PointF[] { Punto1, Punto2 };
                    FunctionsAutoCAD.AddPolyline2D(Puntos, cVariables.C_Ejes, false);
                }
            });

            grid = Grids.OrderBy(x => x.CoordenadaInicial).ToList();

            //Cotas

            void AgregarCotas(float X1, float X2)
            {
                PointF P1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(X1, Ymax + cVariables.Desplazamiento_Cotas * 2f), X, Y);
                PointF P2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(X2, Ymax + cVariables.Desplazamiento_Cotas * 2f), X, Y);
                FunctionsAutoCAD.AddCota(P1, P2, cVariables.C_Cotas, cVariables.Estilo_Cotas, 0,
                                         DeplazaTextY: cVariables.DesplazamientoTexto_Cotas, TextHeight: 0.002f);

            }


            for (int i = 0; i < Grids.Count; i++)
            {
                float X1, X2;
                cGrid Grid = Grids[i];
                if (i == 0)
                {
                    X1 = Lista_Elementos.First().Vistas.Perfil_AutoCAD.Reales[0].X;
                    X2 = Grid.CoordenadaInicial;
                    AgregarCotas(X1, X2);
                }
                if (i == Grids.Count - 1)
                {
                    X1 = Grid.CoordenadaInicial;
                    X2 = Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales[2].X;
                    AgregarCotas(X1, X2);
                }

                if (i + 1 < Grids.Count)
                {
                    X1 = Grid.CoordenadaInicial;
                    X2 = Grids[i + 1].CoordenadaInicial;
                    AgregarCotas(X1, X2);
                }


            }



        }

        private void GraficarAutoCADEstribos(float X, float Y)
        {
            Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.ForEach(y => {

                y.PaintAutoCAD(X, Y);
            });

            Lista_Tramos.ForEach(Tramo =>
            {

                if (Tramo.EstribosDerecha != null)
                {
                    Tramo.EstribosDerecha.Zona1.PaintAutoCAD(X, Y);
                    Tramo.EstribosDerecha.Zona2.PaintAutoCAD(X, Y);
                }

                if (Tramo.EstribosIzquierda != null)
                {
                    Tramo.EstribosIzquierda.Zona1.PaintAutoCAD(X, Y);
                    Tramo.EstribosIzquierda.Zona2.PaintAutoCAD(X, Y);
                }
            });


        }


        #endregion


        #region Metodos Similares

        #region Metodos y Propiedades para Paint
        public bool SelectSimilar = false;
        public void Paint_SimilaresOpenGL()
        {
            Lista_Objetos.ForEach(y => { if (y.Soporte == eSoporte.Vano) { y.Line.PaintPlantaEscalada(SelectSimilar,SimilitudNervioGeometria.IsMaestro,SimilitudNervioGeometria.BoolSoySimiarA,SimilitudNervioCompleto.IsMaestro,SimilitudNervioCompleto.BoolSoySimiarA); } });
        }

        public bool MouseDownSelectSimilar(Point Point,bool AsignarValor=true)
        {
            bool SelectEncotrado = false;
            foreach (cTramo Tramo in Lista_Tramos)
            {
                foreach (cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    if (Objeto.Line.MouseInLineEtabs(Point))
                    {
                        SelectEncotrado = true;
                        if (AsignarValor)
                        {
                            SelectSimilar = !SelectSimilar;
                            if(SimilitudNervioCompleto.BoolSoySimiarA | SimilitudNervioCompleto.IsMaestro
                                | SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro)
                            {
                                SelectSimilar = false;
                            }
                        }
                    }
 
                    if (SelectEncotrado) { break; }
                }
                if (SelectEncotrado) { break; }
            }
            return SelectEncotrado;
        }

        #endregion


        public void AsignarCambiosANerviosSimilares(int IndiceTramo, int IndiceSubTramo)
        {
            cSimilitudNervio Similitud = (SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro) ?
                                         SimilitudNervioGeometria : SimilitudNervioCompleto;

            Similitud.NerviosSimilares.ForEach(N =>
            {
                cSeccion Seccion = N.Lista_Tramos[IndiceTramo].Lista_SubTramos[IndiceSubTramo].Seccion;
                Seccion.B = Lista_Tramos[IndiceTramo].Lista_SubTramos[IndiceSubTramo].Seccion.B; Seccion.H = Lista_Tramos[IndiceTramo].Lista_SubTramos[IndiceSubTramo].Seccion.H;
                N.Lista_Tramos[IndiceTramo].Lista_SubTramos[IndiceSubTramo].Seccion = Seccion;
                N.Lista_Tramos[IndiceTramo].Lista_SubTramos[IndiceSubTramo].Longitud = Lista_Tramos[IndiceTramo].Lista_SubTramos[IndiceSubTramo].Longitud;
            });
        }

        public void AsignarCambiosANerviosSimilares()
        {
            cSimilitudNervio Similitud = (SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro) ?
                                SimilitudNervioGeometria : SimilitudNervioCompleto;
            Similitud.NerviosSimilares.ForEach(N =>
            {
                N.CambioenAncho = CambioenAncho;
                N.CambioenAltura = CambioenAltura;
            });
        }
        public void AsignarCambiosANerviosSimilares(int IndiceApoyo)
        {
            cSimilitudNervio Similitud = (SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro) ?
                             SimilitudNervioGeometria : SimilitudNervioCompleto;
            Similitud.NerviosSimilares.ForEach(N =>
            {

                if (N.Lista_Elementos.Count - 1 >= IndiceApoyo)
                {
                    if (N.Lista_Elementos[IndiceApoyo] is cApoyo)
                    {
                        cSeccion Seccion = N.Lista_Elementos[IndiceApoyo].Seccion;
                        Seccion.B = Lista_Elementos[IndiceApoyo].Seccion.B; Seccion.H = Lista_Elementos[IndiceApoyo].Seccion.H;
                        N.Lista_Elementos[IndiceApoyo].Seccion = Seccion;

                    }
                    else
                    {
                        cFunctionsProgram.VentanaEmergenteError($"No se puede modificar el apoyo de los nervios similares. Verifique que {N.Nombre} tenga la misma cantidad de apoyos.");
                    }

                }
                else
                {
                    cFunctionsProgram.VentanaEmergenteError($"No se puede modificar el apoyo de los nervios similares. Verifique que {N.Nombre} tenga la misma cantidad de apoyos.");
                }
            });

        }

        public void AsignarCambiosANerviosRecubrimientoSimilares()
        {
            cSimilitudNervio Similitud = (SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro) ?
                 SimilitudNervioGeometria : SimilitudNervioCompleto;
            Similitud.NerviosSimilares.ForEach(N =>
            {
                N.r1 = r1;
                N.r2 = r2;
            });
        }
        #endregion


        #region Metodos Auxiliares

        public void CrearApoyosAExtremos(bool ApoyoInicio = false, bool ApoyoFinal = false)
        {
            if (ApoyoInicio)
            {
                if (Lista_Elementos.First() is cSubTramo)
                {
                    int Contador = 0;
                    Lista_Elementos.Insert(0, new cApoyo("Apoyo 0", new cSeccion("FC0", cVariables.AnchoApoyoPredefinido, Lista_Elementos.First().Seccion.H), this));
                    Lista_Elementos.ForEach(x => { x.Indice = Contador; Contador++; });
                    CrearCoordenadasPerfilLongitudinalReales();
                    CrearCoordenadasPerfilLongitudinalAutoCAD();
                    //CrearEnvolvente();
                    CrearAceroAsignadoRefuerzoLongitudinal();
                    CrearAceroAsignadoRefuerzoTransversal();
                    Tendencia_Refuerzos.NervioOrigen = this; Tendencia_Refuerzos.TInfeSelect.MaximaLongitud = cVariables.MaximaLongitud;
                    AsignarMaximaLongitudTendencias();
                }

            }

            if (ApoyoFinal)
            {
                if (Lista_Elementos.Last() is cSubTramo)
                {
                    int Contador = 0;
                    Lista_Elementos.Add(new cApoyo("ApoyoFinal", new cSeccion("FCFinal", cVariables.AnchoApoyoPredefinido, Lista_Elementos.Last().Seccion.H), this));
                    Lista_Elementos.ForEach(x => { x.Indice = Contador; Contador++; });
                    CrearCoordenadasPerfilLongitudinalReales();
                    CrearCoordenadasPerfilLongitudinalAutoCAD();
                    //CrearEnvolvente();
                    CrearAceroAsignadoRefuerzoLongitudinal();
                    CrearAceroAsignadoRefuerzoTransversal();
                    Tendencia_Refuerzos.NervioOrigen = this; Tendencia_Refuerzos.TInfeSelect.MaximaLongitud = cVariables.MaximaLongitud;
                    AsignarMaximaLongitudTendencias();
                }

            }
            cSimilitudNervio Similitud = (SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro) ?
             SimilitudNervioGeometria : SimilitudNervioCompleto;
            Similitud.NerviosSimilares.ForEach(y => y.CrearApoyosAExtremos(ApoyoInicio, ApoyoFinal));
            CrearEnvolvente();
            Similitud.NerviosSimilares.ForEach(y => y.CrearEnvolvente());
        }

        public void EliminarApoyosAExtremos(bool ApoyoInicio = false, bool ApoyoFinal = false)
        {
            if (ApoyoInicio)
            {
                if (PoderEliminarApoyos())
                {
                    int Contador = 0;
                    Lista_Elementos.RemoveAt(0);
                    Lista_Elementos.ForEach(x => { x.Indice = Contador; Contador++; });
                    CrearCoordenadasPerfilLongitudinalReales();
                    CrearCoordenadasPerfilLongitudinalAutoCAD();
                    //CrearEnvolvente();
                    CrearAceroAsignadoRefuerzoLongitudinal();
                    CrearAceroAsignadoRefuerzoTransversal();
                    Tendencia_Refuerzos.NervioOrigen = this; Tendencia_Refuerzos.TInfeSelect.MaximaLongitud = cVariables.MaximaLongitud;
                    AsignarMaximaLongitudTendencias();
                }

            }

            if (ApoyoFinal)
            {
                if (PoderEliminarApoyos())
                {
                    int Contador = 0;
                    Lista_Elementos.RemoveAt(Lista_Elementos.IndexOf(Lista_Elementos.Last()));
                    Lista_Elementos.ForEach(x => { x.Indice = Contador; Contador++; });
                    CrearCoordenadasPerfilLongitudinalReales();
                    CrearCoordenadasPerfilLongitudinalAutoCAD();
                    //CrearEnvolvente();
                    CrearAceroAsignadoRefuerzoLongitudinal();
                    CrearAceroAsignadoRefuerzoTransversal();
                    Tendencia_Refuerzos.NervioOrigen = this; Tendencia_Refuerzos.TInfeSelect.MaximaLongitud = cVariables.MaximaLongitud;
                    AsignarMaximaLongitudTendencias();
                }

            }
            cSimilitudNervio Similitud = (SimilitudNervioGeometria.BoolSoySimiarA | SimilitudNervioGeometria.IsMaestro) ?
            SimilitudNervioGeometria : SimilitudNervioCompleto;
            Similitud.NerviosSimilares.ForEach(y => y.EliminarApoyosAExtremos(ApoyoInicio, ApoyoFinal));
            CrearEnvolvente();
            Similitud.NerviosSimilares.ForEach(y => y.CrearEnvolvente());
        }

        public bool PoderEliminarApoyos()
        {
            return (Lista_Elementos.First() is cApoyo || Lista_Elementos.Last() is cApoyo) && Lista_Elementos.FindAll(y => y is cApoyo).Count > 1 &&
               !SinRefuerzos_();

        }



        public void EliminarTodoElRefuerzo()
        {
            Tendencia_Refuerzos.TendenciasInferior.ForEach(y => y.EliminarBarras());
            Tendencia_Refuerzos.TendenciasSuperior.ForEach(y => y.EliminarBarras());
            Tendencia_Refuerzos.TendenciasEstribos.ForEach(y => y.EliminarBloquesEstribos());
        }

        public bool SinRefuerzos_()
        {
            bool Habilitar = false;
            foreach (cTendencia tendencia in Tendencia_Refuerzos.TendenciasSuperior)
            {
                if (tendencia.Barras.Count == 0)
                    Habilitar = false;
                else
                    return true;
            }
            foreach (cTendencia tendencia in Tendencia_Refuerzos.TendenciasInferior)
            {
                if (tendencia.Barras.Count == 0)
                    Habilitar = false;
                else
                    return true;
            }
            foreach(Clases_y_Enums.Nervio.Estribo.cTendencia_Estribo TE in Tendencia_Refuerzos.TendenciasEstribos)
            {
                if (TE.BloqueEstribos.Count == 0)
                    Habilitar = false;
                else
                    return true;
            }
            return Habilitar;
        }
        #endregion
    }

    [Serializable]
    public enum eCambioenAltura
    {
        Ninguno,
        Central,
        Superior,
        Inferior
    }

    [Serializable]
    public enum eCambioenAncho
    {
        Ninguno,
        Central,
        Superior,  //Izquierda
        Inferior  //Derecha
    }
}