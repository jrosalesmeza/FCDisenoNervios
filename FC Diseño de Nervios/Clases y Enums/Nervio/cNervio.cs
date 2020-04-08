using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cNervio
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Prefijo { get; set; } = "N-";
        public int CantApoyos { get; set; } = 0;
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
        public List<cBarra> ListaBarras { get; set; }
        public eCambioenAltura CambioenAltura { get; set; }
        public eCambioenAncho CambioenAncho { get; set; }

        public cNervio(int ID, string Prefijo, List<cObjeto> Lista_Objetos, eDireccion Direccion, List<cGrid> Grids)
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
            AsignarCambioAlturayCambioAncho();
            CrearTramos();
            CrearElementos();
            if (CantApoyos > 0)
            {
                CrearCoordenadasPerfilLongitudinalReales();
            }
        }

        public void Cambio_Nombre(string Nombre_)
        {
            Nombre = Prefijo + Nombre_;
        }

        private void AsignarCambioAlturayCambioAncho()
        {

            for (int i = 0; i < Lista_Objetos.Count; i++)
            {
                cObjeto ObjetoActual = Lista_Objetos[i];
                cObjeto ObjetoDespues = null; float DeltaH = 0; float DeltaB = 0;
                if (i + 1 < Lista_Objetos.Count)
                {
                    ObjetoDespues = Lista_Objetos[i + 1];
                }

                if (ObjetoDespues != null && ObjetoActual.Soporte == eSoporte.Vano && ObjetoDespues.Soporte == eSoporte.Vano)
                {
                    DeltaB = ObjetoActual.Line.Seccion.B - ObjetoDespues.Line.Seccion.B;
                    DeltaH = ObjetoActual.Line.Seccion.H - ObjetoDespues.Line.Seccion.H;
                }

                if (DeltaB != 0)
                {
                    Bool_CambioAncho = true;
                }
                if (DeltaH != 0)
                {
                    Bool_CambioAltura = true;
                }
            }

            CambioenAltura = Bool_CambioAltura ? eCambioenAltura.Inferior : eCambioenAltura.Ninguno;
            CambioenAncho = Bool_CambioAncho ? eCambioenAncho.Inferior : eCambioenAncho.Ninguno; // Cambios Inferiores Predeterminados
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
                    cApoyo Apoyo = new cApoyo($"Apoyo {CountApoyo}", cFunctionsProgram.DeepClone(Objeto.Line.Seccion));
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
        }

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
                AsignarAlturaVitual(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior);
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
                CrearCoordenadasLongitudinal(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior, 1);
            }
        }


        private void AsignarAlturaVitual(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior)
        {

            if (ElementoAnterior == null && ElementoPosterior != null) //Primer Elemento
            {
                if (ElementoActual is cApoyo)
                {
                    ElementoActual.HVirtual = ElementoPosterior.Seccion.H;

                }
                else
                {
                    ElementoActual.HVirtual = ElementoActual.Seccion.H;
                }
            }
            else if (ElementoAnterior != null && ElementoPosterior != null) //Elemento del Medio
            {

                if (ElementoActual is cApoyo)
                {

                    float DeltaH = ElementoPosterior.Seccion.H - ElementoAnterior.Seccion.H;

                    if (DeltaH < 0)
                    {
                        ElementoActual.HVirtual = ElementoAnterior.Seccion.H;
                    }
                    else
                    {
                        ElementoActual.HVirtual = ElementoPosterior.Seccion.H;
                    }

                }
                else
                {
                    ElementoActual.HVirtual = ElementoActual.Seccion.H;
                }

            }
            else if (ElementoAnterior != null && ElementoPosterior == null)  //Ultimo Elemento
            {

                if (ElementoActual is cApoyo)
                {
                    ElementoActual.HVirtual = ElementoAnterior.Seccion.H;
                }
                else
                {
                    ElementoActual.HVirtual = ElementoActual.Seccion.H;
                }


            }
        }

        private void CrearCoordenadasLongitudinal(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior, float FE)
        {
            PointF PuntoInicial;
            if (ElementoAnterior == null && ElementoPosterior != null) //Primer Elemento
            {
                PuntoInicial = new PointF(0, 0);
            }
            else //(ElementoAnterior != null && ElementoPosterior != null) //Elemento del Medio y Ultimo
            {
                float DeltaH = ElementoActual.HVirtual-ElementoAnterior.HVirtual ;
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
                }else if (CambioenAltura== eCambioenAltura.Superior | CambioenAltura== eCambioenAltura.Ninguno) {
                    DeltaH = 0;
               
                }else if(CambioenAltura == eCambioenAltura.Central)
                {
                    if (DeltaH > 0)
                    {
                        DeltaH = -Math.Abs(DeltaH)/2;
                    }
                    else
                    {
                        DeltaH = Math.Abs(DeltaH)/2;
                    }
                }

                PuntoInicial.Y += DeltaH*FE*cConversiones.Dimension_cm_to_m;
            }






            CrearCoordenadasLongitudinal_Elemento_Reales(ElementoActual, PuntoInicial);
        }

        private void CrearCoordenadasLongitudinal_Elemento_Reales(IElemento Elemento, PointF PuntoInicial)
        {
            Elemento.Vistas.Perfil_Original.Reales = new List<PointF>();

            float H = Elemento.HVirtual* cConversiones.Dimension_cm_to_m;
            float B = Elemento.Seccion.B* cConversiones.Dimension_cm_to_m;
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

        public void CrearCoordenadasLongitudinal_Elementos_Escalados_Original(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom)
        {
            foreach (IElemento Elemento in Lista_Elementos)
            {
                Elemento.Vistas.Perfil_Original.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosEnEjeY(PuntosTodosObjetos, Elemento.Vistas.Perfil_Original.Reales, WidthWindow, HeigthWindow, Zoom, Dx, Dy);
            }

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
            bool Select=false;
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

        public void Paint_Longitudinal_Elementos_Escalados_Original(Graphics e, float Zoom)
        {

            Pen Pen_SinSeleccionar = new Pen(Color.Black, 2);
            Pen Pen__Seleccionado = new Pen(Color.FromArgb( 0, 3,100),3);
            Pen Pen_Definitivo;
            SolidBrush Brush_Selecciondo_Apoyo = new SolidBrush(Color.FromArgb(160, Color.FromArgb(59, 57, 57)));
            SolidBrush Brush_SinSeleccionar_Apoyo = new SolidBrush(Color.FromArgb(85, 85, 85));
            SolidBrush Brush_SinSeleccionado_Subtramos = new SolidBrush(Color.FromArgb(187, 211, 238));
            SolidBrush Brush_Seleccionado_Subtramos = new SolidBrush(Color.FromArgb(160, Color.FromArgb(73, 115, 163)));


            SolidBrush Brush_Definitivo;


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
                    string Texto = $"({Elemento.Seccion.B}x{Elemento.Seccion.H}) L={string.Format("{0:0.00}", ElementoTramo.Longitud)}";
                    SizeF MeasureString = e.MeasureString(Texto, Font1);
                    PointF PuntoString = new PointF(XI + Largo / 2 - MeasureString.Width / 2, YI + MeasureString.Height / 2);
                    e.DrawString(Texto, Font1, Brushes.Black, PuntoString);
                }
            });


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