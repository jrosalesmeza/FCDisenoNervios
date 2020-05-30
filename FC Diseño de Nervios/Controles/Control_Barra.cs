using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Controles
{
    public delegate void DelegateChangeNoBarra(eNoBarra NoBarra);
    public delegate void DelegateChangeGancho(eTipoGancho Gancho);
    public delegate void DelegateChangeTipoRefuerzo(eUbicacionRefuerzo UbicacionRefuerzo);
    public partial class Control_Barra : UserControl
    {
        [Category("La propiedad cambió")]
        [Description("Tiene lugar cuando se realice un cambio en el numero de la barra.")]
        public event DelegateChangeNoBarra ChangeBarra;
        [Category("La propiedad cambió")]
        [Description("Tiene lugar cuando se realice un cambio el gancho izquierdo de la barra.")]
        public event DelegateChangeGancho ChangeGanchoIzquierdo;
        [Category("La propiedad cambió")]
        [Description("Tiene lugar cuando se realice un cambio el gancho derecho de la barra.")]
        public event DelegateChangeGancho ChangeGanchoDerecho;

        private int espesor = 3;

        [Category("Propiedades")]
        [Description("Espesor de la Barra")]
        public int Espesor
        {
            get { return espesor; }
            set
            {
                if (value != espesor)
                {
                    espesor = value;
                    Invalidate();
                }

            }
        }

        private eTipoGancho tipoGancho_izquierdo= eTipoGancho.None;
        [Category("Propiedades")]
        [Description("Tipo gancho de la barra en su extremo izquierdo.")]
        [DisplayName("Gancho Izquierdo")]
        public eTipoGancho TipoGancho_Izquierdo
        {
            get { return tipoGancho_izquierdo; }
            set
            {
                if ( value!= eTipoGancho.G135)
                {
                    tipoGancho_izquierdo = value;
                    ChangeGanchoIzquierdo?.Invoke(value);
                    Invalidate();
                }

            }
        }

        private eTipoGancho tipoGancho_derecho = eTipoGancho.None;
        [Category("Propiedades")]
        [Description("Tipo gancho de la barra en su extremo derecho.")]
        [DisplayName("Gancho Derecho")]
        public eTipoGancho TipoGancho_Derecho
        {
            get { return tipoGancho_derecho; }
            set
            {
                if (value != eTipoGancho.G135)
                {
                    tipoGancho_derecho = value;
                    ChangeGanchoDerecho?.Invoke(value);
                    Invalidate();
                }

            }
        }


        private eNoBarra noBarra= eNoBarra.B2;
        [Category("Propiedades")]
        [Description("Numero de la barra.")]
        [DisplayName("No. Barra")]
        public eNoBarra NoBarra
        {
            get { return noBarra; }

            set
            {
                if(value!=noBarra)
                {
                    noBarra = value;
                    ChangeBarra?.Invoke(value);
                    Invalidate();
                }

            }
        }


        private eUbicacionRefuerzo ubicacionRefuerzo;
        [Category("Propiedades")]
        [Description("Ubicación del Refuerzo: Inferior o Superior.")]
        [DisplayName("Ubicación del Refuerzo")]
        public eUbicacionRefuerzo UbicacionRefuerzo {

            get { return ubicacionRefuerzo; }
            set
            {
                if (ubicacionRefuerzo != value)
                {
                    ubicacionRefuerzo = value;
                    Invalidate();
                }
            }
        }

        public Control_Barra()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            cDiccionarios.LlenarColoresBarra();
            Pen Pen = new Pen(cDiccionarios.ColorBarra[noBarra], espesor);
            Rectangle rec = ClientRectangle;
            float BORDER_GANCHO = espesor / 2f; float DIST_DESPLEGAR_Y = rec.Height / 3;
            float ANCHO_REC_ANGULO = rec.Width / 8f; float ALTO_REC_ANGULO = rec.Height / 3f - espesor / 2f;
            float xi = rec.X; float xf = rec.X + rec.Width;
            float yi = rec.Y + rec.Height / 2;


            if (tipoGancho_derecho != eTipoGancho.None | tipoGancho_izquierdo != eTipoGancho.None)
            {
                yi -= ALTO_REC_ANGULO / 2;
            }
        

            PointF Punto1 = new PointF(xi, yi);
            PointF Punto2 = new PointF(xf, yi);



            if (tipoGancho_izquierdo == eTipoGancho.G90)
            {
                Punto1 = new PointF(xi + ANCHO_REC_ANGULO / 2, yi);
                RectangleF rec_extremoIzquierdo = new RectangleF(xi + BORDER_GANCHO, yi, ANCHO_REC_ANGULO, ALTO_REC_ANGULO);
                e.Graphics.DrawArc(Pen, rec_extremoIzquierdo, 270, -90);
                e.Graphics.DrawLine(Pen, rec_extremoIzquierdo.X, Punto1.Y + ALTO_REC_ANGULO/2, rec_extremoIzquierdo.X, Punto1.Y + ALTO_REC_ANGULO+BORDER_GANCHO   );
            }

            if (tipoGancho_derecho == eTipoGancho.G90)
            {
                Punto2 = new PointF(xf - ANCHO_REC_ANGULO / 2, yi);
                RectangleF rec_extremoDerecho = new RectangleF(xf - ANCHO_REC_ANGULO - BORDER_GANCHO, yi, ANCHO_REC_ANGULO, ALTO_REC_ANGULO);
                e.Graphics.DrawArc(Pen, rec_extremoDerecho, -90, 90);
                e.Graphics.DrawLine(Pen, xf - BORDER_GANCHO, Punto2.Y + ALTO_REC_ANGULO/2, xf - BORDER_GANCHO, Punto2.Y + ALTO_REC_ANGULO+ BORDER_GANCHO);
            }



            if (tipoGancho_izquierdo == eTipoGancho.G180)
            {
                Punto1 = new PointF(xi + ANCHO_REC_ANGULO / 2, yi);
                RectangleF rec_extremoIzquierdo = new RectangleF(xi+BORDER_GANCHO, yi, ANCHO_REC_ANGULO, ALTO_REC_ANGULO);
                e.Graphics.DrawArc(Pen, rec_extremoIzquierdo, 270, -180);
                e.Graphics.DrawLine(Pen, Punto1.X, Punto1.Y + ALTO_REC_ANGULO, Punto1.X + ANCHO_REC_ANGULO / 2, Punto1.Y + ALTO_REC_ANGULO);
            }
            if (tipoGancho_derecho == eTipoGancho.G180)
            {
                Punto2 = new PointF(xf- ANCHO_REC_ANGULO/2, yi);
                RectangleF rec_extremoDerecho = new RectangleF(xf -ANCHO_REC_ANGULO- BORDER_GANCHO, yi, ANCHO_REC_ANGULO, ALTO_REC_ANGULO);
                e.Graphics.DrawArc(Pen, rec_extremoDerecho, -90, 180);
                e.Graphics.DrawLine(Pen, Punto2.X, Punto2.Y + ALTO_REC_ANGULO, Punto2.X - ANCHO_REC_ANGULO / 2, Punto2.Y + ALTO_REC_ANGULO);
            }







            e.Graphics.DrawLine(Pen, Punto1, Punto2);
        }






    }
}
