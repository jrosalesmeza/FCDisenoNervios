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
        public int ID { get; }
        public string Nombre { get; set; }
        public string Prefijo { get; set; } = "N-";
        public eDireccion Direccion { get;set; }
        public List<cTramo> Lista_Tramos { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }
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
            CrearTramos();
        }


        public override string ToString()
        {
            return $"{Nombre} | CountTramos = {Lista_Tramos.Count} | {Direccion}";
        }


        public void CrearTramos()
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

        private bool ElementoEnumerado_MouseMove = false;


        public void PaintNombreElementosEnumerados_MouseMove(Graphics e, float HeigthWindow, float WidthWindow)
        {
            if (ElementoEnumerado_MouseMove)
            {
                Font Font1 = new Font("Calibri", 9, FontStyle.Bold);
                PointF PointString = new PointF(WidthWindow / 2 - e.MeasureString(Nombre, Font1).Width / 2, HeigthWindow / 2 - Font1.Height / 2);
                e.DrawString(Nombre.ToString(), Font1, Brushes.Black, PointString);


            }

        }

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

        


        
    }


    [Serializable]
    public enum eCambioenAltura
    {
        None,
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