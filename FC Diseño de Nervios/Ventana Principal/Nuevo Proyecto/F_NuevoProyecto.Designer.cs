namespace FC_Diseño_de_Nervios
{
    partial class F_NuevoProyecto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_NuevoProyecto));
            this.P_Contenedor = new System.Windows.Forms.Panel();
            this.TB_Cancelar = new System.Windows.Forms.Button();
            this.GB_1 = new System.Windows.Forms.GroupBox();
            this.TB_Ruta1 = new System.Windows.Forms.TextBox();
            this.LB_Fuerzas = new System.Windows.Forms.Label();
            this.BT_CargarE2K = new System.Windows.Forms.Button();
            this.B_CargarCSV = new System.Windows.Forms.Button();
            this.LB_e2k = new System.Windows.Forms.Label();
            this.TB_Ruta2 = new System.Windows.Forms.TextBox();
            this.BT_Aceptar = new System.Windows.Forms.Button();
            this.P_Title = new System.Windows.Forms.Panel();
            this.PB_Nuevo = new System.Windows.Forms.PictureBox();
            this.LB_Title = new System.Windows.Forms.Label();
            this.P_Contenedor.SuspendLayout();
            this.GB_1.SuspendLayout();
            this.P_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nuevo)).BeginInit();
            this.SuspendLayout();
            // 
            // P_Contenedor
            // 
            this.P_Contenedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_Contenedor.Controls.Add(this.TB_Cancelar);
            this.P_Contenedor.Controls.Add(this.GB_1);
            this.P_Contenedor.Controls.Add(this.BT_Aceptar);
            this.P_Contenedor.Controls.Add(this.P_Title);
            this.P_Contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_Contenedor.Location = new System.Drawing.Point(0, 0);
            this.P_Contenedor.Name = "P_Contenedor";
            this.P_Contenedor.Size = new System.Drawing.Size(513, 231);
            this.P_Contenedor.TabIndex = 0;
            // 
            // TB_Cancelar
            // 
            this.TB_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TB_Cancelar.Location = new System.Drawing.Point(332, 195);
            this.TB_Cancelar.Name = "TB_Cancelar";
            this.TB_Cancelar.Size = new System.Drawing.Size(75, 23);
            this.TB_Cancelar.TabIndex = 15;
            this.TB_Cancelar.Text = "Cancelar";
            this.TB_Cancelar.UseVisualStyleBackColor = true;
            this.TB_Cancelar.Click += new System.EventHandler(this.TB_Cancelar_Click);
            // 
            // GB_1
            // 
            this.GB_1.Controls.Add(this.TB_Ruta1);
            this.GB_1.Controls.Add(this.LB_Fuerzas);
            this.GB_1.Controls.Add(this.BT_CargarE2K);
            this.GB_1.Controls.Add(this.B_CargarCSV);
            this.GB_1.Controls.Add(this.LB_e2k);
            this.GB_1.Controls.Add(this.TB_Ruta2);
            this.GB_1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_1.Location = new System.Drawing.Point(14, 46);
            this.GB_1.Name = "GB_1";
            this.GB_1.Size = new System.Drawing.Size(474, 143);
            this.GB_1.TabIndex = 14;
            this.GB_1.TabStop = false;
            this.GB_1.Text = "Archivos de Entrada";
            // 
            // TB_Ruta1
            // 
            this.TB_Ruta1.Enabled = false;
            this.TB_Ruta1.Font = new System.Drawing.Font("Calibri", 9F);
            this.TB_Ruta1.Location = new System.Drawing.Point(36, 46);
            this.TB_Ruta1.Name = "TB_Ruta1";
            this.TB_Ruta1.Size = new System.Drawing.Size(334, 22);
            this.TB_Ruta1.TabIndex = 8;
            // 
            // LB_Fuerzas
            // 
            this.LB_Fuerzas.AutoSize = true;
            this.LB_Fuerzas.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Fuerzas.ForeColor = System.Drawing.Color.Black;
            this.LB_Fuerzas.Location = new System.Drawing.Point(33, 77);
            this.LB_Fuerzas.Name = "LB_Fuerzas";
            this.LB_Fuerzas.Size = new System.Drawing.Size(130, 14);
            this.LB_Fuerzas.TabIndex = 13;
            this.LB_Fuerzas.Text = "Archivo de Fuerzas (.csv)";
            // 
            // BT_CargarE2K
            // 
            this.BT_CargarE2K.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_CargarE2K.Font = new System.Drawing.Font("Calibri", 9F);
            this.BT_CargarE2K.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BT_CargarE2K.Location = new System.Drawing.Point(385, 45);
            this.BT_CargarE2K.Name = "BT_CargarE2K";
            this.BT_CargarE2K.Size = new System.Drawing.Size(64, 23);
            this.BT_CargarE2K.TabIndex = 9;
            this.BT_CargarE2K.Text = "Cargar";
            this.BT_CargarE2K.UseVisualStyleBackColor = true;
            this.BT_CargarE2K.Click += new System.EventHandler(this.BT_CargarE2K_Click);
            // 
            // B_CargarCSV
            // 
            this.B_CargarCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_CargarCSV.Font = new System.Drawing.Font("Calibri", 9F);
            this.B_CargarCSV.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_CargarCSV.Location = new System.Drawing.Point(385, 101);
            this.B_CargarCSV.Name = "B_CargarCSV";
            this.B_CargarCSV.Size = new System.Drawing.Size(64, 23);
            this.B_CargarCSV.TabIndex = 12;
            this.B_CargarCSV.Text = "Cargar";
            this.B_CargarCSV.UseVisualStyleBackColor = true;
            this.B_CargarCSV.Click += new System.EventHandler(this.B_CargarCSV_Click);
            // 
            // LB_e2k
            // 
            this.LB_e2k.AutoSize = true;
            this.LB_e2k.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_e2k.ForeColor = System.Drawing.Color.Black;
            this.LB_e2k.Location = new System.Drawing.Point(33, 25);
            this.LB_e2k.Name = "LB_e2k";
            this.LB_e2k.Size = new System.Drawing.Size(87, 14);
            this.LB_e2k.TabIndex = 10;
            this.LB_e2k.Text = "Archivo e2k, $et";
            // 
            // TB_Ruta2
            // 
            this.TB_Ruta2.Enabled = false;
            this.TB_Ruta2.Font = new System.Drawing.Font("Calibri", 9F);
            this.TB_Ruta2.Location = new System.Drawing.Point(36, 101);
            this.TB_Ruta2.Name = "TB_Ruta2";
            this.TB_Ruta2.Size = new System.Drawing.Size(334, 22);
            this.TB_Ruta2.TabIndex = 11;
            // 
            // BT_Aceptar
            // 
            this.BT_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Aceptar.Location = new System.Drawing.Point(413, 195);
            this.BT_Aceptar.Name = "BT_Aceptar";
            this.BT_Aceptar.Size = new System.Drawing.Size(75, 23);
            this.BT_Aceptar.TabIndex = 1;
            this.BT_Aceptar.Text = "Siguiente";
            this.BT_Aceptar.UseVisualStyleBackColor = true;
            this.BT_Aceptar.Click += new System.EventHandler(this.BT_Aceptar_Click);
            // 
            // P_Title
            // 
            this.P_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(152)))), ((int)(((byte)(41)))));
            this.P_Title.Controls.Add(this.PB_Nuevo);
            this.P_Title.Controls.Add(this.LB_Title);
            this.P_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_Title.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P_Title.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.P_Title.Location = new System.Drawing.Point(0, 0);
            this.P_Title.Name = "P_Title";
            this.P_Title.Size = new System.Drawing.Size(511, 30);
            this.P_Title.TabIndex = 0;
            this.P_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // PB_Nuevo
            // 
            this.PB_Nuevo.Image = ((System.Drawing.Image)(resources.GetObject("PB_Nuevo.Image")));
            this.PB_Nuevo.Location = new System.Drawing.Point(10, 7);
            this.PB_Nuevo.Name = "PB_Nuevo";
            this.PB_Nuevo.Size = new System.Drawing.Size(16, 16);
            this.PB_Nuevo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_Nuevo.TabIndex = 15;
            this.PB_Nuevo.TabStop = false;
            this.PB_Nuevo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // LB_Title
            // 
            this.LB_Title.AutoSize = true;
            this.LB_Title.Location = new System.Drawing.Point(30, 8);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Size = new System.Drawing.Size(85, 14);
            this.LB_Title.TabIndex = 0;
            this.LB_Title.Text = "Nuevo Proyecto";
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // F_NuevoProyecto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(513, 231);
            this.Controls.Add(this.P_Contenedor);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_NuevoProyecto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nuevo Proyecto";
            this.P_Contenedor.ResumeLayout(false);
            this.GB_1.ResumeLayout(false);
            this.GB_1.PerformLayout();
            this.P_Title.ResumeLayout(false);
            this.P_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nuevo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_Contenedor;
        private System.Windows.Forms.Panel P_Title;
        private System.Windows.Forms.Label LB_Title;
        private System.Windows.Forms.Button BT_Aceptar;
        private System.Windows.Forms.Label LB_Fuerzas;
        private System.Windows.Forms.Button B_CargarCSV;
        private System.Windows.Forms.TextBox TB_Ruta2;
        private System.Windows.Forms.Label LB_e2k;
        private System.Windows.Forms.Button BT_CargarE2K;
        private System.Windows.Forms.TextBox TB_Ruta1;
        private System.Windows.Forms.GroupBox GB_1;
        private System.Windows.Forms.PictureBox PB_Nuevo;
        private System.Windows.Forms.Button TB_Cancelar;
    }
}