using System;
using System.Collections.Generic;
using System.Drawing;

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

        public bool Select { get; set; }

        public bool SelectPlantaEnumeracion { get; set; }
        private bool ElementoEnumerado_MouseMove = false;
        public cObjeto ObjetoSelect { get; set; }
        public List<cBarra> ListaBarras { get; set; }
        public eCambioenAltura CambioenAltura { get; set; }
        public eCambioenAncho CambioenAncho { get; set; }

        public cNervio(int ID, string Prefijo, List<cObjeto> Lista_Objetos, eDireccion Direccion,List<cGrid> Grids)
        {
            this.ID = ID;
            if (Prefijo != "")
            {
                this.Prefijo = Prefijo;
            }
            this.Lista_Objetos = Lista_Objetos;
            this.Lista_Objetos.ForEach(x => { if (x.Soporte == eSoporte.Apoyo) { CantApoyos += 1; } });
            this.Direccion = Direccion;
            this.Grids= Grids;
            AsignarCambioAlturayCambioAncho();
            CrearTramos();
            CrearElementos();
        }

        public void Cambio_Nombre(string Nombre_)
        {
            Nombre = Prefijo + Nombre_;
        }

        private void AsignarCambioAlturayCambioAncho()
        {
            bool CambioAltura = false; bool CambioAncho = false;

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
                    CambioAncho = true;
                }
                if (DeltaH != 0)
                {
                    CambioAltura = true;
                }
            }

            CambioenAltura = CambioAltura ? eCambioenAltura.Inferior : eCambioenAltura.Ninguno;
            CambioenAncho = CambioAncho ? eCambioenAncho.Inferior : eCambioenAncho.Ninguno; // Cambios Inferiores Predeterminados
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

        public void CrearCoordenadas()
        {
            for (int i = 0; i < Lista_Elementos.Count; i++)
            {
                IElemento ElementoAnterior = null; IElemento Elemento_Posterior = null;
                if (i - 1 >= 0)
                {
                    ElementoAnterior = Lista_Elementos[i - 1];
                }
                if (i + 1 < Lista_Objetos.Count)
                {
                    Elemento_Posterior = Lista_Elementos[i + 1];
                }
                CrearCoordenadasLongitudinal(ElementoAnterior, Lista_Elementos[i], Elemento_Posterior);
            }
        }

        private void CrearCoordenadasLongitudinal(IElemento ElementoAnterior, IElemento ElementoActual, IElemento ElementoPosterior)
        {
            PointF PuntoInicial = new PointF(0, 0); float HApoyo;
            HApoyo = ElementoActual.Seccion.H;
            if (ElementoActual.Soporte == eSoporte.Apoyo)
            {
                if (ElementoPosterior != null)
                {
                    HApoyo = ElementoPosterior.Seccion.H;
                }
                else
                {
                    HApoyo = ElementoAnterior.Seccion.H;
                }
            }
            if (ElementoAnterior != null && ElementoPosterior != null)
            {
                float H_anterior = ElementoAnterior.Seccion.H;
                float H_Posterior = ElementoPosterior.Seccion.H;
                float DeltaH = Math.Abs(H_anterior - H_Posterior);
                PuntoInicial = ElementoAnterior.Vistas.Perfil_Original.Reales[ElementoAnterior.Vistas.Perfil_Original.Reales.Count - 1];

                if (CambioenAltura == eCambioenAltura.Inferior)
                {
                    if (H_anterior < H_Posterior)
                    {
                        DeltaH = -DeltaH;
                    }
                }
                else if (CambioenAltura == eCambioenAltura.Central)
                {
                    DeltaH = DeltaH / 2;

                    if (H_anterior < H_Posterior)
                    {
                        DeltaH = -DeltaH;
                    }
                }

                PuntoInicial.Y += DeltaH;
            }
            else if (ElementoPosterior == null)
            {
                PuntoInicial = ElementoAnterior.Vistas.Perfil_Original.Reales[ElementoAnterior.Vistas.Perfil_Original.Reales.Count - 1];
                float DeltaH = 0;
                float H_anterior = ElementoAnterior.Seccion.H;
                float H_Actual = ElementoActual.Seccion.H;
                if (ElementoActual.Soporte != eSoporte.Apoyo)
                {
                    DeltaH = Math.Abs(H_anterior - H_Actual);
                }

                if (CambioenAltura == eCambioenAltura.Inferior)
                {
                    if (H_anterior < H_Actual)
                    {
                        DeltaH = -DeltaH;
                    }
                }
                else if (CambioenAltura == eCambioenAltura.Central)
                {
                    DeltaH = DeltaH / 2;

                    if (H_anterior < H_Actual)
                    {
                        DeltaH = -DeltaH;
                    }
                }

                PuntoInicial.Y += DeltaH;
            }

            CrearCoordenadasLongitudinal_Elemento_Reales(ElementoActual, PuntoInicial, HApoyo);
        }

        private void CrearCoordenadasLongitudinal_Elemento_Reales(IElemento Elemento, PointF PuntoInicial, float HApoyo = 0)
        {
            Elemento.Vistas.Perfil_Original.Reales = new List<PointF>();
            float H = Elemento.Seccion.H;
            float B = Elemento.Seccion.B;
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
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X, PuntoInicial.Y + HApoyo));
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y + HApoyo));
                Elemento.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y));
            }
        }

        public void CrearCoordenadasLongitudinal_Elementos_Escalados_Original(List<PointF> PuntosTodosObjetos, float WidthWindow, float HeigthWindow, float Dx, float Dy, float Zoom)
        {
            foreach (IElemento Elemento in Lista_Elementos)
            {
                Elemento.Vistas.Perfil_Original.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntos(PuntosTodosObjetos, Elemento.Vistas.Perfil_Original.Reales, WidthWindow, HeigthWindow, Dx, Dy, Zoom);
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