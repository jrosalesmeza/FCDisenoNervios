using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cNervio
    {
        public float EscalaMayorenX;
        public float EscalaMayorenY;
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Prefijo { get; set; } = "N-";
        public int CantApoyos { get; set; } = 0;

        public float d1_ = 4f;

        public float d1
        {
            get { return d1_; }
            set
            {
                if (value != d1_)
                {
                    d1_ = value;
                    CrearEnvolvente();
                }
            }
        }

        public float d2_;

        public float d2
        {
            get { return d2_; }
            set
            {
                if (value != d2_)
                {
                    d2_ = value;
                    CrearEnvolvente();
                }
            }
        }

        public eDireccion Direccion { get; set; }
        public List<cTramo> Lista_Tramos { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }
        public List<cGrid> Grids { get; set; }
        public List<IElemento> Lista_Elementos { get; set; }
        public bool Bool_CambioAltura { get; set; }
        public bool Bool_CambioAncho { get; set; }
        public bool Select { get; set; }
        public bool SelectPlantaEnumeracion { get; set; }
        private bool ElementoEnumerado_MouseMove = false;
        public cObjeto ObjetoSelect { get; set; }
        public cTendencia_Refuerzo Tendencia_Refuerzos { get; set; } = new cTendencia_Refuerzo();
        public cPiso PisoOrigen { get; set; }

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
            this.Grids = Grids;
            this.PisoOrigen = PisoOrigen; Tendencia_Refuerzos.NervioOrigen = this; 
            Tendencia_Refuerzos.TendenciasInferior.Add(cFunctionsProgram.CrearTendenciaDefault(1,Tendencia_Refuerzos)); Tendencia_Refuerzos.TInfeSelect = Tendencia_Refuerzos.TendenciasInferior.First();
            Tendencia_Refuerzos.TendenciasSuperior.Add(cFunctionsProgram.CrearTendenciaDefault(1, Tendencia_Refuerzos)); Tendencia_Refuerzos.TSupeSelect = Tendencia_Refuerzos.TendenciasSuperior.First();
            AsignarCambioAlturayCambioAnchoObjetos();
            CrearTramos();
            CrearElementos();
            if (CantApoyos > 0)
            {
                CrearCoordenadasPerfilLongitudinalReales();
                CrearCoordenadasPerfilLongitudinalAutoCAD();
                ModificarGridsCoordenadasReales();
                CrearEnvolvente();
            }
        }

        public void Cambio_Nombre(string Nombre_)
        {
            Nombre = Prefijo + Nombre_;
            Tendencia_Refuerzos.NombreNervioOrigen = Nombre;
        }

        public void ModificarGridsCoordenadasReales(float BubbleSize = 0.25f)
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
                    Hs.Add(ObjetoActual.Seccion.H);
                    Bs.Add(ObjetoActual.Seccion.B);
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
                        cTramo Tramo = new cTramo("Tramo " + Contador, Objetos, this);
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
                            cTramo Tramo = new cTramo("Tramo " + Contador, Objetos, this);
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
            Lista_Elementos.ForEach(x => { x.Indice = Contador; Contador++; } ) ;
        }


        #region Coordenadas Perfil Longitudinal AutoCAD
        public void CrearCoordenadasPerfilLongitudinalAutoCAD()
        {

            List<float> DiferentesAlturas = Lista_Elementos.FindAll(x => x is cSubTramo).Select(x => x.Seccion.H).Distinct().ToList();
            DiferentesAlturas = DiferentesAlturas.OrderBy(x => x).ToList();
            List<Tuple<float, float>> Altura_Real_AutoCAD = new List<Tuple<float, float>>();
            int Contador = 0; 
            foreach(float H in DiferentesAlturas)
            {
                Tuple<float, float> Tuple = new Tuple<float, float>(H, cVariables.AltoMinimoNervio + (Contador* cVariables.DeltaNivel));
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
                    ElementoActual.HVirtual_AutoCAD= Altura_Real_AutoCAD.Find(x => x.Item1 == ElementoActual.Seccion.H).Item2;
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

                PuntoInicial.Y += DeltaH * FE ;
            }

            CrearCoordenadasLongitudinal_Elemento_AutoCAD(ElementoActual, PuntoInicial);
        }

        private void CrearCoordenadasLongitudinal_Elemento_AutoCAD(IElemento Elemento, PointF PuntoInicial)
        {
            Elemento.Vistas.Perfil_AutoCAD.Reales = new List<PointF>();

            float H = Elemento.HVirtual_AutoCAD ;
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
                        Estacion.Calculos.Envolvente = new cEnvolvente(Estacion.Lista_Solicitaciones,Estacion.Calculos);
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
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales= new List<PointF>();
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales= new List<PointF>();
                    float CoordenaXMenor = SubtramoAux.Vistas.Perfil_Original.Reales.Min(X => X.X);
                    SubtramoAux.Estaciones.ForEach(Estacion =>
                    {
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.M3[0]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.M3[1]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.V2[0]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Envolvente.V2[1]));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, -Estacion.Calculos.Solicitacion_Asingado_Momentos.SolicitacionesSuperior.Area_Momento));
                        SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales.Add(new PointF(CoordenaXMenor + Estacion.CoordX, Estacion.Calculos.Solicitacion_Asingado_Momentos.SolicitacionesInferior.Area_Momento));
                    });
                }
            });
        }

        public void CrearCoordenadasDiagramaMomentos_Escaladas_Envolvente(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento =>
           {
               if (Elemento is cSubTramo)
               {
                   cSubTramo SubtramoAux = (cSubTramo)Elemento;
                   SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI);
                   SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI);
                   SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI);
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
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales, out float EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales, out float EscalaMayorenY1, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI);
                    SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Y0_Escalado = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntoConEscalasDependientes(PuntosTodosObjetos, new PointF(0, 0), HeigthDraw, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI);
                }
            });
        }
        #endregion Calculos


        public void CrearCoordenadasLongitudinal_Elementos_Escalados_Original(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento => Elemento.Vistas.Perfil_Original.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosEnEjeY(PuntosTodosObjetos, Elemento.Vistas.Perfil_Original.Reales, WidthWindow, HeigthWindow, out EscalaMayorenX, Zoom, Dx, Dy, XI)) ;
            Grids.ForEach(x => x.CrearPuntosPlantaEscaladaEtabs(PuntosTodosObjetos, WidthWindow, HeigthWindow, Dx, Dy, Zoom, true, XI));
        }

        public void CrearCoordenadasLongitudinal_Elementos_Escalados_AutoCAD(List<PointF> PuntosTodosObjetos, float HeigthDraw, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Lista_Elementos.ForEach(Elemento => Elemento.Vistas.Perfil_AutoCAD.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, Elemento.Vistas.Perfil_AutoCAD.Reales, out EscalaMayorenY, HeigthWindow, EscalaMayorenX, Zoom, Dx, Dy, XI));
            Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI));
            Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.CrearCoordenadasEscaladas(PuntosTodosObjetos, EscalaMayorenX, HeigthWindow, Dx, Dy, Zoom, XI));
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

        public void ChangeSelect()
        {
            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(z => z.Line.Select = Select));
        }

        #endregion Metodos Mouse

        #region Metodos Paint

        public void PaintNombreElementosEnumerados_MouseMove(Graphics e, float HeigthWindow, float WidthWindow)
        {
            if (ElementoEnumerado_MouseMove)
            {
                Font Font1 = new Font("Calibri", 9, FontStyle.Bold);
                PointF PointString = new PointF(WidthWindow / 2 - e.MeasureString(Nombre, Font1).Width / 2, HeigthWindow / 2 - Font1.Height / 2);
                e.DrawString(Nombre.ToString(), Font1, Brushes.Black, PointString);
            }
        }

        public void Paint_Planta_ElementosEnumerados(Graphics e)
        {
            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(z => z.Line.PaintPlantaEscaladaEtabsLine(e)));
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

        public void Paint_Longitudinal_DrawMomentos(Graphics e, float Zoom, float HeightForm)
        {

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
                    float YInicial = Momento_Negativos_Escalados[0].Y; float YFinal = Momento_Negativos_Escalados[0].Y;
                    SolidBrush Brush_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(227, 88, 88)));
                    SolidBrush Brush_Negativo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(38, 86, 158)));
                    Pen PenBlack = new Pen(Brushes.Black, 1.5f); PenBlack.LineJoin = LineJoin.Round;
                    e.FillPolygon(Brush_Positivos, Momento_Positivos_Escalados.ToArray());
                    e.FillPolygon(Brush_Negativo, Momento_Negativos_Escalados.ToArray());

                    e.DrawLines(PenBlack, Momento_Positivos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Momento_Negativos_Escalados.ToArray());
                }
            });


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

            GraficarRectaApoyos(e, HeightForm);

            if (PuntoInMouseMomentos_Escalado_Real != null && PuntoInMouseMomentos_Escalado_Real.Length != 0)
            {
                string Text = $"(X= {Math.Round(PuntoInMouseMomentos_Escalado_Real[1].X, 2)}m , M= {Math.Round(PuntoInMouseMomentos_Escalado_Real[1].Y, 2)} Ton-m)";
                SizeF MeasureString = e.MeasureString(Text, Font1);
                float BordeRectangulo = 3f; float XString = PuntoInMouseMomentos_Escalado_Real[0].X + MeasureString.Width / 4; float YString = PuntoInMouseMomentos_Escalado_Real[0].Y;
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
        public void Paint_Longitudinal_DrawAreasMomentos(Graphics e, float Zoom, float HeightForm)
        {
            cSubTramo Subtramo1 = (cSubTramo)Lista_Elementos.First(x => x is cSubTramo);
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
                    SolidBrush Brush_Positivos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(227, 88, 88)));
                    SolidBrush Brush_Negativo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(38, 86, 158)));
                    Pen PenBlack = new Pen(Brushes.Black, 1.5f); PenBlack.LineJoin = LineJoin.Round;
                    e.FillPolygon(Brush_Positivos, Areas_Momento_Positivos_Escalados.ToArray());
                    e.FillPolygon(Brush_Negativo, Areas_Momento_Negativos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Areas_Momento_Positivos_Escalados.ToArray());
                    e.DrawLines(PenBlack, Areas_Momento_Negativos_Escalados.ToArray());
                }
            });


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

            GraficarRectaApoyos(e, HeightForm);

            if (PuntoInMouseAreasMomentos_Escalado_Real != null && PuntoInMouseAreasMomentos_Escalado_Real.Length != 0)
            {
                string Text = $"(X= {Math.Round(PuntoInMouseAreasMomentos_Escalado_Real[1].X, 2)}, A= {Math.Round(PuntoInMouseAreasMomentos_Escalado_Real[1].Y, 2)}cm²)";
                SizeF MeasureString = e.MeasureString(Text, Font1);
                float BordeRectangulo = 3f; float XString = PuntoInMouseAreasMomentos_Escalado_Real[0].X + MeasureString.Width / 4; float YString = PuntoInMouseAreasMomentos_Escalado_Real[0].Y;
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
        public void Paint_Longitudinal_Elementos_Escalados_AutoCAD(Graphics e, float Zoom, float HeightForm)
        {
            
            Pen Pen_Borde_Subtramo= new Pen(Color.Black,1);
            //Pen_Borde_Subtramo.DashStyle = DashStyle.Dot;
            Pen Pen_Borde_Apoyo = new Pen(Color.Black,1);
            //Pen_Borde_Apoyo.DashStyle = DashStyle.Dash;
            Brush relleno = new SolidBrush(Color.FromArgb(200,200,200));
            Brush rellenoApoyo = new SolidBrush(Color.FromArgb(150,150,150));

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
            float Delta = 5f;

            foreach (IElemento Elemento in Lista_Elementos)
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    int Indice = 0;
                    foreach (PointF Punto in SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Escaladas)
                    {
                        PointF[] PointsPaths_Positivo = new PointF[] {new PointF( Punto.X-Delta,Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y -Delta),
                                           new PointF(Punto.X-Delta, Punto.Y - Delta) };
                        GraphicsPath Path_Po = new GraphicsPath();
                        Path_Po.AddPolygon(PointsPaths_Positivo);
                        if (Path_Po.IsVisible(Location))
                        {
                            PuntoInMouseMomentos_Escalado_Real = new PointF[] { Punto, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales[Indice] };
                            return;
                        }
                        else
                        {
                            PuntoInMouseMomentos_Escalado_Real = new PointF[] { };
                        }
                        Indice++;
                    }
                    Indice = 0;
                    foreach (PointF Punto in SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Escaladas)
                    {
                        PointF[] PointsPaths_Negativo = new PointF[] {new PointF( Punto.X-Delta,Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y -Delta),
                                           new PointF(Punto.X-Delta, Punto.Y - Delta) };
                        GraphicsPath Path_Ne = new GraphicsPath();

                        Path_Ne.AddPolygon(PointsPaths_Negativo);

                        if (Path_Ne.IsVisible(Location))
                        {
                            PuntoInMouseMomentos_Escalado_Real = new PointF[] { Punto, SubtramoAux.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales[Indice] };

                            return;
                        }
                        else
                        {
                            PuntoInMouseMomentos_Escalado_Real = new PointF[] { };
                        }
                        Indice++;
                    }
                }
            };
        }

        private PointF[] PuntoInMouseAreasMomentos_Escalado_Real = new PointF[] { };
        public void IsPointMouseAreasMomentos(PointF Location)
        {
            float Delta = 5f;

            foreach (IElemento Elemento in Lista_Elementos)
            {
                if (Elemento is cSubTramo)
                {
                    cSubTramo SubtramoAux = (cSubTramo)Elemento;
                    int Indice = 0;
                    foreach (PointF Punto in SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Escaladas)
                    {
                        PointF[] PointsPaths_Positivo = new PointF[] {new PointF( Punto.X-Delta,Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y -Delta),
                                           new PointF(Punto.X-Delta, Punto.Y - Delta) };
                        GraphicsPath Path_Po = new GraphicsPath();
                        Path_Po.AddPolygon(PointsPaths_Positivo);
                        if (Path_Po.IsVisible(Location))
                        {
                            PuntoInMouseAreasMomentos_Escalado_Real = new PointF[] { Punto, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales[Indice] };
                            return;
                        }
                        else
                        {
                            PuntoInMouseAreasMomentos_Escalado_Real = new PointF[] { };
                        }
                        Indice++;
                    }
                    Indice = 0;
                    foreach (PointF Punto in SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Escaladas)
                    {
                        PointF[] PointsPaths_Negativo = new PointF[] {new PointF( Punto.X-Delta,Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y+Delta),
                                           new PointF(Punto.X + Delta, Punto.Y -Delta),
                                           new PointF(Punto.X-Delta, Punto.Y - Delta) };
                        GraphicsPath Path_Ne = new GraphicsPath();

                        Path_Ne.AddPolygon(PointsPaths_Negativo);

                        if (Path_Ne.IsVisible(Location))
                        {
                            PuntoInMouseAreasMomentos_Escalado_Real = new PointF[] { Punto, SubtramoAux.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales[Indice] };

                            return;
                        }
                        else
                        {
                            PuntoInMouseAreasMomentos_Escalado_Real = new PointF[] { };
                        }
                        Indice++;
                    }
                }
            };
        }


        #endregion Metodos Paint
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