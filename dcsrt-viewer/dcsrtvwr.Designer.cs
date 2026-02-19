namespace dcsrt_viewer
{
    partial class dcsrtvwr
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
            if (disposing && (components != null)) {
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
            this.sourceDocumentTextBox = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.candidateRuleTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer3 = new System.Windows.Forms.ToolStripContainer();
            this.matchingThreshold = new System.Windows.Forms.NumericUpDown();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadDocumentButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripContainer3.ContentPanel.SuspendLayout();
            this.toolStripContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchingThreshold)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceDocumentTextBox
            // 
            this.sourceDocumentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceDocumentTextBox.Location = new System.Drawing.Point(0, 25);
            this.sourceDocumentTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sourceDocumentTextBox.Name = "sourceDocumentTextBox";
            this.sourceDocumentTextBox.ReadOnly = true;
            this.sourceDocumentTextBox.Size = new System.Drawing.Size(366, 474);
            this.sourceDocumentTextBox.TabIndex = 1;
            this.sourceDocumentTextBox.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "pdf";
            this.openFileDialog1.Filter = "PDF Files|*.pdf";
            // 
            // candidateRuleTextBox
            // 
            this.candidateRuleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.candidateRuleTextBox.Location = new System.Drawing.Point(0, 0);
            this.candidateRuleTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.candidateRuleTextBox.Name = "candidateRuleTextBox";
            this.candidateRuleTextBox.Size = new System.Drawing.Size(334, 321);
            this.candidateRuleTextBox.TabIndex = 3;
            this.candidateRuleTextBox.Text = "{\n      \"RuleId\": \"TestRule\",\n      \"Words\": [ \"foo\", \"bar\" ],\n      \"Destination" +
    "\": \"foo\"\n }";
            this.candidateRuleTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBox3_KeyUp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(703, 519);
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 4;
            // 
            // toolStripContainer3
            // 
            // 
            // toolStripContainer3.ContentPanel
            // 
            this.toolStripContainer3.ContentPanel.Controls.Add(this.matchingThreshold);
            this.toolStripContainer3.ContentPanel.Controls.Add(this.sourceDocumentTextBox);
            this.toolStripContainer3.ContentPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer3.ContentPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toolStripContainer3.ContentPanel.Size = new System.Drawing.Size(366, 499);
            this.toolStripContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer3.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toolStripContainer3.Name = "toolStripContainer3";
            this.toolStripContainer3.Size = new System.Drawing.Size(366, 519);
            this.toolStripContainer3.TabIndex = 0;
            this.toolStripContainer3.Text = "toolStripContainer3";
            // 
            // matchingThreshold
            // 
            this.matchingThreshold.Location = new System.Drawing.Point(205, 2);
            this.matchingThreshold.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.matchingThreshold.Name = "matchingThreshold";
            this.matchingThreshold.Size = new System.Drawing.Size(80, 20);
            this.matchingThreshold.TabIndex = 2;
            this.matchingThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.matchingThreshold.Value = new decimal(new int[] {
            92,
            0,
            0,
            0});
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDocumentButton,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(366, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // loadDocumentButton
            // 
            this.loadDocumentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadDocumentButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadDocumentButton.Name = "loadDocumentButton";
            this.loadDocumentButton.Size = new System.Drawing.Size(96, 22);
            this.loadDocumentButton.Text = "Load Document";
            this.loadDocumentButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(116, 22);
            this.toolStripLabel1.Text = "Matching Threshold:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStripContainer2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.resultTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(334, 519);
            this.splitContainer2.SplitterDistance = 346;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.candidateRuleTextBox);
            this.toolStripContainer2.ContentPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(334, 321);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(334, 346);
            this.toolStripContainer2.TabIndex = 0;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2});
            this.toolStrip2.Location = new System.Drawing.Point(4, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(48, 25);
            this.toolStrip2.TabIndex = 0;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(45, 22);
            this.toolStripButton2.Text = "Match";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // resultTextBox
            // 
            this.resultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultTextBox.Location = new System.Drawing.Point(0, 0);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(334, 170);
            this.resultTextBox.TabIndex = 0;
            this.resultTextBox.Text = "";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(703, 499);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(703, 519);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 519);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripContainer1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Document Sorter Testing Viewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripContainer3.ContentPanel.ResumeLayout(false);
            this.toolStripContainer3.ContentPanel.PerformLayout();
            this.toolStripContainer3.ResumeLayout(false);
            this.toolStripContainer3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchingThreshold)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox sourceDocumentTextBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox candidateRuleTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton loadDocumentButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.NumericUpDown matchingThreshold;
    }
}

