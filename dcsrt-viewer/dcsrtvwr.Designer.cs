using System;
using System.Windows.Forms;

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
            this.sourceDocumentTextBox = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.candidateRuleTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadDocumentButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.ruleTtoolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.strategyToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.fuzzyToleranceToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.matchPercentageToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.rootSplitContainer = new System.Windows.Forms.SplitContainer();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootSplitContainer)).BeginInit();
            this.rootSplitContainer.Panel1.SuspendLayout();
            this.rootSplitContainer.Panel2.SuspendLayout();
            this.rootSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceDocumentTextBox
            // 
            this.sourceDocumentTextBox.AllowDrop = true;
            this.sourceDocumentTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.sourceDocumentTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceDocumentTextBox.EnableAutoDragDrop = true;
            this.sourceDocumentTextBox.Location = new System.Drawing.Point(0, 25);
            this.sourceDocumentTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.sourceDocumentTextBox.Name = "sourceDocumentTextBox";
            this.sourceDocumentTextBox.ReadOnly = true;
            this.sourceDocumentTextBox.Size = new System.Drawing.Size(672, 350);
            this.sourceDocumentTextBox.TabIndex = 1;
            this.sourceDocumentTextBox.Text = "";
            this.sourceDocumentTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.sourceDocumentTextBox_DragDrop);
            this.sourceDocumentTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.sourceDocumentTextBox_DragEnter);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "pdf";
            this.openFileDialog1.Filter = "PDF Files|*.pdf";
            // 
            // candidateRuleTextBox
            // 
            this.candidateRuleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.candidateRuleTextBox.Location = new System.Drawing.Point(0, 54);
            this.candidateRuleTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.candidateRuleTextBox.Name = "candidateRuleTextBox";
            this.candidateRuleTextBox.Size = new System.Drawing.Size(688, 247);
            this.candidateRuleTextBox.TabIndex = 3;
            this.candidateRuleTextBox.Text = "{\n      \"RuleId\": \"TestRule\",\n      \"Words\": [ \"foo\", \"bar\" ],\n      \"Destination" +
    "\": \"foo\"\n }";
            this.candidateRuleTextBox.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.candidateRuleTextBox_ContentsResized);
            this.candidateRuleTextBox.TextChanged += new System.EventHandler(this.candidateRuleTextBox_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sourceDocumentTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1364, 375);
            this.splitContainer1.SplitterDistance = 672;
            this.splitContainer1.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDocumentButton,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(672, 25);
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
            this.loadDocumentButton.Click += new System.EventHandler(this.loadDocumentToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.candidateRuleTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.resultTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(688, 375);
            this.splitContainer2.SplitterDistance = 301;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(16, 16);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel4,
            this.ruleTtoolStripComboBox,
            this.toolStripSeparator3,
            this.toolStripLabel2,
            this.strategyToolStripComboBox,
            this.toolStripLabel1,
            this.fuzzyToleranceToolStripComboBox,
            this.toolStripLabel3,
            this.matchPercentageToolStripComboBox,
            this.toolStripSeparator2,
            this.saveToolStripButton,
            this.deleteToolStripButton,
            this.toolStripSeparator4,
            this.runToolStripButton});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(688, 54);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(33, 15);
            this.toolStripLabel4.Text = "Rule:";
            // 
            // ruleTtoolStripComboBox
            // 
            this.ruleTtoolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ruleTtoolStripComboBox.Name = "ruleTtoolStripComboBox";
            this.ruleTtoolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.ruleTtoolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.ruleTtoolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(53, 15);
            this.toolStripLabel2.Text = "Strategy:";
            // 
            // strategyToolStripComboBox
            // 
            this.strategyToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.strategyToolStripComboBox.Items.AddRange(new object[] {
            "StringSearch",
            "Probabilistic"});
            this.strategyToolStripComboBox.Name = "strategyToolStripComboBox";
            this.strategyToolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.strategyToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.strategyToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(92, 15);
            this.toolStripLabel1.Text = "Fuzzy Tolerance:";
            // 
            // fuzzyToleranceToolStripComboBox
            // 
            this.fuzzyToleranceToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuzzyToleranceToolStripComboBox.Items.AddRange(new object[] {
            "85",
            "90",
            "92",
            "95",
            "98"});
            this.fuzzyToleranceToolStripComboBox.Name = "fuzzyToleranceToolStripComboBox";
            this.fuzzyToleranceToolStripComboBox.Size = new System.Drawing.Size(75, 23);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(106, 15);
            this.toolStripLabel3.Text = "Match Percentage:";
            // 
            // matchPercentageToolStripComboBox
            // 
            this.matchPercentageToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.matchPercentageToolStripComboBox.Items.AddRange(new object[] {
            "50",
            "60",
            "70",
            "80",
            "90"});
            this.matchPercentageToolStripComboBox.Name = "matchPercentageToolStripComboBox";
            this.matchPercentageToolStripComboBox.Size = new System.Drawing.Size(75, 23);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // runToolStripButton
            // 
            this.runToolStripButton.Image = global::dcsrt_viewer.Properties.Resources.play;
            this.runToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runToolStripButton.Name = "runToolStripButton";
            this.runToolStripButton.Size = new System.Drawing.Size(56, 28);
            this.runToolStripButton.Text = "Run";
            this.runToolStripButton.Click += new System.EventHandler(this.runToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = global::dcsrt_viewer.Properties.Resources.save;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.saveToolStripButton.Text = "Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // deleteToolStripButton
            // 
            this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteToolStripButton.Image = global::dcsrt_viewer.Properties.Resources.delete;
            this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteToolStripButton.Name = "deleteToolStripButton";
            this.deleteToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.deleteToolStripButton.Text = "Delete";
            this.deleteToolStripButton.Click += new System.EventHandler(this.deleteToolStripButton_Click);
            // 
            // resultTextBox
            // 
            this.resultTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.resultTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultTextBox.Location = new System.Drawing.Point(0, 0);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ReadOnly = true;
            this.resultTextBox.Size = new System.Drawing.Size(688, 71);
            this.resultTextBox.TabIndex = 4;
            this.resultTextBox.Text = "";
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(1364, 146);
            this.logTextBox.TabIndex = 5;
            // 
            // rootSplitContainer
            // 
            this.rootSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.rootSplitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.rootSplitContainer.Name = "rootSplitContainer";
            this.rootSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // rootSplitContainer.Panel1
            // 
            this.rootSplitContainer.Panel1.Controls.Add(this.splitContainer1);
            // 
            // rootSplitContainer.Panel2
            // 
            this.rootSplitContainer.Panel2.Controls.Add(this.logTextBox);
            this.rootSplitContainer.Size = new System.Drawing.Size(1364, 525);
            this.rootSplitContainer.SplitterDistance = 375;
            this.rootSplitContainer.TabIndex = 6;
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // dcsrtvwr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 525);
            this.Controls.Add(this.rootSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "dcsrtvwr";
            this.Text = "dcsrt-viewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.rootSplitContainer.Panel1.ResumeLayout(false);
            this.rootSplitContainer.Panel2.ResumeLayout(false);
            this.rootSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootSplitContainer)).EndInit();
            this.rootSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox sourceDocumentTextBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox candidateRuleTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton loadDocumentButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox ruleTtoolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox fuzzyToleranceToolStripComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox matchPercentageToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton runToolStripButton;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.SplitContainer rootSplitContainer;
        private ToolStripComboBox strategyToolStripComboBox;
        private ToolStripButton saveToolStripButton;
        private ToolStripButton deleteToolStripButton;
        private ToolStripSeparator toolStripSeparator4;
    }
}
