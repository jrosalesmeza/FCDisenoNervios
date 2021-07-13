using System;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios
{
    public partial class F_ModificarSeccion : Form
    {
        private IElemento elmentoSelecionado;
        public IElemento ElementoSeleccionado
        {
            get { return elmentoSelecionado; }
            set
            {
                if (elmentoSelecionado != value)
                {
                    elmentoSelecionado = value;
                    CambiarEstadoVentana();
                }
            }
        }
        
        public F_ModificarSeccion()
        {
            InitializeComponent();
        }


        private void CambioPropiedadesElemento(float Altura,float Ancho, float Longitud=0)
        {
            if (ElementoSeleccionado.Vistas.SelectPerfilLongitudinal)
            {
                F_Base.EnviarEstadoVacio();
                if(ElementoSeleccionado is cApoyo)
                {
                    cSeccion Seccion =ElementoSeleccionado.Seccion;
                    Seccion.B = Ancho; Seccion.H = Altura;
                    ElementoSeleccionado.Seccion = Seccion;
                }
                else
                {
                    cSeccion Seccion = ElementoSeleccionado.Seccion;
                    Seccion.B = Ancho; Seccion.H = Altura;
                    ElementoSeleccionado.Seccion = Seccion;
                    cSubTramo SubtramoAux = (cSubTramo)ElementoSeleccionado;
                    SubtramoAux.Longitud = Longitud;
                }
            }
        }


        private void CambiarEstadoVentana()
        {
            if(ElementoSeleccionado is cApoyo)
            {
                LB_Ancho.Location = new System.Drawing.Point(66, 25);
                TB_Ancho.Location = new System.Drawing.Point(130, 22);
                LB_cm2.Location = new System.Drawing.Point(190, 25);
                TB_Altura.Location = new System.Drawing.Point(130, 57);
                LB_cm1.Location = new System.Drawing.Point(190, 60);
                LB_Altura.Location = new System.Drawing.Point(66, 60);
                GB_1.Location = new System.Drawing.Point(9, 31);
                Height = 172;
                GB_2.Visible = false;
                TB_Longitud.Visible = false;
                LB_Longitud.Visible = false;
                LB_m3.Visible = false;
                TB_Altura.Text= ElementoSeleccionado.Seccion.H.ToString();
                TB_Ancho.Text = ElementoSeleccionado.Seccion.B.ToString();

            }
            else
            {
                LB_Ancho.Location = new System.Drawing.Point(65, 15);
                TB_Ancho.Location = new System.Drawing.Point(129, 12);
                LB_cm2.Location = new System.Drawing.Point(189, 15);
                TB_Altura.Location = new System.Drawing.Point(129, 36);
                LB_cm1.Location = new System.Drawing.Point(189, 39);
                LB_Altura.Location = new System.Drawing.Point(65, 39);
                Height = 239;
                GB_1.Location = new System.Drawing.Point(9, 102);
                GB_2.Visible = true;
                TB_Longitud.Visible = true;
                LB_Longitud.Visible = true;
                LB_m3.Visible = true;
                TB_Altura.Text = ElementoSeleccionado.Seccion.H.ToString();
                TB_Ancho.Text = ElementoSeleccionado.Seccion.B.ToString();
                cSubTramo SubTramo = (cSubTramo)ElementoSeleccionado;
                TB_Longitud.Text = SubTramo.Longitud.ToString();
                TB_fc.Text = SubTramo.Seccion.Material.fc.ToString();
                TB_fy.Text = SubTramo.Seccion.Material.fy.ToString();
            }
        }


        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            if (ElementoSeleccionado != null)
            {
                ElementoSeleccionado.Vistas.SelectPerfilLongitudinal = false;
                F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }
            Close();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_Aplicar_Click(object sender, EventArgs e)
        {
            float.TryParse(TB_Altura.Text, out float H);
            float.TryParse(TB_Ancho.Text, out float B);
            float.TryParse(TB_Longitud.Text, out float L);
            if (!F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.SinRefuerzos_())
            {
                if (ElementoSeleccionado is cApoyo)
                {
                    if (H != 0 && B != 0)
                    {
                        CambioPropiedadesElemento(H, B);

                    }
                }
                else
                {
                    if (H != 0 && B != 0 && L != 0)
                    {
                        CambioPropiedadesElemento(H, B, L);
                    }

                }
                ElementoSeleccionado.Vistas.SelectPerfilLongitudinal = false;
                F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();
                F_Base.F_SelectNervio.ChangeComboBox(); 
                //F_Base.ActualizarTodosLasVentanas();
                Close();

            }
            else
            {
                cFunctionsProgram.VentanaEmergenteExclamacion("Para realizar un cambio en el elemento elimine el refuerzo asignado.");
            }
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            if (ElementoSeleccionado != null)
            {
                ElementoSeleccionado.Vistas.SelectPerfilLongitudinal = false;
                F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();
            }
            Close();
        }
    }
}
