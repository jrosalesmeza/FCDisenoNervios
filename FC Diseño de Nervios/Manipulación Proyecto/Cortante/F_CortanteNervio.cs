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

namespace FC_Diseño_de_Nervios.Manipulación_Proyecto
{
    public partial class F_CortanteNervio : DockContent
    {

        public float Zoom = 1;
        public float Dx = 0;
        public float Dy = 0;
        public F_CortanteNervio()
        {
            InitializeComponent();
            PB_VistaPerfilLongitudinalCortante.MouseMove += PB_VistaPerfilLongitudinalCortante_MouseMove1;
            PB_VistaPerfilLongitudinalCortante.MouseMove += PB_VistaPerfilLongitudinalCortante_MouseMove2;
            PB_VistaPerfilLongitudinalCortante.MouseMove += PB_VistaPerfilLongitudinalCortante_MouseMove3;
        }

        private void PB_VistaPerfilLongitudinalCortante_MouseMove3(object sender, MouseEventArgs e)
        {
            try
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.IsPointMouseFiCortante2(e.Location);
                PB_VistaPerfilLongitudinalCortante.Invalidate();
            }
            catch { }
        }

        private void PB_VistaPerfilLongitudinalCortante_MouseMove2(object sender, MouseEventArgs e)
        {
            try
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.IsPointMouseFiCortante(e.Location);
                PB_VistaPerfilLongitudinalCortante.Invalidate();
            }
            catch { }

        }

        private void PB_VistaPerfilLongitudinalCortante_MouseMove1(object sender, MouseEventArgs e)
        {
            try
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.IsPointMouseCortante(e.Location);
                PB_VistaPerfilLongitudinalCortante.Invalidate();
            }
            catch { }


        }

        private void PB_VistaPerfilLongitudinalCortante_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            float XI = 15f; 
            float WidthPB = PB_VistaPerfilLongitudinalCortante.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinalCortante.Height );

            List<PointF> PuntosNoEscalados = new List<PointF>();
            //Puntos No Escalados - Nervio y Grid
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x =>
            {
                if (x is cSubTramo)
                {
                    cSubTramo cSubTramo = (cSubTramo)x;
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Cortante_Positvos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Cortante_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.Coordenadas_FI_Vc_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.Coordenadas_FI_Vc_Positivos.Reales);
                }
            });

            //Crear Coordenadas Escaladas de Elementos

            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaCortante_Escaladas_Envolvente(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_DrawCortante(e.Graphics, Zoom, PB_VistaPerfilLongitudinalCortante.Height, WidthPB);

        }

        private void F_CortanteNervio_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinalCortante.Invalidate();
        }

        private void F_CortanteNervio_Paint(object sender, PaintEventArgs e)
        {
            PB_VistaPerfilLongitudinalCortante.Invalidate();
            Text = $"Cortante | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | Unidades [Ton]";
        }

    }
}
