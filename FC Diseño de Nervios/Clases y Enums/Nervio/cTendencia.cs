using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cTendencia
    {
        public cTendencia(int ID, cTendencia_Refuerzo Tendencia_Refuerzo_Origen)
        {
            this.ID = ID;
            this.Tendencia_Refuerzo_Origen = Tendencia_Refuerzo_Origen;
        }
        public int ID { get; set; }
        public string Nombre { get; set; }

        public List<cBarra> Barras { get; set; } = new List<cBarra>();

        public cTendencia_Refuerzo Tendencia_Refuerzo_Origen { get; set; }

        public float MinimaLongitud { get; set; }
        public float MaximaLongitud { get; set; }

        public List<eNoBarra> BarrasAEmplear { get; set; }
        public float DeltaAlargamientoBarras { get; set; }

        public float CuantiaMinimaInferior { get; set; }
        public float CuantiaMinimaSuperior { get; set; }


        public float PesoRefuerzo { get; set; }
        public eUbicacionRefuerzo UbicacionRefuerzo { get; set; }


        public void CrearBarra(cBarra Barra)
        {
            Barras.Add(Barra);
            AsignarNivelABarras();
        }
        public void AsignarNivelABarras()
        {
            Barras.ForEach(x => { x.Nivel = 1; x.TraslapoIzquierda = false; x.TraslpaoDerecha = false; x.SubNivel = 0; });
            Barras = Barras.OrderByDescending(x => x.LongitudRecta).ToList();

            //Asigna Niveles
            Barras.ForEach(Barra0 => { CompararBarras(Barra0, Barras); });

            //Ordenar barras de izquierda a derecha asignadon valores numericos
            Barras = Barras.OrderBy(x => x.XI).ToList();

            var GrupoBarras = from Barra in Barras
                              group Barra by Barra.Nivel into ListaBarras
                              select new { Nivel = ListaBarras.Key, ListaBeams = ListaBarras.ToList() };

            GrupoBarras.ToList().ForEach(ListaBarrasPorNivel => {
                int Contador = 0;
                ListaBarrasPorNivel.ListaBeams.ForEach(Barra => {
                    
                    if (Barra.TraslapoIzquierda | Barra.TraslpaoDerecha)
                    {
                        Barra.SubNivel = Contador % 2==0 ? 0 : 1;
                        Contador += 1;
                    }
                });
            });

  
            Barras.ForEach(x => x.CrearCoordenadasReales());
            Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignado();
        }



        private void CompararBarras(cBarra Barra0,List<cBarra> Barras)
        {
            Barras.ForEach(Barra1 => {

                if (Barra0 != Barra1  && Barra0.Nivel== Barra1.Nivel)
                {
                    if (XiPerteneceaX0X1(Barra0.XI, Barra0.XF, Barra1.XI) && XiPerteneceaX0X1(Barra0.XI, Barra0.XF, Barra1.XF)) // Barra 0 Cotiene Barra 1
                    {
                        Barra1.Nivel = Barra0.Nivel + 1;
                    }
                    else
                    {
                        float DistanciaLongitud;
                        if (XiPerteneceaX0X1(Barra0.XI, Barra0.XF, Barra1.XI))
                        {
                            DistanciaLongitud = DeterminarLongBarraRecta(Barra1.XI, Barra0.XF);

                            if (DistanciaLongitud > Barra0.Traslapo + cVariables.ToleranciaTraslapo)
                            {
                                Barra1.Nivel = Barra0.Nivel + 1;
                                CompararBarras(Barra1, Barras);
                            }
                            else if(!Barra0.TraslpaoDerecha | !Barra1.TraslapoIzquierda)
                            {
                                Barra0.TraslpaoDerecha = true; Barra1.TraslapoIzquierda = true;
                                CompararBarras(Barra1, Barras);
                            }
                        }
                        else if (XiPerteneceaX0X1(Barra0.XI, Barra0.XF, Barra1.XF))
                        {
                            DistanciaLongitud = DeterminarLongBarraRecta(Barra0.XI, Barra1.XF);

                            if (DistanciaLongitud > Barra0.Traslapo + cVariables.ToleranciaTraslapo)
                            {
                                Barra1.Nivel = Barra0.Nivel + 1;
                                CompararBarras(Barra1, Barras);
                            }
                            else if (!Barra0.TraslapoIzquierda | !Barra1.TraslpaoDerecha)
                            {
                                Barra0.TraslapoIzquierda = true; Barra1.TraslpaoDerecha = true;
                                CompararBarras(Barra1, Barras);
                            }
                        }

                        else if (XiPerteneceaX0X1(Barra1.XI, Barra1.XF, Barra0.XF))
                        {
                            DistanciaLongitud = DeterminarLongBarraRecta(Barra0.XI, Barra1.XF);

                            if (DistanciaLongitud > Barra0.Traslapo + cVariables.ToleranciaTraslapo)
                            {
                                Barra0.Nivel = Barra1.Nivel + 1;
                                CompararBarras(Barra0, Barras);
                            }
                            else if (!Barra1.TraslapoIzquierda | !Barra0.TraslpaoDerecha)
                            {
                                Barra1.TraslapoIzquierda = true;Barra0.TraslpaoDerecha = true;
                                CompararBarras(Barra0, Barras);
                            }
                        }else if (XiPerteneceaX0X1(Barra1.XI, Barra1.XF, Barra0.XI))
                        {
                            DistanciaLongitud = DeterminarLongBarraRecta(Barra0.XI, Barra1.XF);

                            if (DistanciaLongitud > Barra0.Traslapo + cVariables.ToleranciaTraslapo)
                            {
                                Barra0.Nivel = Barra1.Nivel + 1;
                                CompararBarras(Barra0, Barras);
                            }
                            else if (!Barra1.TraslpaoDerecha | !Barra0.TraslapoIzquierda)
                            {
                                Barra1.TraslpaoDerecha = true; Barra0.TraslapoIzquierda = true;
                                CompararBarras(Barra0, Barras);
                            }
                        }
                    }
                 

                }
            });
            
        }






        private bool XiPerteneceaX0X1(float X0, float X1, float Xi)
        {
            if (X1 >= Xi && X0 <= Xi)
            {
                return true;
            }

            return false;
        }

        private float DeterminarLongBarraRecta(float Xo,float Xi)
        {
            return Xi - Xo;
        }



        public override string ToString()
        {
            Nombre = $"Tendencia {ID}";
            return $"{Nombre} | {Barras.Count}";
        }
    }
}
