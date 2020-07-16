using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios
{
    public partial class F_PropiedadesProyecto : Form
    {
        public F_PropiedadesProyecto()
        {
            InitializeComponent();
        }

        private void LoadWindows()
        {
            if (F_Base.Proyecto != null)
            {
                foreach (Control control in TP1_General.Controls)
                    control.Enabled = true;
                CargarTabPageGeneral();

            }
            else
            {
                foreach (Control control in TP1_General.Controls)
                {
                    if (control != GB_3)
                    {
                        control.Enabled = false;
                    }
                    else
                        CargarGB3();
                }
            }

        }

        private void CargarTabPageGeneral()
        {
            CKB_LabelsBarras.Checked = F_Base.Proyecto.EtiquetasDeBarras;
            CKB_CotaInteligente.Checked = F_Base.Proyecto.AcotamientoInteligente;
            CK_Redondear.Checked = F_Base.Proyecto.RedondearBarra;
            CKB_CoorPI.Checked = F_Base.Proyecto.CoordenadasPInterseccion;
        }
        private void CargarGB3() 
        {
            CKB_AutoGuardado.Checked = F_Base.PropiedadesPrograma.AutoGuardado;
            NUD_Intervalo.Enabled = CKB_AutoGuardado.Checked;
            NUD_Intervalo.Value = F_Base.PropiedadesPrograma.IntervaloMinAutoGuarado.Minutos;
        }

        private void AplicarCambios()
        {
            if (F_Base.Proyecto != null)
            {
                F_Base.Proyecto.EtiquetasDeBarras = CKB_LabelsBarras.Checked;
                F_Base.Proyecto.AcotamientoInteligente = CKB_CotaInteligente.Checked;
                F_Base.Proyecto.RedondearBarra = CK_Redondear.Checked;
                F_Base.Proyecto.CoordenadasPInterseccion= CKB_CoorPI.Checked;
            }
            F_Base.PropiedadesPrograma.AutoGuardado = CKB_AutoGuardado.Checked;
            F_Base.PropiedadesPrograma.IntervaloMinAutoGuarado = new Programa.cPropiedades.TiempoAutoGuardado((int)NUD_Intervalo.Value);
            Programa.cPropiedades.GuardarPropiedades(F_Base.PropiedadesPrograma);
            F_Base.ActualizarTodosLasVentanas();
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            AplicarCambios();
            Close();
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void F_PropiedadesProyecto_Load(object sender, EventArgs e)
        {
            LoadWindows();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void CKB_AutoGuardado_CheckedChanged(object sender, EventArgs e)
        {
            NUD_Intervalo.Enabled = CKB_AutoGuardado.Checked;
        }
    }
}
