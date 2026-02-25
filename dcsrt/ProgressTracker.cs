using System;

namespace dcsrt
{
    public static class ProgressTracker
    {
        private static string currentProgress = string.Empty;
        private static readonly object lockObject = new object();

        public static void Update(int current, int total)
        {
            lock (lockObject)
            {
                currentProgress = $"Processing: {current}/{total}";
                Console.Write($"\r{currentProgress}");
            }
        }

        public static void Clear()
        {
            lock (lockObject)
            {
                if (!string.IsNullOrEmpty(currentProgress))
                {
                    Console.Write($"\r{new string(' ', currentProgress.Length)}\r");
                    currentProgress = string.Empty;
                }
            }
        }

        public static void Complete()
        {
            lock (lockObject)
            {
                if (!string.IsNullOrEmpty(currentProgress))
                {
                    Console.WriteLine();
                    currentProgress = string.Empty;
                }
            }
        }
    }
}
