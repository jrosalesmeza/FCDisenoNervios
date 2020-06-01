using FC_BFunctionsAutoCAD;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public List<eNoBarra> BarrasAEmplearBase { get; set; }

        public List<eNoBarra> BarrasAEmplearAdicional { get; set; }
        public float DeltaAlargamientoBarras { get; set; }

        public float CuantiaMinima { get; set; }

        private float pesoRefuerzo;
        public float PesoRefuerzo {
            get { return pesoRefuerzo; }
            set
            {
                if (pesoRefuerzo != value)
                {
                    pesoRefuerzo = value;
                }
            }
        }
        public eUbicacionRefuerzo UbicacionRefuerzo { get; set; }


        public void EliminarBarra(cBarra Barra)
        {
            Barras.Remove(Barra);
            AsignarNivelABarras();
        }
        public void EliminarBarra(int IndexBarra)
        {
            Barras.RemoveAt(IndexBarra);
            AsignarNivelABarras();
        }
        public void EliminarBarras()
        {
            Barras.Clear();
            AsignarNivelABarras();

        }
        public void AgregarBarra(cBarra Barra)
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
            Tendencia_Refuerzo_Origen.NervioOrigen.CrearAceroAsignadoRefuerzoLongitudinal();
            CalcularPeso();
        }

        private void CalcularPeso()
        {
            PesoRefuerzo = 0;
            Barras.ForEach(x => PesoRefuerzo += x.Peso);
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



        public void Paint_AutoCAD(float X, float Y)
        {
            Barras.ForEach(x => x.Paint_AutoCAD(X, Y));


            //Agregar Cotas

            if (UbicacionRefuerzo == eUbicacionRefuerzo.Inferior)
            {
                Barras.ForEach(Barra =>
                {

                    if (!Barra.TraslpaoDerecha && !Barra.TraslapoIzquierda)
                    {
                        cSubTramo SubTramo = (cSubTramo)Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x.IsVisibleCoordAutoCAD(Barra.XF) && x is cSubTramo);

                        if (SubTramo != null)
                        {
                            float Ymax = Barra.C_Barra.Reales.Max(x => x.Y);

                            PointF P1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(Barra.XF, Ymax), X, Y);
                            PointF P2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(SubTramo.Vistas.Perfil_AutoCAD.Reales[2].X, Ymax), X, Y);
                            FunctionsAutoCAD.AddCota(P1, P2, cVariables.C_Cotas, cVariables.Estilo_Cotas, cVariables.Desplazamiento_Cotas_RefuerzoInferior,
                              DeplazaTextY: cVariables.DesplazamientoTexto_Cotas);
                        }
                    }
                });

            }
            else
            {
                void AgregarCotaBarrasIndependietes(cBarra Barra, IElemento ElementoXF)
                {
                    float Ymax = Barra.C_Barra.Reales.Max(x => x.Y);
                    PointF P1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(ElementoXF.Vistas.Perfil_AutoCAD.Reales[0].X, Ymax), X, Y);
                    PointF P2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(Barra.XF, Ymax), X, Y);
                    FunctionsAutoCAD.AddCota(P1, P2, cVariables.C_Cotas, cVariables.Estilo_Cotas, -cVariables.Desplazamiento_Cotas_RefuerzoSuperior,
                      DeplazaTextY: cVariables.DesplazamientoTexto_Cotas);
                }

                int MaximoNivelBarras = Barras.Max(x => x.Nivel);
                Barras.ForEach(Barra =>
                {

                    IElemento ElementoXI = Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x.IsVisibleCoordAutoCAD(Barra.XI));
                    IElemento ElementoXF = Tendencia_Refuerzo_Origen.NervioOrigen.Lista_Elementos.Find(x => x.IsVisibleCoordAutoCAD(Barra.XF));

                    if (!(ElementoXI is cApoyo) && !(ElementoXF is cApoyo) && Barra.GanchoDerecha == eTipoGancho.None && Barra.GanchoIzquierda == eTipoGancho.None)
                    {

                        if (Barra.Nivel == MaximoNivelBarras)
                        {
                            AgregarCotaBarrasIndependietes(Barra, ElementoXF);
                        }
                        else
                        {
                            cBarra BarraNivelPosterior = EncontrarBarraConentedora(Barra);
                            if (BarraNivelPosterior == null)
                            {
                                AgregarCotaBarrasIndependietes(Barra, ElementoXF);
                            }
                            else
                            {

                                float Ymax = BarraNivelPosterior.C_Barra.Reales.Max(x => x.Y);
                                float DespCota = Barra.C_Barra.Reales.Min(x => x.Y) - Ymax;
                                PointF P1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(BarraNivelPosterior.XF, Ymax), X, Y);
                                PointF P2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(Barra.XF, Barra.C_Barra.Reales.Min(x => x.Y)), X, Y);
                                FunctionsAutoCAD.AddCota(P1, P2, cVariables.C_Cotas, cVariables.Estilo_Cotas, -DespCota - 0.13f,
                                DeplazaTextY: cVariables.DesplazamientoTexto_Cotas, RA: false);
                            }
                        }


                    }




                });
            }

            Barras.ForEach(x => { x.CotaParaAutoCADIzquierda = false;x.CotaParaAutoCADDerecha = false; }) ;
            Barras.ForEach(Barra => {

                if (Barra.TraslpaoDerecha && !Barra.CotaParaAutoCADDerecha)
                {
                    List<cBarra> Barras1 = Barras.FindAll(x => x.TraslapoIzquierda);
                    foreach(cBarra barra in Barras1)
                    {
                        if (barra!= Barra && XiPerteneceaX0X1(Barra.XI, Barra.XF, barra.XI))
                        {
                            Barra.CotaParaAutoCADDerecha = true;
                            barra.CotaParaAutoCADIzquierda = true;
                            cBarra BarraPredominante = barra.C_Barra.Reales.Max(x => x.Y) > Barra.C_Barra.Reales.Max(x => x.Y) ? barra : Barra;
                            float Ymax = BarraPredominante.C_Barra.Reales.Min(x => x.Y);
                            float DespCota= cVariables.Desplazamiento_Cotas_RefuerzoInferior;
                            if (UbicacionRefuerzo == eUbicacionRefuerzo.Superior)
                            {
                                DespCota = -cVariables.Desplazamiento_Cotas_RefuerzoSuperior;
                            }
                            PointF P1 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(barra.XI, Ymax), X, Y);
                            PointF P2 = B_Operaciones_Matricialesl.Operaciones.Traslacion(new PointF(Barra.XF, Ymax), X, Y);
                            FunctionsAutoCAD.AddCota(P1, P2, cVariables.C_Cotas, cVariables.Estilo_Cotas, DespCota,
                            DeplazaTextY: cVariables.DesplazamientoTexto_Cotas, RA: false);
                        }
                    }


                }
   

            });



        }



        public cBarra EncontrarBarraConentedora(cBarra BarraMadre)
        {
            foreach (cBarra barra in Barras)
            {
                if (BarraMadre != barra) {
                    if (Barra1ContieneBarra2(BarraMadre, barra)) {
                        return barra;
                    } 
               }
            }
            return null;
        }


        public bool Barra1ContieneBarra2(cBarra Barra1, cBarra Barra2)
        {
            return Barra1.XI <= Barra2.XI && Barra1.XF >= Barra2.XF;
        }

        public override string ToString()
        {
            Nombre = $"Tendencia {ID}";
            return $"{Nombre} | {Barras.Count}";
        }
    }
}
