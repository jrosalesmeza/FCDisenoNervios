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

        private float localizacionAPonderar=0f;
        public float LocalizacionAPoderar
        {
            get
            {
                if (localizacionAPonderar == 0f)
                    localizacionAPonderar = CoordX;
                return localizacionAPonderar;
            }
            set
            {
                if(value!= localizacionAPonderar)
                {
                    localizacionAPonderar = value;
                }
            }
        }
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
                    Calculos = new cCalculos(subTramo,this);
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


        public cEstacion EstacionMasCercana(List<cEstacion> Estaciones)
        {
            float MenorDistancia = 99999; cEstacion EstacionCercana=null;
            foreach(cEstacion Estacion in Estaciones)
            {
                float Distancia = Math.Abs(Estacion.CoordX - CoordX);

                if (Distancia < MenorDistancia)
                {
                    MenorDistancia = Distancia;
                    EstacionCercana = Estacion;
                }
            }
            return EstacionCercana;
        }

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