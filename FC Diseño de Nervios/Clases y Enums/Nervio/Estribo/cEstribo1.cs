using System;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo
{
    [Serializable]
    public class cEstribo1
    {
        public IElemento Elemento { get; set; }
        public cCoordenadas Coordenadas { get; private set; }
        private cTendencia_Estribo Tendencia_Estribo { get; set; }
        public float Longitud { get; set; }
        public int Ramas { get; private set; }
        public eNoBarra NoBarra { get; set; }
        public float B { get; set; }
        public float H { get; set; }
        public float LGancho { get; set; }

        public cEstribo1(IElemento Elemento, eNoBarra NoBarra,int Ramas, cTendencia_Estribo tendencia_Estribo, cCoordenadas Coordenadas)
        {
            this.Elemento = Elemento;
            this.Ramas = Ramas;

            this.NoBarra = NoBarra;
            Tendencia_Estribo = tendencia_Estribo;
            this.Coordenadas = Coordenadas;

            CalcularLongitud();
        }


        private void CalcularLongitud()
        {
            float H = Elemento.Seccion.H;
            float B = Elemento.Seccion.B;
            float r1 = Tendencia_Estribo.Tendencia_Refuerzo_Origen.NervioOrigen.r1;
            float r2 = Tendencia_Estribo.Tendencia_Refuerzo_Origen.NervioOrigen.r2;
            float G135 = cDiccionarios.G135[NoBarra].Item1 * cConversiones.Dimension_m_to_cm;
            float G180 = cDiccionarios.G180[NoBarra].Item1 * cConversiones.Dimension_m_to_cm;

            if (Ramas == 1)
            {
                this.H = (H - r1 - r2) * cConversiones.Dimension_cm_to_m; this.B = 0f; LGancho = G180 * cConversiones.Dimension_cm_to_m;
                Longitud = (H - r1 - r2 + 2 * G180) * cConversiones.Dimension_cm_to_m;
            }
            else
            {
                this.H = (H - r1 - r2) * cConversiones.Dimension_cm_to_m; this.B = (B - r1 - r2) * cConversiones.Dimension_cm_to_m; LGancho = G135 * cConversiones.Dimension_cm_to_m;
                Longitud = ((H - r1 - r2) * 2 + (B - r1 - r2) * 2 + 2 * G135) * cConversiones.Dimension_cm_to_m + (H - r1 - r2 + 2 * G180) * cConversiones.Dimension_cm_to_m*(Ramas-2);
            }

        }


        public override string ToString()
        {
            return $"E{cFunctionsProgram.ConvertireNoBarraToString(NoBarra)} | #Ramas: {Ramas} |L= {Math.Round(Longitud, 2)}";
        }
    }
}
