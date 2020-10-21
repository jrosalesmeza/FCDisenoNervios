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

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(NUD_ThreadSleep, "Entre menor sea este valor mayor será el rendimiento, sin embargo la exigencia en el procesador aumentará.");

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
                    {
                        CargarGBsAdicionales();
                    }
                }
            }

        }

        private void CargarTabPageGeneral()
        {
            CKB_LabelsBarras.Checked = F_Base.Proyecto.EtiquetasDeBarras;
            CKB_CotaInteligente.Checked = F_Base.Proyecto.AcotamientoInteligente;
            CK_Redondear.Checked = F_Base.Proyecto.RedondearBarra;
            CKB_CoorPI.Checked = F_Base.Proyecto.CoordenadasPInterseccion;
            CKB_AcotamientoTraslapos.Checked = F_Base.Proyecto.AcotamientoTraslapos;
            CargarGBsAdicionales();
        }
        private void CargarGBsAdicionales() 
        {
            CKB_AutoGuardado.Checked = F_Base.PropiedadesPrograma.AutoGuardado;
            NUD_Intervalo.Enabled = CKB_AutoGuardado.Checked;
            NUD_Intervalo.Value = F_Base.PropiedadesPrograma.IntervaloMinAutoGuarado.Minutos;
            CKB_FuncionesParalelo.Checked = F_Base.PropiedadesPrograma.FuncionesEnParalelo;
            CKB_LineasPlanta.Checked = F_Base.PropiedadesPrograma.LineasPretrazado;
            NUD_ThreadSleep.Enabled = F_Base.PropiedadesPrograma.FuncionesEnParalelo;
            NUD_ThreadSleep.Value = F_Base.PropiedadesPrograma.TimerSleep;
            CKB_DeshacerRehacer.Checked = F_Base.PropiedadesPrograma.DeshacerRehacer;
        }

        private void AplicarCambios()
        {
            if (F_Base.Proyecto != null)
            {
                F_Base.Proyecto.EtiquetasDeBarras = CKB_LabelsBarras.Checked;
                F_Base.Proyecto.AcotamientoInteligente = CKB_CotaInteligente.Checked;
                F_Base.Proyecto.RedondearBarra = CK_Redondear.Checked;
                F_Base.Proyecto.CoordenadasPInterseccion= CKB_CoorPI.Checked;
                F_Base.Proyecto.AcotamientoTraslapos = CKB_AcotamientoTraslapos.Checked;
            }
            F_Base.PropiedadesPrograma.AutoGuardado = CKB_AutoGuardado.Checked;
            F_Base.PropiedadesPrograma.IntervaloMinAutoGuarado = new Programa.cPropiedades.TiempoAutoGuardado((int)NUD_Intervalo.Value);
            F_Base.PropiedadesPrograma.LineasPretrazado = CKB_LineasPlanta.Checked;
            F_Base.PropiedadesPrograma.FuncionesEnParalelo = CKB_FuncionesParalelo.Checked;
            F_Base.PropiedadesPrograma.TimerSleep = (int)NUD_ThreadSleep.Value;
            F_Base.PropiedadesPrograma.DeshacerRehacer= CKB_DeshacerRehacer.Checked;
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

        private void CKB_FuncionesParalelo_CheckedChanged(object sender, EventArgs e)
        {
            NUD_ThreadSleep.Enabled = CKB_FuncionesParalelo.Checked;
        }

    }
}
