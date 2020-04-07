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
        public float Longitud { get; set; }
        public cTramo TramoOrigen { get; set; }
        public cSeccion Seccion { get; set; }
        public cVistas Vistas { get; set; } = new cVistas();
        public cEstacion Estacion { get; set; } = new cEstacion();
        public List<cLine> Lista_Lineas { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Vano; 

        public cSubTramo(int Index,string Nombre,List<cLine> Lista_Lineas,cTramo TramoOrigen)
        {
            this.TramoOrigen = TramoOrigen;
            this.Index = Index;
            this.Nombre = Nombre;
            this.Lista_Lineas = Lista_Lineas;
            Seccion = cFunctionsProgram.DeepClone(Lista_Lineas.First().Seccion);
            CalcularLongitud();
        }
        private void CalcularLongitud()
        {
            foreach(cLine Line in Lista_Lineas)
            {
                Longitud += Line.ConfigLinea.Longitud- Line.ConfigLinea.OffSetI - Line.ConfigLinea.OffSetJ;
            }
        }

        public override string ToString()
        {
            return $"{Nombre} |NoLineas= {Lista_Lineas.Count}| {Seccion.ToString()}";
        }

    }
}
