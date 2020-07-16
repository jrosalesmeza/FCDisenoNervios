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
    public partial class F_SelectNervioFunctions : Form
    {
        public enum eEditarSelectNervio
        {
            Diseno,
            AutoCAD
        }

        public static bool Aceptar = false;
        private eEditarSelectNervio EditSelectNervio { get; set; }
        private List<cNervio> Nervios;
        public F_SelectNervioFunctions(eEditarSelectNervio EditSelectNervio)
        {
            InitializeComponent();
            this.EditSelectNervio = EditSelectNervio;
            Nervios = F_Base.Proyecto.Edificio.PisoSelect.Nervios;
            CargarINFO(DGV_Info);
        }


        private void CargarINFO(DataGridView data)
        {
            LB_Title.Text = EditSelectNervio == eEditarSelectNervio.Diseno ? "Seleccionar Nervios a Diseñar" : "Seleccionar Nervios a Graficar";

            data.Rows.Clear();
            Nervios.ForEach(N => {
                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[C_NombreNervio.Index].Value = N.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_Seleccionar.Index].Value = EditSelectNervio == eEditarSelectNervio.Diseno ? N.Propiedades.Diseno : N.Propiedades.DibujoAutoCAD;           
            });
            cFunctionsProgram.EstiloDatGridView(data);
        }

        private void AceptarINFO(DataGridView data)
        {
            foreach(DataGridViewRow row in data.Rows)
            {
                cNervio N = Nervios.Find(y=>y.Nombre==row.Cells[C_NombreNervio.Index].Value.ToString());
                switch (EditSelectNervio)
                {
                    case eEditarSelectNervio.AutoCAD:
                        N.Propiedades.DibujoAutoCAD = (bool)row.Cells[C_Seleccionar.Index].Value;
                        break;
                    default:
                        N.Propiedades.Diseno = (bool)row.Cells[C_Seleccionar.Index].Value;
                        break;
                }
            }
        }




        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_Aceptar_Click(object sender, EventArgs e)
        {
            AceptarINFO(DGV_Info);
            Close();
            cNervio nervio=  EditSelectNervio== eEditarSelectNervio.Diseno?Nervios.Find(y => y.Propiedades.Diseno): Nervios.Find(y => y.Propiedades.DibujoAutoCAD);
            Aceptar = nervio != null;
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close(); Aceptar = false;
        }

        private void BT_Seleccionar_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.SeleccionDataGridView(DGV_Info, C_Seleccionar.Index, false);
        }
    }



}
