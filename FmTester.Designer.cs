namespace AhDung
{
    partial class FmTester
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
            this.txbIcon = new System.Windows.Forms.TextBox();
            this.btnSelectIcon = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txbText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbFloating = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnWarning = new System.Windows.Forms.Button();
            this.btnError = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.txbInput = new System.Windows.Forms.ToolStripTextBox();
            this.btnEnter = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShowInPanel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbIcon
            // 
            this.txbIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbIcon.Location = new System.Drawing.Point(64, 75);
            this.txbIcon.Name = "txbIcon";
            this.txbIcon.Size = new System.Drawing.Size(340, 21);
            this.txbIcon.TabIndex = 1;
            this.txbIcon.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbText_KeyDown);
            // 
            // btnSelectIcon
            // 
            this.btnSelectIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectIcon.Location = new System.Drawing.Point(410, 73);
            this.btnSelectIcon.Name = "btnSelectIcon";
            this.btnSelectIcon.Size = new System.Drawing.Size(36, 23);
            this.btnSelectIcon.TabIndex = 2;
            this.btnSelectIcon.Text = "...";
            this.btnSelectIcon.UseVisualStyleBackColor = true;
            this.btnSelectIcon.Click += new System.EventHandler(this.btnSelectIcon_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Icon";
            // 
            // txbText
            // 
            this.txbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbText.Location = new System.Drawing.Point(64, 48);
            this.txbText.Name = "txbText";
            this.txbText.Size = new System.Drawing.Size(382, 21);
            this.txbText.TabIndex = 0;
            this.txbText.Text = "Try press Enter key";
            this.txbText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbText_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Text";
            // 
            // ckbFloating
            // 
            this.ckbFloating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbFloating.AutoSize = true;
            this.ckbFloating.Checked = true;
            this.ckbFloating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbFloating.Location = new System.Drawing.Point(344, 105);
            this.ckbFloating.Name = "ckbFloating";
            this.ckbFloating.Size = new System.Drawing.Size(102, 16);
            this.ckbFloating.TabIndex = 4;
            this.ckbFloating.Text = "AllowFloating";
            this.ckbFloating.UseVisualStyleBackColor = true;
            this.ckbFloating.CheckedChanged += new System.EventHandler(this.ckbFloating_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Delay";
            // 
            // nudDelay
            // 
            this.nudDelay.Location = new System.Drawing.Point(64, 104);
            this.nudDelay.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudDelay.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Size = new System.Drawing.Size(126, 21);
            this.nudDelay.TabIndex = 3;
            this.nudDelay.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudDelay.ValueChanged += new System.EventHandler(this.nudDelay_ValueChanged);
            this.nudDelay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbText_KeyDown);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(25, 156);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "ShowOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnWarning
            // 
            this.btnWarning.Location = new System.Drawing.Point(132, 156);
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Size = new System.Drawing.Size(101, 23);
            this.btnWarning.TabIndex = 6;
            this.btnWarning.Text = "ShowWarning";
            this.btnWarning.UseVisualStyleBackColor = true;
            this.btnWarning.Click += new System.EventHandler(this.btnWarning_Click);
            // 
            // btnError
            // 
            this.btnError.Location = new System.Drawing.Point(239, 156);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(101, 23);
            this.btnError.TabIndex = 7;
            this.btnError.Text = "ShowError";
            this.btnError.UseVisualStyleBackColor = true;
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // btnShow
            // 
            this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShow.Location = new System.Drawing.Point(371, 156);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 8;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "ms";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txbInput,
            this.btnEnter});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(469, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // txbInput
            // 
            this.txbInput.Name = "txbInput";
            this.txbInput.Size = new System.Drawing.Size(150, 25);
            this.txbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbInput_KeyDown);
            // 
            // btnEnter
            // 
            this.btnEnter.Image = global::AhDung.Properties.Resources.PicDemo;
            this.btnEnter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(107, 22);
            this.btnEnter.Text = "ToolStripItem";
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(25, 200);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 135);
            this.panel1.TabIndex = 10;
            // 
            // btnShowInPanel
            // 
            this.btnShowInPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowInPanel.Location = new System.Drawing.Point(346, 200);
            this.btnShowInPanel.Name = "btnShowInPanel";
            this.btnShowInPanel.Size = new System.Drawing.Size(100, 23);
            this.btnShowInPanel.TabIndex = 11;
            this.btnShowInPanel.Text = " ←ShowInPanel";
            this.btnShowInPanel.UseVisualStyleBackColor = true;
            this.btnShowInPanel.Click += new System.EventHandler(this.btnShowInPanel_Click);
            // 
            // FmTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 347);
            this.Controls.Add(this.btnShowInPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.btnError);
            this.Controls.Add(this.btnWarning);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.nudDelay);
            this.Controls.Add(this.ckbFloating);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectIcon);
            this.Controls.Add(this.txbText);
            this.Controls.Add(this.txbIcon);
            this.MinimumSize = new System.Drawing.Size(461, 230);
            this.Name = "FmTester";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FmTester";
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbIcon;
        private System.Windows.Forms.Button btnSelectIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckbFloating;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnWarning;
        private System.Windows.Forms.Button btnError;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnEnter;
        private System.Windows.Forms.ToolStripTextBox txbInput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShowInPanel;
    }
}