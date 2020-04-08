using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSubTramo: IElemento
    {
        public int Index { get; set; }
        public string Nombre { get; set; }

        private float longitud;
        public float Longitud
        {
            get { return longitud; }
            set
            {
                if (longitud != value && longitud!=0)
                {
                    longitud = value;
                    TramoOrigen.NervioOrigen.CrearCoordenadasPerfilLongitudinalReales();
                }
                longitud = value;
            }

        }

        public cTramo TramoOrigen { get; set; }

        cSeccion seccion;
        public cSeccion Seccion
        {
            get { return seccion; }
            set
            {
                if (seccion != null)
                {
                    if(seccion.CambioB | seccion.CambioH)
                    {
                        TramoOrigen.NervioOrigen.CrearCoordenadasPerfilLongitudinalReales();
                    }
                }
                seccion = value;
            }
        }
        public cVistas Vistas { get; set; } = new cVistas();
        public cEstacion Estacion { get; set; } = new cEstacion();
        public List<cLine> Lista_Lineas { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Vano; 
        public float HVirtual { get; set; }

        public cSubTramo(int Index,string Nombre,List<cLine> Lista_Lineas,cTramo TramoOrigen)
        {
            this.TramoOrigen = TramoOrigen;
            this.Index = Index;
            this.Nombre = Nombre;
            this.Lista_Lineas = Lista_Lineas;
            seccion = cFunctionsProgram.DeepClone(Lista_Lineas.First().Seccion);
            CalcularLongitud();
        }
        private void CalcularLongitud()
        {
            foreach(cLine Line in Lista_Lineas)
            {
                longitud += Line.ConfigLinea.Longitud- Line.ConfigLinea.OffSetI - Line.ConfigLinea.OffSetJ;
            }
            longitud = (float)Math.Round(Longitud, 2);
        }

        public override string ToString()
        {
            return $"{Nombre} |NoLineas= {Lista_Lineas.Count}| {Seccion.ToString()}";
        }

    }
}
