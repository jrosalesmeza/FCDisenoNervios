using System;
using System.Collections.Generic;
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

        public cNervio(int ID, string Prefijo,List<cObjeto> Lista_Objetos,eDireccion Direccion)
        {
            this.ID = ID;
            if (Prefijo != "") {
                this.Prefijo = Prefijo; 
            }
            
            Nombre = Prefijo + ID;
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
            int Contador = 0;
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
            }

        }
        





    }
}