using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cBarra
    {
        private float SX;
        private int id;
        public int ID {
            get { return id; }
            set {
                if (id != value)
                {
                    id = value;
                }
            }
        }

        private eNoBarra noBarra;
        public eNoBarra NoBarra {

            get { return noBarra; }
            set {
                if (noBarra != value)
                {
                    noBarra = value;
                    traslapo = cDiccionarios.FindTraslapo(NoBarra, TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x is cSubTramo).Seccion.Material.fc, true);
                    AreaTotalBarra();
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignado();
                }

            }
        }
        private int cantBarra;
        public int CantBarra
        {
            get { return cantBarra; }
            set
            {
                if (cantBarra != value)
                {
                    cantBarra = value;
                    AreaTotalBarra();
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignado();
                }
            }
        }

        private float longitud;
        public float Longitud
        {
            get { return longitud; }
            set
            {
                if (longitud != value && value != 0)
                {
                    longitud = value;

                }
            }
        }

        private float longitudRecta;
        public float LongitudRecta
        {
            get { return longitudRecta; }

            set
            {
                if (longitudRecta != value)
                {
                    longitudRecta = value;
                }
            }
        }
        private float deltaAlargamiento;
        public float DeltaAlargamiento
        {
            get { return deltaAlargamiento; }
            set
            {
                if (deltaAlargamiento != value && value != 0)
                {
                    deltaAlargamiento = value;
                }
            }
        }
        private float LimiteIzquierdo;
        private float LimiteDerecho;

        private float xi;
        public float XI
        {
            get { return xi; }
            set
            {
                if (xi != value && AplicarCambiosXFiXI(xf - value) && value > LimiteIzquierdo)
                {
                    xi = value;
                    TendenciaOrigen.AsignarNivelABarras();
                    longitudRecta = xf - xi;
                }
            }
        }
        private float xf;
        public float XF
        {
            get { return xf; }
            set
            {

                if (xf != value && AplicarCambiosXFiXI(value - xi) && value < LimiteDerecho)
                {
                    xf = value;
                    TendenciaOrigen.AsignarNivelABarras();
                    longitudRecta = xf - xi;
                }
            }
        }

        private bool traslpaoIzquieda;
        public bool TraslapoIzquierda
        {
            get { return traslpaoIzquieda; }
            set {
                if (traslpaoIzquieda != value)
                {
                    traslpaoIzquieda = value;
                }
            }
        }

        private bool traslpaoDerecha;
        public bool TraslpaoDerecha
        {
            get { return traslpaoDerecha; }
            set
            {
                if (traslpaoDerecha != value)
                {
                    traslpaoDerecha = value;
                }
            }
        }

        private float traslapo;
        public float Traslapo
        {
            get { return traslapo; }
            set
            {
                if (traslapo != value)
                {
                    traslapo = value;
                }
            }

        }


        private eUbicacionRefuerzo ubicacionRefuerzo;
        public eUbicacionRefuerzo UbicacionRefuerzo
        {
            get { return ubicacionRefuerzo; }
            set
            {
                if (ubicacionRefuerzo != value)
                {
                    ubicacionRefuerzo = value;
                }
            }
        }

        private eTipoGancho ganchoIzquierdo;
        public eTipoGancho GanchoIzquierda {

            get { return ganchoIzquierdo; }
            set {
                if (ganchoIzquierdo != value)
                {
                    ganchoIzquierdo = value;
                    CrearCoordenadasReales();
                }
            }
        }

        private eTipoGancho ganchoDerecha;
        public eTipoGancho GanchoDerecha
        {
            get { return ganchoDerecha; }
            set
            {
                if (ganchoDerecha != value)
                {
                    ganchoDerecha = value;
                    CrearCoordenadasReales();
                }
            }
        }

        private int nivel = 1;
        public int Nivel
        {
            get { return nivel; }
            set
            {
                if (nivel != value)
                {
                    nivel = value;

                }
            }
        }

        private int subNivel = 0;
        public int SubNivel {
            get { return subNivel; }
            set { if (subNivel != value) { subNivel = value; } }
        }
        public float AreaTotal { get; set; }

        private cTendencia tendenciaOrigen;
        public cTendencia TendenciaOrigen {
            get
            {
                FindTendeciaOrgien();
                return tendenciaOrigen;
            }
            set {
                if (tendenciaOrigen != value)
                {
                    tendenciaOrigen = value;
                }
            
            }
        
        }
        private void FindTendeciaOrgien()
        {
            //if (UbicacionRefuerzo == eUbicacionRefuerzo.Inferior)
            //{
            //    tendenciaOrigen = tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Tendencia_Refuerzos.TendenciasInferior.Find(x => x.Nombre == tendenciaOrigen.Nombre);
            //}
            //else
            //{
            //    tendenciaOrigen = tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Tendencia_Refuerzos.TendenciasSuperior.Find(x => x.Nombre == tendenciaOrigen.Nombre);

            //}
        }


        public cCoordenadas C_Barra { get; set; } = new cCoordenadas();
        public cCoordenadas C_F_Izquierda { get; set; } = new cCoordenadas();
        public cCoordenadas C_F_Central { get; set; } = new cCoordenadas();
        public cCoordenadas C_F_Derecha { get; set; } = new cCoordenadas();

        public cBarra(int ID, cTendencia TendenciaOrigen, eNoBarra NoBarra, eUbicacionRefuerzo UbicacionRefuerzo, int CantBarra, float xi, float xf)
        {
            this.ID = ID;
            tendenciaOrigen = TendenciaOrigen;
            noBarra = NoBarra;
            this.CantBarra = CantBarra;
            this.xi = xi;
            this.xf = xf;
            longitudRecta = xf - xi;
            DeltaAlargamiento = TendenciaOrigen.DeltaAlargamientoBarras;
            traslapo = cDiccionarios.FindTraslapo(NoBarra, TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x is cSubTramo).Seccion.Material.fc, true);
            this.UbicacionRefuerzo = UbicacionRefuerzo;
            AreaTotalBarra();         
            CrearCoordenadasReales();
        }

        public void CrearCoordenadasReales()
        {
            float y = YInicial();
            C_Barra.Reales = new List<PointF>();
            C_Barra.Reales.Add(new PointF(xi, y)); C_Barra.Reales.Add(new PointF(xf, y));
            C_F_Izquierda.Reales = new List<PointF>(); C_F_Derecha.Reales = new List<PointF>(); float R = 0.04f;
            C_F_Izquierda.Reales.Add(new PointF(xi, y)); C_F_Izquierda.Reales.Add(new PointF(xi + R, y));
            C_F_Derecha.Reales.Add(new PointF(xf, y)); C_F_Derecha.Reales.Add(new PointF(xf + R, y));

            switch (ubicacionRefuerzo)
            {
                case eUbicacionRefuerzo.Inferior:
                    switch (ganchoIzquierdo)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Insert(0, new PointF(xi, y + cDiccionarios.G90[noBarra]));
                            break;
                        case eTipoGancho.G180:
                            float L = cDiccionarios.G180[noBarra].Item1; float D = cDiccionarios.G180[noBarra].Item2;
                            C_Barra.Reales.Insert(0, new PointF(xi + L - D, y + D));
                            C_Barra.Reales.Insert(1, new PointF(xi, y + D));
                            break;
                    }
                    switch (ganchoDerecha)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Add(new PointF(xf, y + cDiccionarios.G90[noBarra]));
                            break;
                        case eTipoGancho.G180:
                            float L = cDiccionarios.G180[noBarra].Item1; float D = cDiccionarios.G180[noBarra].Item2;
                            C_Barra.Reales.Add(new PointF(xf, y + D));
                            C_Barra.Reales.Add(new PointF(xf - L + D, y + D));
                            break;
                    }
                    break;
                default:
                    switch (ganchoIzquierdo)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Insert(0, new PointF(xi, y - cDiccionarios.G90[noBarra]));
                            break;
                        case eTipoGancho.G180:
                            float L = cDiccionarios.G180[noBarra].Item1; float D = cDiccionarios.G180[noBarra].Item2;
                            C_Barra.Reales.Insert(0, new PointF(xi + L - D, y - D));
                            C_Barra.Reales.Insert(1, new PointF(xi, y - D));
                            break;
                    }
                    switch (ganchoDerecha)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Add(new PointF(xf, y - cDiccionarios.G90[noBarra]));
                            break;
                        case eTipoGancho.G180:
                            float L = cDiccionarios.G180[noBarra].Item1; float D = cDiccionarios.G180[noBarra].Item2;
                            C_Barra.Reales.Add(new PointF(xf, y - D));
                            C_Barra.Reales.Add(new PointF(xf - L + D, y - D));
                            break;
                    }
                    break;
            }

            Longitud = (float)Math.Round(cFunctionsProgram.Long(C_Barra.Reales), cVariables.CifrasDeciLongBarra);
            LimiteDerecho = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Last().X - (TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.d1 + cDiccionarios.DiametrosBarras[noBarra]) * cConversiones.Dimension_cm_to_m;
            LimiteIzquierdo = (TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.d1 + cDiccionarios.DiametrosBarras[noBarra]) * cConversiones.Dimension_cm_to_m;
        }


        public bool EstacionEnBarra(cEstacion Estacion,cSubTramo Subtramo)
        {
            float CoordenaXMenor = Subtramo.Vistas.Perfil_Original.Reales.Min(X => X.X);
            return xi <= CoordenaXMenor+Estacion.CoordX && CoordenaXMenor + Estacion.CoordX <= xf;
        }



        private bool AplicarCambiosXFiXI(float Long){
            float LongiMaximaConGancho = TendenciaOrigen.MaximaLongitud - cDiccionarios.LDGancho(NoBarra, ganchoDerecha) - cDiccionarios.LDGancho(NoBarra, ganchoIzquierdo);
            if (Long >= TendenciaOrigen.MinimaLongitud && Long <= LongiMaximaConGancho)
            {
                return true;
            }
            return false;
        }

        private float YInicial()
        {
            cNervio Nervio = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen;
            List<IElemento> ElementosContenedores = new List<IElemento>(); List<int> Enteros = new List<int>();
            PointF Punto1 = new PointF(xi,0); PointF Punto2 = new PointF(xf,0);
            Nervio.Lista_Elementos.ForEach(Elemento =>
            {
                if (cFunctionsProgram.IsPuntoInSeccion(Elemento, Punto1))
                {
                    Enteros.Add(Elemento.Indice);
                }
                if (cFunctionsProgram.IsPuntoInSeccion(Elemento, Punto2))
                {
                    Enteros.Add(Elemento.Indice);
                }
            });
            ElementosContenedores = Nervio.Lista_Elementos.GetRange(Enteros.First(), Enteros.Last()- Enteros.First()+1);

            if (UbicacionRefuerzo == eUbicacionRefuerzo.Inferior)
            {
                return ElementosContenedores.Select(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.Y)).Max() + cVariables.DeltaNivel * nivel + subNivel * cVariables.DeltaSubNivel;
            }
            else
            {
                return ElementosContenedores.Select(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.Y)).Min() - cVariables.DeltaNivel  * nivel - subNivel * cVariables.DeltaSubNivel;
            }

        }

        private void AreaTotalBarra()
        {
            AreaTotal = cantBarra * cDiccionarios.AceroBarras[noBarra];
        }

        public void CrearCoordenadasEscaladas(List<PointF> PuntosTodosObjetos, float SX, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            this.SX = SX;
            C_Barra.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C_Barra.Reales, out float SY, HeigthWindow, SX, Zoom, Dx, Dy, XI);
            C_F_Derecha.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C_F_Derecha.Reales, out float SY1, HeigthWindow, SX, Zoom, Dx, Dy, XI);
            C_F_Izquierda.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C_F_Izquierda.Reales, out float SY2, HeigthWindow, SX, Zoom, Dx, Dy, XI);
            
        }


        #region Metodos Paint
        public void Paint(Graphics e, float Zoom, float HeigthForm)
        {
            float Espesor = 3f;
            Pen Pen; SolidBrush Brush_Form = new SolidBrush(Color.FromArgb(81, 126, 255)); Pen Pen_Form_NoSelect = new Pen(Color.FromArgb(0, 0, 0), 1); Pen Pen_Form_Select = new Pen(Color.FromArgb(0, 0, 0), 3);
            Pen PenSinSelect = new Pen(cDiccionarios.ColorBarra[NoBarra], Espesor); Pen PenFormIzq, PenFormDer;
            Pen PenSelect = new Pen(Color.FromArgb(232, 36, 13),Espesor); Pen PenSombra = new Pen(Color.FromArgb(100, 100, 100), Espesor);
            List<PointF> PuntosSobra = C_Barra.Escaladas.ToList(); for (int i = 0; i < PuntosSobra.Count; i++) { PuntosSobra[i] = new PointF(PuntosSobra[i].X + 2f, PuntosSobra[i].Y + 2f); }
            if (C_Barra.IsSelect | C_Barra.IsSelectArrastre)
            {
                e.DrawLines(PenSombra, PuntosSobra.ToArray());
                Pen = PenSelect;
            }
            else
            {
                Pen = PenSinSelect;
            }
               e.DrawLines(Pen, C_Barra.Escaladas.ToArray());
               
            if (C_Barra.IsSelectArrastre)
            {
                PenFormDer = C_F_Derecha.IsSelect ? Pen_Form_Select : Pen_Form_NoSelect;
                PenFormIzq = C_F_Izquierda.IsSelect ? Pen_Form_Select : Pen_Form_NoSelect;
                e.DrawEllipse(PenFormIzq, cFunctionsProgram.CrearCirculo(C_F_Izquierda.Escaladas));
                e.FillEllipse(Brush_Form, cFunctionsProgram.CrearCirculo(C_F_Izquierda.Escaladas));
                e.DrawEllipse(PenFormDer, cFunctionsProgram.CrearCirculo(C_F_Derecha.Escaladas));
                e.FillEllipse(Brush_Form, cFunctionsProgram.CrearCirculo(C_F_Derecha.Escaladas));
            }

  
        }


        #endregion

        #region Metodos Mouse

        public void MouseDown(PointF Point, bool NoArrastre)
        {
            if (NoArrastre)
            {
                if (isMouseOverNoArrastre(Point))
                {
                    C_Barra.IsSelect = true;
                }
                else
                {
                    C_Barra.IsSelect = false;
                }
            }
            else
            {
                if (isMouseOverNoArrastre(Point))
                {
                    C_Barra.IsSelectArrastre = true;
                
                }
                else
                {
                    C_Barra.IsSelectArrastre = false;
                }
            }
        }
        public void MouseDownDoubleClick(PointF Point)
        {
            if (isMouseOverNoArrastre(Point))
            {
                C_F_Central.IsSelect = true;
                C_F_Derecha.IsSelect = false;
                C_F_Izquierda.IsSelect = false;
                C_F_Central.Escaladas = new List<PointF>() { Point };

            }
            else
            {
                C_F_Central.IsSelect = false;
            }
            
        }

        private bool isMouseOverNoArrastre(PointF PointPerte)
        {
            float DistanciaMinima = cVariables.ToleranciaVentanaDiseno;
            for (int i = 0; i < C_Barra.Reales.Count; i++)
            {
                if(i+1< C_Barra.Reales.Count)
                {
                   float Distancia= cFunctionsProgram.Dist(C_Barra.Escaladas[i], C_Barra.Escaladas[i + 1], PointPerte);
                    if(Distancia<= DistanciaMinima && C_Barra.Escaladas[i].X<= PointPerte.X && PointPerte.X<= C_Barra.Escaladas[i + 1].X)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public void MouseMove(PointF PointF)
        {
            if (C_F_Izquierda.IsSelect)
            {
                float DistanciaPixeles =  PointF.X- C_F_Izquierda.Escaladas.First().X;
                float DeltaDistancia = DistanciaPixeles / SX;
                XI += DeltaDistancia;
            }
            if (C_F_Derecha.IsSelect)
            {
                float DistanciaPixeles = PointF.X - C_F_Derecha.Escaladas.First().X;
                float DeltaDistancia = DistanciaPixeles / SX;
                XF += DeltaDistancia;
            }
            if (C_F_Central.IsSelect)
            {
                float DistanciaPixeles1 = PointF.X - C_F_Central.Escaladas.First().X;
                if (xi+ DistanciaPixeles1 /SX>= LimiteIzquierdo  && xf + DistanciaPixeles1 / SX <= LimiteDerecho)
                {
                    XI += DistanciaPixeles1 / SX;
                    XF += DistanciaPixeles1 / SX;
                }
                C_F_Central.Escaladas = new List<PointF>() { PointF };
            }
        }
        public void MouseDownEsferas(PointF PointPer)
        {
            GraphicsPath graphicsPathIzquierda = new GraphicsPath();
            graphicsPathIzquierda.AddRectangles(new RectangleF[] { cFunctionsProgram.CrearCirculo(C_F_Izquierda.Escaladas) });
            if (graphicsPathIzquierda.IsVisible(PointPer))
            {
                C_F_Izquierda.IsSelect = true;

            }
            else
            {
                C_F_Izquierda.IsSelect = false;
            }

            GraphicsPath graphicsPathDerecha = new GraphicsPath();
            graphicsPathDerecha.AddRectangles(new RectangleF[] { cFunctionsProgram.CrearCirculo(C_F_Derecha.Escaladas) });
            if (graphicsPathDerecha.IsVisible(PointPer))
            {
                C_F_Derecha.IsSelect = true;
            }
            else
            {
                C_F_Derecha.IsSelect = false;
            }
            C_F_Central.IsSelect = false;
        }

        #endregion





        public override string ToString()
        {
            return $"{CantBarra}#{NoBarra.ToString().Replace("B","")} L={Longitud}";
        }







    }
}