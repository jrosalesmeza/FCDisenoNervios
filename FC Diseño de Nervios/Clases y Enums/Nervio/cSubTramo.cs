﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cSubTramo: IElemento
    {

        public int Indice { get; set; }
        public int Index { get; }
        public string Nombre { get; set; }

        private float longitud;
        public float Longitud
        {
            get { return longitud; }
            set
            {
                if (longitud != value && longitud!=0)
                {
                    F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
                    longitud = value;
                    ReasignarEstaciones();
                    TramoOrigen.NervioOrigen.CrearCoordenadasPerfilLongitudinalReales();
                    TramoOrigen.NervioOrigen.CrearCoordenadasPerfilLongitudinalAutoCAD();
                    TramoOrigen.NervioOrigen.CrearCoordenadasDiagramaMomentosyCortantesyAreas_Reales_Envolvente();
                    TramoOrigen.NervioOrigen.CrearCoordenadasDiagramaMomentosyAreas_Reales_Asignado();
                }
                longitud = value;
            }

        }

        public cTramo TramoOrigen { get; set; }

        cSeccion seccion;
        public cSeccion Seccion
        {
            get { return seccion; }
            set
            {
                if (seccion != null)
                {
                    if(seccion.CambioB | seccion.CambioH)
                    {
                        seccion = value;
                        TramoOrigen.NervioOrigen.AsignarCambioAlturayCambioAnchoElementos();
                        TramoOrigen.NervioOrigen.CrearCoordenadasPerfilLongitudinalReales();
                        TramoOrigen.NervioOrigen.CrearCoordenadasPerfilLongitudinalAutoCAD();
                    }
                    seccion = value;
                }
           
            }
        }
        public cVistas Vistas { get; set; } = new cVistas();
        public List<cEstacion> Estaciones { get; set; }
        public List<cLine> Lista_Lineas { get; set; }
        public eSoporte Soporte { get; } = eSoporte.Vano; 
        public float HVirtual_Real { get; set; }
        public float HVirtual_AutoCAD { get; set; }

        public cCoordenadasCalculos CoordenadasCalculosSolicitaciones { get; set; } = new cCoordenadasCalculos();
        public cCoordenadasCalculos CoordenadasCalculosAsignado { get; set; } = new cCoordenadasCalculos();


        public cSubTramo(int Index,string Nombre,List<cLine> Lista_Lineas,cTramo TramoOrigen)
        {
            this.TramoOrigen = TramoOrigen;
            this.Index = Index;
            this.Nombre = Nombre;
            this.Lista_Lineas = Lista_Lineas;
            CrearEstaciones();
            seccion = cFunctionsProgram.DeepClone(Lista_Lineas.First().Seccion);
            TramoOrigen.NervioOrigen.d1_ = seccion.R_Top + cDiccionarios.AceroBarras[eNoBarra.B3];
            TramoOrigen.NervioOrigen.d2_ = seccion.R_Bottom + cDiccionarios.AceroBarras[eNoBarra.B3];
            CalcularLongitud();
        }


        private void CrearEstaciones()
        {
            float Delta = 0;
            for (int i = 0; i < Lista_Lineas.Count; i++)
            {
                cLine LineaAnterior = null;
                cLine LineActual = Lista_Lineas[i];
                if (i - 1 >= 0)
                {
                    LineaAnterior = Lista_Lineas[i - 1];
                    Delta = LineaAnterior.Estaciones.Last().CoordX;
                }
                LineActual.Estaciones.ForEach(x => x.CoordX = Delta + x.Localizacion - LineActual.Estaciones.First().Localizacion);
            }
            Estaciones = new List<cEstacion>();
            Lista_Lineas.ForEach(x => Estaciones.AddRange(x.Estaciones));
            Estaciones.ForEach(X => X.SubTramoOrigen = this);
        }
        private void ReasignarEstaciones()
        {
            float Delta = 0;
            float LongitudRedistribuir = Longitud / (Estaciones.Count - 1);
            Estaciones.ForEach(Estacion =>
            {
                Estacion.CoordX = Delta;
                Delta += LongitudRedistribuir;
            });

        }



        private void CalcularLongitud()
        {
            foreach(cLine Line in Lista_Lineas)
            {
                longitud += Line.ConfigLinea.Longitud- Line.ConfigLinea.OffSetI - Line.ConfigLinea.OffSetJ;
            }
            longitud = (float)Math.Round(Longitud, 2);
        }

        public override string ToString()
        {
            return $"{Nombre} |NoLineas= {Lista_Lineas.Count}| {Seccion.ToString()}";
        }

    }
}
