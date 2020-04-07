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
    public partial class F_ModificarSeccion : Form
    {
        public F_ModificarSeccion()
        {
            InitializeComponent();
        }

        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }
    }
}
