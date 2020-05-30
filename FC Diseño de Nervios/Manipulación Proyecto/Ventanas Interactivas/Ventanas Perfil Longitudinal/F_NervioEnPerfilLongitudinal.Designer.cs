namespace FC_Diseño_de_Nervios
{
    partial class F_NervioEnPerfilLongitudinal
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
            this.PB_VistaPerfilLongitudinal = new System.Windows.Forms.PictureBox();
            this.CTMS_1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MostrarReglaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarEjesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarApoyosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TLSM_ApoyoInicio = new System.Windows.Forms.ToolStripMenuItem();
            this.TLSM_ApoyoFinal = new System.Windows.Forms.ToolStripMenuItem();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinal)).BeginInit();
            this.CTMS_1.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.PB_VistaPerfilLongitudinal);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(1094, 173);
            this.P_1.TabIndex = 0;
            // 
            // PB_VistaPerfilLongitudinal
            // 
            this.PB_VistaPerfilLongitudinal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_VistaPerfilLongitudinal.BackColor = System.Drawing.Color.White;
            this.PB_VistaPerfilLongitudinal.ContextMenuStrip = this.CTMS_1;
            this.PB_VistaPerfilLongitudinal.Location = new System.Drawing.Point(3, 3);
            this.PB_VistaPerfilLongitudinal.Name = "PB_VistaPerfilLongitudinal";
            this.PB_VistaPerfilLongitudinal.Size = new System.Drawing.Size(1088, 167);
            this.PB_VistaPerfilLongitudinal.TabIndex = 0;
            this.PB_VistaPerfilLongitudinal.TabStop = false;
            this.PB_VistaPerfilLongitudinal.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_VistaPerfilLongitudinal_Paint);
            this.PB_VistaPerfilLongitudinal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_VistaPerfilLongitudinal_MouseDown);
            // 
            // CTMS_1
            // 
            this.CTMS_1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MostrarReglaToolStripMenuItem,
            this.editarEjesToolStripMenuItem,
            this.agregarApoyosToolStripMenuItem});
            this.CTMS_1.Name = "CTMS_1";
            this.CTMS_1.Size = new System.Drawing.Size(160, 70);
            // 
            // MostrarReglaToolStripMenuItem
            // 
            this.MostrarReglaToolStripMenuItem.Checked = true;
            this.MostrarReglaToolStripMenuItem.CheckOnClick = true;
            this.MostrarReglaToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MostrarReglaToolStripMenuItem.Name = "MostrarReglaToolStripMenuItem";
            this.MostrarReglaToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.MostrarReglaToolStripMenuItem.Text = "Mostrar Regla";
            this.MostrarReglaToolStripMenuItem.CheckedChanged += new System.EventHandler(this.MostrarReglaToolStripMenuItem_CheckedChanged);
            // 
            // editarEjesToolStripMenuItem
            // 
            this.editarEjesToolStripMenuItem.Name = "editarEjesToolStripMenuItem";
            this.editarEjesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.editarEjesToolStripMenuItem.Text = "Editar Ejes";
            this.editarEjesToolStripMenuItem.Click += new System.EventHandler(this.editarEjesToolStripMenuItem_Click);
            // 
            // agregarApoyosToolStripMenuItem
            // 
            this.agregarApoyosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TLSM_ApoyoInicio,
            this.TLSM_ApoyoFinal});
            this.agregarApoyosToolStripMenuItem.Name = "agregarApoyosToolStripMenuItem";
            this.agregarApoyosToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.agregarApoyosToolStripMenuItem.Text = "Agregar Apoyos";
            // 
            // TLSM_ApoyoInicio
            // 
            this.TLSM_ApoyoInicio.Enabled = false;
            this.TLSM_ApoyoInicio.Name = "TLSM_ApoyoInicio";
            this.TLSM_ApoyoInicio.Size = new System.Drawing.Size(103, 22);
            this.TLSM_ApoyoInicio.Text = "Inicio";
            this.TLSM_ApoyoInicio.Click += new System.EventHandler(this.TLSM_ApoyoInicio_Click);
            // 
            // TLSM_ApoyoFinal
            // 
            this.TLSM_ApoyoFinal.Enabled = false;
            this.TLSM_ApoyoFinal.Name = "TLSM_ApoyoFinal";
            this.TLSM_ApoyoFinal.Size = new System.Drawing.Size(103, 22);
            this.TLSM_ApoyoFinal.Text = "Fin";
            this.TLSM_ApoyoFinal.Click += new System.EventHandler(this.TLSM_ApoyoFinal_Click);
            // 
            // F_NervioEnPerfilLongitudinal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1094, 173);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.P_1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_NervioEnPerfilLongitudinal";
            this.Text = "Vista en Perfil Longitudinal | N-1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_NervioEnPerfilLongitudinal_Paint);
            this.Resize += new System.EventHandler(this.F_NervioEnPerfilLongitudinal_Resize);
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinal)).EndInit();
            this.CTMS_1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.PictureBox PB_VistaPerfilLongitudinal;
        private System.Windows.Forms.ContextMenuStrip CTMS_1;
        private System.Windows.Forms.ToolStripMenuItem MostrarReglaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarEjesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarApoyosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TLSM_ApoyoInicio;
        private System.Windows.Forms.ToolStripMenuItem TLSM_ApoyoFinal;
    }
}