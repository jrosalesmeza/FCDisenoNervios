using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cBarra : cObjetoCoordenadas
    {
        public int ID { get; set; }
        public eNoBarra NoBarra { get; set; }
        public int CantBarra { get; set; }
        public float Longitud { get; set; }
        public float AreaTotal { get; set; }
        public float DeltaAlargamiento { get; set; }
        public eUbicacionRefuerzo UbicacionRefuerzo { get; set; }
        public eTipoGancho GanchoIzquierda { get; set; }
        public eTipoGancho GanchoDerecha { get; set; }
        public cTendencia TendenciaOrigen { get; set; }
        public cCoordenadas C_Barra { get; set; } = new cCoordenadas();
        public cCoordenadas C_F_Izquierda { get; set; } = new cCoordenadas();
        public cCoordenadas C_F_Central { get; set; } = new cCoordenadas();
        public cCoordenadas C_F_Derecha { get; set; } = new cCoordenadas();

        public cBarra(int ID,cTendencia TendenciaOrigen, eNoBarra NoBarra, int CantBarra, List<PointF> C_Barra)
        {
            this.ID = ID;
            this.TendenciaOrigen = TendenciaOrigen;
            this.NoBarra = NoBarra;
            this.CantBarra = CantBarra;
            this.TendenciaOrigen = TendenciaOrigen;
            this.C_Barra.Reales = C_Barra;
            CalcularLongitud();
        }

        public void CalcularLongitud()
        {
            Longitud = (float)Math.Round(cFunctionsProgram.Long(C_Barra.Reales),cVariables.CifrasDeciLongBarra);
        }


        public override string ToString()
        {
            return $"{CantBarra}#{NoBarra.ToString().Replace("B","")} L={Longitud}";
        }


    }
}