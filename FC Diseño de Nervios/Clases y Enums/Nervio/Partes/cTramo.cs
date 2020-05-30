using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cTramo
    {
        public int ID { get; set; }
        public cNervio NervioOrigen { get; set; }
        public List<cObjeto> Lista_Objetos { get; set; }
        public List<cSubTramo> Lista_SubTramos { get; set; }
        public string Nombre { get; set; }

        private float longitud;
        public float Longitud { get { return longitud; } set { if (longitud != value) { longitud = value; } } }
        public eSoporte Soporte { get; } = eSoporte.Vano;
        public cLadoDeEstribos EstribosIzquierda { get; set; }
        public cLadoDeEstribos EstribosDerecha { get; set; }

        public float PesoRefuerzoTransversal { get; set; }

        public void CalcularPesoTransversal()
        {
            PesoRefuerzoTransversal = 0;
            if (EstribosDerecha != null)
                PesoRefuerzoTransversal += EstribosDerecha.PesoRefuerzoTransversal;
            if (EstribosIzquierda != null)
                PesoRefuerzoTransversal += EstribosIzquierda.PesoRefuerzoTransversal;
        }


        public void UnirZonaDeEstribos()
        {
            if(EstribosIzquierda!= null && EstribosDerecha!= null)
            {
                cZonaEstribos ZonaEstribosIzquierda = EstribosIzquierda.Zona1.Xmax > EstribosIzquierda.Zona2.Xmax ? EstribosIzquierda.Zona1: EstribosIzquierda.Zona2;
                float XminZD1 = EstribosDerecha.Zona1.Xmin;
                float XminZD2 = EstribosDerecha.Zona2.Xmin;
                if (XminZD1 == 0)
                    XminZD1 = 999f;
                if (XminZD2 == 0f)
                    XminZD2 = 999f;
                cZonaEstribos ZonaEstribosDerecha = XminZD1 > XminZD2 ? EstribosDerecha.Zona2 : EstribosDerecha.Zona1;

                if(Math.Round(ZonaEstribosIzquierda.Xmax,2)>=Math.Round(ZonaEstribosDerecha.Xmin,2))
                {
                    float S = ZonaEstribosIzquierda.Separacion < ZonaEstribosDerecha.Separacion ? ZonaEstribosIzquierda.Separacion : ZonaEstribosDerecha.Separacion;
                    float LongitudE = longitud - 2 * cVariables.d_CaraApoyo;
                    EstribosDerecha = null;
                    cFunctionsProgram.CrearEstribos(this, eZonas.Zona1, eLadoDeZona.Izquierda, LongitudE, S, ZonaEstribosIzquierda.NoBarra, cFunctionsProgram.CoordXInicialEstribo(eLadoDeZona.Izquierda, this));
                    EstribosIzquierda.Zona2.EliminarEstribos(true);
                }
            }

        }

        #region Campos No Serializables
        [NonSerialized]
        private List<cEstacion> estacions;
        [NonSerialized]
        private float coordX;
        public float CoordX { get { return coordX; } set { coordX = value; } }
        public List<cEstacion> Estaciones { get { return estacions; } set { estacions = value; } }
        [NonSerialized]
        private float deltaAcero;
        public float DeltaAcero { get { return deltaAcero; } set { deltaAcero = value; } }


        #endregion
        public cTramo(string Nombre, List<cObjeto> Lista_Objetos, cNervio NervioOrigen)
        {
            this.Nombre = Nombre;
            this.Lista_Objetos = Lista_Objetos;
            this.NervioOrigen = NervioOrigen;
            CrearSubTramos();
            CalcularLongitud();
        }

        public void CrearSubTramos()
        {
            Lista_SubTramos = new List<cSubTramo>();
            List<cLine> Lista_Iniciales = new List<cLine>();
            int i = 0; int IndexTramo = 0;
            foreach (cObjeto Objeto in Lista_Objetos)
            {
                if (Lista_Iniciales.Count == 0)
                {
                    Lista_Iniciales.Add(Objeto.Line);
                }
                else
                {
                    if (Objeto.Line.Seccion == Lista_Iniciales.Last().Seccion)
                    {
                        Lista_Iniciales.Add(Objeto.Line);
                    }
                    else
                    {
                        
                        cSubTramo subTramo = new cSubTramo(IndexTramo, "SubTramo " + (IndexTramo + 1), Lista_Iniciales, this);
                        Lista_SubTramos.Add(subTramo);
                        IndexTramo += 1;
                        Lista_Iniciales = new List<cLine>();
                        Lista_Iniciales.Add(Objeto.Line);
                    }
                }
                i++;

                if (i == Lista_Objetos.Count)
                {
                    if (Lista_Iniciales.Count != 0)
                    {
                        cSubTramo subTramo = new cSubTramo(IndexTramo, "SubTramo " + (IndexTramo + 1), Lista_Iniciales, this);
                        Lista_SubTramos.Add(subTramo);
                        IndexTramo += 1;
                    }
                }
            }
        }

        public void CalcularLongitud()
        {
            longitud = Lista_SubTramos.Sum(x => x.Longitud);
        }
          
        public override string ToString()
        {
            return $"{Nombre} | L={Longitud} | CountObjetos= {Lista_Objetos.Count}";
        }
    }
}