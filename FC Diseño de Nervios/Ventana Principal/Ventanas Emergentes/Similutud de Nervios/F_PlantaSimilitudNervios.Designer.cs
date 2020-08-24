namespace FC_Diseño_de_Nervios.Ventana_Principal.Ventanas_Emergentes.Similutud_de_Nervios
{
    partial class F_PlantaSimilitudNervios
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_PlantaSimilitudNervios));
            this.P_Title = new System.Windows.Forms.Panel();
            this.PB_Icono = new System.Windows.Forms.PictureBox();
            this.BT_Cerrar = new System.Windows.Forms.Button();
            this.LB_Title = new System.Windows.Forms.Label();
            this.P_C = new System.Windows.Forms.Panel();
            this.TLS_1 = new System.Windows.Forms.ToolStrip();
            this.TLSB_LimpiarSeleccion = new System.Windows.Forms.ToolStripButton();
            this.TLSB_Agrupar = new System.Windows.Forms.ToolStripButton();
            this.GB_Leyenda = new System.Windows.Forms.GroupBox();
            this.GB_3 = new System.Windows.Forms.GroupBox();
            this.LB_Nervio = new System.Windows.Forms.Label();
            this.GL_Control1 = new OpenTK.GLControl();
            this.GB_1 = new System.Windows.Forms.GroupBox();
            this.LV_Stories = new System.Windows.Forms.ListView();
            this.CMS_1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TLSMI_Desagrupar = new System.Windows.Forms.ToolStripMenuItem();
            this.LB_1 = new FontAwesome.Sharp.IconButton();
            this.LB_2 = new FontAwesome.Sharp.IconButton();
            this.LB_4 = new FontAwesome.Sharp.IconButton();
            this.LB_3 = new FontAwesome.Sharp.IconButton();
            this.P_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Icono)).BeginInit();
            this.P_C.SuspendLayout();
            this.TLS_1.SuspendLayout();
            this.GB_Leyenda.SuspendLayout();
            this.GB_3.SuspendLayout();
            this.GB_1.SuspendLayout();
            this.CMS_1.SuspendLayout();
            this.SuspendLayout();
            // 
            // P_Title
            // 
            this.P_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(152)))), ((int)(((byte)(41)))));
            this.P_Title.Controls.Add(this.PB_Icono);
            this.P_Title.Controls.Add(this.BT_Cerrar);
            this.P_Title.Controls.Add(this.LB_Title);
            this.P_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.P_Title.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P_Title.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.P_Title.Location = new System.Drawing.Point(0, 0);
            this.P_Title.Name = "P_Title";
            this.P_Title.Size = new System.Drawing.Size(732, 25);
            this.P_Title.TabIndex = 5;
            // 
            // PB_Icono
            // 
            this.PB_Icono.Image = ((System.Drawing.Image)(resources.GetObject("PB_Icono.Image")));
            this.PB_Icono.Location = new System.Drawing.Point(3, 3);
            this.PB_Icono.Name = "PB_Icono";
            this.PB_Icono.Size = new System.Drawing.Size(16, 18);
            this.PB_Icono.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_Icono.TabIndex = 16;
            this.PB_Icono.TabStop = false;
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
            this.BT_Cerrar.Location = new System.Drawing.Point(692, 0);
            this.BT_Cerrar.Name = "BT_Cerrar";
            this.BT_Cerrar.Size = new System.Drawing.Size(40, 25);
            this.BT_Cerrar.TabIndex = 28;
            this.BT_Cerrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BT_Cerrar.UseVisualStyleBackColor = true;
            this.BT_Cerrar.Click += new System.EventHandler(this.BT_Cerrar_Click);
            // 
            // LB_Title
            // 
            this.LB_Title.AutoSize = true;
            this.LB_Title.Location = new System.Drawing.Point(21, 5);
            this.LB_Title.Name = "LB_Title";
            this.LB_Title.Size = new System.Drawing.Size(159, 14);
            this.LB_Title.TabIndex = 0;
            this.LB_Title.Text = "Similitud de Nervios  | Piso XX";
            // 
            // P_C
            // 
            this.P_C.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P_C.Controls.Add(this.TLS_1);
            this.P_C.Controls.Add(this.GB_Leyenda);
            this.P_C.Controls.Add(this.GB_3);
            this.P_C.Controls.Add(this.GB_1);
            this.P_C.Dock = System.Windows.Forms.DockStyle.Fill;
            this.P_C.Location = new System.Drawing.Point(0, 25);
            this.P_C.Name = "P_C";
            this.P_C.Size = new System.Drawing.Size(732, 607);
            this.P_C.TabIndex = 14;
            // 
            // TLS_1
            // 
            this.TLS_1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TLSB_LimpiarSeleccion,
            this.TLSB_Agrupar});
            this.TLS_1.Location = new System.Drawing.Point(0, 0);
            this.TLS_1.Name = "TLS_1";
            this.TLS_1.Size = new System.Drawing.Size(730, 28);
            this.TLS_1.TabIndex = 13;
            this.TLS_1.Text = "TLS";
            // 
            // TLSB_LimpiarSeleccion
            // 
            this.TLSB_LimpiarSeleccion.AutoSize = false;
            this.TLSB_LimpiarSeleccion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TLSB_LimpiarSeleccion.Image = ((System.Drawing.Image)(resources.GetObject("TLSB_LimpiarSeleccion.Image")));
            this.TLSB_LimpiarSeleccion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TLSB_LimpiarSeleccion.Name = "TLSB_LimpiarSeleccion";
            this.TLSB_LimpiarSeleccion.Size = new System.Drawing.Size(23, 25);
            this.TLSB_LimpiarSeleccion.Text = "Limpiar Selección";
            this.TLSB_LimpiarSeleccion.Click += new System.EventHandler(this.TLSB_LimpiarSeleccion_Click);
            // 
            // TLSB_Agrupar
            // 
            this.TLSB_Agrupar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TLSB_Agrupar.Image = ((System.Drawing.Image)(resources.GetObject("TLSB_Agrupar.Image")));
            this.TLSB_Agrupar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TLSB_Agrupar.Name = "TLSB_Agrupar";
            this.TLSB_Agrupar.Size = new System.Drawing.Size(23, 25);
            this.TLSB_Agrupar.Text = "Agrupar Nervios";
            this.TLSB_Agrupar.Click += new System.EventHandler(this.TLSB_Agrupar_Click);
            // 
            // GB_Leyenda
            // 
            this.GB_Leyenda.Controls.Add(this.LB_4);
            this.GB_Leyenda.Controls.Add(this.LB_3);
            this.GB_Leyenda.Controls.Add(this.LB_2);
            this.GB_Leyenda.Controls.Add(this.LB_1);
            this.GB_Leyenda.ForeColor = System.Drawing.Color.DarkRed;
            this.GB_Leyenda.Location = new System.Drawing.Point(11, 306);
            this.GB_Leyenda.Name = "GB_Leyenda";
            this.GB_Leyenda.Size = new System.Drawing.Size(169, 290);
            this.GB_Leyenda.TabIndex = 1;
            this.GB_Leyenda.TabStop = false;
            this.GB_Leyenda.Text = "Leyenda";
            // 
            // GB_3
            // 
            this.GB_3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.GB_3.Controls.Add(this.LB_Nervio);
            this.GB_3.Controls.Add(this.GL_Control1);
            this.GB_3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_3.ForeColor = System.Drawing.Color.DarkRed;
            this.GB_3.Location = new System.Drawing.Point(186, 31);
            this.GB_3.Name = "GB_3";
            this.GB_3.Size = new System.Drawing.Size(538, 571);
            this.GB_3.TabIndex = 12;
            this.GB_3.TabStop = false;
            this.GB_3.Text = "Planta";
            // 
            // LB_Nervio
            // 
            this.LB_Nervio.AutoSize = true;
            this.LB_Nervio.Location = new System.Drawing.Point(14, 573);
            this.LB_Nervio.Name = "LB_Nervio";
            this.LB_Nervio.Size = new System.Drawing.Size(19, 14);
            this.LB_Nervio.TabIndex = 5;
            this.LB_Nervio.Text = "N-";
            // 
            // GL_Control1
            // 
            this.GL_Control1.BackColor = System.Drawing.Color.White;
            this.GL_Control1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GL_Control1.Location = new System.Drawing.Point(5, 16);
            this.GL_Control1.Name = "GL_Control1";
            this.GL_Control1.Size = new System.Drawing.Size(527, 549);
            this.GL_Control1.TabIndex = 6;
            this.GL_Control1.VSync = false;
            this.GL_Control1.Paint += new System.Windows.Forms.PaintEventHandler(this.GL_Control1_Paint);
            this.GL_Control1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GL_Control1_MouseDown);
            // 
            // GB_1
            // 
            this.GB_1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.GB_1.Controls.Add(this.LV_Stories);
            this.GB_1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_1.ForeColor = System.Drawing.Color.DarkRed;
            this.GB_1.Location = new System.Drawing.Point(8, 31);
            this.GB_1.Name = "GB_1";
            this.GB_1.Size = new System.Drawing.Size(172, 275);
            this.GB_1.TabIndex = 11;
            this.GB_1.TabStop = false;
            this.GB_1.Text = "Pisos";
            // 
            // LV_Stories
            // 
            this.LV_Stories.HideSelection = false;
            this.LV_Stories.Location = new System.Drawing.Point(6, 16);
            this.LV_Stories.MultiSelect = false;
            this.LV_Stories.Name = "LV_Stories";
            this.LV_Stories.Size = new System.Drawing.Size(160, 253);
            this.LV_Stories.TabIndex = 3;
            this.LV_Stories.UseCompatibleStateImageBehavior = false;
            this.LV_Stories.View = System.Windows.Forms.View.List;
            this.LV_Stories.SelectedIndexChanged += new System.EventHandler(this.LV_Stories_SelectedIndexChanged);
            // 
            // CMS_1
            // 
            this.CMS_1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TLSMI_Desagrupar});
            this.CMS_1.Name = "CMS_1";
            this.CMS_1.Size = new System.Drawing.Size(135, 26);
            // 
            // TLSMI_Desagrupar
            // 
            this.TLSMI_Desagrupar.Image = ((System.Drawing.Image)(resources.GetObject("TLSMI_Desagrupar.Image")));
            this.TLSMI_Desagrupar.Name = "TLSMI_Desagrupar";
            this.TLSMI_Desagrupar.Size = new System.Drawing.Size(134, 22);
            this.TLSMI_Desagrupar.Text = "Desagrupar";
            this.TLSMI_Desagrupar.Click += new System.EventHandler(this.TLSMI_Desagrupar_Click);
            // 
            // LB_1
            // 
            this.LB_1.FlatAppearance.BorderSize = 0;
            this.LB_1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_1.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_1.ForeColor = System.Drawing.Color.Black;
            this.LB_1.IconChar = FontAwesome.Sharp.IconChar.LongArrowAltRight;
            this.LB_1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.LB_1.IconSize = 25;
            this.LB_1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_1.Location = new System.Drawing.Point(6, 30);
            this.LB_1.Name = "LB_1";
            this.LB_1.Rotation = 0D;
            this.LB_1.Size = new System.Drawing.Size(157, 30);
            this.LB_1.TabIndex = 4;
            this.LB_1.Text = "        Maestro por Geometría";
            this.LB_1.UseVisualStyleBackColor = true;
            // 
            // LB_2
            // 
            this.LB_2.FlatAppearance.BorderSize = 0;
            this.LB_2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_2.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_2.ForeColor = System.Drawing.Color.Black;
            this.LB_2.IconChar = FontAwesome.Sharp.IconChar.LongArrowAltRight;
            this.LB_2.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(149)))), ((int)(((byte)(26)))));
            this.LB_2.IconSize = 25;
            this.LB_2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_2.Location = new System.Drawing.Point(6, 66);
            this.LB_2.Name = "LB_2";
            this.LB_2.Rotation = 0D;
            this.LB_2.Size = new System.Drawing.Size(157, 30);
            this.LB_2.TabIndex = 5;
            this.LB_2.Text = "       Similar por Geometría";
            this.LB_2.UseVisualStyleBackColor = true;
            // 
            // LB_4
            // 
            this.LB_4.FlatAppearance.BorderSize = 0;
            this.LB_4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_4.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_4.ForeColor = System.Drawing.Color.Black;
            this.LB_4.IconChar = FontAwesome.Sharp.IconChar.LongArrowAltRight;
            this.LB_4.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(188)))), ((int)(((byte)(7)))));
            this.LB_4.IconSize = 25;
            this.LB_4.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_4.Location = new System.Drawing.Point(6, 138);
            this.LB_4.Name = "LB_4";
            this.LB_4.Rotation = 0D;
            this.LB_4.Size = new System.Drawing.Size(147, 30);
            this.LB_4.TabIndex = 7;
            this.LB_4.Text = "Similar de Todo";
            this.LB_4.UseVisualStyleBackColor = true;
            // 
            // LB_3
            // 
            this.LB_3.FlatAppearance.BorderSize = 0;
            this.LB_3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LB_3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LB_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LB_3.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.LB_3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_3.ForeColor = System.Drawing.Color.Black;
            this.LB_3.IconChar = FontAwesome.Sharp.IconChar.LongArrowAltRight;
            this.LB_3.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(70)))), ((int)(((byte)(250)))));
            this.LB_3.IconSize = 25;
            this.LB_3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LB_3.Location = new System.Drawing.Point(6, 102);
            this.LB_3.Name = "LB_3";
            this.LB_3.Rotation = 0D;
            this.LB_3.Size = new System.Drawing.Size(157, 30);
            this.LB_3.TabIndex = 6;
            this.LB_3.Text = "Maestro de Todo";
            this.LB_3.UseVisualStyleBackColor = true;
            // 
            // F_PlantaSimilitudNervios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(732, 632);
            this.Controls.Add(this.P_C);
            this.Controls.Add(this.P_Title);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_PlantaSimilitudNervios";
            this.Text = "Planta de Similitud de Nervios";
            this.P_Title.ResumeLayout(false);
            this.P_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Icono)).EndInit();
            this.P_C.ResumeLayout(false);
            this.P_C.PerformLayout();
            this.TLS_1.ResumeLayout(false);
            this.TLS_1.PerformLayout();
            this.GB_Leyenda.ResumeLayout(false);
            this.GB_3.ResumeLayout(false);
            this.GB_3.PerformLayout();
            this.GB_1.ResumeLayout(false);
            this.CMS_1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel P_Title;
        private System.Windows.Forms.Button BT_Cerrar;
        private System.Windows.Forms.Label LB_Title;
        private System.Windows.Forms.PictureBox PB_Icono;
        private System.Windows.Forms.Panel P_C;
        private System.Windows.Forms.GroupBox GB_1;
        private System.Windows.Forms.ListView LV_Stories;
        private System.Windows.Forms.GroupBox GB_3;
        private System.Windows.Forms.Label LB_Nervio;
        private OpenTK.GLControl GL_Control1;
        private System.Windows.Forms.GroupBox GB_Leyenda;
        private System.Windows.Forms.ToolStrip TLS_1;
        private System.Windows.Forms.ToolStripButton TLSB_LimpiarSeleccion;
        private System.Windows.Forms.ToolStripButton TLSB_Agrupar;
        private System.Windows.Forms.ContextMenuStrip CMS_1;
        private System.Windows.Forms.ToolStripMenuItem TLSMI_Desagrupar;
        private FontAwesome.Sharp.IconButton LB_1;
        private FontAwesome.Sharp.IconButton LB_2;
        private FontAwesome.Sharp.IconButton LB_4;
        private FontAwesome.Sharp.IconButton LB_3;
    }
}