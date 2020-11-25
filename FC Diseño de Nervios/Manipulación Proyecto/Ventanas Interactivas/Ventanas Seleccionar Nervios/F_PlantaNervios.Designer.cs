namespace FC_Diseño_de_Nervios
{
    partial class F_PlantaNervios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_PlantaNervios));
            this.GB_4 = new System.Windows.Forms.GroupBox();
            this.PB_Nervios = new System.Windows.Forms.PictureBox();
            this.CMS_PlantaNervios = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modificarEjesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reasignarEjesANerviosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GB_4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nervios)).BeginInit();
            this.CMS_PlantaNervios.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_4
            // 
            this.GB_4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_4.Controls.Add(this.PB_Nervios);
            this.GB_4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GB_4.ForeColor = System.Drawing.Color.White;
            this.GB_4.Location = new System.Drawing.Point(3, 1);
            this.GB_4.Name = "GB_4";
            this.GB_4.Size = new System.Drawing.Size(529, 485);
            this.GB_4.TabIndex = 12;
            this.GB_4.TabStop = false;
            this.GB_4.Text = "Nervios";
            // 
            // PB_Nervios
            // 
            this.PB_Nervios.BackColor = System.Drawing.Color.White;
            this.PB_Nervios.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Nervios.ContextMenuStrip = this.CMS_PlantaNervios;
            this.PB_Nervios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_Nervios.Location = new System.Drawing.Point(3, 18);
            this.PB_Nervios.Name = "PB_Nervios";
            this.PB_Nervios.Size = new System.Drawing.Size(523, 464);
            this.PB_Nervios.TabIndex = 4;
            this.PB_Nervios.TabStop = false;
            this.PB_Nervios.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_Nervios_Paint);
            this.PB_Nervios.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PB_Nervios_MouseDown_SelectNervio);
            // 
            // CMS_PlantaNervios
            // 
            this.CMS_PlantaNervios.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modificarEjesToolStripMenuItem,
            this.reasignarEjesANerviosToolStripMenuItem});
            this.CMS_PlantaNervios.Name = "CMS_PlantaNervios";
            this.CMS_PlantaNervios.Size = new System.Drawing.Size(201, 48);
            // 
            // modificarEjesToolStripMenuItem
            // 
            this.modificarEjesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("modificarEjesToolStripMenuItem.Image")));
            this.modificarEjesToolStripMenuItem.Name = "modificarEjesToolStripMenuItem";
            this.modificarEjesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.modificarEjesToolStripMenuItem.Text = "Modificar Ejes";
            this.modificarEjesToolStripMenuItem.Click += new System.EventHandler(this.modificarEjesToolStripMenuItem_Click);
            // 
            // reasignarEjesANerviosToolStripMenuItem
            // 
            this.reasignarEjesANerviosToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reasignarEjesANerviosToolStripMenuItem.Image")));
            this.reasignarEjesANerviosToolStripMenuItem.Name = "reasignarEjesANerviosToolStripMenuItem";
            this.reasignarEjesANerviosToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.reasignarEjesANerviosToolStripMenuItem.Text = "Reasignar Ejes a Nervios";
            this.reasignarEjesANerviosToolStripMenuItem.Click += new System.EventHandler(this.reasignarEjesANerviosToolStripMenuItem_Click);
            // 
            // F_PlantaNervios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(536, 491);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.GB_4);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_PlantaNervios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nervios";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.F_PlantaNervios_Paint);
            this.Resize += new System.EventHandler(this.F_PlantaNervios_Resize);
            this.GB_4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nervios)).EndInit();
            this.CMS_PlantaNervios.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_4;
        private System.Windows.Forms.PictureBox PB_Nervios;
        private System.Windows.Forms.ContextMenuStrip CMS_PlantaNervios;
        private System.Windows.Forms.ToolStripMenuItem modificarEjesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reasignarEjesANerviosToolStripMenuItem;
    }
}