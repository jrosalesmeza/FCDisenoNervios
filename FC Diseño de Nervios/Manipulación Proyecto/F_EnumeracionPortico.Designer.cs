namespace FC_Diseño_de_Nervios
{
    partial class F_EnumeracionPortico
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_EnumeracionPortico));
            this.P_Title = new System.Windows.Forms.Panel();
            this.BT_Cerrar = new System.Windows.Forms.Button();
            this.PB_EnumeracionElementos = new System.Windows.Forms.PictureBox();
            this.LB_Title = new System.Windows.Forms.Label();
            this.LV_Stories = new System.Windows.Forms.ListView();
            this.GB_1 = new System.Windows.Forms.GroupBox();
            this.GB_Controles = new System.Windows.Forms.GroupBox();
            this.LB_Nombrar = new System.Windows.Forms.Label();
            this.TB_Nombramiento = new System.Windows.Forms.TextBox();
            this.CKB_SeleccionInteligente = new System.Windows.Forms.CheckBox();
            this.BT_Enumerar = new System.Windows.Forms.Button();
            this.PB_ElementosEnumerados = new System.Windows.Forms.PictureBox();
            this.PB_ElementosNoEnumerados = new System.Windows.Forms.PictureBox();
            this.P_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_EnumeracionElementos)).BeginInit();
            this.GB_1.SuspendLayout();
            this.GB_Controles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ElementosEnumerados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ElementosNoEnumerados)).BeginInit();
            this.SuspendLayout();
            // 
            // P_Title
            // 
            this.P_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(152)))), ((int)(((byte)(41)))));
            this.P_Title.Controls.Add(this.BT_Cerrar);
            this.P_Title.Controls.Add(this.PB_EnumeracionElementos);
            this.P_Title.Controls.Add(this.LB_Title);
            this.P_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_Title.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P_Title.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.P_Title.Location = new System.Drawing.Point(0, 0);
            this.P_Title.Name = "P_Title";
            this.P_Title.Size = new System.Drawing.Size(974, 32);
            this.P_Title.TabIndex = 1;
            this.P_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // BT_Cerrar
            // 
            this.BT_Cerrar.Dock = System.Windows.Forms.DockStyle.Right;
            this.BT_Cerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(152)))), ((int)(((byte)(41)))));
            this.BT_Cerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(173)))), ((int)(((byte)(90)))));
            this.BT_Cerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(123)))), ((int)(((byte)(7)))));
            this.BT_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Cerrar.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Cerrar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BT_Cerrar.Image = global::FC_Diseño_de_Nervios.Properties.Resources.x16Blanca;
            this.BT_Cerrar.Location = new System.Drawing.Point(934, 0);
            this.BT_Cerrar.Name = "BT_Cerrar";
            this.BT_Cerrar.Size = new System.Drawing.Size(40, 32);
            this.BT_Cerrar.TabIndex = 28;
            this.BT_Cerrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Cerrar.UseVisualStyleBackColor = true;
            this.BT_Cerrar.Click += new System.EventHandler(this.BT_Cerrar_Click);
            // 
            // PB_EnumeracionElementos
            // 
            this.PB_EnumeracionElementos.Image = ((System.Drawing.Image)(resources.GetObject("PB_EnumeracionElementos.Image")));
            this.PB_EnumeracionElementos.Location = new System.Drawing.Point(10, 8);
            this.PB_EnumeracionElementos.Name = "PB_EnumeracionElementos";
            this.PB_EnumeracionElementos.Size = new System.Drawing.Size(16, 17);
            this.PB_EnumeracionElementos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_EnumeracionElementos.TabIndex = 15;
            this.PB_EnumeracionElementos.TabStop = false;
            this.PB_EnumeracionElementos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // LB_Title
            // 
            this.LB_Title.AutoSize = true;
            this.LB_Title.Location = new System.Drawing.Point(30, 9);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Size = new System.Drawing.Size(193, 14);
            this.LB_Title.TabIndex = 0;
            this.LB_Title.Text = "Enumeración de Elementos  | Piso XX";
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_Title_MouseDown);
            // 
            // LV_Stories
            // 
            this.LV_Stories.Location = new System.Drawing.Point(6, 15);
            this.LV_Stories.MultiSelect = false;
            this.LV_Stories.Name = "LV_Stories";
            this.LV_Stories.Size = new System.Drawing.Size(128, 247);
            this.LV_Stories.TabIndex = 2;
            this.LV_Stories.UseCompatibleStateImageBehavior = false;
            this.LV_Stories.SelectedIndexChanged += new System.EventHandler(this.LV_Stories_SelectedIndexChanged);
            // 
            // GB_1
            // 
            this.GB_1.Controls.Add(this.GB_Controles);
            this.GB_1.Controls.Add(this.PB_ElementosEnumerados);
            this.GB_1.Controls.Add(this.PB_ElementosNoEnumerados);
            this.GB_1.Controls.Add(this.LV_Stories);
            this.GB_1.Location = new System.Drawing.Point(10, 31);
            this.GB_1.Name = "GB_1";
            this.GB_1.Size = new System.Drawing.Size(957, 475);
            this.GB_1.TabIndex = 3;
            this.GB_1.TabStop = false;
            // 
            // GB_Controles
            // 
            this.GB_Controles.Controls.Add(this.LB_Nombrar);
            this.GB_Controles.Controls.Add(this.TB_Nombramiento);
            this.GB_Controles.Controls.Add(this.CKB_SeleccionInteligente);
            this.GB_Controles.Controls.Add(this.BT_Enumerar);
            this.GB_Controles.Location = new System.Drawing.Point(6, 268);
            this.GB_Controles.Name = "GB_Controles";
            this.GB_Controles.Size = new System.Drawing.Size(128, 201);
            this.GB_Controles.TabIndex = 5;
            this.GB_Controles.TabStop = false;
            this.GB_Controles.Text = "Controles";
            // 
            // LB_Nombrar
            // 
            this.LB_Nombrar.AutoSize = true;
            this.LB_Nombrar.Location = new System.Drawing.Point(7, 66);
            this.LB_Nombrar.Name = "LB_Nombrar";
            this.LB_Nombrar.Size = new System.Drawing.Size(113, 14);
            this.LB_Nombrar.TabIndex = 3;
            this.LB_Nombrar.Text = "Nombrar elemento:";
            // 
            // TB_Nombramiento
            // 
            this.TB_Nombramiento.Location = new System.Drawing.Point(10, 83);
            this.TB_Nombramiento.Name = "TB_Nombramiento";
            this.TB_Nombramiento.Size = new System.Drawing.Size(109, 22);
            this.TB_Nombramiento.TabIndex = 2;
            // 
            // CKB_SeleccionInteligente
            // 
            this.CKB_SeleccionInteligente.AutoSize = true;
            this.CKB_SeleccionInteligente.Location = new System.Drawing.Point(17, 21);
            this.CKB_SeleccionInteligente.Name = "CKB_SeleccionInteligente";
            this.CKB_SeleccionInteligente.Size = new System.Drawing.Size(93, 32);
            this.CKB_SeleccionInteligente.TabIndex = 1;
            this.CKB_SeleccionInteligente.Text = " Selección\r\n  Inteligente";
            this.CKB_SeleccionInteligente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CKB_SeleccionInteligente.UseVisualStyleBackColor = true;
            // 
            // BT_Enumerar
            // 
            this.BT_Enumerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Enumerar.Location = new System.Drawing.Point(6, 167);
            this.BT_Enumerar.Name = "BT_Enumerar";
            this.BT_Enumerar.Size = new System.Drawing.Size(116, 28);
            this.BT_Enumerar.TabIndex = 0;
            this.BT_Enumerar.Text = "Enumerar";
            this.BT_Enumerar.UseVisualStyleBackColor = true;
            // 
            // PB_ElementosEnumerados
            // 
            this.PB_ElementosEnumerados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_ElementosEnumerados.Location = new System.Drawing.Point(546, 15);
            this.PB_ElementosEnumerados.Name = "PB_ElementosEnumerados";
            this.PB_ElementosEnumerados.Size = new System.Drawing.Size(400, 454);
            this.PB_ElementosEnumerados.TabIndex = 4;
            this.PB_ElementosEnumerados.TabStop = false;
            // 
            // PB_ElementosNoEnumerados
            // 
            this.PB_ElementosNoEnumerados.BackColor = System.Drawing.SystemColors.Window;
            this.PB_ElementosNoEnumerados.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_ElementosNoEnumerados.Location = new System.Drawing.Point(140, 15);
            this.PB_ElementosNoEnumerados.Name = "PB_ElementosNoEnumerados";
            this.PB_ElementosNoEnumerados.Size = new System.Drawing.Size(400, 454);
            this.PB_ElementosNoEnumerados.TabIndex = 3;
            this.PB_ElementosNoEnumerados.TabStop = false;
            // 
            // F_EnumeracionPortico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(974, 510);
            this.Controls.Add(this.GB_1);
            this.Controls.Add(this.P_Title);
            this.Font = new System.Drawing.Font("Calibri", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_EnumeracionPortico";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nuevo Proyecto";
            this.Load += new System.EventHandler(this.F_EnumeracionPortico_Load);
            this.P_Title.ResumeLayout(false);
            this.P_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_EnumeracionElementos)).EndInit();
            this.GB_1.ResumeLayout(false);
            this.GB_Controles.ResumeLayout(false);
            this.GB_Controles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ElementosEnumerados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ElementosNoEnumerados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_Title;
        private System.Windows.Forms.PictureBox PB_EnumeracionElementos;
        private System.Windows.Forms.Label LB_Title;
        private System.Windows.Forms.ListView LV_Stories;
        private System.Windows.Forms.GroupBox GB_1;
        private System.Windows.Forms.PictureBox PB_ElementosEnumerados;
        private System.Windows.Forms.PictureBox PB_ElementosNoEnumerados;
        private System.Windows.Forms.GroupBox GB_Controles;
        private System.Windows.Forms.Button BT_Enumerar;
        private System.Windows.Forms.Button BT_Cerrar;
        private System.Windows.Forms.CheckBox CKB_SeleccionInteligente;
        private System.Windows.Forms.Label LB_Nombrar;
        private System.Windows.Forms.TextBox TB_Nombramiento;
    }
}