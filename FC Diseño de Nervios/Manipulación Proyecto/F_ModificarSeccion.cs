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
                TB_Longitud.Visible = false;
                LB_Longitud.Visible = false;
                LB_m3.Visible = false;
                TB_Altura.Text= ElementoSeleccionado.Seccion.H.ToString();
                TB_Ancho.Text = ElementoSeleccionado.Seccion.B.ToString();
            }
            else
            {
                TB_Longitud.Visible = true;
                LB_Longitud.Visible = true;
                LB_m3.Visible = true;
                TB_Altura.Text = ElementoSeleccionado.Seccion.H.ToString();
                TB_Ancho.Text = ElementoSeleccionado.Seccion.B.ToString();
                cSubTramo SubTramo = (cSubTramo)ElementoSeleccionado;
                TB_Longitud.Text = SubTramo.Longitud.ToString();
            }
        }





        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
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
            if(ElementoSeleccionado is cApoyo)
            {
                if(H!=0 && B != 0)
                {
                    CambioPropiedadesElemento(H, B);
             
                }
            }
            else
            {
                if (H != 0 && B != 0 && L!=0)
                {
                    CambioPropiedadesElemento(H, B, L);
                }

            }
            ElementoSeleccionado.Vistas.SelectPerfilLongitudinal = false;
            F_Base.ActualizarTodosLasVentanas();
            Close();


        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            ElementoSeleccionado.Vistas.SelectPerfilLongitudinal = false;
            F_Base.ActualizarTodosLasVentanas();
            Close();
        }
    }
}
