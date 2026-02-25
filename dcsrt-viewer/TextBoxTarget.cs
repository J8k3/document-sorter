using NLog;
using NLog.Targets;
using System;
using System.Windows.Forms;

namespace dcsrt_viewer
{
    [Target("TextBox")]
    public sealed class TextBoxTarget : TargetWithLayout
    {
        public TextBox TargetTextBox { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            if (this.TargetTextBox != null && !this.TargetTextBox.IsDisposed)
            {
                string logMessage = this.Layout.Render(logEvent);
                if (this.TargetTextBox.InvokeRequired)
                {
                    this.TargetTextBox.Invoke(new Action(() => this.TargetTextBox.AppendText(logMessage + System.Environment.NewLine)));
                }
                else
                {
                    this.TargetTextBox.AppendText(logMessage + System.Environment.NewLine);
                }
            }
        }
    }
}
