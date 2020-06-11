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
    public class cApoyo : IElemento
    {
        public int Indice { get; set; }
        public string Nombre { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Apoyo;



        public float Longitud { get; set; }

        private cSeccion seccion;
        public cSeccion Seccion
        {
            get { return seccion; }
            set
            {
                if (seccion != null)
                {
                    if (seccion.CambioB | seccion.CambioH)
                    {
                        seccion = value;
                        NervioOrigen.AsignarCambioAlturayCambioAnchoElementos();
                        NervioOrigen.CrearCoordenadasPerfilLongitudinalReales();
                        NervioOrigen.CrearCoordenadasPerfilLongitudinalAutoCAD();
                        NervioOrigen.CrearCoordenadasDiagramaMomentosyCortantesyAreas_Reales_Envolvente();
                        NervioOrigen.CrearCoordenadasDiagramaMomentosyAreas_Reales_Asignado();
                        NervioOrigen.CrearCoordenadasDiagramaAreasCortante_Reales_Asigando();
                        NervioOrigen.AsignarCambiosANerviosSimilares(Indice);
                        Longitud = seccion.B * cConversiones.Dimension_cm_to_m;
                    }
                }
              
            }
        }
        public cVistas Vistas { get; set; } = new cVistas();
        public float HVirtual_Real { get; set; }
        public float HVirtual_AutoCAD { get; set; }
        public cNervio NervioOrigen { get; set; }
        public cApoyo(string Nombre,cSeccion Seccion,cNervio NervioOrigen)
        {
            this.Nombre = Nombre;
            seccion = Seccion;
            this.NervioOrigen = NervioOrigen;
            Longitud = seccion.B * cConversiones.Dimension_cm_to_m;
        }

        public bool IsApoyo(PointF Punto)
        {
            GraphicsPath Path = new GraphicsPath();
            Path.AddPolygon(Vistas.Perfil_Original.Reales.ToArray());
            if (Path.IsVisible(Punto))
                return true;
            return false;
        }
        public bool IsVisibleCoordAutoCAD(float X)
        {
            return Vistas.Perfil_AutoCAD.Reales.Min(x => x.X) <= X && X <= Vistas.Perfil_AutoCAD.Reales.Max(x => x.X);
        }
        public override string ToString()
        {
            return $"{Nombre} | {Seccion.ToString()}";
        }
    }
}
