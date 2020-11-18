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

namespace FC_Diseño_de_Nervios
{
    public partial class F_PlantaNervios : DockContent
    {
        float DesXButton;
        float DesYButton;
        float Dx = 0;
        float Dy = 0;
        float Zoom = 1;
        public F_PlantaNervios()
        {
            InitializeComponent();
            PB_Nervios.MouseWheel += PB_Nervios_MouseWheel;
            PB_Nervios.MouseDown += PB_Nervios_MouseDown;
            PB_Nervios.MouseMove += PB_Nervios_MouseMove;
            PB_Nervios.MouseMove += PB_Nervios_MouseMove1;

        }

        private void PB_Nervios_MouseMove1(object sender, MouseEventArgs e)
        {
            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null && F_Base.Proyecto.CoordenadasPInterseccion)
            {
                F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.IsPointMousePointLines(e.Location));
                PB_Nervios.Invalidate();
            }
        }

        private void PB_Nervios_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor CursorArraste = new Cursor(Properties.Resources.Arrastre16x16.Handle);
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
                PB_Nervios.Invalidate();
            }
            PB_Nervios.Cursor = cursor;
        }

        private void PB_Nervios_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks == 2)
            {
                Zoom = 1;
                Dx = 0; Dy = 0;
                PB_Nervios.Invalidate();
            }
            if (e.Button == MouseButtons.Middle)
            {
                DesXButton = e.X;
                DesYButton = e.Y;
            }
        }

        private void PB_Nervios_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            float XI = 5f; float YI = 5f;
            float WidthPB = PB_Nervios.Width - XI * 3;
            float HeightPB = PB_Nervios.Height - YI * 3;
            List<PointF> PointsSinEscalar = new List<PointF>();

            F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) { PointsSinEscalar.AddRange(y.Planta_Real); } });

            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => { PointsSinEscalar.AddRange(y.Recta_Real); });


            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.CrearPuntosPlantaEscaladaEtabs(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));

            if (F_Base.PropiedadesPrograma.LineasPretrazado)
            {
                if (F_Base.PropiedadesPrograma.FuncionesEnParalelo)
                {

                    Parallel.ForEach(F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines, y =>
                    {
                        cFunctionsProgram.CheckCPUUsageAndSleepThread(cFunctionsProgram.cpuCounter);
                        y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom);

                    });
                }
                else
                {
                    F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => y.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
                }
                F_Base.Proyecto.Edificio.PisoSelect.Lista_Lines.ForEach(y => { if (y.Type == eType.Beam) y.PaintPlantaEscaladaEtabsLine(e.Graphics); });
            }

            F_Base.Proyecto.Edificio.Lista_Grids.ForEach(y => y.Paint(e.Graphics, Zoom));

            if (F_Base.Proyecto.Edificio.PisoSelect.Nervios != null)
            {
                if (F_Base.PropiedadesPrograma.FuncionesEnParalelo)
                {
                    Parallel.ForEach(F_Base.Proyecto.Edificio.PisoSelect.Nervios,
                    x =>
                    {
                        cFunctionsProgram.CheckCPUUsageAndSleepThread(cFunctionsProgram.cpuCounter);
                        x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));

                    });

                    F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(x => x.Paint_Planta_ElementosEnumerados(e.Graphics, PB_Nervios.Width, Zoom));
                }
                else
                {
                    F_Base.Proyecto.Edificio.PisoSelect.Nervios.ForEach(
                    x =>
                    {
                        x.Lista_Objetos.ForEach(y => y.Line.CrearPuntosPlantaEscaladaEtabsLine(PointsSinEscalar, WidthPB, HeightPB, Dx, Dy, Zoom));
                        x.Paint_Planta_ElementosEnumerados(e.Graphics, PB_Nervios.Width, Zoom);
                    });
                }
            }
        }

        private void PB_Nervios_MouseWheel(object sender, MouseEventArgs e)
        {
            int Vueltas = e.Delta;
            if (Vueltas > 0)
            {
                Zoom += 0.1f;
                PB_Nervios.Invalidate();
            }
            else
            {
                Zoom -= 0.1f;
                PB_Nervios.Invalidate();
            }
        }


        private void PB_Nervios_MouseDown_SelectNervio(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
               F_Base.F_SelectNervio.SelectNervioChanged(e.Location, true);
            }
        }



        private void F_PlantaNervios_Paint(object sender, PaintEventArgs e)
        {

        }

        private void F_PlantaNervios_Resize(object sender, EventArgs e)
        {
            PB_Nervios.Invalidate();
        }
    }
}
