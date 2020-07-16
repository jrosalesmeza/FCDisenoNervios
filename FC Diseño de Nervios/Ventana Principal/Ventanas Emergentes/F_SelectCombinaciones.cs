using FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas;
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
    public partial class F_SelectCombinaciones : Form
    {
        public F_SelectCombinaciones()
        {
            InitializeComponent();
        }

        private void CargarDGV()
        {
            DGV_1.Rows.Clear();
            List<cSolicitacion> Solicitaciones = cFuncion_CargarSolcitaciones.ListaConMayorSolicitaciones(F_Base.Proyecto.Edificio.PisoSelect.Nervios);

            Solicitaciones.ForEach(Solicitacion => {

                DGV_1.Rows.Add();
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_NombreCombinacion.Name].Value = Solicitacion.Nombre;
                DGV_1.Rows[DGV_1.Rows.Count - 1].Cells[C_CheckCombinacion.Name].Value = Solicitacion.SelectEnvolvente;
            });
            cFunctionsProgram.EstiloDatGridView(DGV_1);
        }

        private void ConfirmarCambios()
        {
            F_Base.EnviarEstadoVacio();
            foreach (cPiso Piso in F_Base.Proyecto.Edificio.Lista_Pisos) 
            {
                List<cNervio> NerviosOrdenados = Piso.Nervios.OrderBy(y => !y.SimilitudNervioCompleto.IsMaestro).ToList();

                foreach (cNervio Nervio in NerviosOrdenados)
                {
                    List<IElemento> Subtramos = Nervio.Lista_Elementos.FindAll(x => x is cSubTramo).ToList();
                    for (int i = 0; i < DGV_1.Rows.Count; i++)
                    {
                        Subtramos.ForEach(subtramo =>
                        {
                            cSubTramo SubTramoAux = (cSubTramo)subtramo;
                            SubTramoAux.Estaciones.ForEach(x => {
                                cSolicitacion solicitacionFind = x.Lista_Solicitaciones.Find(y => y.Nombre == (string)DGV_1.Rows[i].Cells[C_NombreCombinacion.Index].Value);
                                if (solicitacionFind != null)
                                {
                                    solicitacionFind.SelectEnvolvente = (bool)DGV_1.Rows[i].Cells[C_CheckCombinacion.Index].Value;
                                }
                            });
                        });
                    }
                    Nervio.CrearEnvolvente();
                    Nervio.CrearAceroAsignadoRefuerzoLongitudinal();
                } 
            }
        }

        private void DGV_1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == C_CheckCombinacion.Index)
            {
                int BooleanosContador = 0;
                for (int i = 0; i < DGV_1.Rows.Count; i++)
                {
                    if ((bool)DGV_1.Rows[i].Cells[C_CheckCombinacion.Index].Value)
                    {
                        BooleanosContador += 1;
                    }
                }
                if (BooleanosContador == 0)
                {
                    DGV_1.Rows[e.RowIndex].Cells[C_CheckCombinacion.Index].Value = true;
                }

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

        private void BT_1_Click(object sender, EventArgs e)
        {
            ConfirmarCambios();
            F_Base.ActualizarVentanaF_NervioEnPerfilLongitudinal();
            Close();
        }


        private void BT_Seleccionar_Click(object sender, EventArgs e)
        {
            cFunctionsProgram.SeleccionDataGridView(DGV_1, C_CheckCombinacion.Index, true);
        }

        private void F_SelectCombinaciones_Load(object sender, EventArgs e)
        {
            CargarDGV();
        }

        private void BT_ImportarCombianciones_Click(object sender, EventArgs e)
        {
            Tuple<string, List<string>> CargarCSV = cFunctionsProgram.CagarArchivoTextoPlanoWindowsForm("Archivo de Fuerzas |*.csv", "Archivo de Fuerzas, Unidades en |Ton- m|");
            if (CargarCSV != null)
            {
                List<string> ErroresCSV = cFunctionsProgram.CoprobarErroresArchivoCSV(CargarCSV.Item2);
                
                foreach (string Error in ErroresCSV)
                {
                    cFunctionsProgram.VentanaEmergenteExclamacion(Error);
                }
                if (ErroresCSV.Count ==0)
                {
                    cFuncion_CargarSolcitaciones.CargarNuevasSolicitacionesANevios(CargarCSV.Item2, F_Base.Proyecto.Edificio.PisoSelect.Nervios);
                    CargarDGV();
                }
            }

        }
    }
}
