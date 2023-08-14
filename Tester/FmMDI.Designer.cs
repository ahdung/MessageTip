namespace AhDung
{
    partial class FmMDI
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNewChild = new System.Windows.Forms.ToolStripButton();
            this.btnNewForm = new System.Windows.Forms.ToolStripButton();
            this.btnTestItem = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShow = new System.Windows.Forms.Button();
            this.txbText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewChild,
            this.btnNewForm,
            this.btnTestItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(856, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNewChild
            // 
            this.btnNewChild.Image = global::AhDung.Properties.Resources.PicDemo;
            this.btnNewChild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewChild.Name = "btnNewChild";
            this.btnNewChild.Size = new System.Drawing.Size(117, 22);
            this.btnNewChild.Text = "New ChildForm";
            this.btnNewChild.Click += new System.EventHandler(this.btnNewChild_Click);
            // 
            // btnNewForm
            // 
            this.btnNewForm.Image = global::AhDung.Properties.Resources.PicDemo;
            this.btnNewForm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewForm.Name = "btnNewForm";
            this.btnNewForm.Size = new System.Drawing.Size(132, 22);
            this.btnNewForm.Text = "New NormalForm";
            this.btnNewForm.Click += new System.EventHandler(this.btnNewForm_Click);
            // 
            // btnTestItem
            // 
            this.btnTestItem.Image = global::AhDung.Properties.Resources.PicDemo;
            this.btnTestItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTestItem.Name = "btnTestItem";
            this.btnTestItem.Size = new System.Drawing.Size(177, 22);
            this.btnTestItem.Text = "MDI Parent ToolStripItem";
            this.btnTestItem.Click += new System.EventHandler(this.btnTestItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.txbText);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(856, 47);
            this.panel1.TabIndex = 1;
            // 
            // btnShow
            // 
            this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShow.Location = new System.Drawing.Point(769, 11);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 1;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // txbText
            // 
            this.txbText.AcceptsReturn = true;
            this.txbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbText.Location = new System.Drawing.Point(49, 13);
            this.txbText.Name = "txbText";
            this.txbText.Size = new System.Drawing.Size(714, 21);
            this.txbText.TabIndex = 0;
            this.txbText.Text = "Try press Enter key";
            this.txbText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbText_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text";
            // 
            // FmMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 654);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.IsMdiContainer = true;
            this.Name = "FmMDI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FmMDI";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNewChild;
        private System.Windows.Forms.ToolStripButton btnNewForm;
        private System.Windows.Forms.ToolStripButton btnTestItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.TextBox txbText;
        private System.Windows.Forms.Label label1;
    }
}