namespace FC_Diseño_de_Nervios.Manipulación_Proyecto
{
    partial class F_CortanteNervio
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
            this.PB_VistaPerfilLongitudinalCortante = new System.Windows.Forms.PictureBox();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalCortante)).BeginInit();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.PB_VistaPerfilLongitudinalCortante);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(899, 169);
            this.P_1.TabIndex = 2;
            // 
            // PB_VistaPerfilLongitudinalCortante
            // 
            this.PB_VistaPerfilLongitudinalCortante.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_VistaPerfilLongitudinalCortante.BackColor = System.Drawing.Color.White;
            this.PB_VistaPerfilLongitudinalCortante.Location = new System.Drawing.Point(3, 3);
            this.PB_VistaPerfilLongitudinalCortante.Name = "PB_VistaPerfilLongitudinalCortante";
            this.PB_VistaPerfilLongitudinalCortante.Size = new System.Drawing.Size(893, 163);
            this.PB_VistaPerfilLongitudinalCortante.TabIndex = 0;
            this.PB_VistaPerfilLongitudinalCortante.TabStop = false;
            this.PB_VistaPerfilLongitudinalCortante.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_VistaPerfilLongitudinalCortante_Paint);
            this.PB_VistaPerfilLongitudinalCortante.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_VistaPerfilLongitudinalCortante_MouseMove);
            // 
            // F_CortanteNervio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 169);
            this.Controls.Add(this.P_1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_CortanteNervio";
            this.Text = "F_CortanteNervio";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_CortanteNervio_Paint);
            this.Resize += new System.EventHandler(this.F_CortanteNervio_Resize);
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalCortante)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.PictureBox PB_VistaPerfilLongitudinalCortante;
    }
}