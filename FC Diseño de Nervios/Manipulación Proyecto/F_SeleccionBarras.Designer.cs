namespace FC_Diseño_de_Nervios.Manipulación_Proyecto
{
    partial class F_SeleccionBarras
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_SeleccionBarras));
            this.P_2 = new System.Windows.Forms.Panel();
            this.LB_Encabezado = new FontAwesome.Sharp.IconButton();
            this.BT_Seleccionar = new System.Windows.Forms.Button();
            this.BT_1 = new System.Windows.Forms.Button();
            this.BT_2 = new System.Windows.Forms.Button();
            this.PB_Image = new System.Windows.Forms.PictureBox();
            this.P_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.Gray;
            this.P_2.Controls.Add(this.LB_Encabezado);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(522, 31);
            this.P_2.TabIndex = 6;
            // 
            // LB_Encabezado
            // 
            this.LB_Encabezado.FlatAppearance.BorderSize = 0;
            this.LB_Encabezado.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_Encabezado.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_Encabezado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_Encabezado.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_Encabezado.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Encabezado.ForeColor = System.Drawing.Color.White;
            this.LB_Encabezado.IconChar = FontAwesome.Sharp.IconChar.CheckSquare;
            this.LB_Encabezado.IconColor = System.Drawing.Color.White;
            this.LB_Encabezado.IconSize = 22;
            this.LB_Encabezado.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_Encabezado.Location = new System.Drawing.Point(3, 1);
            this.LB_Encabezado.Name = "LB_Encabezado";
            this.LB_Encabezado.Rotation = 0D;
            this.LB_Encabezado.Size = new System.Drawing.Size(159, 27);
            this.LB_Encabezado.TabIndex = 3;
            this.LB_Encabezado.Text = "     Selección de Barras";
            this.LB_Encabezado.UseVisualStyleBackColor = true;
            // 
            // BT_Seleccionar
            // 
            this.BT_Seleccionar.Location = new System.Drawing.Point(270, 241);
            this.BT_Seleccionar.Name = "BT_Seleccionar";
            this.BT_Seleccionar.Size = new System.Drawing.Size(79, 23);
            this.BT_Seleccionar.TabIndex = 11;
            this.BT_Seleccionar.Text = "Seleccionar";
            this.BT_Seleccionar.UseVisualStyleBackColor = true;
            // 
            // BT_1
            // 
            this.BT_1.Location = new System.Drawing.Point(435, 241);
            this.BT_1.Name = "BT_1";
            this.BT_1.Size = new System.Drawing.Size(75, 23);
            this.BT_1.TabIndex = 9;
            this.BT_1.Text = "Aceptar";
            this.BT_1.UseVisualStyleBackColor = true;
            // 
            // BT_2
            // 
            this.BT_2.Location = new System.Drawing.Point(355, 241);
            this.BT_2.Name = "BT_2";
            this.BT_2.Size = new System.Drawing.Size(75, 23);
            this.BT_2.TabIndex = 10;
            this.BT_2.Text = "Cancelar";
            this.BT_2.UseVisualStyleBackColor = true;
            // 
            // PB_Image
            // 
            this.PB_Image.Image = ((System.Drawing.Image)(resources.GetObject("PB_Image.Image")));
            this.PB_Image.Location = new System.Drawing.Point(82, 62);
            this.PB_Image.Name = "PB_Image";
            this.PB_Image.Size = new System.Drawing.Size(348, 56);
            this.PB_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_Image.TabIndex = 12;
            this.PB_Image.TabStop = false;
            // 
            // F_SeleccionBarras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 272);
            this.Controls.Add(this.PB_Image);
            this.Controls.Add(this.BT_Seleccionar);
            this.Controls.Add(this.BT_1);
            this.Controls.Add(this.BT_2);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_SeleccionBarras";
            this.Text = "F_SeleccionBarras";
            this.P_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_2;
        private FontAwesome.Sharp.IconButton LB_Encabezado;
        private System.Windows.Forms.Button BT_Seleccionar;
        private System.Windows.Forms.Button BT_1;
        private System.Windows.Forms.Button BT_2;
        private System.Windows.Forms.PictureBox PB_Image;
    }
}