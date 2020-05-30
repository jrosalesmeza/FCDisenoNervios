using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FC_Diseño_de_Nervios
{
    public partial class F_AreasMomentoNervio : DockContent
    {
        public float Zoom = 1;
        public float Dx = 0;
        public float Dy = 0;

        public F_AreasMomentoNervio()
        {
            InitializeComponent();
        }

        private void PB_VistaPerfilLongitudinalAreas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            float XI = 15f; 
            float WidthPB = PB_VistaPerfilLongitudinalAreas.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinalAreas.Height );

            List<PointF> PuntosNoEscalados = new List<PointF>();
            //Puntos No Escalados - Nervio y Grid
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x =>
            {
                if (x is cSubTramo)
                {
                    cSubTramo cSubTramo = (cSubTramo)x;
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Areas_Momentos_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Areas_Momentos_Positivos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosAsignado.Areas_Momentos_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosAsignado.Areas_Momentos_Positivos.Reales);
                }
            });

            //Crear Coordenadas Escaladas de Elementos
            PuntosNoEscalados.Add(new PointF(0, 0));
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaAreasMomentos_Escaladas_Envolvente(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaAreasMomentos_Escaladas_Asignado(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_DrawAreasMomentos(e.Graphics, Zoom, PB_VistaPerfilLongitudinalAreas.Height);

        }

        private void F_AreasMomentoNervio_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinalAreas.Invalidate();
        }

        private void F_AreasMomentoNervio_Paint(object sender, PaintEventArgs e)
        {
            PB_VistaPerfilLongitudinalAreas.Invalidate();
            Text = $"Areas Momentos | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | Unidades [cm²]";
        }

        private void PB_VistaPerfilLongitudinalAreas_MouseMove(object sender, MouseEventArgs e)
        {
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.IsPointMouseAreasMomentos(e.Location);
            PB_VistaPerfilLongitudinalAreas.Invalidate();
        }
    }
}
