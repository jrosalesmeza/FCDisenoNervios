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
    public partial class F_NuevoProyecto : Form
    {
        public F_NuevoProyecto()
        {
            InitializeComponent();

        }

        private void BT_CargarE2K_Click(object sender, EventArgs e)
        {
            CrearObjetosETABS();

        }



        private void B_CargarCSV_Click(object sender, EventArgs e)
        {

        }



        private void CrearObjetosETABS()
        {
            Tuple<string, List<string>> Cargar = cFunctionsProgram.CagarArchivoTextoPlano("Archivo ETABS |*.e2k; *.$et", "Archivo de e2K, $et");
            if (Cargar != null)
            {
                TB_Ruta1.Text = Cargar.Item1;
                cFunctionsProgram.CrearObjetosEtabs(Cargar.Item2);
            }
        }



        #region Eventos Principales Ventana
        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);

        }
        #endregion

     
    }
}
