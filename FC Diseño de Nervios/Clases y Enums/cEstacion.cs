using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cEstacion: cObjetoCoordenadas
    {
        public string NombreLinea { get; set; }
        public string StoryLinea { get; set; }
        public string Nombre { get; set; }
        public float Localizacion { get; set; }
        public float CoordX { get; set; }
        public cLine LineaOrigen { get; set; }

        private cSubTramo subTramo;
        public cSubTramo SubTramoOrigen {
            get { return subTramo; }
            set
            {
                if (value != null)
                {
                    subTramo = value;
                    Calculos = new cCalculos(subTramo);
                }
            }
        
        }

        public cEstacion(string StoryLinea,string NombreLinea,float Localizacion)
        {
            this.StoryLinea = StoryLinea;
            this.NombreLinea = NombreLinea;
            this.Localizacion = Localizacion;
        }

        public cCalculos Calculos { get; set; }
        public List<cSolicitacion> Lista_Solicitaciones { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is cEstacion)
            {
                cEstacion temp = (cEstacion)obj;

                if (temp.StoryLinea == StoryLinea & temp.NombreLinea== NombreLinea & temp.Localizacion== Localizacion)
                    return true;
            }

            return false;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"{StoryLinea} | {NombreLinea} | {Localizacion}";
        }

    }
}