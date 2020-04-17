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
            this.P_1 = new System.Windows.Forms.Panel();
            this.PB_VistaPerfilLongitudinalDiseno = new System.Windows.Forms.PictureBox();
            this.P_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalDiseno)).BeginInit();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.PB_VistaPerfilLongitudinalDiseno);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(1088, 174);
            this.P_1.TabIndex = 3;
            // 
            // PB_VistaPerfilLongitudinalDiseno
            // 
            this.PB_VistaPerfilLongitudinalDiseno.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PB_VistaPerfilLongitudinalDiseno.BackColor = System.Drawing.Color.White;
            this.PB_VistaPerfilLongitudinalDiseno.Location = new System.Drawing.Point(3, 3);
            this.PB_VistaPerfilLongitudinalDiseno.Name = "PB_VistaPerfilLongitudinalDiseno";
            this.PB_VistaPerfilLongitudinalDiseno.Size = new System.Drawing.Size(1082, 167);
            this.PB_VistaPerfilLongitudinalDiseno.TabIndex = 0;
            this.PB_VistaPerfilLongitudinalDiseno.TabStop = false;
            this.PB_VistaPerfilLongitudinalDiseno.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_VistaPerfilLongitudinalDiseno_Paint);
            // 
            // F_VentanaDiseno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 174);
            this.Controls.Add(this.P_1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_VentanaDiseno";
            this.Text = "F_VentanaDiseno";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_VentanaDiseno_Paint);
            this.Resize += new System.EventHandler(this.F_VentanaDiseno_Resize);
            this.P_1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_VistaPerfilLongitudinalDiseno)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.PictureBox PB_VistaPerfilLongitudinalDiseno;
    }
}