namespace FC_Diseño_de_Nervios
{
    partial class F_ModificarSeccion
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
            this.BT_Cerrar = new System.Windows.Forms.Button();
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_1 = new System.Windows.Forms.Label();
            this.GB_1 = new System.Windows.Forms.GroupBox();
            this.P_2 = new System.Windows.Forms.Panel();
            this.BT_Aplicar = new System.Windows.Forms.Button();
            this.BT_Cancelar = new System.Windows.Forms.Button();
            this.TB_Longitud = new System.Windows.Forms.TextBox();
            this.TB_Ancho = new System.Windows.Forms.TextBox();
            this.TB_Altura = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.P_1.SuspendLayout();
            this.GB_1.SuspendLayout();
            this.P_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BT_Cerrar
            // 
            this.BT_Cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_Cerrar.BackColor = System.Drawing.Color.Gray;
            this.BT_Cerrar.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_Cerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BT_Cerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.BT_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Cerrar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Cerrar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Cerrar.Image = global::FC_Diseño_de_Nervios.Properties.Resources.x16Blanca;
            this.BT_Cerrar.Location = new System.Drawing.Point(200, 3);
            this.BT_Cerrar.Name = "BT_Cerrar";
            this.BT_Cerrar.Size = new System.Drawing.Size(40, 17);
            this.BT_Cerrar.TabIndex = 28;
            this.BT_Cerrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Cerrar.UseVisualStyleBackColor = false;
            this.BT_Cerrar.Click += new System.EventHandler(this.BT_Cerrar_Click);
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.LightGray;
            this.P_1.Controls.Add(this.LB_1);
            this.P_1.Controls.Add(this.BT_Cerrar);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(244, 25);
            this.P_1.TabIndex = 29;
            this.P_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // LB_1
            // 
            this.LB_1.AutoSize = true;
            this.LB_1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LB_1.Location = new System.Drawing.Point(12, 6);
            this.LB_1.Name = "LB_1";
            this.LB_1.Size = new System.Drawing.Size(104, 14);
            this.LB_1.TabIndex = 30;
            this.LB_1.Text = "Modificar Elemento";
            // 
            // GB_1
            // 
            this.GB_1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_1.Controls.Add(this.P_2);
            this.GB_1.ForeColor = System.Drawing.Color.White;
            this.GB_1.Location = new System.Drawing.Point(9, 28);
            this.GB_1.Name = "GB_1";
            this.GB_1.Size = new System.Drawing.Size(226, 161);
            this.GB_1.TabIndex = 30;
            this.GB_1.TabStop = false;
            this.GB_1.Text = "Propiedades de Sección";
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.LightGray;
            this.P_2.Controls.Add(this.BT_Aplicar);
            this.P_2.Controls.Add(this.BT_Cancelar);
            this.P_2.Controls.Add(this.TB_Longitud);
            this.P_2.Controls.Add(this.TB_Ancho);
            this.P_2.Controls.Add(this.TB_Altura);
            this.P_2.Controls.Add(this.label2);
            this.P_2.Controls.Add(this.label7);
            this.P_2.Controls.Add(this.label6);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.ForeColor = System.Drawing.Color.White;
            this.P_2.Location = new System.Drawing.Point(3, 16);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(220, 142);
            this.P_2.TabIndex = 0;
            // 
            // BT_Aplicar
            // 
            this.BT_Aplicar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Aplicar.ForeColor = System.Drawing.Color.Black;
            this.BT_Aplicar.Location = new System.Drawing.Point(157, 111);
            this.BT_Aplicar.Name = "BT_Aplicar";
            this.BT_Aplicar.Size = new System.Drawing.Size(59, 23);
            this.BT_Aplicar.TabIndex = 31;
            this.BT_Aplicar.Text = "Aplicar";
            this.BT_Aplicar.UseVisualStyleBackColor = true;
            // 
            // BT_Cancelar
            // 
            this.BT_Cancelar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Cancelar.ForeColor = System.Drawing.Color.Black;
            this.BT_Cancelar.Location = new System.Drawing.Point(93, 111);
            this.BT_Cancelar.Name = "BT_Cancelar";
            this.BT_Cancelar.Size = new System.Drawing.Size(59, 23);
            this.BT_Cancelar.TabIndex = 31;
            this.BT_Cancelar.Text = "Cancelar";
            this.BT_Cancelar.UseVisualStyleBackColor = true;
            // 
            // TB_Longitud
            // 
            this.TB_Longitud.Location = new System.Drawing.Point(107, 77);
            this.TB_Longitud.Name = "TB_Longitud";
            this.TB_Longitud.Size = new System.Drawing.Size(60, 20);
            this.TB_Longitud.TabIndex = 7;
            // 
            // TB_Ancho
            // 
            this.TB_Ancho.Location = new System.Drawing.Point(107, 50);
            this.TB_Ancho.Name = "TB_Ancho";
            this.TB_Ancho.Size = new System.Drawing.Size(60, 20);
            this.TB_Ancho.TabIndex = 7;
            // 
            // TB_Altura
            // 
            this.TB_Altura.Location = new System.Drawing.Point(107, 24);
            this.TB_Altura.Name = "TB_Altura";
            this.TB_Altura.Size = new System.Drawing.Size(60, 20);
            this.TB_Altura.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(49, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Longitud:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(49, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "Ancho:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(49, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 14);
            this.label6.TabIndex = 4;
            this.label6.Text = "Altura:";
            // 
            // F_ModificarSeccion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(244, 192);
            this.Controls.Add(this.GB_1);
            this.Controls.Add(this.P_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_ModificarSeccion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "F_ModificarSeccion";
            this.P_1.ResumeLayout(false);
            this.P_1.PerformLayout();
            this.GB_1.ResumeLayout(false);
            this.P_2.ResumeLayout(false);
            this.P_2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BT_Cerrar;
        private System.Windows.Forms.Panel P_1;
        private System.Windows.Forms.Label LB_1;
        private System.Windows.Forms.GroupBox GB_1;
        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.Button BT_Aplicar;
        private System.Windows.Forms.Button BT_Cancelar;
        private System.Windows.Forms.TextBox TB_Ancho;
        private System.Windows.Forms.TextBox TB_Altura;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TB_Longitud;
        private System.Windows.Forms.Label label2;
    }
}