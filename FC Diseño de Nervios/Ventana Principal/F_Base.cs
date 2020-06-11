using B_FC_DiseñoVigas;
using FC_BFunctionsAutoCAD;
using FC_Diseño_de_Nervios.Manipulación_Proyecto;
using FC_Diseño_de_Nervios.Manipulación_Proyecto.Ventanas_Interactivas.Ventanas_Perfil_Longitudinal;
using FC_Diseño_de_Nervios.Ventana_Principal;
using FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FC_Diseño_de_Nervios
{
    public partial class F_Base : Form
    {
        #region Propiedades Necesarias Formulario Base
        private bool ActivarVentanaEmergenteGuardarCambios = false;
        private  DeserializeDockContent DeserializeDockContent;
        private static cUndoRedo<cProyecto> UndoRedo = new cUndoRedo<cProyecto>();
        private static cUndoRedo<cNervio> UndoRedoNervio = new cUndoRedo<cNervio>();
        public static F_Base F_Base_;


        private string Ruta_ConfigVentanasDefault = Path.Combine(Application.StartupPath,cFunctionsProgram.Ext_ConfigVentana);
        private string Ruta_ConfigVentanasUsuario = Path.Combine(cFunctionsProgram.Ruta_CarpetaLocal, cFunctionsProgram.Ext_ConfigVentana);
        #endregion

        #region Ventanas Emergentes

        private F_NuevoProyecto F_NuevoProyecto = new F_NuevoProyecto();
        public static F_EnumeracionPortico F_EnumeracionPortico = new F_EnumeracionPortico();
        public static F_ModificarSeccion F_ModificarSeccion = new F_ModificarSeccion();
        public static F_SeleccionBarras F_SeleccionBarras = new F_SeleccionBarras();
        public static F_SelectCombinaciones F_SelectCombinaciones = new F_SelectCombinaciones();
        public static F_PropiedadesProyecto F_PropiedadesProyecto = new F_PropiedadesProyecto();
        public static F_Informe F_Informe;
        #endregion Ventanas Emergentes

        #region Ventanas Acopladas
        public static F_SelectNervio F_SelectNervio;
        public static F_NervioEnPerfilLongitudinal F_NervioEnPerfilLongitudinal;
        public static F_MomentosNervio F_MomentosNervio;
        public static F_AreasMomentoNervio F_AreasMomentoNervio;
        public static F_VentanaDiseno F_VentanaDiseno;
        public static F_AreasCortanteNervio F_AreasCortanteNervio;
        public static F_CortanteNervio F_CortanteNervio;
        #endregion

        public F_Base()
        {
            InitializeComponent();
            InstanciamientodeVentanasAcopladas();
            CrearCarpetaLocalConArchivos();
            cFunctionsProgram.Notificador += CFunctionsProgram_Notificador;
            DiseñoYRevisonVigasRectangulares.Notificador += NotificadorDeCalculos;
            cFunctionsProgram.EventoVentanaEmergente += CFunctionsProgram_EventoVentanaEmergente;
            FunctionsAutoCAD.NotificadorErrores += FunctionsAutoCAD_NotificadorErrores;
            new cDiccionarios();
            DeserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            T_Timer.Start();

            SetStyle(ControlStyles.ResizeRedraw, true);
            ST_Base.SizingGrip = false;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;


            F_Base_ = this;
        }

 
        #region Proyecto
        public static cProyecto Proyecto;

        #endregion Proyecto

        #region Funciones Basicas

        #region AcopleDeVentanas y MostrarVetanasEmergente *** 
        public void AcoplarVentana(ref F_SelectNervio Dock)
        {
            if (!Dock.Created) { Dock = new F_SelectNervio(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_NervioEnPerfilLongitudinal Dock)
        {
            if (!Dock.Created) { Dock = new F_NervioEnPerfilLongitudinal(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_MomentosNervio Dock)
        {
            if (!Dock.Created) { Dock = new F_MomentosNervio(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_AreasMomentoNervio Dock)
        {
            if (!Dock.Created) { Dock = new F_AreasMomentoNervio(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_VentanaDiseno Dock)
        {
            if (!Dock.Created) { Dock = new F_VentanaDiseno(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_CortanteNervio Dock)
        {
            if (!Dock.Created) { Dock = new F_CortanteNervio(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_AreasCortanteNervio Dock)
        {
            if (!Dock.Created) { Dock = new F_AreasCortanteNervio(); }
            Dock.Show(DP_ContenedorPrincipal);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void VentanaEmergente(ref F_ModificarSeccion Form)
        {
            if (!Form.Created) { Form = new F_ModificarSeccion(); }
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.Focus();
            Form.Show();
        }
        public void VentanaEmergente(ref F_SeleccionBarras Form)
        {
            if (!Form.Created) { Form = new F_SeleccionBarras(); }
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.Focus();
            Form.Show();
        }

        private void VentanaEmergente(ref F_EnumeracionPortico Form)
        {
            if (!Form.Created) { Form = new F_EnumeracionPortico(); }
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }

        private void VentanaEmergente(ref F_SelectCombinaciones Form)
        {
            if (!Form.Created) { Form = new F_SelectCombinaciones(); }
            Form.NervioSelect = Proyecto.Edificio.PisoSelect.NervioSelect;
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }
        private void VentanaEmergente(ref F_PropiedadesProyecto Form)
        {
            if (!Form.Created) { Form = new F_PropiedadesProyecto(); }
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }
        #endregion

        #region Funciones para Abrir, Crear, Guardar Proyecto 

        private void VerificarGuardadodeProyecto()
        {
            if (Proyecto != null)
            {
                if (ActivarVentanaEmergenteGuardarCambios)
                {
                    DialogResult BoxMensa = MessageBox.Show("¿Desea guardar cambios en el Proyecto: " + Proyecto.Nombre + "?", cFunctionsProgram.Empresa, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (BoxMensa == DialogResult.Yes)
                    {
                        GuardarProyecto_Function();
                    }
                    else
                    {
                        UndoRedo.LimpiarEstados();
                    }
                }
            }
        }
        private void NuevoProyecto_Function()
        {
            VerificarGuardadodeProyecto();
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
                VerificarGuardadodeProyecto();
                cFunctionsProgram.Deserealizar(Ruta, ref Proyecto);
                CerrarTodasLasVentanas();
                if (Proyecto != null)
                {
                    UndoRedo.LimpiarEstadosCtrlZyCtrlY();
                    CargarVentanasXML();
                    Proyecto.Nombre = Path.GetFileName(Ruta).Replace(cFunctionsProgram.Ext, "");
                    
                }
            }
        }

        public void CargarVentanasXML()
        {
            CrearCarpetaLocalConArchivos();
            if (DP_ContenedorPrincipal.Contents.Count == 0)
            {
                DP_ContenedorPrincipal.LoadFromXml(Ruta_ConfigVentanasUsuario, DeserializeDockContent);
                cFunctionsProgram.CambiarSkins(DP_ContenedorPrincipal);
            }
        }
        public void CerrarTodasLasVentanas()
        {
            DP_ContenedorPrincipal.Contents.ToList().ForEach(x => x.DockHandler.Close());
            InstanciamientodeVentanasAcopladas();
        }

        private void CrearCarpetaLocalConArchivos()
        {
            if (!Directory.Exists(cFunctionsProgram.Ruta_CarpetaLocal))
                Directory.CreateDirectory(cFunctionsProgram.Ruta_CarpetaLocal);
            if (!File.Exists(Ruta_ConfigVentanasUsuario))
                File.Copy(Ruta_ConfigVentanasDefault, Ruta_ConfigVentanasUsuario);
        }

        private void GuardarProyecto_Function()
        {
            if (Proyecto.Ruta != "")
            {
                cFunctionsProgram.Serializar(Proyecto.Ruta, Proyecto);
                UndoRedo.LimpiarEstados();
                UndoRedoNervio.LimpiarEstados();
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
            if (SaveFileDialog.FileName != "" && SaveFileDialog.FileName!= Proyecto.Nombre)
            {
                Proyecto.Ruta = SaveFileDialog.FileName;
                Proyecto.Nombre = Path.GetFileName(SaveFileDialog.FileName).Replace(cFunctionsProgram.Ext, "");
                cFunctionsProgram.Serializar(SaveFileDialog.FileName, Proyecto);
                UndoRedo.LimpiarEstados();
                UndoRedoNervio.LimpiarEstados();
            }
        }
        public static void FuncionDiseñarNervios(List<cNervio> Nervios)
        {
            cFunctionsProgram.DiseñarNervios(Nervios);
            if (F_Informe != null && F_Informe.Created)
                try { F_Informe.Close(); } catch { }
            F_Informe = new F_Informe(Nervios);
            F_Informe.Show();
        }
        public static void FuncionVentanaEditarEJesGlobales()
        {
            F_ModificarEjesGlobales F_ModificarEjes = new F_ModificarEjesGlobales();
            F_ModificarEjes.ShowDialog();
        }
        public static void FuncionReasignarEjesaNervios()
        {
            Proyecto.Edificio.PisoSelect.Nervios.ForEach(Nervio => {
                cFunctionsProgram.ReasingarEjesaNervios(Nervio, Proyecto.Edificio.Lista_Grids);
            });
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
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
        #endregion

        #region Deshacer, rehacer y enviar estado completo del proyecto
        public static void Deshacer_Function()
        {
            cProyecto PoryectoAux = UndoRedo.Deshacer(Proyecto);
            if (PoryectoAux != null) { Proyecto = PoryectoAux; }
            ActualizarTodosLasVentanas();
        }

        public static void Rehacer_Function()
        {
            cProyecto PoryectoAux = UndoRedo.Rehacer(Proyecto);
            if (PoryectoAux != null) { Proyecto = PoryectoAux; }
            ActualizarTodosLasVentanas();
        }

  

        public static void LimpiarMemoria()
        {
            UndoRedo.LimpiarEstadosCtrlZyCtrlY();
        }

        #endregion

        #region Deshacer, rehacer y enviar estado del Nervio
        public static void Deshacer_Function_Nervio()
        {
            cNervio NervioAux = UndoRedoNervio.Deshacer(Proyecto.Edificio.PisoSelect.NervioSelect);
            if (NervioAux != null) { Proyecto.Edificio.PisoSelect.NervioSelect = NervioAux; }
            ActualizarTodosLasVentanas();
        }

        public static void Rehacer_Function_Nervio()
        {
            cNervio NervioAux = UndoRedoNervio.Rehacer(Proyecto.Edificio.PisoSelect.NervioSelect);
            if (NervioAux != null) { Proyecto.Edificio.PisoSelect.NervioSelect = NervioAux; }
            ActualizarTodosLasVentanas();
        }

        public static void EnviarEstadoVacio()
        {
            UndoRedoNervio.EnviarEstadoVacio();
        }
        public static void EnviarEstado(cProyecto Proyecto)
        {
            UndoRedo.EnviarEstado(Proyecto);
        }
        public static void EnviarEstado_Nervio(cNervio Nervio)
        {
            UndoRedoNervio.EnviarEstado(Nervio);
        }

        public static void LimpiarMemoria_Nervio()
        {
            UndoRedoNervio.LimpiarEstadosCtrlZyCtrlY();
        }

        #endregion

        #region Actualizar Ventanas
        public static void ActualizarVentanaF_NervioEnPerfilLongitudinal()
        {
            F_NervioEnPerfilLongitudinal.Invalidate();
        }
        public static void ActualizarVentanaF_MomentosNervio(float Zoom,float Dx,float Dy)
        {
            F_MomentosNervio.Zoom = Zoom; F_MomentosNervio.Dx = Dx; F_MomentosNervio.Dy = Dy;
            F_MomentosNervio.Invalidate();
        }
        public static void ActualizarVentanaF_AreasMomentoNervio(float Zoom, float Dx, float Dy)
        {
            F_AreasMomentoNervio.Zoom = Zoom; F_AreasMomentoNervio.Dx = Dx; F_AreasMomentoNervio.Dy = Dy;
            F_AreasMomentoNervio.Invalidate();
        }
        public static void ActualizarVentanaF_VentanaDiseno(float Zoom,float Dx,float Dy)
        {
            F_VentanaDiseno.Zoom = Zoom; F_VentanaDiseno.Dx = Dx; F_VentanaDiseno.Dy = Dy;
            F_VentanaDiseno.Invalidate();
        }
        public static void ActualizarVentanaF_CortanteNervio(float Zoom, float Dx, float Dy)
        {
            F_CortanteNervio.Zoom = Zoom; F_CortanteNervio.Dx = Dx; F_CortanteNervio.Dy = Dy;
            F_CortanteNervio.Invalidate();
        }
        public static void ActualizarVentanaF_AreaCortanteNervio(float Zoom, float Dx, float Dy)
        {
            F_AreasCortanteNervio.Zoom = Zoom; F_AreasCortanteNervio.Dx = Dx; F_AreasCortanteNervio.Dy = Dy;
            F_AreasCortanteNervio.Invalidate();
        }
        public static void ActualizarVentanaF_AreaCortanteNervio()
        {
            F_AreasCortanteNervio.Invalidate();
        }
        public static void ActualizarVentanaF_VentanaDiseno()
        {
            F_VentanaDiseno.Invalidate();
        }
        public static void ActualizarTodosLasVentanas()
        {
            F_EnumeracionPortico.Invalidate();
            F_SelectNervio.Invalidate();
            F_NervioEnPerfilLongitudinal.Invalidate();

        }
        public static void ActualizarVentanaF_SelectNervio()
        {
            F_SelectNervio.Invalidate();
        }
        #endregion


        #endregion Funciones Basicas


        #region Noficadores
        private void NotificadorDeCalculos(string Alerta)
        {
            
        }
        private void FunctionsAutoCAD_NotificadorErrores(string Alert)
        {
            MessageBox.Show(Alert, cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void CFunctionsProgram_EventoVentanaEmergente(string Alert, MessageBoxIcon Icono)
        {
            MessageBox.Show(Alert, cFunctionsProgram.Empresa, MessageBoxButtons.OK, Icono);
        }
        private void CFunctionsProgram_Notificador(string Alert)
        {
            LB_Notificador.Text = Alert; 
            LB_Notificador.Invalidate();
            if(Alert.Contains("Cargando")| Alert.Contains("Guardando")| Alert.Contains("Diseñando"))
            {
                Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }
            Application.DoEvents();
        }

        #endregion

        #region Timer
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
                ActivarDesacitvarBotonesDeRehaceryDeshacer();
                if (Proyecto.Edificio != null && Proyecto.Edificio.PisoSelect != null && Proyecto.Edificio.PisoSelect.Nervios != null)
                {
                    pesoTotalToolStripMenuItem.Enabled = true;
                    selecciónDeNerviosToolStripMenuItem.Enabled = true;
                    TLSB_ModificarEjes.Enabled = true;
                    TSB_ReasignarEjesNervios.Enabled = true;
                    if (Proyecto.Edificio.PisoSelect.NervioSelect != null)
                    {
                        ActivarDesactivarBotonesNervioSelect(true);
                        TLSN_ApoyoInicio.Enabled = Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.First() is cSubTramo;
                        TLSN_ApoyoFinal.Enabled = Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Last() is cSubTramo;
                    }
                    else
                    {
                        ActivarDesactivarBotonesNervioSelect(false);
                    }
                }
                else
                {
                    TSB_ReasignarEjesNervios.Enabled = false;
                    TLSB_ModificarEjes.Enabled = false;
                    pesoTotalToolStripMenuItem.Enabled = false;
                    ActivarDesactivarBotonesNervioSelect(false);
                }
                CambiosTimer_3_F_EnumeracionPortico_Proyecto();
                BloqueoDesbloqueoBotones(true);
            }

            else
            {
                TSB_ReasignarEjesNervios.Enabled = false;
                TLSB_ModificarEjes.Enabled = false;
                Text = cFunctionsProgram.NombrePrograma;
                ActivarVentanaEmergenteGuardarCambios = false;
                pesoTotalToolStripMenuItem.Enabled = false;
                ActivarDesactivarBotonesNervioSelect(false);
                BloqueoDesbloqueoBotones(false);
            }
        }


        private void ActivarDesactivarBotonesNervioSelect(bool Bool)
        {

            /////------------------PESTAÑAS----------------------
            //Editar
            combinacionesToolStripMenuItem.Enabled = Bool;
            tendenciasToolStripMenuItem.Enabled = Bool;

            //Ver
            selecciónDeNerviosToolStripMenuItem.Enabled = Bool;
            esquemaToolStripMenuItem.Enabled = Bool;
            diagramasToolStripMenuItem.Enabled = Bool;

            //Diseño
            diseñarToolStripMenuItem.Enabled = Bool;
            autoCADToolStripMenuItem.Enabled = Bool;

            /////------------------BOTONES--------------------------------------

            TLSB_SeleccionNervios.Enabled = Bool;
            TLS_Refuerzo.Enabled = Bool;
            TLSB_Momentos.Enabled = Bool;
            TLSB_AreasMomentos.Enabled = Bool;
            TLSB_AreasCortante.Enabled = Bool;
            TLB_Cortante.Enabled = Bool;
            TLB_Tendencias.Enabled = Bool;
            TSB_SelectCombinaciones.Enabled = Bool;
            TSB_DiseñarNervio.Enabled = Bool;
            TSB_DiseñarNervios.Enabled = Bool;
            TLSB_GraficarNervioActual.Enabled = Bool;
            TLSB_GraficarNerviosAutoCAD.Enabled = Bool;
            TLSB_GraficarAllNervios.Enabled = Bool;
            TSB_DiseñarAllNervio.Enabled = Bool;

            TLSB_AgregaApoyo.Enabled = Bool;
        }
        private void ActivarDesacitvarBotonesDeRehaceryDeshacer()
        {
            if (F_EnumeracionPortico != null)
            {
                if (F_EnumeracionPortico.Visible)
                {
                    ActivarVentanaEmergenteGuardarCambios = UndoRedo.ObtenerEstadoEstados();
                    TSB_Undo.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                    TSB_Redo.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                    deshacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                    rehacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                }
                else
                {
                    ActivarVentanaEmergenteGuardarCambios = UndoRedoNervio.ObtenerEstadoEstados();
                    TSB_Undo.Enabled = UndoRedoNervio.ObtenerEstadoCtrlZ();
                    TSB_Redo.Enabled = UndoRedoNervio.ObtenerEstadoCtrlY();
                    deshacerToolStripMenuItem.Enabled = UndoRedoNervio.ObtenerEstadoCtrlZ();
                    rehacerToolStripMenuItem.Enabled = UndoRedoNervio.ObtenerEstadoCtrlY();
                }
            }
        }
        private void CambiosTimer_3_F_EnumeracionPortico_Proyecto()
        {
            if (F_EnumeracionPortico != null)
            {
                F_EnumeracionPortico.deshacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                F_EnumeracionPortico.rehacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                F_EnumeracionPortico.TLSB_Rehacer.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                F_EnumeracionPortico.TLSB_Deshacer.Enabled = UndoRedo.ObtenerEstadoCtrlY();
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
        #endregion



        #region Redimensionar Formulario

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
            ST_Base.Region = region;
            Invalidate();
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
            if (F_EnumeracionPortico.Visible == true) { Deshacer_Function(); } else
            {
                Deshacer_Function_Nervio();
            }

        }

        private void TSB_Redo_Click(object sender, EventArgs e)
        {
            if (F_EnumeracionPortico.Visible == true) { Rehacer_Function(); }
            else
            {
                Rehacer_Function_Nervio();
            }
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F_EnumeracionPortico.Visible == true) { Deshacer_Function(); }
            else
            {
                Deshacer_Function_Nervio();
            }
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (F_EnumeracionPortico.Visible == true) { Rehacer_Function(); }
            else
            {
                Rehacer_Function_Nervio();
            }
        }

        private void enumeraciónDeElementosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaEmergente(ref F_EnumeracionPortico);
        }
        private void selecciónDeNerviosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_SelectNervio);
        }
        private void geometríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_NervioEnPerfilLongitudinal);
        }
        private void refuerzoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_VentanaDiseno);
        }

        private void diagramaDeMomentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_MomentosNervio);
        }

        private void diagramaDeÁreasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_AreasMomentoNervio);
        }

        private void diagramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_CortanteNervio);
        }

        private void diagramaDeCortanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcoplarVentana(ref F_AreasCortanteNervio);
        }
        private void TSB_SelectCombinaciones_Click(object sender, EventArgs e)
        {
            VentanaEmergente(ref F_SelectCombinaciones);
        }
        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentanaEmergente(ref F_PropiedadesProyecto);
        }
        private void TSB_DiseñarNervio_Click(object sender, EventArgs e)
        {
            List<cNervio> Nervios = new List<cNervio>() { Proyecto.Edificio.PisoSelect.NervioSelect };
            FuncionDiseñarNervios(Nervios);
            ActualizarVentanaF_VentanaDiseno();
            ActualizarVentanaF_AreaCortanteNervio();
        }
        private void pesoTotalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_PesoTotal F_PesoTotal = new F_PesoTotal();
            F_PesoTotal.ShowDialog();
        }

        private void TSB_DiseñarNervios_Click(object sender, EventArgs e)
        {

        }

        private void TSB_DiseñarAllNervio_Click(object sender, EventArgs e)
        {
            FuncionDiseñarNervios(Proyecto.Edificio.PisoSelect.Nervios);
            ActualizarTodosLasVentanas();
        }

        private void TLSB_GraficarNervioActual_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.GraficarNervios(new List<cNervio>() { Proyecto.Edificio.PisoSelect.NervioSelect});
        }

        private void TLSB_GraficarNerviosAutoCAD_Click(object sender, EventArgs e)
        {

        }

        private void TLSB_GraficarAllNervios_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.GraficarNervios(Proyecto.Edificio.PisoSelect.Nervios);
        }
        private void acercaDeDiseñoDeNerviosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_AcercaDe F_AcercaDe = new F_AcercaDe();
            F_AcercaDe.ShowDialog();
        }

        private void tendenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Tendencia F_Tendencia = new F_Tendencia();
            F_Tendencia.ShowDialog();
        }

        private void BT_ModificarEjes_Click(object sender, EventArgs e)
        {
            FuncionVentanaEditarEJesGlobales();
        }
        private void TSB_ReasignarEjesNervios_Click(object sender, EventArgs e)
        {
            FuncionReasignarEjesaNervios();
        }
        private void TLSN_ApoyoInicio_Click(object sender, EventArgs e)
        {
            EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
            Proyecto.Edificio.PisoSelect.NervioSelect.CrearApoyosAExtremos(true);
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }

        private void TLSN_ApoyoFinal_Click(object sender, EventArgs e)
        {
            EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
            Proyecto.Edificio.PisoSelect.NervioSelect.CrearApoyosAExtremos(ApoyoFinal: true);
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }
        #endregion Eventos de MenuStrip y ToolStrip






        private IDockContent GetContentFromPersistString(string persistString)
        {
            DockContent dockContent;
            if (persistString == typeof(F_SelectNervio).ToString())
                dockContent= F_SelectNervio;
            else if (persistString == typeof(F_NervioEnPerfilLongitudinal).ToString())
                dockContent= F_NervioEnPerfilLongitudinal;
            else if (persistString == typeof(F_MomentosNervio).ToString())
                dockContent= F_MomentosNervio;
            else if (persistString == typeof(F_AreasMomentoNervio).ToString())
                dockContent= F_AreasMomentoNervio;
            else if (persistString == typeof(F_VentanaDiseno).ToString())
                dockContent= F_VentanaDiseno;
            else if (persistString == typeof(F_CortanteNervio).ToString())
                dockContent = F_CortanteNervio;
            else if (persistString == typeof(F_AreasCortanteNervio).ToString())
                dockContent = F_AreasCortanteNervio;
            else
            {
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;
                if (parsedStrings[0] != typeof(DockContent).ToString())
                    return null;

                dockContent = new F_SelectNervio();
                if (parsedStrings[2] != string.Empty)
                    dockContent.Text = parsedStrings[2];

                return dockContent;
            }
            return dockContent;
        }


        private void InstanciamientodeVentanasAcopladas()
        {
            F_SelectNervio = new F_SelectNervio();
            F_NervioEnPerfilLongitudinal = new F_NervioEnPerfilLongitudinal();
            F_MomentosNervio = new F_MomentosNervio();
            F_AreasMomentoNervio = new F_AreasMomentoNervio();
            F_VentanaDiseno = new F_VentanaDiseno();
            F_CortanteNervio = new F_CortanteNervio();
            F_AreasCortanteNervio = new F_AreasCortanteNervio();

        }

        private void F_Base_Load(object sender, EventArgs e)
        {
            string FicheroExterno = Environment.CommandLine;
            if (FicheroExterno.Contains(cFunctionsProgram.Ext))
            {
                AbrirProyecto_Function(true, FicheroExterno.Split(new char[] { '"' })[3]);
            }
        }

        private void similitudDeNerviosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_SimilitudNervios F_SimilitudNervios = new F_SimilitudNervios(Proyecto.Edificio.PisoSelect.Nervios);
            F_SimilitudNervios.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            cPDF.CrearPDF(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Prueba.pdf"), new List<cNervio>() { Proyecto.Edificio.PisoSelect.NervioSelect});
        }
    }
}