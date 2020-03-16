using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cNervio
    {
        public string Nombre
        {
            get => default;
            set
            {
            }
        }

        public string Prefijo
        {
            get => default;
            set
            {
            }
        }

        public List<cTramo> Lista_Tramos
        {
            get => default;
            set
            {
            }
        }

        public cTramo TramoSelect
        {
            get => default;
            set
            {
            }
        }

        public int ID
        {
            get => default;
            set
            {
            }
        }

        public List<cBarra> ListaBarras
        {
            get => default;
            set
            {
            }
        }
    }
}