using NLog;
using NLog.Targets;

namespace dcsrt
{
    [Target("ProgressAwareConsole")]
    public sealed class ProgressAwareConsoleTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            ProgressTracker.Clear();
            string message = this.Layout.Render(logEvent);
            
            System.ConsoleColor originalColor = System.Console.ForegroundColor;
            
            switch (logEvent.Level.Name)
            {
                case "Trace":
                case "Debug":
                    System.Console.ForegroundColor = System.ConsoleColor.Gray;
                    break;
                case "Info":
                    System.Console.ForegroundColor = System.ConsoleColor.White;
                    break;
                case "Warn":
                    System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                    break;
                case "Error":
                case "Fatal":
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                    break;
            }
            
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = originalColor;
        }
    }
}
