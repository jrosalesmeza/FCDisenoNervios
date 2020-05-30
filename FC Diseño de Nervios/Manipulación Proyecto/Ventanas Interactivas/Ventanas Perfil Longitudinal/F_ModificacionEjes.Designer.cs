namespace FC_Diseño_de_Nervios
{
    partial class F_ModificacionEjes
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
            this.components = new System.ComponentModel.Container();
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_ModEjes = new FontAwesome.Sharp.IconButton();
            this.DGV_1 = new System.Windows.Forms.DataGridView();
            this.C_NombreEje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C_Localizacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CTMS_1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BT_1 = new System.Windows.Forms.Button();
            this.BT_2 = new System.Windows.Forms.Button();
            this.TB_BurbujaSize = new System.Windows.Forms.TextBox();
            this.LB_TamBur = new FontAwesome.Sharp.IconButton();
            this.LB_2 = new System.Windows.Forms.Label();
            this.P_2 = new System.Windows.Forms.Panel();
            this.BT_Agregar = new FontAwesome.Sharp.IconButton();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).BeginInit();
            this.CTMS_1.SuspendLayout();
            this.P_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.LB_ModEjes);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(264, 31);
            this.P_1.TabIndex = 0;
            this.P_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // LB_ModEjes
            // 
            this.LB_ModEjes.FlatAppearance.BorderSize = 0;
            this.LB_ModEjes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_ModEjes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_ModEjes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_ModEjes.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_ModEjes.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_ModEjes.ForeColor = System.Drawing.Color.White;
            this.LB_ModEjes.IconChar = FontAwesome.Sharp.IconChar.Columns;
            this.LB_ModEjes.IconColor = System.Drawing.Color.White;
            this.LB_ModEjes.IconSize = 22;
            this.LB_ModEjes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_ModEjes.Location = new System.Drawing.Point(3, 1);
            this.LB_ModEjes.Name = "LB_ModEjes";
            this.LB_ModEjes.Rotation = 0D;
            this.LB_ModEjes.Size = new System.Drawing.Size(159, 27);
            this.LB_ModEjes.TabIndex = 3;
            this.LB_ModEjes.Text = "     Modificación de ejes";
            this.LB_ModEjes.UseVisualStyleBackColor = true;
            this.LB_ModEjes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // DGV_1
            // 
            this.DGV_1.AllowUserToAddRows = false;
            this.DGV_1.AllowUserToDeleteRows = false;
            this.DGV_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C_NombreEje,
            this.C_Localizacion});
            this.DGV_1.ContextMenuStrip = this.CTMS_1;
            this.DGV_1.Location = new System.Drawing.Point(13, 53);
            this.DGV_1.Name = "DGV_1";
            this.DGV_1.RowHeadersVisible = false;
            this.DGV_1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_1.Size = new System.Drawing.Size(236, 184);
            this.DGV_1.TabIndex = 2;
            this.DGV_1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_1_CellEndEdit);
            // 
            // C_NombreEje
            // 
            this.C_NombreEje.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_NombreEje.HeaderText = "Nombre Eje";
            this.C_NombreEje.Name = "C_NombreEje";
            this.C_NombreEje.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // C_Localizacion
            // 
            this.C_Localizacion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.C_Localizacion.HeaderText = "Localización (m)";
            this.C_Localizacion.Name = "C_Localizacion";
            this.C_Localizacion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CTMS_1
            // 
            this.CTMS_1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarToolStripMenuItem});
            this.CTMS_1.Name = "CTMS_1";
            this.CTMS_1.Size = new System.Drawing.Size(141, 26);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.eliminarToolStripMenuItem.Text = "Eliminar Ejes";
            this.eliminarToolStripMenuItem.Click += new System.EventHandler(this.eliminarToolStripMenuItem_Click);
            // 
            // BT_1
            // 
            this.BT_1.Location = new System.Drawing.Point(167, 282);
            this.BT_1.Name = "BT_1";
            this.BT_1.Size = new System.Drawing.Size(75, 23);
            this.BT_1.TabIndex = 3;
            this.BT_1.Text = "Aceptar";
            this.BT_1.UseVisualStyleBackColor = true;
            this.BT_1.Click += new System.EventHandler(this.BT_1_Click);
            // 
            // BT_2
            // 
            this.BT_2.Location = new System.Drawing.Point(86, 282);
            this.BT_2.Name = "BT_2";
            this.BT_2.Size = new System.Drawing.Size(75, 23);
            this.BT_2.TabIndex = 4;
            this.BT_2.Text = "Cancelar";
            this.BT_2.UseVisualStyleBackColor = true;
            this.BT_2.Click += new System.EventHandler(this.BT_2_Click);
            // 
            // TB_BurbujaSize
            // 
            this.TB_BurbujaSize.Location = new System.Drawing.Point(181, 248);
            this.TB_BurbujaSize.Name = "TB_BurbujaSize";
            this.TB_BurbujaSize.Size = new System.Drawing.Size(45, 20);
            this.TB_BurbujaSize.TabIndex = 5;
            // 
            // LB_TamBur
            // 
            this.LB_TamBur.FlatAppearance.BorderSize = 0;
            this.LB_TamBur.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_TamBur.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_TamBur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_TamBur.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_TamBur.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_TamBur.IconChar = FontAwesome.Sharp.IconChar.CircleNotch;
            this.LB_TamBur.IconColor = System.Drawing.Color.Black;
            this.LB_TamBur.IconSize = 18;
            this.LB_TamBur.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_TamBur.Location = new System.Drawing.Point(70, 247);
            this.LB_TamBur.Name = "LB_TamBur";
            this.LB_TamBur.Rotation = 0D;
            this.LB_TamBur.Size = new System.Drawing.Size(114, 23);
            this.LB_TamBur.TabIndex = 4;
            this.LB_TamBur.Text = "     Tamaño Burbuja";
            this.LB_TamBur.UseVisualStyleBackColor = true;
            // 
            // LB_2
            // 
            this.LB_2.AutoSize = true;
            this.LB_2.Location = new System.Drawing.Point(232, 251);
            this.LB_2.Name = "LB_2";
            this.LB_2.Size = new System.Drawing.Size(15, 13);
            this.LB_2.TabIndex = 6;
            this.LB_2.Text = "m";
            // 
            // P_2
            // 
            this.P_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_2.Controls.Add(this.BT_Agregar);
            this.P_2.Controls.Add(this.DGV_1);
            this.P_2.Controls.Add(this.TB_BurbujaSize);
            this.P_2.Controls.Add(this.BT_1);
            this.P_2.Controls.Add(this.LB_2);
            this.P_2.Controls.Add(this.BT_2);
            this.P_2.Controls.Add(this.LB_TamBur);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(264, 318);
            this.P_2.TabIndex = 7;
            // 
            // BT_Agregar
            // 
            this.BT_Agregar.BackColor = System.Drawing.Color.Transparent;
            this.BT_Agregar.FlatAppearance.BorderSize = 0;
            this.BT_Agregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Agregar.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.BT_Agregar.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.BT_Agregar.IconColor = System.Drawing.Color.Black;
            this.BT_Agregar.IconSize = 20;
            this.BT_Agregar.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BT_Agregar.Location = new System.Drawing.Point(225, 31);
            this.BT_Agregar.Name = "BT_Agregar";
            this.BT_Agregar.Rotation = 0D;
            this.BT_Agregar.Size = new System.Drawing.Size(28, 21);
            this.BT_Agregar.TabIndex = 8;
            this.BT_Agregar.UseVisualStyleBackColor = false;
            this.BT_Agregar.Click += new System.EventHandler(this.BT_Agregar_Click);
            // 
            // F_ModificacionEjes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(264, 318);
            this.Controls.Add(this.P_1);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(264, 318);
            this.MinimumSize = new System.Drawing.Size(264, 318);
            this.Name = "F_ModificacionEjes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ModificacionEjes";
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).EndInit();
            this.CTMS_1.ResumeLayout(false);
            this.P_2.ResumeLayout(false);
            this.P_2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.DataGridView DGV_1;
        private FontAwesome.Sharp.IconButton LB_ModEjes;
        private System.Windows.Forms.Button BT_1;
        private System.Windows.Forms.Button BT_2;
        private System.Windows.Forms.TextBox TB_BurbujaSize;
        private FontAwesome.Sharp.IconButton LB_TamBur;
        private System.Windows.Forms.Label LB_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_NombreEje;
        private System.Windows.Forms.DataGridViewTextBoxColumn C_Localizacion;
        private System.Windows.Forms.Panel P_2;
        private FontAwesome.Sharp.IconButton BT_Agregar;
        private System.Windows.Forms.ContextMenuStrip CTMS_1;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem;
    }
}