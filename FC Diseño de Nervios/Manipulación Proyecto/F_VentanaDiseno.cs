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
    public partial class F_VentanaDiseno : DockContent
    {

        public float Zoom = 1;
        public float Dx = 0;
        public float Dy = 0;

        public F_VentanaDiseno()
        {
            InitializeComponent();
        }

        private void PB_VistaPerfilLongitudinalDiseno_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.Clear(Color.White);
            float XI = 15f; float YI = 15f;
            float WidthPB = PB_VistaPerfilLongitudinalDiseno.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinalDiseno.Height);
            List<PointF> PuntosNoEscalados = new List<PointF>();
            //Puntos No Escalados - Nervio y Grid
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x => PuntosNoEscalados.AddRange(x.Vistas.Perfil_AutoCAD.Reales));

            //Crear Coordenadas Escaladas de Elementos
            PuntosNoEscalados.Add(new PointF(0, 0));
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasLongitudinal_Elementos_Escalados_AutoCAD(PuntosNoEscalados, HeightPB/3, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_Elementos_Escalados_AutoCAD(e.Graphics, Zoom, PB_VistaPerfilLongitudinalDiseno.Height);

        }

        private void F_VentanaDiseno_Paint(object sender, PaintEventArgs e)
        {
            PB_VistaPerfilLongitudinalDiseno.Invalidate();
        }

        private void F_VentanaDiseno_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinalDiseno.Invalidate();
        }
    }
}
