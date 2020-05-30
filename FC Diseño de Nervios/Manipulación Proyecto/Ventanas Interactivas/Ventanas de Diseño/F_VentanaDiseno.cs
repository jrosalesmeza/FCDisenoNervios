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

        private cBarra BarraSelect;
        public float Zoom = 1;
        public float Dx = 0;
        public float Dy = 0;

        public F_VentanaDiseno()
        {
            InitializeComponent();
            PB_VistaPerfilLongitudinalDiseno.MouseDown += PB_VistaPerfilLongitudinalDiseno_MouseDown2;
            PB_VistaPerfilLongitudinalDiseno.MouseDown += PB_VistaPerfilLongitudinalDiseno_MouseDown1;
            PB_VistaPerfilLongitudinalDiseno.MouseMove += PB_VistaPerfilLongitudinalDiseno_MouseMove;
            PB_VistaPerfilLongitudinalDiseno.MouseMove += PB_VistaPerfilLongitdunalDiseno_MouseMove3;
            LostFocus += F_VentanaDiseno_LostFocus;

        }

        private void F_VentanaDiseno_LostFocus(object sender, EventArgs e)
        {
            if (F_Base.F_SeleccionBarras.Focus())
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => { x.C_Barra.IsSelect = false; x.C_Barra.IsSelectArrastre = false; });
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => { x.C_Barra.IsSelect = false; x.C_Barra.IsSelectArrastre = false; });
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
        }

        private void PB_VistaPerfilLongitudinalDiseno_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            float XI = 15f;
            float WidthPB = PB_VistaPerfilLongitudinalDiseno.Width - XI * 3;
            float HeightPB = (PB_VistaPerfilLongitudinalDiseno.Height);
            List<PointF> PuntosNoEscalados = new List<PointF>();
            //Puntos No Escalados - Nervio y Grid
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x => PuntosNoEscalados.AddRange(x.Vistas.Perfil_AutoCAD.Reales));

            //Crear Coordenadas Escaladas de Elementos
            PuntosNoEscalados.Add(new PointF(0, 0));
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasLongitudinal_Elementos_Escalados_AutoCAD(PuntosNoEscalados, HeightPB/3, HeightPB, Dx, Dy, Zoom, XI);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_Elementos_Escalados_AutoCAD(e.Graphics, Zoom, PB_VistaPerfilLongitudinalDiseno.Height);
            F_Base.ActualizarVentanaF_MomentosNervio(Zoom, Dx, Dy);
            F_Base.ActualizarVentanaF_AreasMomentoNervio(Zoom, Dx, Dy);

            //Peso Refuerzo Longitudinal
            TSL_PesoLongitudinalValue.Text = string.Format("{0:0.00}", F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.PesoRefuerzo + F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.PesoRefuerzo)+" kg";



            //Peso Refuerzo Transversal
            TSL_PesoTranseversalValue.Text = string.Format("{0:0.00}", F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CalcularPesoTransversal()) + "kg";
        
        
        }

        private void F_VentanaDiseno_Paint(object sender, PaintEventArgs e)
        {
             Text = $"Refuerzo Longitudinal | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre}";
            CargarComboBox();
            PB_VistaPerfilLongitudinalDiseno.Invalidate();
            
        }


        private void CargarComboBox()
        {
            TSCB_RInferior.Items.Clear();
            TSCB_RSuperior.Items.Clear();
            List<int> TendeciasInferior = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasInferior.Select(x => x.ID).ToList();
            List<int> TendenciasSuperior = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasSuperior.Select(x => x.ID).ToList();
            TendeciasInferior.ForEach(y => TSCB_RInferior.Items.Add(y));
            TendenciasSuperior.ForEach(y => TSCB_RSuperior.Items.Add(y));
            TSCB_RInferior.Text = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.ID.ToString();
            TSCB_RSuperior.Text = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.ID.ToString();
        }

        private void F_VentanaDiseno_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinalDiseno.Invalidate();
        }

        private void TSCB_RSuperior_SelectedIndexChanged(object sender, EventArgs e)
        {
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasSuperior.Find(x => x.ID.ToString() == TSCB_RSuperior.Text);
        }

        private void TSCB_RInferior_SelectedIndexChanged(object sender, EventArgs e)
        {
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasInferior.Find(x => x.ID.ToString() == TSCB_RInferior.Text);

        }





        private void AgregarBarraSuperior()
        {
            F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Count == 0)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.AgregarBarra(cFunctionsProgram.CrearBarraDefault(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect, eUbicacionRefuerzo.Superior));
            }
            else
            {
                int IDMax = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Max(y => y.ID);
                cBarra BarraClonada = cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Last(x => x.ID == IDMax));
                BarraClonada.ID = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Last().ID + 1;
                BarraClonada.TendenciaOrigen = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Last().TendenciaOrigen;
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.AgregarBarra(BarraClonada);
            }
            PB_VistaPerfilLongitudinalDiseno.Invalidate();
        }
        private void AgregarBarraInferior()
        {
            F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Count == 0)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.AgregarBarra(cFunctionsProgram.CrearBarraDefault(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect, eUbicacionRefuerzo.Inferior));
            }
            else
            {
                int IDMax = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Max(y => y.ID);
                cBarra BarraClonada = cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Last(x=>x.ID== IDMax));
                BarraClonada.ID = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Last().ID + 1;
                BarraClonada.TendenciaOrigen = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Last().TendenciaOrigen;
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.AgregarBarra(BarraClonada);
            }
            PB_VistaPerfilLongitudinalDiseno.Invalidate();
        }
        private void EliminarBarra()
        {
            cBarra BarraSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Find(x => x.C_Barra.IsSelect | x.C_Barra.IsSelectArrastre);

            if (BarraSelect == null)
                BarraSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Find(x => x.C_Barra.IsSelect | x.C_Barra.IsSelectArrastre);

            if (BarraSelect != null)
            {
                F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
                if (BarraSelect.UbicacionRefuerzo == eUbicacionRefuerzo.Inferior)
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.EliminarBarra(BarraSelect);
                else
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.EliminarBarra(BarraSelect);
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }

        }

        private void CopiarBarra()
        {
            cBarra BarraSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Find(x => x.C_Barra.IsSelect | x.C_Barra.IsSelectArrastre);

            if (BarraSelect == null)
                BarraSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Find(x => x.C_Barra.IsSelect | x.C_Barra.IsSelectArrastre);

            if (BarraSelect != null)
            {
                F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
                cBarra BarraClonada = cFunctionsProgram.DeepClone(BarraSelect);
                if (BarraSelect.UbicacionRefuerzo == eUbicacionRefuerzo.Inferior)
                {
                    int IDMax = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Max(y => y.ID);
                    BarraClonada.ID = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Last().ID + 1;
                    BarraClonada.TendenciaOrigen = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Last().TendenciaOrigen;
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.AgregarBarra(BarraClonada);
                }
                else
                {
                    int IDMax = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Max(y => y.ID);
                    BarraClonada.ID = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Last().ID + 1;
                    BarraClonada.TendenciaOrigen = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Last().TendenciaOrigen;
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.AgregarBarra(BarraClonada);
                }
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
        }
        private void PB_VistaPerfilLongitudinalDiseno_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.MouseDown(e.Location,false));
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.MouseDown(e.Location,false));
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
            else if(e.Button == MouseButtons.Right)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.MouseDown(e.Location,true));
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.MouseDown(e.Location,true));
                BarraSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.Find(x => x.C_Barra.IsSelect);
                if (BarraSelect == null)
                {
                    BarraSelect = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.Find(x => x.C_Barra.IsSelect);
                }
                if (BarraSelect != null)
                {
                    F_Base.F_Base_.VentanaEmergente(ref F_Base.F_SeleccionBarras);
                }
                F_Base.F_SeleccionBarras.BarraSelect = BarraSelect;

                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
            Activate();
        }


        private void PB_VistaPerfilLongitudinalDiseno_MouseDown2(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.MouseDownEsferas(e.Location));
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.MouseDownEsferas(e.Location));
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
        }
        private void PB_VistaPerfilLongitudinalDiseno_MouseDown1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks== 2)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.MouseDownDoubleClick(e.Location));
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.MouseDownDoubleClick(e.Location));
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
        }
        private void PB_VistaPerfilLongitudinalDiseno_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect.Barras.ForEach(x => x.MouseMove(e.Location));
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
        }
        private void PB_VistaPerfilLongitdunalDiseno_MouseMove3(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect.Barras.ForEach(x => x.MouseMove(e.Location));
                PB_VistaPerfilLongitudinalDiseno.Invalidate();
            }
        }

        private void TSB_AgregarInferior_Click(object sender, EventArgs e)
        {
            AgregarBarraInferior();
        }

        private void TSB_AgregarSuperior_Click(object sender, EventArgs e)
        {
            AgregarBarraSuperior();
        }

        private void TSB_Eliminar_Click(object sender, EventArgs e)
        {
            EliminarBarra();
        }

        private void TSB_CopiarRefuerzo_Click(object sender, EventArgs e)
        {
            CopiarBarra();
        }

        private void eliminarBarraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliminarBarra();
        }

        private void eliminarBarraToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            EliminarBarra();
        }


    }
}
