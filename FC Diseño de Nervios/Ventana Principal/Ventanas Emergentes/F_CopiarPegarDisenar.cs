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
    public partial class F_CopiarPegarDisenar : Form
    {
        public static eTipoRefuerzo TipoRefuerzo;
        public F_CopiarPegarDisenar()
        {
            InitializeComponent();
            GB_1.KeyPress += F_CopiarPegarDisenar_KeyPress;
            CKB_RLongitudinal.KeyPress += F_CopiarPegarDisenar_KeyPress;
            CKB_RTransversal.KeyPress += F_CopiarPegarDisenar_KeyPress;
            CargarForm();
        }


        private void CargarForm()
        {
            switch (TipoRefuerzo)
            {
                case eTipoRefuerzo.Longitudinal:
                    CKB_RLongitudinal.Checked = true;
                    break;
                case eTipoRefuerzo.Transversal:
                    CKB_RTransversal.Checked = true;
                    break;
                case eTipoRefuerzo.Todo:
                    CKB_RLongitudinal.Checked = true;
                    CKB_RTransversal.Checked = true;
                    break;
                default:
                    break;
            }

        }
        private void CloseForm()
        {
            if (CKB_RLongitudinal.Checked)
                TipoRefuerzo = eTipoRefuerzo.Longitudinal;
            if (CKB_RTransversal.Checked)
                TipoRefuerzo = eTipoRefuerzo.Transversal;
            if (CKB_RLongitudinal.Checked && CKB_RTransversal.Checked)
                TipoRefuerzo = eTipoRefuerzo.Todo;
        }


        private void F_CopiarPegarDisenar_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseForm();
        }
        private void F_CopiarPegarDisenar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)Keys.Enter)    
               Close();
        }

        private void CKB_RLongitudinal_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == CKB_RLongitudinal)
            {
                if (!CKB_RLongitudinal.Checked && !CKB_RTransversal.Checked)
                    CKB_RTransversal.Checked = true;
            }else if (sender == CKB_RTransversal)
            {
                if (!CKB_RLongitudinal.Checked && !CKB_RTransversal.Checked)
                    CKB_RLongitudinal.Checked = true;
            }
        }

        private void BT_Aceptar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
