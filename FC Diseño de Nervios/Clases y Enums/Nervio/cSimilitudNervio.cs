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
        public bool IsMaestroGeometria { get; set; }

        private List<string> similaresG_String;
        public List<string> SimilaresG_String {
            get { return similaresG_String; }
            set
            {
                similaresG_String = value;
                if (similaresG_String != null && similaresG_String.Count>=0)
                    IsMaestroGeometria = true;
                else
                    IsMaestroGeometria = false;
                
            }
        }

        public string SoySimiarA { get; set; } = "";

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
                    SoySimiarA = "";
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
            if (SimilaresG_String!=null && SimilaresG_String.Count > 0)
            {
                SimilaresG_String.ForEach(x => nerviosSimilares.Add(F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(y => y.Nombre == x)));
            }
        }



    }
}
