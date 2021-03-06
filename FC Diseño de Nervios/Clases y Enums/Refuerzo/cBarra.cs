﻿using FC_BFunctionsAutoCAD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public bool CotaParaAutoCADIzquierda { get; set; } = false;
        public bool CotaParaAutoCADDerecha{ get; set; } = false;
        private eNoBarra noBarra;
        public eNoBarra NoBarra {

            get { return noBarra; }
            set {
                if (noBarra != value)
                {
                    noBarra = value;
                    traslapo = cDiccionarios.FindLdBarra(NoBarra, TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x is cSubTramo).Seccion.Material.fc, true);
                    AreaTotalBarra(); CalcularPeso();
                    tendenciaOrigen.AsignarNivelABarras();
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBarra BarraFind = UbicacionRefuerzo == eUbicacionRefuerzo.Inferior
                            ? y.Tendencia_Refuerzos.TInfeSelect.Barras.Find(B => B.ID == id)
                            : y.Tendencia_Refuerzos.TSupeSelect.Barras.Find(B => B.ID == id);
                        if (BarraFind != null)
                            BarraFind.NoBarra = noBarra;
                    });

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
                    AreaTotalBarra();CalcularPeso();
                    tendenciaOrigen.AsignarNivelABarras();
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBarra BarraFind = UbicacionRefuerzo == eUbicacionRefuerzo.Inferior
                            ? y.Tendencia_Refuerzos.TInfeSelect.Barras.Find(B => B.ID == id)
                            : y.Tendencia_Refuerzos.TSupeSelect.Barras.Find(B => B.ID == id);
                        if (BarraFind != null)
                            BarraFind.CantBarra = cantBarra;
                    });

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
        public float Peso { get; set; }
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
                if (xi != value && AplicarCambiosXFiXI(xf - value) && value >= LimiteIzquierdo)
                {
                    xi = value;
                    longitudRecta = (float)Math.Round(xf - xi,cVariables.CifrasDeciLongBarra);
                    TendenciaOrigen.AsignarNivelABarras();

                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBarra BarraFind = UbicacionRefuerzo == eUbicacionRefuerzo.Inferior
                            ? y.Tendencia_Refuerzos.TInfeSelect.Barras.Find(B => B.ID == id)
                            : y.Tendencia_Refuerzos.TSupeSelect.Barras.Find(B => B.ID == id);
                        if (BarraFind != null)
                            BarraFind.XI = xi;
                    });
                }
            }
        }
        private float xf;
        public float XF
        {
            get { return xf; }
            set
            {

                if (xf != value && AplicarCambiosXFiXI(value - xi) && (float)Math.Round(value, cVariables.CifrasDeciLongBarra) <= LimiteDerecho)
                {
                    xf = value;
                    longitudRecta = (float)Math.Round(xf - xi, cVariables.CifrasDeciLongBarra);
                    TendenciaOrigen.AsignarNivelABarras();

                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBarra BarraFind = UbicacionRefuerzo == eUbicacionRefuerzo.Inferior
                            ? y.Tendencia_Refuerzos.TInfeSelect.Barras.Find(B => B.ID == id)
                            : y.Tendencia_Refuerzos.TSupeSelect.Barras.Find(B => B.ID == id);
                        if (BarraFind != null)
                            BarraFind.XF = xf;
                    });
                }
            }
        }

        private bool traslapoIzquieda;
        public bool TraslapoIzquierda
        {
            get { return traslapoIzquieda; }
            set {
                if (traslapoIzquieda != value)
                {
                    traslapoIzquieda = value;
                }
            }
        }

        private bool traslapoDerecha;
        public bool TraslpaoDerecha
        {
            get { return traslapoDerecha; }
            set
            {
                if (traslapoDerecha != value)
                {
                    traslapoDerecha = value;
                  
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

        public float LGanchoIzquierda { get; set; }
        public float LGanchoDerecha{ get; set; }

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
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignadoRefuerzoLongitudinal();
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBarra BarraFind = UbicacionRefuerzo == eUbicacionRefuerzo.Inferior
                            ? y.Tendencia_Refuerzos.TInfeSelect.Barras.Find(B => B.ID == id)
                            : y.Tendencia_Refuerzos.TSupeSelect.Barras.Find(B => B.ID == id);
                        if (BarraFind != null)
                            BarraFind.GanchoIzquierda = ganchoIzquierdo;
                    });
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
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignadoRefuerzoLongitudinal();
                    tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBarra BarraFind = UbicacionRefuerzo == eUbicacionRefuerzo.Inferior
                            ? y.Tendencia_Refuerzos.TInfeSelect.Barras.Find(B => B.ID == id)
                            : y.Tendencia_Refuerzos.TSupeSelect.Barras.Find(B => B.ID == id);
                        if (BarraFind != null)
                            BarraFind.GanchoDerecha = ganchoDerecha;
                    });
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
                return tendenciaOrigen;
            }
            set {
                if (tendenciaOrigen != value)
                {
                    tendenciaOrigen = value;
                }
            
            }
        
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
            traslapo = cDiccionarios.FindLdBarra(NoBarra, TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x is cSubTramo).Seccion.Material.fc, true);
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

            float LG90 = cDiccionarios.G90[noBarra];
            float LG180 = cDiccionarios.G180[noBarra].Item1;
            float DG180 = cDiccionarios.G180[noBarra].Item2;

            switch (ubicacionRefuerzo)
            {
                case eUbicacionRefuerzo.Inferior:
                    switch (ganchoIzquierdo)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Insert(0, new PointF(xi, y + LG90));
                            break;
                        case eTipoGancho.G180:
                            C_Barra.Reales.Insert(0, new PointF(xi + LG180 - DG180, y + DG180));
                            C_Barra.Reales.Insert(1, new PointF(xi, y + DG180));
                            break;
                    }
                    switch (ganchoDerecha)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Add(new PointF(xf, y + LG90));
                            break;
                        case eTipoGancho.G180:
                            C_Barra.Reales.Add(new PointF(xf, y + DG180));
                            C_Barra.Reales.Add(new PointF(xf - LG180 + DG180, y + DG180));
                            break;
                    }
                    break;
                default:
                    switch (ganchoIzquierdo)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Insert(0, new PointF(xi, y - LG90));
                            break;
                        case eTipoGancho.G180:
                            C_Barra.Reales.Insert(0, new PointF(xi + LG180 - DG180, y - DG180));
                            C_Barra.Reales.Insert(1, new PointF(xi, y - DG180));
                            break;
                    }
                    switch (ganchoDerecha)
                    {
                        case eTipoGancho.G90:
                            C_Barra.Reales.Add(new PointF(xf, y - LG90));
                            break;
                        case eTipoGancho.G180:
                            C_Barra.Reales.Add(new PointF(xf, y - DG180));
                            C_Barra.Reales.Add(new PointF(xf - LG180 + DG180, y - DG180));
                            break;
                    }
                    break;
            }

            Longitud = (float)Math.Round(cFunctionsProgram.Long(C_Barra.Reales), cVariables.CifrasDeciLongBarra);

            LimiteDerecho = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Last().X - cVariables.RExtremoIzquierdo;
            LimiteDerecho = (float)Math.Round(LimiteDerecho, cVariables.CifrasDeciLongBarra);
            LimiteIzquierdo = cVariables.RExtremoIzquierdo;

            if (F_Base.Proyecto.RedondearBarra)
            {
                if (!(Math.Round(longitud % cVariables.DeltaAlargamitoBarras, cVariables.CifrasDeciLongBarra) == 0))//Si es difierente de multiplos de 0.05
                {
                    float Residuo = cFunctionsProgram.DevolverResiduoRedondeado(longitud, cVariables.DeltaAlargamitoBarras, cVariables.CifrasDeciLongBarra);
                    PointF PuntoI = C_Barra.Reales.First(); PointF PuntoD = C_Barra.Reales.Last();
                    //Hay que Redondear
                    if (ganchoDerecha != eTipoGancho.None && ganchoIzquierdo != eTipoGancho.None)//No es Recta la Barra
                    {
                        if (ubicacionRefuerzo== eUbicacionRefuerzo.Inferior) //Inferior
                        {
                            if (ganchoDerecha == eTipoGancho.G180)
                            {
                                PuntoD.X -= Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.Last())] = PuntoD;
                            }
                            else if (ganchoDerecha == eTipoGancho.G90)
                            {
                                PuntoD.Y += Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.Last())] = PuntoD;
                            }
                            if (ganchoIzquierdo == eTipoGancho.G180)
                            {
                                PuntoI.X += Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.First())] = PuntoI;
                            }
                            else if (ganchoIzquierdo == eTipoGancho.G90)
                            {
                                PuntoI.Y+= Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.First())] = PuntoI;
                            }
                        }
                        else //Superior
                        {
                            if (ganchoDerecha == eTipoGancho.G180)
                            {
                                PuntoD.X -= Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.Last())] = PuntoD;

                            }
                            else if (ganchoDerecha == eTipoGancho.G90)
                            {
                                PuntoD.Y -= Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.Last())] = PuntoD;
                            }
                            if (ganchoIzquierdo == eTipoGancho.G180)
                            {
                                PuntoI.X += Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.First())] = PuntoI;
                            }
                            else if (ganchoIzquierdo == eTipoGancho.G90)
                            {
                                PuntoI.Y -= Residuo / 2f;
                                C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.First())] = PuntoI;
                            }
                        }
                    }
                    else if(ganchoDerecha==eTipoGancho.None && ganchoIzquierdo!=eTipoGancho.None)
                    {
                        PuntoD.X += Residuo;
                        C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.Last())] = PuntoD;
                    }else if (ganchoDerecha != eTipoGancho.None && ganchoIzquierdo == eTipoGancho.None)
                    {
                        PuntoI.X -= Residuo;
                        C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.First())] = PuntoI;
                    }else if (ganchoDerecha == eTipoGancho.None && ganchoIzquierdo == eTipoGancho.None)
                    {
                        PuntoI.X -= Residuo/2f;
                        C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.First())] = PuntoI;
                        PuntoD.X += Residuo/2f;
                        C_Barra.Reales[C_Barra.Reales.IndexOf(C_Barra.Reales.Last())] = PuntoD;
                    }
                }
         
            }
            CalcularGanchos();
            Longitud = (float)Math.Round(cFunctionsProgram.Long(C_Barra.Reales), cVariables.CifrasDeciLongBarra);
            CalcularPeso();

        }

        private void CalcularGanchos()
        {
            if (ganchoIzquierdo == eTipoGancho.G90)
                LGanchoIzquierda = C_Barra.Reales.Max(x => x.Y) - C_Barra.Reales.Min(x => x.Y);
            if(ganchoDerecha== eTipoGancho.G90)
                LGanchoDerecha = C_Barra.Reales.Max(x => x.Y) - C_Barra.Reales.Min(x => x.Y);
            if (ganchoIzquierdo == eTipoGancho.G180)
                LGanchoIzquierda = cFunctionsProgram.Long(PuntosGancho180(C_Barra.Reales, eLadoDeZona.Izquierda));
            if (ganchoDerecha == eTipoGancho.G180)
                LGanchoDerecha=cFunctionsProgram.Long(PuntosGancho180(C_Barra.Reales,eLadoDeZona.Derecha));
            if (ganchoIzquierdo == eTipoGancho.None)
                LGanchoIzquierda = 0f;
            if (ganchoDerecha == eTipoGancho.None)
                LGanchoDerecha = 0f;
        }
        private void CalcularPeso()
        {
            Peso = cantBarra * cDiccionarios.PesoBarras[noBarra] * longitud;
        }

        public bool EstacionEnBarra(cEstacion Estacion,cSubTramo Subtramo)
        {
            if (AreaTotal != 0)
            {
                float xiL = xi; float xfL = xf; 
                if (Subtramo.Indice == 0)
                {
                    xiL -= cVariables.RExtremoIzquierdo;
                }
                else if (Subtramo.Indice == TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Last().Indice)
                {
                    xfL += cVariables.RExtremoDerecho;
                    xfL = (float)Math.Ceiling(xfL * 100f) / 100f;
                }
                float CoordenaXMenor = Subtramo.Vistas.Perfil_Original.Reales.Min(X => X.X); float A = (float)Math.Ceiling(xfL * 100f) / 100f;
                float XE = (float)Math.Round(CoordenaXMenor + Estacion.CoordX, cVariables.CifrasDeciLongBarra);
                return Math.Floor(xiL * 100f) / 100f <= CoordenaXMenor + Estacion.CoordX && XE <= xfL;
            }
            else
            {
                return false;
            }
        }
        public float AporteAceroAEstacion(cEstacion Estacion, cSubTramo Subtramo)
        {
            float XE = (float)Math.Round(Subtramo.Vistas.Perfil_Original.Reales.Min(X => X.X) + Estacion.CoordX, cVariables.CifrasDeciLongBarra);
            float Ld = traslapo;
            float xiL = xi; float xfL = xf; 
            if (Subtramo.Indice == 0)
            {
                xiL =(float)Math.Floor((xiL - cVariables.RExtremoIzquierdo) *100f)/100f;
            }
            else if (Subtramo.Indice == TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Last().Indice)
            {
                xfL =(float) Math.Ceiling((xfL + cVariables.RExtremoDerecho) *100f)/100f;
            }

            if (xiL <= XE && XE <= xiL + Ld)
            {
                if (!traslapoIzquieda && ganchoIzquierdo == eTipoGancho.None)
                {
                    return cFunctionsProgram.InterpolacionY(xiL, 0f, xiL + Ld, AreaTotal, XE);
                }
                else
                {
                    return AreaTotal;
                }
            }

            else if (xfL - Ld <= XE && XE <= xfL)
            {
                if (!traslapoDerecha && ganchoDerecha == eTipoGancho.None)
                {
                    return cFunctionsProgram.InterpolacionY(xfL - Ld, AreaTotal, xfL, 0f, XE);
                }
                else
                {
                    return AreaTotal;
                }
            }
            else if(xiL <= XE && XE<= xfL)
            {
                return AreaTotal;
            }
            else
            {
                return 0f;
            }

        }

        public float AporteMomentoAEstacion(cEstacion Estacion, cSubTramo SubTramo)
        {
            float B = SubTramo.Seccion.B; float H = SubTramo.Seccion.H; float fc = SubTramo.Seccion.Material.fc;
            float d1 = SubTramo.TramoOrigen.NervioOrigen.r1 + cDiccionarios.DiametrosBarras[noBarra]/2; float d2 = SubTramo.TramoOrigen.NervioOrigen.r2 + cDiccionarios.DiametrosBarras[noBarra] / 2;
            float fy = SubTramo.Seccion.Material.fy;
            return ubicacionRefuerzo == eUbicacionRefuerzo.Inferior
                ? B_FC_DiseñoVigas.DiseñoYRevisonVigasRectangulares.Revision(B, H, d2, d1, fc, fy, AporteAceroAEstacion(Estacion,SubTramo), 0f)[0] * cConversiones.Momento__kgf_cm_to_Ton_m
                : B_FC_DiseñoVigas.DiseñoYRevisonVigasRectangulares.Revision(B, H, d1, d2, fc, fy, AporteAceroAEstacion(Estacion, SubTramo), 0f)[0] * cConversiones.Momento__kgf_cm_to_Ton_m;

        }


        private bool AplicarCambiosXFiXI(float Long){


            float LongGanchos = cDiccionarios.LDGancho(NoBarra, ganchoDerecha) + cDiccionarios.LDGancho(NoBarra, ganchoIzquierdo);
            float LongiMaximaConGancho = tendenciaOrigen.MaximaLongitud - LongGanchos;
            bool return1 = (Math.Round(Long + LongGanchos, 2) >= Math.Round(tendenciaOrigen.MinimaLongitud, 2)) && (Math.Round(Long, 2) <= Math.Round(LongiMaximaConGancho, 2));
            return return1;

        }

        private float YInicial()
        {
            cNervio Nervio = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen;
            List<IElemento> ElementosContenedores = new List<IElemento>(); List<int> Enteros = new List<int>();
            PointF Punto1 = new PointF(xi,0f); PointF Punto2 = new PointF(xf,0f);
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
            C_Barra.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C_Barra.Reales, out float SY, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
            C_F_Derecha.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C_F_Derecha.Reales, out float SY1, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
            C_F_Izquierda.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C_F_Izquierda.Reales, out float SY2, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
        }


        #region Metodos Paint
        public void Paint(Graphics e, float Zoom, float HeigthForm)
        {
            float Espesor = 3f;
            Pen Pen; SolidBrush Brush_Form = new SolidBrush(Color.FromArgb(81, 126, 255)); Pen Pen_Form_NoSelect = new Pen(Color.FromArgb(0, 0, 0), 1); Pen Pen_Form_Select = new Pen(Color.FromArgb(0, 0, 0), 3);
            
            
            Pen PenSinSelect = new Pen(cDiccionarios.ColorBarra[NoBarra], Espesor); Pen PenFormIzq, PenFormDer;
            Pen PenSelect = new Pen(Color.FromArgb(232, 36, 13), Espesor); Pen PenSombra = new Pen(Color.FromArgb(100, 100, 100), Espesor);
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

            if (tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.BloquearNervio)
                Pen = cNervio.PenBloquearNervio;
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

            if (F_Base.Proyecto.EtiquetasDeBarras)
            {
                float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
                Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
                string TextBarra = $"{cantBarra}{cFunctionsProgram.ConvertireNoBarraToString(noBarra)} L= {string.Format("{0:0.00}",longitud)}";
                SolidBrush Brush_String = new SolidBrush(Color.FromArgb(0, 0, 0));
                SizeF SText = e.MeasureString(TextBarra, Font1);
                float XminBa = C_Barra.Escaladas.Min(x => x.X); float XmaxBa = C_Barra.Escaladas.Max(x => x.X); float YminB = C_F_Izquierda.Escaladas.Max(x => x.Y);
                PointF PointString = new PointF(XminBa + (XmaxBa - XminBa) / 2f - SText.Width / 2f, YminB - SText.Height / 2 - Espesor);
                e.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.DrawString(TextBarra, Font1, Brush_String, PointString);
            }
            if (F_Base.Proyecto.AcotamientoInteligente && C_Barra.IsSelectArrastre)
            {
                Pen PenCota = new Pen(Brushes.Gray, 2); PenCota.DashStyle = DashStyle.Dot;

                if (C_F_Derecha.IsSelect)
                {
                    IElemento Elemento = ObtenerElementoContenedorEscalado(C_F_Derecha.Escaladas.First());
                    CrearCotasInteligentes(e, Elemento, Zoom, PenCota, C_F_Derecha.Escaladas.First(), C_F_Derecha.Reales.First(), Espesor, true, true, true);
                }
                if (C_F_Izquierda.IsSelect)
                {
                    IElemento Elemento = ObtenerElementoContenedorEscalado(C_F_Izquierda.Escaladas.First());
                    CrearCotasInteligentes(e, Elemento, Zoom, PenCota, C_F_Izquierda.Escaladas.First(), C_F_Izquierda.Reales.First(), Espesor, true, true, true);
                }
                if (C_F_Central.IsSelect)
                {
                    IElemento Elemento = ObtenerElementoContenedorEscalado(C_F_Derecha.Escaladas.First());
                    CrearCotasInteligentes(e, Elemento, Zoom, PenCota, C_F_Derecha.Escaladas.First(), C_F_Derecha.Reales.First(), Espesor, true, false, true);
                    IElemento Elemento2 = ObtenerElementoContenedorEscalado(C_F_Izquierda.Escaladas.First());
                    CrearCotasInteligentes(e, Elemento2, Zoom, PenCota, C_F_Izquierda.Escaladas.First(), C_F_Izquierda.Reales.First(), Espesor, true, true, false);
                }


            }

        }

        private void CrearCotasInteligentes(Graphics e, IElemento Elemento,float Zoom, Pen PenCota, PointF PuntoE, PointF PuntoR,float Espesor,bool Linea1,bool Linea2,bool Linea3)
        {
            if (Elemento != null)
            {
                float AlturaPixeles = 30f;
                float YElementoMinE = Elemento.Vistas.Perfil_AutoCAD.Escaladas.Max(x => x.Y);
                float YElementoMaxE = Elemento.Vistas.Perfil_AutoCAD.Escaladas.Min(x => x.Y);
                float XElementoMinE = Elemento.Vistas.Perfil_AutoCAD.Escaladas.Min(x => x.X); float XElementoMinR = Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X);
                float XElementoMaxE = Elemento.Vistas.Perfil_AutoCAD.Escaladas.Max(x => x.X); float XElementoMaxR = Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X);
                string TextDist; SizeF SText;
                float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
                Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
                SolidBrush Brush_String = new SolidBrush(Color.FromArgb(0, 0, 0));
                PointF PointString;

                float YCota;
                if (ubicacionRefuerzo == eUbicacionRefuerzo.Inferior)
                    YCota = YElementoMaxE - AlturaPixeles / 2;
                else
                    YCota = YElementoMinE + AlturaPixeles / 2;

                if (Linea1)
                    e.DrawLine(PenCota, PuntoE.X, PuntoE.Y, PuntoE.X, YCota);

                if (Linea2)
                {
                    TextDist = string.Format("{0:0.00}", PuntoR.X - XElementoMinR); SText = e.MeasureString(TextDist, Font1);
                    PointString = new PointF(XElementoMinE + (PuntoE.X - XElementoMinE) / 2f - SText.Width / 2f, YCota - SText.Height / 2 - Espesor);
                    e.DrawString(TextDist, Font1, Brush_String, PointString);
                    e.DrawLine(PenCota, XElementoMinE, YCota, PuntoE.X, YCota);
                }

                if (Linea3)
                {
                    TextDist = string.Format("{0:0.00}", XElementoMaxR - PuntoR.X); SText = e.MeasureString(TextDist, Font1);
                    PointString = new PointF(PuntoE.X + (XElementoMaxE - PuntoE.X) / 2f - SText.Width / 2f, YCota - SText.Height / 2 - Espesor);
                    e.DrawString(TextDist, Font1, Brush_String, PointString);
                    e.DrawLine(PenCota, XElementoMaxE, YCota, PuntoE.X, YCota);
                }
            }
        }
        private IElemento ObtenerElementoContenedorEscalado(PointF Punto)
        {
            cNervio Nervio = TendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen;
            IElemento Eleme = null;
            Nervio.Lista_Elementos.ForEach(Elemento =>
            {
                if (cFunctionsProgram.IsPuntoInSeccion(Elemento, Punto, false))
                {
                    Eleme = Elemento;
                }
            });
            return Eleme;
        }
        #endregion

        #region Metodos Mouse

        public void MouseDown(PointF Point)
        {

            if (isMouseOverNoArrastre(Point) && !tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.BloquearNervio)
            {
                C_Barra.IsSelect = true;
                C_Barra.IsSelectArrastre = true;
            }
            else
            {
                C_Barra.IsSelect = false;
                C_Barra.IsSelectArrastre = false;
            }

        }
        public void MouseDownDoubleClick(PointF Point)
        {
            if (isMouseOverNoArrastre(Point) && !tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.BloquearNervio)
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
        public void MouseMove(PointF PointF)
        {
            if (!tendenciaOrigen.Tendencia_Refuerzo_Origen.NervioOrigen.BloquearNervio)
            {
                if (C_F_Izquierda.IsSelect && C_Barra.IsSelect)
                {
                    float DistanciaPixeles = PointF.X - C_F_Izquierda.Escaladas.First().X;
                    float DeltaDistancia = DistanciaPixeles / SX;
                    //DeltaDistancia = cFunctionsProgram.PrecisarNumero(DeltaDistancia, deltaAlargamiento, cVariables.CifrasDeciLongBarra);
                    XI += DeltaDistancia;
                    C_F_Izquierda.Escaladas = new List<PointF>() { PointF };
                }
                if (C_F_Derecha.IsSelect && C_Barra.IsSelect)
                {
                    float DistanciaPixeles = PointF.X - C_F_Derecha.Escaladas.First().X;
                    float DeltaDistancia = DistanciaPixeles / SX;
                    //DeltaDistancia = cFunctionsProgram.PrecisarNumero(DeltaDistancia, deltaAlargamiento, cVariables.CifrasDeciLongBarra);
                    XF += DeltaDistancia;
                    C_F_Derecha.Escaladas = new List<PointF>() { PointF };

                }
                if (C_F_Central.IsSelect && C_Barra.IsSelect)
                {
                    float DistanciaPixeles1 = PointF.X - C_F_Central.Escaladas.First().X;
                    if (xi + DistanciaPixeles1 / SX >= LimiteIzquierdo && xf + DistanciaPixeles1 / SX <= LimiteDerecho)
                    {
                        if ((float)Math.Round((xf + DistanciaPixeles1 / SX) - (xi + DistanciaPixeles1 / SX), cVariables.CifrasDeciLongBarra) == longitudRecta)
                        {
                            XI += DistanciaPixeles1 / SX;
                            XF += DistanciaPixeles1 / SX;
                            TendenciaOrigen.AsignarNivelABarras();
                        }
                    }
                    C_F_Central.Escaladas = new List<PointF>() { PointF };
                }
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
        private bool isMouseOverNoArrastre(PointF PointPerte)
        {
            bool Booleano =IsVisibleCoordenadas(C_Barra,PointPerte);
            if(!Booleano)
                Booleano= IsVisibleCoordenadas(C_F_Derecha, PointPerte);
            if(!Booleano)
                Booleano=IsVisibleCoordenadas(C_F_Izquierda, PointPerte);
            return Booleano;
        }
        private bool IsVisibleCoordenadas(cCoordenadas coordenadas, PointF PointPerte)
        {
            float DistanciaMinima = cVariables.ToleranciaVentanaDiseno;
            for (int i = 0; i < coordenadas.Reales.Count; i++)
            {
                if (i + 1 < coordenadas.Reales.Count)
                {
                    float Distancia = cFunctionsProgram.Dist(coordenadas.Escaladas[i], coordenadas.Escaladas[i + 1], PointPerte);
                    if (Distancia <= DistanciaMinima && coordenadas.Escaladas[i].X <= PointPerte.X && PointPerte.X <= coordenadas.Escaladas[i + 1].X)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        #endregion


        public bool IsVisible(IElemento Elemento)
        {
            float Xmax = Elemento.Vistas.Perfil_AutoCAD.Reales.Max(x => x.X);
            float Xmin = Elemento.Vistas.Perfil_AutoCAD.Reales.Min(x => x.X);
            return Xmin >= xi && Xmax <= xf || Xmin <= xi && Xmax >= xi || Xmin <= xf && Xmax >= xf || Xmin <= xi && Xmax >= xf;
        }



        public void Paint_AutoCAD(float X, float Y)
        {
            string TextBarra = $"{CantBarra}#{NoBarra.ToString().Replace("B", "")} L=";
            float LargoTexto = TextBarra.Length*cVariables.W_LetraAutoCADTextRefuerzo;
            float Xmin = C_Barra.Reales.Min(x => x.X); float Xmax = C_Barra.Reales.Max(x => x.X);
            float Ymax = C_Barra.Reales.Max(x => x.Y);
            float Ymin = C_Barra.Reales.Min(x => x.Y);
            float XString = X + Xmin + (Xmax - Xmin) / 2f - LargoTexto/2f;
            float YString = ubicacionRefuerzo == eUbicacionRefuerzo.Inferior ? Y + Ymin + cVariables.H_CuadroTextoBarra : Y + Ymax + cVariables.H_CuadroTextoBarra;
            double[] P_String = new double[] { XString,YString, 0 };
            FunctionsAutoCAD.AddPolyline2DWithLengthText(B_Operaciones_Matricialesl.Operaciones.Traslacion(C_Barra.Reales, X, Y).ToArray(),
                                                            cVariables.C_Refuerzo, TextBarra, P_String, cVariables.W_LetraAutoCADTextRefuerzo, cVariables.H_TextoBarra,
                                                            cVariables.C_TextRefuerzo, cVariables.Estilo_Texto, 0, LargoTexto, JustifyText.Center);
        }



        public override string ToString()
        {
            return $"{CantBarra}#{NoBarra.ToString().Replace("B","")} L={Longitud}";
        }
        private List<PointF> PuntosGancho180(List<PointF> Puntos,eLadoDeZona Lado)
        {
            List<PointF> Puntos180 = new List<PointF>();
            if(Lado== eLadoDeZona.Izquierda)
            {
                for(int i = 0; i <=2; i++)
                {
                    Puntos180.Add(Puntos[i]);
                }
            }
            else
            {
                for (int i = Puntos.FindLastIndex(x=>x==Puntos.Last()); i <= Puntos.FindLastIndex(x => x == Puntos.Last())-2; i++)
                {
                    Puntos180.Add(Puntos[i]);
                }
            }
            return Puntos180;

        }
        public cBarra Clone()
        {
            var temp = new cBarra(ID, tendenciaOrigen, NoBarra, UbicacionRefuerzo, CantBarra, XI, XF)
            {
                AreaTotal = AreaTotal,
                SubNivel = SubNivel,
                nivel = nivel,
                traslapoDerecha = traslapoDerecha,
                traslapoIzquieda = traslapoIzquieda,
                Peso = Peso,
                cantBarra = cantBarra,
                LimiteDerecho = LimiteDerecho,
                LimiteIzquierdo = LimiteIzquierdo,
                ganchoDerecha = ganchoDerecha,
                ganchoIzquierdo = ganchoIzquierdo,
            };
            return temp;
        }

    }
}