using FC_Diseño_de_Nervios.Ventana_Principal.Nuevo_Proyecto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios
{
    public partial class F_EnumeracionPortico : Form
    {

        private float XI = 5; float YI = 5;
        private float WidthPB_NOENUMERADOS;
        private float Height_NOENUMERADOS;
        private float Zoom = 1;
        private float Dx = 0;
        private float Dy = 0;
        private float DesXButton;
        private float DesYButton;
        private bool SeleccionInteligente = false;
        private ToolTip ToolTipPerson = new ToolTip();
        public F_EnumeracionPortico()
        {
            InitializeComponent();
            ToolTipPerson.SetToolTip(BT_CrtierioFC, $"Crea los nervios enumerándolos automáticamente \nsegún criterios de {cFunctionsProgram.Empresa} (Extrayendo los \nelementos que contienen en su nombre el prefijo 'N').");
            WidthPB_NOENUMERADOS = PB_ElementosNoEnumerados.Width - XI * 3;
            Height_NOENUMERADOS = PB_ElementosNoEnumerados.Height - YI * 3;
            PB_ElementosEnumerados.Paint += PB_ElementosEnumerados_Paint;
            PB_ElementosNoEnumerados.MouseDown += PB_ElementosNoEnumerados_MouseDown2;
            PB_ElementosNoEnumerados.MouseMove += PB_ElementosNoEnumerados_MouseMove2;
            CB_Nomenclatura_Hztal.Items.AddRange(new string[] { eNomenclatura.Alfabética.ToString(), eNomenclatura.Numérica.ToString() });
            CB_Nomenclatura_Vertical.Items.AddRange(new string[] { eNomenclatura.Alfabética.ToString(), eNomenclatura.Numérica.ToString() });
        }

        public void CargarListView()
        {
            cDatosEtabs DatosEtabs = F_Base.Proyecto.DatosEtabs;
            LV_Stories.Clear(); 
            foreach (cPiso Piso in DatosEtabs.Lista_Pisos)
            {
                ListViewItem item = new ListViewItem(Piso.Nombre); item.Name = Piso.Nombre;
                LV_Stories.Items.Add(item);
            }
            if (F_Base.Proyecto.DatosEtabs.PisoSelect != null)
            {
                ListViewItem[] listViewItems = LV_Stories.Items.Find(F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre, false);
                listViewItems[0].Selected = true;

            }
            else
            {
                LV_Stories.Items[LV_Stories.Items.Count - 1].Selected = true;
            }

        }
        public void CargarCB_Nomenclatura()
        {
            CB_Nomenclatura_Hztal.SelectedItem = F_Base.Proyecto.Nomenclatura_Hztal.ToString();
            CB_Nomenclatura_Vertical.SelectedItem =  F_Base.Proyecto.Nomenclatura_Vert.ToString();
        }
        private void F_EnumPort_Load()
        {

            CargarListView();
            CargarCB_Nomenclatura();
            T_Timer2.Start();
        }

        private void MensajeDeAlerta()
        {

            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                List<cNervio> NerviosaEliminar = F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x.CantApoyos==0);

                if (NerviosaEliminar != null && NerviosaEliminar.Count>0)
                {
                    string ElemetnosStrings = ""; NerviosaEliminar.ForEach(x => ElemetnosStrings += x.Lista_Objetos.First().Line.Nombre + ", ");
                    DialogResult BoxResult = NerviosaEliminar.Count > 1
                        ? MessageBox.Show($"Los Elementos: {ElemetnosStrings} no pueden ser enumerados debido a que no se encontraron apoyos", cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        : MessageBox.Show($"El Elemento: {ElemetnosStrings} no puede ser enumerdo debido a que no se encontraron apoyos", cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    List<int> IDsAEliminar = NerviosaEliminar.Select(x => x.ID).ToList();
                    NerviosaEliminar.ForEach(x => x.Lista_Objetos.ForEach(y => { F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(z => z.Nombre == y.Line.Nombre && y.Soporte == eSoporte.Vano).ForEach(m => { m.isSelect = true; }); }));
                    foreach (int ID in IDsAEliminar)
                    {
                        F_Base.Proyecto.Edificio.PisoSelect.Nervios.Remove(F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.ID == ID));

                    }
                    int IDaRenombrar = 1;
                    F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(y => { y.ID = IDaRenombrar; IDaRenombrar += 1;});
                    cFunctionsProgram.RenombrarNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios, F_Base.Proyecto.Nomenclatura_Hztal, F_Base.Proyecto.Nomenclatura_Vert);
                }
            }
        }

        private void CambiosTimer()
        {


            if (F_Base.Proyecto.DatosEtabs.PisoSelect == null)
            {
                F_Base.Proyecto.DatosEtabs.PisoSelect = F_Base.Proyecto.DatosEtabs.Lista_Pisos[F_Base.Proyecto.DatosEtabs.Lista_Pisos.Count - 1];
            }
            cLine ElementSelect = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Find(x => x.Select == true);
            if (ElementSelect != null)
            {
                TB_Nombramiento.Enabled = true;
                BT_Enumerar.Enabled = true;
                Text = $"Enumeración de Elementos | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
            }
            else
            {
                TB_Nombramiento.Enabled = false;
                BT_Enumerar.Enabled = false;
            }
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                cNervio NervioSelect = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.SelectPlantaEnumeracion == true);

                if (NervioSelect != null)
                {
                    BT_Regresar.Enabled = true;
                }
                else
                {
                    BT_Regresar.Enabled = false;
                }
            }
            else
            {
                BT_Regresar.Enabled = false;
            }

        }
        private void F_EnumeracionPortico_Load(object sender, EventArgs e)
        {

            F_EnumPort_Load();
        }

        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (LV_Stories.SelectedItems.Count > 0)
            {
                F_Base.Proyecto.DatosEtabs.PisoSelect = F_Base.Proyecto.DatosEtabs.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                F_Base.Proyecto.Edificio.PisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                LB_Title.Text = $"Enumeración de Elementos  | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
                PB_ElementosNoEnumerados.Invalidate(); PB_ElementosEnumerados.Invalidate();
            }
        }

        private void T_Timer2_Tick(object sender, EventArgs e)
        {
            CambiosTimer();
        }


        #region Eventos Principales Ventana
        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);

        }
        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            F_Base.EnviarEstadoVacio();
            List<cPiso> PisosConNervios = F_Base.Proyecto.Edificio.Lista_Pisos.FindAll(x => x.Nervios != null && x.Nervios.Count > 0);
            if (PisosConNervios!=null && PisosConNervios.Count>0)
            {
                F_Base.F_Base_.CargarVentanasXML();
                PisosConNervios.ForEach(x => x.Nervios.ForEach(y => y.SelectPlantaEnumeracion = false));
                F_Base.ActualizarVentanaF_SelectNervio();
            }
            F_Base.LimpiarMemoria();
            T_Timer2.Stop();
            Visible = false;
        }


        #endregion

        private void PB_ElementosNoEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            WidthPB_NOENUMERADOS = PB_ElementosNoEnumerados.Width - XI * 3;
            Height_NOENUMERADOS = PB_ElementosNoEnumerados.Height - YI * 3;
            List<PointF> PointsSinEscalar = new List<PointF>();
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } });
            F_Base.Proyecto.DatosEtabs.Lista_Grids.ForEach(y => { PointsSinEscalar.AddRange(y.Recta_Real); });

            if (F_Base.PropiedadesPrograma.FuncionesEnParalelo)
            {
                Parallel.ForEach(F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines, 
                    y =>
                    {
                        cFunctionsProgram.CheckCPUUsageAndSleepThread(cFunctionsProgram.cpuCounter);
                        y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, Dx, Dy, Zoom);

                    });
            }
            else
            {
                F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, Dx, Dy, Zoom));
            }
               
            F_Base.Proyecto.DatosEtabs.Lista_Grids.ForEach(y => y.CrearPuntosPlantaEscaladaEtabs(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, Dx, Dy, Zoom));
            F_Base.Proyecto.DatosEtabs.Lista_Grids.ForEach(y => y.Paint(e.Graphics,Zoom));
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.PaintPlantaEscaladaEtabsLine(e.Graphics); });

        }

        private void PB_ElementosNoEnumerados_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor CursorArraste = new Cursor(Properties.Resources.Arrastre16x16.Handle);
            //Cursor CursorDefault = new Cursor(Properties.Resources.DefaultCursor2x16.Handle);
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
                PB_ElementosNoEnumerados.Invalidate();
            }
            PB_ElementosNoEnumerados.Cursor = cursor;


        }
        private void PB_ElementosNoEnumerados_MouseMove2(object sender, MouseEventArgs e)
        {
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.MouseMoveElementoSinEnumerar(e.Location); });
            PB_InfoElementosNoEnumerados.Invalidate();
        }
        private void PB_ElementosNoEnumerados_MouseWheel(object sender, MouseEventArgs e)
        {
            int vueltas = e.Delta;
            if (vueltas > 0)
            {

                Zoom += 0.5f;
                PB_ElementosNoEnumerados.Invalidate();
            }
            else
            {
                if (Zoom > 1)
                {
                    Zoom -= 0.5f;
                    PB_ElementosNoEnumerados.Invalidate();
                }
            }

        }

        private void PB_ElementosNoEnumerados_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks == 2)
            {
                Zoom = 1;
                Dx = 0; Dy = 0;
                PB_ElementosNoEnumerados.Invalidate();
            }
            if (e.Button == MouseButtons.Middle)
            {
                DesXButton = e.X;
                DesYButton = e.Y;
            }
        }

        private void PB_ElementosNoEnumerados_MouseDown2(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                int IndiceMaximo = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Max(x => x.IndexSelect);
                F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.MouseDownSelectLineEtabs(e, IndiceMaximo); });
                int MaximoIndice = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Max(x => x.IndexSelect);
                cLine UltimaSelect = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Find(x => x.MouseInLineEtabs(e.Location));

                if (UltimaSelect != null && SeleccionInteligente)
                {
                    cFunctionsProgram.SeleccionInteligente(UltimaSelect, F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(x => x.Type == eType.Beam).ToList(), UltimaSelect.IndexSelect, UltimaSelect.Select);
                }

            }
            PB_ElementosNoEnumerados.Invalidate();
        }

        private void CKB_SeleccionInteligente_CheckedChanged(object sender, EventArgs e)
        {
            SeleccionInteligente = CKB_SeleccionInteligente.Checked;
        }

        private void PB_InfoElementosNoEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y =>
            {
                if (y.Type == eType.Beam)
                    y.PaintMouseMove(e.Graphics, PB_InfoElementosNoEnumerados.Height, PB_InfoElementosNoEnumerados.Width);
            });
        }


        private void mostrarSecciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mostrarSecciónToolStripMenuItem.Checked)
                PB_InfoElementosNoEnumerados.Visible = true;
            else
                PB_InfoElementosNoEnumerados.Visible = false;
        }

        private void limpirarSeleccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(x => { x.Select = false; x.IndexSelect = 0; });
            PB_ElementosNoEnumerados.Invalidate();
        }

        private void F_EnumeracionPortico_Paint(object sender, PaintEventArgs e)
        {
            CargarListView();
            PB_ElementosNoEnumerados.Invalidate();
            PB_ElementosEnumerados.Invalidate();
           
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.Deshacer_Function();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.Rehacer_Function();
        }

        private void F_EnumeracionPortico_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                F_EnumPort_Load();
            }
        }

        private void BT_Enumerar_Click(object sender, EventArgs e)
        {
            if (F_Base.PropiedadesPrograma.DeshacerRehacer)
                F_Base.EnviarEstado(F_Base.Proyecto);

            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(x => x.IndiceConjuntoSeleccion = 0);
            List<cLine> LineasSeleccionadas = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(x => x.Select == true).ToList();
            CreaNervios(TB_Nombramiento.Text, LineasSeleccionadas);
            PB_ElementosEnumerados.Invalidate();
            PB_ElementosNoEnumerados.Invalidate();
        }



        private void CreaNervios(string Prefijo, List<cLine> LineasConCondiciones)
        {
            List<List<cLine>> LineasParaCrearNervios = cFunctionsProgram.LineasParaCrearNervios(LineasConCondiciones);

            List<cNervio> NerviosPisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre).Nervios;
            int IndiceNervio = 1;
            if (NerviosPisoSelect == null || NerviosPisoSelect.Count == 0)
            {
                NerviosPisoSelect = new List<cNervio>();
            }
            else
            {
                IndiceNervio = NerviosPisoSelect.Last().ID + 1;
            }

            foreach (List<cLine> lines in LineasParaCrearNervios)
            {
                cNervio Nervio = cFunctionsProgram.CrearNervio(Prefijo, IndiceNervio, lines, F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines, F_Base.Proyecto.Edificio.Lista_Grids, F_Base.Proyecto.DatosEtabs.PisoSelect,WidthPB_NOENUMERADOS, Height_NOENUMERADOS);
                if (Nervio != null)
                {
                    Nervio.Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.Select = false));
                    NerviosPisoSelect.Add(Nervio);
                    IndiceNervio = NerviosPisoSelect.Last().ID + 1;
                }
            }
            F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre).Nervios = NerviosPisoSelect;
            cNervio NervioMenorListaObjetosMenoraCero = NerviosPisoSelect.Find(x => x.CantApoyos==0);
            if (NervioMenorListaObjetosMenoraCero != null)
            {
                MensajeDeAlerta();
            }
            else
            {
                cFunctionsProgram.RenombrarNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios, F_Base.Proyecto.Nomenclatura_Hztal, F_Base.Proyecto.Nomenclatura_Vert);
            }

        }

        private void PB_ElementosEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            List<PointF> PointsSinEscalar = new List<PointF>();
            F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } });
            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => { PointsSinEscalar.AddRange(y.Recta_Real); });
            if (F_Base.PropiedadesPrograma.LineasPretrazado)
            {
                if (F_Base.PropiedadesPrograma.FuncionesEnParalelo)
                {
                    Parallel.ForEach(F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines,
                    y =>
                    {
                        cFunctionsProgram.CheckCPUUsageAndSleepThread(cFunctionsProgram.cpuCounter);
                        y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, Dx, Dy, Zoom);

                    });
                }
                else
                {
                    F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, Dx, Dy, Zoom));
                }
                F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(x => { x.IndexSelect = 0; x.isSelect = false; x.PaintPlantaEscaladaEtabsLine(e.Graphics); });
            }

            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.CrearPuntosPlantaEscaladaEtabs(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, 0, 0, 1));
            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.Paint(e.Graphics, 1));

            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, 0, 0, 1))); ;
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Paint_Planta_ElementosEnumerados(e.Graphics, 1));
            }
        }

        private void PB_InfoElementos_Enumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.PaintNombreElementosEnumerados_MouseMove(e.Graphics, PB_InfoElementos_Enumerados.Height, PB_InfoElementos_Enumerados.Width));
            }
        }

        private void PB_ElementosEnumerados_MouseMove(object sender, MouseEventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.MouseMoveNervioPlantaEtabs(e.Location));
            }
            PB_InfoElementos_Enumerados.Invalidate();
        }

        private void PB_ElementosEnumerados_MouseDown(object sender, MouseEventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.MouseDownSelectPlantaEnumeracion(e.Location));
            }

            PB_ElementosEnumerados.Invalidate();
        }
        private void BT_Regresar_Click(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                if (F_Base.PropiedadesPrograma.DeshacerRehacer)
                    F_Base.EnviarEstado(F_Base.Proyecto);
                List<cNervio> NerviosSelects = F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x.SelectPlantaEnumeracion == true).ToList();
                List<int> IDsAEliminar;
                if (NerviosSelects != null)
                {
                    IDsAEliminar = NerviosSelects.Select(x => x.ID).ToList();
                    NerviosSelects.ForEach(x => x.Lista_Objetos.ForEach(y => { F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(z => z.Nombre == y.Line.Nombre && y.Soporte == eSoporte.Vano).ForEach(m => { m.isSelect = true; }); }));
                    foreach (int ID in IDsAEliminar)
                    {
                        F_Base.Proyecto.Edificio.PisoSelect.Nervios.Remove(F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.ID == ID));

                    }
                    int IDaRenombrar = 1;
                    F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(y => { y.ID = IDaRenombrar; IDaRenombrar += 1; });
                    cFunctionsProgram.RenombrarNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios, F_Base.Proyecto.Nomenclatura_Hztal, F_Base.Proyecto.Nomenclatura_Vert);
                }
            }
            PB_ElementosNoEnumerados.Invalidate();
            PB_ElementosEnumerados.Invalidate();
        }

        private void BT_CrtierioFC_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.Notificar("Cargando, Creando Nervios.");
            if (F_Base.PropiedadesPrograma.DeshacerRehacer)
                F_Base.EnviarEstado(F_Base.Proyecto);
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(x => { x.IndiceConjuntoSeleccion = 0; x.isSelect = true; });
            F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre).Nervios = new List<cNervio>();
            List<cLine> LineasSeleccionadas = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(x => x.Type == eType.Beam && x.Seccion != null && x.Seccion.Nombre.Contains("N")).ToList();
            CreaNervios("N-", LineasSeleccionadas);
            PB_ElementosEnumerados.Invalidate();
            PB_ElementosNoEnumerados.Invalidate();
            cFunctionsProgram.Notificar("Listo.");
        }

        private void CB_Nomenclatura_Hztal_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_Nomenclatura_Vertical.SelectedItem = F_Base.Proyecto.Nomenclatura_Hztal.ToString();
            F_Base.Proyecto.Nomenclatura_Hztal = cFunctionsProgram.ConvertirStringtoeNomenclatura(CB_Nomenclatura_Hztal.Text);
        }

        private void CB_Nomenclatura_Vertical_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_Nomenclatura_Hztal.SelectedItem = F_Base.Proyecto.Nomenclatura_Vert.ToString();
            F_Base.Proyecto.Nomenclatura_Vert = cFunctionsProgram.ConvertirStringtoeNomenclatura(CB_Nomenclatura_Vertical.Text);
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_EditarPisos f_EditarPisos = new F_EditarPisos(F_Base.Proyecto.Edificio.Lista_Pisos);
            f_EditarPisos.ShowDialog();
        }
    }
}
