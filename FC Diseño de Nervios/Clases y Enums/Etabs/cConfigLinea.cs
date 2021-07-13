using System;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cConfigLinea
    {
        public cConfigLinea(cPoint Point1P, cPoint Point2P)
        {
            this.Point1P = Point1P;
            this.Point2P = Point2P;
            ClasificacionDireccionElemento();
            CalcularLongitud();
        }




        private void ClasificacionDireccionElemento()
        {
            if (Point1P.X == Point2P.X)
            {
                Direccion = eDireccion.Vertical;
            }
            else if (Point1P.Y == Point2P.Y)
            {
                Direccion = eDireccion.Horizontal;
            }
            else
            {
                float DistX = Math.Abs(Point1P.X - Point2P.X);
                float DistY = Math.Abs(Point1P.Y - Point2P.Y);
                float Pendiente_Grados = (float)Math.Atan(DistY / DistX) * cConversiones.Angulo_Rad_to_Grad;
                if (Pendiente_Grados >= cVariables.ToleranciaVertical)
                {
                    Direccion = eDireccion.Vertical;
                }
                else if (Pendiente_Grados <= cVariables.ToleranciaHorizontal)
                {
                    Direccion = eDireccion.Horizontal;
                }
                else
                {
                    Direccion = eDireccion.Diagonal;
                }
            }
        }

        public bool ComprarPendientes(cLine line, float tolerancia)
        {
            float DistX1 = Point1P.X - Point2P.X;
            float DistY1 = Point1P.Y - Point2P.Y;
            float m1 = (float)Math.Round(Math.Atan(DistY1 / DistX1) * cConversiones.Angulo_Rad_to_Grad, cVariables.CifrasDeciLongBarra);
            float DistX2 = line.ConfigLinea.Point1P.X - line.ConfigLinea.Point2P.X;
            float DistY2 = line.ConfigLinea.Point1P.Y - line.ConfigLinea.Point2P.Y;
            float m2 = (float)Math.Round(Math.Atan(DistY2 / DistX2) * cConversiones.Angulo_Rad_to_Grad, cVariables.CifrasDeciLongBarra);

            float tolerancia2 = tolerancia * 90f;
            bool comparacion1= m1 == m2 || (m2 > m1 && m2 - m1 <= tolerancia2) || (m1 > m2 && m1 - m2 <= tolerancia2);

            if (!comparacion1)
            {
                if (Math.Abs(m1) == 90f || Math.Abs(m1)==270f)
                {
                    m1 = Math.Abs(m1);
                }
                if (Math.Abs(m2)== 90f || Math.Abs(m2)==270f)
                {
                    m2 = Math.Abs(m2);
                }
                tolerancia2 = tolerancia * 90f;
                comparacion1 = m1 == m2 || (m2 > m1 && m2 - m1 <= tolerancia2) || (m1 > m2 && m1 - m2 <= tolerancia2);
            }

            return comparacion1;

        
        }

        public float Pendiente()
        {
            float DistX1 = Point1P.X - Point2P.X;
            float DistY1 = Point1P.Y - Point2P.Y;
            float m1 = (float)Math.Round(Math.Atan(DistY1 / DistX1) * cConversiones.Angulo_Rad_to_Grad, cVariables.CifrasDeciLongBarra);
            return m1;
        }
          

        public bool Activar_Cambio_Ejes { get; set; } = false;
        public eDireccion Direccion { get; set; }
        public bool Select { get; set; }

        public cPoint Point1P { get; set; }

        public cPoint Point2P { get; set; }
        public float Longitud { get; set; }

        private float longitudPonderacion = 0f;

        public float LongitudPonderacion
        {
            get
            {
                if (longitudPonderacion == 0)
                    longitudPonderacion= Longitud- OffSetI - OffSetJ;
                return longitudPonderacion;
            }
            set
            {
                if (longitudPonderacion != value)
                    longitudPonderacion = value;
            }
        }
        public float OffSetI { get; set; } = 0;
        public float OffSetJ { get; set; } = 0;
        public float Angulo { get; set; }

        public void CalcularLongitud()
        {
            Longitud = (float)Math.Round(cFunctionsProgram.Long(Point1P, Point2P), 2);
        }

        public override string ToString()
        {
            return $"{Point1P},{Point2P}, L= {Longitud}";
        }

        public void Direccionar_Ejes()
        {
            if (Direccion== eDireccion.Diagonal || Direccion== eDireccion.Horizontal) //Caso Horizontal Y Diagonal
            {
                if (Point1P.X > Point2P.X)
                {
                    Activar_Cambio_Ejes = true;
                }
            }
            else //Caso vertical
            {
                if (Point1P.Y > Point2P.Y)
                {
                    Activar_Cambio_Ejes = true;
                }
            }

            if (Activar_Cambio_Ejes)
            {
                cPoint PuntoAuxiliar1 = cFunctionsProgram.DeepClone(Point1P);
                cPoint PuntoAuxiliar2 = cFunctionsProgram.DeepClone(Point2P);
                Point1P = PuntoAuxiliar2;
                Point2P = PuntoAuxiliar1;

                float Offset_i = OffSetI;
                float offset_j = OffSetJ;

                OffSetI = offset_j;
                OffSetJ = Offset_i;
            }
        }
    }
}