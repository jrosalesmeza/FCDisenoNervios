using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cObjetoCoordenadas
    {
        public List<PointF> Longitudinal_Real { get; set; }


        public List<PointF> Longitudinal_Escalado { get; set; }


        public List<PointF> Planta_Escalado { get; set; }


        public List<PointF> Planta_Real { get; set; }


        public bool Select { get; set; }
        public void CrearPuntosPlantaRealEtabsLine(cPoint Point1, cPoint Point2)
        {
            Longitudinal_Real = new List<PointF>();
            Longitudinal_Real.Add(new PointF(Point1.X,Point1.Y));
            Longitudinal_Real.Add(new PointF(Point2.X, Point2.Y));
        }

    }
}