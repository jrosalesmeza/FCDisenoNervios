using System;
using System.Collections.Generic;
using FC_Diseño_de_Nervios.Controles;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Linq;
using FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas;

namespace FC_Diseño_de_Nervios
{
    public partial class F_SelectNervio : DockContent
    {
        float DesXButton;
        float DesYButton;
        float Dx = 0;
        float Dy = 0;
        float Zoom = 1;
        public F_SelectNervio()
        {
            InitializeComponent();
            PB_Nervios.MouseWheel += PB_Nervios_MouseWheel;
            PB_Nervios.MouseDown += PB_Nervios_MouseDown;
            PB_Nervios.MouseMove += PB_Nervios_MouseMove;
            PB_Nervios.MouseMove += PB_Nervios_MouseMove1;
            CustomizedToolTip ToolTipPerzonalizado = new CustomizedToolTip(); ToolTipPerzonalizado.AutoSize = false; ToolTipPerzonalizado.Size = new Size(150,150);
            ToolTipPerzonalizado.SetToolTip(PB_Info, $" "); PB_Info.Tag = Properties.Resources.Nervio_EsquemaR;
        }

        private void PB_Nervios_MouseMove1(object sender, MouseEventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null && F_Base.Proyecto.CoordenadasPInterseccion)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.IsPointMousePointLines(e.Location));
                PB_Nervios.Invalidate();
            }
            
        }

        private void CargarListViewStories()
        {
            LV_Stories.Clear();
            foreach (cPiso Piso in F_Base.Proyecto.Edificio.Lista_Pisos)
            {
                if (Piso.Nervios != null && Piso.Nervios.Count > 0)
                {
                    ListViewItem item = new ListViewItem(Piso.Nombre); item.Name = Piso.Nombre;
                    LV_Stories.Items.Add(item);
                }
            }
            if (F_Base.Proyecto.Edificio.PisoSelect != null)
            {
                ListViewItem listViewItems = FindListViewItem(F_Base.Proyecto.Edificio.PisoSelect.Nombre,LV_Stories);
                listViewItems.Selected = true;

            }
            else
            {
                LV_Stories.Items[LV_Stories.Items.Count - 1].Selected = true;
            }
            CargarListViewNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios);
        }
        private void CargarListViewNervios(List<cNervio> NerviosSelects)
        {
            LV_Nervios.Clear();

            if (NerviosSelects != null && NerviosSelects.Count>0)
            {
                foreach (cNervio Nervio in NerviosSelects)
                {
                    LV_Nervios.Items.Add(Nervio.Nombre);
                }

                if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
                {
                    ListViewItem listViewItems = LV_Nervios.FindItemWithText(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre);
                    if (listViewItems !=null)
                    {
                        listViewItems.Selected = true;
                    }
                }
                else
                {
                    LV_Nervios.Items[LV_Nervios.Items.Count - 1].Selected = true;
                }
                SelectNervioChanged(new Point(0, 0), false);

            }

        }
        private void PB_Nervios_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            float XI = 5f; float YI = 5f;
            float WidthPB = PB_Nervios.Width - XI * 3;
            float HeightPB = PB_Nervios.Height - YI * 3;
            List<PointF> PointsSinEscalar = new List<PointF>();

            F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } });
                      
            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => { PointsSinEscalar.AddRange(y.Recta_Real); });


            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.CrearPuntosPlantaEscaladaEtabs(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));

            if (F_Base.PropiedadesPrograma.LineasPretrazado)
            {
                if (F_Base.PropiedadesPrograma.FuncionesEnParalelo)
                {

                    Parallel.ForEach(F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines, y =>
                    {
                        cFunctionsProgram.CheckCPUUsageAndSleepThread(cFunctionsProgram.cpuCounter);
                        y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom);

                      });
                }
                else
                {
                    F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
                }
                F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.PaintPlantaEscaladaEtabsLine(e.Graphics); });
            }

            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.Paint(e.Graphics, Zoom));

            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                if (F_Base.PropiedadesPrograma.FuncionesEnParalelo)
                {
                    Parallel.ForEach(F_Base.Proyecto.Edificio.PisoSelect.Nervios,
                    x =>
                    {
                        cFunctionsProgram.CheckCPUUsageAndSleepThread(cFunctionsProgram.cpuCounter);
                        x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));

                    });

                    F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Paint_Planta_ElementosEnumerados(e.Graphics, PB_Nervios.Width, Zoom));
                }
                else
                {
                    F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(
                    x =>
                    {
                        x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
                        x.Paint_Planta_ElementosEnumerados(e.Graphics, PB_Nervios.Width, Zoom);
                    });
                }
            }
        }


        private void F_SelectNervio_Load(object sender, EventArgs e)
        {
            CargarListViewStories();
            CB_Direccion.SelectedItem = eDireccion.Todos.ToString(); 

        }
        private ListViewItem FindListViewItem(string NameItem,ListView listView)
        {
            foreach(ListViewItem item in listView.Items)
            {
                if (NameItem == item.Name || NameItem== item.Text)
                    return item;
            }
            return null;
        }

        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count > 0)
            {
            
                cPiso PisoSelect= F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                F_Base.Proyecto.Edificio.PisoSelect = PisoSelect;
                


                CargarListViewNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios);
                NerviosACargar();
                SelectNervioChanged(new Point(), false);
            }
        }
        private void LV_Nervios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Nervios.SelectedItems.Count > 0)
            {
                ListViewItem LVI = new ListViewItem();
                if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
                {
                    LVI = FindListViewItem(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre, LV_Nervios);
                    if (LVI == null)
                        LVI = new ListViewItem();
                }
                if(LV_Nervios.SelectedItems[0].Text!= LVI.Text){
                    SelectNervioChanged(new Point(0,0), false, LV_Nervios.SelectedItems[0].Text);
                }
            }
        }
        private void PB_Nervios_MouseMove(object sender, MouseEventArgs e)
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
                PB_Nervios.Invalidate();
            }
            PB_Nervios.Cursor = cursor;


        }
        private void Habilitar_DeshabilitarNevioBorde()
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.ActivarBoolNervioBorde)
            {
                CKB_NervioBorde.Checked = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.NervioBorde;
                CKB_NervioBorde.Enabled = true;
            }
            else
            {
                CKB_NervioBorde.Enabled = false;
                CKB_NervioBorde.Checked = false;

            }
        }
        private void HabilitarMaestroSimilarA()
        {
            cNervio Nervio = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect;
            LB_NervioSimilarA.Text = "";
            GB_Similitud.Text = "Similitud";
            if (Nervio.SimilitudNervioGeometria.IsMaestro | Nervio.SimilitudNervioGeometria.BoolSoySimiarA)
            {
                GB_Similitud.Text = "Similitud por Geometría";
                LB_NervioSimilarA.Text = Nervio.SimilitudNervioGeometria.SoySimiarA.ToString(Nervio.PisoOrigen.Nombre);
            }else if (Nervio.SimilitudNervioCompleto.IsMaestro | Nervio.SimilitudNervioCompleto.BoolSoySimiarA)
            {
                GB_Similitud.Text = "Similitud de Todo";
                LB_NervioSimilarA.Text = Nervio.SimilitudNervioCompleto.SoySimiarA.ToString(Nervio.PisoOrigen.Nombre);
            }
            CKB_Maestro.Checked = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.SimilitudNervioGeometria.IsMaestro | F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.SimilitudNervioCompleto.IsMaestro;
        }



        private void PB_Nervios_MouseWheel(object sender, MouseEventArgs e)
        {
            int Vueltas = e.Delta;
            if (Vueltas > 0)
            {
                Zoom += 0.1f;
                PB_Nervios.Invalidate();
            }
            else
            {
                Zoom -= 0.1f;
                PB_Nervios.Invalidate();
            }

        }

        private void PB_Nervios_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks == 2)
            {
                Zoom = 1;
                Dx = 0; Dy = 0;
                PB_Nervios.Invalidate();
            }
            if (e.Button == MouseButtons.Middle)
            {
                DesXButton = e.X;
                DesYButton = e.Y;
            }
        }
        private void PB_Nervios_MouseDown_SelectNervio(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks==1)
            {
                SelectNervioChanged(e.Location, true);
            }
        }
        private void F_SelectNervio_Resize(object sender, EventArgs e)
        {
            PB_Nervios.Invalidate();
        }

        public void SelectNervioChanged(Point Location, bool MouseDown, string NombreABuscar = "")
        {
            cNervio NervioSelectMouse = null;
        
                if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null && F_Base.Proyecto.Edificio.PisoSelect.Nervios.Count > 0)
                {
                    if (NombreABuscar != "")
                    {
                        F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.Nombre == NombreABuscar);
                        F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Select = true;
                    }
                    else if (MouseDown)
                    {
                        NervioSelectMouse = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.MouseDownSelect(Location));
                        if (NervioSelectMouse != null)
                        {
                            F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = NervioSelectMouse;
                            LV_Nervios.SelectedItems.Clear();
                        }
                    }
                }
           
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x.Nombre != F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre).ForEach(x => { x.Select = false; x.ChangeSelect(); });
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Select = true; F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.ChangeSelect();

                ListViewItem LVI = FindListViewItem(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre, LV_Nervios);
                if (LVI != null)
                    LVI.Selected = true;
      
                GB_Propiedades.Text = $" {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | {F_Base.Proyecto.Edificio.PisoSelect.Nombre}";
                Text = $"Selección de Nervios | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | {F_Base.Proyecto.Edificio.PisoSelect.Nombre} ";
                CB_SeccionAltura.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Bool_CambioAltura;
                CB_SeccionAncho.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Bool_CambioAncho;
                ChangeComboBox();
                CB_SeccionAltura.SelectedItem = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAltura.ToString();
                CB_SeccionAncho.SelectedItem = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAncho.ToString();
                TB_r1.Text =string.Format("{0:0.00}",F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.r1);
                TB_r2.Text = string.Format("{0:0.00}", F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.r2);
                Habilitar_DeshabilitarNevioBorde(); HabilitarMaestroSimilarA();
            }
            else
            {
                Text = $"Selección de Nervios";
                GB_Propiedades.Text = "";
            }
            F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();F_Base.ActualizarVentanaF_VentanaDiseno();
            PB_Nervios.Invalidate();
        }

        private void CB_SeccionAltura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAltura = cFunctionsProgram.ConvertirStringtoeCambioAlto(CB_SeccionAltura.Text);
                F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }
        }

        private void CB_SeccionAncho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAncho = cFunctionsProgram.ConvertirStringtoeCambioAncho(CB_SeccionAncho.Text);
            }
        }

        private void ChangeComboBox()
        {
            CB_SeccionAltura.Items.Clear();
            CB_SeccionAncho.Items.Clear();
            if (CB_SeccionAncho.Enabled)
            {
                CB_SeccionAncho.Items.AddRange(new string[] { "Superior", "Central", "Inferior" });
            }
            else
            {
                CB_SeccionAncho.Items.Add("Ninguno");
            }
            if (CB_SeccionAltura.Enabled)
            {
                CB_SeccionAltura.Items.AddRange(new string[] { "Superior", "Central", "Inferior" });
            }
            else
            {
                CB_SeccionAltura.Items.Add("Ninguno");
            }

        }

        private void CB_Direccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            NerviosACargar();
        }
        private void NerviosACargar()
        {
            eDireccion Direccion = cFunctionsProgram.ConvertirStringtoeDireccion(CB_Direccion.Text);
            List<cNervio> NerviosSelects;
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null && F_Base.Proyecto.Edificio.PisoSelect.Nervios.Count > 0)
            {
                if (Direccion == eDireccion.Todos)
                {
                    NerviosSelects = cFunctionsProgram.OrdenarNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios);
                }
                else
                {
                    NerviosSelects = F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x.Direccion == Direccion).OrderBy
                        (y =>
                        {
                            object Variable = y.Nombre;
                            if (y.Nombre.Replace(y.Prefijo, "").IsNumeric())
                            {
                                Variable = int.Parse(y.Nombre.Replace(y.Prefijo, ""));
                            }
                            return Variable;
                        }).ToList();
                }

                CargarListViewNervios(NerviosSelects);
            }
        }

        private void F_SelectNervio_Paint(object sender, PaintEventArgs e)
        {
            CargarListViewStories();
            CB_Direccion.SelectedItem = eDireccion.Todos.ToString();
        }

        private void TB_d1_TextChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                float.TryParse(TB_r1.Text, out float r1);
                if (r1 == 0) { r1 = 4f;}
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.r1 = r1;
            }
        }

        private void TB_d2_TextChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                float.TryParse(TB_r2.Text, out float r2);
                if (r2 == 0) { r2 = 4f; }
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.r2 = r2;

            }
        }

        private void modificarEjesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.FuncionVentanaEditarEJesGlobales();
        }

        private void reasignarEjesANerviosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.FuncionReasignarEjesaNervios();
        }

        private void RB_NervioBorde_CheckedChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.ActivarBoolNervioBorde)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.NervioBorde = CKB_NervioBorde.Checked;
            }
        }
    }
}
