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

        private DeserializeDockContent DeserializeDockContent;
         public F_SelectNervio()
        {
            InitializeComponent();
            DeserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            InstaciamientoVentanasAcoplables();
            CustomizedToolTip ToolTipPerzonalizado = new CustomizedToolTip(); ToolTipPerzonalizado.AutoSize = false; ToolTipPerzonalizado.Size = new Size(150,150);
            ToolTipPerzonalizado.SetToolTip(PB_Info, $" "); PB_Info.Tag = Properties.Resources.Nervio_EsquemaR;
            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Start();
            if (F_Base.Proyecto != null)
            {
                DireccionAnterior = F_Base.Proyecto.FiltroDireccionNervios;
            }
            CB_Direccion.DataSource = cFunctionsProgram.Direcciones.ToArray();
            CB_Direccion.SelectedItem = DireccionAnterior;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (F_Base.Proyecto != null && F_Base.Proyecto.Edificio != null && F_Base.Proyecto.Edificio.PisoSelect != null && F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                TB_r1.Enabled = !F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.BloquearNervio;
                TB_r2.Enabled = !F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.BloquearNervio;
                HabilitarMaestroSimilarA();

                if (!F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.BloquearNervio)
                {
                    CB_SeccionAltura.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Bool_CambioAltura;
                    CB_SeccionAncho.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Bool_CambioAncho;
                }
                else
                {
                    CB_SeccionAltura.Enabled = false;
                    CB_SeccionAncho.Enabled = false;
                }
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
            NerviosACargar();
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
      
        private void F_SelectNervio_Load(object sender, EventArgs e)
        {
            CargarListViewStories();
            CB_Direccion.SelectedItem = F_Base.Proyecto.FiltroDireccionNervios;
            DP_PanelContenedor.LoadFromXml(F_Base.Ruta_ConfiVentanasSelectNervioUsuario, DeserializeDockContent);
            cFunctionsProgram.CambiarSkins(DP_PanelContenedor);
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


        public void AcoplarVentana(ref F_PlantaNervios Dock)
        {
            if (!Dock.Created) { Dock = new F_PlantaNervios(); }
            Dock.Show(DP_PanelContenedor);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        public void AcoplarVentana(ref F_ModificadorDeRefuerzos Dock)
        {
            if (!Dock.Created) { Dock = new F_ModificadorDeRefuerzos(); }
            Dock.Show(DP_PanelContenedor);
            cFunctionsProgram.CambiarSkins(Dock);
        }
        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count > 0)
            {
            
                cPiso PisoSelect= F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                F_Base.Proyecto.Edificio.PisoSelect = PisoSelect;

                CargarListViewNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios);
                NerviosACargar(); ;
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
                ChangeComboBox();
                CB_SeccionAltura.SelectedItem = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAltura.ToString();
                CB_SeccionAncho.SelectedItem = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAncho.ToString();
                TB_r1.Text =string.Format("{0:0.00}",F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.r1);
                TB_r2.Text = string.Format("{0:0.00}", F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.r2);
                Habilitar_DeshabilitarNevioBorde(); 
            }
            else
            {
                Text = $"Selección de Nervios";
                GB_Propiedades.Text = "";
            }
            F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();F_Base.ActualizarVentanaF_VentanaDiseno();

            F_Base.F_PlantaNervios.Invalidate();
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

        eDireccion DireccionAnterior;
        private void CB_Direccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto != null)
            {
                DireccionAnterior = F_Base.Proyecto.FiltroDireccionNervios;
                F_Base.Proyecto.FiltroDireccionNervios = (eDireccion)CB_Direccion.SelectedItem;
                if (DireccionAnterior != F_Base.Proyecto.FiltroDireccionNervios)
                {
                    NerviosACargar();
                }
            }
        }
        private void NerviosACargar()
        {
            List<cNervio> NerviosSelects;

            var Direccion = F_Base.Proyecto.FiltroDireccionNervios;
            if (F_Base.Proyecto.Edificio!=null && F_Base.Proyecto.Edificio.PisoSelect !=null&&  F_Base.Proyecto.Edificio.PisoSelect.Nervios != null && F_Base.Proyecto.Edificio.PisoSelect.Nervios.Count > 0)
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

        private void RB_NervioBorde_CheckedChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.ActivarBoolNervioBorde)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.NervioBorde = CKB_NervioBorde.Checked;
            }
        }





        #region Para ventanas acoplables
        private void InstaciamientoVentanasAcoplables()
        {
            F_Base.F_PlantaNervios = new F_PlantaNervios();
            F_Base.F_ModificadorDeRefuerzos = new F_ModificadorDeRefuerzos();
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            DockContent dockContent;
            if (persistString == typeof(F_PlantaNervios).ToString())
                dockContent = F_Base.F_PlantaNervios;
            else if (persistString == typeof(F_ModificadorDeRefuerzos).ToString())
                dockContent = F_Base.F_ModificadorDeRefuerzos;
            else
            {
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;
                if (parsedStrings[0] != typeof(DockContent).ToString())
                    return null;

                dockContent = new F_PlantaNervios();
                if (parsedStrings[2] != string.Empty)
                    dockContent.Text = parsedStrings[2];

                return dockContent;
            }
            return dockContent;
        }

        #endregion
    }
}
