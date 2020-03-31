using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{

    [Serializable]
    public class cNervio
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Prefijo { get; set; } = "N-";
        public eDireccion Direccion { get;set; }
        public List<cTramo> Lista_Tramos { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }
        public bool SelectPlanta { get; set; }
        private bool ElementoEnumerado_MouseMove = false;
        public cObjeto ObjetoSelect { get; set; }
        public List<cBarra> ListaBarras { get; set; }
        public eCambioenAltura CambioenAltura { get; set; }
        public eCambioenAncho CambioenAncho { get; set; }
        public cNervio(int ID, string Prefijo,List<cObjeto> Lista_Objetos,eDireccion Direccion)
        {
            this.ID = ID;
            if (Prefijo != "") {
                this.Prefijo = Prefijo; 
            }
            Nombre = this.Prefijo + ID;
            this.Lista_Objetos = Lista_Objetos;
            this.Direccion = Direccion;
            AsignarCambioAlturayCambioAncho();
            if (Lista_Objetos.Count > 1)
            {
                CrearCoordenadas();
            }
            CrearTramos();
        }

        public void Cambio_Nombre()
        {
            Nombre = Prefijo + ID;
        }

        private void AsignarCambioAlturayCambioAncho()
        {
            bool CambioAltura = false; bool CambioAncho = false;

            for (int i = 0; i < Lista_Objetos.Count; i++)
            {
                cObjeto ObjetoActual = Lista_Objetos[i];
                cObjeto ObjetoDespues = null;float DeltaH = 0;float DeltaB = 0;
                if (i + 1 < Lista_Objetos.Count)
                {
                    ObjetoDespues = Lista_Objetos[i + 1];
                }

                if (ObjetoDespues != null && ObjetoActual.Soporte== eSoporte.Vano && ObjetoDespues.Soporte== eSoporte.Vano)
                {

                    DeltaB = ObjetoActual.Line.Seccion.B - ObjetoDespues.Line.Seccion.B;
                    DeltaH = ObjetoActual.Line.Seccion.H - ObjetoDespues.Line.Seccion.H;

                }

                if (DeltaB != 0)
                {
                    CambioAncho = true;
                }
                if (DeltaH != 0)
                {
                    CambioAltura = true;
                }

            }

            CambioenAltura = CambioAltura ? eCambioenAltura.Inferior : eCambioenAltura.None;
            CambioenAncho = CambioAncho ? eCambioenAncho.Inferior : eCambioenAncho.None; // Cambios Inferiores Predeterminados


        }







        public override string ToString()
        {
            return $"{Nombre} | CountTramos = {Lista_Tramos.Count} | {Direccion}";
        }


        private void CrearTramos()
        {
            Lista_Tramos = new List<cTramo>();
            List<cObjeto> Objetos = new List<cObjeto>();
            int Contador = 0;int Contador2 = 0;
            foreach (cObjeto Objeto in Lista_Objetos)
            {
                if(Objeto.Soporte!= eSoporte.Apoyo)
                {
                    Objetos.Add(Objeto);
                }
                else
                {
                    if (Objetos.Count != 0)
                    {
                        Contador += 1;
                        cTramo Tramo = new cTramo("Tramo " + Contador,Objetos);
                        Lista_Tramos.Add(Tramo);
                    }
                    Objetos = new List<cObjeto>();
                }
                Contador2 += 1;

                if (Contador2== Lista_Objetos.Count)
                {
                    if(Objeto.Soporte== eSoporte.Vano)
                    {
                        if (Objetos.Count != 0)
                        {
                            Contador += 1;
                            cTramo Tramo = new cTramo("Tramo " + Contador, Objetos);
                            Lista_Tramos.Add(Tramo);
                        }

                    }
                }


         

            }

        }

        public void CrearCoordenadas()
        {
            for (int i = 0; i < Lista_Objetos.Count; i++)
            {
                cObjeto ObjetoAnterior=null; cObjeto Objeto_Posterior = null;
                if (i - 1 >= 0)
                {
                    ObjetoAnterior = Lista_Objetos[i - 1];
                }
                if (i + 1 < Lista_Objetos.Count)
                {
                    Objeto_Posterior = Lista_Objetos[i + 1];
                }
                CrearCoordenadasLongitudinal(ObjetoAnterior, Lista_Objetos[i], Objeto_Posterior);
            }
   
        }

        private void CrearCoordenadasLongitudinal(cObjeto Objeto_Anterior, cObjeto Objeto_Actual, cObjeto Objeto_Posterior)
        {
            PointF PuntoInicial = new PointF(0,0); float HApoyo;
            HApoyo = Objeto_Actual.Line.Seccion.H;
            if (Objeto_Actual.Soporte == eSoporte.Apoyo)
            {
                if (Objeto_Posterior != null)
                {
                    HApoyo = Objeto_Posterior.Line.Seccion.H;
                }
                else
                {
                    HApoyo = Objeto_Anterior.Line.Seccion.H;
                }
            }
            if(Objeto_Anterior !=null && Objeto_Posterior!=null)
            {
                float H_anterior = Objeto_Anterior.Line.Seccion.H;
                float H_Posterior = Objeto_Posterior.Line.Seccion.H;
                float DeltaH = Math.Abs(H_anterior - H_Posterior);
                PuntoInicial = Objeto_Anterior.Vistas.Perfil_Original.Reales[Objeto_Anterior.Vistas.Perfil_Original.Reales.Count - 1];

                if (CambioenAltura == eCambioenAltura.Inferior)
                {
                    if (H_anterior < H_Posterior)
                    {
                        DeltaH = -DeltaH;
                    }
                }
                else if (CambioenAltura == eCambioenAltura.Central)
                {
                    DeltaH = DeltaH / 2;

                    if (H_anterior < H_Posterior)
                    {
                        DeltaH = -DeltaH;
                    }
                }

                PuntoInicial.Y += DeltaH;
            }
            else if(Objeto_Posterior == null)
            {
                PuntoInicial = Objeto_Anterior.Vistas.Perfil_Original.Reales[Objeto_Anterior.Vistas.Perfil_Original.Reales.Count - 1];
                float DeltaH = 0;
                float H_anterior = Objeto_Anterior.Line.Seccion.H;
                float H_Actual = Objeto_Actual.Line.Seccion.H;
                if (Objeto_Actual.Soporte != eSoporte.Apoyo)
                {
                    DeltaH = Math.Abs(H_anterior - H_Actual);
                }

                if (CambioenAltura == eCambioenAltura.Inferior)
                {
                    if (H_anterior < H_Actual)
                    {
                        DeltaH = -DeltaH;
                    }
                }
                else if (CambioenAltura == eCambioenAltura.Central)
                {
                    DeltaH = DeltaH / 2;

                    if (H_anterior < H_Actual)
                    {
                        DeltaH = -DeltaH;
                    }
                }

                PuntoInicial.Y += DeltaH;
            }

            CrearCoordenadasLongitudinal_Elemento_Reales(Objeto_Actual, PuntoInicial, HApoyo);

        }
        private void CrearCoordenadasLongitudinal_Elemento_Reales(cObjeto Objeto,PointF PuntoInicial, float HApoyo=0)
        {
            Objeto.Vistas.Perfil_Original.Reales = new List<PointF>();
            float H = Objeto.Line.Seccion.H;
            float B = Objeto.Line.Seccion.B;
            float Longitud = Objeto.Line.ConfigLinea.Longitud;
            if (Objeto.Soporte== eSoporte.Vano)
            {
                Longitud -= Objeto.Line.ConfigLinea.OffSetI - Objeto.Line.ConfigLinea.OffSetJ;
                Objeto.Vistas.Perfil_Original.Reales.Add(PuntoInicial);
                Objeto.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X,PuntoInicial.Y+H));
                Objeto.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X+ Longitud, PuntoInicial.Y + H));
                Objeto.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + Longitud, PuntoInicial.Y));
            }
            else if(Objeto.Soporte == eSoporte.Apoyo)
            {
                Objeto.Vistas.Perfil_Original.Reales.Add(PuntoInicial);
                Objeto.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X, PuntoInicial.Y + HApoyo));
                Objeto.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y + HApoyo));
                Objeto.Vistas.Perfil_Original.Reales.Add(new PointF(PuntoInicial.X + B, PuntoInicial.Y));
            }


        }

        #region Metodos Mouse
        public void MouseMoveNervioPlantaEtabs(Point Point)
        {

            foreach(cTramo Tramo in Lista_Tramos)
            {
                foreach (cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    ElementoEnumerado_MouseMove= Objeto.Line.MouseInLineEtabs(Point);
                    if (ElementoEnumerado_MouseMove)
                    {
                        break; 
                    }
                    else
                    {
                        ElementoEnumerado_MouseMove = false;
                    }
                }
                if (ElementoEnumerado_MouseMove)
                {
                    break;
                }
                else
                {
                    ElementoEnumerado_MouseMove = false;
                }
            }

          

        }
        public void MouseDownSelectPlanta(Point Point)
        {
            bool EncotroResultado = false;
            foreach (cTramo Tramo in Lista_Tramos)
            {
                foreach (cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    if (Objeto.Line.MouseInLineEtabs(Point))
                    {
                        if (SelectPlanta)
                        {
                            SelectPlanta = false;
                        }
                        else
                        {

                            SelectPlanta = true;

                        }

                        EncotroResultado = true;
                    }
                    if (EncotroResultado) { break; }

                }
                if (EncotroResultado) { break; }
            }

            Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.Select = SelectPlanta));

        }
        #endregion

        #region Metodos Paint
        public void PaintNombreElementosEnumerados_MouseMove(Graphics e, float HeigthWindow, float WidthWindow)
        {
            if (ElementoEnumerado_MouseMove)
            {
                Font Font1 = new Font("Calibri", 9, FontStyle.Bold);
                PointF PointString = new PointF(WidthWindow / 2 - e.MeasureString(Nombre, Font1).Width / 2, HeigthWindow / 2 - Font1.Height / 2);
                e.DrawString(Nombre.ToString(), Font1, Brushes.Black, PointString);
            }

        }
        public void Paint_Planta_ElementosEnumerados(Graphics e)
        {
            foreach(cTramo Tramo in Lista_Tramos)
            {
                foreach(cObjeto Objeto in Tramo.Lista_Objetos)
                {
                    Objeto.Line.PaintPlantaEscaladaEtabsLine(e);
                }
            }

        }
        #endregion




    }


    [Serializable]
    public enum eCambioenAltura
    {
        None,
        Central,
        Superior,
        Inferior
    }
    [Serializable]
    public enum eCambioenAncho
    {
        None,
        Central,
        Superior,  //Izquierda
        Inferior  //Derecha
    }
}