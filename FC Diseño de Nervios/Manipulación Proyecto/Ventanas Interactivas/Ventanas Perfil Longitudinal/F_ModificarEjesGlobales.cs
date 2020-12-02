using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Manipulación_Proyecto.Ventanas_Interactivas.Ventanas_Perfil_Longitudinal
{
    public partial class F_ModificarEjesGlobales : Form
    {

        List<cGrid> ListaX;
        List<cGrid> ListaY;

        public F_ModificarEjesGlobales()
        {
            InitializeComponent();
            DGV_X.CellEndEdit += DGV_CellEndEdit;
            DGV_Y.CellEndEdit += DGV_CellEndEdit;
        }

        private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data = (DataGridView)sender;
            if (data != null)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == C_NombreEje.Index)
                {
                    data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
                }
                if (e.RowIndex >= 0 && e.ColumnIndex == C_Localizacion.Index)
                {
                    float.TryParse(data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),out float Coord);
                    data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Format("{0:0.00}", Coord);
                }
            }
        }

        private void LoadListasGrids()
        {
            ListaX = new List<cGrid>();
            ListaY = new List<cGrid>();

            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(Grid => {

                if (Grid.Direccion == eDireccionGrid.X)
                    ListaX.Add(cFunctionsProgram.DeepClone(Grid));
                else
                    ListaY.Add(cFunctionsProgram.DeepClone(Grid));            
            });
            ListaX = ListaX.OrderBy(x => x.CoordenadaInicial).ToList();
            ListaY = ListaY.OrderBy(x => x.CoordenadaInicial).ToList();
        }

        private void CrearGrids2(DataGridView data,List<cGrid> Grids,eDireccionGrid Direccion)
        {
            Grids.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string NombreGrid = data.Rows[i].Cells[C_NEjeY.Index].Value.ToString() ;
                float Coordenada = Convert.ToSingle(data.Rows[i].Cells[C_Localizacion.Index].Value.ToString());
                cGrid Grid = new cGrid(NombreGrid, Coordenada, Direccion, cVariables.BubblesizePlano);
                Grids.Add(Grid);
            }
        }
        private void CrearGrids1()
        {
            CrearGrids2(DGV_X, ListaX, eDireccionGrid.X);
            CrearGrids2(DGV_Y, ListaY, eDireccionGrid.Y); List<cGrid> Grids = new List<cGrid>();
            Grids.AddRange(ListaX); Grids.AddRange(ListaY);
            List<float> X = Grids.FindAll(x => x.Direccion == eDireccionGrid.X).Select(x => x.CoordenadaInicial).ToList();
            List<float> Y = Grids.FindAll(x => x.Direccion == eDireccionGrid.Y).Select(x => x.CoordenadaInicial).ToList();
            Grids.ForEach(x => x.CrearRecta(X.Max(), Y.Max(), X.Min(), Y.Min()));
            F_Base.Proyecto.Edificio.Lista_Grids = cFunctionsProgram.DeepClone(Grids);
            F_Base.ActualizarVentanaF_SelectNervio();
        }
        private void CargarDGVs(DataGridView data,List<cGrid> Grids)
        {
            data.Rows.Clear();

            Grids.ForEach(Grid => {
                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[C_NombreEje.Index].Value = Grid.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_LocalizaciónY.Index].Value = string.Format("{0:0.00}", Grid.CoordenadaInicial);
            });
            cFunctionsProgram.EstiloDatGridView(data);
        }

        private void AgregarEjeDGV(DataGridView data, List<cGrid> Grids)
        {
            data.Rows.Add();
            if (Grids.Count > 0)
            {
                data.Rows[data.Rows.Count - 1].Cells[C_NombreEje.Index].Value = Grids.Last().Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_LocalizaciónY.Index].Value = string.Format("{0:0.00}", Grids.Last().CoordenadaInicial);
            }
            else
            {
                data.Rows[data.Rows.Count - 1].Cells[C_NombreEje.Index].Value = "A";
                data.Rows[data.Rows.Count - 1].Cells[C_LocalizaciónY.Index].Value = string.Format("{0:0.00}", 0);
            }
            cFunctionsProgram.EstiloDatGridView(data);
        }

        private void EliminarEjeDGV(DataGridView data)
        {
            if (data.SelectedCells.Count > 0)
            {
                if (data.Rows.Count > 2)
                {
                    List<int> Enteros = new List<int>();
                    foreach (DataGridViewCell cell in data.SelectedCells)
                    {
                        Enteros.Add(cell.RowIndex);
                    }
                    Enteros = Enteros.Distinct().OrderByDescending(y=>y).ToList();
                    Enteros.ForEach(x => data.Rows.Remove(data.Rows[x]));
                }
                else
                {
                    MessageBox.Show("Deben existir por lo menos dos ejes.", cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void F_ModificarEjesGlobales_Load(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterParent;
            LoadListasGrids();
            CargarDGVs(DGV_X, ListaX); CargarDGVs(DGV_Y, ListaY);

        }
        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_AgregarX_Click(object sender, EventArgs e)
        {
            AgregarEjeDGV(DGV_X, ListaX);
        }

        private void BT_AgregarY_Click(object sender, EventArgs e)
        {
            AgregarEjeDGV(DGV_Y, ListaY);
        }

        private void BT_EliminarX_Click(object sender, EventArgs e)
        {
            EliminarEjeDGV(DGV_X);
        }

        private void BT_EliminarY_Click(object sender, EventArgs e)
        {
            EliminarEjeDGV(DGV_Y);
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            F_Base.EnviarEstadoVacio();
            CrearGrids1();Close();
        }
        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
