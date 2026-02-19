using System;
using System.IO;

namespace dcsrt
{
    public static class CodexManager
    {
        private const string DefaultCodexFileName = "Codex.json";

        static CodexManager()
        {
            CodexManager.Reload();
        }

        public static CodexDictionary Codex { get; private set; }

        public static string CodexFilePath { get; private set; }

        public static string GetDefaultCodexPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultCodexFileName);
        }

        public static void Save()
        {
            string path = string.IsNullOrWhiteSpace(CodexManager.CodexFilePath)
                ? CodexManager.GetDefaultCodexPath()
                : CodexManager.CodexFilePath;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(CodexManager.Codex);

            File.WriteAllText(path, json);

            CodexManager.Reload(path);
        }

        public static void Reload(string codexPath = null)
        {
            string path = string.IsNullOrWhiteSpace(codexPath)
                ? CodexManager.GetDefaultCodexPath()
                : codexPath;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Codex file not found at path [{path}].", path);
            }

            string json = File.ReadAllText(path);

            CodexManager.Codex = Newtonsoft.Json.JsonConvert.DeserializeObject<CodexDictionary>(json);
            CodexManager.CodexFilePath = path;
        }
    }
}
