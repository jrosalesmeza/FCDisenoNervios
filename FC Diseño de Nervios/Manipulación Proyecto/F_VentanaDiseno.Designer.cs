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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_VentanaDiseno));
            this.P_1 = new System.Windows.Forms.Panel();
            this.TS_1 = new System.Windows.Forms.ToolStrip();
            this.TSB_Agregar = new FontAwesome.Sharp.IconToolStripButton();
            this.TSB_CopiarRefuerzo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripSeparator();
            this.TSL_Tendencias = new System.Windows.Forms.ToolStripLabel();
            this.TSL_RSupeiror = new System.Windows.Forms.ToolStripLabel();
            this.TSCB_RSuperior = new System.Windows.Forms.ToolStripComboBox();
            this.TSL_RInferior = new System.Windows.Forms.ToolStripLabel();
            this.TSCB_RInferior = new System.Windows.Forms.ToolStripComboBox();
            this.PB_VistaPerfilLongitudinalDiseno = new System.Windows.Forms.PictureBox();
            this.P_1.SuspendLayout();
            this.TS_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalDiseno)).BeginInit();
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
            this.TSB_Agregar,
            this.TSB_CopiarRefuerzo,
            this.toolStripButton3,
            this.TSL_Tendencias,
            this.TSL_RSupeiror,
            this.TSCB_RSuperior,
            this.TSL_RInferior,
            this.TSCB_RInferior});
            this.TS_1.Location = new System.Drawing.Point(0, 0);
            this.TS_1.Name = "TS_1";
            this.TS_1.Size = new System.Drawing.Size(1088, 25);
            this.TS_1.TabIndex = 2;
            this.TS_1.Text = "toolStrip1";
            // 
            // TSB_Agregar
            // 
            this.TSB_Agregar.AutoSize = false;
            this.TSB_Agregar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_Agregar.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.TSB_Agregar.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.TSB_Agregar.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.TSB_Agregar.IconSize = 40;
            this.TSB_Agregar.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.TSB_Agregar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_Agregar.Name = "TSB_Agregar";
            this.TSB_Agregar.Rotation = 0D;
            this.TSB_Agregar.Size = new System.Drawing.Size(25, 22);
            this.TSB_Agregar.Text = "Agregar Nuevo Refuerzo";
            this.TSB_Agregar.Click += new System.EventHandler(this.TSB_Agregar_Click);
            // 
            // TSB_CopiarRefuerzo
            // 
            this.TSB_CopiarRefuerzo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_CopiarRefuerzo.Image = ((System.Drawing.Image)(resources.GetObject("TSB_CopiarRefuerzo.Image")));
            this.TSB_CopiarRefuerzo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSB_CopiarRefuerzo.Name = "TSB_CopiarRefuerzo";
            this.TSB_CopiarRefuerzo.Size = new System.Drawing.Size(23, 22);
            this.TSB_CopiarRefuerzo.Text = "Copiar Refuerzo";
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
            // F_VentanaDiseno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 245);
            this.Controls.Add(this.P_1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_VentanaDiseno";
            this.Text = "F_VentanaDiseno";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_VentanaDiseno_Paint);
            this.Resize += new System.EventHandler(this.F_VentanaDiseno_Resize);
            this.P_1.ResumeLayout(false);
            this.P_1.PerformLayout();
            this.TS_1.ResumeLayout(false);
            this.TS_1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalDiseno)).EndInit();
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
        private FontAwesome.Sharp.IconToolStripButton TSB_Agregar;
        private System.Windows.Forms.ToolStripButton TSB_CopiarRefuerzo;
        private System.Windows.Forms.ToolStripSeparator toolStripButton3;
    }
}