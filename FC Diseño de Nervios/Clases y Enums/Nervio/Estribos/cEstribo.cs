using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cEstribo
    {
        public cSubTramo SubTramoOrigen { get; set; }
        public cCoordenadas Coordenadas { get; set; } = new cCoordenadas();
        public float Longitud { get; set; }
        public int Ramas { get; }
        public eNoBarra NoBarra { get; set; }
        public float CoordX { get; set; }

        public float B { get; set; }
        public float H { get; set; }
        public float LGancho { get; set; }

        public cEstribo(cSubTramo SubTramoOrigen,eNoBarra NoBarra,float CoordX)
        {
            this.SubTramoOrigen = SubTramoOrigen;
            if (SubTramoOrigen.Seccion.B > 12f)
                Ramas = 2;
            else
                Ramas = 1;
            this.CoordX = CoordX;

            this.NoBarra = NoBarra;
            CalcularLongitud();
            CrearCoordenadasReales();
        }


        private void CrearCoordenadasReales()
        {
            float DeltaH = cVariables.DeltaH_Estribos_AutoCAD;
            Coordenadas.Reales = new List<PointF>();
            float YminTramo = SubTramoOrigen.TramoOrigen.Lista_SubTramos.Select(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.Y)).Max();
            cSubTramo SubTramoMayorYmin = SubTramoOrigen.TramoOrigen.Lista_SubTramos.Find(x => x.Vistas.Perfil_AutoCAD.Reales.Min(y => y.Y) == YminTramo); 
        
            float XC = CoordX; float YC = SubTramoMayorYmin.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y)+(SubTramoMayorYmin.Vistas.Perfil_AutoCAD.Reales.Max(x => x.Y) - SubTramoMayorYmin.Vistas.Perfil_AutoCAD.Reales.Min(x => x.Y)) / 2;
            PointF Punto1 = new PointF(XC,YC+DeltaH);
            PointF Punto2 = new PointF(XC, YC - DeltaH);
            Coordenadas.Reales.Add(Punto1); Coordenadas.Reales.Add(Punto2);
        }

        public void CrearCoordenadasEscaladas(List<PointF> PuntosTodosObjetos, float SX, float HeigthWindow, float Dx, float Dy, float Zoom, float XI)
        {
            Coordenadas.Escaladas = B_EscalaCoordenadas.cEscalaCoordenadas.EscalarPuntosConEscalasDependientes(PuntosTodosObjetos, Coordenadas.Reales, out float SY2, HeigthWindow, SX, Zoom, Dx, Dy, XI, FactorReduccion: 1.3f);
        }

        private void CalcularLongitud()
        {
            float H = SubTramoOrigen.Seccion.H;
            float B = SubTramoOrigen.Seccion.B;
            float r1 = SubTramoOrigen.TramoOrigen.NervioOrigen.r1;
            float r2 = SubTramoOrigen.TramoOrigen.NervioOrigen.r2;
            float G135 = cDiccionarios.G135[NoBarra].Item1 * cConversiones.Dimension_m_to_cm;
            float G180 = cDiccionarios.G180[NoBarra].Item1*cConversiones.Dimension_m_to_cm;
            switch (Ramas)
            {
                case 1:
                    this.H = (H - r1 - r2)*cConversiones.Dimension_cm_to_m;this.B = 0f; LGancho = G180* cConversiones.Dimension_cm_to_m;
                     Longitud = (H - r1 - r2 + 2 * G180)*cConversiones.Dimension_cm_to_m;
                    break;
                case 2:
                    this.H = (H - r1 - r2) * cConversiones.Dimension_cm_to_m; this.B = (B - r1 - r2)* cConversiones.Dimension_cm_to_m; LGancho = G135 * cConversiones.Dimension_cm_to_m;
                    Longitud =( (H - r1 - r2) * 2 + (B - r1 - r2) * 2 + 2 * G135)*cConversiones.Dimension_cm_to_m;
                    break;
                default:
                    break;
            }

        }


        public override string ToString()
        {
            return $"E{cFunctionsProgram.ConvertireNoBarraToString(NoBarra)} | #Ramas: {Ramas} | X={CoordX} |L= {Math.Round(Longitud,2)}";
        }
    }
}