using dcsrt;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace dcsrt_viewer
{
    public partial class dcsrtvwr : Form
    {
        public dcsrtvwr()
        {
            InitializeComponent();
        }

        private void richTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Codex codex = JsonConvert.DeserializeObject<Codex>(this.candidateRuleTextBox.Text);
                bool isMatchedToCodexEntry = AnalysisActivity.MatchDocument(this.sourceDocumentTextBox.Text, codex.Words, MatchingStrategy.StringSearch, (int)this.matchingThreshold.Value);
                if (isMatchedToCodexEntry)
                {
                    this.candidateRuleTextBox.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.candidateRuleTextBox.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch (JsonException)
            {
                this.candidateRuleTextBox.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.sourceDocumentTextBox.Text = string.Empty;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(this.openFileDialog1.FileName);
                if (file.Exists)
                {
                    string extractedText = PdfUtil.ExtractTextFromPDF(file);
                    this.sourceDocumentTextBox.Text = extractedText ?? string.Empty;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Codex codex = JsonConvert.DeserializeObject<Codex>(this.candidateRuleTextBox.Text);
                bool matched = AnalysisActivity.MatchDocument(this.sourceDocumentTextBox.Text, codex.Words, MatchingStrategy.StringSearch, (int)this.matchingThreshold.Value);
                this.resultTextBox.Text = matched.ToString();
            }
            catch (JsonException ex)
            {
                this.resultTextBox.Text = $"Error: {ex.Message}";
            }
        }
    }
}
