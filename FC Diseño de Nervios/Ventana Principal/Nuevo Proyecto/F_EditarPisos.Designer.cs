namespace FC_Diseño_de_Nervios.Ventana_Principal.Nuevo_Proyecto
{
    partial class F_EditarPisos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_EditarPisos));
            this.P_2 = new System.Windows.Forms.Panel();
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_Title = new FontAwesome.Sharp.IconButton();
            this.DGV_1 = new System.Windows.Forms.DataGridView();
            this.C_NPiso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_NombreRealPiso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_Nivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BT_1 = new System.Windows.Forms.Button();
            this.BT_2 = new System.Windows.Forms.Button();
            this.P_2.SuspendLayout();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).BeginInit();
            this.SuspendLayout();
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.LightGray;
            this.P_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_2.Controls.Add(this.P_1);
            this.P_2.Controls.Add(this.DGV_1);
            this.P_2.Controls.Add(this.BT_1);
            this.P_2.Controls.Add(this.BT_2);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(364, 281);
            this.P_2.TabIndex = 8;
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.LB_Title);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(362, 31);
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
            this.LB_Title.IconChar = FontAwesome.Sharp.IconChar.ThList;
            this.LB_Title.IconColor = System.Drawing.Color.White;
            this.LB_Title.IconSize = 22;
            this.LB_Title.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_Title.Location = new System.Drawing.Point(3, 1);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Rotation = 0D;
            this.LB_Title.Size = new System.Drawing.Size(159, 27);
            this.LB_Title.TabIndex = 3;
            this.LB_Title.Text = "     Propiedades del Piso";
            this.LB_Title.UseVisualStyleBackColor = true;
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // DGV_1
            // 
            this.DGV_1.AllowUserToAddRows = false;
            this.DGV_1.AllowUserToDeleteRows = false;
            this.DGV_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C_NPiso,
            this.C_NombreRealPiso,
            this.C_Nivel});
            this.DGV_1.Location = new System.Drawing.Point(11, 37);
            this.DGV_1.Name = "DGV_1";
            this.DGV_1.RowHeadersVisible = false;
            this.DGV_1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_1.Size = new System.Drawing.Size(340, 204);
            this.DGV_1.TabIndex = 2;
            // 
            // C_NPiso
            // 
            this.C_NPiso.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_NPiso.HeaderText = "Nombre";
            this.C_NPiso.Name = "C_NPiso";
            this.C_NPiso.ReadOnly = true;
            this.C_NPiso.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // C_NombreRealPiso
            // 
            this.C_NombreRealPiso.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_NombreRealPiso.HeaderText = "Nombre Real";
            this.C_NombreRealPiso.Name = "C_NombreRealPiso";
            this.C_NombreRealPiso.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // C_Nivel
            // 
            this.C_Nivel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_Nivel.HeaderText = "Nivel ";
            this.C_Nivel.Name = "C_Nivel";
            this.C_Nivel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BT_1
            // 
            this.BT_1.Location = new System.Drawing.Point(276, 247);
            this.BT_1.Name = "BT_1";
            this.BT_1.Size = new System.Drawing.Size(75, 23);
            this.BT_1.TabIndex = 3;
            this.BT_1.Text = "Aceptar";
            this.BT_1.UseVisualStyleBackColor = true;
            this.BT_1.Click += new System.EventHandler(this.BT_1_Click);
            // 
            // BT_2
            // 
            this.BT_2.Location = new System.Drawing.Point(195, 247);
            this.BT_2.Name = "BT_2";
            this.BT_2.Size = new System.Drawing.Size(75, 23);
            this.BT_2.TabIndex = 4;
            this.BT_2.Text = "Cancelar";
            this.BT_2.UseVisualStyleBackColor = true;
            this.BT_2.Click += new System.EventHandler(this.BT_2_Click);
            // 
            // F_EditarPisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 281);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_EditarPisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editar Pisos";
            this.Load += new System.EventHandler(this.F_EditarPisos_Load);
            this.P_2.ResumeLayout(false);
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.DataGridView DGV_1;
        private System.Windows.Forms.Button BT_1;
        private System.Windows.Forms.Button BT_2;
        private System.Windows.Forms.Panel P_1;
        private FontAwesome.Sharp.IconButton LB_Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_NPiso;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_NombreRealPiso;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_Nivel;
    }
}