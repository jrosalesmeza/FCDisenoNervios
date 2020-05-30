using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cVistas
    {
        public cCoordenadas Perfil_Original { get; set; } = new cCoordenadas();
        public cCoordenadas Perfil_AutoCAD { get; set; } = new cCoordenadas();
        public bool SelectPerfilLongitudinal { get; set; }




        public bool IsSelectPlantaPerfilLongitudinal(PointF Point)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddPolygon(Perfil_Original.Escaladas.ToArray());
            if (graphicsPath.IsVisible(Point))
            {
                SelectPerfilLongitudinal = true;
                return true;
            }
            else
            {
                SelectPerfilLongitudinal = false;
                return false;
            }
        }








    }


  



}
