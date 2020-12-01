using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes.Similutud_de_Nervios
{
    public partial class F_AgrupacionSimilitudNervios : Form
    {
        List<cNervio> NerviosaAgrupar;
        public F_AgrupacionSimilitudNervios(List<cNervio> NerviosaAgrupar)
        {
            InitializeComponent();
            this.NerviosaAgrupar = NerviosaAgrupar;
            CargarDGV(DGV_1);
        }


        private void CargarDGV(DataGridView data)
        {
            data.Rows.Clear();
            foreach(cNervio N in NerviosaAgrupar)
            {
                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[C_NombreNervio.Index].Value = N.Nombre;
                data.Rows[data.Rows.Count - 1].Cells[C_Piso.Index].Value = N.PisoOrigen.Nombre;
            }
            cFunctionsProgram.EstiloDatGridView(data);
        }

        private void DGV_1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == C_Maestro.Index)
            {
                AsignarMaestroDGV(DGV_1,e.RowIndex);               
            }

        }


        private void AsignarMaestroDGV(DataGridView data,int RowIndex)
        {
            if (data.Rows[RowIndex].Cells[C_Maestro.Index].Value!=null && (bool)data.Rows[RowIndex].Cells[C_Maestro.Index].Value)
            {
                data.Rows[RowIndex].Cells[C_Similara.Index].Value = "";
                foreach (DataGridViewRow Row in data.Rows)
                {
                    if (Row.Index != RowIndex)
                    {
                        Row.Cells[C_Maestro.Index].Value = false;
                        Row.Cells[C_Similara.Index].Value = $"{data.Rows[RowIndex].Cells[C_NombreNervio.Index].Value} | {data.Rows[RowIndex].Cells[C_Piso.Index].Value} ";
                    }
                }
            }
            ComprobarSiTodosSonFalsos(data);
            cFunctionsProgram.EstiloDatGridView(data);
        }

        private void ComprobarSiTodosSonFalsos(DataGridView data)
        {
            bool TodosSonFalsos = false;
            foreach (DataGridViewRow Row in data.Rows)
            {
                if (Row.Cells[C_Maestro.Index].Value == null || !(bool)Row.Cells[C_Maestro.Index].Value)
                {
                    TodosSonFalsos = true;
                }
                else
                {
                    TodosSonFalsos = false;
                    break;
                }
            }
            if (TodosSonFalsos)
            {
                foreach (DataGridViewRow Row in data.Rows)
                {
                    Row.Cells[C_Similara.Index].Value = "";
                }
            }


        }

        private void CKBs_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == CKB_Geometria)
            {
                CKB_Todo.Checked = !CKB_Geometria.Checked;
            }
            else if (sender == CKB_Todo)
            {
                CKB_Geometria.Checked = !CKB_Todo.Checked;
                
            }
        }






        private void P_2_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void ConfirmarSimilitudes(DataGridView data)
        {
            bool IsGeometria = CKB_Geometria.Checked;
            List<string> MensajeAlerta = new List<string>();

            NerviosaAgrupar.ForEach(x => { x.SimilitudNervioGeometria.Similares_List_SimilarA = null; x.SimilitudNervioGeometria.BoolSoySimiarA = false; });
            foreach (DataGridViewRow row in data.Rows)
            {
                string NombreNervio = row.Cells[C_NombreNervio.Index].Value.ToString();
                string NombrePiso= row.Cells[C_Piso.Index].Value.ToString();
                bool IsMaestro = (bool)row.Cells[C_Maestro.Index].Value;
                string SoySimilarANombreNervio = ""; string SoySimilarANombrePiso = "";
                if (row.Cells[C_Similara.Index].Value != null && row.Cells[C_Similara.Index].Value.ToString()!="" )
                {
                    string[] Separate = row.Cells[C_Similara.Index].Value.ToString().Split(new string[] { "|"," " },StringSplitOptions.RemoveEmptyEntries);
                    SoySimilarANombreNervio = Separate[0];
                    SoySimilarANombrePiso = Separate[1];
                }
                cNervio NervioMaestro = NerviosaAgrupar.Find(x => x.Nombre == SoySimilarANombreNervio && x.PisoOrigen.Nombre == SoySimilarANombrePiso );
                cNervio NervioSimilar = NerviosaAgrupar.Find(x => x.Nombre == NombreNervio && x.PisoOrigen.Nombre == NombrePiso);
                cFunctionsProgram.AsignarSimilitud(NervioMaestro, NervioSimilar, ref MensajeAlerta, IsGeometria);

            }
            List<cNervio> NerviosOrganizados = IsGeometria
                ? NerviosaAgrupar.OrderBy(y => !y.SimilitudNervioGeometria.IsMaestro).ToList()
                : NerviosaAgrupar.OrderBy(y => !y.SimilitudNervioCompleto.IsMaestro).ToList();
            NerviosOrganizados.ForEach(y => y.CrearEnvolvente());
            NerviosOrganizados.ForEach(y => y.BloquearNervio = y.SimilitudNervioCompleto.BoolSoySimiarA);

            RB_Alerta.Clear();
            RB_Alerta.Lines = MensajeAlerta.ToArray();
            NerviosaAgrupar.ForEach(y => y.SelectSimilar = false);
            if (MensajeAlerta.Count == 0)
            {
                Close();
            }
        }
        private void BT_1_Click(object sender, EventArgs e)
        {
            ConfirmarSimilitudes(DGV_1);
        }
    }
}
