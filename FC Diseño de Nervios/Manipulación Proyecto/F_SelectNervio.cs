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
        }


        private void CargarListViewStories()
        {
            LV_Stories.Clear();
            foreach (cPiso Piso in F_Base.Proyecto.Edificio.Lista_Pisos)
            {
                ListViewItem item = new ListViewItem(Piso.Nombre); item.Name = Piso.Nombre;
                LV_Stories.Items.Add(item);
            }
            if (F_Base.Proyecto.Edificio.PisoSelect != null)
            {
                ListViewItem[] listViewItems = LV_Stories.Items.Find(F_Base.Proyecto.Edificio.PisoSelect.Nombre, false);
                listViewItems[0].Selected = true;

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
                    ListViewItem[] listViewItems = LV_Nervios.Items.Find(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre, false);
                    if (listViewItems.Count() > 0)
                    {
                        listViewItems[0].Selected = true;
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
            F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.CrearPuntosPlantaEscaladaEtabs(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.Paint(e.Graphics, Zoom));
            F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.PaintPlantaEscaladaEtabsLine(e.Graphics); });
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {

                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom))); ;
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Paint_Planta_ElementosEnumerados(e.Graphics));
            }
        }

        private void F_SelectNervio_Load(object sender, EventArgs e)
        {
            CargarListViewStories();
            CB_Direccion.SelectedItem = eDireccion.Todos.ToString(); 

        }

        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count > 0)
            {
                F_Base.Proyecto.Edificio.PisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                CargarListViewNervios(F_Base.Proyecto.Edificio.PisoSelect.Nervios);
                NerviosACargar();
                SelectNervioChanged(new Point(), false);
            }
        }
        private void LV_Nervios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Nervios.SelectedItems.Count > 0)
            {
                SelectNervioChanged(new Point(0,0), false, LV_Nervios.SelectedItems[0].Text);
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

        private void SelectNervioChanged(Point Location, bool MouseDown, string NombreABuscar = "")
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
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x != F_Base.Proyecto.Edificio.PisoSelect.NervioSelect).ForEach(x => { x.Select = false; x.ChangeSelect(); });
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Select = true;
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.ChangeSelect();

                ListViewItem[] listViewItem = LV_Nervios.Items.Find(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre, false);
                GB_Propiedades.Text = $" {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
                Text = $"Selección de Nervios | {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre} ";
                CB_SeccionAltura.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Bool_CambioAltura;
                CB_SeccionAncho.Enabled = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Bool_CambioAncho;
                ChangeComboBox();
                CB_SeccionAltura.SelectedItem = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAltura.ToString();
                CB_SeccionAncho.SelectedItem = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAncho.ToString();
                F_Base.AcutalizarVentanaF_NervioEnPerfilLongitudinal();
            }
            else
            {
                Text = $"Selección de Nervios";
                GB_Propiedades.Text = "";
            }
            PB_Nervios.Invalidate();
        }

        private void CB_SeccionAltura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CambioenAltura = cFunctionsProgram.ConvertirStringtoeCambioAlto(CB_SeccionAltura.Text);
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.CrearCoordenadasPerfilLongitudinalReales();
                F_Base.AcutalizarVentanaF_NervioEnPerfilLongitudinal();
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
                    NerviosSelects = F_Base.Proyecto.Edificio.PisoSelect.Nervios;
                }
                else
                {
                    NerviosSelects = F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x.Direccion == Direccion);
                }
                CargarListViewNervios(NerviosSelects);
            }
        }

    }
}
