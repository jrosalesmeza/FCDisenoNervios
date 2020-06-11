using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes
{
    public partial class F_SimilitudNervios : Form
    {

        public List<cNervio> Nervios;

        public F_SimilitudNervios(List<cNervio> Nervios)
        {
            this.Nervios = Nervios;
            InitializeComponent();
        }

        public void CargarDGV(DataGridView data)
        {
            data.Rows.Clear();

            List<cNervio> NerviosMaestros = Nervios.FindAll(y => y.Maestro.IsMaestroGeometria);
            List<string> NerviosMaestrosString = new List<string>();
            if (NerviosMaestros != null && NerviosMaestros.Count != 0)
                NerviosMaestrosString = NerviosMaestros.Select(y => y.Nombre).ToList();
            Nervios.ForEach(Nervio =>
            {
                data.Rows.Add();

                data.Rows[data.Rows.Count - 1].Cells[C_NombreNervio.Index].Value = Nervio.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_Maestro.Index].Value = Nervio.Maestro.IsMaestroGeometria;

                if (Nervio.Maestro.BoolSoySimiarA)
                {
                    DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)data.Rows[data.Rows.Count - 1].Cells[C_Similara.Index];
                    boxCell.Items.AddRange(NerviosMaestrosString.ToArray());
                    data.Rows[data.Rows.Count - 1].Cells[C_Similara.Index].Value = Nervio.Maestro.SoySimiarA;
                }
                else
                {
                    DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)data.Rows[data.Rows.Count - 1].Cells[C_Similara.Index];
                    boxCell.Items.Add(Nervio.Maestro.SoySimiarA);
                    data.Rows[data.Rows.Count - 1].Cells[C_Similara.Index].Value = Nervio.Maestro.SoySimiarA;
                }


            });

            cFunctionsProgram.EstiloDatGridView(data);
        }


        private void P_2_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void F_SimilitudNervios_Load(object sender, EventArgs e)
        {
            CargarDGV(DGV_1);
        }

        private void DGV_1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0 && e.ColumnIndex== C_Maestro.Index)
            {
                if ((bool)DGV_1.Rows[e.RowIndex].Cells[C_Maestro.Index].Value)
                {
                    DGV_1.Rows[e.RowIndex].Cells[C_Similara.Index].ReadOnly = true;
                    DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)DGV_1.Rows[e.RowIndex].Cells[C_Similara.Index];
                    boxCell.Items.Clear(); boxCell.Items.Add(" ");
                    DGV_1.Rows[e.RowIndex].Cells[C_Similara.Index].Value = " ";
                }
                else
                {
                    DGV_1.Rows[e.RowIndex].Cells[C_Similara.Index].ReadOnly = false;
                    
                }
                MaestrosEnDGV(DGV_1);
            }
        }

        private void MaestrosEnDGV(DataGridView data)
        {
            List<string> Maestros = new List<string>(); Maestros.Add(" ");
            foreach (DataGridViewRow row in data.Rows)
            {
                if ((bool)row.Cells[C_Maestro.Index].Value)
                    Maestros.Add(row.Cells[C_NombreNervio.Index].Value.ToString());
            }
            foreach (DataGridViewRow row in data.Rows)
            {
                if (Maestros.Count > 0)
                {
                    if (!(bool)row.Cells[C_Maestro.Index].Value)
                    {
                        DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)row.Cells[C_Similara.Index];
                        boxCell.Items.Clear();
                        boxCell.Items.AddRange(Maestros.ToArray());
                        if (!Maestros.Contains(row.Cells[C_Similara.Index].Value))
                            row.Cells[C_Similara.Index].Value = null;
                    }
                }
                else
                {
                    DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)row.Cells[C_Similara.Index];
                    boxCell.Items.Clear(); boxCell.Items.Add(" ");
                    row.Cells[C_Similara.Index].Value = " ";
                }
            }


        }
        private void ConfirmarSimilitudes(DataGridView data)
        {
            List<string> MensajeAlerta = new List<string>();
            foreach (DataGridViewRow row in data.Rows)
            {
                string NombreNervio = row.Cells[C_NombreNervio.Index].Value.ToString();
                bool IsMaestro = (bool)row.Cells[C_Maestro.Index].Value;
                string SoySimilarA = "";
                if (row.Cells[C_Similara.Index].Value != null)
                    SoySimilarA = row.Cells[C_Similara.Index].Value.ToString();
                cNervio NervioMaestro = Nervios.Find(x => x.Nombre == SoySimilarA);
                cNervio NervioSimilar = Nervios.Find(x => x.Nombre == NombreNervio);
                cFunctionsProgram.AsignarSimilitud(NervioMaestro, NervioSimilar, ref MensajeAlerta);

            }
            RB_Alerta.Clear();
            RB_Alerta.Lines = MensajeAlerta.ToArray();
            if (MensajeAlerta.Count == 0)
                Close();
        }
        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            ConfirmarSimilitudes(DGV_1);F_Base.ActualizarVentanaF_SelectNervio();
        }
    }
}
