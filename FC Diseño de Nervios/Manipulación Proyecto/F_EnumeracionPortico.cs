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
    public partial class F_EnumeracionPortico :Form
    {


        public F_EnumeracionPortico()
        {
            InitializeComponent();
        }

        private void CargarListView()
        {
            cDatosEtabs DatosEtabs = F_Base.Proyecto.DatosEtabs;
            LV_Stories.Clear();
            foreach (cPiso Piso in DatosEtabs.Lista_Pisos)
            {
                ListViewItem item = new ListViewItem(Piso.Nombre);item.Name = Piso.Nombre;
                LV_Stories.Items.Add(item);
            }
            if (F_Base.Proyecto.DatosEtabs.PisoSelect!=null)
            {
                ListViewItem[] listViewItems = LV_Stories.Items.Find(F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre, false);
                listViewItems[0].Selected = true;

            }
            else
            {
                LV_Stories.Items[LV_Stories.Items.Count - 1].Selected = true;
           }

        }
        private void F_EnumPort_Load()
        {
            CargarListView();
        }

        #region Eventos Principales Ventana
        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);

        }
        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
        }


        #endregion

        private void F_EnumeracionPortico_Load(object sender, EventArgs e)
        {
            F_EnumPort_Load();

        }

        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count > 0)
            {
                F_Base.Proyecto.DatosEtabs.PisoSelect = F_Base.Proyecto.DatosEtabs.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
            }
        }






    }
}
