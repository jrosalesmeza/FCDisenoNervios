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
    public partial class F_Base : Form
    {

        public static Form F_Base_;



        #region Ventanas Emergentes

        private F_NuevoProyecto F_NuevoProyecto = new F_NuevoProyecto();
        #endregion


        #region Ventanas Acopladas



        #endregion


        #region Proyecto
        public static cProyecto Proyecto;


        #endregion

        #region Funciones Basicas



        private void NuevoProyecto_Function()
        {
            F_NuevoProyecto.ShowDialog();
        }

        #endregion








        public F_Base()
        {
            InitializeComponent();
            T_Timer.Start();
            SetStyle(ControlStyles.ResizeRedraw, true);
            ST_Base.SizingGrip = false;
            cFunctionsProgram.Notificador += CFunctionsProgram_Notificador;


            F_Base_ = this;
        }

        private void CFunctionsProgram_Notificador(string Alert)
        {
            LB_Notificador.Text = Alert;
            LB_Notificador.Invalidate();
        }

        private void T_Timer_Tick(object sender, EventArgs e)
        {
            CambiosTimer_1();
        }





        













        #region Dimensionar Formulario

        private int tolerance = 16;
        private int tolerance2W = 2;
        private int tolerance2H = 2;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {


            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, ClientRectangle.Width - tolerance2W, this.ClientRectangle.Height - tolerance2H));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.ST_Base.Region = region;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }
        #endregion

        #region Mover ,Maximizar, Cerrar y Restaurar Ventana - Eventos Clicks
        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BT_MaxRest_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void BT_Minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void CambiosTimer_1()
        {
            if (WindowState == FormWindowState.Normal)
            {
                BT_MaxRest.Image = Properties.Resources.Maximizar14X11;
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                BT_MaxRest.Image = Properties.Resources.Restaurar14x11;
          
            }
        }
        private void DobleClickMaximaze(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    WindowState = FormWindowState.Maximized;
                }
                else if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
            }
        }
        private void MS_BarraPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                DobleClickMaximaze(e);
            }
        }

        private void P_Menu_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                DobleClickMaximaze(e);
            }
        }


        #endregion

        private void TSB_Nuevo_Click(object sender, EventArgs e)
        {
            NuevoProyecto_Function();
        }
    }
}
