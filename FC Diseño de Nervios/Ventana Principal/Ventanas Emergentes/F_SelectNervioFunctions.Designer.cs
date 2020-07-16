namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes
{
    partial class F_SelectNervioFunctions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_SelectNervioFunctions));
            this.P_2 = new System.Windows.Forms.Panel();
            this.BT_Seleccionar = new System.Windows.Forms.Button();
            this.BT_Cancelar = new System.Windows.Forms.Button();
            this.DGV_Info = new System.Windows.Forms.DataGridView();
            this.C_NombreNervio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_Seleccionar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BT_Aceptar = new System.Windows.Forms.Button();
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_Title = new FontAwesome.Sharp.IconButton();
            this.P_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Info)).BeginInit();
            this.P_1.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.LightGray;
            this.P_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_2.Controls.Add(this.BT_Seleccionar);
            this.P_2.Controls.Add(this.BT_Cancelar);
            this.P_2.Controls.Add(this.DGV_Info);
            this.P_2.Controls.Add(this.BT_Aceptar);
            this.P_2.Controls.Add(this.P_1);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(270, 418);
            this.P_2.TabIndex = 11;
            // 
            // BT_Seleccionar
            // 
            this.BT_Seleccionar.Location = new System.Drawing.Point(94, 386);
            this.BT_Seleccionar.Name = "BT_Seleccionar";
            this.BT_Seleccionar.Size = new System.Drawing.Size(82, 23);
            this.BT_Seleccionar.TabIndex = 12;
            this.BT_Seleccionar.Text = "Seleccionar";
            this.BT_Seleccionar.UseVisualStyleBackColor = true;
            this.BT_Seleccionar.Click += new System.EventHandler(this.BT_Seleccionar_Click);
            // 
            // BT_Cancelar
            // 
            this.BT_Cancelar.Location = new System.Drawing.Point(20, 386);
            this.BT_Cancelar.Name = "BT_Cancelar";
            this.BT_Cancelar.Size = new System.Drawing.Size(75, 23);
            this.BT_Cancelar.TabIndex = 11;
            this.BT_Cancelar.Text = "Cancelar";
            this.BT_Cancelar.UseVisualStyleBackColor = true;
            this.BT_Cancelar.Click += new System.EventHandler(this.BT_Cancelar_Click);
            // 
            // DGV_Info
            // 
            this.DGV_Info.AllowUserToAddRows = false;
            this.DGV_Info.AllowUserToDeleteRows = false;
            this.DGV_Info.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Info.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C_NombreNervio,
            this.C_Seleccionar});
            this.DGV_Info.Location = new System.Drawing.Point(11, 37);
            this.DGV_Info.Name = "DGV_Info";
            this.DGV_Info.RowHeadersVisible = false;
            this.DGV_Info.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_Info.Size = new System.Drawing.Size(244, 343);
            this.DGV_Info.TabIndex = 3;
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
            // C_Seleccionar
            // 
            this.C_Seleccionar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_Seleccionar.HeaderText = "Diseño";
            this.C_Seleccionar.MinimumWidth = 80;
            this.C_Seleccionar.Name = "C_Seleccionar";
            this.C_Seleccionar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // BT_Aceptar
            // 
            this.BT_Aceptar.Location = new System.Drawing.Point(182, 386);
            this.BT_Aceptar.Name = "BT_Aceptar";
            this.BT_Aceptar.Size = new System.Drawing.Size(75, 23);
            this.BT_Aceptar.TabIndex = 10;
            this.BT_Aceptar.Text = "Aceptar";
            this.BT_Aceptar.UseVisualStyleBackColor = true;
            this.BT_Aceptar.Click += new System.EventHandler(this.BT_Aceptar_Click);
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.LB_Title);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(268, 31);
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
            this.LB_Title.IconChar = FontAwesome.Sharp.IconChar.CheckSquare;
            this.LB_Title.IconColor = System.Drawing.Color.White;
            this.LB_Title.IconSize = 20;
            this.LB_Title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LB_Title.Location = new System.Drawing.Point(3, 1);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Rotation = 0D;
            this.LB_Title.Size = new System.Drawing.Size(223, 27);
            this.LB_Title.TabIndex = 3;
            this.LB_Title.Text = "Seleccionar Nervios a Graficar";
            this.LB_Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LB_Title.UseVisualStyleBackColor = true;
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // F_SelectNervioFunctions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 418);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(270, 418);
            this.MinimumSize = new System.Drawing.Size(270, 418);
            this.Name = "F_SelectNervioFunctions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Nervios";
            this.P_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Info)).EndInit();
            this.P_1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.DataGridView DGV_Info;
        private System.Windows.Forms.Button BT_Aceptar;
        private System.Windows.Forms.Panel P_1;
        private FontAwesome.Sharp.IconButton LB_Title;
        private System.Windows.Forms.Button BT_Cancelar;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_NombreNervio;
        private System.Windows.Forms.DataGridViewCheckBoxColumn C_Seleccionar;
        private System.Windows.Forms.Button BT_Seleccionar;
    }
}