using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Ventana_Principal
{
    public partial class F_PesoTotal : Form
    {

        private cPiso PisoSelect;
        private float PTotalT;
        private float PTotalL;
        private float PTotalTv;
         public F_PesoTotal()
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

        private void F_PesoTotal_Load(object sender, EventArgs e)
        {
            CargarVentana();
        }

        private void CargarVentana()
        {
            CB_Pisos.Items.Clear();
            CB_Pisos.Items.AddRange(F_Base.Proyecto.Edificio.Lista_Pisos.Select(y => y.Nombre).ToArray());
            CB_Pisos.SelectedIndex = 0;
            PTotalL = (F_Base.Proyecto.Edificio.Lista_Pisos.Select(x => x.Nervios).Sum(y => y.Sum(z => z.Tendencia_Refuerzos.TInfeSelect.PesoRefuerzo)) + F_Base.Proyecto.Edificio.Lista_Pisos.Select(x => x.Nervios).Sum(y => y.Sum(z => z.Tendencia_Refuerzos.TSupeSelect.PesoRefuerzo)));
            PTotalTv = F_Base.Proyecto.Edificio.Lista_Pisos.Select(x => x.Nervios).Sum(y => y.Sum(z => z.Tendencia_Refuerzos.TEstriboSelect.Peso));
            PTotalT = PTotalL + PTotalTv;
            LB_PesoTotalL.Text = string.Format("{0:0.00}", PTotalL) + " kg"; LB_PesoTotalTV.Text = string.Format("{0:0.00}", PTotalTv) + " kg"; LB_PesoTotalT.Text = string.Format("{0:0.00}", PTotalT) + " kg";
        }

        private void CB_Pisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            PisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre==CB_Pisos.Text);
            CrearDataGridViewLabelsActualizar(DGV_PesoTotal);
        }


        private void CrearDataGridViewLabelsActualizar(DataGridView data)
        {
            float PesoPisoL = 0;float PesoPisoTv = 0; 
            float PesoTotal = PisoSelect.Nervios.Sum(x => x.Tendencia_Refuerzos.TEstriboSelect.Peso) + 
                PisoSelect.Nervios.Sum(x => x.Tendencia_Refuerzos.TInfeSelect.PesoRefuerzo) + 
                PisoSelect.Nervios.Sum(x => x.Tendencia_Refuerzos.TSupeSelect.PesoRefuerzo);
            data.Rows.Clear();
            PisoSelect.Nervios = cFunctionsProgram.OrdenarNerviosPorNomenclatura(PisoSelect.Nervios, F_Base.Proyecto.NomenclaturaDiagonal);
            PisoSelect.Nervios.ForEach(Nervio => {
                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[C_NombreNervio.Index].Value = Nervio.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_RL.Index].Value = string.Format("{0:0.00}", Nervio.Tendencia_Refuerzos.TInfeSelect.PesoRefuerzo+ Nervio.Tendencia_Refuerzos.TSupeSelect.PesoRefuerzo);
                data.Rows[data.Rows.Count - 1].Cells[C_Transversal.Index].Value = string.Format("{0:0.00}",Nervio.Tendencia_Refuerzos.TEstriboSelect.Peso);
                float Total = Nervio.Tendencia_Refuerzos.TInfeSelect.PesoRefuerzo + Nervio.Tendencia_Refuerzos.TSupeSelect.PesoRefuerzo + Nervio.Tendencia_Refuerzos.TEstriboSelect.Peso;
                data.Rows[data.Rows.Count - 1].Cells[C_Total.Index].Value = string.Format("{0:0.00}", Total);
                data.Rows[data.Rows.Count - 1].Cells[C_PorcRef.Index].Value = string.Format("{0:0.00}", Total/PesoTotal*100);
                PesoPisoL += Nervio.Tendencia_Refuerzos.TInfeSelect.PesoRefuerzo + Nervio.Tendencia_Refuerzos.TSupeSelect.PesoRefuerzo;
                PesoPisoTv += Nervio.Tendencia_Refuerzos.TEstriboSelect.Peso;
            });

            LB_PesoPorPisoL.Text = string.Format("{0:0.00}", PesoPisoL) + " kg"; LB_PesoPorPisoTV.Text = string.Format("{0:0.00}", PesoPisoTv) + " kg"; LB_PesoPorPisoT.Text = string.Format("{0:0.00}", PesoTotal) + " kg";
            cFunctionsProgram.EstiloDatGridView(data);
        }
    
    }
}
