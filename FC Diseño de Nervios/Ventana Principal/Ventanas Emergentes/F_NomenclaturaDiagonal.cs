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
    public partial class F_NomenclaturaDiagonal : Form
    {
        public F_NomenclaturaDiagonal()
        {
            InitializeComponent();
            CB_Nomenclatura_DiagPosi.Items.Add(cFunctionsProgram.Nomenclaturas_Nervios.First()) ; CB_Nomenclatura_DiagPosi.Items.Add(cFunctionsProgram.Nomenclaturas_Nervios.Last());
            CB_Nomenclatura_DiagNeg.Items.Add(cFunctionsProgram.Nomenclaturas_Nervios.First()); CB_Nomenclatura_DiagNeg.Items.Add(cFunctionsProgram.Nomenclaturas_Nervios.Last());
        }
        public void CargarCB_Nomenclatura()
        {
            CB_Nomenclatura_DiagPosi.SelectedItem = F_Base.Proyecto.NomenclaturaDiagonalPositiva;
            CB_Nomenclatura_DiagNeg.SelectedItem = F_Base.Proyecto.NomenclaturaDiagonalNegativa;
        }
        private void CB_Nomenclatura_DiagPosi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Nomenclatura_DiagNeg.SelectedItem!=null && CB_Nomenclatura_DiagPosi.SelectedItem !=null&& CB_Nomenclatura_DiagNeg.SelectedItem.ToString() == CB_Nomenclatura_DiagPosi.SelectedItem.ToString())
            {
                CB_Nomenclatura_DiagNeg.SelectedItem = (eNomenclatura)CB_Nomenclatura_DiagPosi.SelectedItem == eNomenclatura.Alfabética ? eNomenclatura.Numérica : eNomenclatura.Alfabética;
            }
        }
        private void CB_Nomenclatura_DiagNeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Nomenclatura_DiagNeg.SelectedItem != null && CB_Nomenclatura_DiagPosi.SelectedItem != null && CB_Nomenclatura_DiagNeg.SelectedItem.ToString() == CB_Nomenclatura_DiagPosi.SelectedItem.ToString())
            {
                CB_Nomenclatura_DiagPosi.SelectedItem = (eNomenclatura)CB_Nomenclatura_DiagPosi.SelectedItem == eNomenclatura.Alfabética ? eNomenclatura.Numérica : eNomenclatura.Alfabética;
            }
        }
        private void F_NomenclaturaDiagonal_Load(object sender, EventArgs e)
        {
            CargarCB_Nomenclatura();
        }

        private void BT_Ok_Click(object sender, EventArgs e)
        {
            F_Base.Proyecto.NomenclaturaDiagonalPositiva = (eNomenclatura)CB_Nomenclatura_DiagPosi.SelectedItem;
            F_Base.Proyecto.NomenclaturaDiagonalNegativa = (eNomenclatura)CB_Nomenclatura_DiagNeg.SelectedItem;
            Close();
        }

 
    }
}
