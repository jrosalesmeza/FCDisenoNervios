using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Ventana_Principal
{
    public partial class F_Inicio : Form
    {
        public F_Inicio()
        {
            InitializeComponent();
        }

        private int Contador = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Opacity < 1)
                Opacity += 0.03;
            Contador += 1;
            if (Contador == 60)
            {
                Timer1.Stop();
                Visible = false;
                AbrirFormularioPrincipal();
            }

        }

        private void AbrirFormularioPrincipal()
        {
            if (cFunctionsProgram.ComprobarAccesoPrograma())
            {
                if (!cFunctionsProgram.ComprobarVersionPrograma())
                    MessageBox.Show("Programa sin actualizar, comuníquese con el área de desarrollo de software para realizar la respectiva actualización.", cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                F_Base f_Base = new F_Base();
                f_Base.Show();
            }
            else
            {
                MessageBox.Show("Acceso Denegado", cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
        private void F_Inicio_Load(object sender, EventArgs e)
        {
            Opacity = 0;
            Timer1.Start();
        }
    }
}
