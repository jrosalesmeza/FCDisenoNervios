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
    public partial class F_MomentosNervio : DockContent
    {
        public float Zoom = 1;
        public float Dx = 0;
        public float Dy = 0;
        public F_MomentosNervio()
        {
            InitializeComponent();
        }

        private void PB_VistaPerfilLongitudinalMomentos_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            float XI = 15f;
            float WidthPB = PB_VistaPerfilLongitudinalMomentos.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinalMomentos.Height);

            List<PointF> PuntosNoEscalados = new List<PointF>();
            //Puntos No Escalados - Nervio y Grid
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x =>
            {
                if (x is cSubTramo)
                {
                    cSubTramo cSubTramo = (cSubTramo)x;
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Momentos_Positivos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Momentos_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosAsignado.Momentos_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosAsignado.Momentos_Positivos.Reales);
                }
            });

            //Crear Coordenadas Escaladas de Elementos

            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaMomentos_Escaladas_Envolvente(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaMomentos_Escaladas_Asignado(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_DrawMomentos(e.Graphics, Zoom, PB_VistaPerfilLongitudinalMomentos.Height);
        }

        private void F_MomentosNervio_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinalMomentos.Invalidate();
        }

        private void F_MomentosNervio_Paint(object sender, PaintEventArgs e)
        {
            PB_VistaPerfilLongitudinalMomentos.Invalidate();
            Text = $"Momentos | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre}";
        }

        private void PB_VistaPerfilLongitudinalMomentos_MouseMove(object sender, MouseEventArgs e)
        {
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.IsPointMouseMomentos(e.Location);
            PB_VistaPerfilLongitudinalMomentos.Invalidate();
        }
    }
}
