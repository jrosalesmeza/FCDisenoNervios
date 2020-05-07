﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cApoyo : IElemento
    {
        public int Indice { get; set; }
        public string Nombre { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Apoyo;



        public float Longitud { get; set; }

        private cSeccion seccion;
        public cSeccion Seccion
        {
            get { return seccion; }
            set
            {
                if (seccion != null)
                {
                    if (seccion.CambioB | seccion.CambioH)
                    {
                        seccion = value;
                        NervioOrigen.AsignarCambioAlturayCambioAnchoElementos();
                        NervioOrigen.CrearCoordenadasPerfilLongitudinalReales();
                        NervioOrigen.CrearCoordenadasPerfilLongitudinalAutoCAD();
                        NervioOrigen.CrearCoordenadasDiagramaMomentosyCortantesyAreas_Reales_Envolvente();
                        NervioOrigen.CrearCoordenadasDiagramaMomentosyAreas_Reales_Asignado();
                        Longitud = seccion.B * cConversiones.Dimension_cm_to_m;
                    }
                }
              
            }
        }
        public cVistas Vistas { get; set; } = new cVistas();
        public float HVirtual_Real { get; set; }
        public float HVirtual_AutoCAD { get; set; }
        public cNervio NervioOrigen { get; set; }
        public cApoyo(string Nombre,cSeccion Seccion,cNervio NervioOrigen)
        {
            this.Nombre = Nombre;
            seccion = Seccion;
            this.NervioOrigen = NervioOrigen;
            Longitud = seccion.B * cConversiones.Dimension_cm_to_m;
        }

        public override string ToString()
        {
            return $"{Nombre} | {Seccion.ToString()}";
        }
    }
}
