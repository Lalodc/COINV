namespace pruebaaccess
{
    partial class Inicio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicio));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertarExistenciasDelMesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pedidosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salidasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.empaqueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entradasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salidasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.agentesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entradasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.salidasToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.inventarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarInventarioTeóricoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventarioTeóricoPorFechaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.operacionesToolStripMenuItem,
            this.inventarioToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(382, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertarExistenciasDelMesToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // insertarExistenciasDelMesToolStripMenuItem
            // 
            this.insertarExistenciasDelMesToolStripMenuItem.Name = "insertarExistenciasDelMesToolStripMenuItem";
            this.insertarExistenciasDelMesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.insertarExistenciasDelMesToolStripMenuItem.Text = "Insertar Existencias del Mes";
            this.insertarExistenciasDelMesToolStripMenuItem.Click += new System.EventHandler(this.insertarExistenciasDelMesToolStripMenuItem_Click);
            // 
            // operacionesToolStripMenuItem
            // 
            this.operacionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pedidosToolStripMenuItem,
            this.empaqueToolStripMenuItem,
            this.agentesToolStripMenuItem});
            this.operacionesToolStripMenuItem.Name = "operacionesToolStripMenuItem";
            this.operacionesToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.operacionesToolStripMenuItem.Text = "Operaciones";
            // 
            // pedidosToolStripMenuItem
            // 
            this.pedidosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salidasToolStripMenuItem});
            this.pedidosToolStripMenuItem.Name = "pedidosToolStripMenuItem";
            this.pedidosToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.pedidosToolStripMenuItem.Text = "Pedidos";
            // 
            // salidasToolStripMenuItem
            // 
            this.salidasToolStripMenuItem.Name = "salidasToolStripMenuItem";
            this.salidasToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.salidasToolStripMenuItem.Text = "Salidas";
            this.salidasToolStripMenuItem.Click += new System.EventHandler(this.salidasToolStripMenuItem_Click);
            // 
            // empaqueToolStripMenuItem
            // 
            this.empaqueToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entradasToolStripMenuItem,
            this.salidasToolStripMenuItem1});
            this.empaqueToolStripMenuItem.Name = "empaqueToolStripMenuItem";
            this.empaqueToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.empaqueToolStripMenuItem.Text = "Empaque";
            // 
            // entradasToolStripMenuItem
            // 
            this.entradasToolStripMenuItem.Name = "entradasToolStripMenuItem";
            this.entradasToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.entradasToolStripMenuItem.Text = "Entradas";
            this.entradasToolStripMenuItem.Click += new System.EventHandler(this.entradasToolStripMenuItem_Click);
            // 
            // salidasToolStripMenuItem1
            // 
            this.salidasToolStripMenuItem1.Name = "salidasToolStripMenuItem1";
            this.salidasToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.salidasToolStripMenuItem1.Text = "Salidas";
            this.salidasToolStripMenuItem1.Click += new System.EventHandler(this.salidasToolStripMenuItem1_Click);
            // 
            // agentesToolStripMenuItem
            // 
            this.agentesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entradasToolStripMenuItem1,
            this.salidasToolStripMenuItem2});
            this.agentesToolStripMenuItem.Name = "agentesToolStripMenuItem";
            this.agentesToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.agentesToolStripMenuItem.Text = "Agentes";
            // 
            // entradasToolStripMenuItem1
            // 
            this.entradasToolStripMenuItem1.Name = "entradasToolStripMenuItem1";
            this.entradasToolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.entradasToolStripMenuItem1.Text = "Entradas";
            this.entradasToolStripMenuItem1.Click += new System.EventHandler(this.entradasToolStripMenuItem1_Click);
            // 
            // salidasToolStripMenuItem2
            // 
            this.salidasToolStripMenuItem2.Name = "salidasToolStripMenuItem2";
            this.salidasToolStripMenuItem2.Size = new System.Drawing.Size(119, 22);
            this.salidasToolStripMenuItem2.Text = "Salidas";
            this.salidasToolStripMenuItem2.Click += new System.EventHandler(this.salidasToolStripMenuItem2_Click);
            // 
            // inventarioToolStripMenuItem
            // 
            this.inventarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mostrarInventarioTeóricoToolStripMenuItem,
            this.inventarioTeóricoPorFechaToolStripMenuItem});
            this.inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            this.inventarioToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.inventarioToolStripMenuItem.Text = "Inventario";
            // 
            // mostrarInventarioTeóricoToolStripMenuItem
            // 
            this.mostrarInventarioTeóricoToolStripMenuItem.Name = "mostrarInventarioTeóricoToolStripMenuItem";
            this.mostrarInventarioTeóricoToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.mostrarInventarioTeóricoToolStripMenuItem.Text = "Mostrar Inventario Teórico";
            this.mostrarInventarioTeóricoToolStripMenuItem.Click += new System.EventHandler(this.mostrarInventarioTeóricoToolStripMenuItem_Click);
            // 
            // inventarioTeóricoPorFechaToolStripMenuItem
            // 
            this.inventarioTeóricoPorFechaToolStripMenuItem.Name = "inventarioTeóricoPorFechaToolStripMenuItem";
            this.inventarioTeóricoPorFechaToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(382, 222);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Inicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Almacén PT";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operacionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pedidosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salidasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostrarInventarioTeóricoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventarioTeóricoPorFechaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem empaqueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entradasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salidasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem agentesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entradasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem salidasToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem insertarExistenciasDelMesToolStripMenuItem;
    }
}