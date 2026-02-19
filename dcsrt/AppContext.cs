namespace dcsrt
{
    internal class AppContext
    {
        private NLog.Logger m_Logger;

        internal NLog.Logger Logger
        {
            get
            {
                if (this.m_Logger == null)
                {
                    this.m_Logger = NLog.LogManager.GetCurrentClassLogger();
                }
                return this.m_Logger;
            }
        }

        internal CodexDictionary Codex
        {
            get
            {
                return CodexManager.Codex;
            }
        }
    }
}
