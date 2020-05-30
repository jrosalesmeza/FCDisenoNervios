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
    public partial class F_AreasCortanteNervio : DockContent
    {
        public float Zoom = 1;
        public float Dx = 0;
        public float Dy = 0;
        public F_AreasCortanteNervio()
        {
            InitializeComponent();
            Paint += F_AreasCortanteNervio_Paint;
            PB_VistaPerfilLongitudinalAreasCortante.Paint += PB_VistaPerfilLongitudinalAreasCortante_Paint;
            PB_VistaPerfilLongitudinalAreasCortante.MouseMove += PB_VistaPerfilLongitudinalAreasCortante_MouseMove;
        }

        private void PB_VistaPerfilLongitudinalAreasCortante_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.IsPointMouseAreasCortanteSolicitado(e.Location);
                PB_VistaPerfilLongitudinalAreasCortante.Invalidate();
            }
            catch { }
        }   

        private void PB_VistaPerfilLongitudinalAreasCortante_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            float XI = 15f;
            float WidthPB = PB_VistaPerfilLongitudinalAreasCortante.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinalAreasCortante.Height);

            List<PointF> PuntosNoEscalados = new List<PointF>();
            //Puntos No Escalados - Nervio y Grid
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x =>
            {
                if (x is cSubTramo)
                {
                    cSubTramo cSubTramo = (cSubTramo)x;
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Areas_Cortante_Positivos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosSolicitaciones.Areas_Cortante_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosAsignado.Areas_Cortante_Negativos.Reales);
                    PuntosNoEscalados.AddRange(cSubTramo.CoordenadasCalculosAsignado.Areas_Cortante_Positivos.Reales);
                }
            });

            //Crear Coordenadas Escaladas de Elementos

            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaAreasCortante_Escaladas_Envolvente(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasDiagramaAreasCortante_Escalada_Asignado(PuntosNoEscalados, HeightPB, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_DrawAreasCortante(e.Graphics, Zoom, PB_VistaPerfilLongitudinalAreasCortante.Height);





        }

        private void F_AreasCortanteNervio_Paint(object sender, PaintEventArgs e)
        {
            PB_VistaPerfilLongitudinalAreasCortante.Invalidate();
            Text = $"Areas Cortante | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | Unidades [cm²/cm]";
        }
    }
}
