using dcsrt;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dcsrt_viewer
{
    public partial class dcsrtvwr : Form
    {
        private TextBoxTarget logTarget;
        private bool ruleModified = false;
        private string currentRuleJson = string.Empty;

        public dcsrtvwr()
        {
            InitializeComponent();
            this.SetupLogging();

            this.strategyToolStripComboBox.Items.AddRange(Enum.GetNames(typeof(MatchingStrategy)));
            this.strategyToolStripComboBox.SelectedIndex = 0;

            this.FillPercentageSelectionComboBox(this.fuzzyToleranceToolStripComboBox);
            this.FillPercentageSelectionComboBox(this.matchPercentageToolStripComboBox);
            
            this.LoadCodexRules();
        }

        internal static NLog.Logger Logger
        {
            get
            {
                return NLog.LogManager.GetCurrentClassLogger();
            }
        }

        private void FillPercentageSelectionComboBox(ToolStripComboBox box)
        {
            for (int i = 0; i <= 100; i += 5)
            {
                box.Items.Add(i);
            }
            box.SelectedIndex = 20;
        }

        private void SetupLogging()
        {
            this.logTarget = new TextBoxTarget { TargetTextBox = this.logTextBox, Layout = "${longdate} ${level:uppercase=true} ${message}" };
            LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();
            config.AddTarget("textbox", this.logTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, this.logTarget);
            LogManager.Configuration = config;
        }

        private void candidateRuleTextBox_TextChanged(object sender, EventArgs e)
        {
            this.ValidateRule();
            if (this.candidateRuleTextBox.Text != this.currentRuleJson)
            {
                this.ruleModified = true;
            }
        }

        private void candidateRuleTextBox_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            this.ValidateRule();
        }

        private void ValidateRule()
        {
            this.resultTextBox.Text = string.Empty;
            this.candidateRuleTextBox.ForeColor = System.Drawing.Color.Black;
            this.candidateRuleTextBox.Font = new System.Drawing.Font(this.candidateRuleTextBox.Font, System.Drawing.FontStyle.Regular);

            try
            {
                Codex codex = JsonConvert.DeserializeObject<Codex>(this.candidateRuleTextBox.Text);
            }
            catch (JsonException)
            {
                this.candidateRuleTextBox.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void loadDocumentToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ResetInterface();

                this.HandleDocumentLoad(this.openFileDialog1.FileName);
            }
        }

        private void runToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                dcsrtvwr.Logger.Log(LogLevel.Trace, $"Analyzing Match.");

                bool matched = AnalysisActivity.MatchDocument(this.SourceContent, this.Codex.Words, this.SelectedMatchingStrategy, this.FuzzyTolerance, this.MatchingPercentage);
                this.resultTextBox.Text = matched.ToString();
            }
            catch (JsonException ex)
            {
                this.resultTextBox.Text = $"Error: {ex.Message}";
            }
        }

        private void sourceDocumentTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void sourceDocumentTextBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            this.ResetInterface();

            if (files.Length > 0)
            {
                string filePath = files.First();

                this.HandleDocumentLoad(filePath);
            }
        }

        private void strategyToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MatchingStrategy selectedStrategy = this.SelectedMatchingStrategy;

            switch (selectedStrategy)
            {
                case MatchingStrategy.Probabilistic:
                    this.fuzzyToleranceToolStripComboBox.Enabled = true;
                    break;
                case MatchingStrategy.StringSearch:
                    this.fuzzyToleranceToolStripComboBox.Enabled = false;
                    break;
            }
        }

        private string SourceContent
        {
            get
            {
                return this.sourceDocumentTextBox.Text;
            }
            set
            {
                this.sourceDocumentTextBox.Text = value ?? string.Empty;
            }
        }

        private Codex Codex
        {
            get
            {
                Codex codex = null;
                try
                {
                    codex = JsonConvert.DeserializeObject<Codex>(this.candidateRuleTextBox.Text);
                }
                catch (JsonException ex)
                {
                    dcsrtvwr.Logger.Log(LogLevel.Error, ex, "Failed to deserialize codex from candidate rule text box.");
                    throw;
                }
                return codex;
            }
        }

        private MatchingStrategy SelectedMatchingStrategy
        {
            get
            {
                MatchingStrategy selectedStrategy = MatchingStrategy.Probabilistic;
                if (this.strategyToolStripComboBox.SelectedItem != null)
                {
                    selectedStrategy = (MatchingStrategy)Enum.Parse(typeof(MatchingStrategy), this.strategyToolStripComboBox.SelectedItem.ToString());
                }
                return selectedStrategy;
            }
        }

        private int MatchingPercentage
        {
            get
            {
                return (int)this.matchPercentageToolStripComboBox.SelectedItem;
            }
        }

        private int FuzzyTolerance
        {
            get
            {
                return (int)this.fuzzyToleranceToolStripComboBox.SelectedItem;
            }
        }

        private void HandleDocumentLoad(string filePath)
        {
            FileInfo file = new FileInfo(filePath);

            if (file.Exists)
            {
                dcsrtvwr.Logger.Log(LogLevel.Trace, $"Loading document [{file.FullName}].");

                this.SourceContent = PdfUtil.LoadDocument(file.FullName, 1, true);
            }
        }

        private void ResetInterface()
        {
            this.SourceContent = string.Empty;
            this.resultTextBox.Text = string.Empty;
            this.logTextBox.Clear();
        }

        private void LoadCodexRules()
        {
            this.ruleTtoolStripComboBox.Items.Clear();
            foreach (Codex codex in CodexManager.Codex)
            {
                this.ruleTtoolStripComboBox.Items.Add(codex.RuleId);
            }
        }

        private void ruleTtoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ruleModified)
            {
                DialogResult result = MessageBox.Show(
                    "The current rule has been modified. Do you want to save it?",
                    "Save Rule?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.SaveCurrentRule();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            if (this.ruleTtoolStripComboBox.SelectedItem != null)
            {
                string selectedRuleId = this.ruleTtoolStripComboBox.SelectedItem.ToString();
                Codex selectedCodex = CodexManager.Codex.FirstOrDefault(c => c.RuleId == selectedRuleId);
                if (selectedCodex != null)
                {
                    this.currentRuleJson = JsonConvert.SerializeObject(selectedCodex, Formatting.Indented);
                    this.candidateRuleTextBox.Text = this.currentRuleJson;
                    this.ruleModified = false;
                }
            }
        }

        private void SaveCurrentRule()
        {
            try
            {
                Codex updatedCodex = JsonConvert.DeserializeObject<Codex>(this.candidateRuleTextBox.Text);
                if (updatedCodex != null && !string.IsNullOrEmpty(updatedCodex.RuleId))
                {
                    List<Codex> codexList = CodexManager.Codex.Dictionary.ToList();
                    int index = codexList.FindIndex(c => c.RuleId == updatedCodex.RuleId);
                    
                    if (index >= 0)
                    {
                        codexList[index] = updatedCodex;
                    }
                    else
                    {
                        codexList.Add(updatedCodex);
                    }
                    
                    CodexManager.Codex.Dictionary = codexList;
                    CodexManager.Save();
                    
                    this.currentRuleJson = this.candidateRuleTextBox.Text;
                    this.ruleModified = false;
                    this.LoadCodexRules();
                    
                    MessageBox.Show("Rule saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Failed to save rule: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            this.SaveCurrentRule();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.ruleTtoolStripComboBox.SelectedItem == null)
            {
                MessageBox.Show("No rule selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRuleId = this.ruleTtoolStripComboBox.SelectedItem.ToString();
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the rule '{selectedRuleId}'?",
                "Delete Rule?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                List<Codex> codexList = CodexManager.Codex.Dictionary.ToList();
                codexList.RemoveAll(c => c.RuleId == selectedRuleId);
                CodexManager.Codex.Dictionary = codexList;
                CodexManager.Save();

                this.candidateRuleTextBox.Text = "{\n      \"RuleId\": \"TestRule\",\n      \"Words\": [ \"foo\", \"bar\" ],\n      \"Destination\": \"foo\"\n }";
                this.currentRuleJson = string.Empty;
                this.ruleModified = false;
                this.LoadCodexRules();

                MessageBox.Show("Rule deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
