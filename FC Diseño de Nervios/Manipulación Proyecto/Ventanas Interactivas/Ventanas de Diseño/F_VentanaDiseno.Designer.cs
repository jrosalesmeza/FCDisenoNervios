namespace FC_Diseño_de_Nervios
{
    partial class F_VentanaDiseno
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_VentanaDiseno));
            this.P_1 = new System.Windows.Forms.Panel();
            this.TS_1 = new System.Windows.Forms.ToolStrip();
            this.TSB_CopiarRefuerzo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripSeparator();
            this.TSL_Tendencias = new System.Windows.Forms.ToolStripLabel();
            this.TSL_RSupeiror = new System.Windows.Forms.ToolStripLabel();
            this.TSCB_RSuperior = new System.Windows.Forms.ToolStripComboBox();
            this.TSL_RInferior = new System.Windows.Forms.ToolStripLabel();
            this.TSCB_RInferior = new System.Windows.Forms.ToolStripComboBox();
            this.TSL_PesoLongitudinalText = new System.Windows.Forms.ToolStripLabel();
            this.TSL_PesoLongitudinalValue = new System.Windows.Forms.ToolStripLabel();
            this.TSL_PesoTransversalText = new System.Windows.Forms.ToolStripLabel();
            this.TSL_PesoTranseversalValue = new System.Windows.Forms.ToolStripLabel();
            this.PB_VistaPerfilLongitudinalDiseno = new System.Windows.Forms.PictureBox();
            this.Herramientas1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarBarraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSB_AgregarInferior = new System.Windows.Forms.ToolStripButton();
            this.TSB_AgregarSuperior = new System.Windows.Forms.ToolStripButton();
            this.TSB_Eliminar = new System.Windows.Forms.ToolStripButton();
            this.P_1.SuspendLayout();
            this.TS_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalDiseno)).BeginInit();
            this.Herramientas1.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.TS_1);
            this.P_1.Controls.Add(this.PB_VistaPerfilLongitudinalDiseno);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(1088, 245);
            this.P_1.TabIndex = 3;
            // 
            // TS_1
            // 
            this.TS_1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSB_AgregarInferior,
            this.TSB_AgregarSuperior,
            this.TSB_CopiarRefuerzo,
            this.TSB_Eliminar,
            this.toolStripButton3,
            this.TSL_Tendencias,
            this.TSL_RSupeiror,
            this.TSCB_RSuperior,
            this.TSL_RInferior,
            this.TSCB_RInferior,
            this.TSL_PesoLongitudinalText,
            this.TSL_PesoLongitudinalValue,
            this.TSL_PesoTransversalText,
            this.TSL_PesoTranseversalValue});
            this.TS_1.Location = new System.Drawing.Point(0, 0);
            this.TS_1.Name = "TS_1";
            this.TS_1.Size = new System.Drawing.Size(1088, 25);
            this.TS_1.TabIndex = 2;
            this.TS_1.Text = "toolStrip1";
            // 
            // TSB_CopiarRefuerzo
            // 
            this.TSB_CopiarRefuerzo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_CopiarRefuerzo.Image = ((System.Drawing.Image)(resources.GetObject("TSB_CopiarRefuerzo.Image")));
            this.TSB_CopiarRefuerzo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_CopiarRefuerzo.Name = "TSB_CopiarRefuerzo";
            this.TSB_CopiarRefuerzo.Size = new System.Drawing.Size(23, 22);
            this.TSB_CopiarRefuerzo.Text = "Copiar Refuerzo";
            this.TSB_CopiarRefuerzo.Click += new System.EventHandler(this.TSB_CopiarRefuerzo_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(6, 25);
            // 
            // TSL_Tendencias
            // 
            this.TSL_Tendencias.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSL_Tendencias.Name = "TSL_Tendencias";
            this.TSL_Tendencias.Size = new System.Drawing.Size(74, 22);
            this.TSL_Tendencias.Text = "Tendencias: ";
            // 
            // TSL_RSupeiror
            // 
            this.TSL_RSupeiror.Name = "TSL_RSupeiror";
            this.TSL_RSupeiror.Size = new System.Drawing.Size(106, 22);
            this.TSL_RSupeiror.Text = "Refuerzo Superior: ";
            // 
            // TSCB_RSuperior
            // 
            this.TSCB_RSuperior.AutoSize = false;
            this.TSCB_RSuperior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TSCB_RSuperior.Name = "TSCB_RSuperior";
            this.TSCB_RSuperior.Size = new System.Drawing.Size(35, 23);
            this.TSCB_RSuperior.SelectedIndexChanged += new System.EventHandler(this.TSCB_RSuperior_SelectedIndexChanged);
            // 
            // TSL_RInferior
            // 
            this.TSL_RInferior.Name = "TSL_RInferior";
            this.TSL_RInferior.Size = new System.Drawing.Size(100, 22);
            this.TSL_RInferior.Text = "Refuerzo Inferior: ";
            // 
            // TSCB_RInferior
            // 
            this.TSCB_RInferior.AutoSize = false;
            this.TSCB_RInferior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TSCB_RInferior.Name = "TSCB_RInferior";
            this.TSCB_RInferior.Size = new System.Drawing.Size(35, 23);
            this.TSCB_RInferior.SelectedIndexChanged += new System.EventHandler(this.TSCB_RInferior_SelectedIndexChanged);
            // 
            // TSL_PesoLongitudinalText
            // 
            this.TSL_PesoLongitudinalText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSL_PesoLongitudinalText.Name = "TSL_PesoLongitudinalText";
            this.TSL_PesoLongitudinalText.Size = new System.Drawing.Size(165, 22);
            this.TSL_PesoLongitudinalText.Text = "Peso Refuerzo Longitudinal: ";
            // 
            // TSL_PesoLongitudinalValue
            // 
            this.TSL_PesoLongitudinalValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSL_PesoLongitudinalValue.ForeColor = System.Drawing.Color.DarkBlue;
            this.TSL_PesoLongitudinalValue.Name = "TSL_PesoLongitudinalValue";
            this.TSL_PesoLongitudinalValue.Size = new System.Drawing.Size(55, 22);
            this.TSL_PesoLongitudinalValue.Text = "10.45 kg";
            // 
            // TSL_PesoTransversalText
            // 
            this.TSL_PesoTransversalText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSL_PesoTransversalText.Name = "TSL_PesoTransversalText";
            this.TSL_PesoTransversalText.Size = new System.Drawing.Size(156, 22);
            this.TSL_PesoTransversalText.Text = "Peso Refuerzo Transversal:";
            // 
            // TSL_PesoTranseversalValue
            // 
            this.TSL_PesoTranseversalValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSL_PesoTranseversalValue.ForeColor = System.Drawing.Color.DarkBlue;
            this.TSL_PesoTranseversalValue.Name = "TSL_PesoTranseversalValue";
            this.TSL_PesoTranseversalValue.Size = new System.Drawing.Size(55, 22);
            this.TSL_PesoTranseversalValue.Text = "10.45 kg";
            // 
            // PB_VistaPerfilLongitudinalDiseno
            // 
            this.PB_VistaPerfilLongitudinalDiseno.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_VistaPerfilLongitudinalDiseno.BackColor = System.Drawing.Color.White;
            this.PB_VistaPerfilLongitudinalDiseno.Location = new System.Drawing.Point(3, 27);
            this.PB_VistaPerfilLongitudinalDiseno.Name = "PB_VistaPerfilLongitudinalDiseno";
            this.PB_VistaPerfilLongitudinalDiseno.Size = new System.Drawing.Size(1082, 214);
            this.PB_VistaPerfilLongitudinalDiseno.TabIndex = 0;
            this.PB_VistaPerfilLongitudinalDiseno.TabStop = false;
            this.PB_VistaPerfilLongitudinalDiseno.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_VistaPerfilLongitudinalDiseno_Paint);
            this.PB_VistaPerfilLongitudinalDiseno.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_VistaPerfilLongitudinalDiseno_MouseDown);
            // 
            // Herramientas1
            // 
            this.Herramientas1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarBarraToolStripMenuItem});
            this.Herramientas1.Name = "Herramientas1";
            this.Herramientas1.Size = new System.Drawing.Size(179, 26);
            // 
            // eliminarBarraToolStripMenuItem
            // 
            this.eliminarBarraToolStripMenuItem.Name = "eliminarBarraToolStripMenuItem";
            this.eliminarBarraToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.eliminarBarraToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.eliminarBarraToolStripMenuItem.Text = "Eliminar Barra";
            this.eliminarBarraToolStripMenuItem.Click += new System.EventHandler(this.eliminarBarraToolStripMenuItem_Click_1);
            // 
            // TSB_AgregarInferior
            // 
            this.TSB_AgregarInferior.AutoSize = false;
            this.TSB_AgregarInferior.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_AgregarInferior.Image = ((System.Drawing.Image)(resources.GetObject("TSB_AgregarInferior.Image")));
            this.TSB_AgregarInferior.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.TSB_AgregarInferior.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_AgregarInferior.Name = "TSB_AgregarInferior";
            this.TSB_AgregarInferior.Size = new System.Drawing.Size(25, 22);
            this.TSB_AgregarInferior.Text = "Agregar Refuerzo Inferior";
            this.TSB_AgregarInferior.Click += new System.EventHandler(this.TSB_AgregarInferior_Click);
            // 
            // TSB_AgregarSuperior
            // 
            this.TSB_AgregarSuperior.AutoSize = false;
            this.TSB_AgregarSuperior.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_AgregarSuperior.Image = ((System.Drawing.Image)(resources.GetObject("TSB_AgregarSuperior.Image")));
            this.TSB_AgregarSuperior.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.TSB_AgregarSuperior.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_AgregarSuperior.Name = "TSB_AgregarSuperior";
            this.TSB_AgregarSuperior.Size = new System.Drawing.Size(25, 22);
            this.TSB_AgregarSuperior.Text = "Agregar Refuerzo Superior";
            this.TSB_AgregarSuperior.Click += new System.EventHandler(this.TSB_AgregarSuperior_Click);
            // 
            // TSB_Eliminar
            // 
            this.TSB_Eliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Eliminar.Image = ((System.Drawing.Image)(resources.GetObject("TSB_Eliminar.Image")));
            this.TSB_Eliminar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Eliminar.Name = "TSB_Eliminar";
            this.TSB_Eliminar.Size = new System.Drawing.Size(23, 22);
            this.TSB_Eliminar.Text = "Eliminar Barra";
            this.TSB_Eliminar.ToolTipText = "Eliminar Barra";
            this.TSB_Eliminar.Click += new System.EventHandler(this.TSB_Eliminar_Click);
            // 
            // F_VentanaDiseno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 245);
            this.Controls.Add(this.P_1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_VentanaDiseno";
            this.Text = "F_VentanaDiseno";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_VentanaDiseno_Paint);
            this.Resize += new System.EventHandler(this.F_VentanaDiseno_Resize);
            this.P_1.ResumeLayout(false);
            this.P_1.PerformLayout();
            this.TS_1.ResumeLayout(false);
            this.TS_1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalDiseno)).EndInit();
            this.Herramientas1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.PictureBox PB_VistaPerfilLongitudinalDiseno;
        private System.Windows.Forms.ToolStrip TS_1;
        private System.Windows.Forms.ToolStripLabel TSL_Tendencias;
        private System.Windows.Forms.ToolStripLabel TSL_RSupeiror;
        private System.Windows.Forms.ToolStripComboBox TSCB_RSuperior;
        private System.Windows.Forms.ToolStripLabel TSL_RInferior;
        private System.Windows.Forms.ToolStripComboBox TSCB_RInferior;
        private System.Windows.Forms.ToolStripButton TSB_CopiarRefuerzo;
        private System.Windows.Forms.ToolStripSeparator toolStripButton3;
        private System.Windows.Forms.ContextMenuStrip Herramientas1;
        private System.Windows.Forms.ToolStripMenuItem eliminarBarraToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel TSL_PesoLongitudinalText;
        private System.Windows.Forms.ToolStripLabel TSL_PesoLongitudinalValue;
        private System.Windows.Forms.ToolStripLabel TSL_PesoTransversalText;
        private System.Windows.Forms.ToolStripLabel TSL_PesoTranseversalValue;
        private System.Windows.Forms.ToolStripButton TSB_AgregarInferior;
        private System.Windows.Forms.ToolStripButton TSB_AgregarSuperior;
        private System.Windows.Forms.ToolStripButton TSB_Eliminar;
    }
}