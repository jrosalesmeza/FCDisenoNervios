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

        Tuple<string, List<string>> CargarE2K;
        Tuple<string, List<string>> CargarCSV;
        public F_NuevoProyecto()
        {
            InitializeComponent();

        }

        private void BT_CargarE2K_Click(object sender, EventArgs e)
        {
            CargarRutaArchivoE2K();
        }
        private void CargarRutaArchivoE2K()
        {
            CargarE2K = cFunctionsProgram.CagarArchivoTextoPlano("Archivo ETABS |*.e2k; *.$et", "Archivo de e2K, $et");
            if (CargarE2K != null)
            {
                TB_Ruta1.Text = CargarE2K.Item1;
            }
        }

        private void B_CargarCSV_Click(object sender, EventArgs e)
        {
            CargarCSV = cFunctionsProgram.CagarArchivoTextoPlano("Archivo de Fuerzas |*.csv", "Archivo de Fuerzas, Unidades en |Ton- m|");
            if (CargarCSV != null)
            {
                TB_Ruta2.Text = CargarCSV.Item1;
            }
        }


        private bool VerificarArchivos()
        {
            if (CargarE2K != null && CargarCSV!=null)
            {
                List<string> Errores = cFunctionsProgram.ComprobarErroresArchivoE2K(CargarE2K.Item2);
                if (Errores.Count != 0)
                {
                    foreach (string Error in Errores)
                    {
                        MessageBox.Show(Error, cFunctionsProgram.Empresa, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }


 
        #region Eventos Principales Ventana
        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);

        }

        #endregion

        private void BT_Aceptar_Click(object sender, EventArgs e)
        {
            if (VerificarArchivos())
            {
                Visible = false;
                Close();
                F_Base.Proyecto = new cProyecto("Nuevo Proyecto");
                F_Base.Proyecto.DatosEtabs =  cFunctionsProgram.CrearObjetosEtabs(CargarE2K.Item2);
                F_Pisos _Pisos = new F_Pisos(F_Base.Proyecto.DatosEtabs);
                 CargarCSV = null; CargarE2K = null; TB_Ruta1.Text = ""; TB_Ruta2.Text = "";
                _Pisos.ShowDialog(F_Base.F_Base_);
            
            }
        }

        private void TB_Cancelar_Click(object sender, EventArgs e)
        {
            CargarCSV = null;CargarE2K = null;TB_Ruta1.Text = ""; TB_Ruta2.Text = "";
            Close();
        }
    }
}
