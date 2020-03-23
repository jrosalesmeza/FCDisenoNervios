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
    public partial class F_EnumeracionPortico :Form
    {

        

        private float Zoom = 1;
        private float Dx = 0;
        private float Dy = 0;
        private float DesXButton;
        private float DesYButton;
        private bool SeleccionInteligente = false;


        public F_EnumeracionPortico()
        {
            InitializeComponent();
            PB_ElementosNoEnumerados.MouseDown += PB_ElementosNoEnumerados_MouseDown2;
            PB_ElementosNoEnumerados.MouseMove += PB_ElementosNoEnumerados_MouseMove2;
            T_Timer2.Start();
        }

        private void CargarListView()
        {
            cDatosEtabs DatosEtabs = F_Base.Proyecto.DatosEtabs;
            LV_Stories.Clear();     
            foreach (cPiso Piso in DatosEtabs.Lista_Pisos)
            {
                ListViewItem item = new ListViewItem(Piso.Nombre);item.Name = Piso.Nombre;
                LV_Stories.Items.Add(item);
            }
            if (F_Base.Proyecto.DatosEtabs.PisoSelect!=null)
            {
                ListViewItem[] listViewItems = LV_Stories.Items.Find(F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre, false);
                listViewItems[0].Selected = true;

            }
            else
            {
                LV_Stories.Items[LV_Stories.Items.Count - 1].Selected = true;
           }

        }
        private void F_EnumPort_Load()
        {
            CargarListView();
        }

        private void CambiosTimer()
        {

        }
        private void F_EnumeracionPortico_Load(object sender, EventArgs e)
        {
            F_EnumPort_Load();

        }

        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count > 0)
            {
                F_Base.Proyecto.DatosEtabs.PisoSelect = F_Base.Proyecto.DatosEtabs.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                LB_Title.Text = $"Enumeración de Elementos  | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
                PB_ElementosNoEnumerados.Invalidate();
            }
        }

        private void T_Timer2_Tick(object sender, EventArgs e)
        {
            CambiosTimer();
        }


        #region Eventos Principales Ventana
        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);

        }
        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Visible = false;
        }


        #endregion

        private void PB_ElementosNoEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            float XI = 5;float YI = 5;
            float Width = PB_ElementosNoEnumerados.Width-XI*3;
            float Height = PB_ElementosNoEnumerados.Height-YI*3;
            List<PointF> PointsSinEscalar= new List<PointF>();
            F_Base.Proyecto.DatosEtabs.Lista_Pisos.ForEach(x => x.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } }));
            F_Base.Proyecto.DatosEtabs.Lista_Pisos.ForEach(x => x.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, Width, Height, Dx, Dy, Zoom)));
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.PaintPlantaEscaladaEtabsLine(e.Graphics);});
            

        }

        private void PB_ElementosNoEnumerados_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor CursorArraste = new Cursor(Properties.Resources.Arrastre16x16.Handle);
            //Cursor CursorDefault = new Cursor(Properties.Resources.DefaultCursor2x16.Handle);
            Cursor CursorDefault = Cursors.Default;
            Cursor cursor = CursorDefault;
            if (e.Button == MouseButtons.Middle)
            {
                float DesplazX = (Dx) + (e.X - DesXButton);
                float DesplazY = (Dy) + (e.Y - DesYButton);
                Dx = DesplazX;
                Dy = DesplazY;
                DesXButton = e.X;
                DesYButton = e.Y;
                cursor = CursorArraste;
                PB_ElementosNoEnumerados.Invalidate();
            }
            PB_ElementosNoEnumerados.Cursor = cursor;


        }
        private void PB_ElementosNoEnumerados_MouseMove2(object sender,MouseEventArgs e)
        {
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.MouseMoveElementoSinEnumerar(e.Location); });
            PB_InfoElementosNoEnumerados.Invalidate();
        }
        private void PB_ElementosNoEnumerados_MouseWheel(object sender,MouseEventArgs e)
        {
            int vueltas = e.Delta;
            if (vueltas > 0)
            {

                Zoom += 0.5f;
               // Dx -= e.X;
                //Dy -= e.Y - PB_ElementosNoEnumerados.Height;
                PB_ElementosNoEnumerados.Invalidate();
            }
            else
            {
                if (Zoom > 1)
                {
                   // Dx += e.X;
                    //Dy += e.Y - PB_ElementosNoEnumerados.Height;
                    Zoom -= 0.5f;
                    PB_ElementosNoEnumerados.Invalidate();
                }
            }

        }

        private void PB_ElementosNoEnumerados_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks == 2)
            {
                Zoom = 1;
                Dx = 0; Dy = 0;
                PB_ElementosNoEnumerados.Invalidate();
            }
            if (e.Button == MouseButtons.Middle)
            {
                DesXButton = e.X;
                DesYButton = e.Y;
            }
        }

        private void PB_ElementosNoEnumerados_MouseDown2(object sender,MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
 
                int IndiceMaximo=F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Max(x => x.IndexSelect);
                F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.MouseDownSelectLineEtabs(e, IndiceMaximo);});
                int MaximoIndice = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Max(x => x.IndexSelect);
                cLine LineSelectIntel = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Find(x => x.IndexSelect == MaximoIndice);

                if (SeleccionInteligente && LineSelectIntel != null)
                {
                    
                    cFunctionsProgram.SeleccionInteligente(LineSelectIntel, F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(x => x.Type == eType.Beam).ToList());
                  
                }
            
            }
            PB_ElementosNoEnumerados.Invalidate();
        }

        private void CKB_SeleccionInteligente_CheckedChanged(object sender, EventArgs e)
        {
            SeleccionInteligente = CKB_SeleccionInteligente.Checked;
        }

        private void PB_InfoElementosNoEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { 
                if (y.Type == eType.Beam) 
                    y.PaintMouseMove(e.Graphics,PB_InfoElementosNoEnumerados.Height, PB_InfoElementosNoEnumerados.Width);
            });

     

        }

        private void mostrarSecciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mostrarSecciónToolStripMenuItem.Checked)
                PB_InfoElementosNoEnumerados.Visible = true;
            else
                PB_InfoElementosNoEnumerados.Visible = false;
        }

        private void limpirarSeleccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(x => x.Select =false);
            PB_ElementosNoEnumerados.Invalidate();
        }

        private void F_EnumeracionPortico_Paint(object sender, PaintEventArgs e)
        {
            PB_ElementosNoEnumerados.Invalidate();
   
        }
    }
}
