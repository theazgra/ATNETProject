namespace ExtensibleApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.canvas = new System.Windows.Forms.Panel();
            this.statusBox = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.miClear = new System.Windows.Forms.ToolStripButton();
            this.btnDrawLine = new System.Windows.Forms.ToolStripButton();
            this.miPlugins = new System.Windows.Forms.ToolStripDropDownButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 40);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(972, 474);
            this.canvas.TabIndex = 2;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.SystemColors.MenuBar;
            this.statusBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBox.FormattingEnabled = true;
            this.statusBox.ItemHeight = 16;
            this.statusBox.Location = new System.Drawing.Point(0, 446);
            this.statusBox.Name = "statusBox";
            this.statusBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.statusBox.Size = new System.Drawing.Size(972, 68);
            this.statusBox.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miClear,
            this.btnDrawLine,
            this.miPlugins});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(968, 36);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // miClear
            // 
            this.miClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.miClear.Image = ((System.Drawing.Image)(resources.GetObject("miClear.Image")));
            this.miClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miClear.Name = "miClear";
            this.miClear.Size = new System.Drawing.Size(95, 33);
            this.miClear.Text = "Clear canvas";
            this.miClear.Click += new System.EventHandler(this.miClear_Click);
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.CheckOnClick = true;
            this.btnDrawLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDrawLine.Image = ((System.Drawing.Image)(resources.GetObject("btnDrawLine.Image")));
            this.btnDrawLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(76, 33);
            this.btnDrawLine.Text = "Draw line";
            // 
            // miPlugins
            // 
            this.miPlugins.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.miPlugins.Image = ((System.Drawing.Image)(resources.GetObject("miPlugins.Image")));
            this.miPlugins.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miPlugins.Name = "miPlugins";
            this.miPlugins.Size = new System.Drawing.Size(70, 33);
            this.miPlugins.Text = "Plugins";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(972, 40);
            this.panel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 514);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "DrawingApp";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel canvas;
        private System.Windows.Forms.ListBox statusBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton btnDrawLine;
        private System.Windows.Forms.ToolStripDropDownButton miPlugins;
        private System.Windows.Forms.ToolStripButton miClear;
    }
}

