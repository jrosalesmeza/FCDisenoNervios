using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios
{
    public partial class F_Base : Form
    {
        private bool ActivarVentanaEmergenteGuardarCambios = false;
        private static cUndoRedo<cProyecto> UndoRedo = new cUndoRedo<cProyecto>();
        public static Form F_Base_;

        #region Ventanas Emergentes

        private F_NuevoProyecto F_NuevoProyecto = new F_NuevoProyecto();
        public static F_EnumeracionPortico F_EnumeracionPortico;

        #endregion Ventanas Emergentes



        #region Proyecto
        public static cProyecto Proyecto;

        #endregion Proyecto

        #region Funciones Basicas

        private void NuevoProyecto_Function()
        {
            F_NuevoProyecto.ShowDialog();
        }

        private void AbrirProyecto_Function(bool ArchivoExterno = false, string Ruta = "")
        {
            if (ArchivoExterno == false)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = $"{cFunctionsProgram.NombrePrograma} |*{cFunctionsProgram.Ext}";
                openFileDialog.Title = "Abrir Proyecto";
                openFileDialog.ShowDialog();
                Ruta = openFileDialog.FileName;
            }

            if (Ruta != "")
            {
                if (Proyecto != null)
                {
                    if (ActivarVentanaEmergenteGuardarCambios)
                    {
                        DialogResult BoxMensa = MessageBox.Show("¿Desea guardar cambios en el Proyecto: " + Proyecto.Nombre + "?", cFunctionsProgram.Empresa, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (BoxMensa == DialogResult.Yes)
                        {
                            GuardarProyecto_Function();
                        }
                    }
                }

                cFunctionsProgram.Deserealizar(Ruta, ref Proyecto);
                if (Proyecto != null)
                {
                    UndoRedo.LimpiarEstadosCtrlZyCtrlY();
                }
            }
        }

        private void GuardarProyecto_Function()
        {
            if (Proyecto.Ruta != "")
            {
                cFunctionsProgram.Serializar(Proyecto.Ruta, Proyecto);
                UndoRedo.LimpiarEstados();
            }
            else
            {
                GuardarComoProyecto_Function();
            }
        }

        private void GuardarComoProyecto_Function()
        {
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = $"{cFunctionsProgram.NombrePrograma} |*{cFunctionsProgram.Ext}";
            SaveFileDialog.Title = $"Guardar Proyecto | {Proyecto.Nombre}";
            SaveFileDialog.FileName = Proyecto.Nombre;
            if (Proyecto.Ruta != "")
            {
                SaveFileDialog.InitialDirectory = Proyecto.Ruta;
            }
            SaveFileDialog.ShowDialog();
            if (SaveFileDialog.FileName != "")
            {
                Proyecto.Ruta = SaveFileDialog.FileName;
                Proyecto.Nombre = Path.GetFileName(SaveFileDialog.FileName).Replace(cFunctionsProgram.Ext, "");
                cFunctionsProgram.Serializar(SaveFileDialog.FileName, Proyecto);
                UndoRedo.LimpiarEstados();
            }
        }

        public static void Deshacer_Function()
        {
            Proyecto = UndoRedo.Deshacer();
            ActualizarTodosLasVentanas();
        }

        public static void Rehacer_Function()
        {
            Proyecto = UndoRedo.Rehacer();
            ActualizarTodosLasVentanas();
        }

        public static void EnviarEstado(cProyecto Proyecto)
        {
            UndoRedo.EnviarEstado(Proyecto);
        }

        public static void LimpiarMemoria()
        {
            UndoRedo.LimpiarEstadosCtrlZyCtrlY();
        }

        private static void ActualizarTodosLasVentanas()
        {
            F_EnumeracionPortico.Invalidate();
        }

        #endregion Funciones Basicas

        public F_Base()
        {
            InitializeComponent();
            T_Timer.Start();
            SetStyle(ControlStyles.ResizeRedraw, true);
            ST_Base.SizingGrip = false;
            cFunctionsProgram.Notificador += CFunctionsProgram_Notificador;
            cFunctionsProgram.EventoVentanaEmergente += CFunctionsProgram_EventoVentanaEmergente;

            F_Base_ = this;
        }

        private void CFunctionsProgram_EventoVentanaEmergente(string Alert, MessageBoxIcon Icono)
        {
            MessageBox.Show(Alert, cFunctionsProgram.Empresa, MessageBoxButtons.OK, Icono);
        }

        private void CFunctionsProgram_Notificador(string Alert)
        {
            LB_Notificador.Text = Alert;
            LB_Notificador.Invalidate();
        }

        private void T_Timer_Tick(object sender, EventArgs e)
        {
            CambiosTimer_1();
            CambiosTimer_2_Proyecto();
        }

        private void CambiosTimer_2_Proyecto()
        {
            if (Proyecto != null)
            {
                Text = $"{cFunctionsProgram.NombrePrograma} | {Proyecto.Nombre}";
                LB_NombreProyecto.Text = Proyecto.Nombre;
                TSB_Undo.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                TSB_Redo.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                deshacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                rehacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                ActivarVentanaEmergenteGuardarCambios = UndoRedo.ObtenerEstadoEstados();
                if (Proyecto.Edificio != null && Proyecto.Edificio.PisoSelect != null && Proyecto.Edificio.PisoSelect.Nervios != null)
                {
                    if (Proyecto.Edificio.PisoSelect.NervioSelect != null)
                    {
                        esquemaToolStripMenuItem.Enabled = true;
                    }
                    else { esquemaToolStripMenuItem.Enabled = false; }
                }
                else { esquemaToolStripMenuItem.Enabled = false; }
                CambiosTimer_3_F_EnumeracionPortico_Proyecto();
                BloqueoDesbloqueoBotones(true);
            }

            else
            {
                Text = cFunctionsProgram.NombrePrograma;
                TSB_Undo.Enabled = false;
                TSB_Redo.Enabled = false;
                deshacerToolStripMenuItem.Enabled = false;
                rehacerToolStripMenuItem.Enabled = false;
                ActivarVentanaEmergenteGuardarCambios = false;
                esquemaToolStripMenuItem.Enabled = false;
                BloqueoDesbloqueoBotones(false);
            }
        }

        private void CambiosTimer_3_F_EnumeracionPortico_Proyecto()
        {
            if (F_EnumeracionPortico != null)
            {
                F_EnumeracionPortico.deshacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                F_EnumeracionPortico.rehacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlY();
            }
        }

        private void BloqueoDesbloqueoBotones(bool Bloqueo_Desbloqueo)
        {
            guardarToolStripMenuItem.Enabled = Bloqueo_Desbloqueo;
            gurdarComoToolStripMenuItem.Enabled = Bloqueo_Desbloqueo;
            TSB_Guardar.Enabled = Bloqueo_Desbloqueo;
            TSB_GuardarComo.Enabled = Bloqueo_Desbloqueo;
            enumeraciónDeElementosToolStripMenuItem.Enabled = Bloqueo_Desbloqueo;
        }

        private void VentanaEnumeracionElementos()
        {
            if (F_EnumeracionPortico != null)
            {
                F_EnumeracionPortico.ShowDialog();
            }
            else
            {
                F_EnumeracionPortico = new F_EnumeracionPortico();
                F_EnumeracionPortico.ShowDialog();
            }
        }

        #region Dimensionar Formulario

        private int tolerance = 16;
        private int tolerance2W = 2;
        private int tolerance2H = 2;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, ClientRectangle.Width - tolerance2W, this.ClientRectangle.Height - tolerance2H));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.ST_Base.Region = region;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        #endregion Dimensionar Formulario

        #region Mover ,Maximizar, Cerrar y Restaurar Ventana - Eventos Clicks

        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BT_MaxRest_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void BT_Minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void CambiosTimer_1()
        {
            if (WindowState == FormWindowState.Normal)
            {
                BT_MaxRest.Image = Properties.Resources.Maximizar14X11;
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                BT_MaxRest.Image = Properties.Resources.Restaurar14x11;
            }
        }

        private void DobleClickMaximaze(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    WindowState = FormWindowState.Maximized;
                }
                else if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
            }
        }

        private void MS_BarraPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                DobleClickMaximaze(e);
            }
        }

        private void P_Menu_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                DobleClickMaximaze(e);
            }
        }

        #endregion Mover ,Maximizar, Cerrar y Restaurar Ventana - Eventos Clicks

        #region Eventos de MenuStrip y ToolStrip

        private void TSB_Nuevo_Click(object sender, EventArgs e)
        {
            NuevoProyecto_Function();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuevoProyecto_Function();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirProyecto_Function();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarProyecto_Function();
        }

        private void gurdarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarComoProyecto_Function();
        }

        private void TSB_Abrir_Click(object sender, EventArgs e)
        {
            AbrirProyecto_Function();
        }

        private void TSB_Guardar_Click(object sender, EventArgs e)
        {
            GuardarProyecto_Function();
        }

        private void TSB_GuardarComo_Click(object sender, EventArgs e)
        {
            GuardarComoProyecto_Function();
        }

        private void TSB_Undo_Click(object sender, EventArgs e)
        {
            Deshacer_Function();
        }

        private void TSB_Redo_Click(object sender, EventArgs e)
        {
            Rehacer_Function();
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deshacer_Function();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rehacer_Function();
        }

        #endregion Eventos de MenuStrip y ToolStrip

        private void enumeraciónDeElementosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaEnumeracionElementos();
        }

        private void F_Base_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Proyecto != null)
            {
                if (ActivarVentanaEmergenteGuardarCambios)
                {
                    DialogResult BoxMessage = MessageBox.Show("¿Desea guardar cambios en el Proyecto: " + Proyecto.Nombre + "?", cFunctionsProgram.Empresa, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (BoxMessage == DialogResult.Yes)
                    {
                        GuardarProyecto_Function();
                    }
                    else if (BoxMessage == DialogResult.No)
                    {
                        Application.Exit();
                    }
                    else if (BoxMessage == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }

                }



            }
        }
    }
}