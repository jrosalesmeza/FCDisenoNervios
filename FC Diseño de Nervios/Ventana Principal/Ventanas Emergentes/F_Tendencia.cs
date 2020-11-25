using FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FC_Diseño_de_Nervios.Manipulación_Proyecto
{
    public partial class F_Tendencia : Form
    {

        List<cTendencia> TendenciasCreadasI { get; set; }
        List<cTendencia> TendenciasCreadasS { get; set; }
        List<cTendencia_Estribo> Tendencia_Estribos { get; set; }
        public F_Tendencia()
        {
            InitializeComponent();
        }

        private void P_1_MouseDown(object sender, MouseEventArgs e)
        {
            cHerramientas.Movimiento(Handle);
        }

        private void BT_1_Click(object sender, EventArgs e)
        {
            F_Base.EnviarEstadoVacio();
            EditarTendenciaATodosLosNervios();
            Close();
            F_Base.ActualizarTodosLasVentanas();

        }


        private void CrearDataGridView(DataGridView data,List<eNoBarra> NoBarrasSeleccionadas )
        {
            List<eNoBarra> Barras = cFunctionsProgram.NoBarras; Barras.Remove(eNoBarra.BNone);
            data.Rows.Clear();

            Barras.ForEach(Barra => {

                data.Rows.Add();
                data.Rows[data.Rows.Count - 1].Cells[0].Value = cFunctionsProgram.ConvertireNoBarraToString(Barra);
                eNoBarra Barra1 = NoBarrasSeleccionadas.Find(x=>x==Barra);
                data.Rows[data.Rows.Count - 1].Cells[1].Value = Barra1 != eNoBarra.BNone ? true : (object)false;
            });

            cFunctionsProgram.EstiloDatGridView(data);
            data.Refresh();
        }

        private void F_Tendencia_Load(object sender, EventArgs e)
        {
            TendenciasCreadasI = cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasInferior);
            TendenciasCreadasS= cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasSuperior);
            Tendencia_Estribos = cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasEstribos);
            TendenciasCreadasI.ForEach(x => x.LimpiarTendencia()); TendenciasCreadasS.ForEach(x => x.LimpiarTendencia());
            Tendencia_Estribos.ForEach(x => x.LimpiarTendencia());
            CrearObjetosTendenciasLoad();
        }

        private void CrearObjetosTendenciasLoad()
        {
            TBC_Tendencias.Controls.Clear();
            int Contador = 1;
            List<List<cTendencia>> TSAll = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Select(x => x.Tendencia_Refuerzos.TendenciasSuperior).ToList();
            List<List<cTendencia>> TIAll = F_Base.Proyecto.Edificio.PisoSelect.Nervios.Select(x => x.Tendencia_Refuerzos.TendenciasInferior).ToList();
            foreach (cTendencia Tendencia in TendenciasCreadasI)
            {
                
                float LmaxS = TSAll.Select(x => x.Find(y => y.Nombre == Tendencia.Nombre)).Max(z => z.MaximaLongitud);
                float LmaxI = TIAll.Select(x => x.Find(y => y.Nombre == Tendencia.Nombre)).Max(z => z.MaximaLongitud);
                Tendencia.MaximaLongitud = LmaxI;
                TendenciasCreadasS[Contador - 1].MaximaLongitud = LmaxS;
                cTendencia TS = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasSuperior[Contador-1];
                cTendencia TI = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasInferior[Contador - 1];
                ControlsCreate(Contador,TS,TI,LmaxS,LmaxI);
                Contador += 1;
            }

        }

        public void CrearObjetosTendenciasDespuesLoad()
        {
            TBC_Tendencias.Controls.Clear();
            int Contador = 1;
            foreach (cTendencia Tendencia in TendenciasCreadasI)
            {
                ControlsCreate(Contador,TendenciasCreadasS[Contador-1] ,Tendencia , TendenciasCreadasS[Contador - 1].MaximaLongitud, Tendencia.MaximaLongitud);
                Contador += 1;
            }
        }


        private void ControlsCreate(int Tendencia, cTendencia TS,cTendencia TI,float LmaxS,float LmaxI)
        {
            TabPage TB_PageT1 = new TabPage();
            GroupBox GB_I1 = new GroupBox();
            Label LB_PminI = new Label();
            GroupBox GB_1I = new GroupBox();
            DataGridView DGV_RBI1 = new DataGridView();
            DataGridViewTextBoxColumn C_NoBarraBI = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn C_SeleccionarBI = new DataGridViewCheckBoxColumn();
            TextBox TB_PminI = new TextBox();
            GroupBox GB_2I = new GroupBox();
            DataGridView DGV_RAI1 = new DataGridView();
            DataGridViewTextBoxColumn C_NoBarraAI = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn C_SeleccionarAI = new DataGridViewCheckBoxColumn();
            Label LB_Un1I = new Label();
            Label LB_1I = new Label();
            TextBox TB_LMaximaI = new TextBox();
            GroupBox GB_S1 = new GroupBox();
            Label LB_PminS = new Label();
            TextBox TB_PminS = new TextBox();
            Label LB_Un1 = new Label();
            TextBox TB_LMaximaS = new TextBox();
            Label LB_1S = new Label();
            GroupBox GB_2S = new GroupBox();
            DataGridView DGV_RAS1 = new DataGridView();
            DataGridViewTextBoxColumn C_NoBarraAS = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn C_SeleccionarAS = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn C_NoBarraBS = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn C_SeleccionarBS = new DataGridViewCheckBoxColumn();
            GroupBox GB_1S = new GroupBox();
            DataGridView DGV_RBS1 = new DataGridView();

            TB_PageT1.BackColor = Color.DarkGray;
            TB_PageT1.Controls.Add(GB_I1);
            TB_PageT1.Controls.Add(GB_S1);
            TB_PageT1.Location = new Point(4, 22);
            TB_PageT1.Name = "TB_PageT" + Tendencia;
            TB_PageT1.Padding = new Padding(3);
            TB_PageT1.Size = new Size(592, 410);
            TB_PageT1.TabIndex = Tendencia;
            TB_PageT1.Text = "Tendencia "+Tendencia;
            // 
            // GB_I1
            // 
            GB_I1.BackColor = Color.LightGray;
            GB_I1.Controls.Add(LB_PminI);
            GB_I1.Controls.Add(GB_1I);
            GB_I1.Controls.Add(TB_PminI);
            GB_I1.Controls.Add(GB_2I);
            GB_I1.Controls.Add(LB_Un1I);
            GB_I1.Controls.Add(LB_1I);
            GB_I1.Controls.Add(TB_LMaximaI);
            GB_I1.Font = new Font("Microsoft Sans Serif", 8.25F);
            GB_I1.Location = new Point(6, 202);
            GB_I1.Name = "GB_I"+Tendencia;
            GB_I1.Size = new Size(580, 201);
            GB_I1.TabIndex = 1;
            GB_I1.TabStop = false;
            GB_I1.Text = "Inferior";
            // 
            // LB_PminI
            // 
            LB_PminI.AutoSize = true;
            LB_PminI.Location = new Point(477, 115);
            LB_PminI.Name = "LB_PminI" + Tendencia;
            LB_PminI.Size = new Size(32, 13);
            LB_PminI.TabIndex = 18;
            LB_PminI.Text = "ρmin:";
            // 
            // GB_1I
            // 
            GB_1I.BackColor = Color.LightGray;
            GB_1I.Controls.Add(DGV_RBI1);
            GB_1I.Font = new Font("Microsoft Sans Serif", 8.25F);
            GB_1I.ForeColor = Color.Black;
            GB_1I.Location = new Point(12, 26);
            GB_1I.Name = "GB_1I" + Tendencia;
            GB_1I.Size = new Size(220, 169);
            GB_1I.TabIndex = 12;
            GB_1I.TabStop = false;
            GB_1I.Text = "Refuerzo Base Longitudinal";
            // 
            // DGV_RBI1
            // 
            DGV_RBI1.AllowUserToAddRows = false;
            DGV_RBI1.AllowUserToDeleteRows = false;
            DGV_RBI1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV_RBI1.Columns.AddRange(new DataGridViewColumn[] {
            C_NoBarraBI,
            C_SeleccionarBI});
            DGV_RBI1.Location = new Point(6, 19);
            DGV_RBI1.Name = "DGV_RBI" + Tendencia;
            DGV_RBI1.RowHeadersVisible = false;
            DGV_RBI1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGV_RBI1.Size = new Size(208, 144);
            DGV_RBI1.TabIndex = 3;
            // 
            // C_NoBarraBI
            // 
            C_NoBarraBI.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_NoBarraBI.HeaderText = "No. Barra";
            C_NoBarraBI.Name = "C_NoBarraBI" + Tendencia;
            C_NoBarraBI.ReadOnly = true;
            C_NoBarraBI.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // C_SeleccionarBI
            // 
            C_SeleccionarBI.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_SeleccionarBI.HeaderText = "Seleccionar";
            C_SeleccionarBI.Name = "C_SeleccionarBI"+Tendencia;
            C_SeleccionarBI.Resizable = DataGridViewTriState.True;
            // 
            // TB_PminI
            // 
            TB_PminI.Location = new Point(515, 112);
            TB_PminI.Name = "TB_PminI"+Tendencia;
            TB_PminI.Size = new Size(45, 20);
            TB_PminI.TabIndex = 17;
            TB_PminI.Text = "0.0018";
            // 
            // GB_2I
            // 
            GB_2I.BackColor = Color.LightGray;
            GB_2I.Controls.Add(DGV_RAI1);
            GB_2I.Font = new Font("Microsoft Sans Serif", 8.25F);
            GB_2I.ForeColor = Color.Black;
            GB_2I.Location = new Point(238, 27);
            GB_2I.Name = "GB_2I"+Tendencia;
            GB_2I.Size = new Size(220, 169);
            GB_2I.TabIndex = 13;
            GB_2I.TabStop = false;
            GB_2I.Text = "Refuerzo Adcional Longitudinal";
            // 
            // DGV_RAI1
            // 
            DGV_RAI1.AllowUserToAddRows = false;
            DGV_RAI1.AllowUserToDeleteRows = false;
            DGV_RAI1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV_RAI1.Columns.AddRange(new DataGridViewColumn[] {
            C_NoBarraAI,
            C_SeleccionarAI});
            DGV_RAI1.Location = new Point(6, 19);
            DGV_RAI1.Name = "DGV_RAI"+Tendencia;
            DGV_RAI1.RowHeadersVisible = false;
            DGV_RAI1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGV_RAI1.Size = new Size(208, 144);
            DGV_RAI1.TabIndex = 3;
            // 
            // C_NoBarraAI
            // 
            C_NoBarraAI.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_NoBarraAI.HeaderText = "No. Barra";
            C_NoBarraAI.Name = "C_NoBarraAI" + Tendencia;
            C_NoBarraAI.ReadOnly = true;
            C_NoBarraAI.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // C_SeleccionarAI
            // 
            C_SeleccionarAI.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_SeleccionarAI.HeaderText = "Seleccionar";
            C_SeleccionarAI.Name = "C_SeleccionarAI" + Tendencia;
            C_SeleccionarAI.Resizable = DataGridViewTriState.True;
            // 
            // LB_Un1I
            // 
            LB_Un1I.AutoSize = true;
            LB_Un1I.Location = new Point(565, 75);
            LB_Un1I.Name = "LB_Un1I" + Tendencia;
            LB_Un1I.Size = new Size(15, 13);
            LB_Un1I.TabIndex = 16;
            LB_Un1I.Text = "m";
            // 
            // LB_1I
            // 
            LB_1I.AutoSize = true;
            LB_1I.Location = new Point(458, 72);
            LB_1I.Name = "LB_1I" + Tendencia;
            LB_1I.Size = new Size(55, 26);
            LB_1I.TabIndex = 14;
            LB_1I.Text = "Longitud\r\n   Maxima:";
            // 
            // TB_LMaximaI
            // 
            TB_LMaximaI.Location = new Point(515, 72);
            TB_LMaximaI.Name = "TB_LMaximaI" + Tendencia;
            TB_LMaximaI.Size = new Size(45, 20);
            TB_LMaximaI.TabIndex = 15;
            TB_LMaximaI.Text = "12";
            // 
            // GB_S1
            // 
            GB_S1.BackColor = Color.LightGray;
            GB_S1.Controls.Add(LB_PminS);
            GB_S1.Controls.Add(TB_PminS);
            GB_S1.Controls.Add(LB_Un1);
            GB_S1.Controls.Add(TB_LMaximaS);
            GB_S1.Controls.Add(LB_1S);
            GB_S1.Controls.Add(GB_2S);
            GB_S1.Controls.Add(GB_1S);
            GB_S1.Font = new Font("Microsoft Sans Serif", 8.25F);
            GB_S1.ForeColor = Color.Black;
            GB_S1.Location = new Point(6, 6);
            GB_S1.Name = "GB_S1" + Tendencia;
            GB_S1.Size = new Size(580, 190);
            GB_S1.TabIndex = 0;
            GB_S1.TabStop = false;
            GB_S1.Text = "Superior";
            // 
            // LB_PminS
            // 
            LB_PminS.AutoSize = true;
            LB_PminS.Location = new Point(471, 109);
            LB_PminS.Name = "LB_PminS" + Tendencia;
            LB_PminS.Size = new Size(32, 13);
            LB_PminS.TabIndex = 11;
            LB_PminS.Text = "ρmin:";
            // 
            // TB_PminS
            // 
            TB_PminS.Location = new Point(509, 106);
            TB_PminS.Name = "TB_PminS" + Tendencia;
            TB_PminS.Size = new Size(45, 20);
            TB_PminS.TabIndex = 10;
            TB_PminS.Text = "0.0018";
            // 
            // LB_Un1
            // 
            LB_Un1.AutoSize = true;
            LB_Un1.Location = new Point(561, 69);
            LB_Un1.Name = "LB_Un1" + Tendencia;
            LB_Un1.Size = new Size(15, 13);
            LB_Un1.TabIndex = 8;
            LB_Un1.Text = "m";
            // 
            // TB_LMaximaS
            // 
            TB_LMaximaS.Location = new Point(509, 66);
            TB_LMaximaS.Name = "TB_LMaximaS" + Tendencia;
            TB_LMaximaS.Size = new Size(45, 20);
            TB_LMaximaS.TabIndex = 7;
            TB_LMaximaS.Text = "12";
            // 
            // LB_1S
            // 
            LB_1S.AutoSize = true;
            LB_1S.Location = new Point(452, 66);
            LB_1S.Name = "LB_1S" + Tendencia;
            LB_1S.Size = new Size(55, 26);
            LB_1S.TabIndex = 6;
            LB_1S.Text = "Longitud\r\n   Maxima:";
            // 
            // GB_2S
            // 
            GB_2S.BackColor = Color.LightGray;
            GB_2S.Controls.Add(DGV_RAS1);
            GB_2S.Font = new Font("Microsoft Sans Serif", 8.25F);
            GB_2S.ForeColor = Color.Black;
            GB_2S.Location = new Point(232, 21);
            GB_2S.Name = "GB_2S" + Tendencia;
            GB_2S.Size = new Size(220, 169);
            GB_2S.TabIndex = 5;
            GB_2S.TabStop = false;
            GB_2S.Text = "Refuerzo Adcional Longitudinal";
            // 
            // DGV_RAS1
            // 
            DGV_RAS1.AllowUserToAddRows = false;
            DGV_RAS1.AllowUserToDeleteRows = false;
            DGV_RAS1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV_RAS1.Columns.AddRange(new DataGridViewColumn[] {
            C_NoBarraAS,
            C_SeleccionarAS});
            DGV_RAS1.Location = new Point(6, 19);
            DGV_RAS1.Name = "DGV_RAS" + Tendencia;
            DGV_RAS1.RowHeadersVisible = false;
            DGV_RAS1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGV_RAS1.Size = new Size(208, 144);
            DGV_RAS1.TabIndex = 3;
            // 
            // C_NoBarraAS
            // 
            C_NoBarraAS.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_NoBarraAS.HeaderText = "No. Barra";
            C_NoBarraAS.Name = "C_NoBarraAS" + Tendencia;
            C_NoBarraAS.ReadOnly = true;
            C_NoBarraAS.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // C_SeleccionarAS
            // 
            C_SeleccionarAS.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_SeleccionarAS.HeaderText = "Seleccionar";
            C_SeleccionarAS.Name = "C_SeleccionarAS" + Tendencia;
            C_SeleccionarAS.Resizable = DataGridViewTriState.True;
            // 
            // GB_1S
            // 
            GB_1S.BackColor = Color.LightGray;
            GB_1S.Controls.Add(DGV_RBS1);
            GB_1S.Font = new Font("Microsoft Sans Serif", 8.25F);
            GB_1S.ForeColor = Color.Black;
            GB_1S.Location = new Point(6, 20);
            GB_1S.Name = "GB_1S" + Tendencia;
            GB_1S.Size = new Size(220, 169);
            GB_1S.TabIndex = 4;
            GB_1S.TabStop = false;
            GB_1S.Text = "Refuerzo Base Longitudinal";
            // 
            // DGV_RBS1
            // 
            DGV_RBS1.AllowUserToAddRows = false;
            DGV_RBS1.AllowUserToDeleteRows = false;
            DGV_RBS1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV_RBS1.Columns.AddRange(new DataGridViewColumn[] {
            C_NoBarraBS,
            C_SeleccionarBS});
            DGV_RBS1.Location = new Point(6, 19);
            DGV_RBS1.Name = "DGV_RBS" + Tendencia;
            DGV_RBS1.RowHeadersVisible = false;
            DGV_RBS1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGV_RBS1.Size = new Size(208, 144);
            DGV_RBS1.TabIndex = 3;
            // 
            // C_NoBarraBS
            // 
            C_NoBarraBS.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_NoBarraBS.HeaderText = "No. Barra";
            C_NoBarraBS.Name = "C_NoBarraBS" + Tendencia;
            C_NoBarraBS.ReadOnly = true;
            C_NoBarraBS.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // C_SeleccionarBS
            // 
            C_SeleccionarBS.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            C_SeleccionarBS.HeaderText = "Seleccionar";
            C_SeleccionarBS.Name = "C_SeleccionarBS" + Tendencia;
            C_SeleccionarBS.Resizable = DataGridViewTriState.True;

            TB_PageT1.SuspendLayout();
            GB_I1.SuspendLayout();
            GB_1I.SuspendLayout();
            ((ISupportInitialize)DGV_RBI1).BeginInit();
            GB_2I.SuspendLayout();
            ((ISupportInitialize)DGV_RAI1).BeginInit();
            GB_S1.SuspendLayout();
            GB_2S.SuspendLayout();
            ((ISupportInitialize)DGV_RAS1).BeginInit();
            GB_1S.SuspendLayout();
            ((ISupportInitialize)DGV_RBS1).BeginInit();

            CrearDataGridView(DGV_RBI1, TI.BarrasAEmplearBase);
            CrearDataGridView(DGV_RAI1, TI.BarrasAEmplearAdicional);
            CrearDataGridView(DGV_RBS1, TS.BarrasAEmplearBase);
            CrearDataGridView(DGV_RAS1, TS.BarrasAEmplearAdicional);

            DGV_RAI1.CellEndEdit += DGV_RAI1_CellEndEdit;
            DGV_RBI1.CellEndEdit += DGV_RAI1_CellEndEdit;
            DGV_RBS1.CellEndEdit += DGV_RAI1_CellEndEdit;
            DGV_RAS1.CellEndEdit += DGV_RAI1_CellEndEdit;

            TB_LMaximaI.TextChanged += TB_LMaximaI_TextChanged;
            TB_LMaximaS.TextChanged += TB_LMaximaI_TextChanged;
            TB_PminS.TextChanged += TB_LMaximaI_TextChanged;
            TB_PminI.TextChanged += TB_LMaximaI_TextChanged;

            TB_LMaximaS.Text = string.Format("{0:0.00}", LmaxS);
            TB_LMaximaI.Text = string.Format("{0:0.00}", LmaxI);

            TB_PminS.Text = string.Format("{0:0.00000}", TS.CuantiaMinima);
            TB_PminI.Text = string.Format("{0:0.00000}", TI.CuantiaMinima);
            TBC_Tendencias.Controls.Add(TB_PageT1);

        }

        private void TB_LMaximaI_TextChanged(object sender, EventArgs e)
        {
            TextBox TextBox = (TextBox)sender;
            float.TryParse(TextBox.Text, out float Value);
            if (Value != 0) {
                int C = 0;
                foreach (cTendencia Tendencia in TendenciasCreadasI)
                {
                    cTendencia TS = TendenciasCreadasS[C];
                    cTendencia TI = Tendencia;
                    if (TextBox.Name.Contains("TB_LMaximaI" + TI.ID.ToString())) //LMaxima Inferior
                    {
                        TI.MaximaLongitud = Value;
                    }
                    else if (TextBox.Name.Contains("TB_PminI" + TI.ID.ToString()))    //Pmin Inferior
                    {
                        TI.CuantiaMinima = Value;
                    }
                    if (TextBox.Name.Contains("TB_LMaximaS" + TS.ID.ToString())) //LMaxima Superior
                    {
                        TS.MaximaLongitud = Value;
                    }
                    else if (TextBox.Name.Contains("TB_PminS" + TS.ID.ToString()))    //Pmin Superior
                    {
                        TS.CuantiaMinima = Value;
                    }
                    C += 1;
                } 
            }


        }

        private void DGV_RAI1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data= (DataGridView)sender;

            if(e.ColumnIndex== C_SeleccionarAI.Index)
            {
                DGVEditar(data, e.RowIndex);
            }

        }

        private void DGVEditar(DataGridView data, int Row)
        {
            int C = 0;
            eNoBarra NoBarra = cFunctionsProgram.ConvertirStringToeNoBarra(data.Rows[Row].Cells[C_NoBarraBI.Index].Value.ToString());
            foreach (cTendencia Tendencia in TendenciasCreadasI)
            {
                cTendencia TS = TendenciasCreadasS[C];
                cTendencia TI = Tendencia;
                if (data.Name.Contains("DGV_RAI"+ TI.ID.ToString())) //Refuerzo Adicional Inferior
                {
                    EditarTendencia(data, Row, NoBarra, TI.BarrasAEmplearAdicional);
                }
                else if (data.Name.Contains("DGV_RBI" + TI.ID.ToString()))    //Refuerzo Base Inferior
                {
                    EditarTendencia(data, Row, NoBarra, TI.BarrasAEmplearBase);
                }
                if (data.Name.Contains("DGV_RAS" + TS.ID.ToString())) //Refuerzo Adicional Superior
                {

                    EditarTendencia(data, Row, NoBarra, TS.BarrasAEmplearAdicional);
                }
                else if (data.Name.Contains("DGV_RBS" + TS.ID.ToString()))    //Refuerzo Base Superior
                {
                    EditarTendencia(data, Row, NoBarra, TS.BarrasAEmplearBase);
                }
                C += 1;
            }

        }


        private void EditarTendencia(DataGridView data,int Row,eNoBarra NoBarra,List<eNoBarra> BarrasAEmplear)
        {
            if ((bool)data.Rows[Row].Cells[C_SeleccionarAS.Index].Value)
            {
                if (!BarrasAEmplear.Exists(x => x == NoBarra))
                {
                    BarrasAEmplear.Add(NoBarra);
                    BarrasAEmplear = BarrasAEmplear.OrderBy(y => y.GetHashCode()).ToList();
                }
                else
                {
                    BarrasAEmplear = BarrasAEmplear.OrderBy(y => y.GetHashCode()).ToList();
                }
            }
            else
            {
                if (BarrasAEmplear.Exists(x => x == NoBarra))
                {
                    BarrasAEmplear.Remove(NoBarra);
                    BarrasAEmplear = BarrasAEmplear.OrderBy(y => y.GetHashCode()).ToList();
                }
                else
                {
                    BarrasAEmplear = BarrasAEmplear.OrderBy(y => y.GetHashCode()).ToList();
                }
            }

        }


        private void BT_Agregar_Click(object sender, EventArgs e)
        {
            AgregarTendencia();
            CrearObjetosTendenciasDespuesLoad();
        }

        private void AgregarTendencia()
        {
            int ID = TendenciasCreadasI.Max(x => x.ID);
            cTendencia TI = cFunctionsProgram.DeepClone(TendenciasCreadasI.Find(x => x.ID == ID));
            cTendencia TS = cFunctionsProgram.DeepClone(TendenciasCreadasS.Find(x => x.ID == ID));
            cTendencia_Estribo TE = cFunctionsProgram.DeepClone(Tendencia_Estribos.Find(x => x.ID == ID));
            TI.ID += 1; TI.Nombre = "Tendencia " + (ID+1);
            TS.ID += 1; TS.Nombre = "Tendencia " + (ID+1);
            TE.ID += 1; TE.Nombre = "Tendencia " + (ID + 1);
            TI.LimpiarTendencia(); TS.LimpiarTendencia(); TE.LimpiarTendencia();
            TendenciasCreadasI.Add(TI);TendenciasCreadasS.Add(TS);
            Tendencia_Estribos.Add(TE);
        }




        private void EditarTendenciaATodosLosNervios()
        {

            int i = 0;
            foreach(cTendencia tendencia in TendenciasCreadasI)
            {
                cTendencia TI = tendencia;
                cTendencia TS = TendenciasCreadasS[i];
                cTendencia_Estribo TE = Tendencia_Estribos[i];
                foreach (cNervio Nervio in F_Base.Proyecto.Edificio.PisoSelect.Nervios)
                {
                    cTendencia TIN = Nervio.Tendencia_Refuerzos.TendenciasInferior.Find(x => x.ID == TI.ID);
                    cTendencia TSN = Nervio.Tendencia_Refuerzos.TendenciasSuperior.Find(x => x.ID == TS.ID);
                    cTendencia_Estribo TEN = Nervio.Tendencia_Refuerzos.TendenciasEstribos.Find(x => x.ID == TE.ID);
                    TI.Tendencia_Refuerzo_Origen.NombreNervioOrigen = Nervio.Nombre;
                    TS.Tendencia_Refuerzo_Origen.NombreNervioOrigen = Nervio.Nombre;
                    if (TIN!=null)
                    {
                        TIN.MaximaLongitud = TI.MaximaLongitud;
                        TIN.BarrasAEmplearAdicional = cFunctionsProgram.DeepClone(TI.BarrasAEmplearAdicional);
                        TIN.BarrasAEmplearBase = cFunctionsProgram.DeepClone(TI.BarrasAEmplearBase);
                    }
                    else
                    {
                        Nervio.Tendencia_Refuerzos.TendenciasInferior.Add(cFunctionsProgram.DeepClone(TI));
                    }
                    if (TSN!=null)
                    {
                        TSN.MaximaLongitud = TS.MaximaLongitud;
                        TSN.BarrasAEmplearAdicional = cFunctionsProgram.DeepClone(TS.BarrasAEmplearAdicional);
                        TSN.BarrasAEmplearBase = cFunctionsProgram.DeepClone(TS.BarrasAEmplearBase);
                    }
                    else
                    {
                        Nervio.Tendencia_Refuerzos.TendenciasSuperior.Add(cFunctionsProgram.DeepClone(TS));
                    }

                    if (TEN== null)
                    {
                        Nervio.Tendencia_Refuerzos.TendenciasEstribos.Add(cFunctionsProgram.DeepClone(TE));
                    }

                    Nervio.AsignarMaximaLongitudTendencias();

                }

                i++;
            }


            //int ID = F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasInferior.Max(x => x.ID);     
            //foreach (cNervio Nervio in F_Base.Proyecto.Edificio.PisoSelect.Nervios)
            //{
            //    cTendencia TI = cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasInferior.Find(x => x.ID == ID));
            //    cTendencia TS = cFunctionsProgram.DeepClone(F_Base.Proyecto.Edificio.PisoSelect.NervioSelect.Tendencia_Refuerzos.TendenciasSuperior.Find(x => x.ID == ID));
            //    TI.ID += 1; TI.Nombre = "Tendencia " + ID;
            //    TS.ID += 1; TS.Nombre = "Tendencia " + ID;
            //    TI.LimpiarTendencia(); TS.LimpiarTendencia();
            //    TI.Tendencia_Refuerzo_Origen.NombreNervioOrigen = Nervio.Nombre;
            //    TS.Tendencia_Refuerzo_Origen.NombreNervioOrigen = Nervio.Nombre;
            //    Nervio.Tendencia_Refuerzos.TendenciasInferior.Add(TI);
            //    Nervio.Tendencia_Refuerzos.TendenciasSuperior.Add(TS);
            //    Nervio.AsignarMaximaLongitudTendencias();
            //}            
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
