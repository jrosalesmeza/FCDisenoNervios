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
    public partial class F_SelectCombinaciones : Form
    {

        private cNervio nervioSelect;
        public cNervio NervioSelect
        {
            get { return nervioSelect; }
            set
            {
                if (nervioSelect != value && value != null)
                {
                    nervioSelect = value;
                    LB_Nervio.Text = nervioSelect.Nombre;
                    CargarDGV();
                }
            }
        }
        public F_SelectCombinaciones()
        {
            InitializeComponent();
        }

        private void CargarDGV()
        {
             DGV_1.Rows.Clear();
            cSubTramo SubTramo = (cSubTramo)nervioSelect.Lista_Elementos.First(x=> x is cSubTramo);
            List<cSolicitacion> Solicitaciones = SubTramo.Estaciones.First().Lista_Solicitaciones; ;
            Solicitaciones.ForEach(Solicitacion => {

                DGV_1.Rows.Add();
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_NombreCombinacion.Name].Value = Solicitacion.Nombre;
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_CheckCombinacion.Name].Value = Solicitacion.SelectEnvolvente;
            });
            cFunctionsProgram.EstiloDatGridView(DGV_1);
        }

        private void ConfirmarCambios()
        {
            F_Base.EnviarEstado_Nervio(nervioSelect);
            List<IElemento> Subtramos =nervioSelect.Lista_Elementos.FindAll(x => x is cSubTramo).ToList();
            for (int i = 0; i < DGV_1.Rows.Count; i++)
            {
                Subtramos.ForEach(subtramo =>
                {
                    cSubTramo SubTramoAux = (cSubTramo)subtramo;
                    SubTramoAux.Estaciones.ForEach(x => x.Lista_Solicitaciones.Find(y => y.Nombre == (string)DGV_1.Rows[i].Cells[C_NombreCombinacion.Index].Value).SelectEnvolvente = (bool)DGV_1.Rows[i].Cells[C_CheckCombinacion.Index].Value);
                });
            }
            nervioSelect.CrearEnvolvente();
            nervioSelect.CrearAceroAsignadoRefuerzoLongitudinal();
        }

        private void DGV_1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == C_CheckCombinacion.Index)
            {
                int BooleanosContador = 0;
                for (int i = 0; i < DGV_1.Rows.Count; i++)
                {
                    if ((bool)DGV_1.Rows[i].Cells[C_CheckCombinacion.Index].Value)
                    {
                        BooleanosContador += 1;
                    }
                }
                if (BooleanosContador == 0)
                {
                    DGV_1.Rows[e.RowIndex].Cells[C_CheckCombinacion.Index].Value = true;
                }

            }
        }
        private void P_2_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            ConfirmarCambios();
            F_Base.ActualizarTodosLasVentanas();
            Close();
        }


        private void BT_Seleccionar_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.SeleccionDataGridView(DGV_1, C_CheckCombinacion.Index, true);
        }

        
    }
}
