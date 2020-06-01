using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Ventana_Principal.Nuevo_Proyecto
{
    public partial class F_EditarPisos : Form
    {

        private List<cPiso> Pisos;

        public F_EditarPisos(List<cPiso> pisos)
        {
            InitializeComponent();
            Pisos = pisos;
        }


        private void CrearDGV(DataGridView data)
        {
            data.Rows.Clear();

            Pisos.ForEach(Piso => {

                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[C_NPiso.Index].Value = Piso.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_NombreRealPiso.Index].Value = Piso.NombreReal;
                data.Rows[data.Rows.Count - 1].Cells[C_Nivel.Index].Value = Piso.Nivel;
            });

            cFunctionsProgram.EstiloDatGridView(data);
        }


        private void AceptarCambios()
        {
            foreach(DataGridViewRow row in DGV_1.Rows)
            {
                string Nivel = row.Cells[C_Nivel.Index].Value.ToString();
                string NombreReal = row.Cells[C_NombreRealPiso.Index].Value.ToString();
                string Nombre = row.Cells[C_NPiso.Index].Value.ToString();
                cPiso Piso = Pisos.Find(x => x.Nombre == Nombre);
                Piso.Nivel = Nivel;Piso.NombreReal = NombreReal;
            }
        }

        private void F_EditarPisos_Load(object sender, EventArgs e)
        {
            CrearDGV(DGV_1);
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            AceptarCambios();
            Close();
        }

        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }
    }
}
