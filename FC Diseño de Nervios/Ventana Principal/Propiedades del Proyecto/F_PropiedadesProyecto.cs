using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios
{
    public partial class F_PropiedadesProyecto : Form
    {
        public F_PropiedadesProyecto()
        {
            InitializeComponent();
        }

        private void LoadWindows()
        {
            if (F_Base.Proyecto != null)
            {
                foreach (Control control in TP1_General.Controls)
                    control.Enabled = true;
                CargarTabPageGeneral();

            }
            else
            {
                foreach (Control control in TP1_General.Controls)
                    control.Enabled = false;
            }

        }

        private void CargarTabPageGeneral()
        {
            CKB_LabelsBarras.Checked = F_Base.Proyecto.EtiquetasDeBarras;
            CKB_CotaInteligente.Checked = F_Base.Proyecto.AcotamientoInteligente;
        }


        private void AplicarCambios()
        {
            if (F_Base.Proyecto != null)
            {
                F_Base.Proyecto.EtiquetasDeBarras = CKB_LabelsBarras.Checked;
                F_Base.Proyecto.AcotamientoInteligente = CKB_CotaInteligente.Checked;
            }
            F_Base.ActualizarTodosLasVentanas();
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            AplicarCambios();
            Close();
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void F_PropiedadesProyecto_Load(object sender, EventArgs e)
        {
            LoadWindows();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }
    }
}
