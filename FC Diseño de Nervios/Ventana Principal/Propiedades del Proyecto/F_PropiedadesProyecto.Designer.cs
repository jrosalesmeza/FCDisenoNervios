namespace FC_Diseño_de_Nervios
{
    partial class F_PropiedadesProyecto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_PropiedadesProyecto));
            this.P_1 = new System.Windows.Forms.Panel();
            this.LB_Title = new FontAwesome.Sharp.IconButton();
            this.P_2 = new System.Windows.Forms.Panel();
            this.BT_Cancelar = new System.Windows.Forms.Button();
            this.TC_Control = new System.Windows.Forms.TabControl();
            this.TP1_General = new System.Windows.Forms.TabPage();
            this.GB_2 = new System.Windows.Forms.GroupBox();
            this.CKB_CoorPI = new System.Windows.Forms.CheckBox();
            this.GB_1 = new System.Windows.Forms.GroupBox();
            this.CK_Redondear = new System.Windows.Forms.CheckBox();
            this.CKB_CotaInteligente = new System.Windows.Forms.CheckBox();
            this.CKB_LabelsBarras = new System.Windows.Forms.CheckBox();
            this.BT_1 = new System.Windows.Forms.Button();
            this.BT_Agregar = new FontAwesome.Sharp.IconButton();
            this.GB_3 = new System.Windows.Forms.GroupBox();
            this.CKB_AutoGuardado = new System.Windows.Forms.CheckBox();
            this.NUD_Intervalo = new System.Windows.Forms.NumericUpDown();
            this.LB_U = new System.Windows.Forms.Label();
            this.P_1.SuspendLayout();
            this.P_2.SuspendLayout();
            this.TC_Control.SuspendLayout();
            this.TP1_General.SuspendLayout();
            this.GB_2.SuspendLayout();
            this.GB_1.SuspendLayout();
            this.GB_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Intervalo)).BeginInit();
            this.SuspendLayout();
            // 
            // P_1
            // 
            this.P_1.BackColor = System.Drawing.Color.Gray;
            this.P_1.Controls.Add(this.LB_Title);
            this.P_1.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_1.Location = new System.Drawing.Point(0, 0);
            this.P_1.Name = "P_1";
            this.P_1.Size = new System.Drawing.Size(418, 31);
            this.P_1.TabIndex = 8;
            this.P_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // LB_Title
            // 
            this.LB_Title.FlatAppearance.BorderSize = 0;
            this.LB_Title.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_Title.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_Title.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_Title.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_Title.ForeColor = System.Drawing.Color.White;
            this.LB_Title.IconChar = FontAwesome.Sharp.IconChar.Wrench;
            this.LB_Title.IconColor = System.Drawing.Color.White;
            this.LB_Title.IconSize = 22;
            this.LB_Title.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_Title.Location = new System.Drawing.Point(3, 1);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Rotation = 0D;
            this.LB_Title.Size = new System.Drawing.Size(99, 27);
            this.LB_Title.TabIndex = 3;
            this.LB_Title.Text = "     Opciones";
            this.LB_Title.UseVisualStyleBackColor = true;
            this.LB_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.P_1_MouseDown);
            // 
            // P_2
            // 
            this.P_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_2.Controls.Add(this.BT_Cancelar);
            this.P_2.Controls.Add(this.TC_Control);
            this.P_2.Controls.Add(this.BT_1);
            this.P_2.Controls.Add(this.BT_Agregar);
            this.P_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P_2.Location = new System.Drawing.Point(0, 0);
            this.P_2.Name = "P_2";
            this.P_2.Size = new System.Drawing.Size(418, 407);
            this.P_2.TabIndex = 9;
            // 
            // BT_Cancelar
            // 
            this.BT_Cancelar.Location = new System.Drawing.Point(246, 372);
            this.BT_Cancelar.Name = "BT_Cancelar";
            this.BT_Cancelar.Size = new System.Drawing.Size(75, 23);
            this.BT_Cancelar.TabIndex = 10;
            this.BT_Cancelar.Text = "Cancelar";
            this.BT_Cancelar.UseVisualStyleBackColor = true;
            this.BT_Cancelar.Click += new System.EventHandler(this.BT_Cancelar_Click);
            // 
            // TC_Control
            // 
            this.TC_Control.Controls.Add(this.TP1_General);
            this.TC_Control.Location = new System.Drawing.Point(11, 36);
            this.TC_Control.Name = "TC_Control";
            this.TC_Control.SelectedIndex = 0;
            this.TC_Control.Size = new System.Drawing.Size(391, 330);
            this.TC_Control.TabIndex = 9;
            // 
            // TP1_General
            // 
            this.TP1_General.BackColor = System.Drawing.SystemColors.Control;
            this.TP1_General.Controls.Add(this.GB_3);
            this.TP1_General.Controls.Add(this.GB_2);
            this.TP1_General.Controls.Add(this.GB_1);
            this.TP1_General.Location = new System.Drawing.Point(4, 23);
            this.TP1_General.Name = "TP1_General";
            this.TP1_General.Padding = new System.Windows.Forms.Padding(3);
            this.TP1_General.Size = new System.Drawing.Size(383, 303);
            this.TP1_General.TabIndex = 0;
            this.TP1_General.Text = "General";
            // 
            // GB_2
            // 
            this.GB_2.Controls.Add(this.CKB_CoorPI);
            this.GB_2.Location = new System.Drawing.Point(6, 137);
            this.GB_2.Name = "GB_2";
            this.GB_2.Size = new System.Drawing.Size(371, 64);
            this.GB_2.TabIndex = 10;
            this.GB_2.TabStop = false;
            this.GB_2.Text = "Planta de Ejes";
            // 
            // CKB_CoorPI
            // 
            this.CKB_CoorPI.AutoSize = true;
            this.CKB_CoorPI.Location = new System.Drawing.Point(16, 30);
            this.CKB_CoorPI.Name = "CKB_CoorPI";
            this.CKB_CoorPI.Size = new System.Drawing.Size(245, 18);
            this.CKB_CoorPI.TabIndex = 7;
            this.CKB_CoorPI.Text = "Coordenadas de Puntos de Intersección ";
            this.CKB_CoorPI.UseVisualStyleBackColor = true;
            // 
            // GB_1
            // 
            this.GB_1.Controls.Add(this.CK_Redondear);
            this.GB_1.Controls.Add(this.CKB_CotaInteligente);
            this.GB_1.Controls.Add(this.CKB_LabelsBarras);
            this.GB_1.Location = new System.Drawing.Point(6, 6);
            this.GB_1.Name = "GB_1";
            this.GB_1.Size = new System.Drawing.Size(371, 125);
            this.GB_1.TabIndex = 8;
            this.GB_1.TabStop = false;
            this.GB_1.Text = "Propiedades de Barras";
            // 
            // CK_Redondear
            // 
            this.CK_Redondear.AutoSize = true;
            this.CK_Redondear.Location = new System.Drawing.Point(16, 101);
            this.CK_Redondear.Name = "CK_Redondear";
            this.CK_Redondear.Size = new System.Drawing.Size(192, 18);
            this.CK_Redondear.TabIndex = 9;
            this.CK_Redondear.Text = "Redondear a múltiplos de 5cm";
            this.CK_Redondear.UseVisualStyleBackColor = true;
            // 
            // CKB_CotaInteligente
            // 
            this.CKB_CotaInteligente.AutoSize = true;
            this.CKB_CotaInteligente.Location = new System.Drawing.Point(16, 66);
            this.CKB_CotaInteligente.Name = "CKB_CotaInteligente";
            this.CKB_CotaInteligente.Size = new System.Drawing.Size(158, 18);
            this.CKB_CotaInteligente.TabIndex = 8;
            this.CKB_CotaInteligente.Text = "Acotamiento de la Barra";
            this.CKB_CotaInteligente.UseVisualStyleBackColor = true;
            // 
            // CKB_LabelsBarras
            // 
            this.CKB_LabelsBarras.AutoSize = true;
            this.CKB_LabelsBarras.Location = new System.Drawing.Point(16, 30);
            this.CKB_LabelsBarras.Name = "CKB_LabelsBarras";
            this.CKB_LabelsBarras.Size = new System.Drawing.Size(132, 18);
            this.CKB_LabelsBarras.TabIndex = 7;
            this.CKB_LabelsBarras.Text = "Etiquetas de Barras";
            this.CKB_LabelsBarras.UseVisualStyleBackColor = true;
            // 
            // BT_1
            // 
            this.BT_1.Location = new System.Drawing.Point(327, 372);
            this.BT_1.Name = "BT_1";
            this.BT_1.Size = new System.Drawing.Size(75, 23);
            this.BT_1.TabIndex = 3;
            this.BT_1.Text = "Aplicar";
            this.BT_1.UseVisualStyleBackColor = true;
            this.BT_1.Click += new System.EventHandler(this.BT_1_Click);
            // 
            // BT_Agregar
            // 
            this.BT_Agregar.BackColor = System.Drawing.Color.Transparent;
            this.BT_Agregar.FlatAppearance.BorderSize = 0;
            this.BT_Agregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Agregar.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.BT_Agregar.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.BT_Agregar.IconColor = System.Drawing.Color.Black;
            this.BT_Agregar.IconSize = 20;
            this.BT_Agregar.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BT_Agregar.Location = new System.Drawing.Point(225, 31);
            this.BT_Agregar.Name = "BT_Agregar";
            this.BT_Agregar.Rotation = 0D;
            this.BT_Agregar.Size = new System.Drawing.Size(28, 21);
            this.BT_Agregar.TabIndex = 8;
            this.BT_Agregar.UseVisualStyleBackColor = false;
            // 
            // GB_3
            // 
            this.GB_3.Controls.Add(this.LB_U);
            this.GB_3.Controls.Add(this.NUD_Intervalo);
            this.GB_3.Controls.Add(this.CKB_AutoGuardado);
            this.GB_3.Location = new System.Drawing.Point(6, 207);
            this.GB_3.Name = "GB_3";
            this.GB_3.Size = new System.Drawing.Size(371, 64);
            this.GB_3.TabIndex = 11;
            this.GB_3.TabStop = false;
            this.GB_3.Text = "Auto Guardado";
            // 
            // CKB_AutoGuardado
            // 
            this.CKB_AutoGuardado.AutoSize = true;
            this.CKB_AutoGuardado.Location = new System.Drawing.Point(16, 27);
            this.CKB_AutoGuardado.Name = "CKB_AutoGuardado";
            this.CKB_AutoGuardado.Size = new System.Drawing.Size(155, 18);
            this.CKB_AutoGuardado.TabIndex = 7;
            this.CKB_AutoGuardado.Text = "Habilitar / Deshabilitar";
            this.CKB_AutoGuardado.UseVisualStyleBackColor = true;
            this.CKB_AutoGuardado.CheckedChanged += new System.EventHandler(this.CKB_AutoGuardado_CheckedChanged);
            // 
            // NUD_Intervalo
            // 
            this.NUD_Intervalo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NUD_Intervalo.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NUD_Intervalo.Location = new System.Drawing.Point(177, 25);
            this.NUD_Intervalo.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.NUD_Intervalo.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.NUD_Intervalo.Name = "NUD_Intervalo";
            this.NUD_Intervalo.Size = new System.Drawing.Size(39, 22);
            this.NUD_Intervalo.TabIndex = 8;
            this.NUD_Intervalo.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // LB_U
            // 
            this.LB_U.AutoSize = true;
            this.LB_U.Location = new System.Drawing.Point(222, 29);
            this.LB_U.Name = "LB_U";
            this.LB_U.Size = new System.Drawing.Size(28, 14);
            this.LB_U.TabIndex = 9;
            this.LB_U.Text = "Min";
            // 
            // F_PropiedadesProyecto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 407);
            this.Controls.Add(this.P_1);
            this.Controls.Add(this.P_2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_PropiedadesProyecto";
            this.Text = "Opciones";
            this.Load += new System.EventHandler(this.F_PropiedadesProyecto_Load);
            this.P_1.ResumeLayout(false);
            this.P_2.ResumeLayout(false);
            this.TC_Control.ResumeLayout(false);
            this.TP1_General.ResumeLayout(false);
            this.GB_2.ResumeLayout(false);
            this.GB_2.PerformLayout();
            this.GB_1.ResumeLayout(false);
            this.GB_1.PerformLayout();
            this.GB_3.ResumeLayout(false);
            this.GB_3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Intervalo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel P_1;
        private FontAwesome.Sharp.IconButton LB_Title;
        private System.Windows.Forms.Panel P_2;
        private FontAwesome.Sharp.IconButton BT_Agregar;
        private System.Windows.Forms.TabControl TC_Control;
        private System.Windows.Forms.TabPage TP1_General;
        private System.Windows.Forms.CheckBox CKB_LabelsBarras;
        private System.Windows.Forms.Button BT_Cancelar;
        private System.Windows.Forms.GroupBox GB_1;
        private System.Windows.Forms.Button BT_1;
        private System.Windows.Forms.CheckBox CKB_CotaInteligente;
        private System.Windows.Forms.CheckBox CK_Redondear;
        private System.Windows.Forms.GroupBox GB_2;
        private System.Windows.Forms.CheckBox CKB_CoorPI;
        private System.Windows.Forms.GroupBox GB_3;
        private System.Windows.Forms.CheckBox CKB_AutoGuardado;
        private System.Windows.Forms.NumericUpDown NUD_Intervalo;
        private System.Windows.Forms.Label LB_U;
    }
}