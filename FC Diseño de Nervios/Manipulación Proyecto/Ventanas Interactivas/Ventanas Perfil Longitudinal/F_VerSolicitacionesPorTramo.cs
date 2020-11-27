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
        public F_VerSolicitacionesPorTramo(cSubTramo subTramoSelect)
        {
            InitializeComponent();
            this.subTramoSelect = subTramoSelect;
            Text = $"     Solicitaciones: {subTramoSelect.Nombre}";
        }


        private void BT_1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
