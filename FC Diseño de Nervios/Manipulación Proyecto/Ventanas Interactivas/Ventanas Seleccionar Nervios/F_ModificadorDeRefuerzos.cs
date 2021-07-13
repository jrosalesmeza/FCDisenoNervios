using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo;

namespace FC_Diseño_de_Nervios
{
    public partial class F_ModificadorDeRefuerzos : DockContent
    {

        public F_ModificadorDeRefuerzos()
        {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(BT_MoverALaMitadTramo, "Mover a la mitad del tramo.");
            toolTip.SetToolTip(BT_AñadirACaraApoyo, "Anclar a 0.05 de la cara del apoyo.");
            Timer timer = new Timer(); timer.Start();
            timer.Tick += Timer_Tick;
            //Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (F_Base.Proyecto != null && F_Base.Proyecto.Edificio != null && F_Base.Proyecto.Edificio.PisoSelect != null && F_Base.Proyecto.Edificio.PisoSelect.NervioSelect != null)
            {
                if (!F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.BloquearNervio)
                {
                    Enabled = true;
                    GB_1.Enabled = barraSelect != null;
                    GB_2.Enabled = bloqueEstribosSelect != null;
                    if (bloqueEstribosSelect != null)
                    {
                        BT_AñadirACaraApoyo.Image = bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Izquierda ? Properties.Resources.Align_ends_Left2 : Properties.Resources.Align_Ends_Right2;
                    }
                }
                else
                {
                    Enabled = !F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.BloquearNervio;
                }
            }
        }
        #region Modificador de Barra Longitudinal
        private cBarra barraSelect;
        public cBarra BarraSelect
        {
            get { return barraSelect; }
            set
            {
                if (value != null && value != barraSelect)
                {
                    GB_1.Enabled = true;
                    barraSelect = value;
                    LoadWindow();
                }else if (value != null)
                {
                    GB_1.Enabled = true;
                    ActualizarCambios();
                }
                if (value==null)
                {
                    barraSelect = value;
                }
            }
        }
        private void LoadWindow()
        {
            CB_NoBarra.Items.Clear();
            cFunctionsProgram.NoBarras.Remove(eNoBarra.BNone);
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
        private void ActualizarCambios()
        {
            TB_Longitud.TextChangedProperty = false;
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
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
            if (RB_Dr.Checked)
            {
                BarraSelect.XF += Delta;
            }
            else if (RB_Izq.Checked)
            {
                BarraSelect.XI -= Delta;
            }
            else if (RB_Centro.Checked)
            {
                BarraSelect.XI -= Delta/2F;
                BarraSelect.XF += Delta / 2F;
            }

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
            if (Gancho == eTipoGancho.G180)
            {
                RB_180I.Checked = true;
            }
            else if (Gancho == eTipoGancho.G90)
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

        private void TB_Longitud_KeyPress(object sender, KeyPressEventArgs e)
        {
            TB_Longitud.TextChangedProperty = true;
        }

        private void TB_Precision_TextChanged(object sender, EventArgs e)
        {
            float.TryParse(TB_Precision.Text, out float Precision);
            if (Precision == 0 && TB_Precision.Text.Length == 4)
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
            float.TryParse(TB_C.Text, out float Dist);
            barraSelect.XF += Dist;
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_Centro_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_C.Text, out float Dist);
            barraSelect.XI -= Dist / 2f;
            barraSelect.XF += Dist / 2f;
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_II_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_I.Text, out float Dist);
            barraSelect.XI -= Math.Abs(Dist);
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_ID_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_I.Text, out float Dist);
            barraSelect.XI += Math.Abs(Dist);
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_DI_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_D.Text, out float Dist);
            barraSelect.XF -= Math.Abs(Dist);
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_DD_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_D.Text, out float Dist);
            barraSelect.XF += Math.Abs(Dist);
            TB_Longitud.Text = string.Format("{0:0.00}", barraSelect.XF - barraSelect.XI);
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        #endregion

        #region Modificador de Barra de Estribos


        private cBloqueEstribos bloqueEstribosSelect;
        public cBloqueEstribos BloqueEstribosSelect
        {
            get
            {
                return bloqueEstribosSelect;
            }
            set
            {
                if(value!= bloqueEstribosSelect && value!=null)
                {
                    bloqueEstribosSelect = value;
                    GB_2.Enabled = true;
                    ChangeBloqueEstribos();
                }
                else if(value!=null)
                {
                    GB_2.Enabled = true;
                    ActualizarValores();
                }
                if (value == null)
                {
                    bloqueEstribosSelect = value;
                }
            }
        }

        private void ChangeBloqueEstribos()
        {
            CB_NoBarra2.Items.Clear();
            cFunctionsProgram.NoBarras.Remove(eNoBarra.BNone);
            cFunctionsProgram.NoBarras.ForEach(x => CB_NoBarra2.Items.Add(cFunctionsProgram.ConvertireNoBarraToString(x)));
            CB_NoBarra2.Text = cFunctionsProgram.ConvertireNoBarraToString(bloqueEstribosSelect.NoBarra);
            NUD_CantidadEstribos.Value = bloqueEstribosSelect.Cantidad;
            NUD_NoRamas.Value = bloqueEstribosSelect.NoRamas;
            TB_Separacion.Text = string.Format("{0:0.00}",bloqueEstribosSelect.Separacion * cConversiones.Dimension_m_to_cm);
            ActualizarValores();
        }

        private void CB_NoBarra2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bloqueEstribosSelect.NoBarra =  cFunctionsProgram.ConvertirStringToeNoBarra(CB_NoBarra2.Text);
            F_Base.ActualizarVentanaF_VentanaDiseno();

        }

        private void NUD_NoRamas_ValueChanged(object sender, EventArgs e)
        {
            bloqueEstribosSelect.NoRamas = (int)NUD_NoRamas.Value;
            F_Base.ActualizarVentanaF_VentanaDiseno();

        }

        private void NUD_CantidadEstribos_ValueChanged(object sender, EventArgs e)
        {
            bloqueEstribosSelect.Cantidad = (int)NUD_CantidadEstribos.Value;
            F_Base.ActualizarVentanaF_VentanaDiseno();

        }
        private void TB_Separacion_TextChanged(object sender, EventArgs e)
        {
            if (TB_Separacion.Text != string.Empty)
            {
                if (float.TryParse(TB_Separacion.Text, out float SenCm))
                {
                    if (SenCm * cConversiones.Dimension_cm_to_m <= cVariables.Separacion_MaximaEstribos)
                    {
                        bloqueEstribosSelect.Separacion = SenCm * cConversiones.Dimension_cm_to_m;
                        F_Base.ActualizarVentanaF_VentanaDiseno();

                    }
                    else
                    {
                        TB_Separacion.Text = string.Format("{0:0.00}", cVariables.Separacion_MaximaEstribos* cConversiones.Dimension_m_to_cm);
                    }
                    
                }
                else
                {
                    TB_Separacion.Text = string.Format("{0:0.00}", bloqueEstribosSelect.Separacion * cConversiones.Dimension_m_to_cm);
                }
            }
        }

        private void TB_AbcisaX_TextChanged(object sender, EventArgs e)
        {
            if (TB_AbcisaX.Text != string.Empty)
            {
                if(float.TryParse(TB_AbcisaX.Text,out float abscisaX) )
                {

                    if(bloqueEstribosSelect.LimiteDerecho>= abscisaX && bloqueEstribosSelect.LimiteIzquierdo<= abscisaX)
                    {
                        if (bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Derecha)
                            bloqueEstribosSelect.XI = abscisaX;
                        else
                            bloqueEstribosSelect.XF = abscisaX;

                        F_Base.ActualizarVentanaF_VentanaDiseno();

                    }
                }

            }
        }
        private void TB_AbcisaX_KeyPress(object sender, KeyPressEventArgs e)
        {
            TB_AbcisaX.TextChangedProperty = true;
        }

        private void ActualizarValores()
        {
            TB_AbcisaX.TextChangedProperty = false;
            TB_AbcisaX.Text = bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Derecha
                ? string.Format("{0:0.00}", bloqueEstribosSelect.XI)
                : string.Format("{0:0.00}", bloqueEstribosSelect.XF);

            RB_Izquierda.Checked = bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Izquierda;
            RB_Derecha.Checked = bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Derecha;

        }
        private void RB_Izquierda_CheckStateChanged(object sender, EventArgs e)
        {
            bloqueEstribosSelect.DireccionEstribo = RB_Izquierda.Checked ? eLadoDeZona.Izquierda : eLadoDeZona.Derecha;
            F_Base.ActualizarVentanaF_VentanaDiseno();

        }

        private void RB_Derecha_CheckStateChanged(object sender, EventArgs e)
        {
            bloqueEstribosSelect.DireccionEstribo = RB_Izquierda.Checked ? eLadoDeZona.Izquierda : eLadoDeZona.Derecha;
            F_Base.ActualizarVentanaF_VentanaDiseno();


        }

        private void BT_Izq_Click(object sender, EventArgs e)
        {
            if (float.TryParse(TB_M.Text, out float value)) {
                if (bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Derecha)
                    bloqueEstribosSelect.XI -= value;
                else
                    bloqueEstribosSelect.XF -= value;

                F_Base.ActualizarVentanaF_VentanaDiseno();
                ActualizarValores();
            }
        }

        private void BT_Dr_Click(object sender, EventArgs e)
        {
            if (float.TryParse(TB_M.Text, out float value))
            {
                if (bloqueEstribosSelect.DireccionEstribo == eLadoDeZona.Derecha)
                    bloqueEstribosSelect.XI += value;
                else
                    bloqueEstribosSelect.XF += value;
                F_Base.ActualizarVentanaF_VentanaDiseno();
                ActualizarValores();
            }
        }

        #endregion

        private void F_ModificadorDeRefuerzos_DockStateChanged(object sender, EventArgs e)
        {
            AjustarVentanasAFormulario();
        }


        private void AjustarVentanasAFormulario()
        {
            try
            {
                Control[] controls = TP_ModificarBarra.Controls.Find("GB_1", false);
                controls.First().Location = new Point(TP_ModificarBarra.Left + TP_ModificarBarra.Width / 2 - controls.First().Width / 2, TP_ModificarBarra.Height / 2 - controls.First().Height / 2);
                Control[] controls2 = TP_ModificarEstribos.Controls.Find("GB_2", false);
                controls2.First().Location = new Point(TP_ModificarEstribos.Left + TP_ModificarEstribos.Width / 2 - controls2.First().Width / 2, TP_ModificarEstribos.Height / 2 - controls2.First().Height / 2);
            }
            catch
            {

            }
        }

        private void TC_ModificadorBarras_Resize(object sender, EventArgs e)
        {
            AjustarVentanasAFormulario();
        }

        private void TC_ModificadorBarras_SelectedIndexChanged(object sender, EventArgs e)
        {
            AjustarVentanasAFormulario();
        }

        private void BT_AñadirACaraApoyo_Click(object sender, EventArgs e)
        {
            bloqueEstribosSelect.MoveraCaraApoyo();
            ActualizarValores();
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }

        private void BT_MoverALaMitadTramo_Click(object sender, EventArgs e)
        {
            bloqueEstribosSelect.MoverAMitadTramo();
            ActualizarValores();
            F_Base.ActualizarVentanaF_VentanaDiseno();
        }
    }
}
