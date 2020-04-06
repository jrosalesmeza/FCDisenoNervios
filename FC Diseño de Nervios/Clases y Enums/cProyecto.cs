using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cProyecto
    {
        public cProyecto(string Nombre)
        {
            this.Nombre = Nombre;
        }
        public string Nombre { get; set; }
        public string Ruta { get; set; } = "";
        public cDatosEtabs DatosEtabs { get; set; }
        public cEdificio Edificio { get; set; }


        public eNomenclatura Nomenclatura_Hztal { get; set; } = eNomenclatura.Alfabética;
        public eNomenclatura Nomenclatura_Vert { get; set; } = eNomenclatura.Numérica;

        #region Funciones Proyecto




        #endregion








        public override string ToString()
        {
            return $"{Nombre}";
        }





    }
}