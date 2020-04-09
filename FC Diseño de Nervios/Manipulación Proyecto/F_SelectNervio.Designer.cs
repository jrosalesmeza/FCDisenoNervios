﻿namespace FC_Diseño_de_Nervios
{
    partial class F_SelectNervio
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
            this.P_1 = new System.Windows.Forms.Panel();
            this.GB_Propiedades = new System.Windows.Forms.GroupBox();
            this.P_4 = new System.Windows.Forms.Panel();
            this.LB_3 = new System.Windows.Forms.Label();
            this.CB_SeccionAncho = new System.Windows.Forms.ComboBox();
            this.LB_6 = new System.Windows.Forms.Label();
            this.LB_7 = new System.Windows.Forms.Label();
            this.CB_SeccionAltura = new System.Windows.Forms.ComboBox();
            this.GB_4 = new System.Windows.Forms.GroupBox();
            this.PB_Nervios = new System.Windows.Forms.PictureBox();
            this.GB_Nervios = new System.Windows.Forms.GroupBox();
            this.LB_4 = new System.Windows.Forms.Label();
            this.CB_Direccion = new System.Windows.Forms.ComboBox();
            this.LV_Nervios = new System.Windows.Forms.ListView();
            this.P_2 = new System.Windows.Forms.Panel();
            this.LB_2 = new System.Windows.Forms.Label();
            this.GB_Pisos = new System.Windows.Forms.GroupBox();
            this.LV_Stories = new System.Windows.Forms.ListView();
            this.P_3 = new System.Windows.Forms.Panel();
            this.LB_1 = new System.Windows.Forms.Label();
            this.P_1.SuspendLayout();
            this.GB_Propiedades.SuspendLayout();
            this.P_4.SuspendLayout();
            this.GB_4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nervios)).BeginInit();
            this.GB_Nervios.SuspendLayout();
            this.P_2.SuspendLayout();
            this.GB_Pisos.SuspendLayout();
            this.P_3.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.GB_Propiedades);
            this.P_1.Controls.Add(this.GB_4);
            this.P_1.Controls.Add(this.GB_Nervios);
            this.P_1.Controls.Add(this.GB_Pisos);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(641, 700);
            this.P_1.TabIndex = 1;
            // 
            // GB_Propiedades
            // 
            this.GB_Propiedades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Propiedades.BackColor = System.Drawing.Color.LightGray;
            this.GB_Propiedades.Controls.Add(this.P_4);
            this.GB_Propiedades.Controls.Add(this.CB_SeccionAncho);
            this.GB_Propiedades.Controls.Add(this.LB_6);
            this.GB_Propiedades.Controls.Add(this.LB_7);
            this.GB_Propiedades.Controls.Add(this.CB_SeccionAltura);
            this.GB_Propiedades.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_Propiedades.ForeColor = System.Drawing.Color.Black;
            this.GB_Propiedades.Location = new System.Drawing.Point(170, 3);
            this.GB_Propiedades.Name = "GB_Propiedades";
            this.GB_Propiedades.Size = new System.Drawing.Size(468, 77);
            this.GB_Propiedades.TabIndex = 10;
            this.GB_Propiedades.TabStop = false;
            this.GB_Propiedades.Text = "N - X | Piso X ";
            // 
            // P_4
            // 
            this.P_4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P_4.BackColor = System.Drawing.Color.Gray;
            this.P_4.Controls.Add(this.LB_3);
            this.P_4.ForeColor = System.Drawing.Color.Transparent;
            this.P_4.Location = new System.Drawing.Point(3, 17);
            this.P_4.Name = "P_4";
            this.P_4.Size = new System.Drawing.Size(462, 21);
            this.P_4.TabIndex = 6;
            // 
            // LB_3
            // 
            this.LB_3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_3.Location = new System.Drawing.Point(2, 3);
            this.LB_3.Name = "LB_3";
            this.LB_3.Size = new System.Drawing.Size(455, 18);
            this.LB_3.TabIndex = 1;
            this.LB_3.Text = "CAMBIO SECCIÓN";
            this.LB_3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CB_SeccionAncho
            // 
            this.CB_SeccionAncho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_SeccionAncho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CB_SeccionAncho.FormattingEnabled = true;
            this.CB_SeccionAncho.Items.AddRange(new object[] {
            "Superior",
            "Central",
            "Inferior"});
            this.CB_SeccionAncho.Location = new System.Drawing.Point(170, 48);
            this.CB_SeccionAncho.Name = "CB_SeccionAncho";
            this.CB_SeccionAncho.Size = new System.Drawing.Size(75, 22);
            this.CB_SeccionAncho.TabIndex = 13;
            this.CB_SeccionAncho.SelectedIndexChanged += new System.EventHandler(this.CB_SeccionAncho_SelectedIndexChanged);
            // 
            // LB_6
            // 
            this.LB_6.AutoSize = true;
            this.LB_6.Location = new System.Drawing.Point(6, 51);
            this.LB_6.Name = "LB_6";
            this.LB_6.Size = new System.Drawing.Size(40, 14);
            this.LB_6.TabIndex = 10;
            this.LB_6.Text = "Altura:";
            // 
            // LB_7
            // 
            this.LB_7.AutoSize = true;
            this.LB_7.Location = new System.Drawing.Point(128, 51);
            this.LB_7.Name = "LB_7";
            this.LB_7.Size = new System.Drawing.Size(40, 14);
            this.LB_7.TabIndex = 11;
            this.LB_7.Text = "Ancho:";
            // 
            // CB_SeccionAltura
            // 
            this.CB_SeccionAltura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_SeccionAltura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CB_SeccionAltura.FormattingEnabled = true;
            this.CB_SeccionAltura.Items.AddRange(new object[] {
            "Superior",
            "Central",
            "Inferior"});
            this.CB_SeccionAltura.Location = new System.Drawing.Point(48, 48);
            this.CB_SeccionAltura.Name = "CB_SeccionAltura";
            this.CB_SeccionAltura.Size = new System.Drawing.Size(75, 22);
            this.CB_SeccionAltura.TabIndex = 12;
            this.CB_SeccionAltura.SelectedIndexChanged += new System.EventHandler(this.CB_SeccionAltura_SelectedIndexChanged);
            // 
            // GB_4
            // 
            this.GB_4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_4.Controls.Add(this.PB_Nervios);
            this.GB_4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_4.ForeColor = System.Drawing.Color.White;
            this.GB_4.Location = new System.Drawing.Point(170, 81);
            this.GB_4.Name = "GB_4";
            this.GB_4.Size = new System.Drawing.Size(468, 609);
            this.GB_4.TabIndex = 11;
            this.GB_4.TabStop = false;
            this.GB_4.Text = "Nervios";
            // 
            // PB_Nervios
            // 
            this.PB_Nervios.BackColor = System.Drawing.Color.White;
            this.PB_Nervios.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Nervios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_Nervios.Location = new System.Drawing.Point(3, 18);
            this.PB_Nervios.Name = "PB_Nervios";
            this.PB_Nervios.Size = new System.Drawing.Size(462, 588);
            this.PB_Nervios.TabIndex = 4;
            this.PB_Nervios.TabStop = false;
            this.PB_Nervios.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_Nervios_Paint);
            this.PB_Nervios.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_Nervios_MouseDown_SelectNervio);
            // 
            // GB_Nervios
            // 
            this.GB_Nervios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GB_Nervios.BackColor = System.Drawing.Color.LightGray;
            this.GB_Nervios.Controls.Add(this.LB_4);
            this.GB_Nervios.Controls.Add(this.CB_Direccion);
            this.GB_Nervios.Controls.Add(this.LV_Nervios);
            this.GB_Nervios.Controls.Add(this.P_2);
            this.GB_Nervios.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_Nervios.ForeColor = System.Drawing.Color.Black;
            this.GB_Nervios.Location = new System.Drawing.Point(3, 175);
            this.GB_Nervios.Name = "GB_Nervios";
            this.GB_Nervios.Size = new System.Drawing.Size(161, 515);
            this.GB_Nervios.TabIndex = 9;
            this.GB_Nervios.TabStop = false;
            // 
            // LB_4
            // 
            this.LB_4.AutoSize = true;
            this.LB_4.Location = new System.Drawing.Point(9, 46);
            this.LB_4.Name = "LB_4";
            this.LB_4.Size = new System.Drawing.Size(53, 14);
            this.LB_4.TabIndex = 7;
            this.LB_4.Text = "Dirección";
            // 
            // CB_Direccion
            // 
            this.CB_Direccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Direccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CB_Direccion.FormattingEnabled = true;
            this.CB_Direccion.Items.AddRange(new object[] {
            "Todos",
            "Horizontal",
            "Vertical",
            "Diagonal"});
            this.CB_Direccion.Location = new System.Drawing.Point(68, 42);
            this.CB_Direccion.Name = "CB_Direccion";
            this.CB_Direccion.Size = new System.Drawing.Size(87, 22);
            this.CB_Direccion.TabIndex = 0;
            this.CB_Direccion.SelectedIndexChanged += new System.EventHandler(this.CB_Direccion_SelectedIndexChanged);
            // 
            // LV_Nervios
            // 
            this.LV_Nervios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV_Nervios.BackColor = System.Drawing.Color.LightGray;
            this.LV_Nervios.HideSelection = false;
            this.LV_Nervios.Location = new System.Drawing.Point(6, 69);
            this.LV_Nervios.MultiSelect = false;
            this.LV_Nervios.Name = "LV_Nervios";
            this.LV_Nervios.Size = new System.Drawing.Size(149, 434);
            this.LV_Nervios.TabIndex = 4;
            this.LV_Nervios.UseCompatibleStateImageBehavior = false;
            this.LV_Nervios.View = System.Windows.Forms.View.SmallIcon;
            this.LV_Nervios.SelectedIndexChanged += new System.EventHandler(this.LV_Nervios_SelectedIndexChanged);
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.Gray;
            this.P_2.Controls.Add(this.LB_2);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_2.ForeColor = System.Drawing.Color.Transparent;
            this.P_2.Location = new System.Drawing.Point(3, 18);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(155, 20);
            this.P_2.TabIndex = 6;
            // 
            // LB_2
            // 
            this.LB_2.Location = new System.Drawing.Point(53, 4);
            this.LB_2.Name = "LB_2";
            this.LB_2.Size = new System.Drawing.Size(46, 17);
            this.LB_2.TabIndex = 1;
            this.LB_2.Text = "NERVIO";
            // 
            // GB_Pisos
            // 
            this.GB_Pisos.BackColor = System.Drawing.Color.LightGray;
            this.GB_Pisos.Controls.Add(this.LV_Stories);
            this.GB_Pisos.Controls.Add(this.P_3);
            this.GB_Pisos.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_Pisos.ForeColor = System.Drawing.Color.White;
            this.GB_Pisos.Location = new System.Drawing.Point(3, 3);
            this.GB_Pisos.Name = "GB_Pisos";
            this.GB_Pisos.Size = new System.Drawing.Size(161, 166);
            this.GB_Pisos.TabIndex = 6;
            this.GB_Pisos.TabStop = false;
            // 
            // LV_Stories
            // 
            this.LV_Stories.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.LV_Stories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV_Stories.BackColor = System.Drawing.Color.LightGray;
            this.LV_Stories.HideSelection = false;
            this.LV_Stories.Location = new System.Drawing.Point(6, 39);
            this.LV_Stories.MultiSelect = false;
            this.LV_Stories.Name = "LV_Stories";
            this.LV_Stories.Size = new System.Drawing.Size(149, 124);
            this.LV_Stories.TabIndex = 3;
            this.LV_Stories.UseCompatibleStateImageBehavior = false;
            this.LV_Stories.View = System.Windows.Forms.View.List;
            this.LV_Stories.SelectedIndexChanged += new System.EventHandler(this.LV_Stories_SelectedIndexChanged);
            // 
            // P_3
            // 
            this.P_3.BackColor = System.Drawing.Color.Gray;
            this.P_3.Controls.Add(this.LB_1);
            this.P_3.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_3.ForeColor = System.Drawing.Color.Transparent;
            this.P_3.Location = new System.Drawing.Point(3, 18);
            this.P_3.Name = "P_3";
            this.P_3.Size = new System.Drawing.Size(155, 20);
            this.P_3.TabIndex = 5;
            // 
            // LB_1
            // 
            this.LB_1.Location = new System.Drawing.Point(62, 4);
            this.LB_1.Name = "LB_1";
            this.LB_1.Size = new System.Drawing.Size(37, 14);
            this.LB_1.TabIndex = 0;
            this.LB_1.Text = "PISOS";
            // 
            // F_SelectNervio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(641, 700);
            this.Controls.Add(this.P_1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)));
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_SelectNervio";
            this.Text = "Seleccionar Nervio";
            this.Load += new System.EventHandler(this.F_SelectNervio_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_SelectNervio_Paint);
            this.Resize += new System.EventHandler(this.F_SelectNervio_Resize);
            this.P_1.ResumeLayout(false);
            this.GB_Propiedades.ResumeLayout(false);
            this.GB_Propiedades.PerformLayout();
            this.P_4.ResumeLayout(false);
            this.GB_4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nervios)).EndInit();
            this.GB_Nervios.ResumeLayout(false);
            this.GB_Nervios.PerformLayout();
            this.P_2.ResumeLayout(false);
            this.GB_Pisos.ResumeLayout(false);
            this.P_3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.GroupBox GB_Pisos;
        private System.Windows.Forms.GroupBox GB_Nervios;
        private System.Windows.Forms.ListView LV_Stories;
        private System.Windows.Forms.ComboBox CB_Direccion;
        private System.Windows.Forms.ListView LV_Nervios;
        private System.Windows.Forms.Panel P_3;
        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.Label LB_2;
        private System.Windows.Forms.Label LB_1;
        private System.Windows.Forms.Label LB_4;
        private System.Windows.Forms.GroupBox GB_4;
        private System.Windows.Forms.PictureBox PB_Nervios;
        private System.Windows.Forms.Panel P_4;
        private System.Windows.Forms.Label LB_3;
        private System.Windows.Forms.GroupBox GB_Propiedades;
        private System.Windows.Forms.ComboBox CB_SeccionAncho;
        private System.Windows.Forms.ComboBox CB_SeccionAltura;
        private System.Windows.Forms.Label LB_7;
        private System.Windows.Forms.Label LB_6;
    }
}