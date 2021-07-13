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
        public float VersionPrograma { get; set; } = Program.Version;
        public string Nombre { get; set; }
        public string Ruta { get; set; } = "";
        public cDatosEtabs DatosEtabs { get; set; }
        public cEdificio Edificio { get; set; }
        public eTipoRefuerzo TipoRefuerzo { get; set; } = eTipoRefuerzo.Todo;
        public eDireccion FiltroDireccionNervios { get; set; } = eDireccion.Todos;
        public bool EtiquetasDeBarras { get; set; } = true;
        public bool AcotamientoInteligente { get; set; } = true;
        public bool AcotamientoTraslapos { get; set; } = true;
        public bool RedondearBarra { get; set; } = true;
        public bool CoordenadasPInterseccion { get; set; } = false;

        private int cantidadEstacionesInterpolar = 10;
        public int CantidadEstacionesInterpolar
        {
            get
            {
                if (cantidadEstacionesInterpolar == 0)
                    return 10;
                return cantidadEstacionesInterpolar;
            }
            set
            {
                if(value!= cantidadEstacionesInterpolar)
                {
                    cantidadEstacionesInterpolar = value;
                }
            }
        }
        public bool VerSolicitaciones { get; set; } = false;

        public eNomenclatura Nomenclatura_Hztal { get; set; } = eNomenclatura.Alfabética;
        public eNomenclatura Nomenclatura_Vert { get; set; } = eNomenclatura.Numérica;


        private eNomenclatura nomenclaturaDiagonalNegativa{ get; set; } = eNomenclatura.Alfabética;
        private eNomenclatura nomenclaturaDiagonalPositiva { get; set; } = eNomenclatura.Numérica;

        private bool nomenclaturaDiagonal=false;
        public bool NomenclaturaDiagonal
        {
            get
            {
                return nomenclaturaDiagonal;
            }
            set
            {
                if (value != nomenclaturaDiagonal)
                {
                    nomenclaturaDiagonal = value;
                }
            }
        }
        public eNomenclatura NomenclaturaDiagonalNegativa
        {
            get
            {
                return nomenclaturaDiagonalNegativa;
            }
            set
            {
                if (value != nomenclaturaDiagonalNegativa)
                {
                    nomenclaturaDiagonalNegativa = value;
                }
            }
        }
        public eNomenclatura NomenclaturaDiagonalPositiva
        {
            get
            {
                return nomenclaturaDiagonalPositiva;
            }
            set
            {
                if (value != nomenclaturaDiagonalPositiva)
                {
                    nomenclaturaDiagonalPositiva = value;
                }
            }
        }


        #region Funciones Proyecto




        #endregion


        public override string ToString()
        {
            return $"{Nombre}";
        }





    }
}