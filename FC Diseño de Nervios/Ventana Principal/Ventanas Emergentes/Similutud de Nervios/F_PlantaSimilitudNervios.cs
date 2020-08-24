using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes.Similutud_de_Nervios
{
    public partial class F_PlantaSimilitudNervios : Form
    {

        float DesXButton, DesYButton;
        private float Dx, Dy;
        private float Zoom = 1;

        private cPiso PisoSelect;
        private List<cPiso> Pisos;
        private List<cGrid> Grids;
        private List<cNervio> GrupoNervios;
        private cNervio NervioSelect;

        public F_PlantaSimilitudNervios(cEdificio Edificio)
        {
            InitializeComponent();

            P_Title.MouseDown += P_Title_MouseDown; 
            LB_Title.MouseDown += P_Title_MouseDown; 
            PB_Icono.MouseDown += P_Title_MouseDown;

            GL_Control1.MouseMove += GL_Control1_MouseMove; GL_Control1.MouseMove += GL_Control1_MouseMove1;
            GL_Control1.MouseDown += GL_Control1_MouseDown1;
            GL_Control1.MouseDown += GL_Control1_MouseDown2;
            GL_Control1.MouseWheel += GL_Control1_MouseWheel;

            Pisos = Edificio.Lista_Pisos;
            Grids = Edificio.Lista_Grids;
            LB_Nervio.Text = "";
            LoadLV(LV_Stories);
        }

 

        private void P_Title_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void LoadLV(ListView LV)
        {
            LV.Items.Clear();

            foreach(cPiso Piso in Pisos)
            {
                ListViewItem Item = new ListViewItem(); Item.Text = Piso.Nombre;
                LV.Items.Add(Item);
            }
            LV.Items[LV.Items.Count - 1].Selected = true;
        }


        private void BT_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void GL_Control1_Paint(object sender, PaintEventArgs e)
        {
            int XI = 5; int YI = 5;
            int Width = GL_Control1.Width-XI*3;
            int Height = GL_Control1.Height-YI*3;

            List<PointF> PointsSinEscalar = new List<PointF>();
            PisoSelect.Nervios.ForEach(x => x.Lista_Objetos.ForEach(y => PointsSinEscalar.AddRange(y.Line.Planta_Real)));
            PisoSelect.Lista_Lines.ForEach(y => PointsSinEscalar.AddRange(y.Planta_Real));

            PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, Width, Height, Dx, Dy, Zoom,true));
            PisoSelect.Nervios.ForEach(x => x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, Width, Height, Dx,Dy, Zoom,true)));

            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, 0, Height, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.ClearColor(Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            PisoSelect.Lista_Lines.ForEach(y => y.PaintPlantaEscalada(false, isSelect: false));
            PisoSelect.Nervios.ForEach(y => y.Paint_SimilaresOpenGL());

            GL.Flush();
            GL_Control1.SwapBuffers();


        }
        private void GL_Control1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PisoSelect.Nervios.ForEach(y => y.MouseDownSelectSimilar(new Point(e.X, GL_Control1.Height - e.Y)));
                GL_Control1.Invalidate();
            }
        }
        private void GL_Control1_MouseMove(object sender, MouseEventArgs e)
        {
            cNervio FindNervio = PisoSelect.Nervios.Find(y => y.MouseDownSelectSimilar(new Point(e.X, GL_Control1.Height- e.Y), false));
            if (FindNervio != null)
                LB_Nervio.Text = FindNervio.Nombre;
            else
                LB_Nervio.Text = "";
        }
        private void GL_Control1_MouseMove1(object sender, MouseEventArgs e)
        {
            Cursor CursorArraste = new Cursor(Properties.Resources.Arrastre16x16.Handle);
            Cursor CursorDefault = Cursors.Default;
            Cursor cursor = CursorDefault;
            if (e.Button == MouseButtons.Middle)
            {
                float DesplazX = Dx + (e.X - DesXButton);
                float DesplazY = Dy + (GL_Control1.Height - e.Y - DesYButton);
                Dx = DesplazX;
                Dy = DesplazY;
                DesXButton = e.X;
                DesYButton = GL_Control1.Height - e.Y;
                cursor = CursorArraste;
                GL_Control1.Invalidate();
            }
            GL_Control1.Cursor = cursor;
        }
        private void GL_Control1_MouseDown1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks == 2)
            {
                Zoom = 1;
                Dx = 0; Dy = 0;
                GL_Control1.Invalidate();
            }
            if (e.Button == MouseButtons.Middle)
            {
                DesXButton = e.X;
                DesYButton = GL_Control1.Height - e.Y;
            }
        }

        private void GL_Control1_MouseDown2(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Right)
            {
                NervioSelect = PisoSelect.Nervios.Find(y => y.MouseDownSelectSimilar(new Point(e.X, GL_Control1.Height - e.Y), false));
                if (NervioSelect != null)
                {
                    if (NervioSelect.SimilitudNervioCompleto.BoolSoySimiarA | NervioSelect.SimilitudNervioCompleto.IsMaestro
                                | NervioSelect.SimilitudNervioGeometria.BoolSoySimiarA | NervioSelect.SimilitudNervioGeometria.IsMaestro)
                    {
                        GL_Control1.ContextMenuStrip = CMS_1;

                    }
                    else
                    {
                        GL_Control1.ContextMenuStrip = null;
                    }
                }
                else
                {
                    GL_Control1.ContextMenuStrip = null;
                }
            }
        }
        private void GL_Control1_MouseWheel(object sender, MouseEventArgs e)
        {
            int Vueltas = e.Delta;
            if (Vueltas > 0)
            {
                Zoom += 0.1f;
                GL_Control1.Invalidate();
            }
            else
            {
                Zoom -= 0.1f;
                GL_Control1.Invalidate();
            }

        }

        private void TLSB_Agrupar_Click(object sender, EventArgs e)
        {
            BuscarNerviosAgrupar();
        }

        private void TLSB_LimpiarSeleccion_Click(object sender, EventArgs e)
        {
            PisoSelect.Nervios.ForEach(y => y.SelectSimilar = false);
            GL_Control1.Invalidate();
        }

        private void TLSMI_Desagrupar_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult= DialogResult.OK;
            if(NervioSelect.SimilitudNervioGeometria.IsMaestro || NervioSelect.SimilitudNervioCompleto.IsMaestro)
            {
                dialogResult = MessageBox.Show("Al desagrupar el nervio maestro se desagruparán los demás nervios, ¿Desea continuar?", cFunctionsProgram.Empresa, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
            if (dialogResult == DialogResult.Yes)
            {
                cFunctionsProgram.DesacoplarSimilitudMaestro(NervioSelect);
            }else if(dialogResult== DialogResult.OK)
                cFunctionsProgram.DesacoplarSimilitudMaestro(NervioSelect);

            GL_Control1.Invalidate();

        }

        private void BuscarNerviosAgrupar()
        {
            GrupoNervios = new List<cNervio>();

            Pisos.ForEach(Piso => {
                GrupoNervios.AddRange(Piso.Nervios.FindAll(y => y.SelectSimilar));
            });
            GrupoNervios.RemoveAll(y => y == null);
            if (GrupoNervios.Count > 0)
            {
                F_AgrupacionSimilitudNervios f_AgrupacionSimilitud = new F_AgrupacionSimilitudNervios(GrupoNervios);
                f_AgrupacionSimilitud.ShowDialog();
                GL_Control1.Invalidate();
            }
        }


        private void LV_Stories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_Stories.SelectedItems.Count == 1)
            {
                cPiso temp = Pisos.Find(y => y.Nombre == LV_Stories.SelectedItems[0].Text); 
                if (PisoSelect!=null && temp.Nombre != PisoSelect.Nombre)
                {
                    PisoSelect = temp;
                    GL_Control1.Invalidate();
                }
                else if (PisoSelect == null)
                {
                    PisoSelect = temp;
                    GL_Control1.Invalidate();
                }
                LB_Title.Text = $"Similitud de Nervios  | {PisoSelect.Nombre}";
            }
            
        }
    }
}
