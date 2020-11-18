using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo
{
    [Serializable]
    public class cBloqueEstribos
    {
        private float SX;
        static Pen PenBordeSeleccionado = new Pen(Brushes.Gray) { DashStyle = DashStyle.Dash };
        static Pen PenEstribo = new Pen(Color.FromArgb(93, 93, 93));


        public cBloqueEstribos(int ID, eNoBarra noBarra, int cantidad, float separacion, int noRamas, float x, eLadoDeZona direccionEstribo, cTendencia_Estribo Tendencia_Estribo_Origen)
        {
            this.ID = ID;
            this.noBarra = noBarra;
            this.Tendencia_Estribo_Origen = Tendencia_Estribo_Origen;
            this.cantidad = cantidad;
            this.separacion = separacion;
            this.noRamas = noRamas;
            this.direccionEstribo = direccionEstribo;
            xi = x;
            xf = x + LongitudZonaEstribos;
            CrearCoordenadasReales();
        }


        private float LimiteDerecho { get; set; }
        private float LimiteIzquierdo { get; set; }
        public int ID { get; set; }
        public float PesoTransversal { get; set; }


        public cTendencia_Estribo Tendencia_Estribo_Origen { get; set; }
        public cCoordenadas Recuadro_ModoEdicion { get; set; } = new cCoordenadas();
        public cCoordenadas Recuadro_MoverBloqueEstribos { get; set; } = new cCoordenadas();
        public cCoordenadas Recuadro_CambiarDireccionBloqueEstribos { get; set; } = new cCoordenadas();
        public List<cCoordenadas> CoordendasEstribos { get; set; } = new List<cCoordenadas>();


        private eLadoDeZona direccionEstribo;

        public eLadoDeZona DireccionEstribo
        {
            get
            {
                return DireccionEstribo;
            }
            set
            {
                if (direccionEstribo != value)
                {
                    direccionEstribo = value;
                    CrearCoordenadasReales();
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
                if (noBarra != value)
                    noBarra = value;
            }
        }

        private int noRamas;
        public int NoRamas
        {
            get { return noRamas; }
            set
            {
                if (noRamas != value)
                {
                    noRamas = value;
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
                if (separacion != value)
                {
                    separacion = value;
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
            private set
            {

            }
        }

        private float xi;
        public float XI
        {
            get
            {
                return xi;

            }
            private set
            {
                if (xi != value && value>= LimiteIzquierdo && (float)Math.Round(value+LongitudZonaEstribos, cVariables.CifrasDeciLongBarra) <= LimiteDerecho)
                {
                    xi = value;
                    CrearCoordenadasReales();
                }
            }
        }

        private float xf;

        public float XF
        {
            get
            {
                return xf;

            }
            private set
            {
                if (xf != value)
                {
                    xf = value;
                }
            }
        }
        public float LongitudZonaEstribos { get { return separacion * (cantidad - 1); } private set { } }


        private void CrearCoordenadasReales()
        {


            float Yi = YInicial();
            float DeltaHEstribo = 0.2f;
            float W_Recuadro_MoverEstribos = 0.08f;
            float DeltaEstriboBorde = 0.1f;
            xf= xi + LongitudZonaEstribos;


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
            CoordendasEstribos.Clear();
            for (int i = 0; i < cantidad; i++)
            {

                CoordendasEstribos.Add(new cCoordenadas()
                {
                    Reales = new List<PointF>()
                    {
                        new PointF(xi+deltaS,Yi-DeltaHEstribo),
                        new PointF(xi+deltaS,Yi+DeltaHEstribo)
                    }
                });
                deltaS += separacion;
            }



            LimiteDerecho = Tendencia_Estribo_Origen.Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Last().Vistas.Perfil_AutoCAD.Reales.Last().X - cVariables.d_CaraApoyo;
            LimiteDerecho = (float)Math.Round(LimiteDerecho, cVariables.CifrasDeciLongBarra);
            LimiteIzquierdo = cVariables.d_CaraApoyo;

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
            CoordendasEstribos.ForEach(C => { C.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, C.Reales, out float SY2, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f); });

        }

        public void Paint(Graphics e, List<PointF> PuntosTodosObjetos, float SX, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            CrearCoordenadasEscaladas(PuntosTodosObjetos, SX, HeigthWindow, Dx, Dy, Zoom, XI);
            this.SX = SX;
            Pen PenBordeSeleccionado = new Pen(Brushes.DarkRed) { DashStyle = DashStyle.Dash };
            Pen PenRecuadroMoverEstribos = Pens.Black;
            SolidBrush BrushRecuadroMoverEstribos = new SolidBrush(Color.FromArgb(22, 179, 221));
            SolidBrush BrushRecuadroMoverEstribosSelect = new SolidBrush(Color.FromArgb(22, 89, 220)); //OJO -+-> Cambiar propiedades a Estaticas

            SolidBrush BrushMoverEstribos = Recuadro_MoverBloqueEstribos.IsSelect ? BrushRecuadroMoverEstribosSelect : BrushRecuadroMoverEstribos;



            if (Recuadro_ModoEdicion.IsSelect)
            {
                e.DrawPolygon(PenBordeSeleccionado, Recuadro_ModoEdicion.Escaladas.ToArray());
                e.DrawPolygon(PenRecuadroMoverEstribos, Recuadro_MoverBloqueEstribos.Escaladas.ToArray());
                e.FillPolygon(BrushMoverEstribos, Recuadro_MoverBloqueEstribos.Escaladas.ToArray());


                e.DrawPolygon(PenRecuadroMoverEstribos, Recuadro_CambiarDireccionBloqueEstribos.Escaladas.ToArray());
                e.FillPolygon(BrushRecuadroMoverEstribos, Recuadro_CambiarDireccionBloqueEstribos.Escaladas.ToArray());
        
            }

            CoordendasEstribos.ForEach(CE => {

                e.DrawLine(PenEstribo, CE.Escaladas.First(), CE.Escaladas.Last());
            
            });

        }


        public void MouseDown(PointF Point)
        {
            GraphicsPath pathModoedicion = new GraphicsPath();
            GraphicsPath pathMoverEstribos= new GraphicsPath();
            GraphicsPath pathCambiarDireccionEstribos = new GraphicsPath();
            pathModoedicion.AddPolygon(Recuadro_ModoEdicion.Escaladas.ToArray());
            pathMoverEstribos.AddPolygon(Recuadro_MoverBloqueEstribos.Escaladas.ToArray());
            pathCambiarDireccionEstribos.AddPolygon(Recuadro_CambiarDireccionBloqueEstribos.Escaladas.ToArray());

            Recuadro_ModoEdicion.IsSelect = pathModoedicion.IsVisible(Point) || pathMoverEstribos.IsVisible(Point) || pathCambiarDireccionEstribos.IsVisible(Point);

            Recuadro_MoverBloqueEstribos.IsSelect = pathMoverEstribos.IsVisible(Point);


            if (pathCambiarDireccionEstribos.IsVisible(Point))
            {
                DireccionEstribo = direccionEstribo == eLadoDeZona.Derecha ? eLadoDeZona.Izquierda : eLadoDeZona.Derecha;
            }

        }
        public void MouseMove(PointF point)
        {
            float W_Recuadro_MoverEstribos = 0.08f; //Cambiar a estaticas
            float DeltaEstriboBorde = 0.1f;

            if (Recuadro_MoverBloqueEstribos.IsSelect)
            {
                float DistanciaPixeles = point.X - Recuadro_MoverBloqueEstribos.Escaladas.First().X;
                float DeltaDistancia = DistanciaPixeles / SX;
                if (direccionEstribo == eLadoDeZona.Derecha)
                {
                    XI += DeltaDistancia + W_Recuadro_MoverEstribos / 2f - DeltaEstriboBorde;
                    Recuadro_MoverBloqueEstribos.Escaladas[0] = point;
                }
                else
                {
                    XI += DeltaDistancia - W_Recuadro_MoverEstribos / 2f + DeltaEstriboBorde;
                    Recuadro_MoverBloqueEstribos.Escaladas[1] = point;
                }
            }
       
            //DeltaDistancia = cFunctionsProgram.PrecisarNumero(DeltaDistancia, deltaAlargamiento, cVariables.CifrasDeciLongBarra);
        }






        private bool isMouseOverNoArrastre(PointF PointPerte)
        {
            float DistanciaMinima = cVariables.ToleranciaVentanaDiseno;
            for (int i = 0; i < Recuadro_ModoEdicion.Reales.Count; i++)
            {
                if (i + 1 < Recuadro_ModoEdicion.Reales.Count)
                {
                    float Distancia = cFunctionsProgram.Dist(Recuadro_ModoEdicion.Escaladas[i], Recuadro_ModoEdicion.Escaladas[i + 1], PointPerte);
                    if (Distancia <= DistanciaMinima && Recuadro_ModoEdicion.Escaladas[i].X <= PointPerte.X && PointPerte.X <= Recuadro_ModoEdicion.Escaladas[i + 1].X)
                    {
                        return true;
                    }
                }

            }
            return false;
        }



    }
}
