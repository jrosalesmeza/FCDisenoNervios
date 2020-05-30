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
    public partial class F_ModificacionEjes : Form
    {
        private List<cGrid> grids;
        public List<cGrid> Grids
        {
            get { return grids; }
            set
            {
                if(value!=null && grids!=value)
                {
                    grids = value;
                    CargarVentana();
                }
            }

        }
        
        
        public F_ModificacionEjes()
        {
            InitializeComponent();
                        
        }

        private void CargarVentana()
        {
            float BubbleSize;
            if (grids.Count > 0)
            {
                BubbleSize = grids.First().BubbleSize;
            }
            else
            {
                BubbleSize = 0.25f;
            }
            DGV_1.Rows.Clear();

            grids.ForEach(Grid =>
            {
                DGV_1.Rows.Add();
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_NombreEje.Name].Value = Grid.Nombre;
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_Localizacion.Name].Value = string.Format("{0:0.00}", Grid.CoordenadaInicial);
            }
            );
            TB_BurbujaSize.Text = string.Format("{0:0.00}", BubbleSize);
            cFunctionsProgram.EstiloDatGridView(DGV_1);
        }

        private void Agregar_Grid()
        {
            DGV_1.Rows.Add();

            if (DGV_1.Rows.Count>1)
            {
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_NombreEje.Name].Value = (DGV_1.Rows[DGV_1.Rows.Count - 2].Cells[C_NombreEje.Name].Value);
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_Localizacion.Name].Value = (DGV_1.Rows[DGV_1.Rows.Count - 2].Cells[C_Localizacion.Name].Value);
            }
            else
            {
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_NombreEje.Name].Value = "A";
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_Localizacion.Name].Value = 0;
            }
            cFunctionsProgram.EstiloDatGridView(DGV_1);
        }

        private void EliminarGrid()
        {
            List<int> IndicesElminar = new List<int>();

            for (int m = 0; m < DGV_1.SelectedCells.Count; m++)
            {
                IndicesElminar.Add(DGV_1.SelectedCells[m].RowIndex);
            }
            IndicesElminar = IndicesElminar.OrderByDescending(x => x).ToList();
            IndicesElminar.ForEach(x => DGV_1.Rows.RemoveAt(x));
        }
        private void CreateGrids(float BubbleSize)
        {
            grids.Clear();
            foreach (DataGridViewRow Row in DGV_1.Rows)
            {
                float.TryParse(Row.Cells[C_Localizacion.Name].Value.ToString(), out float Loc);
                cGrid Grid = new cGrid(Row.Cells[C_NombreEje.Name].Value.ToString(), Loc,eDireccionGrid.X, BubbleSize);
                grids.Add(Grid);
            }
            float ExtensionInferior = 0.2f;
            float Ymin = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Min(x => x.Vistas.Perfil_Original.Reales.Min(y => y.Y)) - ExtensionInferior;
            float Ymax = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Lista_Elementos.Max(x => x.Vistas.Perfil_Original.Reales.Max(y => y.Y));
            grids.ForEach(x => x.CrearRecta(0, Ymax, 0, Ymin));
        }


        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BT_Agregar_Click(object sender, EventArgs e)
        {
            Agregar_Grid();
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_BurbujaSize.Text, out float BubbleSize);
            if (BubbleSize>0 && BubbleSize<=0.4f)
            {
                F_Base.EnviarEstado_Nervio(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect);
                CreateGrids(BubbleSize);
                F_Base.ActualizarTodosLasVentanas();
                Close();
            }
            else
            {
                MessageBox.Show("Ingrese un valor entre 0 y 0.4 para \"Tamaño de Burbuja\"", cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
           
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliminarGrid();
        }

        private void DGV_1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex== C_NombreEje.Index)
            {
                DGV_1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DGV_1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
            }
        }
    }
}
