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
    public partial class F_SeleccionBarras : Form
    {
        private cBarra barraSelect;
        public cBarra BarraSelect
        {
            get { return barraSelect; }
            set
            {
                if(value != null && value != barraSelect)
                {
                    barraSelect = value;
                    LoadWindow();
                }
            }
        }

        
        public F_SeleccionBarras()
        {
            InitializeComponent();
            GB_1.MouseDown += P_2_MouseDown;
        }


        private void LoadWindow()
        {
            CB_NoBarra.Items.Clear();
            cFunctionsProgram.NoBarras.ForEach(x => CB_NoBarra.Items.Add(cFunctionsProgram.ConvertireNoBarraToString(x)));
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            Barra_1.UbicacionRefuerzo = barraSelect.UbicacionRefuerzo;
            Barra_1.TipoGancho_Izquierdo = barraSelect.GanchoIzquierda;
            Barra_1.TipoGancho_Derecho = barraSelect.GanchoDerecha;
            Barra_1.NoBarra = barraSelect.NoBarra;
            NUP_Cantidad.Value = barraSelect.CantBarra;
            CB_NoBarra.Text = cFunctionsProgram.ConvertireNoBarraToString(barraSelect.NoBarra);
            TB_Precision.Text = string.Format("{0:0.00}", barraSelect.DeltaAlargamiento);
        }


        private void ChangeTextLong()
        {
            float.TryParse(TB_Longitud.Text, out float Long);
            float LongiMaximaConGancho = barraSelect.TendenciaOrigen.MaximaLongitud - cDiccionarios.LDGancho(Barra_1.NoBarra, Barra_1.TipoGancho_Derecho) - cDiccionarios.LDGancho(Barra_1.NoBarra, Barra_1.TipoGancho_Izquierdo);
            if (Long == 0 && TB_Longitud.Text.Length == 3)
            {
                TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.TendenciaOrigen.MinimaLongitud);
            }
            else if (TB_Longitud.Text.Length == 3)
            {
                if (Long < barraSelect.TendenciaOrigen.MinimaLongitud)
                {
                    TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.TendenciaOrigen.MinimaLongitud);
                }
            }
            else if (Long > LongiMaximaConGancho)
            {

                TB_Longitud.Text = string.Format("{0:0.00}", LongiMaximaConGancho);
            }
        }
        private void ChangeBarra()
        {
            float.TryParse(TB_Precision.Text, out float Prec);
            float.TryParse(TB_Longitud.Text, out float Long);
            float Longitud2 = barraSelect.XF - barraSelect.XI;
            float Delta = Long - Longitud2;
            BarraSelect.XF += Delta;
            barraSelect.DeltaAlargamiento = Prec;
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }


       #region Radio Button Checked
        private void RB_180I_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_180I.Checked)
            {
                Barra_1.TipoGancho_Izquierdo = eTipoGancho.G180;
            }
            ChangeBarra();
        }

        private void RB_90I_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_90I.Checked)
            {
                Barra_1.TipoGancho_Izquierdo = eTipoGancho.G90;
            }
            ChangeBarra();
        }

        private void RB_0I_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_0I.Checked)
            {
                Barra_1.TipoGancho_Izquierdo = eTipoGancho.None;
            }
            ChangeBarra();
        }

        private void RB_180D_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_180D.Checked)
            {
                Barra_1.TipoGancho_Derecho = eTipoGancho.G180;
            }
            ChangeBarra();        
        }

        private void RB_90D_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_90D.Checked)
            {
                Barra_1.TipoGancho_Derecho = eTipoGancho.G90;
            }
            ChangeBarra();
        }
        private void RB_0D_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_0D.Checked)
            {
                Barra_1.TipoGancho_Derecho = eTipoGancho.None;
            }
            ChangeBarra();
        }

        #endregion

        private void Barra_1_ChangeGanchoIzquierdo(eTipoGancho Gancho)
        {
            if(Gancho == eTipoGancho.G180)
            {
                RB_180I.Checked = true;
            }else if(Gancho == eTipoGancho.G90)
            {
                RB_90I.Checked = true;
            }
            else
            {
                RB_0I.Checked = true;
            }
            barraSelect.GanchoIzquierda = Gancho;
            ChangeTextLong();
        }

        private void Barra_1_ChangeGanchoDerecho(eTipoGancho Gancho)
        {
            if (Gancho == eTipoGancho.G180)
            {
                RB_180D.Checked = true;
            }
            else if (Gancho == eTipoGancho.G90)
            {
                RB_90D.Checked = true;
            }
            else
            {
                RB_0D.Checked = true;
            }
            barraSelect.GanchoDerecha = Gancho;
            ChangeTextLong();
        }

        private void P_2_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            barraSelect.C_Barra.IsSelect = false;
            F_Base.ActualizarVentanaF_VentanaDiseno();
            Close();
        }

        private void CB_NoBarra_SelectedIndexChanged(object sender, EventArgs e)
        {
            Barra_1.NoBarra = cFunctionsProgram.ConvertirStringToeNoBarra(CB_NoBarra.Text);
           
        }
        private void TB_Longitud_TextChanged(object sender, EventArgs e)
        {
            ChangeTextLong();
            ChangeBarra();
        }

        private void TB_Precision_TextChanged(object sender, EventArgs e)
        {
            float.TryParse(TB_Precision.Text, out float Precision);
            if (Precision == 0 && TB_Precision.Text.Length==4)
            {
                TB_Precision.Text = string.Format("{0:0.00}", barraSelect.TendenciaOrigen.DeltaAlargamientoBarras);
            }
            else if (TB_Precision.Text.Length == 4)
            {
                if (Precision < 0.01f)
                {
                    TB_Precision.Text = string.Format("{0:0.00}", barraSelect.TendenciaOrigen.DeltaAlargamientoBarras);
                }
            }
            ChangeBarra();
        }

        private void Barra_1_ChangeBarra(eNoBarra NoBarra)
        {
            barraSelect.NoBarra = Barra_1.NoBarra;
            ChangeBarra();
            ChangeTextLong();
        }

        private void NUP_Cantidad_ValueChanged(object sender, EventArgs e)
        {
            barraSelect.CantBarra = (int)NUP_Cantidad.Value;
            ChangeBarra();
            ChangeTextLong();
        }

        private void BT_Der_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_2.Text, out float Dist);
            barraSelect.XF += Dist;
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_Izq_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_2.Text, out float Dist);
            barraSelect.XI -= Dist;
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_Centro_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_2.Text, out float Dist);
            barraSelect.XI -= Dist/2f;
            barraSelect.XF += Dist / 2f;
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

  
    }
}
