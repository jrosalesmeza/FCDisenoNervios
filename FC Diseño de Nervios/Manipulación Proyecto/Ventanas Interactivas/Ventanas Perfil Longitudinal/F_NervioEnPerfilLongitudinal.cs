using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            PB_VistaPerfilLongitudinal.MouseDown += PB_VistaPerfilLongitudinal_MouseDown_RestablecerZoom;
            PB_VistaPerfilLongitudinal.MouseWheel += PB_VistaPerfilLongitudinal_MouseWheel_Zoom;
            PB_VistaPerfilLongitudinal.MouseMove += PB_VistaPerfilLongitudinal_MouseMove_Desplazamiento;
        }

        private void F_NervioEnPerfilLongitudinal_Paint(object sender, PaintEventArgs e)
        {
            PB_VistaPerfilLongitudinal.Invalidate();
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                Text = $"Vista en Perfil Longitudinal | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre}";
            }
        }

        private void PB_VistaPerfilLongitudinal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                CambiosConteMenuStrip();
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                float XI = 15f; float YI = 15f;
                float WidthPB = PB_VistaPerfilLongitudinal.Width - XI * 3;
                float HeightPB = (PB_VistaPerfilLongitudinal.Height - YI * 3);

                List<PointF> PuntosNoEscalados = new List<PointF>();

                //Puntos No Escalados - Nervio y Grid
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x => PuntosNoEscalados.AddRange(x.Vistas.Perfil_Original.Reales));
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Grids.ForEach(x => PuntosNoEscalados.AddRange(x.Recta_Real));

                //Crear Coordenadas Escaladas de Elementos

                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasLongitudinal_Elementos_Escalados_Original(PuntosNoEscalados, WidthPB, HeightPB, Dx, Dy, Zoom, XI);

                //Puntos No Escalados
                List<PointF> PuntosEscalados = new List<PointF>();
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.ForEach(x => PuntosEscalados.AddRange(x.Vistas.Perfil_Original.Escaladas));
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Grids.ForEach(x => { PuntosEscalados.AddRange(x.Recta_Escalada); PuntosEscalados.Add(x.PuntoBubble_Real); }) ;

                PointF PuntoInicialEscalado = new PointF(PuntosEscalados.Min(X => X.X), PuntosEscalados.Max(x => x.Y) + YI);
                PointF PuntoFinalEscalado = new PointF(PuntosEscalados.Max(X => X.X), PuntosEscalados.Max(x => x.Y) + YI);

                if (PuntoInicialEscalado.Y + YI > HeightPB)
                {
                    PuntoInicialEscalado.Y -= YI;
                    PuntoFinalEscalado.Y -= YI;
                }


                if (MostrarReglaToolStripMenuItem.Checked)
                {
                    cFunctionsProgram.DrawRegla(e.Graphics, PuntoInicialEscalado, PuntoFinalEscalado, cFunctionsProgram.LongitudesElementos(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos), Zoom);
                }
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Paint_Longitudinal_Elementos_Escalados_Original(e.Graphics, Zoom, PB_VistaPerfilLongitudinal.Height);
                F_Base.ActualizarVentanaF_VentanaDiseno(Zoom, Dx, Dy);
                F_Base.ActualizarVentanaF_MomentosNervio(Zoom, Dx, Dy);
                F_Base.ActualizarVentanaF_AreasMomentoNervio(Zoom, Dx, Dy);
                F_Base.ActualizarVentanaF_CortanteNervio(Zoom, Dx, Dy);
                F_Base.ActualizarVentanaF_AreaCortanteNervio(Zoom, Dx, Dy);
               
            }
        }

        private void PB_VistaPerfilLongitudinal_MouseDown(object sender, MouseEventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    IElemento ElementoSeleccionado = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Find(x => x.Vistas.IsSelectPlantaPerfilLongitudinal(e.Location));
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.FindAll(x => x != ElementoSeleccionado).ForEach(x => x.Vistas.SelectPerfilLongitudinal = false);
                    if (ElementoSeleccionado != null)
                    {
                        F_Base.F_Base_.VentanaEmergente(ref F_Base.F_ModificarSeccion);
                        F_Base.F_ModificarSeccion.ElementoSeleccionado = ElementoSeleccionado;
                    }
                    else { F_Base.F_ModificarSeccion.Close(); }

                    PB_VistaPerfilLongitudinal.Invalidate();

                }
            }
        }

        #region Metodos para Desplazamiento y ZOOM

        private void PB_VistaPerfilLongitudinal_MouseDown_RestablecerZoom(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks == 2)
            {
                Zoom = 1;
                Dx = 0; Dy = 0;
                PB_VistaPerfilLongitudinal.Invalidate();
            }
            if (e.Button == MouseButtons.Middle)
            {
                DesXButton = e.X;
                DesYButton = e.Y;
            }
        }

        private void PB_VistaPerfilLongitudinal_MouseWheel_Zoom(object sender, MouseEventArgs e)
        {
            int vueltas = e.Delta;
            if (vueltas > 0)
            {
                Zoom += 0.5f;
                PB_VistaPerfilLongitudinal.Invalidate();
            }
            else
            {
                if (Zoom > 1)
                {
                    Zoom -= 0.5f; 
                    PB_VistaPerfilLongitudinal.Invalidate();
                }
            }
        }

        private void PB_VistaPerfilLongitudinal_MouseMove_Desplazamiento(object sender, MouseEventArgs e)
        {
            Cursor CursorArraste = new Cursor(Properties.Resources.Arrastre16x16.Handle);
            Cursor CursorDefault = Cursors.Default;
            Cursor cursor = CursorDefault;
            if (e.Button == MouseButtons.Middle)
            {
                float DesplazX = (Dx) + (e.X - DesXButton);
                float DesplazY = (Dy) + (e.Y - DesYButton);
                Dx = DesplazX;
                Dy = DesplazY;
                DesXButton = e.X;
                DesYButton = e.Y;
                cursor = CursorArraste;
                PB_VistaPerfilLongitudinal.Invalidate();
            }
            PB_VistaPerfilLongitudinal.Cursor = cursor;
        }

        #endregion Metodos para Desplazamiento y ZOOM

        private void F_NervioEnPerfilLongitudinal_Resize(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinal.Invalidate();
        }

        private void MostrarReglaToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            PB_VistaPerfilLongitudinal.Invalidate();
        }

        private void CambiosConteMenuStrip()
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                TLSM_ApoyoInicio.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.First() is cSubTramo;

                TLSM_ApoyoFinal.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Last() is cSubTramo;
            }
        }


        private void editarEjesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                F_ModificacionEjes f_ModificacionEjes = new F_ModificacionEjes();
                f_ModificacionEjes.Grids = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Grids;
                f_ModificacionEjes.ShowDialog();
            }
        }

        private void TLSM_ApoyoInicio_Click(object sender, EventArgs e)
        {
            F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearApoyosAExtremos(true);
            PB_VistaPerfilLongitudinal.Invalidate();
        }

        private void TLSM_ApoyoFinal_Click(object sender, EventArgs e)
        {
            F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearApoyosAExtremos(ApoyoFinal:true);
            PB_VistaPerfilLongitudinal.Invalidate();
        }
    }
}