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
            CB_Direccion.SelectedIndex = 0;
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

        }
        private void CargarListViewNervios(cPiso PisoSelect)
        {
            LV_Nervios.Clear();
            if (PisoSelect != null)
            {
                if (PisoSelect.Nervios != null)
                {
                    foreach (cNervio Nervio in PisoSelect.Nervios)
                    {
                        LV_Nervios.Items.Add(Nervio.Nombre);
                    }

                    if (F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
                    {
                        ListViewItem[] listViewItems = LV_Nervios.Items.Find(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre, false);
                        listViewItems[0].Selected = true;

                    }
                    else
                    {
                        LV_Nervios.Items[LV_Nervios.Items.Count - 1].Selected = true;
                    }

                }
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
        }

        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count > 0)
            {
                F_Base.Proyecto.Edificio.PisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                CargarListViewNervios(F_Base.Proyecto.Edificio.PisoSelect);
                GB_Propiedades.Text = $" | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
                PB_Nervios.Invalidate();
            }
        }
        private void LV_Nervios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Nervios.SelectedItems.Count > 0)
            {
                if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
                {
                    F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.Nombre == LV_Nervios.SelectedItems[0].Text);
                    GB_Propiedades.Text = $" {F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Nombre} | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
                    PB_Nervios.Invalidate();
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
        private void PB_Nervios_MouseDown_1(object sender, MouseEventArgs e)
        {

            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null && F_Base.Proyecto.Edificio.PisoSelect.Nervios.Count > 0)
            {
                F_Base.Proyecto.Edificio.PisoSelect.NervioSelect = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Find(x => x.MouseDownSelect(e.Location));
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.FindAll(x => x != F_Base.Proyecto.Edificio.PisoSelect.NervioSelect).ForEach(x => { x.Select = false;x.ChangeSelect(); });
            }
            PB_Nervios.Invalidate();

        }
        private void F_SelectNervio_Resize(object sender, EventArgs e)
        {
            PB_Nervios.Invalidate();
        }



    }
}
