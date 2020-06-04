namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes
{
    partial class F_Informe
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Informe));
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewRichTextBoxColumn1 = new FC_Diseño_de_Nervios.Controles.DataGridViewRichTextBoxColumn();
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_Title = new FontAwesome.Sharp.IconButton();
            this.BT_1 = new System.Windows.Forms.Button();
            this.DGV_Info = new System.Windows.Forms.DataGridView();
            this.P_2 = new System.Windows.Forms.Panel();
            this.C_NombreNervio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_Diseno = new System.Windows.Forms.DataGridViewImageColumn();
            this.C_Observacion = new FC_Diseño_de_Nervios.Controles.DataGridViewRichTextBoxColumn();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Info)).BeginInit();
            this.P_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewImageColumn1.HeaderText = "Diseño";
            this.dataGridViewImageColumn1.MinimumWidth = 80;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewRichTextBoxColumn1
            // 
            this.dataGridViewRichTextBoxColumn1.FillWeight = 320F;
            this.dataGridViewRichTextBoxColumn1.HeaderText = "Observación";
            this.dataGridViewRichTextBoxColumn1.MinimumWidth = 53;
            this.dataGridViewRichTextBoxColumn1.Name = "dataGridViewRichTextBoxColumn1";
            this.dataGridViewRichTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewRichTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewRichTextBoxColumn1.Width = 320;
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.LB_Title);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(510, 31);
            this.P_1.TabIndex = 9;
            this.P_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // LB_Title
            // 
            this.LB_Title.FlatAppearance.BorderSize = 0;
            this.LB_Title.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_Title.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_Title.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_Title.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Title.ForeColor = System.Drawing.Color.White;
            this.LB_Title.IconChar = FontAwesome.Sharp.IconChar.Flag;
            this.LB_Title.IconColor = System.Drawing.Color.White;
            this.LB_Title.IconSize = 22;
            this.LB_Title.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_Title.Location = new System.Drawing.Point(3, 1);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Rotation = 0D;
            this.LB_Title.Size = new System.Drawing.Size(109, 27);
            this.LB_Title.TabIndex = 3;
            this.LB_Title.Text = "Informe";
            this.LB_Title.UseVisualStyleBackColor = true;
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // BT_1
            // 
            this.BT_1.Location = new System.Drawing.Point(424, 522);
            this.BT_1.Name = "BT_1";
            this.BT_1.Size = new System.Drawing.Size(75, 23);
            this.BT_1.TabIndex = 10;
            this.BT_1.Text = "Aceptar";
            this.BT_1.UseVisualStyleBackColor = true;
            this.BT_1.Click += new System.EventHandler(this.BT_1_Click);
            // 
            // DGV_Info
            // 
            this.DGV_Info.AllowUserToAddRows = false;
            this.DGV_Info.AllowUserToDeleteRows = false;
            this.DGV_Info.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Info.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C_NombreNervio,
            this.C_Diseno,
            this.C_Observacion});
            this.DGV_Info.Location = new System.Drawing.Point(11, 37);
            this.DGV_Info.Name = "DGV_Info";
            this.DGV_Info.RowHeadersVisible = false;
            this.DGV_Info.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_Info.Size = new System.Drawing.Size(488, 479);
            this.DGV_Info.TabIndex = 3;
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.LightGray;
            this.P_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_2.Controls.Add(this.DGV_Info);
            this.P_2.Controls.Add(this.BT_1);
            this.P_2.Controls.Add(this.P_1);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(512, 550);
            this.P_2.TabIndex = 10;
            // 
            // C_NombreNervio
            // 
            this.C_NombreNervio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_NombreNervio.FillWeight = 80F;
            this.C_NombreNervio.HeaderText = "Nervio";
            this.C_NombreNervio.Name = "C_NombreNervio";
            this.C_NombreNervio.ReadOnly = true;
            this.C_NombreNervio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // C_Diseno
            // 
            this.C_Diseno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_Diseno.HeaderText = "Diseño";
            this.C_Diseno.MinimumWidth = 80;
            this.C_Diseno.Name = "C_Diseno";
            this.C_Diseno.ReadOnly = true;
            this.C_Diseno.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // C_Observacion
            // 
            this.C_Observacion.FillWeight = 320F;
            this.C_Observacion.HeaderText = "Observación";
            this.C_Observacion.MinimumWidth = 53;
            this.C_Observacion.Name = "C_Observacion";
            this.C_Observacion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.C_Observacion.Width = 320;
            // 
            // F_Informe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 550);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_Informe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Informe";
            this.Load += new System.EventHandler(this.F_Informe_Load);
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Info)).EndInit();
            this.P_2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private Controles.DataGridViewRichTextBoxColumn dataGridViewRichTextBoxColumn1;
        private System.Windows.Forms.Panel P_1;
        private FontAwesome.Sharp.IconButton LB_Title;
        private System.Windows.Forms.Button BT_1;
        private System.Windows.Forms.DataGridView DGV_Info;
        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_NombreNervio;
        private System.Windows.Forms.DataGridViewImageColumn C_Diseno;
        private Controles.DataGridViewRichTextBoxColumn C_Observacion;
    }
}