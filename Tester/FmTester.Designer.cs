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
            this.txbMultiline = new System.Windows.Forms.TextBox();
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
            this.btnEnter = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShowInPanel = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txbTestCaret = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudFade = new System.Windows.Forms.NumericUpDown();
            this.btnRestore = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFade)).BeginInit();
            this.SuspendLayout();
            // 
            // txbMultiline
            // 
            this.txbMultiline.AcceptsReturn = true;
            this.txbMultiline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMultiline.Location = new System.Drawing.Point(14, 155);
            this.txbMultiline.Multiline = true;
            this.txbMultiline.Name = "txbMultiline";
            this.txbMultiline.Size = new System.Drawing.Size(471, 63);
            this.txbMultiline.TabIndex = 4;
            this.txbMultiline.Text = "消息可以是多行\r\nThe message can be multiline";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Text:";
            // 
            // ckbFloating
            // 
            this.ckbFloating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbFloating.AutoSize = true;
            this.ckbFloating.Checked = true;
            this.ckbFloating.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbFloating.Location = new System.Drawing.Point(413, 16);
            this.ckbFloating.Name = "ckbFloating";
            this.ckbFloating.Size = new System.Drawing.Size(72, 16);
            this.ckbFloating.TabIndex = 2;
            this.ckbFloating.Text = "Floating";
            this.ckbFloating.UseVisualStyleBackColor = true;
            this.ckbFloating.CheckedChanged += new System.EventHandler(this.ckbFloating_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Delay";
            // 
            // nudDelay
            // 
            this.nudDelay.Location = new System.Drawing.Point(53, 15);
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
            this.nudDelay.Size = new System.Drawing.Size(93, 21);
            this.nudDelay.TabIndex = 0;
            this.nudDelay.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudDelay.ValueChanged += new System.EventHandler(this.nudDelay_ValueChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(14, 235);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 36);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "ShowOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnWarning
            // 
            this.btnWarning.Location = new System.Drawing.Point(121, 235);
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Size = new System.Drawing.Size(101, 36);
            this.btnWarning.TabIndex = 6;
            this.btnWarning.Text = "ShowWarning";
            this.btnWarning.UseVisualStyleBackColor = true;
            this.btnWarning.Click += new System.EventHandler(this.btnWarning_Click);
            // 
            // btnError
            // 
            this.btnError.Location = new System.Drawing.Point(228, 235);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(101, 36);
            this.btnError.TabIndex = 7;
            this.btnError.Text = "ShowError";
            this.btnError.UseVisualStyleBackColor = true;
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // btnShow
            // 
            this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShow.Location = new System.Drawing.Point(335, 235);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(150, 36);
            this.btnShow.TabIndex = 8;
            this.btnShow.Text = "Show CustomStyle ->";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "ms";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEnter});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(787, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnEnter
            // 
            this.btnEnter.Image = global::AhDung.Properties.Resources.PicDemo;
            this.btnEnter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(263, 22);
            this.btnEnter.Text = "Show by ToolStripItem with Default Style";
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(14, 289);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 122);
            this.panel1.TabIndex = 10;
            // 
            // btnShowInPanel
            // 
            this.btnShowInPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowInPanel.Location = new System.Drawing.Point(359, 289);
            this.btnShowInPanel.Name = "btnShowInPanel";
            this.btnShowInPanel.Size = new System.Drawing.Size(126, 36);
            this.btnShowInPanel.TabIndex = 9;
            this.btnShowInPanel.Text = " <- Show In Panel";
            this.btnShowInPanel.UseVisualStyleBackColor = true;
            this.btnShowInPanel.Click += new System.EventHandler(this.btnShowInPanel_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 29);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(275, 394);
            this.propertyGrid1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txbTestCaret);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txbMultiline);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.btnShowInPanel);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.ckbFloating);
            this.splitContainer1.Panel1.Controls.Add(this.btnShow);
            this.splitContainer1.Panel1.Controls.Add(this.nudFade);
            this.splitContainer1.Panel1.Controls.Add(this.nudDelay);
            this.splitContainer1.Panel1.Controls.Add(this.btnError);
            this.splitContainer1.Panel1.Controls.Add(this.btnOk);
            this.splitContainer1.Panel1.Controls.Add(this.btnWarning);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Panel2.Controls.Add(this.btnRestore);
            this.splitContainer1.Size = new System.Drawing.Size(787, 427);
            this.splitContainer1.SplitterDistance = 502;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 1;
            // 
            // txbTestCaret
            // 
            this.txbTestCaret.Location = new System.Drawing.Point(14, 69);
            this.txbTestCaret.Multiline = true;
            this.txbTestCaret.Name = "txbTestCaret";
            this.txbTestCaret.Size = new System.Drawing.Size(471, 52);
            this.txbTestCaret.TabIndex = 3;
            this.txbTestCaret.Text = "Click in this textbox and try press Enter key\r\n点进来回车试试\r\n的发萨达佛萨发的萨法阿斯顿发送到发送到发是防守打法" +
    "说";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "for test show by Caret:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(348, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "ms";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(214, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "Fade";
            // 
            // nudFade
            // 
            this.nudFade.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.nudFade.Location = new System.Drawing.Point(249, 15);
            this.nudFade.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudFade.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudFade.Name = "nudFade";
            this.nudFade.Size = new System.Drawing.Size(93, 21);
            this.nudFade.TabIndex = 1;
            this.nudFade.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudFade.ValueChanged += new System.EventHandler(this.nudFade_ValueChanged);
            // 
            // btnRestore
            // 
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRestore.Location = new System.Drawing.Point(0, 0);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(275, 29);
            this.btnRestore.TabIndex = 0;
            this.btnRestore.Text = "Custom TipStyle (Click to Restore)";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // FmTester
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 452);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(461, 230);
            this.Name = "FmTester";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tester";
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudFade)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbMultiline;
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShowInPanel;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudFade;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.TextBox txbTestCaret;
        private System.Windows.Forms.Label label1;
    }
}