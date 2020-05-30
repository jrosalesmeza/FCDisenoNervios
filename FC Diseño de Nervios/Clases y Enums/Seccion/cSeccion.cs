using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSeccion
    {

        public bool CambioH { get; set; }
        public bool CambioB { get; set; }
        public cSeccion(string Nombre, float B, float H)
        {
            this.Nombre = Nombre;
            b = B;
            h = H;

        }

        public string Nombre { get; set; }

        private float b;
        public float B
        {
            get { return b; }
            set
            {
                if (b != value && b!=0)
                {
                    F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
                    CambioB =true;
                }
                else
                {
                    CambioB = false;
                }
                b = value;
            }
        }
        private float h;
        public float H
        {
            get { return h; }
            set
            {
                if (h != value && h != 0)
                {
                    F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
                    CambioH = true;
                }
                else
                {
                    CambioH = false;
                }
                h = value;
            }
        }
        public float R_Top { get; set; }
        public float R_Bottom { get; set; }
        public cMaterial Material { get; set; }

        public eType Type { get; set; }

        public override string ToString()
        {
            return $"{Nombre} | {B}cmx{H}cm | Material: {Material.Nombre}";
        }

    }
}