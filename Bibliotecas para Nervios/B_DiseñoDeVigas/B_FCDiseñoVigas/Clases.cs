using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_FC_DiseñoVigas
{

    /// <summary>
    /// Revisión y diseño de vigas rectangulares [efe Prima Ce].
    /// </summary>
    public static class DiseñoYRevisonVigasRectangulares
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Alerta">Mensaje de Alerta.</param>
        public delegate void DelegateNotificador(string Alerta);
        /// <summary>
        /// Evento notificador. 
        /// </summary>
        public static event DelegateNotificador Notificador;
        private const float ecu = 0.003f;
        private const float Es = 2000000;
        /// <summary>
        /// Tipo de Viga: Sismica, No Sismica, No Aplica.
        /// </summary>
        public enum eTipoViga {
            /// <summary>
            /// 
            /// </summary>
            Sismica, 
            /// <summary>
            /// 
            /// </summary>
            NoSismica,
            /// <summary>
            /// 
            /// </summary>
            NoAplica }

        /// <summary>
        /// Diseña una viga Simple o Doblemente Reforzada  Return [AsT, AsC, fi,fi_max1, Mmax,c,et].
        /// </summary>
        /// <param name="b">Ancho [cm].</param>
        /// <param name="h">Alto [cm].</param>
        /// <param name="d1">Distancia desde la fibra superior hasta el centro de la barras a compresión [cm].</param>
        /// <param name="d2">Distancia desde la fibra inferior hasta el centro de la barras a tracción [cm].</param>
        /// <param name="fc">Resistencia del concreto [kgf/cm²]</param>
        /// <param name="fy">Resistencia del acero [kgf/cm²]</param>
        /// <param name="Mu">Momento a flexión a cual se está sometiendo el elemento [kgf-cm].</param>
        /// <param name="TipoViga">Tipo de Viga.</param>
        /// <param name="es">Deformación unitaria para calcular el Φmax. </param>
        /// <param name="pmin">Cuantía mínima según el tipo de viga [ρ=0.0033].</param>
        /// <param name="pminnosimico">Cuantía mínima según el tipo de viga [ρ=0.0018].</param>
        /// <returns></returns>
        public static float[] Diseñar(float b, float h, float d1, float d2, float fc, float fy, float Mu, eTipoViga TipoViga, float es = 0.004f, float pmin = 0.0033f, float pminnosimico = 0.0018f)
        {
            float alfa, beta2, Mmax,Asmin;
            float fi_max1, pb, c_max, a_max, fi, c, esP;
            float a, p, As, AsC, fsP, DeltaMu, As2, et, fi_n;
            float d = h - d2; float AsT = 0;
            if (TipoViga== eTipoViga.Sismica) { Asmin = pmin * b * d; } else if(TipoViga== eTipoViga.NoSismica) { Asmin = pminnosimico * b * h; } else { Asmin = 0; }
       
            if (fc <= 280f)
            {
                alfa = 0.7225f;
                beta2 = 0.425f * 2;
            }
            else
            {
                alfa = 0.7225f - (0.04f * (fc - 280f)) / 70f;
                beta2 = (0.425f - (0.025f * (fc - 280f)) / 70f) * 2;
            }

            if (es > 0.005f)
            {
                fi_max1 = 0.9f;
            }
            else if (es > 0.002f)
            {
                fi_max1 = (0.65f + (es - 0.002f) * 250f / 3f);
            }
            else
            {
                fi_max1 = 0.65f;
            }
            pb = 0.025f;
            c_max = (d * ecu) / (ecu + 0.004f);
            a_max = beta2 * c_max;
            Mmax = fi_max1 * 0.85f * fc * a_max * b * (d - a_max / 2);
            fi = 0.9f; fsP = 0;

            if (Mu > Mmax)
            {
                Notificador("Doblemente Reforzada.");
                c = d * ecu / (ecu + 0.005f);
                a = beta2 * c;
                As = (0.85f * fc * a * b) / (fy);
                p = As / (b * d);

                //Deformación del Acero a Compresión
                esP = ((c - d1) * ecu) / c;
                //Fuerza Acero a Compresión
                if (esP * Es > fy)
                {
                    fsP = fy;
                }
                else
                {
                    fsP = Es * esP;
                }
                //Determinamos DeltaMu

                DeltaMu = Mu - (fi * 0.85f * fc * a * b) * (d - a / 2);

                //Calculamos Acero a Compresión

                AsC = DeltaMu / (fi * (fsP - 0.85f * fc) * (d - d1));
                As2 = DeltaMu / (fi * fy * (d - d1));

                AsT = As + As2;
                et = ecu * (d - c) / c;

                //Revisamos  Cuantia Balanceada

                if ((AsT) / (b * d) > pb)
                {

                    Notificador("Cuantia de acero a tracción es mayor al 0.025");

                }
                if ((AsC) / (b * d) > pb)
                {

                    Notificador ("Cuantia de acero a compresión es mayor al 0.025");

                }

            }
            else
            {
                AsC = 0;
                float ErrorFi = 10;
                Notificador("Simplemente Reforzada.");
                fi_n = 0; c = 0;et = 0;
                while (ErrorFi > 0.00001f)
                {

                    //K = Mu / (b * Math.Pow(d, 2));
                    //m = (fy) / (0.85 * fc);
                    //p = (1 / m) * (1 - Math.Sqrt(1 - ((2 * m * K) / (fi * fy))));

                    float A = fi * 0.85f * fc * b / 2;
                    float B = -0.85f * fi * fc * b * d;
                    float C = Mu;


                    float a1 = (-B - (float)Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
                    float a2 = (-B + (float)Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
                    if (a1 > 0)
                    {
                        a = a1 > h ? a2 : a1;

                    }
                    else
                    {
                        a = a2;
                    }

                    c = a / beta2;

                    AsT = (0.85f * fc * a * b) / fy;
                    //AsT = p * b * d;

                    if (AsT < Asmin) { AsT = Asmin; }

                    //c = ((AsT * fy) / (0.85f * b * fc)) / (beta2);
                    et = ecu * (d - c) / c;
                    if (et <= 0.002)
                    {
                        fi_n = 0.65f;
                    }
                    else if (et > 0.002 && et <= 0.005)
                    {
                        float Constante = 250f / 3f;
                        fi_n = 0.65f + ((et - 0.002f) * (Constante));
                    }
                    else if (et > 0.005f)
                    {
                        fi_n = 0.90f;
                    }
                    if (fi > 0.90f) { fi_n = 0.90f; }

                    ErrorFi = Math.Abs(fi_n - fi) / (fi_n);
                    fi = fi_n;

                }


            }


            return new float[] { AsT, AsC, fi,fi_max1, Mmax,c,et};
        }



        /// <summary>
        /// Revisa una viga Simple o Doblemente Reforzada, dada su geometría y aceros a Tracción y Compresión.
        /// </summary>
        /// <param name="b">Ancho [cm].</param>
        /// <param name="h">Alto [cm].</param>
        /// <param name="d1">Distancia desde la fibra superior hasta el centro de la barras a compresión [cm].</param>
        /// <param name="d2">Distancia desde la fibra inferior hasta el centro de la barras a tracción [cm].</param>
        /// <param name="fc">Resistencia del concreto [kgf/cm²]</param>
        /// <param name="fy">Resistencia del acero [kgf/cm²]</param>
        ///         /// <param name="AsT">Acero a Tracción [cm²].</param>
        /// <param name="AsC">Acero a Compresión [cm²].</param>
        /// <returns></returns>

        public static float[] Revision(float b, float h, float d1, float d2, float fc, float fy, float AsT, float AsC)
        {
            float beta2, aa, bb, cc, c2, esP2, fsP2, c1, a, fsP, esP, c, fi=0.9f,Mr,est;
            float d = h - d2;
            if (fc <= 280f)
            {
                beta2 = 0.425f * 2;
            }
            else
            {
                beta2 = (0.425f - (0.025f * (fc - 280f)) / 70f) * 2;
            }
       
            aa = 0.85f * fc * b * beta2;
            bb = AsC * ecu * Es - AsT * fy - 0.85f * fc * AsC;
            cc = -AsC * d1 * ecu * Es;
            c2 = -(bb - (float)Math.Sqrt(Math.Pow(bb, 2) - 4 * aa * cc)) / (2 * aa);

            esP2 = (c2 - d1) * ecu / c2;

            fsP2 = esP2 * Es > fy ? fy : esP2 * Es;

            if (fsP2 <= 0)
            {
                fsP = 0;
                esP = 0;
            }
            else
            {
                fsP = fsP2;
                esP = esP2;

            }

            c1 = esP2 <= 0 ? (AsT * fy) / (0.85f * fc * b * beta2) :
                ((AsT - AsC) * fy + 0.85f * fc * AsC) / (0.85f * fc * b * beta2);


            c = esP <= 0 ? c1 : c2;
            a = c * beta2;
            est = (d - c) * ecu / c;


            if (est <= 0.002)
            {
                fi = 0.65f;
            }
            else if (est > 0.002 && est <= 0.005)
            {
                float Constante = 250f / 3f;
                fi = 0.65f + ((est - 0.002f) * (Constante));
            }
            else if (est > 0.005f)
            {
                fi = 0.90f;
            }

            Mr = fsP == 0 ? fi * (0.85f * fc * a * b * (d - a / 2)) : fi * (0.85f * fc * a * b * (d - a / 2) + AsC * (d - d1) * fsP - AsC * (d - d1) * 0.85f * fc);


            return new float[] { Mr, fi, c, est };



        }


    }

}
