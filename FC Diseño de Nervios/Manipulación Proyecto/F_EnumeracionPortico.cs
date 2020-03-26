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

        private float XI = 5; float YI = 5;
        private  float WidthPB_NOENUMERADOS;
        private float Height_NOENUMERADOS; 
        private float Zoom = 1;
        private float Dx = 0;
        private float Dy = 0;
        private float DesXButton;
        private float DesYButton;
        private bool SeleccionInteligente = false;


        public F_EnumeracionPortico()
        {
            InitializeComponent();
            WidthPB_NOENUMERADOS = PB_ElementosNoEnumerados.Width - XI * 3;
            Height_NOENUMERADOS = PB_ElementosNoEnumerados.Height - YI * 3;
            PB_ElementosEnumerados.Paint += PB_ElementosEnumerados_Paint;
            PB_ElementosNoEnumerados.MouseDown += PB_ElementosNoEnumerados_MouseDown2;
            PB_ElementosNoEnumerados.MouseMove += PB_ElementosNoEnumerados_MouseMove2;
   

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
            T_Timer2.Start();
        }

        private void CambiosTimer()
        {
          
            if (F_Base.Proyecto.DatosEtabs.PisoSelect==null)
            {
                F_Base.Proyecto.DatosEtabs.PisoSelect = F_Base.Proyecto.DatosEtabs.Lista_Pisos[F_Base.Proyecto.DatosEtabs.Lista_Pisos.Count - 1];
            }
            cLine ElementSelect = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Find(x => x.Select == true);
            if (ElementSelect != null)
            {
                TB_Nombramiento.Enabled = true;
                BT_Enumerar.Enabled = true;
            }
            else
            {
                TB_Nombramiento.Enabled = false;
                BT_Enumerar.Enabled = false;
            }
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
                F_Base.Proyecto.Edificio.PisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == LV_Stories.SelectedItems[0].Text);
                LB_Title.Text = $"Enumeración de Elementos  | {F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre}";
                PB_ElementosNoEnumerados.Invalidate(); PB_ElementosEnumerados.Invalidate();
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
            T_Timer2.Stop();
            Visible = false;
        }


        #endregion

        private void PB_ElementosNoEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            WidthPB_NOENUMERADOS = PB_ElementosNoEnumerados.Width-XI*3;
            Height_NOENUMERADOS = PB_ElementosNoEnumerados.Height-YI*3;
            List<PointF> PointsSinEscalar= new List<PointF>();
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } });
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, Dx, Dy, Zoom));
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
                        cLine UltimaSelect=F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.Find(x => x.MouseInLineEtabs(e.Location));

                if (UltimaSelect != null && SeleccionInteligente)
                {
                    cFunctionsProgram.SeleccionInteligente(UltimaSelect, F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(x => x.Type == eType.Beam).ToList(), UltimaSelect.IndexSelect, UltimaSelect.Select);
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
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(x => { x.Select = false; x.IndexSelect = 0; });
            PB_ElementosNoEnumerados.Invalidate();
        }

        private void F_EnumeracionPortico_Paint(object sender, PaintEventArgs e)
        {
            PB_ElementosNoEnumerados.Invalidate();
            PB_ElementosEnumerados.Invalidate();
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.Deshacer_Function();
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Base.Rehacer_Function();
        }

        private void F_EnumeracionPortico_VisibleChanged(object sender, EventArgs e)
        {
            F_EnumPort_Load();
        }

        private void BT_Enumerar_Click(object sender, EventArgs e)
        {
            F_Base.EnviarEstado(F_Base.Proyecto);
            F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.ForEach(x => x.IndiceConjuntoSeleccion = 0);
            List<cLine> LineasSeleccionadas = F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines.FindAll(x => x.Select == true).ToList();
            CrearNervio(TB_Nombramiento.Text, LineasSeleccionadas);
            PB_ElementosEnumerados.Invalidate();
            PB_ElementosNoEnumerados.Invalidate();

        }



        private void CrearNervio(string Prefijo,List<cLine> LineasConCondiciones) 
        {
            
            List<List<cLine>> LineasParaCrearNervios = cFunctionsProgram.LineasParaCrearNervios(LineasConCondiciones);

            List<cNervio> NerviosPisoSelect = F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre).Nervios;
            int IndiceNervio = 1;
            if(NerviosPisoSelect==null || NerviosPisoSelect.Count==0)
            {
                NerviosPisoSelect = new List<cNervio>();
            }
            else
            {
                IndiceNervio = NerviosPisoSelect.Last().ID +1;
            }
    

            foreach (List<cLine> lines in LineasParaCrearNervios)
            {
                cNervio Nervio = cFunctionsProgram.CrearNervio(Prefijo, IndiceNervio, lines, F_Base.Proyecto.DatosEtabs.PisoSelect.Lista_Lines, WidthPB_NOENUMERADOS, Height_NOENUMERADOS);
                Nervio.Lista_Tramos.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.Select = false));
                NerviosPisoSelect.Add(Nervio);
                IndiceNervio = NerviosPisoSelect.Last().ID + 1;

            }
            F_Base.Proyecto.Edificio.Lista_Pisos.Find(x => x.Nombre == F_Base.Proyecto.DatosEtabs.PisoSelect.Nombre).Nervios = NerviosPisoSelect;


        }

        private void PB_ElementosEnumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            List<PointF> LineaPirmeraVez=F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.First(x => x.Type == eType.Beam).Planta_Escalado;
            if(LineaPirmeraVez== null)
            {
                List<PointF> PointsSinEscalar = new List<PointF>();
                F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } });
                F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB_NOENUMERADOS, Height_NOENUMERADOS, 0, 0, 1));
            }

            F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(x => { x.IndexSelect = 0; x.isSelect = false;x.PaintPlantaEscaladaEtabsLine(e.Graphics); });

            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Paint_Planta_ElementosEnumerados(e.Graphics));
            }
        }

        private void PB_InfoElementos_Enumerados_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.PaintNombreElementosEnumerados_MouseMove(e.Graphics, PB_InfoElementos_Enumerados.Height, PB_InfoElementos_Enumerados.Width));
            }
        }

        private void PB_ElementosEnumerados_MouseMove(object sender, MouseEventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.MouseMoveNervioPlantaEtabs(e.Location));
            }
            PB_InfoElementos_Enumerados.Invalidate();
        }
    }
}
