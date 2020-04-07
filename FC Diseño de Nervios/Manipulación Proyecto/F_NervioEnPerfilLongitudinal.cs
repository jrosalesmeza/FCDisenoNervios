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
    public partial class F_NervioEnPerfilLongitudinal : DockContent
    {
        private float Zoom = 1;
        private float Dx = 0;
        private float Dy = 0;
        private float DesXButton;
        private float DesYButton;
        public F_NervioEnPerfilLongitudinal()
        {
            InitializeComponent();
        }

        private void F_NervioEnPerfilLongitudinal_Paint(object sender, PaintEventArgs e)
        {
             PB_VistaPerfilLongitudinal.Invalidate();
        }

        private void PB_VistaPerfilLongitudinal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            float XI = 5f; float YI = 5f;
            float WidthPB = PB_VistaPerfilLongitudinal.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinal.Height - YI * 3) * 0.8f;
            List<PointF> PuntosNoEscalados = new List<PointF>();
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x => PuntosNoEscalados.AddRange(x.Vistas.Perfil_Original.Reales));
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasLongitudinal_Elementos_Escalados_Original(PuntosNoEscalados, WidthPB, HeightPB, Dx, Dy, Zoom);
            List<PointF> PuntosEscalados = new List<PointF>();
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x => PuntosEscalados.AddRange(x.Vistas.Perfil_Original.Escaladas));
            PointF PuntoInicialEscalado = new PointF(PuntosEscalados.Min(X => X.X), HeightPB-YI);
            PointF PuntoFinalEscalado = new PointF(PuntosEscalados.Max(X => X.X), HeightPB - YI);
            if (MostrarReglaToolStripMenuItem.Checked)
            {
                cFunctionsProgram.DrawRegla(e.Graphics, PuntoInicialEscalado, PuntoFinalEscalado, cFunctionsProgram.LongitudesElementos(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos), Zoom);
            }
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_Elementos_Escalados_Original(e.Graphics, Zoom);
        }

        private void PB_VistaPerfilLongitudinal_MouseDown(object sender, MouseEventArgs e)
        {
            IElemento ElementoSeleccionado = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Find(x => x.Vistas.IsSelectPlantaPerfilLongitudinal(e.Location));
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.FindAll(x => x != ElementoSeleccionado).ForEach(x => x.Vistas.SelectPerfilLongitudinal = false);
            if (ElementoSeleccionado != null)
            {
                F_Base.F_Base_.VentanaEmergente(ref F_Base.F_ModificarSeccion);
            }
            else { F_Base.F_ModificarSeccion.Close(); }
            
            PB_VistaPerfilLongitudinal.Invalidate();
        }













        private void F_NervioEnPerfilLongitudinal_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinal.Invalidate();
        }

        private void MostrarReglaToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinal.Invalidate();
        }

    
    }
}
