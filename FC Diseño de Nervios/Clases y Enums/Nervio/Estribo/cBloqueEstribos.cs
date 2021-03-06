﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo
{
    public delegate void delegateChangeProperty(object sender);
    [Serializable]
    public class cBloqueEstribos
    {

        public event delegateChangeProperty ChangeProperty;

        static Pen PenEstribo = new Pen(Color.FromArgb(93, 93, 93));
        static Pen PenBordeSeleccionado = new Pen(Brushes.DarkRed) { DashStyle = DashStyle.Dash };
        static Pen PenRecuadroMoverEstribos = Pens.Black;
        static SolidBrush BrushRecuadroMoverEstribos = new SolidBrush(Color.FromArgb(22, 179, 221));
        static SolidBrush BrushRecuadroMoverEstribosSelect = new SolidBrush(Color.FromArgb(22, 89, 220));


        private float coordXEstriboEstriboFinal = cVariables.ValueNull;
        public float CoordXEstriboEstriboFinal
        {
            get
            {
                return coordXEstriboEstriboFinal;
            }
            set
            {
                if (coordXEstriboEstriboFinal != value)
                {
                    coordXEstriboEstriboFinal = value;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                }
            }
        }


        private float SX;

        public cBloqueEstribos(int ID, eNoBarra noBarra, int cantidad, float separacion, int noRamas, float x, eLadoDeZona direccionEstribo, cTendencia_Estribo Tendencia_Estribo_Origen)
        {
            this.ID = ID;
            this.noBarra = noBarra;
            this.Tendencia_Estribo_Origen = Tendencia_Estribo_Origen;
            this.cantidad = cantidad;
            this.separacion = separacion;
            this.noRamas = noRamas;
            this.direccionEstribo = direccionEstribo;
            if (direccionEstribo == eLadoDeZona.Derecha)
            {
                xi = x;
                xf = x + LongitudZonaEstribos;
            }
            else
            {
                xf = x;
                xi = x - LongitudZonaEstribos;
            }

            if (direccionEstribo==eLadoDeZona.Izquierda && !CumpleLimitesX(xi, LongitudZonaEstribos))
            {
                xi = LimiteIzquierdo;
            }else if (direccionEstribo == eLadoDeZona.Derecha && !CumpleLimitesX(xf, LongitudZonaEstribos))
            {
                xf = LimiteDerecho;
            }

            CrearCoordenadasReales();
            
        }
        public float LimiteDerecho { get; set; }
        public float LimiteIzquierdo { get; set; }
        public int ID { get; set; }
        public float PesoTransversal { get; private set; }
        public cTendencia_Estribo Tendencia_Estribo_Origen { get; set; }
        public cCoordenadas Recuadro_ModoEdicion { get; set; } = new cCoordenadas();
        public cCoordenadas Recuadro_MoverBloqueEstribos { get; set; } = new cCoordenadas();
        public cCoordenadas Recuadro_CambiarDireccionBloqueEstribos { get; set; } = new cCoordenadas();
        public List<cEstribo1> ListaEstribos { get; private set; } = new List<cEstribo1>();

        private eLadoDeZona direccionEstribo;
        public eLadoDeZona DireccionEstribo
        {
            get
            {
                return direccionEstribo;
            }
            set
            {
                if (direccionEstribo != value)
                {
                    direccionEstribo = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.DireccionEstribo = direccionEstribo;
                    });
                }
            }
        }

        private eNoBarra noBarra;
        public eNoBarra NoBarra
        {
            get
            {
                return noBarra;
            }
            set
            {
                if (noBarra != value && value!= eNoBarra.BNone)
                {
                    noBarra = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.NoBarra = noBarra;
                    });
                }
            }
        }

        private int noRamas;
        public int NoRamas
        {
            get { return noRamas; }
            set
            {
                if (noRamas != value && value>0f)
                {
                    noRamas = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.NoRamas = noRamas;
                    });
                }
            }
        }

        private float separacion;
        public float Separacion
        {
            get
            {
                return separacion;
            }
            set
            {
                if (separacion != value && value>0f && 
                    value <= cVariables.Separacion_MaximaEstribos && (direccionEstribo == eLadoDeZona.Derecha && CumpleLimitesX(xi, CalcularLongitudZonaEstribos(value, cantidad))
                    || (direccionEstribo == eLadoDeZona.Izquierda && CumpleLimitesX(xf, CalcularLongitudZonaEstribos(value, cantidad)))))
                {
                    separacion = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.Separacion = separacion;
                    });
                }

            }
        }
        private int cantidad;
        public int Cantidad
        {
            get
            {
                return cantidad;
            }
            set
            {
                if (cantidad != value && value > 0 && (direccionEstribo== eLadoDeZona.Derecha && CumpleLimitesX(xi, CalcularLongitudZonaEstribos(separacion, value))
                    || (direccionEstribo == eLadoDeZona.Izquierda && CumpleLimitesX(xf, CalcularLongitudZonaEstribos(separacion, value)))))
                {
                    cantidad = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.Cantidad = cantidad;
                    });
                }

            }
        }

        private float xi;
        public float XI
        {
            get
            {
                if (coordXEstriboEstriboFinal != cVariables.ValueNull && direccionEstribo == eLadoDeZona.Izquierda)
                    return coordXEstriboEstriboFinal;
                return xi;

            }
            set
            {
                if (xi != value && CumpleLimitesX(value, LongitudZonaEstribos))
                {
                    xi = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.XI = xi;
                    });
                }
            }
        }

        private float xf;

        public float XF
        {
            get
            {
                if (coordXEstriboEstriboFinal != cVariables.ValueNull&& direccionEstribo==eLadoDeZona.Derecha)
                    return coordXEstriboEstriboFinal;
                return xf;

            }
            set
            {
                if (xf != value && CumpleLimitesX(value,LongitudZonaEstribos))
                {
                
                    xf = value;
                    coordXEstriboEstriboFinal = cVariables.ValueNull;
                    CrearCoordenadasReales();
                    ChangeProperty?.Invoke(this);
                    Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.SimilitudNervioCompleto.NerviosSimilares.ForEach(y =>
                    {
                        cBloqueEstribos BloqueEstribosFind = y.Tendencia_Refuerzos.TEstriboSelect.BloqueEstribos.Find(B => B.ID == ID);
                        if (BloqueEstribosFind != null)
                            BloqueEstribosFind.XF = xf;
                    });
                }
            }
        }
        public float LongitudZonaEstribos { get { return CalcularLongitudZonaEstribos(separacion,cantidad); } private set { } }

        private float CalcularLongitudZonaEstribos(float separacion, int cantidad)
        {
            return separacion * (cantidad - 1);
        }


        private bool CumpleLimitesX(float value, float longitudZonaEstribos)
        {
            return direccionEstribo == eLadoDeZona.Derecha
                ? value >= LimiteIzquierdo && (float)Math.Round(value + longitudZonaEstribos, cVariables.CifrasDeciLongBarra) <= LimiteDerecho
                : value <= LimiteDerecho && (float)Math.Round(value - longitudZonaEstribos, cVariables.CifrasDeciLongBarra) >= LimiteIzquierdo;
        }
        private void CrearCoordenadasReales()
        {
            float Yi = YInicial();
            float DeltaHEstribo = 0.2f;
            float W_Recuadro_MoverEstribos = 0.08f;
            float DeltaEstriboBorde = 0.1f;

            if (direccionEstribo == eLadoDeZona.Derecha)
            {
                xf = xi + LongitudZonaEstribos;
            }
            else
            {
                xi = xf - LongitudZonaEstribos;
            }
            Recuadro_ModoEdicion.Reales = new List<PointF>()
            {
                new PointF(xi-DeltaEstriboBorde, Yi-DeltaHEstribo-DeltaEstriboBorde),
                new PointF(xi+LongitudZonaEstribos+DeltaEstriboBorde, Yi-DeltaHEstribo-DeltaEstriboBorde),
                new PointF(xi+LongitudZonaEstribos+DeltaEstriboBorde, Yi+DeltaHEstribo+DeltaEstriboBorde),
                new PointF(xi-DeltaEstriboBorde, Yi+DeltaHEstribo+DeltaEstriboBorde)
            };


            if (direccionEstribo == eLadoDeZona.Derecha)
            {
                Recuadro_MoverBloqueEstribos.Reales = new List<PointF>()
                {
                    new PointF(xi-DeltaEstriboBorde-W_Recuadro_MoverEstribos/2f,Yi-DeltaHEstribo/2f),
                    new PointF(xi-DeltaEstriboBorde+W_Recuadro_MoverEstribos/2f,Yi-DeltaHEstribo/2f),
                    new PointF(xi-DeltaEstriboBorde+W_Recuadro_MoverEstribos/2f,Yi+DeltaHEstribo/2f),
                    new PointF(xi-DeltaEstriboBorde-W_Recuadro_MoverEstribos/2f,Yi+DeltaHEstribo/2f)
                };
            }
            else
            {
                Recuadro_MoverBloqueEstribos.Reales = new List<PointF>()
                {
                    new PointF(xf+DeltaEstriboBorde+W_Recuadro_MoverEstribos/2f,Yi-DeltaHEstribo/2f),
                    new PointF(xf+DeltaEstriboBorde-W_Recuadro_MoverEstribos/2f,Yi-DeltaHEstribo/2f),
                    new PointF(xf+DeltaEstriboBorde-W_Recuadro_MoverEstribos/2f,Yi+DeltaHEstribo/2f),
                    new PointF(xf+DeltaEstriboBorde+W_Recuadro_MoverEstribos/2f,Yi+DeltaHEstribo/2f)
                };
            }

            if (direccionEstribo == eLadoDeZona.Derecha)
            {
                Recuadro_CambiarDireccionBloqueEstribos.Reales = new List<PointF>()
                {
                    new PointF(xf+DeltaEstriboBorde-W_Recuadro_MoverEstribos/2f, Yi-DeltaEstriboBorde/1.5f),
                    new PointF(xf+DeltaEstriboBorde+W_Recuadro_MoverEstribos/1.5f,Yi),
                    new PointF(xf+DeltaEstriboBorde-W_Recuadro_MoverEstribos/2f, Yi+DeltaEstriboBorde/1.5f)
                };

            }
            else
            {
                Recuadro_CambiarDireccionBloqueEstribos.Reales = new List<PointF>()
                {
                    new PointF(xi-DeltaEstriboBorde+W_Recuadro_MoverEstribos/2f, Yi-DeltaEstriboBorde/1.5f),
                    new PointF(xi-DeltaEstriboBorde-W_Recuadro_MoverEstribos/1.5f,Yi),
                    new PointF(xi-DeltaEstriboBorde+W_Recuadro_MoverEstribos/2f, Yi+DeltaEstriboBorde/1.5f)
                };
            }


            float deltaS = 0;
            ListaEstribos.Clear();
            for (int i = 0; i < cantidad; i++)
            {
                float x = xi + deltaS;
                if (i + 1 == cantidad && CoordXEstriboEstriboFinal != cVariables.ValueNull && direccionEstribo== eLadoDeZona.Derecha)
                {
                    x = CoordXEstriboEstriboFinal;
                }else if (i== 0 && CoordXEstriboEstriboFinal != cVariables.ValueNull && direccionEstribo == eLadoDeZona.Izquierda)
                {
                    x = CoordXEstriboEstriboFinal;
                }
                IElemento elemento = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.First(y => y.IsVisibleCoordAutoCAD(x));
                cEstribo1 estribo1 = new cEstribo1(elemento, noBarra,noRamas, Tendencia_Estribo_Origen,
                    new cCoordenadas()
                    {
                        Reales = new List<PointF>()
                    {
                        new PointF(x,Yi-DeltaHEstribo),
                        new PointF(x,Yi+DeltaHEstribo)
                    }
                    });
                deltaS += separacion;
                ListaEstribos.Add(estribo1);
            }
            AsignarLimites();
            CalcularPesoRefuerzoTransversal();
        }

        private void AsignarLimites()
        {
            LimiteDerecho = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Last().X - cVariables.d_CaraApoyo;
            LimiteDerecho = (float)Math.Round(LimiteDerecho, cVariables.CifrasDeciLongBarra);
            LimiteIzquierdo = cVariables.d_CaraApoyo;

        }


        private void CalcularPesoRefuerzoTransversal()
        {
            PesoTransversal = 0f;
            ListaEstribos.ForEach(y =>PesoTransversal+= y.Longitud * cDiccionarios.PesoBarras[noBarra]);
            Tendencia_Estribo_Origen.ActualizarRefuerzoTransversal();
        }



        public float AporteRelacion_As_S_aEstacion(cEstacion estacion, List<cBloqueEstribos> bloqueEstribos)
        {
            float As_S = cDiccionarios.AceroBarras[noBarra] * NoRamas / (separacion * cConversiones.Dimension_m_to_cm);
            float x = estacion.CoordX + estacion.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);

            var XCaraAp1 = estacion.SubTramoOrigen.TramoOrigen.Lista_SubTramos.First().Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
            var XCaraAp2 = estacion.SubTramoOrigen.TramoOrigen.Lista_SubTramos.Last().Vistas.Perfil_AutoCAD.Reales.Max(y => y.X);


            if (IsVisible(XI,XF,x) || ((float)Math.Round(Math.Abs(x-XCaraAp1),cVariables.CifrasDeciSepEstribos)<= cVariables.d_CaraApoyo && (float)Math.Round(Math.Abs(XI-XCaraAp1),cVariables.CifrasDeciSepEstribos)<=cVariables.d_CaraApoyo) ||
                ((float)Math.Round(Math.Abs(x - XCaraAp2),cVariables.CifrasDeciSepEstribos) <= cVariables.d_CaraApoyo && (float)Math.Round(Math.Abs(XF - XCaraAp2),cVariables.CifrasDeciSepEstribos) <= cVariables.d_CaraApoyo))
            {
                return As_S;
            }
            else if(cantidad== 1 && IsVisible(XI - Separacion / 2f, XF + Separacion / 2F, x))
            {
                return As_S;
            }
            else
            {
                foreach(cBloqueEstribos bloque in bloqueEstribos)
                {
                    float Xi = XI - Separacion;
                    float Xf = XF + Separacion;

                    if(bloque!= this && bloque.IsVisible(Xi,Xf) && x>= bloque.XF && x<= XI && x<=XF)
                    {
                        return As_S;
                    }
                }

                return 0f;
            }


        }


        private float YInicial()
        {
            cNervio Nervio = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen;
            List<IElemento> ElementosContenedores = new List<IElemento>(); List<int> Enteros = new List<int>();
            PointF Punto1 = new PointF(xi, 0f); PointF Punto2 = new PointF(xf, 0f);

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
            ElementosContenedores = Nervio.Lista_Elementos.GetRange(Enteros.First(), Enteros.Last() - Enteros.First() + 1);

            float ymin = ElementosContenedores.Select(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.Y)).Max();
            float ymax = ElementosContenedores.Select(x => x.Vistas.Perfil_AutoCAD.Reales.Max(y => y.Y)).Min();

            return ymin + (ymax - ymin) / 2f;

        }
        private void CrearCoordenadasEscaladas(List<PointF> PuntosTodosObjetos, float SX, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Recuadro_ModoEdicion.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, Recuadro_ModoEdicion.Reales, out float SY, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
            Recuadro_MoverBloqueEstribos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, Recuadro_MoverBloqueEstribos.Reales, out float SY1, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
            Recuadro_CambiarDireccionBloqueEstribos.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, Recuadro_CambiarDireccionBloqueEstribos.Reales, out SY1, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
            ListaEstribos.ForEach(E => { E.Coordenadas.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, E.Coordenadas.Reales, out float SY2, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f); });

        }

        public void Paint(Graphics e, List<PointF> PuntosTodosObjetos, float SX, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            CrearCoordenadasEscaladas(PuntosTodosObjetos, SX, HeigthWindow, Dx, Dy, Zoom, XI);
            this.SX = SX;
            SolidBrush BrushMoverEstribos = Recuadro_MoverBloqueEstribos.IsSelect ? BrushRecuadroMoverEstribosSelect : BrushRecuadroMoverEstribos;

            if (Recuadro_ModoEdicion.IsSelect)
            {
                e.DrawPolygon(PenBordeSeleccionado, Recuadro_ModoEdicion.Escaladas.ToArray());
               
                e.DrawPolygon(PenRecuadroMoverEstribos, Recuadro_MoverBloqueEstribos.Escaladas.ToArray());
                e.FillPolygon(BrushMoverEstribos, Recuadro_MoverBloqueEstribos.Escaladas.ToArray());

                e.DrawPolygon(PenRecuadroMoverEstribos, Recuadro_CambiarDireccionBloqueEstribos.Escaladas.ToArray());
                e.FillPolygon(BrushRecuadroMoverEstribos, Recuadro_CambiarDireccionBloqueEstribos.Escaladas.ToArray());
            }

            ListaEstribos.ForEach(E =>
            {
                e.DrawLine(PenEstribo, E.Coordenadas.Escaladas.First(), E.Coordenadas.Escaladas.Last());

            });

            float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold); 
            string Text = ToString();
            SizeF MessureText = e.MeasureString(Text, Font1);

            float Xmin = Recuadro_ModoEdicion.Escaladas.Min(y => y.X);
            float Xmax = Recuadro_ModoEdicion.Escaladas.Max(y => y.X);
            float Ymax = Recuadro_ModoEdicion.Escaladas.Min(y => y.Y);

            if (Recuadro_ModoEdicion.IsSelect)
                Ymax -= MessureText.Height/2;
            PointF PointString = new PointF(Xmin + (Xmax - Xmin) / 2f - MessureText.Width/2f, Ymax - MessureText.Height / 2);
            e.DrawString(Text, Font1, Brushes.Black, PointString);
        }


        public void MouseDown(PointF Point)
        {

            GraphicsPath pathModoedicion = new GraphicsPath();
            GraphicsPath pathMoverEstribos = new GraphicsPath();
            GraphicsPath pathCambiarDireccionEstribos = new GraphicsPath();
            pathModoedicion.AddPolygon(Recuadro_ModoEdicion.Escaladas.ToArray());
            pathMoverEstribos.AddPolygon(Recuadro_MoverBloqueEstribos.Escaladas.ToArray());
            pathCambiarDireccionEstribos.AddPolygon(Recuadro_CambiarDireccionBloqueEstribos.Escaladas.ToArray());
            if (!Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.BloquearNervio)
            {
                Recuadro_ModoEdicion.IsSelect = pathModoedicion.IsVisible(Point) || pathMoverEstribos.IsVisible(Point) || pathCambiarDireccionEstribos.IsVisible(Point);
            }
            else
            {
                Recuadro_ModoEdicion.IsSelect = false;
            }
            Recuadro_MoverBloqueEstribos.IsSelect = pathMoverEstribos.IsVisible(Point);

            if (pathCambiarDireccionEstribos.IsVisible(Point))
            {
                DireccionEstribo = direccionEstribo == eLadoDeZona.Derecha ? eLadoDeZona.Izquierda : eLadoDeZona.Derecha;
            }


        }
        public void MouseMove(PointF point)
        {

            if (!Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.BloquearNervio)
            {
                float W_Recuadro_MoverEstribos = cVariables.W_Recuadro_MoverEstribos;
                float DeltaEstriboBorde = cVariables.DeltaEstriboBorde;

                if (Recuadro_MoverBloqueEstribos.IsSelect)
                {
                    float DistanciaPixeles = point.X - Recuadro_MoverBloqueEstribos.Escaladas.First().X;
                    float DeltaDistancia = DistanciaPixeles / SX;
                    if (direccionEstribo == eLadoDeZona.Derecha)
                    {
                        float DistanciaDesplazarXReal = DeltaDistancia + W_Recuadro_MoverEstribos / 2f - DeltaEstriboBorde;
                        XI += DistanciaDesplazarXReal;
                        Recuadro_MoverBloqueEstribos.Escaladas = B_Operaciones_Matricialesl.Operaciones.Traslacion(Recuadro_MoverBloqueEstribos.Escaladas, DistanciaDesplazarXReal * SX, 0);
                        Recuadro_ModoEdicion.Escaladas = B_Operaciones_Matricialesl.Operaciones.Traslacion(Recuadro_ModoEdicion.Escaladas, DistanciaDesplazarXReal * SX, 0);
                        Recuadro_CambiarDireccionBloqueEstribos.Escaladas = B_Operaciones_Matricialesl.Operaciones.Traslacion(Recuadro_CambiarDireccionBloqueEstribos.Escaladas, DistanciaDesplazarXReal * SX, 0);
                    }
                    else
                    {
                        float DistanciaDesplazarXReal = DeltaDistancia - W_Recuadro_MoverEstribos / 2f + DeltaEstriboBorde;
                        XF += DistanciaDesplazarXReal;
                        Recuadro_MoverBloqueEstribos.Escaladas = B_Operaciones_Matricialesl.Operaciones.Traslacion(Recuadro_MoverBloqueEstribos.Escaladas, DistanciaDesplazarXReal * SX, 0);
                        Recuadro_ModoEdicion.Escaladas = B_Operaciones_Matricialesl.Operaciones.Traslacion(Recuadro_ModoEdicion.Escaladas, DistanciaDesplazarXReal * SX, 0);
                        Recuadro_CambiarDireccionBloqueEstribos.Escaladas = B_Operaciones_Matricialesl.Operaciones.Traslacion(Recuadro_CambiarDireccionBloqueEstribos.Escaladas, DistanciaDesplazarXReal * SX, 0);

                    }
                }
            }
        }


        public void PaintAutoCAD(float X, float Y)
        {

            float Ymax = Recuadro_ModoEdicion.Reales.Max(y => y.Y);
            float Ymin = Recuadro_ModoEdicion.Reales.Min(y => y.Y);

            PointF CoordenadasPuntoString = new PointF(XI + (XF - XI) / 2f, Ymin + (Ymax - Ymin) / 2f);

            string Text = $"{Cantidad}{cFunctionsProgram.ConvertireNoBarraToString(NoBarra)}/{Math.Round(separacion * cConversiones.Dimension_m_to_cm, 2)}";
            float LargoTexto = Text.Length * cVariables.W_LetraAutoCADEstribos;
            FC_BFunctionsAutoCAD.FunctionsAutoCAD.AddText(Text, B_Operaciones_Matricialesl.Operaciones.Traslacion(CoordenadasPuntoString, X - LargoTexto / 2, Y),
                                     cVariables.W_LetraAutoCADTextRefuerzo, cVariables.H_TextoEstribos, cVariables.C_Estribos, cVariables.Estilo_Texto, 0,
                                     Width2: LargoTexto, JustifyText: FC_BFunctionsAutoCAD.JustifyText.Center);
        }


        public void MoveraCaraApoyo()
        {
            List<IElemento> elementos = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos;
            List<cBloqueEstribos> bloqueEstribos = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.TEstriboSelect.BloqueEstribos;
            if (direccionEstribo == eLadoDeZona.Derecha)
            {
                IElemento elemento = elementos.Find(y => y.IsVisibleCoordAutoCAD(XI));

                float xi=elemento.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X) + cVariables.d_CaraApoyo;
                float xf = xi + LongitudZonaEstribos;
                if (bloqueEstribos.Count > 0)
                {
                    bloqueEstribos = bloqueEstribos.OrderBy(y => y.XI).ToList();
                    XiFinalCorrerHaciaDerecha(this, bloqueEstribos, ref xi, ref xf);
                }
                XI = xi;
            }
            else
            {
                IElemento elemento = elementos.Find(y => y.IsVisibleCoordAutoCAD(XF));

                float xf = elemento.Vistas.Perfil_AutoCAD.Reales.Max(y => y.X) - cVariables.d_CaraApoyo;
                float xi = xf- LongitudZonaEstribos;
                if (bloqueEstribos.Count > 0)
                {
                    bloqueEstribos = bloqueEstribos.OrderBy(y => y.XI).ToList();
                    XfFinalCorrerHaciaIzquierda(this, bloqueEstribos, ref xi, ref xf);
                }
                XF = xf;


            }
        }

        public void MoverAMitadTramo()
        {
            List<IElemento> elementos = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos;
            IElemento elemento = elementos.Find(y => y.IsVisibleCoordAutoCAD(XI));

            if(elemento is cSubTramo)
            {
                var subtramo = elemento as cSubTramo;
                var Tramo = subtramo.TramoOrigen;
                var xmin=Tramo.Lista_SubTramos.First().Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
                var xmax= Tramo.Lista_SubTramos.First().Vistas.Perfil_AutoCAD.Reales.Max(y => y.X);
                if (direccionEstribo == eLadoDeZona.Derecha)
                {
                    XI = xmin + (xmax - xmin) / 2f - LongitudZonaEstribos / 2f;
                }
                else
                {
                    XF= xmin + (xmax - xmin) / 2f + LongitudZonaEstribos / 2f;
                }
            }


        }

        private void XiFinalCorrerHaciaDerecha(cBloqueEstribos bloque1,List<cBloqueEstribos> bloqueEstribos,ref float xi,ref float xf)
        {
            foreach (var bloque2 in bloqueEstribos)
            {
                if (bloque1 != bloque2)
                {
                    if (bloque2.IsVisible(xi, xf) && bloque2.DireccionEstribo == bloque1.DireccionEstribo)
                    {
                        xi = bloque2.xf+ bloque1.Separacion;
                        xf = bloque1.LongitudZonaEstribos + xi;
                        XiFinalCorrerHaciaDerecha(bloque1, bloqueEstribos, ref xi, ref xf);
                    }
                }
            }
        }

        private void XfFinalCorrerHaciaIzquierda(cBloqueEstribos bloque1, List<cBloqueEstribos> bloqueEstribos, ref float xi, ref float xf)
        {
            foreach (var bloque2 in bloqueEstribos)
            {
                if (bloque1 != bloque2)
                {
                    if (bloque2.IsVisible(xi, xf) && bloque2.DireccionEstribo== bloque1.DireccionEstribo)
                    {
                        xf = bloque2.xi- bloque2.Separacion;
                        xi = xf - bloque1.LongitudZonaEstribos;
                        XiFinalCorrerHaciaDerecha(bloque1, bloqueEstribos, ref xi, ref xf);
                    }
                }
            }
        }


        public bool IsVisible(cBloqueEstribos bloqueEstribo)
        {
            return (bloqueEstribo.XF >= XI && bloqueEstribo.XF <= XF) ||
                   (bloqueEstribo.XI >= XI && bloqueEstribo.XI <= XF);
        }
        public bool IsVisible(float xi, float xf)
        {
            return (xf >= XI && xf <= XF) ||
                   (xi >= XI && xi <= XF);
        }
        public bool IsVisible(float xi, float xf,float X)
        {
            return X >= xi && X <= xf;
        }
        public bool IsVisible(cEstacion estacion)
        {
            float x = estacion.CoordX + estacion.SubTramoOrigen.Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
            var XCaraAp1 = estacion.SubTramoOrigen.TramoOrigen.Lista_SubTramos.First().Vistas.Perfil_AutoCAD.Reales.Min(y => y.X);
            var XCaraAp2 = estacion.SubTramoOrigen.TramoOrigen.Lista_SubTramos.Last().Vistas.Perfil_AutoCAD.Reales.Max(y => y.X);
            return (IsVisible(XI, XF, x) || ((float)Math.Round(Math.Abs(x - XCaraAp1), cVariables.CifrasDeciSepEstribos) <= cVariables.d_CaraApoyo && (float)Math.Round(Math.Abs(XI - XCaraAp1), cVariables.CifrasDeciSepEstribos) <= cVariables.d_CaraApoyo) ||
                ((float)Math.Round(Math.Abs(x - XCaraAp2), cVariables.CifrasDeciSepEstribos) <= cVariables.d_CaraApoyo && (float)Math.Round(Math.Abs(XF - XCaraAp2), cVariables.CifrasDeciSepEstribos) <= cVariables.d_CaraApoyo));
        }

        public override string ToString()
        {
            return $"{Cantidad}E{cFunctionsProgram.ConvertireNoBarraToString(NoBarra)}@{string.Format("{0:0.00}",Math.Round(Separacion, cVariables.CifrasDeciLongBarra))}";
        }

        public cBloqueEstribos Clone()
        {
            float x = direccionEstribo == eLadoDeZona.Derecha ? xi : xf;
            return new cBloqueEstribos(ID, noBarra, cantidad, separacion, noRamas, x, DireccionEstribo, Tendencia_Estribo_Origen);
        }

    }
}
