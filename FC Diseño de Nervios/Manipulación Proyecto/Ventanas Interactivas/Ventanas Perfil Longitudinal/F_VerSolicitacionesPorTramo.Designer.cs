namespace FC_Diseño_de_Nervios
{
    partial class F_VerSolicitacionesPorTramo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_VerSolicitacionesPorTramo));
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_TitleTramo = new FontAwesome.Sharp.IconButton();
            this.P_2 = new System.Windows.Forms.Panel();
            this.DGV_1 = new System.Windows.Forms.DataGridView();
            this.BT_Cerrar = new System.Windows.Forms.Button();
            this.P_1.SuspendLayout();
            this.P_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).BeginInit();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.BT_Cerrar);
            this.P_1.Controls.Add(this.LB_TitleTramo);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(783, 31);
            this.P_1.TabIndex = 8;
            this.P_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // LB_TitleTramo
            // 
            this.LB_TitleTramo.BackColor = System.Drawing.Color.Transparent;
            this.LB_TitleTramo.FlatAppearance.BorderSize = 0;
            this.LB_TitleTramo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_TitleTramo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_TitleTramo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_TitleTramo.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_TitleTramo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_TitleTramo.ForeColor = System.Drawing.Color.White;
            this.LB_TitleTramo.IconChar = FontAwesome.Sharp.IconChar.ChartBar;
            this.LB_TitleTramo.IconColor = System.Drawing.Color.White;
            this.LB_TitleTramo.IconSize = 22;
            this.LB_TitleTramo.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_TitleTramo.Location = new System.Drawing.Point(3, 1);
            this.LB_TitleTramo.Name = "LB_TitleTramo";
            this.LB_TitleTramo.Rotation = 0D;
            this.LB_TitleTramo.Size = new System.Drawing.Size(243, 27);
            this.LB_TitleTramo.TabIndex = 3;
            this.LB_TitleTramo.Text = "     Solicitaciones: SubTramo 1 - Tramo 1";
            this.LB_TitleTramo.UseVisualStyleBackColor = false;
            this.LB_TitleTramo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // P_2
            // 
            this.P_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_2.Controls.Add(this.DGV_1);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(783, 307);
            this.P_2.TabIndex = 9;
            // 
            // DGV_1
            // 
            this.DGV_1.AllowUserToAddRows = false;
            this.DGV_1.AllowUserToDeleteRows = false;
            this.DGV_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_1.Location = new System.Drawing.Point(6, 35);
            this.DGV_1.Name = "DGV_1";
            this.DGV_1.RowHeadersVisible = false;
            this.DGV_1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_1.Size = new System.Drawing.Size(769, 265);
            this.DGV_1.TabIndex = 2;
            // 
            // BT_Cerrar
            // 
            this.BT_Cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_Cerrar.BackColor = System.Drawing.Color.Transparent;
            this.BT_Cerrar.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_Cerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BT_Cerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.BT_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Cerrar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Cerrar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Cerrar.Image = global::FC_Diseño_de_Nervios.Properties.Resources.x16Blanca;
            this.BT_Cerrar.Location = new System.Drawing.Point(739, 7);
            this.BT_Cerrar.Name = "BT_Cerrar";
            this.BT_Cerrar.Size = new System.Drawing.Size(40, 17);
            this.BT_Cerrar.TabIndex = 29;
            this.BT_Cerrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Cerrar.UseVisualStyleBackColor = false;
            this.BT_Cerrar.Click += new System.EventHandler(this.BT_Cerrar_Click);
            // 
            // F_VerSolicitacionesPorTramo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(783, 307);
            this.Controls.Add(this.P_1);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_VerSolicitacionesPorTramo";
            this.Text = "Solicitaciones - Tramo 1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.F_VerSolicitacionesPorTramo_KeyPress);
            this.P_1.ResumeLayout(false);
            this.P_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.DataGridView DGV_1;
        private FontAwesome.Sharp.IconButton LB_TitleTramo;
        private System.Windows.Forms.Button BT_Cerrar;
    }
}