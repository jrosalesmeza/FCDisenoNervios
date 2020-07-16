using FC_BFunctionsAutoCAD;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cZonaEstribos
    {

        public float Xmin { get; set; }
        public float Xmax { get; set; }
        public eZonas Zona { get; set; }
        public eLadoDeZona LadoDeZona { get; set; }
        public List<cEstribo> Estribos { get; } = new List<cEstribo>();

        public cCoordenadas CoordenadasPuntoString = new cCoordenadas();
        public cCoordenadas CoordenadasCuadro= new cCoordenadas();

        public bool UnionEstribos = false;

        private float separacion;
        public float Separacion
        {
            get { return separacion; }
            set
            {
                separacion = value;
                Area_S = NoBarra != eNoBarra.BNone && separacion != 0 ? cDiccionarios.AceroBarras[NoBarra] / (separacion * cConversiones.Dimension_m_to_cm) : 0f;
            }
        }
        public int Cantidad { get; private set; }
        public eNoBarra NoBarra { get; set; }

        private float area_s;
        public float Area_S {

            get { return area_s; }
            set {
                if (area_s != value)
                {
                    area_s = value;
                }
            
            }
        
        }
        public cLadoDeEstribos LadoDeEstriboOrigen { get; set; }

        public void CrearEstribo(cSubTramo SubTramoOrigen,float CoordX) 
        {
            cEstribo Estribo = new cEstribo(SubTramoOrigen, NoBarra, CoordX);
            Estribos.Add(Estribo);
            Cantidad = Estribos.Count;
            AgregarExtremos();
            CrearCoordenadasReales();
            LadoDeEstriboOrigen.CalcularPesoAcero();
        }
        public void EliminarEstribos(bool Todos = false)
        {
            if (Todos)
            {
                NoBarra = eNoBarra.BNone;
                Separacion = 0f;
                Cantidad = 0;
                Estribos.Clear();
                CrearCoordenadasReales();
            }
        }

        public cZonaEstribos(eZonas Zona, eLadoDeZona LadoDeZona, cLadoDeEstribos LadoDeEstriboOrigen)
        {
            this.LadoDeZona = LadoDeZona;
            this.Zona = Zona;
            this.LadoDeEstriboOrigen = LadoDeEstriboOrigen;
            CrearCoordenadasReales();
        }


        public void AgregarExtremos()
        {
            Xmin = float.NegativeInfinity; Xmax = float.NegativeInfinity;
            if (LadoDeZona == eLadoDeZona.Izquierda)
            {
                if (NoBarra != eNoBarra.BNone)
                {
                    Xmin = Estribos.First().CoordX - cVariables.d_CaraApoyo;
                    Xmax = Estribos.Last().CoordX;
                }
            }
            else if (LadoDeZona == eLadoDeZona.Derecha)
            {

                if (NoBarra != eNoBarra.BNone)
                {
                    Xmax = Estribos.First().CoordX + cVariables.d_CaraApoyo;
                    Xmin = Estribos.Last().CoordX;
                }
            }
        }


        public void CrearCoordenadasReales()
        {
            CoordenadasCuadro.Reales = new List<PointF>();
            CoordenadasPuntoString.Reales = new List<PointF>();
            if (Estribos.Count > 0)
            {
                float Longitud = Estribos.Last().CoordX - Estribos.First().CoordX;
                float DeltaH = cVariables.DeltaH_Estribos_AutoCAD / 2f;
                float Xi = Estribos.First().CoordX;
                float Yi = Estribos.First().Coordenadas.Reales.Min(x => x.Y); float Yf = Estribos.First().Coordenadas.Reales.Max(x => x.Y);
                PointF Punto1C = new PointF(Xi, Yi - DeltaH); PointF Punto2C = new PointF(Xi, Yf + DeltaH);
                PointF Punto3C = new PointF(Xi+Longitud, Yf + DeltaH); PointF Punto4C = new PointF(Xi + Longitud, Yi - DeltaH);
                CoordenadasCuadro.Reales.Add(Punto1C); CoordenadasCuadro.Reales.Add(Punto2C); CoordenadasCuadro.Reales.Add(Punto3C); CoordenadasCuadro.Reales.Add(Punto4C);
                float PuntoString = Yf + DeltaH;
                if (Zona == eZonas.Zona2 && LadoDeEstriboOrigen.Zona1.NoBarra != eNoBarra.BNone)
                    PuntoString = Yi-DeltaH;
                PointF PString = new PointF(Xi + Longitud / 2f, PuntoString);
                CoordenadasPuntoString.Reales.Add(PString);            
            }
        }
        public void CrearCoordenadasEscaladas(List<PointF> PuntosTodosObjetos, float SX, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Estribos.ForEach(x => x.CrearCoordenadasEscaladas(PuntosTodosObjetos, SX, HeigthWindow, Dx, Dy, Zoom, XI));
            CoordenadasCuadro.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, CoordenadasCuadro.Reales, out float SY, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
            CoordenadasPuntoString.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, CoordenadasPuntoString.Reales, out SY, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
        }

        public void PaintEstribos(Graphics e, float Zoom)
        {
            float TamanoLetra = Zoom > 0 ? 9 * Zoom : 1;
            Font Font1 = new Font("Calibri", TamanoLetra, FontStyle.Bold);
            Pen PenEstribo = new Pen(Color.FromArgb(93, 93, 93));
            SolidBrush Brush_Text = new SolidBrush(Color.Black);
            Estribos.ForEach(Estribo => {

                e.DrawLines(PenEstribo, Estribo.Coordenadas.Escaladas.ToArray());

            });
            if (Estribos.Count > 0)
            {
                string Text = ToString();
                SizeF MessureText = e.MeasureString(Text, Font1);
                PointF PointString = new PointF(CoordenadasPuntoString.Escaladas.First().X - MessureText.Width / 2f, CoordenadasPuntoString.Escaladas.First().Y - MessureText.Height/2);
                e.DrawString(Text, Font1, Brush_Text, PointString);
            }
            //e.DrawPath(PenEstribo, Path);
        }

        public void PaintAutoCAD(float X,float Y)
        {
            if(NoBarra!= eNoBarra.BNone)
            {
                string Text = $"{Cantidad}{cFunctionsProgram.ConvertireNoBarraToString(NoBarra)}/{Math.Round(separacion * cConversiones.Dimension_m_to_cm, 2)}";
                float LargoTexto = Text.Length * cVariables.W_LetraAutoCADEstribos;
                FunctionsAutoCAD.AddText(Text, B_Operaciones_Matricialesl.Operaciones.Traslacion(CoordenadasPuntoString.Reales.First(), X-LargoTexto/2, Y),
                                         cVariables.W_LetraAutoCADTextRefuerzo, cVariables.H_TextoEstribos, cVariables.C_Estribos, cVariables.Estilo_Texto, 0,
                                         Width2: LargoTexto, JustifyText: JustifyText.Center);
            }

        }

        public override string ToString()
        {
            return $"{Cantidad}E{cFunctionsProgram.ConvertireNoBarraToString(NoBarra)}@{Math.Round(Separacion,3)}";
        }


    }

    [Serializable]
    public enum eLadoDeZona
    {
        Izquierda,
        Derecha
    }

    public enum eZonas
    {
        Zona1,
        Zona2
    }

}
