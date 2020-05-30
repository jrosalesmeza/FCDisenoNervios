namespace FC_Diseño_de_Nervios
{
    partial class F_AreasMomentoNervio
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
            this.PB_VistaPerfilLongitudinalAreas = new System.Windows.Forms.PictureBox();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalAreas)).BeginInit();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.PB_VistaPerfilLongitudinalAreas);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(993, 186);
            this.P_1.TabIndex = 2;
            // 
            // PB_VistaPerfilLongitudinalAreas
            // 
            this.PB_VistaPerfilLongitudinalAreas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_VistaPerfilLongitudinalAreas.BackColor = System.Drawing.Color.White;
            this.PB_VistaPerfilLongitudinalAreas.Location = new System.Drawing.Point(3, 3);
            this.PB_VistaPerfilLongitudinalAreas.Name = "PB_VistaPerfilLongitudinalAreas";
            this.PB_VistaPerfilLongitudinalAreas.Size = new System.Drawing.Size(987, 180);
            this.PB_VistaPerfilLongitudinalAreas.TabIndex = 0;
            this.PB_VistaPerfilLongitudinalAreas.TabStop = false;
            this.PB_VistaPerfilLongitudinalAreas.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_VistaPerfilLongitudinalAreas_Paint);
            this.PB_VistaPerfilLongitudinalAreas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_VistaPerfilLongitudinalAreas_MouseMove);
            // 
            // F_AreasMomentoNervio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 186);
            this.Controls.Add(this.P_1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_AreasMomentoNervio";
            this.Text = "Area de Momentos";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_AreasMomentoNervio_Paint);
            this.Resize += new System.EventHandler(this.F_AreasMomentoNervio_Resize);
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalAreas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.PictureBox PB_VistaPerfilLongitudinalAreas;
    }
}