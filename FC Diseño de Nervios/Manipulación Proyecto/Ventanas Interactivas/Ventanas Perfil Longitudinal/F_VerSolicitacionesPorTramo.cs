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
    public partial class F_VerSolicitacionesPorTramo : Form
    {
        private cSubTramo subTramoSelect;
        public cSubTramo SubTramoSelect
        {
            get
            {
                return subTramoSelect;
            }
            set
            {
                if (subTramoSelect != value)
                {
                    if(subTramoSelect!=null)
                        subTramoSelect.Vistas.SelectPerfilLongitudinal = false;
                    subTramoSelect = value;
                    ConectarTabla();
                }
            }
        }
        public F_VerSolicitacionesPorTramo()
        {
            InitializeComponent();
            ActiveControl = LB_TitleTramo;
            DGV_1.KeyPress += F_VerSolicitacionesPorTramo_KeyPress;
            LB_TitleTramo.KeyPress += F_VerSolicitacionesPorTramo_KeyPress;
            FormClosing += F_VerSolicitacionesPorTramo_FormClosing;
        }

        private void F_VerSolicitacionesPorTramo_FormClosing(object sender, FormClosingEventArgs e)
        {
            subTramoSelect.Vistas.SelectPerfilLongitudinal = false;
            F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();
        }

        private void ConectarTabla()
        {
            Text = $"Solicitaciones: {subTramoSelect.TramoOrigen.Nombre} - {subTramoSelect.Nombre}";
            LB_TitleTramo.Text = $"      Solicitaciones: {subTramoSelect.TramoOrigen.Nombre} - {subTramoSelect.Nombre}";
            DGV_1.DataSource = cFuncionesVerSolciitacionesPorTramo.TablaSolicitaciones(subTramoSelect, F_Base.Proyecto.CantidadEstacionesInterpolar);
            DGV_1.ReadOnly = true;
            EstiloDataGridView(DGV_1);
           
        }


        private void EstiloDataGridView(DataGridView data)
        {
            DataGridViewCellStyle StyleC = new DataGridViewCellStyle();
            StyleC.Alignment = DataGridViewContentAlignment.MiddleLeft;
            StyleC.Font = new Font("Calibri", 8, FontStyle.Bold);

            DataGridViewCellStyle StyleR = new DataGridViewCellStyle();
            StyleR.Alignment = DataGridViewContentAlignment.MiddleLeft;
            StyleR.Font = new Font("Calibri", 8, FontStyle.Bold);

            data.Columns[0].Frozen = true;
            data.Columns[0].Width = 110;
            data.Columns[0].HeaderCell.Style = StyleC;
            foreach(DataGridViewRow row in data.Rows)
            {
                row.Cells[0].Style = StyleR;
                row.Resizable = DataGridViewTriState.False;

            }
            foreach (DataGridViewColumn col in data.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.Resizable = DataGridViewTriState.False;

            }

            

        }



        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void F_VerSolicitacionesPorTramo_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(e.KeyChar== (char)27)
            {
                BT_Cerrar_Click(null, e);
            }
        }
    }
}
