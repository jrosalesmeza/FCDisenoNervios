using System;
using System.Collections.Generic;
using System.Linq;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cTramo
    {
        public int ID { get; set; }
        public cNervio NervioOrigen { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }
        public List<cSubTramo> Lista_SubTramos { get; set; }
        public string Nombre { get; set; }
        public float Longitud { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Vano;
        public List<cEstribo> Estribo { get; set; }

        public cTramo(string Nombre, List<cObjeto> Lista_Objetos, cNervio NervioOrigen)
        {
            this.Nombre = Nombre;
            this.Lista_Objetos = Lista_Objetos;
            this.NervioOrigen = NervioOrigen;
            CrearSubTramos();
        }

        public void CrearSubTramos()
        {
            Lista_SubTramos = new List<cSubTramo>();
            List<cLine> Lista_Iniciales = new List<cLine>();
            int i = 0; int IndexTramo = 0;
            foreach (cObjeto Objeto in Lista_Objetos)
            {
                if (Lista_Iniciales.Count == 0)
                {
                    Lista_Iniciales.Add(Objeto.Line);
                }
                else
                {
                    if (Objeto.Line.Seccion == Lista_Iniciales.Last().Seccion)
                    {
                        Lista_Iniciales.Add(Objeto.Line);
                    }
                    else
                    {
                        
                        cSubTramo subTramo = new cSubTramo(IndexTramo, "SubTramo " + (IndexTramo + 1), Lista_Iniciales, this);
                        Lista_SubTramos.Add(subTramo);
                        IndexTramo += 1;
                        Lista_Iniciales = new List<cLine>();
                        Lista_Iniciales.Add(Objeto.Line);
                    }
                }
                i++;

                if (i == Lista_Objetos.Count)
                {
                    if (Lista_Iniciales.Count != 0)
                    {
                        cSubTramo subTramo = new cSubTramo(IndexTramo, "SubTramo " + (IndexTramo + 1), Lista_Iniciales, this);
                        Lista_SubTramos.Add(subTramo);
                        IndexTramo += 1;
                    }
                }
            }

        }

        public override string ToString()
        {
            return $"{Nombre} | L={Longitud} | CountObjetos= {Lista_Objetos.Count}";
        }
    }
}