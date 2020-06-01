using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Manipulación_Proyecto
{
    public partial class F_Tendencia : Form
    {

        public F_Tendencia()
        {
            InitializeComponent();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            Close();
        }








        private void CargarVentana()
        {
            cTendencia TS = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TSupeSelect;
            cTendencia TI = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TInfeSelect;
            CrearDataGridView(DGV_RBaseSuperior, TS.BarrasAEmplearBase);
            CrearDataGridView(DGV_RAdicionalISuperior, TS.BarrasAEmplearAdicional);
            CrearDataGridView(DGV_RBaseLongitudinalInferior, TI.BarrasAEmplearBase);
            CrearDataGridView(DGV_RAdicionalLongitudinalInferior, TI.BarrasAEmplearAdicional);

            TB_LMaximaSuperior.Text = string.Format("{0:0.00}", TS.MaximaLongitud);
            TB_LMaximaI.Text = string.Format("{0:0.00}", TI.MaximaLongitud);

            TB_PminS.Text = string.Format("{0:0.00000}", TS.CuantiaMinima);
            TB_PminI.Text = string.Format("{0:0.00000}", TI.CuantiaMinima);
        }




        private void CrearDataGridView(DataGridView data,List<eNoBarra> NoBarrasSeleccionadas )
        {

            List<eNoBarra> Barras = cFunctionsProgram.NoBarras; Barras.Remove(eNoBarra.BNone);
            data.Rows.Clear();

            Barras.ForEach(Barra => {

                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[0].Value = cFunctionsProgram.ConvertireNoBarraToString(Barra);
                eNoBarra Barra1 = NoBarrasSeleccionadas.Find(x=>x==Barra);
                data.Rows[data.Rows.Count - 1].Cells[1].Value = Barra1 != eNoBarra.BNone ? true : (object)false;


            });

            cFunctionsProgram.EstiloDatGridView(data);
        }

        private void F_Tendencia_Load(object sender, EventArgs e)
        {
            CargarVentana();
        }

        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
