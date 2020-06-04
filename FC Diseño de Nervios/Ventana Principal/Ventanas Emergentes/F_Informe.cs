using FC_Diseño_de_Nervios.Controles;
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
    public partial class F_Informe : Form
    {


        List<cNervio> Nervios { get; set; }
        public F_Informe(List<cNervio> Nervios)
        {
            this.Nervios = Nervios;
            InitializeComponent();
        }

        public void CargarDagridView(DataGridView data)
        {
            data.Rows.Clear();
            Bitmap B = new Bitmap(Properties.Resources.B, 10, 10);
            Bitmap X= new Bitmap(Properties.Resources.X, 10, 10);
            Nervios.ForEach(Nervio => {

                data.Rows.Add();
                cFunctionsProgram.AgregarEstiloRow(data.Rows[data.Rows.Count - 1]);
                data.Rows[data.Rows.Count - 1].Cells[C_NombreNervio.Index].Value = Nervio.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_Diseno.Index].Value = Nervio.Resultados.Diseñado ? B :X ;

                DataGridViewRichTextBoxCell cell = (DataGridViewRichTextBoxCell)data.Rows[data.Rows.Count - 1].Cells[C_Observacion.Index];

                if(Nervio.Resultados.Errores.Count>1)
                    data.Rows[data.Rows.Count - 1].Height = (data.Rows[data.Rows.Count - 1].DefaultCellStyle.Font.Height+2) * Nervio.Resultados.Errores.Count;

                foreach (string line in Nervio.Resultados.Errores)
                {
                    cell.Value += $"  {line}" + Environment.NewLine;

                }
            });

            cFunctionsProgram.EstiloDatGridView(data,false);
        }
        private void F_Informe_Load(object sender, EventArgs e)
        {
            CargarDagridView(DGV_Info);
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }
    }
}
