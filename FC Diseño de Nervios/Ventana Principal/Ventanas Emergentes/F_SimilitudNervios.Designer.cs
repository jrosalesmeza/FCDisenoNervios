namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes
{
    partial class F_SimilitudNervios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_SimilitudNervios));
            this.P_1 = new System.Windows.Forms.Panel();
            this.BT_1 = new System.Windows.Forms.Button();
            this.BT_2 = new System.Windows.Forms.Button();
            this.P_2 = new System.Windows.Forms.Panel();
            this.LB_Encabezado = new FontAwesome.Sharp.IconButton();
            this.DGV_1 = new System.Windows.Forms.DataGridView();
            this.C_NombreNervio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_Maestro = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.C_Similara = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RB_Alerta = new System.Windows.Forms.RichTextBox();
            this.P_1.SuspendLayout();
            this.P_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).BeginInit();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.LightGray;
            this.P_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_1.Controls.Add(this.RB_Alerta);
            this.P_1.Controls.Add(this.BT_1);
            this.P_1.Controls.Add(this.BT_2);
            this.P_1.Controls.Add(this.P_2);
            this.P_1.Controls.Add(this.DGV_1);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(349, 459);
            this.P_1.TabIndex = 3;
            // 
            // BT_1
            // 
            this.BT_1.Location = new System.Drawing.Point(258, 397);
            this.BT_1.Name = "BT_1";
            this.BT_1.Size = new System.Drawing.Size(75, 23);
            this.BT_1.TabIndex = 6;
            this.BT_1.Text = "Aceptar";
            this.BT_1.UseVisualStyleBackColor = true;
            this.BT_1.Click += new System.EventHandler(this.BT_1_Click);
            // 
            // BT_2
            // 
            this.BT_2.Location = new System.Drawing.Point(258, 426);
            this.BT_2.Name = "BT_2";
            this.BT_2.Size = new System.Drawing.Size(75, 23);
            this.BT_2.TabIndex = 7;
            this.BT_2.Text = "Cancelar";
            this.BT_2.UseVisualStyleBackColor = true;
            this.BT_2.Click += new System.EventHandler(this.BT_2_Click);
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.Gray;
            this.P_2.Controls.Add(this.LB_Encabezado);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(347, 31);
            this.P_2.TabIndex = 5;
            this.P_2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_2_MouseDown);
            // 
            // LB_Encabezado
            // 
            this.LB_Encabezado.FlatAppearance.BorderSize = 0;
            this.LB_Encabezado.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_Encabezado.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_Encabezado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_Encabezado.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_Encabezado.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Encabezado.ForeColor = System.Drawing.Color.White;
            this.LB_Encabezado.IconChar = FontAwesome.Sharp.IconChar.CheckSquare;
            this.LB_Encabezado.IconColor = System.Drawing.Color.White;
            this.LB_Encabezado.IconSize = 22;
            this.LB_Encabezado.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_Encabezado.Location = new System.Drawing.Point(3, 1);
            this.LB_Encabezado.Name = "LB_Encabezado";
            this.LB_Encabezado.Rotation = 0D;
            this.LB_Encabezado.Size = new System.Drawing.Size(156, 27);
            this.LB_Encabezado.TabIndex = 3;
            this.LB_Encabezado.Text = "     Similitud de Nervios";
            this.LB_Encabezado.UseVisualStyleBackColor = true;
            this.LB_Encabezado.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_2_MouseDown);
            // 
            // DGV_1
            // 
            this.DGV_1.AllowUserToAddRows = false;
            this.DGV_1.AllowUserToDeleteRows = false;
            this.DGV_1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DGV_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C_NombreNervio,
            this.C_Maestro,
            this.C_Similara});
            this.DGV_1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.DGV_1.Location = new System.Drawing.Point(13, 37);
            this.DGV_1.Name = "DGV_1";
            this.DGV_1.RowHeadersVisible = false;
            this.DGV_1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_1.Size = new System.Drawing.Size(320, 354);
            this.DGV_1.TabIndex = 0;
            this.DGV_1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_1_CellValueChanged);
            // 
            // C_NombreNervio
            // 
            this.C_NombreNervio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_NombreNervio.FillWeight = 60F;
            this.C_NombreNervio.HeaderText = "Nervio";
            this.C_NombreNervio.Name = "C_NombreNervio";
            this.C_NombreNervio.ReadOnly = true;
            this.C_NombreNervio.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.C_NombreNervio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // C_Maestro
            // 
            this.C_Maestro.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_Maestro.HeaderText = "Maestro";
            this.C_Maestro.Name = "C_Maestro";
            this.C_Maestro.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // C_Similara
            // 
            this.C_Similara.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_Similara.HeaderText = "Similar a";
            this.C_Similara.Name = "C_Similara";
            this.C_Similara.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // RB_Alerta
            // 
            this.RB_Alerta.BackColor = System.Drawing.SystemColors.ControlLight;
            this.RB_Alerta.ForeColor = System.Drawing.Color.Maroon;
            this.RB_Alerta.Location = new System.Drawing.Point(13, 397);
            this.RB_Alerta.Name = "RB_Alerta";
            this.RB_Alerta.ReadOnly = true;
            this.RB_Alerta.Size = new System.Drawing.Size(239, 52);
            this.RB_Alerta.TabIndex = 8;
            this.RB_Alerta.Text = "";
            // 
            // F_SimilitudNervios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 459);
            this.Controls.Add(this.P_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_SimilitudNervios";
            this.Text = "Similitud de Nervios";
            this.Load += new System.EventHandler(this.F_SimilitudNervios_Load);
            this.P_1.ResumeLayout(false);
            this.P_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.Button BT_1;
        private System.Windows.Forms.Button BT_2;
        private System.Windows.Forms.Panel P_2;
        private FontAwesome.Sharp.IconButton LB_Encabezado;
        private System.Windows.Forms.DataGridView DGV_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_NombreNervio;
        private System.Windows.Forms.DataGridViewCheckBoxColumn C_Maestro;
        private System.Windows.Forms.DataGridViewComboBoxColumn C_Similara;
        private System.Windows.Forms.RichTextBox RB_Alerta;
    }
}