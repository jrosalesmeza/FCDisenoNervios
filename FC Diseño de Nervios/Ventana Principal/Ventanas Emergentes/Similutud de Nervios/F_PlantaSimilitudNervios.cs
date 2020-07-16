using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes.Similutud_de_Nervios
{
    public partial class F_PlantaSimilitudNervios : Form
    {

        private float Dx, Dy = 0;
        private float Zoom = 1;

        private cPiso PisoSelect;
        private List<cPiso> Pisos;
        private List<cGrid> Grids;


        public F_PlantaSimilitudNervios(cEdificio Edificio)
        {
            InitializeComponent();
            PB_Planta.Paint += PB_Planta_Paint;
            P_Title.MouseDown += P_Title_MouseDown; LB_Title.MouseDown += P_Title_MouseDown;PB_Icono.MouseDown += P_Title_MouseDown;
            PB_Planta.MouseMove += PB_Planta_MouseMove;
            Pisos = Edificio.Lista_Pisos;
            Grids = Edificio.Lista_Grids;
            LB_Nervio.Text = "";
            LoadLV(LV_Stories);
        }

 

        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void LoadLV(ListView LV)
        {
            LV.Items.Clear();

            foreach(cPiso Piso in Pisos)
            {
                ListViewItem Item = new ListViewItem(); Item.Text = Piso.Nombre;
                LV.Items.Add(Item);
            }
            LV.Items[LV.Items.Count - 1].Selected = true;
        }

        private void PB_Planta_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.Clear(Color.White);
            if (PisoSelect != null)
            {
                float XI = 5f; float YI = 5f;
                float WidthPB = PB_Planta.Width - XI * 3;
                float HeightPB = PB_Planta.Height - YI * 3;

                List<PointF> PointsSinEscalar = new List<PointF>();

                Grids.ForEach(y => PointsSinEscalar.AddRange(y.Recta_Real));

                PisoSelect.Lista_Lines.ForEach(y => PointsSinEscalar.AddRange(y.Planta_Real));
                PisoSelect.Nervios.ForEach(x => x.Lista_Objetos.ForEach(y => PointsSinEscalar.AddRange(y.Line.Planta_Real)));


                PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
                Grids.ForEach(y => y.CrearPuntosPlantaEscaladaEtabs(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
                PisoSelect.Nervios.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom)));




                Grids.ForEach(y => y.Paint(e.Graphics, Zoom));
                PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.PaintPlantaEscaladaEtabsLine(e.Graphics); });
                PisoSelect.Nervios.ForEach(y => y.Paint_Similares(e.Graphics));

            }
        }

        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PB_Planta_MouseDown(object sender, MouseEventArgs e)
        {
            PisoSelect.Nervios.ForEach(y => y.MouseDownSelectSimilar(new Point(e.X, e.Y)));
            PB_Planta.Invalidate();
        }
        private void PB_Planta_MouseMove(object sender, MouseEventArgs e)
        {
            cNervio FindNervio = PisoSelect.Nervios.Find(y => y.MouseDownSelectSimilar(new Point(e.X, e.Y),false));
            if (FindNervio != null)
                LB_Nervio.Text = FindNervio.Nombre;
            else
                LB_Nervio.Text = "";
        }
        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count == 1)
            {
                PisoSelect = Pisos.Find(y => y.Nombre == LV_Stories.SelectedItems[0].Text);
                if (PisoSelect!=null && LV_Stories.SelectedItems[0].Text != PisoSelect.Nombre)
                {
                    PB_Planta.Invalidate();
                }
                else if (Pisos==null)
                {
                    PB_Planta.Invalidate();
                }
            }
            
        }
    }
}
