namespace FC_Diseño_de_Nervios.Manipulación_Proyecto
{
    partial class F_Prop_Refuerzo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BT_Cerrar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BT_Cerrar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 30);
            this.panel1.TabIndex = 0;
            // 
            // BT_Cerrar
            // 
            this.BT_Cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_Cerrar.BackColor = System.Drawing.Color.DarkGray;
            this.BT_Cerrar.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.BT_Cerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(173)))), ((int)(((byte)(90)))));
            this.BT_Cerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(123)))), ((int)(((byte)(7)))));
            this.BT_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Cerrar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Cerrar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Cerrar.Image = global::FC_Diseño_de_Nervios.Properties.Resources.x16Blanca;
            this.BT_Cerrar.Location = new System.Drawing.Point(757, 3);
            this.BT_Cerrar.Name = "BT_Cerrar";
            this.BT_Cerrar.Size = new System.Drawing.Size(40, 24);
            this.BT_Cerrar.TabIndex = 29;
            this.BT_Cerrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Cerrar.UseVisualStyleBackColor = false;
            this.BT_Cerrar.Click += new System.EventHandler(this.BT_Cerrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 14);
            this.label1.TabIndex = 31;
            this.label1.Text = "Modificar Elemento";
            // 
            // F_Prop_Refuerzo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Prop_Refuerzo";
            this.Text = "F_Prop_Refuerzo";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BT_Cerrar;
        private System.Windows.Forms.Label label1;
    }
}