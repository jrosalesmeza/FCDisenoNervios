namespace FC_Diseño_de_Nervios
{
    partial class F_Pisos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Pisos));
            this.LV_Pisos = new System.Windows.Forms.ListView();
            this.P_Title = new System.Windows.Forms.Panel();
            this.PB_Nuevo = new System.Windows.Forms.PictureBox();
            this.LB_Title = new System.Windows.Forms.Label();
            this.BT_Aceptar = new System.Windows.Forms.Button();
            this.P_Contenedor = new System.Windows.Forms.Panel();
            this.P_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nuevo)).BeginInit();
            this.SuspendLayout();
            // 
            // LV_Pisos
            // 
            this.LV_Pisos.HideSelection = false;
            this.LV_Pisos.Location = new System.Drawing.Point(10, 36);
            this.LV_Pisos.Name = "LV_Pisos";
            this.LV_Pisos.Size = new System.Drawing.Size(181, 299);
            this.LV_Pisos.TabIndex = 0;
            this.LV_Pisos.UseCompatibleStateImageBehavior = false;
            this.LV_Pisos.View = System.Windows.Forms.View.List;
            this.LV_Pisos.SelectedIndexChanged += new System.EventHandler(this.LV_Pisos_SelectedIndexChanged);
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
            this.P_Title.Size = new System.Drawing.Size(203, 30);
            this.P_Title.TabIndex = 1;
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
            this.LB_Title.Size = new System.Drawing.Size(96, 14);
            this.LB_Title.TabIndex = 0;
            this.LB_Title.Text = "Selección de Pisos";
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // BT_Aceptar
            // 
            this.BT_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Aceptar.Location = new System.Drawing.Point(116, 341);
            this.BT_Aceptar.Name = "BT_Aceptar";
            this.BT_Aceptar.Size = new System.Drawing.Size(75, 23);
            this.BT_Aceptar.TabIndex = 2;
            this.BT_Aceptar.Text = "Aceptar";
            this.BT_Aceptar.UseVisualStyleBackColor = true;
            this.BT_Aceptar.Click += new System.EventHandler(this.BT_Aceptar_Click);
            // 
            // P_Contenedor
            // 
            this.P_Contenedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_Contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_Contenedor.Location = new System.Drawing.Point(0, 0);
            this.P_Contenedor.Name = "P_Contenedor";
            this.P_Contenedor.Size = new System.Drawing.Size(203, 371);
            this.P_Contenedor.TabIndex = 3;
            // 
            // F_Pisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(203, 371);
            this.Controls.Add(this.BT_Aceptar);
            this.Controls.Add(this.P_Title);
            this.Controls.Add(this.LV_Pisos);
            this.Controls.Add(this.P_Contenedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_Pisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Pisos";
            this.P_Title.ResumeLayout(false);
            this.P_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nuevo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView LV_Pisos;
        private System.Windows.Forms.Panel P_Title;
        private System.Windows.Forms.PictureBox PB_Nuevo;
        private System.Windows.Forms.Label LB_Title;
        private System.Windows.Forms.Button BT_Aceptar;
        private System.Windows.Forms.Panel P_Contenedor;
    }
}