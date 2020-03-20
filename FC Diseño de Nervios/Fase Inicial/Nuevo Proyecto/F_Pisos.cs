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
    public partial class F_Pisos : Form
    {

        public cDatosEtabs DatosEtabs;

        public static List<cPiso> PisosSelects;
        private List<cPiso> PisosNoSelects;
        public F_Pisos(cDatosEtabs DatosEtabs)
        {
            this.DatosEtabs = DatosEtabs;
            InitializeComponent();
            LlenarListView();
        }


        private void LlenarListView()
        {
            foreach (cPiso Piso in DatosEtabs.Lista_Pisos)
            {
                if (Piso.Lista_Lines.Count != 0)
                {
                    ListViewItem item = new ListViewItem(Piso.Nombre);
                    LV_Pisos.Items.Add(item);
                }
            }
        }

        private void LV_Pisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            PisosSelects = new List<cPiso>(); PisosNoSelects = new List<cPiso>();
            foreach (ListViewItem Item in LV_Pisos.SelectedItems)
            {
                PisosSelects.Add(DatosEtabs.Lista_Pisos.Find(x => x.Nombre == Item.Text));
            }
            foreach (cPiso Piso in DatosEtabs.Lista_Pisos)
            {
                if (PisosSelects.Contains(Piso) == false)
                {
                    PisosNoSelects.Add(Piso);
                }

            }
            

        }

        #region Eventos Ventana
        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);

        }

        #endregion

        private void BT_Aceptar_Click(object sender, EventArgs e)
        {
            if (PisosSelects != null && PisosSelects.Count != 0)
            {

                foreach (cPiso PisoNoSelect in PisosNoSelects)
                {
                    F_Base.Proyecto.DatosEtabs.Lista_Pisos.Remove(PisoNoSelect);
                }
                Visible = false;
                F_Base.F_EnumeracionPortico = new F_EnumeracionPortico();
                F_Base.F_EnumeracionPortico.ShowDialog();
                Close();
            }
        }


    }
}
