using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{

    [Serializable]
    public class cSimilitudNervio
    {
        public bool IsMaestro { get; set; }      

        private List<cSimilar> similaresG_String;
        public List<cSimilar> Similares_List_SimilarA {
            get { return similaresG_String; }
            set
            {
                similaresG_String = value;
                if (similaresG_String != null && similaresG_String.Count>=0)
                    IsMaestro = true;
                else
                    IsMaestro = false;
            }
        }

        public cSimilar SoySimiarA { get;  set; } = new cSimilar();

        private bool boolSoySimilarA = false;
        public bool BoolSoySimiarA {

            get { return boolSoySimilarA; }
            set
            {
                if (value != boolSoySimilarA)
                {
                    boolSoySimilarA = value;
                }
                if (!boolSoySimilarA)
                    SoySimiarA = new cSimilar();
            }
        }

        [NonSerialized]
        private List<cNervio> nerviosSimilares;
        public List<cNervio> NerviosSimilares
        {
            get
            {
                AsignarSimilares();
                return nerviosSimilares;
            }
            set { nerviosSimilares = value; }
        }

        private void AsignarSimilares()
        {
            nerviosSimilares = new List<cNervio>();
            if (Similares_List_SimilarA!=null && Similares_List_SimilarA.Count > 0)
            {
                Similares_List_SimilarA.ForEach(SimiliarA => nerviosSimilares.Add(F_Base.Proyecto.Edificio.Lista_Pisos.Find(Piso=> Piso.Nombre== SimiliarA.NombrePiso).Nervios.Find(Nervio=> Nervio.Nombre== SimiliarA.NombreNervio)));
            }
        }







        [Serializable]
        public class cSimilar
        {
            public string NombreNervio { get; set; }
            public string NombrePiso { get; set; }

            public cSimilar()
            {
                NombreNervio = "";
                NombrePiso = "";
            }
            public cSimilar(string NombreNervio, string NombrePiso)
            {
                this.NombreNervio = NombreNervio;
                this.NombrePiso = NombrePiso;
            }

            public string ToString(string NombrePiso)
            {
                if (NombrePiso != "" && NombreNervio != "")
                {
                    return NombrePiso != this.NombrePiso ? $"{NombreNervio} | {this.NombrePiso}" : NombreNervio;
                }
                else
                {
                    return "";
                }
            }

            public cNervio FindNervio()
            {
               return F_Base.Proyecto.Edificio.Lista_Pisos.Find(y => y.Nombre == NombrePiso).Nervios.Find(y => y.Nombre == NombreNervio);
            }

        }





    }
}
