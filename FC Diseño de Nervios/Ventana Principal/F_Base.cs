using B_FC_DiseñoVigas;
using FC_BFunctionsAutoCAD;
using FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas;
using FC_Diseño_de_Nervios.Manipulación_Proyecto;
using FC_Diseño_de_Nervios.Manipulación_Proyecto.Ventanas_Interactivas.Ventanas_Perfil_Longitudinal;
using FC_Diseño_de_Nervios.Programa;
using FC_Diseño_de_Nervios.Ventana_Principal;
using FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes;
using FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes.Similutud_de_Nervios;
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


        private static string Ruta_ConfigVentanasDefault = Path.Combine(Application.StartupPath,cFunctionsProgram.Ext_ConfigVentana);
        private static string Ruta_ConfigVentanasUsuario = Path.Combine(cFunctionsProgram.Ruta_CarpetaLocal, cFunctionsProgram.Ext_ConfigVentana);
        private static string Ruta_MemoriaCalculo = Path.Combine(cFunctionsProgram.Ruta_CarpetaLocal, cFunctionsProgram.Ext_MemoriaCalculos);
        private static string Ruta_ConfiVentanasSelectNervioDefault = Path.Combine(Application.StartupPath, cFunctionsProgram.Ext_ConfigVentanaSelectNervios);
        public static string Ruta_ConfiVentanasSelectNervioUsuario = Path.Combine(cFunctionsProgram.Ruta_CarpetaLocal, cFunctionsProgram.Ext_ConfigVentanaSelectNervios);
        #endregion

        #region Ventanas Emergentes

        private F_NuevoProyecto F_NuevoProyecto = new F_NuevoProyecto();
        public static F_EnumeracionPortico F_EnumeracionPortico = new F_EnumeracionPortico();
        public static F_ModificarSeccion F_ModificarSeccion = new F_ModificarSeccion();
        public static F_SeleccionBarras F_SeleccionBarras = new F_SeleccionBarras();
        public static F_SelectCombinaciones F_SelectCombinaciones = new F_SelectCombinaciones();
        public static F_PropiedadesProyecto F_PropiedadesProyecto = new F_PropiedadesProyecto();
        private F_CopiarPegarDisenar F_CopiarPegarDisenar = new F_CopiarPegarDisenar();
        public static F_Informe F_Informe;
        #endregion Ventanas Emergentes

        #region Ventanas Acopladas
        public static F_SelectNervio F_SelectNervio;
        public static F_PlantaNervios F_PlantaNervios;
        public static F_ModificadorDeRefuerzos F_ModificadorDeRefuerzos;
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
            PropiedadesPrograma = cPropiedades.CargarPropiedades();
            T_AutoGuardado.Interval = PropiedadesPrograma.IntervaloMinAutoGuarado.MiliSegundos;
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
            F_Base_ = this;
            NotificadorVersiones();
        }

        #region Programa 
        public static cPropiedades PropiedadesPrograma;

        private void NotificadorVersiones()
        {
            Timer TimerNotificador = new Timer();
            TimerNotificador.Interval = 1000 * 300; //Cada 5Min
            TimerNotificador.Tick += TimerNotificador_Tick;
            TimerNotificador.Start();
            Notificador.BalloonTipClicked += Notificador_Click;
        }
        private void Notificador_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.AbrirUnidadGoogleDrive();
        }
        private async void TimerNotificador_Tick(object sender, EventArgs e)
        {
            var Result = await cSafe.ComprobarVersionProgramaAsync();
            if (!Result.Item1)
            {
                Notificador.BalloonTipText = $"Versión {Result.Item2} disponible, verifique en la carpeta de Instaladores de su Google Drive el nuevo instalador.";
                Notificador.ShowBalloonTip(1);
            }
        }
        #endregion

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
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }
        private void VentanaEmergente(ref F_PropiedadesProyecto Form)
        {
            if (!Form.Created) { Form = new F_PropiedadesProyecto(); }
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }
        private void VentanaEmergente(ref F_CopiarPegarDisenar Form)
        {
            if (!Form.Created) { Form = new F_CopiarPegarDisenar(); }
            Form.StartPosition = FormStartPosition.CenterScreen;
            Form.ShowDialog();
        }
        #endregion

        #region Funciones para Abrir, Crear, Guardar Proyecto 


        private void FuncionGenerarArchivoDLLNet()
        {

            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Title = "Exportar Cantidades | Archivo TXT DL-NET";
            SaveFileDialog.Filter = $"Archivo TXT |*.txt"; SaveFileDialog.FileOk += SaveFileDialog_FileOk;
            if (Proyecto.Ruta != "")
            {
                SaveFileDialog.InitialDirectory = Proyecto.Ruta;
                SaveFileDialog.FileName = $"Cantidades DLL NET_{Proyecto.Nombre}";
            }
            SaveFileDialog.ShowDialog();

            void SaveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
            {
                cFunctionsProgram.CrearArhivotxtDLNET(Proyecto.Edificio.PisoSelect.Nervios,SaveFileDialog.FileName);
            }
            
        }

        private void FuncionGenerarMemoriaCalculosPDF()
        {
            CrearCarpetaLocalConArchivos();
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Title = "Exportar Memorias de Cálculo";
            SaveFileDialog.Filter= $"Memorias de Cálculo |*.pdf"; SaveFileDialog.FileOk += SaveFileDialog_FileOk;
            if (Proyecto.Ruta != "")
            {
                SaveFileDialog.InitialDirectory = Proyecto.Ruta;
                SaveFileDialog.FileName = $"Memorias de Cálculo_{Proyecto.Nombre}";
            }
            SaveFileDialog.ShowDialog();
            
            void SaveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
            {
                cPDF.CrearPDF(Ruta_MemoriaCalculo, SaveFileDialog.FileName, Proyecto.Edificio.PisoSelect.Nervios);
            }

        }



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
                    Proyecto.Ruta = Ruta;
                    Proyecto.Nombre = Path.GetFileName(Ruta).Replace(cFunctionsProgram.Ext, "");
                    
                }
            }
        }


        private void GuardarVentanasXML()
        {
            CrearCarpetaLocalConArchivos();
            if (DP_ContenedorPrincipal.Contents.Count != 0)
            {
                DP_ContenedorPrincipal.SaveAsXml(Ruta_ConfigVentanasUsuario);
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
            if (!File.Exists(Ruta_ConfiVentanasSelectNervioUsuario))
                File.Copy(Ruta_ConfiVentanasSelectNervioDefault, Ruta_ConfiVentanasSelectNervioUsuario);

        }

        private void GuardarProyecto_Function()
        {
            if (Proyecto.Ruta != "")
            {
                cFunctionsProgram.Serializar(Proyecto.Ruta, Proyecto);
                GuardarVentanasXML();
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
                GuardarVentanasXML();
            }
        }
        public void FuncionDiseñarNervios(List<cNervio> Nervios)
        {
            EnviarEstadoVacio();
            F_CopiarPegarDisenar.TipoRefuerzo = Proyecto.TipoRefuerzo;
            VentanaEmergente(ref F_CopiarPegarDisenar); 
            Proyecto.TipoRefuerzo = F_CopiarPegarDisenar.TipoRefuerzo;
            cFunctionsProgram.DiseñarNervios(Nervios,Proyecto.TipoRefuerzo);
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
            if (NervioAux != null) { 
                Proyecto.Edificio.PisoSelect.NervioSelect = NervioAux;
                Proyecto.Edificio.PisoSelect.Nervios[Proyecto.Edificio.PisoSelect.Nervios.FindIndex(y => y.Nombre== NervioAux.Nombre)] = NervioAux;
            }
            ActualizarTodosLasVentanas();
        }

        public static void Rehacer_Function_Nervio()
        {
            cNervio NervioAux = UndoRedoNervio.Rehacer(Proyecto.Edificio.PisoSelect.NervioSelect);
            if (NervioAux != null) { 
                Proyecto.Edificio.PisoSelect.NervioSelect = NervioAux;
                Proyecto.Edificio.PisoSelect.Nervios[Proyecto.Edificio.PisoSelect.Nervios.FindIndex(y => y.Nombre == NervioAux.Nombre)] = NervioAux;
            }
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
            F_PlantaNervios.Invalidate();
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
                if (Proyecto.Edificio != null && Proyecto.Edificio.PisoSelect != null && Proyecto.Edificio.PisoSelect.Nervios != null && Proyecto.Edificio.PisoSelect.Nervios.Count!=0)
                {
                    ActivarDesactivarBotonesNervios(true);
                    if (Proyecto.Edificio.PisoSelect.NervioSelect != null)
                    {
                        ActivarDesactivarBotonesNervioSelect(true);

                        TLSN_ApoyoInicio.Enabled = Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.First() is cSubTramo;
                        TLSN_ApoyoFinal.Enabled = Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Last() is cSubTramo;
                        TLSN_ApoyoInicioE.Enabled = !(Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.First() is cSubTramo);
                        TLSN_ApoyoFinalE.Enabled = !(Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Last() is cSubTramo);

                    }
                    else
                    {
                        ActivarDesactivarBotonesNervioSelect(false);
                    }
                }
                else
                {
                    ActivarDesactivarBotonesNervios(false);
                    ActivarDesactivarBotonesNervioSelect(false);
                }
                CambiosTimer_3_F_EnumeracionPortico_Proyecto();
                BloqueoDesbloqueoBotones(true);

                if (PropiedadesPrograma.AutoGuardado)
                    T_AutoGuardado.Start();

               // TSMI_ActualizarEstribos.Visible = Proyecto.VersionPrograma<1.05f;
            }
            else
            {
                TLSMI_Deshacer.Enabled = false;TLSMI_Rehacer.Enabled = false;
                ActivarDesactivarBotonesNervios(false);
                Text = cFunctionsProgram.NombrePrograma;
                ActivarVentanaEmergenteGuardarCambios = false;
                ActivarDesactivarBotonesNervioSelect(false);
                BloqueoDesbloqueoBotones(false);
                T_AutoGuardado.Stop();
                TSMI_ActualizarEstribos.Visible = false;
            }
        }

        private void ActivarDesactivarBotonesNervios(bool Bool)
        {
            TLS_RevisarProyecto.Enabled = Bool;
            TLSB_PesoTotal.Enabled = Bool;
            pesoTotalToolStripMenuItem.Enabled = Bool;
            selecciónDeNerviosToolStripMenuItem.Enabled = Bool;
            TLSB_ModificarEjes.Enabled = Bool;
            TSB_ReasignarEjesNervios.Enabled = Bool;
            exportarDLNETNIMBUSToolStripMenuItem.Enabled = Bool;
            TLSB_RenombrarNervios.Enabled = Bool;
        }

        private void ActivarDesactivarBotonesNervioSelect(bool Bool)
        {

            /////------------------PESTAÑAS----------------------
            ///Archivo
            exportarToolStripMenuItem.Enabled = Bool;
            //Editar
            similitudDeNerviosToolStripMenuItem.Enabled = Bool;
            combinacionesToolStripMenuItem.Enabled = Bool;
            tendenciasToolStripMenuItem.Enabled = Bool;

            TLSMI_EliminarRefuerzo.Enabled = Bool;
            TLSMI_CopiarGeometria.Enabled = Bool;
            TLSMI_PegarRefuerzo.Enabled = Bool;
            TLSMI_PegarGeometria.Enabled = Bool;
            //Ver
            selecciónDeNerviosToolStripMenuItem.Enabled = Bool;
            esquemaToolStripMenuItem.Enabled = Bool;
            diagramasToolStripMenuItem.Enabled = Bool;

            //Diseño
            diseñarToolStripMenuItem.Enabled = Bool;
            autoCADToolStripMenuItem.Enabled = Bool;
            revisarProyectoToolStripMenuItem.Enabled = Bool;

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
            TLSB_ExportarPDF.Enabled = Bool;
            TLSB_EliminarApoyo.Enabled = Bool;
            TLSB_AgregarApoyo.Enabled = Bool;

            if (Bool)
            {
                TLSB_AgregarApoyo.Enabled = !Proyecto.Edificio.PisoSelect.NervioSelect.SinRefuerzos_();
                TLSB_EliminarApoyo.Enabled = Proyecto.Edificio.PisoSelect.NervioSelect.PoderEliminarApoyos();
                TLSMI_EliminarRefuerzo.Enabled = Proyecto.Edificio.PisoSelect.NervioSelect.SinRefuerzos_();
                TLSMI_PegarRefuerzo.Enabled = cFuncion_CopiaryPegarRefuerzos.Refuerzos.Count > 0;
                TLSMI_PegarGeometria.Enabled = cFuncion_CopiaryPegarGeometria.Elementos.Count > 0 && TLSB_AgregarApoyo.Enabled;
            }
            TLSMI_CopiarRefuerzo.Enabled = TLSMI_EliminarRefuerzo.Enabled;
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
                    TLSMI_Deshacer.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                    TLSMI_Rehacer.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                }
                else
                {
                    ActivarVentanaEmergenteGuardarCambios = UndoRedoNervio.ObtenerEstadoEstados();
                    TSB_Undo.Enabled = UndoRedoNervio.ObtenerEstadoCtrlZ();
                    TSB_Redo.Enabled = UndoRedoNervio.ObtenerEstadoCtrlY();
                    TLSMI_Deshacer.Enabled = UndoRedoNervio.ObtenerEstadoCtrlZ();
                    TLSMI_Rehacer.Enabled = UndoRedoNervio.ObtenerEstadoCtrlY();
                }
            }
        }
        private void CambiosTimer_3_F_EnumeracionPortico_Proyecto()
        {
            if (F_EnumeracionPortico != null)
            {
                F_EnumeracionPortico.deshacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
                F_EnumeracionPortico.rehacerToolStripMenuItem.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                F_EnumeracionPortico.TLSB_Rehacer.Enabled = UndoRedo.ObtenerEstadoCtrlY();
                F_EnumeracionPortico.TLSB_Deshacer.Enabled = UndoRedo.ObtenerEstadoCtrlZ();
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
            Rectangle RectangeScreen = Screen.FromHandle(Handle).WorkingArea;

            MaximizedBounds = new Rectangle(0, 0, RectangeScreen.Width, RectangeScreen.Height);
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
            Rectangle RectangeScreen = Screen.FromHandle(Handle).WorkingArea;
            MaximizedBounds = new Rectangle(0, 0, RectangeScreen.Width, RectangeScreen.Height);
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
            F_SelectNervioFunctions F_SelectNervio = new F_SelectNervioFunctions(F_SelectNervioFunctions.eEditarSelectNervio.Diseno);
            F_SelectNervio.ShowDialog();
            List<cNervio> Nervios = Proyecto.Edificio.PisoSelect.Nervios.FindAll(y => y.Propiedades.Diseno);
            if (F_SelectNervioFunctions.Aceptar)
            {
                FuncionDiseñarNervios(Nervios);
                ActualizarTodosLasVentanas();
            }
        }

        private void TSB_DiseñarAllNervio_Click(object sender, EventArgs e)
        {
            EnviarEstadoVacio();
            FuncionDiseñarNervios(Proyecto.Edificio.PisoSelect.Nervios);
            ActualizarTodosLasVentanas();
        }

        private void TLSB_GraficarNervioActual_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.GraficarNervios(new List<cNervio>() { Proyecto.Edificio.PisoSelect.NervioSelect});
        }

        private void TLSB_GraficarNerviosAutoCAD_Click(object sender, EventArgs e)
        {
            F_SelectNervioFunctions F_SelectNervio = new F_SelectNervioFunctions(F_SelectNervioFunctions.eEditarSelectNervio.AutoCAD);
            F_SelectNervio.ShowDialog();
            List<cNervio> Nervios = Proyecto.Edificio.PisoSelect.Nervios.FindAll(y => y.Propiedades.DibujoAutoCAD);
            if(F_SelectNervioFunctions.Aceptar)
                cFunctionsProgram.GraficarNervios(Nervios);
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
            if (sender == TLSN_ApoyoInicio)
            {
                EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
                Proyecto.Edificio.PisoSelect.NervioSelect.CrearApoyosAExtremos(true);
                ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }else if (sender == TLSN_ApoyoInicioE)
            {
                EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
                Proyecto.Edificio.PisoSelect.NervioSelect.EliminarApoyosAExtremos(true);
                ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }
        }

        private void TLSN_ApoyoFinal_Click(object sender, EventArgs e)
        {
            if (sender == TLSN_ApoyoFinal)
            {
                EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
                Proyecto.Edificio.PisoSelect.NervioSelect.CrearApoyosAExtremos(ApoyoFinal: true);
                ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }else if (sender== TLSN_ApoyoFinalE)
            {
                EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
                Proyecto.Edificio.PisoSelect.NervioSelect.EliminarApoyosAExtremos(ApoyoFinal: true);
                ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }
        }



        private void TLSB_ExportarPDF_Click(object sender, EventArgs e)
        {
            FuncionGenerarMemoriaCalculosPDF();
        }
        
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void similitudDeNerviosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_PlantaSimilitudNervios f_PlantaSimilitudNervios = new F_PlantaSimilitudNervios(Proyecto.Edificio);
            f_PlantaSimilitudNervios.ShowDialog();
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }
        private void revisarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.RevisarNervios(Proyecto.Edificio.PisoSelect.Nervios);
            if (F_Informe != null && F_Informe.Created)
                try { F_Informe.Close(); } catch { }
            F_Informe = new F_Informe(Proyecto.Edificio.PisoSelect.Nervios);
            F_Informe.Show();

        }
        private void exportarDLNETNIMBUSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FuncionGenerarArchivoDLLNet();
        }
        private void TLSB_EliminarRefuerzos_Click(object sender, EventArgs e)
        {
            EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
            Proyecto.Edificio.PisoSelect.NervioSelect.EliminarTodoElRefuerzo();
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }
        private void TLSMI_CopiarRefuerzo_Click(object sender, EventArgs e)
        {
            cFuncion_CopiaryPegarRefuerzos.Copiar(Proyecto.Edificio.PisoSelect.NervioSelect);
        }

        private void TLSMI_PegarRefuerzo_Click(object sender, EventArgs e)
        {
            EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
            cFuncion_CopiaryPegarRefuerzos.Pegar(Proyecto.Edificio.PisoSelect.NervioSelect);
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }
        private void TLSMI_CopiarGeometria_Click(object sender, EventArgs e)
        {
            cFuncion_CopiaryPegarGeometria.Copiar(Proyecto.Edificio.PisoSelect.NervioSelect);
        }

        private void TLSMI_PegarGeometria_Click(object sender, EventArgs e)
        {
            EnviarEstado_Nervio(Proyecto.Edificio.PisoSelect.NervioSelect);
            cFuncion_CopiaryPegarGeometria.Pegar(Proyecto.Edificio.PisoSelect.NervioSelect);
            ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }

        private void TLSB_RenombrarNervios_Click(object sender, EventArgs e)
        {
            EnviarEstadoVacio();
            Proyecto.Nomenclatura_Hztal = Proyecto.Nomenclatura_Hztal == eNomenclatura.Alfabética ? eNomenclatura.Numérica : eNomenclatura.Alfabética;
            Proyecto.Nomenclatura_Vert = Proyecto.Nomenclatura_Vert == eNomenclatura.Alfabética ? eNomenclatura.Numérica : eNomenclatura.Alfabética;
            Proyecto.Edificio.Lista_Pisos.ForEach(y =>
            {
                cFunctionsProgram.RenombrarNervios(y.Nervios, Proyecto.Nomenclatura_Hztal, Proyecto.Nomenclatura_Vert);
            });

            ActualizarTodosLasVentanas();
            cFunctionsProgram.VentanaEmergenteInformacion("Nervios Renombrados con Éixto");

        }

        private void T_AutoGuardado_Tick(object sender, EventArgs e)
        {
            if (Proyecto.Ruta != "")
            {
                cFunctionsProgram.Serializar(Proyecto.Ruta.Replace(cFunctionsProgram.Ext, cFunctionsProgram.Ext_BackupNervios), Proyecto);
            }
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

        private void TSMI_ActualizarEstribos_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.ActualizarEstribosVersionesAnteriores(Proyecto);
            ActualizarTodosLasVentanas();
        }
    }
}