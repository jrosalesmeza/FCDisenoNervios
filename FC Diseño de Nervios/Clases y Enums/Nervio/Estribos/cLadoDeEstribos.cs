using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cLadoDeEstribos
    {

        public float PesoRefuerzoTransversal { get; set; }
        public cZonaEstribos Zona1 { get; set; }
        public cZonaEstribos Zona2 { get; set; } 
        public eLadoDeZona LadoDeZona { get; set; }
        public cTramo TramoOrigen { get; set; }
        public cLadoDeEstribos(eLadoDeZona LadoDeZona, cTramo TramoOrigen)
        {
            this.LadoDeZona = LadoDeZona;
            this.TramoOrigen = TramoOrigen;
            Zona1 = new cZonaEstribos(eZonas.Zona1, LadoDeZona,this);
            Zona2 =  new cZonaEstribos(eZonas.Zona2, LadoDeZona,this);
        }

        public void CalcularPesoAcero()
        {
            PesoRefuerzoTransversal = 0;
            Zona1.Estribos.ForEach(x => PesoRefuerzoTransversal+=x.Longitud  * cDiccionarios.PesoBarras[x.NoBarra] );
            Zona2.Estribos.ForEach(x => PesoRefuerzoTransversal += x.Longitud * cDiccionarios.PesoBarras[x.NoBarra]);
        }

        public float IsArea_S(float X)
        {
            //if (Zona1.NoBarra != eNoBarra.BNone && Zona2.NoBarra != eNoBarra.BNone)
            //    if (Zona1.Xmin <= X && X <= Zona2.Xmax)
            //        return Zona1.Area_S > Zona2.Area_S ? Zona1.Area_S : Zona2.Area_S;
            if (Zona1.Xmin <= X && X <= Zona1.Xmax)
                return Zona1.Area_S;
            if (Zona2.Xmin <= X && X <= Zona2.Xmax)
                return Zona2.Area_S;
            return 0f;
        }

        public override string ToString()
        {
            return $"Zona 1 {Zona1} | Zona 2 {Zona2}";
        }
    }
}
