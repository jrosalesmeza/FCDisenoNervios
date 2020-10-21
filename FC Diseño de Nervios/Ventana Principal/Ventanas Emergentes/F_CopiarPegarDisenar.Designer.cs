namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes
{
    partial class F_CopiarPegarDisenar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_CopiarPegarDisenar));
            this.GB_1 = new System.Windows.Forms.GroupBox();
            this.P_2 = new System.Windows.Forms.Panel();
            this.CKB_RLongitudinal = new System.Windows.Forms.CheckBox();
            this.CKB_RTransversal = new System.Windows.Forms.CheckBox();
            this.BT_Aceptar = new System.Windows.Forms.Button();
            this.GB_1.SuspendLayout();
            this.P_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_1
            // 
            this.GB_1.Controls.Add(this.P_2);
            this.GB_1.ForeColor = System.Drawing.Color.White;
            this.GB_1.Location = new System.Drawing.Point(3, 2);
            this.GB_1.Name = "GB_1";
            this.GB_1.Size = new System.Drawing.Size(167, 101);
            this.GB_1.TabIndex = 31;
            this.GB_1.TabStop = false;
            this.GB_1.Text = "Seleccionar Refuerzo";
            // 
            // P_2
            // 
            this.P_2.BackColor = System.Drawing.Color.LightGray;
            this.P_2.Controls.Add(this.BT_Aceptar);
            this.P_2.Controls.Add(this.CKB_RLongitudinal);
            this.P_2.Controls.Add(this.CKB_RTransversal);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.ForeColor = System.Drawing.Color.White;
            this.P_2.Location = new System.Drawing.Point(3, 16);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(161, 82);
            this.P_2.TabIndex = 0;
            // 
            // CKB_RLongitudinal
            // 
            this.CKB_RLongitudinal.AutoSize = true;
            this.CKB_RLongitudinal.ForeColor = System.Drawing.Color.Black;
            this.CKB_RLongitudinal.Location = new System.Drawing.Point(12, 14);
            this.CKB_RLongitudinal.Name = "CKB_RLongitudinal";
            this.CKB_RLongitudinal.Size = new System.Drawing.Size(129, 17);
            this.CKB_RLongitudinal.TabIndex = 14;
            this.CKB_RLongitudinal.Text = "Refuerzo Longitudinal";
            this.CKB_RLongitudinal.UseVisualStyleBackColor = true;
            this.CKB_RLongitudinal.CheckedChanged += new System.EventHandler(this.CKB_RLongitudinal_CheckedChanged);
            // 
            // CKB_RTransversal
            // 
            this.CKB_RTransversal.AutoSize = true;
            this.CKB_RTransversal.ForeColor = System.Drawing.Color.Black;
            this.CKB_RTransversal.Location = new System.Drawing.Point(12, 37);
            this.CKB_RTransversal.Name = "CKB_RTransversal";
            this.CKB_RTransversal.Size = new System.Drawing.Size(127, 17);
            this.CKB_RTransversal.TabIndex = 15;
            this.CKB_RTransversal.Text = "Refuerzo Transversal";
            this.CKB_RTransversal.UseVisualStyleBackColor = true;
            this.CKB_RTransversal.CheckedChanged += new System.EventHandler(this.CKB_RLongitudinal_CheckedChanged);
            // 
            // BT_Aceptar
            // 
            this.BT_Aceptar.ForeColor = System.Drawing.Color.Black;
            this.BT_Aceptar.Location = new System.Drawing.Point(51, 56);
            this.BT_Aceptar.Name = "BT_Aceptar";
            this.BT_Aceptar.Size = new System.Drawing.Size(60, 23);
            this.BT_Aceptar.TabIndex = 16;
            this.BT_Aceptar.Text = "Ok";
            this.BT_Aceptar.UseVisualStyleBackColor = true;
            this.BT_Aceptar.Click += new System.EventHandler(this.BT_Aceptar_Click);
            // 
            // F_CopiarPegarDisenar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(174, 106);
            this.Controls.Add(this.GB_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_CopiarPegarDisenar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Refuerzo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_CopiarPegarDisenar_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.F_CopiarPegarDisenar_KeyPress);
            this.GB_1.ResumeLayout(false);
            this.P_2.ResumeLayout(false);
            this.P_2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_1;
        private System.Windows.Forms.Panel P_2;
        private System.Windows.Forms.CheckBox CKB_RLongitudinal;
        private System.Windows.Forms.CheckBox CKB_RTransversal;
        private System.Windows.Forms.Button BT_Aceptar;
    }
}